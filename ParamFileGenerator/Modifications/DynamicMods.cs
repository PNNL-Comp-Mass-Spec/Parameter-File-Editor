using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace ParamFileGenerator
{

    public class DynamicMods : Mods
    {
        // Public Property Dyn_Mod_1_MassDiff As Double
        // Get
        // Return Dyn_Mod_n_MassDiff(1)
        // End Get
        // Set
        // Dyn_Mod_n_MassDiff(1) = Value
        // End Set
        // End Property
        // Public Property Dyn_Mod_2_MassDiff As Double
        // Get
        // Return Dyn_Mod_n_MassDiff(2)
        // End Get
        // Set
        // Dyn_Mod_n_MassDiff(2) = Value
        // End Set
        // End Property
        // Public Property Dyn_Mod_3_MassDiff As Double
        // Get
        // Return Dyn_Mod_n_MassDiff(3)
        // End Get
        // Set
        // Dyn_Mod_n_MassDiff(3) = Value
        // End Set
        // End Property

        // Public Property Dyn_Mod_1_AAList As String
        // Get
        // Return Dyn_Mod_n_AAList(1)
        // End Get
        // Set(Value As String)
        // Dyn_Mod_n_AAList(1) = Value
        // End Set
        // End Property
        // Public Property Dyn_Mod_2_AAList As String
        // Get
        // Return Dyn_Mod_n_AAList(2)
        // End Get
        // Set(Value As String)
        // Dyn_Mod_n_AAList(2) = Value
        // End Set
        // End Property
        // Public Property Dyn_Mod_3_AAList As String
        // Get
        // Return Dyn_Mod_n_AAList(3)
        // End Get
        // Set(Value As String)
        // Dyn_Mod_n_AAList(3) = Value
        // End Set
        // End Property

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
            // If Initialized Then
            s = AssembleModString(maxDynMods);
            return s;
            // Else
            // Return ""
            // End If
        }

        // 'TODO replace with real function for term dyn mods
        // 'Just a placeholder for now
        // 'Public Function ReturnDynTermModString As String
        // '    Return "0.0000 0.0000"
        // 'End Function

        public new void Add(string AffectedResidueString, double MassDifference)
        {

            var residueList = ConvertAffectedResStringToList(AffectedResidueString);
            var newDynMod = new ModEntry(residueList, MassDifference, ModEntry.ModificationTypes.Dynamic);
            List.Add(newDynMod);
        }
        public void Add(ModEntry ModToAdd)
        {
            List.Add(ModToAdd);
        }

        public double Dyn_Mod_n_MassDiff(int DynModNumber)
        {
            ModEntry dm;
            int index = DynModNumber - 1;
            try
            {
                dm = (ModEntry)List[index];
            }
            catch (Exception ex)
            {
                dm = new ModEntry(ConvertAffectedResStringToList("C"), 0.0d, ModEntry.ModificationTypes.Dynamic);
            }
            return dm.MassDifference;
        }

        public void Dyn_Mod_n_MassDiff(int DynModNumber, double value)
        {
            int index = DynModNumber - 1;
            ModEntry dm;
            if (index <= List.Count - 1)
            {
                dm = (ModEntry)List[index];
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
                dm = (ModEntry)List[index];
            }
            catch (Exception ex)
            {
                dm = new ModEntry(ConvertAffectedResStringToList("C"), 0.0d, ModEntry.ModificationTypes.Dynamic);
            }
            return dm.ReturnAllAffectedResiduesString;
        }

        public void Dyn_Mod_n_AAList(int DynModNumber, string value)
        {
            int index = DynModNumber - 1;
            ModEntry dm;
            if (index <= List.Count - 1)
            {
                dm = (ModEntry)List[index];
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
                dm = (ModEntry)List[index];
            }
            catch (Exception ex)
            {
                dm = new ModEntry(ConvertAffectedResStringToList("C"), 0.0d, ModEntry.ModificationTypes.Dynamic);
            }
            return dm.GlobalModID;
        }

        public void Dyn_Mod_n_Global_ModID(int DynModNumber, int value)
        {
            int index = DynModNumber - 1;
            ModEntry dm;
            if (index <= List.Count - 1)
            {
                dm = (ModEntry)List[index];
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
            string tmpModString;
            double tmpModMass;
            int padCount;

            foreach (ModEntry dynMod in List)
            {
                tmpModMass = dynMod.MassDifference;
                tmpModString = dynMod.ReturnAllAffectedResiduesString;
                s = s + Strings.Format(tmpModMass, "0.0000") + " " + tmpModString + " ";
                counter -= 1;
            }

            var loopTo = counter - 1;
            for (padCount = 0; padCount <= loopTo; padCount++)
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
                double tmpMass = Conversions.ToDouble(m.Groups["modmass"].Value);
                string tmpResString = m.Groups["residues"].ToString();

                if (Math.Abs(tmpMass) > float.Epsilon)
                {
                    var residueList = new List<string>();
                    for (int resCounter = 1, loopTo = Strings.Len(tmpResString); resCounter <= loopTo; resCounter++)
                    {
                        string tmpRes = Strings.Mid(tmpResString, resCounter, 1);
                        residueList.Add(tmpRes);
                    }
                    var modEntry = new ModEntry(residueList, tmpMass, ModEntry.ModificationTypes.Dynamic);
                    Add(modEntry);
                }
            }

        }

        protected string m_OrigDynModString;
        // private m_EmptyMod as New ModEntry(

    }
}
