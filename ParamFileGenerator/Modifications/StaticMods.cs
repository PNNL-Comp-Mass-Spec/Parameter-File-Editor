using System;
using System.Linq;
using Microsoft.VisualBasic;

namespace ParamFileGenerator
{

    public class StaticMods : Mods
    {

        public double CtermPeptide
        {
            get
            {
                return FindAAMod(ResidueCode.C_Term_Peptide).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.C_Term_Peptide, value);
            }
        }

        public double CtermProtein
        {
            get
            {
                return FindAAMod(ResidueCode.C_Term_Protein).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.C_Term_Protein, value);
            }
        }
        public double NtermPeptide
        {
            get
            {
                return FindAAMod(ResidueCode.N_Term_Peptide).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.N_Term_Peptide, value);
            }
        }

        public double NtermProtein
        {
            get
            {
                return FindAAMod(ResidueCode.N_Term_Protein).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.N_Term_Protein, value);
            }
        }

        public double G_Glycine
        {
            get
            {
                return FindAAMod(ResidueCode.G_Glycine).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.G_Glycine, value);
            }
        }

        public double A_Alanine
        {
            get
            {
                return FindAAMod(ResidueCode.A_Alanine).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.A_Alanine, value);
            }
        }

        public double S_Serine
        {
            get
            {
                return FindAAMod(ResidueCode.S_Serine).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.S_Serine, value);
            }
        }

        public double P_Proline
        {
            get
            {
                return FindAAMod(ResidueCode.P_Proline).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.P_Proline, value);
            }
        }

        public double V_Valine
        {
            get
            {
                return FindAAMod(ResidueCode.V_Valine).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.V_Valine, value);
            }
        }

        public double T_Threonine
        {
            get
            {
                return FindAAMod(ResidueCode.T_Threonine).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.T_Threonine, value);
            }
        }
        public double C_Cysteine
        {
            get
            {
                return FindAAMod(ResidueCode.C_Cysteine).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.C_Cysteine, value);
            }
        }

        public double L_Leucine
        {
            get
            {
                return FindAAMod(ResidueCode.L_Leucine).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.L_Leucine, value);
            }
        }

        public double I_Isoleucine
        {
            get
            {
                return FindAAMod(ResidueCode.I_Isoleucine).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.I_Isoleucine, value);
            }
        }

        public double X_LorI
        {
            get
            {
                return FindAAMod(ResidueCode.X_LorI).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.X_LorI, value);
            }
        }

        public double N_Asparagine
        {
            get
            {
                return FindAAMod(ResidueCode.N_Asparagine).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.N_Asparagine, value);
            }
        }

        public double O_Ornithine
        {
            get
            {
                return FindAAMod(ResidueCode.O_Ornithine).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.O_Ornithine, value);
            }
        }

        public double B_avg_NandD
        {
            get
            {
                return FindAAMod(ResidueCode.B_avg_NandD).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.B_avg_NandD, value);
            }
        }

        public double D_Aspartic_Acid
        {
            get
            {
                return FindAAMod(ResidueCode.D_Aspartic_Acid).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.D_Aspartic_Acid, value);
            }
        }

        public double Q_Glutamine
        {
            get
            {
                return FindAAMod(ResidueCode.Q_Glutamine).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.Q_Glutamine, value);
            }
        }

        public double K_Lysine
        {
            get
            {
                return FindAAMod(ResidueCode.K_Lysine).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.K_Lysine, value);
            }
        }

        public double Z_avg_QandE
        {
            get
            {
                return FindAAMod(ResidueCode.Z_avg_QandE).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.Z_avg_QandE, value);
            }
        }

        public double E_Glutamic_Acid
        {
            get
            {
                return FindAAMod(ResidueCode.E_Glutamic_Acid).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.E_Glutamic_Acid, value);
            }
        }

        public double M_Methionine
        {
            get
            {
                return FindAAMod(ResidueCode.M_Methionine).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.M_Methionine, value);
            }
        }

        public double H_Histidine
        {
            get
            {
                return FindAAMod(ResidueCode.H_Histidine).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.H_Histidine, value);
            }
        }

        public double F_Phenylalanine
        {
            get
            {
                return FindAAMod(ResidueCode.F_Phenylalanine).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.F_Phenylalanine, value);
            }
        }

        public double R_Arginine
        {
            get
            {
                return FindAAMod(ResidueCode.R_Arginine).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.R_Arginine, value);
            }
        }

        public double Y_Tyrosine
        {
            get
            {
                return FindAAMod(ResidueCode.Y_Tyrosine).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.Y_Tyrosine, value);
            }
        }

        public double W_Tryptophan
        {
            get
            {
                return FindAAMod(ResidueCode.W_Tryptophan).MassDifference;
            }
            set
            {
                ChangeAAMod(ResidueCode.W_Tryptophan, value);
            }
        }

        public StaticMods() : base()
        {
        }

        public string GetResidue(int index)
        {
            ModEntry m = (ModEntry)List[index];
            return m.ReturnResidueAffected(0);
        }
        public void ChangeAAModification(ResidueCode ModifiedAA, double MassDifference, bool Additive = false)
        {
            ChangeAAMod(ModifiedAA, MassDifference, Additive);
        }
        public void EradicateEmptyMods()
        {
            KillBlankMods();
        }

        private ModEntry FindAAMod(ResidueCode ModifiedAA)
        {
            return m_FindMod(ConvertResidueCodeToSLC(ModifiedAA));
        }

        private void ChangeAAMod(ResidueCode ModifiedAA, double MassDifference, bool Additive = false)
        {
            var foundMod = FindAAMod(ModifiedAA);
            string ModAAString = ConvertResidueCodeToSLC(ModifiedAA);
            m_ChangeMod(foundMod, ModAAString, MassDifference, Additive);

        }

        private void KillBlankMods()
        {
            string AASLC;
            var AAEnums = Enum.GetNames(typeof(ResidueCode)).ToList();
            int currIndex;
            ModEntry modEntry;

            foreach (var AA in AAEnums)
            {
                if (Strings.InStr(AA, "Term") == 0)
                {
                    AASLC = Strings.Left(AA, 1);
                    currIndex = m_FindModIndex(AASLC);
                    if (currIndex != -1)
                    {
                        modEntry = GetModEntry(currIndex);
                        if (Math.Abs(modEntry.MassDifference) < float.Epsilon)
                        {
                            List.Remove(modEntry);
                        }
                    }
                }
            }
        }

    }
}
