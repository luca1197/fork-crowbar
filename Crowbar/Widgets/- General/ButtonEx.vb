Public Class ButtonEx
	Inherits Button

#Region "Create and Destroy"

	Public Sub New()
		MyBase.New()

		'Me.BackColor = WidgetConstants.WidgetHighBackColor
		Me.theMouseIsOverButton = False
	End Sub

#End Region

#Region "Init and Free"

	'Public Sub Init()
	'End Sub

	'Private Sub Free()
	'End Sub

#End Region

#Region "Properties"

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

#End Region

#Region "Enums"

	Public Enum SpecialImageType
		None
		DownArrow
		RightArrow
	End Enum

#End Region

#Region "Methods"

	Public Sub Highlight()
		Me.theButtonShouldBeHighlighted = True
		Me.Invalidate()
	End Sub

	Public Sub Diminish()
		Me.theButtonShouldBeHighlighted = False
		Me.Invalidate()
	End Sub

#End Region

#Region "Events"

#End Region

#Region "Private Methods"

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

		Dim backColor1 As Color
		Dim backColor2 As Color
		Dim textColor As Color
		Dim textBackColor As Color
		Dim borderColor As Color

		Dim theme As ButtonTheme = Nothing
		' This check prevents problems with viewing and saving Forms in VS Designer.
		If TheApp IsNot Nothing Then
			theme = TheApp.Settings.SelectedAppTheme.ButtonTheme
		End If
		If theme IsNot Nothing Then
			If Me.Enabled Then
				If Me.theMouseIsOverButton OrElse Me.theButtonShouldBeHighlighted Then
					' Focus
					backColor1 = theme.FocusBackColor
					backColor2 = theme.FocusBackColor
					'backColor1 = theme.FocusTopBackColor
					'backColor2 = theme.FocusBottomBackColor
					textColor = theme.FocusForeColor
					textBackColor = Color.Transparent
					borderColor = theme.FocusBorderColor
				Else
					backColor1 = theme.EnabledBackColor
					backColor2 = theme.EnabledBackColor
					textColor = theme.EnabledForeColor
					textBackColor = Color.Transparent
					borderColor = theme.EnabledBorderColor
				End If
			Else
				backColor1 = theme.DisabledBackColor
				backColor2 = theme.DisabledBackColor
				textColor = theme.DisabledForeColor
				textBackColor = Color.Transparent
				borderColor = theme.DisabledBorderColor
			End If
			'Else
			'	If Me.Enabled Then
			'		If Me.theMouseIsOverButton OrElse Me.theButtonShouldBeHighlighted Then
			'			' Focus
			'			'backColor1 = Color.Green
			'			'backColor2 = WidgetHighBackColor
			'			'textColor = WidgetTextColor
			'			backColor1 = Me.BackColor
			'			backColor2 = Me.BackColor
			'			textColor = Me.ForeColor
			'			textBackColor = Me.BackColor
			'			borderColor = Me.BackColor
			'		Else
			'			backColor1 = Me.BackColor
			'			backColor2 = Me.BackColor
			'			textColor = Me.ForeColor
			'			textBackColor = Me.BackColor
			'			borderColor = Me.BackColor
			'		End If
			'	Else
			'		backColor1 = Me.BackColor
			'		backColor2 = Me.BackColor
			'		textColor = Me.ForeColor
			'		textBackColor = Me.BackColor
			'		borderColor = Me.BackColor
			'	End If

			Dim g As Graphics = e.Graphics
			Dim clientRectangle As Rectangle = Me.ClientRectangle

			' Draw background.
			Using aColorBrush As New Drawing2D.LinearGradientBrush(clientRectangle, backColor1, backColor2, Drawing2D.LinearGradientMode.Vertical)
				g.FillRectangle(aColorBrush, clientRectangle)
			End Using
			' Draw border.
			Using borderColorPen As New Pen(borderColor)
				'NOTE: DrawRectangle width and height are interpreted as the right and bottom pixels to draw.
				g.DrawRectangle(borderColorPen, clientRectangle.Left, clientRectangle.Top, clientRectangle.Width - 1, clientRectangle.Height - 1)
			End Using

			' Draw text or image.
			If Me.Image Is Nothing Then
				If Me.theSpecialImage = SpecialImageType.None Then
					TextRenderer.DrawText(g, Me.Text, Me.Font, clientRectangle, textColor, textBackColor, TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or TextFormatFlags.WordBreak)
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
				g.DrawImage(Me.Image, New Point(CInt(Me.Width * 0.5 - Me.Image.Width * 0.5), CInt(Me.Height * 0.5 - Me.Image.Height * 0.5)))
			End If
		End If
	End Sub

#End Region

#Region "Data"

	Private theMouseIsOverButton As Boolean
	Private theButtonShouldBeHighlighted As Boolean
	Private theSpecialImage As ButtonEx.SpecialImageType

#End Region

End Class
