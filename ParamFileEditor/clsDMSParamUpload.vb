Imports ParamFileGenerator
Imports ParamFileGenerator.DownloadParams
Imports PRISMDatabaseUtils


Friend Class clsDMSParamUpload
    Inherits clsParamsFromDMS

#Region "Member Properties"
    'Private m_clsUpdateModsTable As clsUpdateModsTable
    Private m_SPError As String = String.Empty

#End Region

#Region "Friend Procedures"

    Public ReadOnly Property ErrorMessage As String
        Get
            Return m_SPError
        End Get
    End Property

    Public Sub New(dbTools As IDBTools)
        MyBase.New(dbTools)
        'm_mgrParams = mgrParams
        'm_clsUpdateModsTable = New clsUpdateModsTable(mgrParams)
    End Sub

    Friend Function WriteParamsToDMS(paramSet As clsParams, blnAutoGenerateDescription As Boolean) As Boolean
        Dim success As Boolean
        success = SaveParams(paramSet, blnAutoGenerateDescription)
        Return success
    End Function

    Friend Function WriteParamsToLocalStructure(paramSet As clsParams, paramSetIDToUpdate As Integer) As Boolean
        Return SaveParams(paramSet, True)
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

    Protected Function SaveParams(ByRef paramSet As clsParams, blnAutoGenerateDescription As Boolean) As Boolean
        Dim diff As clsDMSParamStorage.ParamsEntry
        Dim diffCollection As clsDMSParamStorage
        Dim intIndex As Integer
        Dim intUpdateFailCount As Integer
        Dim success As Boolean

        m_SPError = String.Empty

        If blnAutoGenerateDescription OrElse paramSet.Description Is Nothing OrElse paramSet.Description.Length = 0 Then
            paramSet.Description = DistillFeaturesFromParamSet(paramSet)
        End If

        success = RunSP_AddUpdateParamFile(paramSet.FileName, paramSet.Description)

        If Not success Then
            If m_SPError.Length = 0 Then
                m_SPError = "RunSP_AddUpdateParamFile returned False; aborting"
            End If
            Return False
        End If

        diffCollection = GetDiffColl(clsMainProcess.BaseLineParamSet, paramSet)

        ' Lookup the param file ID for this param file
        Dim paramFileID = RunSP_GetParamFileID(paramSet.FileName)

        If paramFileID = 0 Then
            If m_SPError.Length = 0 Then
                m_SPError = "RunSP_GetParamFileID returned a Param_File_ID of 0; aborting"
            End If
            Return False
        End If

        success = RunSP_DeleteParamEntries(paramSet.FileName)
        If Not success Then
            If m_SPError.Length = 0 Then
                m_SPError = "RunSP_DeleteParamEntries returned False; aborting"
            End If
            Return False
        End If

        intIndex = 1
        intUpdateFailCount = 0
        For Each diff In diffCollection
            success = RunSP_AddUpdateParamEntry(paramFileID, intIndex, diff.Type, diff.Specifier, diff.Value)
            If Not success Then
                intUpdateFailCount += 1
            End If
            intIndex += 1
        Next

        If intUpdateFailCount > 0 Then

            Dim strErrMsg As String
            strErrMsg = "RunSP_AddUpdateParamEntry returned False for " & intUpdateFailCount.ToString() & " of the " & (intIndex - 1).ToString & " parameters"

            If m_SPError.Length = 0 Then
                m_SPError = strErrMsg
            Else
                m_SPError = strErrMsg & "; " & m_SPError
            End If


            success = False
        End If

        RefreshParamsFromDMS()

        Return success

    End Function

    Protected Sub tmpSaveMods(ByRef paramSet As clsParams, ByRef at As IMassTweaker)
        Dim diff As clsDMSParamStorage.ParamsEntry
        Dim diffCollection As clsDMSParamStorage
        Dim counter As Integer
        Dim tmpMass As Double

        diffCollection = GetDiffColl(clsMainProcess.BaseLineParamSet, paramSet)

        Dim paramSetID As Integer = RunSP_GetParamFileID(paramSet.FileName)

        counter = 1
        For Each diff In diffCollection
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

        Try

            Dim cmdSave = mDBTools.CreateCommand(SP_NAME, CommandType.StoredProcedure)

            ' Define the stored procedure return value
            mDBTools.AddParameter(cmdSave, "@Return", SqlType.Int).Direction = ParameterDirection.ReturnValue

            ' Define parameters for the stored procedure arguments
            mDBTools.AddParameter(cmdSave, "@paramFileName", SqlType.VarChar, 255).Value = paramFileName

            mDBTools.AddParameter(cmdSave, "@paramFileDesc", SqlType.VarChar, 1024).Value = paramFileDescription

            mDBTools.AddParameter(cmdSave, "@ParamFileTypeID", SqlType.Int).Value = 1000

            Dim messageParam = mDBTools.AddParameter(cmdSave, "@message", SqlType.VarChar, 512, ParameterDirection.Output)

            Dim errMsg As String = String.Empty

            ' Execute the stored procedure
            Dim returnValue = mDBTools.ExecuteSP(cmdSave, errMsg)

            If returnValue = 0 Then
                Return True
            End If

            m_SPError = "Procedure " & SP_NAME & " returned a non-zero value (" & returnValue.ToString() & ")"

            Dim strMsg = messageParam.CastDBVal(String.Empty)

            If Not String.IsNullOrWhiteSpace(strMsg) Then
                m_SPError &= ": " & strMsg
            End If

            Return False

        Catch ex As Exception
            m_SPError = "Exception calling stored procedure " & SP_NAME & ": " & ex.Message
            Return False
        End Try

    End Function

    Protected Function RunSP_AddUpdateParamEntry(
        paramFileID As Integer,
        entrySeqOrder As Integer,
        entryType As clsDMSParamStorage.ParamTypes,
        entrySpecifier As String,
        entryValue As String) As Boolean

        Const SP_NAME = "AddUpdateParamFileEntry"

        Try

            Dim cmdSave = mDBTools.CreateCommand(SP_NAME, CommandType.StoredProcedure)

            ' Define the stored procedure return value
            mDBTools.AddParameter(cmdSave, "@Return", SqlType.Int, ParameterDirection.ReturnValue)

            ' Define parameters for the stored procedure arguments
            mDBTools.AddParameter(cmdSave, "@paramFileID", SqlType.Int).Value = paramFileID

            mDBTools.AddParameter(cmdSave, "@entrySeqOrder", SqlType.Int).Value = entrySeqOrder

            mDBTools.AddParameter(cmdSave, "@entryType", SqlType.VarChar, 32).Value = entryType.ToString()

            mDBTools.AddParameter(cmdSave, "@entrySpecifier", SqlType.VarChar, 32).Value = entrySpecifier

            mDBTools.AddParameter(cmdSave, "@entryValue", SqlType.VarChar, 32).Value = entryValue

            mDBTools.AddParameter(cmdSave, "@mode", SqlType.VarChar, 12).Value = "Add"

            Dim messageParam = mDBTools.AddParameter(cmdSave, "@message", SqlType.VarChar, 512, ParameterDirection.Output)

            Dim errMsg As String = String.Empty

            ' Execute the stored procedure
            Dim returnValue = mDBTools.ExecuteSP(cmdSave, errMsg)

            If returnValue = 0 Then
                Return True
            End If

            m_SPError = "Procedure " & SP_NAME & " returned a non-zero value (" & returnValue.ToString & ")"

            Dim strMsg = messageParam.CastDBVal(String.Empty)
            If Not String.IsNullOrWhiteSpace(strMsg) Then
                m_SPError &= ": " & strMsg
            End If

            Return False

        Catch ex As Exception
            m_SPError = "Exception calling stored procedure " & SP_NAME & ": " & ex.Message
            Return False
        End Try


    End Function

    Protected Function RunSP_DeleteParamEntries(paramFileName As String) As Boolean
        Dim paramFileID As Integer
        Dim success As Boolean

        paramFileID = RunSP_GetParamFileID(paramFileName)

        If paramFileID > 0 Then
            success = RunSP_DeleteParamEntries(paramFileID)
        Else
            ' Param file paramFileName doesn't exist
            ' Return True
            success = True
        End If

        Return success

    End Function

    Protected Function RunSP_DeleteParamEntries(paramFileID As Integer) As Boolean

        Const SP_NAME = "DeleteParamEntriesForID"

        Try
            Dim cmdDelete = mDBTools.CreateCommand(SP_NAME, CommandType.StoredProcedure)

            mDBTools.AddParameter(cmdDelete, "@Return", SqlType.Int, ParameterDirection.ReturnValue)

            mDBTools.AddParameter(cmdDelete, "@ParamFileID", SqlType.Int).Value = paramFileID

            Dim messageParam = mDBTools.AddParameter(cmdDelete, "@message", SqlType.VarChar, 512, ParameterDirection.Output)

            Dim errMsg As String = String.Empty
            Dim returnValue = mDBTools.ExecuteSP(cmdDelete, errMsg)

            If returnValue = 0 Then
                Return True
            End If

            m_SPError = "Procedure " & SP_NAME & " returned a non-zero value (" & returnValue.ToString & ")"

            Dim strMsg = messageParam.CastDBVal(String.Empty)

            If Not String.IsNullOrWhiteSpace(strMsg) Then
                m_SPError &= ": " & strMsg
            End If

            Return False

        Catch ex As Exception
            m_SPError = "Exception calling stored procedure " & SP_NAME & ": " & ex.Message
            Return False
        End Try

    End Function

    Protected Function RunSP_GetParamFileID(paramFileName As String) As Integer

        Const SP_NAME = "GetParamFileID"

        Try
            Dim lookupCmd = mDBTools.CreateCommand(SP_NAME, CommandType.StoredProcedure)

            mDBTools.AddParameter(lookupCmd, "@Return", SqlType.Int, ParameterDirection.ReturnValue)

            mDBTools.AddParameter(lookupCmd, "@ParamFileName", SqlType.VarChar, 255).Value = paramFileName

            Dim errMsg As String = String.Empty

            Dim returnValue = mDBTools.ExecuteSP(lookupCmd, errMsg)
            Return returnValue

        Catch ex As Exception
            m_SPError = "Exception calling stored procedure " & SP_NAME & ": " & ex.Message
            Return 0
        End Try

    End Function

#End Region
End Class
