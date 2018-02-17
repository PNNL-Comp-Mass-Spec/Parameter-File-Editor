Imports System.Collections.Specialized
Imports System.IO
Imports System.Text

Public Interface IBasicParams
    Enum MassTypeList As Integer
        Average = 0
        Monoisotopic = 1
    End Enum

    ReadOnly Property FileType As clsParams.ParamFileTypes
    Property DMS_ID As Integer
    Property FileName As String
    Property Description As String
    Property SelectedEnzymeDetails As clsEnzymeDetails
    Property SelectedEnzymeIndex As Integer
    Property SelectedEnzymeCleavagePosition As Integer
    Property MaximumNumberMissedCleavages As Integer
    Property ParentMassType As MassTypeList
    Property FragmentMassType As MassTypeList
    Property DynamicMods As clsDynamicMods
    Property TermDynamicMods As clsTermDynamicMods
    Property StaticModificationsList As clsStaticMods
    Property IsotopicModificationsList As clsIsoMods
    Property PartialSequenceToMatch As String
    Property EnzymeList As clsEnzymeCollection
    Function RetrieveEnzymeDetails(index As Integer) As clsEnzymeDetails
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
    Enum MassUnitList As Integer
        amu = 0
        mmu = 1
        ppm = 2
    End Enum
    Property DefaultFASTAPath As String
    Property DefaultFASTAPath2 As String
    Property CreateOutputFiles As Boolean
    Property IonSeries As clsIonSeries
    Property NumberOfResultsToProcess As Integer
    Property MaximumNumAAPerDynMod As Integer
    Property MaximumDifferentialPerPeptide As Integer
    Property PeptideMassTolerance As Single
    Property FragmentIonTolerance As Single
    Property NumberOfOutputLines As Integer
    Property NumberOfDescriptionLines As Integer
    Property ShowFragmentIons As Boolean
    Property PrintDuplicateReferences As Boolean
    Property SelectedNucReadingFrameIndex As Integer
    Property SelectedNucReadingFrame As FrameList
    Property RemovePrecursorPeak As Boolean
    Property IonCutoffPercentage As Single
    Property MinimumProteinMassToSearch As Single
    Property MaximumProteinMassToSearch As Single
    Property NumberOfDetectedPeaksToMatch As Integer
    Property NumberOfAllowedDetectedPeakErrors As Integer
    Property MatchedPeakMassTolerance As Single
    Property AminoAcidsAllUpperCase As Boolean
    Property SequenceHeaderInfoToFilter As String
    Property UsePhosphoFragmentation As Integer
    Property PeptideMassUnits As Integer
    Property FragmentMassUnits As Integer
End Interface

Public Class clsParams
    Implements IBasicParams
    Implements IAdvancedParams

    Public Enum ParamFileTypes
        BioWorks_20 = 0 'Normal BioWorks 2.0 Sequest
        BioWorks_30 = 1 'BioWorks 3.0+ TurboSequest
        BioWorks_31 = 2 'BioWorks 3.1 ClusterQuest
        BioWorks_32 = 3 'BioWorks 3.2 ClusterF***
    End Enum

    Const DEF_DB_NAME As String = "C:\Xcalibur\database\nr.fasta"     'Not really used, just a placeholder

#Region " Private Variables "
    Const DEF_ENZ_SECTION_NAME As String = "SEQUEST_ENZYME_INFO"

    'Basic Parameters
    Private m_type As ParamFileTypes
    ' Obsolete: Private m_parentMassUnits As Integer
    ' Peptide terminal dynamic mods

    'Advanced Parameters
    ' Obsolete: Private m_Path As String
    Private m_ionSeriesString As String
    Private m_protMassFilterString As String
    ' Obsolete: Private m_fragmentMassUnits As Integer = IAdvancedParams.MassUnitList.amu

    Private m_fullTemplate As clsRetrieveParams
    Private m_templateFilePath As String
    'Shared s_BaseLineParamSet As clsParams

#End Region

#Region " clsParams Properties "
    Public Shared ReadOnly Property BaseLineParamSet As clsParams
        Get
            Return clsMainProcess.BaseLineParamSet
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

    Public Property IonSeries As clsIonSeries Implements IAdvancedParams.IonSeries

    Public Property DynamicMods As clsDynamicMods Implements IBasicParams.DynamicMods

    Public Property TermDynamicMods As clsTermDynamicMods Implements IBasicParams.TermDynamicMods

    Public Property IsotopicMods As clsIsoMods Implements IBasicParams.IsotopicModificationsList

    Public Property MaximumNumAAPerDynMod As Integer Implements IAdvancedParams.MaximumNumAAPerDynMod

    Public Property MaximumNumDifferentialPerPeptide As Integer Implements IAdvancedParams.MaximumDifferentialPerPeptide

    Public Property UsePhosphoFragmentation As Integer Implements IAdvancedParams.UsePhosphoFragmentation

    Public Property FragmentIonTolerance As Single Implements IAdvancedParams.FragmentIonTolerance

    Public Property NumberOfOutputLines As Integer Implements IAdvancedParams.NumberOfOutputLines

    Public Property NumberOfDescriptionLines As Integer Implements IAdvancedParams.NumberOfDescriptionLines

    Public Property ShowFragmentIons As Boolean Implements IAdvancedParams.ShowFragmentIons

    Public Property PrintDuplicateReferences As Boolean Implements IAdvancedParams.PrintDuplicateReferences

    Public Property SelectedEnzymeDetails As clsEnzymeDetails Implements IBasicParams.SelectedEnzymeDetails

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

    Public Property StaticModificationsList As clsStaticMods Implements IBasicParams.StaticModificationsList

    Public Property EnzymeList As clsEnzymeCollection Implements IBasicParams.EnzymeList

    Public Property LoadedParamNames As Hashtable = New Hashtable

    Public Sub AddLoadedParamName(ParameterName As String, ParameterValue As String)
        If LoadedParamNames Is Nothing Then
            LoadedParamNames = New Hashtable
        End If
        If Not LoadedParamNames.ContainsKey(ParameterName) Then
            LoadedParamNames.Add(ParameterName, ParameterValue)
        End If
    End Sub

    Public Function RetrieveEnzymeDetails(EnzymeListIndex As Integer) As clsEnzymeDetails Implements IBasicParams.RetrieveEnzymeDetails
        Return EnzymeList.Item(EnzymeListIndex)
    End Function
#End Region

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()
        IonSeries = New clsIonSeries()
        EnzymeList = New clsEnzymeCollection()
        SelectedEnzymeDetails = New clsEnzymeDetails()
        DynamicMods = New clsDynamicMods()
        StaticModificationsList = New clsStaticMods()
        IsotopicMods = New clsIsoMods()
        TermDynamicMods = New clsTermDynamicMods("0.0 0.0")

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

    Private Sub LoadTemplateParams(TemplateFileName As String)

        Dim tmpProtMassFilterStringArray() As String
        Dim SectionName As String
        m_templateFilePath = GetFilePath(TemplateFileName)
        Dim m_getEnzymeList As New clsGetEnzymeBlock(m_templateFilePath, DEF_ENZ_SECTION_NAME)
        EnzymeList = m_getEnzymeList.EnzymeList
        'Dim m_Compare As New clsParamsFromDMS

        'm_desc = GetDescription()
        m_type = GetTemplateType()

        'If m_type = ParamFileTypes.BioWorks_20 Then
        SectionName = "SEQUEST"
        'ElseIf m_type = ParamFileTypes.BioWorks_30 Or m_type = ParamFileTypes.BioWorks_31 Then
        '    SectionName = "SEQUEST"
        'End If

        m_fullTemplate = New clsRetrieveParams(m_templateFilePath)
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
            DynamicMods = New clsDynamicMods(.GetParam("diff_search_options"))
            TermDynamicMods = New clsTermDynamicMods(.GetParam("term_diff_search_options"))
            StaticModificationsList = New clsStaticMods


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
            If Not .GetParam("create_output_files") Is Nothing Then
                CreateOutputFiles = CBool(.GetParam("create_output_files"))
            Else
                CreateOutputFiles = True
            End If
            m_ionSeriesString = .GetParam("ion_series")
            IonSeries = New clsIonSeries(m_ionSeriesString)
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
            tmpProtMassFilterStringArray = InterpretMassFilterString(m_protMassFilterString)
            MinimumProteinMassToSearch = CSng(tmpProtMassFilterStringArray(0))
            MaximumProteinMassToSearch = CSng(tmpProtMassFilterStringArray(1))
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

    Private Function InterpretMassFilterString(massFilterString As String) As String()
        Dim s() As String = Nothing
        Dim placeCounter = 0
        Dim prevChar = ""
        Dim tmpString = ""

        For counter = 1 To Len(massFilterString)
            Dim currChar = Mid(massFilterString, counter, 1)
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

    Private Function ReturnMassFilterString(
        minMass As Single,
        maxMass As Single) As String

        Return Format(minMass.ToString, "0") & " " & Format(maxMass.ToString, "0")

    End Function

    Private Function GetFilePath(templateFileName As String) As String
        Return templateFileName
    End Function


End Class

Public Class clsIonSeries
    Public m_origIonSeriesString As String

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

    Public Sub New(IonSeriesString As String)
        m_origIonSeriesString = IonSeriesString
        Call ParseISS(m_origIonSeriesString)
    End Sub

    Public Sub New()
        m_origIonSeriesString = Nothing
        Initialized = True
    End Sub

    Public Sub RevertToOriginalIonString()
        If m_origIonSeriesString <> "" Then
            Call ParseISS(m_origIonSeriesString)
        End If
    End Sub

    Private Sub ParseISS(ionString As String)
        Dim tmpSplit() As String
        tmpSplit = ionString.Split(Convert.ToChar(" "))
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

    Public Property Initialized As Boolean

    Public Property Use_a_Ions As Integer
        Get
            Return Math.Abs(m_use_a_Ions)
        End Get
        Set(value As Integer)
            m_use_a_Ions = Math.Abs(value)
        End Set
    End Property
    Public Property Use_b_Ions As Integer
        Get
            Return Math.Abs(m_use_b_Ions)
        End Get
        Set
            m_use_b_Ions = Math.Abs(Value)
        End Set
    End Property
    Public Property Use_y_Ions As Integer
        Get
            Return Math.Abs(m_use_y_Ions)
        End Get
        Set
            m_use_y_Ions = Math.Abs(Value)
        End Set
    End Property
    Public Property a_Ion_Weighting As Single
        Get
            Return m_aWeight
        End Get
        Set
            m_aWeight = Value
        End Set
    End Property
    Public Property b_Ion_Weighting As Single
        Get
            Return m_bWeight
        End Get
        Set
            m_bWeight = Value
        End Set
    End Property
    Public Property c_Ion_Weighting As Single
        Get
            Return m_cWeight
        End Get
        Set
            m_cWeight = Value
        End Set
    End Property
    Public Property d_Ion_Weighting As Single
        Get
            Return m_dWeight
        End Get
        Set
            m_dWeight = Value
        End Set
    End Property
    Public Property v_Ion_Weighting As Single
        Get
            Return m_vWeight
        End Get
        Set
            m_vWeight = Value
        End Set
    End Property
    Public Property w_Ion_Weighting As Single
        Get
            Return m_wWeight
        End Get
        Set
            m_wWeight = Value
        End Set
    End Property
    Public Property x_Ion_Weighting As Single
        Get
            Return m_xWeight
        End Get
        Set
            m_xWeight = Value
        End Set
    End Property
    Public Property y_Ion_Weighting As Single
        Get
            Return m_yWeight
        End Get
        Set
            m_yWeight = Value
        End Set
    End Property
    Public Property z_Ion_Weighting As Single
        Get
            Return m_zWeight
        End Get
        Set
            m_zWeight = Value
        End Set
    End Property
#End Region

    Public Function ReturnIonString() As String
        Dim s As String = AssembleIonString()
        Return s
    End Function

    Private Function AssembleIonString() As String
        Dim s = Use_a_Ions.ToString & " " & Use_b_Ions.ToString & " " & Use_y_Ions.ToString & " " &
                Format(a_Ion_Weighting, "0.0").ToString & " " & Format(b_Ion_Weighting, "0.0").ToString & " " &
                Format(c_Ion_Weighting, "0.0").ToString & " " & Format(d_Ion_Weighting, "0.0").ToString & " " &
                Format(v_Ion_Weighting, "0.0").ToString & " " & Format(w_Ion_Weighting, "0.0").ToString & " " &
                Format(x_Ion_Weighting, "0.0").ToString & " " &
                Format(y_Ion_Weighting, "0.0").ToString & " " & Format(z_Ion_Weighting, "0.0").ToString & " "

        Return s
    End Function

End Class


Public Class clsEnzymeDetails

    Private ReadOnly m_EnzymeString As String

    Private m_Number As Integer         'Enzyme ID Number
    Private m_Name As String            'Descriptive Name
    Private m_Offset As Integer         'Cut position --> 0 = N-terminal, 1 = C-Terminal
    Private m_CleavePoints As String    'Amino Acids at which to cleave
    Private m_NoCleavePoints As String  'Amino Acids to skip cleavage

    Public Sub New(EnzymeString As String)
        m_EnzymeString = EnzymeString
        Call ParseEnzymeString(m_EnzymeString)
    End Sub

    Public Sub New()

    End Sub

    Private Sub ParseEnzymeString(enzStr As String)
        Dim s() As String = Nothing
        Dim placeCounter = 0
        Dim prevChar = ""
        Dim tmpString = ""

        For counter = 1 To Len(enzStr) + 1
            Dim currChar = Mid(enzStr, counter, 1)
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

    Public Property EnzymeID As Integer
        Get
            Return m_Number
        End Get
        Set
            m_Number = Value
        End Set
    End Property
    Public Property EnzymeName As String
        Get
            Return m_Name
        End Get
        Set
            m_Name = Value
        End Set
    End Property
    Public Property EnzymeCleaveOffset As Integer
        Get
            Return m_Offset
        End Get
        Set
            m_Offset = Value
        End Set
    End Property
    Public Property EnzymeCleavePoints As String
        Get
            Return m_CleavePoints
        End Get
        Set
            m_CleavePoints = Value
        End Set
    End Property
    Public Property EnzymeNoCleavePoints As String
        Get
            Return m_NoCleavePoints
        End Get
        Set
            m_NoCleavePoints = Value
        End Set
    End Property

    Public Function ReturnEnzymeString() As String
        Dim s As String

        s = EnzymeID.ToString & "."
        s = s.PadRight(4, Convert.ToChar(" ")) & EnzymeName
        s = s.PadRight(30, Convert.ToChar(" ")) & EnzymeCleaveOffset.ToString
        s = s.PadRight(35, Convert.ToChar(" ")) & EnzymeCleavePoints
        s = s.PadRight(48, Convert.ToChar(" ")) & EnzymeNoCleavePoints

        Return s

    End Function

    Public Function ReturnBW32EnzymeInfoString(cleavagePosition As Integer) As String
        Dim sb = New StringBuilder

        sb.Append(EnzymeName)
        sb.Append("(")
        sb.Append(EnzymeCleavePoints)
        If EnzymeNoCleavePoints.Length > 0 And EnzymeNoCleavePoints <> "-" Then
            sb.Append("/")
            sb.Append(EnzymeNoCleavePoints)
        End If
        sb.Append(")")
        sb.Append(" ")
        sb.Append(cleavagePosition.ToString)
        sb.Append(" ")
        sb.Append(EnzymeCleaveOffset.ToString)
        sb.Append(" ")
        sb.Append(EnzymeCleavePoints)
        sb.Append(" ")
        sb.Append(EnzymeNoCleavePoints)

        Return sb.ToString
    End Function

End Class

Public Class clsEnzymeCollection
    Inherits CollectionBase

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub add(Enzyme As clsEnzymeDetails)
        List.Add(Enzyme)
    End Sub

    Default Public Property Item(index As Integer) As clsEnzymeDetails
        Get
            Return DirectCast(List(index), clsEnzymeDetails)
        End Get
        Set
            List(index) = Value
        End Set
    End Property
End Class

Public Class clsGetEnzymeBlock

    Private ReadOnly m_templateFilePath As String
    Private ReadOnly m_sectionName As String
    Private ReadOnly m_EnzymeBlockCollection As StringCollection

    Public Property EnzymeList As clsEnzymeCollection

    Public Sub New(
        TemplateFilePath As String,
        SectionName As String)

        m_templateFilePath = TemplateFilePath
        m_sectionName = SectionName

        m_EnzymeBlockCollection = GetEnzymeBlock()
        EnzymeList = InterpretEnzymeBlockCollection(m_EnzymeBlockCollection)

    End Sub

    Private Function GetEnzymeBlock() As StringCollection

        Dim sc As New StringCollection

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

    Private Function LoadDefaultEnzymes() As StringCollection
        Dim sc As New StringCollection

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
        sc.Add("17. ArgC                   1      R-          P")
        sc.Add("18. Do_not_cleave          1      B           -")
        sc.Add("19. LysN                   0      K           -")

        Return sc

    End Function

    Private Function InterpretEnzymeBlockCollection(
    enzymeBlock As StringCollection) As clsEnzymeCollection

        Dim tempEnzyme As clsEnzymeDetails
        Dim tempStorage = New clsEnzymeCollection()
        Dim s As String
        Dim sTmp As String
        'Dim counter As Integer

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

