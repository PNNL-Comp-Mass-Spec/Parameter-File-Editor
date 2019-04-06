
Imports System.Collections.Generic

Public Interface IRetrieveParams
    Function GetParam(section As String, item As String, attributeName As String) As String
    Function GetParam(section As String, item As String) As String
    Function GetParam(item As String) As String
    Sub SetParam(section As String, name As String, Value As String)
    Sub SetParam(name As String, Value As String)
    Sub SetSection(name As String)
End Interface

Public Class clsRetrieveParams
    Implements IRetrieveParams

    Private m_iniFileReader As IniFileReader

    ''' <summary>
    ''' Default section name
    ''' </summary>
    Private m_defaultSection As String = ""

    Public Sub New(Optional iniFilePath As String = "", Optional IsCaseSensitive As Boolean = False)
        If iniFilePath <> "" Then
            Me.IniFilePath = iniFilePath
            LoadSettings(IsCaseSensitive)
        End If
    End Sub

    Public Property IniFilePath As String = ""

    Public Function LoadSettings(Optional IsCaseSensitive As Boolean = False) As Boolean
        m_iniFileReader = New IniFileReader(IniFilePath, IsCaseSensitive)
        Return Not (m_iniFileReader Is Nothing)
    End Function

    Public Sub SaveSettings()
        m_iniFileReader.OutputFilename = IniFilePath
        m_iniFileReader.Save()
    End Sub
    Public Function LoadSettings(settingsFilePath As String) As Boolean
        IniFilePath = settingsFilePath
        Return LoadSettings()
    End Function

    Public Function GetParam(item As String) As String Implements IRetrieveParams.GetParam
        Return m_iniFileReader.GetIniValue(m_defaultSection, item)
    End Function

    Public Function GetParam(section As String, item As String) As String Implements IRetrieveParams.GetParam
        Dim s As String = m_iniFileReader.GetIniValue(section, item)
        If s Is Nothing Then Throw New Exception("No ini value for parameter '" & item & "'")
        Return s
    End Function

    Public Function GetParam(section As String, item As String, attributeName As String) As String Implements IRetrieveParams.GetParam
        Dim s As String = m_iniFileReader.GetCustomIniAttribute(section, item, attributeName)
        If s Is Nothing Then Throw New Exception("No custom ini value for parameter '" & item & "'")
        Return s
    End Function

    Public Sub SetParam(name As String, Value As String) Implements IRetrieveParams.SetParam
        m_iniFileReader.SetIniValue(m_defaultSection, name, Value)
    End Sub

    Public Sub SetParam(section As String, name As String, Value As String) Implements IRetrieveParams.SetParam
        m_iniFileReader.SetIniValue(section, name, Value)
    End Sub

    Public Sub SetSection(name As String) Implements IRetrieveParams.SetSection
        m_defaultSection = name
    End Sub

    Public Function GetAllKeysInSection(section As String) As List(Of String)
        Dim keyNames As List(Of String)
        keyNames = m_iniFileReader.AllKeysInSection(section)
        If keyNames Is Nothing Then Throw New Exception("No Keys in section '" & section & "'")
        Return keyNames
    End Function

End Class

