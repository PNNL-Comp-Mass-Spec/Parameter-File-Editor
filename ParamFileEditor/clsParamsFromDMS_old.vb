Imports System.Collections.Specialized
Imports System.Reflection
Imports ParamFileEditor.SequestParams
Imports ParamFileEditor.ProgramSettings


Namespace DownloadParams

    Public Class clsParamsFromDMS
        Inherits clsDBTask
#Region " Constants "
        Private Const Param_Table = "T_Sequest_Params"
        Private Const Param_FileTypes_Table = "T_Sequest_Params_FileType_Name"
        Private Const param_Class_Table = "T_Sequest_Params_Class_Name"
        Private Const param_MassType_Table = "T_Sequest_Params_MassType_Name"
#End Region

#Region " Member Properties "
        Private m_ID As Integer
        Private m_Name As String
        Private m_Params As clsParams
        Private m_ParamsTable As DataTable
        Private m_ParamsSet As DataSet
        Private m_ParamSetCount As Integer
#End Region

#Region " Public Properties "
        Public ReadOnly Property ParamSetTable() As DataTable
            Get
                Return m_ParamsTable
            End Get
        End Property

        Public ReadOnly Property ParamSetCount() As Integer
            Get
                Return (m_ParamSetCount)
            End Get
        End Property
#End Region


        Public Sub New(ByVal ProgramSettings As IProgramSettings)
            MyBase.New(ProgramSettings)
            m_ParamsTable = GetParamsTable()
            m_ParamSetCount = GetParamSetTableCount(m_ParamsTable)

        End Sub

#Region " Public Functions "

        Public Function ReadParamsFromDMS(ByVal ParamSetName As String) As clsParams
            'Retrieve ID number first, then use the same procedure as below
            m_Name = ParamSetName
            m_ID = GetIDWithName(m_Name)

            m_Params = RetrieveParams(m_ID)
            Return m_Params
        End Function
        Public Function ReadParamsFromDMS(ByVal ParamSetID As Integer) As clsParams
            m_ID = ParamSetID

            m_Params = RetrieveParams(m_ID)
            Return m_Params
        End Function

        Public Function WriteParamsToDMS(ByVal ParamSet As clsParams, ByVal ParamSetIDToUpdate As Integer) As Boolean
            Dim success As Boolean = SaveParams(ParamSet, ParamSetIDToUpdate)
            Call Me.Write_T_Sequest_Params_Table()
            Return True
        End Function

        Public Function WriteParamsToLocalStructure(ByVal Paramset As clsParams, ByVal ParamSetIDToUpdate As Integer) As Boolean
            Return SaveParams(Paramset, ParamSetIDToUpdate)
        End Function

        Public Sub WriteLocalStructureToDMS()
            Call Me.Write_T_Sequest_Params_Table()
        End Sub

        Public Function RetrieveAvailableParams() As DataTable
            Return GetAvailableParamSets()
        End Function

        Public Function ParamSetNameExists(ByVal ParamSetName As String) As Boolean
            Return DoesParamSetNameExist(ParamSetName)
        End Function

        Public Function ParamSetIDExists(ByVal ParamSetID As Integer) As Boolean
            Return Me.DoesParamSetIDExist(ParamSetID)
        End Function

        Public Function GetDiffsFromTemplate(ByVal paramSetID As Integer) As String
            Return DistillFeaturesFromParamSet(paramSetID)
        End Function

        Public Function GetDiffsFromTemplate(ByVal paramSet As clsParams) As String
            Return DistillFeaturesFromParamSet(paramSet)
        End Function

        Public Function GetDiffsBetweenSets(ByVal checkSet As clsParams, ByVal templateSet As clsParams) As String
            Return Me.CompareParamSets(checkSet, templateSet)
        End Function

        Public Function GetNextParamSetID() As Integer
            Return Me.GetNextParamIDValue(Me.m_ParamsTable, Me.m_ParamsTable.Columns(0))
        End Function

        Public Function GetParamSetIDFromName(ByVal Name As String) As Integer
            Return Me.GetIDWithName(Name)
        End Function

#End Region

#Region " Member Functions "

        Private Function GetParamsTable()
            Dim SQL As String

            SQL = "SELECT * FROM " & Param_Table
            'SQL = SQL & " WHERE [SQ_ParamSet_Active] = '1'"

            Dim tmpTable As DataTable = GetTable(SQL)
            Dim pKey(0) As DataColumn
            pKey(0) = tmpTable.Columns(0)
            tmpTable.PrimaryKey = pKey

            'Me.DB_PrintTableContents(tmpTable)

            Return tmpTable

        End Function

        Private Function GetParamSetTableCount(ByVal psTable As DataTable) As Integer
            Dim count As Integer = psTable.Rows.Count
            Return count
        End Function

        Private Function RetrieveParams(ByVal ParamSetID As Integer) As clsParams
            Dim p As clsParams = GetParamSetWithID(ParamSetID)
            Return p
        End Function

        Private Function SaveParams(ByVal ParamSet As clsParams, ByVal ParamSetID As Integer) As Boolean
            'Dim rowtemplate As DataRow = GetRowWithID(1000)
            'Dim tmpRow As DataRow
            'Dim dr As DataRow = Me.m_ParamsTable.NewRow
            'dr.Item(0) = Me.GetNextParamIDValue(Me.m_ParamsTable, Me.m_ParamsTable.Columns(0))
            'dr = WriteDatarowFromParamSet(ParamSet, dr)
            'Me.m_ParamsTable.Rows.Add(dr)
            Dim dr As DataRow
            Dim idExists As Boolean = Me.ParamSetIDExists(ParamSetID)
            If idExists Then
                dr = Me.GetRowWithID(ParamSetID)
                dr = Me.WriteDatarowFromParamSet(ParamSet, dr)
                dr.Item(0) = ParamSetID
            ElseIf Not idExists Then
                dr = Me.m_ParamsTable.NewRow
                dr = Me.WriteDatarowFromParamSet(ParamSet, dr)
                dr.Item(0) = ParamSetID
                Me.m_ParamsTable.Rows.Add(dr)
            End If
            Me.VerifyChangeState(Me.m_ParamsTable, ParamSetID)

        End Function
        Private Sub Write_T_Sequest_Params_Table()
            Me.UpdateSQLServerDB(Me.m_ParamsTable)
        End Sub

        Private Sub VerifyChangeState(ByVal dt As DataTable, ByVal ParamSetID As Integer)
            Dim dr As DataRow
            For Each dr In dt.Rows
                If dr.RowState <> DataRowState.Unchanged And dr.Item(0) = 1000 Then
                    dr.RejectChanges()
                End If
            Next
        End Sub

        Private Sub DB_PrintTableContents(ByVal dt As DataTable)
            Dim dr As DataRow
            For Each dr In dt.Rows
                Console.WriteLine(dr.Item(0) & ", " & dr.Item(1) & ", State = " & dr.RowState)
            Next
        End Sub


        Private Function GetParamSetWithID(ByVal ParamSetID As Integer) As clsParams
            Dim dr As DataRow = GetRowWithID(ParamSetID)
            Dim p As clsParams = GetParamSetFromDatarow(dr)
            Return p
        End Function

        Private Function GetNextParamIDValue(ByVal dt As DataTable, ByVal IDColumn As DataColumn) As Integer
            dt.DefaultView.Sort = IDColumn.ColumnName
            Dim rowCount As Integer = dt.Rows.Count
            Dim maxIndex = rowCount - 1
            Dim nextID As Integer = dt.Rows(maxIndex).Item(0) + 1
            Return nextID
        End Function

        Private Function GetParamSetFromDatarow(ByVal ParamRow As DataRow) As clsParams

            Dim dr As DataRow
            dr = ParamRow
            Dim p As New clsParams

            With p
                .DMS_ID = dr.Item("Sequest_Param_ID").ToString
                .FileName = dr.Item("SQ_FileName").ToString
                .Description = dr.Item("SQ_Param_File_Desc").ToString
                .DefaultFASTAPath = dr.Item("SQ_Database_1").ToString
                .DefaultFASTAPath2 = dr.Item("SQ_Database_2").ToString
                .PeptideMassTolerance = CSng(dr.Item("SQ_Peptide_Mass_Tolerance"))
                .CreateOutputFiles = CBool(dr.Item("SQ_Create_Output_Files"))
                .IonSeries.Use_a_Ions = CBool(dr.Item("SQ_Ions_Use_A"))
                .IonSeries.Use_b_Ions = CBool(dr.Item("SQ_Ions_Use_B"))
                .IonSeries.Use_y_Ions = CBool(dr.Item("SQ_Ions_Use_Y"))
                .IonSeries.a_Ion_Weighting = CSng(dr.Item("SQ_Ions_a_Weight"))
                .IonSeries.b_Ion_Weighting = CSng(dr.Item("SQ_Ions_b_Weight"))
                .IonSeries.c_Ion_Weighting = CSng(dr.Item("SQ_Ions_c_Weight"))
                .IonSeries.d_Ion_Weighting = CSng(dr.Item("SQ_Ions_d_Weight"))
                .IonSeries.v_Ion_Weighting = CSng(dr.Item("SQ_Ions_v_Weight"))
                .IonSeries.w_Ion_Weighting = CSng(dr.Item("SQ_Ions_w_Weight"))
                .IonSeries.x_Ion_Weighting = CSng(dr.Item("SQ_Ions_x_Weight"))
                .IonSeries.y_Ion_Weighting = CSng(dr.Item("SQ_Ions_y_Weight"))
                .IonSeries.z_Ion_Weighting = CSng(dr.Item("SQ_Ions_z_Weight"))
                .DynamicMods.Dyn_Mod_1_AAList = dr.Item("SQ_Dyn_Mod1_AAList").ToString
                .DynamicMods.Dyn_Mod_2_AAList = dr.Item("SQ_Dyn_Mod2_AAList").ToString
                .DynamicMods.Dyn_Mod_3_AAList = dr.Item("SQ_Dyn_Mod3_AAList").ToString
                .DynamicMods.Dyn_Mod_1_MassDiff = CSng(dr.Item("SQ_Dyn_Mod1_Mass"))
                .DynamicMods.Dyn_Mod_2_MassDiff = CSng(dr.Item("SQ_Dyn_Mod2_Mass"))
                .DynamicMods.Dyn_Mod_3_MassDiff = CSng(dr.Item("SQ_Dyn_Mod3_Mass"))
                .MaximumNumAAPerDynMod = CInt(dr.Item("SQ_Max_Dyn_Mods"))
                .FragmentIonTolerance = CSng(dr.Item("SQ_Frag_Ion_Tolerance"))
                .NumberOfOutputLines = CInt(dr.Item("SQ_Num_Output_Lines"))
                .NumberOfDescriptionLines = CInt(dr.Item("SQ_Num_Desc_Lines"))
                .NumberOfResultsToProcess = CInt(dr.Item("SQ_Num_Results_Used"))
                .ShowFragmentIons = CBool(dr.Item("SQ_Show_Fragments"))
                .PrintDuplicateReferences = CBool(dr.Item("SQ_Print_Dup_Refs"))
                .SelectedEnzymeIndex = CInt(dr.Item("SQ_Enzyme_Num"))
                .SelectedNucReadingFrame = CType(dr.Item("SQ_Nuc_Reading_Frame"), IAdvancedParams.FrameList)
                .ParentMassType = CType(dr.Item("SQ_Parent_Mass_Type"), IBasicParams.MassTypeList)
                .FragmentMassType = CType(dr.Item("SQ_Fragment_Mass_Type"), IBasicParams.MassTypeList)
                .RemovePrecursorPeak = CBool(dr.Item("SQ_Rem_Precursor_Peak"))
                .IonCutoffPercentage = CSng(dr.Item("SQ_Ion_Cutoff_Value"))
                .MaximumNumberMissedCleavages = CInt(dr.Item("SQ_Max_Num_IC_Sites"))
                .MinimumProteinMassToSearch = CInt(dr.Item("SQ_Min_Mass_To_Use"))
                .MinimumProteinMassToSearch = CInt(dr.Item("SQ_Max_Mass_To_Use"))
                .MatchedPeakMassTolerance = CSng(dr.Item("SQ_Match_Peak_Tolerance"))
                .NumberOfDetectedPeaksToMatch = CInt(dr.Item("SQ_Match_Peak_Count"))
                .NumberOfAllowedDetectedPeakErrors = CInt(dr.Item("SQ_Match_Peak_Errors"))
                .AminoAcidsAllUpperCase = CBool(dr.Item("SQ_AA_In_Uppercase"))
                .PartialSequenceToMatch = dr.Item("SQ_Partial_Sequence").ToString
                .SequenceHeaderInfoToFilter = dr.Item("SQ_Seq_Header_Filter").ToString

                .StaticModificationsList.CtermPeptide = CSng(dr.Item("SQ_Stat_CT_Pep"))
                .StaticModificationsList.CtermProtein = CSng(dr.Item("SQ_Stat_CT_Prot"))
                .StaticModificationsList.NtermPeptide = CSng(dr.Item("SQ_Stat_NT_Pep"))
                .StaticModificationsList.CtermProtein = CSng(dr.Item("SQ_Stat_NT_Prot"))
                .StaticModificationsList.G_Glycine = CSng(dr.Item("SQ_Stat_G_Gly"))
                .StaticModificationsList.A_Alanine = CSng(dr.Item("SQ_Stat_A_Ala"))
                .StaticModificationsList.S_Serine = CSng(dr.Item("SQ_Stat_S_Ser"))
                .StaticModificationsList.P_Proline = CSng(dr.Item("SQ_Stat_P_Pro"))
                .StaticModificationsList.V_Valine = CSng(dr.Item("SQ_Stat_V_Val"))
                .StaticModificationsList.T_Threonine = CSng(dr.Item("SQ_Stat_T_Thr"))
                .StaticModificationsList.C_Cysteine = CSng(dr.Item("SQ_Stat_C_Cys"))
                .StaticModificationsList.L_Leucine = CSng(dr.Item("SQ_Stat_L_Leu"))
                .StaticModificationsList.I_Isoleucine = CSng(dr.Item("SQ_Stat_I_Ile"))
                .StaticModificationsList.X_LorI = CSng(dr.Item("SQ_Stat_X_LorI"))
                .StaticModificationsList.N_Asparagine = CSng(dr.Item("SQ_Stat_N_Asn"))
                .StaticModificationsList.O_Ornithine = CSng(dr.Item("SQ_Stat_O_Orn"))
                .StaticModificationsList.B_avg_NandD = CSng(dr.Item("SQ_Stat_B_avgNandD"))
                .StaticModificationsList.D_Aspartic_Acid = CSng(dr.Item("SQ_Stat_D_Asp"))
                .StaticModificationsList.Q_Glutamine = CSng(dr.Item("SQ_Stat_Q_Gln"))
                .StaticModificationsList.E_Glutamic_Acid = CSng(dr.Item("SQ_Stat_E_Glu"))
                .StaticModificationsList.M_Methionine = CSng(dr.Item("SQ_Stat_M_Met"))
                .StaticModificationsList.H_Histidine = CSng(dr.Item("SQ_Stat_H_His"))
                .StaticModificationsList.F_Phenylalanine = CSng(dr.Item("SQ_Stat_F_Phe"))
                .StaticModificationsList.R_Arginine = CSng(dr.Item("SQ_Stat_R_Arg"))
                .StaticModificationsList.Y_Tyrosine = CSng(dr.Item("SQ_Stat_Y_Tyr"))
                .StaticModificationsList.W_Tryptophan = CSng(dr.Item("SQ_Stat_W_Trp"))

            End With

            Return p

        End Function

        Private Function WriteDataCollectionFromParamSet(ByVal ParamSet As clsParams) As NameValueCollection
            Dim c As NameValueCollection = New NameValueCollection

            Dim o As Object

            Dim pType As Type = ParamSet.GetType
            Dim pProps As PropertyInfo() = pType.GetProperties((BindingFlags.Public Or BindingFlags.Instance))
            Dim pProp As PropertyInfo

            Dim tmpName As String
            Dim tmpValue As String

            For Each pProp In pProps
                tmpName = pProp.Name
                tmpValue = pProp.GetValue(ParamSet, Nothing)

                c.Add(tmpName, tmpValue.ToString)
            Next

            Return c

        End Function

        Private Function WriteDataCollectionFromDatarow(ByVal ParamRow As DataRow) As NameValueCollection
            Dim c As New NameValueCollection
            Dim index As Integer
            Dim maxIndex As Integer = ParamRow.Table.Columns.Count - 1

            Dim tmpName As String
            Dim tmpValue As String

            For index = 6 To maxIndex
                tmpName = ParamRow.Table.Columns(index).ColumnName.ToString
                tmpValue = ParamRow(index).ToString
                c.Add(tmpName, tmpValue)
            Next

            Return c
        End Function

        Private Function WriteDatarowFromParamSet(ByVal ParamSet As clsParams, ByVal DatarowTemplate As DataRow) As DataRow
            Dim tmpID As Integer
            Dim dr As DataRow = DatarowTemplate

            Dim p As clsParams
            p = ParamSet

            With p
                'dr.Item("Sequest_Param_ID") = .DMS_ID
                dr.Item("SQ_FileName") = .FileName
                dr.Item("SQ_FileType") = CInt(.FileTypeIndex) + 1000
                Console.WriteLine(dr.Item("SQ_FileType"))
                dr.Item("SQ_Param_File_Desc") = .Description
                dr.Item("SQ_Database_1") = .DefaultFASTAPath
                dr.Item("SQ_Database_2") = .DefaultFASTAPath2
                dr.Item("SQ_Peptide_Mass_Tolerance") = .PeptideMassTolerance
                dr.Item("SQ_Create_Output_Files") = .CreateOutputFiles
                dr.Item("SQ_Ions_Use_A") = .IonSeries.Use_a_Ions
                dr.Item("SQ_Ions_Use_B") = .IonSeries.Use_b_Ions
                dr.Item("SQ_Ions_Use_Y") = .IonSeries.Use_y_Ions
                dr.Item("SQ_Ions_a_Weight") = .IonSeries.a_Ion_Weighting
                dr.Item("SQ_Ions_b_Weight") = .IonSeries.b_Ion_Weighting
                dr.Item("SQ_Ions_c_Weight") = .IonSeries.c_Ion_Weighting
                dr.Item("SQ_Ions_d_Weight") = .IonSeries.d_Ion_Weighting
                dr.Item("SQ_Ions_v_Weight") = .IonSeries.v_Ion_Weighting
                dr.Item("SQ_Ions_w_Weight") = .IonSeries.w_Ion_Weighting
                dr.Item("SQ_Ions_x_Weight") = .IonSeries.x_Ion_Weighting
                dr.Item("SQ_Ions_y_Weight") = .IonSeries.y_Ion_Weighting
                dr.Item("SQ_Ions_z_Weight") = .IonSeries.z_Ion_Weighting
                dr.Item("SQ_Dyn_Mod1_AAList") = .DynamicMods.Dyn_Mod_1_AAList
                dr.Item("SQ_Dyn_Mod2_AAList") = .DynamicMods.Dyn_Mod_2_AAList
                dr.Item("SQ_Dyn_Mod3_AAList") = .DynamicMods.Dyn_Mod_3_AAList
                dr.Item("SQ_Dyn_Mod1_Mass") = Format(.DynamicMods.Dyn_Mod_1_MassDiff, "0.0000")
                dr.Item("SQ_Dyn_Mod2_Mass") = Format(.DynamicMods.Dyn_Mod_2_MassDiff, "0.0000")
                dr.Item("SQ_Dyn_Mod3_Mass") = Format(.DynamicMods.Dyn_Mod_3_MassDiff, "0.0000")
                dr.Item("SQ_Max_Dyn_Mods") = .MaximumNumAAPerDynMod
                dr.Item("SQ_Frag_Ion_Tolerance") = .FragmentIonTolerance
                dr.Item("SQ_Num_Output_Lines") = .NumberOfOutputLines
                dr.Item("SQ_Num_Desc_Lines") = .NumberOfDescriptionLines
                dr.Item("SQ_Num_Results_Used") = .NumberOfResultsToProcess
                dr.Item("SQ_Show_Fragments") = .ShowFragmentIons
                dr.Item("SQ_Print_Dup_Refs") = .PrintDuplicateReferences
                dr.Item("SQ_Enzyme_Num") = .SelectedEnzymeIndex
                dr.Item("SQ_Nuc_Reading_Frame") = CInt(.SelectedNucReadingFrame)
                dr.Item("SQ_Parent_Mass_Type") = CInt(.ParentMassType)
                dr.Item("SQ_Fragment_Mass_Type") = CInt(.FragmentMassType)
                dr.Item("SQ_Rem_Precursor_Peak") = .RemovePrecursorPeak
                dr.Item("SQ_Ion_Cutoff_Value") = Format(.IonCutoffPercentage, "0.00")
                dr.Item("SQ_Max_Num_IC_Sites") = .MaximumNumberMissedCleavages
                dr.Item("SQ_Min_Mass_To_Use") = .MinimumProteinMassToSearch
                dr.Item("SQ_Max_Mass_To_Use") = .MinimumProteinMassToSearch
                dr.Item("SQ_Match_Peak_Count") = .NumberOfDetectedPeaksToMatch
                dr.Item("SQ_Match_Peak_Errors") = .NumberOfAllowedDetectedPeakErrors
                dr.Item("SQ_Match_Peak_Tolerance") = .MatchedPeakMassTolerance
                dr.Item("SQ_AA_In_Uppercase") = .AminoAcidsAllUpperCase
                dr.Item("SQ_Partial_Sequence") = .PartialSequenceToMatch
                dr.Item("SQ_Seq_Header_Filter") = .SequenceHeaderInfoToFilter

                dr.Item("SQ_Stat_CT_Pep") = .StaticModificationsList.CtermPeptide
                dr.Item("SQ_Stat_CT_Prot") = .StaticModificationsList.CtermProtein
                dr.Item("SQ_Stat_NT_Pep") = .StaticModificationsList.NtermPeptide
                dr.Item("SQ_Stat_NT_Prot") = .StaticModificationsList.CtermProtein
                dr.Item("SQ_Stat_G_Gly") = .StaticModificationsList.G_Glycine
                dr.Item("SQ_Stat_A_Ala") = .StaticModificationsList.A_Alanine
                dr.Item("SQ_Stat_S_Ser") = .StaticModificationsList.S_Serine
                dr.Item("SQ_Stat_P_Pro") = .StaticModificationsList.P_Proline
                dr.Item("SQ_Stat_V_Val") = .StaticModificationsList.V_Valine
                dr.Item("SQ_Stat_T_Thr") = .StaticModificationsList.T_Threonine
                dr.Item("SQ_Stat_C_Cys") = .StaticModificationsList.C_Cysteine
                dr.Item("SQ_Stat_L_Leu") = .StaticModificationsList.L_Leucine
                dr.Item("SQ_Stat_I_Ile") = .StaticModificationsList.I_Isoleucine
                dr.Item("SQ_Stat_X_LorI") = .StaticModificationsList.X_LorI
                dr.Item("SQ_Stat_N_Asn") = .StaticModificationsList.N_Asparagine
                dr.Item("SQ_Stat_O_Orn") = .StaticModificationsList.O_Ornithine
                dr.Item("SQ_Stat_B_avgNandD") = .StaticModificationsList.B_avg_NandD
                dr.Item("SQ_Stat_D_Asp") = .StaticModificationsList.D_Aspartic_Acid
                dr.Item("SQ_Stat_Q_Gln") = .StaticModificationsList.Q_Glutamine
                dr.Item("SQ_Stat_E_Glu") = .StaticModificationsList.E_Glutamic_Acid
                dr.Item("SQ_Stat_M_Met") = .StaticModificationsList.M_Methionine
                dr.Item("SQ_Stat_H_His") = .StaticModificationsList.H_Histidine
                dr.Item("SQ_Stat_F_Phe") = .StaticModificationsList.F_Phenylalanine
                dr.Item("SQ_Stat_R_Arg") = .StaticModificationsList.R_Arginine
                dr.Item("SQ_Stat_Y_Tyr") = .StaticModificationsList.Y_Tyrosine
                dr.Item("SQ_Stat_W_Trp") = .StaticModificationsList.W_Tryptophan

            End With

            'tmpTable.Rows.Add(dr)

            'Return tmpTable.Rows(tmpTable.Rows.Count - 1)
            Return dr

        End Function
        Private Function GetIDWithName(ByVal Name As String) As Integer
            Dim foundRows As DataRow() = Me.m_ParamsTable.Select("[SQ_FileName] = '" & Name & "'")
            Dim foundRow As DataRow
            Dim tmpID As Integer
            If foundRows.Length <> 0 Then
                foundRow = foundRows(0)
                tmpID = CInt(foundRow.Item(0))
            Else
                tmpID = -1
            End If
            Return tmpID
        End Function
        'Private Function GetIDWithName(ByVal Name As String) As Integer
        '    Dim sql As String
        '    Dim foundRow As DataRow
        '    Dim tmpID As Integer

        '    sql = "SELECT TOP 1 [Sequest_Param_ID] "
        '    sql = sql & "FROM " & Param_Table & " "
        '    sql = sql & "WHERE [SQ_FileName] = '" & Name & "'"

        '    foundRow = GetSingleRowFromDatabase(sql)
        '    If foundRow Is Nothing Then
        '        tmpID = -1
        '    Else
        '        tmpID = CInt(foundRow.Item("Sequest_Param_ID"))
        '    End If

        '    Return tmpID
        'End Function
        Private Function GetRowWithID(ByVal ID As Integer) As DataRow
            Dim foundRow As DataRow = m_ParamsTable.Rows.Find(ID)
            Return foundRow
        End Function
        'Private Function GetRowWithID(ByVal ID As Integer) As DataRow
        '    Dim sql As String
        '    Dim foundRow As DataRow

        '    sql = "SELECT TOP 1 * "
        '    sql = sql & "FROM " & Param_Table & " "
        '    sql = sql & "WHERE [Sequest_Param_ID] = " & ID.ToString

        '    foundRow = GetSingleRowFromDatabase(sql)
        '    Return foundRow
        'End Function

        Private Function GetAvailableParamSets() As DataTable
            'Dim sql As String

            'sql = "SELECT Sequest_Param_ID as ID, SQ_Filename as Filename "
            'sql = sql & "FROM " & Param_Table

            Dim tmpIDTable As New DataTable

            With tmpIDTable
                .Columns.Add("ID", System.Type.GetType("System.Int32"))
                .Columns.Add("Filename", System.Type.GetType("System.String"))
                .Columns.Add("Diffs", System.Type.GetType("System.String"))
            End With

            'Load tmpIDTable
            Dim tmpRow As DataRow

            Dim tmpID As Integer
            Dim tmpFileName As String
            Dim tmpDiffs As String
            Dim index As Integer
            Dim maxIndex As Integer

            maxIndex = Me.m_ParamSetCount - 1

            For index = 0 To maxIndex
                tmpID = m_ParamsTable.Rows(index).Item("Sequest_Param_ID")
                tmpFileName = m_ParamsTable.Rows(index).Item("SQ_FileName")
                tmpDiffs = DistillFeaturesFromParamSet(tmpID)
                tmpRow = tmpIDTable.NewRow
                tmpRow(0) = tmpID
                tmpRow(1) = tmpFileName
                tmpRow(2) = tmpDiffs
                tmpIDTable.Rows.Add(tmpRow)
            Next

            Return tmpIDTable

        End Function
        Private Function GetSingleRowFromDatabase(ByVal SelectSQL As String) As DataRow

            Dim tmpIDTable As DataTable = GetTable(SelectSQL)
            If tmpIDTable.Rows.Count = 0 Then
                Return Nothing
            Else
                Dim foundRow As DataRow = tmpIDTable.Rows(0)
                Return foundRow
            End If

        End Function

        'Private Function AddUpdateRowInDatabase(ByVal RowToUpdate As DataRow) As Boolean

        'End Function

        'Private Function AddUpdateRowInDatabase(ByVal RowToUpdate As DataRow, ByVal RowIDToUpdate As Integer) As Boolean

        'End Function

        'Private Function UpdateRowInDatabase( _
        '    ByVal RowToUpdate As DataRow, _
        '    ByVal RowID As Integer) As Boolean

        '    If RowID = -1 Then

        '    Else

        '    End If

        '    Dim updateStatement As String = "UPDATE From " & Param_Table & " WHERE [Sequest_Param_ID] = " & RowID.ToString
        '    Dim da As SqlClient.SqlDataAdapter = New SqlClient.SqlDataAdapter

        '    da.UpdateCommand = New SqlClient.SqlCommand(selectStatement)

        '    Dim commBuilder As SqlClient.SqlCommandBuilder = New SqlClient.SqlCommandBuilder(da)

        '    Me.OpenConnection()

        '    Dim dt As DataTable = New DataTable
        '    da.Fill(dt)

        '    Dim index As Integer

        '    For index = 0 To dt.Columns.Count
        '        dt.Rows(0).Item(index) = RowToUpdate(index)
        '    Next

        '    da.Update(dt)


        'End Function

        Private Function DistillFeaturesFromParamSet(ByVal ParamSetID As Integer) As String
            Dim p As clsParams = Me.GetParamSetWithID(ParamSetID)

            Return DistillFeaturesFromParamSet(p)

            'Dim templateRow As DataRow = GetRowWithID(1000)
            'Dim checkRow As DataRow = GetRowWithID(ParamSetID)

            'Return CompareDataRows(templateRow, checkRow)


        End Function

        Private Function DistillFeaturesFromParamSet(ByVal ParamSet As clsParams) As String
            Dim templateRow As DataRow = GetRowWithID(1000)
            Dim templateColl As NameValueCollection = WriteDataCollectionFromDatarow(templateRow)
            Dim checkRow As DataRow = Me.WriteDatarowFromParamSet(ParamSet, templateRow)
            Dim checkColl As NameValueCollection = WriteDataCollectionFromDatarow(checkRow)


            Return CompareDataCollections(templateColl, checkColl)

        End Function

        Private Function CompareParamSets(ByVal ParamSet1 As clsParams, ByVal ParamSet2 As clsParams) As String
            Dim templateRow As DataRow = GetRowWithID(1000)
            Dim row1 As DataRow = Me.WriteDatarowFromParamSet(ParamSet1, templateRow)
            Dim coll1 As NameValueCollection = Me.WriteDataCollectionFromDatarow(row1)

            Dim row2 As DataRow = Me.WriteDatarowFromParamSet(ParamSet2, templateRow)
            Dim coll2 As NameValueCollection = Me.WriteDataCollectionFromDatarow(row2)

            Return CompareDataCollections(coll1, coll2)

        End Function

        Private Function CompareDataCollections(ByVal templateColl As NameValueCollection, ByVal checkColl As NameValueCollection) As String
            Dim diffColl As New NameValueCollection
            Dim maxIndex As Integer = templateColl.Count - 1
            Dim index As Integer
            Dim tmpString As String
            Dim tmpTemp As String
            Dim tmpCheck As String

            For index = 0 To maxIndex
                tmpTemp = templateColl.GetKey(index).ToString & " = " & templateColl.Item(index).ToString
                tmpCheck = checkColl.GetKey(index).ToString & " = " & checkColl.Item(index).ToString

                If templateColl.GetKey(index).ToString = checkColl.GetKey(index).ToString Then
                    If templateColl.Item(index).ToString.Equals(checkColl.Item(index).ToString) Then

                    ElseIf templateColl.Item(index).ToString <> checkColl.Item(index).ToString Then
                        diffColl.Add(templateColl.GetKey(index).ToString, checkColl.Item(index).ToString)

                    End If
                End If
            Next

            maxIndex = diffColl.Count - 1

            For index = 0 To maxIndex
                If index <> 0 Then
                    tmpString = tmpString & ", "
                End If
                tmpString = tmpString & diffColl.GetKey(index).ToString & " = " & diffColl.Item(index).ToString
            Next

            If tmpString = Nothing Then tmpString = " --No Change-- "

            Return tmpString
        End Function

        'Private Function CompareDataRows(ByVal templateRow As DataRow, ByVal checkRow As DataRow) As String

        '    Dim diffCollection As New NameValueCollection

        '    Dim index As Integer
        '    Dim maxIndex As Integer = templateRow.Table.Columns.Count - 1
        '    Dim Different As Boolean
        '    Dim delta As Double

        '    For index = 6 To maxIndex
        '        If templateRow(index).ToString.Equals(checkRow(index).ToString) Then
        '            Different = False
        '        Else
        '            'If templateRow(index).GetType Is System.Type.GetType("System.Double") Then 'Check for rounding errors
        '            '    delta = Math.Abs(templateRow(index) - checkRow(index))
        '            '    If Math.Abs(delta) < 0.0001 Then
        '            '        Different = False
        '            '    Else
        '            '        Different = True
        '            '    End If
        '            'End If
        '            Different = True

        '        End If

        '        If Different Then
        '            diffCollection.Add(checkRow.Table.Columns(index).ColumnName, checkRow.Item(index))
        '        End If
        '    Next

        '    Dim tmpString As String

        '    maxIndex = diffCollection.Count - 1

        '    For index = 0 To maxIndex
        '        If index <> 0 Then
        '            tmpString = tmpString & ", "
        '        End If
        '        tmpString = tmpString & diffCollection.GetKey(index).ToString & " = " & diffCollection.Item(index)
        '    Next

        '    If tmpString = Nothing Then tmpString = ""

        '    Return tmpString

        'End Function

        'Private Function CompareParamSets(ByVal templateSet As clsParams, ByVal checkSet As clsParams)
        '    Dim diffCollection As New NameValueCollection

        '    Dim templateType As Type = templateSet.GetType
        '    Dim checkType As Type = checkSet.GetType

        '    Dim templateProps As PropertyInfo() = GetProperties(templateType)
        '    Dim checkProps As PropertyInfo() = GetProperties(checkType)

        '    Dim tProp As PropertyInfo
        '    Dim cProp As PropertyInfo

        '    Dim tPI As PropertyInfo
        '    Dim cPI As PropertyInfo

        '    Dim o As Object
        '    Dim name As String

        '    Dim index As Integer
        '    Dim maxIndex As Integer = UBound(templateProps)

        '    For index = 0 To maxIndex
        '        tProp = templateProps(index)
        '        cProp = checkProps(index)

        '        tPI = templateType.GetProperty(tProp.Name)
        '        cPI = checkType.GetProperty(cProp.Name)

        '        If tPI.GetValue(templateSet, Nothing) <> cPI.GetValue(checkSet, Nothing) Then
        '            diffCollection.Add(cProp.Name, cPI.GetValue(checkSet, Nothing).ToString)

        '        End If

        '    Next

        '    Dim tmpString As String

        '    maxIndex = diffCollection.Count - 1

        '    For index = 0 To maxIndex
        '        If index <> 0 Then
        '            tmpString = tmpString & ", "
        '        End If
        '        tmpString = tmpString & diffCollection.GetKey(index).ToString & " = " & diffCollection.Item(index)
        '    Next

        '    Return tmpString

        'End Function

        Private Function GetProperties(ByRef theType As Type) As PropertyInfo()
            Dim myProperties As PropertyInfo() = theType.GetProperties((BindingFlags.Public Or BindingFlags.Instance))
            Return myProperties
        End Function

        Private Function DoesParamSetNameExist(ByVal paramSetName As String) As Boolean
            Dim tmpID As Integer = GetIDWithName(paramSetName)
            Return DoesParamSetIDExist(tmpID)

        End Function

        Private Function DoesParamSetIDExist(ByVal paramSetID As Integer) As Boolean
            Dim IDExists As Boolean = Me.m_ParamsTable.Rows.Contains(paramSetID)
            Return IDExists
        End Function

#End Region

    End Class

End Namespace


