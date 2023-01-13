﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ParamFileGenerator.DownloadParams;
using PRISM;
using PRISMDatabaseUtils;

// ReSharper disable once CheckNamespace
namespace ParamFileGenerator.MakeParams
{
    public interface IGenerateFile
    {
        // Ignore Spelling: Sequest

        public enum ParamFileType
        {
            Invalid = -1,            // Other stuff not currently handled
            BioWorks_20 = 0,         // Normal BioWorks 2.0 SEQUEST
            BioWorks_30 = 1,         // BioWorks 3.0+ TurboSequest
            BioWorks_31 = 2,         // BioWorks 3.1 ClusterQuest
            BioWorks_32 = 3,         // BioWorks 3.2 Cluster
            BioWorks_Current = 4,
            X_Tandem = 5,            // X!Tandem XML file
            Inspect = 6,             // Inspect
            MSGFPlus = 7,            // MSGF-DB or MSGF+
            MSAlign = 8,             // MSAlign
            MSAlignHistone = 9,      // MSAlign_Histone (which is MS-Align+)
            MODa = 10,
            MSPathFinder = 11,
            MODPlus = 12,
            TopPIC = 13,
            MSFragger = 14,
            MaxQuant = 15
        }

        bool MakeFile(
            string paramFileName,
            ParamFileType paramFileType,
            string fastaFilePath,
            string outputFilePath,
            string dmsConnectionString);

        bool MakeFile(
            string paramFileName,
            ParamFileType paramFileType,
            string fastaFilePath,
            string outputFilePath,
            string dmsConnectionString,
            int DatasetID);

        bool MakeFile(
            string paramFileName,
            ParamFileType paramFileType,
            string fastaFilePath,
            string outputFilePath,
            string dmsConnectionString,
            string datasetName);

#pragma warning disable CS3001 // Type of parameter is not CLS-compliant
        List<string> GetAvailableParamSetNames(IDBTools dbTools);
        DataTable GetAvailableParamSetTable(IDBTools dbTools);
        DataTable GetAvailableParamFileTypes(IDBTools dbTools);
#pragma warning restore CS3001 // Type of parameter is not CLS-compliant

        string LastError { get; }
    }

    public class MakeParameterFile : EventNotifier, IGenerateFile
    {
        private IDBTools m_DbTools;
        private WriteOutput m_FileWriter;

        public string TemplateFilePath { get; set; }

        private string LastErrorMsg { get; set; } = string.Empty;

        public bool MakeFile(
            string paramFileName,
            IGenerateFile.ParamFileType paramFileType,
            string fastaFilePath,
            string outputFilePath,
            string dmsConnectionString)
        {
            return MakeFile(paramFileName, paramFileType, fastaFilePath, outputFilePath, dmsConnectionString, false);
        }

        public bool MakeFile(
            string paramFileName,
            IGenerateFile.ParamFileType paramFileType,
            string fastaFilePath,
            string outputFilePath,
            string dmsConnectionString,
            string datasetName)
        {
            bool forceMonoStatus = GetMonoMassStatus(datasetName, dmsConnectionString);

            return MakeFile(paramFileName, paramFileType, fastaFilePath, outputFilePath, dmsConnectionString, forceMonoStatus);
        }

        public bool MakeFile(
            string paramFileName,
            IGenerateFile.ParamFileType paramFileType,
            string fastaFilePath,
            string outputFilePath,
            string dmsConnectionString,
            int DatasetID)
        {
            bool forceMonoStatus = GetMonoMassStatus(DatasetID, dmsConnectionString);

            return MakeFile(paramFileName, paramFileType, fastaFilePath, outputFilePath, dmsConnectionString, forceMonoStatus);
        }

        private bool MakeFile(
            string paramFileName,
            IGenerateFile.ParamFileType paramFileType,
            string fastaFilePath,
            string outputFilePath,
            string dmsConnectionString,
            bool forceMonoParentMass)
        {
            LastErrorMsg = string.Empty;

            try
            {
                switch (paramFileType)
                {
                    case IGenerateFile.ParamFileType.X_Tandem:
                        return RetrieveStaticPSMParameterFile("XTandem", paramFileName, outputFilePath, dmsConnectionString);

                    case IGenerateFile.ParamFileType.Inspect:
                        return RetrieveStaticPSMParameterFile("Inspect", paramFileName, outputFilePath, dmsConnectionString);

                    case IGenerateFile.ParamFileType.MODa:
                        return RetrieveStaticPSMParameterFile("MODa", paramFileName, outputFilePath, dmsConnectionString);

                    case IGenerateFile.ParamFileType.MSGFPlus:
                        return RetrieveStaticPSMParameterFile("MSGFPlus", paramFileName, outputFilePath, dmsConnectionString);

                    case IGenerateFile.ParamFileType.MSAlign:
                        return RetrieveStaticPSMParameterFile("MSAlign", paramFileName, outputFilePath, dmsConnectionString);

                    case IGenerateFile.ParamFileType.MSAlignHistone:
                        return RetrieveStaticPSMParameterFile("MSAlign_Histone", paramFileName, outputFilePath, dmsConnectionString);

                    case IGenerateFile.ParamFileType.MSPathFinder:
                        return RetrieveStaticPSMParameterFile("MSPathFinder", paramFileName, outputFilePath, dmsConnectionString);

                    case IGenerateFile.ParamFileType.MODPlus:
                        return RetrieveStaticPSMParameterFile("MODPlus", paramFileName, outputFilePath, dmsConnectionString);

                    case IGenerateFile.ParamFileType.TopPIC:
                        return RetrieveStaticPSMParameterFile("TopPIC", paramFileName, outputFilePath, dmsConnectionString);

                    case IGenerateFile.ParamFileType.MSFragger:
                        return RetrieveStaticPSMParameterFile("MSFragger", paramFileName, outputFilePath, dmsConnectionString);

                    case IGenerateFile.ParamFileType.MaxQuant:
                        return RetrieveStaticPSMParameterFile("MaxQuant", paramFileName, outputFilePath, dmsConnectionString);

                    case IGenerateFile.ParamFileType.Invalid:
                        return default;

                    default:
                        paramFileType = IGenerateFile.ParamFileType.BioWorks_32;
                        return MakeFileSQ(
                            paramFileName,
                            paramFileType,
                            fastaFilePath,
                            outputFilePath,
                            dmsConnectionString,
                            forceMonoParentMass);
                }
            }

            catch (Exception ex)
            {
                ReportError("Error in MakeFile: " + ex.Message, ex);
                return false;
            }
        }

        private bool GetMonoMassStatus(int DatasetID, string dmsConnectionString)
        {
            string TypeCheckSQL = "SELECT use_mono_parent FROM V_Analysis_Job_Use_Mono_Mass WHERE dataset_id = " + DatasetID.ToString();
            return GetMonoParentStatusWorker(TypeCheckSQL, dmsConnectionString);
        }

        private bool GetMonoMassStatus(string datasetName, string dmsConnectionString)
        {
            string TypeCheckSQL = "SELECT use_mono_parent FROM V_Analysis_Job_Use_Mono_Mass WHERE dataset_name = '" + datasetName + "'";
            return GetMonoParentStatusWorker(TypeCheckSQL, dmsConnectionString);
        }

        private bool GetMonoParentStatusWorker(string sqlQuery, string dmsConnectionString)
        {
            if (m_DbTools is null)
            {
                string connectionStringToUse = DbToolsFactory.AddApplicationNameToConnectionString(dmsConnectionString, "ParamFileGenerator");
                m_DbTools = DbToolsFactory.GetDBTools(connectionStringToUse);
            }

            List<List<string>> typeCheckTable = null;
            m_DbTools.GetQueryResults(sqlQuery, out typeCheckTable);

            if (typeCheckTable.Count > 0)
            {
                int useMonoMassInt;

                if (int.TryParse(typeCheckTable[0].First(), out useMonoMassInt))
                {
                    if (useMonoMassInt > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Create SEQUEST parameter file
        /// </summary>
        /// <param name="paramFileName"></param>
        /// <param name="paramFileType"></param>
        /// <param name="fastaFilePath"></param>
        /// <param name="outputFilePath"></param>
        /// <param name="dmsConnectionString"></param>
        /// <param name="forceMonoParentMass"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool MakeFileSQ(
            string paramFileName,
            IGenerateFile.ParamFileType paramFileType,
            string fastaFilePath,
            string outputFilePath,
            string dmsConnectionString,
            bool forceMonoParentMass)
        {
            if (m_DbTools is null)
            {
                string connectionStringToUse = DbToolsFactory.AddApplicationNameToConnectionString(dmsConnectionString, "ParamFileGenerator");
                m_DbTools = DbToolsFactory.GetDBTools(connectionStringToUse);
            }

            const string DEF_TEMPLATE_FILEPATH = @"\\Gigasax\DMS_Parameter_Files\Sequest\sequest_N14_NE_Template.params";

            if (string.IsNullOrWhiteSpace(TemplateFilePath))
            {
                TemplateFilePath = DEF_TEMPLATE_FILEPATH;
            }

            var fi = new FileInfo(TemplateFilePath);
            if (!fi.Exists)
            {
                ReportError("Default template file '" + TemplateFilePath + "' does not exist");
                return false;
            }

            // Instantiate MainProcess so we can access its properties later

            // ReSharper disable once UnusedVariable.Compiler
            var processor = new MainProcess(TemplateFilePath);

            Params loadedParams;
            var dmsParams = new ParamsFromDMS(m_DbTools);
            IReconstituteIsoMods modProcessor;
            modProcessor = new ReconstituteIsoMods(m_DbTools);

            if (!dmsParams.ParamFileTableLoaded)
            {
                ReportError("Could Not Establish Database Connection");
                return false;
            }

            if (!dmsParams.ParamSetNameExists(paramFileName))
            {
                ReportError("Parameter File '" + paramFileName + "' does not exist in the database");
                return false;
            }

            loadedParams = dmsParams.ReadParamsFromDMS(paramFileName);
            loadedParams = modProcessor.ReconstituteIsoMods(loadedParams);

            if (forceMonoParentMass && !loadedParams.LoadedParamNames.ContainsKey("ParentMassType"))
            {
                loadedParams.ParentMassType = IBasicParams.MassTypeList.Monoisotopic;
            }

            if (!loadedParams.LoadedParamNames.ContainsKey("PeptideMassUnits"))
            {
                loadedParams.PeptideMassUnits = (int)IAdvancedParams.MassUnitList.amu;
            }

            if (!loadedParams.LoadedParamNames.ContainsKey("FragmentMassUnits"))
            {
                loadedParams.FragmentMassUnits = (int)IAdvancedParams.MassUnitList.amu;
            }

            loadedParams.DefaultFASTAPath = fastaFilePath;

            if (m_FileWriter is null)
            {
                m_FileWriter = new WriteOutput();
            }

            bool writeSuccess = m_FileWriter.WriteOutputFile(loadedParams, Path.Combine(outputFilePath, paramFileName), paramFileType);

            MakeSeqInfoRelatedFiles(paramFileName, outputFilePath, dmsConnectionString);

            return writeSuccess;
        }

        private void MakeSeqInfoRelatedFiles(
            string paramFileName,
            string targetDirectory,
            string dmsConnectionString)
        {
            const string MAXQUANT_MOD_NAME_COLUMN = "MaxQuant_Mod_Name";
            const string UNIMOD_MOD_NAME_COLUMN = "UniMod_Mod_Name";

            string mctSQL;
            string mdSQL;

            if (m_DbTools is null)
            {
                string connectionStringToUse = DbToolsFactory.AddApplicationNameToConnectionString(dmsConnectionString, "ParamFileGenerator");
                m_DbTools = DbToolsFactory.GetDBTools(connectionStringToUse);
            }

            string baseParamFileName = Path.GetFileNameWithoutExtension(paramFileName);

            if (m_FileWriter is null)
            {
                m_FileWriter = new WriteOutput();
            }

            var massCorrectionTagsHeaderNames = new List<string>()
            {
                "Mass_Correction_Tag",
                "Monoisotopic_Mass",
                "Affected_Atom"
            };

            // Note that "MaxQuant_Mod_Name" will be added below if MaxQuant mods are defined
            var modDefHeaderNames = new List<string>()
            {
                "Modification_Symbol",
                "Monoisotopic_Mass",
                "Target_Residues",
                "Modification_Type",
                "Mass_Correction_Tag"
            };

            mctSQL =
                "SELECT Mass_Correction_Tag, Monoisotopic_Mass, Affected_Atom " +
                "FROM T_Mass_Correction_Factors " +
                "ORDER BY Mass_Correction_Tag";

            mdSQL =
                "SELECT " +
                "Local_Symbol As Modification_Symbol, " +
                "Monoisotopic_Mass, " +
                "Residue_Symbol As Target_Residues, " +
                "Mod_Type_Symbol As Modification_Type, " +
                "Mass_Correction_Tag, " +
                MAXQUANT_MOD_NAME_COLUMN + ", " +
                UNIMOD_MOD_NAME_COLUMN + " " +
                "FROM V_Param_File_Mass_Mod_Info " +
                "WHERE Param_File_Name = '" + paramFileName + "'";

            List<List<string>> massCorrectionTags = null;
            m_DbTools.GetQueryResults(mctSQL, out massCorrectionTags);

            List<List<string>> paramFileModInfo = null;
            m_DbTools.GetQueryResults(mdSQL, out paramFileModInfo);

            // Create the Mass_Correction_Tags file in the working directory
            m_FileWriter.WriteDataTableToOutputFile(massCorrectionTags, Path.Combine(targetDirectory, "Mass_Correction_Tags.txt"), massCorrectionTagsHeaderNames);

            // Check whether any MaxQuant mods are actually defined
            bool includeMaxQuant = false;
            foreach (var item in paramFileModInfo)
            {
                if (item[5].Length > 0)
                {
                    includeMaxQuant = true;
                    modDefHeaderNames.Add(MAXQUANT_MOD_NAME_COLUMN);
                    break;
                }
            }

            // Populate a new list, only including the MaxQuant column if includeMaxQuant is true
            var paramFileModInfoToWrite = new List<List<string>>();

            foreach (var item in paramFileModInfo)
            {
                var currentRow = new List<string>();

                currentRow.AddRange(item.Take(5));

                if (includeMaxQuant)
                {
                    currentRow.Add(item[5]);
                }

                currentRow.Add(item[6]);

                paramFileModInfoToWrite.Add(currentRow);
            }

            // Always include the UniMod column
            modDefHeaderNames.Add(UNIMOD_MOD_NAME_COLUMN);

            // Create the param file specific modification definitions file in the working directory
            m_FileWriter.WriteDataTableToOutputFile(paramFileModInfoToWrite, Path.Combine(targetDirectory, baseParamFileName + "_ModDefs.txt"), modDefHeaderNames);
        }

        private bool RetrieveStaticPSMParameterFile(
            string analysisToolName,
            string paramFileName,
            string targetDirectory,
            string dmsConnectionString)
        {
            string paramFilePath;

            if (m_DbTools is null)
            {
                string connectionStringToUse = DbToolsFactory.AddApplicationNameToConnectionString(dmsConnectionString, "ParamFileGenerator");
                m_DbTools = DbToolsFactory.GetDBTools(connectionStringToUse);
            }

            // ReSharper disable once StringLiteralTypo
            string paramFilePathSQL =
                "SELECT TOP 1 AJT_parmFileStoragePath " +
                "FROM T_Analysis_Tool " +
                "WHERE AJT_ToolName = '" + analysisToolName + "'";

            List<List<string>> paramFilePathTable = null;
            m_DbTools.GetQueryResults(paramFilePathSQL, out paramFilePathTable);

            if (paramFilePathTable.Count == 0)
            {
                ReportError("Tool not found in T_Analysis_Tool: " + analysisToolName);
                return false;
            }

            string paramFileDirectory = paramFilePathTable[0].First();
            paramFilePath = Path.Combine(paramFileDirectory, paramFileName);

            if (!Directory.Exists(paramFileDirectory))
            {
                ReportError(string.Format("Directory defined in T_Analysis_Tool for {0} was not found: {1}",
                    analysisToolName, paramFileDirectory));

                return false;
            }

            var fiSource = new FileInfo(paramFilePath);
            if (!fiSource.Exists)
            {
                ReportError("Parameter file not found: " + fiSource.FullName);
                return false;
            }

            // Copy the param file from Gigasax to the working directory
            fiSource.CopyTo(Path.Combine(targetDirectory, paramFileName), true);

            MakeSeqInfoRelatedFiles(paramFileName, targetDirectory, dmsConnectionString);

            return true;
        }

#pragma warning disable CS3001 // Type of parameter is not CLS-compliant
        public List<string> GetAvailableParamSetNames(IDBTools dbTools)
#pragma warning restore CS3001 // Type of parameter is not CLS-compliant
        {
            var availableParamSets = new List<string>();
            var dmsParams = new ParamsFromDMS(dbTools);

            var retrievedParamSets = dmsParams.RetrieveAvailableParams();
            foreach (DataRow dr in retrievedParamSets.Rows)
                availableParamSets.Add(dr["FileName"].ToString());
            return availableParamSets;
        }

#pragma warning disable CS3001 // Type of parameter is not CLS-compliant
        public DataTable GetAvailableParamSetTable(IDBTools dbTools)
#pragma warning restore CS3001 // Type of parameter is not CLS-compliant
        {
            var paramGenerator = new ParamsFromDMS(dbTools);
            var paramSetsAvailable = paramGenerator.RetrieveAvailableParams();

            return paramSetsAvailable;
        }

#pragma warning disable CS3001 // Type of parameter is not CLS-compliant
        public DataTable GetAvailableParamFileTypes(IDBTools dbTools)
#pragma warning restore CS3001 // Type of parameter is not CLS-compliant
        {
            var paramGenerator = new ParamsFromDMS(dbTools);
            var paramTypesAvailable = paramGenerator.RetrieveParamFileTypes();
            return paramTypesAvailable;
        }

        public string LastError => LastErrorMsg;

        private void ReportError(string errorMessage, Exception ex = null)
        {
            OnErrorEvent(errorMessage, ex);
            LastErrorMsg = errorMessage;
        }
    }
}