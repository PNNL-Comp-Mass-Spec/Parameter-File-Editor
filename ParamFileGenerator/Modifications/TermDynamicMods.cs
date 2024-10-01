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
        /// <param name="terminalDynamicModString"></param>
        public TermDynamicMods(string terminalDynamicModString)
        {
            ParseTerminalDynamicModString(terminalDynamicModString);
        }

        public double NTerminal_Dynamic_Mod
        {
            get => GetTerminalDynamicMod(NTERM_SYMBOL);
            set => UpdateTerminalDynamicMod(NTERM_SYMBOL, value);
        }

        public double CTerminal_Dynamic_Mod
        {
            get => GetTerminalDynamicMod(CTERM_SYMBOL);
            set => UpdateTerminalDynamicMod(CTERM_SYMBOL, value);
        }

        protected double GetTerminalDynamicMod(string strSymbol)
        {
            var objModEntry = FindMod(strSymbol);

            return objModEntry?.MassDifference ?? 0d;
        }

        protected void UpdateTerminalDynamicMod(string strSymbol, double modMass)
        {
            var intIndex = FindModIndex(strSymbol);

            if (intIndex < 0)
            {
                // Mod was not found
                // Add it (assuming modMass is non-zero)
                if (Math.Abs(modMass) > float.Epsilon)
                {
                    var resCollection = new List<string> { strSymbol };

                    Add(new ModEntry(resCollection, modMass, ModEntry.ModificationTypes.Dynamic));
                }
            }
            // Mod was found
            // Update the mass (or remove it if modMass is zero)
            else if (Math.Abs(modMass) < float.Epsilon)
            {
                RemoveAt(intIndex);
            }
            else
            {
                var objModEntry = GetModEntry(intIndex);
                objModEntry.MassDifference = modMass;
            }
        }

        protected sealed override void ParseTerminalDynamicModString(string dynamicModString)
        {
            var splitRE = new Regex(@"(?<ctmodmass>\d+\.*\d*)\s+(?<ntmodmass>\d+\.*\d*)");

            if (dynamicModString is null)
            {
                return;
            }

            if (splitRE.IsMatch(dynamicModString))
            {
                var m = splitRE.Match(dynamicModString);

                NTerminal_Dynamic_Mod = double.Parse(m.Groups["ntmodmass"].Value);
                CTerminal_Dynamic_Mod = double.Parse(m.Groups["ctmodmass"].Value);
            }
        }

        protected override string AssembleModString(int counter)
        {
            var sb = new StringBuilder();

            //const string ctRes = ">";
            //const string ntRes = "<";

            var ctModMass = 0.0d;
            var ntModMass = 0.0d;

            foreach (var dynMod in this)
            {
                var massDifference = dynMod.MassDifference;
                var affectedResidues = dynMod.ReturnAllAffectedResiduesString;

                if (affectedResidues == ">")
                {
                    ctModMass = massDifference;
                    // ctRes = modString;
                }
                else if (affectedResidues == "<")
                {
                    ntModMass = massDifference;
                    // ntRes = modString;
                }
            }

            sb.Append(ctModMass.ToString("0.000000"));
            sb.Append(" ");
            sb.Append(ntModMass.ToString("0.000000"));

            return sb.ToString().Trim();
        }
    }
}
