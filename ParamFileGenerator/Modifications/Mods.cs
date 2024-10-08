﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ParamFileGenerator.Modifications
{
    public class Mods : List<ModEntry>
    {
        // Ignore Spelling: Diff

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

        public ModEntry GetModEntry(int index)
        {
            return base[index];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Mods()
        {
            LoadAAMappingColl();
        }

        // ReSharper disable once UnusedMember.Global

        public virtual void Add(ResidueCode affectedResidue, double massDifference, int globalModID = 0)
        {
            Add(ConvertResidueCodeToSingleLetterSymbol(affectedResidue), massDifference, ModEntry.ModificationTypes.Static, globalModID);
        }

        public void Add(string affectedResidueString, double massDifference)
        {
            Add(affectedResidueString, massDifference, ModEntry.ModificationTypes.Static);
        }

        public void Replace(int index, ModEntry newMod)
        {
            this[index] = newMod;
        }

        public string GetMassDiff(int index)
        {
            var m = this[index];
            return m.MassDifference.ToString("0.00000");
        }

        /// <summary>
        /// Keys are residue names from ResidueCode (e.g. P_Proline)
        /// Values are the single letter abbreviation if an amino acid
        /// Or, if not an amino acid, one of: C_Term_Protein, C_Term_Peptide, N_Term_Protein, or N_Term_Peptide
        /// </summary>
        protected Dictionary<string, string> mAAMappingTable;

        protected void Add(string affectedEntity, double massDifference, ModEntry.ModificationTypes modType, int globalModID = 0)
        {
            var residueList = ConvertAffectedResStringToList(affectedEntity);
            var newMod = new ModEntry(residueList, massDifference, modType, globalModID);
            Add(newMod);
        }

        protected void LoadAAMappingColl()
        {
            var aaEnums = Enum.GetNames(typeof(ResidueCode)).ToList();

            mAAMappingTable = new Dictionary<string, string>();

            foreach (var aa in aaEnums)
            {
                if (aa is "C_Term_Protein" or "C_Term_Peptide" or "N_Term_Protein" or "N_Term_Peptide")
                {
                    mAAMappingTable.Add(aa, aa);
                }
                else
                {
                    mAAMappingTable.Add(aa, aa.Substring(0, 1));
                }
            }
        }

        protected List<string> ConvertAffectedResStringToList(string affectedResidueString)
        {
            var aaList = new List<string>();

            if (affectedResidueString is "C_Term_Protein" or "C_Term_Peptide" or "N_Term_Protein" or "N_Term_Peptide")
            {
                aaList.Add(affectedResidueString);
            }
            else
            {
                for (var counter = 0; counter < affectedResidueString.Length; counter++)
                {
                    var aminoAcid = affectedResidueString.Substring(counter, 1);

                    //if ("><[]".Contains(aminoAcid))
                    //{
                    aaList.Add(aminoAcid);
                    //}
                }
            }

            return aaList;
        }

        protected string ConvertResidueCodeToSingleLetterSymbol(ResidueCode residue)
        {
            if (mAAMappingTable.TryGetValue(residue.ToString(), out var singleLetterSymbol))
            {
                return singleLetterSymbol;
            }

            return string.Empty;
        }

        // ReSharper disable once UnusedMember.Global

        protected ResidueCode ConvertSingleLetterSymbolToResidueCode(string singleLetterAA)
        {
            foreach (var item in mAAMappingTable)
            {
                var resString = item.Key;

                if ((singleLetterAA ?? "") == resString.Substring(0, 1) && !resString.Contains("Term"))
                {
                    return (ResidueCode)Enum.Parse(typeof(ResidueCode), resString);
                }
            }

            return default;
        }

        protected int FindModIndex(string modifiedEntity)
        {
            foreach (var statMod in this)
            {
                var testCase = statMod.ReturnResidueAffected(0);

                if ((testCase ?? "") == (modifiedEntity ?? ""))
                {
                    return IndexOf(statMod);
                }
            }

            return -1;
        }

        protected ModEntry FindMod(string modifiedEntity)
        {
            ModEntry modEntry;
            var modIndex = FindModIndex(modifiedEntity);

            if (modIndex == -1)
            {
                modEntry = null;
            }
            else
            {
                modEntry = this[modIndex];
            }

            if (modEntry is null)
            {
                var sc = new List<string>
                {
                    modifiedEntity
                };

                // Empty mod
                return new ModEntry(sc, 0.0d, ModEntry.ModificationTypes.Dynamic);
            }

            return modEntry;
        }

        protected void ChangeMod(ModEntry foundMod, string modifiedEntity, double massDifference, bool additive = false)
        {
            if (Math.Abs(foundMod.MassDifference) < float.Epsilon && Math.Abs(massDifference) > float.Epsilon)
            {
                Add(modifiedEntity, massDifference, foundMod.ModificationType);
                return;
            }

            if (Math.Abs(foundMod.MassDifference) < float.Epsilon && Math.Abs(massDifference) < float.Epsilon)
            {
                return;
            }

            if (!(Math.Abs(foundMod.MassDifference) > float.Epsilon))
                return;

            // Not an emptyMod
            var counter = default(int);

            var residueList = ConvertAffectedResStringToList(modifiedEntity);
            ModEntry changeMod;

            if (additive)
            {
                changeMod = new ModEntry(residueList, massDifference + foundMod.MassDifference, foundMod.ModificationType);
            }
            else
            {
                changeMod = new ModEntry(residueList, massDifference, foundMod.ModificationType);
            }

            foreach (var tempMod in this)
            {
                if (foundMod.Equals(tempMod) && Math.Abs(massDifference) > float.Epsilon)
                {
                    Replace(counter, changeMod);
                    return;
                }

                if (foundMod.Equals(tempMod) && Math.Abs(massDifference) < float.Epsilon)
                {
                    RemoveAt(counter);
                }

                if (Count == 0)
                    break;

                counter++;
            }
        }
    }
}
