<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TESTFORM
	Inherits BaseForm

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()>
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		Try
			If disposing AndAlso components IsNot Nothing Then
				components.Dispose()
			End If
		Finally
			MyBase.Dispose(disposing)
		End Try
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()>
	Private Sub InitializeComponent()
		Me.TabControlEx1 = New Crowbar.TabControlEx()
		Me.TabPage1 = New System.Windows.Forms.TabPage()
		Me.TabPage2 = New System.Windows.Forms.TabPage()
		Me.TextBoxEx1 = New Crowbar.TextBoxEx()
		Me.TabControlEx1.SuspendLayout()
		Me.TabPage1.SuspendLayout()
		Me.SuspendLayout()
		'
		'TabControlEx1
		'
		Me.TabControlEx1.Controls.Add(Me.TabPage1)
		Me.TabControlEx1.Controls.Add(Me.TabPage2)
		Me.TabControlEx1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.TabControlEx1.HotTrack = True
		Me.TabControlEx1.Location = New System.Drawing.Point(0, 0)
		Me.TabControlEx1.Name = "TabControlEx1"
		Me.TabControlEx1.SelectedIndex = 0
		Me.TabControlEx1.SelectedTabBackColor = System.Drawing.Color.Red
		Me.TabControlEx1.ShowToolTips = True
		Me.TabControlEx1.Size = New System.Drawing.Size(800, 450)
		Me.TabControlEx1.TabBackColor1 = System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(75, Byte), Integer), CType(CType(75, Byte), Integer))
		Me.TabControlEx1.TabBackColor2 = System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(75, Byte), Integer), CType(CType(75, Byte), Integer))
		Me.TabControlEx1.TabIndex = 0
		Me.TabControlEx1.TabPageBackColor = System.Drawing.Color.FromArgb(CType(CType(45, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(45, Byte), Integer))
		Me.TabControlEx1.TabPageForeColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer))
		'
		'TabPage1
		'
		Me.TabPage1.Controls.Add(Me.TextBoxEx1)
		Me.TabPage1.Location = New System.Drawing.Point(4, 22)
		Me.TabPage1.Name = "TabPage1"
		Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
		Me.TabPage1.Size = New System.Drawing.Size(792, 424)
		Me.TabPage1.TabIndex = 0
		Me.TabPage1.Text = "TabPage1"
		Me.TabPage1.UseVisualStyleBackColor = True
		'
		'TabPage2
		'
		Me.TabPage2.Location = New System.Drawing.Point(4, 22)
		Me.TabPage2.Name = "TabPage2"
		Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
		Me.TabPage2.Size = New System.Drawing.Size(792, 424)
		Me.TabPage2.TabIndex = 1
		Me.TabPage2.Text = "TabPage2"
		Me.TabPage2.UseVisualStyleBackColor = True
		'
		'TextBoxEx1
		'
		Me.TextBoxEx1.BackColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
		Me.TextBoxEx1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
		Me.TextBoxEx1.CueBannerText = ""
		Me.TextBoxEx1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer))
		Me.TextBoxEx1.Location = New System.Drawing.Point(54, 103)
		Me.TextBoxEx1.Name = "TextBoxEx1"
		Me.TextBoxEx1.Size = New System.Drawing.Size(100, 22)
		Me.TextBoxEx1.TabIndex = 1
		'
		'TESTFORM
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(800, 450)
		Me.Controls.Add(Me.TabControlEx1)
		Me.Location = New System.Drawing.Point(0, 0)
		Me.Name = "TESTFORM"
		Me.Text = "TESTFORM"
		Me.TabControlEx1.ResumeLayout(False)
		Me.TabPage1.ResumeLayout(False)
		Me.TabPage1.PerformLayout()
		Me.ResumeLayout(False)

	End Sub

	Friend WithEvents TabControlEx1 As TabControlEx
	Friend WithEvents TabPage1 As TabPage
	Friend WithEvents TabPage2 As TabPage
	Friend WithEvents TextBoxEx1 As TextBoxEx
End Class
