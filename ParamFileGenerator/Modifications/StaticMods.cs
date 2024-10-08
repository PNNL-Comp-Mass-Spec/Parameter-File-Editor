﻿using System;
using System.Linq;

namespace ParamFileGenerator.Modifications
{
    public class StaticMods : Mods
    {
        // Ignore Spelling: CTerm, NTerm, Lor, Nand, Qand
        // Ignore Spelling: Asparagine, Aspartic, Cysteine, Glutamic, Glutamine, Isoleucine, Leucine, Lysine, Ornithine, Proline, Serine, Threonine, Tryptophan, Valine

        public double CtermPeptide
        {
            get => FindAAMod(ResidueCode.C_Term_Peptide).MassDifference;
            set => ChangeAAMod(ResidueCode.C_Term_Peptide, value);
        }

        public double CtermProtein
        {
            get => FindAAMod(ResidueCode.C_Term_Protein).MassDifference;
            set => ChangeAAMod(ResidueCode.C_Term_Protein, value);
        }

        public double NtermPeptide
        {
            get => FindAAMod(ResidueCode.N_Term_Peptide).MassDifference;
            set => ChangeAAMod(ResidueCode.N_Term_Peptide, value);
        }

        public double NtermProtein
        {
            get => FindAAMod(ResidueCode.N_Term_Protein).MassDifference;
            set => ChangeAAMod(ResidueCode.N_Term_Protein, value);
        }

        // ReSharper disable InconsistentNaming

        public double G_Glycine
        {
            get => FindAAMod(ResidueCode.G_Glycine).MassDifference;
            set => ChangeAAMod(ResidueCode.G_Glycine, value);
        }

        public double A_Alanine
        {
            get => FindAAMod(ResidueCode.A_Alanine).MassDifference;
            set => ChangeAAMod(ResidueCode.A_Alanine, value);
        }

        public double S_Serine
        {
            get => FindAAMod(ResidueCode.S_Serine).MassDifference;
            set => ChangeAAMod(ResidueCode.S_Serine, value);
        }

        public double P_Proline
        {
            get => FindAAMod(ResidueCode.P_Proline).MassDifference;
            set => ChangeAAMod(ResidueCode.P_Proline, value);
        }

        public double V_Valine
        {
            get => FindAAMod(ResidueCode.V_Valine).MassDifference;
            set => ChangeAAMod(ResidueCode.V_Valine, value);
        }

        public double T_Threonine
        {
            get => FindAAMod(ResidueCode.T_Threonine).MassDifference;
            set => ChangeAAMod(ResidueCode.T_Threonine, value);
        }

        public double C_Cysteine
        {
            get => FindAAMod(ResidueCode.C_Cysteine).MassDifference;
            set => ChangeAAMod(ResidueCode.C_Cysteine, value);
        }

        public double L_Leucine
        {
            get => FindAAMod(ResidueCode.L_Leucine).MassDifference;
            set => ChangeAAMod(ResidueCode.L_Leucine, value);
        }

        public double I_Isoleucine
        {
            get => FindAAMod(ResidueCode.I_Isoleucine).MassDifference;
            set => ChangeAAMod(ResidueCode.I_Isoleucine, value);
        }

        public double X_LorI
        {
            get => FindAAMod(ResidueCode.X_LorI).MassDifference;
            set => ChangeAAMod(ResidueCode.X_LorI, value);
        }

        public double N_Asparagine
        {
            get => FindAAMod(ResidueCode.N_Asparagine).MassDifference;
            set => ChangeAAMod(ResidueCode.N_Asparagine, value);
        }

        public double O_Ornithine
        {
            get => FindAAMod(ResidueCode.O_Ornithine).MassDifference;
            set => ChangeAAMod(ResidueCode.O_Ornithine, value);
        }

        public double B_avg_NandD
        {
            get => FindAAMod(ResidueCode.B_avg_NandD).MassDifference;
            set => ChangeAAMod(ResidueCode.B_avg_NandD, value);
        }

        public double D_Aspartic_Acid
        {
            get => FindAAMod(ResidueCode.D_Aspartic_Acid).MassDifference;
            set => ChangeAAMod(ResidueCode.D_Aspartic_Acid, value);
        }

        public double Q_Glutamine
        {
            get => FindAAMod(ResidueCode.Q_Glutamine).MassDifference;
            set => ChangeAAMod(ResidueCode.Q_Glutamine, value);
        }

        public double K_Lysine
        {
            get => FindAAMod(ResidueCode.K_Lysine).MassDifference;
            set => ChangeAAMod(ResidueCode.K_Lysine, value);
        }

        public double Z_avg_QandE
        {
            get => FindAAMod(ResidueCode.Z_avg_QandE).MassDifference;
            set => ChangeAAMod(ResidueCode.Z_avg_QandE, value);
        }

        public double E_Glutamic_Acid
        {
            get => FindAAMod(ResidueCode.E_Glutamic_Acid).MassDifference;
            set => ChangeAAMod(ResidueCode.E_Glutamic_Acid, value);
        }

        public double M_Methionine
        {
            get => FindAAMod(ResidueCode.M_Methionine).MassDifference;
            set => ChangeAAMod(ResidueCode.M_Methionine, value);
        }

        public double H_Histidine
        {
            get => FindAAMod(ResidueCode.H_Histidine).MassDifference;
            set => ChangeAAMod(ResidueCode.H_Histidine, value);
        }

        public double F_Phenylalanine
        {
            get => FindAAMod(ResidueCode.F_Phenylalanine).MassDifference;
            set => ChangeAAMod(ResidueCode.F_Phenylalanine, value);
        }

        public double R_Arginine
        {
            get => FindAAMod(ResidueCode.R_Arginine).MassDifference;
            set => ChangeAAMod(ResidueCode.R_Arginine, value);
        }

        public double Y_Tyrosine
        {
            get => FindAAMod(ResidueCode.Y_Tyrosine).MassDifference;
            set => ChangeAAMod(ResidueCode.Y_Tyrosine, value);
        }

        public double W_Tryptophan
        {
            get => FindAAMod(ResidueCode.W_Tryptophan).MassDifference;
            set => ChangeAAMod(ResidueCode.W_Tryptophan, value);
        }

        // ReSharper restore InconsistentNaming

        public string GetResidue(int index)
        {
            var m = this[index];
            return m.ReturnResidueAffected(0);
        }

        public void ChangeAAModification(ResidueCode modifiedAA, double massDifference, bool additive = false)
        {
            ChangeAAMod(modifiedAA, massDifference, additive);
        }

        /// <summary>
        /// Remove empty mods
        /// </summary>
        /// <remarks>Used by the GUI editor</remarks>
        // ReSharper disable once UnusedMember.Global
        public void EradicateEmptyMods()
        {
            KillBlankMods();
        }

        private ModEntry FindAAMod(ResidueCode modifiedAA)
        {
            return FindMod(ConvertResidueCodeToSingleLetterSymbol(modifiedAA));
        }

        private void ChangeAAMod(ResidueCode modifiedAA, double massDifference, bool additive = false)
        {
            var foundMod = FindAAMod(modifiedAA);
            var modAAString = ConvertResidueCodeToSingleLetterSymbol(modifiedAA);
            ChangeMod(foundMod, modAAString, massDifference, additive);
        }

        private void KillBlankMods()
        {
            foreach (var aminoAcid in Enum.GetNames(typeof(ResidueCode)).ToList())
            {
                if (!aminoAcid.Contains("Term"))
                    continue;

                // C or N terminal mod
                // The first letter of the enum name should be N or C
                var terminus = aminoAcid.Substring(0, 1);

                var currentIndex = FindModIndex(terminus);

                if (currentIndex < 0)
                    continue;

                var modEntry = GetModEntry(currentIndex);

                if (Math.Abs(modEntry.MassDifference) < float.Epsilon)
                {
                    Remove(modEntry);
                }
            }
        }
    }
}
