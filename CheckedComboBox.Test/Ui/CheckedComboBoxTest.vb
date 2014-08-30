﻿Imports NUnit.Framework

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

        Public MustInherit Class BaseTest : Inherits CheckedComboBoxTest

            <Test()> Public Sub ValueSeparatorを変更すれば_区切り文字が変更される(<Values(",", "/")> ByVal separator As String)
                sut.ValueSeparator = separator

                sut.SetItemChecked(0, True)
                sut.SetItemChecked(1, True)

                Assert.That(sut.Text, [Is].EqualTo("Red" & separator & "Green"))
            End Sub

            <Test()> Public Sub GetItemTextでDisplayMember値を取得する(<Values(0, 2, 5, 9)> ByVal index As Integer)
                Assert.That(sut.GetItemText(index), [Is].EqualTo(TESTING_NAMES(index)))
            End Sub

            <Test()> Public Sub GetItemValueでValueMember値を取得する(<Values(0, 3, 4, 6, 7, 10)> ByVal index As Integer)
                Assert.That(sut.GetItemValue(index), [Is].EqualTo(index + 20))
            End Sub

            <Test()> Public Sub GetItemValueでValueMember値を取得するが_ValueMemberが空ならnull()
                sut.ValueMember = ""
                Assert.That(sut.GetItemValue(5), [Is].Null)
            End Sub

            <Test()> Public Sub GetValuesChecked_選択値のValueMemberの値を取得する()
                sut.SetItemChecked(2, True)
                sut.SetItemChecked(5, True)
                sut.SetItemChecked(8, True)

                Assert.That(sut.GetValuesChecked(0), [Is].EqualTo(22))
                Assert.That(sut.GetValuesChecked(1), [Is].EqualTo(25))
                Assert.That(sut.GetValuesChecked(2), [Is].EqualTo(28))
            End Sub

            <Test()> Public Sub GetValuesChecked_未選択なら長さ0のList()
                Assert.That(sut.GetValuesChecked, [Is].Empty)
                Assert.That(sut.GetValuesChecked, [Is].Not.Null)
            End Sub
        End Class

        Public Class Itemsで表示する : Inherits BaseTest

            Public Overrides Sub SetUp()
                MyBase.SetUp()
                For i As Integer = 0 To TESTING_NAMES.Length - 1
                    sut.Items.Add(New TestingItem(TESTING_NAMES(i), i + 20))
                Next
                sut.DisplayMember = "Name"
                sut.ValueMember = "Val"
            End Sub

            <Test()> Public Sub GetItemValue_ItemsでバインドできないValueMemberならNull(<Values("dummy", "Hoge")> ByVal valueMember As String)
                sut.ValueMember = valueMember
                Assert.That(sut.GetItemValue(5), [Is].Null)
            End Sub

        End Class

        Public Class DataSourceで表示する : Inherits BaseTest

            Public Overrides Sub SetUp()
                MyBase.SetUp()
                Dim items As New List(Of TestingItem)
                For i As Integer = 0 To TESTING_NAMES.Length - 1
                    items.Add(New TestingItem(TESTING_NAMES(i), i + 20))
                Next
                sut.DataSource = items
                sut.DisplayMember = "Name"
                sut.ValueMember = "Val"
            End Sub

            <Test()> Public Sub DataSourceでバインドできないValueMemberなら例外(<Values("dummy", "Hoge")> ByVal valueMember As String)
                Try
                    sut.ValueMember = valueMember
                    Assert.Fail()
                Catch ex As ArgumentException
                    Assert.True(True)
                End Try
            End Sub

        End Class

    End Class
End Namespace