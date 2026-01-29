Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Windows.Forms.VisualStyles

Public Class ComboUserControl

#Region "Creation and Destruction"

	Public Sub New()

		' This call is required by the designer.
		InitializeComponent()

		'NOTE: Disable to use custom.
		MyBase.BorderStyle = BorderStyle.None

		Me.theBorderStyle = BorderStyle.FixedSingle
		Me.theControlIsReadOnly = False
		Me.theComboPanelBorderColor = Color.Red
		Me.CreateContextMenu()

		' IMPORTANT: Need to assign the BackColor here so that a later assignment covers the entire TextBox.
		'            Without this first assignment, the later assignment to SystemColors.Control does not cover the top two rows of pixels.
		Me.ComboTextBox.BackColor = SystemColors.Control

		Me.theMultipleInputsIsAllowed = True
		Me.MultipleInputsDropDownButton.Visible = Me.theMultipleInputsIsAllowed
		Me.MultipleInputsDropDownButton.Enabled = False
		'Me.MultipleInputsDropDownButton.BackColor = WidgetConstants.WidgetDeepBackColor
		'Me.MultipleInputsDataGridView.BackColor = WidgetConstants.WidgetBackColor

		Me.theMaxDropDownItemCount = 30
		Me.theTextHistoryIsKept = False
		Me.theTextHistoryMaxSize = 15
		Me.theTextIsValidatingViaMe = False
		Me.theSelectedIndexIsChangingViaMe = False
		Me.theTextIsPathFileNames = False
		Me.TextHistoryDropDownButton.SpecialImage = ButtonEx.SpecialImageType.DownArrow
		'Me.TextHistoryDropDownButton.BackColor = WidgetConstants.WidgetDeepBackColor
		'Me.TextHistoryDataGridView.BackColor = WidgetConstants.WidgetBackColor
		Me.TextHistoryDataGridView.AutoGenerateColumns = False

		Me.thePointerIsOverTheDropDownButton = False
	End Sub

#End Region

#Region "Init and Free"

#End Region

#Region "Properties"

	<Browsable(True)>
	<Category("Appearance")>
	Public Overloads Property BorderColor As Color
		Get
			Return Me.theBorderColor
		End Get
		Set
			Me.theBorderColor = Value
		End Set
	End Property

	<Browsable(True)>
	<Category("Appearance")>
	<Description("Colorable BorderStyle.")>
	Public Overloads Property BorderStyle As BorderStyle
		Get
			Return Me.theBorderStyle
		End Get
		Set
			Me.theBorderStyle = Value
		End Set
	End Property

	<Browsable(False)>
	Public Property DataSource() As Object
		Get
			Return Me.TextHistoryDataGridView.DataSource
		End Get
		Set
			Me.TextHistoryDataGridView.DataSource = Value
		End Set
	End Property

	'<Browsable(False)>
	'Public Property DisplayMember() As String
	'	Get
	'		Return Me.TextHistoryDataGridView.Columns(Me.TextHistoryDataGridView.FirstDisplayedScrollingColumnIndex).Name
	'	End Get
	'	Set
	'		For Each column As DataGridViewColumn In Me.TextHistoryDataGridView.Columns
	'			If column.Name = Value Then
	'				column.Visible = True
	'			Else
	'				column.Visible = False
	'			End If
	'		Next
	'	End Set
	'End Property

	<Browsable(False)>
	Public Property ValueMember() As String
		Get
			' This check prevents problems with viewing and saving Forms in VS Designer.
			Dim columnName As String = ""
			If Me.TextHistoryDataGridView.Columns.Count > 0 Then
				columnName = Me.TextHistoryDataGridView.Columns(0).Name
			End If
			Return columnName
		End Get
		Set
			Me.TextHistoryDataGridView.Columns.Clear()
			Dim column As DataGridViewColumn = New DataGridViewTextBoxColumn()
			column.DataPropertyName = Value
			column.Name = Value
			column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
			Me.TextHistoryDataGridView.Columns.Add(column)
		End Set
	End Property

	'<Browsable(False)>
	'Public Overloads ReadOnly Property DataBindings() As ControlBindingsCollection
	'	Get
	'		'Me.ComboTextBox.DataBindings.Add("Text", Me.TextHistoryDataGridView.DataSource, Me.TextHistoryDataGridView.DisplayMember, False, DataSourceUpdateMode.OnPropertyChanged)
	'		'Me.ComboTextBox.Text = Me.TextHistoryDataGridView.GetItemText(Me.TextHistoryDataGridView.SelectedItem)
	'		Return MyBase.DataBindings
	'	End Get
	'End Property

	<Browsable(False)>
	Public Property SelectedIndex() As Integer
		Get
			' This check prevents problems with viewing and saving Forms in VS Designer.
			Dim rowIndex As Integer = -1
			If Me.TextHistoryDataGridView.SelectedRows.Count > 0 Then
				rowIndex = Me.TextHistoryDataGridView.SelectedRows(0).Index
			End If
			Return rowIndex
		End Get
		Set
			If Me.TextHistoryDataGridView.Rows.Count > 0 Then
				If Me.TextHistoryDataGridView.SelectedRows.Count > 0 Then
					Me.TextHistoryDataGridView.SelectedRows(0).Selected = False
				End If
				Me.TextHistoryDataGridView.Rows(Value).Selected = True
				'Me.ComboTextBox.Text = CStr(Me.TextHistoryDataGridView.Rows(Value).Cells(0).Value)
				'RaiseEvent SelectedIndexChanged(Me, New EventArgs())
				'RaiseEvent SelectedValueChanged(Me, New EventArgs())
				Me.UpdateTextBoxWithSelectedHistoryText()
			End If
		End Set
	End Property

	<Browsable(False)>
	Public Property SelectedValue() As String
		Get
			' This check prevents problems with viewing and saving Forms in VS Designer.
			Dim aSelectedValue As String = ""
			If Me.TextHistoryDataGridView.SelectedRows.Count > 0 Then
				aSelectedValue = CStr(Me.TextHistoryDataGridView.SelectedRows(0).Cells(0).Value)
			End If
			Return aSelectedValue
		End Get
		Set
			If Me.TextHistoryDataGridView.Rows.Count > 0 Then
				If Me.TextHistoryDataGridView.SelectedRows.Count > 0 Then
					Me.TextHistoryDataGridView.SelectedRows(0).Selected = False
				End If
				For Each row As DataGridViewRow In Me.TextHistoryDataGridView.Rows
					Dim text As String = CStr(row.Cells(0).Value)
					If Value = text Then
						row.Selected = True
						'Me.ComboTextBox.Text = Value
						'RaiseEvent SelectedIndexChanged(Me, New EventArgs())
						'RaiseEvent SelectedValueChanged(Me, New EventArgs())
						Me.UpdateTextBoxWithSelectedHistoryText()
						Exit For
					End If
				Next
			End If
		End Set
	End Property

	Public Property MaxDropDownItems() As Integer
		Get
			Return Me.theMaxDropDownItemCount
		End Get
		Set
			Me.theMaxDropDownItemCount = Value
		End Set
	End Property

	Public Property IsReadOnly() As Boolean
		Get
			Return Me.theControlIsReadOnly
		End Get
		Set(ByVal value As Boolean)
			If Me.theControlIsReadOnly <> value Then
				Me.theControlIsReadOnly = value

				''TODO: Somehow disable value selection (i.e. no dropdown)
				''Me.Enabled = Not Me.theControlIsReadOnly
				'If Me.theControlIsReadOnly Then
				'	Me.ForeColor = SystemColors.ControlText
				'	Me.BackColor = SystemColors.Control
				'Else
				'	Me.ForeColor = SystemColors.ControlText
				'	Me.BackColor = SystemColors.Window
				'End If

				Me.ComboTextBox.ReadOnly = Me.theControlIsReadOnly
				If Me.ComboTextBox.ReadOnly Then
					Me.ComboTextBox.Cursor = Cursors.Arrow
				Else
					Me.ComboTextBox.Cursor = Cursors.IBeam
				End If
			End If
		End Set
	End Property

	Public Overrides Property Text() As String
		Get
			Return Me.ComboTextBox.Text
		End Get
		Set
			Me.ComboTextBox.Text = Value
			Me.UpdateMultipleInputsWidgets()
		End Set
	End Property

	Public Property MultipleInputsIsAllowed() As Boolean
		Get
			Return Me.theMultipleInputsIsAllowed
		End Get
		Set(ByVal value As Boolean)
			If Me.theMultipleInputsIsAllowed <> value Then
				Me.theMultipleInputsIsAllowed = value
				Me.MultipleInputsDropDownButton.Visible = Me.theMultipleInputsIsAllowed

				If Me.theMultipleInputsIsAllowed Then
					Me.ComboTextBox.Width = Me.MultipleInputsDropDownButton.Left - Me.ComboTextBox.Left - Me.ComboTextBox.Margin.Right
				Else
					Me.ComboTextBox.Width = Me.TextHistoryDropDownButton.Left - Me.ComboTextBox.Left - Me.ComboTextBox.Margin.Right
				End If
			End If
		End Set
	End Property

	Public Property TextHistoryIsKept() As Boolean
		Get
			Return Me.theTextHistoryIsKept
		End Get
		Set(ByVal value As Boolean)
			If Me.theTextHistoryIsKept <> value Then
				Me.theTextHistoryIsKept = value

				If Me.theTextHistoryIsKept Then
					Me.CustomMenu.Items.Add(Me.Separator2ToolStripSeparator)
					Me.CustomMenu.Items.Add(Me.ClearTextHistoryToolStripMenuItem)
				Else
					Me.CustomMenu.Items.Remove(Me.Separator2ToolStripSeparator)
					Me.CustomMenu.Items.Remove(Me.ClearTextHistoryToolStripMenuItem)
				End If
			End If
		End Set
	End Property

	'Public Property TextHistory() As BindingListEx(Of String)
	'	Get
	'		'Return Me.theTextHistory
	'		Return CType(Me.TextHistoryDataGridView.DataSource, BindingListEx(Of String))
	'	End Get
	'	Set(ByVal value As BindingListEx(Of String))
	'		Me.theSelectedIndexIsChangingViaMe = True
	'		If value Is Nothing Then
	'			Me.TextHistoryDataGridView.DataSource = New BindingListEx(Of String)
	'		Else
	'			Me.TextHistoryDataGridView.DataSource = value
	'		End If
	'		Me.theSelectedIndexIsChangingViaMe = False
	'	End Set
	'End Property

	Public Property TextHistoryMaxSize() As Integer
		Get
			Return Me.theTextHistoryMaxSize
		End Get
		Set(ByVal value As Integer)
			If Me.theTextHistoryMaxSize <> value Then
				Me.theTextHistoryMaxSize = value

				If Me.TextHistoryDataGridView.DataSource Is Nothing Then
					Me.TextHistoryDataGridView.DataSource = New BindingListEx(Of String)
				Else
					Dim textHistory As BindingListEx(Of String) = CType(Me.TextHistoryDataGridView.DataSource, BindingListEx(Of String))
					For i As Integer = textHistory.Count - 1 To TextHistoryMaxSize Step -1
						textHistory.RemoveAt(i)
					Next
				End If
			End If
		End Set
	End Property

	Public Property TextIsPathFileNames() As Boolean
		Get
			Return Me.theTextIsPathFileNames
		End Get
		Set(ByVal value As Boolean)
			If Me.theTextIsPathFileNames <> value Then
				Me.theTextIsPathFileNames = value
			End If
		End Set
	End Property

#End Region

#Region "Events and Delegates"

	Public Event SelectedIndexChanged As EventHandler
	Public Event SelectedValueChanged As EventHandler

#End Region

#Region "Widget Event Handlers"

	Protected Overrides Sub OnHandleCreated(e As EventArgs)
		MyBase.OnHandleCreated(e)

		'If Me.theOriginalFont Is Nothing Then
		'    Me.Font = New Font(SystemFonts.MessageBoxFont.Name, 8.25)
		'    'NOTE: Font gets changed at some point after changing style, messing up when cue banner is turned off, 
		'    '      so save the Font before changing style.
		'    Me.theOriginalFont = New System.Drawing.Font(Me.Font.FontFamily, Me.Font.Size, Me.Font.Style, Me.Font.Unit)

		'    SetStyle(ControlStyles.AllPaintingInWmPaint, True)
		'    SetStyle(ControlStyles.DoubleBuffer, True)
		'    SetStyle(ControlStyles.UserPaint, True)
		'End If

		Me.InitMultipleInputsPopop()
		Me.InitTextHistoryPopop()
	End Sub

	Protected Overrides Sub OnPaint(e As PaintEventArgs)
		MyBase.OnPaint(e)

		Dim theme As ComboUserControlTheme = Nothing
		' This check prevents problems with viewing and saving Forms in VS Designer.
		If TheApp IsNot Nothing Then
			theme = TheApp.Settings.SelectedAppTheme.ComboUserControlTheme
		End If
		If theme IsNot Nothing Then
			If Me.Enabled Then
				Me.theBorderColor = theme.EnabledBorderColor
			Else
				Me.theBorderColor = theme.DisabledBorderColor
			End If
		Else
			Me.ComboTextBox.BackColor = SystemColors.Control
		End If

		'Dim g As Graphics = e.Graphics
		'Dim clientRectangle As Rectangle = Me.ClientRectangle
		'' Draw border.
		'If Me.theBorderStyle = BorderStyle.FixedSingle Then
		'	'Using borderColorPen As New Pen(WidgetDisabledTextColor)
		'	Using borderColorPen As New Pen(Me.theBorderColor)
		'		'NOTE: DrawRectangle width and height are interpreted as the right and bottom pixels to draw.
		'		g.DrawRectangle(borderColorPen, clientRectangle.Left, clientRectangle.Top, clientRectangle.Width - 1, clientRectangle.Height - 1)
		'	End Using
		'End If
	End Sub

	'Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
	'	'NOTE: Completely override painting by OS.
	'	'MyBase.OnPaintBackground(e)

	'	' Draw background.
	'	Using backColorBrush As New SolidBrush(Me.theComboPanelBorderColor)
	'		'Using backColorBrush As New SolidBrush(Color.Red)
	'		Dim aRect As Rectangle = Me.ClientRectangle
	'		arect.Inflate(1, 1)
	'		e.Graphics.FillRectangle(backColorBrush, aRect)
	'	End Using
	'End Sub

	Private Sub ComboUserControl_Resize(sender As Object, e As EventArgs) Handles Me.Resize
		If Me.theMultipleInputsIsAllowed Then
			Workarounds.WorkaroundForFrameworkAnchorRightSizingBug(Me.ComboTextBox, Me.MultipleInputsDropDownButton)
		Else
			Workarounds.WorkaroundForFrameworkAnchorRightSizingBug(Me.ComboTextBox, Me.TextHistoryDropDownButton)
		End If
		' Adjust by one pixel to hide the textbox right border.
		Me.ComboTextBox.Width += 1
	End Sub

	Protected Overrides Sub WndProc(ByRef m As Message)
		Select Case m.Msg
			Case Win32Api.WindowsMessages.WM_NCCALCSIZE
				Me.OnNonClientCalcSize(m)
			Case Win32Api.WindowsMessages.WM_NCPAINT
				Me.OnNonClientPaint(m)
		End Select

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
		Dim theme As ComboUserControlTheme = Nothing
		' This check prevents problems with viewing and saving Forms in VS Designer.
		If TheApp IsNot Nothing Then
			theme = TheApp.Settings.SelectedAppTheme.ComboUserControlTheme
		End If
		If theme IsNot Nothing Then
			If Me.Enabled Then
				Me.theBorderColor = theme.EnabledBorderColor
			Else
				Me.theBorderColor = theme.DisabledBorderColor
			End If

			Dim hDC As IntPtr = Win32Api.GetWindowDC(Me.Handle)
			Try
				Using g As Graphics = Graphics.FromHdc(hDC)
					Dim aRect As RectangleF = g.VisibleClipBounds
					' Draw border.
					If Me.theBorderStyle = BorderStyle.FixedSingle Then
						Using borderColorPen As New Pen(Me.theBorderColor)
							'NOTE: DrawRectangle width and height are interpreted as the right and bottom pixels to draw.
							g.DrawRectangle(borderColorPen, aRect.Left, aRect.Top, aRect.Width - 1, aRect.Height - 1)
						End Using
					End If
				End Using
			Finally
				Win32Api.ReleaseDC(Me.Handle, hDC)
			End Try
			m.Result = IntPtr.Zero
		Else
			Dim hDC As IntPtr = Win32Api.GetWindowDC(Me.Handle)
			Try
				Using g As Graphics = Graphics.FromHdc(hDC)
					Dim aRect As Rectangle = Rectangle.Truncate(g.VisibleClipBounds)
					If VisualStyleRenderer.IsSupported Then
						'ButtonRenderer.DrawParentBackground(g, aRect, Me)
						'' Inflate because Button border is deflated by 1 pixel.
						'aRect.Inflate(1, 1)
						'ButtonRenderer.DrawButton(g, aRect, PushButtonState.Normal)
						'ComboBoxRenderer.DrawTextBox(g, aRect, "", Me.Font, ComboBoxState.Normal)
						'------
						' This draws a border closest to what it looks like on Win11 for Crowbar 0.74.
						ComboBoxRenderer.DrawDropDownButton(g, aRect, ComboBoxState.Normal)
					Else
						g.Clear(SystemColors.ControlLight)
					End If
				End Using
			Finally
				Win32Api.ReleaseDC(Me.Handle, hDC)
			End Try
			m.Result = IntPtr.Zero
		End If
	End Sub

#End Region

#Region "Child Widget Event Handlers"

	'Private Sub ComboPanel_Paint(sender As Object, e As PaintEventArgs) Handles ComboBackgroundPanel.Paint
	'	Using backgroundColorBrush As New SolidBrush(ComboBackgroundPanel.BackColor)
	'		e.Graphics.FillRectangle(backgroundColorBrush, ComboBackgroundPanel.DisplayRectangle)
	'	End Using

	'	Dim rect As Rectangle = ComboBackgroundPanel.DisplayRectangle
	'	rect.Inflate(1, 1)
	'	'e.Graphics.ResetClip()
	'	ControlPaint.DrawBorder(e.Graphics, rect, Me.theComboPanelBorderColor, ButtonBorderStyle.Solid)
	'End Sub

	Private Sub CustomMenu_Opening(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomMenu.Opening
		Me.UndoToolStripMenuItem.Enabled = Not Me.IsReadOnly AndAlso Me.ComboTextBox.CanUndo
		'Me.RedoToolStripMenuItem.Enabled = Not Me.IsReadOnly AndAlso Me.InternalTextBox.CanRedo
		Me.CutToolStripMenuItem.Enabled = Not Me.IsReadOnly AndAlso Me.ComboTextBox.SelectionLength > 0
		Me.CopyToolStripMenuItem.Enabled = Me.ComboTextBox.SelectionLength > 0
		Me.PasteToolStripMenuItem.Enabled = Not Me.IsReadOnly AndAlso Clipboard.ContainsText()
		Me.DeleteToolStripMenuItem.Enabled = Not Me.IsReadOnly AndAlso Me.ComboTextBox.SelectionLength > 0
		Me.SelectAllToolStripMenuItem.Enabled = Me.Text.Length > 0 AndAlso Me.ComboTextBox.SelectionLength < Me.Text.Length
		Me.CopyAllToolStripMenuItem.Enabled = Me.Text.Length > 0 AndAlso Me.ComboTextBox.SelectionLength < Me.Text.Length
	End Sub

	Private Sub UndoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UndoToolStripMenuItem.Click
		Me.ComboTextBox.Undo()
	End Sub

	'Private Sub RedoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RedoToolStripMenuItem.Click
	'	Me.ComboTextBox.Redo()
	'End Sub

	Private Sub CutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutToolStripMenuItem.Click
		Me.ComboTextBox.Cut()
	End Sub

	Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
		Me.ComboTextBox.Copy()
	End Sub

	Private Sub PasteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteToolStripMenuItem.Click
		Me.ComboTextBox.Paste()
	End Sub

	Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click
		'NOTE: Must using P/Invoke EM_REPLACESEL to allow an Undo of Delete as is done with default TextBox.
		'      https://docs.microsoft.com/en-us/windows/win32/controls/em-replacesel?redirectedfrom=MSDN
		Win32Api.SendMessage(Me.ComboTextBox.Handle, Win32Api.EditControlMessage.EM_REPLACESEL, New IntPtr(1), IntPtr.Zero)
	End Sub

	Private Sub SelectAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click
		Me.ComboTextBox.SelectAll()
	End Sub

	Private Sub CopyAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyAllToolStripMenuItem.Click
		Me.ComboTextBox.SelectAll()
		Me.ComboTextBox.Copy()
	End Sub

	Private Sub ClearTextHistoryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearTextHistoryToolStripMenuItem.Click
		Me.ClearTextHistory()
	End Sub

	Private Sub ComboTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles ComboTextBox.KeyDown
		If e.KeyCode = Keys.Tab OrElse e.KeyCode = Keys.Enter OrElse e.KeyCode = Keys.Return Then
			Me.Validate()
		End If
	End Sub

	Private Sub ComboTextBox_MouseEnter(sender As Object, e As EventArgs) Handles ComboTextBox.MouseEnter
		Me.Highlight()
		Me.TextHistoryDropDownButton.Highlight()
	End Sub

	Private Sub ComboTextBox_MouseLeave(sender As Object, e As EventArgs) Handles ComboTextBox.MouseLeave
		Me.Diminish()
		Me.TextHistoryDropDownButton.Diminish()
	End Sub

	Private Sub ComboTextBox_MouseWheel(sender As Object, e As MouseEventArgs) Handles ComboTextBox.MouseWheel
		Dim debug As Integer = 4242
		Dim selectedRow As DataGridViewRow = Me.TextHistoryDataGridView.SelectedRows(0)
		Dim selectedRowIndex As Integer = Me.TextHistoryDataGridView.SelectedRows(0).Index
		If e.Delta > 0 AndAlso selectedRowIndex > 0 Then
			' Moving wheel away from user = up.
			'Me.TextHistoryDataGridView.SelectedRows(0).Index -= 1
			selectedRow.Selected = False
			Me.TextHistoryDataGridView.Rows(selectedRowIndex - 1).Selected = True
			Me.UpdateTextBoxWithSelectedHistoryText()
		ElseIf e.Delta < 0 AndAlso selectedRowIndex < Me.TextHistoryDataGridView.Rows.Count - 1 Then
			' Moving wheel toward user = down.
			'Me.TextHistoryDataGridView.SelectedRows(0).Index += 1
			selectedRow.Selected = False
			Me.TextHistoryDataGridView.Rows(selectedRowIndex + 1).Selected = True
			Me.UpdateTextBoxWithSelectedHistoryText()
		End If
	End Sub

	'NOTE: Using TextChanged() or TextUpdate() instead of Validated() causes every character change to be stored and user typing is reversed due to cursor reset.
	Private Sub ComboTextBox_Validated(sender As Object, e As EventArgs) Handles ComboTextBox.Validated
		'IMPORTANT: This is only needed for "Text" property due to Framework doing different stuff for the property.
		Me.OnTextChanged(e)

		If Not Me.theTextIsValidatingViaMe Then
			Me.theSelectedIndexIsChangingViaMe = True
			If Me.theTextHistoryIsKept AndAlso Me.Text <> "" Then
				Dim textHistory As BindingListEx(Of String) = CType(Me.TextHistoryDataGridView.DataSource, BindingListEx(Of String))
				If Not textHistory.Contains(Me.Text) Then
					textHistory.Insert(0, Me.Text)
					'Me.TextHistoryDataGridView.HorizontalExtent = 1
					'If textHistory.Count > Me.theTextHistoryMaxSize Then
					'	textHistory.RemoveAt(textHistory.Count - 1)
					'	Dim textSize As Size
					'	For Each itemText As String In textHistory
					'		textSize = TextRenderer.MeasureText(itemText, Me.TextHistoryDataGridView.Font)
					'		If textSize.Width > Me.TextHistoryDataGridView.HorizontalExtent Then
					'			Me.TextHistoryDataGridView.HorizontalExtent = textSize.Width
					'		End If
					'	Next
					'Else
					'	Dim textSize As Size = TextRenderer.MeasureText(Me.Text, Me.TextHistoryDataGridView.Font)
					'	If textSize.Width > Me.TextHistoryDataGridView.HorizontalExtent Then
					'		Me.TextHistoryDataGridView.HorizontalExtent = textSize.Width
					'	End If
					'End If
					''Me.TextHistoryDataGridView.Height = Me.TextHistoryDataGridView.ItemHeight * (Me.TextHistoryDataGridView.Items.Count + 1)
				End If
			End If
			Me.TextHistoryDataGridView.Text = Me.ComboTextBox.Text
			Me.theSelectedIndexIsChangingViaMe = False

			Me.UpdateMultipleInputsWidgets()
		End If
	End Sub

	Private Sub MultipleInputsButton_MouseDown(sender As Object, e As EventArgs) Handles MultipleInputsDropDownButton.MouseDown
		Me.OnMultipleInputsButton_MouseDown()
	End Sub

	Private Sub MultipleInputsPopup_VisibleChanged(sender As Object, e As EventArgs) Handles MultipleInputsPopup.VisibleChanged
		Me.OnMultipleInputsPopup_VisibleChanged()
	End Sub

	Private Sub TextHistoryDropDownButton_MouseDown(sender As Object, e As EventArgs) Handles TextHistoryDropDownButton.MouseDown
		Me.OnDropDownButton_MouseDown()
	End Sub

	' This event is not raised when dropdown list is showing.
	Private Sub TextHistoryDropDownButton_MouseEnter(sender As Object, e As EventArgs) Handles TextHistoryDropDownButton.MouseEnter
		'Me.thePointerIsOverTheDropDownButton = True
		Me.Highlight()
	End Sub

	Private Sub TextHistoryDropDownButton_MouseLeave(sender As Object, e As EventArgs) Handles TextHistoryDropDownButton.MouseLeave
		'Me.thePointerIsOverTheDropDownButton = False
		Me.Diminish()
	End Sub

	Private Sub TextHistoryListBox_KeyDown(sender As Object, e As KeyEventArgs)
		If e.KeyCode = Keys.Tab OrElse e.KeyCode = Keys.Enter OrElse e.KeyCode = Keys.Return Then
			Me.TextHistoryPopup.Hide()
			Me.UpdateTextBoxWithSelectedHistoryText()
		End If
	End Sub

	Private Sub TextHistoryListBox_MouseClick(sender As Object, e As EventArgs) Handles TextHistoryDataGridView.MouseClick
		Me.OnTextHistoryDataGridView_MouseClick()
	End Sub

	'Private Sub TextHistoryListBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'	If Not TextHistoryPopup.Visible Then
	'		Me.Text = CStr(Me.TextHistoryDataGridView.SelectedRows(0).Cells(0).Value)
	'	End If
	'End Sub

	Private Sub TextHistoryPopup_VisibleChanged(sender As Object, e As EventArgs) Handles TextHistoryPopup.VisibleChanged
		Me.OnTextHistoryPopup_VisibleChanged()
	End Sub

#End Region

#Region "Core Event Handlers"

#End Region

#Region "Private Methods"

	Private Sub UpdateNonClientPadding()
		If Me.theBorderStyle = BorderStyle.FixedSingle Then
			Me.NonClientPadding = New Padding(1)
		Else
			Me.NonClientPadding = New Padding(0)
		End If
	End Sub

	Private Sub ResizeClientRect(ByVal padding As Padding, ByRef rect As Win32Api.RECT)
		rect.Left += padding.Left
		rect.Top += padding.Top
		rect.Right -= padding.Right
		rect.Bottom -= padding.Bottom
	End Sub

	Private Sub Highlight()
		Dim theme As ComboUserControlTheme = Nothing
		' This check prevents problems with viewing and saving Forms in VS Designer.
		If TheApp IsNot Nothing Then
			theme = TheApp.Settings.SelectedAppTheme.ComboUserControlTheme
		End If
		If theme IsNot Nothing Then
			Me.theBorderColor = theme.FocusBorderColor
			'Else
			'    Me.theBorderColor = WidgetConstants.Windows10GlobalAccentColor
		End If
		''NOTE: Raise the OnNonClientCalcSize and OnNonClientPaint "events".
		'Win32Api.SetWindowPos(Me.Handle, IntPtr.Zero, 0, 0, 0, 0, Win32Api.SWP.SWP_FRAMECHANGED Or Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE Or Win32Api.SWP.SWP_NOZORDER)
		Me.Invalidate()
	End Sub

	Private Sub Diminish()
		Dim theme As ComboUserControlTheme = Nothing
		' This check prevents problems with viewing and saving Forms in VS Designer.
		If TheApp IsNot Nothing Then
			theme = TheApp.Settings.SelectedAppTheme.ComboUserControlTheme
		End If
		If theme IsNot Nothing Then
			Me.theBorderColor = theme.EnabledBorderColor
			'Else
			'    Me.theBorderColor = WidgetConstants.WidgetDisabledTextColor
		End If
		''NOTE: Raise the OnNonClientCalcSize and OnNonClientPaint "events".
		'Win32Api.SetWindowPos(Me.Handle, IntPtr.Zero, 0, 0, 0, 0, Win32Api.SWP.SWP_FRAMECHANGED Or Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE Or Win32Api.SWP.SWP_NOZORDER)
		Me.Invalidate()
	End Sub

	Private Sub InitMultipleInputsPopop()
		If Me.MultipleInputsPopup Is Nothing Then
			'IMPORTANT: Avoid using an incorrect font.
			Me.MultipleInputsDataGridView.Font = Me.Font
			Me.MultipleInputsDataGridView.BindingContext = New BindingContext()

			Me.theDropDownButtonWasClickedWhenPopupShowing = False
			Me.MultipleInputsPopup = New Popup(Me.MultipleInputsDataGridView)
			Me.MultipleInputsPopup.Name = "MultipleInputsPopup"
			'Me.MultipleInputsDropDownButton.OuterPopup = Me.MultipleInputsPopup
		End If
	End Sub

	Private Sub InitTextHistoryPopop()
		If Me.TextHistoryPopup Is Nothing Then
			'IMPORTANT: Avoid using an incorrect font.
			Me.TextHistoryDataGridView.Font = Me.Font
			Me.TextHistoryDataGridView.BindingContext = New BindingContext()

			Me.theDropDownButtonWasClickedWhenPopupShowing = False
			Me.TextHistoryPopup = New Popup(Me.TextHistoryDataGridView)
			Me.TextHistoryPopup.Name = "TextHistoryPopup"
			'Me.TextHistoryDropDownButton.OuterPopup = Me.TextHistoryPopup

			'DEBUG:
			'Me.theTextHistoryMaxSize = 15
			'Me.TextHistoryDataGridView.DataSource = New BindingListEx(Of String)
			'Dim textHistory As BindingListEx(Of String) = CType(Me.TextHistoryDataGridView.DataSource, BindingListEx(Of String))
			'textHistory.Insert(0, "Test01")
			'textHistory.Insert(0, "Test02")
			'textHistory.Insert(0, "Test03")
			''Me.TextHistoryDataGridView.HorizontalExtent = 1
			''If textHistory.Count > Me.theTextHistoryMaxSize Then
			''	textHistory.RemoveAt(textHistory.Count - 1)
			''	Dim textSize As Size
			''	For Each itemText As String In textHistory
			''		textSize = TextRenderer.MeasureText(itemText, Me.TextHistoryDataGridView.Font)
			''		If textSize.Width > Me.TextHistoryDataGridView.HorizontalExtent Then
			''			Me.TextHistoryDataGridView.HorizontalExtent = textSize.Width
			''		End If
			''	Next
			''Else
			''	Dim textSize As Size = TextRenderer.MeasureText(Me.Text, Me.TextHistoryDataGridView.Font)
			''	If textSize.Width > Me.TextHistoryDataGridView.HorizontalExtent Then
			''		Me.TextHistoryDataGridView.HorizontalExtent = textSize.Width
			''	End If
			''End If
			''Me.TextHistoryDataGridView.Height = Me.TextHistoryDataGridView.ItemHeight * (Me.TextHistoryDataGridView.Items.Count + 1)
		End If
	End Sub

	Private Sub OnMultipleInputsButton_MouseDown()
		If Not Me.theDropDownButtonWasClickedWhenPopupShowing Then
			'IMPORTANT: Resize the ListBox.
			Me.MultipleInputsDataGridView.Size = Me.MultipleInputsDataGridView.PreferredSize

			Dim position As Point = New Point(0, Me.Height)
			position = Me.PointToScreen(position)
			Me.MultipleInputsPopup.Show(position)
			Me.theDropDownButtonWasClickedWhenPopupShowing = True
		Else
			'NOTE: Make sure Popup hides because TextHistoryPopup can still be visible when user clicks very quickly .
			Me.MultipleInputsPopup.Hide()
			Me.theDropDownButtonWasClickedWhenPopupShowing = False
		End If
	End Sub

	Private Sub OnMultipleInputsPopup_VisibleChanged()
		If Not Me.MultipleInputsPopup.Visible Then
			Me.MultipleInputsDropDownButton.Invalidate()
			'Dim cursorPositionInClient As Point = Me.PointToClient(Cursor.Position)
			'Dim aControl As Control = Me.GetChildAtPoint(cursorPositionInClient)
			'If aControl IsNot Me.MultipleInputsDropDownButton Then
			'    Me.theDropDownButtonWasClickedWhenPopupShowing = False
			'End If
		End If
	End Sub

	Private Sub OnDropDownButton_MouseDown()
		If Not Me.theDropDownButtonWasClickedWhenPopupShowing Then
			'IMPORTANT: Resize the ListBox.
			'Me.TextHistoryPopup.Size = Me.Size
			Me.TextHistoryDataGridView.Width = Me.Width
			Dim itemCount As Integer = Me.TextHistoryDataGridView.Rows.Count
			If itemCount > Me.theMaxDropDownItemCount Then
				itemCount = Me.theMaxDropDownItemCount
			End If
			'Me.TextHistoryDataGridView.Height = Me.TextHistoryDataGridView.Rows(0).Height * (itemCount + 1)

			' Select the list item with the textbox text, if it exists.
			Me.theSelectedIndexIsChangingViaMe = True
			Me.TextHistoryDataGridView.Text = Me.ComboTextBox.Text
			Me.theSelectedIndexIsChangingViaMe = False

			' Use -1 for X to account for left border.
			Dim position As Point = New Point(-1, Me.Height)
			position = Me.PointToScreen(position)
			Me.TextHistoryPopup.Show(position)
			Me.TextHistoryPopup.Height = Me.TextHistoryDataGridView.Rows(0).Height * (itemCount + 1)
			Me.TextHistoryPopup.Width = Me.Width
			Me.TextHistoryDataGridView.Focus()
			Me.theDropDownButtonWasClickedWhenPopupShowing = True
		Else
			'NOTE: Make sure Popup hides because TextHistoryPopup can still be visible when user clicks very quickly .
			Me.TextHistoryPopup.Hide()
			Me.theDropDownButtonWasClickedWhenPopupShowing = False
		End If
	End Sub

	Private Sub OnTextHistoryPopup_VisibleChanged()
		If Not Me.TextHistoryPopup.Visible Then
			Me.TextHistoryDropDownButton.Invalidate()

			Dim cursorPositionInClient As Point = Me.PointToClient(Cursor.Position)
			Dim aControl As Control = Me.GetChildAtPoint(cursorPositionInClient)
			If aControl IsNot Me.TextHistoryDropDownButton Then
				Me.theDropDownButtonWasClickedWhenPopupShowing = False
			End If
		End If
	End Sub

	Private Sub OnTextHistoryDataGridView_MouseClick()
		Me.TextHistoryPopup.Hide()
		Me.UpdateTextBoxWithSelectedHistoryText()
	End Sub

	Private Sub CreateContextMenu()
		Me.CustomMenu = New ContextMenuStrip()
		Me.CustomMenu.Items.Add(Me.UndoToolStripMenuItem)
		'Me.CustomMenu.Items.Add(Me.RedoToolStripMenuItem)
		Me.CustomMenu.Items.Add(Me.Separator0ToolStripSeparator)
		Me.CustomMenu.Items.Add(Me.CutToolStripMenuItem)
		Me.CustomMenu.Items.Add(Me.CopyToolStripMenuItem)
		Me.CustomMenu.Items.Add(Me.PasteToolStripMenuItem)
		Me.CustomMenu.Items.Add(Me.DeleteToolStripMenuItem)
		Me.CustomMenu.Items.Add(Me.Separator1ToolStripSeparator)
		Me.CustomMenu.Items.Add(Me.SelectAllToolStripMenuItem)
		Me.CustomMenu.Items.Add(Me.CopyAllToolStripMenuItem)
		Me.ContextMenuStrip = Me.CustomMenu
		Me.ComboTextBox.ContextMenuStrip = Me.CustomMenu
	End Sub

	Private Sub UpdateMultipleInputsWidgets()
		If Me.theTextIsPathFileNames Then
			Dim pathFileNames As String() = FileManager.GetPathFileNames(Me.ComboTextBox.Text)
			If pathFileNames.Length = 1 Then
				Me.MultipleInputsDropDownButton.Enabled = False
			Else
				Me.MultipleInputsDropDownButton.Enabled = True
				Me.MultipleInputsDataGridView.DataSource = pathFileNames
			End If
		End If
	End Sub

	Private Sub ClearTextHistory()
		'' Save and later restore current text because inexplicably it is deleted when the history is cleared.
		'Dim currentText As String = Me.Text

		Dim textHistory As List(Of String) = CType(Me.TextHistoryDataGridView.DataSource, List(Of String))
		textHistory.Clear()

		'Me.Text = currentText
	End Sub

	Private Sub UpdateTextBoxWithSelectedHistoryText()
		'Me.Text = Me.TextHistoryDataGridView.Text
		' This check prevents problems with viewing and saving Forms in VS Designer.
		Dim aSelectedValue As String = Me.TextHistoryDataGridView.Text
		If Me.TextHistoryDataGridView.SelectedRows.Count > 0 Then
			aSelectedValue = CStr(Me.TextHistoryDataGridView.SelectedRows(0).Cells(0).Value)
		End If
		Me.Text = aSelectedValue
		RaiseEvent SelectedIndexChanged(Me, New EventArgs())
		RaiseEvent SelectedValueChanged(Me, New EventArgs())
	End Sub

#End Region

#Region "Data"

	Private NonClientPadding As Padding
	Private theBorderColor As Color
	Private theBorderStyle As BorderStyle
	'Private theOriginalFont As Font
	Protected theControlIsReadOnly As Boolean
	Protected theComboPanelBorderColor As Color

	Private WithEvents CustomMenu As ContextMenuStrip
	Private WithEvents UndoToolStripMenuItem As New ToolStripMenuItem("&Undo")
	'Private WithEvents RedoToolStripMenuItem As New ToolStripMenuItem("&Redo")
	Private WithEvents Separator0ToolStripSeparator As New ToolStripSeparator()
	Private WithEvents CutToolStripMenuItem As New ToolStripMenuItem("Cu&t")
	Private WithEvents CopyToolStripMenuItem As New ToolStripMenuItem("&Copy")
	Private WithEvents PasteToolStripMenuItem As New ToolStripMenuItem("&Paste")
	Private WithEvents DeleteToolStripMenuItem As New ToolStripMenuItem("&Delete")
	Private WithEvents Separator1ToolStripSeparator As New ToolStripSeparator()
	Private WithEvents SelectAllToolStripMenuItem As New ToolStripMenuItem("Select &All")
	Private WithEvents CopyAllToolStripMenuItem As New ToolStripMenuItem("Copy A&ll")
	Private WithEvents Separator2ToolStripSeparator As New ToolStripSeparator()
	Private WithEvents ClearTextHistoryToolStripMenuItem As New ToolStripMenuItem("Clear &History")

	Protected WithEvents MultipleInputsPopup As Popup
	Protected theMultipleInputsIsAllowed As Boolean

	Protected WithEvents TextHistoryPopup As Popup
	Protected theMaxDropDownItemCount As Integer
	Protected theTextHistoryIsKept As Boolean
	'Protected theTextHistory As List(Of String)
	Protected theTextHistoryMaxSize As Integer
	Protected theTextIsValidatingViaMe As Boolean
	Protected theSelectedIndexIsChangingViaMe As Boolean
	Protected theTextIsPathFileNames As Boolean

	'NOTE: Need this to handle when dropdown button can show the Popup because the Popup automatically hides itself.
	Protected theDropDownButtonWasClickedWhenPopupShowing As Boolean
	Protected thePointerIsOverTheDropDownButton As Boolean

#End Region

	''NOTE: Using Panel instead of Button because Button has inconsistency between display and actual rectangle.
	'Public Class DropDownButton
	'    Inherits Panel

	'    Public Property OuterPopup As Popup
	'        Get
	'            Return Me.theOuterPopup
	'        End Get
	'        Set(value As Popup)
	'            Me.theOuterPopup = value
	'        End Set
	'    End Property

	'    Protected Overrides Sub OnHandleCreated(e As EventArgs)
	'        MyBase.OnHandleCreated(e)
	'        'NOTE: Me.Parent is incorrect type in DesignMode.
	'        If Not Me.DesignMode Then
	'            Me.theParentComboUserControl = CType(Me.Parent, ComboUserControl)
	'        End If
	'    End Sub

	'    Protected Overrides Sub OnMouseCaptureChanged(e As EventArgs)
	'        MyBase.OnMouseCaptureChanged(e)
	'        If Me.Enabled AndAlso Me.theParentComboUserControl IsNot Nothing AndAlso Not Me.theOuterPopup.Visible Then
	'            If Not Me.Capture Then
	'                Me.Invalidate()
	'            End If
	'        End If
	'    End Sub

	'    Protected Overrides Sub OnMouseEnter(e As EventArgs)
	'        MyBase.OnMouseEnter(e)
	'        If Me.Enabled AndAlso Me.theParentComboUserControl IsNot Nothing AndAlso Not Me.theOuterPopup.Visible Then
	'            Me.Capture = True
	'            'Me.Invalidate()
	'        End If
	'    End Sub

	'    Protected Overrides Sub OnMouseLeave(e As EventArgs)
	'        MyBase.OnMouseLeave(e)
	'        If Me.Enabled AndAlso Me.theParentComboUserControl IsNot Nothing AndAlso Not Me.theOuterPopup.Visible Then
	'            Me.Capture = False
	'            'Me.Invalidate()
	'        End If
	'    End Sub

	'    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
	'        MyBase.OnMouseMove(e)
	'        If Me.Enabled AndAlso Me.theParentComboUserControl IsNot Nothing AndAlso Not Me.theOuterPopup.Visible Then
	'            Dim pt As New Point(e.X, e.Y)
	'            If Not Me.ClientRectangle.Contains(pt) Then
	'                Me.Capture = False
	'                'Me.Invalidate()
	'            End If
	'        End If
	'    End Sub

	'    Protected Overrides Sub OnPaint(e As PaintEventArgs)
	'        MyBase.OnPaint(e)

	'        Dim mouseIsOverControl As Boolean = HelperFunctions.MouseIsOverControl(Me)
	'        If Me.Name = "MultipleInputsDropDownButton" Then
	'            Dim state As VisualStyles.PushButtonState = VisualStyles.PushButtonState.Normal
	'            If Me.Enabled Then
	'                If mouseIsOverControl Then
	'                    If MouseButtons = MouseButtons.Left Then
	'                        state = VisualStyles.PushButtonState.Pressed
	'                    Else
	'                        state = VisualStyles.PushButtonState.Hot
	'                    End If
	'                End If
	'            Else
	'                state = VisualStyles.PushButtonState.Disabled
	'            End If
	'            Dim rect As Rectangle = Me.DisplayRectangle
	'            rect.Inflate(1, 1)
	'            ButtonRenderer.DrawButton(e.Graphics, rect, state)
	'            If Me.Enabled Then
	'                TextRenderer.DrawText(e.Graphics, Me.Text, Me.Font, rect, Me.ForeColor)
	'            Else
	'                ControlPaint.DrawStringDisabled(e.Graphics, Me.Text, Me.Font, Me.BackColor, rect, TextFormatFlags.LeftAndRightPadding Or TextFormatFlags.VerticalCenter)
	'            End If
	'        Else
	'            If ComboBoxRenderer.IsSupported Then
	'                Dim state As VisualStyles.ComboBoxState = VisualStyles.ComboBoxState.Normal
	'                If mouseIsOverControl Then
	'                    If MouseButtons = MouseButtons.Left Then
	'                        state = VisualStyles.ComboBoxState.Pressed
	'                    Else
	'                        state = VisualStyles.ComboBoxState.Hot
	'                    End If
	'                End If
	'                ComboBoxRenderer.DrawDropDownButton(e.Graphics, Me.DisplayRectangle, state)
	'            Else
	'                ' Draw button without visualstyle.
	'                Dim state As ButtonState = ButtonState.Flat
	'                If mouseIsOverControl Then
	'                    If MouseButtons = MouseButtons.Left Then
	'                        state = ButtonState.Pushed
	'                    Else
	'                        state = ButtonState.Checked
	'                    End If
	'                End If
	'                ControlPaint.DrawComboButton(e.Graphics, Me.DisplayRectangle, state)
	'            End If
	'        End If

	'        '' Draw two or three lines around button to match border of ComboUserControl.
	'        'If Me.theParentComboUserControl IsNot Nothing AndAlso Not mouseIsOverControl Then
	'        '    Dim rect As Rectangle = Me.DisplayRectangle
	'        '    'rect.Inflate(-1, -1)
	'        '    rect.Width -= 1
	'        '    rect.Height -= 1
	'        '    Dim blackPen As New Pen(Me.theParentComboUserControl.theComboPanelBorderColor, 1)
	'        '    e.Graphics.DrawLine(blackPen, rect.Left, rect.Top, rect.Right, rect.Top)
	'        '    e.Graphics.DrawLine(blackPen, rect.Left, rect.Bottom, rect.Right, rect.Bottom)

	'        '    'NOTE: Only draw right-side vertical line if at right edge of outer panel.
	'        '    'Dim outerPanelRect As Rectangle = Me.OuterComboUserControl.ComboPanel.DisplayRectangle
	'        '    'outerPanelRect.Inflate(-1, -1)
	'        '    If Me.Right = Me.theParentComboUserControl.ComboBackgroundPanel.Right Then
	'        '        e.Graphics.DrawLine(blackPen, rect.Right, rect.Top, rect.Right, rect.Bottom)
	'        '    End If
	'        'End If
	'    End Sub

	'    Private theParentComboUserControl As ComboUserControl
	'    Private theOuterPopup As Popup

	'End Class

End Class
