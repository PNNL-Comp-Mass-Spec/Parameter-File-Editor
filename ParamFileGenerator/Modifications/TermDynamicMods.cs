using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace ParamFileGenerator
{

    public class TermDynamicMods : DynamicMods
    {

        public const string NTERM_SYMBOL = "<";
        public const string CTERM_SYMBOL = ">";

        public TermDynamicMods(string TermDynModString) : base()
        {
            m_OrigDynModString = TermDynModString;
            ParseDynModString(m_OrigDynModString);

        }

        public double Dyn_Mod_NTerm
        {
            get
            {
                return GetTermDynMod(NTERM_SYMBOL);
            }
            set
            {
                UpdateTermDynMod(NTERM_SYMBOL, value);
            }
        }

        public double Dyn_Mod_CTerm
        {
            get
            {
                return GetTermDynMod(CTERM_SYMBOL);
            }
            set
            {
                UpdateTermDynMod(CTERM_SYMBOL, value);
            }
        }

        protected double GetTermDynMod(string strSymbol)
        {
            ModEntry objModEntry;
            objModEntry = m_FindMod(strSymbol);

            if (objModEntry is null)
            {
                return 0d;
            }
            else
            {
                return objModEntry.MassDifference;
            }
        }

        protected void UpdateTermDynMod(string strSymbol, double sngMass)
        {

            int intIndex;
            intIndex = m_FindModIndex(strSymbol);

            if (intIndex < 0)
            {
                // Mod was not found
                // Add it (assuming sngMass is non-zero)
                if (Math.Abs(sngMass) > float.Epsilon)
                {
                    var resCollection = new List<string>() { strSymbol };

                    Add(new ModEntry(resCollection, sngMass, ModEntry.ModificationTypes.Dynamic));
                }
            }
            // Mod was found
            // Update the mass (or remove it if sngMass is zero)
            else if (Math.Abs(sngMass) < float.Epsilon)
            {
                Remove(intIndex);
            }
            else
            {
                ModEntry objModEntry;
                objModEntry = GetModEntry(intIndex);
                objModEntry.MassDifference = sngMass;
            }
        }

        protected override void ParseDynModString(string DMString)
        {
            double tmpCTMass;
            double tmpNTMass;

            var splitRE = new Regex(@"(?<ctmodmass>\d+\.*\d*)\s+(?<ntmodmass>\d+\.*\d*)");
            Match m;

            if (DMString is null)
            {
                return;
            }


            if (splitRE.IsMatch(DMString))
            {
                m = splitRE.Match(DMString);

                tmpCTMass = Conversions.ToDouble(m.Groups["ctmodmass"].Value);
                tmpNTMass = Conversions.ToDouble(m.Groups["ntmodmass"].Value);

                Dyn_Mod_NTerm = tmpNTMass;
                Dyn_Mod_CTerm = tmpCTMass;

            }

        }

        protected override string AssembleModString(int counter)
        {
            var sb = new StringBuilder();

            string tmpModString;
            // Dim ctRes As String = ">"
            // Dim ntRes As String = "<"

            double ctModMass = 0.0d;
            double ntModMass = 0.0d;

            double tmpModMass;

            foreach (ModEntry dynMod in List)
            {
                tmpModMass = dynMod.MassDifference;
                tmpModString = dynMod.ReturnAllAffectedResiduesString;
                if (tmpModString == ">")
                {
                    ctModMass = tmpModMass;
                }
                // ctRes = tmpModString
                else if (tmpModString == "<")
                {
                    ntModMass = tmpModMass;
                    // ntRes = tmpModString
                }
            }

            sb.Append(Strings.Format(ctModMass, "0.000000"));
            sb.Append(" ");
            sb.Append(Strings.Format(ntModMass, "0.000000"));

            return sb.ToString().Trim();
        }

    }
}
