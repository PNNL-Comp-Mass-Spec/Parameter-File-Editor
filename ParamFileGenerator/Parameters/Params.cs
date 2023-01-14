using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ParamFileGenerator
{
    public class Params : IBasicParams, IAdvancedParams
    {
        // Ignore Spelling: Xcalibur

        public enum ParamFileTypes
        {
            BioWorks_20 = 0, // Normal BioWorks 2.0 SEQUEST
            BioWorks_30 = 1, // BioWorks 3.0+ TurboSEQUEST
            BioWorks_31 = 2, // BioWorks 3.1 ClusterQuest
            BioWorks_32 = 3 // BioWorks 3.2 ClusterF***
        }

        private const string DEF_DB_NAME = @"C:\Xcalibur\database\nr.fasta";     // Not really used, just a placeholder

        private const string DEF_ENZ_SECTION_NAME = "SEQUEST_ENZYME_INFO";

        // Basic Parameters
        private ParamFileTypes mType;

        // Advanced Parameters
        private string mIonSeriesString;
        private string mProtMassFilterString;

        private RetrieveParams mFullTemplate;
        private string mTemplateFilePath;

        public static Params BaseLineParamSet => MainProcess.BaseLineParamSet;

        public ParamFileTypes FileType => mType;

        public int DMS_ID { get; set; }

        public string FileName { get; set; }

        public int FileTypeIndex { get; set; }

        public string Description { get; set; }

        public string DefaultFASTAPath { get; set; }

        public string DefaultFASTAPath2 { get; set; }

        public int NumberOfResultsToProcess { get; set; }

        public float PeptideMassTolerance { get; set; }

        public bool CreateOutputFiles { get; set; }

        public IonSeries IonSeries { get; set; }

        public DynamicMods DynamicMods { get; set; }

        public TermDynamicMods TermDynamicMods { get; set; }

        public IsoMods IsotopicModificationsList { get; set; }

        [Obsolete("Use IsotopicModificationsList instead.")]
        public IsoMods IsotopicMods => IsotopicModificationsList;

        public int MaximumNumAAPerDynMod { get; set; }

        public int MaximumNumDifferentialPerPeptide { get; set; }

        public int UsePhosphoFragmentation { get; set; }

        public float FragmentIonTolerance { get; set; }

        public int NumberOfOutputLines { get; set; }

        public int NumberOfDescriptionLines { get; set; }

        public bool ShowFragmentIons { get; set; }

        public bool PrintDuplicateReferences { get; set; }

        public EnzymeDetails SelectedEnzymeDetails { get; set; }

        public int SelectedEnzymeIndex { get; set; }

        public int SelectedEnzymeCleavagePosition { get; set; }

        public IAdvancedParams.FrameList SelectedNucReadingFrame { get; set; }

        public int SelectedNucReadingFrameIndex
        {
            get => (int)SelectedNucReadingFrame;
            set => SelectedNucReadingFrame = (IAdvancedParams.FrameList)value;
        }

        public IBasicParams.MassTypeList ParentMassType { get; set; }

        public IBasicParams.MassTypeList FragmentMassType { get; set; }

        public bool RemovePrecursorPeak { get; set; }

        public float IonCutoffPercentage { get; set; }

        public int MaximumNumberMissedCleavages { get; set; }

        public float MinimumProteinMassToSearch { get; set; }

        public float MaximumProteinMassToSearch { get; set; }

        public int NumberOfDetectedPeaksToMatch { get; set; }

        public int NumberOfAllowedDetectedPeakErrors { get; set; }

        public float MatchedPeakMassTolerance { get; set; }

        public bool AminoAcidsAllUpperCase { get; set; }

        public string PartialSequenceToMatch { get; set; }

        public string SequenceHeaderInfoToFilter { get; set; }

        public int PeptideMassUnits { get; set; }

        public int FragmentMassUnits { get; set; }

        public StaticMods StaticModificationsList { get; set; }

        public List<EnzymeDetails> EnzymeList { get; set; }

        public Hashtable LoadedParamNames { get; set; } = new();

        public void AddLoadedParamName(string parameterName, string parameterValue)
        {
            LoadedParamNames ??= new Hashtable();
            if (!LoadedParamNames.ContainsKey(parameterName))
            {
                LoadedParamNames.Add(parameterName, parameterValue);
            }
        }

        public EnzymeDetails RetrieveEnzymeDetails(int enzymeListIndex)
        {
            return EnzymeList[enzymeListIndex];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Params()
        {
            IonSeries = new IonSeries();
            EnzymeList = new List<EnzymeDetails>();
            SelectedEnzymeDetails = new EnzymeDetails();
            DynamicMods = new DynamicMods();
            StaticModificationsList = new StaticMods();
            IsotopicModificationsList = new IsoMods();
            TermDynamicMods = new TermDynamicMods("0.0 0.0");

            MaximumNumDifferentialPerPeptide = 3;
            UsePhosphoFragmentation = 0;
            PeptideMassUnits = (int)IAdvancedParams.MassUnitList.amu;
        }

        public string ReturnMassFilter(float minimumMassToFilter, float maximumMassToFilter)
        {
            return ReturnMassFilterString(minimumMassToFilter, maximumMassToFilter);
        }

        public void LoadTemplate(string templateFileName)
        {
            LoadTemplateParams(templateFileName);
        }

        private void LoadTemplateParams(string templateFileName)
        {
            mTemplateFilePath = GetFilePath(templateFileName);
            var getEnzymeList = new GetEnzymeBlockType(mTemplateFilePath, DEF_ENZ_SECTION_NAME);
            EnzymeList = getEnzymeList.EnzymeList;

            mType = GetTemplateType();

            const string sectionName = "SEQUEST";

            mFullTemplate = new RetrieveParams(mTemplateFilePath);
            // Retrieve Basic Parameters
            mFullTemplate.SetSection(sectionName);
            FileName = Path.GetFileName(mTemplateFilePath);
            SelectedEnzymeIndex = SafeCastInt(mFullTemplate.GetParam("enzyme_number"));
            SelectedEnzymeDetails = EnzymeList[SelectedEnzymeIndex]; // TODO: Add an item first!!
            SelectedEnzymeCleavagePosition = 1;
            MaximumNumberMissedCleavages = SafeCastInt(mFullTemplate.GetParam("max_num_internal_cleavage_sites"));
            ParentMassType = (IBasicParams.MassTypeList)SafeCastInt(mFullTemplate.GetParam("mass_type_parent"));
            FragmentMassType = (IBasicParams.MassTypeList)SafeCastInt(mFullTemplate.GetParam("mass_type_fragment"));
            PartialSequenceToMatch = mFullTemplate.GetParam("partial_sequence");
            DynamicMods = new DynamicMods(mFullTemplate.GetParam("diff_search_options"));
            TermDynamicMods = new TermDynamicMods(mFullTemplate.GetParam("term_diff_search_options"));

            StaticModificationsList = new StaticMods
            {
                // Get Static Mods
                CtermPeptide = SafeCastDouble(mFullTemplate.GetParam("add_Cterm_peptide")),
                CtermProtein = SafeCastDouble(mFullTemplate.GetParam("add_Cterm_protein")),
                NtermPeptide = SafeCastDouble(mFullTemplate.GetParam("add_Nterm_peptide")),
                NtermProtein = SafeCastDouble(mFullTemplate.GetParam("add_Nterm_protein")),
                G_Glycine = SafeCastDouble(mFullTemplate.GetParam("add_G_Glycine")),
                A_Alanine = SafeCastDouble(mFullTemplate.GetParam("add_A_Alanine")),
                S_Serine = SafeCastDouble(mFullTemplate.GetParam("add_S_Serine")),
                P_Proline = SafeCastDouble(mFullTemplate.GetParam("add_P_Proline")),
                V_Valine = SafeCastDouble(mFullTemplate.GetParam("add_V_Valine")),
                T_Threonine = SafeCastDouble(mFullTemplate.GetParam("add_T_Threonine")),
                C_Cysteine = SafeCastDouble(mFullTemplate.GetParam("add_C_Cysteine")),
                L_Leucine = SafeCastDouble(mFullTemplate.GetParam("add_L_Leucine")),
                I_Isoleucine = SafeCastDouble(mFullTemplate.GetParam("add_I_Isoleucine")),
                X_LorI = SafeCastDouble(mFullTemplate.GetParam("add_X_LorI")),
                N_Asparagine = SafeCastDouble(mFullTemplate.GetParam("add_N_Asparagine")),
                O_Ornithine = SafeCastDouble(mFullTemplate.GetParam("add_O_Ornithine")),
                B_avg_NandD = SafeCastDouble(mFullTemplate.GetParam("add_B_avg_NandD")),
                D_Aspartic_Acid = SafeCastDouble(mFullTemplate.GetParam("add_D_Aspartic_Acid")),
                Q_Glutamine = SafeCastDouble(mFullTemplate.GetParam("add_Q_Glutamine")),
                K_Lysine = SafeCastDouble(mFullTemplate.GetParam("add_K_Lysine")),
                Z_avg_QandE = SafeCastDouble(mFullTemplate.GetParam("add_Z_avg_QandE")),
                E_Glutamic_Acid = SafeCastDouble(mFullTemplate.GetParam("add_E_Glutamic_Acid")),
                M_Methionine = SafeCastDouble(mFullTemplate.GetParam("add_M_Methionine")),
                H_Histidine = SafeCastDouble(mFullTemplate.GetParam("add_H_Histidine")),
                F_Phenylalanine = SafeCastDouble(mFullTemplate.GetParam("add_F_Phenylalanine")),
                R_Arginine = SafeCastDouble(mFullTemplate.GetParam("add_R_Arginine")),
                Y_Tyrosine = SafeCastDouble(mFullTemplate.GetParam("add_Y_Tyrosine")),
                W_Tryptophan = SafeCastDouble(mFullTemplate.GetParam("add_W_Tryptophan"))
            };

            // add code to check for existence of isotopic mods

            // Retrieve Advanced Parameters
            mFullTemplate.SetSection(sectionName);
            if (mType == ParamFileTypes.BioWorks_20)
            {
                DefaultFASTAPath = mFullTemplate.GetParam("database_name");
                DefaultFASTAPath2 = "";
                NumberOfResultsToProcess = 500;
            }
            else if (mType == ParamFileTypes.BioWorks_30)
            {
                DefaultFASTAPath = mFullTemplate.GetParam("first_database_name");
                DefaultFASTAPath2 = mFullTemplate.GetParam("second_database_name");
                NumberOfResultsToProcess = SafeCastInt(mFullTemplate.GetParam("num_results"));
            }
            PeptideMassTolerance = (float) SafeCastDouble(mFullTemplate.GetParam("peptide_mass_tolerance"));
            if (mFullTemplate.GetParam("create_output_files") is not null)
            {
                CreateOutputFiles = SafeCastBool(mFullTemplate.GetParam("create_output_files"));
            }
            else
            {
                CreateOutputFiles = true;
            }
            mIonSeriesString = mFullTemplate.GetParam("ion_series");
            IonSeries = new IonSeries(mIonSeriesString);
            MaximumNumAAPerDynMod = SafeCastInt(mFullTemplate.GetParam("max_num_differential_AA_per_mod"));
            if (mType == ParamFileTypes.BioWorks_32)
            {
                MaximumNumDifferentialPerPeptide = SafeCastInt(mFullTemplate.GetParam("max_num_differential_per_peptide"));
            }
            FragmentIonTolerance = (float) SafeCastDouble(mFullTemplate.GetParam("fragment_ion_tolerance"));
            NumberOfOutputLines = SafeCastInt(mFullTemplate.GetParam("num_output_lines"));
            NumberOfDescriptionLines = SafeCastInt(mFullTemplate.GetParam("num_description_lines"));
            ShowFragmentIons = SafeCastBool(mFullTemplate.GetParam("show_fragment_ions"));
            PrintDuplicateReferences = SafeCastBool(mFullTemplate.GetParam("print_duplicate_references"));
            SelectedNucReadingFrame = (IAdvancedParams.FrameList)SafeCastInt(mFullTemplate.GetParam("enzyme_number"));
            RemovePrecursorPeak = SafeCastBool(mFullTemplate.GetParam("remove_precursor_peak"));
            IonCutoffPercentage = (float) SafeCastDouble(mFullTemplate.GetParam("ion_cutoff_percentage"));
            mProtMassFilterString = mFullTemplate.GetParam("protein_mass_filter");

            var protMassFilterList = mProtMassFilterString.Split(' ');
            if (protMassFilterList.Length > 0)
            {
                MinimumProteinMassToSearch = (float) SafeCastDouble(protMassFilterList[0]);
            }

            if (protMassFilterList.Length > 1)
            {
                MaximumProteinMassToSearch = (float) SafeCastDouble(protMassFilterList[1]);
            }

            NumberOfDetectedPeaksToMatch = SafeCastInt(mFullTemplate.GetParam("match_peak_count"));
            NumberOfAllowedDetectedPeakErrors = SafeCastInt(mFullTemplate.GetParam("match_peak_allowed_error"));
            MatchedPeakMassTolerance = (float) SafeCastDouble(mFullTemplate.GetParam("match_peak_tolerance"));
            AminoAcidsAllUpperCase = true;
            SequenceHeaderInfoToFilter = mFullTemplate.GetParam("sequence_header_filter");
            DMS_ID = -1;
        }

        [Obsolete("Unused")]
        private string GetDescription()
        {
            var fi = new FileInfo(mTemplateFilePath);
            TextReader tr = fi.OpenText();
            var s = tr.ReadLine();
            // Find the correct section block)
            while (s is not null)
            {
                if (s.Contains(";DMS_Description = "))
                {
                    return s.Substring(s.IndexOf(" = ", StringComparison.Ordinal) + 3);
                }
                s = tr.ReadLine();
            }

            return "";
        }

        private ParamFileTypes GetTemplateType()
        {
            var fi = new FileInfo(mTemplateFilePath);
            TextReader tr = fi.OpenText();
            var s = tr.ReadLine();

            while (s is not null)
            {
                ParamFileTypes type;
                if (s.ToLower().Contains("num_results = "))
                {
                    type = ParamFileTypes.BioWorks_31;
                    return type;
                }

                type = ParamFileTypes.BioWorks_20;
                return type;
            }

            return default;
        }

        private string ReturnMassFilterString(float minMass, float maxMass)
        {
            return minMass.ToString("0") + " " + maxMass.ToString("0");
        }

        private string GetFilePath(string templateFileName)
        {
            return templateFileName;
        }

        public static int SafeCastInt(object value)
        {
            if (value is int valueInt) return valueInt;
            if (value == null) return 0;

            var valueString = value.ToString().Trim();
            if (string.IsNullOrEmpty(valueString)) return 0;

            if (int.TryParse(valueString, out valueInt)) return valueInt;

            if (double.TryParse(valueString, out var valueDbl))
            {
                return (int) Math.Round(valueDbl);
            }

            return 0;
        }

        public static double SafeCastDouble(string value)
        {
            if (value == null) return 0;

            var valueString = value.Trim();
            if (string.IsNullOrEmpty(valueString)) return 0;

            if (double.TryParse(valueString, out var valueDbl))
            {
                return valueDbl;
            }

            return 0;
        }

        public static bool SafeCastBool(string value)
        {
            if (value == null) return false;

            var valueString = value.Trim();
            if (string.IsNullOrEmpty(valueString)) return false;

            if (bool.TryParse(valueString, out var valueBool))
            {
                return valueBool;
            }

            return false;
        }
    }
}
