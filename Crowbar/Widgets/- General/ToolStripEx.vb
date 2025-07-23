Public Class ToolStripEx
	Inherits ToolStrip

	Public Sub New()
		MyBase.New()

		Me.BackColor = WidgetConstants.WidgetBackColor
		Me.Renderer = New ToolStripRendererOverride()
	End Sub

	'Public Overloads Property Renderer() As ToolStripRenderer
	'	Get
	'		Return MyBase.Renderer
	'	End Get
	'	Set
	'		MyBase.Renderer = Value
	'	End Set
	'End Property

	Public Class ToolStripRendererOverride
		Inherits ToolStripSystemRenderer
		'Inherits ToolStripProfessionalRenderer
		'Inherits ToolStripRenderer

		Public Sub New()
			MyBase.New()
		End Sub

		'NOTE: Intentionally do nothing to remove the incomplete border.
		Protected Overrides Sub OnRenderToolStripBorder(ByVal e As ToolStripRenderEventArgs)
			If TypeOf e.ToolStrip IsNot ToolStrip Then
				MyBase.OnRenderToolStripBorder(e)
				'Else
				'	Using backColorPen As New Pen(Color.Red)
				'		Dim aRect As Rectangle = e.AffectedBounds
				'		aRect.X += 1
				'		aRect.Width -= 3
				'		aRect.Height -= 2
				'		e.Graphics.DrawRectangle(backColorPen, aRect)
				'	End Using
			End If
		End Sub

		'Protected Overrides Sub OnRenderLabelBackground(ByVal e As ToolStripItemRenderEventArgs)
		'	Using brush As New SolidBrush(e.Item.BackColor)
		'		e.Graphics.FillRectangle(brush, New Rectangle(Point.Empty, e.Item.Size))
		'	End Using
		'End Sub

	End Class

End Class
