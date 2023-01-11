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
    Property IonSeries As IonSeries
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
