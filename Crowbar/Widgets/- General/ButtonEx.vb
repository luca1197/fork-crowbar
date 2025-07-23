Public Class ButtonEx
	Inherits Button

	Public Sub New()
		MyBase.New()

		Me.BackColor = WidgetConstants.WidgetHighBackColor
		Me.theMouseIsOverButton = False
	End Sub

	Public Property SpecialImage() As ButtonEx.SpecialImageType
		Get
			Return Me.theSpecialImage
		End Get
		Set(ByVal value As ButtonEx.SpecialImageType)
			If Me.theSpecialImage <> value Then
				Me.theSpecialImage = value
			End If
		End Set
	End Property

	Public Sub Highlight()
		Me.theButtonShouldBeHighlighted = True
		Me.Invalidate()
	End Sub

	Public Sub Diminish()
		Me.theButtonShouldBeHighlighted = False
		Me.Invalidate()
	End Sub

	Protected Overrides Sub OnMouseEnter(e As EventArgs)
		MyBase.OnMouseEnter(e)
		Me.theMouseIsOverButton = True
	End Sub

	Protected Overrides Sub OnMouseLeave(e As EventArgs)
		MyBase.OnMouseLeave(e)
		Me.theMouseIsOverButton = False
	End Sub

	Protected Overrides Sub OnPaint(e As PaintEventArgs)
		MyBase.OnPaint(e)

		Dim g As Graphics = e.Graphics
		Dim clientRectangle As Rectangle = Me.ClientRectangle

		Dim backColor1 As Color = WidgetHighBackColor
		Dim backColor2 As Color = WidgetHighBackColor
		Dim textColor As Color = WidgetTextColor

		'If Me.Image Is Nothing Then
		If Me.Enabled Then
			If Me.theMouseIsOverButton OrElse Me.theButtonShouldBeHighlighted Then
				backColor1 = Color.Green
				backColor2 = WidgetHighBackColor
				textColor = WidgetTextColor
			Else
				''Using aColorBrush As New Drawing2D.LinearGradientBrush(clientRectangle, WidgetHighBackColor, WidgetHighBackColor, Drawing2D.LinearGradientMode.Vertical)
				'Using aColorBrush As New Drawing2D.LinearGradientBrush(clientRectangle, Color.Green, WidgetHighBackColor, Drawing2D.LinearGradientMode.Vertical)
				'	g.FillRectangle(aColorBrush, clientRectangle)
				'End Using
				'TextRenderer.DrawText(g, Me.Text, Me.Font, clientRectangle, WidgetTextColor, TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or TextFormatFlags.WordBreak)
				'------
				'backColor1 = WidgetHighBackColor
				'backColor2 = WidgetHighBackColor
				backColor1 = Me.BackColor
				backColor2 = Me.BackColor
				textColor = WidgetTextColor
			End If
		Else
			'Using aColorBrush As New Drawing2D.LinearGradientBrush(clientRectangle, WidgetDeepBackColor, WidgetDeepBackColor, Drawing2D.LinearGradientMode.Vertical)
			'	g.FillRectangle(aColorBrush, clientRectangle)
			'End Using
			'TextRenderer.DrawText(g, Me.Text, Me.Font, clientRectangle, WidgetDisabledTextColor)
			'------
			'backColor1 = WidgetDeepBackColor
			'backColor2 = WidgetDeepBackColor
			backColor1 = Me.BackColor
			backColor2 = Me.BackColor
			textColor = WidgetDisabledTextColor
		End If

		' Draw background.
		Using aColorBrush As New Drawing2D.LinearGradientBrush(clientRectangle, backColor1, backColor2, Drawing2D.LinearGradientMode.Vertical)
			g.FillRectangle(aColorBrush, clientRectangle)
		End Using

		If Me.Image Is Nothing Then
			If Me.theSpecialImage = SpecialImageType.None Then
				TextRenderer.DrawText(g, Me.Text, Me.Font, clientRectangle, textColor, TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or TextFormatFlags.WordBreak)
			ElseIf Me.theSpecialImage = SpecialImageType.DownArrow Then
				' Draw drop-down arrow.
				Dim dropDownRect As Rectangle = Me.ClientRectangle
				Dim middle As New Point(CInt((dropDownRect.Left + dropDownRect.Width) * 0.5), CInt((dropDownRect.Top + dropDownRect.Height) * 0.5))
				Dim arrow As Point() = {New Point(middle.X - 3, middle.Y - 2), New Point(middle.X + 4, middle.Y - 2), New Point(middle.X, middle.Y + 2)}
				Using backColorBrush As New SolidBrush(WidgetDisabledTextColor)
					e.Graphics.FillPolygon(backColorBrush, arrow)
				End Using
			ElseIf Me.theSpecialImage = SpecialImageType.RightArrow Then
			End If
		Else
			' Draw image.
			g.DrawImage(Me.Image, New Point(CInt(Me.Width * 0.5 - Me.Image.Width * 0.5), CInt(Me.Height * 0.5 - Me.Image.Height * 0.5)))
		End If
		'End If
	End Sub

	Private theMouseIsOverButton As Boolean
	Private theButtonShouldBeHighlighted As Boolean
	Private theSpecialImage As ButtonEx.SpecialImageType

	Public Enum SpecialImageType
		None
		DownArrow
		RightArrow
	End Enum

End Class
