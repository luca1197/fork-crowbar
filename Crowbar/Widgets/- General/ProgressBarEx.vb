Imports System.Drawing.Drawing2D

Public Class ProgressBarEx
	Inherits ProgressBar

	'      Note that Paint() is not called unless UserPaint style is True.
	'      Also OnPaint() is not called unless Paint() is called, but then bar is not drawn.
	'      Overriding and then calling MyBase.OnPaint() does not draw bar.
	'      Conclusion: Must override OnPaint() to draw text and bar. 

	Public Sub New()
		MyBase.New()

		Me.theText = ""

		Me.SetStyle(ControlStyles.UserPaint, True)
		Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
		Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
	End Sub

	Public Overrides Property Text As String
		Get
			Return Me.theText
		End Get
		Set(value As String)
			If Me.theText <> value Then
				Me.theText = value
			End If
		End Set
	End Property

	Public Overloads Property [Value]() As Integer
		Get
			Return MyBase.Value
		End Get
		Set(ByVal value As Integer)
			If value < MyBase.Minimum Then
				value = MyBase.Minimum
			ElseIf value > MyBase.Maximum Then
				value = MyBase.Maximum
			End If
			MyBase.Value = value
			'NOTE: Do this so bar is re-painted when Value changes.
			Me.Invalidate()
		End Set
	End Property

	Protected Overrides Sub OnPaint(e As PaintEventArgs)
		Dim g As Graphics = e.Graphics
		Dim range As Integer = Maximum - Minimum
		Dim percent As Double = CDbl(Value - Minimum) / CDbl(range)
		Dim rect As Rectangle = Me.ClientRectangle
		Dim barColor As Color
		Dim borderColor As Color

		Dim theme As ProgressBarTheme = Nothing
		' This check prevents problems with viewing and saving Forms in VS Designer.
		If TheApp IsNot Nothing Then
			theme = TheApp.Settings.SelectedAppTheme.ProgressBarTheme
		End If
		If theme IsNot Nothing Then
			'Me.ForeColor = WidgetTextColor
			'Me.BackColor = WidgetHighDisabledBackColor
			'barColor = WidgetDeepBackColor
			'borderColor = WidgetDisabledTextColor
			Me.ForeColor = theme.EnabledForeColor
			Me.BackColor = theme.DisabledBackColor
			barColor = theme.EnabledBackColor
			borderColor = theme.EnabledBorderColor
		ElseIf ProgressBarRenderer.IsSupported Then
			ProgressBarRenderer.DrawHorizontalBar(g, Me.DisplayRectangle)
			If rect.Width > 0 AndAlso percent > 0 Then
				rect.Width = CInt(rect.Width * percent)
				'NOTE: This always draws with Green color; it is *NOT* the Windows Accent Color.
				ProgressBarRenderer.DrawHorizontalChunks(g, rect)
			End If
		Else
			Me.ForeColor = MyBase.DefaultForeColor
			Me.BackColor = MyBase.DefaultBackColor
			barColor = SystemColors.ControlDarkDark
			borderColor = SystemColors.ControlDark
		End If

		' Draw progressbar manually.
		If Not ProgressBarRenderer.IsSupported Then
			' Draw background.
			Using backBrush As New SolidBrush(Me.BackColor)
				g.FillRectangle(backBrush, rect)
			End Using

			' Draw progress bar.
			If rect.Width > 0 AndAlso percent > 0 Then
				Dim barRect As Rectangle = rect
				barRect.Width = CInt(rect.Width * percent)
				Using barBrush As New LinearGradientBrush(barRect, Me.BackColor, barColor, LinearGradientMode.Vertical)
					g.FillRectangle(barBrush, barRect)
				End Using
			End If

			' Draw border.
			ControlPaint.DrawBorder(g, rect, borderColor, ButtonBorderStyle.Solid)
		End If

		' Draw progress text.
		If Me.theText <> "" Then
			Dim x As Double
			Dim y As Double
			Dim textSize As Size = TextRenderer.MeasureText(Me.theText, Me.Font)
			x = Me.Width * 0.5 - (textSize.Width * 0.5)
			y = Me.Height * 0.5 - (textSize.Height * 0.5)
			TextRenderer.DrawText(g, Me.theText, Me.Font, New Point(CInt(x), CInt(y)), Me.ForeColor, Me.BackColor)
		End If
	End Sub

	Private theText As String

End Class
