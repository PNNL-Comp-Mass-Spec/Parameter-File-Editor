Imports ParamFileEditor.ProgramSettings
Imports System.IO

Namespace ProgramSettings
    Public Interface IProgramSettings
        Property DMS_ConnectionString() As String
        Property DMS_ParamFileTableName() As String
        Property MT_ConnectionString() As String
        Property MT_ModParamFileTable() As String
        Property MT_GlobalModListTable() As String
        Property TemplateFileName() As String
        Property CommonModsCollection() As clsCommonModsCollection
        ReadOnly Property FieldMappingsTable() As DataTable
        ReadOnly Property AAMappingsTable() As DataTable
    End Interface

    Public Class clsSettings
        Implements IProgramSettings
        Private m_DMSconnectionString As String
        Private m_DMSParamFileTableName As String
        Private m_MTConnectionString As String
        Private m_MTModParamFileTable As String
        Private m_MTGlobalModListTable As String
        Private m_templateFileName As String
        Private m_tmpCommonMods As System.Collections.Specialized.StringCollection
        Private m_commonModsColl As clsCommonModsCollection
        Private m_fieldMappingsTable As DataTable
        Private m_aaMappingsTable As DataTable
        Private m_iniFilePath As String

        Private Const DEF_DMS_CONNSTR As String = "Data Source=gigasax;Initial Catalog=DMS5;Integrated Security=SSPI;"
        Private Const DEF_DMS_PARAM_TABLE_NAME As String = "T_Sequest_Params"
        Private Const DEF_MT_CONNSTR As String = "Data Source=pogo;Initial Catalog=MT_Main;Integrated Security=SSPI;"
        Private Const DEF_MT_MOD_PARAM_TABLE_NAME As String = "T_Peptide_Mod_Param_File_List"
        Private Const DEF_MT_GLOBAL_MOD_TABLE_NAME As String = "T_Peptide_Global_Mod_List"
        Private Const DEF_TEMPLATEFILENAME = "sequest_N14_NE_Template.params"

#Region " Public Properties and Functions "

        Public Sub LoadSettings(ByVal iniFileName As String)
            Dim loadingSuccessful As Boolean
            m_iniFilePath = GetFilePath(iniFileName)
            loadingSuccessful = LoadProgramSettings(m_iniFilePath)

        End Sub

        Public Property DMS_ConnectionString() As String Implements IProgramSettings.DMS_ConnectionString
            Get
                Return Me.m_DMSconnectionString
            End Get
            Set(ByVal Value As String)
                Me.m_DMSconnectionString = Value
            End Set
        End Property
        Public Property DMS_ParamFileTableName() As String Implements IProgramSettings.DMS_ParamFileTableName
            Get
                Return Me.m_DMSParamFileTableName
            End Get
            Set(ByVal Value As String)
                Me.m_DMSParamFileTableName = Value
            End Set
        End Property
        Public Property MT_ConnectionString() As String Implements IProgramSettings.MT_ConnectionString
            Get
                Return Me.m_MTConnectionString
            End Get
            Set(ByVal Value As String)
                Me.m_MTConnectionString = Value
            End Set
        End Property
        Public Property MT_ModParamFileTable() As String Implements IProgramSettings.MT_ModParamFileTable
            Get
                Return Me.m_MTModParamFileTable
            End Get
            Set(ByVal Value As String)
                Me.m_MTModParamFileTable = Value
            End Set
        End Property
        Public Property MT_GlobalModListTable() As String Implements IProgramSettings.MT_GlobalModListTable
            Get
                Return Me.m_MTGlobalModListTable
            End Get
            Set(ByVal Value As String)
                Me.m_MTGlobalModListTable = Value
            End Set
        End Property
        Public Property TemplateFileName() As String Implements IProgramSettings.TemplateFileName
            Get
                Return Me.m_templateFileName
            End Get
            Set(ByVal Value As String)
                Me.m_templateFileName = Value
            End Set
        End Property
        Public Property CommonModsCollection() As clsCommonModsCollection Implements IProgramSettings.CommonModsCollection
            Get
                Return Me.m_commonModsColl
            End Get
            Set(ByVal Value As clsCommonModsCollection)
                Me.m_commonModsColl = Value
            End Set
        End Property
        Public ReadOnly Property FieldMappingsTable() As DataTable Implements IProgramSettings.FieldMappingsTable
            Get
                Return Me.m_fieldMappingsTable
            End Get
        End Property
        Public ReadOnly Property AAMappingsTable() As DataTable Implements IProgramSettings.AAMappingsTable
            Get
                Return Me.m_aaMappingsTable
            End Get
        End Property

#End Region

#Region " Member Functions "

        Private Function LoadProgramSettings(ByVal IniFilePath As String) As Boolean
            Dim programSettings As New clsRetrieveParams(IniFilePath, True)
            'Get connection string settings
            Dim fi As New FileInfo(IniFilePath)

            Try
                Me.m_DMSconnectionString = programSettings.GetParam("dms_database", "connectionstring")
            Catch ex As Exception
                programSettings.SetParam("dms_database", "connectionstring", Me.DEF_DMS_CONNSTR)
                Me.m_DMSconnectionString = Me.DEF_DMS_CONNSTR
                programSettings.SaveSettings()
            End Try

            Try
                Me.m_DMSParamFileTableName = programSettings.GetParam("dms_database", "paramstablename")
            Catch ex As Exception
                programSettings.SetParam("dms_database", "paramstablename", Me.DEF_DMS_PARAM_TABLE_NAME)
                Me.m_DMSParamFileTableName = Me.DEF_DMS_PARAM_TABLE_NAME
                programSettings.SaveSettings()
            End Try

            Try
                Me.m_MTConnectionString = programSettings.GetParam("mt_database", "connectionstring")
            Catch ex As Exception
                programSettings.SetParam("mt_database", "connectionstring", Me.DEF_MT_CONNSTR)
                Me.m_MTConnectionString = Me.DEF_MT_CONNSTR
                programSettings.SaveSettings()
            End Try

            Try
                Me.m_MTModParamFileTable = programSettings.GetParam("mt_database", "modsparamfiletable")
            Catch ex As Exception
                programSettings.SetParam("mt_database", "modsparamfiletable", Me.DEF_MT_MOD_PARAM_TABLE_NAME)
                Me.m_MTModParamFileTable = Me.DEF_MT_MOD_PARAM_TABLE_NAME
                programSettings.SaveSettings()
            End Try
            Try
                Me.m_MTGlobalModListTable = programSettings.GetParam("mt_database", "globalmodstable")
            Catch ex As Exception
                programSettings.SetParam("mt_database", "globalmodstable", Me.DEF_MT_GLOBAL_MOD_TABLE_NAME)
                Me.m_MTGlobalModListTable = Me.DEF_MT_GLOBAL_MOD_TABLE_NAME
                programSettings.SaveSettings()
            End Try


            'Get Template file name and path
            Try
                m_templateFileName = programSettings.GetParam("programcontrol", "templatefilename")
            Catch ex As Exception
                programSettings.SetParam("programcontrol", "templatefilename", DEF_TEMPLATEFILENAME)
                programSettings.SaveSettings()
                m_templateFileName = DEF_TEMPLATEFILENAME
            End Try

            'Get common modifications list keys for mass calc helper

            Try
                m_tmpCommonMods = programSettings.GetAllKeysInSection("commonmods")
            Catch ex As Exception
                m_tmpCommonMods = Nothing
            End Try


            'Get details for the common mods listed above
            Dim s As String
            Dim m_tmpDesc As String
            Dim m_tmpFormula As String
            Dim m_modDetails As clsCommonModDetails
            m_commonModsColl = New clsCommonModsCollection

            If m_tmpCommonMods.Count > 0 Then
                For Each s In m_tmpCommonMods
                    With programSettings
                        m_tmpDesc = .GetParam("commonmods", s)
                        m_tmpFormula = .GetParam("commonmods", s, "formula")
                    End With
                    m_modDetails = New clsCommonModDetails
                    With m_modDetails
                        .Name = s
                        .Description = m_tmpDesc
                        .Formula = m_tmpFormula
                    End With

                    m_commonModsColl.add(m_modDetails)
                Next
            End If

            Me.m_aaMappingsTable = Me.LoadAAMappingsTable(programSettings)

            'Get Field Mappings for DMS and SQ files
            'm_fieldMappingsTable = SetupFieldMappingTable("T_Field_Mappings")

            'Dim m_tmpFieldMappings As System.Collections.Specialized.StringCollection
            'Dim m_tmpIntName As String
            'Dim m_tmpSQName As String
            'Dim m_tmpDMSName As String
            'Dim m_tmpValueType As String
            'Dim m_tmpDR As DataRow

            'Try
            '    m_tmpFieldMappings = programSettings.GetAllKeysInSection("fieldmappings")
            'Catch ex As Exception
            '    Me.m_tmp = Nothing
            'End Try

            'If m_tmpFieldMappings.Count > 0 Then
            '    For Each s In m_tmpFieldMappings
            '        With programSettings
            '            m_tmpIntName = s
            '            m_tmpValueType = .GetParam("fieldmappings", m_tmpIntName)
            '            m_tmpDMSName = .GetParam("fieldmappings", m_tmpIntName, "DMS_Name")
            '            m_tmpSQName = .GetParam("fieldmappings", m_tmpIntName, "SQ_Name")
            '        End With

            '        'workRow = workTable.NewRow
            '        'For Each strField In tmpArray
            '        '    If fieldCounter <> 0 Then
            '        '        workRow(fieldCounter - 1) = strField
            '        '    End If
            '        '    fieldCounter = fieldCounter + 1
            '        'Next
            '        'fieldCounter = 0
            '        'workTable.Rows.Add(workRow)

            '        m_tmpDR = m_fieldMappingsTable.NewRow

            '        m_tmpDR(0) = m_tmpIntName
            '        m_tmpDR(1) = m_tmpSQName
            '        m_tmpDR(2) = m_tmpDMSName
            '        m_tmpDR(3) = m_tmpValueType

            '        m_fieldMappingsTable.Rows.Add(m_tmpDR)
            '    Next
            'End If

        End Function
        Private Function GetFilePath(ByVal fileName As String) As String
            Dim fi As New FileInfo(Application.ExecutablePath)
            Return Path.Combine(fi.DirectoryName, fileName)
        End Function


        Private Function LoadAAMappingsTable(ByVal pr As clsRetrieveParams) As DataTable
            Dim t As DataTable
            Dim dr As DataRow
            Dim sn As String = "aminoacidmappings"
            Dim counter As Integer = 1000

            t = Me.SetupAAMappingsTable
            Dim sc As System.Collections.Specialized.StringCollection = pr.GetAllKeysInSection(sn)
            Dim s As String
            Dim tmpSLC As String
            Dim tmpTLC As String
            Dim tmpFN As String

            For Each s In sc
                tmpSLC = pr.GetParam(sn, s)
                tmpTLC = pr.GetParam(sn, s, "threeletter")
                tmpFN = pr.GetParam(sn, s, "fullname")

                dr = t.NewRow
                dr("AA_ID") = counter
                dr("Enum_Name") = s
                dr("SL_Code") = tmpSLC
                dr("TL_Code") = tmpTLC
                dr("Full_Name") = tmpFN
                t.Rows.Add(dr)
                counter += 1
            Next

            Return t

        End Function
        Private Function SetupFieldMappingTable(ByVal tableName As String) As DataTable
            Dim t As New DataTable(tableName)

            With t.Columns
                .Add("Internal_Name", System.Type.GetType("System.String"))
                .Add("SQ_Name", System.Type.GetType("System.String"))
                .Add("DMS_Name", System.Type.GetType("System.String"))
                .Add("Value_Type", System.Type.GetType("System.String"))
            End With

            Return t

        End Function
        Private Function SetupAAMappingsTable() As DataTable
            Dim t As New DataTable
            With t.Columns
                .Add("AA_ID", System.Type.GetType("System.Int32"))
                .Add("Enum_Name", System.Type.GetType("System.String"))
                .Add("SL_Code", System.Type.GetType("System.String"))
                .Add("TL_Code", System.Type.GetType("System.String"))
                .Add("Full_Name", System.Type.GetType("System.String"))
            End With
            Return t
        End Function


#End Region

    End Class

    Public Class clsCommonModDetails
        Private m_Name As String
        Private m_Desc As String
        Private m_Formula As String

        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal Value As String)
                m_Name = Value
            End Set
        End Property
        Public Property Description() As String
            Get
                Return m_Desc
            End Get
            Set(ByVal Value As String)
                m_Desc = Value
            End Set
        End Property
        Public Property Formula() As String
            Get
                Return m_Formula
            End Get
            Set(ByVal Value As String)
                m_Formula = Value
            End Set
        End Property

    End Class

    Public Class clsCommonModsCollection
        Inherits CollectionBase

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub add(ByVal Modification As clsCommonModDetails)
            MyBase.InnerList.Add(Modification)
        End Sub

        Public Sub sort()
            MyBase.InnerList.Sort(New ParamFileEditor.clsSimpleComparer("Description"))
        End Sub

        Default Public Property item(ByVal index As Integer) As clsCommonModDetails
            Get
                Return MyBase.InnerList(index)
            End Get
            Set(ByVal Value As clsCommonModDetails)
                MyBase.InnerList(index) = Value
            End Set
        End Property

    End Class

End Namespace