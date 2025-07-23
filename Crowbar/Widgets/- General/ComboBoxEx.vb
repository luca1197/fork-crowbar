Imports System.Runtime.InteropServices

Public Class ComboBoxEx
	Inherits ComboBox

	Public Sub New()
		MyBase.New()

		Me.ForeColor = WidgetTextColor
		Me.BackColor = WidgetHighBackColor

		Me.DrawMode = DrawMode.OwnerDrawFixed
		Me.SetStyle(ControlStyles.UserPaint, True)

		'FROM: https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.combobox.dropdownwidth?view=netframework-4.0
		'    The width of the drop-down cannot be smaller than the ComboBox width.
		'Me.DropDownWidth -= ScrollBarEx.Consts.ScrollBarSize

		Me.VerticalScrollbar = New ScrollBarEx()
		'' Must place Parent assignment here to prevent scrollbar from showing immediately.
		'Me.VerticalScrollbar.Parent = Me.FindForm
		'Me.VerticalScrollbar.Location = New System.Drawing.Point(0, 0)
		Me.VerticalScrollbar.Name = "VerticalScrollbar"
		'Me.VerticalScrollbar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, 100)
		Me.VerticalScrollbar.ScrollOrientation = ScrollBarEx.DarkScrollOrientation.Vertical
		Me.VerticalScrollbar.TabIndex = 3
		Me.VerticalScrollbar.Text = "VerticalScrollbar"
		'Me.VerticalScrollbar.Visible = False
		Me.VerticalScrollbar.Visible = True
		Me.VerticalScrollbar.ForeColor = Color.Green
		Me.VerticalScrollbar.BackColor = WidgetBackColor
		'Me.VerticalScrollbar.Dock = DockStyle.Fill

		Me.VerticalScrollbarPopup = New Popup(Me.VerticalScrollbar)
		Me.VerticalScrollbarPopup.Name = "VerticalScrollbarPopup"

		Me.theScrollingIsActive = False
		Me.theListViewIsOpen = False
	End Sub

	Public Property IsReadOnly() As Boolean
		Get
			Return Me.theControlIsReadOnly
		End Get
		Set(ByVal value As Boolean)
			If Me.theControlIsReadOnly <> value Then
				Me.theControlIsReadOnly = value

				'TODO: Somehow disable value selection (i.e. no dropdown)
				'Me.Enabled = Not Me.theControlIsReadOnly
				If Me.theControlIsReadOnly Then
					Me.ForeColor = WidgetDisabledTextColor
					Me.BackColor = WidgetHighDisabledBackColor
				Else
					Me.ForeColor = WidgetTextColor
					Me.BackColor = WidgetHighBackColor
				End If
			End If
		End Set
	End Property

	'Public Property ListBorderColor As Color
	'	Get
	'		Return m_ListBorderColor
	'	End Get
	'	Set
	'		m_ListBorderColor = Value
	'		If listControl IsNot Nothing Then
	'			listControl.BorderColor = m_ListBorderColor
	'		End If
	'	End Set
	'End Property

#Region "Widget Event Handlers"

	Protected Overrides Sub OnDropDown(e As EventArgs)
		MyBase.OnDropDown(e)
		Me.theListViewIsOpen = True
		Me.UpdateVerticalScrollbar()
	End Sub

	Protected Overrides Sub OnDropDownClosed(e As EventArgs)
		MyBase.OnDropDownClosed(e)
		Me.theListViewIsOpen = False
		Me.UpdateVerticalScrollbar()
	End Sub

	Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
		'MyBase.OnDrawItem(e)

		If e.Index >= 0 Then
			' Draw drop-down-list text background.
			If (e.State And DrawItemState.Focus) > 0 Then
				Using backColorBrush As New SolidBrush(WidgetHighSelectedBackColor)
					e.Graphics.FillRectangle(backColorBrush, e.Bounds)
				End Using
			Else
				Using backColorBrush As New SolidBrush(WidgetHighBackColor)
					e.Graphics.FillRectangle(backColorBrush, e.Bounds)
				End Using
			End If

			' Draw drop-down-list text.
			TextRenderer.DrawText(e.Graphics, Me.GetItemText(Me.Items(e.Index)), e.Font, e.Bounds, WidgetTextColor, TextFormatFlags.Left Or TextFormatFlags.VerticalCenter)
		End If
	End Sub

	Protected Overrides Sub OnHandleCreated(e As EventArgs)
		MyBase.OnHandleCreated(e)
		Me.DropDownListControl = New ListNativeWindow(Win32Api.GetComboBoxListInternal(Me.Handle), Me)
		'Me.DropDownListControl.Padding = New Padding(0, 0, ScrollBarEx.Consts.ScrollBarSize, 0)
	End Sub

	Protected Overrides Sub OnHandleDestroyed(e As EventArgs)
		Me.DropDownListControl.ReleaseHandle()
		MyBase.OnHandleDestroyed(e)
	End Sub

	Protected Overrides Sub OnSelectedIndexChanged(e As EventArgs)
		MyBase.OnSelectedIndexChanged(e)
	End Sub

	Protected Overrides Sub OnKeyDown(ByVal e As System.Windows.Forms.KeyEventArgs)
		If Me.theControlIsReadOnly Then
			e.Handled = True
		End If
		MyBase.OnKeyDown(e)
	End Sub

	Protected Overrides Sub OnKeyPress(ByVal e As System.Windows.Forms.KeyPressEventArgs)
		If Me.theControlIsReadOnly Then
			e.Handled = True
		End If
		MyBase.OnKeyPress(e)
	End Sub

	Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
		MyBase.OnPaint(e)

		' Draw textbox text.
		'TextRenderer.DrawText(e.Graphics, Me.Text, Me.Font, e.ClipRectangle, WidgetTextColor, TextFormatFlags.Left Or TextFormatFlags.VerticalCenter Or TextFormatFlags.LeftAndRightPadding)
		TextRenderer.DrawText(e.Graphics, Me.Text, Me.Font, Me.ClientRectangle, WidgetTextColor, TextFormatFlags.Left Or TextFormatFlags.VerticalCenter Or TextFormatFlags.LeftAndRightPadding)

		' Draw drop-down arrow.
		Dim dropDownRect As New Rectangle(Me.ClientRectangle.Right - SystemInformation.HorizontalScrollBarArrowWidth, Me.ClientRectangle.Y, SystemInformation.HorizontalScrollBarArrowWidth, Me.ClientRectangle.Height)
		Dim middle As New Point(CInt(dropDownRect.Left + dropDownRect.Width / 2), CInt(dropDownRect.Top + dropDownRect.Height / 2))
		Dim arrow As Point() = {New Point(middle.X - 3, middle.Y - 2), New Point(middle.X + 4, middle.Y - 2), New Point(middle.X, middle.Y + 2)}
		Using backColorBrush As New SolidBrush(WidgetDisabledTextColor)
			e.Graphics.FillPolygon(backColorBrush, arrow)
		End Using
	End Sub

	Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
		'MyBase.OnPaintBackground(e)

		' Draw textbox background.
		Using backColorBrush As New SolidBrush(WidgetHighBackColor)
			e.Graphics.FillRectangle(backColorBrush, Me.ClientRectangle)
		End Using

		' Draw textbox border.
		Using thinBorderPen As New Pen(WidgetDisabledTextColor, 1)
			Dim thinBorderRect As Rectangle = Me.ClientRectangle
			'thinBorderRect.Inflate(2, 2)
			e.Graphics.DrawRectangle(thinBorderPen, thinBorderRect)
		End Using
	End Sub

	'Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
	'	Select Case m.Msg
	'		'NOTE: This message occurs before the OnDropDown() is called; it is probably called from this message in MyBase.WndProc().
	'		'Case Win32Api.WindowsMessages.WM_REFLECT + Win32Api.WindowsMessages.WM_COMMAND
	'		'	If Win32Api.HiWord(CLng(m.WParam)) = Win32Api.ComboBoxNotifications.CBN_DROPDOWN Then
	'		'		'	COMBOBOXINFO combInfo = New COMBOBOXINFO()
	'		'		'	combInfo.cbSize = Marshal.SizeOf(combInfo)
	'		'		'	Win32.GetComboBoxInfo(this.Handle, ref combInfo )

	'		'		'	VScrollBar.Location = New Point(this.Width - 23, 1)
	'		'		'	VScrollBar.Size = New Size(23, DropDownHeight)
	'		'		'	VScrollBar.Visible = True
	'		'		'	Win32.SetParent(VScrollBar.Handle, combInfo.hwndList)
	'		'		'	Win32.ShowWindow(VScrollBar.Handle, ShowWindowCommands.Show)
	'		'		'	Win32.SetWindowPos(VScrollBar.Handle, HWND.TopMost, 155, 1, 23, 105, SetWindowPosFlags.SWP_SHOWWINDOW)
	'		'		Me.UpdateVerticalScrollbar()
	'		'	End If
	'	End Select

	'	MyBase.WndProc(m)
	'End Sub

#End Region

#Region "Child Widget Event Handlers"

	'Private Sub VerticalScrollBar_ValueChanged(ByVal sender As Object, ByVal e As ScrollValueEventArgs) Handles VerticalScrollbar.ValueChanged
	'	Me.UpdateScrolling(0, e.Value)
	'End Sub

#End Region

#Region "Private Methods"

	'Private Sub UpdateScrolling(ByVal leftOrRightValue As Integer, ByVal upOrDownValue As Integer)
	'	If Not Me.theScrollingIsActive Then
	'		Me.theScrollingIsActive = True

	'		Dim scrollPosition As New Point(leftOrRightValue, upOrDownValue)
	'		Win32Api.RtfScroll(Me.Handle, Win32Api.WindowsMessages.EM_SETSCROLLPOS, IntPtr.Zero, scrollPosition)

	'		Me.theScrollingIsActive = False
	'	End If
	'End Sub

	Private Sub UpdateVerticalScrollbar()
		'NOTE: Parent can be Nothing on exiting. Prevent the exception with this check.
		If Not Me.theScrollingIsActive AndAlso Me.Parent IsNot Nothing Then
			If Me.theListViewIsOpen Then
				Dim contentHeight As Integer = Me.Items.Count * Me.ItemHeight
				If contentHeight > Me.MaxDropDownItems * Me.ItemHeight Then
					Me.theScrollingIsActive = True

					Me.VerticalScrollbar.Minimum = 0
					Me.VerticalScrollbar.Maximum = contentHeight
					Me.VerticalScrollbar.Value = Me.SelectedIndex * Me.ItemHeight
					Me.VerticalScrollbar.ViewSize = Me.MaxDropDownItems * Me.ItemHeight
					Me.VerticalScrollbar.SmallChange = Me.ItemHeight
					Me.VerticalScrollbar.LargeChange = Me.MaxDropDownItems * (Me.ItemHeight - 2)

					'NOTE: The DropDownListControl Y is ComboBoxEx Height. This seems to give coordinates relative to ComboBoxEx. This changes between showings for unknown reason.
					'NOTE: +1 on Height to skip the dropdownlist border.

					''NOTE: Assign to Parent so it can draw over non-client area.
					''NOTE: Assigning a parent also shows the scrollbar.
					''Me.VerticalScrollbar.Parent = Me
					''Me.VerticalScrollbar.Parent = Me.Parent
					''Win32Api.SetParent(Me.VerticalScrollbar.Handle, Me.DropDownListControl.Handle)
					''Me.VerticalScrollbar.Parent = Nothing
					''Me.VerticalScrollbar.BringToFront()
					''NOTE: Location must be relative to Parent.
					'''Dim aPoint As New Point(Me.DropDownWidth - ScrollBarEx.Consts.ScrollBarSize, Me.Top + Me.Bottom)
					''Dim aPoint As New Point(Me.DropDownWidth - ScrollBarEx.Consts.ScrollBarSize - 1, Me.Top + Me.Bottom)
					'''Dim aPoint As New Point(Me.DropDownWidth - ScrollBarEx.Consts.ScrollBarSize * 2, 0)
					''Dim aPoint As New Point(Me.Width - ScrollBarEx.Consts.ScrollBarSize - 10, 10)
					'Dim aPoint As New Point(Me.Width - ScrollBarEx.Consts.ScrollBarSize + 10, Me.Height + 1)
					''Dim aPoint As New Point(100, 100)
					''Dim aPoint As New Point(Me.ClientRectangle.Width + 1, Me.ClientRectangle.Top - 1)
					'aPoint = Me.PointToScreen(aPoint)
					''aPoint = Me.VerticalScrollbar.Parent.PointToClient(aPoint)
					'Me.VerticalScrollbar.Location = aPoint
					'''Me.VerticalScrollbar.Location = New Point(0, 0)
					''Me.VerticalScrollbar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.DropDownListControl.WindowRectangle.Height)
					''Me.VerticalScrollbar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.MaxDropDownItems * Me.ItemHeight)
					'Me.VerticalScrollbar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, 50)
					''Me.VerticalScrollbar.Parent = Me
					''Me.VerticalScrollbar.Hide()
					'Me.VerticalScrollbar.Show()
					'Me.VerticalScrollbar.BringToFront()
					'------
					'Me.VerticalScrollbar.Location = New System.Drawing.Point(Me.DropDownWidth, Me.Top + Me.Bottom)
					'Me.VerticalScrollbar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.DropDownHeight)
					'------
					'Win32Api.SetParent(Me.VerticalScrollbarPopup.Handle, Me.DropDownListControl.Handle)
					'Me.VerticalScrollbar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.MaxDropDownItems * Me.ItemHeight)
					Me.VerticalScrollbar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, 50)
					'Dim aPoint As New Point(Me.Width - ScrollBarEx.Consts.ScrollBarSize - 1, Me.Height + 1)
					Dim aPoint As New Point(Me.Width - ScrollBarEx.Consts.ScrollBarSize + 10, Me.Height + 1)
					'Dim aPoint As New Point(100, 200)
					aPoint = Me.PointToScreen(aPoint)
					'aPoint = Me.VerticalScrollbarPopup.PointToClient(aPoint)
					Me.VerticalScrollbarPopup.Location = aPoint
					'Me.VerticalScrollbarPopup.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.MaxDropDownItems * Me.ItemHeight)
					'Win32Api.SetWindowPos(Me.DropDownListControl.Handle, Win32Api.HWND_NOTOPMOST, 0, 0, 0, 0, Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE)
					'Win32Api.SetWindowPos(Me.VerticalScrollbarPopup.Handle, Win32Api.HWND_TOPMOST, 0, 0, 0, 0, Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE)
					Me.VerticalScrollbarPopup.Show(aPoint)
					Me.VerticalScrollbarPopup.BringToFront()
					'Me.VerticalScrollbarPopup.Invalidate()

					''TEST: Show the scrollbar, then adjust the listview's non-client size.
					'Me.DropDownListControl.Padding = New Padding(0, 0, ScrollBarEx.Consts.ScrollBarSize, 0)
					''NOTE: Raise the OnNonClientCalcSize and OnNonClientPaint "events".
					'Win32Api.SetWindowPos(Me.DropDownListControl.Handle, IntPtr.Zero, 0, 0, 0, 0, Win32Api.SWP.SWP_FRAMECHANGED Or Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE Or Win32Api.SWP.SWP_NOZORDER)

					Me.theScrollingIsActive = False
				End If
			Else
				'Me.VerticalScrollbar.Hide()
				Me.VerticalScrollbarPopup.Hide()
			End If
		End If
	End Sub

#End Region

#Region "Data"

	Private theControlIsReadOnly As Boolean

	Private DropDownListControl As ListNativeWindow = Nothing
	Private WithEvents VerticalScrollbar As ScrollBarEx
	Private VerticalScrollbarPopup As Popup
	Private theScrollingIsActive As Boolean
	Private theListViewIsOpen As Boolean

#End Region

	Public Class ListNativeWindow
		Inherits NativeWindow

		Public Sub New(hWnd As IntPtr, parent As ComboBoxEx)
			If hWnd <> IntPtr.Zero Then
				AssignHandle(hWnd)
			End If
			Me.theComboBoxEx = parent

			'Me.theWindowRectangle = New Rectangle()
			'Me.theClientRectangle = New Rectangle()
			Me.thePadding = New Padding()

			'Me.theClientRectangleIsAlreadySet = False

			'' Hide the default scrollbar.
			'Dim style As Integer = Win32Api.GetWindowLong(Me.Handle, Win32Api.GWL_STYLE)
			'If (style And Win32Api.WindowsStyles.WS_VSCROLL) > 0 Then
			'	Win32Api.SetWindowLong(Me.Handle, Win32Api.GWL_STYLE, style And (Not Win32Api.WindowsStyles.WS_VSCROLL))
			'	''NOTE: Raise the OnNonClientCalcSize and OnNonClientPaint "events".
			'	'Win32Api.SetWindowPos(Me.Handle, IntPtr.Zero, 0, 0, 0, 0, Win32Api.SWP.SWP_FRAMECHANGED Or Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE Or Win32Api.SWP.SWP_NOZORDER)
			'End If
			'If (style And Win32Api.WindowsStyles.WS_VSCROLL) > 0 Then
			'	Dim debug As Integer = 4242
			'End If
			''NOTE: The ListNativeWindow Y is ComboBoxEx Height. This seems to give coordinates relative to ComboBoxEx.
			'Win32Api.GetWindowRect(Me.Handle, Me.theWindowRectangle)
			'Dim hDC As IntPtr = Win32Api.GetWindowDC(Me.Handle)
			'Try
			'	Using g As Graphics = Graphics.FromHdc(hDC)
			'		Dim aRect As RectangleF = g.VisibleClipBounds
			'		Me.theWindowRectangle = Rectangle.Round(aRect)
			'	End Using
			'Finally
			'	Win32Api.ReleaseDC(Me.Handle, hDC)
			'End Try

			'Me.VerticalScrollbar = New ScrollBarEx()
			'Me.VerticalScrollbar.Location = New System.Drawing.Point(0, 0)
			'Me.VerticalScrollbar.Name = "VerticalScrollbar"
			'Me.VerticalScrollbar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, 0)
			'Me.VerticalScrollbar.ScrollOrientation = ScrollBarEx.DarkScrollOrientation.Vertical
			'Me.VerticalScrollbar.TabIndex = 3
			'Me.VerticalScrollbar.Text = "VerticalScrollbar"
			''Me.VerticalScrollbar.Visible = False
			'Me.VerticalScrollbar.Visible = True
			''Me.VerticalScrollbar.ForeColor = Color.Green
			'Me.VerticalScrollbar.BackColor = WidgetBackColor

			'Me.theScrollbarIsShowingForFirstTime = True
		End Sub

		Public Property WindowRectangle As Rectangle
			Get
				Win32Api.GetWindowRect(Me.Handle, Me.theWindowRectangle)
				Return Me.theWindowRectangle
			End Get
			Set
				Me.theWindowRectangle = Value
			End Set
		End Property

		'Public Property ClientRectangle As Rectangle
		'	Get
		'		Return Me.theClientRectangle
		'	End Get
		'	Set
		'		Me.theClientRectangle = Value
		'	End Set
		'End Property

		Public Property Padding As Padding
			Get
				Return Me.thePadding
			End Get
			Set
				Me.thePadding = Value
			End Set
		End Property

		Protected Overrides Sub WndProc(ByRef m As Message)
			Select Case m.Msg
				Case Win32Api.WindowsMessages.WM_NCCALCSIZE
					Me.OnNonClientCalcSize(m)
					'Case Win32Api.WindowsMessages.WM_PAINT
					'	Me.OnPaint(m)
					'	m.Result = IntPtr.Zero
					'	Return
			End Select

			MyBase.WndProc(m)

			Select Case m.Msg
				'Case Win32Api.WindowsMessages.WM_ERASEBKGND
				'	m.Result = CType(1, IntPtr)
				'NOTE: Windows still draws a 1-pixel-wide border, so draw over it AFTER calling MyBase.WndProc.
				Case Win32Api.WindowsMessages.WM_NCPAINT
					Me.OnNonClientPaint(m)
				Case Win32Api.WindowsMessages.WM_PAINT
					Me.OnPaint(m)
			End Select
		End Sub

		Private Sub OnNonClientCalcSize(ByRef m As Message)
			' Hide the default scrollbar.
			Dim style As Integer = Win32Api.GetWindowLong(Me.Handle, Win32Api.GWL_STYLE)
			If (style And Win32Api.WindowsStyles.WS_VSCROLL) > 0 Then
				Win32Api.SetWindowLong(Me.Handle, Win32Api.GWL_STYLE, style And (Not Win32Api.WindowsStyles.WS_VSCROLL))
			End If
			'If (style And Win32Api.WindowsStyles.WS_VSCROLL) > 0 Then
			'	Dim debug As Integer = 4242
			'End If

			''NOTE: This is the only way I could get the popup in front of the dropdownlist, but not on first time showing dropdownlist.
			'Win32Api.SetWindowPos(Me.Handle, Win32Api.HWND_NOTOPMOST, 0, 0, 0, 0, Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE)

			''NOTE: The ListNativeWindow Y is ComboBoxEx Height. This seems to give coordinates relative to ComboBoxEx.
			'Win32Api.GetWindowRect(Me.Handle, Me.theWindowRectangle)

			'Dim exstyle As Integer = Win32Api.GetWindowLong(Me.Handle, Win32Api.GWL_EXSTYLE)
			If CInt(m.WParam) = 0 Then
				Dim rect As Win32Api.RECT = CType(Marshal.PtrToStructure(m.LParam, GetType(Win32Api.RECT)), Win32Api.RECT)
				Me.ResizeClientRect(Me.thePadding, rect)
				Marshal.StructureToPtr(rect, m.LParam, False)
				m.Result = IntPtr.Zero

				'If (exstyle And Win32Api.WindowsStyles.WS_EX_TOPMOST) > 0 Then
				'	'NOTE: Remove the 1-pixel-wide border to make ClientRectangle.
				'	Me.theClientRectangle.X = rect.X + 1
				'	Me.theClientRectangle.Y = rect.Y + 1
				'	Me.theClientRectangle.Width = rect.Width - 2
				'	Me.theClientRectangle.Height = rect.Height - 2
				'End If
			ElseIf CInt(m.WParam) = 1 Then
				Dim nccsp As Win32Api.NCCALCSIZE_PARAMS = CType(Marshal.PtrToStructure(m.LParam, GetType(Win32Api.NCCALCSIZE_PARAMS)), Win32Api.NCCALCSIZE_PARAMS)
				Me.ResizeClientRect(Me.thePadding, nccsp.rect0)
				Marshal.StructureToPtr(nccsp, m.LParam, False)
				m.Result = IntPtr.Zero
				'm.Result = New IntPtr(&H300)

				'If (exstyle And Win32Api.WindowsStyles.WS_EX_TOPMOST) > 0 Then
				'	'NOTE: Remove the 1-pixel-wide border to make ClientRectangle.
				'	Me.theClientRectangle.X = nccsp.rect2.X
				'	Me.theClientRectangle.Y = nccsp.rect2.Y
				'	Me.theClientRectangle.Width = nccsp.rect2.Width
				'	Me.theClientRectangle.Height = nccsp.rect2.Height
				'Else
				'	'NOTE: Remove the 1-pixel-wide border to make ClientRectangle.
				'	Me.theClientRectangle.X = nccsp.rect2.X
				'	Me.theClientRectangle.Y = nccsp.rect2.Y
				'	Me.theClientRectangle.Width = nccsp.rect2.Width
				'	Me.theClientRectangle.Height = nccsp.rect2.Height
				'End If
				'------
				''If Not Me.theClientRectangleIsAlreadySet AndAlso nccsp.rect2.Width > 0 AndAlso nccsp.rect2.Height > 0 Then
				'If nccsp.rect2.Width > 0 AndAlso nccsp.rect2.Height > 0 Then
				'	'NOTE: Remove the 1-pixel-wide border to make ClientRectangle.
				'	Me.theClientRectangle.X = nccsp.rect2.X
				'	Me.theClientRectangle.Y = nccsp.rect2.Y
				'	Me.theClientRectangle.Width = nccsp.rect2.Width
				'	Me.theClientRectangle.Height = nccsp.rect2.Height

				'	Me.theClientRectangleIsAlreadySet = True
				'End If
			End If
		End Sub

		'NOTE: Windows still draws a 1-pixel-wide border, so draw over it AFTER calling MyBase.WndProc.
		Private Sub OnNonClientPaint(ByRef m As Message)
			Dim style As Integer = Win32Api.GetWindowLong(Me.Handle, Win32Api.GWL_STYLE)
			If (style And Win32Api.WindowsStyles.WS_VISIBLE) > 0 Then
				' Draw drop-down-list border.
				Dim hDC As IntPtr = Win32Api.GetWindowDC(Me.Handle)
				Try
					Using g As Graphics = Graphics.FromHdc(hDC)
						Using aPen As New Pen(WidgetHighSelectedBackColor)
							'Using aPen As New Pen(Color.Red)
							Dim aRect As RectangleF = g.VisibleClipBounds
							g.DrawRectangle(aPen, 0, 0, aRect.Width - 1, aRect.Height - 1)

							Me.theWindowRectangle = Rectangle.Round(aRect)
						End Using
					End Using
				Finally
					Win32Api.ReleaseDC(Me.Handle, hDC)
					'Me.theComboBoxEx.VerticalScrollbarPopup.BringToFront()
					'Me.theComboBoxEx.VerticalScrollbarPopup.Invalidate()
				End Try
				'Me.UpdateVerticalScrollbar()
				'If Me.theScrollbarIsShowingForFirstTime Then
				'	Dim hDC2 As IntPtr = Win32Api.GetWindowDC(Me.Handle)
				'	Try
				'		Using g As Graphics = Graphics.FromHdc(hDC2)
				'			Dim aRect As RectangleF = g.VisibleClipBounds
				'			g.FillRectangle(Brushes.Red, aRect.Width - 10, 10, aRect.Width - 1, aRect.Height - 1)
				'		End Using
				'	Finally
				'		Win32Api.ReleaseDC(Me.Handle, hDC2)
				'	End Try
				'	Me.theScrollbarIsShowingForFirstTime = False
				'End If
			End If

			m.Result = IntPtr.Zero
		End Sub

		' Is not called when first opened. Only called when list is scrolled.
		Private Sub OnPaint(ByRef m As Message)
			''NOTE: This is the only way I could get the popup in front of the dropdownlist.  [25-Feb-2024] Could not reproduce this with following code.
			'Win32Api.SetWindowPos(Me.Handle, Win32Api.HWND_NOTOPMOST, 0, 0, 0, 0, Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE)
			'Dim debug As Integer = 4242
			'Me.theComboBoxEx.VerticalScrollbarPopup.BringToFront()
			'Me.theComboBoxEx.VerticalScrollbarPopup.Invalidate()
		End Sub

		Private Sub ResizeClientRect(ByVal padding As Padding, ByRef rect As Win32Api.RECT)
			'NOTE: The rect INCLUDES the default 1-pixel-wide border.
			rect.Left += padding.Left
			rect.Top += padding.Top
			rect.Right -= padding.Right
			rect.Bottom -= padding.Bottom
		End Sub

#Region "Child Widget Event Handlers"

		'Private Sub VerticalScrollBar_ValueChanged(ByVal sender As Object, ByVal e As ScrollValueEventArgs) Handles VerticalScrollbar.ValueChanged
		'	Me.UpdateScrolling(0, e.Value)
		'End Sub

#End Region

#Region "Private Methods"

		'Private Sub UpdateScrolling(ByVal leftOrRightValue As Integer, ByVal upOrDownValue As Integer)
		'	If Not Me.theScrollingIsActive Then
		'		Me.theScrollingIsActive = True

		'		Dim scrollPosition As New Point(leftOrRightValue, upOrDownValue)
		'		Win32Api.RtfScroll(Me.Handle, Win32Api.WindowsMessages.EM_SETSCROLLPOS, IntPtr.Zero, scrollPosition)

		'		Me.theScrollingIsActive = False
		'	End If
		'End Sub

		'Private Sub UpdateVerticalScrollbar()
		'	If Not Me.theScrollingIsActive Then
		'		Dim contentHeight As Integer = Me.theComboBoxEx.Items.Count * Me.theComboBoxEx.ItemHeight
		'		If contentHeight > Me.theComboBoxEx.MaxDropDownItems * Me.theComboBoxEx.ItemHeight Then
		'			Me.theScrollingIsActive = True

		'			Me.VerticalScrollbar.Minimum = 0
		'			Me.VerticalScrollbar.Maximum = contentHeight
		'			Me.VerticalScrollbar.Value = Me.theComboBoxEx.SelectedIndex * Me.theComboBoxEx.ItemHeight
		'			Me.VerticalScrollbar.ViewSize = Me.theComboBoxEx.MaxDropDownItems * Me.theComboBoxEx.ItemHeight
		'			Me.VerticalScrollbar.SmallChange = Me.theComboBoxEx.ItemHeight
		'			Me.VerticalScrollbar.LargeChange = Me.theComboBoxEx.MaxDropDownItems * (Me.theComboBoxEx.ItemHeight - 2)

		'			'NOTE: The DropDownList uses "TOPMOST" style, so need to set scrollbar to be topmost.
		'			Win32Api.SetParent(Me.VerticalScrollbar.Handle, Me.Handle)
		'			'Win32Api.SetParent(Me.VerticalScrollbar.Handle, Me.theComboBoxEx.FindForm.Handle)
		'			Win32Api.SetWindowPos(Me.Handle, Win32Api.HWND_NOTOPMOST, 0, 0, 0, 0, Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE)
		'			Win32Api.SetWindowPos(Me.VerticalScrollbar.Handle, Win32Api.HWND_TOPMOST, 0, 0, 0, 0, Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE)
		'			Me.VerticalScrollbar.BringToFront()
		'			'NOTE: Location must be relative to Parent.
		'			Dim aPoint As New Point(Me.theComboBoxEx.DropDownWidth - ScrollBarEx.Consts.ScrollBarSize - 10, 0)
		'			aPoint = Me.theComboBoxEx.PointToScreen(aPoint)
		'			aPoint = Me.theComboBoxEx.FindForm.PointToClient(aPoint)
		'			Me.VerticalScrollbar.Location = aPoint
		'			Me.VerticalScrollbar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.WindowRectangle.Height)
		'			Me.VerticalScrollbar.Show()

		'			Me.theScrollingIsActive = False
		'		Else
		'			Me.VerticalScrollbar.Hide()
		'		End If
		'	End If
		'End Sub

#End Region

#Region "Data"

		Private theComboBoxEx As ComboBoxEx
		'NOTE: Could not find a way to get the true WindowRectangle that does not include the default scrollbar width; 
		'      thus, the rectangle is too small to be useful.
		Private theWindowRectangle As Rectangle
		'Private theClientRectangle As Rectangle
		Private thePadding As Padding

		'Private theClientRectangleIsAlreadySet As Boolean

		'Private WithEvents VerticalScrollbar As ScrollBarEx
		'Private theScrollingIsActive As Boolean
		'Private theScrollbarIsShowingForFirstTime As Boolean

		Private theNativeScrollbar As NativeScrollbar

#End Region

		Friend Class NativeScrollbar
			Inherits NativeWindow

			Public Sub New()
				MyBase.New()
			End Sub

			Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
				If m.Msg = Win32Api.WindowsMessages.WM_DESTROY OrElse m.Msg = Win32Api.WindowsMessages.WM_NCDESTROY Then
					Me.ReleaseHandle()
				ElseIf m.Msg = Win32Api.WindowsMessages.WM_WINDOWPOSCHANGING Then
					'Move the updown control off the edge so it's not visible
					Dim wp As Win32Api.WINDOWPOS = DirectCast(m.GetLParam(GetType(Win32Api.WINDOWPOS)), Win32Api.WINDOWPOS)
					wp.x += wp.cx
					Runtime.InteropServices.Marshal.StructureToPtr(wp, m.LParam, True)
					'_bounds = New Rectangle(wp.x, wp.y, wp.cx, wp.cy)
				End If

				MyBase.WndProc(m)
			End Sub

			'Private _bounds As Rectangle

		End Class

	End Class

End Class
