Imports System.ComponentModel
Imports System.Runtime.InteropServices

Public Class DataGridViewEx
    Inherits DataGridView

#Region "Create and Destroy"

    Public Sub New()
        MyBase.New()

        'NOTE: Disable to use custom.
        MyBase.ScrollBars = Windows.Forms.ScrollBars.None

        'Me.thePaddingColor = WidgetDeepBackColor
        Me.theNonClientPaddingColor = WidgetDeepBackColor
        'TEST:
        'Me.theNonClientPaddingColor = Color.Pink

        Me.CustomHorizontalScrollbar = New ScrollBarEx()
        Me.Controls.Add(Me.CustomHorizontalScrollbar)
        Me.CustomHorizontalScrollbar.Location = New System.Drawing.Point(0, Me.ClientRectangle.Height)
        Me.CustomHorizontalScrollbar.Name = "CustomHorizontalScrollbar"
        Me.CustomHorizontalScrollbar.Size = New System.Drawing.Size(Me.Width, ScrollBarEx.Consts.ScrollBarSize)
        Me.CustomHorizontalScrollbar.ScrollOrientation = ScrollBarEx.DarkScrollOrientation.Horizontal
        Me.CustomHorizontalScrollbar.TabIndex = 7
        Me.CustomHorizontalScrollbar.Text = "CustomHorizontalScrollbar"
        Me.CustomHorizontalScrollbar.Visible = False

        Me.CustomVerticalScrollBar = New ScrollBarEx()
        Me.Controls.Add(Me.CustomVerticalScrollBar)
        Me.CustomVerticalScrollBar.Location = New System.Drawing.Point(Me.ClientRectangle.Width, 0)
        Me.CustomVerticalScrollBar.Name = "CustomVerticalScrollBar"
        Me.CustomVerticalScrollBar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.Height)
        Me.CustomVerticalScrollBar.ScrollOrientation = ScrollBarEx.DarkScrollOrientation.Vertical
        Me.CustomVerticalScrollBar.TabIndex = 7
        Me.CustomVerticalScrollBar.Text = "CustomVerticalScrollBar"
        Me.CustomVerticalScrollBar.Visible = False

        Me.theControlHasShown = False

        'NOTE: Need these settings so that ColumnHeadersDefaultCellStyle, DefaultCellStyle, and GridColor properties are used.
        '      Might affect other properties, too.
        Me.EnableHeadersVisualStyles = False
        Me.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        Me.CellBorderStyle = DataGridViewCellBorderStyle.Single
        Me.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single

        Me.ForeColor = WidgetConstants.WidgetTextColor
        Me.BackgroundColor = WidgetConstants.WidgetBackColor

        Me.ColumnHeadersDefaultCellStyle.ForeColor = WidgetConstants.WidgetTextColor
        Me.ColumnHeadersDefaultCellStyle.BackColor = WidgetConstants.WidgetBackColor
        Me.ColumnHeadersDefaultCellStyle.SelectionForeColor = WidgetConstants.WidgetTextColor
        Me.ColumnHeadersDefaultCellStyle.SelectionBackColor = WidgetConstants.WidgetSelectedBackColor

        Me.DefaultCellStyle.ForeColor = WidgetConstants.WidgetTextColor
        Me.DefaultCellStyle.BackColor = WidgetConstants.WidgetBackColor
        Me.DefaultCellStyle.SelectionForeColor = WidgetConstants.WidgetTextColor
        Me.DefaultCellStyle.SelectionBackColor = WidgetConstants.WidgetSelectedBackColor

        Me.RowHeadersDefaultCellStyle.ForeColor = WidgetConstants.WidgetTextColor
        Me.RowHeadersDefaultCellStyle.BackColor = WidgetConstants.WidgetBackColor
        Me.RowHeadersDefaultCellStyle.SelectionForeColor = WidgetConstants.WidgetTextColor
        Me.RowHeadersDefaultCellStyle.SelectionBackColor = WidgetConstants.WidgetSelectedBackColor

        Me.GridColor = WidgetConstants.WidgetDisabledTextColor
        'Me.GridColor = Color.Green
        Me.BorderStyle = BorderStyle.None

        Me.theCurrentCellIsChangingBecauseOfMe = False
        Me.theSelectionIsChangingBecauseOfMe = False

        Me.theCellWherePointerIs = Nothing
    End Sub

#End Region

#Region "Init and Free"

#End Region

#Region "Properties"

    Public Overloads Property AutoGenerateColumns() As Boolean
        Get
            Return MyBase.AutoGenerateColumns
        End Get
        Set(ByVal value As Boolean)
            MyBase.AutoGenerateColumns = value
        End Set
    End Property

    Public Overloads Property DataSource() As Object
        Get
            Return MyBase.DataSource
        End Get
        Set(ByVal value As Object)
            If Me.DesignMode Then
                Exit Property
            End If
            If Me.IsCurrentCellInEditMode Then
                Me.EndEdit()
            End If
            MyBase.DataSource = value
        End Set
    End Property

    '<Browsable(False)>
    'Public Overloads ReadOnly Property HorizontalScrollBar() As ScrollBar
    '	Get
    '		Return MyBase.HorizontalScrollBar
    '	End Get
    'End Property

    Public Overloads Property [ReadOnly]() As Boolean
        Get
            Return MyBase.ReadOnly
        End Get
        Set(ByVal value As Boolean)
            If MyBase.ReadOnly <> value Then
                MyBase.ReadOnly = value

                If MyBase.ReadOnly Then
                    Me.DefaultCellStyle.BackColor = WidgetConstants.WidgetBackColor
                Else
                    Me.DefaultCellStyle.BackColor = WidgetConstants.WidgetDisabledTextColor
                End If
            End If
        End Set
    End Property

    <Browsable(True)>
    <Category("Layout")>
    <Description("Colorable scrollbars.")>
    Public Overloads Property ScrollBars As ScrollBars
        Get
            Return Me.theScrollBars
        End Get
        Set
            Me.theScrollBars = Value
        End Set
    End Property

#End Region

#Region "Methods"

#End Region

#Region "Widget Event Handlers"

    Protected Overrides Sub OnCellClick(ByVal e As DataGridViewCellEventArgs)
        If Not Me.ReadOnly AndAlso Me.Enabled AndAlso (e.RowIndex > -1) AndAlso (e.ColumnIndex > -1) Then
            Dim cell As DataGridViewCell = Me(e.ColumnIndex, e.RowIndex)
            If TypeOf cell.OwningColumn Is DataGridViewRadioButtonColumn Then
                If cell.FormattedValue.ToString().Length = 0 Then
                    For i As Integer = 0 To Me.RowCount - 1
                        Me(e.ColumnIndex, i).Value = String.Empty
                    Next
                    cell.Value = "Selected"
                End If
            End If
        End If

        MyBase.OnCellClick(e)
    End Sub

    'NOTE: This works for avoiding read-only cells, but it completely disallows entering the cells, so can't copy values if wanted.
    'Protected Overrides Sub OnCellEnter(ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
    '	Dim dgc As DataGridViewCell = TryCast(Me.Item(e.ColumnIndex, e.RowIndex), DataGridViewCell)
    '	If dgc IsNot Nothing AndAlso dgc.ReadOnly Then
    '		SendKeys.Send("{Tab}")
    '	End If
    '	MyBase.OnCellEnter(e)
    'End Sub

    'Private theMouseIsDown As Boolean = False

    'Protected Overrides Sub OnCellMouseDown(ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs)
    '	Me.theMouseIsDown = True
    '	MyBase.OnCellMouseDown(e)
    'End Sub

    'Protected Overrides Sub OnCellMouseMove(ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs)
    '	If Me.theMouseIsDown AndAlso Me.AllowUserToAddRows = True AndAlso e.RowIndex <> Me.NewRowIndex Then
    '		Me.theWidgetTempOfAllowUserToAddRows = Me.AllowUserToAddRows
    '		Me.AllowUserToAddRows = False
    '	End If
    '	MyBase.OnCellMouseMove(e)
    'End Sub

    'Protected Overrides Sub OnCellMouseUp(ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs)
    '	MyBase.OnCellMouseUp(e)
    '	Me.AllowUserToAddRows = Me.theWidgetTempOfAllowUserToAddRows
    '	Me.theMouseIsDown = False
    'End Sub

    Protected Overrides Sub OnCellMouseEnter(e As DataGridViewCellEventArgs)
        MyBase.OnCellMouseEnter(e)
        If (e.RowIndex > -1) AndAlso (e.ColumnIndex > -1) Then
            If Me.theCellWherePointerIs IsNot Nothing Then
                Me.InvalidateCell(Me.theCellWherePointerIs)
            End If
            Me.theCellWherePointerIs = Me.Rows(e.RowIndex).Cells(e.ColumnIndex)
            Me.InvalidateCell(e.ColumnIndex, e.RowIndex)
        End If
    End Sub

    Protected Overrides Sub OnCellMouseLeave(e As DataGridViewCellEventArgs)
        MyBase.OnCellMouseLeave(e)
        If (e.RowIndex > -1) AndAlso (e.ColumnIndex > -1) Then
            Me.theCellWherePointerIs = Nothing
            Me.InvalidateCell(e.ColumnIndex, e.RowIndex)
        End If
    End Sub

    Protected Overrides Sub OnCellEnter(ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        MyBase.OnCellEnter(e)
        If (e.RowIndex > -1) AndAlso (e.ColumnIndex > -1) Then
            If Me.theCellWherePointerIs IsNot Nothing AndAlso (Me.theCellWherePointerIs.RowIndex > -1) AndAlso (Me.theCellWherePointerIs.ColumnIndex > -1) Then
                Me.InvalidateCell(Me.theCellWherePointerIs)
            End If
            Me.theCellWherePointerIs = Me.Rows(e.RowIndex).Cells(e.ColumnIndex)
            Me.InvalidateCell(e.ColumnIndex, e.RowIndex)
        End If
    End Sub

    Protected Overrides Sub OnCellLeave(ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        MyBase.OnCellLeave(e)
        If (e.RowIndex > -1) AndAlso (e.ColumnIndex > -1) Then
            Me.theCellWherePointerIs = Nothing
            Me.InvalidateCell(e.ColumnIndex, e.RowIndex)
        End If
    End Sub

    Protected Overrides Sub OnCellPainting(ByVal e As DataGridViewCellPaintingEventArgs)
        If (e.RowIndex > -1) AndAlso (e.ColumnIndex > -1) Then
            If TypeOf Me.Columns(e.ColumnIndex) Is DataGridViewRadioButtonColumn Then
                e.PaintBackground(e.CellBounds, False)
                DataGridViewRadioButtonColumn.Paint(e.Graphics, e.CellBounds, (e.FormattedValue.ToString().Length > 0))
                e.Handled = True
            ElseIf TypeOf Me.Columns(e.ColumnIndex) Is DataGridViewButtonColumn Then
                'Dim column As DataGridViewButtonColumn = CType(Me.Columns(e.ColumnIndex), DataGridViewButtonColumn)
                Dim cell As DataGridViewCell = Me.Rows(e.RowIndex).Cells(e.ColumnIndex)

                Dim g As Graphics = e.Graphics
                Dim clientRectangle As Rectangle = e.CellBounds
                'clientRectangle.Width -= 2
                'clientRectangle.Height -= 2

                Dim backColor1 As Color = WidgetHighBackColor
                Dim backColor2 As Color = WidgetHighBackColor
                Dim textColor As Color = WidgetTextColor

                If Me.Enabled Then
                    If cell.Selected Then
                        backColor1 = Windows10GlobalAccentColor
                        backColor2 = Windows10GlobalAccentColor
                        textColor = WidgetTextColor
                    ElseIf cell Is Me.theCellWherePointerIs Then
                        backColor1 = Windows10GlobalAccentColor
                        backColor2 = WidgetHighBackColor
                        textColor = WidgetTextColor
                    Else
                        backColor1 = WidgetHighBackColor
                        backColor2 = WidgetHighBackColor
                        textColor = WidgetTextColor
                    End If
                Else
                    backColor1 = WidgetDeepBackColor
                    backColor2 = WidgetDeepBackColor
                    textColor = WidgetDisabledTextColor
                End If

                ' Draw background.
                Using aColorBrush As New Drawing2D.LinearGradientBrush(clientRectangle, backColor1, backColor2, Drawing2D.LinearGradientMode.Vertical)
                    g.FillRectangle(aColorBrush, clientRectangle)
                End Using
                TextRenderer.DrawText(g, CType(cell.Value, String), Me.Font, clientRectangle, textColor, TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or TextFormatFlags.WordBreak)

                ' Draw the grid lines (only the right and bottom lines;
                ' DataGridView takes care of the others).
                Dim gridBrush As New SolidBrush(Me.GridColor)
                Dim gridLinePen As New Pen(gridBrush)
                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1)
                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1)

                e.Handled = True
            ElseIf TypeOf Me.Columns(e.ColumnIndex) Is DataGridViewTextBoxColumn Then
                Dim cell As DataGridViewCell = Me.Rows(e.RowIndex).Cells(e.ColumnIndex)

                Dim g As Graphics = e.Graphics
                Dim clientRectangle As Rectangle = e.CellBounds

                Dim backColor1 As Color = WidgetHighBackColor
                Dim backColor2 As Color = WidgetHighBackColor
                Dim textColor As Color = WidgetTextColor

                If Me.Enabled Then
                    If cell.Selected Then
                        backColor1 = Windows10GlobalAccentColor
                        backColor2 = Windows10GlobalAccentColor
                        textColor = WidgetTextColor
                    ElseIf cell Is Me.theCellWherePointerIs Then
                        backColor1 = Windows10GlobalAccentColor
                        backColor2 = WidgetHighBackColor
                        textColor = WidgetTextColor
                    Else
                        backColor1 = WidgetHighBackColor
                        backColor2 = WidgetHighBackColor
                        textColor = WidgetTextColor
                    End If
                Else
                    backColor1 = WidgetDeepBackColor
                    backColor2 = WidgetDeepBackColor
                    textColor = WidgetDisabledTextColor
                End If

                ' Draw background.
                Using aColorBrush As New Drawing2D.LinearGradientBrush(clientRectangle, backColor1, backColor2, Drawing2D.LinearGradientMode.Vertical)
                    g.FillRectangle(aColorBrush, clientRectangle)
                End Using
                Dim textFormat As TextFormatFlags = TextFormatFlags.Default
                If cell.InheritedStyle.WrapMode = DataGridViewTriState.True Then
                    textFormat = textFormat Or TextFormatFlags.WordBreak
                End If
                TextRenderer.DrawText(g, CType(cell.Value, String), Me.Font, clientRectangle, textColor, TextFormatFlags.VerticalCenter)

                e.Handled = True
            End If
        End If

        MyBase.OnCellPainting(e)
    End Sub

    ' When new row is added, commit it by tricking the datagridview.
    Protected Overrides Sub OnCurrentCellChanged(ByVal e As System.EventArgs)
        Try
            If Me.CurrentCell IsNot Nothing Then
                If Not Me.theCurrentCellIsChangingBecauseOfMe AndAlso Me.CurrentRow.IsNewRow AndAlso TypeOf Me.DataSource Is System.ComponentModel.IBindingList Then
                    'Grab the object bound to the new row of the grid. We have to
                    ' access this object from the underlying BindingContext because
                    ' the DataBoundItem of the grid's new row returns nothing. This
                    ' is because the new object hasn't been added to the bound list,
                    ' so it isn't technically data-bound to the grid.

                    Dim newObject As Object = Me.BindingContext(Me.DataSource).Current
                    Dim curCell As Point = Me.CurrentCellAddress

                    Me.theCurrentCellIsChangingBecauseOfMe = True
                    Me.CancelEdit()
                    'NOTE: This line raises an exception if there are no rows already in the DataSource.
                    Me.CurrentCell = Nothing
                    'Programmatically add the new object to the bound list.
                    ' We're assuming the bound list is an implementation of the
                    ' IBindingList interface. 
                    DirectCast(Me.DataSource, System.ComponentModel.IBindingList).Add(newObject)
                    Me.CurrentCell = Me(curCell.X, curCell.Y)
                    Me.theCurrentCellIsChangingBecauseOfMe = False
                End If
            End If

            MyBase.OnCurrentCellChanged(e)
        Catch ex As Exception
        Finally
        End Try
    End Sub

    Protected Overrides Sub OnCurrentCellDirtyStateChanged(ByVal e As System.EventArgs)
        ' Force immediate update when clicking a checkbox cell or selecting a value in combobox cell.
        If TypeOf Me.CurrentCell Is DataGridViewCheckBoxCell OrElse TypeOf Me.CurrentCell Is DataGridViewComboBoxCell Then
            Me.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If

        MyBase.OnCurrentCellDirtyStateChanged(e)
    End Sub

    Protected Overrides Sub OnDataError(ByVal displayErrorDialogIfNoHandler As Boolean, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs)
        ' Disable default error window.
        'e.Cancel = False
        displayErrorDialogIfNoHandler = False

        MyBase.OnDataError(displayErrorDialogIfNoHandler, e)
    End Sub

    Protected Overrides Sub OnColumnAdded(e As DataGridViewColumnEventArgs)
        MyBase.OnColumnAdded(e)
        If TypeOf e.Column Is DataGridViewButtonColumn Then
            Dim buttonColumn As DataGridViewButtonColumn = CType(e.Column, DataGridViewButtonColumn)
            buttonColumn.FlatStyle = FlatStyle.Flat
            'buttonColumn.FlatStyle = FlatStyle.Popup
            'buttonColumn.DefaultCellStyle.ForeColor = WidgetConstants.WidgetTextColor
            'buttonColumn.DefaultCellStyle.BackColor = WidgetConstants.WidgetHighBackColor
            'buttonColumn.DefaultCellStyle.SelectionForeColor = Color.Red
            'buttonColumn.DefaultCellStyle.SelectionBackColor = Color.Green
        End If
    End Sub

    Protected Overrides Sub OnColumnWidthChanged(e As DataGridViewColumnEventArgs)
        MyBase.OnColumnWidthChanged(e)
        'NOTE: Raise the OnNonClientCalcSize and OnNonClientPaint "events".
        Win32Api.SetWindowPos(Me.Handle, IntPtr.Zero, 0, 0, 0, 0, Win32Api.SWP.SWP_FRAMECHANGED Or Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE Or Win32Api.SWP.SWP_NOZORDER)
        Me.Invalidate()
        Me.UpdateScrollbars()
    End Sub

    Protected Overrides Sub OnRowsAdded(e As DataGridViewRowsAddedEventArgs)
        MyBase.OnRowsAdded(e)
        'NOTE: Raise the OnNonClientCalcSize and OnNonClientPaint "events".
        Win32Api.SetWindowPos(Me.Handle, IntPtr.Zero, 0, 0, 0, 0, Win32Api.SWP.SWP_FRAMECHANGED Or Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE Or Win32Api.SWP.SWP_NOZORDER)
        Me.UpdateScrollbars()
    End Sub

    Protected Overrides Sub OnRowsRemoved(e As DataGridViewRowsRemovedEventArgs)
        MyBase.OnRowsRemoved(e)
        'NOTE: Raise the OnNonClientCalcSize and OnNonClientPaint "events".
        Win32Api.SetWindowPos(Me.Handle, IntPtr.Zero, 0, 0, 0, 0, Win32Api.SWP.SWP_FRAMECHANGED Or Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE Or Win32Api.SWP.SWP_NOZORDER)
        Me.UpdateScrollbars()
    End Sub

    Protected Overrides Sub OnSelectionChanged(ByVal e As System.EventArgs)
        'NOTE: Prevent the "New" row being selected so it doesn't cause problems.
        If Not Me.theSelectionIsChangingBecauseOfMe AndAlso Me.NewRowIndex >= 0 Then
            Me.theSelectionIsChangingBecauseOfMe = True
            Me.Rows(Me.NewRowIndex).Selected = False
            Me.theSelectionIsChangingBecauseOfMe = False
        End If

        MyBase.OnSelectionChanged(e)
    End Sub

    Protected Overrides Sub OnScroll(e As ScrollEventArgs)
        MyBase.OnScroll(e)
        Me.Invalidate()
        Me.UpdateScrollbars()
    End Sub

    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        MyBase.OnSizeChanged(e)
        Me.Invalidate()
        Me.UpdateScrollbars()
    End Sub

    Protected Overrides Sub OnVisibleChanged(e As EventArgs)
        MyBase.OnVisibleChanged(e)

        If Me.Visible Then
            If Not Me.theControlHasShown Then
                Me.theControlHasShown = True

                'NOTE: Raise the OnNonClientCalcSize and OnNonClientPaint "events".
                Win32Api.SetWindowPos(Me.Handle, IntPtr.Zero, 0, 0, 0, 0, Win32Api.SWP.SWP_FRAMECHANGED Or Win32Api.SWP.SWP_NOMOVE Or Win32Api.SWP.SWP_NOSIZE Or Win32Api.SWP.SWP_NOZORDER)
            End If

            Me.Invalidate()
            Me.UpdateScrollbars()
        End If
    End Sub

    Protected Function MyProcessTabKey(ByVal keysPressed As Keys) As Boolean
        Dim retValue As Boolean = MyBase.ProcessTabKey(keysPressed)

        'While Me.CurrentCell.[ReadOnly]
        '	retValue = MyBase.ProcessTabKey(keysPressed)
        'End While
        '------
        Dim previousCell As DataGridViewCell = Nothing
        While Me.CurrentCell.[ReadOnly] AndAlso previousCell IsNot Me.CurrentCell
            previousCell = Me.CurrentCell
            retValue = MyBase.ProcessTabKey(keysPressed)
        End While

        '' Reverse direction of tabbing in case at end of grid and on a read-only cell.
        'keysPressed = keysPressed Xor Keys.Shift
        'previousCell = Nothing
        ''While Me.CurrentCell.[ReadOnly] AndAlso previousCell IsNot Me.CurrentCell AndAlso (Me.CurrentCell.RowIndex = 0 AndAlso Me.CurrentCell.ColumnIndex = 0) OrElse (Me.CurrentCell.RowIndex = Me.RowCount - 1 AndAlso Me.CurrentCell.ColumnIndex = Me.ColumnCount - 1)
        'While Me.CurrentCell.[ReadOnly] AndAlso previousCell IsNot Me.CurrentCell
        '	previousCell = Me.CurrentCell
        '	retValue = MyBase.ProcessTabKey(keysPressed)
        'End While

        Return retValue
    End Function

    Protected Overloads Overrides Function ProcessDataGridViewKey(ByVal e As KeyEventArgs) As Boolean
        If e.KeyCode = Keys.Tab Then
            Dim keysPressed As Keys
            If e.Shift Then
                keysPressed = (Keys.Shift Or Keys.Tab)
            Else
                keysPressed = Keys.Tab
            End If
            MyProcessTabKey(keysPressed)
            Return True
        End If
        'If e.KeyCode = Keys.Enter Then
        '	' Instead of moving down to next row, begin editing of cell.
        '	Me.BeginEdit(True)
        '	Return True
        'End If
        Return MyBase.ProcessDataGridViewKey(e)
    End Function

    Protected Overloads Overrides Function ProcessDialogKey(ByVal keyData As Keys) As Boolean
        'NOTE: If this 'if' block is used, then tabbing in and out of datagridview won't work.
        'If keyData = Keys.Tab Then
        '	'Dim keysPressed As Keys
        '	'If e.Shift Then
        '	'	keysPressed = (Keys.Shift Or Keys.Tab)
        '	'Else
        '	'	keysPressed = Keys.Tab
        '	'End If
        '	'MyProcessTabKey(keysPressed)
        '	MyProcessTabKey(Keys.Tab)
        '	Return True
        'End If
        Dim key As Keys = (keyData And Keys.KeyCode)
        If key = Keys.Enter Then
            ' Instead of moving down to next row, validate the cell (by changing the CurrentCell), which effectively ends editing of cell.
            Dim savedCurrentCell As DataGridViewCell = Me.CurrentCell
            Me.CurrentCell = Nothing
            Me.CurrentCell = savedCurrentCell
            'If savedCurrentCell.IsInEditMode Then
            '	Me.EndEdit()
            'End If
            Return True
        End If
        Return MyBase.ProcessDialogKey(keyData)
    End Function

    Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)
        MyBase.OnMouseWheel(e)

        If Me.CustomVerticalScrollBar.Visible Then
            'NOTE: Scroll by 3 rows.
            Dim rowCount As Integer = Me.RowCount
            If rowCount > 0 Then
                Dim upOrDownValue As Integer = Me.Rows(0).Height * 3
                If e.Delta > 0 Then
                    ' Moving wheel away from user = up.
                    Me.CustomVerticalScrollBar.Value -= upOrDownValue
                Else
                    ' Moving wheel toward user = down.
                    Me.CustomVerticalScrollBar.Value += upOrDownValue
                End If
            End If
        End If
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        ' Draw outer border.
        If Me.BorderStyle <> BorderStyle.None Then
            Using backColorPen As New Pen(WidgetConstants.WidgetDisabledTextColor)
                Dim aRect As Rectangle = Me.ClientRectangle
                aRect.Width -= 1
                aRect.Height -= 1
                e.Graphics.DrawRectangle(backColorPen, aRect)
            End Using
        End If
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
        Dim hDC As IntPtr = Win32Api.GetWindowDC(Me.Handle)
        Try
            Using g As Graphics = Graphics.FromHdc(hDC)
                Using backColorBrush As New SolidBrush(Me.theNonClientPaddingColor)
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

    Private Sub HorizontalScrollbar_ValueChanged(ByVal sender As Object, ByVal e As ScrollValueEventArgs) Handles CustomHorizontalScrollbar.ValueChanged
        Me.UpdateScrolling(e.Value, 0)
    End Sub

    Private Sub VerticalScrollBar_ValueChanged(ByVal sender As Object, ByVal e As ScrollValueEventArgs) Handles CustomVerticalScrollBar.ValueChanged
        Me.UpdateScrolling(0, e.Value)
    End Sub

#End Region

#Region "Private Methods"

    Private Sub UpdateNonClientPadding()
        Dim left As Integer = 0
        Dim top As Integer = 0
        Dim right As Integer = 0
        Dim bottom As Integer = 0
        'TEST: Use 2 for testing. Use 0 for final.
        'Dim left As Integer = 2
        'Dim top As Integer = 2
        'Dim right As Integer = 2
        'Dim bottom As Integer = 2

        Dim contentWidth As Integer = Me.Columns.GetColumnsWidth(DataGridViewElementStates.None)
        Dim clientWidth As Integer = Me.ClientRectangle.Width
        'If Me.CustomVerticalScrollBar.Visible Then
        '	clientWidth += ScrollBarEx.Consts.ScrollBarSize
        'End If
        If contentWidth > clientWidth Then
            bottom += ScrollBarEx.Consts.ScrollBarSize
        End If
        Dim rowCount As Integer = Me.RowCount
        If rowCount > 0 Then
            'Dim contentHeight As Integer = rowCount * Me.Rows(0).Height
            Dim contentHeight As Integer = Me.Rows.GetRowsHeight(DataGridViewElementStates.None)
            If Me.ColumnHeadersVisible Then
                contentHeight += Me.ColumnHeadersHeight
            End If
            If contentHeight > Me.ClientRectangle.Height Then
                right += ScrollBarEx.Consts.ScrollBarSize
            End If
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

            If Me.RowCount > 0 Then
                Dim rowHeight As Integer = Me.Rows(0).Height
                Dim rowIndex As Integer = CInt(upOrDownValue / rowHeight)
                If rowIndex > Me.RowCount - 1 Then
                    rowIndex = Me.RowCount - 1
                End If
                Me.FirstDisplayedScrollingRowIndex = rowIndex
            End If
            Me.Invalidate()

            Me.theScrollingIsActive = False
        End If
    End Sub

    Private Sub UpdateScrollbars()
        Me.UpdateHorizontalScrollbar()
        Me.UpdateVerticalScrollbar()

        If Me.CustomHorizontalScrollbar.Visible AndAlso Me.CustomVerticalScrollBar.Visible Then
            Me.CustomHorizontalScrollbar.Size = New System.Drawing.Size(Me.Width - ScrollBarEx.Consts.ScrollBarSize, ScrollBarEx.Consts.ScrollBarSize)
            Me.CustomVerticalScrollBar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.Height - ScrollBarEx.Consts.ScrollBarSize)
        End If
    End Sub

    Private Sub UpdateHorizontalScrollbar()
        'NOTE: Parent can be Nothing on exiting. Prevent the exception with this check.
        If Not Me.theScrollingIsActive AndAlso Me.Parent IsNot Nothing Then
            Dim contentWidth As Integer = Me.Columns.GetColumnsWidth(DataGridViewElementStates.None)
            Dim clientWidth As Integer = Me.ClientRectangle.Width
            If contentWidth > clientWidth Then
                Me.theScrollingIsActive = True

                Me.CustomHorizontalScrollbar.Minimum = 0
                Me.CustomHorizontalScrollbar.Maximum = contentWidth
                Me.CustomHorizontalScrollbar.Value = Me.HorizontalScrollingOffset
                Me.CustomHorizontalScrollbar.ViewSize = clientWidth
                Me.CustomHorizontalScrollbar.SmallChange = 100
                Me.CustomHorizontalScrollbar.LargeChange = clientWidth

                Me.CustomHorizontalScrollbar.Show()

                Me.CustomHorizontalScrollbar.Size = New System.Drawing.Size(Me.Width, ScrollBarEx.Consts.ScrollBarSize)
                'NOTE: Assign to Parent so it can draw over non-client area.
                Me.CustomHorizontalScrollbar.Parent = Me.Parent
                Me.CustomHorizontalScrollbar.BringToFront()
                'NOTE: Point is relative to Me.ClientRectangle.
                Dim aPoint As New Point(Me.ClientRectangle.Left - Me.NonClientPadding.Left, Me.ClientRectangle.Height + Me.NonClientPadding.Bottom - ScrollBarEx.Consts.ScrollBarSize)
                'NOTE: Location must be relative to Parent.
                aPoint = Me.PointToScreen(aPoint)
                aPoint = Me.Parent.PointToClient(aPoint)
                Me.CustomHorizontalScrollbar.Location = aPoint

                Me.theScrollingIsActive = False
            Else
                Me.CustomHorizontalScrollbar.Hide()
            End If
        End If
    End Sub

    Private Sub UpdateVerticalScrollbar()
        'NOTE: Parent can be Nothing on exiting. Prevent the exception with this check.
        If Not Me.theScrollingIsActive AndAlso Me.Parent IsNot Nothing Then
            Dim rowCount As Integer = Me.RowCount
            If rowCount > 0 Then
                Dim rowHeight As Integer = Me.Rows(0).Height
                'Dim contentHeight As Integer = rowCount * rowHeight
                Dim contentHeight As Integer = Me.Rows.GetRowsHeight(DataGridViewElementStates.None)
                Dim clientHeight As Integer = Me.ClientRectangle.Height
                If Me.ColumnHeadersVisible Then
                    contentHeight += Me.ColumnHeadersHeight
                    clientHeight -= Me.ColumnHeadersHeight
                End If
                If contentHeight > Me.ClientRectangle.Height Then
                    Me.theScrollingIsActive = True

                    Me.CustomVerticalScrollBar.Minimum = 0
                    Me.CustomVerticalScrollBar.Maximum = contentHeight
                    Me.CustomVerticalScrollBar.Value = Me.VerticalScrollingOffset
                    Me.CustomVerticalScrollBar.ViewSize = clientHeight
                    Me.CustomVerticalScrollBar.SmallChange = rowHeight
                    Me.CustomVerticalScrollBar.LargeChange = clientHeight - rowHeight * 2

                    'NOTE: Assign to Parent so it can draw over non-client area.
                    Me.CustomVerticalScrollBar.Parent = Me.Parent
                    Me.CustomVerticalScrollBar.BringToFront()
                    'NOTE: Point is relative to Me.ClientRectangle.
                    Dim aPoint As New Point(Me.ClientRectangle.Width + Me.NonClientPadding.Right - ScrollBarEx.Consts.ScrollBarSize, Me.ClientRectangle.Top - Me.NonClientPadding.Top)
                    'NOTE: Location must be relative to Parent.
                    aPoint = Me.PointToScreen(aPoint)
                    aPoint = Me.CustomVerticalScrollBar.Parent.PointToClient(aPoint)
                    Me.CustomVerticalScrollBar.Location = aPoint
                    Me.CustomVerticalScrollBar.Size = New System.Drawing.Size(ScrollBarEx.Consts.ScrollBarSize, Me.Height)

                    Me.CustomVerticalScrollBar.Show()

                    Me.theScrollingIsActive = False
                Else
                    Me.CustomVerticalScrollBar.Hide()
                End If
            Else
                Me.CustomVerticalScrollBar.Hide()
            End If
        End If
    End Sub

#End Region

#Region "Data"

    Private theScrollBars As ScrollBars

    Private NonClientPadding As Padding
    Private theNonClientPaddingColor As Color

    Private WithEvents CustomHorizontalScrollbar As ScrollBarEx
    Private WithEvents CustomVerticalScrollBar As ScrollBarEx
    Private theControlHasShown As Boolean
    Private theScrollingIsActive As Boolean

    Private theCurrentCellIsChangingBecauseOfMe As Boolean
    Private theSelectionIsChangingBecauseOfMe As Boolean
    Private theWidgetTempOfAllowUserToAddRows As Boolean

    Private theCellWherePointerIs As DataGridViewCell

#End Region

End Class
