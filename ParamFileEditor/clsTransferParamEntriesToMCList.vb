Friend Class clsTransferParamEntriesToMassModList
    Inherits clsDMSParamUpload

    Private m_mgrParams As ParamFileEditor.ProgramSettings.IProgramSettings
    Private m_ParamUpload As clsDMSParamUpload
    Private m_AutoTweak As IMassTweaker
    Private m_IsoFix As IDeconvolveIsoMods

    Public Sub New(ByVal mgrParams As ParamFileEditor.ProgramSettings.IProgramSettings)
        MyBase.New(mgrParams)
        Me.m_mgrParams = mgrParams
        Me.m_AutoTweak = New clsMassTweaker(Me.m_mgrParams)
        Me.m_ParamUpload = New clsDMSParamUpload(Me.m_mgrParams)
        Me.m_IsoFix = New clsDeconvolveIsoMods(Me.m_mgrParams.DMS_ConnectionString)
    End Sub

    Public Sub SyncAll()
        Me.SyncMassModList2PETable()
    End Sub

    Public Sub SyncOneJob(ByVal paramFileID As Integer)
        Me.SyncSingleJob(paramFileID)
    End Sub

    Public Sub SyncDescriptions(ByRef BaseLineParams As ParamFileGenerator.clsParams)
        Me.SyncDesc(BaseLineParams)
    End Sub


    Private Sub SyncMassModList2PETable()

        Dim ParamEntryTable As DataTable
        Dim PESQL As String = "SELECT * FROM T_Param_Entries WHERE ([Entry_Type] like '%Modification')"
        ParamEntryTable = Me.GetTable(PESQL)

        Dim AffectedParamFiles As DataTable
        Dim fileSQL As String = "SELECT DISTINCT [Param_File_ID] FROM T_Param_Entries WHERE ([Entry_Type] like '%Modification') AND ([Param_File_ID] NOT IN " & _
            "(SELECT DISTINCT [Param_File_ID] FROM T_Param_File_Mass_Mods))"
        AffectedParamFiles = GetTable(fileSQL)

        Dim fileRow As DataRow
        Dim fileRows() As DataRow

        Dim currParamFileID As Integer

        fileRows = AffectedParamFiles.Select("", "[Param_File_ID]")
        For Each fileRow In fileRows        'Loop through the affected param files and get the appropriate mods
            currParamFileID = CInt(fileRow.Item(0))
            SyncSingleJob(currParamFileID)
        Next
    End Sub

    Private Sub SyncSingleJob(ByVal paramFileID As Integer)
        Dim tmpParams As ParamFileGenerator.clsParams


        Debug.WriteLine("Working on: " & paramFileID)


        tmpParams = Me.m_ParamUpload.GetParamsSet(paramFileID, True)
        tmpParams = Me.m_IsoFix.DeriveIsoMods(tmpParams)


        Me.tmpSaveMods(tmpParams, Me.m_AutoTweak)

        Debug.WriteLine("Finished: " & paramFileID)

    End Sub

    Private Sub SyncDesc(ByRef BaseLineParams As ParamFileGenerator.clsParams)
		Dim ParamFileTable As DataTable
		Dim eParamFileType As ParamFileGenerator.DownloadParams.clsParamsFromDMS.eParamFileTypeConstants
		eParamFileType = ParamFileGenerator.DownloadParams.clsParamsFromDMS.eParamFileTypeConstants.Sequest

		Dim PFSQL As String = "SELECT * FROM T_Param_Files WHERE [Param_File_Type_ID] = " & eParamFileType

        Dim tmpDA As SqlClient.SqlDataAdapter = Nothing
        Dim tmpCB As SqlClient.SqlCommandBuilder = Nothing

        ParamFileTable = Me.GetTable(PFSQL, tmpDA, tmpCB)

        Dim filerow As DataRow
        Dim fileRows() As DataRow

        Dim currParamFileID As Integer
        Dim testparams As ParamFileGenerator.clsParams

        fileRows = ParamFileTable.Select("", "[Param_File_ID]")

        For Each filerow In fileRows
            currParamFileID = CInt(filerow.Item("Param_File_ID"))
            If currParamFileID = 1092 Then
                Debug.WriteLine("")
            End If
            Debug.Write("Param File = " & currParamFileID & " | CS = " & filerow.RowState.ToString & ", ")
			testparams = Me.GetParamSetWithID(currParamFileID, eParamFileType)

            filerow.Item("Param_File_Description") = testparams.Description

            Debug.Write("NS = " & filerow.RowState.ToString & Chr(13) & Chr(10))
        Next

        tmpDA.Update(ParamFileTable)
    End Sub

End Class
