﻿using System.Collections.Generic;
using System.Text;

namespace ParamFileGenerator.Modifications
{
    public class ModEntry
    {
        // Ignore Spelling: Diff

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
            var aminoAcids = new StringBuilder();

            foreach (var s in resCollection)
            {
                var s1 = s.Substring(0, 1);
                aminoAcids.Append(s1);
            }

            return aminoAcids.ToString();
        }

        private string GetModTypeSymbol()
        {
            return ModificationType switch
            {
                ModificationTypes.Dynamic => "D",
                ModificationTypes.Static => "S",
                ModificationTypes.Isotopic => "I",
                ModificationTypes.TermPep => "T",
                ModificationTypes.TermProt => "P",
                _ => null
            };
        }

        // ReSharper disable once ConvertToPrimaryConstructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="affectedResidueList"></param>
        /// <param name="massDiff"></param>
        /// <param name="modType"></param>
        /// <param name="modID"></param>
        public ModEntry(List<string> affectedResidueList, double massDiff, ModificationTypes modType, int modID = 0)
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

        public override string ToString()
        {
            return string.Format("{0} mod on {1}, {2} Da", ModificationType, string.Concat(ResidueCollection), MassDifference);
        }
    }
}
