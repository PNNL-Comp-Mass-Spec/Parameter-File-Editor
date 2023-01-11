using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using PRISMDatabaseUtils;

namespace ParamFileGenerator.DownloadParams
{
    public class ParamsFromDMS : DBTask
    {
        // Ignore Spelling: diffs, mc

        private const string Param_File_Table = "T_Param_Files";
        private const string Param_Entry_Table = "T_Param_Entries";
        private const string Param_Mass_Mods_Table = "T_Param_File_Mass_Mods";
        private const string Mass_Corr_Factors = "T_Mass_Correction_Factors";
        private const string Residues_Table = "T_Residues";

        public enum AcceptableParams
        {
            SelectedEnzymeIndex,
            SelectedEnzymeCleavagePosition,
            MaximumNumberMissedCleavages,
            ParentMassType,
            FragmentMassType,
            DynamicMods,
            TermDynamicMods,
            StaticModificationsList,
            IsotopicMods,
            PartialSequenceToMatch,
            CreateOutputFiles,
            NumberOfResultsToProcess,
            MaximumNumAAPerDynMod,
            MaximumNumDifferentialPerPeptide,
            PeptideMassTolerance,
            FragmentIonTolerance,
            NumberOfOutputLines,
            NumberOfDescriptionLines,
            ShowFragmentIons,
            PrintDuplicateReferences,
            SelectedNucReadingFrameIndex,
            RemovePrecursorPeak,
            IonSeries,
            IonCutoffPercentage,
            MinimumProteinMassToSearch,
            MaximumProteinMassToSearch,
            NumberOfDetectedPeaksToMatch,
            NumberOfAllowedDetectedPeakErrors,
            MatchedPeakMassTolerance,
            AminoAcidsAllUpperCase,
            SequenceHeaderInfoToFilter,
            PeptideMassUnits,
            FragmentMassUnits
        }

        public enum BasicParams
        {
            SelectedEnzymeIndex,
            SelectedEnzymeCleavagePosition,
            MaximumNumberMissedCleavages,
            ParentMassType,
            FragmentMassType,
            PartialSequenceToMatch
        }

        public enum AdvancedParams
        {
            CreateOutputFiles,
            NumberOfResultsToProcess,
            MaximumNumAAPerDynMod,
            MaximumNumDifferentialPerPeptide,
            PeptideMassTolerance,
            FragmentIonTolerance,
            NumberOfOutputLines,
            NumberOfDescriptionLines,
            ShowFragmentIons,
            PrintDuplicateReferences,
            SelectedNucReadingFrameIndex,
            RemovePrecursorPeak,
            IonSeries,
            IonCutoffPercentage,
            MinimumProteinMassToSearch,
            MaximumProteinMassToSearch,
            NumberOfDetectedPeaksToMatch,
            NumberOfAllowedDetectedPeakErrors,
            MatchedPeakMassTolerance,
            AminoAcidsAllUpperCase,
            SequenceHeaderInfoToFilter,
            PeptideMassUnits,
            FragmentMassUnits
        }

        public enum IonSeriesParams
        {
            Use_a_Ions,
            Use_b_Ions,
            Use_y_Ions,
            a_Ion_Weighting,
            b_Ion_Weighting,
            c_Ion_Weighting,
            d_Ion_Weighting,
            v_Ion_Weighting,
            w_Ion_Weighting,
            x_Ion_Weighting,
            y_Ion_Weighting,
            z_Ion_Weighting
        }

        public enum eParamFileTypeConstants
        {
            Unknown = 0,
            None = 1,
            Sequest = 1000,
            QTOFPek = 1001,
            DeNovoPeak = 1002,
            icr2ls = 1003,
            MLynxPek = 1004,
            AgilentTOFPek = 1005,
            LTQ_FTPek = 1006,
            MASIC = 1007,
            XTandem = 1008,
            Decon2LS = 1010,
            TIC_D2L = 1011,
            Inspect = 1012,
            MSXML_Gen = 1013,
            DTA_Gen = 1014,
            MSClusterDAT_Gen = 1015,
            OMSSA = 1016,
            MultiAlign = 1017,
            MSGFDB = 1018,
            MSAlign = 1019,
            SMAQC = 1020,
            LipidMapSearch = 1021,
            MSAlign_Histone = 1022,
            MODa = 1023,
            GlyQIQ = 1024,
            MSPathFinder = 1025,
            MODPlus = 1028,
            TopFD = 1031,
            TopPIC = 1032,
            MSFragger = 1033,
            MaxQuant = 1034
        }

        private int m_ID;
        private string m_Name;
        private eParamFileTypeConstants m_ParamFileType;
        private Params m_Params;

        /// <summary>
        /// Parameter file table
        /// </summary>
        private DataTable m_ParamFileTable;
        private DataTable m_ParamEntryTable;

        private readonly Params m_BaseLineParamSet;
        private readonly List<string> m_AcceptableParams;
        private readonly List<string> m_BasicParams;
        private readonly List<string> m_AdvancedParams;
        private readonly List<string> m_IonSeriesParams;
        private DataTable m_MassMods;

        public bool ParamFileTableLoaded => m_ParamFileTable is not null && m_ParamFileTable.Rows.Count > 0;

#pragma warning disable CS3001 // Type of parameter is not CLS-compliant
        public ParamsFromDMS(IDBTools dbTools) : base(dbTools)
#pragma warning restore CS3001 // Type of parameter is not CLS-compliant
        {
            m_AcceptableParams = LoadAcceptableParamList();
            m_BasicParams = LoadBasicParams();
            m_AdvancedParams = LoadAdvancedParams();
            m_IonSeriesParams = LoadIonSeriesParams();
            m_BaseLineParamSet = MainProcess.BaseLineParamSet;

            bool success = GetParamsFromDMS();
            if (!success)
            {
                throw new Exception("Unable to obtain data from " + Param_File_Table + " and/or " + Param_Entry_Table);
            }
        }

        public void RefreshParamsFromDMS()
        {
            GetParamsFromDMS();
        }

        public Params ReadParamsFromDMS(string paramSetName)
        {
            // Retrieve ID number first, then use the same procedure as below
            m_Name = paramSetName;

            m_ParamFileType = GetTypeWithName(paramSetName);

            if (m_ParamFileType == eParamFileTypeConstants.Unknown)
            {
                throw new Exception("Parameter file " + paramSetName + " was not found in table " + Param_File_Table);
            }

            if (m_ParamFileType != eParamFileTypeConstants.Sequest)
            {
                // This param file type is not supported for export from DMS
                string paramFileTypeName = Enum.GetName(typeof(eParamFileTypeConstants), m_ParamFileType);
                if (string.IsNullOrEmpty(paramFileTypeName))
                {
                    paramFileTypeName = "Unknown";
                }

                throw new NotSupportedException("Parameter file " + paramSetName + " is of type " + paramFileTypeName + ", which isn't support for export from DMS");
            }

            m_ID = GetIDWithName(m_Name, m_ParamFileType);

            m_Params = RetrieveParams(m_ID, m_ParamFileType);
            return m_Params;
        }

        public Params ReadParamsFromDMS(int paramSetID)
        {
            m_ID = paramSetID;
            m_ParamFileType = GetTypeWithID(m_ID);

            if (m_ParamFileType == eParamFileTypeConstants.Unknown)
            {
                throw new Exception("Parameter file ID " + paramSetID + " was not found in table " + Param_File_Table);
            }

            if (m_ParamFileType != eParamFileTypeConstants.Sequest)
            {
                // This param file type is not supported for export from DMS
                throw new NotSupportedException("Parameter file ID " + paramSetID + " is of type " + Enum.GetName(typeof(eParamFileTypeConstants), m_ParamFileType) + ", which isn't support for export from DMS");
            }

            m_Params = RetrieveParams(m_ID, m_ParamFileType);
            return m_Params;
        }

        public DataTable RetrieveAvailableParams()
        {
            return GetAvailableParamSets();
        }

        public DataTable RetrieveParamFileTypes()
        {
            return GetParamFileTypes();
        }

        public bool ParamSetNameExists(string paramSetName)
        {
            return DoesParamSetNameExist(paramSetName);
        }

        public bool ParamSetIDExists(int paramSetID)
        {
            return DoesParamSetIDExist(paramSetID);
        }

        public int GetParamSetIDFromName(string Name)
        {
            eParamFileTypeConstants eParamFileType;
            eParamFileType = GetTypeWithName(Name);

            if (eParamFileType == eParamFileTypeConstants.Unknown)
            {
                Console.WriteLine("Parameter file " + Name + "  was not found in table " + Param_File_Table);
                return -1;
            }

            return GetIDWithName(Name, eParamFileType);
        }

        private List<string> LoadAcceptableParamList()
        {
            var paramEnum = Enum.GetNames(typeof(AcceptableParams));
            var paramList = new List<string>();
            foreach (var param in paramEnum)
                paramList.Add(param);
            return paramList;
        }

        private List<string> LoadBasicParams()
        {
            var paramEnum = Enum.GetNames(typeof(BasicParams));
            var paramList = new List<string>();
            foreach (var param in paramEnum)
                paramList.Add(param);
            return paramList;
        }

        private List<string> LoadAdvancedParams()
        {
            var paramEnum = Enum.GetNames(typeof(AdvancedParams));
            var paramList = new List<string>();
            foreach (var param in paramEnum)
                paramList.Add(param);
            return paramList;
        }

        private List<string> LoadIonSeriesParams()
        {
            var paramEnum = Enum.GetNames(typeof(IonSeriesParams));
            var paramList = new List<string>();
            foreach (var param in paramEnum)
                paramList.Add(param);
            return paramList;
        }

        private bool GetParamsFromDMS()
        {
            // SQL to grab param file table
            // The ID column is named Param_File_ID
            string query1 = "SELECT Param_File_ID, Param_File_Name, Param_File_Description, Param_File_Type_ID " +
                            "FROM " + Param_File_Table;
            // Optional: " WHERE [Param_File_Type_ID] = 1000"

            m_ParamFileTable = GetTable(query1);

            // SQL to grab param entry table
            // The ID column is named Param_Entry_ID
            string query2 = "SELECT Param_Entry_ID, Entry_Sequence_Order, Entry_Type, Entry_Specifier, Entry_Value, Param_File_ID " +
                            "FROM " + Param_Entry_Table + " " +
                            "WHERE [Entry_Type] not like '%Modification'";

            m_ParamEntryTable = GetTable(query2);

            return true;
        }

        private Params RetrieveParams(int paramSetID, eParamFileTypeConstants eParamFileType)
        {
            var p = GetParamSetWithID(paramSetID, eParamFileType);
            return p;
        }

        // TODO Fix this function for new mod handling
        protected Params GetParamSetWithID(int paramSetID, eParamFileTypeConstants eParamFileType, bool DisableMassLookup = false)
        {
            DataRow matchingRow = null;

            if (!GetParamFileRowByID(paramSetID, out matchingRow))
            {
                // Match not found
                return new Params();
            }

            var foundRows = m_ParamEntryTable.Select("[Param_File_ID] = " + paramSetID, "[Entry_Sequence_Order]");

            var storageSet = MakeStorageClassFromTableRowSet(foundRows);

            if (!DisableMassLookup)
            {
                storageSet = GetMassModsFromDMS(paramSetID, eParamFileType, ref storageSet);
            }

            var p = UpdateParamSetFromDataCollection(storageSet);
            p.FileName = (string)matchingRow["Param_File_Name"];
            p.Description = SummarizeDiffColl(ref storageSet);

            foreach (DataRow paramRow in foundRows)
                p.AddLoadedParamName(paramRow["Entry_Specifier"].ToString(), paramRow["Entry_Value"].ToString());

            return p;
        }

        private DMSParamStorage MakeStorageClassFromTableRowSet(IEnumerable<DataRow> foundRows)
        {
            var storageClass = new DMSParamStorage();
            string tmpSpec;
            string tmpValue;

            DMSParamStorage.ParamTypes tmpType;

            foreach (var foundRow in foundRows)
            {
                tmpSpec = (string)foundRow["Entry_Specifier"];
                tmpValue = (string)foundRow["Entry_Value"];
                tmpType = (DMSParamStorage.ParamTypes)Enum.Parse(typeof(DMSParamStorage.ParamTypes), foundRow["Entry_Type"].ToString());

                storageClass.Add(tmpSpec, tmpValue, tmpType);
            }

            return storageClass;
        }

        private DMSParamStorage GetMassModsFromDMS(int paramSetID, eParamFileTypeConstants eParamFileType, ref DMSParamStorage @params)
        {
            const int MaxDynMods = 15;

            DataRow foundRow;
            DataRow[] foundRows;
            string tmpSpec;
            string tmpValue;

            DMSParamStorage.ParamTypes tmpType;

            // If m_MassMods Is Nothing Or m_MassMods.Rows.Count = 0 Then
            string SQL;

            SQL = "SELECT mm.Mod_Type_Symbol as Mod_Type_Symbol, r.Residue_Symbol as Residue_Symbol, " +
                  "mc.Monoisotopic_Mass as Monoisotopic_Mass, " +
                  "mm.Local_Symbol_ID as Local_Symbol_ID, mc.Affected_Atom as Affected_Atom " +
                  "FROM " + Param_Mass_Mods_Table + " mm INNER JOIN " +
                  Mass_Corr_Factors + " mc ON mm.Mass_Correction_ID = mc.Mass_Correction_ID INNER JOIN " +
                  Residues_Table + " r ON mm.Residue_ID = r.Residue_ID " +
                  "WHERE mm.Param_File_ID = " + paramSetID;

            m_MassMods = GetTable(SQL);

            // Look for Dynamic mods

            var lstLocalSymbolIDs = new List<int>();

            switch (eParamFileType)
            {
                case eParamFileTypeConstants.Sequest:
                    lstLocalSymbolIDs.Add(1);   // *
                    lstLocalSymbolIDs.Add(2);   // #
                    lstLocalSymbolIDs.Add(3);   // @
                    lstLocalSymbolIDs.Add(10);  // ^
                    lstLocalSymbolIDs.Add(11);  // ~
                    lstLocalSymbolIDs.Add(4);   // $
                    break;
                case eParamFileTypeConstants.XTandem:
                    lstLocalSymbolIDs.Add(1);   // *
                    lstLocalSymbolIDs.Add(2);   // #
                    lstLocalSymbolIDs.Add(3);   // @
                    lstLocalSymbolIDs.Add(4);   // $
                    lstLocalSymbolIDs.Add(5);   // &
                    lstLocalSymbolIDs.Add(6);   // !
                    lstLocalSymbolIDs.Add(7);   // %
                    break;
            }

            for (int intSymbolID = 1; intSymbolID <= MaxDynMods; intSymbolID++)
            {
                if (!lstLocalSymbolIDs.Contains(intSymbolID))
                {
                    lstLocalSymbolIDs.Add(intSymbolID);
                }
            }

            foreach (int intSymbolID in lstLocalSymbolIDs)
            {
                foundRows = m_MassMods.Select("[Mod_Type_Symbol] = 'D' AND [Local_Symbol_ID] = " + intSymbolID + " AND [Residue_Symbol] <> '<' AND [Residue_Symbol] <> '>'", "[Local_Symbol_ID]");
                if (foundRows.Length > 0)
                {
                    tmpSpec = GetDynModSpecifier(foundRows);
                    tmpValue = foundRows[0]["Monoisotopic_Mass"].ToString();
                    tmpType = DMSParamStorage.ParamTypes.DynamicModification;
                    @params.Add(tmpSpec, tmpValue, tmpType);
                }
            }

            // Find N-Term Dynamic Mods
            foundRows = m_MassMods.Select("[Mod_Type_Symbol] = 'D' AND [Residue_Symbol] = '<'");
            if (foundRows.Length > 0)
            {
                tmpSpec = GetDynModSpecifier(foundRows);
                tmpValue = foundRows[0]["Monoisotopic_Mass"].ToString();
                tmpType = DMSParamStorage.ParamTypes.TermDynamicModification;
                @params.Add(tmpSpec, tmpValue, tmpType);
            }

            // Find C-Term Dynamic Mods
            foundRows = m_MassMods.Select("[Mod_Type_Symbol] = 'D' AND [Residue_Symbol] = '>'");
            if (foundRows.Length > 0)
            {
                tmpSpec = GetDynModSpecifier(foundRows);
                tmpValue = foundRows[0]["Monoisotopic_Mass"].ToString();
                tmpType = DMSParamStorage.ParamTypes.TermDynamicModification;
                @params.Add(tmpSpec, tmpValue, tmpType);
            }

            // Look for Static and terminal mods

            foundRows = m_MassMods.Select("[Mod_Type_Symbol] = 'S' OR [Mod_Type_Symbol] = 'P' or [Mod_Type_Symbol] = 'T'");

            foreach (var currentFoundRow in foundRows)
            {
                foundRow = currentFoundRow;
                tmpSpec = foundRow["Residue_Symbol"].ToString();
                switch (tmpSpec ?? "")
                {
                    case "<":
                        tmpSpec = "N_Term_Peptide";
                        break;
                    case ">":
                        tmpSpec = "C_Term_Peptide";
                        break;
                    case "[":
                        tmpSpec = "N_Term_Protein";
                        break;
                    case "]":
                        tmpSpec = "C_Term_Protein";
                        break;
                }

                tmpValue = foundRow["Monoisotopic_Mass"].ToString();
                tmpType = DMSParamStorage.ParamTypes.StaticModification;
                @params.Add(tmpSpec, tmpValue, tmpType);
            }

            // TODO Still need code to handle import/export of isotopic mods

            foundRows = m_MassMods.Select("[Mod_Type_Symbol] = 'I'");

            foreach (var currentFoundRow1 in foundRows)
            {
                foundRow = currentFoundRow1;
                tmpSpec = foundRow["Affected_Atom"].ToString();
                tmpValue = foundRow["Monoisotopic_Mass"].ToString();
                tmpType = DMSParamStorage.ParamTypes.IsotopicModification;
                @params.Add(tmpSpec, tmpValue, tmpType);
            }

            return @params;
        }

        private string GetDynModSpecifier(DataRow[] rowSet)
        {
            string tmpSpec = "";

            if (rowSet.Length > 0)               // We have dynamic mods
            {
                foreach (DataRow foundRow in rowSet)
                    tmpSpec += foundRow["Residue_Symbol"].ToString();
                return tmpSpec;
            }
            else
            {
                return null;
            }
        }

        private int GetIDWithName(string Name, eParamFileTypeConstants eParamFileType)
        {
            var foundRows = m_ParamFileTable.Select("[Param_File_Name] = '" + Name + "' AND [Param_File_Type_ID] = " + ((int)eParamFileType).ToString());
            DataRow foundRow;
            int tmpID;
            if (foundRows.Length > 0)
            {
                foundRow = foundRows[0];
                tmpID = Params.SafeCastInt(foundRow["Param_File_ID"]);
            }
            else
            {
                tmpID = -1;
            }
            return tmpID;
        }

        private eParamFileTypeConstants GetTypeWithID(int paramFileID)
        {
            var foundRows = m_ParamFileTable.Select("[Param_File_ID] = " + paramFileID.ToString());
            DataRow foundRow;
            eParamFileTypeConstants tmpID;
            if (foundRows.Length > 0)
            {
                foundRow = foundRows[0];
                tmpID = (eParamFileTypeConstants)Params.SafeCastInt(foundRow["Param_File_Type_ID"]);
            }
            else
            {
                tmpID = eParamFileTypeConstants.None;
            }
            return tmpID;
        }

        private eParamFileTypeConstants GetTypeWithName(string paramFileName)
        {
            var foundRows = m_ParamFileTable.Select("[Param_File_Name] = '" + paramFileName + "'");
            DataRow foundRow;
            eParamFileTypeConstants tmpID;
            if (foundRows.Length > 0)
            {
                foundRow = foundRows[0];
                tmpID = (eParamFileTypeConstants)Params.SafeCastInt(foundRow["Param_File_Type_ID"]);
            }
            else
            {
                tmpID = eParamFileTypeConstants.None;
            }
            return tmpID;
        }

        private string GetDescriptionWithID(int paramFileID)
        {
            DataRow matchingRow = null;

            if (GetParamFileRowByID(paramFileID, out matchingRow))
            {
                string tmpString = matchingRow["Param_File_Description"].ToString();
                if (string.IsNullOrWhiteSpace(tmpString))
                {
                    return string.Empty;
                }
                else
                {
                    return tmpString;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Finds the row in m_ParamFileTable with the given parameter file ID
        /// </summary>
        /// <param name="paramFileID"></param>
        /// <param name="matchingRow">The row if found, otherwise null</param>
        /// <returns>True if the parameter file was found, otherwise false</returns>
        private bool GetParamFileRowByID(int paramFileID, out DataRow matchingRow)
        {
            var foundRows = m_ParamFileTable.Select("[Param_File_ID] = " + paramFileID);

            if (foundRows.Length > 0)
            {
                matchingRow = foundRows[0];
                return true;
            }

            matchingRow = null;
            return false;
        }

        /// <summary>
        /// Finds parameter file info for SEQUEST, X!Tandem, MSGF+, MSPathFinder, MODPlus, TopPIC, MSFragger, or MaxQuant
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private DataTable GetAvailableParamSets()
        {
            string paramTableSQL =
                "SELECT " +
                "  Param_File_ID as ID, " +
                "  Param_File_Name AS Filename, " +
                "  Param_File_Description as Diffs, " +
                "  Param_File_Type_ID as Type_ID " +
                "FROM T_Param_Files " +
                "WHERE Param_File_Type_ID = " + ((int)eParamFileTypeConstants.Sequest).ToString() +
                "   or Param_File_Type_ID = " + ((int)eParamFileTypeConstants.XTandem).ToString() +
                "   or Param_File_Type_ID = " + ((int)eParamFileTypeConstants.MSGFDB).ToString() +
                "   or Param_File_Type_ID = " + ((int)eParamFileTypeConstants.MSPathFinder).ToString() +
                "   or Param_File_Type_ID = " + ((int)eParamFileTypeConstants.MODPlus).ToString() +
                "   or Param_File_Type_ID = " + ((int)eParamFileTypeConstants.TopPIC).ToString() +
                "   or Param_File_Type_ID = " + ((int)eParamFileTypeConstants.MSFragger).ToString() +
                "   or Param_File_Type_ID = " + ((int)eParamFileTypeConstants.MaxQuant).ToString();

            var tmpIDTable = GetTable(paramTableSQL);

            // 'Load tmpIDTable
            int tmpID;
            string tmpDiffs;
            int tmpType;

            foreach (DataRow dr in tmpIDTable.Rows)
            {
                tmpType = (int)dr["Type_ID"];

                if (tmpType == (int)eParamFileTypeConstants.Sequest)
                {
                    tmpID = (int)dr["ID"];
                    tmpDiffs = (string)dr["Diffs"];
                    if (tmpDiffs is null)
                    {
                        eParamFileTypeConstants eParamFileTypeID;
                        eParamFileTypeID = (eParamFileTypeConstants)tmpType;

                        tmpDiffs = DistillFeaturesFromParamSet(tmpID, eParamFileTypeID);

                        dr["Diffs"] = tmpDiffs;
                        dr.AcceptChanges();
                    }
                }
            }

            return tmpIDTable;

            // Need filtering code for tmpIDTable here...
        }

        private DataTable GetParamFileTypes()
        {
            DataTable tmpTypeTable;
            string tableTypesSQL;
            tableTypesSQL =
                "SELECT Param_File_Type_ID as ID, Param_File_Type AS Type " +
                "FROM T_Param_File_Types";

            tmpTypeTable = GetTable(tableTypesSQL);

            return tmpTypeTable;
        }

        protected string DistillFeaturesFromParamSet(Params paramSet)
        {
            var templateColl = WriteDataCollectionFromParamSet(m_BaseLineParamSet);
            var checkColl = WriteDataCollectionFromParamSet(paramSet);

            var diffColl = CompareDataCollections(templateColl, checkColl);
            return SummarizeDiffColl(ref diffColl);
        }

        protected string DistillFeaturesFromParamSet(int paramSetID, eParamFileTypeConstants eParamFileTypeID)
        {
            var p = GetParamSetWithID(paramSetID, eParamFileTypeID);

            return p.Description;
        }

        private DMSParamStorage WriteDataCollectionFromParamSet(Params paramSet)
        {
            var c = new DMSParamStorage();

            var pType = paramSet.GetType();
            Type tmpType;
            var pProps = pType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            string tmpName;
            string tmpValue;

            foreach (var pProp in pProps)
            {
                tmpName = pProp.Name;
                tmpType = pProp.PropertyType;
                if (m_AcceptableParams.Contains(tmpName))
                {
                    if (tmpType.Name == "IonSeries")
                    {
                        c = ExpandIonSeries(paramSet.IonSeries, ref c);
                    }

                    else if (tmpType.Name == "IsoMods")
                    {
                        c = ExpandIsoTopicMods(paramSet.IsotopicMods, ref c);
                    }

                    else if (tmpType.Name == "DynamicMods")
                    {
                        c = ExpandDynamicMods(paramSet.DynamicMods, ref c, DMSParamStorage.ParamTypes.DynamicModification);
                    }

                    else if (tmpType.Name == "TermDynamicMods")
                    {
                        c = ExpandDynamicMods(paramSet.TermDynamicMods, ref c, DMSParamStorage.ParamTypes.TermDynamicModification);
                    }

                    else if (tmpType.Name == "StaticMods")
                    {
                        c = ExpandStaticMods(paramSet.StaticModificationsList, ref c);
                    }

                    else if (m_BasicParams.Contains(tmpName))
                    {
                        tmpValue = pProp.GetValue(paramSet, null).ToString();
                        c.Add(tmpName, tmpValue.ToString(), DMSParamStorage.ParamTypes.BasicParam);
                    }
                    else if (m_AdvancedParams.Contains(tmpName))
                    {
                        tmpValue = pProp.GetValue(paramSet, null).ToString();
                        c.Add(tmpName, tmpValue.ToString(), DMSParamStorage.ParamTypes.AdvancedParam);
                    }
                }
            }

            return c;
        }

        private Params UpdateParamSetFromDataCollection(DMSParamStorage dc)
        {
            var p = new Params();

            // p = MainProcess.BaseLineParamSet
            p.LoadTemplate(MainProcess.TemplateFileName);
            var pType = typeof(Params);
            var pFields = pType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo pField = null;

            var ionType = typeof(IonSeries);
            var ionFields = ionType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (DMSParamStorage.ParamsEntry paramEntry in dc)
            {
                var tmpSpec = paramEntry.Specifier;
                var tmpValue = paramEntry.Value;
                var tmpType = paramEntry.Type;

                double valueDouble = 0;
                var valueInteger = 0;
                if (double.TryParse(tmpValue.Trim(), out var tmpValueDouble))
                {
                    valueDouble = tmpValueDouble;
                    valueInteger = (int)Math.Round(valueDouble);
                }

                var valueBool = false;
                if (bool.TryParse(tmpValue.Trim(), out var tmpValueBool))
                {
                    valueBool = tmpValueBool;
                }

                if (tmpType == DMSParamStorage.ParamTypes.BasicParam &&
                    m_BasicParams.Contains(tmpSpec))
                {
                    foreach (var currentPField in pFields)
                    {
                        pField = currentPField;
                        if ((pField.Name ?? "") == (tmpSpec ?? ""))
                        {
                            var tmpTypeName = pField.PropertyType.Name;
                            if (tmpTypeName == "Int32")
                            {
                                pField.SetValue(p, valueInteger, null);
                                break;
                            }
                            else if (tmpTypeName == "Single")
                            {
                                pField.SetValue(p, (float)valueDouble, null);
                                break;
                            }
                            else if (tmpTypeName == "String")
                            {
                                pField.SetValue(p, tmpValue, null);
                                break;
                            }
                            else if (tmpTypeName == "Boolean")
                            {
                                pField.SetValue(p, valueBool, null);
                                break;
                            }
                            else if (tmpTypeName == "MassTypeList")
                            {
                                pField.SetValue(p, (IBasicParams.MassTypeList)Enum.Parse(typeof(IBasicParams.MassTypeList), tmpValue, true), null);
                                break;
                            }
                            else if (tmpTypeName == "MassUnitList")
                            {
                                pField.SetValue(p, (IAdvancedParams.MassUnitList)Enum.Parse(typeof(IAdvancedParams.MassUnitList), tmpValue, true), null);
                            }
                            else
                            {
                                Console.WriteLine(pField.PropertyType.Name);
                            }

                            break;
                        }
                    }
                }

                else if (tmpType == DMSParamStorage.ParamTypes.AdvancedParam &&
                         m_AdvancedParams.Contains(tmpSpec))
                {
                    foreach (var currentPField1 in pFields)
                    {
                        pField = currentPField1;
                        if ((pField.Name ?? "") == (tmpSpec ?? ""))
                        {
                            var tmpTypeName = pField.PropertyType.Name;
                            if (tmpTypeName == "Int32")
                            {
                                pField.SetValue(p, valueInteger, null);
                                break;
                            }
                            else if (tmpTypeName == "Single")
                            {
                                pField.SetValue(p, (float)valueDouble, null);
                                break;
                            }
                            else if (tmpTypeName == "String")
                            {
                                pField.SetValue(p, tmpValue, null);
                                break;
                            }
                            else if (tmpTypeName == "Boolean")
                            {
                                pField.SetValue(p, valueBool, null);
                                break;
                            }
                            else
                            {
                                Console.WriteLine(pField.PropertyType.Name);
                            }

                            break;
                        }
                    }
                }

                else if (tmpType == DMSParamStorage.ParamTypes.AdvancedParam &&
                         m_IonSeriesParams.Contains(tmpSpec))
                {
                    foreach (var ionField in ionFields)
                    {
                        if ((ionField.Name ?? "") == (tmpSpec ?? ""))
                        {
                            var tmpTypeName = ionField.PropertyType.Name;
                            if (tmpTypeName == "Int32")
                            {
                                ionField.SetValue(p.IonSeries, valueInteger, null);
                                break;
                            }
                            else if (tmpTypeName == "Single")
                            {
                                ionField.SetValue(p.IonSeries, (float)valueDouble, null);
                                break;
                            }
                            else if (tmpTypeName == "String")
                            {
                                ionField.SetValue(p.IonSeries, tmpValue, null);
                                break;
                            }
                            else if (tmpTypeName == "Boolean")
                            {
                                ionField.SetValue(p.IonSeries, valueBool, null);
                                break;
                            }
                            else
                            {
                                Console.WriteLine(pField.PropertyType.Name);
                            }
                        }
                    }
                }

                else if (tmpType == DMSParamStorage.ParamTypes.DynamicModification)
                {
                    p.DynamicMods.Add(tmpSpec, valueDouble);
                }
                else if (tmpType == DMSParamStorage.ParamTypes.StaticModification)
                {
                    p.StaticModificationsList.Add(tmpSpec, valueDouble);
                }
                else if (tmpType == DMSParamStorage.ParamTypes.IsotopicModification)
                {
                    p.IsotopicMods.Add(tmpSpec, valueDouble);
                }
                else if (tmpType == DMSParamStorage.ParamTypes.TermDynamicModification)
                {
                    p.TermDynamicMods.Add(tmpSpec, valueDouble);
                }
            }

            return p;
        }

        private DMSParamStorage ExpandDynamicMods(DynamicMods DynModSet, ref DMSParamStorage ParamCollection, DMSParamStorage.ParamTypes eDynModType)
        {
            int maxCount = DynModSet.Count;
            int counter;
            string tmpName;
            string tmpValue;

            if (maxCount == 0)
            {
                return ParamCollection;
            }

            if (eDynModType != DMSParamStorage.ParamTypes.DynamicModification &&
                eDynModType != DMSParamStorage.ParamTypes.TermDynamicModification)
            {
                // This is unexpected; force eDynModType to be .DynamicModification
                eDynModType = DMSParamStorage.ParamTypes.DynamicModification;
            }

            for (counter = 1; counter < maxCount; counter++)
            {
                tmpName = DynModSet.Dyn_Mod_n_AAList(counter);
                tmpValue = DynModSet.Dyn_Mod_n_MassDiff(counter).ToString("0.00000");
                ParamCollection.Add(tmpName, tmpValue, eDynModType);
            }

            return ParamCollection;
        }

        private DMSParamStorage ExpandStaticMods(StaticMods StatModSet, ref DMSParamStorage ParamCollection)
        {
            int maxCount = StatModSet.Count;
            int counter;
            string tmpName;
            string tmpValue;

            if (maxCount == 0)
            {
                return ParamCollection;
            }

            for (counter = 0; counter < maxCount; counter++)
            {
                tmpName = StatModSet.GetResidue(counter);
                tmpValue = StatModSet.GetMassDiff(counter);
                ParamCollection.Add(tmpName, tmpValue, DMSParamStorage.ParamTypes.StaticModification);
            }

            return ParamCollection;
        }

        private DMSParamStorage ExpandIsoTopicMods(IsoMods IsoModSet, ref DMSParamStorage ParamCollection)
        {
            int maxCount = IsoModSet.Count;
            int counter;
            string tmpName;
            string tmpValue;

            if (maxCount == 0)
            {
                return ParamCollection;
            }

            for (counter = 0; counter < maxCount; counter++)
            {
                tmpName = IsoModSet.GetAtom(counter);
                tmpValue = IsoModSet.GetMassDiff(counter);
                ParamCollection.Add(tmpName, tmpValue, DMSParamStorage.ParamTypes.IsotopicModification);
            }

            return ParamCollection;
        }

        private DMSParamStorage ExpandIonSeries(IonSeries IonSeriesSet, ref DMSParamStorage ParamCollection)
        {
            var pType = typeof(IonSeries);
            var pFields = pType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var pField in pFields)
            {
                string tmpName = pField.Name;
                string tmpValue = pField.GetValue(IonSeriesSet, null).ToString();
                ParamCollection.Add(tmpName, tmpValue, DMSParamStorage.ParamTypes.AdvancedParam);
            }

            return ParamCollection;
        }

        private bool DoesParamSetNameExist(string paramSetName)
        {
            eParamFileTypeConstants eParamFileType;
            eParamFileType = GetTypeWithName(paramSetName);

            if (eParamFileType == eParamFileTypeConstants.Unknown)
            {
                Console.WriteLine("Parameter file " + paramSetName + "  was not found in table " + Param_File_Table);
                return false;
            }

            if (eParamFileType != eParamFileTypeConstants.Sequest)
            {
                // This param file type is not supported for export from DMS
                string paramFileTypeName = Enum.GetName(typeof(eParamFileTypeConstants), eParamFileType);
                if (string.IsNullOrEmpty(paramFileTypeName))
                {
                    paramFileTypeName = "Unknown";
                }

                Console.WriteLine("Parameter file " + paramSetName + " is of type " + paramFileTypeName + ", which isn't supported for export from DMS");
                return false;
            }

            int tmpID = GetIDWithName(paramSetName, eParamFileType);
            if (tmpID < 0)
            {
                Console.WriteLine("Parameter file " + paramSetName + " with type ID " + eParamFileType.ToString() + " was not found in table " + Param_File_Table);
                return false;
            }

            return true;
        }

        private bool DoesParamSetIDExist(int paramSetID)
        {
            DataRow matchingRow = null;
            return GetParamFileRowByID(paramSetID, out matchingRow);
        }

        protected string CompareParamSets(ref Params templateSet, ref Params checkSet)
        {
            var diffCollection = GetDiffColl(ref templateSet, ref checkSet);
            return SummarizeDiffColl(ref diffCollection);
        }

        protected DMSParamStorage GetDiffColl(ref Params templateSet, ref Params checkSet)
        {
            var templateColl = WriteDataCollectionFromParamSet(templateSet);
            var checkColl = WriteDataCollectionFromParamSet(checkSet);

            var diffCollection = CompareDataCollections(templateColl, checkColl);
            return diffCollection;
        }

        private string SummarizeDiffColl(ref DMSParamStorage diffColl)
        {
            int index;
            string tmpString;

            string tmpIsoMods = "";
            string tmpOtherParams = "";

            Queue tmpDynModsList = null;
            Queue tmpTermDynModsList = null;
            Queue tmpStatModsList = null;
            Queue tmpIsoModsList = null;
            Queue tmpOtherParamsList = null;

            int intDynModCount = 0;
            int intTermDynModCount = 0;

            for (index = 0; index < diffColl.Count; index++)
            {
                var tmpType = diffColl[index].Type;
                string tmpSpec = diffColl[index].Specifier;
                string tmpValue = diffColl[index].Value;

                double dblValue;
                string tmpValueFormatted;
                string tmpSign;

                if (double.TryParse(tmpValue, out dblValue))
                {
                    tmpValueFormatted = dblValue.ToString("0.0000");
                    if (dblValue > 0d)
                    {
                        tmpSign = "+";
                    }
                    else
                    {
                        tmpSign = "";
                    }
                }
                else
                {
                    tmpValueFormatted = string.Copy(tmpValue);
                    tmpSign = "";
                }

                if (tmpType == DMSParamStorage.ParamTypes.StaticModification)
                {
                    if (tmpStatModsList is null)
                    {
                        tmpStatModsList = new Queue();
                        tmpStatModsList.Enqueue("Static Mods: ");
                    }
                    tmpStatModsList.Enqueue(tmpSpec + " (" + tmpSign + tmpValueFormatted + ")");
                }

                else if (tmpType == DMSParamStorage.ParamTypes.DynamicModification)
                {
                    if (tmpDynModsList is null)
                    {
                        tmpDynModsList = new Queue();
                        tmpDynModsList.Enqueue("Dynamic Mods: ");
                    }
                    tmpDynModsList.Enqueue(tmpSpec + " (" + tmpSign + tmpValueFormatted + ")");

                    intDynModCount += 1;
                }

                else if (tmpType == DMSParamStorage.ParamTypes.TermDynamicModification)
                {
                    if (tmpSpec == "<")
                    {
                        tmpSpec = "N-Term Peptide";
                    }
                    else if (tmpSpec == ">")
                    {
                        tmpSpec = "C-Term Peptide";
                    }

                    if (tmpTermDynModsList is null)
                    {
                        tmpTermDynModsList = new Queue();
                        tmpTermDynModsList.Enqueue("PepTerm Dynamic Mods: ");
                    }
                    tmpTermDynModsList.Enqueue(tmpSpec + " (" + tmpSign + tmpValueFormatted + ")");

                    intTermDynModCount += 1;
                }

                else if (tmpType == DMSParamStorage.ParamTypes.IsotopicModification)
                {
                    if (string.IsNullOrEmpty(tmpIsoMods))
                    {
                        tmpIsoMods = "Isotopic Mods: ";
                    }

                    if (tmpIsoModsList is null)
                    {
                        tmpIsoModsList = new Queue();
                        tmpIsoModsList.Enqueue(tmpIsoMods);
                    }
                    tmpIsoModsList.Enqueue(tmpSpec + " (" + tmpSign + tmpValueFormatted + ")");
                }

                else
                {
                    if (string.IsNullOrEmpty(tmpOtherParams))
                    {
                        tmpOtherParams = "Other Parameters: ";
                    }

                    if (tmpOtherParamsList is null)
                    {
                        tmpOtherParamsList = new Queue();
                        tmpOtherParamsList.Enqueue(tmpOtherParams);
                    }
                    tmpOtherParamsList.Enqueue(tmpSpec + " = " + tmpValue);
                }
            }

            // Build the string describing the mods
            tmpString = "";

            tmpString = MakeListOfMods(tmpString, tmpDynModsList, true);

            tmpString = MakeListOfMods(tmpString, tmpTermDynModsList, false);
            if (intDynModCount == 0 && intTermDynModCount > 0)
            {
                tmpString = "Dynamic Mods: " + tmpString;
            }

            tmpString = MakeListOfMods(tmpString, tmpStatModsList, true);
            tmpString = MakeListOfMods(tmpString, tmpIsoModsList, true);
            tmpString = MakeListOfMods(tmpString, tmpOtherParamsList, true);

            if (string.IsNullOrEmpty(tmpString) || tmpString.Length == 0)
            {
                tmpString = " --No Change-- ";
            }

            return tmpString;
        }

        private string MakeListOfMods(
            string strModDescriptionPrevious,
            Queue objModList,
            bool blnAddTitlePrefix)
        {
            if (strModDescriptionPrevious is null)
                strModDescriptionPrevious = "";
            if (objModList is null)
            {
                return strModDescriptionPrevious;
            }

            if (objModList.Count > 0)
            {
                if (strModDescriptionPrevious.Length > 0)
                    strModDescriptionPrevious += ", ";

                string tmpElement = "";
                string elementTitle = objModList.Dequeue().ToString();
                while (objModList.Count > 0)
                {
                    string subItem = objModList.Dequeue().ToString();
                    if (tmpElement.Length > 0)
                        tmpElement += ", ";
                    tmpElement += subItem;
                }

                if (blnAddTitlePrefix)
                {
                    strModDescriptionPrevious += elementTitle + tmpElement;
                }
                else
                {
                    strModDescriptionPrevious += tmpElement;
                }
            }

            return strModDescriptionPrevious;
        }

        private DMSParamStorage CompareDataCollections(DMSParamStorage templateColl, DMSParamStorage checkColl)
        {
            var diffColl = new DMSParamStorage();

            for (int checkIndex = 0; checkIndex < checkColl.Count; checkIndex++)
            {
                var tmpCType = checkColl[checkIndex].Type;
                string tmpCSpec = checkColl[checkIndex].Specifier;
                string tmpCVal = checkColl[checkIndex].Value;

                int templateIndex = templateColl.IndexOf(tmpCSpec, tmpCType);

                if (templateIndex >= 0)
                {
                    var tmpTType = templateColl[templateIndex].Type;
                    string tmpTSpec = templateColl[templateIndex].Specifier;
                    string tmpTVal = templateColl[templateIndex].Value;
                    //var tmpTemp = tmpTType.ToString() + " - " + tmpTSpec + " = " + tmpTVal
                    //var tmpCheck = tmpCType.ToString() + " - " + tmpCSpec + " = " + tmpCVal

                    if ((tmpTType.ToString() + tmpTSpec ?? "") == (tmpCType.ToString() + tmpCSpec ?? ""))
                    {
                        if (tmpTVal.Equals(tmpCVal))
                        {
                        }

                        else
                        {
                            diffColl.Add(tmpCSpec, tmpCVal, tmpTType);
                        }
                    }
                }
                else
                {
                    diffColl.Add(tmpCSpec, tmpCVal, tmpCType);
                }
            }

            return diffColl;
        }
    }
}
