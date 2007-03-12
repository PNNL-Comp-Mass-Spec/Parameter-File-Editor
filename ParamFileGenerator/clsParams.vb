Imports System.IO
Imports ParamFileGenerator
Imports ParamFileGenerator.DownloadParams


Public Interface IBasicParams
    Enum MassTypeList As Integer
        Average = 0
        Monoisotopic = 1
    End Enum
    ReadOnly Property FileType() As clsParams.ParamFileTypes
    Property DMS_ID() As Integer
    Property FileName() As String
    Property Description() As String
    Property SelectedEnzymeDetails() As clsEnzymeDetails
    Property SelectedEnzymeIndex() As Integer
    Property SelectedEnzymeCleavagePosition() As Integer
    Property MaximumNumberMissedCleavages() As Integer
    Property ParentMassType() As MassTypeList
    Property FragmentMassType() As MassTypeList
    Property DynamicMods() As clsDynamicMods
    Property TermDynamicMods() As clsTermDynamicMods
    Property StaticModificationsList() As clsStaticMods
    Property IsotopicModificationsList() As clsIsoMods
    Property PartialSequenceToMatch() As String
    Property EnzymeList() As clsEnzymeCollection
    Function RetrieveEnzymeDetails(ByVal index As Integer) As clsEnzymeDetails
End Interface
Public Interface IAdvancedParams
    Enum FrameList As Integer
        Protein = 0
        Forward_1 = 1
        Forward_2 = 2
        Forward_3 = 3
        Reverse_1 = 4
        Reverse_2 = 5
        Reverse_3 = 6
        All_3_Forward = 7
        All_3_Reverse = 8
        All_Six = 9

    End Enum
    Property DefaultFASTAPath() As String
    Property DefaultFASTAPath2() As String
    Property CreateOutputFiles() As Boolean
    Property IonSeries() As clsIonSeries
    Property NumberOfResultsToProcess() As Integer
    Property MaximumNumAAPerDynMod() As Integer
    Property MaximumDifferentialPerPeptide() As Integer
    Property PeptideMassTolerance() As Single
    Property FragmentIonTolerance() As Single
    Property NumberOfOutputLines() As Integer
    Property NumberOfDescriptionLines() As Integer
    Property ShowFragmentIons() As Boolean
    Property PrintDuplicateReferences() As Boolean
    Property SelectedNucReadingFrameIndex() As Integer
    Property SelectedNucReadingFrame() As FrameList
    Property RemovePrecursorPeak() As Boolean
    Property IonCutoffPercentage() As Single
    Property MinimumProteinMassToSearch() As Single
    Property MaximumProteinMassToSearch() As Single
    Property NumberOfDetectedPeaksToMatch() As Integer
    Property NumberOfAllowedDetectedPeakErrors() As Integer
    Property MatchedPeakMassTolerance() As Single
    Property AminoAcidsAllUpperCase() As Boolean
    Property SequenceHeaderInfoToFilter() As String
    Property UsePhosphoFragmentation() As Integer
    Property PeptideMassUnits() As Integer
End Interface

Public Class clsParams
    Implements IBasicParams
    Implements IAdvancedParams

    Public Enum ParamFileTypes
        BioWorks_20  'Normal BioWorks 2.0 Sequest
        BioWorks_30  'BioWorks 3.0+ TurboSequest
        BioWorks_31  'BioWorks 3.1 ClusterQuest
        BioWorks_32  'BioWorks 3.2 ClusterF***
    End Enum

    Const DEF_DB_NAME As String = "C:\Xcalibur\database\nr.fasta"     'Not really used, just a placeholder

#Region " Private Variables "
    Const DEF_ENZ_SECTION_NAME As String = "SEQUEST_ENZYME_INFO"

    'Basic Parameters
    Private m_ID As Integer
    Private m_fileName As String
    Private m_desc As String
    Private m_type As ParamFileTypes
    Private m_typeIndex As Integer
    Private m_enzymeNumber As Integer
    Private m_enzymeCleavagePosition As Integer
    Private m_enzymeDetails As clsEnzymeDetails
    Private m_maxICSites As Integer
    Private m_parentMassType As IBasicParams.MassTypeList
    Private m_fragMassType As IBasicParams.MassTypeList
    Private m_dynMods As clsDynamicMods
    Private m_termDynMods As clsTermDynamicMods
    Private m_staticModsList As clsStaticMods
    Private m_partialSeq As String

    Private m_isoMods As clsIsoMods

    'Advanced Parameters
    Private m_Path As String
    Private m_defDBPath1 As String              'A
    Private m_defDBPath2 As String              'A
    Private m_numResults As Integer
    Private m_pepMassTol As Single              'A
    Private m_createOutputFiles As Boolean      'A
    Private m_ionSeriesString As String         'A
    Private m_ionSeries As clsIonSeries         'A
    Private m_maxAAPerDynMod As Integer         'A
    Private m_maxDiffPerPeptide As Integer = 3  'A
    Private m_fragIonTol As Single              'A
    Private m_numOutLines As Integer            'A
    Private m_numDescLines As Integer           'A
    Private m_showFragIons As Boolean           'A
    Private m_printDupRef As Boolean            'A
    Private m_readingFrameIndex As Integer      'A
    Private m_readingFrame As IAdvancedParams.FrameList         'A
    Private m_removePrecursorPeak As Boolean    'A
    Private m_ionCutoffPer As Single            'A
    Private m_protMassFilterString As String
    Private m_minProtMassFilter As Single       'A
    Private m_maxProtMassFilter As Single       'A
    Private m_matchPeakCount As Integer         'A
    Private m_matchPeakCountErrors As Integer   'A
    Private m_matchPeakTol As Single
    Private m_upperCase As Boolean              'A
    Private m_seqHdrFilter As String            'A
    Private m_peptideMassUnits As Integer = 0   'A
    Private m_usePhosphoFragmentation As Integer = 0
    Private m_enzymeDetailStorage As clsEnzymeCollection

    Private m_fullTemplate As clsRetrieveParams
    Private m_templateFilePath As String
    'Shared s_BaseLineParamSet As clsParams
#End Region

#Region " clsParams Properties "
    Public Shared ReadOnly Property BaseLineParamSet() As clsParams
        Get
            Return ParamFileGenerator.clsMainProcess.BaseLineParamSet
        End Get
    End Property

    Public ReadOnly Property FileType() As clsParams.ParamFileTypes Implements IBasicParams.FileType
        Get
            Return m_type
        End Get
    End Property
    Public Property DMS_ID() As Integer Implements IBasicParams.DMS_ID
        Get
            Return m_ID
        End Get
        Set(ByVal Value As Integer)
            m_ID = Value
        End Set
    End Property
    Public Property FileName() As String Implements IBasicParams.FileName
        Get
            Return m_fileName
        End Get
        Set(ByVal Value As String)
            m_fileName = Value
        End Set
    End Property
    Public Property FileTypeIndex() As Integer
        Get
            Return m_typeIndex
        End Get
        Set(ByVal Value As Integer)
            m_typeIndex = Value
        End Set
    End Property
    Public Property Description() As String Implements IBasicParams.Description
        Get
            Return m_desc
        End Get
        Set(ByVal Value As String)
            m_desc = Value
        End Set
    End Property
    Public Property DefaultFASTAPath() As String Implements IAdvancedParams.DefaultFASTAPath
        Get
            Return m_defDBPath1
        End Get
        Set(ByVal Value As String)
            m_defDBPath1 = Value
        End Set
    End Property
    Public Property DefaultFASTAPath2() As String Implements IAdvancedParams.DefaultFASTAPath2
        Get
            Return m_defDBPath2
        End Get
        Set(ByVal Value As String)
            m_defDBPath2 = Value
        End Set
    End Property
    Public Property NumberOfResultsToProcess() As Integer Implements IAdvancedParams.NumberOfResultsToProcess
        Get
            Return m_numResults
        End Get
        Set(ByVal Value As Integer)
            m_numResults = Value
        End Set
    End Property
    Public Property PeptideMassTolerance() As Single Implements IAdvancedParams.PeptideMassTolerance
        Get
            Return m_pepMassTol
        End Get
        Set(ByVal Value As Single)
            m_pepMassTol = Value
        End Set
    End Property
    Public Property CreateOutputFiles() As Boolean Implements IAdvancedParams.CreateOutputFiles
        Get
            Return m_createOutputFiles
        End Get
        Set(ByVal Value As Boolean)
            m_createOutputFiles = Value
        End Set
    End Property
    Public Property IonSeries() As clsIonSeries Implements IAdvancedParams.IonSeries
        Get
            Return m_ionSeries
        End Get
        Set(ByVal Value As clsIonSeries)
            m_ionSeries = Value
        End Set
    End Property
    Public Property DynamicMods() As clsDynamicMods Implements IBasicParams.DynamicMods
        Get
            Return m_dynMods
        End Get
        Set(ByVal Value As clsDynamicMods)
            m_dynMods = Value
        End Set
    End Property
    Public Property TermDynamicMods() As clsTermDynamicMods Implements IBasicParams.TermDynamicMods
        Get
            Return Me.m_termDynMods
        End Get
        Set(ByVal Value As clsTermDynamicMods)
            Me.m_termDynMods = Value
        End Set
    End Property
    Public Property IsotopicMods() As clsIsoMods Implements IBasicParams.IsotopicModificationsList
        Get
            Return Me.m_isoMods
        End Get
        Set(ByVal Value As clsIsoMods)
            Me.m_isoMods = Value
        End Set
    End Property
    Public Property MaximumNumAAPerDynMod() As Integer Implements IAdvancedParams.MaximumNumAAPerDynMod
        Get
            Return m_maxAAPerDynMod
        End Get
        Set(ByVal Value As Integer)
            m_maxAAPerDynMod = Value
        End Set
    End Property
    Public Property MaximumNumDifferentialPerPeptide() As Integer Implements IAdvancedParams.MaximumDifferentialPerPeptide
        Get
            Return m_maxDiffPerPeptide
        End Get
        Set(ByVal Value As Integer)
            m_maxDiffPerPeptide = Value
        End Set
    End Property
    Public Property UsePhosphoFragmentation() As Integer Implements IAdvancedParams.UsePhosphoFragmentation
        Get
            Return m_usePhosphoFragmentation
        End Get
        Set(ByVal Value As Integer)
            m_usePhosphoFragmentation = Value
        End Set
    End Property
    Public Property FragmentIonTolerance() As Single Implements IAdvancedParams.FragmentIonTolerance
        Get
            Return m_fragIonTol
        End Get
        Set(ByVal Value As Single)
            m_fragIonTol = Value
        End Set
    End Property
    Public Property NumberOfOutputLines() As Integer Implements IAdvancedParams.NumberOfOutputLines
        Get
            Return m_numOutLines
        End Get
        Set(ByVal Value As Integer)
            m_numOutLines = Value
        End Set
    End Property
    Public Property NumberOfDescriptionLines() As Integer Implements IAdvancedParams.NumberOfDescriptionLines
        Get
            Return m_numDescLines
        End Get
        Set(ByVal Value As Integer)
            m_numDescLines = Value
        End Set
    End Property
    Public Property ShowFragmentIons() As Boolean Implements IAdvancedParams.ShowFragmentIons
        Get
            Return m_showFragIons
        End Get
        Set(ByVal Value As Boolean)
            m_showFragIons = Value
        End Set
    End Property
    Public Property PrintDuplicateReferences() As Boolean Implements IAdvancedParams.PrintDuplicateReferences
        Get
            Return m_printDupRef
        End Get
        Set(ByVal Value As Boolean)
            m_printDupRef = Value
        End Set
    End Property
    Public Property SelectedEnzymeDetails() As clsEnzymeDetails Implements IBasicParams.SelectedEnzymeDetails
        Get
            Return m_enzymeDetails
        End Get
        Set(ByVal Value As clsEnzymeDetails)
            If Me.m_enzymeDetails Is Nothing Then
                m_enzymeDetails = New clsEnzymeDetails
            End If
            m_enzymeDetails = Value
        End Set
    End Property
    Public Property SelectedEnzymeIndex() As Integer Implements IBasicParams.SelectedEnzymeIndex
        Get
            Return m_enzymeNumber
        End Get
        Set(ByVal Value As Integer)
            m_enzymeNumber = Value
        End Set
    End Property
    Public Property SelectedEnzymeCleavagePosition() As Integer Implements IBasicParams.SelectedEnzymeCleavagePosition
        Get
            Return Me.m_enzymeCleavagePosition
        End Get
        Set(ByVal Value As Integer)
            Me.m_enzymeCleavagePosition = Value
        End Set
    End Property
    Public Property SelectedNucReadingFrame() As IAdvancedParams.FrameList Implements IAdvancedParams.SelectedNucReadingFrame
        Get
            Return m_readingFrame
        End Get
        Set(ByVal Value As IAdvancedParams.FrameList)
            m_readingFrame = Value
        End Set
    End Property
    Public Property SelectedNucReadingFrameIndex() As Integer Implements IAdvancedParams.SelectedNucReadingFrameIndex
        Get
            Return m_readingFrameIndex
        End Get
        Set(ByVal Value As Integer)
            m_readingFrameIndex = Value
        End Set
    End Property
    Public Property ParentMassType() As IBasicParams.MassTypeList Implements IBasicParams.ParentMassType
        Get
            Return m_parentMassType
        End Get
        Set(ByVal Value As IBasicParams.MassTypeList)
            m_parentMassType = Value
        End Set
    End Property
    Public Property FragmentMassType() As IBasicParams.MassTypeList Implements IBasicParams.FragmentMassType
        Get
            Return m_fragMassType
        End Get
        Set(ByVal Value As IBasicParams.MassTypeList)
            m_fragMassType = Value
        End Set
    End Property
    Public Property RemovePrecursorPeak() As Boolean Implements IAdvancedParams.RemovePrecursorPeak
        Get
            Return m_removePrecursorPeak
        End Get
        Set(ByVal Value As Boolean)
            m_removePrecursorPeak = Value
        End Set
    End Property
    Public Property IonCutoffPercentage() As Single Implements IAdvancedParams.IonCutoffPercentage
        Get
            Return m_ionCutoffPer
        End Get
        Set(ByVal Value As Single)
            m_ionCutoffPer = Value
        End Set
    End Property
    Public Property MaximumNumberMissedCleavages() As Integer Implements IBasicParams.MaximumNumberMissedCleavages
        Get
            Return m_maxICSites
        End Get
        Set(ByVal Value As Integer)
            m_maxICSites = Value
        End Set
    End Property
    Public Property MinimumProteinMassToSearch() As Single Implements IAdvancedParams.MinimumProteinMassToSearch
        Get
            Return m_minProtMassFilter
        End Get
        Set(ByVal Value As Single)
            m_minProtMassFilter = Value
        End Set
    End Property
    Public Property MaximumProteinMassToSearch() As Single Implements IAdvancedParams.MaximumProteinMassToSearch
        Get
            Return m_maxProtMassFilter
        End Get
        Set(ByVal Value As Single)
            m_maxProtMassFilter = Value
        End Set
    End Property
    Public Property NumberOfDetectedPeaksToMatch() As Integer Implements IAdvancedParams.NumberOfDetectedPeaksToMatch
        Get
            Return m_matchPeakCount
        End Get
        Set(ByVal Value As Integer)
            m_matchPeakCount = Value
        End Set
    End Property
    Public Property NumberOfAllowedDetectedPeakErrors() As Integer Implements IAdvancedParams.NumberOfAllowedDetectedPeakErrors
        Get
            Return m_matchPeakCountErrors
        End Get
        Set(ByVal Value As Integer)
            m_matchPeakCountErrors = Value
        End Set
    End Property
    Public Property MatchedPeakMassTolerance() As Single Implements IAdvancedParams.MatchedPeakMassTolerance
        Get
            Return m_matchPeakTol
        End Get
        Set(ByVal Value As Single)
            m_matchPeakTol = Value
        End Set
    End Property
    Public Property AminoAcidsAllUpperCase() As Boolean Implements IAdvancedParams.AminoAcidsAllUpperCase
        Get
            Return m_upperCase
        End Get
        Set(ByVal Value As Boolean)
            m_upperCase = Value
        End Set
    End Property
    Public Property PartialSequenceToMatch() As String Implements IBasicParams.PartialSequenceToMatch
        Get
            Return m_partialSeq
        End Get
        Set(ByVal Value As String)
            m_partialSeq = Value
        End Set
    End Property
    Public Property SequenceHeaderInfoToFilter() As String Implements IAdvancedParams.SequenceHeaderInfoToFilter
        Get
            Return m_seqHdrFilter
        End Get
        Set(ByVal Value As String)
            m_seqHdrFilter = Value
        End Set
    End Property
    Public Property PeptideMassUnits() As Integer Implements IAdvancedParams.PeptideMassUnits
        Get
            Return Me.m_peptideMassUnits
        End Get
        Set(ByVal Value As Integer)
            Me.m_peptideMassUnits = Value
        End Set
    End Property
    Public Property StaticModificationsList() As clsStaticMods Implements IBasicParams.StaticModificationsList
        Get
            Return m_staticModsList
        End Get
        Set(ByVal Value As clsStaticMods)
            If Me.m_staticModsList Is Nothing Then
                m_staticModsList = New clsStaticMods
            End If
            m_staticModsList = Value
        End Set
    End Property
    Public Property EnzymeList() As clsEnzymeCollection Implements IBasicParams.EnzymeList
        Get
            Return m_enzymeDetailStorage
        End Get
        Set(ByVal Value As clsEnzymeCollection)
            m_enzymeDetailStorage = Value
        End Set
    End Property
    Public Function RetrieveEnzymeDetails(ByVal EnzymeListIndex As Integer) As clsEnzymeDetails Implements IBasicParams.RetrieveEnzymeDetails
        Return m_enzymeDetailStorage.Item(EnzymeListIndex)
    End Function
#End Region

    Public Sub New()
        m_ionSeries = New clsIonSeries
        m_enzymeDetailStorage = New clsEnzymeCollection
        m_enzymeDetails = New clsEnzymeDetails
        m_dynMods = New clsDynamicMods
        m_staticModsList = New clsStaticMods
        m_isoMods = New clsIsoMods
        'If s_BaseLineParamSet Is Nothing Then
        '    s_BaseLineParamSet = New clsParams
        '    s_BaseLineParamSet.LoadTemplate(Me.DEF_TEMPLATE_FILEPATH)
        'End If


    End Sub

    Public Function ReturnMassFilter(ByVal MinimumMassToFilter As Single, ByVal MaximumMassToFilter As Single) As String
        Return ReturnMassFilterString(MinimumMassToFilter, MaximumMassToFilter)
    End Function


    Public Sub LoadTemplate(ByVal templateFileName As String)
        Dim loadingSuccessful As Boolean
        loadingSuccessful = LoadTemplateParams(templateFileName)
    End Sub

    Private Function LoadTemplateParams(ByVal TemplateFileName As String) As Boolean
        Dim Success As Boolean
        Dim tmpProtMassFilterStringArray() As String
        Dim SectionName As String
        m_templateFilePath = GetFilePath(TemplateFileName)
        Dim m_getEnzymeList As New clsGetEnzymeBlock(m_templateFilePath, DEF_ENZ_SECTION_NAME)
        m_enzymeDetailStorage = m_getEnzymeList.EnzymeList
        Dim m_enzymedetails As New clsEnzymeDetails
        'Dim m_Compare As New clsParamsFromDMS

        'm_desc = GetDescription()
        m_type = Me.GetTemplateType

        'If m_type = ParamFileTypes.BioWorks_20 Then
        SectionName = "SEQUEST"
        'ElseIf m_type = ParamFileTypes.BioWorks_30 Or m_type = ParamFileTypes.BioWorks_31 Then
        '    SectionName = "SEQUEST"
        'End If

        m_fullTemplate = New clsRetrieveParams(m_templateFilePath)
        'Retrieve Basic Parameters
        With m_fullTemplate
            .SetSection(SectionName)
            Me.FileName = System.IO.Path.GetFileName(Me.m_templateFilePath)
            Me.SelectedEnzymeIndex = CInt(.GetParam("enzyme_number"))
            Me.SelectedEnzymeDetails = m_enzymeDetailStorage(m_enzymeNumber)
            Me.SelectedEnzymeCleavagePosition = 1
            Me.MaximumNumberMissedCleavages = CInt(.GetParam("max_num_internal_cleavage_sites"))
            Me.ParentMassType = CType(CInt(.GetParam("mass_type_parent")), IBasicParams.MassTypeList)
            Me.FragmentMassType = CType(CInt(.GetParam("mass_type_fragment")), IBasicParams.MassTypeList)
            Me.PartialSequenceToMatch = .GetParam("partial_sequence")
            Me.DynamicMods = New clsDynamicMods(.GetParam("diff_search_options"))
            Me.TermDynamicMods = New clsTermDynamicMods(.GetParam("term_diff_search_options"))
            Me.StaticModificationsList = New clsStaticMods


            'Get Static Mods
            Me.StaticModificationsList.CtermPeptide = CSng(.GetParam("add_Cterm_peptide"))
            Me.StaticModificationsList.CtermProtein = CSng(.GetParam("add_Cterm_protein"))
            Me.StaticModificationsList.NtermPeptide = CSng(.GetParam("add_Nterm_peptide"))
            Me.StaticModificationsList.NtermProtein = CSng(.GetParam("add_Nterm_protein"))
            Me.StaticModificationsList.G_Glycine = CSng(.GetParam("add_G_Glycine"))
            Me.StaticModificationsList.A_Alanine = CSng(.GetParam("add_A_Alanine"))
            Me.StaticModificationsList.S_Serine = CSng(.GetParam("add_S_Serine"))
            Me.StaticModificationsList.P_Proline = CSng(.GetParam("add_P_Proline"))
            Me.StaticModificationsList.V_Valine = CSng(.GetParam("add_V_Valine"))
            Me.StaticModificationsList.T_Threonine = CSng(.GetParam("add_T_Threonine"))
            Me.StaticModificationsList.C_Cysteine = CSng(.GetParam("add_C_Cysteine"))
            Me.StaticModificationsList.L_Leucine = CSng(.GetParam("add_L_Leucine"))
            Me.StaticModificationsList.I_Isoleucine = CSng(.GetParam("add_I_Isoleucine"))
            Me.StaticModificationsList.X_LorI = CSng(.GetParam("add_X_LorI"))
            Me.StaticModificationsList.N_Asparagine = CSng(.GetParam("add_N_Asparagine"))
            Me.StaticModificationsList.O_Ornithine = CSng(.GetParam("add_O_Ornithine"))
            Me.StaticModificationsList.B_avg_NandD = CSng(.GetParam("add_B_avg_NandD"))
            Me.StaticModificationsList.D_Aspartic_Acid = CSng(.GetParam("add_D_Aspartic_Acid"))
            Me.StaticModificationsList.Q_Glutamine = CSng(.GetParam("add_Q_Glutamine"))
            Me.StaticModificationsList.K_Lysine = CSng(.GetParam("add_K_Lysine"))
            Me.StaticModificationsList.Z_avg_QandE = CSng(.GetParam("add_Z_avg_QandE"))
            Me.StaticModificationsList.E_Glutamic_Acid = CSng(.GetParam("add_E_Glutamic_Acid"))
            Me.StaticModificationsList.M_Methionine = CSng(.GetParam("add_M_Methionine"))
            Me.StaticModificationsList.H_Histidine = CSng(.GetParam("add_H_Histidine"))
            Me.StaticModificationsList.F_Phenylalanine = CSng(.GetParam("add_F_Phenylalanine"))
            Me.StaticModificationsList.R_Arginine = CSng(.GetParam("add_R_Arginine"))
            Me.StaticModificationsList.Y_Tyrosine = CSng(.GetParam("add_Y_Tyrosine"))
            Me.StaticModificationsList.W_Tryptophan = CSng(.GetParam("add_W_Tryptophan"))
        End With

        'add code to check for existence of isotopic mods



        'Retrieve Advanced Parameters
        With m_fullTemplate
            .SetSection(SectionName)
            If m_type = ParamFileTypes.BioWorks_20 Then
                Me.DefaultFASTAPath = .GetParam("database_name")
                Me.DefaultFASTAPath2 = ""
                Me.NumberOfResultsToProcess = 500
            ElseIf m_type = ParamFileTypes.BioWorks_30 Then
                Me.DefaultFASTAPath = .GetParam("first_database_name")
                Me.DefaultFASTAPath2 = .GetParam("second_database_name")
                Me.NumberOfResultsToProcess = DirectCast(.GetParam("num_results"), Integer)
            End If
            Me.PeptideMassTolerance = CSng(.GetParam("peptide_mass_tolerance"))
            If Not .GetParam("create_output_files") Is Nothing Then
                Me.CreateOutputFiles = CBool(.GetParam("create_output_files"))
            Else
                Me.CreateOutputFiles = True
            End If
            m_ionSeriesString = .GetParam("ion_series")
            Me.IonSeries = New clsIonSeries(m_ionSeriesString)
            Me.MaximumNumAAPerDynMod = CInt(.GetParam("max_num_differential_AA_per_mod"))
            If Me.m_type = ParamFileTypes.BioWorks_32 Then
                Me.MaximumNumDifferentialPerPeptide = CInt(.GetParam("max_num_differential_per_peptide"))
            End If
            Me.FragmentIonTolerance = CSng(.GetParam("fragment_ion_tolerance"))
            Me.NumberOfOutputLines = CInt(.GetParam("num_output_lines"))
            Me.NumberOfDescriptionLines = CInt(.GetParam("num_description_lines"))
            Me.ShowFragmentIons = CBool(.GetParam("show_fragment_ions"))
            Me.PrintDuplicateReferences = CBool(.GetParam("print_duplicate_references"))
            Me.SelectedNucReadingFrame = CType(CInt(.GetParam("enzyme_number")), IAdvancedParams.FrameList)
            Me.RemovePrecursorPeak = CBool(.GetParam("remove_precursor_peak"))
            Me.IonCutoffPercentage = CSng(.GetParam("ion_cutoff_percentage"))
            m_protMassFilterString = .GetParam("protein_mass_filter")
            tmpProtMassFilterStringArray = InterpretMassFilterString(m_protMassFilterString)
            Me.MinimumProteinMassToSearch = CSng(tmpProtMassFilterStringArray(0))
            Me.MaximumProteinMassToSearch = CSng(tmpProtMassFilterStringArray(1))
            Me.NumberOfDetectedPeaksToMatch = CInt(.GetParam("match_peak_count"))
            Me.NumberOfAllowedDetectedPeakErrors = CInt(.GetParam("match_peak_allowed_error"))
            Me.MatchedPeakMassTolerance = CSng(.GetParam("match_peak_tolerance"))
            Me.AminoAcidsAllUpperCase = True
            Me.SequenceHeaderInfoToFilter = .GetParam("sequence_header_filter")
            Me.DMS_ID = -1
        End With

        Return Success
    End Function

    Private Function GetDescription() As String

        Dim s As String
        Dim desc As String

        Dim fi As FileInfo
        Dim tr As TextReader

        fi = New FileInfo(m_templateFilePath)
        tr = fi.OpenText
        s = tr.ReadLine
        'Find the correct section block)
        Do While Not s Is Nothing
            If InStr(s, ";DMS_Description = ") > 0 Then
                desc = Mid(s, InStr(s, " = ") + 3)
                Return desc
            End If
            s = tr.ReadLine
        Loop

        Return s

    End Function

    Private Function GetTemplateType() As ParamFileTypes
        Dim s As String
        Dim type As ParamFileTypes

        Dim fi As FileInfo
        Dim tr As TextReader

        fi = New FileInfo(m_templateFilePath)
        tr = fi.OpenText
        s = tr.ReadLine

        Do While Not s Is Nothing
            If InStr(s.ToLower, "num_results = ") > 0 Then
                type = ParamFileTypes.BioWorks_31
                Return type
            Else
                type = ParamFileTypes.BioWorks_20
                Return type
            End If
        Loop

    End Function

    Private Function InterpretMassFilterString(ByVal massFilterString As String) As String()
        Dim s() As String
        Dim counter As Integer
        Dim placeCounter As Integer = 0
        Dim currChar As String
        Dim prevChar As String
        Dim tmpString As String

        For counter = 1 To Len(massFilterString)
            currChar = Mid(massFilterString, counter, 1)
            If (currChar = " " And prevChar <> " ") Or (counter = Len(massFilterString)) Then
                ReDim Preserve s(placeCounter)
                If currChar <> " " Then
                    tmpString = tmpString + currChar
                End If
                s(placeCounter) = tmpString
                placeCounter = placeCounter + 1
                tmpString = ""
            ElseIf currChar <> " " Then
                tmpString = tmpString + currChar
            End If

            prevChar = currChar
        Next

        Return s

    End Function

    Private Function ReturnMassFilterString( _
        ByVal minMass As Single, _
        ByVal maxMass As Single) As String

        Return minMass.ToString.Format("0") & " " & maxMass.ToString.Format("0")

    End Function

    Private Function GetFilePath(ByVal templateFileName As String) As String
        'Dim fi As New FileInfo(Application.ExecutablePath)
        'Return Path.Combine(fi.DirectoryName, templateFileName)
        Return templateFileName
    End Function

    'Private Function DumpToSequestParamFile(ByVal outputPath As String) As Boolean
    '    Dim enz As New clsEnzymeDetails
    '    Dim sc As New System.Collections.Specialized.StringCollection

    '    With sc
    '        .Add("[Sequest]")
    '        .Add(";DMS_Description = " & m_desc)
    '        .Add("database_name = " & m_defDBPath)
    '        .Add("peptide_mass_tolerance = " & m_pepMassTol.ToString.Format("0.0000"))
    '        .Add("create_output_files = " & CType(m_createOutputFiles, Integer).ToString)
    '        .Add("ion_series = " & m_ionSeries.ReturnIonString)
    '        .Add("diff_search_options = " & m_dynMods.ReturnDynModString)
    '        .Add("max_num_differential_AA_per_mod = " & m_maxAAPerDynMod.ToString)
    '        .Add("fragment_ion_tolerance = " & m_fragIonTol.ToString.Format("0.0000"))
    '        ("num_output_lines = " & m_numOutLines.ToString)
    '        ("num_description_lines = " & m_numDescLines.ToString)
    '        ("show_fragment_ions = " & CType(m_showFragIons, Integer).ToString)
    '        ("print_duplicate_references = " & CType(m_printDupRef, Integer).ToString)
    '        ("enzyme_number = " & m_enzymeDetails.EnzymeID.ToString)
    '        ("nucleotide_reading_frame = " & CType(m_readingFrame, Integer).ToString)
    '        ("mass_type_parent = " & CType(m_parentMassType, Integer).ToString)
    '        ("mass_type_fragment = " & CType(m_fragMassType, Integer).ToString)
    '        ("remove_precursor_peak = " & CType(m_removePrecursorPeak, Integer).ToString)
    '        ("ion_cutoff_percentage = " & m_ionCutoffPer.ToString.Format("0.0000"))
    '        ("max_num_internal_cleavage_sites = " & m_maxICSites.ToString)
    '        ("protein_mass_filter = " & ReturnMassFilterString(m_minProtMassFilter, m_maxProtMassFilter))
    '        ("match_peak_count = " & m_matchPeakCount.ToString)
    '        .Add("match_peak_allowed_error = " & m_matchPeakCountErrors)
    '        .Add("residues_in_upper_case = " & CType(m_upperCase, Integer))
    '        .Add("partial_sequence = " & m_partialSeq)
    '        .Add("sequence_header_filter = " & m_seqHdrFilter)
    '        .Add("")
    '        .Add("add_Cterm_peptide = " & m_staticModsList.CtermPeptide.ToString.Format("0.0000"))
    '        .Add("add_Cterm_protein = " & m_staticModsList.CtermProtein.ToString.Format("0.0000"))
    '        .Add("add_Nterm_peptide = " & m_staticModsList.NtermPeptide.ToString.Format("0.0000"))
    '        .Add("add_Nterm_protein = " & m_staticModsList.NtermProtein.ToString.Format("0.0000"))
    '        .Add("add_G_Glycine = " & m_staticModsList.G_Glycine.ToString.Format("0.0000"))
    '        .Add("add_A_Alanine = " & m_staticModsList.A_Alanine.ToString.Format("0.0000"))
    '        .Add("add_S_Serine = " & m_staticModsList.S_Serine.ToString.Format("0.0000"))
    '        .Add("add_P_Proline = " & m_staticModsList.P_Proline.ToString.Format("0.0000"))
    '        .Add("add_V_Valine = " & m_staticModsList.V_Valine.ToString.Format("0.0000"))
    '        .Add("add_T_Threonine = " & m_staticModsList.T_Threonine.ToString.Format("0.0000"))
    '        .Add("add_C_Cysteine = " & m_staticModsList.C_Cysteine.ToString.Format("0.0000"))
    '        .Add("add_L_Leucine = " & m_staticModsList.L_Leucine.ToString.Format("0.0000"))
    '        .Add("add_I_Isoleucine = " & m_staticModsList.I_Isoleucine.ToString.Format("0.0000"))
    '        .Add("add_X_LorI = " & m_staticModsList.X_LorI.ToString.Format("0.0000"))
    '        .Add("add_N_Asparagine = " & m_staticModsList.N_Asparagine.ToString.Format("0.0000"))
    '        .Add("add_O_Ornithine = " & m_staticModsList.O_Ornithine.ToString.Format("0.0000"))
    '        .Add("add_B_avg_NandD = " & m_staticModsList.B_avg_NandD.ToString.Format("0.0000"))
    '        .Add("add_D_Aspartic_Acid = " & m_staticModsList.D_Aspartic_Acid.ToString.Format("0.0000"))
    '        .Add("add_Q_Glutamine = " & m_staticModsList.Q_Glutamine.ToString.Format("0.0000"))
    '        .Add("add_K_Lysine = " & m_staticModsList.K_Lysine.ToString.Format("0.0000"))
    '        .Add("add_Z_avg_QandE = " & m_staticModsList.Z_avg_QandE.ToString.Format("0.0000"))
    '        .Add("add_E_Glutamic_Acid = " & m_staticModsList.E_Glutamic_Acid.ToString.Format("0.0000"))
    '        .Add("add_M_Methionine = " & m_staticModsList.M_Methionine.ToString.Format("0.0000"))
    '        .Add("add_H_Histidine = " & m_staticModsList.H_Histidine.ToString.Format("0.0000"))
    '        .Add("add_F_Phenylalanine = " & m_staticModsList.F_Phenylalanine.ToString.Format("0.0000"))
    '        .Add("add_R_Arginine = " & m_staticModsList.R_Arginine.ToString.Format("0.0000"))
    '        .Add("add_Y_Tyrosine = " & m_staticModsList.Y_Tyrosine.ToString.Format("0.0000"))
    '        .Add("add_W_Tryptophan = " & m_staticModsList.W_Tryptophan.ToString.Format("0.0000"))
    '        .Add("")
    '        .Add("[SEQUEST_ENZYME_INFO]")
    '        For Each enz In m_enzymeDetailStorage
    '            .Add(enz.ReturnEnzymeString)
    '        Next
    '    End With

    '    Dim sw As New StreamWriter(outputPath)
    '    Dim s As String

    '    For Each s In sc
    '        sw.WriteLine(s)
    '    Next

    '    sw.Close()

    'End Function
End Class

Public Class clsIonSeries
    public m_origIonSeriesString As String

    Private m_initialized As Boolean
    Private m_use_a_Ions As Integer
    Private m_use_b_Ions As Integer
    Private m_use_y_Ions As Integer
    Private m_aWeight As Single
    Private m_bWeight As Single
    Private m_cWeight As Single
    Private m_dWeight As Single
    Private m_vWeight As Single
    Private m_wWeight As Single
    Private m_xWeight As Single
    Private m_yWeight As Single
    Private m_zWeight As Single

    Public Sub New(ByVal IonSeriesString As String)
        m_origIonSeriesString = IonSeriesString
        Call ParseISS(m_origIonSeriesString)
    End Sub

    Public Sub New()
        m_origIonSeriesString = Nothing
        m_initialized = True
    End Sub

    Public Sub RevertToOriginalIonString()
        If m_origIonSeriesString <> "" Then
            Call ParseISS(m_origIonSeriesString)
        End If
    End Sub

    Private Sub ParseISS(ByVal ionString As String)
        Dim tmpSplit() As String
        tmpSplit = ionString.Split(System.Convert.ToChar(" "))
        m_use_a_Ions = CInt(tmpSplit(0))
        m_use_b_Ions = CInt(tmpSplit(1))
        m_use_y_Ions = CInt(tmpSplit(2))
        m_aWeight = CSng(tmpSplit(3))
        m_bWeight = CSng(tmpSplit(4))
        m_cWeight = CSng(tmpSplit(5))
        m_dWeight = CSng(tmpSplit(6))
        m_vWeight = CSng(tmpSplit(7))
        m_wWeight = CSng(tmpSplit(8))
        m_xWeight = CSng(tmpSplit(9))
        m_yWeight = CSng(tmpSplit(10))
        m_zWeight = CSng(tmpSplit(11))
    End Sub

#Region " Properties List "

    Public Property Initialized() As Boolean
        Get
            Return m_initialized
        End Get
        Set(ByVal Value As Boolean)
            m_initialized = Value
        End Set
    End Property
    Public Property Use_a_Ions() As Integer
        Get
            Return Math.Abs(m_use_a_Ions)
        End Get
        Set(ByVal Value As Integer)
            m_use_a_Ions = Math.Abs(Value)
        End Set
    End Property
    Public Property Use_b_Ions() As Integer
        Get
            Return Math.Abs(m_use_b_Ions)
        End Get
        Set(ByVal Value As Integer)
            m_use_b_Ions = Math.Abs(Value)
        End Set
    End Property
    Public Property Use_y_Ions() As Integer
        Get
            Return Math.Abs(m_use_y_Ions)
        End Get
        Set(ByVal Value As Integer)
            m_use_y_Ions = Math.Abs(Value)
        End Set
    End Property
    Public Property a_Ion_Weighting() As Single
        Get
            Return m_aWeight
        End Get
        Set(ByVal Value As Single)
            m_aWeight = Value
        End Set
    End Property
    Public Property b_Ion_Weighting() As Single
        Get
            Return m_bWeight
        End Get
        Set(ByVal Value As Single)
            m_bWeight = Value
        End Set
    End Property
    Public Property c_Ion_Weighting() As Single
        Get
            Return m_cWeight
        End Get
        Set(ByVal Value As Single)
            m_cWeight = Value
        End Set
    End Property
    Public Property d_Ion_Weighting() As Single
        Get
            Return m_dWeight
        End Get
        Set(ByVal Value As Single)
            m_dWeight = Value
        End Set
    End Property
    Public Property v_Ion_Weighting() As Single
        Get
            Return m_vWeight
        End Get
        Set(ByVal Value As Single)
            m_vWeight = Value
        End Set
    End Property
    Public Property w_Ion_Weighting() As Single
        Get
            Return m_wWeight
        End Get
        Set(ByVal Value As Single)
            m_wWeight = Value
        End Set
    End Property
    Public Property x_Ion_Weighting() As Single
        Get
            Return m_xWeight
        End Get
        Set(ByVal Value As Single)
            m_xWeight = Value
        End Set
    End Property
    Public Property y_Ion_Weighting() As Single
        Get
            Return m_yWeight
        End Get
        Set(ByVal Value As Single)
            m_yWeight = Value
        End Set
    End Property
    Public Property z_Ion_Weighting() As Single
        Get
            Return m_zWeight
        End Get
        Set(ByVal Value As Single)
            m_zWeight = Value
        End Set
    End Property
#End Region

    Public Function ReturnIonString() As String
        Dim s As String
        s = AssembleIonString()
        Return s
    End Function

    Private Function AssembleIonString() As String
        Dim s As String

        s = Use_a_Ions.ToString & " " & Use_b_Ions.ToString & " " & Use_y_Ions.ToString & " "
        s = s & Format(a_Ion_Weighting, "0.0").ToString & " " & Format(b_Ion_Weighting, "0.0").ToString & " "
        s = s & Format(c_Ion_Weighting, "0.0").ToString & " " & Format(d_Ion_Weighting, "0.0").ToString & " "
        s = s & Format(v_Ion_Weighting, "0.0").ToString & " " & Format(w_Ion_Weighting, "0.0").ToString & " "
        s = s & Format(x_Ion_Weighting, "0.0").ToString & " "
        s = s & Format(y_Ion_Weighting, "0.0").ToString & " " & Format(z_Ion_Weighting, "0.0").ToString & " "

        Return s
    End Function

End Class



Public Class clsEnzymeDetails

    Private m_EnzymeString As String

    Private m_Number As Integer         'Enzyme ID Number
    Private m_Name As String            'Descriptive Name
    Private m_Offset As Integer         'Cut position --> 0 = N-terminal, 1 = C-Terminal
    Private m_CleavePoints As String    'Amino Acids at which to cleave
    Private m_NoCleavePoints As String  'Amino Acids to skip cleavage

    Public Sub New(ByVal EnzymeString As String)
        m_EnzymeString = EnzymeString
        Call ParseEnzymeString(m_EnzymeString)
    End Sub

    Public Sub New()

    End Sub

    Private Sub ParseEnzymeString(ByVal enzStr As String)
        Dim s() As String
        Dim counter As Integer
        Dim placeCounter As Integer = 0
        Dim currChar As String
        Dim prevChar As String
        Dim tmpString As String

        For counter = 1 To Len(enzStr) + 1
            currChar = Mid(enzStr, counter, 1)
            If (currChar = " " And prevChar <> " ") Or (counter = Len(enzStr) + 1) Then
                ReDim Preserve s(placeCounter)
                s(placeCounter) = tmpString
                placeCounter = placeCounter + 1
                tmpString = ""
            ElseIf currChar <> " " Then
                tmpString = tmpString + currChar
            End If

            prevChar = currChar
        Next


        m_Number = CInt(s(0))
        m_Name = s(1)
        m_Offset = CInt(s(2))
        m_CleavePoints = s(3)
        m_NoCleavePoints = s(4)

    End Sub

    Public Property EnzymeID() As Integer
        Get
            Return m_Number
        End Get
        Set(ByVal Value As Integer)
            m_Number = Value
        End Set
    End Property
    Public Property EnzymeName() As String
        Get
            Return m_Name
        End Get
        Set(ByVal Value As String)
            m_Name = Value
        End Set
    End Property
    Public Property EnzymeCleaveOffset() As Integer
        Get
            Return m_Offset
        End Get
        Set(ByVal Value As Integer)
            m_Offset = Value
        End Set
    End Property
    Public Property EnzymeCleavePoints() As String
        Get
            Return m_CleavePoints
        End Get
        Set(ByVal Value As String)
            m_CleavePoints = Value
        End Set
    End Property
    Public Property EnzymeNoCleavePoints() As String
        Get
            Return m_NoCleavePoints
        End Get
        Set(ByVal Value As String)
            m_NoCleavePoints = Value
        End Set
    End Property

    Public Function ReturnEnzymeString() As String
        Dim s As String

        s = EnzymeID.ToString & "."
        s = s.PadRight(4, System.Convert.ToChar(" ")) & EnzymeName
        s = s.PadRight(30, System.Convert.ToChar(" ")) & EnzymeCleaveOffset.ToString
        s = s.PadRight(35, System.Convert.ToChar(" ")) & EnzymeCleavePoints
        s = s.PadRight(48, System.Convert.ToChar(" ")) & EnzymeNoCleavePoints

        Return s

    End Function

    Public Function ReturnBW32EnzymeInfoString(ByVal cleavagePosition As Integer) As String
        Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder

        sb.Append(EnzymeName)
        sb.Append("(")
        sb.Append(Me.EnzymeCleavePoints)
        If Me.EnzymeNoCleavePoints.Length > 0 And Me.EnzymeNoCleavePoints <> "-" Then
            sb.Append("/")
            sb.Append(Me.EnzymeNoCleavePoints)
        End If
        sb.Append(")")
        sb.Append(" ")
        sb.Append(cleavagePosition.ToString)
        sb.Append(" ")
        sb.Append(Me.EnzymeCleaveOffset.ToString)
        sb.Append(" ")
        sb.Append(Me.EnzymeCleavePoints)
        sb.Append(" ")
        sb.Append(Me.EnzymeNoCleavePoints)

        Return sb.ToString
    End Function

End Class

Public Class clsEnzymeCollection
    Inherits CollectionBase

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub add(ByVal Enzyme As clsEnzymeDetails)
        Me.List.Add(Enzyme)
    End Sub

    Default Public Property Item(ByVal index As Integer) As clsEnzymeDetails
        Get
            Return DirectCast(Me.List(index), clsEnzymeDetails)
        End Get
        Set(ByVal Value As clsEnzymeDetails)
            Me.List(index) = Value
        End Set
    End Property
End Class

Public Class clsGetEnzymeBlock

    Private m_templateFilePath As String
    Private m_sectionName As String
    Private m_EnzymeBlockCollection As System.Collections.Specialized.StringCollection
    Private m_EnzymeCollection As clsEnzymeCollection

    Public Property EnzymeList() As clsEnzymeCollection
        Get
            Return m_EnzymeCollection
        End Get
        Set(ByVal Value As clsEnzymeCollection)
            m_EnzymeCollection = Value
        End Set
    End Property

    Public Sub New( _
        ByVal TemplateFilePath As String, _
        ByVal SectionName As String)

        m_templateFilePath = TemplateFilePath
        m_sectionName = SectionName

        m_EnzymeBlockCollection = GetEnzymeBlock()
        m_EnzymeCollection = InterpretEnzymeBlockCollection(m_EnzymeBlockCollection)

    End Sub

    Private Function GetEnzymeBlock() As System.Collections.Specialized.StringCollection

        Dim sc As New System.Collections.Specialized.StringCollection

        Dim fi As FileInfo
        Dim tr As TextReader
        Dim s As String

        fi = New FileInfo(m_templateFilePath)
        tr = fi.OpenText
        s = tr.ReadLine
        'Find the correct section block)
        Do While Not s Is Nothing
            If s = "[" & m_sectionName & "]" Then
                s = tr.ReadLine
                Do While Not s Is Nothing
                    sc.Add(s)
                    s = tr.ReadLine
                Loop
                Exit Do
            End If
            s = tr.ReadLine
        Loop

        If sc.Count = 0 Then
            sc = LoadDefaultEnzymes()
        End If

        Return sc

    End Function

    Private Function LoadDefaultEnzymes() As System.Collections.Specialized.StringCollection
        Dim sc As New System.Collections.Specialized.StringCollection

        sc.Add("0.  No_Enzyme              0      -           -")
        sc.Add("1.  Trypsin                1      KR          -")
        sc.Add("2.  Trypsin_modified       1      KRLNH       -")
        sc.Add("3.  Chymotrypsin           1      FWYL        -")
        sc.Add("4.  Chymotrypsin__modified 1      FWY         -")
        sc.Add("5.  Clostripain            1      R           -")
        sc.Add("6.  Cyanogen_Bromide       1      M           -")
        sc.Add("7.  IodosoBenzoate         1      W           -")
        sc.Add("8.  Proline_Endopept       1      P           -")
        sc.Add("9.  Staph_Protease         1      E           -")
        sc.Add("10. Trypsin_K              1      K           P")
        sc.Add("11. Trypsin_R              1      R           P")
        sc.Add("12. GluC                   1      ED          -")
        sc.Add("13. LysC                   1      K           -")
        sc.Add("14. AspN                   0      D           -")
        sc.Add("15. Elastase               1      ALIV        P")
        sc.Add("16. Elastase/Tryp/Chymo    1      ALIVKRWFY   P")

        Return sc

    End Function

    Private Function InterpretEnzymeBlockCollection( _
    ByVal enzymeBlock As System.Collections.Specialized.StringCollection) As clsEnzymeCollection

        Dim tempEnzyme As clsEnzymeDetails
        Dim tempStorage As clsEnzymeCollection = New clsEnzymeCollection
        Dim s As String
        Dim sTmp As String
        Dim counter As Integer

        For Each s In enzymeBlock
            sTmp = s.Substring(0, InStr(s, " "))
            If InStr(sTmp, ". ") > 0 Then
                tempEnzyme = New clsEnzymeDetails(s)
                tempStorage.add(tempEnzyme)
            End If
        Next

        Return tempStorage

    End Function


End Class

