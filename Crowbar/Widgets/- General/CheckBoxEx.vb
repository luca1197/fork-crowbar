Imports System.Drawing.Drawing2D

Public Class CheckBoxEx
	Inherits CheckBox

#Region "Create and Destroy"

	Public Sub New()
		MyBase.New()

	End Sub

#End Region

#Region "Init and Free"

	'Public Sub Init()
	'End Sub

	'Private Sub Free()
	'End Sub

#End Region

#Region "Properties"

	Public Property IsReadOnly() As Boolean
		Get
			Return Me.theControlIsReadOnly
		End Get
		Set(ByVal value As Boolean)
			If Me.theControlIsReadOnly <> value Then
				Me.theControlIsReadOnly = value

				Dim theme As CheckBoxTheme = Nothing
				' This check prevents problems with viewing and saving Forms in VS Designer.
				If TheApp IsNot Nothing Then
					theme = TheApp.Settings.SelectedAppTheme.CheckBoxTheme
				End If
				If theme IsNot Nothing Then
					If Me.theControlIsReadOnly Then
						Me.ForeColor = theme.DisabledForeColor
						Me.BackColor = theme.DisabledBackColor
					Else
						Me.ForeColor = theme.EnabledForeColor
						Me.BackColor = theme.EnabledBackColor
					End If
				End If
			End If
		End Set
	End Property

#End Region

#Region "Methods"

#End Region

#Region "Events"

#End Region

#Region "Private Methods"

	Protected Overrides Sub OnHandleCreated(e As EventArgs)
		MyBase.OnHandleCreated(e)

		Dim theme As CheckBoxTheme = Nothing
		' This check prevents problems with viewing and saving Forms in VS Designer.
		If TheApp IsNot Nothing Then
			theme = TheApp.Settings.SelectedAppTheme.CheckBoxTheme
		End If
		If theme IsNot Nothing AndAlso Me.theOriginalFont Is Nothing Then
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
		Dim theme As CheckBoxTheme = Nothing
		' This check prevents problems with viewing and saving Forms in VS Designer.
		If TheApp IsNot Nothing Then
			theme = TheApp.Settings.SelectedAppTheme.CheckBoxTheme
		End If
		If theme IsNot Nothing Then
			'IMPORTANT: Only assign ForeColor and BackColor once in OnPaint();
			'           otherwise OnPaint will be called over 100 times
			'           and much of the window will not be painted.
			'If Me.Enabled Then
			'	Me.ForeColor = theme.EnabledForeColor
			'Else
			'	Me.ForeColor = theme.DisabledForeColor
			'End If
			'If Me.theControlIsReadOnly Then
			'	Me.BackColor = theme.DisabledBackColor
			'Else
			'	Me.BackColor = theme.EnabledBackColor
			'End If
			Dim textColor As Color = WidgetConstants.WidgetTextColor
			If Not Me.Enabled Then
				textColor = WidgetConstants.WidgetDisabledTextColor
			End If

			Dim boxBorderColor As Color = WidgetConstants.WidgetDisabledTextColor
			If Not Me.Enabled Then
				boxBorderColor = WidgetConstants.WidgetDisabledTextColor
			End If

			Dim boxBackgroundColor As Color = WidgetConstants.WidgetHighBackColor
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

			Dim g As Graphics = e.Graphics
			'Dim rect As Rectangle = New Rectangle(0, 0, ClientSize.Width, ClientSize.Height)
			Dim rect As Rectangle = Me.ClientRectangle

			Dim boxWidth As Integer = 12

			' Draw background of entire checkbox widget.
			Using b As New SolidBrush(Me.Parent.BackColor)
				g.FillRectangle(b, rect)
			End Using

			Dim boxRect As New Rectangle(0, CInt((rect.Height / 2) - (boxWidth / 2)), boxWidth, boxWidth)

			If Me.Checked Then
				' Draw box background.
				Using b As New SolidBrush(fillColor)
					g.FillRectangle(b, boxRect)
				End Using
				' Draw box border.
				Using p As New Pen(boxBorderColor)
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
				' Draw box background.
				Using b As New SolidBrush(boxBackgroundColor)
					g.FillRectangle(b, boxRect)
				End Using
				' Draw box border.
				Using p As New Pen(boxBorderColor)
					g.DrawRectangle(p, boxRect)
				End Using
			End If

			Using b As New SolidBrush(textColor)
				Dim textRect As New Rectangle(boxWidth + 4, 0, rect.Width - boxWidth, rect.Height)
				Dim formatFlags As TextFormatFlags = TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter

				TextRenderer.DrawText(g, Me.Text, Me.theOriginalFont, textRect, textColor, WidgetConstants.WidgetBackColor, formatFlags)
			End Using
		Else
			MyBase.OnPaint(e)
		End If
	End Sub

#End Region

#Region "Data"

	Protected theControlIsReadOnly As Boolean
	Private theOriginalFont As Font

#End Region

End Class
