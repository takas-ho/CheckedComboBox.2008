Imports System.Text

Public Class Form1

    Private colorArr As String() = {"Red", "Green", "Black", "White", "Orange", "Yellow", "Blue", "Maroon", "Pink", "Purple", "Brown"}

    Public Sub New()

        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        'AddHandler ccb.ItemCheck, AddressOf ccb_ItemCheck
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        For i As Integer = 0 To colorArr.Length - 1
            ccb.Items.Add(New Item(colorArr(i), i))
        Next
        ccb.MaxDropDownItems = 5
        ccb.DisplayMember = "Name"
        ccb.ValueSeparator = ", "
        ccb.SetItemChecked(0, True)
        ccb.SetItemChecked(1, True)

        For i As Integer = 0 To colorArr.Length - 1
            cb.Items.Add(New Item(colorArr(i), i))
        Next
        cb.MaxDropDownItems = 5
        cb.DisplayMember = "Name"

    End Sub

    Private Sub ccb_DropDownClosed(ByVal sender As Object, ByVal e As EventArgs) Handles ccb.DropDownClosed
        txtOut.AppendText("DropdownClosed" & vbCrLf)
        txtOut.AppendText(String.Format("value changed: {0}", ccb.ValueChanged) & vbCrLf)
        txtOut.AppendText(String.Format("value: {0}", ccb.Text) & vbCrLf)

        Dim sb As New StringBuilder("Item checked: ")
        For Each anItem As Item In ccb.CheckedItems
            sb.Append(anItem.Name).Append(ccb.ValueSeparator)
        Next
        sb.Remove(sb.Length - ccb.ValueSeparator.Length, ccb.ValueSeparator.Length)
        txtOut.AppendText(sb.ToString & vbCrLf)
    End Sub

    Private Sub ccb_ItemCheck(ByVal sender As Object, ByVal e As ItemCheckEventArgs) Handles ccb.ItemCheck
        Dim anItem As Item = DirectCast(ccb.Items(e.Index), Item)
        txtOut.AppendText(String.Format("Item '{0}' is about to be {1}", anItem.Name, e.NewValue.ToString) & vbCrLf)
    End Sub

    Private Class Item
        Private _val As Integer
        Private _name As String

        Public Property Val() As Integer
            Get
                Return _val
            End Get
            Set(ByVal value As Integer)
                _val = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Sub New()
        End Sub
        Public Sub New(ByVal name As String, ByVal val As Integer)
            Me.Name = name
            Me.Val = val
        End Sub
    End Class

    Private Sub ccb_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ccb.SelectedIndexChanged
        txtOut.AppendText(String.Format("SelectedIndexChanged '{0}' ", ccb.SelectedIndex) & vbCrLf)
    End Sub

    Private Sub ccb_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ccb.SelectedValueChanged
        txtOut.AppendText(String.Format("SelectedValueChanged '{0}' ", ccb.SelectedValue) & vbCrLf)
    End Sub

    Private Sub ccb_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ccb.SelectionChangeCommitted
        txtOut.AppendText(String.Format("SelectionChangeCommitted '{0}' ", ccb.SelectedText) & vbCrLf)
    End Sub
End Class
