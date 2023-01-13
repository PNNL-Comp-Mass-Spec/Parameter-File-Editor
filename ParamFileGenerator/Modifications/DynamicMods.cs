using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ParamFileGenerator
{
    public class DynamicMods : Mods
    {
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

        public DynamicMods(string DynamicModString) : base()
        {
            m_OrigDynModString = DynamicModString;
            ParseDynModString(m_OrigDynModString);
        }

        public DynamicMods() : base()
        {
            m_OrigDynModString = null;
        }

        public string ReturnDynModString(int maxDynMods)
        {
            string s;
            //if (Initialized)
            //{
            s = AssembleModString(maxDynMods);
            return s;
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

        public new void Add(string AffectedResidueString, double MassDifference)
        {
            m_Add(AffectedResidueString, MassDifference, ModEntry.ModificationTypes.Dynamic);
        }

        public double Dyn_Mod_n_MassDiff(int DynModNumber)
        {
            ModEntry dm;
            int index = DynModNumber - 1;
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

        public void Dyn_Mod_n_MassDiff(int DynModNumber, double value)
        {
            int index = DynModNumber - 1;
            ModEntry dm;
            if (index < Count)
            {
                dm = this[index];
                dm.MassDifference = value;
                Replace(index, dm);
            }
            else
            {
                Add("C", value);
            }
        }

        public string Dyn_Mod_n_AAList(int DynModNumber)
        {
            ModEntry dm;
            int index = DynModNumber - 1;
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

        public void Dyn_Mod_n_AAList(int DynModNumber, string value)
        {
            int index = DynModNumber - 1;
            ModEntry dm;
            if (index < Count)
            {
                dm = this[index];
                dm.ResidueCollection = ConvertAffectedResStringToList(value);
                Replace(index, dm);
            }
            else
            {
                Add(value, 0.0d);
            }
        }

        public int Dyn_Mod_n_Global_ModID(int DynModNumber)
        {
            ModEntry dm;
            int index = DynModNumber - 1;
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

        public void Dyn_Mod_n_Global_ModID(int DynModNumber, int value)
        {
            int index = DynModNumber - 1;
            ModEntry dm;
            if (index < Count)
            {
                dm = this[index];
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
            string s = "";
            int padCount;

            foreach (ModEntry dynMod in this)
            {
                var tmpModMass = dynMod.MassDifference;
                var tmpModString = dynMod.ReturnAllAffectedResiduesString;
                s = s + tmpModMass.ToString("0.0000") + " " + tmpModString + " ";
                counter -= 1;
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

        protected virtual void ParseDynModString(string DMString)
        {
            var splitRE = new Regex(@"(?<modmass>\d+\.\d+)\s+(?<residues>[A-Za-z]+)");
            var matches = splitRE.Matches(DMString);

            foreach (Match m in matches)
            {
                double tmpMass = double.Parse(m.Groups["modmass"].Value);
                string tmpResString = m.Groups["residues"].ToString();

                if (Math.Abs(tmpMass) > float.Epsilon)
                {
                    var residueList = new List<string>();
                    for (int resCounter = 0; resCounter < tmpResString.Length; resCounter++)
                    {
                        string tmpRes = tmpResString.Substring(resCounter, 1);
                        residueList.Add(tmpRes);
                    }
                    var modEntry = new ModEntry(residueList, tmpMass, ModEntry.ModificationTypes.Dynamic);
                    Add(modEntry);
                }
            }
        }

        protected string m_OrigDynModString;
        //private ModEntry m_EmptyMod = new ModEntry(
    }
}
