Imports System.Xml.Serialization

Public Class CheckBoxTheme
    Inherits WidgetTheme

#Region "Create and Destroy"

    Public Sub New()
        MyBase.New()

        Me.theUntickedEnabledForeColor = New XmlColor(Color.FromArgb(&HF1F1F1))
        Me.theUntickedEnabledBackColor = New XmlColor(Color.FromArgb(&H4B4B4B))
        Me.theUntickedEnabledBorderColor = New XmlColor(Color.FromArgb(&HF1F1F1))

        Me.theUntickedDisabledForeColor = New XmlColor(Color.FromArgb(&HF1F1F1))
        Me.theUntickedDisabledBackColor = New XmlColor(Color.FromArgb(&H4B4B4B))
        Me.theUntickedDisabledBorderColor = New XmlColor(Color.FromArgb(&HF1F1F1))

        Me.theUntickedFocusForeColor = New XmlColor("WindowColorizationColor")
        Me.theUntickedFocusBackColor = New XmlColor(Color.FromArgb(&H4B4B4B))
        Me.theUntickedFocusBorderColor = New XmlColor(Color.FromArgb(&HF1F1F1))

        Me.theTickedEnabledForeColor = New XmlColor(Color.FromArgb(&HF1F1F1))
        Me.theTickedEnabledBackColor = New XmlColor(Color.FromArgb(&H4B4B4B))
        Me.theTickedEnabledBorderColor = New XmlColor(Color.FromArgb(&HF1F1F1))

        Me.theTickedDisabledForeColor = New XmlColor(Color.FromArgb(&HF1F1F1))
        Me.theTickedDisabledBackColor = New XmlColor(Color.FromArgb(&H4B4B4B))
        Me.theTickedDisabledBorderColor = New XmlColor(Color.FromArgb(&HF1F1F1))

        Me.theTickedFocusForeColor = New XmlColor("WindowColorizationColor")
        Me.theTickedFocusBackColor = New XmlColor(Color.FromArgb(&H4B4B4B))
        Me.theTickedFocusBorderColor = New XmlColor(Color.FromArgb(&HF1F1F1))

    End Sub

#End Region

#Region "Init and Free"

    'Public Sub Init()
    'End Sub

    'Private Sub Free()
    'End Sub

#End Region

#Region "Properties"

    Public Property UntickedEnabledForeColor As XmlColor
        Get
            Return Me.theUntickedEnabledForeColor
        End Get
        Set(value As XmlColor)
            Me.theUntickedEnabledForeColor = value
        End Set
    End Property

    Public Property UntickedEnabledBackColor As XmlColor
        Get
            Return Me.theUntickedEnabledBackColor
        End Get
        Set(value As XmlColor)
            Me.theUntickedEnabledBackColor = value
        End Set
    End Property

    Public Property UntickedEnabledBorderColor As XmlColor
        Get
            Return Me.theUntickedEnabledBorderColor
        End Get
        Set(value As XmlColor)
            Me.theUntickedEnabledBorderColor = value
        End Set
    End Property

    Public Property UntickedDisabledForeColor As XmlColor
        Get
            Return Me.theUntickedDisabledForeColor
        End Get
        Set(value As XmlColor)
            Me.theUntickedDisabledForeColor = value
        End Set
    End Property

    Public Property UntickedDisabledBackColor As XmlColor
        Get
            Return Me.theUntickedDisabledBackColor
        End Get
        Set(value As XmlColor)
            Me.theUntickedDisabledBackColor = value
        End Set
    End Property

    Public Property UntickedDisabledBorderColor As XmlColor
        Get
            Return Me.theUntickedDisabledBorderColor
        End Get
        Set(value As XmlColor)
            Me.theUntickedDisabledBorderColor = value
        End Set
    End Property

    Public Property UntickedFocusForeColor As XmlColor
        Get
            Return Me.theUntickedFocusForeColor
        End Get
        Set(value As XmlColor)
            Me.theUntickedFocusForeColor = value
        End Set
    End Property

    Public Property UntickedFocusBackColor As XmlColor
        Get
            Return Me.theUntickedFocusBackColor
        End Get
        Set(value As XmlColor)
            Me.theUntickedFocusBackColor = value
        End Set
    End Property

    Public Property UntickedFocusBorderColor As XmlColor
        Get
            Return Me.theUntickedFocusBorderColor
        End Get
        Set(value As XmlColor)
            Me.theUntickedFocusBorderColor = value
        End Set
    End Property

    Public Property TickedEnabledForeColor As XmlColor
        Get
            Return Me.theTickedEnabledForeColor
        End Get
        Set(value As XmlColor)
            Me.theTickedEnabledForeColor = value
        End Set
    End Property

    Public Property TickedEnabledBackColor As XmlColor
        Get
            Return Me.theTickedEnabledBackColor
        End Get
        Set(value As XmlColor)
            Me.theTickedEnabledBackColor = value
        End Set
    End Property

    Public Property TickedEnabledBorderColor As XmlColor
        Get
            Return Me.theTickedEnabledBorderColor
        End Get
        Set(value As XmlColor)
            Me.theTickedEnabledBorderColor = value
        End Set
    End Property

    Public Property TickedDisabledForeColor As XmlColor
        Get
            Return Me.theTickedDisabledForeColor
        End Get
        Set(value As XmlColor)
            Me.theTickedDisabledForeColor = value
        End Set
    End Property

    Public Property TickedDisabledBackColor As XmlColor
        Get
            Return Me.theTickedDisabledBackColor
        End Get
        Set(value As XmlColor)
            Me.theTickedDisabledBackColor = value
        End Set
    End Property

    Public Property TickedDisabledBorderColor As XmlColor
        Get
            Return Me.theTickedDisabledBorderColor
        End Get
        Set(value As XmlColor)
            Me.theTickedDisabledBorderColor = value
        End Set
    End Property

    Public Property TickedFocusForeColor As XmlColor
        Get
            Return Me.theTickedFocusForeColor
        End Get
        Set(value As XmlColor)
            Me.theTickedFocusForeColor = value
        End Set
    End Property

    Public Property TickedFocusBackColor As XmlColor
        Get
            Return Me.theTickedFocusBackColor
        End Get
        Set(value As XmlColor)
            Me.theTickedFocusBackColor = value
        End Set
    End Property

    Public Property TickedFocusBorderColor As XmlColor
        Get
            Return Me.theTickedFocusBorderColor
        End Get
        Set(value As XmlColor)
            Me.theTickedFocusBorderColor = value
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

    Private theUntickedEnabledForeColor As XmlColor
    Private theUntickedEnabledBackColor As XmlColor
    Private theUntickedEnabledBorderColor As XmlColor

    Private theUntickedDisabledForeColor As XmlColor
    Private theUntickedDisabledBackColor As XmlColor
    Private theUntickedDisabledBorderColor As XmlColor

    Private theUntickedFocusForeColor As XmlColor
    Private theUntickedFocusBackColor As XmlColor
    Private theUntickedFocusBorderColor As XmlColor

    Private theTickedEnabledForeColor As XmlColor
    Private theTickedEnabledBackColor As XmlColor
    Private theTickedEnabledBorderColor As XmlColor

    Private theTickedDisabledForeColor As XmlColor
    Private theTickedDisabledBackColor As XmlColor
    Private theTickedDisabledBorderColor As XmlColor

    Private theTickedFocusForeColor As XmlColor
    Private theTickedFocusBackColor As XmlColor
    Private theTickedFocusBorderColor As XmlColor

#End Region

End Class
