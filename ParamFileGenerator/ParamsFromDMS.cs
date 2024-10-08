﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using ParamFileGenerator.Modifications;
using ParamFileGenerator.Parameters;
using PRISMDatabaseUtils;

namespace ParamFileGenerator
{
    public class ParamsFromDMS : DBTask
    {
        // Ignore Spelling: diffs, mc

        private const string Param_File_Table = "T_Param_Files";
        private const string Param_File_Type_Table = "T_Param_File_Types";
        private const string Param_Entry_Table = "T_Param_Entries";

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
            MaxQuant = 1034,
            DiaNN = 1035,
            FragPipe = 1036
        }

        private int mID;
        private string mName;
        private eParamFileTypeConstants mParamFileType;
        private Params mParams;

        /// <summary>
        /// Parameter file table
        /// </summary>
        private DataTable mParamFileTable;
        private DataTable mParamEntryTable;

        private readonly Params mBaseLineParamSet;
        private readonly List<string> mAcceptableParams;
        private readonly List<string> mBasicParams;
        private readonly List<string> mAdvancedParams;
        private readonly List<string> mIonSeriesParams;
        private DataTable mMassMods;

        public bool ParamFileTableLoaded => mParamFileTable?.Rows.Count > 0;

#pragma warning disable CS3001 // Type of parameter is not CLS-compliant
        public ParamsFromDMS(IDBTools dbTools) : base(dbTools)
#pragma warning restore CS3001 // Type of parameter is not CLS-compliant
        {
            mAcceptableParams = LoadAcceptableParamList();
            mBasicParams = LoadBasicParams();
            mAdvancedParams = LoadAdvancedParams();
            mIonSeriesParams = LoadIonSeriesParams();
            mBaseLineParamSet = MainProcess.BaseLineParamSet;

            var success = GetParamsFromDMS();

            if (!success)
            {
                throw new Exception("Unable to obtain data from " + Param_File_Table + " and/or " + Param_Entry_Table);
            }
        }

        /// <summary>
        /// Refresh parameter file names from DMS
        /// </summary>
        /// <remarks>Used by the GUI editor</remarks>
        // ReSharper disable once UnusedMember.Global
        public void RefreshParamsFromDMS()
        {
            GetParamsFromDMS();
        }

        /// <summary>
        /// Retrieve parameter file details from DMS (SEQUEST only)
        /// </summary>
        /// <param name="paramSetName"></param>
        /// <returns>SEQUEST parameters</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public Params ReadParamsFromDMS(string paramSetName)
        {
            // Retrieve ID number first, then use the same procedure as below
            mName = paramSetName;

            mParamFileType = GetTypeWithName(paramSetName);

            if (mParamFileType == eParamFileTypeConstants.Unknown)
            {
                throw new Exception("Parameter file " + paramSetName + " was not found in table " + Param_File_Table);
            }

            if (mParamFileType != eParamFileTypeConstants.Sequest)
            {
                // This param file type is not supported for export from DMS
                var paramFileTypeName = Enum.GetName(typeof(eParamFileTypeConstants), mParamFileType);

                if (string.IsNullOrEmpty(paramFileTypeName))
                {
                    paramFileTypeName = "Unknown";
                }

                throw new NotSupportedException("Parameter file " + paramSetName + " is of type " + paramFileTypeName + ", which isn't support for export from DMS");
            }

            mID = GetIDWithName(mName, mParamFileType);

            mParams = RetrieveParams(mID, mParamFileType);
            return mParams;
        }

        /// <summary>
        /// Retrieve parameter file details from DMS (SEQUEST only)
        /// </summary>
        /// <param name="paramSetID"></param>
        /// <remarks>Used by the GUI editor</remarks>
        /// <returns>SEQUEST parameters</returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="NotSupportedException"></exception>
        // ReSharper disable once UnusedMember.Global
        public Params ReadParamsFromDMS(int paramSetID)
        {
            mID = paramSetID;
            mParamFileType = GetTypeWithID(mID);

            if (mParamFileType == eParamFileTypeConstants.Unknown)
            {
                throw new Exception("Parameter file ID " + paramSetID + " was not found in table " + Param_File_Table);
            }

            if (mParamFileType != eParamFileTypeConstants.Sequest)
            {
                // This param file type is not supported for export from DMS
                throw new NotSupportedException("Parameter file ID " + paramSetID + " is of type " + Enum.GetName(typeof(eParamFileTypeConstants), mParamFileType) + ", which isn't support for export from DMS");
            }

            mParams = RetrieveParams(mID, mParamFileType);
            return mParams;
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

        /// <summary>
        /// Check whether the given parameter file exists (SEQUEST only)
        /// </summary>
        /// <param name="paramSetID"></param>
        /// <remarks>Used by the GUI editor</remarks>
        // ReSharper disable once UnusedMember.Global
        public bool ParamSetIDExists(int paramSetID)
        {
            return DoesParamSetIDExist(paramSetID);
        }

        /// <summary>
        /// Get parameter file ID from the parameter file name (SEQUEST only)
        /// </summary>
        /// <param name="name"></param>
        /// <remarks>Used by the GUI editor</remarks>
        // ReSharper disable once UnusedMember.Global
        public int GetParamSetIDFromName(string name)
        {
            var eParamFileType = GetTypeWithName(name);

            if (eParamFileType != eParamFileTypeConstants.Unknown)
                return GetIDWithName(name, eParamFileType);

            Console.WriteLine("Parameter file " + name + "  was not found in table " + Param_File_Table);
            return -1;
        }

        private List<string> LoadAcceptableParamList()
        {
            var paramEnum = Enum.GetNames(typeof(AcceptableParams));

            return paramEnum.ToList();
        }

        private List<string> LoadBasicParams()
        {
            var paramEnum = Enum.GetNames(typeof(BasicParams));

            return paramEnum.ToList();
        }

        private List<string> LoadAdvancedParams()
        {
            var paramEnum = Enum.GetNames(typeof(AdvancedParams));

            return paramEnum.ToList();
        }

        private List<string> LoadIonSeriesParams()
        {
            var paramEnum = Enum.GetNames(typeof(IonSeriesParams));

            return paramEnum.ToList();
        }

        private bool GetParamsFromDMS()
        {
            // SQL to grab param file table
            // The ID column is named param_file_id
            const string query1 = "SELECT param_file_id, param_file_name, param_file_description, param_file_type_id " +
                                  "FROM " + Param_File_Table;
            // Optional: " WHERE param_file_type_id = 1000"

            mParamFileTable = GetTable(query1);

            // SQL to grab param entry table
            // The ID column is named param_entry_id
            const string query2 = "SELECT param_entry_id, entry_sequence_order, entry_type, entry_specifier, entry_value, param_file_id " +
                                  "FROM " + Param_Entry_Table + " " +
                                  "WHERE entry_type not like '%Modification'";

            mParamEntryTable = GetTable(query2);

            return true;
        }

        private Params RetrieveParams(int paramSetID, eParamFileTypeConstants eParamFileType)
        {
            return GetParamSetWithID(paramSetID, eParamFileType);
        }

        // TODO Fix this function for new mod handling
        protected Params GetParamSetWithID(int paramSetID, eParamFileTypeConstants eParamFileType, bool disableMassLookup = false)
        {
            if (!GetParamFileRowByID(paramSetID, out var matchingRow))
            {
                // Match not found
                return new Params();
            }

            var foundRows = mParamEntryTable.Select("param_file_id = " + paramSetID, "entry_sequence_order");

            var storageSet = MakeStorageClassFromTableRowSet(foundRows);

            if (!disableMassLookup)
            {
                storageSet.AddRange(GetMassModsFromDMS(paramSetID, eParamFileType));
            }

            var p = UpdateParamSetFromDataCollection(storageSet);
            p.FileName = (string)matchingRow["param_file_name"];
            p.Description = SummarizeDiffColl(storageSet);

            foreach (var paramRow in foundRows)
            {
                p.AddLoadedParamName(paramRow["entry_specifier"].ToString(), paramRow["entry_value"].ToString());
            }

            return p;
        }

        private List<ParamsEntry> MakeStorageClassFromTableRowSet(IEnumerable<DataRow> foundRows)
        {
            var storageClass = new List<ParamsEntry>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var foundRow in foundRows)
            {
                var param = new ParamsEntry(
                    (string)foundRow["entry_specifier"],
                    (string)foundRow["entry_value"],
                    (ParamTypes)Enum.Parse(typeof(ParamTypes), foundRow["entry_type"].ToString()));

                storageClass.Add(param);
            }

            return storageClass;
        }

        private IEnumerable<ParamsEntry> GetMassModsFromDMS(int paramSetID, eParamFileTypeConstants paramFileType)
        {
            const int MaxDynMods = 15;
            var paramList = new List<ParamsEntry>();

            var sql = "SELECT mod_type_symbol, residue_symbol, monoisotopic_mass, local_symbol_id, affected_atom " +
                      "FROM V_Param_File_Mass_Mod_Info " +
                      "WHERE param_file_id = " + paramSetID;

            mMassMods = GetTable(sql);

            // Look for Dynamic mods

            var lstLocalSymbolIDs = new List<int>();

            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (paramFileType)
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

            for (var symbolID = 1; symbolID <= MaxDynMods; symbolID++)
            {
                if (!lstLocalSymbolIDs.Contains(symbolID))
                {
                    lstLocalSymbolIDs.Add(symbolID);
                }
            }

            // Add the dynamic mods (but not N-terminal or C-terminal dynamic mods)
            foreach (var symbolID in lstLocalSymbolIDs)
            {
                var dynamicMods = mMassMods.Select("mod_type_symbol = 'D' AND local_symbol_id = " + symbolID + " AND residue_symbol <> '<' AND residue_symbol <> '>'", "local_symbol_id");

                if (dynamicMods.Length == 0)
                    continue;

                var param = new ParamsEntry(
                    GetDynModSpecifier(dynamicMods),
                    dynamicMods[0]["monoisotopic_mass"].ToString(),
                    ParamTypes.DynamicModification);

                paramList.Add(param);
            }

            // Find N-Term Dynamic Mods
            var nTermDynamicMods = mMassMods.Select("mod_type_symbol = 'D' AND residue_symbol = '<'");

            if (nTermDynamicMods.Length > 0)
            {
                var param = new ParamsEntry(
                    GetDynModSpecifier(nTermDynamicMods),
                    nTermDynamicMods[0]["monoisotopic_mass"].ToString(),
                    ParamTypes.TermDynamicModification);

                paramList.Add(param);
            }

            // Find C-Term Dynamic Mods
            var cTermDynamicMods = mMassMods.Select("mod_type_symbol = 'D' AND residue_symbol = '>'");

            if (cTermDynamicMods.Length > 0)
            {
                var param = new ParamsEntry(
                    GetDynModSpecifier(cTermDynamicMods),
                    cTermDynamicMods[0]["monoisotopic_mass"].ToString(),
                    ParamTypes.TermDynamicModification);
                paramList.Add(param);
            }

            // Look for static and terminal mods

            foreach (var staticMod in mMassMods.Select("mod_type_symbol = 'S' OR mod_type_symbol = 'P' or mod_type_symbol = 'T'"))
            {
                var aminoAcidSymbol = staticMod["residue_symbol"].ToString();

                var affectedAminoAcid = aminoAcidSymbol switch
                {
                    "<" => "N_Term_Peptide",
                    ">" => "C_Term_Peptide",
                    "[" => "N_Term_Protein",
                    "]" => "C_Term_Protein",
                    _ => aminoAcidSymbol
                };

                var param = new ParamsEntry(
                    affectedAminoAcid,
                    staticMod["monoisotopic_mass"].ToString(),
                    ParamTypes.StaticModification);

                paramList.Add(param);
            }

            // Old To Do ... still need code to handle import/export of isotopic mods

            // Look for isotopic mods

            foreach (var isotopicMod in mMassMods.Select("mod_type_symbol = 'I'"))
            {
                var param = new ParamsEntry(
                    isotopicMod["affected_atom"].ToString(),
                    isotopicMod["monoisotopic_mass"].ToString(),
                    ParamTypes.IsotopicModification);

                paramList.Add(param);
            }

            return paramList;
        }

        private string GetDynModSpecifier(IReadOnlyCollection<DataRow> rowSet)
        {
            var affectedAminoAcids = new StringBuilder();

            if (rowSet.Count == 0)
                return null;

            // We have dynamic mods
            foreach (var foundRow in rowSet)
            {
                affectedAminoAcids.Append(foundRow["residue_symbol"]);
            }

            return affectedAminoAcids.ToString();
        }

        private int GetIDWithName(string name, eParamFileTypeConstants eParamFileType)
        {
            var foundRows = mParamFileTable.Select("param_file_name = '" + name + "' AND param_file_type_id = " + (int)eParamFileType);

            if (foundRows.Length == 0)
            {
                return -1;
            }

            var foundRow = foundRows[0];
            return Params.SafeCastInt(foundRow["param_file_id"]);
        }

        private eParamFileTypeConstants GetTypeWithID(int paramFileID)
        {
            var foundRows = mParamFileTable.Select("param_file_id = " + paramFileID);

            if (foundRows.Length > 0)
            {
                var foundRow = foundRows[0];
                return (eParamFileTypeConstants)Params.SafeCastInt(foundRow["param_file_type_id"]);
            }

            return eParamFileTypeConstants.None;
        }

        private eParamFileTypeConstants GetTypeWithName(string paramFileName)
        {
            var foundRows = mParamFileTable.Select("param_file_name = '" + paramFileName + "'");

            if (foundRows.Length > 0)
            {
                var foundRow = foundRows[0];
                return (eParamFileTypeConstants)Params.SafeCastInt(foundRow["param_file_type_id"]);
            }

            return eParamFileTypeConstants.None;
        }

        [Obsolete("Unused")]
        private string GetDescriptionWithID(int paramFileID)
        {
            if (!GetParamFileRowByID(paramFileID, out var matchingRow))
                return string.Empty;

            var paramFileDescription = matchingRow["param_file_description"].ToString();

            return string.IsNullOrWhiteSpace(paramFileDescription) ? string.Empty : paramFileDescription;
        }

        /// <summary>
        /// Finds the row in m_ParamFileTable with the given parameter file ID
        /// </summary>
        /// <param name="paramFileID"></param>
        /// <param name="matchingRow">The row if found, otherwise null</param>
        /// <returns>True if the parameter file was found, otherwise false</returns>
        private bool GetParamFileRowByID(int paramFileID, out DataRow matchingRow)
        {
            var foundRows = mParamFileTable.Select("param_file_id = " + paramFileID);

            if (foundRows.Length > 0)
            {
                matchingRow = foundRows[0];
                return true;
            }

            matchingRow = null;
            return false;
        }

        /// <summary>
        /// Finds parameter file info for MS-GF+, MSPathFinder, TopPIC, FragPipe, MSFragger, MaxQuant, DIA-NN, etc.
        /// </summary>
        /// <returns>Data table</returns>
        private DataTable GetAvailableParamSets()
        {
            var paramTableSQL =
                "SELECT " +
                "  param_file_id AS id, " +
                "  param_file_name AS filename, " +
                "  param_file_description AS diffs, " +
                "  param_file_type_id AS type_id " +
                "FROM " + Param_File_Table + " " +
                "WHERE param_file_type_id = " + (int)eParamFileTypeConstants.Sequest +
                "   or param_file_type_id = " + (int)eParamFileTypeConstants.XTandem +
                "   or param_file_type_id = " + (int)eParamFileTypeConstants.MSGFDB +
                "   or param_file_type_id = " + (int)eParamFileTypeConstants.MSPathFinder +
                "   or param_file_type_id = " + (int)eParamFileTypeConstants.MODPlus +
                "   or param_file_type_id = " + (int)eParamFileTypeConstants.TopPIC +
                "   or param_file_type_id = " + (int)eParamFileTypeConstants.FragPipe +
                "   or param_file_type_id = " + (int)eParamFileTypeConstants.MSFragger +
                "   or param_file_type_id = " + (int)eParamFileTypeConstants.MaxQuant +
                "   or param_file_type_id = " + (int)eParamFileTypeConstants.DiaNN;

            var resultSet = GetTable(paramTableSQL);

            // Load resultSet
            foreach (DataRow dr in resultSet.Rows)
            {
                var typeID = (int)dr["type_id"];

                if (typeID != (int)eParamFileTypeConstants.Sequest)
                    continue;

                var paramSetID = (int)dr["id"];

                if ((string)dr["diffs"] is not null)
                    continue;

                var paramFileTypeID = (eParamFileTypeConstants)typeID;

                dr["diffs"] = DistillFeaturesFromParamSet(paramSetID, paramFileTypeID);
                dr.AcceptChanges();
            }

            return resultSet;

            // Need filtering code for resultSet here...
        }

        private DataTable GetParamFileTypes()
        {
            const string tableTypesSQL = "SELECT param_file_type_id AS id, param_file_type AS type " +
                                         "FROM " + Param_File_Type_Table;

            return GetTable(tableTypesSQL);
        }

        /// <summary>
        /// Distill features from the parameter set
        /// </summary>
        /// <param name="paramSet"></param>
        /// <remarks>Used by the GUI editor</remarks>
        protected string DistillFeaturesFromParamSet(Params paramSet)
        {
            var templateColl = WriteDataCollectionFromParamSet(mBaseLineParamSet);
            var checkColl = WriteDataCollectionFromParamSet(paramSet);

            var diffColl = CompareDataCollections(templateColl, checkColl);
            return SummarizeDiffColl(diffColl);
        }

        protected string DistillFeaturesFromParamSet(int paramSetID, eParamFileTypeConstants eParamFileTypeID)
        {
            var p = GetParamSetWithID(paramSetID, eParamFileTypeID);

            return p.Description;
        }

        private List<ParamsEntry> WriteDataCollectionFromParamSet(Params paramSet)
        {
            var c = new List<ParamsEntry>();

            var pType = paramSet.GetType();

            foreach (var pProp in pType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propertyName = pProp.Name;
                var propertyType = pProp.PropertyType;

                if (!mAcceptableParams.Contains(propertyName))
                    continue;

                switch (propertyType.Name)
                {
                    case "IonSeries":
                        c.AddRange(ExpandIonSeries(paramSet.IonSeries));
                        break;

                    case "IsoMods":
                        c.AddRange(ExpandIsoTopicMods(paramSet.IsotopicModificationsList));
                        break;

                    case "DynamicMods":
                        c.AddRange(ExpandDynamicMods(paramSet.DynamicMods, ParamTypes.DynamicModification));
                        break;

                    case "TermDynamicMods":
                        c.AddRange(ExpandDynamicMods(paramSet.TermDynamicMods, ParamTypes.TermDynamicModification));
                        break;

                    case "StaticMods":
                        c.AddRange(ExpandStaticMods(paramSet.StaticModificationsList));
                        break;

                    default:
                        var value = pProp.GetValue(paramSet, null).ToString();

                        if (mBasicParams.Contains(propertyName))
                        {
                            c.Add(new ParamsEntry(propertyName, value, ParamTypes.BasicParam));
                        }
                        else if (mAdvancedParams.Contains(propertyName))
                        {
                            c.Add(new ParamsEntry(propertyName, value, ParamTypes.AdvancedParam));
                        }

                        break;
                }
            }

            return c;
        }

        private Params UpdateParamSetFromDataCollection(IEnumerable<ParamsEntry> dc)
        {
            var p = new Params();

            // p = MainProcess.BaseLineParamSet
            p.LoadTemplate(MainProcess.TemplateFileName);

            var pType = typeof(Params);
            var pFields = pType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo pField = null;

            var ionType = typeof(IonSeries);
            var ionFields = ionType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var paramEntry in dc)
            {
                var specifier = paramEntry.Specifier;
                var value = paramEntry.Value;
                var paramType = paramEntry.Type;

                double valueDouble = 0;
                var valueInteger = 0;

                if (double.TryParse(value.Trim(), out var parsedDouble))
                {
                    valueDouble = parsedDouble;
                    valueInteger = (int)Math.Round(valueDouble);
                }

                var valueBool = false;

                if (bool.TryParse(value.Trim(), out var parsedBoolean))
                {
                    valueBool = parsedBoolean;
                }

                if (paramType == ParamTypes.BasicParam &&
                    mBasicParams.Contains(specifier))
                {
                    foreach (var currentPField in pFields)
                    {
                        pField = currentPField;

                        if (pField.Name != (specifier ?? ""))
                            continue;

                        switch (pField.PropertyType.Name)
                        {
                            case "Int32":
                                pField.SetValue(p, valueInteger, null);
                                break;

                            case "Single":
                                pField.SetValue(p, (float)valueDouble, null);
                                break;

                            case "String":
                                pField.SetValue(p, value, null);
                                break;

                            case "Boolean":
                                pField.SetValue(p, valueBool, null);
                                break;

                            case "MassTypeList":
                                pField.SetValue(p, (IBasicParams.MassTypeList)Enum.Parse(typeof(IBasicParams.MassTypeList), value, true), null);
                                break;

                            case "MassUnitList":
                                pField.SetValue(p, (IAdvancedParams.MassUnitList)Enum.Parse(typeof(IAdvancedParams.MassUnitList), value, true), null);
                                break;

                            default:
                                Console.WriteLine(pField.PropertyType.Name);
                                break;
                        }

                        break;
                    }
                }
                else if (paramType == ParamTypes.AdvancedParam &&
                         mAdvancedParams.Contains(specifier))
                {
                    foreach (var currentPField1 in pFields)
                    {
                        pField = currentPField1;

                        if (pField.Name != (specifier ?? ""))
                            continue;

                        switch (pField.PropertyType.Name)
                        {
                            case "Int32":
                                pField.SetValue(p, valueInteger, null);
                                break;

                            case "Single":
                                pField.SetValue(p, (float)valueDouble, null);
                                break;

                            case "String":
                                pField.SetValue(p, value, null);
                                break;

                            case "Boolean":
                                pField.SetValue(p, valueBool, null);
                                break;

                            default:
                                Console.WriteLine(pField.PropertyType.Name);
                                break;
                        }

                        break;
                    }
                }
                else if (paramType == ParamTypes.AdvancedParam &&
                         mIonSeriesParams.Contains(specifier))
                {
                    foreach (var ionField in ionFields)
                    {
                        if (ionField.Name != (specifier ?? ""))
                            continue;

                        var typeName = ionField.PropertyType.Name;

                        if (typeName == "Int32")
                        {
                            ionField.SetValue(p.IonSeries, valueInteger, null);
                            break;
                        }

                        if (typeName == "Single")
                        {
                            ionField.SetValue(p.IonSeries, (float)valueDouble, null);
                            break;
                        }

                        if (typeName == "String")
                        {
                            ionField.SetValue(p.IonSeries, value, null);
                            break;
                        }

                        if (typeName == "Boolean")
                        {
                            ionField.SetValue(p.IonSeries, valueBool, null);
                            break;
                        }

                        Console.WriteLine(pField.PropertyType.Name);
                    }
                }
                else if (paramType == ParamTypes.DynamicModification)
                {
                    p.DynamicMods.Add(specifier, valueDouble);
                }
                else if (paramType == ParamTypes.StaticModification)
                {
                    p.StaticModificationsList.Add(specifier, valueDouble);
                }
                else if (paramType == ParamTypes.IsotopicModification)
                {
                    p.IsotopicModificationsList.Add(specifier, valueDouble);
                }
                else if (paramType == ParamTypes.TermDynamicModification)
                {
                    p.TermDynamicMods.Add(specifier, valueDouble);
                }
            }

            return p;
        }

        private IEnumerable<ParamsEntry> ExpandDynamicMods(DynamicMods dynModSet, ParamTypes dynModType)
        {
            var maxCount = dynModSet.Count;
            var paramList = new List<ParamsEntry>();

            if (maxCount == 0)
            {
                return paramList;
            }

            if (dynModType != ParamTypes.DynamicModification &&
                dynModType != ParamTypes.TermDynamicModification)
            {
                // This is unexpected; force eDynModType to be .DynamicModification
                dynModType = ParamTypes.DynamicModification;
            }

            for (var counter = 1; counter < maxCount; counter++)
            {
                var param = new ParamsEntry(
                    dynModSet.Dyn_Mod_n_AAList(counter),
                    dynModSet.Dyn_Mod_n_MassDiff(counter).ToString("0.00000"),
                    dynModType);

                paramList.Add(param);
            }

            return paramList;
        }

        private IEnumerable<ParamsEntry> ExpandStaticMods(StaticMods statModSet)
        {
            var maxCount = statModSet.Count;
            var paramList = new List<ParamsEntry>();

            if (maxCount == 0)
            {
                return paramList;
            }

            for (var counter = 0; counter < maxCount; counter++)
            {
                var param = new ParamsEntry(
                    statModSet.GetResidue(counter),
                    statModSet.GetMassDiff(counter),
                    ParamTypes.StaticModification);

                paramList.Add(param);
            }

            return paramList;
        }

        private IEnumerable<ParamsEntry> ExpandIsoTopicMods(IsoMods isoModSet)
        {
            var maxCount = isoModSet.Count;
            var paramList = new List<ParamsEntry>();

            if (maxCount == 0)
            {
                return paramList;
            }

            for (var counter = 0; counter < maxCount; counter++)
            {
                var param = new ParamsEntry(
                    isoModSet.GetAtom(counter),
                    isoModSet.GetMassDiff(counter),
                    ParamTypes.StaticModification);
                paramList.Add(param);
            }

            return paramList;
        }

        private IEnumerable<ParamsEntry> ExpandIonSeries(IonSeries ionSeriesSet)
        {
            var pType = typeof(IonSeries);
            var paramList = new List<ParamsEntry>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var pField in pType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var param = new ParamsEntry(
                    pField.Name,
                    pField.GetValue(ionSeriesSet, null).ToString(),
                    ParamTypes.AdvancedParam);
                paramList.Add(param);
            }

            return paramList;
        }

        private bool DoesParamSetNameExist(string paramSetName)
        {
            var eParamFileType = GetTypeWithName(paramSetName);

            if (eParamFileType == eParamFileTypeConstants.Unknown)
            {
                Console.WriteLine("Parameter file " + paramSetName + "  was not found in table " + Param_File_Table);
                return false;
            }

            if (eParamFileType != eParamFileTypeConstants.Sequest)
            {
                // This param file type is not supported for export from DMS
                var paramFileTypeName = Enum.GetName(typeof(eParamFileTypeConstants), eParamFileType);

                if (string.IsNullOrEmpty(paramFileTypeName))
                {
                    paramFileTypeName = "Unknown";
                }

                Console.WriteLine("Parameter file " + paramSetName + " is of type " + paramFileTypeName + ", which isn't supported for export from DMS");
                return false;
            }

            var paramFileID = GetIDWithName(paramSetName, eParamFileType);

            if (paramFileID < 0)
            {
                Console.WriteLine("Parameter file " + paramSetName + " with type ID " + eParamFileType + " was not found in table " + Param_File_Table);
                return false;
            }

            return true;
        }

        private bool DoesParamSetIDExist(int paramSetID)
        {
            return GetParamFileRowByID(paramSetID, out _);
        }

        /// <summary>
        /// Compare param sets
        /// </summary>
        /// <param name="templateSet"></param>
        /// <param name="checkSet"></param>
        /// <remarks>Used by the GUI editor</remarks>
        protected string CompareParamSets(Params templateSet, Params checkSet)
        {
            var diffCollection = GetModCollection(templateSet, checkSet);
            return SummarizeDiffColl(diffCollection);
        }

        protected List<ParamsEntry> GetModCollection(Params templateSet, Params checkSet)
        {
            var templateColl = WriteDataCollectionFromParamSet(templateSet);
            var checkColl = WriteDataCollectionFromParamSet(checkSet);

            return CompareDataCollections(templateColl, checkColl);
        }

        private string SummarizeDiffColl(IReadOnlyList<ParamsEntry> diffColl)
        {
            int index;

            var isoMods = "";
            var otherParams = "";

            Queue dynModsList = null;
            Queue termDynModsList = null;
            Queue statModsList = null;
            Queue isoModsList = null;
            Queue otherParamsList = null;

            var dynModCount = 0;
            var termDynModCount = 0;

            for (index = 0; index < diffColl.Count; index++)
            {
                var modType = diffColl[index].Type;
                var specifier = diffColl[index].Specifier;
                var value = diffColl[index].Value;

                string formattedValue;
                string sign;

                if (double.TryParse(value, out var dblValue))
                {
                    formattedValue = dblValue.ToString("0.0000");

                    sign = dblValue > 0d ? "+" : string.Empty;
                }
                else
                {
                    formattedValue = string.Copy(value);
                    sign = "";
                }

                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                switch (modType)
                {
                    case ParamTypes.StaticModification:
                        if (statModsList is null)
                        {
                            statModsList = new Queue();
                            statModsList.Enqueue("Static Mods: ");
                        }

                        statModsList.Enqueue(specifier + " (" + sign + formattedValue + ")");
                        break;

                    case ParamTypes.DynamicModification:
                        if (dynModsList is null)
                        {
                            dynModsList = new Queue();
                            dynModsList.Enqueue("Dynamic Mods: ");
                        }

                        dynModsList.Enqueue(specifier + " (" + sign + formattedValue + ")");

                        dynModCount++;
                        break;

                    case ParamTypes.TermDynamicModification:
                        var specToUse = specifier switch
                        {
                            "<" => "N-Term Peptide",
                            ">" => "C-Term Peptide",
                            _ => specifier
                        };

                        if (termDynModsList is null)
                        {
                            termDynModsList = new Queue();
                            termDynModsList.Enqueue("PepTerm Dynamic Mods: ");
                        }

                        termDynModsList.Enqueue(specToUse + " (" + sign + formattedValue + ")");

                        termDynModCount++;
                        break;

                    case ParamTypes.IsotopicModification:
                        if (string.IsNullOrEmpty(isoMods))
                        {
                            isoMods = "Isotopic Mods: ";
                        }

                        if (isoModsList is null)
                        {
                            isoModsList = new Queue();
                            isoModsList.Enqueue(isoMods);
                        }

                        isoModsList.Enqueue(specifier + " (" + sign + formattedValue + ")");
                        break;

                    default:
                        if (string.IsNullOrEmpty(otherParams))
                        {
                            otherParams = "Other Parameters: ";
                        }

                        if (otherParamsList is null)
                        {
                            otherParamsList = new Queue();
                            otherParamsList.Enqueue(otherParams);
                        }

                        otherParamsList.Enqueue(specifier + " = " + value);
                        break;
                }
            }

            // Build the string describing the mods
            var modDescription = "";

            modDescription = MakeListOfMods(modDescription, dynModsList, true);

            modDescription = MakeListOfMods(modDescription, termDynModsList, false);

            if (dynModCount == 0 && termDynModCount > 0)
            {
                modDescription = "Dynamic Mods: " + modDescription;
            }

            modDescription = MakeListOfMods(modDescription, statModsList, true);
            modDescription = MakeListOfMods(modDescription, isoModsList, true);
            modDescription = MakeListOfMods(modDescription, otherParamsList, true);

            if (string.IsNullOrEmpty(modDescription) || modDescription.Length == 0)
            {
                modDescription = " --No Change-- ";
            }

            return modDescription;
        }

        private string MakeListOfMods(string modDescriptionPrevious, Queue objModList, bool addTitlePrefix)
        {
            modDescriptionPrevious ??= "";

            if (objModList is null)
            {
                return modDescriptionPrevious;
            }

            if (objModList.Count <= 0)
                return modDescriptionPrevious;

            if (modDescriptionPrevious.Length > 0)
                modDescriptionPrevious += ", ";

            var element = "";
            var elementTitle = objModList.Dequeue().ToString();

            while (objModList.Count > 0)
            {
                var subItem = objModList.Dequeue().ToString();

                if (element.Length > 0)
                    element += ", ";
                element += subItem;
            }

            if (addTitlePrefix)
            {
                modDescriptionPrevious += elementTitle + element;
            }
            else
            {
                modDescriptionPrevious += element;
            }

            return modDescriptionPrevious;
        }

        private List<ParamsEntry> CompareDataCollections(IReadOnlyList<ParamsEntry> templateColl, IEnumerable<ParamsEntry> checkColl)
        {
            var diffColl = new List<ParamsEntry>();

            foreach (var check in checkColl)
            {
                var template = templateColl.FirstOrDefault(x => x.TypeSpecifierEquals(check));

                if (template is not null)
                {
                    if (!template.Value.Equals(check.Value))
                    {
                        diffColl.Add(check);
                    }
                }
                else
                {
                    diffColl.Add(check);
                }
            }

            return diffColl;
        }
    }
}
