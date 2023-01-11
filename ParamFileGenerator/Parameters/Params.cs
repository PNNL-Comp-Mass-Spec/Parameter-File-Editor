using System;
using System.Collections;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

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

        public static Params BaseLineParamSet
        {
            get
            {
                return MainProcess.BaseLineParamSet;
            }
        }

        public ParamFileTypes FileType
        {
            get
            {
                return m_type;
            }
        }

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

        public IsoMods IsotopicMods { get; set; }
        IsoMods IBasicParams.IsotopicModificationsList { get => IsotopicMods; set => IsotopicMods = value; }

        public int MaximumNumAAPerDynMod { get; set; }

        public int MaximumNumDifferentialPerPeptide { get; set; }
        int IAdvancedParams.MaximumDifferentialPerPeptide { get => MaximumNumDifferentialPerPeptide; set => MaximumNumDifferentialPerPeptide = value; }

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
            get
            {
                return (int)SelectedNucReadingFrame;
            }
            set
            {
                SelectedNucReadingFrame = (IAdvancedParams.FrameList)value;
            }
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

        public EnzymeCollection EnzymeList { get; set; }

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
            EnzymeList = new EnzymeCollection();
            SelectedEnzymeDetails = new EnzymeDetails();
            DynamicMods = new DynamicMods();
            StaticModificationsList = new StaticMods();
            IsotopicMods = new IsoMods();
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
            {
                ref var withBlock = ref m_fullTemplate;
                withBlock.SetSection(SectionName);
                FileName = Path.GetFileName(m_templateFilePath);
                SelectedEnzymeIndex = Conversions.ToInteger(withBlock.GetParam("enzyme_number"));
                SelectedEnzymeDetails = EnzymeList[SelectedEnzymeIndex];
                SelectedEnzymeCleavagePosition = 1;
                MaximumNumberMissedCleavages = Conversions.ToInteger(withBlock.GetParam("max_num_internal_cleavage_sites"));
                ParentMassType = (IBasicParams.MassTypeList)Conversions.ToInteger(withBlock.GetParam("mass_type_parent"));
                FragmentMassType = (IBasicParams.MassTypeList)Conversions.ToInteger(withBlock.GetParam("mass_type_fragment"));
                PartialSequenceToMatch = withBlock.GetParam("partial_sequence");
                DynamicMods = new DynamicMods(withBlock.GetParam("diff_search_options"));
                TermDynamicMods = new TermDynamicMods(withBlock.GetParam("term_diff_search_options"));
                StaticModificationsList = new StaticMods();


                // Get Static Mods
                StaticModificationsList.CtermPeptide = (double)Conversions.ToSingle(withBlock.GetParam("add_Cterm_peptide"));
                StaticModificationsList.CtermProtein = (double)Conversions.ToSingle(withBlock.GetParam("add_Cterm_protein"));
                StaticModificationsList.NtermPeptide = (double)Conversions.ToSingle(withBlock.GetParam("add_Nterm_peptide"));
                StaticModificationsList.NtermProtein = (double)Conversions.ToSingle(withBlock.GetParam("add_Nterm_protein"));
                StaticModificationsList.G_Glycine = (double)Conversions.ToSingle(withBlock.GetParam("add_G_Glycine"));
                StaticModificationsList.A_Alanine = (double)Conversions.ToSingle(withBlock.GetParam("add_A_Alanine"));
                StaticModificationsList.S_Serine = (double)Conversions.ToSingle(withBlock.GetParam("add_S_Serine"));
                StaticModificationsList.P_Proline = (double)Conversions.ToSingle(withBlock.GetParam("add_P_Proline"));
                StaticModificationsList.V_Valine = (double)Conversions.ToSingle(withBlock.GetParam("add_V_Valine"));
                StaticModificationsList.T_Threonine = (double)Conversions.ToSingle(withBlock.GetParam("add_T_Threonine"));
                StaticModificationsList.C_Cysteine = (double)Conversions.ToSingle(withBlock.GetParam("add_C_Cysteine"));
                StaticModificationsList.L_Leucine = (double)Conversions.ToSingle(withBlock.GetParam("add_L_Leucine"));
                StaticModificationsList.I_Isoleucine = (double)Conversions.ToSingle(withBlock.GetParam("add_I_Isoleucine"));
                StaticModificationsList.X_LorI = (double)Conversions.ToSingle(withBlock.GetParam("add_X_LorI"));
                StaticModificationsList.N_Asparagine = (double)Conversions.ToSingle(withBlock.GetParam("add_N_Asparagine"));
                StaticModificationsList.O_Ornithine = (double)Conversions.ToSingle(withBlock.GetParam("add_O_Ornithine"));
                StaticModificationsList.B_avg_NandD = (double)Conversions.ToSingle(withBlock.GetParam("add_B_avg_NandD"));
                StaticModificationsList.D_Aspartic_Acid = (double)Conversions.ToSingle(withBlock.GetParam("add_D_Aspartic_Acid"));
                StaticModificationsList.Q_Glutamine = (double)Conversions.ToSingle(withBlock.GetParam("add_Q_Glutamine"));
                StaticModificationsList.K_Lysine = (double)Conversions.ToSingle(withBlock.GetParam("add_K_Lysine"));
                StaticModificationsList.Z_avg_QandE = (double)Conversions.ToSingle(withBlock.GetParam("add_Z_avg_QandE"));
                StaticModificationsList.E_Glutamic_Acid = (double)Conversions.ToSingle(withBlock.GetParam("add_E_Glutamic_Acid"));
                StaticModificationsList.M_Methionine = (double)Conversions.ToSingle(withBlock.GetParam("add_M_Methionine"));
                StaticModificationsList.H_Histidine = (double)Conversions.ToSingle(withBlock.GetParam("add_H_Histidine"));
                StaticModificationsList.F_Phenylalanine = (double)Conversions.ToSingle(withBlock.GetParam("add_F_Phenylalanine"));
                StaticModificationsList.R_Arginine = (double)Conversions.ToSingle(withBlock.GetParam("add_R_Arginine"));
                StaticModificationsList.Y_Tyrosine = (double)Conversions.ToSingle(withBlock.GetParam("add_Y_Tyrosine"));
                StaticModificationsList.W_Tryptophan = (double)Conversions.ToSingle(withBlock.GetParam("add_W_Tryptophan"));
            }

            // add code to check for existence of isotopic mods



            // Retrieve Advanced Parameters
            {
                ref var withBlock1 = ref m_fullTemplate;
                withBlock1.SetSection(SectionName);
                if (m_type == ParamFileTypes.BioWorks_20)
                {
                    DefaultFASTAPath = withBlock1.GetParam("database_name");
                    DefaultFASTAPath2 = "";
                    NumberOfResultsToProcess = 500;
                }
                else if (m_type == ParamFileTypes.BioWorks_30)
                {
                    DefaultFASTAPath = withBlock1.GetParam("first_database_name");
                    DefaultFASTAPath2 = withBlock1.GetParam("second_database_name");
                    NumberOfResultsToProcess = Conversions.ToInteger(withBlock1.GetParam("num_results"));
                }
                PeptideMassTolerance = Conversions.ToSingle(withBlock1.GetParam("peptide_mass_tolerance"));
                if (withBlock1.GetParam("create_output_files") is not null)
                {
                    CreateOutputFiles = Conversions.ToBoolean(withBlock1.GetParam("create_output_files"));
                }
                else
                {
                    CreateOutputFiles = true;
                }
                m_ionSeriesString = withBlock1.GetParam("ion_series");
                IonSeries = new IonSeries(m_ionSeriesString);
                MaximumNumAAPerDynMod = Conversions.ToInteger(withBlock1.GetParam("max_num_differential_AA_per_mod"));
                if (m_type == ParamFileTypes.BioWorks_32)
                {
                    MaximumNumDifferentialPerPeptide = Conversions.ToInteger(withBlock1.GetParam("max_num_differential_per_peptide"));
                }
                FragmentIonTolerance = Conversions.ToSingle(withBlock1.GetParam("fragment_ion_tolerance"));
                NumberOfOutputLines = Conversions.ToInteger(withBlock1.GetParam("num_output_lines"));
                NumberOfDescriptionLines = Conversions.ToInteger(withBlock1.GetParam("num_description_lines"));
                ShowFragmentIons = Conversions.ToBoolean(withBlock1.GetParam("show_fragment_ions"));
                PrintDuplicateReferences = Conversions.ToBoolean(withBlock1.GetParam("print_duplicate_references"));
                SelectedNucReadingFrame = (IAdvancedParams.FrameList)Conversions.ToInteger(withBlock1.GetParam("enzyme_number"));
                RemovePrecursorPeak = Conversions.ToBoolean(withBlock1.GetParam("remove_precursor_peak"));
                IonCutoffPercentage = Conversions.ToSingle(withBlock1.GetParam("ion_cutoff_percentage"));
                m_protMassFilterString = withBlock1.GetParam("protein_mass_filter");

                var protMassFilterList = m_protMassFilterString.Split(' ');
                if (protMassFilterList.Count() > 0)
                {
                    MinimumProteinMassToSearch = Conversions.ToSingle(protMassFilterList[0]);
                }

                if (protMassFilterList.Count() > 1)
                {
                    MaximumProteinMassToSearch = Conversions.ToSingle(protMassFilterList[1]);
                }

                NumberOfDetectedPeaksToMatch = Conversions.ToInteger(withBlock1.GetParam("match_peak_count"));
                NumberOfAllowedDetectedPeakErrors = Conversions.ToInteger(withBlock1.GetParam("match_peak_allowed_error"));
                MatchedPeakMassTolerance = Conversions.ToSingle(withBlock1.GetParam("match_peak_tolerance"));
                AminoAcidsAllUpperCase = true;
                SequenceHeaderInfoToFilter = withBlock1.GetParam("sequence_header_filter");
                DMS_ID = -1;
            }

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
                if (Strings.InStr(s, ";DMS_Description = ") > 0)
                {
                    desc = Strings.Mid(s, Strings.InStr(s, " = ") + 3);
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
                if (Strings.InStr(s.ToLower(), "num_results = ") > 0)
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

        private string ReturnMassFilterString(float minMass, float maxMass)
        {

            return Strings.Format(minMass.ToString(), "0") + " " + Strings.Format(maxMass.ToString(), "0");

        }

        private string GetFilePath(string templateFileName)
        {
            return templateFileName;
        }

    }
}
