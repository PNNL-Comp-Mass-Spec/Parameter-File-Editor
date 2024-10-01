namespace ParamFileGenerator.Parameters
{
    public interface IAdvancedParams
    {
        public enum FrameList
        {
            Protein = 0,
            Forward_1 = 1,
            Forward_2 = 2,
            Forward_3 = 3,
            Reverse_1 = 4,
            Reverse_2 = 5,
            Reverse_3 = 6,
            All_3_Forward = 7,
            All_3_Reverse = 8,
            All_Six = 9
        }

        public enum MassUnitList
        {
            amu = 0,
            mmu = 1,
            ppm = 2
        }

        string DefaultFASTAPath { get; set; }
        string DefaultFASTAPath2 { get; set; }
        bool CreateOutputFiles { get; set; }
        IonSeries IonSeries { get; set; }
        int NumberOfResultsToProcess { get; set; }
        int MaximumNumAAPerDynamicMod { get; set; }
        int MaximumNumDifferentialPerPeptide { get; set; }
        float PeptideMassTolerance { get; set; }
        float FragmentIonTolerance { get; set; }
        int NumberOfOutputLines { get; set; }
        int NumberOfDescriptionLines { get; set; }
        bool ShowFragmentIons { get; set; }
        bool PrintDuplicateReferences { get; set; }
        int SelectedNucleotideReadingFrameIndex { get; set; }
        FrameList SelectedNucleotideReadingFrame { get; set; }
        bool RemovePrecursorPeak { get; set; }
        float IonCutoffPercentage { get; set; }
        float MinimumProteinMassToSearch { get; set; }
        float MaximumProteinMassToSearch { get; set; }
        int NumberOfDetectedPeaksToMatch { get; set; }
        int NumberOfAllowedDetectedPeakErrors { get; set; }
        float MatchedPeakMassTolerance { get; set; }
        bool AminoAcidsAllUpperCase { get; set; }
        string SequenceHeaderInfoToFilter { get; set; }
        int UsePhosphoFragmentation { get; set; }
        int PeptideMassUnits { get; set; }
        int FragmentMassUnits { get; set; }
    }
}
