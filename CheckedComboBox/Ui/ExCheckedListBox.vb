Imports System.ComponentModel
Imports System.Drawing.Design

Namespace Ui
    <ToolboxBitmap(GetType(CheckedListBox))> _
    Public Class ExCheckedListBox : Inherits CheckedListBox

        Public Sub New()
            Me.CheckOnClick = True
        End Sub

        <DefaultValue(""), _
        TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), _
        Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", GetType(UITypeEditor))> _
        Public Shadows Property DisplayMember() As String
            Get
                Return MyBase.DisplayMember
            End Get
            Set(ByVal value As String)
                MyBase.DisplayMember = value
            End Set
        End Property

        '<DefaultValue(""), _
        'TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), _
        'Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", GetType(UITypeEditor))> _
        'Public Shadows Property ValueMember() As String
        '    Get
        '        Return MyBase.ValueMember
        '    End Get
        '    Set(ByVal value As String)
        '        MyBase.ValueMember = value
        '    End Set
        'End Property

        <DefaultValue(""), _
        AttributeProvider(GetType(IListSource)), _
        RefreshProperties(RefreshProperties.All), _
        Browsable(True)> _
        Public Shadows Property DataSource() As Object
            Get
                Return MyBase.DataSource
            End Get
            Set(ByVal value As Object)
                MyBase.DataSource = value
            End Set
        End Property
    End Class
End Namespace