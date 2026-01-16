Imports System.Runtime.InteropServices

Public Class ListViewEx
	Inherits ListView

	Public Sub New()
		MyBase.New()

		Me.BorderStyle = BorderStyle.None
		Me.ForeColor = WidgetTextColor
		Me.BackColor = WidgetDeepBackColor
		Me.OwnerDraw = True
		MyBase.Scrollable = True

		Me.FillerColumn = Nothing
		Me.theFillerColumnIsBeingAdded = False
		Me.theFillerColumnIsResizing = False
		Me.theListViewIsResizing = False

		Me.theNonClientPaddingColor = WidgetDeepBackColor
		Me.theScrollingIsActive = False
		Me.theMouseWheelHasMoved = False

		Me.CustomHorizontalScrollbar = New ScrollBarEx()
		Me.Controls.Add(Me.CustomHorizontalScrollbar)
		Me.CustomHorizontalScrollbar.Name = "CustomHorizontalScrollbar"
		Me.CustomHorizontalScrollbar.ScrollOrientation = ScrollBarEx.DarkScrollOrientation.Horizontal
		Me.CustomHorizontalScrollbar.TabIndex = 7
		Me.CustomHorizontalScrollbar.Visible = False

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
		'Me.theHeaderHeight = 24
	End Sub

	Public Overloads Sub AutoResizeColumns(headerAutoResize As ColumnHeaderAutoResizeStyle)
		If Me.Columns.Count > 0 Then
			MyBase.AutoResizeColumns(headerAutoResize)
			Me.UpdateScrollbars()
		End If
	End Sub

#Region "Widget Event Handlers"

#Region "filler column"

	'Private Sub ResizeFillerColumn()
	'	Me.theFillerColumnIsResizing = True
	'	'Dim columnsWidth As Integer = 0
	'	'For i As Integer = 0 To Me.Columns.Count - 2
	'	'	columnsWidth += Me.Columns(i).Width
	'	'Next
	'	'Me.Columns(Me.Columns.Count - 1).Width = Me.Width - columnsWidth
	'	Me.theFillerColumnIsResizing = False
	'End Sub

	'Protected Overrides Sub OnColumnReordered(e As ColumnReorderedEventArgs)
	'	Dim fillerColumnIndex As Integer = Me.Columns.Count - 1
	'	If e.OldDisplayIndex = fillerColumnIndex Then
	'		e.Cancel = True
	'	End If
	'	If e.NewDisplayIndex = fillerColumnIndex Then
	'		e.Cancel = True
	'	End If
	'	MyBase.OnColumnReordered(e)
	'End Sub

	'Private Sub ListView_ColumnWidthChanged(sender As Object, e As ColumnWidthChangedEventArgs) Handles Me.ColumnWidthChanged
	'	If Not theFillerColumnIsResizing Then
	'		ResizeFillerColumn()
	'	End If
	'End Sub

	'Private Sub ListView_ColumnWidthChanging(sender As Object, e As ColumnWidthChangingEventArgs) Handles Me.ColumnWidthChanging
	'	If Not theFillerColumnIsResizing Then
	'		ResizeFillerColumn()
	'	End If
	'End Sub

#End Region

	''Public Event ItemsCountChanged As EventHandler
	'Protected Overridable Sub OnItemsCountChanged(ByVal e As EventArgs)
	'	'RaiseEvent ItemsCountChanged(Me, e)
	'	''NOTE: Force calling UpdateNonClientPadding() here so that the correct clientHeight is used for scrollbars.
	'	''NOTE: Raise the OnNonClientCalcSize and OnNonClientPaint "events".
	'	'Win32Api.SetWindowPos(Me.Handle, IntPtr.Zero, 0, 0, 0, 0, Win32Api.SWP.SWP_FRAMECHANGED Or Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE Or Win32Api.SWP.SWP_NOZORDER)
	'	'Me.Invalidate()
	'	'Me.OnResize(EventArgs.Empty)
	'	'Me.OnSizeChanged(EventArgs.Empty)
	'	'Me.Width += 1
	'	'Me.Width -= 1
	'	Me.UpdateScrollbars()
	'End Sub

	Protected Overrides Sub OnColumnWidthChanged(e As ColumnWidthChangedEventArgs)
		MyBase.OnColumnWidthChanged(e)
		Me.Invalidate()
	End Sub

	'Protected Overrides Sub OnHandleDestroyed(e As EventArgs)
	'	If Me.ListViewHeader IsNot Nothing Then
	'		Me.ListViewHeader.ReleaseHandle()
	'	End If
	'	MyBase.OnHandleDestroyed(e)
	'End Sub

	Protected Overrides Sub OnSizeChanged(e As EventArgs)
		If Not Me.theListViewIsResizing AndAlso Me.Columns.Count > 0 Then
			Me.theListViewIsResizing = True

			'Dim columnsTotalWidth As Integer = 0
			'For i As Integer = 0 To Me.Columns.Count - 2
			'	columnsTotalWidth += Me.Columns(i).Width
			'	'= CInt((colPercentage * PackageListView.ClientRectangle.Width))
			'Next
			'If columnsTotalWidth < Me.Width Then
			'	Me.Columns(Me.Columns.Count - 1).Width = Me.Width - columnsTotalWidth
			'End If

			'If Me.FillerColumn Is Nothing Then
			'	Me.FillerColumn = Me.Columns.Add("", 1)
			'End If

			'Dim hDC As IntPtr = Win32Api.GetWindowDC(Me.Handle)
			'Try
			'	Using g As Graphics = Graphics.FromHdc(hDC)
			'		Using backColorBrush As New SolidBrush(Color.Red)
			'			Dim aRect As RectangleF = Me.ClientRectangle
			'			g.ResetClip()
			'			g.FillRectangle(backColorBrush, aRect)
			'		End Using
			'	End Using
			'Finally
			'	Win32Api.ReleaseDC(Me.Handle, hDC)
			'End Try

			Me.theListViewIsResizing = False
		End If

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

		If Me.Visible Then
			If Not Me.theControlHasShown Then
				Me.theControlHasShown = True

				'Me.AddFillerColumnIfNeeded()

				Dim hwnd As IntPtr = Win32Api.SendMessage(Me.Handle, Win32Api.ListViewMessages.LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero)
				If hwnd <> IntPtr.Zero Then
					Dim rect As New Rectangle()
					Dim blah As Integer = Win32Api.GetWindowRect(hwnd, rect)
					Me.theHeaderHeight = rect.Height
					'Me.ListViewHeader = New NativeListViewHeader(Me)
				End If

				'NOTE: Raise the OnNonClientCalcSize and OnNonClientPaint "events".
				Win32Api.SetWindowPos(Me.Handle, IntPtr.Zero, 0, 0, 0, 0, Win32Api.SWP.SWP_FRAMECHANGED Or Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE Or Win32Api.SWP.SWP_NOZORDER)
			End If

			Me.UpdateScrollbars()
		End If
	End Sub

	Protected Overrides Sub OnDrawColumnHeader(e As DrawListViewColumnHeaderEventArgs)
		'e.DrawDefault = True
		'MyBase.OnDrawColumnHeader(e)
		'------
		'If Me.CustomVerticalScrollBar.Visible Then
		'	Dim rect As Rectangle = Me.ClientRectangle
		'	rect.Width -= ScrollBarEx.Consts.ScrollBarSize - SystemInformation.VerticalScrollBarWidth
		'	e.Graphics.Clip = New Region(rect)
		'End If
		'Me.theHeaderHeight = e.Bounds.Height
		Using aBrush As New SolidBrush(WidgetHighBackColor)
			Dim aRect As Rectangle = e.Bounds
			aRect.Inflate(-1, 0)
			e.Graphics.FillRectangle(aBrush, aRect)
		End Using
		Dim textRect As Rectangle = e.Bounds
		textRect.X += 1
		TextRenderer.DrawText(e.Graphics, e.Header.Text, Me.Font, textRect, WidgetTextColor, TextFormatFlags.Left Or TextFormatFlags.VerticalCenter Or TextFormatFlags.SingleLine Or TextFormatFlags.EndEllipsis Or TextFormatFlags.PreserveGraphicsClipping)
		'If Me.CustomVerticalScrollBar.Visible Then
		'	e.Graphics.ResetClip()
		'End If
	End Sub

	Protected Overrides Sub OnDrawItem(e As DrawListViewItemEventArgs)
		'Me.theItemHeight = e.Bounds.Height
		e.DrawDefault = True
		MyBase.OnDrawItem(e)
	End Sub

	Protected Overrides Sub OnDrawSubItem(e As DrawListViewSubItemEventArgs)
		e.DrawDefault = True
		MyBase.OnDrawSubItem(e)
	End Sub

	'Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)
	'	MyBase.OnMouseWheel(e)
	'	Me.UpdateScrollbars()
	'End Sub

	'Protected Overrides Sub OnPaint(e As PaintEventArgs)
	'	MyBase.OnPaint(e)
	'End Sub

	'Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
	'	MyBase.OnPaintBackground(e)
	'End Sub

	Protected Overrides Sub WndProc(ByRef m As Message)
		If Not Me.theScrollingIsActive Then
			Select Case m.Msg
				'Case Win32Api.WindowsMessages.WM_VSCROLL
				'	Me.UpdateVerticalScrollbar()
				Case Win32Api.WindowsMessages.WM_NCCALCSIZE
					Me.OnNonClientCalcSize(m)
					'Case Win32Api.WindowsMessages.WM_NCPAINT
					'	Me.OnNonClientPaint(m)
					'Case Win32Api.WindowsMessages.WM_PAINT
					'	Me.OnClientPaint(m)
					'Case Win32Api.ListViewMessages.LVM_INSERTITEM, Win32Api.ListViewMessages.LVM_DELETEITEM, Win32Api.ListViewMessages.LVM_DELETEALLITEMS
					'	Me.OnItemsCountChanged(EventArgs.Empty)
					'Case Win32Api.ListViewMessages.LVM_INSERTCOLUMN
					'	Me.OnColumnAdding(EventArgs.Empty)
					'Case Win32Api.ListViewMessages.LVM_DELETECOLUMN
					'	Me.OnColumnDeleting(EventArgs.Empty)
			End Select
		End If

		MyBase.WndProc(m)
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
		'Dim style As Integer = Win32Api.GetWindowLong(Me.Handle, Win32Api.GWL_STYLE)
		'If (style And Win32Api.WindowsStyles.WS_HSCROLL) > 0 Then
		'	Win32Api.SetWindowLong(Me.Handle, Win32Api.GWL_STYLE, style And (Not Win32Api.WindowsStyles.WS_HSCROLL))
		'End If
		'If (style And Win32Api.WindowsStyles.WS_VSCROLL) > 0 Then
		'	Win32Api.SetWindowLong(Me.Handle, Win32Api.GWL_STYLE, style And (Not Win32Api.WindowsStyles.WS_VSCROLL))
		'End If
		'------
		'Dim hDC As IntPtr = Win32Api.GetWindowDC(Me.Handle)
		'Try
		'	Using g As Graphics = Graphics.FromHdc(hDC)
		'		Using backColorBrush As New SolidBrush(Color.Red)
		'			Dim aRect As RectangleF = g.VisibleClipBounds
		'			g.FillRectangle(backColorBrush, aRect)
		'		End Using
		'	End Using
		'Finally
		'	Win32Api.ReleaseDC(Me.Handle, hDC)
		'End Try
	End Sub

	'' Paint the background where custom scrollbars will be.
	'Private Sub OnNonClientPaint(ByRef m As Message)
	'	Dim hDC As IntPtr = Win32Api.GetWindowDC(Me.Handle)
	'	Try
	'		Using g As Graphics = Graphics.FromHdc(hDC)
	'			Using backColorBrush As New SolidBrush(Color.Red)
	'				Dim aRect As RectangleF = g.VisibleClipBounds
	'				g.FillRectangle(backColorBrush, aRect)
	'			End Using
	'		End Using
	'	Finally
	'		Win32Api.ReleaseDC(Me.Handle, hDC)
	'	End Try
	'	m.Result = IntPtr.Zero
	'End Sub

	'Private Sub OnClientPaint(ByRef m As Message)
	'	Dim hwnd As IntPtr = Win32Api.SendMessage(Me.Handle, Win32Api.ListViewMessages.LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero)
	'	If hwnd <> IntPtr.Zero Then
	'		Dim columnsTotalWidth As Integer = 0
	'		For i As Integer = 0 To Me.Columns.Count - 1
	'			columnsTotalWidth += Me.Columns(i).Width
	'		Next
	'		Dim rect As New Rectangle(columnsTotalWidth, 0, Me.Width - columnsTotalWidth, Me.Font.Height + 4)
	'		Dim headerRect As New Rectangle()
	'		Dim blah As Integer = Win32Api.GetWindowRect(hwnd, headerRect)
	'		Dim hDC As IntPtr = Win32Api.GetWindowDC(hwnd)
	'		Try
	'			Using g As Graphics = Graphics.FromHdc(hDC)
	'				Using backColorBrush As New SolidBrush(Color.Red)
	'					g.FillRectangle(backColorBrush, headerRect)
	'				End Using
	'			End Using
	'		Finally
	'			Win32Api.ReleaseDC(hwnd, hDC)
	'		End Try
	'	End If
	'	m.Result = IntPtr.Zero
	'End Sub

	Private Sub HorizontalScrollbar_ValueChanged(ByVal sender As Object, ByVal e As ScrollValueEventArgs) Handles CustomHorizontalScrollbar.ValueChanged
		'Me.UpdateScrolling(e.Value, 0)
		'------
		'Dim horizontalValue As Integer = Win32Api.GetScrollPos(Me.Handle, Win32Api.ScrollBarType.SB_HORZ)
		'If e.Value < horizontalValue Then
		'	If horizontalValue - e.Value <= 18 Then
		'		Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_HSCROLL, Win32Api.SB.SB_LINELEFT, IntPtr.Zero)
		'	Else
		'		Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_HSCROLL, Win32Api.SB.SB_PAGELEFT, IntPtr.Zero)
		'	End If
		'ElseIf e.Value > horizontalValue Then
		'	If e.Value - horizontalValue <= 18 Then
		'		Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_HSCROLL, Win32Api.SB.SB_LINERIGHT, IntPtr.Zero)
		'	Else
		'		Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_HSCROLL, Win32Api.SB.SB_PAGERIGHT, IntPtr.Zero)
		'	End If
		'End If
		'------
		'' Does not move internal scrollbar or contents.
		'If e.Value >= 0 Then
		'	Dim thumbValue As UInt32 = CUInt(e.Value * &H10000 + Win32Api.SB.SB_THUMBPOSITION)
		'	Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_HSCROLL, thumbValue, IntPtr.Zero)
		'End If
		'------
		'' Does not move contents.
		'Dim thumbValue As Integer = Win32Api.GetScrollPos(Me.Handle, Win32Api.ScrollBarType.SB_HORZ)
		'Win32Api.SetScrollPos(Me.Handle, Win32Api.ScrollBarType.SB_HORZ, e.Value, True)
		'------
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
		Dim value As Integer = e.Value
		Dim currentThumbValue As Integer = thumbValue
		If value < currentThumbValue Then
			If currentThumbValue - value < pageChange Then
				For i As Integer = currentThumbValue - 1 To value Step -1
					Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_VSCROLL, Win32Api.SB.SB_LINEUP, IntPtr.Zero)
				Next
			Else
				Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_VSCROLL, Win32Api.SB.SB_PAGEUP, IntPtr.Zero)
			End If
		ElseIf value > currentThumbValue Then
			If value - currentThumbValue < pageChange Then
				For i As Integer = currentThumbValue + 1 To value
					Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_VSCROLL, Win32Api.SB.SB_LINEDOWN, IntPtr.Zero)
				Next
			Else
				Win32Api.SendMessage(Me.Handle, Win32Api.WindowsMessages.WM_VSCROLL, Win32Api.SB.SB_PAGEDOWN, IntPtr.Zero)
			End If
		End If
	End Sub

#End Region

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

	Private Sub UpdateHorizontalScrollbar()
		If Not Me.theScrollingIsActive AndAlso Me.Scrollable Then
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
				'If lRet > 0 AndAlso scrollInfo.nPage > 0 AndAlso scrollInfo.nMax >= scrollInfo.nPage Then
				If lRet > 0 Then
					Me.CustomHorizontalScrollbar.Minimum = scrollInfo.nMin
					Me.CustomHorizontalScrollbar.Maximum = scrollInfo.nMax
					Me.CustomHorizontalScrollbar.Value = scrollInfo.nTrackPos
					'Me.CustomHorizontalScrollbar.ViewSize = scrollInfo.nPage
					Me.CustomHorizontalScrollbar.ViewSize = Me.ClientRectangle.Width
					Me.CustomHorizontalScrollbar.SmallChange = 6
					Me.CustomHorizontalScrollbar.LargeChange = scrollInfo.nPage
				End If

				'NOTE: Assign to Parent so it can draw over non-client area.
				Me.CustomHorizontalScrollbar.Parent = Me.Parent
				Me.CustomHorizontalScrollbar.BringToFront()
				Dim aPoint As New Point(Me.ClientRectangle.Left, Me.ClientRectangle.Height)
				'Dim aPoint As New Point(Me.ClientRectangle.Left, Me.ClientRectangle.Height - ScrollBarEx.Consts.ScrollBarSize)
				'NOTE: Location must be relative to Parent.
				aPoint = Me.PointToScreen(aPoint)
				'aPoint = Me.CustomHorizontalScrollbar.Parent.PointToClient(aPoint)
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
				'If lRet > 0 AndAlso scrollInfo.nPage > 0 AndAlso scrollInfo.nMax >= scrollInfo.nPage Then
				If lRet > 0 Then
					Me.CustomVerticalScrollBar.Minimum = scrollInfo.nMin
					'' The +1 is needed to handle the last item correctly.
					'Me.CustomVerticalScrollBar.Maximum = scrollInfo.nMax + 1
					Me.CustomVerticalScrollBar.Maximum = scrollInfo.nMax
					Me.CustomVerticalScrollBar.Value = scrollInfo.nTrackPos
					'' The +2 is for the empty space above and below text. The -1 is needed for scrolling to partially-shown bottom-most node in tree.
					'Me.CustomVerticalScrollBar.ViewSize = ((Me.ClientRectangle.Height - Me.theHeaderHeight) \ (Me.FontHeight + 2)) - 1
					'Me.CustomVerticalScrollBar.ViewSize = ((Me.ClientRectangle.Height - Me.theHeaderHeight) \ Me.theItemHeight) + 1
					'Me.CustomVerticalScrollBar.ViewSize = scrollInfo.nPage
					'Me.CustomVerticalScrollBar.ViewSize = Me.ClientRectangle.Height
					'Me.CustomVerticalScrollBar.ViewSize = (Me.ClientRectangle.Height \ Me.theHeaderHeight) - 1
					' The -1 is needed for scrolling to partially-shown bottom-most node in tree.
					'Me.CustomVerticalScrollBar.ViewSize = (Me.ClientRectangle.Height \ Me.FontHeight) - 1
					' The -5 gives best results. No idea why.
					Me.CustomVerticalScrollBar.ViewSize = (Me.ClientRectangle.Height - Me.NonClientPadding.Bottom) \ Me.FontHeight - 5
					'Me.CustomVerticalScrollBar.ViewSize = (Me.ClientRectangle.Height - Me.NonClientPadding.Bottom) \ Me.FontHeight - CInt(Me.FontHeight * 0.5)
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

	'Private ListViewHeader As NativeListViewHeader = Nothing
	Private NonClientPadding As Padding
	Private theNonClientPaddingColor As Color
	Private WithEvents CustomHorizontalScrollbar As ScrollBarEx
	Private WithEvents CustomVerticalScrollBar As ScrollBarEx
	Private ScrollbarCornerPanel As PanelEx
	Private theControlHasShown As Boolean
	Private theScrollingIsActive As Boolean
	Private theMouseWheelHasMoved As Boolean
	Private theHeaderHeight As Integer
	'Private theItemHeight As Integer
	Private theListViewIsResizing As Boolean
	Private FillerColumn As ColumnHeader
	Private theFillerColumnIsBeingAdded As Boolean
	Private theFillerColumnIsBeingDeleted As Boolean
	Private theFillerColumnIsResizing As Boolean

	'Friend Class NativeListViewHeader
	'	Inherits NativeWindow

	'	Public Sub New(parent As ListViewEx)
	'		Dim hwnd As IntPtr = Win32Api.SendMessage(parent.Handle, Win32Api.ListViewMessages.LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero)
	'		If hwnd <> IntPtr.Zero Then
	'			AssignHandle(hwnd)
	'		End If
	'		Me.theListViewEx = parent
	'	End Sub

	'	Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
	'		Select Case m.Msg
	'			'Case Win32Api.WindowsMessages.WM_ERASEBKGND
	'			'	Me.OnEraseBackground()
	'			'	m.Result = CType(1, IntPtr)
	'			'	Return
	'			Case Win32Api.WindowsMessages.WM_PAINT
	'				Me.OnClientPaint(m)
	'				m.Result = IntPtr.Zero
	'				Return
	'		End Select

	'		MyBase.WndProc(m)

	'		'Select Case m.Msg
	'		'	Case Win32Api.WindowsMessages.WM_PAINT
	'		'		Me.OnClientPaint(m)
	'		'		m.Result = IntPtr.Zero
	'		'End Select
	'	End Sub

	'	Private Sub OnEraseBackground()
	'		Dim headerRect As New Rectangle()
	'		Dim blah As Integer = Win32Api.GetWindowRect(Me.Handle, headerRect)
	'		Dim hDC As IntPtr = Win32Api.GetWindowDC(Me.Handle)
	'		Try
	'			Using g As Graphics = Graphics.FromHdc(hDC)
	'				Using backColorBrush As New SolidBrush(Color.Red)
	'					g.FillRectangle(backColorBrush, headerRect)
	'				End Using
	'			End Using
	'		Finally
	'			Win32Api.ReleaseDC(Me.Handle, hDC)
	'		End Try
	'	End Sub

	'	Private Sub OnClientPaint(m As Message)
	'		Dim columnsTotalWidth As Integer = 0
	'		For i As Integer = 0 To Me.theListViewEx.Columns.Count - 1
	'			columnsTotalWidth += Me.theListViewEx.Columns(i).Width
	'		Next
	'		Dim rect As New Rectangle(columnsTotalWidth, 0, Me.theListViewEx.Width - columnsTotalWidth, Me.theListViewEx.Font.Height + 4)
	'		Dim headerRect As New Rectangle()
	'		Dim blah As Integer = Win32Api.GetWindowRect(Me.Handle, headerRect)
	'		Dim hDC As IntPtr
	'		If m.WParam <> IntPtr.Zero Then
	'			hDC = m.WParam
	'		Else
	'			hDC = Win32Api.GetWindowDC(Me.Handle)
	'		End If
	'		Try
	'			Using g As Graphics = Graphics.FromHdc(hDC)
	'				Using backColorBrush As New SolidBrush(Color.Red)
	'					g.FillRectangle(backColorBrush, headerRect)
	'				End Using
	'			End Using
	'		Finally
	'			Win32Api.ReleaseDC(Me.Handle, hDC)
	'		End Try
	'	End Sub

	'	Private theListViewEx As ListViewEx

	'End Class

End Class
