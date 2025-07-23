Imports System.Drawing.Drawing2D

Public Class RadioButtonEx
	Inherits RadioButton

	Public Sub New()
		MyBase.New()

	End Sub

	Protected Overrides Sub OnHandleCreated(e As EventArgs)
		MyBase.OnHandleCreated(e)

		If Me.theOriginalFont Is Nothing Then
			Me.Font = New Font(SystemFonts.MessageBoxFont.Name, 8.25)
			'NOTE: Font gets changed at some point after changing style, messing up when cue banner is turned off, 
			'      so save the Font before changing style.
			Me.theOriginalFont = New System.Drawing.Font(Me.Font.FontFamily, Me.Font.Size, Me.Font.Style, Me.Font.Unit)

			SetStyle(ControlStyles.AllPaintingInWmPaint, True)
			SetStyle(ControlStyles.DoubleBuffer, True)
			SetStyle(ControlStyles.UserPaint, True)
		End If
	End Sub

	Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
		'MyBase.OnPaint(e)

		Dim g As Graphics = e.Graphics
		Dim rect As Rectangle = Me.ClientRectangle

		Dim checkBoxWidth As Integer = 12

		Dim textColor As Color = WidgetConstants.WidgetTextColor
		If Not Me.Enabled Then
			textColor = WidgetConstants.WidgetDisabledTextColor
		End If

		Dim checkboxBorderColor As Color = WidgetConstants.WidgetDisabledTextColor
		If Not Me.Enabled Then
			checkboxBorderColor = WidgetConstants.WidgetDisabledTextColor
		End If

		'Dim checkboxBackgroundColor As Color = WidgetConstants.WidgetHighBackColor
		Dim fillColor As Color = WidgetConstants.Windows10GlobalAccentColor

		' Draw background of entire checkbox widget.
		Using b As New SolidBrush(Me.Parent.BackColor)
			g.FillRectangle(b, rect)
		End Using

		Dim originalSmoothingMode As SmoothingMode = g.SmoothingMode
		g.SmoothingMode = SmoothingMode.AntiAlias

		Dim rectRadio As New RectangleF(0, CSng((ClientRectangle.Height - 13) / 2 - 1), 13, 13)
		e.Graphics.DrawEllipse(Pens.Black, rectRadio)
		rectRadio.Inflate(New Size(-1, -1))
		If Me.Checked Then
			e.Graphics.FillEllipse(New SolidBrush(fillColor), rectRadio)
		Else
			e.Graphics.FillEllipse(New SolidBrush(Me.Parent.BackColor), rectRadio)
		End If

		g.SmoothingMode = originalSmoothingMode

		Using b As New SolidBrush(textColor)
			Dim modRect As New Rectangle(checkBoxWidth + 4, 0, rect.Width - checkBoxWidth, rect.Height)
			Dim formatFlags As TextFormatFlags = TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter

			TextRenderer.DrawText(g, Me.Text, Me.theOriginalFont, modRect, textColor, WidgetConstants.WidgetBackColor, formatFlags)
		End Using
	End Sub

	Protected theControlIsReadOnly As Boolean
	Private theOriginalFont As Font

End Class
