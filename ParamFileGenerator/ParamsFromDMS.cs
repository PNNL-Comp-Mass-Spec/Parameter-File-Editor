﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using PRISMDatabaseUtils;

namespace ParamFileGenerator.DownloadParams
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
            MaxQuant = 1034
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

        public bool ParamFileTableLoaded => mParamFileTable is not null && mParamFileTable.Rows.Count > 0;

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

        public void RefreshParamsFromDMS()
        {
            GetParamsFromDMS();
        }

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

        public bool ParamSetIDExists(int paramSetID)
        {
            return DoesParamSetIDExist(paramSetID);
        }

        public int GetParamSetIDFromName(string name)
        {
            var eParamFileType = GetTypeWithName(name);

            if (eParamFileType == eParamFileTypeConstants.Unknown)
            {
                Console.WriteLine("Parameter file " + name + "  was not found in table " + Param_File_Table);
                return -1;
            }

            return GetIDWithName(name, eParamFileType);
        }

        private List<string> LoadAcceptableParamList()
        {
            var paramEnum = Enum.GetNames(typeof(AcceptableParams));
            var paramList = new List<string>();

            foreach (var param in paramEnum)
            {
                paramList.Add(param);
            }

            return paramList;
        }

        private List<string> LoadBasicParams()
        {
            var paramEnum = Enum.GetNames(typeof(BasicParams));
            var paramList = new List<string>();

            foreach (var param in paramEnum)
            {
                paramList.Add(param);
            }

            return paramList;
        }

        private List<string> LoadAdvancedParams()
        {
            var paramEnum = Enum.GetNames(typeof(AdvancedParams));
            var paramList = new List<string>();

            foreach (var param in paramEnum)
            {
                paramList.Add(param);
            }

            return paramList;
        }

        private List<string> LoadIonSeriesParams()
        {
            var paramEnum = Enum.GetNames(typeof(IonSeriesParams));
            var paramList = new List<string>();

            foreach (var param in paramEnum)
            {
                paramList.Add(param);
            }

            return paramList;
        }

        private bool GetParamsFromDMS()
        {
            // SQL to grab param file table
            // The ID column is named param_file_id
            var query1 = "SELECT param_file_id, param_file_name, param_file_description, param_file_type_id " +
                         "FROM " + Param_File_Table;
            // Optional: " WHERE param_file_type_id = 1000"

            mParamFileTable = GetTable(query1);

            // SQL to grab param entry table
            // The ID column is named param_entry_id
            var query2 = "SELECT param_entry_id, entry_sequence_order, entry_type, entry_specifier, entry_value, param_file_id " +
                         "FROM " + Param_Entry_Table + " " +
                         "WHERE entry_type not like '%Modification'";

            mParamEntryTable = GetTable(query2);

            return true;
        }

        private Params RetrieveParams(int paramSetID, eParamFileTypeConstants eParamFileType)
        {
            var p = GetParamSetWithID(paramSetID, eParamFileType);
            return p;
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

        private List<ParamsEntry> GetMassModsFromDMS(int paramSetID, eParamFileTypeConstants eParamFileType)
        {
            const int MaxDynMods = 15;
            var paramList = new List<ParamsEntry>();

            DataRow foundRow;
            DataRow[] foundRows;

            var sql = "SELECT mod_type_symbol, residue_symbol, monoisotopic_mass, local_symbol_id, affected_atom " +
                      "FROM V_Param_File_Mass_Mod_Info " +
                      "WHERE param_file_id = " + paramSetID;

            mMassMods = GetTable(sql);

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

            for (var intSymbolID = 1; intSymbolID <= MaxDynMods; intSymbolID++)
            {
                if (!lstLocalSymbolIDs.Contains(intSymbolID))
                {
                    lstLocalSymbolIDs.Add(intSymbolID);
                }
            }

            foreach (var intSymbolID in lstLocalSymbolIDs)
            {
                foundRows = mMassMods.Select("mod_type_symbol = 'D' AND local_symbol_id = " + intSymbolID + " AND residue_symbol <> '<' AND residue_symbol <> '>'", "local_symbol_id");
                if (foundRows.Length > 0)
                {
                    var param = new ParamsEntry(
                        GetDynModSpecifier(foundRows),
                        foundRows[0]["monoisotopic_mass"].ToString(),
                        ParamTypes.DynamicModification);
                    paramList.Add(param);
                }
            }

            // Find N-Term Dynamic Mods
            foundRows = mMassMods.Select("mod_type_symbol = 'D' AND residue_symbol = '<'");
            if (foundRows.Length > 0)
            {
                var param = new ParamsEntry(
                    GetDynModSpecifier(foundRows),
                    foundRows[0]["monoisotopic_mass"].ToString(),
                    ParamTypes.TermDynamicModification);
                paramList.Add(param);
            }

            // Find C-Term Dynamic Mods
            foundRows = mMassMods.Select("mod_type_symbol = 'D' AND residue_symbol = '>'");
            if (foundRows.Length > 0)
            {
                var param = new ParamsEntry(
                    GetDynModSpecifier(foundRows),
                    foundRows[0]["monoisotopic_mass"].ToString(),
                    ParamTypes.TermDynamicModification);
                paramList.Add(param);
            }

            // Look for Static and terminal mods

            foundRows = mMassMods.Select("mod_type_symbol = 'S' OR mod_type_symbol = 'P' or mod_type_symbol = 'T'");

            foreach (var currentFoundRow in foundRows)
            {
                foundRow = currentFoundRow;
                string tmpSpec = "";
                switch (foundRow["residue_symbol"].ToString())
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

                var param = new ParamsEntry(
                    tmpSpec,
                    foundRow["monoisotopic_mass"].ToString(),
                    ParamTypes.StaticModification);
                paramList.Add(param);
            }

            // TODO Still need code to handle import/export of isotopic mods

            foundRows = mMassMods.Select("mod_type_symbol = 'I'");

            foreach (var currentFoundRow1 in foundRows)
            {
                foundRow = currentFoundRow1;
                var param = new ParamsEntry(
                    foundRow["affected_atom"].ToString(),
                    foundRow["monoisotopic_mass"].ToString(),
                    ParamTypes.IsotopicModification);
                paramList.Add(param);
            }

            return paramList;
        }

        private string GetDynModSpecifier(DataRow[] rowSet)
        {
            var tmpSpec = "";

            if (rowSet.Length > 0)               // We have dynamic mods
            {
                foreach (var foundRow in rowSet)
                {
                    tmpSpec += foundRow["residue_symbol"].ToString();
                }

                return tmpSpec;
            }
            else
            {
                return null;
            }
        }

        private int GetIDWithName(string name, eParamFileTypeConstants eParamFileType)
        {
            var foundRows = mParamFileTable.Select("param_file_name = '" + name + "' AND param_file_type_id = " + ((int)eParamFileType).ToString());
            int tmpID;
            if (foundRows.Length > 0)
            {
                var foundRow = foundRows[0];
                tmpID = Params.SafeCastInt(foundRow["param_file_id"]);
            }
            else
            {
                tmpID = -1;
            }
            return tmpID;
        }

        private eParamFileTypeConstants GetTypeWithID(int paramFileID)
        {
            var foundRows = mParamFileTable.Select("param_file_id = " + paramFileID.ToString());
            eParamFileTypeConstants tmpID;
            if (foundRows.Length > 0)
            {
                var foundRow = foundRows[0];
                tmpID = (eParamFileTypeConstants)Params.SafeCastInt(foundRow["param_file_type_id"]);
            }
            else
            {
                tmpID = eParamFileTypeConstants.None;
            }
            return tmpID;
        }

        private eParamFileTypeConstants GetTypeWithName(string paramFileName)
        {
            var foundRows = mParamFileTable.Select("param_file_name = '" + paramFileName + "'");
            eParamFileTypeConstants tmpID;
            if (foundRows.Length > 0)
            {
                var foundRow = foundRows[0];
                tmpID = (eParamFileTypeConstants)Params.SafeCastInt(foundRow["param_file_type_id"]);
            }
            else
            {
                tmpID = eParamFileTypeConstants.None;
            }
            return tmpID;
        }

        private string GetDescriptionWithID(int paramFileID)
        {
            if (GetParamFileRowByID(paramFileID, out var matchingRow))
            {
                var tmpString = matchingRow["param_file_description"].ToString();
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
        /// Finds parameter file info for SEQUEST, X!Tandem, MSGF+, MSPathFinder, MODPlus, TopPIC, MSFragger, or MaxQuant
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private DataTable GetAvailableParamSets()
        {
            var paramTableSQL =
                "SELECT " +
                "  param_file_id AS id, " +
                "  param_file_name AS filename, " +
                "  param_file_description AS diffs, " +
                "  param_file_type_id AS type_id " +
                "FROM " + Param_File_Table + " " +
                "WHERE param_file_type_id = " + ((int)eParamFileTypeConstants.Sequest).ToString() +
                "   or param_file_type_id = " + ((int)eParamFileTypeConstants.XTandem).ToString() +
                "   or param_file_type_id = " + ((int)eParamFileTypeConstants.MSGFDB).ToString() +
                "   or param_file_type_id = " + ((int)eParamFileTypeConstants.MSPathFinder).ToString() +
                "   or param_file_type_id = " + ((int)eParamFileTypeConstants.MODPlus).ToString() +
                "   or param_file_type_id = " + ((int)eParamFileTypeConstants.TopPIC).ToString() +
                "   or param_file_type_id = " + ((int)eParamFileTypeConstants.MSFragger).ToString() +
                "   or param_file_type_id = " + ((int)eParamFileTypeConstants.MaxQuant).ToString();

            var tmpIDTable = GetTable(paramTableSQL);

            // Load tmpIDTable
            foreach (DataRow dr in tmpIDTable.Rows)
            {
                var tmpType = (int)dr["type_id"];

                if (tmpType == (int)eParamFileTypeConstants.Sequest)
                {
                    var tmpID = (int)dr["id"];
                    var tmpDiffs = (string)dr["diffs"];
                    if (tmpDiffs is null)
                    {
                        var eParamFileTypeID = (eParamFileTypeConstants)tmpType;

                        tmpDiffs = DistillFeaturesFromParamSet(tmpID, eParamFileTypeID);

                        dr["diffs"] = tmpDiffs;
                        dr.AcceptChanges();
                    }
                }
            }

            return tmpIDTable;

            // Need filtering code for tmpIDTable here...
        }

        private DataTable GetParamFileTypes()
        {
            var tableTypesSQL = "SELECT param_file_type_id AS id, param_file_type AS type " +
                                "FROM " + Param_File_Type_Table;

            var tmpTypeTable = GetTable(tableTypesSQL);

            return tmpTypeTable;
        }

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
            var pProps = pType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var pProp in pProps)
            {
                var tmpName = pProp.Name;
                var tmpType = pProp.PropertyType;
                if (mAcceptableParams.Contains(tmpName))
                {
                    if (tmpType.Name == "IonSeries")
                    {
                        c.AddRange(ExpandIonSeries(paramSet.IonSeries));
                    }
                    else if (tmpType.Name == "IsoMods")
                    {
                        c.AddRange(ExpandIsoTopicMods(paramSet.IsotopicModificationsList));
                    }
                    else if (tmpType.Name == "DynamicMods")
                    {
                        c.AddRange(ExpandDynamicMods(paramSet.DynamicMods, ParamTypes.DynamicModification));
                    }
                    else if (tmpType.Name == "TermDynamicMods")
                    {
                        c.AddRange(ExpandDynamicMods(paramSet.TermDynamicMods, ParamTypes.TermDynamicModification));
                    }
                    else if (tmpType.Name == "StaticMods")
                    {
                        c.AddRange(ExpandStaticMods(paramSet.StaticModificationsList));
                    }
                    else
                    {
                        var tmpValue = pProp.GetValue(paramSet, null).ToString();
                        if (mBasicParams.Contains(tmpName))
                        {
                            c.Add(new ParamsEntry(tmpName, tmpValue, ParamTypes.BasicParam));
                        }
                        else if (mAdvancedParams.Contains(tmpName))
                        {
                            c.Add(new ParamsEntry(tmpName, tmpValue, ParamTypes.AdvancedParam));
                        }
                    }
                }
            }

            return c;
        }

        private Params UpdateParamSetFromDataCollection(IReadOnlyList<ParamsEntry> dc)
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

                if (tmpType == ParamTypes.BasicParam &&
                    mBasicParams.Contains(tmpSpec))
                {
                    foreach (var currentPField in pFields)
                    {
                        pField = currentPField;
                        if ((pField.Name) == (tmpSpec ?? ""))
                        {
                            var tmpTypeName = pField.PropertyType.Name;
                            if (tmpTypeName == "Int32")
                            {
                                pField.SetValue(p, valueInteger, null);
                            }
                            else if (tmpTypeName == "Single")
                            {
                                pField.SetValue(p, (float)valueDouble, null);
                            }
                            else if (tmpTypeName == "String")
                            {
                                pField.SetValue(p, tmpValue, null);
                            }
                            else if (tmpTypeName == "Boolean")
                            {
                                pField.SetValue(p, valueBool, null);
                            }
                            else if (tmpTypeName == "MassTypeList")
                            {
                                pField.SetValue(p, (IBasicParams.MassTypeList)Enum.Parse(typeof(IBasicParams.MassTypeList), tmpValue, true), null);
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
                else if (tmpType == ParamTypes.AdvancedParam &&
                         mAdvancedParams.Contains(tmpSpec))
                {
                    foreach (var currentPField1 in pFields)
                    {
                        pField = currentPField1;
                        if ((pField.Name) == (tmpSpec ?? ""))
                        {
                            var tmpTypeName = pField.PropertyType.Name;
                            if (tmpTypeName == "Int32")
                            {
                                pField.SetValue(p, valueInteger, null);
                            }
                            else if (tmpTypeName == "Single")
                            {
                                pField.SetValue(p, (float)valueDouble, null);
                            }
                            else if (tmpTypeName == "String")
                            {
                                pField.SetValue(p, tmpValue, null);
                            }
                            else if (tmpTypeName == "Boolean")
                            {
                                pField.SetValue(p, valueBool, null);
                            }
                            else
                            {
                                Console.WriteLine(pField.PropertyType.Name);
                            }

                            break;
                        }
                    }
                }
                else if (tmpType == ParamTypes.AdvancedParam &&
                         mIonSeriesParams.Contains(tmpSpec))
                {
                    foreach (var ionField in ionFields)
                    {
                        if ((ionField.Name) == (tmpSpec ?? ""))
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
                else if (tmpType == ParamTypes.DynamicModification)
                {
                    p.DynamicMods.Add(tmpSpec, valueDouble);
                }
                else if (tmpType == ParamTypes.StaticModification)
                {
                    p.StaticModificationsList.Add(tmpSpec, valueDouble);
                }
                else if (tmpType == ParamTypes.IsotopicModification)
                {
                    p.IsotopicModificationsList.Add(tmpSpec, valueDouble);
                }
                else if (tmpType == ParamTypes.TermDynamicModification)
                {
                    p.TermDynamicMods.Add(tmpSpec, valueDouble);
                }
            }

            return p;
        }

        private List<ParamsEntry> ExpandDynamicMods(DynamicMods dynModSet, ParamTypes eDynModType)
        {
            var maxCount = dynModSet.Count;
            var paramList = new List<ParamsEntry>();

            if (maxCount == 0)
            {
                return paramList;
            }

            if (eDynModType != ParamTypes.DynamicModification &&
                eDynModType != ParamTypes.TermDynamicModification)
            {
                // This is unexpected; force eDynModType to be .DynamicModification
                eDynModType = ParamTypes.DynamicModification;
            }

            for (var counter = 1; counter < maxCount; counter++)
            {
                var param = new ParamsEntry(
                    dynModSet.Dyn_Mod_n_AAList(counter),
                    dynModSet.Dyn_Mod_n_MassDiff(counter).ToString("0.00000"),
                    eDynModType);
                paramList.Add(param);
            }

            return paramList;
        }

        private List<ParamsEntry> ExpandStaticMods(StaticMods statModSet)
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

        private List<ParamsEntry> ExpandIsoTopicMods(IsoMods isoModSet)
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

        private List<ParamsEntry> ExpandIonSeries(IonSeries ionSeriesSet)
        {
            var pType = typeof(IonSeries);
            var paramList = new List<ParamsEntry>();
            var pFields = pType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var pField in pFields)
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

            var tmpID = GetIDWithName(paramSetName, eParamFileType);
            if (tmpID < 0)
            {
                Console.WriteLine("Parameter file " + paramSetName + " with type ID " + eParamFileType.ToString() + " was not found in table " + Param_File_Table);
                return false;
            }

            return true;
        }

        private bool DoesParamSetIDExist(int paramSetID)
        {
            return GetParamFileRowByID(paramSetID, out _);
        }

        protected string CompareParamSets(Params templateSet, Params checkSet)
        {
            var diffCollection = GetDiffColl(templateSet, checkSet);
            return SummarizeDiffColl(diffCollection);
        }

        protected List<ParamsEntry> GetDiffColl(Params templateSet, Params checkSet)
        {
            var templateColl = WriteDataCollectionFromParamSet(templateSet);
            var checkColl = WriteDataCollectionFromParamSet(checkSet);

            return CompareDataCollections(templateColl, checkColl);
        }

        private string SummarizeDiffColl(IReadOnlyList<ParamsEntry> diffColl)
        {
            int index;

            var tmpIsoMods = "";
            var tmpOtherParams = "";

            Queue tmpDynModsList = null;
            Queue tmpTermDynModsList = null;
            Queue tmpStatModsList = null;
            Queue tmpIsoModsList = null;
            Queue tmpOtherParamsList = null;

            var intDynModCount = 0;
            var intTermDynModCount = 0;

            for (index = 0; index < diffColl.Count; index++)
            {
                var tmpType = diffColl[index].Type;
                var tmpSpec = diffColl[index].Specifier;
                var tmpValue = diffColl[index].Value;

                string tmpValueFormatted;
                string tmpSign;

                if (double.TryParse(tmpValue, out var dblValue))
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

                if (tmpType == ParamTypes.StaticModification)
                {
                    if (tmpStatModsList is null)
                    {
                        tmpStatModsList = new Queue();
                        tmpStatModsList.Enqueue("Static Mods: ");
                    }
                    tmpStatModsList.Enqueue(tmpSpec + " (" + tmpSign + tmpValueFormatted + ")");
                }
                else if (tmpType == ParamTypes.DynamicModification)
                {
                    if (tmpDynModsList is null)
                    {
                        tmpDynModsList = new Queue();
                        tmpDynModsList.Enqueue("Dynamic Mods: ");
                    }
                    tmpDynModsList.Enqueue(tmpSpec + " (" + tmpSign + tmpValueFormatted + ")");

                    intDynModCount++;
                }
                else if (tmpType == ParamTypes.TermDynamicModification)
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

                    intTermDynModCount++;
                }
                else if (tmpType == ParamTypes.IsotopicModification)
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
            var tmpString = "";

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

        private string MakeListOfMods(string strModDescriptionPrevious, Queue objModList, bool blnAddTitlePrefix)
        {
            strModDescriptionPrevious ??= "";
            if (objModList is null)
            {
                return strModDescriptionPrevious;
            }

            if (objModList.Count > 0)
            {
                if (strModDescriptionPrevious.Length > 0)
                    strModDescriptionPrevious += ", ";

                var tmpElement = "";
                var elementTitle = objModList.Dequeue().ToString();
                while (objModList.Count > 0)
                {
                    var subItem = objModList.Dequeue().ToString();
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

        private List<ParamsEntry> CompareDataCollections(IReadOnlyList<ParamsEntry> templateColl, IReadOnlyList<ParamsEntry> checkColl)
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
