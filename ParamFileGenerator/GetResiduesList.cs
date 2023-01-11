using System.Collections.Generic;
using PRISMDatabaseUtils;

namespace ParamFileGenerator
{
    public class GetResiduesList
    {
        /// <summary>
        /// Dictionary where keys are amino acid residue (one letter abbreviation)
        /// and values are a dictionary with atom counts (number of C, H, N, O, and S atoms)
        /// </summary>
        /// <returns></returns>
        public Dictionary<char, Dictionary<char, int>> ResidueAtomCounts { get; private set; }

#pragma warning disable CS3001 // Type of parameter is not CLS-compliant
        public GetResiduesList(IDBTools dbTools)
#pragma warning restore CS3001 // Type of parameter is not CLS-compliant
        {
            ResidueAtomCounts = new Dictionary<char, Dictionary<char, int>>();

            string SQL = "SELECT * FROM T_Residues WHERE [Num_C] > 0";

            List<List<string>> residuesTable = null;
            dbTools.GetQueryResults(SQL, out residuesTable);

            var columnNames = new List<string>()
            {
                "Residue_ID",
                "Residue_Symbol",
                "Description",
                "Average_Mass",
                "Monoisotopic_Mass",
                "Num_C",
                "Num_H",
                "Num_N",
                "Num_O",
                "Num_S"
            };

            // ' This maps column name to column index
            var columnMap = dbTools.GetColumnMapping(columnNames);

            var elementSymbols = new List<char>()
            {
                'C',
                'H',
                'N',
                'O',
                'S'
            };

            foreach (var resultRow in residuesTable)
            {
                string residueSymbol = dbTools.GetColumnValue(resultRow, columnMap, "Residue_Symbol");

                var atomCountsForResidue = new Dictionary<char, int>();

                // Get the atom counts
                // This for loop access columns Num_C, Num_H, Num_N, etc.
                foreach (var elementSymbol in elementSymbols)
                {
                    string columnName = "Num_" + elementSymbol;
                    string elementCount = dbTools.GetColumnValue(resultRow, columnMap, columnName);

                    int elementCountVal;
                    if (int.TryParse(elementCount, out elementCountVal))
                    {
                        atomCountsForResidue.Add(elementSymbol, elementCountVal);
                    }
                    else
                    {
                        atomCountsForResidue.Add(elementSymbol, 0);
                    }
                }

                ResidueAtomCounts.Add(residueSymbol[0], atomCountsForResidue);
            }
        }
    }
}
