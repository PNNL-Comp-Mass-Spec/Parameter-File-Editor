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
        public Dictionary<char, Dictionary<char, int>> ResidueAtomCounts { get; }

#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public GetResiduesList(IDBTools dbTools)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            ResidueAtomCounts = new Dictionary<char, Dictionary<char, int>>();

            const string sql = "SELECT residue_id, residue_symbol, description, average_mass, " +
                                      "monoisotopic_mass, num_c, num_h, num_n, num_o, num_s " +
                               "FROM V_Residues WHERE num_c > 0";

            dbTools.GetQueryResults(sql, out var residuesTable);

            var columnNames = new List<string>()
            {
                "residue_id",
                "residue_symbol",
                "description",
                "average_mass",
                "monoisotopic_mass",
                "num_c",
                "num_h",
                "num_n",
                "num_o",
                "num_s"
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
                var residueSymbol = dbTools.GetColumnValue(resultRow, columnMap, "Residue_Symbol");

                var atomCountsForResidue = new Dictionary<char, int>();

                // Get the atom counts
                // This for loop access columns Num_C, Num_H, Num_N, etc.
                foreach (var elementSymbol in elementSymbols)
                {
                    var columnName = "Num_" + elementSymbol;
                    var elementCount = dbTools.GetColumnValue(resultRow, columnMap, columnName);

                    if (int.TryParse(elementCount, out var elementCountVal))
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
