<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ccb = New CheckedComboBox.Ui.CheckedComboBox
        Me.txtOut = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'ccb
        '
        Me.ccb.CheckOnClick = True
        Me.ccb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.ccb.DropDownHeight = 1
        Me.ccb.FormattingEnabled = True
        Me.ccb.IntegralHeight = False
        Me.ccb.Location = New System.Drawing.Point(7, 25)
        Me.ccb.Name = "ccb"
        Me.ccb.Size = New System.Drawing.Size(276, 20)
        Me.ccb.TabIndex = 0
        Me.ccb.ValueSeparator = ", "
        '
        'txtOut
        '
        Me.txtOut.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOut.Location = New System.Drawing.Point(7, 74)
        Me.txtOut.Multiline = True
        Me.txtOut.Name = "txtOut"
        Me.txtOut.Size = New System.Drawing.Size(275, 182)
        Me.txtOut.TabIndex = 1
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 266)
        Me.Controls.Add(Me.txtOut)
        Me.Controls.Add(Me.ccb)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ccb As CheckedComboBox.Ui.CheckedComboBox
    Friend WithEvents txtOut As System.Windows.Forms.TextBox

End Class
