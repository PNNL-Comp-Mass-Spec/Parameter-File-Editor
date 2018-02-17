Imports System.Data.SqlClient
Imports ParamFileEditor.ProgramSettings
Imports ParamFileGenerator
Imports ParamFileGenerator.DownloadParams


Friend Class clsDMSParamUpload
    Inherits clsParamsFromDMS

#Region " Member Properties "
    'Private m_clsUpdateModsTable As clsUpdateModsTable
    Private m_SPError As String = String.Empty

#End Region

#Region " Friend Procedures "

    Public ReadOnly Property ErrorMessage() As String
        Get
            Return m_SPError
        End Get
    End Property

    Public Sub New(mgrParams As IProgramSettings)
        MyBase.New(mgrParams.DMS_ConnectionString)
        'm_mgrParams = mgrParams
        'm_clsUpdateModsTable = New clsUpdateModsTable(mgrParams)
    End Sub

    Friend Function WriteParamsToDMS(ParamSet As clsParams, blnAutoGenerateDescription As Boolean) As Boolean
        Dim success As Boolean
        success = SaveParams(ParamSet, blnAutoGenerateDescription)
        Return success
    End Function

    Friend Function WriteParamsToLocalStructure(Paramset As clsParams, ParamSetIDToUpdate As Integer) As Boolean
        Return SaveParams(Paramset, True)
    End Function

    Friend Function GetDiffsFromTemplate(paramSetID As Integer, eParamFileTypeID As eParamFileTypeConstants) As String
        Return DistillFeaturesFromParamSet(paramSetID, eParamFileTypeID)
    End Function

    Friend Function GetDiffsFromTemplate(paramSet As clsParams) As String
        Return DistillFeaturesFromParamSet(paramSet)
    End Function

    Friend Function GetDiffsBetweenSets(templateSet As clsParams, checkSet As clsParams) As String
        Return CompareParamSets(templateSet, checkSet)
    End Function

    Friend Sub AddParamFileMappingEntry(
        paramFileID As Integer,
        entrySeqOrder As Integer,
        entryTypeString As String,
        entrySpecifier As String,
        entryValue As String)

        Dim entryType As clsDMSParamStorage.ParamTypes

        entryType = DirectCast([Enum].Parse(GetType(clsDMSParamStorage.ParamTypes), entryTypeString), clsDMSParamStorage.ParamTypes)


        RunSP_AddUpdateParamEntry(paramFileID, entrySeqOrder, entryType, entrySpecifier, entryValue)

    End Sub

    Friend Function GetParamsSet(paramSetID As Integer, Optional DisableMassLookup As Boolean = False) As clsParams
        Return GetParamSetWithID(paramSetID, eParamFileTypeConstants.Sequest, DisableMassLookup)
    End Function

#End Region

#Region "Protected & Private Functions "

    Protected Function SaveParams(ByRef ParamSet As clsParams, blnAutoGenerateDescription As Boolean) As Boolean
        Dim diff As clsDMSParamStorage.ParamsEntry
        Dim diffcollection As clsDMSParamStorage
        Dim intIndex As Integer
        Dim intUpdateFailCount As Integer
        Dim blnSuccess As Boolean

        m_SPError = String.Empty

        If blnAutoGenerateDescription OrElse ParamSet.Description Is Nothing OrElse ParamSet.Description.Length = 0 Then
            ParamSet.Description = DistillFeaturesFromParamSet(ParamSet)
        End If

        blnSuccess = RunSP_AddUpdateParamFile(ParamSet.FileName, ParamSet.Description)

        If Not blnSuccess Then
            If m_SPError.Length = 0 Then
                m_SPError = "RunSP_AddUpdateParamFile returned False; aborting"
            End If
            Return False
        End If

        diffcollection = GetDiffColl(clsMainProcess.BaseLineParamSet, ParamSet)

        ' Lookup the param file ID for this param file
        Dim intParamFileID As Integer
        intParamFileID = RunSP_GetParamFileID(ParamSet.FileName)

        If intParamFileID = 0 Then
            If m_SPError.Length = 0 Then
                m_SPError = "RunSP_GetParamFileID returned a Param_File_ID of 0; aborting"
            End If
            Return False
        End If

        blnSuccess = RunSP_DeleteParamEntries(ParamSet.FileName)
        If Not blnSuccess Then
            If m_SPError.Length = 0 Then
                m_SPError = "RunSP_DeleteParamEntries returned False; aborting"
            End If
            Return False
        End If

        intIndex = 1
        intUpdateFailCount = 0
        For Each diff In diffcollection
            blnSuccess = RunSP_AddUpdateParamEntry(intParamFileID, intIndex, diff.Type, diff.Specifier, diff.Value)
            If Not blnSuccess Then
                intUpdateFailCount += 1
            End If
            intIndex += 1
        Next

        If intUpdateFailCount > 0 Then

            Dim strErrMsg As String
            strErrMsg = "RunSP_AddUpdateParamEntry returned False for " & intUpdateFailCount.ToString & " of the " & (intIndex - 1).ToString & " parameters"

            If m_SPError.Length = 0 Then
                m_SPError = strErrMsg
            Else
                m_SPError = strErrMsg & "; " & m_SPError
            End If


            blnSuccess = False
        End If

        RefreshParamsFromDMS()

        Return blnSuccess

    End Function

    Protected Sub tmpSaveMods(ByRef ParamSet As clsParams, ByRef at As IMassTweaker)
        Dim diff As clsDMSParamStorage.ParamsEntry
        Dim diffcollection As clsDMSParamStorage
        Dim counter As Integer
        Dim tmpMass As Double

        diffcollection = GetDiffColl(clsMainProcess.BaseLineParamSet, ParamSet)

        Dim paramSetID As Integer = RunSP_GetParamFileID(ParamSet.FileName)

        counter = 1
        For Each diff In diffcollection
            If InStr(diff.Type.ToString, "Param") = 0 Then
                If diff.Type <> clsDMSParamStorage.ParamTypes.IsotopicModification Then
                    tmpMass = at.GetTweakedMass(CDbl(diff.Value))
                Else
                    tmpMass = CDbl(diff.Value)
                End If
                RunSP_AddUpdateParamEntry(paramSetID, counter, diff.Type, diff.Specifier, tmpMass.ToString("0.00000"))
                counter += 1
            End If
        Next

    End Sub

    Protected Function RunSP_AddUpdateParamFile(
        paramFileName As String,
        paramFileDescription As String) As Boolean

        Const SP_NAME = "AddUpdateParamFile"

        Dim sp_Save As SqlCommand
        Dim blnSuccess As Boolean

        Try
            blnSuccess = False
            If Not m_PersistConnection Then OpenConnection()

            sp_Save = New SqlCommand(SP_NAME, m_DBCn)

            sp_Save.CommandType = CommandType.StoredProcedure

            'Define parameters
            Dim myParam As SqlParameter

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
            Dim ret = CInt(sp_Save.Parameters("@Return").Value)

            If ret <> 0 Then
                m_SPError = "Procedure " & SP_NAME & " returned a non-zero value (" & ret.ToString & ")"

                Dim strMsg = CStr(sp_Save.Parameters("@message").Value)
                If Not strMsg Is Nothing AndAlso strMsg.Length > 0 Then
                    m_SPError &= ": " & strMsg
                End If

                blnSuccess = False
            Else
                blnSuccess = True
            End If

            If Not m_PersistConnection Then CloseConnection()

        Catch ex As Exception
            m_SPError = "Exception calling stored procedure " & SP_NAME & ": " & ex.Message
        End Try

        Return blnSuccess

    End Function

    Protected Function RunSP_AddUpdateParamEntry(
        paramFileID As Integer,
        entrySeqOrder As Integer,
        entryType As clsDMSParamStorage.ParamTypes,
        entrySpecifier As String,
        entryValue As String) As Boolean

        Const SP_NAME = "AddUpdateParamFileEntry"

        Dim sp_Save As SqlCommand
        Dim blnSuccess As Boolean

        Try
            blnSuccess = False
            If Not m_PersistConnection Then OpenConnection()

            sp_Save = New SqlCommand(SP_NAME, m_DBCn)

            sp_Save.CommandType = CommandType.StoredProcedure

            'Define parameters
            Dim myParam As SqlParameter

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
            Dim ret = CInt(sp_Save.Parameters("@Return").Value)

            If ret <> 0 Then
                m_SPError = "Procedure " & SP_NAME & " returned a non-zero value (" & ret.ToString & ")"

                Dim strMsg = CStr(sp_Save.Parameters("@message").Value)
                If Not strMsg Is Nothing AndAlso strMsg.Length > 0 Then
                    m_SPError &= ": " & strMsg
                End If

                blnSuccess = False
            Else
                blnSuccess = True
            End If

            If Not m_PersistConnection Then CloseConnection()

        Catch ex As Exception
            m_SPError = "Exception calling stored procedure " & SP_NAME & ": " & ex.Message
        End Try

        Return blnSuccess

    End Function

    ''Protected Function RunSP_AddUpdateGlobalModModMappingEntry( _
    ''    paramFileID As Integer, _
    ''    localSymbolID As Integer, _
    ''    globalModID As Integer) As Boolean

    ''    Const SP_NAME As String = "AddUpdateGlobalModMappingEntry"

    ''    Dim sp_Save As SqlClient.SqlCommand
    ''    Dim blnSuccess As Boolean

    ''    Try
    ''        blnSuccess = False
    ''        If Not m_PersistConnection Then OpenConnection()

    ''        sp_Save = New SqlClient.SqlCommand(SP_NAME, m_DBCn)

    ''        sp_Save.CommandType = CommandType.StoredProcedure

    ''        'Define parameters
    ''        Dim myParam As SqlClient.SqlParameter

    ''        'Define parameter for sp's return value
    ''        myParam = sp_Save.Parameters.Add("@Return", SqlDbType.Int)
    ''        myParam.Direction = ParameterDirection.ReturnValue

    ''        'Define parameters for the sp's arguments
    ''        myParam = sp_Save.Parameters.Add("@paramFileID", SqlDbType.Int)
    ''        myParam.Direction = ParameterDirection.Input
    ''        myParam.Value = paramFileID

    ''        myParam = sp_Save.Parameters.Add("@globalModID", SqlDbType.Int)
    ''        myParam.Direction = ParameterDirection.Input
    ''        myParam.Value = globalModID

    ''        myParam = sp_Save.Parameters.Add("@localSymbolID", SqlDbType.Int)
    ''        myParam.Direction = ParameterDirection.Input
    ''        myParam.Value = localSymbolID

    ''        myParam = sp_Save.Parameters.Add("@message", SqlDbType.VarChar, 512)
    ''        myParam.Direction = ParameterDirection.Output


    ''        'Execute the sp
    ''        sp_Save.ExecuteNonQuery()

    ''        'Get return value
    ''        Dim ret As Integer = CInt(sp_Save.Parameters("@Return").Value)

    ''        If ret <> 0 Then
    ''            m_SPError = "Procedure " & SP_NAME & " returned a non-zero value (" & ret.ToString & ")"

    ''            Dim strMsg As String = CStr(sp_Save.Parameters("@message").Value)
    ''            If Not strMsg Is Nothing AndAlso strMsg.Length > 0 Then
    ''                m_SPError &= ": " & strMsg
    ''            End If

    ''            blnSuccess = False
    ''        Else
    ''            blnSuccess = True
    ''        End If

    ''        If Not m_PersistConnection Then CloseConnection()

    ''    Catch ex As Exception
    ''        m_SPError = "Exception calling stored procedure " & SP_NAME & ": " & ex.Message
    ''    End Try

    ''    Return blnSuccess


    ''End Function

    Protected Function RunSP_DeleteParamFile(paramFileName As String) As Boolean

        Const SP_NAME = "DeleteParamFile"

        Dim sp_Delete As SqlCommand
        Dim blnSuccess As Boolean

        Try
            blnSuccess = False
            If Not m_PersistConnection Then OpenConnection()

            sp_Delete = New SqlCommand(SP_NAME, m_DBCn)
            sp_Delete.CommandType = CommandType.StoredProcedure

            Dim myParam As SqlParameter

            myParam = sp_Delete.Parameters.Add("@Return", SqlDbType.Int)
            myParam.Direction = ParameterDirection.ReturnValue

            myParam = sp_Delete.Parameters.Add("@ParamFileName", SqlDbType.VarChar, 255)
            myParam.Direction = ParameterDirection.Input
            myParam.Value = paramFileName

            myParam = sp_Delete.Parameters.Add("@message", SqlDbType.VarChar, 512)
            myParam.Direction = ParameterDirection.Output

            sp_Delete.ExecuteNonQuery()

            Dim ret = CInt(sp_Delete.Parameters("@Return").Value)

            If ret <> 0 Then
                m_SPError = "Procedure " & SP_NAME & " returned a non-zero value (" & ret.ToString & ")"

                Dim strMsg = CStr(sp_Delete.Parameters("@message").Value)
                If Not strMsg Is Nothing AndAlso strMsg.Length > 0 Then
                    m_SPError &= ": " & strMsg
                End If

                blnSuccess = False
            Else
                blnSuccess = True
            End If

            If Not m_PersistConnection Then CloseConnection()

        Catch ex As Exception
            m_SPError = "Exception calling stored procedure " & SP_NAME & ": " & ex.Message
        End Try

        Return blnSuccess

    End Function

    Protected Function RunSP_DeleteParamEntries(paramFileName As String) As Boolean
        Dim paramFileID As Integer
        Dim blnSuccess As Boolean

        paramFileID = RunSP_GetParamFileID(paramFileName)

        If paramFileID > 0 Then
            blnSuccess = RunSP_DeleteParamEntries(paramFileID)
        Else
            ' Param file paramFileName doesn't exist
            ' Return True
            blnSuccess = True
        End If

        Return blnSuccess

    End Function

    Protected Function RunSP_DeleteParamEntries(paramFileID As Integer) As Boolean

        Const SP_NAME = "DeleteParamEntriesForID"

        Dim sp_Delete As SqlCommand
        Dim blnSuccess As Boolean

        Try
            blnSuccess = False
            If Not m_PersistConnection Then OpenConnection()

            sp_Delete = New SqlCommand(SP_NAME, m_DBCn)
            sp_Delete.CommandType = CommandType.StoredProcedure

            Dim myparam As SqlParameter

            myparam = sp_Delete.Parameters.Add("@Return", SqlDbType.Int)
            myparam.Direction = ParameterDirection.ReturnValue

            myparam = sp_Delete.Parameters.Add("@ParamFileID", SqlDbType.Int)
            myparam.Direction = ParameterDirection.Input
            myparam.Value = paramFileID

            myparam = sp_Delete.Parameters.Add("@message", SqlDbType.VarChar, 512)
            myparam.Direction = ParameterDirection.Output


            sp_Delete.ExecuteNonQuery()

            Dim ret = CInt(sp_Delete.Parameters("@Return").Value)

            If ret <> 0 Then
                m_SPError = "Procedure " & SP_NAME & " returned a non-zero value (" & ret.ToString & ")"

                Dim strMsg = CStr(sp_Delete.Parameters("@message").Value)
                If Not strMsg Is Nothing AndAlso strMsg.Length > 0 Then
                    m_SPError &= ": " & strMsg
                End If

                blnSuccess = False
            Else
                blnSuccess = True
            End If

            If Not m_PersistConnection Then CloseConnection()

        Catch ex As Exception
            m_SPError = "Exception calling stored procedure " & SP_NAME & ": " & ex.Message
        End Try

        Return blnSuccess

    End Function

    Protected Function RunSP_GetParamFileID(paramFileName As String) As Integer


        Const SP_NAME = "GetParamFileID"

        Dim sp_Lookup As SqlCommand
        Dim ret As Integer

        Try
            If Not m_PersistConnection Then OpenConnection()

            sp_Lookup = New SqlCommand(SP_NAME, m_DBCn)
            sp_Lookup.CommandType = CommandType.StoredProcedure

            Dim myparam As SqlParameter

            myparam = sp_Lookup.Parameters.Add("@Return", SqlDbType.Int)
            myparam.Direction = ParameterDirection.ReturnValue

            myparam = sp_Lookup.Parameters.Add("@ParamFileName", SqlDbType.VarChar, 255)
            myparam.Direction = ParameterDirection.Input
            myparam.Value = paramFileName

            sp_Lookup.ExecuteNonQuery()

            ret = CInt(sp_Lookup.Parameters("@Return").Value)

            If Not m_PersistConnection Then CloseConnection()

        Catch ex As Exception
            m_SPError = "Exception calling stored procedure " & SP_NAME & ": " & ex.Message
        End Try

        Return ret

    End Function

#End Region
End Class
