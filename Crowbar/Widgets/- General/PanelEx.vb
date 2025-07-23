Imports System.ComponentModel
Imports System.Runtime.InteropServices

Public Class PanelEx
	Inherits Panel

	'NOTE: DataBind a group of RadioButtons to a DataSource property that is an Integer or an Enum.
	'thisPanelEx.DataBindings.Add("SelectedIndex", theDataSourceThatHasTheIntegerProperty, "NameOfIntegerProperty", False, DataSourceUpdateMode.OnPropertyChanged)
	'thisPanelEx.DataBindings.Add("SelectedValue", theDataSourceThatHasTheEnumProperty, "NameOfEnumProperty", False, DataSourceUpdateMode.OnPropertyChanged)


#Region "Create and Destroy"

	Public Sub New()
		MyBase.New()

		'Me.ResizeRedraw = True

		'NOTE: Hide built-in scrollbars and use custom scrollbars.
		' AutoScroll = False allows changing Minimum, Maximum, and Visible properties of each built-in scrollbar. 
		' Scrollbars ignore changes to Value property, so use AutoScrollPosition instead.
		' AutoScrollPosition returns the negative of actual value.
		MyBase.AutoScroll = False
		'Me.HorizontalScroll.Enabled = False
		'Me.HorizontalScroll.Visible = False
		'Me.HorizontalScroll.Maximum = 0
		'Me.VerticalScroll.Enabled = False
		'Me.VerticalScroll.Visible = False
		'Me.VerticalScroll.Maximum = 0
		'MyBase.AutoScroll = True
		Me.theAutoScroll = False

		Me.theNonClientPaddingColor = WidgetDeepBackColor
		'TEST:
		'Me.theNonClientPaddingColor = Color.Pink
		Me.theScrollingIsActive = False

		Me.CustomHorizontalScrollbar = New ScrollBarEx()
		Me.Controls.Add(Me.CustomHorizontalScrollbar)
		'Me.CustomHorizontalScrollbar.Location = New System.Drawing.Point(0, Me.ClientRectangle.Height)
		Me.CustomHorizontalScrollbar.Name = "CustomHorizontalScrollbar"
		'Me.CustomHorizontalScrollbar.Size = New System.Drawing.Size(Me.Width, ScrollBarEx.Consts.ScrollBarSize)
		Me.CustomHorizontalScrollbar.ScrollOrientation = ScrollBarEx.DarkScrollOrientation.Horizontal
		Me.CustomHorizontalScrollbar.TabIndex = 7
		Me.CustomHorizontalScrollbar.Text = "CustomHorizontalScrollbar"
		Me.CustomHorizontalScrollbar.Visible = False

		Me.CustomVerticalScrollBar = New ScrollBarEx()
		Me.Controls.Add(Me.CustomVerticalScrollBar)
		'Me.CustomVerticalScrollBar.Location = New System.Drawing.Point(Me.ClientRectangle.Width, 0)
		Me.CustomVerticalScrollBar.Name = "CustomVerticalScrollBar"
		'Me.CustomVerticalScrollBar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.Height)
		Me.CustomVerticalScrollBar.ScrollOrientation = ScrollBarEx.DarkScrollOrientation.Vertical
		Me.CustomVerticalScrollBar.TabIndex = 7
		Me.CustomVerticalScrollBar.Text = "CustomVerticalScrollBar"
		Me.CustomVerticalScrollBar.Visible = False

		Me.theControlHasShown = False

		Me.theSelectedIndex = -1
		Me.ForeColor = WidgetTextColor
		Me.BackColor = WidgetBackColor
	End Sub

#End Region

#Region "Init and Free"

#End Region

#Region "Properties"

	<Browsable(True)>
	<Category("Layout")>
	<Description("Scrollbars appear when needed.")>
	Public Overloads Property AutoScroll As Boolean
		Get
			Return Me.theAutoScroll
		End Get
		Set
			MyBase.AutoScroll = False
			Me.theAutoScroll = Value
		End Set
	End Property

	'Public Property IsReadOnly() As Boolean
	'	Get
	'		Return Me.theControlIsReadOnly
	'	End Get
	'	Set(ByVal value As Boolean)
	'		If Me.theControlIsReadOnly <> value Then
	'			Me.theControlIsReadOnly = value

	'			If Me.theControlIsReadOnly Then
	'				Me.ForeColor = SystemColors.GrayText
	'			Else
	'				Me.ForeColor = SystemColors.ControlText
	'			End If
	'		End If
	'	End Set
	'End Property

	Public ReadOnly Property RadioButtons() As RadioButton()
		Get
			Return Me.theRadioButtonList.ToArray()
		End Get
	End Property

	Public Property SelectedIndex() As Integer
		Get
			Return Me.theSelectedIndex
		End Get
		Set(ByVal value As Integer)
			If value < 0 OrElse value >= Me.theRadioButtonList.Count Then
				Return
			End If
			If value <> Me.theSelectedIndex Then
				Dim radioButton As RadioButton
				radioButton = Me.theRadioButtonList(value)
				radioButton.Checked = True
				Me.SetIndex(value)
			End If
		End Set
	End Property

	Public Property SelectedValue() As System.Enum
		Get
			Return Me.theSelectedValue
		End Get
		Set(ByVal value As System.Enum)
			'NOTE: This test is needed because Visual Studio Designer sets the property to Nothing in InitializeComponent().
			If value Is Nothing Then
				Return
			End If
			If Not value.Equals(Me.theSelectedValue) Then
				Dim radioButton As RadioButton
				For i As Integer = 0 To Me.theRadioButtonList.Count - 1
					radioButton = Me.theRadioButtonList(i)
					If value.Equals(radioButton.Tag) Then
						radioButton.Checked = True
						Me.SetValue(CType(radioButton.Tag, System.Enum))
						Exit For
					End If
				Next
			End If
		End Set
	End Property

#End Region

#Region "Methods"

#End Region

#Region "Events"

	Public Event SelectedValueChanged As EventHandler
	Public Event SelectedIndexChanged As EventHandler

#End Region

#Region "Widget Event Handlers"

	Protected Overloads Overrides Sub OnControlAdded(ByVal e As ControlEventArgs)
		If TypeOf e.Control Is RadioButton Then
			Dim radioButton As RadioButton = CType(e.Control, RadioButton)
			Me.theRadioButtonList.Add(radioButton)
			AddHandler radioButton.CheckedChanged, AddressOf Me.RadioButton_CheckedChanged

			If Me.theRadioButtonList.Count = 1 Then
				Me.SelectedIndex = 0
				If radioButton.Tag IsNot Nothing Then
					Me.theSelectedValue = CType(radioButton.Tag, System.Enum)
				End If
			End If
		End If
		MyBase.OnControlAdded(e)
	End Sub

	Protected Overloads Overrides Sub OnControlRemoved(ByVal e As ControlEventArgs)
		If TypeOf e.Control Is RadioButton Then
			Dim radioButton As RadioButton = CType(e.Control, RadioButton)
			Me.theRadioButtonList.Remove(radioButton)
			RemoveHandler radioButton.CheckedChanged, AddressOf Me.RadioButton_CheckedChanged
		End If
		MyBase.OnControlRemoved(e)
	End Sub

	Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)
		MyBase.OnMouseWheel(e)

		If Me.CustomVerticalScrollBar.Visible Then
			Dim upOrDownValue As Integer = Me.CustomHorizontalScrollbar.SmallChange * 3
			If e.Delta > 0 Then
				' Moving wheel away from user = up.
				Me.CustomVerticalScrollBar.Value -= upOrDownValue
			Else
				' Moving wheel toward user = down.
				Me.CustomVerticalScrollBar.Value += upOrDownValue
			End If
		End If
	End Sub

	'Protected Overrides Sub OnPaint(e As PaintEventArgs)
	'	e.Graphics.TranslateTransform(-Me.CustomHorizontalScrollbar.Value, -Me.CustomVerticalScrollBar.Value)
	'	MyBase.OnPaint(e)
	'End Sub

	'Protected Overrides Sub OnScroll(e As ScrollEventArgs)
	'	MyBase.OnScroll(e)
	'	Me.Invalidate()
	'	Me.UpdateScrollbars()
	'End Sub

	Protected Overridable Sub OnSelectedIndexChanged(ByVal e As EventArgs)
		RaiseEvent SelectedIndexChanged(Me, e)
	End Sub

	Protected Overridable Sub OnSelectedValueChanged(ByVal e As EventArgs)
		RaiseEvent SelectedValueChanged(Me, e)
	End Sub

	Protected Overrides Sub OnSizeChanged(e As EventArgs)
		'NOTE: Need this "If" to prevent unneeded resizing and painting when scrolling.
		If Not Me.theScrollingIsActive Then
			MyBase.OnSizeChanged(e)

			'TODO: Find better way because the following 3 lines update the interface properly, but UpdateNonClientPadding() is called 2 or 3 times, and UpdateVerticalScrollbar() is called 1 or 2 times.
			'NOTE: Force calling UpdateNonClientPadding() here so that the correct clientHeight is used for scrollbars.
			'NOTE: Raise the OnNonClientCalcSize and OnNonClientPaint "events".
			Win32Api.SetWindowPos(Me.Handle, IntPtr.Zero, 0, 0, 0, 0, Win32Api.SWP.SWP_FRAMECHANGED Or Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE Or Win32Api.SWP.SWP_NOZORDER)
			Me.UpdateScrollbars()
			Me.Refresh()
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
			Me.Invalidate()
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
	End Sub

	Private Sub OnNonClientPaint(ByRef m As Message)
		Dim hDC As IntPtr = Win32Api.GetWindowDC(Me.Handle)
		Try
			Using g As Graphics = Graphics.FromHdc(hDC)
				Using backColorBrush As New SolidBrush(Me.theNonClientPaddingColor)
					Dim rect As Rectangle = Me.ClientRectangle
					rect.Offset(Me.NonClientPadding.Left, Me.NonClientPadding.Top)
					g.ExcludeClip(rect)
					Dim aRect As RectangleF = g.VisibleClipBounds
					g.FillRectangle(backColorBrush, aRect)
				End Using
			End Using
		Finally
			Win32Api.ReleaseDC(Me.Handle, hDC)
		End Try
		m.Result = IntPtr.Zero
	End Sub

#End Region

#Region "Child Widget Event Handlers"

	Private Sub RadioButton_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
		Dim radioButton As RadioButton = CType(sender, RadioButton)
		If radioButton.Checked Then
			Me.SetIndex(Me.theRadioButtonList.IndexOf(radioButton))
			Me.SetValue(CType(radioButton.Tag, System.Enum))
		End If
	End Sub

	Private Sub HorizontalScrollbar_ValueChanged(ByVal sender As Object, ByVal e As ScrollValueEventArgs) Handles CustomHorizontalScrollbar.ValueChanged
		Me.UpdateScrolling(e.Value, 0)
	End Sub

	Private Sub VerticalScrollBar_ValueChanged(ByVal sender As Object, ByVal e As ScrollValueEventArgs) Handles CustomVerticalScrollBar.ValueChanged
		Me.UpdateScrolling(0, e.Value)
	End Sub

#End Region

#Region "Private Methods"

	Private Sub UpdateNonClientPadding()
		If Me.DesignMode Then
			Exit Sub
		End If

		Dim left As Integer = 0
		Dim top As Integer = 0
		Dim right As Integer = 0
		Dim bottom As Integer = 0
		''TEST: Use 2 for testing. Use 0 for final.
		'Dim left As Integer = 2
		'Dim top As Integer = 2
		'Dim right As Integer = 2
		'Dim bottom As Integer = 2

		'Dim contentWidth As Integer = Me.PreferredSize.Width - ScrollBarEx.Consts.ScrollBarSize - Me.Margin.Left
		Dim contentWidth As Integer = Me.PreferredSize.Width
		Dim clientWidth As Integer = Me.ClientRectangle.Width
		'Dim contentHeight As Integer = Me.PreferredSize.Height - ScrollBarEx.Consts.ScrollBarSize - Me.Margin.Top
		Dim contentHeight As Integer = Me.PreferredSize.Height
		Dim clientHeight As Integer = Me.ClientRectangle.Height

		If contentHeight > clientHeight AndAlso Me.theAutoScroll Then
			right += ScrollBarEx.Consts.ScrollBarSize
			clientWidth -= ScrollBarEx.Consts.ScrollBarSize
		End If
		If contentWidth > clientWidth AndAlso Me.theAutoScroll Then
			'bottom += ScrollBarEx.Consts.ScrollBarSize
		End If

		Me.NonClientPadding = New Padding(left, top, right, bottom)
	End Sub

	Private Sub ResizeClientRect(ByVal padding As Padding, ByRef rect As Win32Api.RECT)
		rect.Left += padding.Left
		rect.Top += padding.Top
		rect.Right -= padding.Right
		rect.Bottom -= padding.Bottom
	End Sub

	Private Sub UpdateScrolling(ByVal leftOrRightValue As Integer, ByVal upOrDownValue As Integer)
		If Not Me.theScrollingIsActive Then
			Me.theScrollingIsActive = True

			'SetDisplayRectLocation(0, AutoScrollPosition.Y + upOrDownValue)
			'Me.Invalidate()
			'------
			'Dim scrollPosition As New Point(leftOrRightValue, upOrDownValue)
			'Win32Api.RtfScroll(Me.Handle, Win32Api.WindowsMessages.EM_SETSCROLLPOS, IntPtr.Zero, scrollPosition)
			'------
			'Me.CustomHorizontalScrollbar.Value += leftOrRightValue
			'Me.CustomVerticalScrollBar.Value += upOrDownValue
			'Me.HorizontalScroll.Value = leftOrRightValue
			If upOrDownValue <= Me.CustomVerticalScrollBar.Minimum OrElse upOrDownValue > Me.CustomVerticalScrollBar.Maximum Then
				Me.AutoScrollPosition = New Point(leftOrRightValue, upOrDownValue)
			Else
				Me.VerticalScroll.Value = upOrDownValue
			End If
			'Me.Invalidate()
			'Me.Invalidate(True)
			Me.Refresh()

			Me.theScrollingIsActive = False
		End If
	End Sub

	Private Sub UpdateScrollbars()
		If Me.DesignMode Then
			Exit Sub
		End If

		Me.UpdateHorizontalScrollbar()
		Me.UpdateVerticalScrollbar()

		If Me.CustomHorizontalScrollbar.Visible AndAlso Me.CustomVerticalScrollBar.Visible Then
			'Me.CustomHorizontalScrollbar.Size = New System.Drawing.Size(Me.Width - ScrollBarEx.Consts.ScrollBarSize, ScrollBarEx.Consts.ScrollBarSize)
			'Me.CustomVerticalScrollBar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.Height - ScrollBarEx.Consts.ScrollBarSize)
			'Me.CustomHorizontalScrollbar.Size = New System.Drawing.Size(Me.ClientRectangle.Width - ScrollBarEx.Consts.ScrollBarSize, ScrollBarEx.Consts.ScrollBarSize)
			Me.CustomVerticalScrollBar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.ClientRectangle.Height - ScrollBarEx.Consts.ScrollBarSize)
		End If
	End Sub

	Private Sub UpdateHorizontalScrollbar()
		'NOTE: Parent can be Nothing on exiting. Prevent the exception with this check.
		If Not Me.theScrollingIsActive AndAlso Me.Parent IsNot Nothing AndAlso Me.theAutoScroll Then
			'Dim contentWidth As Integer = Me.PreferredSize.Width - ScrollBarEx.Consts.ScrollBarSize - Me.Margin.Left
			Dim contentWidth As Integer = Me.PreferredSize.Width
			Dim clientWidth As Integer = Me.ClientRectangle.Width
			Dim contentHeight As Integer = Me.PreferredSize.Height
			Dim clientHeight As Integer = Me.ClientRectangle.Height
			If contentHeight > clientHeight AndAlso Me.theAutoScroll Then
				'clientWidth -= ScrollBarEx.Consts.ScrollBarSize
			End If
			If contentWidth > clientWidth Then
				Me.theScrollingIsActive = True

				Me.CustomHorizontalScrollbar.Minimum = 0
				Me.CustomHorizontalScrollbar.Maximum = contentWidth
				Me.CustomHorizontalScrollbar.Value = -Me.AutoScrollPosition.X
				Me.CustomHorizontalScrollbar.ViewSize = clientWidth
				Me.CustomHorizontalScrollbar.SmallChange = 5
				Me.CustomHorizontalScrollbar.LargeChange = clientWidth - 5 * 2

				'NOTE: Assign to Parent so it can draw over non-client area of RichTextBoxEx.
				Me.CustomHorizontalScrollbar.Parent = Me.Parent
				Me.CustomHorizontalScrollbar.BringToFront()
				'NOTE: Point is relative to Me.ClientRectangle.
				Dim aPoint As New Point(Me.ClientRectangle.Left - Me.NonClientPadding.Left, Me.ClientRectangle.Height + Me.NonClientPadding.Bottom - ScrollBarEx.Consts.ScrollBarSize)
				'NOTE: Location must be relative to Parent.
				aPoint = Me.PointToScreen(aPoint)
				aPoint = Me.Parent.PointToClient(aPoint)
				Me.CustomHorizontalScrollbar.Location = aPoint
				'Me.CustomHorizontalScrollbar.Size = New System.Drawing.Size(Me.Width, ScrollBarEx.Consts.ScrollBarSize)
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
		'NOTE: Parent can be Nothing on exiting. Prevent the exception with this check.
		If Not Me.theScrollingIsActive AndAlso Me.Parent IsNot Nothing AndAlso Me.theAutoScroll Then
			'Dim contentHeight As Integer = Me.PreferredSize.Height - ScrollBarEx.Consts.ScrollBarSize - Me.Margin.Top
			Dim contentHeight As Integer = Me.PreferredSize.Height
			Dim clientHeight As Integer = Me.ClientRectangle.Height
			If contentHeight > clientHeight Then
				Me.theScrollingIsActive = True

				Me.CustomVerticalScrollBar.Minimum = 0
				Me.CustomVerticalScrollBar.Maximum = contentHeight
				Me.CustomVerticalScrollBar.Value = -Me.AutoScrollPosition.Y
				Me.CustomVerticalScrollBar.ViewSize = clientHeight
				Me.CustomVerticalScrollBar.SmallChange = 5
				Me.CustomVerticalScrollBar.LargeChange = clientHeight - 5 * 2

				Me.VerticalScroll.Minimum = 0
				Me.VerticalScroll.Maximum = contentHeight
				Me.VerticalScroll.SmallChange = 5
				Me.VerticalScroll.LargeChange = clientHeight - 5 * 2

				'NOTE: Assign to Parent so it can draw over non-client area.
				Me.CustomVerticalScrollBar.Parent = Me.Parent
				Me.CustomVerticalScrollBar.BringToFront()
				'NOTE: Point is relative to Me.ClientRectangle.
				Dim aPoint As New Point(Me.ClientRectangle.Width + Me.NonClientPadding.Right - ScrollBarEx.Consts.ScrollBarSize, Me.ClientRectangle.Top - Me.NonClientPadding.Top)
				'NOTE: Location must be relative to Parent.
				aPoint = Me.PointToScreen(aPoint)
				aPoint = Me.CustomVerticalScrollBar.Parent.PointToClient(aPoint)
				Me.CustomVerticalScrollBar.Location = aPoint
				'Me.CustomVerticalScrollBar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.Height)
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

	Private Sub SetIndex(ByVal index As Integer)
		Me.theSelectedIndex = index
		Me.OnSelectedIndexChanged(New EventArgs())
	End Sub

	Private Sub SetValue(ByVal enumValue As System.Enum)
		Me.theSelectedValue = enumValue
		Me.OnSelectedValueChanged(New EventArgs())
	End Sub

#End Region

#Region "Data"

	'Protected theControlIsReadOnly As Boolean
	Private theRadioButtonList As New System.Collections.Generic.List(Of RadioButton)()
	Private theSelectedIndex As Integer
	Private theSelectedValue As System.Enum

	Private NonClientPadding As Padding
	Private theNonClientPaddingColor As Color
	Private theAutoScroll As Boolean
	Private WithEvents CustomHorizontalScrollbar As ScrollBarEx
	Private WithEvents CustomVerticalScrollBar As ScrollBarEx
	Private theControlHasShown As Boolean
	Private theScrollingIsActive As Boolean

#End Region

End Class
