namespace ParamFileGenerator
{
    public class IsoMods : Mods
    {
        public void Add(IsotopeList affectedAtom, double massDifference, int globalModId = 0)
        {
            m_Add(affectedAtom.ToString(), massDifference, ModEntry.ModificationTypes.Isotopic, globalModId);
        }

        public string GetAtom(int index)
        {
            var m = this[index];
            return m.ReturnResidueAffected(0);
        }

        public double Iso_C
        {
            get => FindIsoMod(IsotopeList.C).MassDifference;
            set => ChangeIsoMod(IsotopeList.C, value);
        }

        public double Iso_H
        {
            get => FindIsoMod(IsotopeList.H).MassDifference;
            set => ChangeIsoMod(IsotopeList.H, value);
        }

        public double Iso_O
        {
            get => FindIsoMod(IsotopeList.O).MassDifference;
            set => ChangeIsoMod(IsotopeList.O, value);
        }

        public double Iso_N
        {
            get => FindIsoMod(IsotopeList.N).MassDifference;
            set => ChangeIsoMod(IsotopeList.N, value);
        }

        public double Iso_S
        {
            get => FindIsoMod(IsotopeList.S).MassDifference;
            set => ChangeIsoMod(IsotopeList.S, value);
        }

        private void ChangeIsoMod(IsotopeList affectedAtom, double massDifference)
        {
            var foundMod = FindIsoMod(affectedAtom);
            var modAAString = affectedAtom.ToString();
            m_ChangeMod(foundMod, modAAString, massDifference);
        }

        private ModEntry FindIsoMod(IsotopeList affectedAtom)
        {
            return m_FindMod(affectedAtom.ToString());
        }
    }
}
