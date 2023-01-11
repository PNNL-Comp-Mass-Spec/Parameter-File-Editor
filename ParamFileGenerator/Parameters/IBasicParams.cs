namespace ParamFileGenerator
{
    public interface IBasicParams
    {
        public enum MassTypeList : int
        {
            Average = 0,
            Monoisotopic = 1
        }

        Params.ParamFileTypes FileType { get; }
        int DMS_ID { get; set; }
        string FileName { get; set; }
        string Description { get; set; }
        EnzymeDetails SelectedEnzymeDetails { get; set; }
        int SelectedEnzymeIndex { get; set; }
        int SelectedEnzymeCleavagePosition { get; set; }
        int MaximumNumberMissedCleavages { get; set; }
        MassTypeList ParentMassType { get; set; }
        MassTypeList FragmentMassType { get; set; }
        DynamicMods DynamicMods { get; set; }
        TermDynamicMods TermDynamicMods { get; set; }
        StaticMods StaticModificationsList { get; set; }
        IsoMods IsotopicModificationsList { get; set; }
        string PartialSequenceToMatch { get; set; }
        EnzymeCollection EnzymeList { get; set; }
        EnzymeDetails RetrieveEnzymeDetails(int index);
    }
}
