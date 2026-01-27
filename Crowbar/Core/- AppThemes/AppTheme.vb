Imports System.Xml.Serialization

Public Class AppTheme

#Region "Create and Destroy"

    Public Sub New()
        'MyBase.New()

        Me.theName = "Windows Default"
    End Sub

#End Region

#Region "Init and Free"

    'Public Sub Init()
    'End Sub

    'Private Sub Free()
    'End Sub

#End Region

#Region "Properties"

    Public Property Name() As String
        Get
            Return Me.theName
        End Get
        Set(ByVal value As String)
            Me.theName = value
        End Set
    End Property

    Public Property GlobalTheme() As GlobalTheme
        Get
            Return Me.theGlobalTheme
        End Get
        Set(ByVal value As GlobalTheme)
            Me.theGlobalTheme = value
        End Set
    End Property

    Public Property ButtonTheme() As ButtonTheme
        Get
            Return Me.theButtonTheme
        End Get
        Set(ByVal value As ButtonTheme)
            Me.theButtonTheme = value
        End Set
    End Property

    Public Property CheckboxTheme() As WidgetTheme
        Get
            Return Me.theCheckboxTheme
        End Get
        Set(ByVal value As WidgetTheme)
            Me.theCheckboxTheme = value
        End Set
    End Property

    Public Property ComboUserControlTheme() As ComboUserControlTheme
        Get
            Return Me.theComboUserControlTheme
        End Get
        Set(ByVal value As ComboUserControlTheme)
            Me.theComboUserControlTheme = value
        End Set
    End Property

    Public Property DataGridViewTheme() As DataGridViewTheme
        Get
            Return Me.theDataGridViewTheme
        End Get
        Set(ByVal value As DataGridViewTheme)
            Me.theDataGridViewTheme = value
        End Set
    End Property

    Public Property DateTimeTextBoxTheme() As WidgetTheme
        Get
            Return Me.theDateTimeTextBoxTheme
        End Get
        Set(ByVal value As WidgetTheme)
            Me.theDateTimeTextBoxTheme = value
        End Set
    End Property

    Public Property GroupBoxTheme() As GroupBoxTheme
        Get
            Return Me.theGroupBoxTheme
        End Get
        Set(ByVal value As GroupBoxTheme)
            Me.theGroupBoxTheme = value
        End Set
    End Property

    Public Property ListBoxTheme() As WidgetTheme
        Get
            Return Me.theListBoxTheme
        End Get
        Set(ByVal value As WidgetTheme)
            Me.theListBoxTheme = value
        End Set
    End Property

    Public Property ListViewTheme() As WidgetTheme
        Get
            Return Me.theListViewTheme
        End Get
        Set(ByVal value As WidgetTheme)
            Me.theListViewTheme = value
        End Set
    End Property

    Public Property PanelTheme() As PanelTheme
        Get
            Return Me.thePanelTheme
        End Get
        Set(ByVal value As PanelTheme)
            Me.thePanelTheme = value
        End Set
    End Property

    Public Property ProgressBarTheme() As WidgetTheme
        Get
            Return Me.theProgressBarTheme
        End Get
        Set(ByVal value As WidgetTheme)
            Me.theProgressBarTheme = value
        End Set
    End Property

    Public Property RadioButtonTheme() As WidgetTheme
        Get
            Return Me.theRadioButtonTheme
        End Get
        Set(ByVal value As WidgetTheme)
            Me.theRadioButtonTheme = value
        End Set
    End Property

    Public Property RichTextBoxTheme() As RichTextBoxTheme
        Get
            Return Me.theRichTextBoxTheme
        End Get
        Set(ByVal value As RichTextBoxTheme)
            Me.theRichTextBoxTheme = value
        End Set
    End Property

    Public Property ScrollBarTheme() As WidgetTheme
        Get
            Return Me.theScrollBarTheme
        End Get
        Set(ByVal value As WidgetTheme)
            Me.theScrollBarTheme = value
        End Set
    End Property

    Public Property SplitContainerTheme() As WidgetTheme
        Get
            Return Me.theSplitContainerTheme
        End Get
        Set(ByVal value As WidgetTheme)
            Me.theSplitContainerTheme = value
        End Set
    End Property

    Public Property TabControlTheme() As TabControlTheme
        Get
            Return Me.theTabControlTheme
        End Get
        Set(ByVal value As TabControlTheme)
            Me.theTabControlTheme = value
        End Set
    End Property

    Public Property TabPageTheme() As WidgetTheme
        Get
            Return Me.theTabPageTheme
        End Get
        Set(ByVal value As WidgetTheme)
            Me.theTabPageTheme = value
        End Set
    End Property

    Public Property TabScrollerTheme() As WidgetTheme
        Get
            Return Me.theTabScrollerTheme
        End Get
        Set(ByVal value As WidgetTheme)
            Me.theTabScrollerTheme = value
        End Set
    End Property

    Public Property TreeViewTheme() As WidgetTheme
        Get
            Return Me.theTreeViewTheme
        End Get
        Set(ByVal value As WidgetTheme)
            Me.theTreeViewTheme = value
        End Set
    End Property

#End Region

#Region "Methods"

#End Region

#Region "Events"

#End Region

#Region "Private Methods"

#End Region

#Region "Data"

    Private theName As String
    Private theGlobalTheme As GlobalTheme

    Private theButtonTheme As ButtonTheme
    Private theCheckboxTheme As WidgetTheme
    Private theComboUserControlTheme As ComboUserControlTheme
    Private theDataGridViewTheme As DataGridViewTheme
    Private theDateTimeTextBoxTheme As WidgetTheme
    Private theGroupBoxTheme As GroupBoxTheme
    Private theListBoxTheme As WidgetTheme
    Private theListViewTheme As WidgetTheme
    Private thePanelTheme As PanelTheme
    Private theProgressBarTheme As WidgetTheme
    Private theRadioButtonTheme As WidgetTheme
    Private theRichTextBoxTheme As RichTextBoxTheme
    Private theScrollBarTheme As WidgetTheme
    Private theSplitContainerTheme As WidgetTheme
    Private theTabControlTheme As TabControlTheme
    Private theTabPageTheme As WidgetTheme
    Private theTabScrollerTheme As WidgetTheme
    Private theTreeViewTheme As WidgetTheme

#End Region

End Class
