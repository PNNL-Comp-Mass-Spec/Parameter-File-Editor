using System;
using System.Collections.Generic;
using ParamFileGenerator.Modifications;
using ParamFileGenerator.Parameters;
using PRISMDatabaseUtils;

namespace ParamFileGenerator
{
    public interface IReconstituteIsoMods
    {
        Params ReconstituteIsoMods(Params paramsClass);
    }

    public class ReconstituteIsoMods : IReconstituteIsoMods
    {
        public enum AvailableAtoms
        {
            N,
            C,
            H,
            O,
            S
        }

        /// <summary>
        /// Dictionary where keys are amino acid residue (one letter abbreviation)
        /// and values are a dictionary with atom counts (number of C, H, N, O, and S atoms)
        /// </summary>
        private readonly Dictionary<char, Dictionary<char, int>> mResidueAtomCounts;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbTools"></param>
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public ReconstituteIsoMods(IDBTools dbTools)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            var getResTable = new GetResiduesList(dbTools);
            mResidueAtomCounts = getResTable.ResidueAtomCounts;
        }

        internal Params ReconIsoMods(Params paramsClass)
        {
            return StreamlineIsoModsToStatics(paramsClass, paramsClass.IsotopicModificationsList);
        }

        Params IReconstituteIsoMods.ReconstituteIsoMods(Params paramsClass) => ReconIsoMods(paramsClass);

        protected int GetMultiplier(char aa, AvailableAtoms atom)
        {
            if (!mResidueAtomCounts.TryGetValue(aa, out var atomCounts))
                return 0;

            var atomSymbol = atom.ToString()[0];

            if (atomCounts.TryGetValue(atomSymbol, out var atomCount))
            {
                return atomCount;
            }

            return 0;
        }

        protected Params StreamlineIsoModsToStatics(Params paramsClass, IsoMods isoMods)
        {
            var AAEnums = Enum.GetNames(typeof(Mods.ResidueCode));

            foreach (var isotopicMod in isoMods)
            {
                var atom = isotopicMod.ReturnResidueAffected(0);
                var massDifference = isotopicMod.MassDifference;

                foreach (var aminoAcid in AAEnums)
                {
                    if (!aminoAcid.StartsWith("Term"))
                    {
                        var atomCount = GetMultiplier(aminoAcid[0], (AvailableAtoms)Enum.Parse(typeof(AvailableAtoms), atom));

                        paramsClass.StaticModificationsList.ChangeAAModification(
                            (Mods.ResidueCode)Enum.Parse(typeof(Mods.ResidueCode), aminoAcid),
                            massDifference * atomCount, true);
                    }
                }
            }

            return paramsClass;
        }
    }
}
