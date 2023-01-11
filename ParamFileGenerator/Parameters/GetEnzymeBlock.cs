﻿using System.Collections.Generic;
using System.IO;

namespace ParamFileGenerator
{
    public class GetEnzymeBlockType
    {
        private readonly string m_templateFilePath;
        private readonly string m_sectionName;
        private readonly List<string> m_EnzymeBlockCollection;

        public EnzymeCollection EnzymeList { get; set; }

        public GetEnzymeBlockType(
            string TemplateFilePath,
            string SectionName)
        {
            m_templateFilePath = TemplateFilePath;
            m_sectionName = SectionName;

            m_EnzymeBlockCollection = GetEnzymeBlock();
            EnzymeList = InterpretEnzymeBlockCollection(m_EnzymeBlockCollection);
        }

        private List<string> GetEnzymeBlock()
        {
            var enzymesFromFile = new List<string>();

            var fi = new FileInfo(m_templateFilePath);

            using (var reader = new StreamReader(new FileStream(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                while (!reader.EndOfStream)
                {
                    string dataLine = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(dataLine))
                    {
                        continue;
                    }

                    if ((dataLine ?? "") == ("[" + m_sectionName + "]" ?? ""))
                    {
                        while (!reader.EndOfStream)
                        {
                            string enzymeLine = reader.ReadLine();
                            if (string.IsNullOrWhiteSpace(enzymeLine))
                            {
                                continue;
                            }

                            enzymesFromFile.Add(enzymeLine);
                        }
                        break;
                    }
                }
            }

            if (enzymesFromFile.Count == 0)
            {
                enzymesFromFile = LoadDefaultEnzymes();
            }

            return enzymesFromFile;
        }

        private List<string> LoadDefaultEnzymes()
        {
            var defaultEnzymes = new List<string>()
            {
                "0.  No_Enzyme              0      -           -",
                "1.  Trypsin                1      KR          -",
                "2.  Trypsin_modified       1      KRLNH       -",
                "3.  Chymotrypsin           1      FWYL        -",
                "4.  Chymotrypsin__modified 1      FWY         -",
                "5.  Clostripain            1      R           -",
                "6.  Cyanogen_Bromide       1      M           -",
                "7.  IodosoBenzoate         1      W           -",
                "8.  Proline_Endopept       1      P           -",
                "9.  Staph_Protease         1      E           -",
                "10. Trypsin_K              1      K           P",
                "11. Trypsin_R              1      R           P",
                "12. GluC                   1      ED          -",
                "13. LysC                   1      K           -",
                "14. AspN                   0      D           -",
                "15. Elastase               1      ALIV        P",
                "16. Elastase/Tryp/Chymo    1      ALIVKRWFY   P",
                "17. ArgC                   1      R-          P",
                "18. Do_not_cleave          1      B           -",
                "19. LysN                   0      K           -"
            };

            return defaultEnzymes;
        }

        private EnzymeCollection InterpretEnzymeBlockCollection(IEnumerable<string> enzymeBlock)
        {
            var enzymeInfo = new EnzymeCollection();

            foreach (var s in enzymeBlock)
            {
                string sTmp = s.Substring(0, s.IndexOf(" "));
                if (sTmp.IndexOf(". ") >= 0)
                {
                    var tempEnzyme = new EnzymeDetails(s);
                    enzymeInfo.add(tempEnzyme);
                }
            }

            return enzymeInfo;
        }
    }
}
