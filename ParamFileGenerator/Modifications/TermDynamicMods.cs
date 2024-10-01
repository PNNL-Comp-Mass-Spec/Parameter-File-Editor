using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ParamFileGenerator.Modifications
{
    public class TermDynamicMods : DynamicMods
    {
        // Ignore Spelling: CTerm, NTerm

        public const string NTERM_SYMBOL = "<";
        public const string CTERM_SYMBOL = ">";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="termDynModString"></param>
        public TermDynamicMods(string termDynModString)
        {
            ParseDynModString(termDynModString);
        }

        public double Dyn_Mod_NTerm
        {
            get => GetTermDynMod(NTERM_SYMBOL);
            set => UpdateTermDynMod(NTERM_SYMBOL, value);
        }

        public double Dyn_Mod_CTerm
        {
            get => GetTermDynMod(CTERM_SYMBOL);
            set => UpdateTermDynMod(CTERM_SYMBOL, value);
        }

        protected double GetTermDynMod(string strSymbol)
        {
            var objModEntry = FindMod(strSymbol);

            return objModEntry?.MassDifference ?? 0d;
        }

        protected void UpdateTermDynMod(string strSymbol, double sngMass)
        {
            var intIndex = FindModIndex(strSymbol);

            if (intIndex < 0)
            {
                // Mod was not found
                // Add it (assuming sngMass is non-zero)
                if (Math.Abs(sngMass) > float.Epsilon)
                {
                    var resCollection = new List<string> { strSymbol };

                    Add(new ModEntry(resCollection, sngMass, ModEntry.ModificationTypes.Dynamic));
                }
            }
            // Mod was found
            // Update the mass (or remove it if sngMass is zero)
            else if (Math.Abs(sngMass) < float.Epsilon)
            {
                RemoveAt(intIndex);
            }
            else
            {
                var objModEntry = GetModEntry(intIndex);
                objModEntry.MassDifference = sngMass;
            }
        }

        protected sealed override void ParseDynModString(string dmString)
        {
            var splitRE = new Regex(@"(?<ctmodmass>\d+\.*\d*)\s+(?<ntmodmass>\d+\.*\d*)");

            if (dmString is null)
            {
                return;
            }

            if (splitRE.IsMatch(dmString))
            {
                var m = splitRE.Match(dmString);

                Dyn_Mod_NTerm = double.Parse(m.Groups["ntmodmass"].Value);
                Dyn_Mod_CTerm = double.Parse(m.Groups["ctmodmass"].Value);
            }
        }

        protected sealed override string AssembleModString(int counter)
        {
            var sb = new StringBuilder();

            //const string ctRes = ">";
            //const string ntRes = "<";

            var ctModMass = 0.0d;
            var ntModMass = 0.0d;

            foreach (var dynMod in this)
            {
                var tmpModMass = dynMod.MassDifference;
                var tmpModString = dynMod.ReturnAllAffectedResiduesString;

                if (tmpModString == ">")
                {
                    ctModMass = tmpModMass;
                    // ctRes = tmpModString;
                }
                else if (tmpModString == "<")
                {
                    ntModMass = tmpModMass;
                    // ntRes = tmpModString;
                }
            }

            sb.Append(ctModMass.ToString("0.000000"));
            sb.Append(" ");
            sb.Append(ntModMass.ToString("0.000000"));

            return sb.ToString().Trim();
        }
    }
}
