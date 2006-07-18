Imports System.Collections.Specialized
Imports System.Reflection
Imports ParamFileGenerator


Friend Class clsDMSParamUpload
    Inherits ParamFileGenerator.DownloadParams.clsparamsfromdms

#Region " Member Properties "
    'Private m_clsUpdateModsTable As clsUpdateModsTable
    Private m_SPError As String
    Private m_mgrParams As ParamFileEditor.ProgramSettings.IProgramSettings
    Private m_DynamicModCounter As Integer = 1
#End Region

#Region " Friend Procedures "

    Public Sub New(ByVal mgrParams As ParamFileEditor.ProgramSettings.IProgramSettings)
        MyBase.New(mgrParams.DMS_ConnectionString)
        Me.m_mgrParams = mgrParams
        'Me.m_clsUpdateModsTable = New clsUpdateModsTable(mgrParams)
    End Sub
    Friend Function WriteParamsToDMS(ByVal ParamSet As clsParams) As Boolean
        Dim success As Boolean = SaveParams(ParamSet)
        Return True
    End Function

    Friend Function WriteParamsToLocalStructure(ByVal Paramset As clsParams, ByVal ParamSetIDToUpdate As Integer) As Boolean
        Return SaveParams(Paramset)
    End Function

    Friend Function GetDiffsFromTemplate(ByVal paramSetID As Integer) As String
        Return DistillFeaturesFromParamSet(paramSetID)
    End Function

    Friend Function GetDiffsFromTemplate(ByVal paramSet As clsParams) As String
        Return DistillFeaturesFromParamSet(paramSet)
    End Function

    Friend Function GetDiffsBetweenSets(ByVal templateSet As clsParams, ByVal checkSet As clsParams) As String
        Return Me.CompareParamSets(templateSet, checkSet)
    End Function

    Friend Sub AddParamFileMappingEntry( _
        ByVal paramFileID As Integer, _
        ByVal entrySeqOrder As Integer, _
        ByVal entryTypeString As String, _
        ByVal entrySpecifier As String, _
        ByVal entryValue As String)

        Dim entryType As clsDMSParamStorage.ParamTypes

        entryType = DirectCast(System.Enum.Parse(GetType(clsDMSParamStorage.ParamTypes), entryTypeString), ParamFileGenerator.clsDMSParamStorage.ParamTypes)


        RunSP_AddUpdateParamEntry(paramFileID, entrySeqOrder, entryType, entrySpecifier, entryValue)

    End Sub

    Friend Function GetParamsSet(ByVal paramSetID As Integer, Optional ByVal DisableMassLookup As Boolean = False) As clsParams
        Return Me.GetParamSetWithID(paramSetID, DisableMassLookup)
    End Function

#End Region

#Region " Protected & Private Functions "

    Protected Function SaveParams(ByRef ParamSet As clsParams) As Boolean
        Dim diff As clsDMSParamStorage.ParamsEntry
        Dim diffcollection As clsDMSParamStorage
        Dim counter As Integer

        ParamSet.Description = Me.DistillFeaturesFromParamSet(ParamSet)

        Me.RunSP_AddUpdateParamFile(ParamSet.FileName, ParamSet.Description)

        diffcollection = Me.GetDiffColl(clsMainProcess.BaseLineParamSet, ParamSet)

        Dim paramSetID As Integer = Me.RunSP_GetParamFileID(ParamSet.FileName)

        Me.RunSP_DeleteParamEntries(ParamSet.FileName)

        counter = 1
        For Each diff In diffcollection
            RunSP_AddUpdateParamEntry(paramSetID, counter, diff.Type, diff.Specifier, diff.Value)
            counter += 1
        Next

        Me.RefreshParamsFromDMS()

    End Function

    Protected Sub tmpSaveMods(ByRef ParamSet As clsParams, ByRef at As IMassTweaker)
        Dim diff As clsDMSParamStorage.ParamsEntry
        Dim diffcollection As clsDMSParamStorage
        Dim counter As Integer
        Dim tmpMass As Single

        diffcollection = Me.GetDiffColl(clsMainProcess.BaseLineParamSet, ParamSet)

        Dim paramSetID As Integer = Me.RunSP_GetParamFileID(ParamSet.FileName)

        counter = 1
        For Each diff In diffcollection
            If InStr(diff.Type.ToString, "Param") = 0 Then
                If diff.Type <> clsDMSParamStorage.ParamTypes.IsotopicModification Then
                    tmpMass = at.GetTweakedMass(diff.Value)
                Else
                    tmpMass = CSng(diff.Value)
                End If
                Me.RunSP_AddUpdateParamEntry(paramSetID, counter, diff.Type, diff.Specifier, tmpMass)
                counter += 1
            End If
        Next

    End Sub

    Protected Sub RunSP_AddUpdateParamFile( _
        ByVal paramFileName As String, _
        ByVal paramFileDescription As String)

        Dim sp_Save As SqlClient.SqlCommand

        If Not Me.m_PersistConnection Then Me.OpenConnection()

        sp_Save = New SqlClient.SqlCommand("AddUpdateParamFile", Me.m_DBCn)

        sp_Save.CommandType = CommandType.StoredProcedure

        'Define parameters
        Dim myParam As SqlClient.SqlParameter

        'Define parameter for sp's return value
        myParam = sp_Save.Parameters.Add("@Return", SqlDbType.Int)
        myParam.Direction = ParameterDirection.ReturnValue

        'Define parameters for the sp's arguments
        myParam = sp_Save.Parameters.Add("@paramFileName", SqlDbType.VarChar, 255)
        myParam.Direction = ParameterDirection.Input
        myParam.Value = paramFileName

        myParam = sp_Save.Parameters.Add("@paramFileDesc", SqlDbType.VarChar, 1024)
        myParam.Direction = ParameterDirection.Input
        myParam.Value = paramFileDescription

        myParam = sp_Save.Parameters.Add("@ParamFileTypeID", SqlDbType.Int)
        myParam.Direction = ParameterDirection.Input
        myParam.Value = 1000

        myParam = sp_Save.Parameters.Add("@message", SqlDbType.VarChar, 512)
        myParam.Direction = ParameterDirection.Output


        'Execute the sp
        sp_Save.ExecuteNonQuery()

        'Get return value
        Dim ret As Integer = CInt(sp_Save.Parameters("@Return").Value)

        If ret <> 0 Then
            Me.m_SPError = CStr(sp_Save.Parameters("@message").Value)
        End If

        If Not Me.m_PersistConnection Then Me.CloseConnection()

    End Sub

    Protected Sub RunSP_AddUpdateParamEntry( _
        ByVal paramFileID As Integer, _
        ByVal entrySeqOrder As Integer, _
        ByVal entryType As clsDMSParamStorage.ParamTypes, _
        ByVal entrySpecifier As String, _
        ByVal entryValue As String)

        Dim sp_Save As SqlClient.SqlCommand

        If Not Me.m_PersistConnection Then Me.OpenConnection()

        sp_Save = New SqlClient.SqlCommand("AddUpdateParamFileEntry", Me.m_DBCn)

        sp_Save.CommandType = CommandType.StoredProcedure

        'Define parameters
        Dim myParam As SqlClient.SqlParameter

        'Define parameter for sp's return value
        myParam = sp_Save.Parameters.Add("@Return", SqlDbType.Int)
        myParam.Direction = ParameterDirection.ReturnValue

        'Define parameters for the sp's arguments
        myParam = sp_Save.Parameters.Add("@paramFileID", SqlDbType.Int)
        myParam.Direction = ParameterDirection.Input
        myParam.Value = paramFileID

        myParam = sp_Save.Parameters.Add("@entrySeqOrder", SqlDbType.Int)
        myParam.Direction = ParameterDirection.Input
        myParam.Value = entrySeqOrder

        myParam = sp_Save.Parameters.Add("@entryType", SqlDbType.VarChar, 32)
        myParam.Direction = ParameterDirection.Input
        myParam.Value = entryType.ToString

        myParam = sp_Save.Parameters.Add("@entrySpecifier", SqlDbType.VarChar, 32)
        myParam.Direction = ParameterDirection.Input
        myParam.Value = entrySpecifier

        myParam = sp_Save.Parameters.Add("@entryValue", SqlDbType.VarChar, 32)
        myParam.Direction = ParameterDirection.Input
        myParam.Value = entryValue

        myParam = sp_Save.Parameters.Add("@mode", SqlDbType.VarChar, 12)
        myParam.Direction = ParameterDirection.Input
        myParam.Value = "Add"

        myParam = sp_Save.Parameters.Add("@message", SqlDbType.VarChar, 512)
        myParam.Direction = ParameterDirection.Output


        'Execute the sp
        sp_Save.ExecuteNonQuery()

        'Get return value
        Dim ret As Integer = CInt(sp_Save.Parameters("@Return").Value)

        If ret <> 0 Then
            Me.m_SPError = CStr(sp_Save.Parameters("@message").Value)
        End If

        If Not Me.m_PersistConnection Then Me.CloseConnection()

    End Sub

    Protected Sub RunSP_AddUpdateGlobalModModMappingEntry( _
        ByVal paramFileID As Integer, _
        ByVal localSymbolID As Integer, _
        ByVal globalModID As Integer)

        Dim sp_Save As SqlClient.SqlCommand

        If Not Me.m_PersistConnection Then Me.OpenConnection()

        sp_Save = New SqlClient.SqlCommand("AddUpdateGlobalModMappingEntry", Me.m_DBCn)

        sp_Save.CommandType = CommandType.StoredProcedure

        'Define parameters
        Dim myParam As SqlClient.SqlParameter

        'Define parameter for sp's return value
        myParam = sp_Save.Parameters.Add("@Return", SqlDbType.Int)
        myParam.Direction = ParameterDirection.ReturnValue

        'Define parameters for the sp's arguments
        myParam = sp_Save.Parameters.Add("@paramFileID", SqlDbType.Int)
        myParam.Direction = ParameterDirection.Input
        myParam.Value = paramFileID

        myParam = sp_Save.Parameters.Add("@globalModID", SqlDbType.Int)
        myParam.Direction = ParameterDirection.Input
        myParam.Value = globalModID

        myParam = sp_Save.Parameters.Add("@localSymbolID", SqlDbType.Int)
        myParam.Direction = ParameterDirection.Input
        myParam.Value = localSymbolID

        myParam = sp_Save.Parameters.Add("@message", SqlDbType.VarChar, 512)
        myParam.Direction = ParameterDirection.Output


        'Execute the sp
        sp_Save.ExecuteNonQuery()

        'Get return value
        Dim ret As Integer = CInt(sp_Save.Parameters("@Return").Value)

        If ret <> 0 Then
            Me.m_SPError = CStr(sp_Save.Parameters("@message").Value)
        End If

        If Not Me.m_PersistConnection Then Me.CloseConnection()


    End Sub

    Protected Sub RunSP_DeleteParamFile(ByVal paramFileName As String)
        Dim sp_Delete As SqlClient.SqlCommand
        If Not Me.m_PersistConnection Then Me.OpenConnection()

        sp_Delete = New SqlClient.SqlCommand("DeleteParamFile", Me.m_DBCn)
        sp_Delete.CommandType = CommandType.StoredProcedure

        Dim myParam As SqlClient.SqlParameter

        myParam = sp_Delete.Parameters.Add("@Return", SqlDbType.Int)
        myParam.Direction = ParameterDirection.ReturnValue

        myParam = sp_Delete.Parameters.Add("@ParamFileName", SqlDbType.VarChar, 255)
        myParam.Direction = ParameterDirection.Input
        myParam.Value = paramFileName

        myParam = sp_Delete.Parameters.Add("@message", SqlDbType.VarChar, 512)
        myParam.Direction = ParameterDirection.Output

        sp_Delete.ExecuteNonQuery()

        Dim ret As Integer = CInt(sp_Delete.Parameters("@Return").Value)

        If ret <> 0 Then
            Me.m_SPError = CStr(sp_Delete.Parameters("@message").Value)

        End If

        If Not Me.m_PersistConnection Then Me.CloseConnection()
    End Sub

    Protected Sub RunSP_DeleteParamEntries(ByVal paramFileName As String)
        Dim paramFileID As Integer

        paramFileID = Me.RunSP_GetParamFileID(paramFileName)

        Me.RunSP_DeleteParamEntries(paramFileID)

    End Sub

    Protected Sub RunSP_DeleteParamEntries(ByVal paramFileID As Integer)
        Dim sp_Delete As SqlClient.SqlCommand


        If Not Me.m_PersistConnection Then Me.OpenConnection()

        sp_Delete = New SqlClient.SqlCommand("DeleteParamEntriesForID", Me.m_DBCn)
        sp_Delete.CommandType = CommandType.StoredProcedure

        Dim myparam As SqlClient.SqlParameter

        myparam = sp_Delete.Parameters.Add("@Return", SqlDbType.Int)
        myparam.Direction = ParameterDirection.ReturnValue

        myparam = sp_Delete.Parameters.Add("@ParamFileID", SqlDbType.Int)
        myparam.Direction = ParameterDirection.Input
        myparam.Value = paramFileID

        myparam = sp_Delete.Parameters.Add("@message", SqlDbType.VarChar, 512)
        myparam.Direction = ParameterDirection.Output


        sp_Delete.ExecuteNonQuery()

        Dim ret As Integer = CInt(sp_Delete.Parameters("@Return").Value)

        If ret <> 0 Then
            Me.m_SPError = CStr(sp_Delete.Parameters("@message").Value)

        End If

        If Not Me.m_PersistConnection Then Me.CloseConnection()

    End Sub

    Protected Function RunSP_GetParamFileID(ByVal paramFileName As String) As Integer
        Dim sp_Lookup As SqlClient.SqlCommand


        If Not Me.m_PersistConnection Then Me.OpenConnection()

        sp_Lookup = New SqlClient.SqlCommand("GetParamFileID", Me.m_DBCn)
        sp_Lookup.CommandType = CommandType.StoredProcedure

        Dim myparam As SqlClient.SqlParameter

        myparam = sp_Lookup.Parameters.Add("@Return", SqlDbType.Int)
        myparam.Direction = ParameterDirection.ReturnValue

        myparam = sp_Lookup.Parameters.Add("@ParamFileName", SqlDbType.VarChar, 255)
        myparam.Direction = ParameterDirection.Input
        myparam.Value = paramFileName

        sp_Lookup.ExecuteNonQuery()

        If Not Me.m_PersistConnection Then Me.CloseConnection()
        Return CInt(sp_Lookup.Parameters("@Return").Value)
    End Function

#End Region
End Class
