Imports NUnit.Framework

Namespace Ui
    Public MustInherit Class CheckedComboBoxTest

#Region "Nested classes..."
        Private Class TestingItem
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
#End Region
        Private Shared ReadOnly TESTING_NAMES As String() = {"Red", "Green", "Black", "White", "Orange", "Yellow", "Blue", "Maroon", "Pink", "Purple", "Brown"}

        Private sut As CheckedComboBox

        <SetUp()> Public Overridable Sub SetUp()
            sut = New CheckedComboBox
        End Sub

        Public Class ValueSeparator : Inherits CheckedComboBoxTest

            Public Overrides Sub SetUp()
                MyBase.SetUp()
                For i As Integer = 0 To TESTING_NAMES.Length - 1
                    sut.Items.Add(New TestingItem(TESTING_NAMES(i), i + 10))
                Next
                sut.DisplayMember = "Name"
            End Sub

            <Test()> Public Sub Hoge(<Values(",", "/")> ByVal separator As String)
                sut.ValueSeparator = separator
                sut.SetItemChecked(0, True)
                sut.SetItemChecked(1, True)

                Assert.That(sut.Text, [Is].EqualTo("Red" & separator & "Green"))
            End Sub

        End Class

    End Class
End Namespace