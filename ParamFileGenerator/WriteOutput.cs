using System;
using System.Collections.Generic;
using System.IO;
using ParamFileGenerator.MakeParams;

namespace ParamFileGenerator
{
    public class WriteOutput
    {
        public bool WriteOutputFile(Params paramList, string outputPathName, IGenerateFile.ParamFileType fileType)
        {
            var sequestParamList = ConvertSequestParamsToList(paramList, fileType);
            OutputTextParamFile(sequestParamList, outputPathName);
            return true;
        }

        public void WriteDataTableToOutputFile(List<List<string>> tableToWrite, string outputFilePath, List<string> headerNames)
        {
            using var writer = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite));

            writer.WriteLine(string.Join("\t", headerNames));

            foreach (var tableRow in tableToWrite)
            {
                writer.WriteLine(string.Join("\t", tableRow));
            }
        }

        private IEnumerable<string> ConvertSequestParamsToList(Params paramsIn, IGenerateFile.ParamFileType type)
        {
            var paramList = new List<string>();

            var maxDynMods = default(int);

            if (type == IGenerateFile.ParamFileType.BioWorks_20)
            {
                paramList.Add("[Sequest]");
                maxDynMods = 3;
            }
            else if (type == IGenerateFile.ParamFileType.BioWorks_30 || type == IGenerateFile.ParamFileType.BioWorks_31 || type == IGenerateFile.ParamFileType.BioWorks_32)
            {
                paramList.Add("[Sequest]");
                maxDynMods = 6;
            }

            paramList.Add(";DMS_Description = " + paramsIn.Description);
            if (type == (int)Params.ParamFileTypes.BioWorks_20)
            {
                paramList.Add("database_name = " + paramsIn.DefaultFASTAPath);
            }
            else if ((int)type == (int)Params.ParamFileTypes.BioWorks_30 || type == IGenerateFile.ParamFileType.BioWorks_31 || type == IGenerateFile.ParamFileType.BioWorks_32)
            {
                paramList.Add("first_database_name = " + paramsIn.DefaultFASTAPath);
                paramList.Add("second_database_name = " + paramsIn.DefaultFASTAPath2);
            }

            paramList.Add("peptide_mass_tolerance = " + paramsIn.PeptideMassTolerance.ToString("0.0000"));
            if (type == IGenerateFile.ParamFileType.BioWorks_32)
            {
                paramList.Add("peptide_mass_units = " + paramsIn.PeptideMassUnits.ToString());
            }

            paramList.Add("ion_series = " + paramsIn.IonSeries.ReturnIonString());
            paramList.Add("fragment_ion_tolerance = " + paramsIn.FragmentIonTolerance.ToString("0.0000"));
            if (type == IGenerateFile.ParamFileType.BioWorks_32)
            {
                // MEM Note from February 2010
                // Our version of Sequest [ TurboSEQUEST - PVM Slave v.27 (rev. 12), (c) 1998-2005 ]
                //  does not support mmu or ppm for Fragment Mass Units
                // In fact, it's possible it completely ignores the fragment_ion_units entry in the .params file
                // Thus, it is advisable you always use fragment_ion_units = 0  (which means Da)

                paramList.Add("fragment_ion_units = " + paramsIn.FragmentMassUnits.ToString());
            }

            paramList.Add("num_output_lines = " + paramsIn.NumberOfOutputLines.ToString());
            if ((int)type == (int)Params.ParamFileTypes.BioWorks_30 || type == IGenerateFile.ParamFileType.BioWorks_31 || type == IGenerateFile.ParamFileType.BioWorks_32)
            {
                paramList.Add("num_results = " + paramsIn.NumberOfResultsToProcess);
            }

            paramList.Add("num_description_lines = " + paramsIn.NumberOfDescriptionLines.ToString());
            //if ((int)type == (int)Params.ParamFileTypes.BioWorks_30 || type == IGenerateFile.ParamFileType.BioWorks_31 || type == IGenerateFile.ParamFileType.BioWorks_32)
            //{
            //    paramList.add("show_fragment_ions = 0");
            //}
            //else
            //{
            paramList.Add("show_fragment_ions = " + ConvertBoolToInteger(paramsIn.ShowFragmentIons).ToString());
            //}
            paramList.Add("print_duplicate_references = " + ConvertBoolToInteger(paramsIn.PrintDuplicateReferences).ToString());

            if (type != IGenerateFile.ParamFileType.BioWorks_32)
            {
                paramList.Add("enzyme_number = " + paramsIn.SelectedEnzymeIndex.ToString());
            }
            else
            {
                if (paramsIn.SelectedEnzymeIndex >= paramsIn.EnzymeList.Count)
                {
                    throw new IndexOutOfRangeException("Enzyme ID " + paramsIn.SelectedEnzymeIndex + " is not recognized; template file is out of date");
                }
                var enz = paramsIn.EnzymeList[paramsIn.SelectedEnzymeIndex];
                paramList.Add("enzyme_info = " + enz.ReturnBW32EnzymeInfoString(paramsIn.SelectedEnzymeCleavagePosition));
            }

            if (type == IGenerateFile.ParamFileType.BioWorks_32)
            {
                paramList.Add("max_num_differential_per_peptide = " + paramsIn.MaximumNumDifferentialPerPeptide.ToString());
            }
            paramList.Add("max_num_differential_AA_per_mod = " + paramsIn.MaximumNumAAPerDynMod.ToString());

            paramList.Add("diff_search_options = " + paramsIn.DynamicMods.ReturnDynModString(maxDynMods));

            paramList.Add("term_diff_search_options = " + paramsIn.TermDynamicMods.ReturnDynModString(0));

            if (type == IGenerateFile.ParamFileType.BioWorks_32)
            {
                paramList.Add("use_phospho_fragmentation = " + paramsIn.UsePhosphoFragmentation.ToString());
            }

            paramList.Add("nucleotide_reading_frame = " + ((int)paramsIn.SelectedNucReadingFrame).ToString());
            paramList.Add("mass_type_parent = " + ((int)paramsIn.ParentMassType).ToString());
            paramList.Add("mass_type_fragment = " + ((int)paramsIn.FragmentMassType).ToString());
            if (type == IGenerateFile.ParamFileType.BioWorks_32)
            {
                paramList.Add("normalize_xcorr = " + "0");
            }

            paramList.Add("remove_precursor_peak = " + ConvertBoolToInteger(paramsIn.RemovePrecursorPeak).ToString());
            paramList.Add("ion_cutoff_percentage = " + paramsIn.IonCutoffPercentage.ToString("0.0000"));
            paramList.Add("max_num_internal_cleavage_sites = " + paramsIn.MaximumNumberMissedCleavages.ToString());
            paramList.Add("protein_mass_filter = " + paramsIn.ReturnMassFilter(paramsIn.MinimumProteinMassToSearch, paramsIn.MaximumProteinMassToSearch));
            paramList.Add("match_peak_count = " + paramsIn.NumberOfDetectedPeaksToMatch.ToString());
            paramList.Add("match_peak_allowed_error = " + paramsIn.NumberOfAllowedDetectedPeakErrors.ToString());
            paramList.Add("match_peak_tolerance = " + paramsIn.MatchedPeakMassTolerance.ToString("0.0000"));
            paramList.Add("create_output_files = " + ConvertBoolToInteger(paramsIn.CreateOutputFiles).ToString());

            //paramList.add("residues_in_upper_case = " + ConvertBoolToInteger(params.AminoAcidsAllUpperCase).ToString());
            paramList.Add("partial_sequence = " + paramsIn.PartialSequenceToMatch);
            paramList.Add("sequence_header_filter = " + paramsIn.SequenceHeaderInfoToFilter);
            paramList.Add("");
            paramList.Add("add_Cterm_peptide = " + paramsIn.StaticModificationsList.CtermPeptide.ToString("0.0000"));
            paramList.Add("add_Cterm_protein = " + paramsIn.StaticModificationsList.CtermProtein.ToString("0.0000"));
            paramList.Add("add_Nterm_peptide = " + paramsIn.StaticModificationsList.NtermPeptide.ToString("0.0000"));
            paramList.Add("add_Nterm_protein = " + paramsIn.StaticModificationsList.NtermProtein.ToString("0.0000"));
            paramList.Add("add_G_Glycine = " + paramsIn.StaticModificationsList.G_Glycine.ToString("0.0000"));
            paramList.Add("add_A_Alanine = " + paramsIn.StaticModificationsList.A_Alanine.ToString("0.0000"));
            paramList.Add("add_S_Serine = " + paramsIn.StaticModificationsList.S_Serine.ToString("0.0000"));
            paramList.Add("add_P_Proline = " + paramsIn.StaticModificationsList.P_Proline.ToString("0.0000"));
            paramList.Add("add_V_Valine = " + paramsIn.StaticModificationsList.V_Valine.ToString("0.0000"));
            paramList.Add("add_T_Threonine = " + paramsIn.StaticModificationsList.T_Threonine.ToString("0.0000"));
            paramList.Add("add_C_Cysteine = " + paramsIn.StaticModificationsList.C_Cysteine.ToString("0.0000"));
            paramList.Add("add_L_Leucine = " + paramsIn.StaticModificationsList.L_Leucine.ToString("0.0000"));
            paramList.Add("add_I_Isoleucine = " + paramsIn.StaticModificationsList.I_Isoleucine.ToString("0.0000"));
            paramList.Add("add_X_LorI = " + paramsIn.StaticModificationsList.X_LorI.ToString("0.0000"));
            paramList.Add("add_N_Asparagine = " + paramsIn.StaticModificationsList.N_Asparagine.ToString("0.0000"));
            paramList.Add("add_O_Ornithine = " + paramsIn.StaticModificationsList.O_Ornithine.ToString("0.0000"));
            paramList.Add("add_B_avg_NandD = " + paramsIn.StaticModificationsList.B_avg_NandD.ToString("0.0000"));
            paramList.Add("add_D_Aspartic_Acid = " + paramsIn.StaticModificationsList.D_Aspartic_Acid.ToString("0.0000"));
            paramList.Add("add_Q_Glutamine = " + paramsIn.StaticModificationsList.Q_Glutamine.ToString("0.0000"));
            paramList.Add("add_K_Lysine = " + paramsIn.StaticModificationsList.K_Lysine.ToString("0.0000"));
            paramList.Add("add_Z_avg_QandE = " + paramsIn.StaticModificationsList.Z_avg_QandE.ToString("0.0000"));
            paramList.Add("add_E_Glutamic_Acid = " + paramsIn.StaticModificationsList.E_Glutamic_Acid.ToString("0.0000"));
            paramList.Add("add_M_Methionine = " + paramsIn.StaticModificationsList.M_Methionine.ToString("0.0000"));
            paramList.Add("add_H_Histidine = " + paramsIn.StaticModificationsList.H_Histidine.ToString("0.0000"));
            paramList.Add("add_F_Phenylalanine = " + paramsIn.StaticModificationsList.F_Phenylalanine.ToString("0.0000"));
            paramList.Add("add_R_Arginine = " + paramsIn.StaticModificationsList.R_Arginine.ToString("0.0000"));
            paramList.Add("add_Y_Tyrosine = " + paramsIn.StaticModificationsList.Y_Tyrosine.ToString("0.0000"));
            paramList.Add("add_W_Tryptophan = " + paramsIn.StaticModificationsList.W_Tryptophan.ToString("0.0000"));
            if (type == IGenerateFile.ParamFileType.BioWorks_32)
            {
                paramList.Add("add_J_user_amino_acid = 0.0000");
                paramList.Add("add_U_user_amino_acid = 0.0000");          // Note: you'd use 150.9536 for selenocysteine.  However, manual testing by Sam Purvine in 2010 showed that our version of Sequest ignores this parameter
            }

            paramList.Add("");

            if (type != IGenerateFile.ParamFileType.BioWorks_32)
            {
                paramList.Add("[SEQUEST_ENZYME_INFO]");
                foreach (var item in paramsIn.EnzymeList)
                {
                    paramList.Add(item.ReturnEnzymeString());
                }
            }

            return paramList;
        }

        private void OutputTextParamFile(IEnumerable<string> paramList, string outputPath)
        {
            using var writer = new StreamWriter(outputPath);

            foreach (var item in paramList)
            {
                writer.WriteLine(item);
            }
        }

        private int ConvertBoolToInteger(bool value)
        {
            return value ? 1 : 0;
        }
    }
}
