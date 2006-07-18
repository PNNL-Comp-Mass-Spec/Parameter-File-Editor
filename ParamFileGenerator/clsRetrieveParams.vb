
    Public Interface IRetrieveParams
        Function GetParam(ByVal Section As String, ByVal Item As String, ByVal AttributeName As String) As String
        Function GetParam(ByVal Section As String, ByVal Item As String) As String
        Function GetParam(ByVal Item As String) As String
        Sub SetParam(ByVal Section As String, ByVal Name As String, ByVal Value As String)
        Sub SetParam(ByVal Name As String, ByVal Value As String)
        Sub SetSection(ByVal Name As String)
    End Interface

    Public Class clsRetrieveParams
        Implements IRetrieveParams

        'ini file reader
        Private m_IniFilePath As String = ""
        Private m_iniFileReader As IniFileReader

        'default section name
        Private m_defaultSection As String = ""

    Public Sub New(Optional ByVal iniFilePath As String = "", Optional ByVal IsCaseSensitive As Boolean = False)
        If iniFilePath <> "" Then
            m_IniFilePath = iniFilePath
            LoadSettings(IsCaseSensitive)
        End If
    End Sub

    Public Property IniFilePath() As String
        Get
            Return m_IniFilePath
        End Get
        Set(ByVal Value As String)
            m_IniFilePath = Value
        End Set
    End Property

    Public Function LoadSettings(Optional ByVal IsCaseSensitive As Boolean = False) As Boolean
        m_iniFileReader = New IniFileReader(m_IniFilePath, IsCaseSensitive)
        Return Not (m_iniFileReader Is Nothing)
    End Function

    Public Sub SaveSettings()
        m_iniFileReader.OutputFilename = m_IniFilePath
        m_iniFileReader.Save()
    End Sub
    Public Function LoadSettings(ByVal IniFilePath As String) As Boolean
        m_IniFilePath = IniFilePath
        Return LoadSettings()
    End Function

    Public Function GetParam(ByVal Item As String) As String Implements IRetrieveParams.GetParam
        Return m_iniFileReader.GetIniValue(m_defaultSection, Item)
    End Function

    Public Function GetParam(ByVal Section As String, ByVal Item As String) As String Implements IRetrieveParams.GetParam
        Dim s As String = m_iniFileReader.GetIniValue(Section, Item)
        If s Is Nothing Then Throw New Exception("No ini value for parameter '" & Item & "'")
        Return s
    End Function

    Public Function GetParam(ByVal Section As String, ByVal Item As String, ByVal AttributeName As String) As String Implements IRetrieveParams.GetParam
        Dim s As String = m_iniFileReader.GetCustomIniAttribute(Section, Item, AttributeName)
        If s Is Nothing Then Throw New Exception("No custom ini value for parameter '" & Item & "'")
        Return s
    End Function

    Public Sub SetParam(ByVal Name As String, ByVal Value As String) Implements IRetrieveParams.SetParam
        m_iniFileReader.SetIniValue(m_defaultSection, Name, Value)
    End Sub

    Public Sub SetParam(ByVal Section As String, ByVal Name As String, ByVal Value As String) Implements IRetrieveParams.SetParam
        m_iniFileReader.SetIniValue(Section, Name, Value)
    End Sub

    Public Sub SetSection(ByVal Name As String) Implements IRetrieveParams.SetSection
        m_defaultSection = Name
    End Sub

    Public Function GetAllKeysInSection(ByVal Section As String) As System.Collections.Specialized.StringCollection
        Dim sc As System.Collections.Specialized.StringCollection
        sc = m_iniFileReader.AllKeysInSection(Section)
        If sc Is Nothing Then Throw New Exception("No Keys in Section '" & Section & "'")
        Return sc
    End Function

End Class

