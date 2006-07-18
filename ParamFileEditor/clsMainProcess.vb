Imports System.IO
Imports ParamFileEditor.ProgramSettings

Public Class clsMainProcess

#Region " Member Properties "
    'Private basicTemplate As IBasicParams
    'Private advTemplate As IAdvancedParams

    Private m_TemplateFileName As String
    'Private m_clsModsUpdate As clsUpdateModsTable
    'Private m_mainProcess As clsMainProcess
    Shared m_BaseLineParams As clsParams
    'Const DEF_TEMPLATE_LABEL_TEXT As String = "Currently Loaded Template: "

#End Region

#Region " Public Properties "
    'Public Property ModsParamFileTable() As DataTable
    '    Get
    '        Return Me.m_clsModsUpdate.ModParamFileList
    '    End Get
    '    Set(ByVal Value As DataTable)
    '        Me.m_clsModsUpdate.ModParamFileList = Value
    '    End Set
    'End Property
    'Public Property ModsGlobalListTable() As DataTable
    '    Get
    '        Return Me.m_clsModsUpdate.ModGlobaList
    '    End Get
    '    Set(ByVal Value As DataTable)
    '        Me.m_clsModsUpdate.ModGlobaList = Value
    '    End Set
    'End Property
    Public Shared Property BaseLineParamSet() As clsParams
        Get
            Return m_BaseLineParams
        End Get
        Set(ByVal Value As clsParams)
            m_BaseLineParams = Value
        End Set
    End Property

#End Region

#Region " Member Procedures "
    '    Private Sub SetupBasicTab(ByVal frm As frmMainGUI, ByVal bt As IBasicParams)

    '        'Load comboboxes
    '        With frm.cboParentMassType.Items
    '            .Add("Average")
    '            .Add("MonoIsotopic")
    '        End With

    '        With frm.cboFragmentMassType.Items
    '            .Add("Average")
    '            .Add("MonoIsotopic")
    '        End With

    '        Dim enz As New clsEnzymeDetails

    '        For Each enz In frm.newParams.EnzymeList
    '            frm.cboEnzymeSelect.Items.Add(enz.EnzymeID & " - " & enz.EnzymeName & " [" & enz.EnzymeCleavePoints & "]")
    '        Next

    '        Dim counter As Integer = 0
    '        For counter = 0 To 5
    '            frm.cboMissedCleavages.Items.Add(counter.ToString)
    '        Next

    '        With bt
    '            'Name and description info
    '            frm.lblParamFileInfo.Text = DEF_TEMPLATE_LABEL_TEXT & .FileName
    '            frm.txtDescription.Text = .Description

    '            'Search settings
    '            frm.cboParentMassType.SelectedIndex = CType(.ParentMassType, Integer)
    '            frm.cboFragmentMassType.SelectedIndex = CType(.FragmentMassType, Integer)
    '            frm.cboEnzymeSelect.SelectedIndex = .SelectedEnzymeIndex()
    '            frm.cboMissedCleavages.SelectedIndex = .MaximumNumberMissedCleavages
    '            frm.txtPartialSeq.Text = .PartialSequenceToMatch

    '            'Dynamic Mods
    '            frm.txtDynMod1List.Text = .DynamicMods.Dyn_Mod_1_AAList
    '            frm.txtDynMod2List.Text = .DynamicMods.Dyn_Mod_2_AAList
    '            frm.txtDynMod3List.Text = .DynamicMods.Dyn_Mod_3_AAList
    '            frm.txtDynMod1MassDiff.Text = Format(.DynamicMods.Dyn_Mod_1_MassDiff, "0.0000").ToString
    '            frm.txtDynMod2MassDiff.Text = Format(.DynamicMods.Dyn_Mod_2_MassDiff, "0.0000").ToString
    '            frm.txtDynMod3MassDiff.Text = Format(.DynamicMods.Dyn_Mod_3_MassDiff, "0.0000").ToString

    '            'Static Mods
    '            frm.txtCTPep.Text = Format(.StaticModificationsList.CtermPeptide, "0.0000").ToString
    '            frm.txtCTProt.Text = Format(.StaticModificationsList.CtermProtein, "0.0000").ToString
    '            frm.txtNTPep.Text = Format(.StaticModificationsList.NtermPeptide, "0.0000").ToString
    '            frm.txtNTProt.Text = Format(.StaticModificationsList.NtermProtein, "0.0000").ToString
    '            frm.txtGly.Text = Format(.StaticModificationsList.G_Glycine, "0.0000").ToString
    '            frm.txtAla.Text = Format(.StaticModificationsList.A_Alanine, "0.0000").ToString
    '            frm.txtSer.Text = Format(.StaticModificationsList.S_Serine, "0.0000").ToString

    '            frm.txtPro.Text = Format(.StaticModificationsList.P_Proline, "0.0000").ToString
    '            frm.txtVal.Text = Format(.StaticModificationsList.V_Valine, "0.0000").ToString
    '            frm.txtThr.Text = Format(.StaticModificationsList.T_Threonine, "0.0000").ToString
    '            frm.txtCys.Text = Format(.StaticModificationsList.C_Cysteine, "0.0000").ToString
    '            frm.txtLeu.Text = Format(.StaticModificationsList.L_Leucine, "0.0000").ToString
    '            frm.txtIle.Text = Format(.StaticModificationsList.I_Isoleucine, "0.0000").ToString
    '            frm.TxtLorI.Text = Format(.StaticModificationsList.X_LorI, "0.0000").ToString

    '            frm.txtAsn.Text = Format(.StaticModificationsList.N_Asparagine, "0.0000").ToString
    '            frm.txtOrn.Text = Format(.StaticModificationsList.O_Ornithine, "0.0000").ToString
    '            frm.txtNandD.Text = Format(.StaticModificationsList.B_avg_NandD, "0.0000").ToString
    '            frm.txtAsp.Text = Format(.StaticModificationsList.D_Aspartic_Acid, "0.0000").ToString
    '            frm.txtGln.Text = Format(.StaticModificationsList.Q_Glutamine, "0.0000").ToString
    '            frm.txtLys.Text = Format(.StaticModificationsList.K_Lysine, "0.0000").ToString
    '            frm.txtQandE.Text = Format(.StaticModificationsList.Z_avg_QandE, "0.0000").ToString

    '            frm.txtGlu.Text = Format(.StaticModificationsList.E_Glutamic_Acid, "0.0000").ToString
    '            frm.txtMet.Text = Format(.StaticModificationsList.M_Methionine, "0.0000").ToString
    '            frm.txtHis.Text = Format(.StaticModificationsList.H_Histidine, "0.0000").ToString
    '            frm.txtPhe.Text = Format(.StaticModificationsList.F_Phenylalanine, "0.0000").ToString
    '            frm.txtArg.Text = Format(.StaticModificationsList.R_Arginine, "0.0000").ToString
    '            frm.txtTyr.Text = Format(.StaticModificationsList.Y_Tyrosine, "0.0000").ToString
    '            frm.txtTrp.Text = Format(.StaticModificationsList.W_Tryptophan, "0.0000").ToString

    '        End With



    '    End Sub
    '    Private Sub SetupAdvancedTab(ByVal frm As frmMainGUI, ByVal at As IAdvancedParams)

    '        'Load Combo Box
    '        With frm.cboNucReadingFrame.Items
    '            .Add("None - Protein Database Used")
    '            .Add("Forward - Frame 1")
    '            .Add("Forward - Frame 2")
    '            .Add("Forward - Frame 3")
    '            .Add("Reverse - Frame 1")
    '            .Add("Reverse - Frame 2")
    '            .Add("Reverse - Frame 3")
    '            .Add("Forward - All 3 Frames")
    '            .Add("Reverse - All 3 Frames")
    '            .Add("All Six Frames")
    '        End With

    '        With at
    '            'Setup checkboxes
    '            frm.chkUseAIons.Checked = .IonSeries.Use_a_Ions
    '            frm.chkUseBIons.Checked = .IonSeries.Use_b_Ions
    '            frm.chkUseYIons.Checked = .IonSeries.Use_y_Ions

    '            frm.chkCreateOutputFiles.Checked = .CreateOutputFiles
    '            frm.chkShowFragmentIons.Checked = .ShowFragmentIons
    '            frm.chkRemovePrecursorPeaks.Checked = .RemovePrecursorPeak
    '            frm.chkPrintDupRefs.Checked = .PrintDuplicateReferences
    '            frm.chkResiduesInUpperCase.Checked = .AminoAcidsAllUpperCase

    '            'Setup Search Tolerances
    '            frm.txtPepMassTol.Text = Format(.PeptideMassTolerance, "0.0000").ToString
    '            frm.txtFragMassTol.Text = Format(.FragmentIonTolerance, "0.0000").ToString
    '            frm.txtPeakMatchingTol.Text = Format(.MatchedPeakMassTolerance, "0.0000").ToString
    '            frm.txtIonCutoff.Text = Format(.IonCutoffPercentage, "0.0000").ToString
    '            frm.txtMinProtMass.Text = Format(.MinimumProteinMassToSearch, "0.0000").ToString
    '            frm.txtMaxProtMass.Text = Format(.MaximumProteinMassToSearch, "0.0000").ToString
    '            frm.cboNucReadingFrame.SelectedIndex = .SelectedNucReadingFrame
    '            frm.txtNumResults.Text = CInt(.NumberOfResultsToProcess)


    '            'Setup Misc Options
    '            frm.txtNumOutputLines.Text = Format(.NumberOfOutputLines, "0").ToString
    '            frm.txtNumDescLines.Text = Format(.NumberOfDescriptionLines, "0").ToString
    '            frm.txtMatchPeakCount.Text = Format(.NumberOfDetectedPeaksToMatch, "0").ToString
    '            frm.txtMatchPeakCountErrors.Text = Format(.NumberOfAllowedDetectedPeakErrors, "0").ToString
    '            frm.txtMaxAAPerDynMod.Text = Format(.MaximumNumAAPerDynMod, "0").ToString
    '            frm.txtSeqHdrFilter.Text = .SequenceHeaderInfoToFilter

    '            'Setup Ion Weighting
    '            frm.txtAWeight.Text = Format(.IonSeries.a_Ion_Weighting, "0.0").ToString
    '            frm.txtBWeight.Text = Format(.IonSeries.b_Ion_Weighting, "0.0").ToString
    '            frm.txtCWeight.Text = Format(.IonSeries.c_Ion_Weighting, "0.0").ToString
    '            frm.txtDWeight.Text = Format(.IonSeries.d_Ion_Weighting, "0.0").ToString
    '            frm.txtVWeight.Text = Format(.IonSeries.v_Ion_Weighting, "0.0").ToString
    '            frm.txtWWeight.Text = Format(.IonSeries.w_Ion_Weighting, "0.0").ToString
    '            frm.txtXWeight.Text = Format(.IonSeries.x_Ion_Weighting, "0.0").ToString
    '            frm.txtYWeight.Text = Format(.IonSeries.y_Ion_Weighting, "0.0").ToString
    '            frm.txtZWeight.Text = Format(.IonSeries.z_Ion_Weighting, "0.0").ToString
    '        End With


    '    End Sub
#End Region

#Region " Public Procedures "
    Public Sub New()
        'mySettings = New clsSettings
        'mySettings.LoadSettings(m_SettingsFileName)
        'm_TemplateFileName = mySettings.TemplateFileName
        'frm.newParams = New clsParams
        Me.m_BaseLineParams = New clsParams

        'With frm.newParams
        '    .FileName = m_TemplateFileName
        '    .LoadTemplate(m_TemplateFileName)
        '    If .FileType = clsParams.ParamFileTypes.BioWorks_20 Then
        '        frm.lblNumResults.Enabled = False
        '        frm.txtNumResults.Enabled = False
        '    Else
        '        frm.lblNumResults.Enabled = True
        '        frm.txtNumResults.Enabled = True
        '    End If
        'End With

        With Me.m_BaseLineParams
            .FileName = m_TemplateFileName
            .LoadTemplate(m_TemplateFileName)
        End With

        'RefreshTabs(frm, frm.newParams)

    End Sub

    'Public Sub RefreshTabs(ByVal frm As frmMainGUI, ByVal ParamsClass As clsParams)
    '    SetupBasicTab(frm, ParamsClass)
    '    SetupAdvancedTab(frm, ParamsClass)
    'End Sub

    'Public Sub UpdateModTables()
    '    Me.m_clsModsUpdate.CommitChangesToDMS()
    'End Sub

#End Region

End Class
