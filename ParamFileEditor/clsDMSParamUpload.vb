Imports System.Collections.Specialized
Imports System.Reflection
Imports ParamFileGenerator


Friend Class clsDMSParamUpload
    Inherits ParamFileGenerator.DownloadParams.clsparamsfromdms

#Region " Member Properties "
    Private m_clsUpdateModsTable As clsUpdateModsTable
#End Region

#Region " Friend Properties "

#End Region

#Region " Friend Procedures "

    Public Sub New(ByVal mgrParams As ParamFileEditor.ProgramSettings.IProgramSettings)
        MyBase.New(mgrParams.DMS_ConnectionString)
        Me.m_clsUpdateModsTable = New clsUpdateModsTable(mgrParams)
    End Sub
    Friend Function WriteParamsToDMS(ByVal ParamSet As clsParams, ByVal ParamSetIDToUpdate As Integer) As Boolean
        Dim success As Boolean = SaveParams(ParamSet, ParamSetIDToUpdate)
        Call Me.Write_T_Param_File_Table()
        Return True
    End Function

    Friend Function WriteParamsToLocalStructure(ByVal Paramset As clsParams, ByVal ParamSetIDToUpdate As Integer) As Boolean
        Return SaveParams(Paramset, ParamSetIDToUpdate)
    End Function

    Friend Sub WriteLocalStructureToDMS()
        Call Me.Write_T_Param_File_Table()
    End Sub
    Friend Function GetDiffsFromTemplate(ByVal paramSetID As Integer) As String
        Return DistillFeaturesFromParamSet(paramSetID)
    End Function

    Friend Function GetDiffsFromTemplate(ByVal paramSet As clsParams) As String
        Return DistillFeaturesFromParamSet(paramSet)
    End Function

    Friend Function GetDiffsBetweenSets(ByVal templateSet As clsParams, ByVal checkSet As clsParams) As String
        Return Me.CompareParamSets(templateSet, checkSet)
    End Function

    Friend Function GetNextParamSetID() As Integer
        Dim tmpTable As DataTable = Me.m_ParamsSet.Tables(Me.Param_File_Table)
        Return Me.GetNextParamIDValue((tmpTable), tmpTable.Columns(0))
    End Function

#End Region

#Region " Protected & Private Functions "

    Private Function GetNextParamIDValue(ByVal dt As DataTable, ByVal IDColumn As DataColumn) As Integer        'Upload
        dt.DefaultView.Sort = IDColumn.ColumnName
        Dim rowCount As Integer = dt.Rows.Count
        If rowCount = 0 Then Return 1000
        Dim maxIndex As Integer = rowCount - 1
        Dim nextID As Integer = CInt(dt.Rows(maxIndex).Item(0)) + 1
        Return nextID
    End Function
    'Private Function SaveParams(ByVal ParamSet As clsParams, ByVal ParamSetID As Integer) As Boolean        'Upload
    '    Dim FileDR As DataRow
    '    Dim idExists As Boolean = Me.ParamSetIDExists(ParamSetID)
    '    If idExists Then
    '        FileDR = Me.GetRowWithID(ParamSetID)
    '        FileDR = Me.WriteDatarowFromParamSet(ParamSet, FileDR)
    '        'dr.Item(0) = ParamSetID
    '    ElseIf Not idExists Then
    '        'dr = Me.m_ParamsTable.NewRow
    '        'dr = Me.WriteDatarowFromParamSet(ParamSet, dr)
    '        'dr.Item(0) = ParamSetID
    '        'Me.m_ParamsTable.Rows.Add(dr)
    '    End If
    '    ' Me.VerifyChangeState(Me.m_ParamsTable, ParamSetID)

    'End Function
    Private Function SaveParams(ByRef ParamSet As clsParams, ByVal ParamSetID As Integer) As Boolean
        'Check if current ID exists
        Dim idExists As Boolean = Me.ParamSetIDExists(ParamSetID)
        Dim diff As clsDMSParamStorage.ParamsEntry
        Dim diffCollection As clsDMSParamStorage
        Dim FileRows As DataRow()
        Dim FileRow As DataRow
        Dim EntryRows As DataRow()
        Dim EntryRow As DataRow
        Dim counter As Integer

        FileRows = Me.m_ParamsSet.Tables(Me.Param_File_Table).Select("[Param_File_ID] = " & ParamSetID)
        If FileRows.Length > 0 Then
            FileRow = FileRows(0)
        Else
            FileRow = Me.m_ParamsSet.Tables(Me.Param_File_Table).NewRow
            FileRow.Item("Date_Created") = System.DateTime.Now
        End If

        With FileRow
            .Item("Param_File_ID") = ParamSetID
            .Item("Param_File_Name") = ParamSet.FileName
            .Item("Param_File_Description") = ParamSet.Description
            .Item("Param_File_Type_ID") = 1000                 'Seqeuest Param File ID
            .Item("Date_Modified") = System.DateTime.Now
        End With

        With Me.m_ParamsSet.Tables(Me.Param_File_Table)
            .Rows.Add(FileRow)
            '.AcceptChanges()
        End With

        'Get differences from basic template params and Convert to storage class
        diffCollection = Me.GetDiffColl(clsMainProcess.BaseLineParamSet, ParamSet)

        'Get the affected records from the params table
        EntryRows = Me.m_ParamsSet.Tables(Me.Param_Entry_Table).Select("[Param_File_ID] = " & ParamSetID, "[Entry_Sequence_Order]")

        For Each EntryRow In EntryRows
            EntryRow.Delete()
        Next

        'Write to storage class to local data table
        counter = 1
        For Each diff In diffCollection
            EntryRow = Me.m_ParamsSet.Tables(Me.Param_Entry_Table).NewRow
            With EntryRow
                .Item("Entry_Sequence_Order") = counter
                .Item("Entry_Type") = diff.Type.ToString
                .Item("Entry_Specifier") = diff.Specifier
                .Item("Entry_Value") = diff.Value
                .Item("Entry_Parent_Type") = 1000
                .Item("Param_File_ID") = ParamSetID
            End With
            Me.m_ParamsSet.Tables(Me.Param_Entry_Table).Rows.Add(EntryRow)
            counter += 1

        Next
        'Me.m_ParamsSet.Tables(Me.Param_Entry_Table).AcceptChanges()

    End Function

    Private Sub Write_T_Param_File_Table()      'Upload

        Dim dr As DataRow
        For Each dr In Me.ParamFileTable.Rows
            Console.WriteLine(dr.Item(0).ToString & " " & dr.Item(1).ToString & ": " & dr.RowState.ToString)
        Next

        For Each dr In Me.m_ParamsSet.Tables(Me.Param_Entry_Table).Rows
            Console.WriteLine(dr.Item(0).ToString & " - " & dr.Item(1).ToString & " - " & dr.Item(2).ToString & " - " _
                & dr.Item(3).ToString & " - " & dr.Item(4).ToString & " - " & dr.Item(5).ToString & ": " & dr.RowState.ToString)
        Next

        Me.OpenConnection()
        Me.m_GetID_DA.Update(Me.m_ParamsSet.Tables(Me.Param_File_Table))
        Me.m_GetEntries_DA.Update(Me.m_ParamsSet.Tables(Me.Param_Entry_Table))

        Me.CloseConnection()

    End Sub

    'Private Sub VerifyChangeState(ByVal dt As DataTable, ByVal ParamSetID As Integer)       'upload
    '    Dim dr As DataRow
    '    For Each dr In dt.Rows
    '        If dr.RowState <> DataRowState.Unchanged And CInt(dr.Item(0)) = 1000 Then
    '            dr.RejectChanges()
    '        End If
    '    Next
    'End Sub

#End Region
End Class
