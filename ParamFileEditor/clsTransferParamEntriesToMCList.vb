Imports PRISMDatabaseUtils

Friend Class clsTransferParamEntriesToMassModList
    Inherits clsDMSParamUpload

    Private ReadOnly m_ParamUpload As clsDMSParamUpload
    Private m_AutoTweak As IMassTweaker
    Private ReadOnly m_IsoFix As IDeconvolveIsoMods

    Public Sub New(dbTools As IDBTools)
        MyBase.New(dbTools)
        Me.m_AutoTweak = New clsMassTweaker(dbTools)
        Me.m_ParamUpload = New clsDMSParamUpload(dbTools)
        Me.m_IsoFix = New clsDeconvolveIsoMods(mDBTools)
    End Sub

    Public Sub SyncAll()
        Me.SyncMassModList2PETable()
    End Sub

    Public Sub SyncOneJob(paramFileID As Integer)
        Me.SyncSingleJob(paramFileID)
    End Sub

    Public Sub SyncDescriptions()
        Me.SyncDesc()
    End Sub

    Private Sub SyncMassModList2PETable()

        'Dim PESQL = "SELECT * FROM T_Param_Entries WHERE ([Entry_Type] like '%Modification')"
        'Dim paramEntryTable = Me.GetTable(PESQL)

        Dim fileSQL As String = " SELECT DISTINCT [Param_File_ID] FROM T_Param_Entries" & " WHERE ([Entry_Type] like '%Modification') AND " & "       ([Param_File_ID] NOT IN (SELECT DISTINCT [Param_File_ID] FROM T_Param_File_Mass_Mods))"

        Dim affectedParamFiles = GetTable(fileSQL)

        Dim currentParamFileID As Integer

        Dim fileRows = affectedParamFiles.Select("", "[Param_File_ID]")

        'Loop through the affected param files and get the appropriate mods
        For Each fileRow As DataRow In fileRows
            currentParamFileID = CInt(fileRow.Item(0))
            SyncSingleJob(currentParamFileID)
        Next
    End Sub

    Private Sub SyncSingleJob(paramFileID As Integer)
        Dim tmpParams As ParamFileGenerator.Params


        Debug.WriteLine("Working on: " & paramFileID)


        tmpParams = Me.m_ParamUpload.GetParamsSet(paramFileID, True)
        tmpParams = Me.m_IsoFix.DeriveIsoMods(tmpParams)


        Me.tmpSaveMods(tmpParams, Me.m_AutoTweak)

        Debug.WriteLine("Finished: " & paramFileID)
    End Sub

    Private Sub SyncDesc()

        Dim eParamFileType = eParamFileTypeConstants.Sequest

        Dim PFSQL As String = "SELECT * FROM T_Param_Files WHERE [Param_File_Type_ID] = " & eParamFileType

        Dim tmpDA As SqlClient.SqlDataAdapter = Nothing

        Dim paramFiles = Me.GetTable(PFSQL)

        Dim fileRows = paramFiles.Select("", "[Param_File_ID]")

        For Each fileRow As DataRow In fileRows
            Dim currParamFileID = CInt(fileRow.Item("Param_File_ID"))
            If currParamFileID = 1092 Then
                Debug.WriteLine("")
            End If
            Debug.Write("Param File = " & currParamFileID & " | CS = " & fileRow.RowState.ToString & ", ")
            Dim testparams = Me.GetParamSetWithID(currParamFileID, eParamFileType)

            fileRow.Item("Param_File_Description") = testparams.Description

            Debug.Write("NS = " & fileRow.RowState.ToString & Chr(13) & Chr(10))
        Next

        tmpDA.Update(paramFiles)
    End Sub
End Class
