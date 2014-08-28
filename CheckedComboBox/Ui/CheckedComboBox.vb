Imports System.Text
Imports System.ComponentModel
Imports System.Drawing.Design

Namespace Ui
    ''' <summary>
    ''' 複数選択できるコンボボックス
    ''' </summary>
    ''' <remarks>http://www.codeproject.com/Articles/31105/A-ComboBox-with-a-CheckedListBox-as-a-Dropdown</remarks>
    Public Class CheckedComboBox : Inherits ComboBox

#Region "Nested classes..."
        Friend Shadows Class Dropdown : Inherits Form
            Friend Class CustomCheckedListBox : Inherits CheckedListBox
                Private curSelIndex As Integer = -1

                Public Sub New()
                    Me.SelectionMode = Windows.Forms.SelectionMode.One
                    Me.HorizontalScrollbar = True
                End Sub

                Protected Overrides Sub OnKeyDown(ByVal e As KeyEventArgs)
                    If e.KeyCode = Keys.Enter Then
                        DirectCast(Parent, Dropdown).OnDeactivate(New CCBoxEventArgs(Nothing, True))
                        e.Handled = True

                    ElseIf e.KeyCode = Keys.Escape Then
                        DirectCast(Parent, Dropdown).OnDeactivate(New CCBoxEventArgs(Nothing, False))
                        e.Handled = True

                    ElseIf e.KeyCode = Keys.Delete Then
                        For i As Integer = 0 To Items.Count - 1
                            SetItemChecked(i, e.Shift)
                        Next
                        e.Handled = True
                    End If
                    MyBase.OnKeyDown(e)
                End Sub

                Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
                    MyBase.OnMouseMove(e)

                    Dim index As Integer = IndexFromPoint(e.Location)
                    Debug.WriteLine("Mouse over item: " + If(0 <= index, GetItemText(Items(index)), "None"))
                    If 0 <= index AndAlso curSelIndex <> index Then
                        curSelIndex = index
                        SetSelected(index, True)
                    End If
                End Sub

            End Class

            Private ReadOnly ccbParent As CheckedComboBox

            Private oldStrValue As String = ""
            Public ReadOnly Property ValueChanged() As Boolean
                Get
                    Dim newStrValue As String = ccbParent.Text
                    If 0 < oldStrValue.Length AndAlso 0 < newStrValue.Length Then
                        Return oldStrValue.CompareTo(newStrValue) <> 0
                    End If
                    Return oldStrValue.Length <> newStrValue.Length
                End Get
            End Property

            Private checkedStateArr As Boolean()

            Private dropdownClosed As Boolean = True

            Private cclb As CustomCheckedListBox
            Friend Property List() As CustomCheckedListBox
                Get
                    Return cclb
                End Get
                Set(ByVal value As CustomCheckedListBox)
                    cclb = value
                End Set
            End Property

            Public Sub New(ByVal ccbParent As CheckedComboBox)
                Me.ccbParent = ccbParent
                InitializeComponent()
                Me.ShowInTaskbar = False
                AddHandler Me.cclb.ItemCheck, AddressOf Me.cclb_ItemCheck
            End Sub

            Private Sub InitializeComponent()
                Me.cclb = New CustomCheckedListBox
                Me.SuspendLayout()

                With Me.cclb
                    .BorderStyle = BorderStyle.None
                    .Dock = DockStyle.Fill
                    .FormattingEnabled = True
                    .Location = New Point(0, 0)
                    .Name = "cclb"
                    .Size = New Size(47, 15)
                    .TabIndex = 0
                End With

                AutoScaleDimensions = New SizeF(6.0F, 13.0F)
                AutoScaleMode = Windows.Forms.AutoScaleMode.Font
                BackColor = SystemColors.Menu
                ClientSize = New Size(47, 16)
                ControlBox = False
                Controls.Add(Me.cclb)
                ForeColor = SystemColors.ControlText
                FormBorderStyle = Windows.Forms.FormBorderStyle.FixedToolWindow
                MinimizeBox = False
                Name = "ccbParent"
                StartPosition = FormStartPosition.Manual
                ResumeLayout(False)
            End Sub

            Public Function GetCheckedItemsStringValue() As String
                Dim sb As New StringBuilder("")
                For i As Integer = 0 To cclb.CheckedItems.Count - 1
                    sb.Append(cclb.GetItemText(cclb.CheckedItems(i))).Append(ccbParent.ValueSeparator)
                Next
                If 0 < sb.Length Then
                    sb.Remove(sb.Length - ccbParent.ValueSeparator.Length, ccbParent.ValueSeparator.Length)
                End If
                Return sb.ToString
            End Function

            Public Sub CloseDropdown(ByVal enactChanges As Boolean)
                If dropdownClosed Then
                    Return
                End If
                Debug.WriteLine("CloseDropdown")

                If enactChanges Then
                    ccbParent.SelectedIndex = -1
                    ccbParent.Text = GetCheckedItemsStringValue()
                Else
                    For i As Integer = 0 To cclb.Items.Count - 1
                        cclb.SetItemChecked(i, checkedStateArr(i))
                    Next
                End If

                dropdownClosed = True

                ccbParent.Focus()
                Me.Hide()

                ccbParent.OnDropDownClosed(New CCBoxEventArgs(Nothing, False))
            End Sub

            Protected Overrides Sub OnActivated(ByVal e As EventArgs)
                Debug.WriteLine("OnActivated")
                MyBase.OnActivated(e)
                dropdownClosed = False
                oldStrValue = ccbParent.Text
                Dim b As New List(Of Boolean)
                For i As Integer = 0 To cclb.Items.Count - 1
                    b.Add(cclb.GetItemChecked(i))
                Next
                checkedStateArr = b.ToArray
            End Sub

            Protected Overrides Sub OnDeactivate(ByVal e As EventArgs)
                Debug.WriteLine("ObDeactivate")
                MyBase.OnDeactivate(e)
                If TypeOf e Is CCBoxEventArgs Then
                    CloseDropdown(DirectCast(e, CCBoxEventArgs).AssignValues)
                Else
                    CloseDropdown(True)
                End If
            End Sub

            Private Sub cclb_ItemCheck(ByVal sender As Object, ByVal e As ItemCheckEventArgs)
                ccbParent.OnItemCheck(sender, e)
            End Sub
        End Class

        Private Class CCBoxEventArgs : Inherits EventArgs
            Private _assignValues As Boolean
            Private _eventArgs As EventArgs

            Public Property AssignValues() As Boolean
                Get
                    Return _assignValues
                End Get
                Set(ByVal value As Boolean)
                    _assignValues = value
                End Set
            End Property

            Public Property EventArgs() As EventArgs
                Get
                    Return _eventArgs
                End Get
                Set(ByVal value As EventArgs)
                    _eventArgs = value
                End Set
            End Property

            Public Sub New(ByVal e As EventArgs, ByVal assignValues As Boolean)
                Me.EventArgs = e
                Me.AssignValues = assignValues
            End Sub

        End Class

#End Region

        Private components As IContainer
        Private ReadOnly _dropdown As Dropdown

        Private _valueSeparator As String
        Public Property ValueSeparator() As String
            Get
                Return _valueSeparator
            End Get
            Set(ByVal value As String)
                _valueSeparator = value
            End Set
        End Property

        ''' <summary>
        ''' 項目が選択されたときに、チェック ボックスを切り替えるかどうかを示す値を取得または設定します。
        ''' </summary>
        ''' <returns>
        ''' すぐにチェック マークが適用される場合は true。それ以外の場合は false。既定値は false です。
        ''' </returns>
        ''' <filterpriority>1</filterpriority>
        Public Property CheckOnClick() As Boolean
            Get
                Return _dropdown.List.CheckOnClick
            End Get
            Set(ByVal value As Boolean)
                _dropdown.List.CheckOnClick = value
            End Set
        End Property

        <DefaultValue(""), _
        TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), _
        Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", GetType(UITypeEditor))> _
        Public Overloads Property DisplayMember() As String
            Get
                Return _dropdown.List.DisplayMember
            End Get
            Set(ByVal value As String)
                _dropdown.List.DisplayMember = value
            End Set
        End Property

        <DefaultValue(""), _
        TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), _
        Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", GetType(UITypeEditor))> _
        Public Overloads Property ValueMember() As String
            Get
                Return _dropdown.List.ValueMember
            End Get
            Set(ByVal value As String)
                _dropdown.List.ValueMember = value
            End Set
        End Property

        <DefaultValue(""), _
        AttributeProvider(GetType(IListSource)), _
        RefreshProperties(RefreshProperties.All), _
        Browsable(True)> _
        Public Overloads Property DataSource() As Object
            Get
                Return _dropdown.List.DataSource
            End Get
            Set(ByVal value As Object)
                _dropdown.List.DataSource = value
            End Set
        End Property

        ''' <summary>
        ''' この <see cref="T:System.Windows.Forms.CheckedListBox"/> 内の項目のコレクションを取得します。
        ''' </summary>
        ''' <returns>
        ''' <see cref="T:System.Windows.Forms.CheckedListBox"/> 内の項目を表す <see cref="T:System.Windows.Forms.CheckedListBox.ObjectCollection"/> コレクション。
        ''' </returns>
        ''' <filterpriority>1</filterpriority>
        Public Overloads ReadOnly Property Items() As CheckedListBox.ObjectCollection
            Get
                Return _dropdown.List.Items
            End Get
        End Property

        Public ReadOnly Property CheckedItems() As CheckedListBox.CheckedItemCollection
            Get
                Return _dropdown.List.CheckedItems
            End Get
        End Property

        Public ReadOnly Property CheckedIndices() As CheckedListBox.CheckedIndexCollection
            Get
                Return _dropdown.List.CheckedIndices
            End Get
        End Property

        ''' <summary>DropDownClosed時に値が変更されていればtrue</summary>
        Public ReadOnly Property ValueChanged() As Boolean
            Get
                Return _dropdown.ValueChanged
            End Get
        End Property

        Public Event ItemCheck As ItemCheckEventHandler

        Protected Overridable Sub OnItemCheck(ByVal sender As Object, ByVal e As ItemCheckEventArgs)
            RaiseEvent ItemCheck(sender, e)
        End Sub

        Public Sub New()
            Me.DrawMode = Windows.Forms.DrawMode.OwnerDrawVariable
            Me._valueSeparator = ", "
            Me.DropDownHeight = 1
            Me.DropDownStyle = ComboBoxStyle.DropDown
            Me._dropdown = New Dropdown(Me)
            Me.CheckOnClick = True
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Protected Overrides Sub OnFontChanged(ByVal e As EventArgs)
            MyBase.OnFontChanged(e)
            _dropdown.Font = Me.Font
            _dropdown.List.Font = Me.Font
        End Sub

        Protected Overrides Sub OnDropDown(ByVal e As EventArgs)
            MyBase.OnDropDown(e)
            DoDropDown()
        End Sub

        Private Sub DoDropDown()
            If Not _dropdown.Visible Then
                Dim rect As Rectangle = RectangleToScreen(Me.ClientRectangle)
                _dropdown.Location = New Point(rect.X, rect.Y + Me.Size.Height)
                Dim count As Integer = _dropdown.List.Items.Count
                If Me.MaxDropDownItems < count Then
                    count = Me.MaxDropDownItems
                ElseIf count = 0 Then
                    count = 1
                End If
                _dropdown.Size = New Size(Me.Size.Width, _dropdown.List.ItemHeight * count + 2)
                _dropdown.Show()
            End If
        End Sub

        Protected Overrides Sub OnDropDownClosed(ByVal e As EventArgs)
            If TypeOf e Is CCBoxEventArgs Then
                MyBase.OnDropDownClosed(e)
            End If
        End Sub

        Protected Overrides Sub OnKeyDown(ByVal e As KeyEventArgs)
            If e.KeyCode = Keys.Down Then
                OnDropDown(Nothing)
            End If
            e.Handled = Not e.Alt AndAlso e.KeyCode <> Keys.Tab AndAlso Not (e.KeyCode = Keys.Left OrElse e.KeyCode = Keys.Right OrElse e.KeyCode = Keys.Home OrElse e.KeyCode = Keys.End)
            MyBase.OnKeyDown(e)
        End Sub

        Protected Overrides Sub OnKeyPress(ByVal e As KeyPressEventArgs)
            e.Handled = True
            MyBase.OnKeyPress(e)
        End Sub

        Public Function GetItemChecked(ByVal index As Integer) As Boolean
            If index < 0 OrElse Items.Count <= index Then
                Throw New ArgumentOutOfRangeException("index", "value out of range")
            End If
            Return _dropdown.List.GetItemChecked(index)
        End Function

        Public Sub SetItemChecked(ByVal index As Integer, ByVal isChecked As Boolean)
            If index < 0 OrElse Items.Count <= index Then
                Throw New ArgumentOutOfRangeException("index", "value out of range")
            End If
            _dropdown.List.SetItemChecked(index, isChecked)
            Me.Text = _dropdown.GetCheckedItemsStringValue
        End Sub

        Public Function GetItemCheckState(ByVal index As Integer) As CheckState
            If index < 0 OrElse Items.Count <= index Then
                Throw New ArgumentOutOfRangeException("index", "value out of range")
            End If
            Return _dropdown.List.GetItemCheckState(index)
        End Function

        Public Sub SetItemCheckState(ByVal index As Integer, ByVal state As CheckState)
            If index < 0 OrElse Items.Count <= index Then
                Throw New ArgumentOutOfRangeException("index", "value out of range")
            End If
            _dropdown.List.SetItemCheckState(index, state)
            Me.Text = _dropdown.GetCheckedItemsStringValue
        End Sub
    End Class
End Namespace