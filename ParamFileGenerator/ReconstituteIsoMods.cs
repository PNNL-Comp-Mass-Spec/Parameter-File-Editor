using System;
using System.Collections.Generic;
using PRISMDatabaseUtils;

namespace ParamFileGenerator
{
    public interface IReconstituteIsoMods
    {
        Params ReconstituteIsoMods(Params ParamsClass);
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
        private readonly Dictionary<char, Dictionary<char, int>> m_ResidueAtomCounts;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbTools"></param>
#pragma warning disable CS3001 // Type of parameter is not CLS-compliant
        public ReconstituteIsoMods(IDBTools dbTools)
#pragma warning restore CS3001 // Type of parameter is not CLS-compliant
        {
            var getResTable = new GetResiduesList(dbTools);
            m_ResidueAtomCounts = getResTable.ResidueAtomCounts;
        }
        internal Params ReconIsoMods(Params ParamsClass)
        {
            return StreamlineIsoModsToStatics(ParamsClass, ParamsClass.IsotopicModificationsList);
        }

        Params IReconstituteIsoMods.ReconstituteIsoMods(Params ParamsClass) => ReconIsoMods(ParamsClass);

        protected int GetMultiplier(char AA, AvailableAtoms Atom)
        {
            Dictionary<char, int> atomCounts = null;
            if (m_ResidueAtomCounts.TryGetValue(AA, out atomCounts))
            {
                char atomSymbol = Atom.ToString()[0];

                int atomCount;
                if (atomCounts.TryGetValue(atomSymbol, out atomCount))
                {
                    return atomCount;
                }
            }

            return 0;
        }

        protected Params StreamlineIsoModsToStatics(
            Params ParamsClass,
            IsoMods IsoMods)
        {
            string tmpAtom;
            double tmpIsoMass;

            var AAEnums = Enum.GetNames(typeof(Mods.ResidueCode));

            foreach (ModEntry im in IsoMods)
            {
                tmpAtom = im.ReturnResidueAffected(0);
                tmpIsoMass = im.MassDifference;

                foreach (var tmpAA in AAEnums)
                {
                    if (!tmpAA.StartsWith("Term"))
                    {
                        char tmpAASLC = tmpAA[0];
                        int tmpAtomCount = GetMultiplier(tmpAASLC, (AvailableAtoms)Enum.Parse(typeof(AvailableAtoms), tmpAtom));
                        ParamsClass.StaticModificationsList.ChangeAAModification(
                            (Mods.ResidueCode)Enum.Parse(typeof(Mods.ResidueCode), tmpAA),
                            tmpIsoMass * tmpAtomCount, true);
                    }
                }
            }

            return ParamsClass;
        }
    }
}
