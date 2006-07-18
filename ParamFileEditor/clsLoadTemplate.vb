Imports System.Collections.Specialized
Imports System.IO

Friend Class clsLoadTemplate
    Inherits clsParamsNew

    Protected Function LoadTemplate(ByVal TemplateFileName As String) As Boolean
        Dim ParamNameCollection As StringCollection
        Dim m_TemplateFilePath As String = GetFilePath(TemplateFileName)
        Dim m_template As New clsRetrieveParams(m_TemplateFilePath)
        Dim m_SectionName As String
        Dim ParamKeys As StringCollection
        Dim ParamKey As String

        Dim tmpName As String
        Dim tmpDesc As String
        Dim tmpValue As Object

        With m_template
            m_SectionName = "SEQUEST"
            ParamKeys = .GetAllKeysInSection(m_SectionName)
            For Each ParamKey In ParamKeys
                tmpName = ParamKey
                tmpValue = .GetParam(ParamKey)
                tmpDesc = Nothing
                Me.Add(tmpName, tmpDesc, tmpValue)
            Next



        End With
    End Function

    Protected Function GetFilePath(ByVal templateFileName As String) As String
        Dim fi As New FileInfo(Application.ExecutablePath)
        Return Path.Combine(fi.DirectoryName, templateFileName)
    End Function

    'Private Function LoadTemplateParams(ByVal TemplateFileName As String) As Boolean
    '    Dim Success As Boolean
    '    Dim tmpProtMassFilterStringArray() As String
    '    Dim SectionName As String
    '    m_templateFilePath = GetFilePath(TemplateFileName)
    '    Dim m_getEnzymeList As New clsGetEnzymeBlock(m_templateFilePath, DEF_ENZ_SECTION_NAME)
    '    m_enzymeDetailStorage = m_getEnzymeList.EnzymeList
    '    Dim m_enzymedetails As New clsEnzymeDetails

    '    m_desc = GetDescription()
    '    m_type = Me.GetTemplateType

    '    If m_type = ParamFileTypes.BioWorks_20 Then
    '        SectionName = "SEQUEST"
    '    ElseIf m_type = ParamFileTypes.BioWorks_30 Then
    '        SectionName = "TurboSEQUEST"
    '    End If

    '    m_fullTemplate = New clsRetrieveParams(m_templateFilePath)
    '    'Retrieve Basic Parameters
    '    With m_fullTemplate
    '        .SetSection(SectionName)
    '        Me.SelectedEnzymeIndex = CInt(.GetParam("enzyme_number"))
    '        Me.SelectedEnzymeDetails = m_enzymeDetailStorage(m_enzymeNumber)
    '        Me.MaximumNumberMissedCleavages = CInt(.GetParam("max_num_internal_cleavage_sites"))
    '        Me.ParentMassType = CType(CInt(.GetParam("mass_type_parent")), IBasicParams.MassTypeList)
    '        Me.FragmentMassType = CType(CInt(.GetParam("mass_type_fragment")), IBasicParams.MassTypeList)
    '        Me.PartialSequenceToMatch = .GetParam("partial_sequence")
    '        Me.DynamicMods = New clsDynamicMods(.GetParam("diff_search_options"))
    '        Me.StaticModificationsList = New clsStaticMods

    '        'Get Static Mods
    '        Me.StaticModificationsList.CtermPeptide = CSng(.GetParam("add_Cterm_peptide"))
    '        Me.StaticModificationsList.CtermProtein = CSng(.GetParam("add_Cterm_protein"))
    '        Me.StaticModificationsList.NtermPeptide = CSng(.GetParam("add_Nterm_peptide"))
    '        Me.StaticModificationsList.NtermProtein = CSng(.GetParam("add_Nterm_protein"))
    '        Me.StaticModificationsList.G_Glycine = CSng(.GetParam("add_G_Glycine"))
    '        Me.StaticModificationsList.A_Alanine = CSng(.GetParam("add_A_Alanine"))
    '        Me.StaticModificationsList.S_Serine = CSng(.GetParam("add_S_Serine"))
    '        Me.StaticModificationsList.P_Proline = CSng(.GetParam("add_P_Proline"))
    '        Me.StaticModificationsList.V_Valine = CSng(.GetParam("add_V_Valine"))
    '        Me.StaticModificationsList.T_Threonine = CSng(.GetParam("add_T_Threonine"))
    '        Me.StaticModificationsList.C_Cysteine = CSng(.GetParam("add_C_Cysteine"))
    '        Me.StaticModificationsList.L_Leucine = CSng(.GetParam("add_L_Leucine"))
    '        Me.StaticModificationsList.I_Isoleucine = CSng(.GetParam("add_I_Isoleucine"))
    '        Me.StaticModificationsList.X_LorI = CSng(.GetParam("add_X_LorI"))
    '        Me.StaticModificationsList.N_Asparagine = CSng(.GetParam("add_N_Asparagine"))
    '        Me.StaticModificationsList.O_Ornithine = CSng(.GetParam("add_O_Ornithine"))
    '        Me.StaticModificationsList.B_avg_NandD = CSng(.GetParam("add_B_avg_NandD"))
    '        Me.StaticModificationsList.D_Aspartic_Acid = CSng(.GetParam("add_D_Aspartic_Acid"))
    '        Me.StaticModificationsList.Q_Glutamine = CSng(.GetParam("add_Q_Glutamine"))
    '        Me.StaticModificationsList.K_Lysine = CSng(.GetParam("add_K_Lysine"))
    '        Me.StaticModificationsList.Z_avg_QandE = CSng(.GetParam("add_Z_avg_QandE"))
    '        Me.StaticModificationsList.E_Glutamic_Acid = CSng(.GetParam("add_E_Glutamic_Acid"))
    '        Me.StaticModificationsList.M_Methionine = CSng(.GetParam("add_M_Methionine"))
    '        Me.StaticModificationsList.H_Histidine = CSng(.GetParam("add_H_Histidine"))
    '        Me.StaticModificationsList.F_Phenylalanine = CSng(.GetParam("add_F_Phenylalanine"))
    '        Me.StaticModificationsList.R_Arginine = CSng(.GetParam("add_R_Arginine"))
    '        Me.StaticModificationsList.Y_Tyrosine = CSng(.GetParam("add_Y_Tyrosine"))
    '        Me.StaticModificationsList.W_Tryptophan = CSng(.GetParam("add_W_Tryptophan"))
    '    End With

    '    'Retrieve Advanced Parameters
    '    With m_fullTemplate
    '        .SetSection(SectionName)
    '        If m_type = ParamFileTypes.BioWorks_20 Then
    '            Me.DefaultFASTAPath = .GetParam("database_name")
    '            Me.DefaultFASTAPath2 = ""
    '            Me.NumberOfResultsToProcess = 500
    '        ElseIf m_type = ParamFileTypes.BioWorks_30 Then
    '            Me.DefaultFASTAPath = .GetParam("first_database_name")
    '            Me.DefaultFASTAPath2 = .GetParam("second_database_name")
    '            Me.NumberOfResultsToProcess = .GetParam("num_results")
    '        End If
    '        Me.PeptideMassTolerance = CSng(.GetParam("peptide_mass_tolerance"))
    '        Me.CreateOutputFiles = CBool(.GetParam("create_output_files"))
    '        m_ionSeriesString = .GetParam("ion_series")
    '        Me.IonSeries = New clsIonSeries(m_ionSeriesString)
    '        Me.MaximumNumAAPerDynMod = CInt(.GetParam("max_num_differential_AA_per_mod"))
    '        Me.FragmentIonTolerance = CSng(.GetParam("fragment_ion_tolerance"))
    '        Me.NumberOfOutputLines = CInt(.GetParam("num_output_lines"))
    '        Me.NumberOfDescriptionLines = CInt(.GetParam("num_description_lines"))
    '        Me.ShowFragmentIons = CBool(.GetParam("show_fragment_ions"))
    '        Me.PrintDuplicateReferences = CBool(.GetParam("print_duplicate_references"))
    '        Me.SelectedNucReadingFrame = CType(CInt(.GetParam("enzyme_number")), IAdvancedParams.FrameList)
    '        Me.RemovePrecursorPeak = CBool(.GetParam("remove_precursor_peak"))
    '        Me.IonCutoffPercentage = CSng(.GetParam("ion_cutoff_percentage"))
    '        m_protMassFilterString = .GetParam("protein_mass_filter")
    '        tmpProtMassFilterStringArray = InterpretMassFilterString(m_protMassFilterString)
    '        Me.MinimumProteinMassToSearch = CSng(tmpProtMassFilterStringArray(0))
    '        Me.MaximumProteinMassToSearch = CSng(tmpProtMassFilterStringArray(1))
    '        Me.NumberOfDetectedPeaksToMatch = CInt(.GetParam("match_peak_count"))
    '        Me.NumberOfAllowedDetectedPeakErrors = CInt(.GetParam("match_peak_allowed_error"))
    '        Me.MatchedPeakMassTolerance = CSng(.GetParam("match_peak_tolerance"))
    '        Me.AminoAcidsAllUpperCase = CBool(.GetParam("residues_in_upper_case"))
    '        Me.SequenceHeaderInfoToFilter = .GetParam("sequence_header_filter")
    '        Me.DMS_ID = -1
    '    End With

    '    Return Success
    'End Function



End Class
