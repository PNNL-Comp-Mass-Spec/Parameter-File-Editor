using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        private ParamFileTypes m_type;

        // Advanced Parameters
        private string m_ionSeriesString;
        private string m_protMassFilterString;

        private RetrieveParams m_fullTemplate;
        private string m_templateFilePath;

        public static Params BaseLineParamSet => MainProcess.BaseLineParamSet;

        public ParamFileTypes FileType => m_type;

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

        public Hashtable LoadedParamNames { get; set; } = new Hashtable();

        public void AddLoadedParamName(string ParameterName, string ParameterValue)
        {
            if (LoadedParamNames is null)
            {
                LoadedParamNames = new Hashtable();
            }
            if (!LoadedParamNames.ContainsKey(ParameterName))
            {
                LoadedParamNames.Add(ParameterName, ParameterValue);
            }
        }

        public EnzymeDetails RetrieveEnzymeDetails(int EnzymeListIndex)
        {
            return EnzymeList[EnzymeListIndex];
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

        public string ReturnMassFilter(float MinimumMassToFilter, float MaximumMassToFilter)
        {
            return ReturnMassFilterString(MinimumMassToFilter, MaximumMassToFilter);
        }

        public void LoadTemplate(string templateFileName)
        {
            LoadTemplateParams(templateFileName);
        }

        private void LoadTemplateParams(string templateFileName)
        {
            m_templateFilePath = GetFilePath(templateFileName);
            var m_getEnzymeList = new GetEnzymeBlockType(m_templateFilePath, DEF_ENZ_SECTION_NAME);
            EnzymeList = m_getEnzymeList.EnzymeList;

            m_type = GetTemplateType();

            string SectionName = "SEQUEST";

            m_fullTemplate = new RetrieveParams(m_templateFilePath);
            // Retrieve Basic Parameters
            m_fullTemplate.SetSection(SectionName);
            FileName = Path.GetFileName(m_templateFilePath);
            SelectedEnzymeIndex = SafeCastInt(m_fullTemplate.GetParam("enzyme_number"));
            SelectedEnzymeDetails = EnzymeList[SelectedEnzymeIndex]; // TODO: Add an item first!!
            SelectedEnzymeCleavagePosition = 1;
            MaximumNumberMissedCleavages = SafeCastInt(m_fullTemplate.GetParam("max_num_internal_cleavage_sites"));
            ParentMassType = (IBasicParams.MassTypeList)SafeCastInt(m_fullTemplate.GetParam("mass_type_parent"));
            FragmentMassType = (IBasicParams.MassTypeList)SafeCastInt(m_fullTemplate.GetParam("mass_type_fragment"));
            PartialSequenceToMatch = m_fullTemplate.GetParam("partial_sequence");
            DynamicMods = new DynamicMods(m_fullTemplate.GetParam("diff_search_options"));
            TermDynamicMods = new TermDynamicMods(m_fullTemplate.GetParam("term_diff_search_options"));
            StaticModificationsList = new StaticMods();

            // Get Static Mods
            StaticModificationsList.CtermPeptide = SafeCastDouble(m_fullTemplate.GetParam("add_Cterm_peptide"));
            StaticModificationsList.CtermProtein = SafeCastDouble(m_fullTemplate.GetParam("add_Cterm_protein"));
            StaticModificationsList.NtermPeptide = SafeCastDouble(m_fullTemplate.GetParam("add_Nterm_peptide"));
            StaticModificationsList.NtermProtein = SafeCastDouble(m_fullTemplate.GetParam("add_Nterm_protein"));
            StaticModificationsList.G_Glycine = SafeCastDouble(m_fullTemplate.GetParam("add_G_Glycine"));
            StaticModificationsList.A_Alanine = SafeCastDouble(m_fullTemplate.GetParam("add_A_Alanine"));
            StaticModificationsList.S_Serine = SafeCastDouble(m_fullTemplate.GetParam("add_S_Serine"));
            StaticModificationsList.P_Proline = SafeCastDouble(m_fullTemplate.GetParam("add_P_Proline"));
            StaticModificationsList.V_Valine = SafeCastDouble(m_fullTemplate.GetParam("add_V_Valine"));
            StaticModificationsList.T_Threonine = SafeCastDouble(m_fullTemplate.GetParam("add_T_Threonine"));
            StaticModificationsList.C_Cysteine = SafeCastDouble(m_fullTemplate.GetParam("add_C_Cysteine"));
            StaticModificationsList.L_Leucine = SafeCastDouble(m_fullTemplate.GetParam("add_L_Leucine"));
            StaticModificationsList.I_Isoleucine = SafeCastDouble(m_fullTemplate.GetParam("add_I_Isoleucine"));
            StaticModificationsList.X_LorI = SafeCastDouble(m_fullTemplate.GetParam("add_X_LorI"));
            StaticModificationsList.N_Asparagine = SafeCastDouble(m_fullTemplate.GetParam("add_N_Asparagine"));
            StaticModificationsList.O_Ornithine = SafeCastDouble(m_fullTemplate.GetParam("add_O_Ornithine"));
            StaticModificationsList.B_avg_NandD = SafeCastDouble(m_fullTemplate.GetParam("add_B_avg_NandD"));
            StaticModificationsList.D_Aspartic_Acid = SafeCastDouble(m_fullTemplate.GetParam("add_D_Aspartic_Acid"));
            StaticModificationsList.Q_Glutamine = SafeCastDouble(m_fullTemplate.GetParam("add_Q_Glutamine"));
            StaticModificationsList.K_Lysine = SafeCastDouble(m_fullTemplate.GetParam("add_K_Lysine"));
            StaticModificationsList.Z_avg_QandE = SafeCastDouble(m_fullTemplate.GetParam("add_Z_avg_QandE"));
            StaticModificationsList.E_Glutamic_Acid = SafeCastDouble(m_fullTemplate.GetParam("add_E_Glutamic_Acid"));
            StaticModificationsList.M_Methionine = SafeCastDouble(m_fullTemplate.GetParam("add_M_Methionine"));
            StaticModificationsList.H_Histidine = SafeCastDouble(m_fullTemplate.GetParam("add_H_Histidine"));
            StaticModificationsList.F_Phenylalanine = SafeCastDouble(m_fullTemplate.GetParam("add_F_Phenylalanine"));
            StaticModificationsList.R_Arginine = SafeCastDouble(m_fullTemplate.GetParam("add_R_Arginine"));
            StaticModificationsList.Y_Tyrosine = SafeCastDouble(m_fullTemplate.GetParam("add_Y_Tyrosine"));
            StaticModificationsList.W_Tryptophan = SafeCastDouble(m_fullTemplate.GetParam("add_W_Tryptophan"));

            // add code to check for existence of isotopic mods

            // Retrieve Advanced Parameters
            m_fullTemplate.SetSection(SectionName);
            if (m_type == ParamFileTypes.BioWorks_20)
            {
                DefaultFASTAPath = m_fullTemplate.GetParam("database_name");
                DefaultFASTAPath2 = "";
                NumberOfResultsToProcess = 500;
            }
            else if (m_type == ParamFileTypes.BioWorks_30)
            {
                DefaultFASTAPath = m_fullTemplate.GetParam("first_database_name");
                DefaultFASTAPath2 = m_fullTemplate.GetParam("second_database_name");
                NumberOfResultsToProcess = SafeCastInt(m_fullTemplate.GetParam("num_results"));
            }
            PeptideMassTolerance = (float) SafeCastDouble(m_fullTemplate.GetParam("peptide_mass_tolerance"));
            if (m_fullTemplate.GetParam("create_output_files") is not null)
            {
                CreateOutputFiles = SafeCastBool(m_fullTemplate.GetParam("create_output_files"));
            }
            else
            {
                CreateOutputFiles = true;
            }
            m_ionSeriesString = m_fullTemplate.GetParam("ion_series");
            IonSeries = new IonSeries(m_ionSeriesString);
            MaximumNumAAPerDynMod = SafeCastInt(m_fullTemplate.GetParam("max_num_differential_AA_per_mod"));
            if (m_type == ParamFileTypes.BioWorks_32)
            {
                MaximumNumDifferentialPerPeptide = SafeCastInt(m_fullTemplate.GetParam("max_num_differential_per_peptide"));
            }
            FragmentIonTolerance = (float) SafeCastDouble(m_fullTemplate.GetParam("fragment_ion_tolerance"));
            NumberOfOutputLines = SafeCastInt(m_fullTemplate.GetParam("num_output_lines"));
            NumberOfDescriptionLines = SafeCastInt(m_fullTemplate.GetParam("num_description_lines"));
            ShowFragmentIons = SafeCastBool(m_fullTemplate.GetParam("show_fragment_ions"));
            PrintDuplicateReferences = SafeCastBool(m_fullTemplate.GetParam("print_duplicate_references"));
            SelectedNucReadingFrame = (IAdvancedParams.FrameList)SafeCastInt(m_fullTemplate.GetParam("enzyme_number"));
            RemovePrecursorPeak = SafeCastBool(m_fullTemplate.GetParam("remove_precursor_peak"));
            IonCutoffPercentage = (float) SafeCastDouble(m_fullTemplate.GetParam("ion_cutoff_percentage"));
            m_protMassFilterString = m_fullTemplate.GetParam("protein_mass_filter");

            var protMassFilterList = m_protMassFilterString.Split(' ');
            if (protMassFilterList.Count() > 0)
            {
                MinimumProteinMassToSearch = (float) SafeCastDouble(protMassFilterList[0]);
            }

            if (protMassFilterList.Count() > 1)
            {
                MaximumProteinMassToSearch = (float) SafeCastDouble(protMassFilterList[1]);
            }

            NumberOfDetectedPeaksToMatch = SafeCastInt(m_fullTemplate.GetParam("match_peak_count"));
            NumberOfAllowedDetectedPeakErrors = SafeCastInt(m_fullTemplate.GetParam("match_peak_allowed_error"));
            MatchedPeakMassTolerance = (float) SafeCastDouble(m_fullTemplate.GetParam("match_peak_tolerance"));
            AminoAcidsAllUpperCase = true;
            SequenceHeaderInfoToFilter = m_fullTemplate.GetParam("sequence_header_filter");
            DMS_ID = -1;
        }

        [Obsolete("Unused")]
        private string GetDescription()
        {
            string s;
            string desc;

            FileInfo fi;
            TextReader tr;

            fi = new FileInfo(m_templateFilePath);
            tr = fi.OpenText();
            s = tr.ReadLine();
            // Find the correct section block)
            while (s is not null)
            {
                if (s.Contains(";DMS_Description = "))
                {
                    desc = s.Substring(s.IndexOf(" = ") + 3);
                    return desc;
                }
                s = tr.ReadLine();
            }

            return s;
        }

        private ParamFileTypes GetTemplateType()
        {
            string s;
            ParamFileTypes type;

            FileInfo fi;
            TextReader tr;

            fi = new FileInfo(m_templateFilePath);
            tr = fi.OpenText();
            s = tr.ReadLine();

            while (s is not null)
            {
                if (s.ToLower().Contains("num_results = "))
                {
                    type = ParamFileTypes.BioWorks_31;
                    return type;
                }
                else
                {
                    type = ParamFileTypes.BioWorks_20;
                    return type;
                }
            }

            return default;
        }

        private string ReturnMassFilterString(
            float minMass,
            float maxMass)
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
