Imports System.Runtime.InteropServices

Public Class ListBoxEx
	Inherits ListBox

	Public Sub New()
		MyBase.New()

		Me.DrawMode = DrawMode.OwnerDrawFixed
		Me.thePreviousHighlightItemIndex = -2
		Me.theHighlightItemIndex = -1
		Me.HorizontalScrollbar = True
		Me.ScrollAlwaysVisible = False
		Me.theScrollingIsActive = False
		Me.theControlHasShown = False

		Me.CustomHorizontalScrollbar = New ScrollBarEx()
		Me.Controls.Add(Me.CustomHorizontalScrollbar)
		Me.CustomHorizontalScrollbar.Name = "CustomHorizontalScrollbar"
		Me.CustomHorizontalScrollbar.ScrollOrientation = ScrollBarEx.DarkScrollOrientation.Horizontal
		Me.CustomHorizontalScrollbar.TabIndex = 7
		Me.CustomHorizontalScrollbar.Text = "CustomHorizontalScrollbar"
		Me.CustomHorizontalScrollbar.Visible = False

		Me.CustomVerticalScrollBar = New ScrollBarEx()
		Me.Controls.Add(Me.CustomVerticalScrollBar)
		Me.CustomVerticalScrollBar.Name = "CustomVerticalScrollBar"
		Me.CustomVerticalScrollBar.ScrollOrientation = ScrollBarEx.DarkScrollOrientation.Vertical
		Me.CustomVerticalScrollBar.TabIndex = 7
		Me.CustomVerticalScrollBar.Visible = False
	End Sub

	Public Overloads ReadOnly Property PreferredSize() As Size
		Get
			'Return MyBase.PreferredSize
			Dim defaultSize As Size = MyBase.PreferredSize
			Dim maxWidth As Integer = defaultSize.Width
			Dim maxHeight As Integer = defaultSize.Height
			Dim itemText As String
			Dim textSize As Size
			For Each item As Object In Me.Items
				itemText = item.ToString()
				textSize = TextRenderer.MeasureText(itemText, Me.Font)
				If textSize.Width > maxWidth Then
					maxWidth = textSize.Width
				End If
				If textSize.Height > maxHeight Then
					maxHeight = textSize.Height
				End If
			Next
			Return New Size(maxWidth, maxHeight)
		End Get
	End Property

	Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
		'MyBase.OnDrawItem(e)

		If e.Index >= 0 AndAlso e.Index < Me.Items.Count Then
			e.DrawBackground()

			'Dim itemIsSelected As Boolean = ((e.State And DrawItemState.Selected) > 0)
			'Dim itemIsHighlighted As Boolean = (e.Index = Me.theHighlightItemIndex)
			'Dim itemTextForeColor As Color
			'Dim itemTextBackColor As Color

			'If itemIsHighlighted OrElse (itemIsSelected AndAlso Me.theHighlightItemIndex < 0) Then
			'	itemTextForeColor = Color.White
			'	itemTextBackColor = Color.Green
			'Else
			'	itemTextForeColor = Color.Black
			'	itemTextBackColor = Color.White
			'End If

			'Using backgroundColorBrush As New SolidBrush(itemTextBackColor)
			'	e.Graphics.FillRectangle(backgroundColorBrush, e.Bounds)
			'End Using

			'Dim itemText As String = Me.Items(e.Index).ToString()
			'Dim rect As Rectangle = e.Bounds
			'TextRenderer.DrawText(e.Graphics, itemText, e.Font, rect, itemTextForeColor, TextFormatFlags.Default)

			'If itemIsHighlighted OrElse (itemIsSelected AndAlso Me.theHighlightItemIndex < 0) Then
			'	e.DrawFocusRectangle()
			'End If

			'------

			Dim bounds As Rectangle = e.Bounds
			If Me.CustomVerticalScrollBar.Visible Then
				bounds.Width -= ScrollBarEx.Consts.ScrollBarSize
			End If

			Dim itemIsSelected As Boolean = ((e.State And DrawItemState.Selected) > 0)
			Dim itemTextForeColor As Color
			Dim itemTextBackColor As Color

			If itemIsSelected Then
				itemTextForeColor = WidgetConstants.WidgetTextColor
				itemTextBackColor = WidgetConstants.Windows10GlobalAccentColor
			Else
				itemTextForeColor = WidgetConstants.WidgetTextColor
				itemTextBackColor = Me.BackColor
			End If

			Using backgroundColorBrush As New SolidBrush(itemTextBackColor)
				e.Graphics.FillRectangle(backgroundColorBrush, bounds)
			End Using

			'Dim itemText As String = Me.Items(e.Index).ToString()
			Dim itemText As String = Me.GetItemText(Me.Items(e.Index))
			TextRenderer.DrawText(e.Graphics, itemText, e.Font, bounds, itemTextForeColor, TextFormatFlags.Default)

			If itemIsSelected Then
				e.DrawFocusRectangle()
			End If

		End If
	End Sub

	'Protected Overrides Sub OnEnter(e As EventArgs)
	'	MyBase.OnEnter(e)

	'End Sub

	'Protected Overrides Sub OnLeave(e As EventArgs)
	'	MyBase.OnLeave(e)
	'End Sub

	'NOTE: Why does this function disable arrow key movement (or at least not show the highlight moving)? Me.theHighlightItemIndex in OnDrawText().
	Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
		MyBase.OnMouseMove(e)

		Me.thePreviousHighlightItemIndex = Me.theHighlightItemIndex

		Dim cursorPositionInClient As Point = e.Location
		Me.theHighlightItemIndex = Me.IndexFromPoint(cursorPositionInClient)

		If Me.thePreviousHighlightItemIndex <> Me.theHighlightItemIndex Then
			Dim itemRect As Rectangle
			If Me.thePreviousHighlightItemIndex >= 0 Then
				itemRect = Me.GetItemRectangle(Me.thePreviousHighlightItemIndex)
				Me.Invalidate(itemRect)
			End If
			If Me.SelectedIndex >= 0 Then
				itemRect = Me.GetItemRectangle(Me.SelectedIndex)
				Me.Invalidate(itemRect)
			End If
			If Me.theHighlightItemIndex >= 0 Then
				itemRect = Me.GetItemRectangle(Me.theHighlightItemIndex)
				Me.SelectedIndex = Me.theHighlightItemIndex
				Me.Invalidate(itemRect)
			End If
		End If
	End Sub

	Protected Overrides Sub OnSizeChanged(e As EventArgs)
		'NOTE: Need this "If" to prevent unneeded resizing and painting when scrolling.
		If Not Me.theScrollingIsActive Then
			MyBase.OnSizeChanged(e)

			'NOTE: Force calling UpdateNonClientPadding() here so that the correct clientHeight is used for scrollbars.
			'NOTE: Raise the OnNonClientCalcSize and OnNonClientPaint "events".
			Win32Api.SetWindowPos(Me.Handle, IntPtr.Zero, 0, 0, 0, 0, Win32Api.SWP.SWP_FRAMECHANGED Or Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE Or Win32Api.SWP.SWP_NOZORDER)
			Me.UpdateScrollbars()
		End If
	End Sub

	Protected Overrides Sub OnVisibleChanged(e As EventArgs)
		MyBase.OnVisibleChanged(e)
		Me.theHighlightItemIndex = -1

		If Me.Visible Then
			If Not Me.theControlHasShown Then
				Me.theControlHasShown = True

				'NOTE: Raise the OnNonClientCalcSize and OnNonClientPaint "events".
				Win32Api.SetWindowPos(Me.Handle, IntPtr.Zero, 0, 0, 0, 0, Win32Api.SWP.SWP_FRAMECHANGED Or Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE Or Win32Api.SWP.SWP_NOZORDER)
			End If

			Me.UpdateScrollbars()
		End If
	End Sub

	'Protected Overrides Sub OnVisibleChanged(e As EventArgs)
	'	MyBase.OnVisibleChanged(e)
	'	Me.theHighlightItemIndex = -1
	'End Sub

	Protected Overrides Sub WndProc(ByRef m As Message)
		If Not Me.theScrollingIsActive Then
			Select Case m.Msg
				Case Win32Api.WindowsMessages.WM_NCCALCSIZE
					Me.OnNonClientCalcSize(m)
					'Case Win32Api.WindowsMessages.WM_NCPAINT
					'	Me.OnNonClientPaint(m)
			End Select
		End If

		MyBase.WndProc(m)
	End Sub

	Private Sub HorizontalScrollbar_ValueChanged(ByVal sender As Object, ByVal e As ScrollValueEventArgs) Handles CustomHorizontalScrollbar.ValueChanged
		Dim scrollInfo As Win32Api.SCROLLINFO
		Dim lRet As Integer
		scrollInfo.cbSize = Marshal.SizeOf(scrollInfo)
		scrollInfo.fMask = Win32Api.SIF_ALL
		lRet = Win32Api.GetScrollInfo(Me.Handle, Win32Api.ScrollBarType.SB_HORZ, scrollInfo)
		Dim pageChange As Integer = 0
		If lRet > 0 Then
			pageChange = scrollInfo.nPage
		End If
		Dim thumbValue As Integer = Win32Api.GetScrollPos(Me.Handle, Win32Api.ScrollBarType.SB_HORZ)
		Dim value As Integer = e.Value \ 6
		Dim currentThumbValue As Integer = thumbValue \ 6
		If value < currentThumbValue Then
			If currentThumbValue - value <= pageChange Then
				For i As Integer = currentThumbValue To value + 1 Step -1
					Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_HSCROLL, Win32Api.SB.SB_LINELEFT, IntPtr.Zero)
				Next
			Else
				Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_HSCROLL, Win32Api.SB.SB_PAGELEFT, IntPtr.Zero)
			End If
		ElseIf value > currentThumbValue Then
			If value - currentThumbValue <= pageChange Then
				For i As Integer = currentThumbValue To value - 1
					Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_HSCROLL, Win32Api.SB.SB_LINERIGHT, IntPtr.Zero)
				Next
			Else
				Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_HSCROLL, Win32Api.SB.SB_PAGERIGHT, IntPtr.Zero)
			End If
		End If
	End Sub

	Private Sub VerticalScrollBar_ValueChanged(ByVal sender As Object, ByVal e As ScrollValueEventArgs) Handles CustomVerticalScrollBar.ValueChanged
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
			If thumbValue - e.Value < pageChange Then
				For i As Integer = thumbValue To e.Value + 1 Step -1
					Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_VSCROLL, Win32Api.SB.SB_LINEUP, IntPtr.Zero)
				Next
			Else
				Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_VSCROLL, Win32Api.SB.SB_PAGEUP, IntPtr.Zero)
			End If
		ElseIf e.Value > thumbValue Then
			If e.Value - thumbValue < pageChange Then
				For i As Integer = thumbValue To e.Value - 1
					Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_VSCROLL, Win32Api.SB.SB_LINEDOWN, IntPtr.Zero)
				Next
			Else
				Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_VSCROLL, Win32Api.SB.SB_PAGEDOWN, IntPtr.Zero)
			End If
		End If

		' Works, except for some blinking and the thumb does not reach bottom.
		Me.UpdateScrollbars()
		'Me.CustomVerticalScrollBar.Invalidate()
		'------
		'Dim thumbValue As UInt32 = CUInt(e.Value * &H10000 + Win32Api.SB.SB_THUMBPOSITION)
		'Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_VSCROLL, thumbValue, IntPtr.Zero)
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
		Dim style As Integer = Win32Api.GetWindowLong(Me.Handle, Win32Api.GWL_STYLE)
		If (style And Win32Api.WindowsStyles.WS_HSCROLL) > 0 Then
			Win32Api.SetWindowLong(Me.Handle, Win32Api.GWL_STYLE, style And (Not Win32Api.WindowsStyles.WS_HSCROLL))
		End If
		If (style And Win32Api.WindowsStyles.WS_VSCROLL) > 0 Then
			Win32Api.SetWindowLong(Me.Handle, Win32Api.GWL_STYLE, style And (Not Win32Api.WindowsStyles.WS_VSCROLL))
		End If
	End Sub

	Private Sub UpdateNonClientPadding()
		If Me.DesignMode Then
			Exit Sub
		End If

		Dim left As Integer = 0
		Dim top As Integer = 0
		Dim right As Integer = 0
		Dim bottom As Integer = 0

		'Dim scrollBarInfo As New Win32Api.SCROLLBARINFO()
		'scrollBarInfo.cbSize = Marshal.SizeOf(scrollBarInfo.[GetType]())
		'Dim resultIsSuccess As Boolean = Win32Api.GetScrollBarInfo(Me.Handle, Win32Api.OBJID_VSCROLL, scrollBarInfo)
		'If (scrollBarInfo.scrollbar And Win32Api.STATE_SYSTEM_INVISIBLE) = 0 Then
		'	'right += scrollBarInfo.dxyLineButton
		'	right += scrollBarInfo.dxyLineButton - ScrollBarEx.Consts.ScrollBarSize
		'End If
		'If Me.HorizontalScrollbar Then
		'	resultIsSuccess = Win32Api.GetScrollBarInfo(Me.Handle, Win32Api.OBJID_HSCROLL, scrollBarInfo)
		'	If (scrollBarInfo.scrollbar And Win32Api.STATE_SYSTEM_INVISIBLE) = 0 Then
		'		'bottom += scrollBarInfo.dxyLineButton
		'		bottom += scrollBarInfo.dxyLineButton - ScrollBarEx.Consts.ScrollBarSize
		'	End If
		'End If

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

	Private Sub UpdateScrollbars()
		If Me.DesignMode Then
			Exit Sub
		End If

		Me.UpdateHorizontalScrollbar()
		Me.UpdateVerticalScrollbar()

		If Me.CustomHorizontalScrollbar.Visible AndAlso Me.CustomVerticalScrollBar.Visible Then
			Me.CustomHorizontalScrollbar.Size = New System.Drawing.Size(Me.Width - ScrollBarEx.Consts.ScrollBarSize, ScrollBarEx.Consts.ScrollBarSize)
			Me.CustomVerticalScrollBar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.Height - ScrollBarEx.Consts.ScrollBarSize)
		End If
	End Sub

	Private Sub UpdateHorizontalScrollbar()
		If Not Me.theScrollingIsActive AndAlso Me.HorizontalScrollbar Then
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
					Me.CustomHorizontalScrollbar.SmallChange = 6
					Me.CustomHorizontalScrollbar.LargeChange = scrollInfo.nPage
				End If

				Dim aPoint As New Point(Me.ClientRectangle.Left, Me.ClientRectangle.Height - ScrollBarEx.Consts.ScrollBarSize)
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
	'	If Not Me.theScrollingIsActive Then
	'		Me.theScrollingIsActive = True

	'		Dim scrollInfo As Win32Api.SCROLLINFO
	'		Dim lRet As Integer
	'		scrollInfo.cbSize = Marshal.SizeOf(scrollInfo)
	'		scrollInfo.fMask = Win32Api.SIF_ALL
	'		lRet = Win32Api.GetScrollInfo(Me.Handle, Win32Api.ScrollBarType.SB_VERT, scrollInfo)

	'		If lRet > 0 AndAlso scrollInfo.nPage > 0 AndAlso scrollInfo.nMax >= scrollInfo.nPage Then
	'			' Must set Minimum, Maximum, and ViewSize before Value. 
	'			Me.CustomVerticalScrollBar.Minimum = scrollInfo.nMin
	'			' The +1 is needed to handle the last item correctly.
	'			Me.CustomVerticalScrollBar.Maximum = scrollInfo.nMax + 1
	'			'' The -1 is needed for scrolling to partially-shown bottom-most node in tree.
	'			'Me.CustomVerticalScrollBar.ViewSize = (Me.ClientRectangle.Height \ Me.FontHeight) - 1
	'			Me.CustomVerticalScrollBar.ViewSize = scrollInfo.nPage
	'			Me.CustomVerticalScrollBar.Value = scrollInfo.nTrackPos
	'			Me.CustomVerticalScrollBar.SmallChange = 1
	'			Me.CustomVerticalScrollBar.LargeChange = scrollInfo.nPage

	'			''NOTE: Assign to Parent so it can draw over non-client area.
	'			''Me.CustomVerticalScrollBar.Parent = Me.Parent
	'			'Me.CustomVerticalScrollBar.Parent = Me
	'			'Me.CustomVerticalScrollBar.BringToFront()
	'			Dim aPoint As New Point(Me.ClientRectangle.Width - ScrollBarEx.Consts.ScrollBarSize, Me.ClientRectangle.Top)
	'			'NOTE: Location must be relative to Parent.
	'			aPoint = Me.PointToScreen(aPoint)
	'			aPoint = Me.PointToClient(aPoint)
	'			'aPoint = Me.CustomVerticalScrollBar.Parent.PointToClient(aPoint)
	'			Me.CustomVerticalScrollBar.Location = aPoint
	'			Me.CustomVerticalScrollBar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.ClientRectangle.Height)
	'			Me.CustomVerticalScrollBar.Show()
	'		Else
	'			Me.CustomVerticalScrollBar.Hide()
	'		End If

	'		Me.theScrollingIsActive = False
	'	End If
	'End Sub

	Private Sub UpdateVerticalScrollbar()
		If Not Me.theScrollingIsActive Then
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
					Me.CustomVerticalScrollBar.ViewSize = (Me.ClientRectangle.Height \ Me.FontHeight) - 1
					Me.CustomVerticalScrollBar.SmallChange = 1
					Me.CustomVerticalScrollBar.LargeChange = scrollInfo.nPage
				End If

				''NOTE: Assign to Parent so it can draw over non-client area.
				'Me.CustomVerticalScrollBar.Parent = Me.Parent
				'Me.CustomVerticalScrollBar.BringToFront()
				'Dim aPoint As New Point(Me.ClientRectangle.Width, Me.ClientRectangle.Top)
				Dim aPoint As New Point(Me.ClientRectangle.Width - ScrollBarEx.Consts.ScrollBarSize, Me.ClientRectangle.Top)
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

	Private thePreviousHighlightItemIndex As Integer
	Private theHighlightItemIndex As Integer

	Private NonClientPadding As Padding
	Private WithEvents CustomHorizontalScrollbar As ScrollBarEx
	Private WithEvents CustomVerticalScrollBar As ScrollBarEx
	Private theControlHasShown As Boolean
	Private theScrollingIsActive As Boolean

End Class
