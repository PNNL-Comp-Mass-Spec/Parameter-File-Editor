Imports System.Collections.Specialized
Imports System.Reflection
Imports ParamFileGenerator


Friend Class clsDMSParamUpload
    Inherits ParamFileGenerator.DownloadParams.clsParamsFromDMS

#Region " Member Properties "
    'Private m_clsUpdateModsTable As clsUpdateModsTable
    Private m_SPError As String = String.Empty
    Private m_mgrParams As ParamFileEditor.ProgramSettings.IProgramSettings
    Private m_DynamicModCounter As Integer = 1
#End Region

#Region " Friend Procedures "

    Public ReadOnly Property ErrorMessage() As String
        Get
            Return m_SPError
        End Get
    End Property

    Public Sub New(ByVal mgrParams As ParamFileEditor.ProgramSettings.IProgramSettings)
        MyBase.New(mgrParams.DMS_ConnectionString)
        Me.m_mgrParams = mgrParams
        'Me.m_clsUpdateModsTable = New clsUpdateModsTable(mgrParams)
    End Sub

    Friend Function WriteParamsToDMS(ByVal ParamSet As clsParams, ByVal blnAutoGenerateDescription As Boolean) As Boolean
        Dim success As Boolean
        success = SaveParams(ParamSet, blnAutoGenerateDescription)
        Return success
    End Function

    Friend Function WriteParamsToLocalStructure(ByVal Paramset As clsParams, ByVal ParamSetIDToUpdate As Integer) As Boolean
        Return SaveParams(Paramset, True)
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

#Region "Protected & Private Functions "

    Protected Function SaveParams(ByRef ParamSet As clsParams, ByVal blnAutoGenerateDescription As Boolean) As Boolean
        Dim diff As clsDMSParamStorage.ParamsEntry
        Dim diffcollection As clsDMSParamStorage
        Dim intIndex As Integer
        Dim intUpdateFailCount As Integer
        Dim blnSuccess As Boolean

        m_SPError = String.Empty

        If blnAutoGenerateDescription OrElse ParamSet.Description Is Nothing OrElse ParamSet.Description.Length = 0 Then
            ParamSet.Description = Me.DistillFeaturesFromParamSet(ParamSet)
        End If

        blnSuccess = Me.RunSP_AddUpdateParamFile(ParamSet.FileName, ParamSet.Description)

        If Not blnSuccess Then
            If m_SPError.Length = 0 Then
                m_SPError = "RunSP_AddUpdateParamFile returned False; aborting"
            End If
            Return False
        End If

        diffcollection = Me.GetDiffColl(clsMainProcess.BaseLineParamSet, ParamSet)

        ' Lookup the param file ID for this param file
        Dim intParamFileID As Integer
        intParamFileID = Me.RunSP_GetParamFileID(ParamSet.FileName)

        If intParamFileID = 0 Then
            If m_SPError.Length = 0 Then
                m_SPError = "RunSP_GetParamFileID returned a Param_File_ID of 0; aborting"
            End If
            Return False
        End If

        blnSuccess = Me.RunSP_DeleteParamEntries(ParamSet.FileName)
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

        Me.RefreshParamsFromDMS()

        Return blnSuccess

    End Function

    Protected Sub tmpSaveMods(ByRef ParamSet As clsParams, ByRef at As IMassTweaker)
        Dim diff As clsDMSParamStorage.ParamsEntry
        Dim diffcollection As clsDMSParamStorage
        Dim counter As Integer
        Dim tmpMass As Double

        diffcollection = Me.GetDiffColl(clsMainProcess.BaseLineParamSet, ParamSet)

        Dim paramSetID As Integer = Me.RunSP_GetParamFileID(ParamSet.FileName)

        counter = 1
        For Each diff In diffcollection
            If InStr(diff.Type.ToString, "Param") = 0 Then
                If diff.Type <> clsDMSParamStorage.ParamTypes.IsotopicModification Then
                    tmpMass = at.GetTweakedMass(diff.Value)
                Else
                    tmpMass = CDbl(diff.Value)
                End If
                Me.RunSP_AddUpdateParamEntry(paramSetID, counter, diff.Type, diff.Specifier, tmpMass)
                counter += 1
            End If
        Next

    End Sub

    Protected Function RunSP_AddUpdateParamFile( _
        ByVal paramFileName As String, _
        ByVal paramFileDescription As String) As Boolean

        Const SP_NAME As String = "AddUpdateParamFile"

        Dim sp_Save As SqlClient.SqlCommand
        Dim blnSuccess As Boolean

        Try
            blnSuccess = False
            If Not Me.m_PersistConnection Then Me.OpenConnection()

            sp_Save = New SqlClient.SqlCommand(SP_NAME, Me.m_DBCn)

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
                Me.m_SPError = "Procedure " & SP_NAME & " returned a non-zero value (" & ret.ToString & ")"

                Dim strMsg As String = CStr(sp_Save.Parameters("@message").Value)
                If Not strMsg Is Nothing AndAlso strMsg.Length > 0 Then
                    Me.m_SPError &= ": " & strMsg
                End If

                blnSuccess = False
            Else
                blnSuccess = True
            End If

            If Not Me.m_PersistConnection Then Me.CloseConnection()

        Catch ex As Exception
            Me.m_SPError = "Exception calling stored procedure " & SP_NAME & ": " & ex.Message
        End Try

        Return blnSuccess

    End Function

    Protected Function RunSP_AddUpdateParamEntry( _
        ByVal paramFileID As Integer, _
        ByVal entrySeqOrder As Integer, _
        ByVal entryType As clsDMSParamStorage.ParamTypes, _
        ByVal entrySpecifier As String, _
        ByVal entryValue As String) As Boolean

        Const SP_NAME As String = "AddUpdateParamFileEntry"

        Dim sp_Save As SqlClient.SqlCommand
        Dim blnSuccess As Boolean

        Try
            blnSuccess = False
            If Not Me.m_PersistConnection Then Me.OpenConnection()

            sp_Save = New SqlClient.SqlCommand(SP_NAME, Me.m_DBCn)

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
                Me.m_SPError = "Procedure " & SP_NAME & " returned a non-zero value (" & ret.ToString & ")"

                Dim strMsg As String = CStr(sp_Save.Parameters("@message").Value)
                If Not strMsg Is Nothing AndAlso strMsg.Length > 0 Then
                    Me.m_SPError &= ": " & strMsg
                End If

                blnSuccess = False
            Else
                blnSuccess = True
            End If

            If Not Me.m_PersistConnection Then Me.CloseConnection()

        Catch ex As Exception
            Me.m_SPError = "Exception calling stored procedure " & SP_NAME & ": " & ex.Message
        End Try

        Return blnSuccess

    End Function

    Protected Function RunSP_AddUpdateGlobalModModMappingEntry( _
        ByVal paramFileID As Integer, _
        ByVal localSymbolID As Integer, _
        ByVal globalModID As Integer) As Boolean

        Const SP_NAME As String = "AddUpdateGlobalModMappingEntry"

        Dim sp_Save As SqlClient.SqlCommand
        Dim blnSuccess As Boolean

        Try
            blnSuccess = False
            If Not Me.m_PersistConnection Then Me.OpenConnection()

            sp_Save = New SqlClient.SqlCommand(SP_NAME, Me.m_DBCn)

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
                Me.m_SPError = "Procedure " & SP_NAME & " returned a non-zero value (" & ret.ToString & ")"

                Dim strMsg As String = CStr(sp_Save.Parameters("@message").Value)
                If Not strMsg Is Nothing AndAlso strMsg.Length > 0 Then
                    Me.m_SPError &= ": " & strMsg
                End If

                blnSuccess = False
            Else
                blnSuccess = True
            End If

            If Not Me.m_PersistConnection Then Me.CloseConnection()

        Catch ex As Exception
            Me.m_SPError = "Exception calling stored procedure " & SP_NAME & ": " & ex.Message
        End Try

        Return blnSuccess


    End Function

    Protected Function RunSP_DeleteParamFile(ByVal paramFileName As String) As Boolean

        Const SP_NAME As String = "DeleteParamFile"

        Dim sp_Delete As SqlClient.SqlCommand
        Dim blnSuccess As Boolean

        Try
            blnSuccess = False
            If Not Me.m_PersistConnection Then Me.OpenConnection()

            sp_Delete = New SqlClient.SqlCommand(SP_NAME, Me.m_DBCn)
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
                Me.m_SPError = "Procedure " & SP_NAME & " returned a non-zero value (" & ret.ToString & ")"

                Dim strMsg As String = CStr(sp_Delete.Parameters("@message").Value)
                If Not strMsg Is Nothing AndAlso strMsg.Length > 0 Then
                    Me.m_SPError &= ": " & strMsg
                End If

                blnSuccess = False
            Else
                blnSuccess = True
            End If

            If Not Me.m_PersistConnection Then Me.CloseConnection()

        Catch ex As Exception
            Me.m_SPError = "Exception calling stored procedure " & SP_NAME & ": " & ex.Message
        End Try

        Return blnSuccess

    End Function

    Protected Function RunSP_DeleteParamEntries(ByVal paramFileName As String) As Boolean
        Dim paramFileID As Integer
        Dim blnSuccess As Boolean

        paramFileID = Me.RunSP_GetParamFileID(paramFileName)

        If paramFileID > 0 Then
            blnSuccess = Me.RunSP_DeleteParamEntries(paramFileID)
        Else
            ' Param file paramFileName doesn't exist
            ' Return True
            blnSuccess = True
        End If

        Return blnSuccess

    End Function

    Protected Function RunSP_DeleteParamEntries(ByVal paramFileID As Integer) As Boolean

        Const SP_NAME As String = "DeleteParamEntriesForID"

        Dim sp_Delete As SqlClient.SqlCommand
        Dim blnSuccess As Boolean

        Try
            blnSuccess = False
            If Not Me.m_PersistConnection Then Me.OpenConnection()

            sp_Delete = New SqlClient.SqlCommand(SP_NAME, Me.m_DBCn)
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
                Me.m_SPError = "Procedure " & SP_NAME & " returned a non-zero value (" & ret.ToString & ")"

                Dim strMsg As String = CStr(sp_Delete.Parameters("@message").Value)
                If Not strMsg Is Nothing AndAlso strMsg.Length > 0 Then
                    Me.m_SPError &= ": " & strMsg
                End If

                blnSuccess = False
            Else
                blnSuccess = True
            End If

            If Not Me.m_PersistConnection Then Me.CloseConnection()

        Catch ex As Exception
            Me.m_SPError = "Exception calling stored procedure " & SP_NAME & ": " & ex.Message
        End Try

        Return blnSuccess

    End Function

    Protected Function RunSP_GetParamFileID(ByVal paramFileName As String) As Integer


        Const SP_NAME As String = "GetParamFileID"

        Dim sp_Lookup As SqlClient.SqlCommand
        Dim ret As Integer

        Try
            If Not Me.m_PersistConnection Then Me.OpenConnection()

            sp_Lookup = New SqlClient.SqlCommand(SP_NAME, Me.m_DBCn)
            sp_Lookup.CommandType = CommandType.StoredProcedure

            Dim myparam As SqlClient.SqlParameter

            myparam = sp_Lookup.Parameters.Add("@Return", SqlDbType.Int)
            myparam.Direction = ParameterDirection.ReturnValue

            myparam = sp_Lookup.Parameters.Add("@ParamFileName", SqlDbType.VarChar, 255)
            myparam.Direction = ParameterDirection.Input
            myparam.Value = paramFileName

            sp_Lookup.ExecuteNonQuery()

            ret = CInt(sp_Lookup.Parameters("@Return").Value)

            If Not Me.m_PersistConnection Then Me.CloseConnection()

        Catch ex As Exception
            Me.m_SPError = "Exception calling stored procedure " & SP_NAME & ": " & ex.Message
        End Try

        Return ret

    End Function

#End Region
End Class
