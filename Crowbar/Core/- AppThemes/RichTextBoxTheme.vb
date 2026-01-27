Imports System.Xml.Serialization

Public Class RichTextBoxTheme
    Inherits WidgetTheme

#Region "Create and Destroy"

    Public Sub New()
        MyBase.New()

        Me.theEnabledBackColor = New XmlColor(Color.FromArgb(&HFF1E1E1E))
        Me.theDisabledBackColor = New XmlColor(Color.FromArgb(&HFF2D2D2D))

    End Sub

#End Region

#Region "Init and Free"

    'Public Sub Init()
    'End Sub

    'Private Sub Free()
    'End Sub

#End Region

#Region "Properties"

#End Region

#Region "Methods"

#End Region

#Region "Events"

#End Region

#Region "Private Methods"

#End Region

#Region "Data"

#End Region

End Class
