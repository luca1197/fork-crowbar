Imports System.Drawing.Drawing2D

Public Class CheckBoxEx
	Inherits CheckBox

	Public Sub New()
		MyBase.New()

	End Sub

	Public Property IsReadOnly() As Boolean
		Get
			Return Me.theControlIsReadOnly
		End Get
		Set(ByVal value As Boolean)
			If Me.theControlIsReadOnly <> value Then
				Me.theControlIsReadOnly = value

				If Me.theControlIsReadOnly Then
					Me.ForeColor = WidgetConstants.WidgetTextColor
					Me.BackColor = WidgetConstants.WidgetBackColor
				Else
					Me.ForeColor = WidgetConstants.WidgetTextColor
					Me.BackColor = WidgetConstants.WidgetBackColor
				End If
			End If
		End Set
	End Property

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
		'Dim rect As Rectangle = New Rectangle(0, 0, ClientSize.Width, ClientSize.Height)
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

		Dim checkboxBackgroundColor As Color = WidgetConstants.WidgetHighBackColor
		Dim fillColor As Color = WidgetConstants.Windows10GlobalAccentColor

		'If (Enabled) Then
		'	If (Focused) Then
		'		borderColor = Colors.BlueHighlight
		'		fillColor = Colors.BlueSelection
		'	End If
		'	If (_controlState == DarkControlState.Hover) Then
		'		borderColor = Colors.BlueHighlight
		'		fillColor = Colors.BlueSelection
		'	ElseIf (_controlState == DarkControlState.Pressed) Then
		'		borderColor = Colors.GreyHighlight
		'		fillColor = Colors.GreySelection
		'	End If
		'Else
		'	textColor = Colors.DisabledText
		'	borderColor = Colors.GreyHighlight
		'	fillColor = Colors.GreySelection
		'End If

		' Draw background of entire checkbox widget.
		Using b As New SolidBrush(Me.Parent.BackColor)
			g.FillRectangle(b, rect)
		End Using

		Dim boxRect As New Rectangle(0, CInt((rect.Height / 2) - (checkBoxWidth / 2)), checkBoxWidth, checkBoxWidth)

		If Me.Checked Then
			' Draw checkbox background.
			Using b As New SolidBrush(fillColor)
				g.FillRectangle(b, boxRect)
			End Using
			' Draw checkbox border.
			Using p As New Pen(checkboxBorderColor)
				g.DrawRectangle(p, boxRect)
			End Using

			Dim originalSmoothingMode As SmoothingMode = g.SmoothingMode
			g.SmoothingMode = SmoothingMode.AntiAlias

			' Checkmark is 9 pixels wide, 6 pixels high.
			Dim left As Integer = boxRect.Left
			Dim top As Integer = boxRect.Top

			Using checkmarkPen As New Pen(textColor)
				'checkmarkPen.Width = 2
				Dim pt1 As New Point(left + 2, top + 6)
				Dim pt2 As New Point(left + 4, top + 8)
				g.DrawLine(checkmarkPen, pt1, pt2)
				pt1 = New Point(left + 2, top + 7)
				pt2 = New Point(left + 4, top + 9)
				g.DrawLine(checkmarkPen, pt1, pt2)
				'checkmarkPen.Width = 1
				pt1 = New Point(left + 5, top + 8)
				pt2 = New Point(left + 10, top + 3)
				g.DrawLine(checkmarkPen, pt1, pt2)
				pt1 = New Point(left + 5, top + 9)
				pt2 = New Point(left + 10, top + 4)
				g.DrawLine(checkmarkPen, pt1, pt2)
			End Using

			g.SmoothingMode = originalSmoothingMode
		Else
			' Draw checkbox background.
			Using b As New SolidBrush(checkboxBackgroundColor)
				g.FillRectangle(b, boxRect)
			End Using
			' Draw checkbox border.
			Using p As New Pen(checkboxBorderColor)
				g.DrawRectangle(p, boxRect)
			End Using
		End If

		Using b As New SolidBrush(textColor)
			Dim modRect As New Rectangle(checkBoxWidth + 4, 0, rect.Width - checkBoxWidth, rect.Height)
			Dim formatFlags As TextFormatFlags = TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter

			TextRenderer.DrawText(g, Me.Text, Me.theOriginalFont, modRect, textColor, WidgetConstants.WidgetBackColor, formatFlags)
		End Using
	End Sub

	Protected theControlIsReadOnly As Boolean
	Private theOriginalFont As Font

End Class
