Imports System.IO
Imports System.Reflection

Public Class MainProcess

    'Private basicTemplate As IBasicParams
    ' Private advTemplate As IAdvancedParams

    'Private m_SettingsFileName As String = "ParamFileEditorSettings.xml"
    Shared m_TemplateFileName As String
    Shared m_TemplateFilePath As String
    'Private m_modsUpdate As UpdateModsTable
    'Private m_mainProcess As MainProcess
    Shared m_BaseLineParams As Params
    'Const DEF_TEMPLATE_LABEL_TEXT As String = "Currently Loaded Template: "
    Const DEF_TEMPLATE_FILENAME As String = "sequest_N14_NE.params"
    Shared ReadOnly DEF_TEMPLATE_FILEPATH As String = Path.GetDirectoryName(Assembly.GetEntryAssembly.Location)

    'Public Shared mySettings As Settings

    Public Shared ReadOnly Property BaseLineParamSet As Params
        Get
            Return m_BaseLineParams
        End Get
    End Property

    Public Shared ReadOnly Property TemplateFileName As String
        Get
            Return Path.Combine(m_TemplateFilePath, Path.GetFileName(m_TemplateFileName))
        End Get
    End Property

    Public Sub New()

        m_TemplateFileName = Path.Combine(DEF_TEMPLATE_FILEPATH, DEF_TEMPLATE_FILENAME)
        m_BaseLineParams = New Params
        m_TemplateFilePath = DEF_TEMPLATE_FILEPATH


        With m_BaseLineParams
            .FileName = DEF_TEMPLATE_FILENAME
            .LoadTemplate(m_TemplateFileName)
        End With

    End Sub

    Public Sub New(templateFilePath As String)
        m_TemplateFileName = templateFilePath
        m_BaseLineParams = New Params
        m_TemplateFilePath = Path.GetDirectoryName(templateFilePath)

        With m_BaseLineParams
            .FileName = Path.GetFileName(templateFilePath)
            .LoadTemplate(templateFilePath)

        End With
    End Sub

End Class
