using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ParamFileGenerator.Parameters;
using PRISM;
using PRISMDatabaseUtils;

namespace ParamFileGenerator
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

        bool MakeFile(string paramFileName, ParamFileType paramFileType, string fastaFilePath, string outputFilePath, string dmsConnectionString);

        bool MakeFile(string paramFileName, ParamFileType paramFileType, string fastaFilePath, string outputFilePath, string dmsConnectionString, int datasetID);

        bool MakeFile(string paramFileName, ParamFileType paramFileType, string fastaFilePath, string outputFilePath, string dmsConnectionString, string datasetName);

#pragma warning disable CS3001 // Type of parameter is not CLS-compliant
        List<string> GetAvailableParamSetNames(IDBTools dbTools);

        DataTable GetAvailableParamSetTable(IDBTools dbTools);

        DataTable GetAvailableParamFileTypes(IDBTools dbTools);

#pragma warning restore CS3001 // Type of parameter is not CLS-compliant

        string LastError { get; }
    }

    public class MakeParameterFile : EventNotifier, IGenerateFile
    {
        private IDBTools mDbTools;
        private WriteOutput mFileWriter;

        public string TemplateFilePath { get; set; }

        private string LastErrorMsg { get; set; } = string.Empty;

        /// <summary>
        /// Retrieve or create the specified parameter file
        /// </summary>
        /// <param name="paramFileName">Parameter file name</param>
        /// <param name="paramFileType">Parameter file type</param>
        /// <param name="fastaFilePath">FASTA file path</param>
        /// <param name="outputFilePath">Output file path</param>
        /// <param name="dmsConnectionString">DMS connection string</param>
        /// <returns>True if successful, false if an error</returns>
        public bool MakeFile(string paramFileName, IGenerateFile.ParamFileType paramFileType, string fastaFilePath, string outputFilePath, string dmsConnectionString)
        {
            return MakeFile(paramFileName, paramFileType, fastaFilePath, outputFilePath, dmsConnectionString, false);
        }

        /// <summary>
        /// Retrieve or create the specified parameter file
        /// </summary>
        /// <param name="paramFileName">Parameter file name</param>
        /// <param name="paramFileType">Parameter file type</param>
        /// <param name="fastaFilePath">FASTA file path</param>
        /// <param name="outputFilePath">Output file path</param>
        /// <param name="dmsConnectionString">DMS connection string</param>
        /// <param name="datasetName">Dataset name</param>
        /// <remarks>Uses the dataset name to lookup the value of use_mono_parent in view V_Analysis_Job_Use_Mono_Mass (SEQUEST only)</remarks>
        /// <returns>True if successful, false if an error</returns>
        public bool MakeFile(
            string paramFileName,
            IGenerateFile.ParamFileType paramFileType,
            string fastaFilePath,
            string outputFilePath,
            string dmsConnectionString,
            string datasetName)
        {
            var forceMonoStatus = GetMonoMassStatus(datasetName, dmsConnectionString);

            return MakeFile(paramFileName, paramFileType, fastaFilePath, outputFilePath, dmsConnectionString, forceMonoStatus);
        }

        /// <summary>
        /// Retrieve or create the specified parameter file
        /// </summary>
        /// <param name="paramFileName">Parameter file name</param>
        /// <param name="paramFileType">Parameter file type</param>
        /// <param name="fastaFilePath">FASTA file path</param>
        /// <param name="outputFilePath">Output file path</param>
        /// <param name="dmsConnectionString">DMS connection string</param>
        /// <param name="datasetID">Dataset ID</param>
        /// <remarks>Uses the dataset ID to lookup the value of use_mono_parent in view V_Analysis_Job_Use_Mono_Mass (SEQUEST only)</remarks>
        /// <returns>True if successful, false if an error</returns>
        public bool MakeFile(
            string paramFileName,
            IGenerateFile.ParamFileType paramFileType,
            string fastaFilePath,
            string outputFilePath,
            string dmsConnectionString,
            int datasetID)
        {
            var forceMonoStatus = GetMonoMassStatus(datasetID, dmsConnectionString);

            return MakeFile(paramFileName, paramFileType, fastaFilePath, outputFilePath, dmsConnectionString, forceMonoStatus);
        }

        /// <summary>
        /// Retrieve or create the specified parameter file
        /// </summary>
        /// <param name="paramFileName">Parameter file name</param>
        /// <param name="paramFileType">Parameter file type</param>
        /// <param name="fastaFilePath">FASTA file path</param>
        /// <param name="outputFilePath">Output file path</param>
        /// <param name="dmsConnectionString">DMS connection string</param>
        /// <param name="forceMonoParentMass">When true, set ?? to 1 (SEQUEST only)</param>
        /// <returns>True if successful, false if an error</returns>
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
                        return MakeFileSQL(paramFileName, paramFileType, fastaFilePath, outputFilePath, dmsConnectionString, forceMonoParentMass);
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
            var typeCheckSQL = "SELECT use_mono_parent FROM V_Analysis_Job_Use_Mono_Mass WHERE dataset_id = " + DatasetID;
            return GetMonoParentStatusWorker(typeCheckSQL, dmsConnectionString);
        }

        private bool GetMonoMassStatus(string datasetName, string dmsConnectionString)
        {
            var typeCheckSQL = "SELECT use_mono_parent FROM V_Analysis_Job_Use_Mono_Mass WHERE dataset_name = '" + datasetName + "'";
            return GetMonoParentStatusWorker(typeCheckSQL, dmsConnectionString);
        }

        private bool GetMonoParentStatusWorker(string sqlQuery, string dmsConnectionString)
        {
            if (mDbTools is null)
            {
                var connectionStringToUse = DbToolsFactory.AddApplicationNameToConnectionString(dmsConnectionString, "ParamFileGenerator");
                mDbTools = DbToolsFactory.GetDBTools(connectionStringToUse);
            }

            mDbTools.GetQueryResults(sqlQuery, out var typeCheckTable);

            if (typeCheckTable.Count > 0 && int.TryParse(typeCheckTable[0].First(), out var useMonoMassInt))
            {
                return useMonoMassInt > 0;
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
        private bool MakeFileSQL(string paramFileName, IGenerateFile.ParamFileType paramFileType, string fastaFilePath, string outputFilePath, string dmsConnectionString, bool forceMonoParentMass)
        {
            if (mDbTools is null)
            {
                var connectionStringToUse = DbToolsFactory.AddApplicationNameToConnectionString(dmsConnectionString, "ParamFileGenerator");
                mDbTools = DbToolsFactory.GetDBTools(connectionStringToUse);
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

            var dmsParams = new ParamsFromDMS(mDbTools);
            IReconstituteIsoMods modProcessor = new ReconstituteIsoMods(mDbTools);

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

            var loadedParams = dmsParams.ReadParamsFromDMS(paramFileName);
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

            mFileWriter ??= new WriteOutput();

            var writeSuccess = mFileWriter.WriteOutputFile(loadedParams, Path.Combine(outputFilePath, paramFileName), paramFileType);

            MakeSeqInfoRelatedFiles(paramFileName, outputFilePath, dmsConnectionString);

            return writeSuccess;
        }

        private void MakeSeqInfoRelatedFiles(string paramFileName, string targetDirectory, string dmsConnectionString)
        {
            const string MAXQUANT_MOD_NAME_COLUMN = "MaxQuant_Mod_Name";
            const string UNIMOD_MOD_NAME_COLUMN = "UniMod_Mod_Name";

            if (mDbTools is null)
            {
                var connectionStringToUse = DbToolsFactory.AddApplicationNameToConnectionString(dmsConnectionString, "ParamFileGenerator");
                mDbTools = DbToolsFactory.GetDBTools(connectionStringToUse);
            }

            var baseParamFileName = Path.GetFileNameWithoutExtension(paramFileName);

            mFileWriter ??= new WriteOutput();

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

            const string mctSQL = "SELECT mass_correction_tag, monoisotopic_mass, affected_atom " +
                         "FROM V_Mass_Correction_Factors " +
                         "ORDER BY mass_correction_tag";

            var mdSQL = "SELECT " +
                        "local_symbol As modification_symbol, " +
                        "monoisotopic_mass, " +
                        "residue_symbol As target_residues, " +
                        "mod_type_symbol As modification_type, " +
                        "mass_correction_tag, " +
                        MAXQUANT_MOD_NAME_COLUMN.ToLowerInvariant() + ", " +
                        UNIMOD_MOD_NAME_COLUMN.ToLowerInvariant() + " " +
                        "FROM V_Param_File_Mass_Mod_Info " +
                        "WHERE param_file_name = '" + paramFileName + "'";

            mDbTools.GetQueryResults(mctSQL, out var massCorrectionTags);

            mDbTools.GetQueryResults(mdSQL, out var paramFileModInfo);

            // Create the Mass_Correction_Tags file in the working directory
            mFileWriter.WriteDataTableToOutputFile(massCorrectionTags, Path.Combine(targetDirectory, "Mass_Correction_Tags.txt"), massCorrectionTagsHeaderNames);

            // Check whether any MaxQuant mods are actually defined
            var includeMaxQuant = false;

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
            mFileWriter.WriteDataTableToOutputFile(paramFileModInfoToWrite, Path.Combine(targetDirectory, baseParamFileName + "_ModDefs.txt"), modDefHeaderNames);
        }

        private bool RetrieveStaticPSMParameterFile(string analysisToolName, string paramFileName, string targetDirectory, string dmsConnectionString)
        {
            if (mDbTools is null)
            {
                var connectionStringToUse = DbToolsFactory.AddApplicationNameToConnectionString(dmsConnectionString, "ParamFileGenerator");
                mDbTools = DbToolsFactory.GetDBTools(connectionStringToUse);
            }

            // ReSharper disable once StringLiteralTypo
            var paramFilePathSQL =
                "SELECT param_file_storage_path " +
                "FROM V_Analysis_Tool_Paths " +
                "WHERE tool_name = '" + analysisToolName + "'";

            // The query should only return one row
            mDbTools.GetQueryResults(paramFilePathSQL, out var paramFilePathTable);

            if (paramFilePathTable.Count == 0)
            {
                ReportError("Tool not found in V_Analysis_Tool_Paths: " + analysisToolName);
                return false;
            }

            var paramFileDirectory = paramFilePathTable[0].First();
            var paramFilePath = Path.Combine(paramFileDirectory, paramFileName);

            if (!Directory.Exists(paramFileDirectory))
            {
                ReportError(string.Format("Directory defined in V_Analysis_Tool_Paths for {0} was not found: {1}",
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
            {
                availableParamSets.Add(dr["FileName"].ToString());
            }

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
