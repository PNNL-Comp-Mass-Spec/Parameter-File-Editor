
namespace ParamFileGenerator
{
    public class IsoMods : Mods
    {

        public void Add(IsotopeList AffectedAtom, double MassDifference, int GlobalModID = 0)
        {
            m_Add(AffectedAtom.ToString(), MassDifference, ModEntry.ModificationTypes.Isotopic, GlobalModID);
        }

        public string GetAtom(int index)
        {
            ModEntry m = (ModEntry)List[index];
            return m.ReturnResidueAffected(0);
        }

        public double Iso_C
        {
            get
            {
                return FindIsoMod(IsotopeList.C).MassDifference;
            }
            set
            {
                ChangeIsoMod(IsotopeList.C, value);
            }
        }

        public double Iso_H
        {
            get
            {
                return FindIsoMod(IsotopeList.H).MassDifference;
            }
            set
            {
                ChangeIsoMod(IsotopeList.H, value);
            }
        }

        public double Iso_O
        {
            get
            {
                return FindIsoMod(IsotopeList.O).MassDifference;
            }
            set
            {
                ChangeIsoMod(IsotopeList.O, value);
            }
        }

        public double Iso_N
        {
            get
            {
                return FindIsoMod(IsotopeList.N).MassDifference;
            }
            set
            {
                ChangeIsoMod(IsotopeList.N, value);
            }
        }

        public double Iso_S
        {
            get
            {
                return FindIsoMod(IsotopeList.S).MassDifference;
            }
            set
            {
                ChangeIsoMod(IsotopeList.S, value);
            }
        }

        private void ChangeIsoMod(IsotopeList AffectedAtom, double MassDifference)
        {
            var foundMod = FindIsoMod(AffectedAtom);
            string ModAAString = AffectedAtom.ToString();
            m_ChangeMod(foundMod, ModAAString, MassDifference);
        }

        private ModEntry FindIsoMod(IsotopeList AffectedAtom)
        {
            return m_FindMod(AffectedAtom.ToString());
        }

    }
}
