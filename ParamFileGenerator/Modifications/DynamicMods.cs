using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ParamFileGenerator.Modifications
{
    public class DynamicMods : Mods
    {
        // Ignore Spelling: Diff, Dyn

        //public double Dyn_Mod_1_MassDiff
        //{
        //    get => Dyn_Mod_n_MassDiff(1);
        //    set => Dyn_Mod_n_MassDiff(1, value);
        //}
        //public double Dyn_Mod_2_MassDiff
        //{
        //    get => Dyn_Mod_n_MassDiff(2);
        //    set => Dyn_Mod_n_MassDiff(2, value);
        //}
        //public double Dyn_Mod_3_MassDiff
        //{
        //    get => Dyn_Mod_n_MassDiff(3);
        //    set => Dyn_Mod_n_MassDiff(3, value);
        //}
        //
        //public string Dyn_Mod_1_AAList
        //{
        //    get => Dyn_Mod_n_AAList(1);
        //    set => Dyn_Mod_n_AAList(1, value);
        //}
        //public string Dyn_Mod_2_AAList
        //{
        //    get => Dyn_Mod_n_AAList(2);
        //    set => Dyn_Mod_n_AAList(2, value);
        //}
        //public string Dyn_Mod_3_AAList
        //{
        //    get => Dyn_Mod_n_AAList(3);
        //    set => Dyn_Mod_n_AAList(3, value);
        //}

        /// <summary>
        /// Constructor that accepts a dynamic mod string
        /// </summary>
        /// <param name="dynamicModString">Dynamic mod string</param>
        public DynamicMods(string dynamicModString)
        {
            ParseDynamicModString(dynamicModString);
        }

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        public DynamicMods()
        {
        }

        public string ReturnDynModString(int maxDynMods)
        {
            //if (Initialized)
            //{
            return AssembleModString(maxDynMods);
            //}
            //else
            //{
            //    return "";
            //}
        }

        // TODO replace with real function for term dyn mods
        // Just a placeholder for now
        //public string ReturnDynTermModString()
        //{
        //    return "0.0000 0.0000";
        //}

        public new void Add(string affectedResidueString, double massDifference)
        {
            Add(affectedResidueString, massDifference, ModEntry.ModificationTypes.Dynamic);
        }

        public double Dyn_Mod_n_MassDiff(int dynModNumber)
        {
            ModEntry dm;
            var index = dynModNumber - 1;

            try
            {
                dm = this[index];
            }
            catch
            {
                dm = new ModEntry(ConvertAffectedResStringToList("C"), 0.0d, ModEntry.ModificationTypes.Dynamic);
            }
            return dm.MassDifference;
        }

        public void Dyn_Mod_n_MassDiff(int dynModNumber, double value)
        {
            var index = dynModNumber - 1;

            if (index < Count)
            {
                var dm = this[index];
                dm.MassDifference = value;
                Replace(index, dm);
            }
            else
            {
                Add("C", value);
            }
        }

        public string Dyn_Mod_n_AAList(int dynModNumber)
        {
            ModEntry dm;
            var index = dynModNumber - 1;

            try
            {
                dm = this[index];
            }
            catch
            {
                dm = new ModEntry(ConvertAffectedResStringToList("C"), 0.0d, ModEntry.ModificationTypes.Dynamic);
            }
            return dm.ReturnAllAffectedResiduesString;
        }

        public void Dyn_Mod_n_AAList(int dynModNumber, string value)
        {
            var index = dynModNumber - 1;

            if (index < Count)
            {
                var dm = this[index];
                dm.ResidueCollection = ConvertAffectedResStringToList(value);
                Replace(index, dm);
            }
            else
            {
                Add(value, 0.0d);
            }
        }

        public int Dyn_Mod_n_Global_ModID(int dynModNumber)
        {
            ModEntry dm;
            var index = dynModNumber - 1;

            try
            {
                dm = this[index];
            }
            catch
            {
                dm = new ModEntry(ConvertAffectedResStringToList("C"), 0.0d, ModEntry.ModificationTypes.Dynamic);
            }
            return dm.GlobalModID;
        }

        public void Dyn_Mod_n_Global_ModID(int dynModNumber, int value)
        {
            var index = dynModNumber - 1;

            if (index < Count)
            {
                var dm = this[index];
                dm.GlobalModID = value;
                Replace(index, dm);
            }
            else
            {
                Add("C", 0.0d);
            }
        }

        protected virtual string AssembleModString(int counter)
        {
            var s = "";
            int padCount;

            foreach (var dynMod in this)
            {
                var tmpModMass = dynMod.MassDifference;
                var tmpModString = dynMod.ReturnAllAffectedResiduesString;
                s = s + tmpModMass.ToString("0.0000") + " " + tmpModString + " ";
                counter--;
            }

            for (padCount = 0; padCount < counter; padCount++)
            {
                if (padCount <= 2)
                {
                    s += "0.0000 C ";
                }
                else
                {
                    s += "0.0000 X ";
                }
            }

            return s.Trim();
        }

        private void ParseDynamicModString(string dynamicModString)
        {
            var splitRE = new Regex(@"(?<modmass>\d+\.\d+)\s+(?<residues>[A-Za-z]+)");

            foreach (Match m in splitRE.Matches(dynamicModString))
            {
                var modMass = double.Parse(m.Groups["modmass"].Value);
                var affectedResidues = m.Groups["residues"].ToString();

                if (Math.Abs(modMass) > float.Epsilon)
                {
                    var residueList = new List<string>();
                    for (var resCounter = 0; resCounter < affectedResidues.Length; resCounter++)
                    {
                        var residue = affectedResidues.Substring(resCounter, 1);
                        residueList.Add(residue);
                    }
                    var modEntry = new ModEntry(residueList, modMass, ModEntry.ModificationTypes.Dynamic);
                    Add(modEntry);
                }
            }
        }
    }
}
