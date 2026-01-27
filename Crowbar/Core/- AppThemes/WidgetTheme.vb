Imports System.Xml.Serialization

Public Class WidgetTheme

#Region "Create and Destroy"

    Public Sub New()
        'MyBase.New()

        Me.theEnabledForeColor = New XmlColor(Color.FromArgb(&HFFF1F1F1))
        Me.theEnabledBackColor = New XmlColor(Color.FromArgb(&HFF2D2D2D))
        Me.theEnabledBorderColor = New XmlColor(Color.FromArgb(&HFF555555))

        Me.theDisabledForeColor = New XmlColor(Color.FromArgb(&HFF808080))
        Me.theDisabledBackColor = New XmlColor(Color.FromArgb(&HFF2D2D2D))
        Me.theDisabledBorderColor = New XmlColor(Color.FromArgb(&HFF454545))

        Me.theSelectedForeColor = New XmlColor(Color.FromArgb(&HFFF1F1F1))
        Me.theSelectedBackColor = New XmlColor("WindowColorizationColor")
        Me.theSelectedBorderColor = New XmlColor(Color.FromArgb(&HFFF1F1F1))

        Me.theFocusForeColor = New XmlColor(Color.FromArgb(&HFFF1F1F1))
        Me.theFocusBackColor = New XmlColor(Color.FromArgb(&HFF4B4B4B))
        'Me.theFocusTopBackColor = New XmlColor("WindowColorizationColor")
        'Me.theFocusBottomBackColor = New XmlColor(Color.FromArgb(&HFF4B4B4B))
        Me.theFocusBorderColor = New XmlColor("WindowColorizationColor")

        'Me.theTextEnabledForeColor = New XmlColor(Color.FromArgb(&HFFF1F1F1))
        'Me.theTextEnabledBackColor = New XmlColor(Color.FromArgb(&HFF4B4B4B))

        'Me.theTextDisabledForeColor = New XmlColor(Color.FromArgb(&HFF808080))
        'Me.theTextDisabledBackColor = New XmlColor(Color.FromArgb(&HFF4B4B4B))

        'Me.theTextFocusForeColor = New XmlColor(Color.FromArgb(&HFFF1F1F1))
        'Me.theTextFocusBackColor = New XmlColor(Color.FromArgb(&HFF4B4B4B))

        'Me.theTextSelectedForeColor = New XmlColor(Color.FromArgb(&HFFF1F1F1))
        'Me.theTextSelectedBackColor = New XmlColor("WindowColorizationColor")
    End Sub

#End Region

#Region "Init and Free"

    'Public Sub Init()
    'End Sub

    'Private Sub Free()
    'End Sub

#End Region

#Region "Properties"

    Public Property EnabledForeColor As XmlColor
        Get
            Return Me.theEnabledForeColor
        End Get
        Set(value As XmlColor)
            Me.theEnabledForeColor = value
        End Set
    End Property

    Public Property EnabledBackColor As XmlColor
        Get
            Return Me.theEnabledBackColor
        End Get
        Set(value As XmlColor)
            Me.theEnabledBackColor = value
        End Set
    End Property

    Public Property EnabledBorderColor As XmlColor
        Get
            Return Me.theEnabledBorderColor
        End Get
        Set(value As XmlColor)
            Me.theEnabledBorderColor = value
        End Set
    End Property

    Public Property DisabledForeColor As XmlColor
        Get
            Return Me.theDisabledForeColor
        End Get
        Set(value As XmlColor)
            Me.theDisabledForeColor = value
        End Set
    End Property

    Public Property DisabledBackColor As XmlColor
        Get
            Return Me.theDisabledBackColor
        End Get
        Set(value As XmlColor)
            Me.theDisabledBackColor = value
        End Set
    End Property

    Public Property DisabledBorderColor As XmlColor
        Get
            Return Me.theDisabledBorderColor
        End Get
        Set(value As XmlColor)
            Me.theDisabledBorderColor = value
        End Set
    End Property

    Public Property SelectedForeColor As XmlColor
        Get
            Return Me.theSelectedForeColor
        End Get
        Set(value As XmlColor)
            Me.theSelectedForeColor = value
        End Set
    End Property

    Public Property SelectedBackColor As XmlColor
        Get
            Return Me.theSelectedBackColor
        End Get
        Set(value As XmlColor)
            Me.theSelectedBackColor = value
        End Set
    End Property

    Public Property SelectedBorderColor As XmlColor
        Get
            Return Me.theSelectedBorderColor
        End Get
        Set(value As XmlColor)
            Me.theSelectedBorderColor = value
        End Set
    End Property

    Public Property FocusForeColor As XmlColor
        Get
            Return Me.theFocusForeColor
        End Get
        Set(value As XmlColor)
            Me.theFocusForeColor = value
        End Set
    End Property

    Public Property FocusBackColor As XmlColor
        Get
            Return Me.theFocusBackColor
        End Get
        Set(value As XmlColor)
            Me.theFocusBackColor = value
        End Set
    End Property

    Public Property FocusBorderColor As XmlColor
        Get
            Return Me.theFocusBorderColor
        End Get
        Set(value As XmlColor)
            Me.theFocusBorderColor = value
        End Set
    End Property

    'Public Property TextEnabledForeColor() As XmlColor
    '    Get
    '        Return Me.theTextEnabledForeColor
    '    End Get
    '    Set(ByVal value As XmlColor)
    '        Me.theTextEnabledForeColor = value
    '    End Set
    'End Property

    'Public Property TextEnabledBackColor As XmlColor
    '    Get
    '        Return Me.theTextEnabledBackColor
    '    End Get
    '    Set(value As XmlColor)
    '        Me.theTextEnabledBackColor = value
    '    End Set
    'End Property

    ''Public Property TextEnabledBorderColor As XmlColor
    ''    Get
    ''        Return Me.theTextEnabledBorderColor
    ''    End Get
    ''    Set(value As XmlColor)
    ''        Me.theTextEnabledBorderColor = value
    ''    End Set
    ''End Property

    'Public Property TextDisabledForeColor As XmlColor
    '    Get
    '        Return Me.theTextDisabledForeColor
    '    End Get
    '    Set(value As XmlColor)
    '        Me.theTextDisabledForeColor = value
    '    End Set
    'End Property

    'Public Property TextDisabledBackColor As XmlColor
    '    Get
    '        Return Me.theTextDisabledBackColor
    '    End Get
    '    Set(value As XmlColor)
    '        Me.theTextDisabledBackColor = value
    '    End Set
    'End Property

    ''Public Property TextDisabledBorderColor As XmlColor
    ''    Get
    ''        Return Me.theTextDisabledBorderColor
    ''    End Get
    ''    Set(value As XmlColor)
    ''        Me.theTextDisabledBorderColor = value
    ''    End Set
    ''End Property

    'Public Property TextFocusForeColor As XmlColor
    '    Get
    '        Return Me.theTextFocusForeColor
    '    End Get
    '    Set(value As XmlColor)
    '        Me.theTextFocusForeColor = value
    '    End Set
    'End Property

    'Public Property TextFocusBackColor As XmlColor
    '    Get
    '        Return Me.theTextFocusBackColor
    '    End Get
    '    Set(value As XmlColor)
    '        Me.theTextFocusBackColor = value
    '    End Set
    'End Property

    ''Public Property TextFocusBorderColor As XmlColor
    ''    Get
    ''        Return Me.theTextFocusBorderColor
    ''    End Get
    ''    Set(value As XmlColor)
    ''        Me.theTextFocusBorderColor = value
    ''    End Set
    ''End Property

    'Public Property TextSelectedForeColor As XmlColor
    '    Get
    '        Return Me.theTextSelectedForeColor
    '    End Get
    '    Set(value As XmlColor)
    '        Me.theTextSelectedForeColor = value
    '    End Set
    'End Property

    'Public Property TextSelectedBackColor As XmlColor
    '    Get
    '        Return Me.theTextSelectedBackColor
    '    End Get
    '    Set(value As XmlColor)
    '        Me.theTextSelectedBackColor = value
    '    End Set
    'End Property

    ''Public Property TextSelectedBorderColor As XmlColor
    ''    Get
    ''        Return Me.theTextSelectedBorderColor
    ''    End Get
    ''    Set(value As XmlColor)
    ''        Me.theTextSelectedBorderColor = value
    ''    End Set
    ''End Property

#End Region

#Region "Methods"

#End Region

#Region "Events"


#End Region

#Region "Private Methods"

#End Region

#Region "Data"

    Protected theEnabledForeColor As XmlColor
    Protected theEnabledBackColor As XmlColor
    Protected theEnabledBorderColor As XmlColor

    Protected theDisabledForeColor As XmlColor
    Protected theDisabledBackColor As XmlColor
    Protected theDisabledBorderColor As XmlColor

    Protected theSelectedForeColor As XmlColor
    Protected theSelectedBackColor As XmlColor
    Protected theSelectedBorderColor As XmlColor

    Protected theFocusForeColor As XmlColor
    Protected theFocusBackColor As XmlColor
    Protected theFocusBorderColor As XmlColor

    'Protected theTextEnabledForeColor As XmlColor
    'Protected theTextEnabledBackColor As XmlColor
    ''Protected theTextEnabledBorderColor As XmlColor

    'Protected theTextDisabledForeColor As XmlColor
    'Protected theTextDisabledBackColor As XmlColor
    ''Protected theTextDisabledBorderColor As XmlColor

    'Protected theTextFocusForeColor As XmlColor
    'Protected theTextFocusBackColor As XmlColor
    ''Protected theTextFocusBorderColor As XmlColor

    'Protected theTextSelectedForeColor As XmlColor
    'Protected theTextSelectedBackColor As XmlColor
    ''Protected theTextSelectedBorderColor As XmlColor

#End Region

End Class
