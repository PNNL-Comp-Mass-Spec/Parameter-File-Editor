using System.Collections.Generic;

namespace ParamFileGenerator
{
    public class ModEntry
    {
        public enum ModificationTypes
        {
            Dynamic,
            Static,
            Isotopic,
            TermProt,
            TermPep
        }

        public int TotalNumResiduesAffected => ResidueCollection.Count;

        public string ReturnResidueAffected(int residueSCIndex)
        {
            return ResidueCollection[residueSCIndex];
        }

        public List<string> ReturnAllAffectedResidues => ResidueCollection;

        public List<string> ResidueCollection { get; set; }

        public string ReturnAllAffectedResiduesString => ConvertListToAAString(ResidueCollection);

        public double MassDifference { get; set; }

        public int GlobalModID { get; set; }

        public string ModificationTypeString => GetModTypeSymbol();

        public ModificationTypes ModificationType { get; }

        private void AddResidue(string newResidue)
        {
            ResidueCollection.Add(newResidue);
        }

        private void RemoveResidue(string badResidue)
        {
            ResidueCollection.Remove(badResidue);
        }

        private string ConvertListToAAString(IEnumerable<string> resCollection)
        {
            var returnString = "";
            foreach (var s in resCollection)
            {
                var s1 = s.Substring(0, 1);
                returnString += s1;
            }
            return returnString;
        }

        private string GetModTypeSymbol()
        {
            switch (ModificationType)
            {
                case ModificationTypes.Dynamic:
                    return "D";
                case ModificationTypes.Static:
                    return "S";
                case ModificationTypes.Isotopic:
                    return "I";
                case ModificationTypes.TermPep:
                    return "T";
                case ModificationTypes.TermProt:
                    return "P";
                default:
                    return null;
            }
        }

        public ModEntry(
            List<string> affectedResidueList,
            double massDiff,
            ModificationTypes modType,
            int modID = 0)
        {
            ModificationType = modType;
            ResidueCollection = affectedResidueList;
            MassDifference = massDiff;
            GlobalModID = modID;
        }

        public void AddAffectedResidue(string residueToAdd)
        {
            AddResidue(residueToAdd);
        }

        public void RemoveAffectedResidue(string residueToRemove)
        {
            RemoveResidue(residueToRemove);
        }
    }
}
