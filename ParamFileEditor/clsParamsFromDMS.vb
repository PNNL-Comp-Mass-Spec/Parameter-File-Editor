Imports System.Collections.Specialized
Imports System.Reflection
Imports ParamFileEditor.ProgramSettings
'Imports ParamFileEditor.DownloadParams

Namespace DownloadParams

    Friend Class clsParamsFromDMS
        Inherits clsDBTask
#Region " Constants "
        Protected Const Param_File_Table As String = "T_Param_Files"
        Protected Const Param_Entry_Table As String = "T_Param_Entries"
        Protected Const Param_FileTypes_Table As String = "T_Param_File_Types"
        Protected Const Param_Class_Table As String = "T_Param_Entry_Types"
        Protected Const Param_MassType_Table As String = "T_Sequest_Params_MassType_Name"
#End Region

#Region " Enums "
        Friend Enum AcceptableParams
            SelectedEnzymeIndex
            MaximumNumberMissedCleavages
            ParentMassType
            FragmentMassType
            DynamicMods
            StaticModificationsList
            PartialSequenceToMatch
            CreateOutputFiles
            NumberOfResultsToProcess
            MaximumNumAAPerDynMod
            PeptideMassTolerance
            FragmentIonTolerance
            NumberOfOutputLines
            NumberOfDescriptionLines
            ShowFragmentIons
            PrintDuplicateReferences
            SelectedNucReadingFrameIndex
            RemovePrecursorPeak
            IonCutoffPercentage
            MinimumProteinMassToSearch
            MaximumProteinMassToSearch
            NumberOfDetectedPeaksToMatch
            NumberOfAllowedDetectedPeakErrors
            MatchedPeakMassTolerance
            AminoAcidsAllUpperCase
            SequenceHeaderInfoToFilter
        End Enum
        Friend Enum BasicParams
            SelectedEnzymeIndex
            MaximumNumberMissedCleavages
            ParentMassType
            FragmentMassType
            PartialSequenceToMatch
        End Enum
        Friend Enum AdvancedParams
            CreateOutputFiles
            NumberOfResultsToProcess
            MaximumNumAAPerDynMod
            PeptideMassTolerance
            FragmentIonTolerance
            NumberOfOutputLines
            NumberOfDescriptionLines
            ShowFragmentIons
            PrintDuplicateReferences
            SelectedNucReadingFrameIndex
            RemovePrecursorPeak
            IonCutoffPercentage
            MinimumProteinMassToSearch
            MaximumProteinMassToSearch
            NumberOfDetectedPeaksToMatch
            NumberOfAllowedDetectedPeakErrors
            MatchedPeakMassTolerance
            AminoAcidsAllUpperCase
            SequenceHeaderInfoToFilter
        End Enum

#End Region

#Region " Member Properties "
        Protected m_ID As Integer
        Protected m_Name As String
        Protected m_Params As clsParams
        'Protected m_ParamFileTable As DataTable
        'Protected m_ParamEntryTypeTable As DataTable
        'Protected m_ParamsTable As DataTable
        Protected m_ParamsSet As DataSet
        Protected m_ParamSetCount As Integer
        Protected m_BaseLineParamSet As clsParams
        Protected m_AcceptableParams As StringCollection
        Protected m_BasicParams As StringCollection
        Protected m_AdvancedParams As StringCollection
        Protected m_GetID_DA As SqlClient.SqlDataAdapter
        Protected m_GetID_DB As SqlClient.SqlCommandBuilder
        Protected m_GetEntries_DA As SqlClient.SqlDataAdapter
        Protected m_GetEntries_CB As SqlClient.SqlCommandBuilder

#End Region

#Region " Friend Properties "
        Friend ReadOnly Property ParamFileTable() As DataTable
            Get
                Return Me.m_ParamsSet.Tables(Me.Param_File_Table)
            End Get
        End Property
        Friend ReadOnly Property ParamEntryTable() As DataTable
            Get
                Return Me.m_ParamsSet.Tables(Me.Param_Entry_Table)
            End Get
        End Property

        Friend ReadOnly Property ParamSetCount() As Integer
            Get
                Return m_ParamSetCount
            End Get
        End Property
#End Region


#Region " Friend Functions "
        Friend Sub New(ByVal ConnectionString As String)
            MyBase.New(ConnectionString)
            Me.m_AcceptableParams = Me.LoadAcceptableParamList
            Me.m_BasicParams = Me.LoadBasicParams
            Me.m_AdvancedParams = Me.LoadAdvancedParams
            Me.m_BaseLineParamSet = clsMainProcess.BaseLineParamSet
            Me.m_ParamsSet = GetParamsFromDMS()
            If Me.m_ParamsSet Is Nothing Then
                Exit Sub
            End If
            'm_ParamSetCount = GetParamSetTableCount(m_ParamsTable)
        End Sub
        Friend Function ReadParamsFromDMS(ByVal ParamSetName As String) As clsParams
            'Retrieve ID number first, then use the same procedure as below
            m_Name = ParamSetName
            m_ID = GetIDWithName(m_Name)

            m_Params = RetrieveParams(m_ID)
            Return m_Params
        End Function
        Friend Function ReadParamsFromDMS(ByVal ParamSetID As Integer) As clsParams
            m_ID = ParamSetID

            m_Params = RetrieveParams(m_ID)
            Return m_Params
        End Function
        Friend Function RetrieveAvailableParams() As DataTable
            Return Me.GetAvailableParamSets()
        End Function

        Friend Function ParamSetNameExists(ByVal ParamSetName As String) As Boolean
            Return DoesParamSetNameExist(ParamSetName)
        End Function

        Friend Function ParamSetIDExists(ByVal ParamSetID As Integer) As Boolean
            Return Me.DoesParamSetIDExist(ParamSetID)
        End Function

        Friend Function GetParamSetIDFromName(ByVal Name As String) As Integer
            Return Me.GetIDWithName(Name)
        End Function

#End Region

#Region " Member Functions "
        Protected Function LoadAcceptableParamList() As StringCollection
            Dim ParamEnum() As String = System.Enum.GetNames(GetType(AcceptableParams))
            Dim Param As String
            Dim sc As New StringCollection
            For Each Param In ParamEnum
                sc.Add(Param)
            Next
            Return sc
        End Function
        Protected Function LoadBasicParams() As StringCollection
            Dim ParamEnum() As String = System.Enum.GetNames(GetType(BasicParams))
            Dim Param As String
            Dim sc As New StringCollection
            For Each Param In ParamEnum
                sc.Add(Param)
            Next
            Return sc
        End Function
        Protected Function LoadAdvancedParams() As StringCollection
            Dim ParamEnum() As String = System.Enum.GetNames(GetType(AdvancedParams))
            Dim Param As String
            Dim sc As New StringCollection
            For Each Param In ParamEnum
                sc.Add(Param)
            Next
            Return sc
        End Function

        Protected Function GetParamsFromDMS() As DataSet      'Common
            Dim SQL As String
            Dim tmpFileTable As DataTable
            Dim tmpEntryTable As DataTable
            Dim tmpSet As New DataSet

            'SQL to grab param file table
            SQL = "SELECT * FROM " & Me.Param_File_Table

            tmpFileTable = GetTable(SQL, Me.m_GetID_DA, Me.m_GetID_DB)
            tmpFileTable.TableName = Me.Param_File_Table
            setprimarykey(0, tmpFileTable)

            tmpSet.Tables.Add(tmpFileTable)

            'SQL to grab param entry table
            SQL = "SELECT * FROM " & Me.Param_Entry_Table

            tmpEntryTable = gettable(SQL, Me.m_GetEntries_DA, Me.m_GetEntries_CB)
            tmpEntryTable.TableName = Me.Param_Entry_Table

            tmpSet.Tables.Add(tmpEntryTable)

            Return tmpSet

        End Function

        Protected Function GetParamSetTableCount(ByVal psTable As DataTable) As Integer           'Common
            Dim count As Integer = psTable.Rows.Count
            Return count
        End Function

        Protected Function RetrieveParams(ByVal ParamSetID As Integer) As clsParams               'Download
            Dim p As clsParams = GetParamSetWithID(ParamSetID)
            Return p
        End Function

        Protected Function GetParamSetWithID(ByVal ParamSetID As Integer) As clsParams        'Download
            Dim dr As DataRow = GetFileRowWithID(ParamSetID)
            Dim foundrows As DataRow() = Me.m_ParamsSet.Tables(Me.Param_Entry_Table).Select("[Param_File_ID] = " & ParamSetID, "[Entry_Sequence_Order]")
            Dim storageSet As clsDMSParamStorage = Me.MakeStorageClassFromTableRowSet(foundrows)
            Dim p As clsParams = Me.UpdateParamSetFromDataCollection(storageSet)
            p.FileName = dr.Item("Param_File_Name")
            p.Description = Me.SummarizeDiffColl(storageSet)

            Return p
        End Function

        Protected Function MakeStorageClassFromTableRowSet(ByVal foundRows As DataRow()) As clsDMSParamStorage
            Dim foundRow As DataRow
            Dim storageClass As New clsDMSParamStorage
            Dim tmpSpec As String
            Dim tmpValue As String
            Dim tmpTypeString As String
            Dim tmpType As clsDMSParamStorage.ParamTypes

            For Each foundRow In foundRows
                tmpSpec = foundRow.Item("Entry_Specifier")
                tmpValue = foundRow.Item("Entry_Value")
                tmpType = System.Enum.Parse(GetType(clsDMSParamStorage.ParamTypes), foundRow.Item("Entry_Type"))

                storageClass.Add(tmpSpec, tmpValue, tmpType)
            Next
            Return storageClass

        End Function

        'Protected Function GetParamSetFromDatarow(ByVal ParamRow As DataRow) As clsParams         'Download

        '    Dim dr As DataRow
        '    dr = ParamRow
        '    Dim p As New clsParams

        '    With p
        '        .DMS_ID = CInt(dr.Item("Sequest_Param_ID"))
        '        .FileName = dr.Item("SQ_FileName").ToString
        '        .Description = dr.Item("SQ_Param_File_Desc").ToString
        '        .DefaultFASTAPath = dr.Item("SQ_Database_1").ToString
        '        .DefaultFASTAPath2 = dr.Item("SQ_Database_2").ToString
        '        .PeptideMassTolerance = CSng(dr.Item("SQ_Peptide_Mass_Tolerance"))
        '        .CreateOutputFiles = CBool(dr.Item("SQ_Create_Output_Files"))
        '        .IonSeries.Use_a_Ions = CInt(dr.Item("SQ_Ions_Use_A"))
        '        .IonSeries.Use_b_Ions = CInt(dr.Item("SQ_Ions_Use_B"))
        '        .IonSeries.Use_y_Ions = CInt(dr.Item("SQ_Ions_Use_Y"))
        '        .IonSeries.a_Ion_Weighting = CSng(dr.Item("SQ_Ions_a_Weight"))
        '        .IonSeries.b_Ion_Weighting = CSng(dr.Item("SQ_Ions_b_Weight"))
        '        .IonSeries.c_Ion_Weighting = CSng(dr.Item("SQ_Ions_c_Weight"))
        '        .IonSeries.d_Ion_Weighting = CSng(dr.Item("SQ_Ions_d_Weight"))
        '        .IonSeries.v_Ion_Weighting = CSng(dr.Item("SQ_Ions_v_Weight"))
        '        .IonSeries.w_Ion_Weighting = CSng(dr.Item("SQ_Ions_w_Weight"))
        '        .IonSeries.x_Ion_Weighting = CSng(dr.Item("SQ_Ions_x_Weight"))
        '        .IonSeries.y_Ion_Weighting = CSng(dr.Item("SQ_Ions_y_Weight"))
        '        .IonSeries.z_Ion_Weighting = CSng(dr.Item("SQ_Ions_z_Weight"))
        '        .DynamicMods.Dyn_Mod_1_AAList = dr.Item("SQ_Dyn_Mod1_AAList").ToString
        '        .DynamicMods.Dyn_Mod_2_AAList = dr.Item("SQ_Dyn_Mod2_AAList").ToString
        '        .DynamicMods.Dyn_Mod_3_AAList = dr.Item("SQ_Dyn_Mod3_AAList").ToString
        '        .DynamicMods.Dyn_Mod_1_MassDiff = CSng(dr.Item("SQ_Dyn_Mod1_Mass"))
        '        .DynamicMods.Dyn_Mod_2_MassDiff = CSng(dr.Item("SQ_Dyn_Mod2_Mass"))
        '        .DynamicMods.Dyn_Mod_3_MassDiff = CSng(dr.Item("SQ_Dyn_Mod3_Mass"))
        '        .MaximumNumAAPerDynMod = CInt(dr.Item("SQ_Max_Dyn_Mods"))
        '        .FragmentIonTolerance = CSng(dr.Item("SQ_Frag_Ion_Tolerance"))
        '        .NumberOfOutputLines = CInt(dr.Item("SQ_Num_Output_Lines"))
        '        .NumberOfDescriptionLines = CInt(dr.Item("SQ_Num_Desc_Lines"))
        '        .NumberOfResultsToProcess = CInt(dr.Item("SQ_Num_Results_Used"))
        '        .ShowFragmentIons = CBool(dr.Item("SQ_Show_Fragments"))
        '        .PrintDuplicateReferences = CBool(dr.Item("SQ_Print_Dup_Refs"))
        '        .SelectedEnzymeIndex = CInt(dr.Item("SQ_Enzyme_Num"))
        '        .SelectedNucReadingFrame = CType(dr.Item("SQ_Nuc_Reading_Frame"), IAdvancedParams.FrameList)
        '        .ParentMassType = CType(dr.Item("SQ_Parent_Mass_Type"), IBasicParams.MassTypeList)
        '        .FragmentMassType = CType(dr.Item("SQ_Fragment_Mass_Type"), IBasicParams.MassTypeList)
        '        .RemovePrecursorPeak = CBool(dr.Item("SQ_Rem_Precursor_Peak"))
        '        .IonCutoffPercentage = CSng(dr.Item("SQ_Ion_Cutoff_Value"))
        '        .MaximumNumberMissedCleavages = CInt(dr.Item("SQ_Max_Num_IC_Sites"))
        '        .MinimumProteinMassToSearch = CInt(dr.Item("SQ_Min_Mass_To_Use"))
        '        .MinimumProteinMassToSearch = CInt(dr.Item("SQ_Max_Mass_To_Use"))
        '        .MatchedPeakMassTolerance = CSng(dr.Item("SQ_Match_Peak_Tolerance"))
        '        .NumberOfDetectedPeaksToMatch = CInt(dr.Item("SQ_Match_Peak_Count"))
        '        .NumberOfAllowedDetectedPeakErrors = CInt(dr.Item("SQ_Match_Peak_Errors"))
        '        .AminoAcidsAllUpperCase = CBool(dr.Item("SQ_AA_In_Uppercase"))
        '        .PartialSequenceToMatch = dr.Item("SQ_Partial_Sequence").ToString
        '        .SequenceHeaderInfoToFilter = dr.Item("SQ_Seq_Header_Filter").ToString

        '        .StaticModificationsList.CtermPeptide = CSng(dr.Item("SQ_Stat_CT_Pep"))
        '        .StaticModificationsList.CtermProtein = CSng(dr.Item("SQ_Stat_CT_Prot"))
        '        .StaticModificationsList.NtermPeptide = CSng(dr.Item("SQ_Stat_NT_Pep"))
        '        .StaticModificationsList.CtermProtein = CSng(dr.Item("SQ_Stat_NT_Prot"))
        '        .StaticModificationsList.G_Glycine = CSng(dr.Item("SQ_Stat_G_Gly"))
        '        .StaticModificationsList.A_Alanine = CSng(dr.Item("SQ_Stat_A_Ala"))
        '        .StaticModificationsList.S_Serine = CSng(dr.Item("SQ_Stat_S_Ser"))
        '        .StaticModificationsList.P_Proline = CSng(dr.Item("SQ_Stat_P_Pro"))
        '        .StaticModificationsList.V_Valine = CSng(dr.Item("SQ_Stat_V_Val"))
        '        .StaticModificationsList.T_Threonine = CSng(dr.Item("SQ_Stat_T_Thr"))
        '        .StaticModificationsList.C_Cysteine = CSng(dr.Item("SQ_Stat_C_Cys"))
        '        .StaticModificationsList.L_Leucine = CSng(dr.Item("SQ_Stat_L_Leu"))
        '        .StaticModificationsList.I_Isoleucine = CSng(dr.Item("SQ_Stat_I_Ile"))
        '        .StaticModificationsList.X_LorI = CSng(dr.Item("SQ_Stat_X_LorI"))
        '        .StaticModificationsList.N_Asparagine = CSng(dr.Item("SQ_Stat_N_Asn"))
        '        .StaticModificationsList.O_Ornithine = CSng(dr.Item("SQ_Stat_O_Orn"))
        '        .StaticModificationsList.B_avg_NandD = CSng(dr.Item("SQ_Stat_B_avgNandD"))
        '        .StaticModificationsList.D_Aspartic_Acid = CSng(dr.Item("SQ_Stat_D_Asp"))
        '        .StaticModificationsList.Q_Glutamine = CSng(dr.Item("SQ_Stat_Q_Gln"))
        '        .StaticModificationsList.E_Glutamic_Acid = CSng(dr.Item("SQ_Stat_E_Glu"))
        '        .StaticModificationsList.M_Methionine = CSng(dr.Item("SQ_Stat_M_Met"))
        '        .StaticModificationsList.H_Histidine = CSng(dr.Item("SQ_Stat_H_His"))
        '        .StaticModificationsList.F_Phenylalanine = CSng(dr.Item("SQ_Stat_F_Phe"))
        '        .StaticModificationsList.R_Arginine = CSng(dr.Item("SQ_Stat_R_Arg"))
        '        .StaticModificationsList.Y_Tyrosine = CSng(dr.Item("SQ_Stat_Y_Tyr"))
        '        .StaticModificationsList.W_Tryptophan = CSng(dr.Item("SQ_Stat_W_Trp"))

        '    End With

        '    Return p

        'End Function

        Protected Function GetIDWithName(ByVal Name As String) As Integer             'Common
            Dim foundRows As DataRow() = Me.ParamFileTable.Select("[Param_File_Name] = '" & Name & "'")
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

        Protected Function GetNameWithID(ByVal ID As Integer) As String
            Dim foundrows As DataRow() = Me.ParamFileTable.Select("[Param_File_ID] = " & ID)
            Dim foundrow As DataRow
            If foundrows.Length <> 0 Then
                foundrow = foundrows(0)
                Return CStr(foundrow.Item("Param_File_Name"))
            Else
                Return Nothing
            End If

        End Function

        Protected Function GetDescriptionWithID(ByVal ID As Integer) As String
            Dim foundrows As DataRow() = Me.ParamFileTable.Select("[Param_File_ID] = " & ID)
            Dim foundrow As DataRow
            Dim tmpString As String
            If foundrows.Length <> 0 Then
                foundrow = foundrows(0)
                tmpString = CStr(foundrow.Item("Param_File_Description"))
                If tmpString = "" Then
                    Return ""
                Else
                    Return tmpString
                End If
            Else
                Return Nothing
            End If
        End Function

        Protected Function GetFileRowWithID(ByVal ID As Integer) As DataRow           'Download
            Dim foundRow As DataRow = ParamFileTable.Rows.Find(ID)
            Return foundRow
        End Function
        Protected Function GetAvailableParamSets() As DataTable
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

            maxIndex = Me.ParamFileTable.Rows.Count - 1

            For index = 0 To maxIndex
                tmpID = Me.ParamFileTable.Rows(index).Item("Param_File_ID")
                tmpFileName = Me.ParamFileTable.Rows(index).Item("Param_File_Name")
                tmpDiffs = DistillFeaturesFromParamSet(tmpID)
                tmpRow = tmpIDTable.NewRow
                tmpRow(0) = tmpID
                tmpRow(1) = tmpFileName
                tmpRow(2) = tmpDiffs
                tmpIDTable.Rows.Add(tmpRow)
            Next

            Return tmpIDTable

        End Function

        Protected Function DistillFeaturesFromParamSet(ByVal ParamSet As clsParams) As String         'Neither
            Dim templateColl As clsDMSParamStorage = WriteDataCollectionFromParamSet(Me.m_BaseLineParamSet)
            Dim checkColl As clsDMSParamStorage = WriteDataCollectionFromParamSet(ParamSet)


            Dim diffColl As clsDMSParamStorage = CompareDataCollections(templateColl, checkColl)
            Return Me.SummarizeDiffColl(diffColl)

        End Function
        Protected Function DistillFeaturesFromParamSet(ByVal ParamSetID As Integer) As String         'Common
            Dim p As clsParams = Me.GetParamSetWithID(ParamSetID)

            Return DistillFeaturesFromParamSet(p)

        End Function

        Protected Function WriteDataCollectionFromParamSet(ByVal ParamSet As clsParams) As clsDMSParamStorage        'Upload
            Dim c As clsDMSParamStorage = New clsDMSParamStorage

            Dim o As Object

            Dim pType As Type = ParamSet.GetType
            Dim tmpType As Type
            Dim pProps As PropertyInfo() = pType.GetProperties((BindingFlags.Public Or BindingFlags.Instance))
            Dim pProp As PropertyInfo

            Dim tmpName As String
            Dim tmpValue As String

            For Each pProp In pProps
                tmpName = pProp.Name
                tmpType = pProp.PropertyType
                If Me.m_AcceptableParams.Contains(tmpName) Then
                    If Me.m_BasicParams.Contains(tmpName) Then
                        tmpValue = (pProp.GetValue(ParamSet, Nothing)).ToString
                        c.Add(tmpName, tmpValue.ToString, clsDMSParamStorage.ParamTypes.BasicParam)
                    ElseIf Me.m_AdvancedParams.Contains(tmpName) Then
                        tmpValue = (pProp.GetValue(ParamSet, Nothing)).ToString
                        c.Add(tmpName, tmpValue.ToString, clsDMSParamStorage.ParamTypes.AdvancedParam)
                    ElseIf tmpType.Name = "clsDynamicMods" Then
                        c = ExpandDynamicMods(ParamSet.DynamicMods, c)
                    ElseIf (tmpType.Name = "clsStaticMods") Then
                        c = ExpandStaticMods(ParamSet.StaticModificationsList, c)
                    End If
                    'tmpValue = (pProp.GetValue(ParamSet, Nothing)).ToString
                End If

            Next

            Return c

        End Function

        Protected Function UpdateParamSetFromDataCollection(ByVal dc As clsDMSParamStorage) As clsParams
            Dim p As New clsParams
            Dim tmpSpec As String
            Dim tmpValue As Object
            Dim tmpType As clsDMSParamStorage.ParamTypes
            Dim tmpTypeName As String

            p.LoadTemplate(clsMainProcess.mySettings.TemplateFileName)
            Dim pType As Type = GetType(clsParams)
            Dim pFields As PropertyInfo() = pType.GetProperties(BindingFlags.Instance Or BindingFlags.Public)
            Dim pField As PropertyInfo

            Dim paramEntry As clsDMSParamStorage.ParamsEntry

            For Each paramEntry In dc
                tmpSpec = paramEntry.Specifier
                tmpValue = paramEntry.Value
                tmpType = paramEntry.Type

                If tmpType = clsDMSParamStorage.ParamTypes.BasicParam And _
                    Me.m_BasicParams.Contains(tmpSpec) Then

                    For Each pField In pFields
                        If pField.Name = tmpSpec Then
                            tmpTypeName = pField.PropertyType.Name
                            If tmpTypeName = "Int32" Then
                                pField.SetValue(p, CType(tmpValue, Int32), Nothing)
                            ElseIf tmpTypeName = "Single" Then
                                pField.SetValue(p, CType(tmpValue, Single), Nothing)
                            ElseIf tmpTypeName = "String" Then
                                pField.SetValue(p, CType(tmpValue, String), Nothing)
                            Else
                                Console.WriteLine(pField.PropertyType.Name.ToString)
                            End If

                            Exit For
                        End If
                    Next

                ElseIf tmpType = clsDMSParamStorage.ParamTypes.AdvancedParam And _
                    Me.m_AdvancedParams.Contains(tmpSpec) Then

                    For Each pField In pFields
                        If pField.Name = tmpSpec Then
                            pField.SetValue(p, tmpValue, Nothing)
                        End If
                    Next

                ElseIf tmpType = clsDMSParamStorage.ParamTypes.DynamicModification Then

                    p.DynamicMods.Add(tmpSpec, CSng(Val(tmpValue)))
                ElseIf tmpType = clsDMSParamStorage.ParamTypes.StaticModification Then
                    p.StaticModificationsList.Add(tmpSpec, CSng(Val(tmpValue)))

                End If
            Next

            Return p

        End Function

        Protected Function UpdateParamSetMember( _
            ByRef ParamSet As clsParams, _
            ByVal Specifier As String, _
            ByVal Value As String) As Integer



        End Function

        Private Function ExpandDynamicMods(ByVal DynModSet As clsDynamicMods, ByRef ParamCollection As clsDMSParamStorage) As clsDMSParamStorage
            Dim maxCount As Integer = DynModSet.Count
            Dim counter As Integer
            Dim tmpName As String
            Dim tmpValue As String

            If maxCount = 0 Then
                Return ParamCollection
            End If

            For counter = 1 To maxCount
                tmpName = DynModSet.Dyn_Mod_n_AAList(counter)
                tmpValue = Format(DynModSet.Dyn_Mod_n_MassDiff(counter), "0.0000")
                ParamCollection.Add(tmpName, tmpValue, clsDMSParamStorage.ParamTypes.DynamicModification)
            Next

            Return ParamCollection

        End Function

        Private Function ExpandStaticMods(ByVal StatModSet As clsStaticMods, ByRef ParamCollection As clsDMSParamStorage) As clsDMSParamStorage
            Dim maxCount As Integer = StatModSet.Count
            Dim counter As Integer
            Dim tmpName As String
            Dim tmpValue As String

            If maxCount = 0 Then
                Return ParamCollection
            End If

            For counter = 0 To maxCount - 1
                tmpName = StatModSet.GetResidue(counter)
                tmpValue = StatModSet.GetMassDiff(counter)
                ParamCollection.Add(tmpName, tmpValue, clsDMSParamStorage.ParamTypes.StaticModification)
            Next

            Return ParamCollection

        End Function

        'Protected Function WriteDatarowFromParamSet(ByVal ParamSet As clsParams, ByVal DatarowTemplate As DataRow) As DataRow         'Upload
        '    Dim tmpID As Integer
        '    Dim dr As DataRow = DatarowTemplate

        '    Dim p As clsParams
        '    p = ParamSet

        '    With p
        '        'dr.Item("Sequest_Param_ID") = .DMS_ID
        '        dr.Item("SQ_FileName") = .FileName
        '        dr.Item("SQ_FileType") = CInt(.FileTypeIndex) + 1000
        '        Console.WriteLine(dr.Item("SQ_FileType"))
        '        dr.Item("SQ_Param_File_Desc") = .Description
        '        dr.Item("SQ_Database_1") = .DefaultFASTAPath
        '        dr.Item("SQ_Database_2") = .DefaultFASTAPath2
        '        dr.Item("SQ_Peptide_Mass_Tolerance") = .PeptideMassTolerance
        '        dr.Item("SQ_Create_Output_Files") = .CreateOutputFiles
        '        dr.Item("SQ_Ions_Use_A") = .IonSeries.Use_a_Ions
        '        dr.Item("SQ_Ions_Use_B") = .IonSeries.Use_b_Ions
        '        dr.Item("SQ_Ions_Use_Y") = .IonSeries.Use_y_Ions
        '        dr.Item("SQ_Ions_a_Weight") = .IonSeries.a_Ion_Weighting
        '        dr.Item("SQ_Ions_b_Weight") = .IonSeries.b_Ion_Weighting
        '        dr.Item("SQ_Ions_c_Weight") = .IonSeries.c_Ion_Weighting
        '        dr.Item("SQ_Ions_d_Weight") = .IonSeries.d_Ion_Weighting
        '        dr.Item("SQ_Ions_v_Weight") = .IonSeries.v_Ion_Weighting
        '        dr.Item("SQ_Ions_w_Weight") = .IonSeries.w_Ion_Weighting
        '        dr.Item("SQ_Ions_x_Weight") = .IonSeries.x_Ion_Weighting
        '        dr.Item("SQ_Ions_y_Weight") = .IonSeries.y_Ion_Weighting
        '        dr.Item("SQ_Ions_z_Weight") = .IonSeries.z_Ion_Weighting
        '        dr.Item("SQ_Dyn_Mod1_AAList") = .DynamicMods.Dyn_Mod_1_AAList
        '        dr.Item("SQ_Dyn_Mod2_AAList") = .DynamicMods.Dyn_Mod_2_AAList
        '        dr.Item("SQ_Dyn_Mod3_AAList") = .DynamicMods.Dyn_Mod_3_AAList
        '        dr.Item("SQ_Dyn_Mod1_Mass") = Format(.DynamicMods.Dyn_Mod_1_MassDiff, "0.0000")
        '        dr.Item("SQ_Dyn_Mod2_Mass") = Format(.DynamicMods.Dyn_Mod_2_MassDiff, "0.0000")
        '        dr.Item("SQ_Dyn_Mod3_Mass") = Format(.DynamicMods.Dyn_Mod_3_MassDiff, "0.0000")
        '        dr.Item("SQ_Max_Dyn_Mods") = .MaximumNumAAPerDynMod
        '        dr.Item("SQ_Frag_Ion_Tolerance") = .FragmentIonTolerance
        '        dr.Item("SQ_Num_Output_Lines") = .NumberOfOutputLines
        '        dr.Item("SQ_Num_Desc_Lines") = .NumberOfDescriptionLines
        '        dr.Item("SQ_Num_Results_Used") = .NumberOfResultsToProcess
        '        dr.Item("SQ_Show_Fragments") = .ShowFragmentIons
        '        dr.Item("SQ_Print_Dup_Refs") = .PrintDuplicateReferences
        '        dr.Item("SQ_Enzyme_Num") = .SelectedEnzymeIndex
        '        dr.Item("SQ_Nuc_Reading_Frame") = CInt(.SelectedNucReadingFrame)
        '        dr.Item("SQ_Parent_Mass_Type") = CInt(.ParentMassType)
        '        dr.Item("SQ_Fragment_Mass_Type") = CInt(.FragmentMassType)
        '        dr.Item("SQ_Rem_Precursor_Peak") = .RemovePrecursorPeak
        '        dr.Item("SQ_Ion_Cutoff_Value") = Format(.IonCutoffPercentage, "0.00")
        '        dr.Item("SQ_Max_Num_IC_Sites") = .MaximumNumberMissedCleavages
        '        dr.Item("SQ_Min_Mass_To_Use") = .MinimumProteinMassToSearch
        '        dr.Item("SQ_Max_Mass_To_Use") = .MinimumProteinMassToSearch
        '        dr.Item("SQ_Match_Peak_Count") = .NumberOfDetectedPeaksToMatch
        '        dr.Item("SQ_Match_Peak_Errors") = .NumberOfAllowedDetectedPeakErrors
        '        dr.Item("SQ_Match_Peak_Tolerance") = .MatchedPeakMassTolerance
        '        dr.Item("SQ_AA_In_Uppercase") = .AminoAcidsAllUpperCase
        '        dr.Item("SQ_Partial_Sequence") = .PartialSequenceToMatch
        '        dr.Item("SQ_Seq_Header_Filter") = .SequenceHeaderInfoToFilter

        '        dr.Item("SQ_Stat_CT_Pep") = .StaticModificationsList.CtermPeptide
        '        dr.Item("SQ_Stat_CT_Prot") = .StaticModificationsList.CtermProtein
        '        dr.Item("SQ_Stat_NT_Pep") = .StaticModificationsList.NtermPeptide
        '        dr.Item("SQ_Stat_NT_Prot") = .StaticModificationsList.CtermProtein
        '        dr.Item("SQ_Stat_G_Gly") = .StaticModificationsList.G_Glycine
        '        dr.Item("SQ_Stat_A_Ala") = .StaticModificationsList.A_Alanine
        '        dr.Item("SQ_Stat_S_Ser") = .StaticModificationsList.S_Serine
        '        dr.Item("SQ_Stat_P_Pro") = .StaticModificationsList.P_Proline
        '        dr.Item("SQ_Stat_V_Val") = .StaticModificationsList.V_Valine
        '        dr.Item("SQ_Stat_T_Thr") = .StaticModificationsList.T_Threonine
        '        dr.Item("SQ_Stat_C_Cys") = .StaticModificationsList.C_Cysteine
        '        dr.Item("SQ_Stat_L_Leu") = .StaticModificationsList.L_Leucine
        '        dr.Item("SQ_Stat_I_Ile") = .StaticModificationsList.I_Isoleucine
        '        dr.Item("SQ_Stat_X_LorI") = .StaticModificationsList.X_LorI
        '        dr.Item("SQ_Stat_N_Asn") = .StaticModificationsList.N_Asparagine
        '        dr.Item("SQ_Stat_O_Orn") = .StaticModificationsList.O_Ornithine
        '        dr.Item("SQ_Stat_B_avgNandD") = .StaticModificationsList.B_avg_NandD
        '        dr.Item("SQ_Stat_D_Asp") = .StaticModificationsList.D_Aspartic_Acid
        '        dr.Item("SQ_Stat_Q_Gln") = .StaticModificationsList.Q_Glutamine
        '        dr.Item("SQ_Stat_E_Glu") = .StaticModificationsList.E_Glutamic_Acid
        '        dr.Item("SQ_Stat_M_Met") = .StaticModificationsList.M_Methionine
        '        dr.Item("SQ_Stat_H_His") = .StaticModificationsList.H_Histidine
        '        dr.Item("SQ_Stat_F_Phe") = .StaticModificationsList.F_Phenylalanine
        '        dr.Item("SQ_Stat_R_Arg") = .StaticModificationsList.R_Arginine
        '        dr.Item("SQ_Stat_Y_Tyr") = .StaticModificationsList.Y_Tyrosine
        '        dr.Item("SQ_Stat_W_Trp") = .StaticModificationsList.W_Tryptophan

        '    End With

        '    'tmpTable.Rows.Add(dr)

        '    'Return tmpTable.Rows(tmpTable.Rows.Count - 1)
        '    Return dr

        'End Function
        'Protected Function GetSingleRowFromDatabase(ByVal SelectSQL As String) As DataRow         'Common

        '    Dim tmpIDTable As DataTable = GetTable(SelectSQL)
        '    If tmpIDTable.Rows.Count = 0 Then
        '        Return Nothing
        '    Else
        '        Dim foundRow As DataRow = tmpIDTable.Rows(0)
        '        Return foundRow
        '    End If

        'End Function

        'Protected Function GetProperties(ByRef theType As Type) As PropertyInfo()         'Common
        '    Dim myProperties As PropertyInfo() = theType.GetProperties((BindingFlags.Public Or BindingFlags.Instance))
        '    Return myProperties
        'End Function

        Protected Function DoesParamSetNameExist(ByVal paramSetName As String) As Boolean         'Common
            Dim tmpID As Integer = GetIDWithName(paramSetName)
            If tmpID < 0 Then
                Return False
            End If
            Return True

        End Function

        Protected Function DoesParamSetIDExist(ByVal paramSetID As Integer) As Boolean                'Common

            Dim IDExists As Boolean = Me.ParamFileTable.Rows.Contains(paramSetID)
            Return IDExists
        End Function

        Protected Function CompareParamSets(ByRef templateSet As clsParams, ByRef checkSet As clsParams) As String         'Neither
            Dim diffCollection As clsDMSParamStorage = GetDiffColl(templateSet, checkSet)
            Return SummarizeDiffColl(diffCollection)
        End Function
        Protected Function GetDiffColl(ByRef templateSet As clsParams, ByRef checkSet As clsParams) As clsDMSParamStorage
            Dim templateColl As clsDMSParamStorage = Me.WriteDataCollectionFromParamSet(templateSet)
            Dim checkColl As clsDMSParamStorage = Me.WriteDataCollectionFromParamSet(checkSet)

            Dim diffCollection As clsDMSParamStorage = CompareDataCollections(templateColl, checkColl)
            Return diffCollection
        End Function
        Protected Function SummarizeDiffColl(ByRef diffColl As clsDMSParamStorage) As String

            Dim maxIndex As Integer = diffColl.Count - 1
            Dim index As Integer
            Dim tmpString As String

            Dim tmpDynMods As String
            Dim tmpStatMods As String
            Dim tmpOtherParams As String

            Dim tmpType As clsDMSParamStorage.ParamTypes
            Dim tmpSpec As String
            Dim tmpValue As String
            Dim tmpSign As String

            For index = 0 To maxIndex
                With diffColl.Item(index)
                    tmpType = .Type
                    tmpSpec = .Specifier
                    tmpValue = .Value

                    If tmpValue > 0 Then
                        tmpSign = "+"
                    ElseIf tmpValue = 0 Then
                        tmpSign = ""
                    Else
                        tmpSign = "-"
                    End If

                End With
                If tmpType = clsDMSParamStorage.ParamTypes.StaticModification Then
                    If tmpStatMods = "" Then
                        tmpStatMods = "Static Mods: "
                    End If
                    tmpStatMods = tmpStatMods & tmpSpec & " (" & tmpSign & tmpValue & "), "
                ElseIf tmpType = clsDMSParamStorage.ParamTypes.DynamicModification Then
                If tmpDynMods = "" Then
                    tmpDynMods = "Dynamic Mods: "
                End If
                    tmpDynMods = tmpDynMods & tmpSpec & " (" & tmpSign & tmpValue & "), "
                    Else
                If tmpOtherParams = "" Then
                    tmpOtherParams = "Other Parameters: "
                End If
                tmpOtherParams = tmpOtherParams & tmpSpec & "=" & tmpValue & ", "
                    End If
            Next

            If Right(tmpDynMods, 2) <> ", " And Not tmpDynMods Is Nothing Then
                tmpDynMods = tmpDynMods & ", "
            End If

            If Right(tmpStatMods, 2) <> ", " And Not tmpStatMods Is Nothing Then
                tmpStatMods = tmpStatMods & ", "
            End If

            If Right(tmpOtherParams, 2) = ", " And Not tmpOtherParams Is Nothing Then
                tmpOtherParams = Left(tmpOtherParams, Len(tmpOtherParams) - 2)
            End If

            tmpString = tmpDynMods & tmpStatMods & tmpOtherParams

            If Right(tmpString, 2) = ", " Then
                tmpString = Left(tmpString, Len(tmpString) - 2)
            End If


            'For index = 0 To maxIndex
            '    If index <> 0 Then
            '        tmpString = tmpString & ", "
            '    End If
            '    tmpString = tmpString & diffColl.Item(index).Type.ToString & " - " & diffColl.Item(index).Specifier & " = " & diffColl.Item(index).Value
            'Next

            If tmpString = Nothing Then tmpString = " --No Change-- "

            Return tmpString


        End Function

        Protected Function CompareDataCollections(ByVal templateColl As clsDMSParamStorage, ByVal checkColl As clsDMSParamStorage) As clsDMSParamStorage        'Neither
            Dim diffColl As New clsDMSParamStorage
            Dim maxIndex As Integer = checkColl.Count - 1
            Dim templateIndex As Integer
            Dim checkIndex As Integer

            Dim tmpString As String
            Dim tmpTemp As String
            Dim tmpCheck As String

            Dim tmpCType As clsDMSParamStorage.ParamTypes
            Dim tmpCSpec As String
            Dim tmpCVal As String

            Dim tmpTType As clsDMSParamStorage.ParamTypes
            Dim tmpTSpec As String
            Dim tmpTVal As String

            For checkIndex = 0 To maxIndex
                With checkColl.Item(checkIndex)
                    tmpCType = .Type
                    tmpCSpec = .Specifier
                    tmpCVal = .Value
                End With

                templateIndex = templateColl.IndexOf(tmpCSpec, tmpCType)

                If templateIndex >= 0 Then
                    With templateColl.Item(templateIndex)
                        tmpTType = .Type
                        tmpTSpec = .Specifier
                        tmpTVal = .Value
                        tmpTemp = tmpTType.ToString & " - " & tmpTSpec & " = " & tmpTVal
                        tmpCheck = tmpCType.ToString & " - " & tmpCSpec & " = " & tmpCVal
                    End With

                    If tmpTType.ToString & tmpTSpec = tmpCType.ToString & tmpCSpec Then
                        If tmpTVal.Equals(tmpCVal) Then

                        Else
                            diffColl.Add(tmpCSpec, tmpCVal, tmpTType)
                        End If
                    End If
                Else
                    diffColl.Add(tmpCSpec, tmpCVal, tmpCType)
                End If


            Next

            Return diffColl

        End Function

#End Region

    End Class

End Namespace