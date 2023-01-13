using System;
using System.Collections.Generic;
using System.Linq;

namespace ParamFileGenerator
{
    public class Mods : List<ModEntry>
    {
        public enum ResidueCode
        {
            C_Term_Protein,
            C_Term_Peptide,
            N_Term_Protein,
            N_Term_Peptide,
            G_Glycine,
            A_Alanine,
            S_Serine,
            P_Proline,
            V_Valine,
            T_Threonine,
            C_Cysteine,
            L_Leucine,
            I_Isoleucine,
            X_LorI,
            N_Asparagine,
            O_Ornithine,
            B_avg_NandD,
            D_Aspartic_Acid,
            Q_Glutamine,
            K_Lysine,
            Z_avg_QandE,
            E_Glutamic_Acid,
            M_Methionine,
            H_Histidine,
            F_Phenylalanine,
            R_Arginine,
            Y_Tyrosine,
            W_Tryptophan
        }

        public enum IsotopeList
        {
            C,
            H,
            O,
            N,
            S
        }

        public int ModCount => Count;

        public bool Initialized
        {
            get
            {
                if (Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public ModEntry GetModEntry(int index)
        {
            return base[index];
        }

        public int NumMods => Count;

        public Mods() : base()
        {
            LoadAAMappingColl();
        }

        public virtual void Add(
            ResidueCode AffectedResidue,
            double MassDifference,
            int GlobalModID = 0)
        {
            m_Add(ConvertResidueCodeToSLC(AffectedResidue), MassDifference, ModEntry.ModificationTypes.Static, GlobalModID);
        }

        public void Add(
            string AffectedResidueString,
            double MassDifference)
        {
            m_Add(AffectedResidueString, MassDifference, ModEntry.ModificationTypes.Static);
        }

        public void Replace(int index, ModEntry newMod)
        {
            this[index] = newMod;
        }

        public string GetMassDiff(int index)
        {
            ModEntry m = this[index];
            return m.MassDifference.ToString("0.00000");
        }

        /// <summary>
        /// Keys are residue names from ResidueCode (e.g. P_Proline)
        /// Values are the single letter abbreviation if an amino acid
        /// Or, if not an amino acid, one of: C_Term_Protein, C_Term_Peptide, N_Term_Protein, or N_Term_Peptide
        /// </summary>
        protected Dictionary<string, string> m_AAMappingTable;
        protected void m_Add(
            string AffectedEntity,
            double MassDifference,
            ModEntry.ModificationTypes ModType,
            int GlobalModID = 0)
        {
            var residueList = ConvertAffectedResStringToList(AffectedEntity);
            var newMod = new ModEntry(residueList, MassDifference, ModType, GlobalModID);
            Add(newMod);
        }

        protected void LoadAAMappingColl()
        {
            var AAEnums = Enum.GetNames(typeof(ResidueCode)).ToList();

            m_AAMappingTable = new Dictionary<string, string>();

            foreach (var AA in AAEnums)
            {
                if (AA == "C_Term_Protein" || AA == "C_Term_Peptide" || AA == "N_Term_Protein" || AA == "N_Term_Peptide")
                {
                    m_AAMappingTable.Add(AA, AA);
                }
                else
                {
                    m_AAMappingTable.Add(AA, AA.Substring(0, 1));
                }
            }
        }

        protected List<string> ConvertAffectedResStringToList(string affectedResidueString)
        {
            var aaList = new List<string>();

            if (affectedResidueString == "C_Term_Protein" ||
                affectedResidueString == "C_Term_Peptide" ||
                affectedResidueString == "N_Term_Protein" ||
                affectedResidueString == "N_Term_Peptide")
            {
                aaList.Add(affectedResidueString);
            }
            else
            {
                for (int counter = 0; counter < affectedResidueString.Length; counter++)
                {
                    string tmpAA = affectedResidueString.Substring(counter, 1);
                    //if ("><[]".Contains(tmpAA))
                    //{
                    aaList.Add(tmpAA);
                    //}
                }
            }

            return aaList;
        }

        protected string ConvertResidueCodeToSLC(ResidueCode Residue)
        {
            string tmpRes = Residue.ToString();

            string tmpSLC = null;
            if (m_AAMappingTable.TryGetValue(tmpRes, out tmpSLC))
            {
                return tmpSLC;
            }

            return string.Empty;
        }

        protected ResidueCode ConvertSLCToResidueCode(string SingleLetterAA)
        {
            foreach (var item in m_AAMappingTable)
            {
                string ResString = item.Key;
                if ((SingleLetterAA ?? "") == (ResString.Substring(0, 1) ?? "") && !ResString.Contains("Term"))
                {
                    return (ResidueCode)Enum.Parse(typeof(ResidueCode), ResString);
                }
            }

            return default;
        }

        protected int m_FindModIndex(string modifiedEntity)
        {
            foreach (ModEntry statMod in this)
            {
                string testCase = statMod.ReturnResidueAffected(0);
                if ((testCase ?? "") == (modifiedEntity ?? ""))
                {
                    return IndexOf(statMod);
                }
            }

            return -1;
        }

        protected ModEntry m_FindMod(string ModifiedEntity)
        {
            ModEntry ModEntry;
            int ModIndex = m_FindModIndex(ModifiedEntity);
            if (ModIndex == -1)
            {
                ModEntry = null;
            }
            else
            {
                ModEntry = this[ModIndex];
            }

            if (ModEntry is null)
            {
                var sc = new List<string>()
                {
                    ModifiedEntity
                };

                var emptyMod = new ModEntry(sc, 0.0d, ModEntry.ModificationTypes.Dynamic);
                return emptyMod;
            }
            else
            {
                return ModEntry;
            }
        }

        protected void m_ChangeMod(
            ModEntry foundMod,
            string ModifiedEntity,
            double MassDifference,
            bool Additive = false)
        {
            if (Math.Abs(foundMod.MassDifference) < float.Epsilon && Math.Abs(MassDifference) > float.Epsilon)
            {
                m_Add(ModifiedEntity, MassDifference, foundMod.ModificationType);
                return;
            }
            else if (Math.Abs(foundMod.MassDifference) < float.Epsilon && Math.Abs(MassDifference) < float.Epsilon)
            {
                return;
            }
            else if (Math.Abs(foundMod.MassDifference) > float.Epsilon)          // Not an emptyMod
            {
                var counter = default(int);

                var residueList = ConvertAffectedResStringToList(ModifiedEntity);
                ModEntry changeMod;

                if (Additive)
                {
                    changeMod = new ModEntry(residueList, MassDifference + foundMod.MassDifference, foundMod.ModificationType);
                }
                else
                {
                    changeMod = new ModEntry(residueList, MassDifference, foundMod.ModificationType);
                }

                foreach (ModEntry tempMod in this)
                {
                    if (foundMod.Equals(tempMod) && Math.Abs(MassDifference) > float.Epsilon)
                    {
                        Replace(counter, changeMod);
                        return;
                    }
                    else if (foundMod.Equals(tempMod) && Math.Abs(MassDifference) < float.Epsilon)
                    {
                        RemoveAt(counter);
                    }
                    if (Count == 0)
                        break;
                    counter += 1;
                }
            }
        }
    }
}
