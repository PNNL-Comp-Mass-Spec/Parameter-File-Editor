using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using ParamFileGenerator.MakeParams;

namespace ParamFileGenerator
{

    public class WriteOutput
    {

        public bool WriteOutputFile(Params @params, string outputPathName, IGenerateFile.ParamFileType fileType)
        {

            var sequestParamList = ConvertSequestParamsToList(@params, fileType);
            OutputTextParamFile(sequestParamList, outputPathName);
            return true;

        }

        public void WriteDataTableToOutputFile(List<List<string>> tableToWrite, string outputFilePath, List<string> headerNames)
        {

            using (var writer = new StreamWriter(new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)))
            {
                writer.WriteLine(string.Join(Conversions.ToString(ControlChars.Tab), headerNames));

                foreach (var tableRow in tableToWrite)
                    writer.WriteLine(string.Join(Conversions.ToString(ControlChars.Tab), tableRow));
            }

        }

        private IEnumerable<string> ConvertSequestParamsToList(Params @params, IGenerateFile.ParamFileType type)
        {

            var paramList = new List<string>();

            var maxDynMods = default(int);

            if (type == IGenerateFile.ParamFileType.BioWorks_20)
            {
                paramList.Add("[Sequest]");
                maxDynMods = 3;
            }
            else if (type == IGenerateFile.ParamFileType.BioWorks_30 | type == IGenerateFile.ParamFileType.BioWorks_31 | type == IGenerateFile.ParamFileType.BioWorks_32)
            {
                paramList.Add("[Sequest]");
                maxDynMods = 6;
            }
            paramList.Add(";DMS_Description = " + @params.Description);
            if (type == (int)Params.ParamFileTypes.BioWorks_20)
            {
                paramList.Add("database_name = " + @params.DefaultFASTAPath);
            }
            else if ((int)type == (int)Params.ParamFileTypes.BioWorks_30 | type == IGenerateFile.ParamFileType.BioWorks_31 | type == IGenerateFile.ParamFileType.BioWorks_32)
            {
                paramList.Add("first_database_name = " + @params.DefaultFASTAPath);
                paramList.Add("second_database_name = " + @params.DefaultFASTAPath2);
            }
            paramList.Add("peptide_mass_tolerance = " + Strings.Format(@params.PeptideMassTolerance, "0.0000").ToString());
            if (type == IGenerateFile.ParamFileType.BioWorks_32)
            {
                paramList.Add("peptide_mass_units = " + @params.PeptideMassUnits.ToString());
            }
            paramList.Add("ion_series = " + @params.IonSeries.ReturnIonString());
            paramList.Add("fragment_ion_tolerance = " + Strings.Format(@params.FragmentIonTolerance, "0.0000").ToString());
            if (type == IGenerateFile.ParamFileType.BioWorks_32)
            {

                // MEM Note from February 2010
                // Our version of Sequest [ TurboSEQUEST - PVM Slave v.27 (rev. 12), (c) 1998-2005 ]
                // does not support mmu or ppm for Fragment Mass Units
                // In fact, it's possible it completely ignores the fragment_ion_units entry in the .params file
                // Thus, it is advisable you always use fragment_ion_units = 0  (which means Da)

                paramList.Add("fragment_ion_units = " + @params.FragmentMassUnits.ToString());
            }

            paramList.Add("num_output_lines = " + @params.NumberOfOutputLines.ToString());
            if ((int)type == (int)Params.ParamFileTypes.BioWorks_30 | type == IGenerateFile.ParamFileType.BioWorks_31 | type == IGenerateFile.ParamFileType.BioWorks_32)
            {
                paramList.Add("num_results = " + @params.NumberOfResultsToProcess);
            }
            paramList.Add("num_description_lines = " + @params.NumberOfDescriptionLines.ToString());
            // If type = Params.ParamFileTypes.BioWorks_30 Or type = MakeParams.IGenerateFile.ParamFileType.BioWorks_31 Or type = MakeParams.IGenerateFile.ParamFileType.BioWorks_32 Then
            // paramList.add("show_fragment_ions = 0")
            // Else
            paramList.Add("show_fragment_ions = " + ConvertBoolToInteger(@params.ShowFragmentIons).ToString());
            // End If
            paramList.Add("print_duplicate_references = " + ConvertBoolToInteger(@params.PrintDuplicateReferences).ToString());

            if (!(type == IGenerateFile.ParamFileType.BioWorks_32))
            {
                paramList.Add("enzyme_number = " + @params.SelectedEnzymeIndex.ToString());
            }
            else
            {
                if (@params.SelectedEnzymeIndex >= @params.EnzymeList.Count)
                {
                    throw new IndexOutOfRangeException("Enzyme ID " + @params.SelectedEnzymeIndex + " is not recognized; template file is out of date");
                }
                var enz = @params.EnzymeList[@params.SelectedEnzymeIndex];
                paramList.Add("enzyme_info = " + enz.ReturnBW32EnzymeInfoString(@params.SelectedEnzymeCleavagePosition));
            }
            if (type == IGenerateFile.ParamFileType.BioWorks_32)
            {
                paramList.Add("max_num_differential_per_peptide = " + @params.MaximumNumDifferentialPerPeptide.ToString());
            }
            paramList.Add("max_num_differential_AA_per_mod = " + @params.MaximumNumAAPerDynMod.ToString());


            paramList.Add("diff_search_options = " + @params.DynamicMods.ReturnDynModString(maxDynMods));

            paramList.Add("term_diff_search_options = " + @params.TermDynamicMods.ReturnDynModString(0));

            if (type == IGenerateFile.ParamFileType.BioWorks_32)
            {
                paramList.Add("use_phospho_fragmentation = " + @params.UsePhosphoFragmentation.ToString());
            }

            paramList.Add("nucleotide_reading_frame = " + ((int)@params.SelectedNucReadingFrame).ToString());
            paramList.Add("mass_type_parent = " + ((int)@params.ParentMassType).ToString());
            paramList.Add("mass_type_fragment = " + ((int)@params.FragmentMassType).ToString());
            if (type == IGenerateFile.ParamFileType.BioWorks_32)
            {
                paramList.Add("normalize_xcorr = " + "0");
            }
            paramList.Add("remove_precursor_peak = " + ConvertBoolToInteger(@params.RemovePrecursorPeak).ToString());
            paramList.Add("ion_cutoff_percentage = " + Strings.Format(@params.IonCutoffPercentage, "0.0000").ToString());
            paramList.Add("max_num_internal_cleavage_sites = " + @params.MaximumNumberMissedCleavages.ToString());
            paramList.Add("protein_mass_filter = " + @params.ReturnMassFilter(@params.MinimumProteinMassToSearch, @params.MaximumProteinMassToSearch));
            paramList.Add("match_peak_count = " + @params.NumberOfDetectedPeaksToMatch.ToString());
            paramList.Add("match_peak_allowed_error = " + @params.NumberOfAllowedDetectedPeakErrors.ToString());
            paramList.Add("match_peak_tolerance = " + Strings.Format(@params.MatchedPeakMassTolerance, "0.0000").ToString());
            paramList.Add("create_output_files = " + ConvertBoolToInteger(@params.CreateOutputFiles).ToString());

            // paramList.add("residues_in_upper_case = " & ConvertBoolToInteger(params.AminoAcidsAllUpperCase).ToString)
            paramList.Add("partial_sequence = " + @params.PartialSequenceToMatch);
            paramList.Add("sequence_header_filter = " + @params.SequenceHeaderInfoToFilter);
            paramList.Add("");
            paramList.Add("add_Cterm_peptide = " + Strings.Format(@params.StaticModificationsList.CtermPeptide, "0.0000").ToString());
            paramList.Add("add_Cterm_protein = " + Strings.Format(@params.StaticModificationsList.CtermProtein, "0.0000").ToString());
            paramList.Add("add_Nterm_peptide = " + Strings.Format(@params.StaticModificationsList.NtermPeptide, "0.0000").ToString());
            paramList.Add("add_Nterm_protein = " + Strings.Format(@params.StaticModificationsList.NtermProtein, "0.0000").ToString());
            paramList.Add("add_G_Glycine = " + Strings.Format(@params.StaticModificationsList.G_Glycine, "0.0000").ToString());
            paramList.Add("add_A_Alanine = " + Strings.Format(@params.StaticModificationsList.A_Alanine, "0.0000").ToString());
            paramList.Add("add_S_Serine = " + Strings.Format(@params.StaticModificationsList.S_Serine, "0.0000").ToString());
            paramList.Add("add_P_Proline = " + Strings.Format(@params.StaticModificationsList.P_Proline, "0.0000").ToString());
            paramList.Add("add_V_Valine = " + Strings.Format(@params.StaticModificationsList.V_Valine, "0.0000").ToString());
            paramList.Add("add_T_Threonine = " + Strings.Format(@params.StaticModificationsList.T_Threonine, "0.0000").ToString());
            paramList.Add("add_C_Cysteine = " + Strings.Format(@params.StaticModificationsList.C_Cysteine, "0.0000").ToString());
            paramList.Add("add_L_Leucine = " + Strings.Format(@params.StaticModificationsList.L_Leucine, "0.0000").ToString());
            paramList.Add("add_I_Isoleucine = " + Strings.Format(@params.StaticModificationsList.I_Isoleucine, "0.0000").ToString());
            paramList.Add("add_X_LorI = " + Strings.Format(@params.StaticModificationsList.X_LorI, "0.0000").ToString());
            paramList.Add("add_N_Asparagine = " + Strings.Format(@params.StaticModificationsList.N_Asparagine, "0.0000").ToString());
            paramList.Add("add_O_Ornithine = " + Strings.Format(@params.StaticModificationsList.O_Ornithine, "0.0000").ToString());
            paramList.Add("add_B_avg_NandD = " + Strings.Format(@params.StaticModificationsList.B_avg_NandD, "0.0000").ToString());
            paramList.Add("add_D_Aspartic_Acid = " + Strings.Format(@params.StaticModificationsList.D_Aspartic_Acid, "0.0000").ToString());
            paramList.Add("add_Q_Glutamine = " + Strings.Format(@params.StaticModificationsList.Q_Glutamine, "0.0000").ToString());
            paramList.Add("add_K_Lysine = " + Strings.Format(@params.StaticModificationsList.K_Lysine, "0.0000").ToString());
            paramList.Add("add_Z_avg_QandE = " + Strings.Format(@params.StaticModificationsList.Z_avg_QandE, "0.0000").ToString());
            paramList.Add("add_E_Glutamic_Acid = " + Strings.Format(@params.StaticModificationsList.E_Glutamic_Acid, "0.0000").ToString());
            paramList.Add("add_M_Methionine = " + Strings.Format(@params.StaticModificationsList.M_Methionine, "0.0000").ToString());
            paramList.Add("add_H_Histidine = " + Strings.Format(@params.StaticModificationsList.H_Histidine, "0.0000").ToString());
            paramList.Add("add_F_Phenylalanine = " + Strings.Format(@params.StaticModificationsList.F_Phenylalanine, "0.0000").ToString());
            paramList.Add("add_R_Arginine = " + Strings.Format(@params.StaticModificationsList.R_Arginine, "0.0000").ToString());
            paramList.Add("add_Y_Tyrosine = " + Strings.Format(@params.StaticModificationsList.Y_Tyrosine, "0.0000").ToString());
            paramList.Add("add_W_Tryptophan = " + Strings.Format(@params.StaticModificationsList.W_Tryptophan, "0.0000").ToString());
            if (type == IGenerateFile.ParamFileType.BioWorks_32)
            {
                paramList.Add("add_J_user_amino_acid = 0.0000");
                paramList.Add("add_U_user_amino_acid = 0.0000");          // Note: you'd use 150.9536 for selenocysteine.  However, manual testing by Sam Purvine in 2010 showed that our version of Sequest ignores this parameter
            }
            paramList.Add("");

            if (!(type == IGenerateFile.ParamFileType.BioWorks_32))
            {
                paramList.Add("[SEQUEST_ENZYME_INFO]");
                foreach (EnzymeDetails item in @params.EnzymeList)
                    paramList.Add(item.ReturnEnzymeString());
            }

            return paramList;

        }

        private void OutputTextParamFile(IEnumerable<string> paramList, string outputPath)
        {

            using (var writer = new StreamWriter(outputPath))
            {
                foreach (var item in paramList)
                    writer.WriteLine(item);
            }

        }

        private int ConvertBoolToInteger(bool value)
        {
            if (value == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

    }
}
