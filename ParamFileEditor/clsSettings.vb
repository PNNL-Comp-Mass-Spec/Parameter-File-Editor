Imports System.Collections.Specialized
Imports ParamFileGenerator
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
        Private m_tmpCommonMods As StringCollection
        Private m_commonModsColl As clsCommonModsCollection
        ' Unused: Private m_fieldMappingsTable As DataTable
        Private m_aaMappingsTable As DataTable
        Private m_iniFilePath As String

        Private Const DEF_DMS_CONNSTR As String = "Data Source=gigasax;Initial Catalog=DMS5;Integrated Security=SSPI;"
        Private Const DEF_DMS_PARAM_TABLE_NAME As String = "T_Sequest_Params"
        Private Const DEF_MT_CONNSTR As String = "Data Source=pogo;Initial Catalog=MT_Main;Integrated Security=SSPI;"
        Private Const DEF_MT_MOD_PARAM_TABLE_NAME As String = "T_Peptide_Mod_Param_File_List"
        Private Const DEF_MT_GLOBAL_MOD_TABLE_NAME As String = "T_Peptide_Global_Mod_List"
        Private Const DEF_TEMPLATEFILENAME As String = "sequest_N14_NE_Template.params"

#Region " Public Properties and Functions "

        Public Sub LoadSettings(iniFileName As String)
            Dim loadingSuccessful As Boolean
            m_iniFilePath = GetFilePath(iniFileName)
            loadingSuccessful = LoadProgramSettings(m_iniFilePath)

        End Sub

        Public Property DMS_ConnectionString() As String Implements IProgramSettings.DMS_ConnectionString
            Get
                Return m_DMSconnectionString
            End Get
            Set
                m_DMSconnectionString = Value
            End Set
        End Property
        Public Property DMS_ParamFileTableName() As String Implements IProgramSettings.DMS_ParamFileTableName
            Get
                Return m_DMSParamFileTableName
            End Get
            Set
                m_DMSParamFileTableName = Value
            End Set
        End Property
        Public Property MT_ConnectionString() As String Implements IProgramSettings.MT_ConnectionString
            Get
                Return m_MTConnectionString
            End Get
            Set
                m_MTConnectionString = Value
            End Set
        End Property
        Public Property MT_ModParamFileTable() As String Implements IProgramSettings.MT_ModParamFileTable
            Get
                Return m_MTModParamFileTable
            End Get
            Set
                m_MTModParamFileTable = Value
            End Set
        End Property
        Public Property MT_GlobalModListTable() As String Implements IProgramSettings.MT_GlobalModListTable
            Get
                Return m_MTGlobalModListTable
            End Get
            Set
                m_MTGlobalModListTable = Value
            End Set
        End Property
        Public Property TemplateFileName() As String Implements IProgramSettings.TemplateFileName
            Get
                Return m_templateFileName
            End Get
            Set
                m_templateFileName = Value
            End Set
        End Property
        Public Property CommonModsCollection() As clsCommonModsCollection Implements IProgramSettings.CommonModsCollection
            Get
                Return m_commonModsColl
            End Get
            Set
                m_commonModsColl = Value
            End Set
        End Property

        <Obsolete("Unused")>
        Public ReadOnly Property FieldMappingsTable() As DataTable Implements IProgramSettings.FieldMappingsTable
            Get
                'Return m_fieldMappingsTable
                Return New DataTable
            End Get
        End Property
        Public ReadOnly Property AAMappingsTable() As DataTable Implements IProgramSettings.AAMappingsTable
            Get
                Return m_aaMappingsTable
            End Get
        End Property

#End Region

#Region " Member Functions "

        Private Function LoadProgramSettings(IniFilePath As String) As Boolean
            Dim programSettings As New clsRetrieveParams(IniFilePath, True)

            'Get connection string settings

            Try
                m_DMSconnectionString = programSettings.GetParam("dms_database", "connectionstring")
            Catch ex As Exception
                programSettings.SetParam("dms_database", "connectionstring", DEF_DMS_CONNSTR)
                m_DMSconnectionString = DEF_DMS_CONNSTR
                programSettings.SaveSettings()
            End Try

            Try
                m_DMSParamFileTableName = programSettings.GetParam("dms_database", "paramstablename")
            Catch ex As Exception
                programSettings.SetParam("dms_database", "paramstablename", DEF_DMS_PARAM_TABLE_NAME)
                m_DMSParamFileTableName = DEF_DMS_PARAM_TABLE_NAME
                programSettings.SaveSettings()
            End Try

            Try
                m_MTConnectionString = programSettings.GetParam("mt_database", "connectionstring")
            Catch ex As Exception
                programSettings.SetParam("mt_database", "connectionstring", DEF_MT_CONNSTR)
                m_MTConnectionString = DEF_MT_CONNSTR
                programSettings.SaveSettings()
            End Try

            Try
                m_MTModParamFileTable = programSettings.GetParam("mt_database", "modsparamfiletable")
            Catch ex As Exception
                programSettings.SetParam("mt_database", "modsparamfiletable", DEF_MT_MOD_PARAM_TABLE_NAME)
                m_MTModParamFileTable = DEF_MT_MOD_PARAM_TABLE_NAME
                programSettings.SaveSettings()
            End Try
            Try
                m_MTGlobalModListTable = programSettings.GetParam("mt_database", "globalmodstable")
            Catch ex As Exception
                programSettings.SetParam("mt_database", "globalmodstable", DEF_MT_GLOBAL_MOD_TABLE_NAME)
                m_MTGlobalModListTable = DEF_MT_GLOBAL_MOD_TABLE_NAME
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

            m_aaMappingsTable = LoadAAMappingsTable(programSettings)

        End Function
        Private Function GetFilePath(fileName As String) As String
            Dim fi As New FileInfo(Application.ExecutablePath)
            Return Path.Combine(fi.DirectoryName, fileName)
        End Function


        Private Function LoadAAMappingsTable(pr As clsRetrieveParams) As DataTable
            Dim t As DataTable
            Dim dr As DataRow
            Dim sn = "aminoacidmappings"
            Dim counter = 1000

            t = SetupAAMappingsTable()
            Dim sc As StringCollection = pr.GetAllKeysInSection(sn)
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

        <Obsolete("Unused")>
        Private Function SetupFieldMappingTable(tableName As String) As DataTable
            Dim t As New DataTable(tableName)

            With t.Columns
                .Add("Internal_Name", Type.GetType("System.String"))
                .Add("SQ_Name", Type.GetType("System.String"))
                .Add("DMS_Name", Type.GetType("System.String"))
                .Add("Value_Type", Type.GetType("System.String"))
            End With

            Return t

        End Function
        Private Function SetupAAMappingsTable() As DataTable
            Dim t As New DataTable
            With t.Columns
                .Add("AA_ID", Type.GetType("System.Int32"))
                .Add("Enum_Name", Type.GetType("System.String"))
                .Add("SL_Code", Type.GetType("System.String"))
                .Add("TL_Code", Type.GetType("System.String"))
                .Add("Full_Name", Type.GetType("System.String"))
            End With
            Return t
        End Function


#End Region

    End Class

    Public Class clsCommonModDetails
        Public Property Name As String

        Public Property Description As String

        Public Property Formula As String
    End Class

    Public Class clsCommonModsCollection
        Inherits CollectionBase

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub Add(Modification As clsCommonModDetails)
            MyBase.InnerList.Add(Modification)
        End Sub

        Public Sub Sort()
            MyBase.InnerList.Sort(New clsSimpleComparer("Description"))
        End Sub

        Default Public Property Item(index As Integer) As clsCommonModDetails
            Get
                Return DirectCast(InnerList(index), clsCommonModDetails)
            End Get
            Set
                MyBase.InnerList(index) = Value
            End Set
        End Property

    End Class

End Namespace