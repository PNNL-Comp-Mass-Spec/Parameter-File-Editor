Imports System.IO
Imports System.Linq

Public Class Params
    Implements IBasicParams
    Implements IAdvancedParams

    ' Ignore Spelling: Xcalibur

    Public Enum ParamFileTypes
        BioWorks_20 = 0 'Normal BioWorks 2.0 SEQUEST
        BioWorks_30 = 1 'BioWorks 3.0+ TurboSEQUEST
        BioWorks_31 = 2 'BioWorks 3.1 ClusterQuest
        BioWorks_32 = 3 'BioWorks 3.2 ClusterF***
    End Enum

    Const DEF_DB_NAME As String = "C:\Xcalibur\database\nr.fasta"     'Not really used, just a placeholder

    Const DEF_ENZ_SECTION_NAME As String = "SEQUEST_ENZYME_INFO"

    'Basic Parameters
    Private m_type As ParamFileTypes

    'Advanced Parameters
    Private m_ionSeriesString As String
    Private m_protMassFilterString As String

    Private m_fullTemplate As RetrieveParams
    Private m_templateFilePath As String

    Public Shared ReadOnly Property BaseLineParamSet As Params
        Get
            Return MainProcess.BaseLineParamSet
        End Get
    End Property

    Public ReadOnly Property FileType As ParamFileTypes Implements IBasicParams.FileType
        Get
            Return m_type
        End Get
    End Property

    Public Property DMS_ID As Integer Implements IBasicParams.DMS_ID

    Public Property FileName As String Implements IBasicParams.FileName

    Public Property FileTypeIndex As Integer

    Public Property Description As String Implements IBasicParams.Description

    Public Property DefaultFASTAPath As String Implements IAdvancedParams.DefaultFASTAPath

    Public Property DefaultFASTAPath2 As String Implements IAdvancedParams.DefaultFASTAPath2

    Public Property NumberOfResultsToProcess As Integer Implements IAdvancedParams.NumberOfResultsToProcess

    Public Property PeptideMassTolerance As Single Implements IAdvancedParams.PeptideMassTolerance

    Public Property CreateOutputFiles As Boolean Implements IAdvancedParams.CreateOutputFiles

    Public Property IonSeries As IonSeries Implements IAdvancedParams.IonSeries

    Public Property DynamicMods As DynamicMods Implements IBasicParams.DynamicMods

    Public Property TermDynamicMods As TermDynamicMods Implements IBasicParams.TermDynamicMods

    Public Property IsotopicMods As IsoMods Implements IBasicParams.IsotopicModificationsList

    Public Property MaximumNumAAPerDynMod As Integer Implements IAdvancedParams.MaximumNumAAPerDynMod

    Public Property MaximumNumDifferentialPerPeptide As Integer Implements IAdvancedParams.MaximumDifferentialPerPeptide

    Public Property UsePhosphoFragmentation As Integer Implements IAdvancedParams.UsePhosphoFragmentation

    Public Property FragmentIonTolerance As Single Implements IAdvancedParams.FragmentIonTolerance

    Public Property NumberOfOutputLines As Integer Implements IAdvancedParams.NumberOfOutputLines

    Public Property NumberOfDescriptionLines As Integer Implements IAdvancedParams.NumberOfDescriptionLines

    Public Property ShowFragmentIons As Boolean Implements IAdvancedParams.ShowFragmentIons

    Public Property PrintDuplicateReferences As Boolean Implements IAdvancedParams.PrintDuplicateReferences

    Public Property SelectedEnzymeDetails As EnzymeDetails Implements IBasicParams.SelectedEnzymeDetails

    Public Property SelectedEnzymeIndex As Integer Implements IBasicParams.SelectedEnzymeIndex

    Public Property SelectedEnzymeCleavagePosition As Integer Implements IBasicParams.SelectedEnzymeCleavagePosition

    Public Property SelectedNucReadingFrame As IAdvancedParams.FrameList Implements IAdvancedParams.SelectedNucReadingFrame

    Public Property SelectedNucReadingFrameIndex As Integer Implements IAdvancedParams.SelectedNucReadingFrameIndex
        Get
            Return SelectedNucReadingFrame
        End Get
        Set
            SelectedNucReadingFrame = CType(Value, IAdvancedParams.FrameList)
        End Set
    End Property

    Public Property ParentMassType As IBasicParams.MassTypeList Implements IBasicParams.ParentMassType

    Public Property FragmentMassType As IBasicParams.MassTypeList Implements IBasicParams.FragmentMassType

    Public Property RemovePrecursorPeak As Boolean Implements IAdvancedParams.RemovePrecursorPeak

    Public Property IonCutoffPercentage As Single Implements IAdvancedParams.IonCutoffPercentage

    Public Property MaximumNumberMissedCleavages As Integer Implements IBasicParams.MaximumNumberMissedCleavages

    Public Property MinimumProteinMassToSearch As Single Implements IAdvancedParams.MinimumProteinMassToSearch

    Public Property MaximumProteinMassToSearch As Single Implements IAdvancedParams.MaximumProteinMassToSearch

    Public Property NumberOfDetectedPeaksToMatch As Integer Implements IAdvancedParams.NumberOfDetectedPeaksToMatch

    Public Property NumberOfAllowedDetectedPeakErrors As Integer Implements IAdvancedParams.NumberOfAllowedDetectedPeakErrors

    Public Property MatchedPeakMassTolerance As Single Implements IAdvancedParams.MatchedPeakMassTolerance

    Public Property AminoAcidsAllUpperCase As Boolean Implements IAdvancedParams.AminoAcidsAllUpperCase

    Public Property PartialSequenceToMatch As String Implements IBasicParams.PartialSequenceToMatch

    Public Property SequenceHeaderInfoToFilter As String Implements IAdvancedParams.SequenceHeaderInfoToFilter

    Public Property PeptideMassUnits As Integer Implements IAdvancedParams.PeptideMassUnits

    Public Property FragmentMassUnits As Integer Implements IAdvancedParams.FragmentMassUnits

    Public Property StaticModificationsList As StaticMods Implements IBasicParams.StaticModificationsList

    Public Property EnzymeList As EnzymeCollection Implements IBasicParams.EnzymeList

    Public Property LoadedParamNames As Hashtable = New Hashtable

    Public Sub AddLoadedParamName(ParameterName As String, ParameterValue As String)
        If LoadedParamNames Is Nothing Then
            LoadedParamNames = New Hashtable
        End If
        If Not LoadedParamNames.ContainsKey(ParameterName) Then
            LoadedParamNames.Add(ParameterName, ParameterValue)
        End If
    End Sub

    Public Function RetrieveEnzymeDetails(EnzymeListIndex As Integer) As EnzymeDetails Implements IBasicParams.RetrieveEnzymeDetails
        Return EnzymeList.Item(EnzymeListIndex)
    End Function

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()
        IonSeries = New IonSeries()
        EnzymeList = New EnzymeCollection()
        SelectedEnzymeDetails = New EnzymeDetails()
        DynamicMods = New DynamicMods()
        StaticModificationsList = New StaticMods()
        IsotopicMods = New IsoMods()
        TermDynamicMods = New TermDynamicMods("0.0 0.0")

        MaximumNumDifferentialPerPeptide = 3
        UsePhosphoFragmentation = 0
        PeptideMassUnits = IAdvancedParams.MassUnitList.amu
    End Sub

    Public Function ReturnMassFilter(MinimumMassToFilter As Single, MaximumMassToFilter As Single) As String
        Return ReturnMassFilterString(MinimumMassToFilter, MaximumMassToFilter)
    End Function

    Public Sub LoadTemplate(templateFileName As String)
        LoadTemplateParams(templateFileName)
    End Sub

    Private Sub LoadTemplateParams(templateFileName As String)

        m_templateFilePath = GetFilePath(templateFileName)
        Dim m_getEnzymeList As New GetEnzymeBlock(m_templateFilePath, DEF_ENZ_SECTION_NAME)
        EnzymeList = m_getEnzymeList.EnzymeList

        m_type = GetTemplateType()

        Dim SectionName = "SEQUEST"

        m_fullTemplate = New RetrieveParams(m_templateFilePath)
        'Retrieve Basic Parameters
        With m_fullTemplate
            .SetSection(SectionName)
            FileName = Path.GetFileName(m_templateFilePath)
            SelectedEnzymeIndex = CInt(.GetParam("enzyme_number"))
            SelectedEnzymeDetails = EnzymeList(SelectedEnzymeIndex)
            SelectedEnzymeCleavagePosition = 1
            MaximumNumberMissedCleavages = CInt(.GetParam("max_num_internal_cleavage_sites"))
            ParentMassType = CType(CInt(.GetParam("mass_type_parent")), IBasicParams.MassTypeList)
            FragmentMassType = CType(CInt(.GetParam("mass_type_fragment")), IBasicParams.MassTypeList)
            PartialSequenceToMatch = .GetParam("partial_sequence")
            DynamicMods = New DynamicMods(.GetParam("diff_search_options"))
            TermDynamicMods = New TermDynamicMods(.GetParam("term_diff_search_options"))
            StaticModificationsList = New StaticMods


            'Get Static Mods
            StaticModificationsList.CtermPeptide = CSng(.GetParam("add_Cterm_peptide"))
            StaticModificationsList.CtermProtein = CSng(.GetParam("add_Cterm_protein"))
            StaticModificationsList.NtermPeptide = CSng(.GetParam("add_Nterm_peptide"))
            StaticModificationsList.NtermProtein = CSng(.GetParam("add_Nterm_protein"))
            StaticModificationsList.G_Glycine = CSng(.GetParam("add_G_Glycine"))
            StaticModificationsList.A_Alanine = CSng(.GetParam("add_A_Alanine"))
            StaticModificationsList.S_Serine = CSng(.GetParam("add_S_Serine"))
            StaticModificationsList.P_Proline = CSng(.GetParam("add_P_Proline"))
            StaticModificationsList.V_Valine = CSng(.GetParam("add_V_Valine"))
            StaticModificationsList.T_Threonine = CSng(.GetParam("add_T_Threonine"))
            StaticModificationsList.C_Cysteine = CSng(.GetParam("add_C_Cysteine"))
            StaticModificationsList.L_Leucine = CSng(.GetParam("add_L_Leucine"))
            StaticModificationsList.I_Isoleucine = CSng(.GetParam("add_I_Isoleucine"))
            StaticModificationsList.X_LorI = CSng(.GetParam("add_X_LorI"))
            StaticModificationsList.N_Asparagine = CSng(.GetParam("add_N_Asparagine"))
            StaticModificationsList.O_Ornithine = CSng(.GetParam("add_O_Ornithine"))
            StaticModificationsList.B_avg_NandD = CSng(.GetParam("add_B_avg_NandD"))
            StaticModificationsList.D_Aspartic_Acid = CSng(.GetParam("add_D_Aspartic_Acid"))
            StaticModificationsList.Q_Glutamine = CSng(.GetParam("add_Q_Glutamine"))
            StaticModificationsList.K_Lysine = CSng(.GetParam("add_K_Lysine"))
            StaticModificationsList.Z_avg_QandE = CSng(.GetParam("add_Z_avg_QandE"))
            StaticModificationsList.E_Glutamic_Acid = CSng(.GetParam("add_E_Glutamic_Acid"))
            StaticModificationsList.M_Methionine = CSng(.GetParam("add_M_Methionine"))
            StaticModificationsList.H_Histidine = CSng(.GetParam("add_H_Histidine"))
            StaticModificationsList.F_Phenylalanine = CSng(.GetParam("add_F_Phenylalanine"))
            StaticModificationsList.R_Arginine = CSng(.GetParam("add_R_Arginine"))
            StaticModificationsList.Y_Tyrosine = CSng(.GetParam("add_Y_Tyrosine"))
            StaticModificationsList.W_Tryptophan = CSng(.GetParam("add_W_Tryptophan"))
        End With

        'add code to check for existence of isotopic mods



        'Retrieve Advanced Parameters
        With m_fullTemplate
            .SetSection(SectionName)
            If m_type = ParamFileTypes.BioWorks_20 Then
                DefaultFASTAPath = .GetParam("database_name")
                DefaultFASTAPath2 = ""
                NumberOfResultsToProcess = 500
            ElseIf m_type = ParamFileTypes.BioWorks_30 Then
                DefaultFASTAPath = .GetParam("first_database_name")
                DefaultFASTAPath2 = .GetParam("second_database_name")
                NumberOfResultsToProcess = CInt(.GetParam("num_results"))
            End If
            PeptideMassTolerance = CSng(.GetParam("peptide_mass_tolerance"))
            If .GetParam("create_output_files") IsNot Nothing Then
                CreateOutputFiles = CBool(.GetParam("create_output_files"))
            Else
                CreateOutputFiles = True
            End If
            m_ionSeriesString = .GetParam("ion_series")
            IonSeries = New IonSeries(m_ionSeriesString)
            MaximumNumAAPerDynMod = CInt(.GetParam("max_num_differential_AA_per_mod"))
            If m_type = ParamFileTypes.BioWorks_32 Then
                MaximumNumDifferentialPerPeptide = CInt(.GetParam("max_num_differential_per_peptide"))
            End If
            FragmentIonTolerance = CSng(.GetParam("fragment_ion_tolerance"))
            NumberOfOutputLines = CInt(.GetParam("num_output_lines"))
            NumberOfDescriptionLines = CInt(.GetParam("num_description_lines"))
            ShowFragmentIons = CBool(.GetParam("show_fragment_ions"))
            PrintDuplicateReferences = CBool(.GetParam("print_duplicate_references"))
            SelectedNucReadingFrame = CType(CInt(.GetParam("enzyme_number")), IAdvancedParams.FrameList)
            RemovePrecursorPeak = CBool(.GetParam("remove_precursor_peak"))
            IonCutoffPercentage = CSng(.GetParam("ion_cutoff_percentage"))
            m_protMassFilterString = .GetParam("protein_mass_filter")

            Dim protMassFilterList = m_protMassFilterString.Split(" "c)
            If (protMassFilterList.Count > 0) Then
                MinimumProteinMassToSearch = CSng(protMassFilterList(0))
            End If

            If (protMassFilterList.Count > 1) Then
                MaximumProteinMassToSearch = CSng(protMassFilterList(1))
            End If

            NumberOfDetectedPeaksToMatch = CInt(.GetParam("match_peak_count"))
            NumberOfAllowedDetectedPeakErrors = CInt(.GetParam("match_peak_allowed_error"))
            MatchedPeakMassTolerance = CSng(.GetParam("match_peak_tolerance"))
            AminoAcidsAllUpperCase = True
            SequenceHeaderInfoToFilter = .GetParam("sequence_header_filter")
            DMS_ID = -1
        End With

    End Sub

    <Obsolete("Unused")>
    Private Function GetDescription() As String

        Dim s As String
        Dim desc As String

        Dim fi As FileInfo
        Dim tr As TextReader

        fi = New FileInfo(m_templateFilePath)
        tr = fi.OpenText
        s = tr.ReadLine
        'Find the correct section block)
        Do While s IsNot Nothing
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

        Do While s IsNot Nothing
            If InStr(s.ToLower, "num_results = ") > 0 Then
                type = ParamFileTypes.BioWorks_31
                Return type
            Else
                type = ParamFileTypes.BioWorks_20
                Return type
            End If
        Loop

    End Function

    Private Function ReturnMassFilterString(
        minMass As Single,
        maxMass As Single) As String

        Return Format(minMass.ToString, "0") & " " & Format(maxMass.ToString, "0")

    End Function

    Private Function GetFilePath(templateFileName As String) As String
        Return templateFileName
    End Function

End Class
