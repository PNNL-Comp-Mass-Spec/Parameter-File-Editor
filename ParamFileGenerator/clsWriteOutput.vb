Imports System.IO
Imports System.Collections.Specialized

Public Class clsWriteOutput

    Public Function WriteOutputFile( _
        Params As clsParams, _
        OutputPathName As String, _
        FileType As ParamFileGenerator.MakeParams.IGenerateFile.ParamFileType) As Boolean

        Dim paramCollection As StringCollection = DumpToSQStringCollection(Params, FileType)
        Call OutputTextParamFile(paramCollection, OutputPathName)
        Return True

    End Function

    Public Function WriteDatatableToOutputFile( _
        TableToWrite As DataTable, _
        OutputPathName As String) As Boolean

        Me.DumpDataTableToOutputFile(TableToWrite, OutputPathName)
        Return True

    End Function

    Private Sub DumpDataTableToOutputFile( _
        dt As DataTable, _
        outputPath As String)

        Dim dr As DataRow
        Dim rowElement As Object
        Dim sb As System.Text.StringBuilder
        Dim elementCount As Integer

        Dim sw As New StreamWriter(outputPath)


        For Each dr In dt.Rows
            elementCount = dr.ItemArray.Length
            sb = New System.Text.StringBuilder
            For Each rowElement In dr.ItemArray
                sb.Append(rowElement.ToString)
                elementCount -= 1
                If elementCount > 0 Then
                    sb.Append(Chr(9))
                End If
            Next
            sw.WriteLine(sb.ToString)
        Next

        sw.Close()

    End Sub

    Private Function DumpToSQStringCollection( _
        p As clsParams, _
        type As ParamFileGenerator.MakeParams.IGenerateFile.ParamFileType) As StringCollection

        Dim enz As New clsEnzymeDetails
        Dim sc As New StringCollection
        Dim maxDynMods As Integer

        With sc
            If type = MakeParams.IGenerateFile.ParamFileType.BioWorks_20 Then
                .Add("[Sequest]")
                maxDynMods = 3
            ElseIf type = MakeParams.IGenerateFile.ParamFileType.BioWorks_30 Or type = MakeParams.IGenerateFile.ParamFileType.BioWorks_31 Or type = MakeParams.IGenerateFile.ParamFileType.BioWorks_32 Then
                .Add("[Sequest]")
                maxDynMods = 6
            End If
            .Add(";DMS_Description = " & p.Description)
            If type = clsParams.ParamFileTypes.BioWorks_20 Then
                .Add("database_name = " & p.DefaultFASTAPath)
            ElseIf type = clsParams.ParamFileTypes.BioWorks_30 Or _
                    type = MakeParams.IGenerateFile.ParamFileType.BioWorks_31 Or _
                    type = MakeParams.IGenerateFile.ParamFileType.BioWorks_32 Then
                .Add("first_database_name = " & p.DefaultFASTAPath)
                .Add("second_database_name = " & p.DefaultFASTAPath2)
            End If
            .Add("peptide_mass_tolerance = " & Format(p.PeptideMassTolerance, "0.0000").ToString)
            If type = MakeParams.IGenerateFile.ParamFileType.BioWorks_32 Then
                .Add("peptide_mass_units = " + p.PeptideMassUnits.ToString)
            End If
            .Add("ion_series = " & p.IonSeries.ReturnIonString)
            .Add("fragment_ion_tolerance = " & Format(p.FragmentIonTolerance, "0.0000").ToString)
            If type = MakeParams.IGenerateFile.ParamFileType.BioWorks_32 Then

                ' MEM Note from February 2010
                '  Our version of Sequest [ TurboSEQUEST - PVM Slave v.27 (rev. 12), (c) 1998-2005 ]
                '   does not support mmu or ppm for Fragment Mass Units
                '  In fact, it's possible it completely ignores the fragment_ion_units entry in the .params file
                '  Thus, it is advisable you always use fragment_ion_units = 0  (which means Da)

                .Add("fragment_ion_units = " + p.FragmentMassUnits.ToString)
            End If

            .Add("num_output_lines = " & p.NumberOfOutputLines.ToString)
            If type = clsParams.ParamFileTypes.BioWorks_30 Or type = MakeParams.IGenerateFile.ParamFileType.BioWorks_31 Or type = MakeParams.IGenerateFile.ParamFileType.BioWorks_32 Then
                .Add("num_results = " & p.NumberOfResultsToProcess)
            End If
            .Add("num_description_lines = " & p.NumberOfDescriptionLines.ToString)
            'If type = clsParams.ParamFileTypes.BioWorks_30 Or type = MakeParams.IGenerateFile.ParamFileType.BioWorks_31 Or type = MakeParams.IGenerateFile.ParamFileType.BioWorks_32 Then
            '.Add("show_fragment_ions = 0")
            'Else
            .Add("show_fragment_ions = " & ConvertBoolToInteger(p.ShowFragmentIons).ToString)
            'End If
            .Add("print_duplicate_references = " & ConvertBoolToInteger(p.PrintDuplicateReferences).ToString)
            If Not type = MakeParams.IGenerateFile.ParamFileType.BioWorks_32 Then
                .Add("enzyme_number = " & p.SelectedEnzymeIndex.ToString)
            Else
                If p.SelectedEnzymeIndex >= p.EnzymeList.Count Then
                    Throw New IndexOutOfRangeException("Enzyme ID " & p.SelectedEnzymeIndex & " is not recognized; template file is out of date")
                End If
                enz = p.EnzymeList(p.SelectedEnzymeIndex)
                .Add("enzyme_info = " + enz.ReturnBW32EnzymeInfoString(p.SelectedEnzymeCleavagePosition))
            End If
            If type = MakeParams.IGenerateFile.ParamFileType.BioWorks_32 Then
                .Add("max_num_differential_per_peptide = " + p.MaximumNumDifferentialPerPeptide.ToString)
            End If
            .Add("max_num_differential_AA_per_mod = " & p.MaximumNumAAPerDynMod.ToString)


            .Add("diff_search_options = " & p.DynamicMods.ReturnDynModString(maxDynMods))

            .Add("term_diff_search_options = " + p.TermDynamicMods.ReturnDynModString(0))

            If type = MakeParams.IGenerateFile.ParamFileType.BioWorks_32 Then
                .Add("use_phospho_fragmentation = " + p.UsePhosphoFragmentation.ToString)
            End If

            .Add("nucleotide_reading_frame = " & CType(p.SelectedNucReadingFrame, Integer).ToString)
            .Add("mass_type_parent = " & CType(p.ParentMassType, Integer).ToString)
            .Add("mass_type_fragment = " & CType(p.FragmentMassType, Integer).ToString)
            If type = MakeParams.IGenerateFile.ParamFileType.BioWorks_32 Then
                .Add("normalize_xcorr = " + "0")
            End If
            .Add("remove_precursor_peak = " & ConvertBoolToInteger(p.RemovePrecursorPeak).ToString)
            .Add("ion_cutoff_percentage = " & Format(p.IonCutoffPercentage, "0.0000").ToString)
            .Add("max_num_internal_cleavage_sites = " & p.MaximumNumberMissedCleavages.ToString)
            .Add("protein_mass_filter = " & p.ReturnMassFilter(p.MinimumProteinMassToSearch, p.MaximumProteinMassToSearch))
            .Add("match_peak_count = " & p.NumberOfDetectedPeaksToMatch.ToString)
            .Add("match_peak_allowed_error = " & p.NumberOfAllowedDetectedPeakErrors.ToString)
            .Add("match_peak_tolerance = " & Format(p.MatchedPeakMassTolerance, "0.0000").ToString)
            .Add("create_output_files = " & ConvertBoolToInteger(p.CreateOutputFiles).ToString)

            '.Add("residues_in_upper_case = " & ConvertBoolToInteger(p.AminoAcidsAllUpperCase).ToString)
            .Add("partial_sequence = " & p.PartialSequenceToMatch)
            .Add("sequence_header_filter = " & p.SequenceHeaderInfoToFilter)
            .Add("")
            .Add("add_Cterm_peptide = " & Format(p.StaticModificationsList.CtermPeptide, "0.0000").ToString)
            .Add("add_Cterm_protein = " & Format(p.StaticModificationsList.CtermProtein, "0.0000").ToString)
            .Add("add_Nterm_peptide = " & Format(p.StaticModificationsList.NtermPeptide, "0.0000").ToString)
            .Add("add_Nterm_protein = " & Format(p.StaticModificationsList.NtermProtein, "0.0000").ToString)
            .Add("add_G_Glycine = " & Format(p.StaticModificationsList.G_Glycine, "0.0000").ToString)
            .Add("add_A_Alanine = " & Format(p.StaticModificationsList.A_Alanine, "0.0000").ToString)
            .Add("add_S_Serine = " & Format(p.StaticModificationsList.S_Serine, "0.0000").ToString)
            .Add("add_P_Proline = " & Format(p.StaticModificationsList.P_Proline, "0.0000").ToString)
            .Add("add_V_Valine = " & Format(p.StaticModificationsList.V_Valine, "0.0000").ToString)
            .Add("add_T_Threonine = " & Format(p.StaticModificationsList.T_Threonine, "0.0000").ToString)
            .Add("add_C_Cysteine = " & Format(p.StaticModificationsList.C_Cysteine, "0.0000").ToString)
            .Add("add_L_Leucine = " & Format(p.StaticModificationsList.L_Leucine, "0.0000").ToString)
            .Add("add_I_Isoleucine = " & Format(p.StaticModificationsList.I_Isoleucine, "0.0000").ToString)
            .Add("add_X_LorI = " & Format(p.StaticModificationsList.X_LorI, "0.0000").ToString)
            .Add("add_N_Asparagine = " & Format(p.StaticModificationsList.N_Asparagine, "0.0000").ToString)
            .Add("add_O_Ornithine = " & Format(p.StaticModificationsList.O_Ornithine, "0.0000").ToString)
            .Add("add_B_avg_NandD = " & Format(p.StaticModificationsList.B_avg_NandD, "0.0000").ToString)
            .Add("add_D_Aspartic_Acid = " & Format(p.StaticModificationsList.D_Aspartic_Acid, "0.0000").ToString)
            .Add("add_Q_Glutamine = " & Format(p.StaticModificationsList.Q_Glutamine, "0.0000").ToString)
            .Add("add_K_Lysine = " & Format(p.StaticModificationsList.K_Lysine, "0.0000").ToString)
            .Add("add_Z_avg_QandE = " & Format(p.StaticModificationsList.Z_avg_QandE, "0.0000").ToString)
            .Add("add_E_Glutamic_Acid = " & Format(p.StaticModificationsList.E_Glutamic_Acid, "0.0000").ToString)
            .Add("add_M_Methionine = " & Format(p.StaticModificationsList.M_Methionine, "0.0000").ToString)
            .Add("add_H_Histidine = " & Format(p.StaticModificationsList.H_Histidine, "0.0000").ToString)
            .Add("add_F_Phenylalanine = " & Format(p.StaticModificationsList.F_Phenylalanine, "0.0000").ToString)
            .Add("add_R_Arginine = " & Format(p.StaticModificationsList.R_Arginine, "0.0000").ToString)
            .Add("add_Y_Tyrosine = " & Format(p.StaticModificationsList.Y_Tyrosine, "0.0000").ToString)
            .Add("add_W_Tryptophan = " & Format(p.StaticModificationsList.W_Tryptophan, "0.0000").ToString)
            If type = MakeParams.IGenerateFile.ParamFileType.BioWorks_32 Then
                .Add("add_J_user_amino_acid = 0.0000")
                .Add("add_U_user_amino_acid = 0.0000")          ' Note: you'd use 150.9536 for selenocysteine.  However, manual testing by Sam Purvine in 2010 showed that our version of Sequest ignores this parameter
            End If
            .Add("")

            If Not type = MakeParams.IGenerateFile.ParamFileType.BioWorks_32 Then
                .Add("[SEQUEST_ENZYME_INFO]")
                For Each enz In p.EnzymeList
                    .Add(enz.ReturnEnzymeString)
                Next
            End If
        End With

        Return sc


    End Function

    Private Function WriteModificationDefinitionsFile( _
        Params As clsParams, _
        OutputPathName As String) As Boolean



    End Function

    Private Sub OutputTextParamFile( _
        sc As System.Collections.Specialized.StringCollection, _
        outputPath As String)

        Dim sw As New StreamWriter(outputPath)
        Dim s As String

        For Each s In sc
            sw.WriteLine(s)
        Next

        sw.Close()

    End Sub

    Private Function ConvertBoolToInteger(bln As Boolean) As Integer
        If bln = True Then
            Return 1
        Else
            Return 0
        End If
    End Function

End Class
