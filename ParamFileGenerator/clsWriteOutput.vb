Imports System.Collections.Generic
Imports System.IO

Public Class clsWriteOutput

    Public Function WriteOutputFile(
        params As clsParams,
        outputPathName As String,
        fileType As MakeParams.IGenerateFile.paramFileType) As Boolean

        Dim sequestParamList As IEnumerable(Of String) = ConvertSequestParamsToList(params, fileType)
        Call OutputTextParamFile(sequestParamList, outputPathName)
        Return True

    End Function

    Public Sub WriteDataTableToOutputFile(
        tableToWrite As List(Of List(Of String)),
        outputFilePath As String)

        Using writer As New StreamWriter(New FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            For Each tableRow In tableToWrite
                writer.WriteLine(String.Join(Chr(9), tableRow))
            Next
        End Using

    End Sub

    Private Function ConvertSequestParamsToList(
        params As clsParams,
        type As MakeParams.IGenerateFile.paramFileType) As IEnumerable(Of String)

        Dim paramList As New List(Of String)

        Dim maxDynMods As Integer

        If type = MakeParams.IGenerateFile.paramFileType.BioWorks_20 Then
            paramList.Add("[Sequest]")
            maxDynMods = 3
        ElseIf type = MakeParams.IGenerateFile.paramFileType.BioWorks_30 Or type = MakeParams.IGenerateFile.paramFileType.BioWorks_31 Or type = MakeParams.IGenerateFile.paramFileType.BioWorks_32 Then
            paramList.Add("[Sequest]")
            maxDynMods = 6
        End If
        paramList.Add(";DMS_Description = " & params.Description)
        If type = clsParams.ParamFileTypes.BioWorks_20 Then
            paramList.Add("database_name = " & params.DefaultFASTAPath)
        ElseIf type = clsParams.ParamFileTypes.BioWorks_30 Or
                    type = MakeParams.IGenerateFile.paramFileType.BioWorks_31 Or
                    type = MakeParams.IGenerateFile.paramFileType.BioWorks_32 Then
            paramList.Add("first_database_name = " & params.DefaultFASTAPath)
            paramList.Add("second_database_name = " & params.DefaultFASTAPath2)
        End If
        paramList.Add("peptide_mass_tolerance = " & Format(params.PeptideMassTolerance, "0.0000").ToString)
        If type = MakeParams.IGenerateFile.paramFileType.BioWorks_32 Then
            paramList.Add("peptide_mass_units = " + params.PeptideMassUnits.ToString)
        End If
        paramList.Add("ion_series = " & params.IonSeries.ReturnIonString)
        paramList.Add("fragment_ion_tolerance = " & Format(params.FragmentIonTolerance, "0.0000").ToString)
        If type = MakeParams.IGenerateFile.paramFileType.BioWorks_32 Then

            ' MEM Note from February 2010
            '  Our version of Sequest [ TurboSEQUEST - PVM Slave v.27 (rev. 12), (c) 1998-2005 ]
            '   does not support mmu or ppm for Fragment Mass Units
            '  In fact, it's possible it completely ignores the fragment_ion_units entry in the .params file
            '  Thus, it is advisable you always use fragment_ion_units = 0  (which means Da)

            paramList.Add("fragment_ion_units = " + params.FragmentMassUnits.ToString)
        End If

        paramList.Add("num_output_lines = " & params.NumberOfOutputLines.ToString)
        If type = clsParams.ParamFileTypes.BioWorks_30 Or type = MakeParams.IGenerateFile.paramFileType.BioWorks_31 Or type = MakeParams.IGenerateFile.paramFileType.BioWorks_32 Then
            paramList.Add("num_results = " & params.NumberOfResultsToProcess)
        End If
        paramList.Add("num_description_lines = " & params.NumberOfDescriptionLines.ToString)
        'If type = clsParams.ParamFileTypes.BioWorks_30 Or type = MakeParams.IGenerateFile.ParamFileType.BioWorks_31 Or type = MakeParams.IGenerateFile.ParamFileType.BioWorks_32 Then
        'paramList.add("show_fragment_ions = 0")
        'Else
        paramList.Add("show_fragment_ions = " & ConvertBoolToInteger(params.ShowFragmentIons).ToString)
        'End If
        paramList.Add("print_duplicate_references = " & ConvertBoolToInteger(params.PrintDuplicateReferences).ToString)

        If Not type = MakeParams.IGenerateFile.paramFileType.BioWorks_32 Then
            paramList.Add("enzyme_number = " & params.SelectedEnzymeIndex.ToString)
        Else
            If params.SelectedEnzymeIndex >= params.EnzymeList.Count Then
                Throw New IndexOutOfRangeException("Enzyme ID " & params.SelectedEnzymeIndex & " is not recognized; template file is out of date")
            End If
            Dim enz = params.EnzymeList(params.SelectedEnzymeIndex)
            paramList.Add("enzyme_info = " + enz.ReturnBW32EnzymeInfoString(params.SelectedEnzymeCleavagePosition))
        End If
        If type = MakeParams.IGenerateFile.paramFileType.BioWorks_32 Then
            paramList.Add("max_num_differential_per_peptide = " + params.MaximumNumDifferentialPerPeptide.ToString)
        End If
        paramList.Add("max_num_differential_AA_per_mod = " & params.MaximumNumAAPerDynMod.ToString)


        paramList.Add("diff_search_options = " & params.DynamicMods.ReturnDynModString(maxDynMods))

        paramList.Add("term_diff_search_options = " + params.TermDynamicMods.ReturnDynModString(0))

        If type = MakeParams.IGenerateFile.paramFileType.BioWorks_32 Then
            paramList.Add("use_phospho_fragmentation = " + params.UsePhosphoFragmentation.ToString)
        End If

        paramList.Add("nucleotide_reading_frame = " & CType(params.SelectedNucReadingFrame, Integer).ToString)
        paramList.Add("mass_type_parent = " & CType(params.ParentMassType, Integer).ToString)
        paramList.Add("mass_type_fragment = " & CType(params.FragmentMassType, Integer).ToString)
        If type = MakeParams.IGenerateFile.paramFileType.BioWorks_32 Then
            paramList.Add("normalize_xcorr = " + "0")
        End If
        paramList.Add("remove_precursor_peak = " & ConvertBoolToInteger(params.RemovePrecursorPeak).ToString)
        paramList.Add("ion_cutoff_percentage = " & Format(params.IonCutoffPercentage, "0.0000").ToString)
        paramList.Add("max_num_internal_cleavage_sites = " & params.MaximumNumberMissedCleavages.ToString)
        paramList.Add("protein_mass_filter = " & params.ReturnMassFilter(params.MinimumProteinMassToSearch, params.MaximumProteinMassToSearch))
        paramList.Add("match_peak_count = " & params.NumberOfDetectedPeaksToMatch.ToString)
        paramList.Add("match_peak_allowed_error = " & params.NumberOfAllowedDetectedPeakErrors.ToString)
        paramList.Add("match_peak_tolerance = " & Format(params.MatchedPeakMassTolerance, "0.0000").ToString)
        paramList.Add("create_output_files = " & ConvertBoolToInteger(params.CreateOutputFiles).ToString)

        'paramList.add("residues_in_upper_case = " & ConvertBoolToInteger(params.AminoAcidsAllUpperCase).ToString)
        paramList.Add("partial_sequence = " & params.PartialSequenceToMatch)
        paramList.Add("sequence_header_filter = " & params.SequenceHeaderInfoToFilter)
        paramList.Add("")
        paramList.Add("add_Cterm_peptide = " & Format(params.StaticModificationsList.CtermPeptide, "0.0000").ToString)
        paramList.Add("add_Cterm_protein = " & Format(params.StaticModificationsList.CtermProtein, "0.0000").ToString)
        paramList.Add("add_Nterm_peptide = " & Format(params.StaticModificationsList.NtermPeptide, "0.0000").ToString)
        paramList.Add("add_Nterm_protein = " & Format(params.StaticModificationsList.NtermProtein, "0.0000").ToString)
        paramList.Add("add_G_Glycine = " & Format(params.StaticModificationsList.G_Glycine, "0.0000").ToString)
        paramList.Add("add_A_Alanine = " & Format(params.StaticModificationsList.A_Alanine, "0.0000").ToString)
        paramList.Add("add_S_Serine = " & Format(params.StaticModificationsList.S_Serine, "0.0000").ToString)
        paramList.Add("add_P_Proline = " & Format(params.StaticModificationsList.P_Proline, "0.0000").ToString)
        paramList.Add("add_V_Valine = " & Format(params.StaticModificationsList.V_Valine, "0.0000").ToString)
        paramList.Add("add_T_Threonine = " & Format(params.StaticModificationsList.T_Threonine, "0.0000").ToString)
        paramList.Add("add_C_Cysteine = " & Format(params.StaticModificationsList.C_Cysteine, "0.0000").ToString)
        paramList.Add("add_L_Leucine = " & Format(params.StaticModificationsList.L_Leucine, "0.0000").ToString)
        paramList.Add("add_I_Isoleucine = " & Format(params.StaticModificationsList.I_Isoleucine, "0.0000").ToString)
        paramList.Add("add_X_LorI = " & Format(params.StaticModificationsList.X_LorI, "0.0000").ToString)
        paramList.Add("add_N_Asparagine = " & Format(params.StaticModificationsList.N_Asparagine, "0.0000").ToString)
        paramList.Add("add_O_Ornithine = " & Format(params.StaticModificationsList.O_Ornithine, "0.0000").ToString)
        paramList.Add("add_B_avg_NandD = " & Format(params.StaticModificationsList.B_avg_NandD, "0.0000").ToString)
        paramList.Add("add_D_Aspartic_Acid = " & Format(params.StaticModificationsList.D_Aspartic_Acid, "0.0000").ToString)
        paramList.Add("add_Q_Glutamine = " & Format(params.StaticModificationsList.Q_Glutamine, "0.0000").ToString)
        paramList.Add("add_K_Lysine = " & Format(params.StaticModificationsList.K_Lysine, "0.0000").ToString)
        paramList.Add("add_Z_avg_QandE = " & Format(params.StaticModificationsList.Z_avg_QandE, "0.0000").ToString)
        paramList.Add("add_E_Glutamic_Acid = " & Format(params.StaticModificationsList.E_Glutamic_Acid, "0.0000").ToString)
        paramList.Add("add_M_Methionine = " & Format(params.StaticModificationsList.M_Methionine, "0.0000").ToString)
        paramList.Add("add_H_Histidine = " & Format(params.StaticModificationsList.H_Histidine, "0.0000").ToString)
        paramList.Add("add_F_Phenylalanine = " & Format(params.StaticModificationsList.F_Phenylalanine, "0.0000").ToString)
        paramList.Add("add_R_Arginine = " & Format(params.StaticModificationsList.R_Arginine, "0.0000").ToString)
        paramList.Add("add_Y_Tyrosine = " & Format(params.StaticModificationsList.Y_Tyrosine, "0.0000").ToString)
        paramList.Add("add_W_Tryptophan = " & Format(params.StaticModificationsList.W_Tryptophan, "0.0000").ToString)
        If type = MakeParams.IGenerateFile.paramFileType.BioWorks_32 Then
            paramList.Add("add_J_user_amino_acid = 0.0000")
            paramList.Add("add_U_user_amino_acid = 0.0000")          ' Note: you'd use 150.9536 for selenocysteine.  However, manual testing by Sam Purvine in 2010 showed that our version of Sequest ignores this parameter
        End If
        paramList.Add("")

        If Not type = MakeParams.IGenerateFile.paramFileType.BioWorks_32 Then
            paramList.Add("[SEQUEST_ENZYME_INFO]")
            For Each item As clsEnzymeDetails In params.EnzymeList
                paramList.Add(item.ReturnEnzymeString)
            Next
        End If

        Return paramList

    End Function

    Private Sub OutputTextParamFile(
        paramList As IEnumerable(Of String),
        outputPath As String)

        Using writer As New StreamWriter(outputPath)
            For Each item In paramList
                writer.WriteLine(item)
            Next
        End Using

    End Sub

    Private Function ConvertBoolToInteger(value As Boolean) As Integer
        If value = True Then
            Return 1
        Else
            Return 0
        End If
    End Function

End Class
