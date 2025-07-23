Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports System.Runtime.InteropServices
Imports System.Windows.Forms.VisualStyles

Public Class TreeViewEx
	Inherits TreeView

#Region "Create and Destroy"

	Public Sub New()
		MyBase.New()

		Me.BorderStyle = BorderStyle.None
		Me.DrawMode = TreeViewDrawMode.OwnerDrawAll
		Me.theTreePlusIcon = New VisualStyleRenderer(VisualStyleElement.TreeView.Glyph.Closed)
		Me.theTreeMinusIcon = New VisualStyleRenderer(VisualStyleElement.TreeView.Glyph.Opened)

		'TODO: Try the following trick to keep the default scrollbars, but clip them.
		'FROM: https://www.vbforums.com/showthread.php?830825-RESOLVED-Scrolling-a-TreeView-without-scrollbars&p=5059789&viewfull=1#post5059789
		'      "increasing the size of my TreeView and clipping its region to exclude the scrollbars will work"
		'MyBase.Scrollable = False
		MyBase.Scrollable = True
		'Me.theAutoScroll = True

		Me.theNonClientPaddingColor = WidgetDeepBackColor
		'TEST:
		'Me.theNonClientPaddingColor = Color.Pink
		Me.theScrollingIsActive = False
		Me.theMouseWheelHasMoved = False

		Me.CustomHorizontalScrollbar = New ScrollBarEx()
		Me.Controls.Add(Me.CustomHorizontalScrollbar)
		Me.CustomHorizontalScrollbar.Name = "CustomHorizontalScrollbar"
		Me.CustomHorizontalScrollbar.ScrollOrientation = ScrollBarEx.DarkScrollOrientation.Horizontal
		Me.CustomHorizontalScrollbar.TabIndex = 7
		Me.CustomHorizontalScrollbar.Visible = False
		'Me.CustomHorizontalScrollbar.Visible = True
		'Me.CustomHorizontalScrollbar.Location = New System.Drawing.Point(0, 0)
		'Me.CustomHorizontalScrollbarPopup = New Popup(Me.CustomHorizontalScrollbar)
		'Me.CustomHorizontalScrollbarPopup.Name = "CustomHorizontalScrollbarPopup"

		Me.CustomVerticalScrollBar = New ScrollBarEx()
		Me.Controls.Add(Me.CustomVerticalScrollBar)
		Me.CustomVerticalScrollBar.Name = "CustomVerticalScrollBar"
		Me.CustomVerticalScrollBar.ScrollOrientation = ScrollBarEx.DarkScrollOrientation.Vertical
		Me.CustomVerticalScrollBar.TabIndex = 7
		Me.CustomVerticalScrollBar.Visible = False

		Me.ScrollbarCornerPanel = New PanelEx()
		Me.Controls.Add(Me.ScrollbarCornerPanel)
		Me.ScrollbarCornerPanel.Name = "ScrollbarCornerPanel"
		Me.ScrollbarCornerPanel.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, ScrollBarEx.Consts.ScrollBarSize)
		Me.ScrollbarCornerPanel.Visible = False

		Me.theControlHasShown = False

		Me.theTextFormatFlags = TextFormatFlags.GlyphOverhangPadding Or TextFormatFlags.PreserveGraphicsTranslateTransform

		Me.ForeColor = WidgetTextColor
		Me.BackColor = WidgetDeepBackColor
	End Sub

#End Region

#Region "Init and Free"

#End Region

#Region "Properties"

	'<Browsable(True)>
	'<Category("Layout")>
	'<Description("Scrollbars appear when needed.")>
	'Public Overloads Property Scrollable As Boolean
	'	Get
	'		Return Me.theAutoScroll
	'	End Get
	'	Set
	'		MyBase.Scrollable = False
	'		Me.theAutoScroll = Value
	'	End Set
	'End Property

#End Region

#Region "Methods"

#End Region

#Region "Events"

#End Region

#Region "Widget Event Handlers"

	Protected Overrides Sub OnAfterSelect(e As TreeViewEventArgs)
		MyBase.OnAfterSelect(e)

		'If Me.theOriginalFont IsNot Nothing Then
		'	' EnsureVisible() does not seem to work without scrollbars.
		'	MyBase.Scrollable = True
		'	e.Node.EnsureVisible()
		'	MyBase.Scrollable = False
		'	Me.UpdateScrollbars()
		'End If
		'------
		e.Node.EnsureVisible()
		Me.UpdateScrollbars()
	End Sub

	Protected Overrides Sub OnAfterExpand(e As TreeViewEventArgs)
		MyBase.OnAfterExpand(e)
		Me.UpdateScrollbars()
	End Sub

	Protected Overrides Sub OnAfterCollapse(e As TreeViewEventArgs)
		MyBase.OnAfterCollapse(e)
		Me.UpdateScrollbars()
	End Sub

	'NOTE: Windows calls the default painting AFTER the call to OnDrawNode, so can not modify the default painting here.
	Protected Overrides Sub OnDrawNode(e As DrawTreeNodeEventArgs)
		If Me.theMouseWheelHasMoved Then
			Me.UpdateScrollbars()
			Me.theMouseWheelHasMoved = False
		End If
		If e.Node.Bounds.IsEmpty Then
			Exit Sub
		End If

		'Dim left As Integer = 0
		'Dim top As Integer = 0
		'If Me.CustomHorizontalScrollbar.Visible Then
		'	left = Me.CustomHorizontalScrollbar.Value
		'End If
		'If Me.CustomVerticalScrollBar.Visible Then
		'	top = Me.CustomVerticalScrollBar.Value
		'End If
		'e.Graphics.TranslateTransform(-left, -top)

		Dim nodeRect As Rectangle = e.Bounds
		Dim expandRect As Rectangle = nodeRect
		expandRect.X += Me.Indent * e.Node.Level + 4
		expandRect.Width = 16
		Dim iconRect As Rectangle = nodeRect
		iconRect.X += Me.Indent * e.Node.Level + 20
		iconRect.Width = 16

		' Draw expansion icon.
		If e.Node.Nodes.Count > 0 Then
			If e.Node.IsExpanded Then
				Me.theTreeMinusIcon.DrawBackground(e.Graphics, expandRect)
			Else
				Me.theTreePlusIcon.DrawBackground(e.Graphics, expandRect)
			End If
		End If

		' Draw node icon.
		'Me.ImageList.Draw(e.Graphics, iconRect.X, iconRect.Y, Me.ImageIndex)
		e.Graphics.DrawImage(Me.ImageList.Images(0), iconRect.X, iconRect.Y)

		' Draw text.
		Dim textRect As Rectangle = e.Node.Bounds
		textRect.Width += 2
		Dim textForeColor As Color = Me.ForeColor
		Dim textBackColor As Color = Me.BackColor
		If (e.State And TreeNodeStates.Selected) > 0 Then
			If (e.State And TreeNodeStates.Focused) > 0 Then
				'Using backColorBrush As New SolidBrush(textBackColor)
				'	e.Graphics.FillRectangle(backColorBrush, textRect)
				'End Using
				'TextRenderer.DrawText(e.Graphics, e.Node.Text, e.Node.NodeFont, textRect, textForeColor, textBackColor, Me.theTextFormatFlags)
				'textForeColor = WidgetTextColor
				textBackColor = WidgetConstants.Windows10GlobalAccentColor
			Else
				'e.Graphics.FillRectangle(SystemBrushes.ControlDark, textRect)
				'TextRenderer.DrawText(e.Graphics, e.Node.Text, e.Node.NodeFont, textRect, WidgetTextColor, WidgetDeepBackColor, Me.theTextFormatFlags)
				'textForeColor = WidgetTextColor
				textBackColor = WidgetHighBackColor
			End If
			'Else
			'	Using backColorBrush As New SolidBrush(WidgetDeepBackColor)
			'		e.Graphics.FillRectangle(backColorBrush, textRect)
			'	End Using
			'	TextRenderer.DrawText(e.Graphics, e.Node.Text, e.Node.NodeFont, textRect, WidgetTextColor, WidgetDeepBackColor, Me.theTextFormatFlags)
		End If
		Using backColorBrush As New SolidBrush(textBackColor)
			e.Graphics.FillRectangle(backColorBrush, textRect)
		End Using
		TextRenderer.DrawText(e.Graphics, e.Node.Text, Me.theOriginalFont, textRect, textForeColor, textBackColor, Me.theTextFormatFlags)
		Dim textSize As Size = TextRenderer.MeasureText(e.Graphics, e.Node.Text, Me.theOriginalFont, textRect.Size, Me.theTextFormatFlags)
		If textSize.Width > e.Node.Bounds.Left + e.Node.Bounds.Width Then
			Dim debug As Integer = 4242
		End If
		'e.Node.Tag = e.Node.Bounds.Left + textSize.Width

		e.DrawDefault = False
		MyBase.OnDrawNode(e)

		'e.Graphics.TranslateTransform(left, top)
	End Sub

	Protected Overrides Sub OnHandleCreated(e As EventArgs)
		MyBase.OnHandleCreated(e)

		If Me.theOriginalFont Is Nothing Then
			Me.Font = New Font(SystemFonts.MessageBoxFont.Name, 8.25)
			'NOTE: Font gets changed at some point after changing style, messing up when cue banner is turned off, 
			'      so save the Font before changing style.
			Me.theOriginalFont = New System.Drawing.Font(Me.Font.FontFamily, Me.Font.Size, Me.Font.Style, Me.Font.Unit)

			''SetStyle(ControlStyles.AllPaintingInWmPaint, True)
			''SetStyle(ControlStyles.DoubleBuffer, True)
			'SetStyle(ControlStyles.UserPaint, True)
		End If
	End Sub

	' Can not use this because it updates the scrollbar on the *next* wheel move.
	'Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)
	'	MyBase.OnMouseWheel(e)
	'	Me.UpdateScrollbars()
	'End Sub

	'Protected Overrides Sub OnPaint(e As PaintEventArgs)
	'	MyBase.OnPaint(e)
	'	Using borderColorPen As New Pen(Color.Green)
	'		Dim aRect As Rectangle = Me.ClientRectangle
	'		'NOTE: DrawRectangle width and height are interpreted as the right and bottom pixels to draw.
	'		aRect.Width -= 1
	'		aRect.Height -= 1
	'		e.Graphics.DrawRectangle(borderColorPen, aRect.Left, aRect.Top, aRect.Width, aRect.Height)
	'	End Using
	'End Sub

	Protected Overrides Sub OnSizeChanged(e As EventArgs)
		'NOTE: Need this "If" to prevent unneeded resizing and painting when scrolling.
		If Not Me.theScrollingIsActive Then
			MyBase.OnSizeChanged(e)

			'TODO: Find better way because the following 3 lines update the interface properly, but UpdateNonClientPadding() is called 2 or 3 times, and UpdateVerticalScrollbar() is called 1 or 2 times.
			'NOTE: Force calling UpdateNonClientPadding() here so that the correct clientHeight is used for scrollbars.
			'NOTE: Raise the OnNonClientCalcSize and OnNonClientPaint "events".
			Win32Api.SetWindowPos(Me.Handle, IntPtr.Zero, 0, 0, 0, 0, Win32Api.SWP.SWP_FRAMECHANGED Or Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE Or Win32Api.SWP.SWP_NOZORDER)
			Me.UpdateScrollbars()
			'Me.Refresh()
		End If
	End Sub

	Protected Overrides Sub OnVisibleChanged(e As EventArgs)
		MyBase.OnVisibleChanged(e)

		If Me.Visible Then
			If Not Me.theControlHasShown Then
				Me.theControlHasShown = True

				'NOTE: Raise the OnNonClientCalcSize and OnNonClientPaint "events".
				Win32Api.SetWindowPos(Me.Handle, IntPtr.Zero, 0, 0, 0, 0, Win32Api.SWP.SWP_FRAMECHANGED Or Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE Or Win32Api.SWP.SWP_NOZORDER)
			End If

			''NOTE: Raise the OnNonClientCalcSize and OnNonClientPaint "events".
			'Win32Api.SetWindowPos(Me.Handle, IntPtr.Zero, 0, 0, 0, 0, Win32Api.SWP.SWP_FRAMECHANGED Or Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE Or Win32Api.SWP.SWP_NOZORDER)
			'Me.Invalidate()
			Me.UpdateScrollbars()
		End If
	End Sub

	Protected Overrides Sub WndProc(ByRef m As Message)
		If Not Me.theScrollingIsActive Then
			Select Case m.Msg
				Case Win32Api.WindowsMessages.WM_NCCALCSIZE
					Me.OnNonClientCalcSize(m)
				Case Win32Api.WindowsMessages.WM_NCPAINT
					Me.OnNonClientPaint(m)
				Case Win32Api.WindowsMessages.WM_MOUSEWHEEL
					Me.theMouseWheelHasMoved = True
			End Select
		End If

		MyBase.WndProc(m)

		'If m.Msg = Win32Api.WindowsMessages.WM_PAINT Then
		'	Me.UpdateScrollbars()
		'End If
		'If m.Msg = Win32Api.WindowsMessages.WM_NCPAINT Then
		'	Me.UpdateScrollbars()
		'End If
	End Sub

	Private Sub OnNonClientCalcSize(ByRef m As Message)
		Me.UpdateNonClientPadding()
		If CInt(m.WParam) = 0 Then
			Dim rect As Win32Api.RECT = CType(Marshal.PtrToStructure(m.LParam, GetType(Win32Api.RECT)), Win32Api.RECT)
			Me.ResizeClientRect(Me.NonClientPadding, rect)
			Marshal.StructureToPtr(rect, m.LParam, False)
			m.Result = IntPtr.Zero
		ElseIf CInt(m.WParam) = 1 Then
			Dim nccsp As Win32Api.NCCALCSIZE_PARAMS = CType(Marshal.PtrToStructure(m.LParam, GetType(Win32Api.NCCALCSIZE_PARAMS)), Win32Api.NCCALCSIZE_PARAMS)
			Me.ResizeClientRect(Me.NonClientPadding, nccsp.rect0)
			Marshal.StructureToPtr(nccsp, m.LParam, False)
			m.Result = IntPtr.Zero
		End If
		'------
		'' Disabling horizontal scrollbar also disables scrolling.
		'Dim style As Integer = Win32Api.GetWindowLong(Me.Handle, Win32Api.GWL_STYLE)
		'If (style And Win32Api.WindowsStyles.WS_hSCROLL) > 0 Then
		'	Win32Api.SetWindowLong(Me.Handle, Win32Api.GWL_STYLE, style And (Not Win32Api.WindowsStyles.WS_hSCROLL))
		'End If
		'If (style And Win32Api.WindowsStyles.WS_VSCROLL) > 0 Then
		'	Win32Api.SetWindowLong(Me.Handle, Win32Api.GWL_STYLE, style And (Not Win32Api.WindowsStyles.WS_VSCROLL))
		'End If
	End Sub

	Private Sub OnNonClientPaint(ByRef m As Message)
		Dim hDC As IntPtr = Win32Api.GetWindowDC(Me.Handle)
		Try
			Using g As Graphics = Graphics.FromHdc(hDC)
				Using backColorBrush As New SolidBrush(Me.theNonClientPaddingColor)
					'Dim rect As Rectangle = Me.ClientRectangle
					'rect.Offset(Me.NonClientPadding.Left, Me.NonClientPadding.Top)
					'g.ExcludeClip(rect)
					Dim aRect As RectangleF = g.VisibleClipBounds
					g.FillRectangle(backColorBrush, aRect)
				End Using
				'Using borderColorPen As New Pen(Color.Green)
				'	Dim aRect As RectangleF = g.VisibleClipBounds
				'	'NOTE: DrawRectangle width and height are interpreted as the right and bottom pixels to draw.
				'	aRect.Width -= 1
				'	aRect.Height -= 1
				'	g.DrawRectangle(borderColorPen, aRect.Left, aRect.Top, aRect.Width, aRect.Height)
				'End Using
			End Using
		Finally
			Win32Api.ReleaseDC(Me.Handle, hDC)
		End Try
		m.Result = IntPtr.Zero
	End Sub

	Private Sub HorizontalScrollbar_ValueChanged(ByVal sender As Object, ByVal e As ScrollValueEventArgs) Handles CustomHorizontalScrollbar.ValueChanged
		'Me.UpdateScrolling(e.Value, 0)
		'------
		'Dim horizontalValue As Integer = Win32Api.GetScrollPos(Me.Handle, Win32Api.ScrollBarType.SB_HORZ)
		'If e.Value < horizontalValue Then
		'	If horizontalValue - e.Value <= 5 Then
		'		Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_HSCROLL, Win32Api.SB.SB_LINELEFT, IntPtr.Zero)
		'	Else
		'		Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_HSCROLL, Win32Api.SB.SB_PAGELEFT, IntPtr.Zero)
		'	End If
		'ElseIf e.Value > horizontalValue Then
		'	If e.Value - horizontalValue <= 5 Then
		'		Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_HSCROLL, Win32Api.SB.SB_LINERIGHT, IntPtr.Zero)
		'	Else
		'		Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_HSCROLL, Win32Api.SB.SB_PAGERIGHT, IntPtr.Zero)
		'	End If
		'End If
		'------
		Dim thumbValue As UInt32 = CUInt(e.Value * &H10000 + Win32Api.SB.SB_THUMBPOSITION)
		Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_HSCROLL, thumbValue, IntPtr.Zero)
	End Sub

	Private Sub VerticalScrollBar_ValueChanged(ByVal sender As Object, ByVal e As ScrollValueEventArgs) Handles CustomVerticalScrollBar.ValueChanged
		'Me.UpdateScrolling(0, e.Value)
		'------
		'Dim aNode As TreeNode = Me.TopNode
		'Dim visibleIndex As Integer = Win32Api.GetScrollPos(Me.Handle, Win32Api.ScrollBarType.SB_VERT)
		'While aNode IsNot Nothing AndAlso visibleIndex < e.Value
		'	aNode = aNode.NextVisibleNode
		'	visibleIndex += 1
		'End While
		'While aNode IsNot Nothing AndAlso visibleIndex > e.Value
		'	aNode = aNode.PrevVisibleNode
		'	visibleIndex -= 1
		'End While
		'Me.TopNode = aNode
		'------
		'' Does not move internal scrollbar or contents.
		'Dim thumbValue As UInt32 = CUInt(e.Value * &H10000 + Win32Api.SB.SB_THUMBPOSITION)
		'Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_VSCROLL, thumbValue, IntPtr.Zero)
		'------
		'' Does not move internal scrollbar or contents.
		'Dim thumbValue As UInt32 = CUInt(e.Value * &H10000 + Win32Api.SB.SB_THUMBTRACK)
		'Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_VSCROLL, thumbValue, IntPtr.Zero)
		'------
		'' Does not move contents.
		'Dim thumbValue As Integer = Win32Api.GetScrollPos(Me.Handle, Win32Api.ScrollBarType.SB_VERT)
		'Win32Api.SetScrollPos(Me.Handle, Win32Api.ScrollBarType.SB_VERT, e.Value, True)
		'------
		' Works -- scrolls internal scrollbar and contents.
		Dim scrollInfo As Win32Api.SCROLLINFO
		Dim lRet As Integer
		scrollInfo.cbSize = Marshal.SizeOf(scrollInfo)
		scrollInfo.fMask = Win32Api.SIF_ALL
		lRet = Win32Api.GetScrollInfo(Me.Handle, Win32Api.ScrollBarType.SB_VERT, scrollInfo)
		Dim pageChange As Integer = 0
		If lRet > 0 Then
			pageChange = scrollInfo.nPage
		End If
		Dim thumbValue As Integer = Win32Api.GetScrollPos(Me.Handle, Win32Api.ScrollBarType.SB_VERT)
		If e.Value < thumbValue Then
			If thumbValue - e.Value <= pageChange Then
				For i As Integer = thumbValue To e.Value + 1 Step -1
					Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_VSCROLL, Win32Api.SB.SB_LINEUP, IntPtr.Zero)
				Next
			Else
				Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_VSCROLL, Win32Api.SB.SB_PAGEUP, IntPtr.Zero)
			End If
		ElseIf e.Value > thumbValue Then
			If e.Value - thumbValue <= pageChange Then
				For i As Integer = thumbValue To e.Value - 1
					Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_VSCROLL, Win32Api.SB.SB_LINEDOWN, IntPtr.Zero)
				Next
			Else
				Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_VSCROLL, Win32Api.SB.SB_PAGEDOWN, IntPtr.Zero)
			End If
		End If
	End Sub

#End Region

#Region "Child Widget Event Handlers"

#End Region

#Region "Private Methods"

	'Private Function GetContentSize() As Size
	'	Dim contentSize As New Size()

	'	'Dim contentWidth As Integer = Me.PreferredSize.Width - ScrollBarEx.Consts.ScrollBarSize - Me.Margin.Left
	'	'Dim contentWidth As Integer = Me.PreferredSize.Width

	'	'Dim contentHeight As Integer = Me.PreferredSize.Height - ScrollBarEx.Consts.ScrollBarSize - Me.Margin.Top
	'	'Dim contentHeight As Integer = Me.PreferredSize.Height
	'	'Dim contentHeight As Integer = Me.theContentRectangle.Height

	'	If Me.Nodes.Count > 0 Then
	'		Dim node As TreeNode = Me.Nodes(0)
	'		While node IsNot Nothing
	'			''Dim nodeWidth As Integer = Me.Indent * node.Level + node.Bounds.Width
	'			Dim nodeWidth As Integer = node.Bounds.Left + node.Bounds.Width
	'			'If node.Bounds.Left < 0 Then

	'			'End If
	'			'Dim nodeWidth As Integer = CType(node.Tag, Integer)
	'			If contentSize.Width < nodeWidth Then
	'				contentSize.Width = nodeWidth
	'			End If

	'			contentSize.Height += Me.ItemHeight

	'			If Not node.IsVisible Then
	'				Dim debug As Integer = 4242
	'			End If

	'			node = node.NextVisibleNode
	'		End While
	'	End If

	'	Return contentSize
	'End Function

	'Private Sub DrawDebugRectangle(ByVal rect As RectangleF, ByVal color As Color)
	'	Dim hDC As IntPtr = Win32Api.GetWindowDC(Me.Handle)
	'	Try
	'		Using g As Graphics = Graphics.FromHdc(hDC)
	'			Using backColorBrush As New SolidBrush(color)
	'				g.FillRectangle(backColorBrush, rect)
	'			End Using
	'		End Using
	'	Finally
	'		Win32Api.ReleaseDC(Me.Handle, hDC)
	'	End Try
	'End Sub

	Private Sub UpdateNonClientPadding()
		If Me.DesignMode Then
			Exit Sub
		End If

		Dim left As Integer = 0
		Dim top As Integer = 0
		Dim right As Integer = 0
		Dim bottom As Integer = 0
		'TEST: Use 2 for testing. Use 0 for final.
		'Dim left As Integer = 2
		'Dim top As Integer = 2
		'Dim right As Integer = 2
		'Dim bottom As Integer = 2

		'Dim contentSize As Size = Me.GetContentSize()
		'Dim contentWidth As Integer = contentSize.Width
		'Dim clientWidth As Integer = Me.ClientRectangle.Width
		'Dim contentHeight As Integer = contentSize.Height
		'Dim clientHeight As Integer = Me.ClientRectangle.Height

		'If contentHeight > clientHeight AndAlso Me.theAutoScroll Then
		'	right += ScrollBarEx.Consts.ScrollBarSize
		'	clientWidth -= ScrollBarEx.Consts.ScrollBarSize
		'End If
		'If contentWidth > clientWidth AndAlso Me.theAutoScroll Then
		'	bottom += ScrollBarEx.Consts.ScrollBarSize
		'End If
		'------
		'If contentHeight > clientHeight AndAlso Me.Scrollable Then
		'	right += ScrollBarEx.Consts.ScrollBarSize + 4
		'End If
		'If contentWidth > clientWidth AndAlso Me.Scrollable Then
		'	bottom += ScrollBarEx.Consts.ScrollBarSize + 4
		'End If
		'------
		If Me.Scrollable Then
			Dim scrollBarInfo As New Win32Api.SCROLLBARINFO()
			scrollBarInfo.cbSize = Marshal.SizeOf(scrollBarInfo.[GetType]())
			Dim resultIsSuccess As Boolean = Win32Api.GetScrollBarInfo(Me.Handle, Win32Api.OBJID_VSCROLL, scrollBarInfo)
			If (scrollBarInfo.scrollbar And Win32Api.STATE_SYSTEM_INVISIBLE) = 0 Then
				'right += scrollBarInfo.dxyLineButton
				right += scrollBarInfo.dxyLineButton - ScrollBarEx.Consts.ScrollBarSize
			End If
			resultIsSuccess = Win32Api.GetScrollBarInfo(Me.Handle, Win32Api.OBJID_HSCROLL, scrollBarInfo)
			If (scrollBarInfo.scrollbar And Win32Api.STATE_SYSTEM_INVISIBLE) = 0 Then
				'bottom += scrollBarInfo.dxyLineButton
				bottom += scrollBarInfo.dxyLineButton - ScrollBarEx.Consts.ScrollBarSize
			End If
		End If

		Me.NonClientPadding = New Padding(left, top, right, bottom)
	End Sub

	Private Sub ResizeClientRect(ByVal padding As Padding, ByRef rect As Win32Api.RECT)
		rect.Left += padding.Left
		rect.Top += padding.Top

		'rect.Right -= padding.Right
		'rect.Bottom -= padding.Bottom
		'------
		rect.Right += padding.Right
		rect.Bottom += padding.Bottom
	End Sub

	'Private Sub UpdateScrolling(ByVal leftOrRightValue As Integer, ByVal upOrDownValue As Integer)
	'	If Not Me.theScrollingIsActive Then
	'		Me.theScrollingIsActive = True

	'		''Me.CustomHorizontalScrollbar.Value += leftOrRightValue
	'		''Me.CustomVerticalScrollBar.Value += upOrDownValue
	'		''Me.HorizontalScroll.Value = leftOrRightValue
	'		'If upOrDownValue <= Me.CustomVerticalScrollBar.Minimum OrElse upOrDownValue > Me.CustomVerticalScrollBar.Maximum Then
	'		'	Me.AutoScrollPosition = New Point(leftOrRightValue, upOrDownValue)
	'		'Else
	'		'	Me.VerticalScroll.Value = upOrDownValue
	'		'End If
	'		''Me.Invalidate()
	'		''Me.Invalidate(True)
	'		'Me.Refresh()
	'		'======
	'		'Win32Api.SetScrollPos(Me.Handle, Win32Api.ScrollBarType.SB_HORZ, leftOrRightValue, True)
	'		'------
	'		Dim horizontalValue As Integer = Win32Api.GetScrollPos(Me.Handle, Win32Api.ScrollBarType.SB_HORZ)
	'		If leftOrRightValue < horizontalValue Then
	'			If horizontalValue - leftOrRightValue <= 5 Then
	'				Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_HSCROLL, Win32Api.SB.SB_LINELEFT, IntPtr.Zero)
	'			Else
	'				Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_HSCROLL, Win32Api.SB.SB_PAGELEFT, IntPtr.Zero)
	'			End If
	'		Else
	'			If leftOrRightValue - horizontalValue <= 5 Then
	'				Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_HSCROLL, Win32Api.SB.SB_LINERIGHT, IntPtr.Zero)
	'			Else
	'				Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_HSCROLL, Win32Api.SB.SB_PAGERIGHT, IntPtr.Zero)
	'			End If
	'		End If

	'		''DEBUG: 
	'		'Dim value As Integer = Win32Api.GetScrollPos(Me.Handle, Win32Api.ScrollBarType.SB_VERT)
	'		Dim aNode As TreeNode = Me.TopNode
	'		Dim visibleIndex As Integer = Win32Api.GetScrollPos(Me.Handle, Win32Api.ScrollBarType.SB_VERT)
	'		While aNode IsNot Nothing AndAlso visibleIndex < upOrDownValue
	'			aNode = aNode.NextVisibleNode
	'			visibleIndex += 1
	'		End While
	'		While aNode IsNot Nothing AndAlso visibleIndex > upOrDownValue
	'			aNode = aNode.PrevVisibleNode
	'			visibleIndex -= 1
	'		End While
	'		Me.TopNode = aNode
	'		'======
	'		'Dim hDC As IntPtr = Win32Api.GetWindowDC(Me.Handle)
	'		'Try
	'		'	Using g As Graphics = Graphics.FromHdc(hDC)
	'		'		Dim left As Integer = 0
	'		'		Dim top As Integer = 0
	'		'		If Me.CustomHorizontalScrollbar.Visible Then
	'		'			left = Me.CustomHorizontalScrollbar.Value
	'		'		End If
	'		'		If Me.CustomVerticalScrollBar.Visible Then
	'		'			top = Me.CustomVerticalScrollBar.Value
	'		'		End If
	'		'		g.TranslateTransform(left, top)
	'		'	End Using
	'		'Finally
	'		'	Win32Api.ReleaseDC(Me.Handle, hDC)
	'		'End Try
	'		'======
	'		'Me.Refresh()
	'		'Me.Invalidate()

	'		Me.theScrollingIsActive = False
	'	End If
	'End Sub

	Private Sub UpdateScrollbars()
		If Me.DesignMode Then
			Exit Sub
		End If

		Me.UpdateHorizontalScrollbar()
		Me.UpdateVerticalScrollbar()

		If Me.CustomHorizontalScrollbar.Visible AndAlso Me.CustomVerticalScrollBar.Visible Then
			'Me.CustomHorizontalScrollbar.Size = New System.Drawing.Size(Me.ClientRectangle.Width - ScrollBarEx.Consts.ScrollBarSize, ScrollBarEx.Consts.ScrollBarSize)
			'Me.CustomVerticalScrollBar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.ClientRectangle.Height - ScrollBarEx.Consts.ScrollBarSize)
			'------
			'Me.CustomHorizontalScrollbar.Size = New System.Drawing.Size(Me.Width - 1 - ScrollBarEx.Consts.ScrollBarSize, ScrollBarEx.Consts.ScrollBarSize)
			'Me.CustomVerticalScrollBar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.Height - 1 - ScrollBarEx.Consts.ScrollBarSize)
			'------
			Me.CustomHorizontalScrollbar.Size = New System.Drawing.Size(Me.Width - ScrollBarEx.Consts.ScrollBarSize, ScrollBarEx.Consts.ScrollBarSize)
			Me.CustomVerticalScrollBar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.Height - ScrollBarEx.Consts.ScrollBarSize)

			'NOTE: Assign to Parent so it can draw over non-client area.
			Me.ScrollbarCornerPanel.Parent = Me.Parent
			Me.ScrollbarCornerPanel.BringToFront()
			Dim aPoint As New Point(Me.Width - ScrollBarEx.Consts.ScrollBarSize, Me.Height - ScrollBarEx.Consts.ScrollBarSize)
			'NOTE: Location must be relative to Parent.
			aPoint = Me.PointToScreen(aPoint)
			aPoint = Me.ScrollbarCornerPanel.Parent.PointToClient(aPoint)
			Me.ScrollbarCornerPanel.Location = aPoint
			Me.ScrollbarCornerPanel.Visible = True
		Else
			Me.ScrollbarCornerPanel.Visible = False
		End If
	End Sub

	'Private Sub UpdateHorizontalScrollbar()
	'	'NOTE: Parent can be Nothing on exiting. Prevent the exception with this check.
	'	'If Not Me.theScrollingIsActive AndAlso Me.Parent IsNot Nothing AndAlso Me.theAutoScroll Then
	'	If Not Me.theScrollingIsActive AndAlso Me.Parent IsNot Nothing AndAlso Me.Scrollable Then
	'		Dim contentSize As Size = Me.GetContentSize()
	'		Dim contentWidth As Integer = contentSize.Width
	'		Dim clientWidth As Integer = Me.ClientRectangle.Width
	'		Dim contentHeight As Integer = contentSize.Height
	'		Dim clientHeight As Integer = Me.ClientRectangle.Height
	'		'If contentHeight > clientHeight AndAlso Me.theAutoScroll Then
	'		'	'clientWidth -= ScrollBarEx.Consts.ScrollBarSize
	'		'End If
	'		If contentWidth > clientWidth Then
	'			Me.theScrollingIsActive = True

	'			Me.CustomHorizontalScrollbar.Minimum = 0
	'			Me.CustomHorizontalScrollbar.Maximum = contentWidth
	'			Me.CustomHorizontalScrollbar.Value = Win32Api.GetScrollPos(Me.Handle, Win32Api.ScrollBarType.SB_HORZ)
	'			Me.CustomHorizontalScrollbar.ViewSize = clientWidth
	'			Me.CustomHorizontalScrollbar.SmallChange = 5
	'			Me.CustomHorizontalScrollbar.LargeChange = clientWidth - 5 * 2

	'			'NOTE: Assign to Parent so it can draw over non-client area of RichTextBoxEx.
	'			Me.CustomHorizontalScrollbar.Parent = Me.Parent
	'			Me.CustomHorizontalScrollbar.BringToFront()
	'			'NOTE: Point is relative to Me.ClientRectangle.
	'			'Dim aPoint As New Point(Me.ClientRectangle.Left - Me.NonClientPadding.Left, Me.ClientRectangle.Height + Me.NonClientPadding.Bottom - ScrollBarEx.Consts.ScrollBarSize)
	'			Dim aPoint As New Point(Me.ClientRectangle.Left - Me.NonClientPadding.Left, Me.ClientRectangle.Height + Me.NonClientPadding.Bottom - ScrollBarEx.Consts.ScrollBarSize * 2 - 5)
	'			'NOTE: Location must be relative to Parent.
	'			aPoint = Me.PointToScreen(aPoint)
	'			aPoint = Me.Parent.PointToClient(aPoint)
	'			Me.CustomHorizontalScrollbar.Location = aPoint
	'			'Me.CustomHorizontalScrollbar.Size = New System.Drawing.Size(Me.Width, ScrollBarEx.Consts.ScrollBarSize)
	'			Me.CustomHorizontalScrollbar.Size = New System.Drawing.Size(Me.NonClientPadding.Left + Me.ClientRectangle.Left + Me.ClientRectangle.Width + Me.NonClientPadding.Right, ScrollBarEx.Consts.ScrollBarSize)

	'			Me.CustomHorizontalScrollbar.Show()

	'			Me.theScrollingIsActive = False
	'		Else
	'			Me.theScrollingIsActive = True
	'			Me.CustomHorizontalScrollbar.Hide()
	'			Me.theScrollingIsActive = False
	'		End If
	'	End If
	'End Sub

	Private Sub UpdateHorizontalScrollbar()
		If Not Me.theScrollingIsActive AndAlso Me.Scrollable Then
			'If Not Me.theScrollingIsActive Then
			Dim scrollBarInfo As New Win32Api.SCROLLBARINFO()
			scrollBarInfo.cbSize = Marshal.SizeOf(scrollBarInfo.[GetType]())
			Dim resultIsSuccess As Boolean = Win32Api.GetScrollBarInfo(Me.Handle, Win32Api.OBJID_HSCROLL, scrollBarInfo)

			If (scrollBarInfo.scrollbar And Win32Api.STATE_SYSTEM_INVISIBLE) = 0 Then
				Me.theScrollingIsActive = True

				Dim scrollInfo As Win32Api.SCROLLINFO
				Dim lRet As Integer
				scrollInfo.cbSize = Marshal.SizeOf(scrollInfo)
				scrollInfo.fMask = Win32Api.SIF_ALL
				lRet = Win32Api.GetScrollInfo(Me.Handle, Win32Api.ScrollBarType.SB_HORZ, scrollInfo)
				If lRet > 0 Then
					Me.CustomHorizontalScrollbar.Minimum = scrollInfo.nMin
					Me.CustomHorizontalScrollbar.Maximum = scrollInfo.nMax
					Me.CustomHorizontalScrollbar.Value = scrollInfo.nTrackPos
					Me.CustomHorizontalScrollbar.ViewSize = Me.ClientRectangle.Width
					Me.CustomHorizontalScrollbar.SmallChange = 5
					Me.CustomHorizontalScrollbar.LargeChange = scrollInfo.nPage
				End If

				'NOTE: Assign to Parent so it can draw over non-client area.
				Me.CustomHorizontalScrollbar.Parent = Me.Parent
				Me.CustomHorizontalScrollbar.BringToFront()
				Dim aPoint As New Point(Me.ClientRectangle.Left, Me.ClientRectangle.Height)
				'Dim aPoint As New Point(Me.ClientRectangle.Left, CInt(Me.ClientRectangle.Height - ScrollBarEx.Consts.ScrollBarSize))
				'Dim aPoint As New Point(Me.ClientRectangle.Left, CInt(Me.ClientRectangle.Height - ScrollBarEx.Consts.ScrollBarSize * 0.5))
				'NOTE: Location must be relative to Parent.
				aPoint = Me.PointToScreen(aPoint)
				aPoint = Me.PointToClient(aPoint)
				Me.CustomHorizontalScrollbar.Location = aPoint
				Me.CustomHorizontalScrollbar.Size = New System.Drawing.Size(Me.ClientRectangle.Width, ScrollBarEx.Consts.ScrollBarSize)
				Me.CustomHorizontalScrollbar.Show()

				Me.theScrollingIsActive = False
			Else
				Me.theScrollingIsActive = True
				Me.CustomHorizontalScrollbar.Hide()
				Me.theScrollingIsActive = False
			End If
		End If
	End Sub

	'Private Sub UpdateVerticalScrollbar()
	'	'NOTE: Parent can be Nothing on exiting. Prevent the exception with this check.
	'	'If Not Me.theScrollingIsActive AndAlso Me.Parent IsNot Nothing AndAlso Me.theAutoScroll Then
	'	If Not Me.theScrollingIsActive AndAlso Me.Parent IsNot Nothing AndAlso Me.Scrollable Then
	'		Dim contentSize As Size = Me.GetContentSize()
	'		Dim contentHeight As Integer = contentSize.Height()
	'		Dim clientHeight As Integer = Me.ClientRectangle.Height
	'		If contentHeight > clientHeight Then
	'			Me.theScrollingIsActive = True

	'			'Me.CustomVerticalScrollBar.Minimum = 0
	'			'Me.CustomVerticalScrollBar.Maximum = contentHeight
	'			''Me.CustomVerticalScrollBar.Value = Win32Api.GetScrollPos(Me.Handle, Win32Api.ScrollBarType.SB_VERT)
	'			'Me.CustomVerticalScrollBar.ViewSize = clientHeight
	'			'Me.CustomVerticalScrollBar.SmallChange = Me.ItemHeight
	'			'Me.CustomVerticalScrollBar.LargeChange = Me.ItemHeight * 4
	'			'------
	'			Me.CustomVerticalScrollBar.Minimum = 0
	'			Me.CustomVerticalScrollBar.Maximum = contentHeight \ Me.ItemHeight
	'			Me.CustomVerticalScrollBar.Value = Win32Api.GetScrollPos(Me.Handle, Win32Api.ScrollBarType.SB_VERT)
	'			Me.CustomVerticalScrollBar.ViewSize = clientHeight \ Me.ItemHeight
	'			Me.CustomVerticalScrollBar.SmallChange = 1
	'			Me.CustomVerticalScrollBar.LargeChange = 4

	'			'NOTE: Assign to Parent so it can draw over non-client area.
	'			Me.CustomVerticalScrollBar.Parent = Me.Parent
	'			Me.CustomVerticalScrollBar.BringToFront()
	'			'NOTE: Point is relative to Me.ClientRectangle.
	'			'Dim aPoint As New Point(Me.ClientRectangle.Width + Me.NonClientPadding.Right - ScrollBarEx.Consts.ScrollBarSize, Me.ClientRectangle.Top - Me.NonClientPadding.Top)
	'			Dim aPoint As New Point(Me.ClientRectangle.Width + Me.NonClientPadding.Right - ScrollBarEx.Consts.ScrollBarSize * 2 - 5, Me.ClientRectangle.Top - Me.NonClientPadding.Top)
	'			'NOTE: Location must be relative to Parent.
	'			aPoint = Me.PointToScreen(aPoint)
	'			aPoint = Me.CustomVerticalScrollBar.Parent.PointToClient(aPoint)
	'			Me.CustomVerticalScrollBar.Location = aPoint
	'			'Me.CustomVerticalScrollBar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.Height)
	'			Me.CustomVerticalScrollBar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.ClientRectangle.Top + Me.NonClientPadding.Top + Me.ClientRectangle.Height + Me.NonClientPadding.Bottom)

	'			Me.CustomVerticalScrollBar.Show()

	'			Me.theScrollingIsActive = False
	'		Else
	'			Me.theScrollingIsActive = True
	'			Me.CustomVerticalScrollBar.Hide()
	'			Me.theScrollingIsActive = False
	'		End If
	'	End If
	'End Sub

	Private Sub UpdateVerticalScrollbar()
		If Not Me.theScrollingIsActive AndAlso Me.Scrollable Then
			Dim scrollBarInfo As New Win32Api.SCROLLBARINFO()
			scrollBarInfo.cbSize = Marshal.SizeOf(scrollBarInfo.[GetType]())
			Dim resultIsSuccess As Boolean = Win32Api.GetScrollBarInfo(Me.Handle, Win32Api.OBJID_VSCROLL, scrollBarInfo)

			If (scrollBarInfo.scrollbar And Win32Api.STATE_SYSTEM_INVISIBLE) = 0 Then
				Me.theScrollingIsActive = True

				Dim scrollInfo As Win32Api.SCROLLINFO
				Dim lRet As Integer
				scrollInfo.cbSize = Marshal.SizeOf(scrollInfo)
				scrollInfo.fMask = Win32Api.SIF_ALL
				lRet = Win32Api.GetScrollInfo(Me.Handle, Win32Api.ScrollBarType.SB_VERT, scrollInfo)
				If lRet > 0 Then
					Me.CustomVerticalScrollBar.Minimum = scrollInfo.nMin
					Me.CustomVerticalScrollBar.Maximum = scrollInfo.nMax
					Me.CustomVerticalScrollBar.Value = scrollInfo.nTrackPos
					' The -1 is needed for scrolling to partially-shown bottom-most node in tree.
					Me.CustomVerticalScrollBar.ViewSize = (Me.ClientRectangle.Height \ Me.ItemHeight) - 1
					Me.CustomVerticalScrollBar.SmallChange = 1
					Me.CustomVerticalScrollBar.LargeChange = scrollInfo.nPage
				End If

				'NOTE: Assign to Parent so it can draw over non-client area.
				Me.CustomVerticalScrollBar.Parent = Me.Parent
				Me.CustomVerticalScrollBar.BringToFront()
				Dim aPoint As New Point(Me.ClientRectangle.Width, Me.ClientRectangle.Top)
				'Dim aPoint As New Point(Me.ClientRectangle.Width - ScrollBarEx.Consts.ScrollBarSize, Me.ClientRectangle.Top)
				'NOTE: Location must be relative to Parent.
				aPoint = Me.PointToScreen(aPoint)
				aPoint = Me.PointToClient(aPoint)
				Me.CustomVerticalScrollBar.Location = aPoint
				Me.CustomVerticalScrollBar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.ClientRectangle.Height)
				Me.CustomVerticalScrollBar.Show()

				Me.theScrollingIsActive = False
			Else
				Me.theScrollingIsActive = True
				Me.CustomVerticalScrollBar.Hide()
				Me.theScrollingIsActive = False
			End If
		End If
	End Sub

#End Region

#Region "Data"

	Private NonClientPadding As Padding
	Private theNonClientPaddingColor As Color
	'Private theAutoScroll As Boolean
	'Private CustomHorizontalScrollbarPopup As Popup
	Private WithEvents CustomHorizontalScrollbar As ScrollBarEx
	Private WithEvents CustomVerticalScrollBar As ScrollBarEx
	Private ScrollbarCornerPanel As PanelEx
	Private theControlHasShown As Boolean
	Private theScrollingIsActive As Boolean
	Private theMouseWheelHasMoved As Boolean
	Private theOriginalFont As Font

	Private theTreePlusIcon As VisualStyleRenderer
	Private theTreeMinusIcon As VisualStyleRenderer
	Private theTextFormatFlags As TextFormatFlags

#End Region

End Class
