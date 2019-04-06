Imports System.Collections.Generic
Imports ParamFileGenerator
Imports System.IO

Namespace ProgramSettings
    Public Interface IProgramSettings
        Property DMS_ConnectionString As String
        Property DMS_ParamFileTableName As String
        Property MT_ConnectionString As String
        Property MT_ModParamFileTable As String
        Property MT_GlobalModListTable As String
        Property templateFileName As String
        Property CommonModsCollection As clsCommonModsCollection
        ReadOnly Property FieldMappingsTable As DataTable
        ReadOnly Property AAMappingsTable As DataTable
    End Interface

    Public Class clsSettings
        Implements IProgramSettings

        Private m_tmpCommonMods As List(Of String)
        ' Unused: Private m_fieldMappingsTable As DataTable
        Private m_aaMappingsTable As DataTable
        Private m_iniFilePath As String

        Private Const DEF_DMS_CONNECTION_STRING As String = "Data Source=gigasax;Initial Catalog=DMS5;Integrated Security=SSPI;"
        Private Const DEF_DMS_PARAM_TABLE_NAME As String = "T_Sequest_Params"
        Private Const DEF_MT_CONNECTION_STRING As String = "Data Source=pogo;Initial Catalog=MT_Main;Integrated Security=SSPI;"
        Private Const DEF_MT_MOD_PARAM_TABLE_NAME As String = "T_Peptide_Mod_Param_File_List"
        Private Const DEF_MT_GLOBAL_MOD_TABLE_NAME As String = "T_Peptide_Global_Mod_List"
        Private Const DEF_TEMPLATE_FILENAME As String = "sequest_N14_NE_Template.params"

#Region "Public Properties and Functions"

        Public Sub LoadSettings(iniFileName As String)
            m_iniFilePath = GetFilePath(iniFileName)
            Dim loadingSuccessful = LoadProgramSettings(m_iniFilePath)

        End Sub

        Public Property DMS_ConnectionString As String Implements IProgramSettings.DMS_ConnectionString
        Public Property DMS_ParamFileTableName As String Implements IProgramSettings.DMS_ParamFileTableName
        Public Property MT_ConnectionString As String Implements IProgramSettings.MT_ConnectionString
        Public Property MT_ModParamFileTable As String Implements IProgramSettings.MT_ModParamFileTable
        Public Property MT_GlobalModListTable As String Implements IProgramSettings.MT_GlobalModListTable
        Public Property TemplateFileName As String Implements IProgramSettings.templateFileName
        Public Property CommonModsCollection As clsCommonModsCollection Implements IProgramSettings.CommonModsCollection

        <Obsolete("Unused")>
        Public ReadOnly Property FieldMappingsTable As DataTable Implements IProgramSettings.FieldMappingsTable
            Get
                'Return m_fieldMappingsTable
                Return New DataTable
            End Get
        End Property
        Public ReadOnly Property AAMappingsTable As DataTable Implements IProgramSettings.AAMappingsTable
            Get
                Return m_aaMappingsTable
            End Get
        End Property

#End Region

#Region "Member Functions"

        Private Function LoadProgramSettings(IniFilePath As String) As Boolean
            Dim programSettings As New clsRetrieveParams(IniFilePath, True)

            'Get connection string settings

            Try
                DMS_ConnectionString = programSettings.GetParam("dms_database", "connectionstring")
            Catch ex As Exception
                programSettings.SetParam("dms_database", "connectionstring", DEF_DMS_CONNECTION_STRING)
                DMS_ConnectionString = DEF_DMS_CONNECTION_STRING
                programSettings.SaveSettings()
            End Try

            Try
                DMS_ParamFileTableName = programSettings.GetParam("dms_database", "paramstablename")
            Catch ex As Exception
                programSettings.SetParam("dms_database", "paramstablename", DEF_DMS_PARAM_TABLE_NAME)
                DMS_ParamFileTableName = DEF_DMS_PARAM_TABLE_NAME
                programSettings.SaveSettings()
            End Try

            Try
                MT_ConnectionString = programSettings.GetParam("mt_database", "connectionstring")
            Catch ex As Exception
                programSettings.SetParam("mt_database", "connectionstring", DEF_MT_CONNECTION_STRING)
                MT_ConnectionString = DEF_MT_CONNECTION_STRING
                programSettings.SaveSettings()
            End Try

            Try
                MT_ModParamFileTable = programSettings.GetParam("mt_database", "modsparamfiletable")
            Catch ex As Exception
                programSettings.SetParam("mt_database", "modsparamfiletable", DEF_MT_MOD_PARAM_TABLE_NAME)
                MT_ModParamFileTable = DEF_MT_MOD_PARAM_TABLE_NAME
                programSettings.SaveSettings()
            End Try
            Try
                MT_GlobalModListTable = programSettings.GetParam("mt_database", "globalmodstable")
            Catch ex As Exception
                programSettings.SetParam("mt_database", "globalmodstable", DEF_MT_GLOBAL_MOD_TABLE_NAME)
                MT_GlobalModListTable = DEF_MT_GLOBAL_MOD_TABLE_NAME
                programSettings.SaveSettings()
            End Try


            'Get Template file name and path
            Try
                templateFileName = programSettings.GetParam("programcontrol", "templateFileName")
            Catch ex As Exception
                programSettings.SetParam("programcontrol", "templateFileName", DEF_TEMPLATE_FILENAME)
                programSettings.SaveSettings()
                templateFileName = DEF_TEMPLATE_FILENAME
            End Try

            'Get common modifications list keys for mass calc helper

            Try
                m_tmpCommonMods = programSettings.GetAllKeysInSection("commonmods")
            Catch ex As Exception
                m_tmpCommonMods = Nothing
            End Try

            'Get details for the common mods listed above
            CommonModsCollection = New clsCommonModsCollection

            If m_tmpCommonMods.Count > 0 Then
                For Each commonMod In m_tmpCommonMods
                    Dim tmpDesc = programSettings.GetParam("commonmods", commonMod)
                    Dim tmpFormula = programSettings.GetParam("commonmods", commonMod, "formula")

                    Dim modDetails = New clsCommonModDetails With {
                        .Name = commonMod,
                        .Description = tmpDesc,
                        .Formula = tmpFormula
                    }

                    CommonModsCollection.Add(modDetails)
                Next
            End If

            m_aaMappingsTable = LoadAAMappingsTable(programSettings)

        End Function
        Private Function GetFilePath(fileName As String) As String
            Dim fi As New FileInfo(Application.ExecutablePath)
            Return Path.Combine(fi.DirectoryName, fileName)
        End Function

        Private Function LoadAAMappingsTable(pr As clsRetrieveParams) As DataTable
            Dim sn = "aminoacidmappings"
            Dim counter = 1000

            Dim t = SetupAAMappingsTable()
            Dim keyNames = pr.GetAllKeysInSection(sn)

            For Each keyName In keyNames
                Dim tmpSLC = pr.GetParam(sn, keyName)
                Dim tmpTLC = pr.GetParam(sn, keyName, "threeletter")
                Dim tmpFN = pr.GetParam(sn, keyName, "fullname")

                Dim dr = t.NewRow
                dr("AA_ID") = counter
                dr("Enum_Name") = keyName
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