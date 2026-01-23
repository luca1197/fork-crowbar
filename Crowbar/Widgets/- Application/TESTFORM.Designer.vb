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
        Me.PanelEx1 = New Crowbar.PanelEx()
        Me.CheckBoxEx4 = New Crowbar.CheckBoxEx()
        Me.CheckBoxEx3 = New Crowbar.CheckBoxEx()
        Me.CheckBoxEx2 = New Crowbar.CheckBoxEx()
        Me.CheckBoxEx1 = New Crowbar.CheckBoxEx()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.TabControlEx1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.PanelEx1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControlEx1
        '
        Me.TabControlEx1.Controls.Add(Me.TabPage1)
        Me.TabControlEx1.Controls.Add(Me.TabPage2)
        Me.TabControlEx1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TabControlEx1.HotTrack = True
        Me.TabControlEx1.Location = New System.Drawing.Point(0, 0)
        Me.TabControlEx1.Name = "TabControlEx1"
        Me.TabControlEx1.SelectedIndex = 0
        Me.TabControlEx1.SelectedTabBackColor = System.Drawing.Color.Red
        Me.TabControlEx1.ShowToolTips = True
        Me.TabControlEx1.Size = New System.Drawing.Size(800, 225)
        Me.TabControlEx1.TabBackColor1 = System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(75, Byte), Integer), CType(CType(75, Byte), Integer))
        Me.TabControlEx1.TabBackColor2 = System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(75, Byte), Integer), CType(CType(75, Byte), Integer))
        Me.TabControlEx1.TabIndex = 0
        Me.TabControlEx1.TabPageBackColor = System.Drawing.Color.FromArgb(CType(CType(45, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(45, Byte), Integer))
        Me.TabControlEx1.TabPageForeColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer))
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.PanelEx1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(792, 199)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "TabPage1"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'PanelEx1
        '
        Me.PanelEx1.AutoScroll = True
        Me.PanelEx1.BackColor = System.Drawing.Color.FromArgb(CType(CType(45, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(45, Byte), Integer))
        Me.PanelEx1.Controls.Add(Me.CheckBoxEx4)
        Me.PanelEx1.Controls.Add(Me.CheckBoxEx3)
        Me.PanelEx1.Controls.Add(Me.CheckBoxEx2)
        Me.PanelEx1.Controls.Add(Me.CheckBoxEx1)
        Me.PanelEx1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelEx1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.PanelEx1.Location = New System.Drawing.Point(0, 0)
        Me.PanelEx1.Name = "PanelEx1"
        Me.PanelEx1.SelectedIndex = -1
        Me.PanelEx1.SelectedValue = Nothing
        Me.PanelEx1.Size = New System.Drawing.Size(792, 199)
        Me.PanelEx1.TabIndex = 0
        '
        'CheckBoxEx4
        '
        Me.CheckBoxEx4.AutoSize = True
        Me.CheckBoxEx4.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.CheckBoxEx4.IsReadOnly = False
        Me.CheckBoxEx4.Location = New System.Drawing.Point(688, 364)
        Me.CheckBoxEx4.Name = "CheckBoxEx4"
        Me.CheckBoxEx4.Size = New System.Drawing.Size(93, 17)
        Me.CheckBoxEx4.TabIndex = 11
        Me.CheckBoxEx4.Text = "CheckBoxEx4"
        Me.CheckBoxEx4.UseVisualStyleBackColor = True
        '
        'CheckBoxEx3
        '
        Me.CheckBoxEx3.AutoSize = True
        Me.CheckBoxEx3.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.CheckBoxEx3.IsReadOnly = False
        Me.CheckBoxEx3.Location = New System.Drawing.Point(27, 364)
        Me.CheckBoxEx3.Name = "CheckBoxEx3"
        Me.CheckBoxEx3.Size = New System.Drawing.Size(93, 17)
        Me.CheckBoxEx3.TabIndex = 10
        Me.CheckBoxEx3.Text = "CheckBoxEx3"
        Me.CheckBoxEx3.UseVisualStyleBackColor = True
        '
        'CheckBoxEx2
        '
        Me.CheckBoxEx2.AutoSize = True
        Me.CheckBoxEx2.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.CheckBoxEx2.IsReadOnly = False
        Me.CheckBoxEx2.Location = New System.Drawing.Point(688, 33)
        Me.CheckBoxEx2.Name = "CheckBoxEx2"
        Me.CheckBoxEx2.Size = New System.Drawing.Size(93, 17)
        Me.CheckBoxEx2.TabIndex = 9
        Me.CheckBoxEx2.Text = "CheckBoxEx2"
        Me.CheckBoxEx2.UseVisualStyleBackColor = True
        '
        'CheckBoxEx1
        '
        Me.CheckBoxEx1.AutoSize = True
        Me.CheckBoxEx1.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.CheckBoxEx1.IsReadOnly = False
        Me.CheckBoxEx1.Location = New System.Drawing.Point(27, 33)
        Me.CheckBoxEx1.Name = "CheckBoxEx1"
        Me.CheckBoxEx1.Size = New System.Drawing.Size(93, 17)
        Me.CheckBoxEx1.TabIndex = 8
        Me.CheckBoxEx1.Text = "CheckBoxEx1"
        Me.CheckBoxEx1.UseVisualStyleBackColor = True
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
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TabControl1.Location = New System.Drawing.Point(0, 246)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(800, 204)
        Me.TabControl1.TabIndex = 1
        '
        'TabPage3
        '
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(792, 178)
        Me.TabPage3.TabIndex = 0
        Me.TabPage3.Text = "TabPage3"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'TabPage4
        '
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(192, 74)
        Me.TabPage4.TabIndex = 1
        Me.TabPage4.Text = "TabPage4"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'TESTFORM
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.TabControlEx1)
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Name = "TESTFORM"
        Me.Text = "TESTFORM"
        Me.TabControlEx1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.PanelEx1.ResumeLayout(False)
        Me.PanelEx1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControlEx1 As TabControlEx
	Friend WithEvents TabPage1 As TabPage
	Friend WithEvents TabPage2 As TabPage
	Friend WithEvents PanelEx1 As PanelEx
	Friend WithEvents CheckBoxEx4 As CheckBoxEx
	Friend WithEvents CheckBoxEx3 As CheckBoxEx
	Friend WithEvents CheckBoxEx2 As CheckBoxEx
	Friend WithEvents CheckBoxEx1 As CheckBoxEx
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents TabPage4 As TabPage
End Class
