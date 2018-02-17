Option Strict On
Option Infer On

Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports ParamFileEditor.ProgramSettings
Imports ParamFileGenerator
Imports ParamFileGenerator.DownloadParams
Imports System.IO
Imports System.Linq
Imports System.Text.RegularExpressions

Public Class frmMainGUI
    Inherits Form

    Private m_clsParamsFromDMS As clsParamsFromDMS
    Private m_clsMassTweaker As IMassTweaker

    Private m_DMSUpload As clsDMSParamUpload
    Friend WithEvents MenuItem2 As MenuItem

    ' ReSharper disable once NotAccessedField.Local
    ''' <summary>
    ''' This class needs to be instantiated so that we can read properties BaseLineParamSet and TemplateFileName
    ''' </summary>
    Private m_sharedMain As clsMainProcess

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        CheckForSettingsFileExistence()
        CheckForParamFileExistence()
        m_sharedMain = New clsMainProcess()

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuOptionsAutoTweakParams As System.Windows.Forms.MenuItem
    Friend WithEvents tcMain As System.Windows.Forms.TabControl
    Friend WithEvents tabAdvanced As System.Windows.Forms.TabPage
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents lblEnzymeSelect As System.Windows.Forms.Label
    Friend WithEvents cboEnzymeSelect As System.Windows.Forms.ComboBox
    Friend WithEvents gbxDynMods As System.Windows.Forms.GroupBox
    Friend WithEvents lblParentMassType As System.Windows.Forms.Label
    Friend WithEvents cboParentMassType As System.Windows.Forms.ComboBox
    Friend WithEvents cboMissedCleavages As System.Windows.Forms.ComboBox
    Friend WithEvents cboFragmentMassType As System.Windows.Forms.ComboBox
    Friend WithEvents txtPartialSeq As System.Windows.Forms.TextBox
    Friend WithEvents lblPartialSeq As System.Windows.Forms.Label
    Friend WithEvents txtDynMod1List As System.Windows.Forms.TextBox
    Friend WithEvents txtDynMod1MassDiff As NumericTextBox
    Friend WithEvents txtDynMod2List As System.Windows.Forms.TextBox
    Friend WithEvents txtDynMod2MassDiff As NumericTextBox
    Friend WithEvents lblDynMod1List As System.Windows.Forms.Label
    Friend WithEvents lblDynMod2List As System.Windows.Forms.Label
    Friend WithEvents lblDynMod1MassDiff As System.Windows.Forms.Label
    Friend WithEvents lblDynMod2MassDiff As System.Windows.Forms.Label
    Friend WithEvents lblDynMod3MassDiff As System.Windows.Forms.Label
    Friend WithEvents gbxStaticMods As System.Windows.Forms.GroupBox
    Friend WithEvents txtCTPep As NumericTextBox
    Friend WithEvents txtAla As NumericTextBox
    Friend WithEvents txtCTProt As NumericTextBox
    Friend WithEvents txtNTPep As NumericTextBox
    Friend WithEvents txtNTProt As NumericTextBox
    Friend WithEvents txtGly As NumericTextBox
    Friend WithEvents txtSer As NumericTextBox
    Friend WithEvents txtCys As NumericTextBox
    Friend WithEvents txtPro As NumericTextBox
    Friend WithEvents txtThr As NumericTextBox
    Friend WithEvents txtIle As NumericTextBox
    Friend WithEvents txtVal As NumericTextBox
    Friend WithEvents txtLeu As NumericTextBox
    Friend WithEvents txtNandD As NumericTextBox
    Friend WithEvents txtQandE As NumericTextBox
    Friend WithEvents txtAsn As NumericTextBox
    Friend WithEvents txtLys As NumericTextBox
    Friend WithEvents txtOrn As NumericTextBox
    Friend WithEvents txtGln As NumericTextBox
    Friend WithEvents txtAsp As NumericTextBox
    Friend WithEvents txtArg As NumericTextBox
    Friend WithEvents txtTrp As NumericTextBox
    Friend WithEvents txtGlu As NumericTextBox
    Friend WithEvents txtHis As NumericTextBox
    Friend WithEvents txtPhe As NumericTextBox
    Friend WithEvents txtTyr As NumericTextBox
    Friend WithEvents txtMet As NumericTextBox
    Friend WithEvents lblCTPep As System.Windows.Forms.Label
    Friend WithEvents lblCTProt As System.Windows.Forms.Label
    Friend WithEvents lblNTPep As System.Windows.Forms.Label
    Friend WithEvents lblNTProt As System.Windows.Forms.Label
    Friend WithEvents lblGly As System.Windows.Forms.Label
    Friend WithEvents lblAla As System.Windows.Forms.Label
    Friend WithEvents lblSer As System.Windows.Forms.Label
    Friend WithEvents lblCys As System.Windows.Forms.Label
    Friend WithEvents lblLorI As System.Windows.Forms.Label
    Friend WithEvents lblThr As System.Windows.Forms.Label
    Friend WithEvents lblVal As System.Windows.Forms.Label
    Friend WithEvents lblLeu As System.Windows.Forms.Label
    Friend WithEvents lblIle As System.Windows.Forms.Label
    Friend WithEvents lblPro As System.Windows.Forms.Label
    Friend WithEvents lblAsn As System.Windows.Forms.Label
    Friend WithEvents lblGln As System.Windows.Forms.Label
    Friend WithEvents lblQandE As System.Windows.Forms.Label
    Friend WithEvents lblNandD As System.Windows.Forms.Label
    Friend WithEvents lblOrn As System.Windows.Forms.Label
    Friend WithEvents lblAsp As System.Windows.Forms.Label
    Friend WithEvents lblLys As System.Windows.Forms.Label
    Friend WithEvents lblArg As System.Windows.Forms.Label
    Friend WithEvents lblTrp As System.Windows.Forms.Label
    Friend WithEvents lblHis As System.Windows.Forms.Label
    Friend WithEvents lblMet As System.Windows.Forms.Label
    Friend WithEvents lblPhe As System.Windows.Forms.Label
    Friend WithEvents lblTyr As System.Windows.Forms.Label
    Friend WithEvents lblGlu As System.Windows.Forms.Label
    Friend WithEvents txtDynMod3MassDiff As NumericTextBox
    Friend WithEvents txtDynMod3List As System.Windows.Forms.TextBox
    Friend WithEvents lblDynMod3List As System.Windows.Forms.Label
    Friend WithEvents lblMissedCleavages As System.Windows.Forms.Label
    Friend WithEvents lblFragmentMassType As System.Windows.Forms.Label
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents gbxSearch As System.Windows.Forms.GroupBox
    Friend WithEvents gbxDesc As System.Windows.Forms.GroupBox
    Friend WithEvents tabBasic As System.Windows.Forms.TabPage
    Friend WithEvents gbxSwitches As System.Windows.Forms.GroupBox
    Friend WithEvents chkCreateOutputFiles As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowFragmentIons As System.Windows.Forms.CheckBox
    Friend WithEvents chkRemovePrecursorPeaks As System.Windows.Forms.CheckBox
    Friend WithEvents chkPrintDupRefs As System.Windows.Forms.CheckBox
    Friend WithEvents chkResiduesInUpperCase As System.Windows.Forms.CheckBox
    Friend WithEvents gbxToleranceValues As System.Windows.Forms.GroupBox
    Friend WithEvents txtPepMassTol As System.Windows.Forms.TextBox
    Friend WithEvents lblPepMassTol As System.Windows.Forms.Label
    Friend WithEvents txtFragMassTol As System.Windows.Forms.TextBox
    Friend WithEvents lblFragMassTol As System.Windows.Forms.Label
    Friend WithEvents lblIonCutoff As System.Windows.Forms.Label
    Friend WithEvents lblPeakMatchingTol As System.Windows.Forms.Label
    Friend WithEvents lblMaxProtMass As System.Windows.Forms.Label
    Friend WithEvents lblMinProtMass As System.Windows.Forms.Label
    Friend WithEvents gbxMiscParams As System.Windows.Forms.GroupBox
    Friend WithEvents txtNumDescLines As System.Windows.Forms.TextBox
    Friend WithEvents lblOutputLines As System.Windows.Forms.Label
    Friend WithEvents txtNumOutputLines As System.Windows.Forms.TextBox
    Friend WithEvents lblNumDescLines As System.Windows.Forms.Label
    Friend WithEvents txtMatchPeakCountErrors As System.Windows.Forms.TextBox
    Friend WithEvents lblMatchPeakCountErrors As System.Windows.Forms.Label
    Friend WithEvents lblMatchPeakCount As System.Windows.Forms.Label
    Friend WithEvents txtMatchPeakCount As System.Windows.Forms.TextBox
    Friend WithEvents lblSeqHdrFilter As System.Windows.Forms.Label
    Friend WithEvents lblMaxAAPerDynMod As System.Windows.Forms.Label
    Friend WithEvents txtMaxAAPerDynMod As System.Windows.Forms.TextBox
    Friend WithEvents cboNucReadingFrame As System.Windows.Forms.ComboBox
    Friend WithEvents lblVWeight As System.Windows.Forms.Label
    Friend WithEvents txtVWeight As System.Windows.Forms.TextBox
    Friend WithEvents chkUseAIons As System.Windows.Forms.CheckBox
    Friend WithEvents chkUseBIons As System.Windows.Forms.CheckBox
    Friend WithEvents chkUseYIons As System.Windows.Forms.CheckBox
    Friend WithEvents txtYWeight As System.Windows.Forms.TextBox
    Friend WithEvents lblYWeight As System.Windows.Forms.Label
    Friend WithEvents txtZWeight As System.Windows.Forms.TextBox
    Friend WithEvents lblZWeight As System.Windows.Forms.Label
    Friend WithEvents txtAWeight As System.Windows.Forms.TextBox
    Friend WithEvents lblAWeight As System.Windows.Forms.Label
    Friend WithEvents txtBWeight As System.Windows.Forms.TextBox
    Friend WithEvents lblBWeight As System.Windows.Forms.Label
    Friend WithEvents txtCWeight As System.Windows.Forms.TextBox
    Friend WithEvents lblCWeight As System.Windows.Forms.Label
    Friend WithEvents txtDWeight As System.Windows.Forms.TextBox
    Friend WithEvents lblDWeight As System.Windows.Forms.Label
    Friend WithEvents txtXWeight As System.Windows.Forms.TextBox
    Friend WithEvents lblXWeight As System.Windows.Forms.Label
    Friend WithEvents txtWWeight As System.Windows.Forms.TextBox
    Friend WithEvents lblWWeight As System.Windows.Forms.Label
    Friend WithEvents gbxIonWeighting As System.Windows.Forms.GroupBox
    Friend WithEvents txtPeakMatchingTol As System.Windows.Forms.TextBox
    Friend WithEvents txtIonCutoff As System.Windows.Forms.TextBox
    Friend WithEvents txtMaxProtMass As System.Windows.Forms.TextBox
    Friend WithEvents txtMinProtMass As System.Windows.Forms.TextBox
    Friend WithEvents mnuFileSaveToFile As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileExit As System.Windows.Forms.MenuItem
    Friend WithEvents mnuHelpAbout As System.Windows.Forms.MenuItem
    Friend WithEvents mnuLoadFromFile As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileLoadFromDMS As System.Windows.Forms.MenuItem
    Friend WithEvents lblNucReadingFrame As System.Windows.Forms.Label
    Friend WithEvents lblNumResults As System.Windows.Forms.Label
    Friend WithEvents txtNumResults As System.Windows.Forms.TextBox
    Friend WithEvents mnuFileSaveBW2 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSaveBW3 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileUploadDMS As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
    Friend WithEvents mnuBatchUploadDMS As System.Windows.Forms.MenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem
    Friend WithEvents StatModErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents chkAutoTweak As System.Windows.Forms.CheckBox
    Friend WithEvents cmdReTweak As System.Windows.Forms.Button
    Friend WithEvents TxtLorI As NumericTextBox
    Friend WithEvents tooltipProvider As System.Windows.Forms.ToolTip
    Friend WithEvents gbxIsoMods As System.Windows.Forms.GroupBox
    Friend WithEvents lblIsoH As System.Windows.Forms.Label
    Friend WithEvents txtIsoH As NumericTextBox
    Friend WithEvents txtIsoN As NumericTextBox
    Friend WithEvents txtIsoO As NumericTextBox
    Friend WithEvents txtIsoC As NumericTextBox
    Friend WithEvents lblIsoC As System.Windows.Forms.Label
    Friend WithEvents lblIsoO As System.Windows.Forms.Label
    Friend WithEvents lblIsoN As System.Windows.Forms.Label
    Friend WithEvents lblIsoS As System.Windows.Forms.Label
    Friend WithEvents txtIsoS As NumericTextBox
    Friend WithEvents mnuDebugSyncAll As System.Windows.Forms.MenuItem
    Friend WithEvents mnuDebug As System.Windows.Forms.MenuItem
    Friend WithEvents mnuDebugSyncSingle As System.Windows.Forms.MenuItem
    Friend WithEvents mnuDebugSyncDesc As System.Windows.Forms.MenuItem
    Friend WithEvents cboCleavagePosition As System.Windows.Forms.ComboBox
    Friend WithEvents lblCleavagePosition As System.Windows.Forms.Label
    Friend WithEvents txtDynMod4List As System.Windows.Forms.TextBox
    Friend WithEvents txtDynMod4MassDiff As ParamFileEditor.NumericTextBox
    Friend WithEvents lblDynMod4List As System.Windows.Forms.Label
    Friend WithEvents txtDynMod5List As System.Windows.Forms.TextBox
    Friend WithEvents txtDynMod5MassDiff As ParamFileEditor.NumericTextBox
    Friend WithEvents lblDynMod5List As System.Windows.Forms.Label
    Friend WithEvents mnuFileSaveBW32 As System.Windows.Forms.MenuItem
    Friend WithEvents lblDynMod4MassDiff As System.Windows.Forms.Label
    Friend WithEvents lblDynMod5MassDiff As System.Windows.Forms.Label
    Friend WithEvents mnuMain As System.Windows.Forms.MainMenu
    Friend WithEvents mnuDiv1 As System.Windows.Forms.MenuItem
    Friend WithEvents txtMaxDiffPerPeptide As System.Windows.Forms.TextBox
    Friend WithEvents lblDynCTPep As System.Windows.Forms.Label
    Friend WithEvents txtDynCTPep As ParamFileEditor.NumericTextBox
    Friend WithEvents lblDynNTPep As System.Windows.Forms.Label
    Friend WithEvents txtDynNTPep As ParamFileEditor.NumericTextBox
    Friend WithEvents txtParamInfo As ParamFileEditor.NumericTextBox

    Friend WithEvents cboParentMassUnits As System.Windows.Forms.ComboBox
    Friend WithEvents cboFragmentMassUnits As System.Windows.Forms.ComboBox
    Friend WithEvents lblFragmentMassUnits As System.Windows.Forms.Label
    Friend WithEvents lblParentMassUnits As System.Windows.Forms.Label

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMainGUI))
        tcMain = New System.Windows.Forms.TabControl()
        tabBasic = New System.Windows.Forms.TabPage()
        gbxIsoMods = New System.Windows.Forms.GroupBox()
        txtIsoS = New ParamFileEditor.NumericTextBox()
        txtIsoH = New ParamFileEditor.NumericTextBox()
        txtIsoN = New ParamFileEditor.NumericTextBox()
        txtIsoO = New ParamFileEditor.NumericTextBox()
        txtIsoC = New ParamFileEditor.NumericTextBox()
        lblIsoS = New System.Windows.Forms.Label()
        lblIsoN = New System.Windows.Forms.Label()
        lblIsoH = New System.Windows.Forms.Label()
        lblIsoC = New System.Windows.Forms.Label()
        lblIsoO = New System.Windows.Forms.Label()
        cmdReTweak = New System.Windows.Forms.Button()
        chkAutoTweak = New System.Windows.Forms.CheckBox()
        gbxStaticMods = New System.Windows.Forms.GroupBox()
        lblCTPep = New System.Windows.Forms.Label()
        txtCTPep = New ParamFileEditor.NumericTextBox()
        txtAla = New ParamFileEditor.NumericTextBox()
        txtCTProt = New ParamFileEditor.NumericTextBox()
        txtNTPep = New ParamFileEditor.NumericTextBox()
        txtNTProt = New ParamFileEditor.NumericTextBox()
        txtGly = New ParamFileEditor.NumericTextBox()
        txtSer = New ParamFileEditor.NumericTextBox()
        txtCys = New ParamFileEditor.NumericTextBox()
        txtPro = New ParamFileEditor.NumericTextBox()
        TxtLorI = New ParamFileEditor.NumericTextBox()
        txtThr = New ParamFileEditor.NumericTextBox()
        txtIle = New ParamFileEditor.NumericTextBox()
        txtVal = New ParamFileEditor.NumericTextBox()
        txtLeu = New ParamFileEditor.NumericTextBox()
        txtNandD = New ParamFileEditor.NumericTextBox()
        txtQandE = New ParamFileEditor.NumericTextBox()
        txtAsn = New ParamFileEditor.NumericTextBox()
        txtLys = New ParamFileEditor.NumericTextBox()
        txtOrn = New ParamFileEditor.NumericTextBox()
        txtGln = New ParamFileEditor.NumericTextBox()
        txtAsp = New ParamFileEditor.NumericTextBox()
        txtArg = New ParamFileEditor.NumericTextBox()
        txtTrp = New ParamFileEditor.NumericTextBox()
        txtGlu = New ParamFileEditor.NumericTextBox()
        txtHis = New ParamFileEditor.NumericTextBox()
        txtPhe = New ParamFileEditor.NumericTextBox()
        txtTyr = New ParamFileEditor.NumericTextBox()
        txtMet = New ParamFileEditor.NumericTextBox()
        lblCTProt = New System.Windows.Forms.Label()
        lblNTPep = New System.Windows.Forms.Label()
        lblNTProt = New System.Windows.Forms.Label()
        lblGly = New System.Windows.Forms.Label()
        lblAla = New System.Windows.Forms.Label()
        lblSer = New System.Windows.Forms.Label()
        lblCys = New System.Windows.Forms.Label()
        lblLorI = New System.Windows.Forms.Label()
        lblThr = New System.Windows.Forms.Label()
        lblVal = New System.Windows.Forms.Label()
        lblLeu = New System.Windows.Forms.Label()
        lblIle = New System.Windows.Forms.Label()
        lblPro = New System.Windows.Forms.Label()
        lblAsn = New System.Windows.Forms.Label()
        lblGln = New System.Windows.Forms.Label()
        lblQandE = New System.Windows.Forms.Label()
        lblNandD = New System.Windows.Forms.Label()
        lblOrn = New System.Windows.Forms.Label()
        lblAsp = New System.Windows.Forms.Label()
        lblLys = New System.Windows.Forms.Label()
        lblArg = New System.Windows.Forms.Label()
        lblTrp = New System.Windows.Forms.Label()
        lblHis = New System.Windows.Forms.Label()
        lblMet = New System.Windows.Forms.Label()
        lblPhe = New System.Windows.Forms.Label()
        lblTyr = New System.Windows.Forms.Label()
        lblGlu = New System.Windows.Forms.Label()
        gbxDesc = New System.Windows.Forms.GroupBox()
        txtDescription = New System.Windows.Forms.TextBox()
        lblDescription = New System.Windows.Forms.Label()
        gbxSearch = New System.Windows.Forms.GroupBox()
        txtPartialSeq = New System.Windows.Forms.TextBox()
        lblPartialSeq = New System.Windows.Forms.Label()
        cboParentMassUnits = New System.Windows.Forms.ComboBox()
        cboFragmentMassUnits = New System.Windows.Forms.ComboBox()
        cboFragmentMassType = New System.Windows.Forms.ComboBox()
        cboMissedCleavages = New System.Windows.Forms.ComboBox()
        cboParentMassType = New System.Windows.Forms.ComboBox()
        lblFragmentMassUnits = New System.Windows.Forms.Label()
        lblParentMassUnits = New System.Windows.Forms.Label()
        lblParentMassType = New System.Windows.Forms.Label()
        cboEnzymeSelect = New System.Windows.Forms.ComboBox()
        lblEnzymeSelect = New System.Windows.Forms.Label()
        lblMissedCleavages = New System.Windows.Forms.Label()
        lblFragmentMassType = New System.Windows.Forms.Label()
        cboCleavagePosition = New System.Windows.Forms.ComboBox()
        lblCleavagePosition = New System.Windows.Forms.Label()
        gbxDynMods = New System.Windows.Forms.GroupBox()
        txtDynNTPep = New ParamFileEditor.NumericTextBox()
        lblDynCTPep = New System.Windows.Forms.Label()
        txtDynCTPep = New ParamFileEditor.NumericTextBox()
        lblDynNTPep = New System.Windows.Forms.Label()
        txtDynMod1List = New System.Windows.Forms.TextBox()
        txtDynMod1MassDiff = New ParamFileEditor.NumericTextBox()
        txtDynMod2List = New System.Windows.Forms.TextBox()
        txtDynMod2MassDiff = New ParamFileEditor.NumericTextBox()
        txtDynMod3List = New System.Windows.Forms.TextBox()
        txtDynMod3MassDiff = New ParamFileEditor.NumericTextBox()
        lblDynMod1List = New System.Windows.Forms.Label()
        lblDynMod2List = New System.Windows.Forms.Label()
        lblDynMod3List = New System.Windows.Forms.Label()
        lblDynMod1MassDiff = New System.Windows.Forms.Label()
        lblDynMod3MassDiff = New System.Windows.Forms.Label()
        lblDynMod2MassDiff = New System.Windows.Forms.Label()
        txtDynMod4List = New System.Windows.Forms.TextBox()
        txtDynMod4MassDiff = New ParamFileEditor.NumericTextBox()
        lblDynMod4List = New System.Windows.Forms.Label()
        lblDynMod4MassDiff = New System.Windows.Forms.Label()
        lblDynMod5MassDiff = New System.Windows.Forms.Label()
        txtDynMod5List = New System.Windows.Forms.TextBox()
        txtDynMod5MassDiff = New ParamFileEditor.NumericTextBox()
        lblDynMod5List = New System.Windows.Forms.Label()
        tabAdvanced = New System.Windows.Forms.TabPage()
        gbxIonWeighting = New System.Windows.Forms.GroupBox()
        lblWWeight = New System.Windows.Forms.Label()
        lblXWeight = New System.Windows.Forms.Label()
        lblVWeight = New System.Windows.Forms.Label()
        lblYWeight = New System.Windows.Forms.Label()
        lblZWeight = New System.Windows.Forms.Label()
        txtWWeight = New System.Windows.Forms.TextBox()
        txtXWeight = New System.Windows.Forms.TextBox()
        txtDWeight = New System.Windows.Forms.TextBox()
        lblDWeight = New System.Windows.Forms.Label()
        txtCWeight = New System.Windows.Forms.TextBox()
        lblCWeight = New System.Windows.Forms.Label()
        txtBWeight = New System.Windows.Forms.TextBox()
        lblBWeight = New System.Windows.Forms.Label()
        txtVWeight = New System.Windows.Forms.TextBox()
        txtYWeight = New System.Windows.Forms.TextBox()
        txtZWeight = New System.Windows.Forms.TextBox()
        txtAWeight = New System.Windows.Forms.TextBox()
        lblAWeight = New System.Windows.Forms.Label()
        chkUseAIons = New System.Windows.Forms.CheckBox()
        chkUseBIons = New System.Windows.Forms.CheckBox()
        chkUseYIons = New System.Windows.Forms.CheckBox()
        gbxMiscParams = New System.Windows.Forms.GroupBox()
        lblNumResults = New System.Windows.Forms.Label()
        txtNumResults = New System.Windows.Forms.TextBox()
        cboNucReadingFrame = New System.Windows.Forms.ComboBox()
        txtNumDescLines = New System.Windows.Forms.TextBox()
        lblOutputLines = New System.Windows.Forms.Label()
        txtNumOutputLines = New System.Windows.Forms.TextBox()
        lblNumDescLines = New System.Windows.Forms.Label()
        txtMatchPeakCountErrors = New System.Windows.Forms.TextBox()
        lblMatchPeakCountErrors = New System.Windows.Forms.Label()
        lblMatchPeakCount = New System.Windows.Forms.Label()
        txtMatchPeakCount = New System.Windows.Forms.TextBox()
        txtMaxDiffPerPeptide = New System.Windows.Forms.TextBox()
        lblMaxAAPerDynMod = New System.Windows.Forms.Label()
        txtMaxAAPerDynMod = New System.Windows.Forms.TextBox()
        lblNucReadingFrame = New System.Windows.Forms.Label()
        lblSeqHdrFilter = New System.Windows.Forms.Label()
        gbxToleranceValues = New System.Windows.Forms.GroupBox()
        txtFragMassTol = New System.Windows.Forms.TextBox()
        lblPepMassTol = New System.Windows.Forms.Label()
        txtPepMassTol = New System.Windows.Forms.TextBox()
        lblFragMassTol = New System.Windows.Forms.Label()
        txtIonCutoff = New System.Windows.Forms.TextBox()
        lblIonCutoff = New System.Windows.Forms.Label()
        lblPeakMatchingTol = New System.Windows.Forms.Label()
        txtPeakMatchingTol = New System.Windows.Forms.TextBox()
        lblMaxProtMass = New System.Windows.Forms.Label()
        txtMaxProtMass = New System.Windows.Forms.TextBox()
        lblMinProtMass = New System.Windows.Forms.Label()
        txtMinProtMass = New System.Windows.Forms.TextBox()
        gbxSwitches = New System.Windows.Forms.GroupBox()
        chkResiduesInUpperCase = New System.Windows.Forms.CheckBox()
        chkPrintDupRefs = New System.Windows.Forms.CheckBox()
        chkRemovePrecursorPeaks = New System.Windows.Forms.CheckBox()
        chkShowFragmentIons = New System.Windows.Forms.CheckBox()
        chkCreateOutputFiles = New System.Windows.Forms.CheckBox()
        mnuMain = New System.Windows.Forms.MainMenu(components)
        mnuFile = New System.Windows.Forms.MenuItem()
        mnuFileLoadFromDMS = New System.Windows.Forms.MenuItem()
        mnuLoadFromFile = New System.Windows.Forms.MenuItem()
        MenuItem2 = New System.Windows.Forms.MenuItem()
        mnuFileSaveToFile = New System.Windows.Forms.MenuItem()
        mnuFileSaveBW2 = New System.Windows.Forms.MenuItem()
        mnuFileSaveBW3 = New System.Windows.Forms.MenuItem()
        mnuFileSaveBW32 = New System.Windows.Forms.MenuItem()
        mnuFileUploadDMS = New System.Windows.Forms.MenuItem()
        mnuBatchUploadDMS = New System.Windows.Forms.MenuItem()
        mnuDiv1 = New System.Windows.Forms.MenuItem()
        mnuFileExit = New System.Windows.Forms.MenuItem()
        MenuItem1 = New System.Windows.Forms.MenuItem()
        mnuOptionsAutoTweakParams = New System.Windows.Forms.MenuItem()
        mnuHelp = New System.Windows.Forms.MenuItem()
        mnuHelpAbout = New System.Windows.Forms.MenuItem()
        mnuDebug = New System.Windows.Forms.MenuItem()
        mnuDebugSyncAll = New System.Windows.Forms.MenuItem()
        mnuDebugSyncSingle = New System.Windows.Forms.MenuItem()
        mnuDebugSyncDesc = New System.Windows.Forms.MenuItem()
        StatModErrorProvider = New System.Windows.Forms.ErrorProvider(components)
        tooltipProvider = New System.Windows.Forms.ToolTip(components)
        txtParamInfo = New ParamFileEditor.NumericTextBox()
        tcMain.SuspendLayout()
        tabBasic.SuspendLayout()
        gbxIsoMods.SuspendLayout()
        gbxStaticMods.SuspendLayout()
        gbxDesc.SuspendLayout()
        gbxSearch.SuspendLayout()
        gbxDynMods.SuspendLayout()
        tabAdvanced.SuspendLayout()
        gbxIonWeighting.SuspendLayout()
        gbxMiscParams.SuspendLayout()
        gbxToleranceValues.SuspendLayout()
        gbxSwitches.SuspendLayout()
        CType(StatModErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        '
        'tcMain
        '
        tcMain.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        tcMain.Controls.Add(tabBasic)
        tcMain.Controls.Add(tabAdvanced)
        tcMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        tcMain.ItemSize = New System.Drawing.Size(100, 20)
        tcMain.Location = New System.Drawing.Point(0, 0)
        tcMain.Name = "tcMain"
        tcMain.SelectedIndex = 0
        tcMain.Size = New System.Drawing.Size(633, 743)
        tcMain.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        tcMain.TabIndex = 0
        '
        'tabBasic
        '
        tabBasic.Controls.Add(gbxIsoMods)
        tabBasic.Controls.Add(cmdReTweak)
        tabBasic.Controls.Add(chkAutoTweak)
        tabBasic.Controls.Add(gbxStaticMods)
        tabBasic.Controls.Add(gbxDesc)
        tabBasic.Controls.Add(gbxSearch)
        tabBasic.Controls.Add(gbxDynMods)
        tabBasic.Location = New System.Drawing.Point(4, 24)
        tabBasic.Name = "tabBasic"
        tabBasic.Size = New System.Drawing.Size(625, 715)
        tabBasic.TabIndex = 3
        tabBasic.Text = "Basic Parameters"
        '
        'gbxIsoMods
        '
        gbxIsoMods.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        gbxIsoMods.Controls.Add(txtIsoS)
        gbxIsoMods.Controls.Add(txtIsoH)
        gbxIsoMods.Controls.Add(txtIsoN)
        gbxIsoMods.Controls.Add(txtIsoO)
        gbxIsoMods.Controls.Add(txtIsoC)
        gbxIsoMods.Controls.Add(lblIsoS)
        gbxIsoMods.Controls.Add(lblIsoN)
        gbxIsoMods.Controls.Add(lblIsoH)
        gbxIsoMods.Controls.Add(lblIsoC)
        gbxIsoMods.Controls.Add(lblIsoO)
        gbxIsoMods.FlatStyle = System.Windows.Forms.FlatStyle.System
        gbxIsoMods.Location = New System.Drawing.Point(10, 605)
        gbxIsoMods.Name = "gbxIsoMods"
        gbxIsoMods.Size = New System.Drawing.Size(606, 71)
        gbxIsoMods.TabIndex = 4
        gbxIsoMods.TabStop = False
        gbxIsoMods.Text = "Isotopic Modifications to Apply"
        '
        'txtIsoS
        '
        txtIsoS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtIsoS.ForceNewValue = False
        txtIsoS.Location = New System.Drawing.Point(437, 36)
        txtIsoS.Name = "txtIsoS"
        txtIsoS.Size = New System.Drawing.Size(77, 23)
        txtIsoS.TabIndex = 9
        '
        'txtIsoH
        '
        txtIsoH.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtIsoH.ForceNewValue = False
        txtIsoH.Location = New System.Drawing.Point(120, 36)
        txtIsoH.Name = "txtIsoH"
        txtIsoH.Size = New System.Drawing.Size(77, 23)
        txtIsoH.TabIndex = 3
        '
        'txtIsoN
        '
        txtIsoN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtIsoN.ForceNewValue = False
        txtIsoN.Location = New System.Drawing.Point(331, 36)
        txtIsoN.Name = "txtIsoN"
        txtIsoN.Size = New System.Drawing.Size(77, 23)
        txtIsoN.TabIndex = 7
        '
        'txtIsoO
        '
        txtIsoO.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtIsoO.ForceNewValue = False
        txtIsoO.Location = New System.Drawing.Point(226, 36)
        txtIsoO.Name = "txtIsoO"
        txtIsoO.Size = New System.Drawing.Size(76, 23)
        txtIsoO.TabIndex = 5
        '
        'txtIsoC
        '
        txtIsoC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtIsoC.ForceNewValue = False
        txtIsoC.Location = New System.Drawing.Point(19, 36)
        txtIsoC.Name = "txtIsoC"
        txtIsoC.Size = New System.Drawing.Size(77, 23)
        txtIsoC.TabIndex = 1
        '
        'lblIsoS
        '
        lblIsoS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblIsoS.Location = New System.Drawing.Point(427, 21)
        lblIsoS.Name = "lblIsoS"
        lblIsoS.Size = New System.Drawing.Size(101, 18)
        lblIsoS.TabIndex = 8
        lblIsoS.Text = "S - Sulfur"
        lblIsoS.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblIsoN
        '
        lblIsoN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblIsoN.Location = New System.Drawing.Point(326, 21)
        lblIsoN.Name = "lblIsoN"
        lblIsoN.Size = New System.Drawing.Size(82, 18)
        lblIsoN.TabIndex = 6
        lblIsoN.Text = "N - Nitrogen"
        lblIsoN.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblIsoH
        '
        lblIsoH.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblIsoH.Location = New System.Drawing.Point(115, 21)
        lblIsoH.Name = "lblIsoH"
        lblIsoH.Size = New System.Drawing.Size(87, 18)
        lblIsoH.TabIndex = 2
        lblIsoH.Text = "H - Hydrogen"
        lblIsoH.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblIsoC
        '
        lblIsoC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblIsoC.Location = New System.Drawing.Point(14, 21)
        lblIsoC.Name = "lblIsoC"
        lblIsoC.Size = New System.Drawing.Size(77, 18)
        lblIsoC.TabIndex = 0
        lblIsoC.Text = "C- Carbon"
        lblIsoC.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblIsoO
        '
        lblIsoO.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblIsoO.Location = New System.Drawing.Point(226, 21)
        lblIsoO.Name = "lblIsoO"
        lblIsoO.Size = New System.Drawing.Size(76, 18)
        lblIsoO.TabIndex = 4
        lblIsoO.Text = "O - Oxygen"
        lblIsoO.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'cmdReTweak
        '
        cmdReTweak.FlatStyle = System.Windows.Forms.FlatStyle.System
        cmdReTweak.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        cmdReTweak.Location = New System.Drawing.Point(422, 683)
        cmdReTweak.Name = "cmdReTweak"
        cmdReTweak.Size = New System.Drawing.Size(120, 24)
        cmdReTweak.TabIndex = 6
        cmdReTweak.Text = "&Retweak Masses"
        '
        'chkAutoTweak
        '
        chkAutoTweak.FlatStyle = System.Windows.Forms.FlatStyle.System
        chkAutoTweak.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        chkAutoTweak.Location = New System.Drawing.Point(19, 678)
        chkAutoTweak.Name = "chkAutoTweak"
        chkAutoTweak.Size = New System.Drawing.Size(173, 28)
        chkAutoTweak.TabIndex = 5
        chkAutoTweak.Text = "Auto Tweak Masses?"
        '
        'gbxStaticMods
        '
        gbxStaticMods.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        gbxStaticMods.Controls.Add(lblCTPep)
        gbxStaticMods.Controls.Add(txtCTPep)
        gbxStaticMods.Controls.Add(txtAla)
        gbxStaticMods.Controls.Add(txtCTProt)
        gbxStaticMods.Controls.Add(txtNTPep)
        gbxStaticMods.Controls.Add(txtNTProt)
        gbxStaticMods.Controls.Add(txtGly)
        gbxStaticMods.Controls.Add(txtSer)
        gbxStaticMods.Controls.Add(txtCys)
        gbxStaticMods.Controls.Add(txtPro)
        gbxStaticMods.Controls.Add(TxtLorI)
        gbxStaticMods.Controls.Add(txtThr)
        gbxStaticMods.Controls.Add(txtIle)
        gbxStaticMods.Controls.Add(txtVal)
        gbxStaticMods.Controls.Add(txtLeu)
        gbxStaticMods.Controls.Add(txtNandD)
        gbxStaticMods.Controls.Add(txtQandE)
        gbxStaticMods.Controls.Add(txtAsn)
        gbxStaticMods.Controls.Add(txtLys)
        gbxStaticMods.Controls.Add(txtOrn)
        gbxStaticMods.Controls.Add(txtGln)
        gbxStaticMods.Controls.Add(txtAsp)
        gbxStaticMods.Controls.Add(txtArg)
        gbxStaticMods.Controls.Add(txtTrp)
        gbxStaticMods.Controls.Add(txtGlu)
        gbxStaticMods.Controls.Add(txtHis)
        gbxStaticMods.Controls.Add(txtPhe)
        gbxStaticMods.Controls.Add(txtTyr)
        gbxStaticMods.Controls.Add(txtMet)
        gbxStaticMods.Controls.Add(lblCTProt)
        gbxStaticMods.Controls.Add(lblNTPep)
        gbxStaticMods.Controls.Add(lblNTProt)
        gbxStaticMods.Controls.Add(lblGly)
        gbxStaticMods.Controls.Add(lblAla)
        gbxStaticMods.Controls.Add(lblSer)
        gbxStaticMods.Controls.Add(lblCys)
        gbxStaticMods.Controls.Add(lblLorI)
        gbxStaticMods.Controls.Add(lblThr)
        gbxStaticMods.Controls.Add(lblVal)
        gbxStaticMods.Controls.Add(lblLeu)
        gbxStaticMods.Controls.Add(lblIle)
        gbxStaticMods.Controls.Add(lblPro)
        gbxStaticMods.Controls.Add(lblAsn)
        gbxStaticMods.Controls.Add(lblGln)
        gbxStaticMods.Controls.Add(lblQandE)
        gbxStaticMods.Controls.Add(lblNandD)
        gbxStaticMods.Controls.Add(lblOrn)
        gbxStaticMods.Controls.Add(lblAsp)
        gbxStaticMods.Controls.Add(lblLys)
        gbxStaticMods.Controls.Add(lblArg)
        gbxStaticMods.Controls.Add(lblTrp)
        gbxStaticMods.Controls.Add(lblHis)
        gbxStaticMods.Controls.Add(lblMet)
        gbxStaticMods.Controls.Add(lblPhe)
        gbxStaticMods.Controls.Add(lblTyr)
        gbxStaticMods.Controls.Add(lblGlu)
        gbxStaticMods.FlatStyle = System.Windows.Forms.FlatStyle.System
        gbxStaticMods.Location = New System.Drawing.Point(10, 411)
        gbxStaticMods.Name = "gbxStaticMods"
        gbxStaticMods.Size = New System.Drawing.Size(606, 189)
        gbxStaticMods.TabIndex = 3
        gbxStaticMods.TabStop = False
        gbxStaticMods.Text = "Static Modifications to Apply"
        '
        'lblCTPep
        '
        lblCTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblCTPep.Location = New System.Drawing.Point(8, 18)
        lblCTPep.Name = "lblCTPep"
        lblCTPep.Size = New System.Drawing.Size(84, 18)
        lblCTPep.TabIndex = 1
        lblCTPep.Text = "C-Term Pep"
        lblCTPep.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtCTPep
        '
        txtCTPep.BackColor = System.Drawing.SystemColors.Window
        txtCTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtCTPep.ForceNewValue = False
        txtCTPep.Location = New System.Drawing.Point(14, 37)
        txtCTPep.Name = "txtCTPep"
        txtCTPep.Size = New System.Drawing.Size(66, 23)
        txtCTPep.TabIndex = 12
        '
        'txtAla
        '
        txtAla.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtAla.ForceNewValue = False
        txtAla.Location = New System.Drawing.Point(440, 37)
        txtAla.Name = "txtAla"
        txtAla.Size = New System.Drawing.Size(66, 23)
        txtAla.TabIndex = 17
        '
        'txtCTProt
        '
        txtCTProt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtCTProt.ForceNewValue = False
        txtCTProt.Location = New System.Drawing.Point(100, 37)
        txtCTProt.Name = "txtCTProt"
        txtCTProt.Size = New System.Drawing.Size(66, 23)
        txtCTProt.TabIndex = 13
        '
        'txtNTPep
        '
        txtNTPep.BackColor = System.Drawing.SystemColors.Window
        txtNTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtNTPep.ForceNewValue = False
        txtNTPep.Location = New System.Drawing.Point(185, 37)
        txtNTPep.Name = "txtNTPep"
        txtNTPep.Size = New System.Drawing.Size(66, 23)
        txtNTPep.TabIndex = 14
        '
        'txtNTProt
        '
        txtNTProt.BackColor = System.Drawing.SystemColors.Window
        txtNTProt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtNTProt.ForceNewValue = False
        txtNTProt.Location = New System.Drawing.Point(270, 37)
        txtNTProt.Name = "txtNTProt"
        txtNTProt.Size = New System.Drawing.Size(66, 23)
        txtNTProt.TabIndex = 15
        '
        'txtGly
        '
        txtGly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtGly.ForceNewValue = False
        txtGly.Location = New System.Drawing.Point(355, 37)
        txtGly.Name = "txtGly"
        txtGly.Size = New System.Drawing.Size(66, 23)
        txtGly.TabIndex = 16
        '
        'txtSer
        '
        txtSer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtSer.ForceNewValue = False
        txtSer.Location = New System.Drawing.Point(526, 37)
        txtSer.Name = "txtSer"
        txtSer.Size = New System.Drawing.Size(66, 23)
        txtSer.TabIndex = 18
        '
        'txtCys
        '
        txtCys.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtCys.ForceNewValue = False
        txtCys.Location = New System.Drawing.Point(270, 76)
        txtCys.Name = "txtCys"
        txtCys.Size = New System.Drawing.Size(66, 23)
        txtCys.TabIndex = 22
        '
        'txtPro
        '
        txtPro.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtPro.ForceNewValue = False
        txtPro.Location = New System.Drawing.Point(14, 76)
        txtPro.Name = "txtPro"
        txtPro.Size = New System.Drawing.Size(66, 23)
        txtPro.TabIndex = 19
        '
        'TxtLorI
        '
        TxtLorI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        TxtLorI.ForceNewValue = False
        TxtLorI.Location = New System.Drawing.Point(530, 76)
        TxtLorI.Name = "TxtLorI"
        TxtLorI.Size = New System.Drawing.Size(66, 23)
        TxtLorI.TabIndex = 25
        '
        'txtThr
        '
        txtThr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtThr.ForceNewValue = False
        txtThr.Location = New System.Drawing.Point(185, 76)
        txtThr.Name = "txtThr"
        txtThr.Size = New System.Drawing.Size(66, 23)
        txtThr.TabIndex = 21
        '
        'txtIle
        '
        txtIle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtIle.ForceNewValue = False
        txtIle.Location = New System.Drawing.Point(440, 76)
        txtIle.Name = "txtIle"
        txtIle.Size = New System.Drawing.Size(66, 23)
        txtIle.TabIndex = 24
        '
        'txtVal
        '
        txtVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtVal.ForceNewValue = False
        txtVal.Location = New System.Drawing.Point(100, 76)
        txtVal.Name = "txtVal"
        txtVal.Size = New System.Drawing.Size(66, 23)
        txtVal.TabIndex = 20
        '
        'txtLeu
        '
        txtLeu.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtLeu.ForceNewValue = False
        txtLeu.Location = New System.Drawing.Point(355, 76)
        txtLeu.Name = "txtLeu"
        txtLeu.Size = New System.Drawing.Size(66, 23)
        txtLeu.TabIndex = 23
        '
        'txtNandD
        '
        txtNandD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtNandD.ForceNewValue = False
        txtNandD.Location = New System.Drawing.Point(185, 115)
        txtNandD.Name = "txtNandD"
        txtNandD.Size = New System.Drawing.Size(66, 23)
        txtNandD.TabIndex = 28
        '
        'txtQandE
        '
        txtQandE.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtQandE.ForceNewValue = False
        txtQandE.Location = New System.Drawing.Point(530, 115)
        txtQandE.Name = "txtQandE"
        txtQandE.Size = New System.Drawing.Size(66, 23)
        txtQandE.TabIndex = 32
        '
        'txtAsn
        '
        txtAsn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtAsn.ForceNewValue = False
        txtAsn.Location = New System.Drawing.Point(14, 115)
        txtAsn.Name = "txtAsn"
        txtAsn.Size = New System.Drawing.Size(66, 23)
        txtAsn.TabIndex = 26
        '
        'txtLys
        '
        txtLys.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtLys.ForceNewValue = False
        txtLys.Location = New System.Drawing.Point(440, 115)
        txtLys.Name = "txtLys"
        txtLys.Size = New System.Drawing.Size(66, 23)
        txtLys.TabIndex = 31
        '
        'txtOrn
        '
        txtOrn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtOrn.ForceNewValue = False
        txtOrn.Location = New System.Drawing.Point(100, 115)
        txtOrn.Name = "txtOrn"
        txtOrn.Size = New System.Drawing.Size(66, 23)
        txtOrn.TabIndex = 27
        '
        'txtGln
        '
        txtGln.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtGln.ForceNewValue = False
        txtGln.Location = New System.Drawing.Point(355, 115)
        txtGln.Name = "txtGln"
        txtGln.Size = New System.Drawing.Size(66, 23)
        txtGln.TabIndex = 30
        '
        'txtAsp
        '
        txtAsp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtAsp.ForceNewValue = False
        txtAsp.Location = New System.Drawing.Point(270, 115)
        txtAsp.Name = "txtAsp"
        txtAsp.Size = New System.Drawing.Size(66, 23)
        txtAsp.TabIndex = 29
        '
        'txtArg
        '
        txtArg.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtArg.ForceNewValue = False
        txtArg.Location = New System.Drawing.Point(355, 155)
        txtArg.Name = "txtArg"
        txtArg.Size = New System.Drawing.Size(66, 23)
        txtArg.TabIndex = 37
        '
        'txtTrp
        '
        txtTrp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtTrp.ForceNewValue = False
        txtTrp.Location = New System.Drawing.Point(530, 155)
        txtTrp.Name = "txtTrp"
        txtTrp.Size = New System.Drawing.Size(66, 23)
        txtTrp.TabIndex = 39
        '
        'txtGlu
        '
        txtGlu.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtGlu.ForceNewValue = False
        txtGlu.Location = New System.Drawing.Point(14, 155)
        txtGlu.Name = "txtGlu"
        txtGlu.Size = New System.Drawing.Size(66, 23)
        txtGlu.TabIndex = 33
        '
        'txtHis
        '
        txtHis.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtHis.ForceNewValue = False
        txtHis.Location = New System.Drawing.Point(185, 155)
        txtHis.Name = "txtHis"
        txtHis.Size = New System.Drawing.Size(66, 23)
        txtHis.TabIndex = 35
        '
        'txtPhe
        '
        txtPhe.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtPhe.ForceNewValue = False
        txtPhe.Location = New System.Drawing.Point(270, 155)
        txtPhe.Name = "txtPhe"
        txtPhe.Size = New System.Drawing.Size(66, 23)
        txtPhe.TabIndex = 36
        '
        'txtTyr
        '
        txtTyr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtTyr.ForceNewValue = False
        txtTyr.Location = New System.Drawing.Point(440, 155)
        txtTyr.Name = "txtTyr"
        txtTyr.Size = New System.Drawing.Size(66, 23)
        txtTyr.TabIndex = 38
        '
        'txtMet
        '
        txtMet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtMet.ForceNewValue = False
        txtMet.Location = New System.Drawing.Point(100, 155)
        txtMet.Name = "txtMet"
        txtMet.Size = New System.Drawing.Size(66, 23)
        txtMet.TabIndex = 34
        '
        'lblCTProt
        '
        lblCTProt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblCTProt.Location = New System.Drawing.Point(94, 18)
        lblCTProt.Name = "lblCTProt"
        lblCTProt.Size = New System.Drawing.Size(84, 18)
        lblCTProt.TabIndex = 1
        lblCTProt.Text = "C-Term Prot"
        lblCTProt.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblNTPep
        '
        lblNTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblNTPep.Location = New System.Drawing.Point(179, 18)
        lblNTPep.Name = "lblNTPep"
        lblNTPep.Size = New System.Drawing.Size(84, 18)
        lblNTPep.TabIndex = 1
        lblNTPep.Text = "N-Term Pep"
        lblNTPep.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblNTProt
        '
        lblNTProt.BackColor = Color.Transparent
        lblNTProt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblNTProt.Location = New System.Drawing.Point(264, 18)
        lblNTProt.Name = "lblNTProt"
        lblNTProt.Size = New System.Drawing.Size(84, 18)
        lblNTProt.TabIndex = 1
        lblNTProt.Text = "N-Term Prot"
        lblNTProt.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblGly
        '
        lblGly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblGly.Location = New System.Drawing.Point(349, 18)
        lblGly.Name = "lblGly"
        lblGly.Size = New System.Drawing.Size(84, 18)
        lblGly.TabIndex = 1
        lblGly.Text = "Gly (G)"
        lblGly.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblAla
        '
        lblAla.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblAla.Location = New System.Drawing.Point(434, 18)
        lblAla.Name = "lblAla"
        lblAla.Size = New System.Drawing.Size(84, 18)
        lblAla.TabIndex = 1
        lblAla.Text = "Ala (A)"
        lblAla.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblSer
        '
        lblSer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblSer.Location = New System.Drawing.Point(520, 18)
        lblSer.Name = "lblSer"
        lblSer.Size = New System.Drawing.Size(84, 18)
        lblSer.TabIndex = 1
        lblSer.Text = "Ser (S)"
        lblSer.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblCys
        '
        lblCys.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblCys.Location = New System.Drawing.Point(264, 60)
        lblCys.Name = "lblCys"
        lblCys.Size = New System.Drawing.Size(84, 18)
        lblCys.TabIndex = 1
        lblCys.Text = "Cys (C)"
        lblCys.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblLorI
        '
        lblLorI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblLorI.Location = New System.Drawing.Point(520, 60)
        lblLorI.Name = "lblLorI"
        lblLorI.Size = New System.Drawing.Size(84, 18)
        lblLorI.TabIndex = 1
        lblLorI.Text = "L or I (X)"
        lblLorI.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblThr
        '
        lblThr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblThr.Location = New System.Drawing.Point(179, 60)
        lblThr.Name = "lblThr"
        lblThr.Size = New System.Drawing.Size(84, 18)
        lblThr.TabIndex = 1
        lblThr.Text = "Thr (T)"
        lblThr.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblVal
        '
        lblVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblVal.Location = New System.Drawing.Point(94, 60)
        lblVal.Name = "lblVal"
        lblVal.Size = New System.Drawing.Size(84, 18)
        lblVal.TabIndex = 1
        lblVal.Text = "Val (V)"
        lblVal.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblLeu
        '
        lblLeu.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblLeu.Location = New System.Drawing.Point(349, 60)
        lblLeu.Name = "lblLeu"
        lblLeu.Size = New System.Drawing.Size(84, 18)
        lblLeu.TabIndex = 1
        lblLeu.Text = "Leu (L)"
        lblLeu.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblIle
        '
        lblIle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblIle.Location = New System.Drawing.Point(434, 60)
        lblIle.Name = "lblIle"
        lblIle.Size = New System.Drawing.Size(84, 18)
        lblIle.TabIndex = 1
        lblIle.Text = "Ile (I)"
        lblIle.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPro
        '
        lblPro.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblPro.Location = New System.Drawing.Point(8, 60)
        lblPro.Name = "lblPro"
        lblPro.Size = New System.Drawing.Size(84, 18)
        lblPro.TabIndex = 1
        lblPro.Text = "Pro (P)"
        lblPro.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblAsn
        '
        lblAsn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblAsn.Location = New System.Drawing.Point(8, 97)
        lblAsn.Name = "lblAsn"
        lblAsn.Size = New System.Drawing.Size(84, 18)
        lblAsn.TabIndex = 1
        lblAsn.Text = "Asn (N)"
        lblAsn.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblGln
        '
        lblGln.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblGln.Location = New System.Drawing.Point(349, 97)
        lblGln.Name = "lblGln"
        lblGln.Size = New System.Drawing.Size(84, 18)
        lblGln.TabIndex = 1
        lblGln.Text = "Gln (Q)"
        lblGln.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblQandE
        '
        lblQandE.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblQandE.Location = New System.Drawing.Point(520, 97)
        lblQandE.Name = "lblQandE"
        lblQandE.Size = New System.Drawing.Size(84, 18)
        lblQandE.TabIndex = 1
        lblQandE.Text = "Avg Q && E (Z)"
        lblQandE.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblNandD
        '
        lblNandD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblNandD.Location = New System.Drawing.Point(179, 97)
        lblNandD.Name = "lblNandD"
        lblNandD.Size = New System.Drawing.Size(84, 18)
        lblNandD.TabIndex = 1
        lblNandD.Text = "Avg N && D (B)"
        lblNandD.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblOrn
        '
        lblOrn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblOrn.Location = New System.Drawing.Point(94, 97)
        lblOrn.Name = "lblOrn"
        lblOrn.Size = New System.Drawing.Size(84, 18)
        lblOrn.TabIndex = 1
        lblOrn.Text = "Orn (O)"
        lblOrn.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblAsp
        '
        lblAsp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblAsp.Location = New System.Drawing.Point(264, 97)
        lblAsp.Name = "lblAsp"
        lblAsp.Size = New System.Drawing.Size(84, 18)
        lblAsp.TabIndex = 1
        lblAsp.Text = "Asp (D)"
        lblAsp.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblLys
        '
        lblLys.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblLys.Location = New System.Drawing.Point(434, 97)
        lblLys.Name = "lblLys"
        lblLys.Size = New System.Drawing.Size(84, 18)
        lblLys.TabIndex = 1
        lblLys.Text = "Lys (K)"
        lblLys.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblArg
        '
        lblArg.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblArg.Location = New System.Drawing.Point(349, 136)
        lblArg.Name = "lblArg"
        lblArg.Size = New System.Drawing.Size(84, 18)
        lblArg.TabIndex = 1
        lblArg.Text = "Arg (R)"
        lblArg.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblTrp
        '
        lblTrp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblTrp.Location = New System.Drawing.Point(520, 136)
        lblTrp.Name = "lblTrp"
        lblTrp.Size = New System.Drawing.Size(84, 18)
        lblTrp.TabIndex = 1
        lblTrp.Text = "Trp (W)"
        lblTrp.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblHis
        '
        lblHis.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblHis.Location = New System.Drawing.Point(179, 136)
        lblHis.Name = "lblHis"
        lblHis.Size = New System.Drawing.Size(84, 18)
        lblHis.TabIndex = 1
        lblHis.Text = "His (H)"
        lblHis.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblMet
        '
        lblMet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblMet.Location = New System.Drawing.Point(94, 136)
        lblMet.Name = "lblMet"
        lblMet.Size = New System.Drawing.Size(84, 18)
        lblMet.TabIndex = 1
        lblMet.Text = "Met (M)"
        lblMet.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPhe
        '
        lblPhe.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblPhe.Location = New System.Drawing.Point(264, 136)
        lblPhe.Name = "lblPhe"
        lblPhe.Size = New System.Drawing.Size(84, 18)
        lblPhe.TabIndex = 1
        lblPhe.Text = "Phe (F)"
        lblPhe.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblTyr
        '
        lblTyr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblTyr.Location = New System.Drawing.Point(434, 136)
        lblTyr.Name = "lblTyr"
        lblTyr.Size = New System.Drawing.Size(84, 18)
        lblTyr.TabIndex = 1
        lblTyr.Text = "Tyr (Y)"
        lblTyr.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblGlu
        '
        lblGlu.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblGlu.Location = New System.Drawing.Point(8, 136)
        lblGlu.Name = "lblGlu"
        lblGlu.Size = New System.Drawing.Size(84, 18)
        lblGlu.TabIndex = 1
        lblGlu.Text = "Glu (E)"
        lblGlu.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'gbxDesc
        '
        gbxDesc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        gbxDesc.Controls.Add(txtDescription)
        gbxDesc.Controls.Add(lblDescription)
        gbxDesc.FlatStyle = System.Windows.Forms.FlatStyle.System
        gbxDesc.Location = New System.Drawing.Point(10, 5)
        gbxDesc.Name = "gbxDesc"
        gbxDesc.Size = New System.Drawing.Size(606, 110)
        gbxDesc.TabIndex = 0
        gbxDesc.TabStop = False
        gbxDesc.Text = "Name and Description Information"
        '
        'txtDescription
        '
        txtDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        txtDescription.BackColor = System.Drawing.SystemColors.Window
        txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtDescription.Location = New System.Drawing.Point(14, 39)
        txtDescription.Multiline = True
        txtDescription.Name = "txtDescription"
        txtDescription.ReadOnly = True
        txtDescription.Size = New System.Drawing.Size(578, 58)
        txtDescription.TabIndex = 1
        '
        'lblDescription
        '
        lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblDescription.Location = New System.Drawing.Point(14, 23)
        lblDescription.Name = "lblDescription"
        lblDescription.Size = New System.Drawing.Size(317, 19)
        lblDescription.TabIndex = 0
        lblDescription.Text = "Parameter File Descriptive Text"
        '
        'gbxSearch
        '
        gbxSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        gbxSearch.Controls.Add(txtPartialSeq)
        gbxSearch.Controls.Add(lblPartialSeq)
        gbxSearch.Controls.Add(cboParentMassUnits)
        gbxSearch.Controls.Add(cboFragmentMassUnits)
        gbxSearch.Controls.Add(cboFragmentMassType)
        gbxSearch.Controls.Add(cboMissedCleavages)
        gbxSearch.Controls.Add(cboParentMassType)
        gbxSearch.Controls.Add(lblFragmentMassUnits)
        gbxSearch.Controls.Add(lblParentMassUnits)
        gbxSearch.Controls.Add(lblParentMassType)
        gbxSearch.Controls.Add(cboEnzymeSelect)
        gbxSearch.Controls.Add(lblEnzymeSelect)
        gbxSearch.Controls.Add(lblMissedCleavages)
        gbxSearch.Controls.Add(lblFragmentMassType)
        gbxSearch.Controls.Add(cboCleavagePosition)
        gbxSearch.Controls.Add(lblCleavagePosition)
        gbxSearch.FlatStyle = System.Windows.Forms.FlatStyle.System
        gbxSearch.Location = New System.Drawing.Point(10, 120)
        gbxSearch.Name = "gbxSearch"
        gbxSearch.Size = New System.Drawing.Size(606, 162)
        gbxSearch.TabIndex = 1
        gbxSearch.TabStop = False
        gbxSearch.Text = "Search Settings"
        '
        'txtPartialSeq
        '
        txtPartialSeq.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtPartialSeq.Location = New System.Drawing.Point(320, 127)
        txtPartialSeq.Name = "txtPartialSeq"
        txtPartialSeq.Size = New System.Drawing.Size(265, 23)
        txtPartialSeq.TabIndex = 15
        '
        'lblPartialSeq
        '
        lblPartialSeq.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblPartialSeq.Location = New System.Drawing.Point(320, 111)
        lblPartialSeq.Name = "lblPartialSeq"
        lblPartialSeq.Size = New System.Drawing.Size(192, 18)
        lblPartialSeq.TabIndex = 14
        lblPartialSeq.Text = "Partial Sequence To Match"
        '
        'cboParentMassUnits
        '
        cboParentMassUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cboParentMassUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        cboParentMassUnits.Location = New System.Drawing.Point(218, 39)
        cboParentMassUnits.Name = "cboParentMassUnits"
        cboParentMassUnits.Size = New System.Drawing.Size(72, 25)
        cboParentMassUnits.TabIndex = 3
        '
        'cboFragmentMassUnits
        '
        cboFragmentMassUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cboFragmentMassUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        cboFragmentMassUnits.Location = New System.Drawing.Point(513, 39)
        cboFragmentMassUnits.Name = "cboFragmentMassUnits"
        cboFragmentMassUnits.Size = New System.Drawing.Size(72, 25)
        cboFragmentMassUnits.TabIndex = 7
        '
        'cboFragmentMassType
        '
        cboFragmentMassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cboFragmentMassType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        cboFragmentMassType.Location = New System.Drawing.Point(320, 39)
        cboFragmentMassType.Name = "cboFragmentMassType"
        cboFragmentMassType.Size = New System.Drawing.Size(186, 25)
        cboFragmentMassType.TabIndex = 5
        '
        'cboMissedCleavages
        '
        cboMissedCleavages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cboMissedCleavages.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        cboMissedCleavages.Location = New System.Drawing.Point(320, 83)
        cboMissedCleavages.Name = "cboMissedCleavages"
        cboMissedCleavages.Size = New System.Drawing.Size(265, 25)
        cboMissedCleavages.TabIndex = 11
        '
        'cboParentMassType
        '
        cboParentMassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cboParentMassType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        cboParentMassType.Location = New System.Drawing.Point(14, 39)
        cboParentMassType.Name = "cboParentMassType"
        cboParentMassType.Size = New System.Drawing.Size(188, 25)
        cboParentMassType.TabIndex = 1
        '
        'lblFragmentMassUnits
        '
        lblFragmentMassUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblFragmentMassUnits.Location = New System.Drawing.Point(512, 23)
        lblFragmentMassUnits.Name = "lblFragmentMassUnits"
        lblFragmentMassUnits.Size = New System.Drawing.Size(57, 23)
        lblFragmentMassUnits.TabIndex = 6
        lblFragmentMassUnits.Text = "Units"
        '
        'lblParentMassUnits
        '
        lblParentMassUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblParentMassUnits.Location = New System.Drawing.Point(217, 23)
        lblParentMassUnits.Name = "lblParentMassUnits"
        lblParentMassUnits.Size = New System.Drawing.Size(58, 23)
        lblParentMassUnits.TabIndex = 2
        lblParentMassUnits.Text = "Units"
        '
        'lblParentMassType
        '
        lblParentMassType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblParentMassType.Location = New System.Drawing.Point(14, 23)
        lblParentMassType.Name = "lblParentMassType"
        lblParentMassType.Size = New System.Drawing.Size(140, 23)
        lblParentMassType.TabIndex = 0
        lblParentMassType.Text = "Parent Ion Mass Type"
        '
        'cboEnzymeSelect
        '
        cboEnzymeSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cboEnzymeSelect.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        cboEnzymeSelect.Location = New System.Drawing.Point(14, 83)
        cboEnzymeSelect.Name = "cboEnzymeSelect"
        cboEnzymeSelect.Size = New System.Drawing.Size(276, 25)
        cboEnzymeSelect.TabIndex = 9
        '
        'lblEnzymeSelect
        '
        lblEnzymeSelect.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblEnzymeSelect.Location = New System.Drawing.Point(14, 67)
        lblEnzymeSelect.Name = "lblEnzymeSelect"
        lblEnzymeSelect.Size = New System.Drawing.Size(159, 18)
        lblEnzymeSelect.TabIndex = 8
        lblEnzymeSelect.Text = "Enzyme Cleavage Rule"
        '
        'lblMissedCleavages
        '
        lblMissedCleavages.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblMissedCleavages.Location = New System.Drawing.Point(320, 67)
        lblMissedCleavages.Name = "lblMissedCleavages"
        lblMissedCleavages.Size = New System.Drawing.Size(240, 18)
        lblMissedCleavages.TabIndex = 10
        lblMissedCleavages.Text = "Number of Allowed Missed Cleavages"
        '
        'lblFragmentMassType
        '
        lblFragmentMassType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblFragmentMassType.Location = New System.Drawing.Point(320, 23)
        lblFragmentMassType.Name = "lblFragmentMassType"
        lblFragmentMassType.Size = New System.Drawing.Size(158, 23)
        lblFragmentMassType.TabIndex = 4
        lblFragmentMassType.Text = "Fragment Ion Mass Type"
        '
        'cboCleavagePosition
        '
        cboCleavagePosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cboCleavagePosition.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        cboCleavagePosition.Location = New System.Drawing.Point(14, 127)
        cboCleavagePosition.Name = "cboCleavagePosition"
        cboCleavagePosition.Size = New System.Drawing.Size(276, 25)
        cboCleavagePosition.TabIndex = 13
        '
        'lblCleavagePosition
        '
        lblCleavagePosition.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblCleavagePosition.Location = New System.Drawing.Point(14, 111)
        lblCleavagePosition.Name = "lblCleavagePosition"
        lblCleavagePosition.Size = New System.Drawing.Size(178, 18)
        lblCleavagePosition.TabIndex = 12
        lblCleavagePosition.Text = "Enzyme Cleavage Positions"
        '
        'gbxDynMods
        '
        gbxDynMods.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        gbxDynMods.Controls.Add(txtDynNTPep)
        gbxDynMods.Controls.Add(lblDynCTPep)
        gbxDynMods.Controls.Add(txtDynCTPep)
        gbxDynMods.Controls.Add(lblDynNTPep)
        gbxDynMods.Controls.Add(txtDynMod1List)
        gbxDynMods.Controls.Add(txtDynMod1MassDiff)
        gbxDynMods.Controls.Add(txtDynMod2List)
        gbxDynMods.Controls.Add(txtDynMod2MassDiff)
        gbxDynMods.Controls.Add(txtDynMod3List)
        gbxDynMods.Controls.Add(txtDynMod3MassDiff)
        gbxDynMods.Controls.Add(lblDynMod1List)
        gbxDynMods.Controls.Add(lblDynMod2List)
        gbxDynMods.Controls.Add(lblDynMod3List)
        gbxDynMods.Controls.Add(lblDynMod1MassDiff)
        gbxDynMods.Controls.Add(lblDynMod3MassDiff)
        gbxDynMods.Controls.Add(lblDynMod2MassDiff)
        gbxDynMods.Controls.Add(txtDynMod4List)
        gbxDynMods.Controls.Add(txtDynMod4MassDiff)
        gbxDynMods.Controls.Add(lblDynMod4List)
        gbxDynMods.Controls.Add(lblDynMod4MassDiff)
        gbxDynMods.Controls.Add(lblDynMod5MassDiff)
        gbxDynMods.Controls.Add(txtDynMod5List)
        gbxDynMods.Controls.Add(txtDynMod5MassDiff)
        gbxDynMods.Controls.Add(lblDynMod5List)
        gbxDynMods.FlatStyle = System.Windows.Forms.FlatStyle.System
        gbxDynMods.Location = New System.Drawing.Point(10, 286)
        gbxDynMods.Name = "gbxDynMods"
        gbxDynMods.Size = New System.Drawing.Size(606, 120)
        gbxDynMods.TabIndex = 2
        gbxDynMods.TabStop = False
        gbxDynMods.Text = "Dynamic Modifications to Apply"
        '
        'txtDynNTPep
        '
        txtDynNTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtDynNTPep.ForceNewValue = False
        txtDynNTPep.Location = New System.Drawing.Point(524, 42)
        txtDynNTPep.Name = "txtDynNTPep"
        txtDynNTPep.Size = New System.Drawing.Size(72, 23)
        txtDynNTPep.TabIndex = 24
        txtDynNTPep.Tag = "0"
        '
        'lblDynCTPep
        '
        lblDynCTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblDynCTPep.Location = New System.Drawing.Point(524, 69)
        lblDynCTPep.Name = "lblDynCTPep"
        lblDynCTPep.Size = New System.Drawing.Size(96, 16)
        lblDynCTPep.TabIndex = 22
        lblDynCTPep.Text = "C-Term Pep"
        '
        'txtDynCTPep
        '
        txtDynCTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtDynCTPep.ForceNewValue = False
        txtDynCTPep.Location = New System.Drawing.Point(524, 85)
        txtDynCTPep.Name = "txtDynCTPep"
        txtDynCTPep.Size = New System.Drawing.Size(72, 23)
        txtDynCTPep.TabIndex = 23
        txtDynCTPep.Tag = "0"
        '
        'lblDynNTPep
        '
        lblDynNTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblDynNTPep.Location = New System.Drawing.Point(524, 23)
        lblDynNTPep.Name = "lblDynNTPep"
        lblDynNTPep.Size = New System.Drawing.Size(96, 16)
        lblDynNTPep.TabIndex = 20
        lblDynNTPep.Text = "N-Term Pep"
        '
        'txtDynMod1List
        '
        txtDynMod1List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtDynMod1List.Location = New System.Drawing.Point(14, 39)
        txtDynMod1List.Name = "txtDynMod1List"
        txtDynMod1List.Size = New System.Drawing.Size(72, 23)
        txtDynMod1List.TabIndex = 1
        '
        'txtDynMod1MassDiff
        '
        txtDynMod1MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtDynMod1MassDiff.ForceNewValue = False
        txtDynMod1MassDiff.Location = New System.Drawing.Point(14, 85)
        txtDynMod1MassDiff.Name = "txtDynMod1MassDiff"
        txtDynMod1MassDiff.Size = New System.Drawing.Size(72, 23)
        txtDynMod1MassDiff.TabIndex = 3
        txtDynMod1MassDiff.Tag = "0"
        '
        'txtDynMod2List
        '
        txtDynMod2List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtDynMod2List.Location = New System.Drawing.Point(116, 39)
        txtDynMod2List.Name = "txtDynMod2List"
        txtDynMod2List.Size = New System.Drawing.Size(72, 23)
        txtDynMod2List.TabIndex = 5
        '
        'txtDynMod2MassDiff
        '
        txtDynMod2MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtDynMod2MassDiff.ForceNewValue = False
        txtDynMod2MassDiff.Location = New System.Drawing.Point(116, 85)
        txtDynMod2MassDiff.Name = "txtDynMod2MassDiff"
        txtDynMod2MassDiff.Size = New System.Drawing.Size(72, 23)
        txtDynMod2MassDiff.TabIndex = 7
        txtDynMod2MassDiff.Tag = "0"
        '
        'txtDynMod3List
        '
        txtDynMod3List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtDynMod3List.Location = New System.Drawing.Point(218, 39)
        txtDynMod3List.Name = "txtDynMod3List"
        txtDynMod3List.Size = New System.Drawing.Size(72, 23)
        txtDynMod3List.TabIndex = 9
        '
        'txtDynMod3MassDiff
        '
        txtDynMod3MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtDynMod3MassDiff.ForceNewValue = False
        txtDynMod3MassDiff.Location = New System.Drawing.Point(218, 85)
        txtDynMod3MassDiff.Name = "txtDynMod3MassDiff"
        txtDynMod3MassDiff.Size = New System.Drawing.Size(72, 23)
        txtDynMod3MassDiff.TabIndex = 11
        txtDynMod3MassDiff.Tag = "0"
        '
        'lblDynMod1List
        '
        lblDynMod1List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblDynMod1List.Location = New System.Drawing.Point(14, 23)
        lblDynMod1List.Name = "lblDynMod1List"
        lblDynMod1List.Size = New System.Drawing.Size(96, 16)
        lblDynMod1List.TabIndex = 0
        lblDynMod1List.Text = "AA List 1"
        '
        'lblDynMod2List
        '
        lblDynMod2List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblDynMod2List.Location = New System.Drawing.Point(116, 23)
        lblDynMod2List.Name = "lblDynMod2List"
        lblDynMod2List.Size = New System.Drawing.Size(96, 16)
        lblDynMod2List.TabIndex = 4
        lblDynMod2List.Text = "AA List 2"
        '
        'lblDynMod3List
        '
        lblDynMod3List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblDynMod3List.Location = New System.Drawing.Point(218, 23)
        lblDynMod3List.Name = "lblDynMod3List"
        lblDynMod3List.Size = New System.Drawing.Size(96, 16)
        lblDynMod3List.TabIndex = 8
        lblDynMod3List.Text = "AA List 3"
        '
        'lblDynMod1MassDiff
        '
        lblDynMod1MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblDynMod1MassDiff.Location = New System.Drawing.Point(14, 69)
        lblDynMod1MassDiff.Name = "lblDynMod1MassDiff"
        lblDynMod1MassDiff.Size = New System.Drawing.Size(96, 16)
        lblDynMod1MassDiff.TabIndex = 2
        lblDynMod1MassDiff.Text = "Mass Delta 1"
        '
        'lblDynMod3MassDiff
        '
        lblDynMod3MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblDynMod3MassDiff.Location = New System.Drawing.Point(218, 69)
        lblDynMod3MassDiff.Name = "lblDynMod3MassDiff"
        lblDynMod3MassDiff.Size = New System.Drawing.Size(96, 16)
        lblDynMod3MassDiff.TabIndex = 10
        lblDynMod3MassDiff.Text = "Mass Delta 3"
        '
        'lblDynMod2MassDiff
        '
        lblDynMod2MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblDynMod2MassDiff.Location = New System.Drawing.Point(116, 69)
        lblDynMod2MassDiff.Name = "lblDynMod2MassDiff"
        lblDynMod2MassDiff.Size = New System.Drawing.Size(96, 16)
        lblDynMod2MassDiff.TabIndex = 6
        lblDynMod2MassDiff.Text = "Mass Delta 2"
        '
        'txtDynMod4List
        '
        txtDynMod4List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtDynMod4List.Location = New System.Drawing.Point(320, 42)
        txtDynMod4List.Name = "txtDynMod4List"
        txtDynMod4List.Size = New System.Drawing.Size(72, 23)
        txtDynMod4List.TabIndex = 13
        '
        'txtDynMod4MassDiff
        '
        txtDynMod4MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtDynMod4MassDiff.ForceNewValue = False
        txtDynMod4MassDiff.Location = New System.Drawing.Point(320, 85)
        txtDynMod4MassDiff.Name = "txtDynMod4MassDiff"
        txtDynMod4MassDiff.Size = New System.Drawing.Size(72, 23)
        txtDynMod4MassDiff.TabIndex = 15
        txtDynMod4MassDiff.Tag = "0"
        '
        'lblDynMod4List
        '
        lblDynMod4List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblDynMod4List.Location = New System.Drawing.Point(320, 23)
        lblDynMod4List.Name = "lblDynMod4List"
        lblDynMod4List.Size = New System.Drawing.Size(96, 16)
        lblDynMod4List.TabIndex = 12
        lblDynMod4List.Text = "AA List 4"
        '
        'lblDynMod4MassDiff
        '
        lblDynMod4MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblDynMod4MassDiff.Location = New System.Drawing.Point(320, 69)
        lblDynMod4MassDiff.Name = "lblDynMod4MassDiff"
        lblDynMod4MassDiff.Size = New System.Drawing.Size(96, 16)
        lblDynMod4MassDiff.TabIndex = 14
        lblDynMod4MassDiff.Text = "Mass Delta 4"
        '
        'lblDynMod5MassDiff
        '
        lblDynMod5MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblDynMod5MassDiff.Location = New System.Drawing.Point(422, 69)
        lblDynMod5MassDiff.Name = "lblDynMod5MassDiff"
        lblDynMod5MassDiff.Size = New System.Drawing.Size(96, 16)
        lblDynMod5MassDiff.TabIndex = 18
        lblDynMod5MassDiff.Text = "Mass Delta 5"
        '
        'txtDynMod5List
        '
        txtDynMod5List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtDynMod5List.Location = New System.Drawing.Point(422, 42)
        txtDynMod5List.Name = "txtDynMod5List"
        txtDynMod5List.Size = New System.Drawing.Size(72, 23)
        txtDynMod5List.TabIndex = 17
        '
        'txtDynMod5MassDiff
        '
        txtDynMod5MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtDynMod5MassDiff.ForceNewValue = False
        txtDynMod5MassDiff.Location = New System.Drawing.Point(422, 85)
        txtDynMod5MassDiff.Name = "txtDynMod5MassDiff"
        txtDynMod5MassDiff.Size = New System.Drawing.Size(72, 23)
        txtDynMod5MassDiff.TabIndex = 19
        txtDynMod5MassDiff.Tag = "0"
        '
        'lblDynMod5List
        '
        lblDynMod5List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblDynMod5List.Location = New System.Drawing.Point(422, 23)
        lblDynMod5List.Name = "lblDynMod5List"
        lblDynMod5List.Size = New System.Drawing.Size(96, 16)
        lblDynMod5List.TabIndex = 16
        lblDynMod5List.Text = "AA List 5"
        '
        'tabAdvanced
        '
        tabAdvanced.Controls.Add(gbxIonWeighting)
        tabAdvanced.Controls.Add(gbxMiscParams)
        tabAdvanced.Controls.Add(gbxToleranceValues)
        tabAdvanced.Controls.Add(gbxSwitches)
        tabAdvanced.Location = New System.Drawing.Point(4, 24)
        tabAdvanced.Name = "tabAdvanced"
        tabAdvanced.Size = New System.Drawing.Size(625, 715)
        tabAdvanced.TabIndex = 1
        tabAdvanced.Text = "Advanced Parameters"
        '
        'gbxIonWeighting
        '
        gbxIonWeighting.Controls.Add(lblWWeight)
        gbxIonWeighting.Controls.Add(lblXWeight)
        gbxIonWeighting.Controls.Add(lblVWeight)
        gbxIonWeighting.Controls.Add(lblYWeight)
        gbxIonWeighting.Controls.Add(lblZWeight)
        gbxIonWeighting.Controls.Add(txtWWeight)
        gbxIonWeighting.Controls.Add(txtXWeight)
        gbxIonWeighting.Controls.Add(txtDWeight)
        gbxIonWeighting.Controls.Add(lblDWeight)
        gbxIonWeighting.Controls.Add(txtCWeight)
        gbxIonWeighting.Controls.Add(lblCWeight)
        gbxIonWeighting.Controls.Add(txtBWeight)
        gbxIonWeighting.Controls.Add(lblBWeight)
        gbxIonWeighting.Controls.Add(txtVWeight)
        gbxIonWeighting.Controls.Add(txtYWeight)
        gbxIonWeighting.Controls.Add(txtZWeight)
        gbxIonWeighting.Controls.Add(txtAWeight)
        gbxIonWeighting.Controls.Add(lblAWeight)
        gbxIonWeighting.Controls.Add(chkUseAIons)
        gbxIonWeighting.Controls.Add(chkUseBIons)
        gbxIonWeighting.Controls.Add(chkUseYIons)
        gbxIonWeighting.FlatStyle = System.Windows.Forms.FlatStyle.System
        gbxIonWeighting.Location = New System.Drawing.Point(10, 443)
        gbxIonWeighting.Name = "gbxIonWeighting"
        gbxIonWeighting.Size = New System.Drawing.Size(609, 106)
        gbxIonWeighting.TabIndex = 3
        gbxIonWeighting.TabStop = False
        gbxIonWeighting.Text = "Ion Weighting Parameters"
        '
        'lblWWeight
        '
        lblWWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblWWeight.Location = New System.Drawing.Point(251, 51)
        lblWWeight.Name = "lblWWeight"
        lblWWeight.Size = New System.Drawing.Size(90, 18)
        lblWWeight.TabIndex = 14
        lblWWeight.Text = "w Ion Weight"
        lblWWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblXWeight
        '
        lblXWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblXWeight.Location = New System.Drawing.Point(342, 51)
        lblXWeight.Name = "lblXWeight"
        lblXWeight.Size = New System.Drawing.Size(90, 18)
        lblXWeight.TabIndex = 12
        lblXWeight.Text = "x Ion Weight"
        lblXWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblVWeight
        '
        lblVWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblVWeight.Location = New System.Drawing.Point(162, 51)
        lblVWeight.Name = "lblVWeight"
        lblVWeight.Size = New System.Drawing.Size(90, 18)
        lblVWeight.TabIndex = 3
        lblVWeight.Text = "v Ion Weight"
        lblVWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblYWeight
        '
        lblYWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblYWeight.Location = New System.Drawing.Point(427, 51)
        lblYWeight.Name = "lblYWeight"
        lblYWeight.Size = New System.Drawing.Size(90, 18)
        lblYWeight.TabIndex = 3
        lblYWeight.Text = "y Ion Weight"
        lblYWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblZWeight
        '
        lblZWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblZWeight.Location = New System.Drawing.Point(518, 51)
        lblZWeight.Name = "lblZWeight"
        lblZWeight.Size = New System.Drawing.Size(90, 18)
        lblZWeight.TabIndex = 3
        lblZWeight.Text = "z Ion Weight"
        lblZWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtWWeight
        '
        txtWWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtWWeight.Location = New System.Drawing.Point(261, 69)
        txtWWeight.Name = "txtWWeight"
        txtWWeight.Size = New System.Drawing.Size(66, 23)
        txtWWeight.TabIndex = 19
        '
        'txtXWeight
        '
        txtXWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtXWeight.Location = New System.Drawing.Point(349, 69)
        txtXWeight.Name = "txtXWeight"
        txtXWeight.Size = New System.Drawing.Size(66, 23)
        txtXWeight.TabIndex = 20
        '
        'txtDWeight
        '
        txtDWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtDWeight.Location = New System.Drawing.Point(489, 28)
        txtDWeight.Name = "txtDWeight"
        txtDWeight.Size = New System.Drawing.Size(66, 23)
        txtDWeight.TabIndex = 17
        '
        'lblDWeight
        '
        lblDWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblDWeight.Location = New System.Drawing.Point(478, 10)
        lblDWeight.Name = "lblDWeight"
        lblDWeight.Size = New System.Drawing.Size(90, 18)
        lblDWeight.TabIndex = 10
        lblDWeight.Text = "d Ion Weight"
        lblDWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtCWeight
        '
        txtCWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtCWeight.Location = New System.Drawing.Point(398, 28)
        txtCWeight.Name = "txtCWeight"
        txtCWeight.Size = New System.Drawing.Size(66, 23)
        txtCWeight.TabIndex = 16
        '
        'lblCWeight
        '
        lblCWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblCWeight.Location = New System.Drawing.Point(384, 10)
        lblCWeight.Name = "lblCWeight"
        lblCWeight.Size = New System.Drawing.Size(90, 18)
        lblCWeight.TabIndex = 8
        lblCWeight.Text = "c Ion Weight"
        lblCWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtBWeight
        '
        txtBWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtBWeight.Location = New System.Drawing.Point(307, 28)
        txtBWeight.Name = "txtBWeight"
        txtBWeight.Size = New System.Drawing.Size(66, 23)
        txtBWeight.TabIndex = 15
        '
        'lblBWeight
        '
        lblBWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblBWeight.Location = New System.Drawing.Point(294, 10)
        lblBWeight.Name = "lblBWeight"
        lblBWeight.Size = New System.Drawing.Size(90, 18)
        lblBWeight.TabIndex = 6
        lblBWeight.Text = "b Ion Weight"
        lblBWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtVWeight
        '
        txtVWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtVWeight.Location = New System.Drawing.Point(173, 69)
        txtVWeight.Name = "txtVWeight"
        txtVWeight.Size = New System.Drawing.Size(66, 23)
        txtVWeight.TabIndex = 18
        '
        'txtYWeight
        '
        txtYWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtYWeight.Location = New System.Drawing.Point(437, 69)
        txtYWeight.Name = "txtYWeight"
        txtYWeight.Size = New System.Drawing.Size(66, 23)
        txtYWeight.TabIndex = 21
        '
        'txtZWeight
        '
        txtZWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtZWeight.Location = New System.Drawing.Point(525, 69)
        txtZWeight.Name = "txtZWeight"
        txtZWeight.Size = New System.Drawing.Size(66, 23)
        txtZWeight.TabIndex = 22
        '
        'txtAWeight
        '
        txtAWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtAWeight.Location = New System.Drawing.Point(216, 28)
        txtAWeight.Name = "txtAWeight"
        txtAWeight.Size = New System.Drawing.Size(66, 23)
        txtAWeight.TabIndex = 14
        '
        'lblAWeight
        '
        lblAWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblAWeight.Location = New System.Drawing.Point(203, 10)
        lblAWeight.Name = "lblAWeight"
        lblAWeight.Size = New System.Drawing.Size(90, 18)
        lblAWeight.TabIndex = 3
        lblAWeight.Text = "a Ion Weight"
        lblAWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'chkUseAIons
        '
        chkUseAIons.FlatStyle = System.Windows.Forms.FlatStyle.System
        chkUseAIons.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        chkUseAIons.Location = New System.Drawing.Point(14, 27)
        chkUseAIons.Name = "chkUseAIons"
        chkUseAIons.Size = New System.Drawing.Size(150, 18)
        chkUseAIons.TabIndex = 23
        chkUseAIons.Text = "A ion neutral loss?"
        '
        'chkUseBIons
        '
        chkUseBIons.FlatStyle = System.Windows.Forms.FlatStyle.System
        chkUseBIons.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        chkUseBIons.Location = New System.Drawing.Point(14, 50)
        chkUseBIons.Name = "chkUseBIons"
        chkUseBIons.Size = New System.Drawing.Size(150, 18)
        chkUseBIons.TabIndex = 24
        chkUseBIons.Text = "B ion neutral loss?"
        '
        'chkUseYIons
        '
        chkUseYIons.FlatStyle = System.Windows.Forms.FlatStyle.System
        chkUseYIons.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        chkUseYIons.Location = New System.Drawing.Point(14, 73)
        chkUseYIons.Name = "chkUseYIons"
        chkUseYIons.Size = New System.Drawing.Size(150, 18)
        chkUseYIons.TabIndex = 25
        chkUseYIons.Text = "Y ion neutral loss?"
        '
        'gbxMiscParams
        '
        gbxMiscParams.Controls.Add(lblNumResults)
        gbxMiscParams.Controls.Add(txtNumResults)
        gbxMiscParams.Controls.Add(cboNucReadingFrame)
        gbxMiscParams.Controls.Add(txtNumDescLines)
        gbxMiscParams.Controls.Add(lblOutputLines)
        gbxMiscParams.Controls.Add(txtNumOutputLines)
        gbxMiscParams.Controls.Add(lblNumDescLines)
        gbxMiscParams.Controls.Add(txtMatchPeakCountErrors)
        gbxMiscParams.Controls.Add(lblMatchPeakCountErrors)
        gbxMiscParams.Controls.Add(lblMatchPeakCount)
        gbxMiscParams.Controls.Add(txtMatchPeakCount)
        gbxMiscParams.Controls.Add(txtMaxDiffPerPeptide)
        gbxMiscParams.Controls.Add(lblMaxAAPerDynMod)
        gbxMiscParams.Controls.Add(txtMaxAAPerDynMod)
        gbxMiscParams.Controls.Add(lblNucReadingFrame)
        gbxMiscParams.Controls.Add(lblSeqHdrFilter)
        gbxMiscParams.FlatStyle = System.Windows.Forms.FlatStyle.System
        gbxMiscParams.Location = New System.Drawing.Point(10, 198)
        gbxMiscParams.Name = "gbxMiscParams"
        gbxMiscParams.Size = New System.Drawing.Size(542, 236)
        gbxMiscParams.TabIndex = 2
        gbxMiscParams.TabStop = False
        gbxMiscParams.Text = "Miscellaneous Options"
        '
        'lblNumResults
        '
        lblNumResults.Enabled = False
        lblNumResults.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblNumResults.Location = New System.Drawing.Point(293, 175)
        lblNumResults.Name = "lblNumResults"
        lblNumResults.Size = New System.Drawing.Size(192, 19)
        lblNumResults.TabIndex = 18
        lblNumResults.Text = "Number of Results To Process"
        '
        'txtNumResults
        '
        txtNumResults.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtNumResults.Location = New System.Drawing.Point(293, 194)
        txtNumResults.Name = "txtNumResults"
        txtNumResults.Size = New System.Drawing.Size(235, 23)
        txtNumResults.TabIndex = 13
        '
        'cboNucReadingFrame
        '
        cboNucReadingFrame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        cboNucReadingFrame.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        cboNucReadingFrame.Location = New System.Drawing.Point(14, 194)
        cboNucReadingFrame.Name = "cboNucReadingFrame"
        cboNucReadingFrame.Size = New System.Drawing.Size(240, 25)
        cboNucReadingFrame.TabIndex = 12
        '
        'txtNumDescLines
        '
        txtNumDescLines.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtNumDescLines.Location = New System.Drawing.Point(293, 42)
        txtNumDescLines.Name = "txtNumDescLines"
        txtNumDescLines.Size = New System.Drawing.Size(235, 23)
        txtNumDescLines.TabIndex = 7
        '
        'lblOutputLines
        '
        lblOutputLines.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblOutputLines.Location = New System.Drawing.Point(14, 23)
        lblOutputLines.Name = "lblOutputLines"
        lblOutputLines.Size = New System.Drawing.Size(226, 19)
        lblOutputLines.TabIndex = 9
        lblOutputLines.Text = "Number of Peptide Results to Show"
        '
        'txtNumOutputLines
        '
        txtNumOutputLines.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtNumOutputLines.Location = New System.Drawing.Point(14, 42)
        txtNumOutputLines.Name = "txtNumOutputLines"
        txtNumOutputLines.Size = New System.Drawing.Size(236, 23)
        txtNumOutputLines.TabIndex = 6
        '
        'lblNumDescLines
        '
        lblNumDescLines.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblNumDescLines.Location = New System.Drawing.Point(293, 23)
        lblNumDescLines.Name = "lblNumDescLines"
        lblNumDescLines.Size = New System.Drawing.Size(225, 19)
        lblNumDescLines.TabIndex = 13
        lblNumDescLines.Text = "Number of Descriptions to Show"
        '
        'txtMatchPeakCountErrors
        '
        txtMatchPeakCountErrors.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtMatchPeakCountErrors.Location = New System.Drawing.Point(293, 92)
        txtMatchPeakCountErrors.Name = "txtMatchPeakCountErrors"
        txtMatchPeakCountErrors.Size = New System.Drawing.Size(235, 23)
        txtMatchPeakCountErrors.TabIndex = 9
        '
        'lblMatchPeakCountErrors
        '
        lblMatchPeakCountErrors.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblMatchPeakCountErrors.Location = New System.Drawing.Point(293, 74)
        lblMatchPeakCountErrors.Name = "lblMatchPeakCountErrors"
        lblMatchPeakCountErrors.Size = New System.Drawing.Size(225, 18)
        lblMatchPeakCountErrors.TabIndex = 14
        lblMatchPeakCountErrors.Text = "Number of Peak Errors Allowed"
        '
        'lblMatchPeakCount
        '
        lblMatchPeakCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblMatchPeakCount.Location = New System.Drawing.Point(14, 74)
        lblMatchPeakCount.Name = "lblMatchPeakCount"
        lblMatchPeakCount.Size = New System.Drawing.Size(264, 18)
        lblMatchPeakCount.TabIndex = 8
        lblMatchPeakCount.Text = "Number of Peaks to Try to Match"
        '
        'txtMatchPeakCount
        '
        txtMatchPeakCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtMatchPeakCount.Location = New System.Drawing.Point(14, 92)
        txtMatchPeakCount.Name = "txtMatchPeakCount"
        txtMatchPeakCount.Size = New System.Drawing.Size(236, 23)
        txtMatchPeakCount.TabIndex = 8
        '
        'txtMaxDiffPerPeptide
        '
        txtMaxDiffPerPeptide.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtMaxDiffPerPeptide.Location = New System.Drawing.Point(293, 143)
        txtMaxDiffPerPeptide.Name = "txtMaxDiffPerPeptide"
        txtMaxDiffPerPeptide.Size = New System.Drawing.Size(235, 23)
        txtMaxDiffPerPeptide.TabIndex = 11
        '
        'lblMaxAAPerDynMod
        '
        lblMaxAAPerDynMod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblMaxAAPerDynMod.Location = New System.Drawing.Point(14, 125)
        lblMaxAAPerDynMod.Name = "lblMaxAAPerDynMod"
        lblMaxAAPerDynMod.Size = New System.Drawing.Size(226, 18)
        lblMaxAAPerDynMod.TabIndex = 7
        lblMaxAAPerDynMod.Text = "Maximum Dynamic Mods Per AA"
        '
        'txtMaxAAPerDynMod
        '
        txtMaxAAPerDynMod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtMaxAAPerDynMod.Location = New System.Drawing.Point(14, 143)
        txtMaxAAPerDynMod.Name = "txtMaxAAPerDynMod"
        txtMaxAAPerDynMod.Size = New System.Drawing.Size(236, 23)
        txtMaxAAPerDynMod.TabIndex = 10
        '
        'lblNucReadingFrame
        '
        lblNucReadingFrame.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblNucReadingFrame.Location = New System.Drawing.Point(14, 175)
        lblNucReadingFrame.Name = "lblNucReadingFrame"
        lblNucReadingFrame.Size = New System.Drawing.Size(226, 19)
        lblNucReadingFrame.TabIndex = 7
        lblNucReadingFrame.Text = "Nucleotide Reading Frame"
        '
        'lblSeqHdrFilter
        '
        lblSeqHdrFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblSeqHdrFilter.Location = New System.Drawing.Point(293, 125)
        lblSeqHdrFilter.Name = "lblSeqHdrFilter"
        lblSeqHdrFilter.Size = New System.Drawing.Size(245, 18)
        lblSeqHdrFilter.TabIndex = 15
        lblSeqHdrFilter.Text = "Maximum Differential Mods Per Peptide"
        '
        'gbxToleranceValues
        '
        gbxToleranceValues.Controls.Add(txtFragMassTol)
        gbxToleranceValues.Controls.Add(lblPepMassTol)
        gbxToleranceValues.Controls.Add(txtPepMassTol)
        gbxToleranceValues.Controls.Add(lblFragMassTol)
        gbxToleranceValues.Controls.Add(txtIonCutoff)
        gbxToleranceValues.Controls.Add(lblIonCutoff)
        gbxToleranceValues.Controls.Add(lblPeakMatchingTol)
        gbxToleranceValues.Controls.Add(txtPeakMatchingTol)
        gbxToleranceValues.Controls.Add(lblMaxProtMass)
        gbxToleranceValues.Controls.Add(txtMaxProtMass)
        gbxToleranceValues.Controls.Add(lblMinProtMass)
        gbxToleranceValues.Controls.Add(txtMinProtMass)
        gbxToleranceValues.FlatStyle = System.Windows.Forms.FlatStyle.System
        gbxToleranceValues.Location = New System.Drawing.Point(10, 5)
        gbxToleranceValues.Name = "gbxToleranceValues"
        gbxToleranceValues.Size = New System.Drawing.Size(542, 184)
        gbxToleranceValues.TabIndex = 1
        gbxToleranceValues.TabStop = False
        gbxToleranceValues.Text = "Search Tolerance Values"
        '
        'txtFragMassTol
        '
        txtFragMassTol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtFragMassTol.Location = New System.Drawing.Point(293, 42)
        txtFragMassTol.Name = "txtFragMassTol"
        txtFragMassTol.Size = New System.Drawing.Size(235, 23)
        txtFragMassTol.TabIndex = 1
        '
        'lblPepMassTol
        '
        lblPepMassTol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblPepMassTol.Location = New System.Drawing.Point(14, 23)
        lblPepMassTol.Name = "lblPepMassTol"
        lblPepMassTol.Size = New System.Drawing.Size(226, 19)
        lblPepMassTol.TabIndex = 1
        lblPepMassTol.Text = "Parent Mass Tolerance"
        '
        'txtPepMassTol
        '
        txtPepMassTol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtPepMassTol.Location = New System.Drawing.Point(14, 42)
        txtPepMassTol.Name = "txtPepMassTol"
        txtPepMassTol.Size = New System.Drawing.Size(236, 23)
        txtPepMassTol.TabIndex = 0
        '
        'lblFragMassTol
        '
        lblFragMassTol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblFragMassTol.Location = New System.Drawing.Point(293, 23)
        lblFragMassTol.Name = "lblFragMassTol"
        lblFragMassTol.Size = New System.Drawing.Size(225, 19)
        lblFragMassTol.TabIndex = 3
        lblFragMassTol.Text = "Fragment Mass Tolerance"
        '
        'txtIonCutoff
        '
        txtIonCutoff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtIonCutoff.Location = New System.Drawing.Point(293, 92)
        txtIonCutoff.Name = "txtIonCutoff"
        txtIonCutoff.Size = New System.Drawing.Size(235, 23)
        txtIonCutoff.TabIndex = 3
        '
        'lblIonCutoff
        '
        lblIonCutoff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblIonCutoff.Location = New System.Drawing.Point(293, 74)
        lblIonCutoff.Name = "lblIonCutoff"
        lblIonCutoff.Size = New System.Drawing.Size(225, 18)
        lblIonCutoff.TabIndex = 3
        lblIonCutoff.Text = "Preliminary Score Cutoff Percentage"
        '
        'lblPeakMatchingTol
        '
        lblPeakMatchingTol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblPeakMatchingTol.Location = New System.Drawing.Point(14, 74)
        lblPeakMatchingTol.Name = "lblPeakMatchingTol"
        lblPeakMatchingTol.Size = New System.Drawing.Size(250, 18)
        lblPeakMatchingTol.TabIndex = 1
        lblPeakMatchingTol.Text = "Detected Peak Matching Tolerance"
        '
        'txtPeakMatchingTol
        '
        txtPeakMatchingTol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtPeakMatchingTol.Location = New System.Drawing.Point(14, 92)
        txtPeakMatchingTol.Name = "txtPeakMatchingTol"
        txtPeakMatchingTol.Size = New System.Drawing.Size(236, 23)
        txtPeakMatchingTol.TabIndex = 2
        '
        'lblMaxProtMass
        '
        lblMaxProtMass.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblMaxProtMass.Location = New System.Drawing.Point(293, 125)
        lblMaxProtMass.Name = "lblMaxProtMass"
        lblMaxProtMass.Size = New System.Drawing.Size(225, 18)
        lblMaxProtMass.TabIndex = 3
        lblMaxProtMass.Text = "Maximum Allowed Protein Mass"
        '
        'txtMaxProtMass
        '
        txtMaxProtMass.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtMaxProtMass.Location = New System.Drawing.Point(293, 143)
        txtMaxProtMass.Name = "txtMaxProtMass"
        txtMaxProtMass.Size = New System.Drawing.Size(235, 23)
        txtMaxProtMass.TabIndex = 5
        '
        'lblMinProtMass
        '
        lblMinProtMass.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblMinProtMass.Location = New System.Drawing.Point(14, 125)
        lblMinProtMass.Name = "lblMinProtMass"
        lblMinProtMass.Size = New System.Drawing.Size(226, 18)
        lblMinProtMass.TabIndex = 1
        lblMinProtMass.Text = "Minimum Allowed Protein Mass"
        '
        'txtMinProtMass
        '
        txtMinProtMass.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtMinProtMass.Location = New System.Drawing.Point(14, 143)
        txtMinProtMass.Name = "txtMinProtMass"
        txtMinProtMass.Size = New System.Drawing.Size(236, 23)
        txtMinProtMass.TabIndex = 4
        '
        'gbxSwitches
        '
        gbxSwitches.Controls.Add(chkResiduesInUpperCase)
        gbxSwitches.Controls.Add(chkPrintDupRefs)
        gbxSwitches.Controls.Add(chkRemovePrecursorPeaks)
        gbxSwitches.Controls.Add(chkShowFragmentIons)
        gbxSwitches.Controls.Add(chkCreateOutputFiles)
        gbxSwitches.FlatStyle = System.Windows.Forms.FlatStyle.System
        gbxSwitches.Location = New System.Drawing.Point(10, 554)
        gbxSwitches.Name = "gbxSwitches"
        gbxSwitches.Size = New System.Drawing.Size(542, 143)
        gbxSwitches.TabIndex = 0
        gbxSwitches.TabStop = False
        gbxSwitches.Text = "Search Options"
        '
        'chkResiduesInUpperCase
        '
        chkResiduesInUpperCase.FlatStyle = System.Windows.Forms.FlatStyle.System
        chkResiduesInUpperCase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        chkResiduesInUpperCase.Location = New System.Drawing.Point(14, 111)
        chkResiduesInUpperCase.Name = "chkResiduesInUpperCase"
        chkResiduesInUpperCase.Size = New System.Drawing.Size(298, 27)
        chkResiduesInUpperCase.TabIndex = 30
        chkResiduesInUpperCase.Text = "FASTA File has Residues in Upper Case?"
        '
        'chkPrintDupRefs
        '
        chkPrintDupRefs.FlatStyle = System.Windows.Forms.FlatStyle.System
        chkPrintDupRefs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        chkPrintDupRefs.Location = New System.Drawing.Point(14, 88)
        chkPrintDupRefs.Name = "chkPrintDupRefs"
        chkPrintDupRefs.Size = New System.Drawing.Size(298, 27)
        chkPrintDupRefs.TabIndex = 29
        chkPrintDupRefs.Text = "Print Duplicate References (ORFs)?"
        '
        'chkRemovePrecursorPeaks
        '
        chkRemovePrecursorPeaks.FlatStyle = System.Windows.Forms.FlatStyle.System
        chkRemovePrecursorPeaks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        chkRemovePrecursorPeaks.Location = New System.Drawing.Point(14, 65)
        chkRemovePrecursorPeaks.Name = "chkRemovePrecursorPeaks"
        chkRemovePrecursorPeaks.Size = New System.Drawing.Size(298, 27)
        chkRemovePrecursorPeaks.TabIndex = 28
        chkRemovePrecursorPeaks.Text = "Remove Precursor Ion Peaks?"
        '
        'chkShowFragmentIons
        '
        chkShowFragmentIons.FlatStyle = System.Windows.Forms.FlatStyle.System
        chkShowFragmentIons.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        chkShowFragmentIons.Location = New System.Drawing.Point(14, 42)
        chkShowFragmentIons.Name = "chkShowFragmentIons"
        chkShowFragmentIons.Size = New System.Drawing.Size(298, 27)
        chkShowFragmentIons.TabIndex = 27
        chkShowFragmentIons.Text = "Show Fragment Ions?"
        '
        'chkCreateOutputFiles
        '
        chkCreateOutputFiles.FlatStyle = System.Windows.Forms.FlatStyle.System
        chkCreateOutputFiles.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        chkCreateOutputFiles.Location = New System.Drawing.Point(14, 18)
        chkCreateOutputFiles.Name = "chkCreateOutputFiles"
        chkCreateOutputFiles.Size = New System.Drawing.Size(298, 28)
        chkCreateOutputFiles.TabIndex = 26
        chkCreateOutputFiles.Text = "Create Output Files?"
        '
        'mnuMain
        '
        mnuMain.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {mnuFile, MenuItem1, mnuHelp, mnuDebug})
        '
        'mnuFile
        '
        mnuFile.Index = 0
        mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {mnuFileLoadFromDMS, mnuLoadFromFile, MenuItem2, mnuFileSaveToFile, mnuFileUploadDMS, mnuBatchUploadDMS, mnuDiv1, mnuFileExit})
        mnuFile.Text = "&File"
        '
        'mnuFileLoadFromDMS
        '
        mnuFileLoadFromDMS.Index = 0
        mnuFileLoadFromDMS.Shortcut = System.Windows.Forms.Shortcut.CtrlL
        mnuFileLoadFromDMS.Text = "Load Param File from &DMS..."
        '
        'mnuLoadFromFile
        '
        mnuLoadFromFile.Index = 1
        mnuLoadFromFile.Text = "Load Param File from &Local Template File..."
        '
        'MenuItem2
        '
        MenuItem2.Index = 2
        MenuItem2.Text = "-"
        '
        'mnuFileSaveToFile
        '
        mnuFileSaveToFile.Index = 3
        mnuFileSaveToFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {mnuFileSaveBW2, mnuFileSaveBW3, mnuFileSaveBW32})
        mnuFileSaveToFile.Shortcut = System.Windows.Forms.Shortcut.CtrlS
        mnuFileSaveToFile.Text = "&Save Current Settings as New Param File"
        '
        'mnuFileSaveBW2
        '
        mnuFileSaveBW2.Index = 0
        mnuFileSaveBW2.Text = "BioWorks 2.0 Format..."
        '
        'mnuFileSaveBW3
        '
        mnuFileSaveBW3.Index = 1
        mnuFileSaveBW3.Text = "BioWorks 3.0 Format..."
        '
        'mnuFileSaveBW32
        '
        mnuFileSaveBW32.Index = 2
        mnuFileSaveBW32.Shortcut = System.Windows.Forms.Shortcut.CtrlS
        mnuFileSaveBW32.Text = "&BioWorks 3.2 Format..."
        '
        'mnuFileUploadDMS
        '
        mnuFileUploadDMS.Index = 4
        mnuFileUploadDMS.Shortcut = System.Windows.Forms.Shortcut.CtrlU
        mnuFileUploadDMS.Text = "&Upload Current Settings to DMS (Restricted)..."
        '
        'mnuBatchUploadDMS
        '
        mnuBatchUploadDMS.Index = 5
        mnuBatchUploadDMS.Text = "&Batch Upload Param Files to DMS (Restricted)"
        '
        'mnuDiv1
        '
        mnuDiv1.Index = 6
        mnuDiv1.Text = "-"
        '
        'mnuFileExit
        '
        mnuFileExit.Index = 7
        mnuFileExit.Text = "E&xit"
        '
        'MenuItem1
        '
        MenuItem1.Enabled = False
        MenuItem1.Index = 1
        MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {mnuOptionsAutoTweakParams})
        MenuItem1.Text = "Options"
        MenuItem1.Visible = False
        '
        'mnuOptionsAutoTweakParams
        '
        mnuOptionsAutoTweakParams.Index = 0
        mnuOptionsAutoTweakParams.Text = "Change Auto Tweak Parameters..."
        '
        'mnuHelp
        '
        mnuHelp.Index = 2
        mnuHelp.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {mnuHelpAbout})
        mnuHelp.Text = "&Help"
        '
        'mnuHelpAbout
        '
        mnuHelpAbout.Index = 0
        mnuHelpAbout.Text = "&About Parameter File Editor..."
        '
        'mnuDebug
        '
        mnuDebug.Index = 3
        mnuDebug.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {mnuDebugSyncAll, mnuDebugSyncSingle, mnuDebugSyncDesc})
        mnuDebug.Text = "Debug"
        mnuDebug.Visible = False
        '
        'mnuDebugSyncAll
        '
        mnuDebugSyncAll.Index = 0
        mnuDebugSyncAll.Text = "Sync Old Tables"
        '
        'mnuDebugSyncSingle
        '
        mnuDebugSyncSingle.Index = 1
        mnuDebugSyncSingle.Text = "Sync Single Job..."
        '
        'mnuDebugSyncDesc
        '
        mnuDebugSyncDesc.Index = 2
        mnuDebugSyncDesc.Text = "Sync Param File Descriptions"
        '
        'StatModErrorProvider
        '
        StatModErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink
        StatModErrorProvider.ContainerControl = Me
        '
        'txtParamInfo
        '
        txtParamInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        txtParamInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        txtParamInfo.ForceNewValue = False
        txtParamInfo.Location = New System.Drawing.Point(5, 750)
        txtParamInfo.Multiline = True
        txtParamInfo.Name = "txtParamInfo"
        txtParamInfo.ReadOnly = True
        txtParamInfo.Size = New System.Drawing.Size(625, 38)
        txtParamInfo.TabIndex = 12
        txtParamInfo.Tag = "0"
        txtParamInfo.Text = "Currently Loaded Template: "
        '
        'frmMainGUI
        '
        AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        ClientSize = New System.Drawing.Size(635, 719)
        Controls.Add(txtParamInfo)
        Controls.Add(tcMain)
        Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        MaximumSize = New System.Drawing.Size(720, 858)
        Menu = mnuMain
        MinimumSize = New System.Drawing.Size(240, 231)
        Name = "frmMainGUI"
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Text = "Sequest Parameter File Editor"
        tcMain.ResumeLayout(False)
        tabBasic.ResumeLayout(False)
        gbxIsoMods.ResumeLayout(False)
        gbxIsoMods.PerformLayout()
        gbxStaticMods.ResumeLayout(False)
        gbxStaticMods.PerformLayout()
        gbxDesc.ResumeLayout(False)
        gbxDesc.PerformLayout()
        gbxSearch.ResumeLayout(False)
        gbxSearch.PerformLayout()
        gbxDynMods.ResumeLayout(False)
        gbxDynMods.PerformLayout()
        tabAdvanced.ResumeLayout(False)
        gbxIonWeighting.ResumeLayout(False)
        gbxIonWeighting.PerformLayout()
        gbxMiscParams.ResumeLayout(False)
        gbxMiscParams.PerformLayout()
        gbxToleranceValues.ResumeLayout(False)
        gbxToleranceValues.PerformLayout()
        gbxSwitches.ResumeLayout(False)
        CType(StatModErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    <System.STAThread()> Public Shared Sub Main()
        System.Windows.Forms.Application.EnableVisualStyles()
        System.Windows.Forms.Application.DoEvents()
        Try
            System.Windows.Forms.Application.Run(New frmMainGUI)  ' replace frmDecode by the name of your form!!!
        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show("Last-chance exception caught: " & ex.Message & "; " & GetExceptionStackTrace(ex, True), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Console.WriteLine("")
            Console.WriteLine(GetExceptionStackTrace(ex, False))
        End Try
    End Sub

#End Region

    Public Shared mySettings As clsSettings

    ' Unused: Private basicTemplate As IBasicParams
    ' Unused: Private advTemplate As IAdvancedParams
    Public newParams As clsParams
    ' Unused: Private newBasic As IBasicParams
    ' Unused: Private newAdv As IAdvancedParams
    Private ReadOnly m_SettingsFileName As String = "ParamFileEditorSettings.xml"
    Const DEF_TEMPLATE_LABEL_TEXT As String = "Currently Loaded Template: "
    Private ReadOnly embeddedResource As New clsAccessEmbeddedRsrc
    Private m_UseAutoTweak As Boolean

    Private Sub RefreshTabs(frm As frmMainGUI, ParamsClass As clsParams)
        SetupBasicTab(frm, ParamsClass)
        SetupAdvancedTab(frm, ParamsClass)
        RetweakMasses()
    End Sub

    Private Sub frmMainGUI_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        mySettings = New clsSettings
        mySettings.LoadSettings(m_SettingsFileName)
        newParams = New clsParams
        m_DMSUpload = New clsDMSParamUpload(mySettings)


        With newParams
            .FileName = Mid(clsMainProcess.TemplateFileName, InStrRev(clsMainProcess.TemplateFileName, "\") + 1)
            .LoadTemplate(mySettings.TemplateFileName)
        End With

        RefreshTabs(Me, newParams)

        'AddBasicTabHandlers()
        'AddAdvTabHandlers()

    End Sub

    Private Sub SetupBasicTab(frm As frmMainGUI, bt As IBasicParams)
        RemoveBasicTabHandlers()

        Try

            'Load comboboxes
            With frm.cboParentMassType
                .BeginUpdate()
                .Items.Clear()
                .Items.Add("Average")
                .Items.Add("Monoisotopic")
                .EndUpdate()
            End With

            With frm.cboFragmentMassType
                .BeginUpdate()
                .Items.Clear()
                .Items.Add("Average")
                .Items.Add("Monoisotopic")
                .EndUpdate()
            End With


            With frm.cboParentMassUnits
                .BeginUpdate()
                .Items.Clear()
                .Items.Add("amu")
                .Items.Add("mmu")
                .Items.Add("ppm")
                .EndUpdate()
            End With

            With frm.cboFragmentMassUnits
                .BeginUpdate()
                .Items.Clear()
                .Items.Add("amu")

                ' MEM Note from February 2010
                '  Our version of Sequest [ TurboSEQUEST - PVM Slave v.27 (rev. 12), (c) 1998-2005 ]
                '   does not support mmu or ppm for Fragment Mass Units
                '  In fact, it's possible it completely ignores the fragment_ion_units entry in the .params file
                '  Thus, the only value added to cboFragmentMassUnits is "amu"
                ''.Items.Add("mmu")
                ''.Items.Add("ppm")
                .EndUpdate()
            End With

            With frm.cboEnzymeSelect
                .BeginUpdate()
                .Items.Clear()

                For Each enz As clsEnzymeDetails In frm.newParams.EnzymeList
                    .Items.Add(enz.EnzymeID & " - " & enz.EnzymeName & " [" & enz.EnzymeCleavePoints & "]")
                Next
                .EndUpdate()

                If .Items.Count = 0 Then
                    Try
                        MessageBox.Show("Current parameter file had an empty enzyme list; will load the defaults from " & Path.GetFileName(clsMainProcess.TemplateFileName), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        ' No enzymes were defined; update to show the defaults

                        Dim objParams As New clsParams

                        With objParams
                            .FileName = Path.GetFileName(clsMainProcess.TemplateFileName)
                            .LoadTemplate(mySettings.TemplateFileName)
                        End With

                        .BeginUpdate()
                        .Items.Clear()
                        For Each enz As clsEnzymeDetails In objParams.EnzymeList
                            .Items.Add(enz.EnzymeID & " - " & enz.EnzymeName & " [" & enz.EnzymeCleavePoints & "]")
                        Next
                        .EndUpdate()
                    Catch ex As Exception
                        MessageBox.Show("Error loading default enzyme definitions: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End Try

                End If
            End With

            With frm.cboCleavagePosition
                .DisplayMember = "DisplayName"
                .ValueMember = "Value"
                .BeginUpdate()
                .Items.Clear()
                .Items.Add(New ComboBoxContents("No Enzyme", "0"))
                .EndUpdate()
                .Enabled = False
            End With

            With frm.cboMissedCleavages
                .BeginUpdate()
                .Items.Clear()

                For counter = 0 To 5
                    .Items.Add(counter.ToString)
                Next
                .EndUpdate()
            End With

            txtDescription.Focus()

            With bt
                'Name and description info
                frm.txtParamInfo.Text = DEF_TEMPLATE_LABEL_TEXT & .FileName
                frm.txtDescription.Text = .Description

                'Search settings

                SetComboBox(frm.cboParentMassType, .ParentMassType, lblParentMassType.Text)
                SetComboBox(frm.cboFragmentMassType, .FragmentMassType, lblFragmentMassType.Text)
                SetComboBox(frm.cboEnzymeSelect, .SelectedEnzymeIndex(), lblEnzymeSelect.Text)
                SetComboBox(frm.cboMissedCleavages, .MaximumNumberMissedCleavages, lblMissedCleavages.Text)

                Try
                    frm.cboCleavagePosition.SelectedValue = .SelectedEnzymeCleavagePosition
                    EnableDisableCleavagePositionListbox()
                Catch ex As Exception
                    MessageBox.Show("Error selecting " & .SelectedEnzymeCleavagePosition.ToString & " in ComboBox " & lblCleavagePosition.Text & ": " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End Try

                frm.txtPartialSeq.Text = .PartialSequenceToMatch

                'Dynamic Mods
                frm.txtDynMod1List.Text = .DynamicMods.Dyn_Mod_n_AAList(1)
                frm.txtDynMod2List.Text = .DynamicMods.Dyn_Mod_n_AAList(2)
                frm.txtDynMod3List.Text = .DynamicMods.Dyn_Mod_n_AAList(3)
                frm.txtDynMod4List.Text = .DynamicMods.Dyn_Mod_n_AAList(4)
                frm.txtDynMod5List.Text = .DynamicMods.Dyn_Mod_n_AAList(5)
                frm.txtDynMod1MassDiff.Text = PRISM.StringUtilities.DblToString(.DynamicMods.Dyn_Mod_n_MassDiff(1), 5)
                frm.txtDynMod2MassDiff.Text = PRISM.StringUtilities.DblToString(.DynamicMods.Dyn_Mod_n_MassDiff(2), 5)
                frm.txtDynMod3MassDiff.Text = PRISM.StringUtilities.DblToString(.DynamicMods.Dyn_Mod_n_MassDiff(3), 5)
                frm.txtDynMod4MassDiff.Text = PRISM.StringUtilities.DblToString(.DynamicMods.Dyn_Mod_n_MassDiff(4), 5)
                frm.txtDynMod5MassDiff.Text = PRISM.StringUtilities.DblToString(.DynamicMods.Dyn_Mod_n_MassDiff(5), 5)

                frm.txtDynNTPep.Text = PRISM.StringUtilities.DblToString(.TermDynamicMods.Dyn_Mod_NTerm, 5)
                frm.txtDynCTPep.Text = PRISM.StringUtilities.DblToString(.TermDynamicMods.Dyn_Mod_CTerm, 5)

                'Static Mods
                frm.txtCTPep.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.CtermPeptide, 5)
                frm.txtCTProt.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.CtermProtein, 5)
                frm.txtNTPep.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.NtermPeptide, 5)
                frm.txtNTProt.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.NtermProtein, 5)
                frm.txtGly.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.G_Glycine, 5)
                frm.txtAla.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.A_Alanine, 5)
                frm.txtSer.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.S_Serine, 5)

                frm.txtPro.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.P_Proline, 5)
                frm.txtVal.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.V_Valine, 5)
                frm.txtThr.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.T_Threonine, 5)
                frm.txtCys.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.C_Cysteine, 5)
                frm.txtLeu.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.L_Leucine, 5)
                frm.txtIle.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.I_Isoleucine, 5)
                frm.TxtLorI.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.X_LorI, 5)

                frm.txtAsn.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.N_Asparagine, 5)
                frm.txtOrn.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.O_Ornithine, 5)
                frm.txtNandD.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.B_avg_NandD, 5)
                frm.txtAsp.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.D_Aspartic_Acid, 5)
                frm.txtGln.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.Q_Glutamine, 5)
                frm.txtLys.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.K_Lysine, 5)
                frm.txtQandE.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.Z_avg_QandE, 5)

                frm.txtGlu.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.E_Glutamic_Acid, 5)
                frm.txtMet.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.M_Methionine, 5)
                frm.txtHis.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.H_Histidine, 5)
                frm.txtPhe.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.F_Phenylalanine, 5)
                frm.txtArg.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.R_Arginine, 5)
                frm.txtTyr.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.Y_Tyrosine, 5)
                frm.txtTrp.Text = PRISM.StringUtilities.DblToString(.StaticModificationsList.W_Tryptophan, 5)

                frm.txtIsoC.Text = PRISM.StringUtilities.DblToString(.IsotopicModificationsList.Iso_C, 5)
                frm.txtIsoH.Text = PRISM.StringUtilities.DblToString(.IsotopicModificationsList.Iso_H, 5)
                frm.txtIsoO.Text = PRISM.StringUtilities.DblToString(.IsotopicModificationsList.Iso_O, 5)
                frm.txtIsoN.Text = PRISM.StringUtilities.DblToString(.IsotopicModificationsList.Iso_N, 5)
                frm.txtIsoS.Text = PRISM.StringUtilities.DblToString(.IsotopicModificationsList.Iso_S, 5)

                frm.txtDynMod1List.BackColor = SystemColors.Window
                frm.txtDynMod2List.BackColor = SystemColors.Window
                frm.txtDynMod3List.BackColor = SystemColors.Window
                frm.txtDynMod4List.BackColor = SystemColors.Window
                frm.txtDynMod5List.BackColor = SystemColors.Window
                frm.txtDynMod1MassDiff.BackColor = SystemColors.Window
                frm.txtDynMod2MassDiff.BackColor = SystemColors.Window
                frm.txtDynMod3MassDiff.BackColor = SystemColors.Window
                frm.txtDynMod4MassDiff.BackColor = SystemColors.Window
                frm.txtDynMod5MassDiff.BackColor = SystemColors.Window

                frm.txtDynNTPep.BackColor = SystemColors.Window
                frm.txtDynCTPep.BackColor = SystemColors.Window

                'Static Mods
                frm.txtCTPep.BackColor = SystemColors.Window
                frm.txtCTProt.BackColor = SystemColors.Window
                frm.txtNTPep.BackColor = SystemColors.Window
                frm.txtNTProt.BackColor = SystemColors.Window
                frm.txtGly.BackColor = SystemColors.Window
                frm.txtAla.BackColor = SystemColors.Window
                frm.txtSer.BackColor = SystemColors.Window

                frm.txtPro.BackColor = SystemColors.Window
                frm.txtVal.BackColor = SystemColors.Window
                frm.txtThr.BackColor = SystemColors.Window
                frm.txtCys.BackColor = SystemColors.Window
                frm.txtLeu.BackColor = SystemColors.Window
                frm.txtIle.BackColor = SystemColors.Window
                frm.TxtLorI.BackColor = SystemColors.Window

                frm.txtAsn.BackColor = SystemColors.Window
                frm.txtOrn.BackColor = SystemColors.Window
                frm.txtNandD.BackColor = SystemColors.Window
                frm.txtAsp.BackColor = SystemColors.Window
                frm.txtGln.BackColor = SystemColors.Window
                frm.txtLys.BackColor = SystemColors.Window
                frm.txtQandE.BackColor = SystemColors.Window

                frm.txtGlu.BackColor = SystemColors.Window
                frm.txtMet.BackColor = SystemColors.Window
                frm.txtHis.BackColor = SystemColors.Window
                frm.txtPhe.BackColor = SystemColors.Window
                frm.txtArg.BackColor = SystemColors.Window
                frm.txtTyr.BackColor = SystemColors.Window
                frm.txtTrp.BackColor = SystemColors.Window

                frm.txtIsoC.BackColor = SystemColors.Window
                frm.txtIsoH.BackColor = SystemColors.Window
                frm.txtIsoO.BackColor = SystemColors.Window
                frm.txtIsoN.BackColor = SystemColors.Window
                frm.txtIsoS.BackColor = SystemColors.Window
            End With

        Catch ex As Exception
            MessageBox.Show("Error in SetupBasicTab: " & ex.Message)
        End Try

        'TODO Change code to check connection status/availability and set accordingly
        frm.chkAutoTweak.Checked = True

        AddBasicTabHandlers()

    End Sub

    Private Sub SetupAdvancedTab(frm As frmMainGUI, at As IAdvancedParams)

        RemoveAdvTabHandlers()

        Try

            'Load Combo Box
            With frm.cboNucReadingFrame.Items
                .Clear()
                .Add("None - Protein Database Used")
                .Add("Forward - Frame 1")
                .Add("Forward - Frame 2")
                .Add("Forward - Frame 3")
                .Add("Reverse - Frame 1")
                .Add("Reverse - Frame 2")
                .Add("Reverse - Frame 3")
                .Add("Forward - All 3 Frames")
                .Add("Reverse - All 3 Frames")
                .Add("All Six Frames")
            End With

            With at
                'Setup checkboxes
                frm.chkUseAIons.Checked = Convert.ToBoolean(.IonSeries.Use_a_Ions)
                frm.chkUseBIons.Checked = Convert.ToBoolean(.IonSeries.Use_b_Ions)
                frm.chkUseYIons.Checked = Convert.ToBoolean(.IonSeries.Use_y_Ions)
                frm.cboParentMassUnits.SelectedIndex = .PeptideMassUnits

                If .FragmentMassUnits <> 0 Then

                    ' MEM Note from February 2010
                    '  Our version of Sequest [ TurboSEQUEST - PVM Slave v.27 (rev. 12), (c) 1998-2005 ]
                    '   does not support mmu or ppm for Fragment Mass Units
                    '  In fact, it's possible it completely ignores the fragment_ion_units entry in the .params file
                    '  Thus, the only value added allowed for .FragmentMassUnits is 0

                    ' Old parameter file with FragmentMassUnits set as a non-zero value
                    ' Force it to be 0
                    .FragmentMassUnits = 0

                    ' In addition, if .FragmentIonTolerance is over 1, then set it to 0
                    If .FragmentIonTolerance > 1 Then
                        .FragmentIonTolerance = 0
                    End If
                End If
                frm.cboFragmentMassUnits.SelectedIndex = .FragmentMassUnits

                frm.chkCreateOutputFiles.Checked = .CreateOutputFiles
                frm.chkShowFragmentIons.Checked = .ShowFragmentIons
                frm.chkRemovePrecursorPeaks.Checked = .RemovePrecursorPeak
                frm.chkPrintDupRefs.Checked = .PrintDuplicateReferences
                frm.chkResiduesInUpperCase.Checked = .AminoAcidsAllUpperCase

                'Setup Search Tolerances
                frm.txtPepMassTol.Text = PRISM.StringUtilities.DblToString(.PeptideMassTolerance, 5)
                frm.txtFragMassTol.Text = PRISM.StringUtilities.DblToString(.FragmentIonTolerance, 5)
                frm.txtPeakMatchingTol.Text = PRISM.StringUtilities.DblToString(.MatchedPeakMassTolerance, 5)
                frm.txtIonCutoff.Text = PRISM.StringUtilities.DblToString(.IonCutoffPercentage, 5)
                frm.txtMinProtMass.Text = PRISM.StringUtilities.DblToString(.MinimumProteinMassToSearch, 5)
                frm.txtMaxProtMass.Text = PRISM.StringUtilities.DblToString(.MaximumProteinMassToSearch, 5)
                frm.cboNucReadingFrame.SelectedIndex = .SelectedNucReadingFrame
                frm.txtNumResults.Text = CType(.NumberOfResultsToProcess, String)


                'Setup Misc Options
                frm.txtNumOutputLines.Text = PRISM.StringUtilities.DblToString(.NumberOfOutputLines, 5)
                frm.txtNumDescLines.Text = PRISM.StringUtilities.DblToString(.NumberOfDescriptionLines, 5)
                frm.txtMatchPeakCount.Text = PRISM.StringUtilities.DblToString(.NumberOfDetectedPeaksToMatch, 5)
                frm.txtMatchPeakCountErrors.Text = PRISM.StringUtilities.DblToString(.NumberOfAllowedDetectedPeakErrors, 5)
                frm.txtMaxAAPerDynMod.Text = PRISM.StringUtilities.DblToString(.MaximumNumAAPerDynMod, 5)
                frm.txtMaxDiffPerPeptide.Text = .MaximumDifferentialPerPeptide.ToString()

                'Setup Ion Weighting
                frm.txtAWeight.Text = PRISM.StringUtilities.DblToString(.IonSeries.a_Ion_Weighting, 5)
                frm.txtBWeight.Text = PRISM.StringUtilities.DblToString(.IonSeries.b_Ion_Weighting, 5)
                frm.txtCWeight.Text = PRISM.StringUtilities.DblToString(.IonSeries.c_Ion_Weighting, 5)
                frm.txtDWeight.Text = PRISM.StringUtilities.DblToString(.IonSeries.d_Ion_Weighting, 5)
                frm.txtVWeight.Text = PRISM.StringUtilities.DblToString(.IonSeries.v_Ion_Weighting, 5)
                frm.txtWWeight.Text = PRISM.StringUtilities.DblToString(.IonSeries.w_Ion_Weighting, 5)
                frm.txtXWeight.Text = PRISM.StringUtilities.DblToString(.IonSeries.x_Ion_Weighting, 5)
                frm.txtYWeight.Text = PRISM.StringUtilities.DblToString(.IonSeries.y_Ion_Weighting, 5)
                frm.txtZWeight.Text = PRISM.StringUtilities.DblToString(.IonSeries.z_Ion_Weighting, 5)
            End With

        Catch ex As Exception
            MessageBox.Show("Error in SetupAdvancedTab: " & ex.Message)
        End Try

        AddAdvTabHandlers()

    End Sub

    Private Sub SetComboBox(objComboBox As ComboBox, intSelectedIndex As Integer, strComboboxName As String)

        Try
            If objComboBox.Items.Count > intSelectedIndex Then
                objComboBox.SelectedIndex = intSelectedIndex
            Else
                MessageBox.Show("Unable to select item " & (intSelectedIndex + 1).ToString & " in ComboBox " & strComboboxName & "; row does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        Catch ex As Exception
            MessageBox.Show("Error selecting item " & (intSelectedIndex + 1).ToString & " in ComboBox " & strComboboxName & ": " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try

    End Sub

    Private Sub AddBasicTabHandlers()
        AddHandler txtDescription.Leave, AddressOf txtDescription_Leave

        AddHandler cboParentMassType.SelectedIndexChanged, AddressOf cboParentMassType_SelectedIndexChanged
        AddHandler cboFragmentMassType.SelectedIndexChanged, AddressOf cboFragmentMassType_SelectedIndexChanged

        AddHandler cboEnzymeSelect.SelectedIndexChanged, AddressOf cboEnzymeSelect_SelectedIndexChanged
        AddHandler cboCleavagePosition.SelectedIndexChanged, AddressOf cboCleavagePosition_SelectedIndexChanged
        AddHandler cboMissedCleavages.SelectedIndexChanged, AddressOf cboMissedCleavages_SelectedIndexChanged
        AddHandler txtPartialSeq.Validating, AddressOf AATextbox_Validating
        AddHandler txtPartialSeq.Validated, AddressOf txtPartialSeq_Validated

        AddHandler txtDynMod1List.Validating, AddressOf AATextbox_Validating
        AddHandler txtDynMod2List.Validating, AddressOf AATextbox_Validating
        AddHandler txtDynMod3List.Validating, AddressOf AATextbox_Validating
        AddHandler txtDynMod4List.Validating, AddressOf AATextbox_Validating
        AddHandler txtDynMod5List.Validating, AddressOf AATextbox_Validating

        AddHandler txtDynMod1List.Validated, AddressOf txtDynMod1List_Validated
        AddHandler txtDynMod2List.Validated, AddressOf txtDynMod2List_Validated
        AddHandler txtDynMod3List.Validated, AddressOf txtDynMod3List_Validated
        AddHandler txtDynMod4List.Validated, AddressOf txtDynMod4List_Validated
        AddHandler txtDynMod5List.Validated, AddressOf txtDynMod5List_Validated

        AddHandler txtDynMod1MassDiff.Validating, AddressOf numericTextbox_Validating
        AddHandler txtDynMod2MassDiff.Validating, AddressOf numericTextbox_Validating
        AddHandler txtDynMod3MassDiff.Validating, AddressOf numericTextbox_Validating
        AddHandler txtDynMod4MassDiff.Validating, AddressOf numericTextbox_Validating
        AddHandler txtDynMod5MassDiff.Validating, AddressOf numericTextbox_Validating
        AddHandler txtDynNTPep.Validating, AddressOf numericTextbox_Validating
        AddHandler txtDynCTPep.Validating, AddressOf numericTextbox_Validating

        AddHandler txtDynMod1MassDiff.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtDynMod2MassDiff.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtDynMod3MassDiff.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtDynMod4MassDiff.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtDynMod5MassDiff.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtDynNTPep.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtDynCTPep.KeyDown, AddressOf numericTextBox_KeyDown

        AddHandler txtDynMod1MassDiff.Validated, AddressOf txtDynMod1MassDiff_Validated
        AddHandler txtDynMod2MassDiff.Validated, AddressOf txtDynMod2MassDiff_Validated
        AddHandler txtDynMod3MassDiff.Validated, AddressOf txtDynMod3MassDiff_Validated
        AddHandler txtDynMod4MassDiff.Validated, AddressOf txtDynMod4MassDiff_Validated
        AddHandler txtDynMod5MassDiff.Validated, AddressOf txtDynMod5MassDiff_Validated
        AddHandler txtDynNTPep.Validated, AddressOf txtDynNTPep_Validated
        AddHandler txtDynCTPep.Validated, AddressOf txtDynCTPep_Validated

        AddHandler txtCTPep.Validating, AddressOf numericTextbox_Validating
        AddHandler txtCTProt.Validating, AddressOf numericTextbox_Validating
        AddHandler txtNTPep.Validating, AddressOf numericTextbox_Validating
        AddHandler txtNTProt.Validating, AddressOf numericTextbox_Validating
        AddHandler txtGly.Validating, AddressOf numericTextbox_Validating
        AddHandler txtAla.Validating, AddressOf numericTextbox_Validating
        AddHandler txtSer.Validating, AddressOf numericTextbox_Validating
        AddHandler txtPro.Validating, AddressOf numericTextbox_Validating
        AddHandler txtVal.Validating, AddressOf numericTextbox_Validating
        AddHandler txtThr.Validating, AddressOf numericTextbox_Validating
        AddHandler txtCys.Validating, AddressOf numericTextbox_Validating
        AddHandler txtLeu.Validating, AddressOf numericTextbox_Validating
        AddHandler txtIle.Validating, AddressOf numericTextbox_Validating
        AddHandler TxtLorI.Validating, AddressOf numericTextbox_Validating
        AddHandler txtAsn.Validating, AddressOf numericTextbox_Validating
        AddHandler txtOrn.Validating, AddressOf numericTextbox_Validating
        AddHandler txtNandD.Validating, AddressOf numericTextbox_Validating
        AddHandler txtAsp.Validating, AddressOf numericTextbox_Validating
        AddHandler txtGln.Validating, AddressOf numericTextbox_Validating
        AddHandler txtLys.Validating, AddressOf numericTextbox_Validating
        AddHandler txtQandE.Validating, AddressOf numericTextbox_Validating
        AddHandler txtGlu.Validating, AddressOf numericTextbox_Validating
        AddHandler txtMet.Validating, AddressOf numericTextbox_Validating
        AddHandler txtHis.Validating, AddressOf numericTextbox_Validating
        AddHandler txtPhe.Validating, AddressOf numericTextbox_Validating
        AddHandler txtArg.Validating, AddressOf numericTextbox_Validating
        AddHandler txtTyr.Validating, AddressOf numericTextbox_Validating
        AddHandler txtTrp.Validating, AddressOf numericTextbox_Validating
        AddHandler txtIsoC.Validating, AddressOf numericTextbox_Validating
        AddHandler txtIsoH.Validating, AddressOf numericTextbox_Validating
        AddHandler txtIsoO.Validating, AddressOf numericTextbox_Validating
        AddHandler txtIsoN.Validating, AddressOf numericTextbox_Validating
        AddHandler txtIsoS.Validating, AddressOf numericTextbox_Validating


        AddHandler txtCTPep.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtCTProt.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtNTPep.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtNTProt.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtGly.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtAla.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtSer.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtPro.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtVal.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtThr.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtCys.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtLeu.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtIle.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler TxtLorI.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtAsn.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtOrn.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtNandD.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtAsp.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtGln.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtLys.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtQandE.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtGlu.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtMet.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtHis.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtPhe.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtArg.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtTyr.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtTrp.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtIsoC.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtIsoH.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtIsoO.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtIsoN.KeyDown, AddressOf numericTextBox_KeyDown
        AddHandler txtIsoS.KeyDown, AddressOf numericTextBox_KeyDown


        AddHandler txtCTPep.Validated, AddressOf txtCTPep_Validated
        AddHandler txtCTProt.Validated, AddressOf txtCTProt_Validated
        AddHandler txtNTPep.Validated, AddressOf txtNTPep_Validated
        AddHandler txtNTProt.Validated, AddressOf txtNTProt_Validated
        AddHandler txtGly.Validated, AddressOf txtGly_Validated
        AddHandler txtAla.Validated, AddressOf txtAla_Validated
        AddHandler txtSer.Validated, AddressOf txtSer_Validated
        AddHandler txtPro.Validated, AddressOf txtPro_Validated
        AddHandler txtVal.Validated, AddressOf txtVal_Validated
        AddHandler txtThr.Validated, AddressOf txtThr_Validated
        AddHandler txtCys.Validated, AddressOf txtCys_Validated
        AddHandler txtLeu.Validated, AddressOf txtLeu_Validated
        AddHandler txtIle.Validated, AddressOf txtIle_Validated
        AddHandler TxtLorI.Validated, AddressOf TxtLorI_Validated
        AddHandler txtAsn.Validated, AddressOf txtAsn_Validated
        AddHandler txtOrn.Validated, AddressOf txtOrn_Validated
        AddHandler txtNandD.Validated, AddressOf txtNandD_Validated
        AddHandler txtAsp.Validated, AddressOf txtAsp_Validated
        AddHandler txtGln.Validated, AddressOf txtGln_Validated
        AddHandler txtLys.Validated, AddressOf txtLys_Validated
        AddHandler txtQandE.Validated, AddressOf txtQandE_Validated
        AddHandler txtGlu.Validated, AddressOf txtGlu_Validated
        AddHandler txtMet.Validated, AddressOf txtMet_Validated
        AddHandler txtHis.Validated, AddressOf txtHis_Validated
        AddHandler txtPhe.Validated, AddressOf txtPhe_Validated
        AddHandler txtArg.Validated, AddressOf txtArg_Validated
        AddHandler txtTyr.Validated, AddressOf txtTyr_Validated
        AddHandler txtTrp.Validated, AddressOf txtTrp_Validated
        AddHandler txtIsoC.Validated, AddressOf txtIsoC_Validated
        AddHandler txtIsoH.Validated, AddressOf txtIsoH_Validated
        AddHandler txtIsoO.Validated, AddressOf txtIsoO_Validated
        AddHandler txtIsoN.Validated, AddressOf txtIsoN_Validated
        AddHandler txtIsoS.Validated, AddressOf txtIsoS_Validated
    End Sub
    Private Sub AddAdvTabHandlers()
        AddHandler txtPepMassTol.Leave, AddressOf txtPepMassTol_Leave
        AddHandler txtFragMassTol.Leave, AddressOf txtFragMassTol_Leave
        AddHandler txtPeakMatchingTol.Leave, AddressOf txtPeakMatchingTol_Leave
        AddHandler txtIonCutoff.Leave, AddressOf txtIonCutoff_Leave
        AddHandler txtMinProtMass.Leave, AddressOf txtMinProtMass_Leave
        AddHandler cboFragmentMassUnits.SelectedIndexChanged, AddressOf cboFragmentMassUnits_SelectedIndexChanged
        AddHandler cboParentMassUnits.SelectedIndexChanged, AddressOf cboParentMassUnits_SelectedIndexChanged
        AddHandler txtMaxProtMass.Leave, AddressOf txtMaxProtMass_Leave

        AddHandler txtNumOutputLines.Leave, AddressOf txtNumOutputLines_Leave
        AddHandler txtNumDescLines.Leave, AddressOf txtNumDescLines_Leave
        AddHandler txtNumResults.Leave, AddressOf txtNumResults_Leave
        AddHandler txtMatchPeakCount.Leave, AddressOf txtMatchPeakCount_Leave
        AddHandler txtMatchPeakCountErrors.Leave, AddressOf txtMatchPeakCountErrors_Leave
        AddHandler txtMaxAAPerDynMod.Leave, AddressOf txtMaxAAPerDynMod_Leave
        AddHandler txtMaxDiffPerPeptide.Leave, AddressOf txtMaxDiffPerPeptide_Leave
        AddHandler cboNucReadingFrame.SelectedIndexChanged, AddressOf cboNucReadingFrame_SelectedIndexChanged

        AddHandler chkUseAIons.CheckedChanged, AddressOf chkUseAIons_CheckedChanged
        AddHandler chkUseBIons.CheckedChanged, AddressOf chkUseBIons_CheckedChanged
        AddHandler chkUseYIons.CheckedChanged, AddressOf chkUseYIons_CheckedChanged
        AddHandler chkCreateOutputFiles.CheckedChanged, AddressOf chkCreateOutputFiles_CheckedChanged
        AddHandler chkShowFragmentIons.CheckedChanged, AddressOf chkShowFragmentIons_CheckedChanged
        AddHandler chkRemovePrecursorPeaks.CheckedChanged, AddressOf chkRemovePrecursorPeaks_CheckedChanged
        AddHandler chkPrintDupRefs.CheckedChanged, AddressOf chkPrintDupRefs_CheckedChanged
        AddHandler chkResiduesInUpperCase.CheckedChanged, AddressOf chkResiduesInUpperCase_CheckedChanged

        AddHandler txtAWeight.Leave, AddressOf txtAWeight_Leave
        AddHandler txtBWeight.Leave, AddressOf txtBWeight_Leave
        AddHandler txtCWeight.Leave, AddressOf txtCWeight_Leave
        AddHandler txtDWeight.Leave, AddressOf txtDWeight_Leave
        AddHandler txtVWeight.Leave, AddressOf txtVWeight_Leave
        AddHandler txtWWeight.Leave, AddressOf txtWWeight_Leave
        AddHandler txtXWeight.Leave, AddressOf txtXWeight_Leave
        AddHandler txtYWeight.Leave, AddressOf txtYWeight_Leave
        AddHandler txtZWeight.Leave, AddressOf txtZWeight_Leave

    End Sub

    Private Sub RemoveBasicTabHandlers()
        RemoveHandler txtDescription.Leave, AddressOf txtDescription_Leave

        RemoveHandler cboFragmentMassType.SelectedIndexChanged, AddressOf cboFragmentMassType_SelectedIndexChanged
        RemoveHandler cboParentMassType.SelectedIndexChanged, AddressOf cboParentMassType_SelectedIndexChanged

        RemoveHandler cboEnzymeSelect.SelectedIndexChanged, AddressOf cboEnzymeSelect_SelectedIndexChanged
        RemoveHandler cboCleavagePosition.SelectedIndexChanged, AddressOf cboCleavagePosition_SelectedIndexChanged
        RemoveHandler cboMissedCleavages.SelectedIndexChanged, AddressOf cboMissedCleavages_SelectedIndexChanged
        RemoveHandler txtPartialSeq.Validating, AddressOf AATextbox_Validating
        RemoveHandler txtPartialSeq.Validated, AddressOf txtPartialSeq_Validated

        RemoveHandler txtDynMod1List.Validating, AddressOf AATextbox_Validating
        RemoveHandler txtDynMod2List.Validating, AddressOf AATextbox_Validating
        RemoveHandler txtDynMod3List.Validating, AddressOf AATextbox_Validating
        RemoveHandler txtDynMod4List.Validating, AddressOf AATextbox_Validating
        RemoveHandler txtDynMod5List.Validating, AddressOf AATextbox_Validating

        RemoveHandler txtDynMod1List.Validated, AddressOf txtDynMod1List_Validated
        RemoveHandler txtDynMod2List.Validated, AddressOf txtDynMod2List_Validated
        RemoveHandler txtDynMod3List.Validated, AddressOf txtDynMod3List_Validated
        RemoveHandler txtDynMod4List.Validated, AddressOf txtDynMod4List_Validated
        RemoveHandler txtDynMod5List.Validated, AddressOf txtDynMod5List_Validated

        RemoveHandler txtDynMod1MassDiff.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtDynMod2MassDiff.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtDynMod3MassDiff.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtDynMod4MassDiff.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtDynMod5MassDiff.Validating, AddressOf numericTextbox_Validating

        RemoveHandler txtDynMod1MassDiff.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtDynMod2MassDiff.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtDynMod3MassDiff.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtDynMod4MassDiff.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtDynMod5MassDiff.KeyDown, AddressOf numericTextBox_KeyDown

        RemoveHandler txtDynMod1MassDiff.Validated, AddressOf txtDynMod1MassDiff_Validated
        RemoveHandler txtDynMod2MassDiff.Validated, AddressOf txtDynMod2MassDiff_Validated
        RemoveHandler txtDynMod3MassDiff.Validated, AddressOf txtDynMod3MassDiff_Validated
        RemoveHandler txtDynMod4MassDiff.Validated, AddressOf txtDynMod4MassDiff_Validated
        RemoveHandler txtDynMod5MassDiff.Validated, AddressOf txtDynMod5MassDiff_Validated

        RemoveHandler txtCTPep.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtCTProt.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtNTPep.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtNTProt.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtGly.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtAla.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtSer.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtPro.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtVal.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtThr.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtCys.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtLeu.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtIle.Validating, AddressOf numericTextbox_Validating
        RemoveHandler TxtLorI.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtAsn.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtOrn.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtNandD.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtAsp.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtGln.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtLys.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtQandE.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtGlu.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtMet.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtHis.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtPhe.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtArg.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtTyr.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtTrp.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtIsoC.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtIsoH.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtIsoO.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtIsoN.Validating, AddressOf numericTextbox_Validating
        RemoveHandler txtIsoS.Validating, AddressOf numericTextbox_Validating


        RemoveHandler txtCTPep.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtCTProt.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtNTPep.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtNTProt.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtGly.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtAla.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtSer.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtPro.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtVal.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtThr.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtCys.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtLeu.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtIle.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler TxtLorI.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtAsn.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtOrn.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtNandD.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtAsp.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtGln.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtLys.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtQandE.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtGlu.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtMet.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtHis.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtPhe.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtArg.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtTyr.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtTrp.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtIsoC.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtIsoH.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtIsoO.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtIsoN.KeyDown, AddressOf numericTextBox_KeyDown
        RemoveHandler txtIsoS.KeyDown, AddressOf numericTextBox_KeyDown


        RemoveHandler txtCTPep.Validated, AddressOf txtCTPep_Validated
        RemoveHandler txtCTProt.Validated, AddressOf txtCTProt_Validated
        RemoveHandler txtNTPep.Validated, AddressOf txtNTPep_Validated
        RemoveHandler txtNTProt.Validated, AddressOf txtNTProt_Validated
        RemoveHandler txtGly.Validated, AddressOf txtGly_Validated
        RemoveHandler txtAla.Validated, AddressOf txtAla_Validated
        RemoveHandler txtSer.Validated, AddressOf txtSer_Validated
        RemoveHandler txtPro.Validated, AddressOf txtPro_Validated
        RemoveHandler txtVal.Validated, AddressOf txtVal_Validated
        RemoveHandler txtThr.Validated, AddressOf txtThr_Validated
        RemoveHandler txtCys.Validated, AddressOf txtCys_Validated
        RemoveHandler txtLeu.Validated, AddressOf txtLeu_Validated
        RemoveHandler txtIle.Validated, AddressOf txtIle_Validated
        RemoveHandler TxtLorI.Validated, AddressOf TxtLorI_Validated
        RemoveHandler txtAsn.Validated, AddressOf txtAsn_Validated
        RemoveHandler txtOrn.Validated, AddressOf txtOrn_Validated
        RemoveHandler txtNandD.Validated, AddressOf txtNandD_Validated
        RemoveHandler txtAsp.Validated, AddressOf txtAsp_Validated
        RemoveHandler txtGln.Validated, AddressOf txtGln_Validated
        RemoveHandler txtLys.Validated, AddressOf txtLys_Validated
        RemoveHandler txtQandE.Validated, AddressOf txtQandE_Validated
        RemoveHandler txtGlu.Validated, AddressOf txtGlu_Validated
        RemoveHandler txtMet.Validated, AddressOf txtMet_Validated
        RemoveHandler txtHis.Validated, AddressOf txtHis_Validated
        RemoveHandler txtPhe.Validated, AddressOf txtPhe_Validated
        RemoveHandler txtArg.Validated, AddressOf txtArg_Validated
        RemoveHandler txtTyr.Validated, AddressOf txtTyr_Validated
        RemoveHandler txtTrp.Validated, AddressOf txtTrp_Validated
        RemoveHandler txtIsoC.Validated, AddressOf txtIsoC_Validated
        RemoveHandler txtIsoH.Validated, AddressOf txtIsoH_Validated
        RemoveHandler txtIsoO.Validated, AddressOf txtIsoO_Validated
        RemoveHandler txtIsoN.Validated, AddressOf txtIsoN_Validated
        RemoveHandler txtIsoS.Validated, AddressOf txtIsoS_Validated
    End Sub

    Private Sub RemoveAdvTabHandlers()
        RemoveHandler txtPepMassTol.Leave, AddressOf txtPepMassTol_Leave
        RemoveHandler txtFragMassTol.Leave, AddressOf txtFragMassTol_Leave
        RemoveHandler txtPeakMatchingTol.Leave, AddressOf txtPeakMatchingTol_Leave
        RemoveHandler txtIonCutoff.Leave, AddressOf txtIonCutoff_Leave
        RemoveHandler txtMinProtMass.Leave, AddressOf txtMinProtMass_Leave
        RemoveHandler txtMaxProtMass.Leave, AddressOf txtMaxProtMass_Leave
        RemoveHandler cboParentMassUnits.SelectedIndexChanged, AddressOf cboParentMassUnits_SelectedIndexChanged
        RemoveHandler cboFragmentMassUnits.SelectedIndexChanged, AddressOf cboFragmentMassUnits_SelectedIndexChanged

        RemoveHandler txtNumOutputLines.Leave, AddressOf txtNumOutputLines_Leave
        RemoveHandler txtNumDescLines.Leave, AddressOf txtNumDescLines_Leave
        RemoveHandler txtNumResults.Leave, AddressOf txtNumResults_Leave
        RemoveHandler txtMatchPeakCount.Leave, AddressOf txtMatchPeakCount_Leave
        RemoveHandler txtMatchPeakCountErrors.Leave, AddressOf txtMatchPeakCountErrors_Leave
        RemoveHandler txtMaxAAPerDynMod.Leave, AddressOf txtMaxAAPerDynMod_Leave
        RemoveHandler txtMaxDiffPerPeptide.Leave, AddressOf txtMaxDiffPerPeptide_Leave
        RemoveHandler cboNucReadingFrame.SelectedIndexChanged, AddressOf cboNucReadingFrame_SelectedIndexChanged

        RemoveHandler chkUseAIons.CheckedChanged, AddressOf chkUseAIons_CheckedChanged
        RemoveHandler chkUseBIons.CheckedChanged, AddressOf chkUseBIons_CheckedChanged
        RemoveHandler chkUseYIons.CheckedChanged, AddressOf chkUseYIons_CheckedChanged
        RemoveHandler chkCreateOutputFiles.CheckedChanged, AddressOf chkCreateOutputFiles_CheckedChanged
        RemoveHandler chkShowFragmentIons.CheckedChanged, AddressOf chkShowFragmentIons_CheckedChanged
        RemoveHandler chkRemovePrecursorPeaks.CheckedChanged, AddressOf chkRemovePrecursorPeaks_CheckedChanged
        RemoveHandler chkPrintDupRefs.CheckedChanged, AddressOf chkPrintDupRefs_CheckedChanged
        RemoveHandler chkResiduesInUpperCase.CheckedChanged, AddressOf chkResiduesInUpperCase_CheckedChanged

        RemoveHandler txtAWeight.Leave, AddressOf txtAWeight_Leave
        RemoveHandler txtBWeight.Leave, AddressOf txtBWeight_Leave
        RemoveHandler txtCWeight.Leave, AddressOf txtCWeight_Leave
        RemoveHandler txtDWeight.Leave, AddressOf txtDWeight_Leave
        RemoveHandler txtVWeight.Leave, AddressOf txtVWeight_Leave
        RemoveHandler txtWWeight.Leave, AddressOf txtWWeight_Leave
        RemoveHandler txtXWeight.Leave, AddressOf txtXWeight_Leave
        RemoveHandler txtYWeight.Leave, AddressOf txtYWeight_Leave
        RemoveHandler txtZWeight.Leave, AddressOf txtZWeight_Leave

    End Sub

    Private Sub EnableDisableCleavagePositionListbox()

        Dim intIndex As Integer

        intIndex = cboEnzymeSelect.SelectedIndex
        If intIndex <= 0 Then
            With cboCleavagePosition
                .DisplayMember = "DisplayName"
                .ValueMember = "Value"
                .BeginUpdate()
                .Items.Clear()
                .Items.Add(New ComboBoxContents("No Enzyme", "0"))
                .EndUpdate()
            End With
            cboCleavagePosition.Text = "No Enzyme"
            cboCleavagePosition.Enabled = False
        Else
            With cboCleavagePosition
                .DisplayMember = "DisplayName"
                .ValueMember = "Value"
                .BeginUpdate()
                .Items.Clear()
                .Items.Add(New ComboBoxContents("Full Cleavage", "1"))
                .Items.Add(New ComboBoxContents("Partial Cleavage", "2"))
                .Items.Add(New ComboBoxContents("N-Term Partial Cleavage", "3"))
                .Items.Add(New ComboBoxContents("C-Term Partial Cleavage", "4"))
                .EndUpdate()
            End With

            If newParams.SelectedEnzymeCleavagePosition = 0 Then
                newParams.SelectedEnzymeCleavagePosition = 1
            End If

            Dim cItem As ComboBoxContents
            For Each cItem In cboCleavagePosition.Items
                If CInt(cItem.Value) = newParams.SelectedEnzymeCleavagePosition Then
                    cboCleavagePosition.Text = cItem.DisplayName
                    Exit For
                End If
            Next

            cboCleavagePosition.Enabled = True
        End If

    End Sub

    Private Sub LoadParamsetFromDMS(ParamSetID As Integer)
        Try
            If m_clsParamsFromDMS Is Nothing Then
                m_clsParamsFromDMS = LoadDMSParamsClass(mySettings)
            Else
                ' Need to run a Refresh of m_clsParamsFromDMS so that it knows about any newly imported param files
                m_clsParamsFromDMS.RefreshParamsFromDMS()
            End If
            newParams = m_clsParamsFromDMS.ReadParamsFromDMS(ParamSetID)

            Dim iso As New clsDeconvolveIsoMods(mySettings.DMS_ConnectionString)
            newParams = iso.DeriveIsoMods(newParams)

            RefreshTabs(Me, newParams)
        Catch ex As Exception
            MessageBox.Show("Error in LoadParamsetFromDMS: " & ex.Message)
        End Try

    End Sub

    Private Sub LoadParamsetFromFile(FilePath As String)
        newParams.LoadTemplate(FilePath)
        Dim iso As New clsDeconvolveIsoMods(mySettings.DMS_ConnectionString)
        newParams = iso.DeriveIsoMods(newParams)
        RefreshTabs(Me, newParams)
    End Sub

    Private Function SetupAutoTweak(sender As Object) As Boolean
        Dim successCode As Boolean
        Try
            If m_clsMassTweaker Is Nothing Then
                m_clsMassTweaker = New clsMassTweaker(mySettings)
                successCode = True
                StatModErrorProvider.SetError(DirectCast(sender, Control), "")
            End If
        Catch ex As Exception
            StatModErrorProvider.SetError(DirectCast(sender, Control), "Could not connect to DMS to retrieve Global Mod Info")
            successCode = False
        End Try

        Return successCode

    End Function

    Private Function LoadDMSParamsClass(Settings As IProgramSettings) As clsParamsFromDMS
        Dim dms As New clsParamsFromDMS(Settings.DMS_ConnectionString)
        Return dms
    End Function

    ' Unused
    'Private Function LoadDMSParamUploadClass(Settings As clsSettings) As clsDMSParamUpload
    '    Dim dms As New clsDMSParamUpload(Settings)
    '    Return dms
    'End Function

    Public Sub LoadDMSParamsFromID(ParamSetID As Integer)
        LoadParamsetFromDMS(ParamSetID)
    End Sub

    Public Sub LoadParamsFromFile(FilePath As String)
        LoadParamsetFromFile(FilePath)
    End Sub

    Private Sub chkAutoTweak_CheckedChanged(sender As Object, e As EventArgs) Handles chkAutoTweak.CheckedChanged
        If chkAutoTweak.CheckState = CheckState.Checked Then
            Dim success = SetupAutoTweak(sender)
            m_UseAutoTweak = True
            cmdReTweak.Enabled = True
        Else
            m_UseAutoTweak = False
            cmdReTweak.Enabled = False
        End If

    End Sub

#Region " [Basic] Name and Description Handlers "

    Private Sub txtDescription_Leave(sender As Object, e As EventArgs)
        newParams.Description = txtDescription.Text
    End Sub

#End Region

#Region " [Basic] Search Settings Handlers "

    Private Sub cboParentMassType_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            newParams.ParentMassType = CType(cboParentMassType.SelectedIndex, IBasicParams.MassTypeList)
        Catch ex As Exception
            cboParentMassType.SelectedIndex = CInt(newParams.ParentMassType)
        End Try
        UpdateDescription()
    End Sub
    Private Sub cboFragmentMassType_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            newParams.FragmentMassType = CType(cboFragmentMassType.SelectedIndex, IBasicParams.MassTypeList)
        Catch ex As Exception
            cboFragmentMassType.SelectedIndex = CInt(newParams.FragmentMassType)
        End Try
        UpdateDescription()
    End Sub
    Private Sub cboFragmentMassUnits_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            newParams.FragmentMassUnits = CType(cboFragmentMassUnits.SelectedIndex, IAdvancedParams.MassUnitList)
        Catch ex As Exception
            cboFragmentMassUnits.SelectedIndex = CInt(newParams.FragmentMassUnits)
        End Try
        UpdateDescription()
    End Sub
    Private Sub cboParentMassUnits_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            newParams.PeptideMassUnits = CType(cboParentMassUnits.SelectedIndex, IAdvancedParams.MassUnitList)
        Catch ex As Exception
            cboParentMassUnits.SelectedIndex = CInt(newParams.PeptideMassUnits)
        End Try
        UpdateDescription()
    End Sub

    Private Sub cboEnzymeSelect_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim tmpIndex As Integer
            tmpIndex = cboEnzymeSelect.SelectedIndex
            newParams.SelectedEnzymeIndex = tmpIndex
            newParams.SelectedEnzymeDetails = newParams.RetrieveEnzymeDetails(tmpIndex)

            EnableDisableCleavagePositionListbox()
        Catch ex As Exception
            MessageBox.Show("Exception in cboEnzymeSelect_SelectedIndexChanged: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
        UpdateDescription()
    End Sub

    Private Sub cboMissedCleavages_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            newParams.MaximumNumberMissedCleavages = cboMissedCleavages.SelectedIndex
        Catch ex As Exception
            cboMissedCleavages.SelectedIndex = newParams.MaximumNumberMissedCleavages
        End Try
        UpdateDescription()
    End Sub

    Private Sub cboCleavagePosition_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCleavagePosition.SelectedIndexChanged
        Try
            Dim cbc = DirectCast(cboCleavagePosition.SelectedItem, ComboBoxContents)
            If Not cbc Is Nothing Then
                newParams.SelectedEnzymeCleavagePosition = CInt(cbc.Value)
            Else
                newParams.SelectedEnzymeCleavagePosition = 1
            End If
        Catch ex As Exception
            Try
                cboCleavagePosition.SelectedValue = newParams.SelectedEnzymeCleavagePosition
            Catch ex2 As Exception
                ' Ignore errors when handing errors
            End Try
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtPartialSeq_Validated(sender As Object, e As EventArgs)
        newParams.PartialSequenceToMatch = txtPartialSeq.Text
        UpdateDescription()
    End Sub

#End Region

#Region " [Basic] Dynamic Modification Handlers "

    Private Sub txtDynMod1List_Validated(sender As Object, e As EventArgs)
        UpdateDynamicModAA(sender, txtDynMod1List, txtDynMod1MassDiff, 1)
    End Sub

    Private Sub txtDynMod2List_Validated(sender As Object, e As EventArgs)
        UpdateDynamicModAA(sender, txtDynMod2List, txtDynMod2MassDiff, 2)
    End Sub

    Private Sub txtDynMod3List_Validated(sender As Object, e As EventArgs)
        UpdateDynamicModAA(sender, txtDynMod3List, txtDynMod3MassDiff, 3)
    End Sub

    Private Sub txtDynMod4List_Validated(sender As Object, e As EventArgs)
        UpdateDynamicModAA(sender, txtDynMod4List, txtDynMod4MassDiff, 4)
    End Sub

    Private Sub txtDynMod5List_Validated(sender As Object, e As EventArgs)
        UpdateDynamicModAA(sender, txtDynMod5List, txtDynMod5MassDiff, 5)
    End Sub


    Private Sub txtDynMod1MassDiff_Validated(sender As Object, e As EventArgs)
        UpdateDynamicModMass(txtDynMod1MassDiff, txtDynMod1List, 1)
    End Sub

    Private Sub txtDynMod2MassDiff_Validated(sender As Object, e As EventArgs)
        UpdateDynamicModMass(txtDynMod2MassDiff, txtDynMod2List, 2)
    End Sub

    Private Sub txtDynMod3MassDiff_Validated(sender As Object, e As EventArgs)
        UpdateDynamicModMass(txtDynMod3MassDiff, txtDynMod3List, 3)
    End Sub

    Private Sub txtDynMod4MassDiff_Validated(sender As Object, e As EventArgs)
        UpdateDynamicModMass(txtDynMod4MassDiff, txtDynMod4List, 4)
    End Sub

    Private Sub txtDynMod5MassDiff_Validated(sender As Object, e As EventArgs)
        UpdateDynamicModMass(txtDynMod5MassDiff, txtDynMod5List, 5)
    End Sub

    Private Sub UpdateDynamicModAA(sender As Object,
                                   ByRef txtModAATextbox As TextBox,
                                   ByRef txtModMassTextbox As NumericTextBox,
                                   intModNumber As Integer)

        Try
            If txtModAATextbox.TextLength = 0 Then
                txtModAATextbox.Text = "C"
            End If

            StatModErrorProvider.SetError(DirectCast(sender, Control), "")
            txtModAATextbox.Text = txtModAATextbox.Text.ToUpper
            If Math.Abs(CSng(txtModMassTextbox.Text)) > Single.Epsilon Then
                ' Only update this mod if the mass is non-zero
                newParams.DynamicMods.Dyn_Mod_n_AAList(intModNumber) = txtModAATextbox.Text
            Else
                ' If this mod, and all other mods after this mod are 0, then remove this mod and all subsequent mods
                If newParams.DynamicMods.Count = intModNumber Then
                    newParams.DynamicMods.Remove(intModNumber - 1)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Exception in UpdateDynamicModAA: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try

        UpdateDescription()

    End Sub

    Private Sub UpdateDynamicModMass(ByRef txtModMassTextbox As NumericTextBox,
                                     ByRef txtModAATextbox As TextBox,
                                     intModNumber As Integer)
        Dim dblModMass As Double

        Try
            dblModMass = CDbl(txtModMassTextbox.Text)
            If Math.Abs(dblModMass) > Single.Epsilon Then
                ' Make sure this mod is defined
                newParams.DynamicMods.Dyn_Mod_n_AAList(intModNumber) = txtModAATextbox.Text
            End If

            If newParams.DynamicMods.Count < intModNumber And Math.Abs(dblModMass) < Single.Epsilon Then
                ' Nothing to update
            Else
                If Math.Abs(dblModMass) > Single.Epsilon Then
                    newParams.DynamicMods.Dyn_Mod_n_MassDiff(intModNumber) = dblModMass
                Else
                    ' If this mod, and all other mods after this mod are 0, then remove this mod and all subsequent mods
                    If newParams.DynamicMods.Count = intModNumber Then
                        newParams.DynamicMods.Remove(intModNumber - 1)
                    Else
                        newParams.DynamicMods.Dyn_Mod_n_MassDiff(intModNumber) = dblModMass
                    End If
                End If
            End If

        Catch ex As Exception
            MessageBox.Show("Exception in UpdateDynamicModMass: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try

        UpdateDescription()

    End Sub

    Private Sub txtDynCTPep_Validated(sender As Object, e As EventArgs)
        Try
            newParams.TermDynamicMods.Dyn_Mod_CTerm = CDbl(txtDynCTPep.Text)
        Catch
            txtDynCTPep.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtDynNTPep_Validated(sender As Object, e As EventArgs)
        Try
            newParams.TermDynamicMods.Dyn_Mod_NTerm = CDbl(txtDynNTPep.Text)
        Catch
            txtDynNTPep.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub

#End Region

#Region " [Basic] Static Modification Handlers "

    Private Sub txtCTPep_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.CtermPeptide = CDbl(txtCTPep.Text)
        Catch
            txtCTPep.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtCTProt_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.CtermProtein = CDbl(txtCTProt.Text)
        Catch
            txtCTProt.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtNTPep_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.NtermPeptide = CDbl(txtNTPep.Text)
        Catch
            txtNTPep.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtNTProt_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.NtermProtein = CDbl(txtNTProt.Text)
        Catch
            txtNTProt.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtGly_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.G_Glycine = CDbl(txtGly.Text)
        Catch
            txtGly.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtAla_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.A_Alanine = CDbl(txtAla.Text)
        Catch
            txtAla.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtSer_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.S_Serine = CDbl(txtSer.Text)
        Catch
            txtSer.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtPro_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.P_Proline = CDbl(txtPro.Text)
        Catch
            txtPro.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtVal_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.V_Valine = CDbl(txtVal.Text)
        Catch
            txtVal.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtThr_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.T_Threonine = CDbl(txtThr.Text)
        Catch
            txtThr.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtCys_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.C_Cysteine = CDbl(txtCys.Text)
        Catch
            txtCys.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtLeu_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.L_Leucine = CDbl(txtLeu.Text)
        Catch
            txtLeu.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtIle_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.I_Isoleucine = CDbl(txtIle.Text)
        Catch
            txtIle.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub TxtLorI_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.X_LorI = CDbl(TxtLorI.Text)
        Catch
            TxtLorI.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtAsn_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.N_Asparagine = CDbl(txtAsn.Text)
        Catch
            txtAsn.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtOrn_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.O_Ornithine = CDbl(txtOrn.Text)
        Catch
            txtOrn.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtNandD_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.B_avg_NandD = CDbl(txtNandD.Text)
        Catch
            txtNandD.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtAsp_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.D_Aspartic_Acid = CDbl(txtAsp.Text)
        Catch
            txtAsp.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtGln_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.Q_Glutamine = CDbl(txtGln.Text)
        Catch
            txtGln.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtLys_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.K_Lysine = CDbl(txtLys.Text)
        Catch
            txtLys.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtQandE_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.Z_avg_QandE = CDbl(txtQandE.Text)
        Catch
            txtQandE.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtGlu_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.E_Glutamic_Acid = CDbl(txtGlu.Text)
        Catch
            txtGlu.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtMet_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.M_Methionine = CDbl(txtMet.Text)
        Catch
            txtMet.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtHis_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.H_Histidine = CDbl(txtHis.Text)
        Catch
            txtHis.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtPhe_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.F_Phenylalanine = CDbl(txtPhe.Text)
        Catch
            txtPhe.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtArg_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.R_Arginine = CDbl(txtArg.Text)
        Catch
            txtArg.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtTyr_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.Y_Tyrosine = CDbl(txtTyr.Text)
        Catch
            txtTyr.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtTrp_Validated(sender As Object, e As EventArgs)
        Try
            newParams.StaticModificationsList.W_Tryptophan = CDbl(txtTrp.Text)
        Catch
            txtTrp.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub

#End Region

#Region " [Basic] Isotopic Modification Handlers "
    Private Sub txtIsoC_Validated(sender As Object, e As EventArgs)
        Dim thisControl = DirectCast(sender, Control)
        Try
            newParams.IsotopicMods.Iso_C = CDbl(thisControl.Text)
        Catch
            thisControl.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtIsoH_Validated(sender As Object, e As EventArgs)
        Dim thisControl = DirectCast(sender, Control)
        Try
            newParams.IsotopicMods.Iso_H = CDbl(thisControl.Text)
        Catch
            thisControl.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtIsoO_Validated(sender As Object, e As EventArgs)
        Dim thisControl = DirectCast(sender, Control)
        Try
            newParams.IsotopicMods.Iso_O = CDbl(thisControl.Text)
        Catch
            thisControl.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtIsoN_Validated(sender As Object, e As EventArgs)
        Dim thisControl = DirectCast(sender, Control)
        Try
            newParams.IsotopicMods.Iso_N = CDbl(thisControl.Text)
        Catch
            thisControl.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtIsoS_Validated(sender As Object, e As EventArgs)
        Dim thisControl = DirectCast(sender, Control)
        Try
            newParams.IsotopicMods.Iso_S = CDbl(thisControl.Text)
        Catch
            thisControl.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub


#End Region

#Region " [Advanced] Searching Tolerances "

    Private Sub txtPepMassTol_Leave(sender As Object, e As EventArgs)
        Try
            newParams.PeptideMassTolerance = CSng(txtPepMassTol.Text)
        Catch ex As Exception
            txtPepMassTol.Text = newParams.PeptideMassTolerance.ToString()
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtFragMassTol_Leave(sender As Object, e As EventArgs)
        Try
            newParams.FragmentIonTolerance = CSng(txtFragMassTol.Text)
        Catch ex As Exception
            txtFragMassTol.Text = newParams.FragmentIonTolerance.ToString()
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtPeakMatchingTol_Leave(sender As Object, e As EventArgs)
        Try
            newParams.MatchedPeakMassTolerance = CSng(txtPeakMatchingTol.Text)
        Catch ex As Exception
            txtPeakMatchingTol.Text = newParams.MatchedPeakMassTolerance.ToString()
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtIonCutoff_Leave(sender As Object, e As EventArgs)
        Try
            newParams.IonCutoffPercentage = CSng(txtIonCutoff.Text)
        Catch ex As Exception
            txtIonCutoff.Text = newParams.IonCutoffPercentage.ToString()
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtMinProtMass_Leave(sender As Object, e As EventArgs)
        Try
            newParams.MinimumProteinMassToSearch = CInt(txtMinProtMass.Text)
        Catch ex As Exception
            txtMinProtMass.Text = newParams.MinimumProteinMassToSearch.ToString()
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtMaxProtMass_Leave(sender As Object, e As EventArgs)
        Try
            newParams.MaximumProteinMassToSearch = CInt(txtMaxProtMass.Text)
        Catch ex As Exception
            txtMaxProtMass.Text = newParams.MaximumProteinMassToSearch.ToString()
        End Try
        UpdateDescription()
    End Sub

#End Region

#Region " [Advanced] Miscellaneous Options "

    Private Sub txtNumOutputLines_Leave(sender As Object, e As EventArgs)
        Try
            newParams.NumberOfOutputLines = CInt(txtNumOutputLines.Text)
        Catch ex As Exception
            txtNumOutputLines.Text = newParams.NumberOfOutputLines.ToString()
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtNumDescLines_Leave(sender As Object, e As EventArgs)
        Try
            newParams.NumberOfDescriptionLines = CInt(txtNumDescLines.Text)
        Catch ex As Exception
            txtNumDescLines.Text = newParams.NumberOfDescriptionLines.ToString()
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtNumResults_Leave(sender As Object, e As EventArgs)
        Try
            newParams.NumberOfResultsToProcess = CInt(txtNumResults.Text)
        Catch ex As Exception
            txtNumResults.Text = newParams.NumberOfResultsToProcess.ToString()
        End Try
        UpdateDescription()
    End Sub
    Private Sub txtMatchPeakCount_Leave(sender As Object, e As EventArgs)
        Try
            newParams.NumberOfDetectedPeaksToMatch = CInt(txtMatchPeakCountErrors.Text)
        Catch ex As Exception
            txtMatchPeakCountErrors.Text = newParams.NumberOfDetectedPeaksToMatch.ToString()
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtMatchPeakCountErrors_Leave(sender As Object, e As EventArgs)
        Try
            newParams.NumberOfAllowedDetectedPeakErrors = CInt(txtMatchPeakCountErrors.Text)
        Catch ex As Exception
            txtMatchPeakCountErrors.Text = newParams.NumberOfAllowedDetectedPeakErrors.ToString()
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtMaxAAPerDynMod_Leave(sender As Object, e As EventArgs)
        Try
            newParams.MaximumNumAAPerDynMod = CInt(txtMaxAAPerDynMod.Text)
        Catch ex As Exception
            txtMaxAAPerDynMod.Text = newParams.MaximumNumAAPerDynMod.ToString()
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtMaxDiffPerPeptide_Leave(sender As Object, e As EventArgs)
        Dim newValue As Integer
        If Integer.TryParse(txtMaxDiffPerPeptide.Text, newValue) Then
            newParams.MaximumNumDifferentialPerPeptide = newValue
            UpdateDescription()
        End If
    End Sub

    Private Sub cboNucReadingFrame_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            newParams.SelectedNucReadingFrame = CType(cboNucReadingFrame.SelectedIndex, IAdvancedParams.FrameList)
        Catch ex As Exception
            cboNucReadingFrame.SelectedIndex = newParams.SelectedNucReadingFrame
        End Try
        UpdateDescription()
    End Sub

#End Region

#Region " [Advanced] Option Checkboxes "

    Private Sub chkUseAIons_CheckedChanged(sender As Object, e As EventArgs)
        newParams.IonSeries.Use_a_Ions = CInt(IIf(chkUseAIons.Checked, 1, 0))
        UpdateDescription()
    End Sub

    Private Sub chkUseBIons_CheckedChanged(sender As Object, e As EventArgs)
        newParams.IonSeries.Use_b_Ions = CInt(IIf(chkUseBIons.Checked, 1, 0))
        UpdateDescription()
    End Sub

    Private Sub chkUseYIons_CheckedChanged(sender As Object, e As EventArgs)
        newParams.IonSeries.Use_y_Ions = CInt(IIf(chkUseYIons.Checked, 1, 0))
        UpdateDescription()
    End Sub

    Private Sub chkCreateOutputFiles_CheckedChanged(sender As Object, e As EventArgs)
        newParams.CreateOutputFiles = chkCreateOutputFiles.Checked
        UpdateDescription()
    End Sub

    Private Sub chkShowFragmentIons_CheckedChanged(sender As Object, e As EventArgs)
        newParams.ShowFragmentIons = chkShowFragmentIons.Checked
        UpdateDescription()
    End Sub

    Private Sub chkRemovePrecursorPeaks_CheckedChanged(sender As Object, e As EventArgs)
        newParams.RemovePrecursorPeak = chkRemovePrecursorPeaks.Checked
        UpdateDescription()
    End Sub

    Private Sub chkPrintDupRefs_CheckedChanged(sender As Object, e As EventArgs)
        newParams.PrintDuplicateReferences = chkPrintDupRefs.Checked
        UpdateDescription()
    End Sub

    Private Sub chkResiduesInUpperCase_CheckedChanged(sender As Object, e As EventArgs)
        newParams.AminoAcidsAllUpperCase = chkResiduesInUpperCase.Checked
        UpdateDescription()
    End Sub

#End Region

#Region " [Advanced] Ion Weighting Constants"

    Private Sub txtAWeight_Leave(sender As Object, e As EventArgs)
        Try
            newParams.IonSeries.a_Ion_Weighting = CSng(txtAWeight.Text)
        Catch ex As Exception
            txtAWeight.Text = newParams.IonSeries.a_Ion_Weighting.ToString("0.0")
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtBWeight_Leave(sender As Object, e As EventArgs)
        Try
            newParams.IonSeries.b_Ion_Weighting = CSng(txtBWeight.Text)
        Catch ex As Exception
            txtBWeight.Text = newParams.IonSeries.b_Ion_Weighting.ToString("0.0")
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtCWeight_Leave(sender As Object, e As EventArgs)
        Try
            newParams.IonSeries.c_Ion_Weighting = CSng(txtCWeight.Text)
        Catch ex As Exception
            txtCWeight.Text = newParams.IonSeries.c_Ion_Weighting.ToString("0.0")
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtDWeight_Leave(sender As Object, e As EventArgs)
        Try
            newParams.IonSeries.d_Ion_Weighting = CSng(txtDWeight.Text)
        Catch ex As Exception
            txtDWeight.Text = newParams.IonSeries.d_Ion_Weighting.ToString("0.0")
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtVWeight_Leave(sender As Object, e As EventArgs)
        Try
            newParams.IonSeries.v_Ion_Weighting = CSng(txtVWeight.Text)
        Catch ex As Exception
            txtVWeight.Text = newParams.IonSeries.v_Ion_Weighting.ToString("0.0")
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtWWeight_Leave(sender As Object, e As EventArgs)
        Try
            newParams.IonSeries.w_Ion_Weighting = CSng(txtWWeight.Text)
        Catch ex As Exception
            txtWWeight.Text = newParams.IonSeries.w_Ion_Weighting.ToString("0.0")
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtXWeight_Leave(sender As Object, e As EventArgs)
        Try
            newParams.IonSeries.x_Ion_Weighting = CSng(txtXWeight.Text)
        Catch ex As Exception
            txtXWeight.Text = newParams.IonSeries.x_Ion_Weighting.ToString("0.0")
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtYWeight_Leave(sender As Object, e As EventArgs)
        Try
            newParams.IonSeries.y_Ion_Weighting = CSng(txtYWeight.Text)
        Catch ex As Exception
            txtYWeight.Text = newParams.IonSeries.y_Ion_Weighting.ToString("0.0")
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtZWeight_Leave(sender As Object, e As EventArgs)
        Try
            newParams.IonSeries.z_Ion_Weighting = CSng(txtZWeight.Text)
        Catch ex As Exception
            txtZWeight.Text = newParams.IonSeries.z_Ion_Weighting.ToString("0.0")
        End Try
        UpdateDescription()
    End Sub

#End Region

#Region " Menu Handlers "

    Private Sub mnuFileSaveBW2_Click(sender As Object, e As EventArgs) Handles mnuFileSaveBW2.Click
        SaveSequestFile("Save Sequest/BioWorks v2.0 Parameter File", clsParams.ParamFileTypes.BioWorks_20)
    End Sub

    Private Sub mnuFileSaveBW3_Click(sender As Object, e As EventArgs) Handles mnuFileSaveBW3.Click
        SaveSequestFile("Save Sequest/BioWorks v3.0 Parameter File", clsParams.ParamFileTypes.BioWorks_30)
    End Sub

    Private Sub mnuFileSaveBW32_Click(sender As Object, e As EventArgs) Handles mnuFileSaveBW32.Click
        SaveSequestFile("Save Sequest/BioWorks v3.2 Parameter File", clsParams.ParamFileTypes.BioWorks_32)
    End Sub

    Private Sub mnuHelpAbout_Click(sender As Object, e As EventArgs) Handles mnuHelpAbout.Click
        Dim AboutBox As New frmAboutBox
        AboutBox.ConnectionStringInUse = mySettings.DMS_ConnectionString
        AboutBox.Show()
    End Sub

    Private Sub mnuFileLoadFromDMS_Click(sender As Object, e As EventArgs) Handles mnuFileLoadFromDMS.Click
        Try
            Dim frmPicker As New frmDMSPicker(Me)
            frmPicker.MySettings = mySettings

            frmPicker.txtLiveSearch.Focus()
            frmPicker.Show()
        Catch ex As Exception
            MessageBox.Show("Error in mnuFileLoadFromDMS_Click: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try

    End Sub

    Private Sub mnuFileUploadDMS_Click(sender As Object, e As EventArgs) Handles mnuFileUploadDMS.Click
        'Gonna need security stuff in here to keep morons outa DMS
        'Dim DMSParams As New clsParamsFromDMS(MainCode.mySettings)

        'Dim ident As System.Security.Principal.WindowsIdentity

        'ident = ident.GetCurrent

        'If m_clsParamsFromDMS Is Nothing Then
        '    m_clsParamsFromDMS = LoadDMSParamUploadClass(MainCode.mySettings)
        'End If
        Try
            Dim SaveFrm As New frmDMSParamNamer(Me, newParams)
            SaveFrm.Show()

        Catch ex As Exception
            MessageBox.Show("Error in mnuFileUploadDMS_Click: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
        'Dim success As Boolean = dmsparams.ParamSetNameExists(testName)

        'Call m_clsParamsFromDMS.WriteParamsToDMS(newParams, True)

    End Sub

    Private Sub mnuLoadFromFile_Click(sender As Object, e As EventArgs) Handles mnuLoadFromFile.Click
        Try
            Dim newFilePath As String
            Dim OpenDialog As New OpenFileDialog

            With OpenDialog
                .Title = "Open Sequest/BioWorks Parameter File"
                .DereferenceLinks = False
                .InitialDirectory = "\\gigasax\DMS_Parameter_Files\Sequest\"
                .Filter = "Sequest Param files (*.params)|*.params|All files (*.*)|*.*"
                .FilterIndex = 1
                .RestoreDirectory = True
                .Multiselect = False
            End With

            If OpenDialog.ShowDialog = DialogResult.OK Then
                newFilePath = OpenDialog.FileName
                LoadParamsFromFile(newFilePath)
            End If

        Catch ex As Exception
            MessageBox.Show("Error in mnuLoadFromFile_Click: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub mnuBatchUploadDMS_Click(sender As Object, e As EventArgs) Handles mnuBatchUploadDMS.Click
        Try
            Dim batch = New clsBatchLoadTemplates()
            Dim openDialog As New OpenFileDialog
            Dim fc As StringCollection
            Dim numAdded As Integer
            Dim numChanged As Integer
            Dim numSkipped As Integer

            With openDialog
                .Multiselect = True
                .InitialDirectory = "\\gigasax\DMS_Parameter_Files\Sequest\"
                .Filter = "Sequest Param files (*.params)|*.params|All files (*.*)|*.*"
                .FilterIndex = 1
                .RestoreDirectory = True
            End With

            If openDialog.ShowDialog = DialogResult.OK Then
                Dim fileNameList As List(Of String) = openDialog.FileNames.ToList()
                fc = ConvertStringArrayToSC(fileNameList)
                batch.UploadParamSetsToDMS(fc)
                numAdded = batch.NumParamSetsAdded
                numChanged = batch.NumParamSetsChanged
                numSkipped = batch.NumParamSetsSkipped
            End If

            MessageBox.Show(numAdded & " new Parameter sets added; " & numChanged & " Parameter sets changed; " & numSkipped & " Parameter sets skipped",
                "Operation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error in mnuBatchUploadDMS_Click: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try

    End Sub

    Private Function ConvertStringArrayToSC(stringArray As IList(Of String)) As StringCollection
        Dim sc As New StringCollection
        Dim count As Integer

        If Not stringArray Is Nothing Then
            For count = 0 To stringArray.Count - 1
                sc.Add(stringArray(count))
            Next
        End If

        Return sc
    End Function
#End Region

    Private Sub CheckForParamFileExistence()

        Dim TemplateName = "sequest_N14_NE.params"

        If Not embeddedResource.ResourceExists(TemplateName) Then
            embeddedResource.RestoreFromEmbeddedResource(TemplateName)
        End If

    End Sub

    Private Sub CheckForSettingsFileExistence()
        If Not embeddedResource.ResourceExists(m_SettingsFileName) Then
            embeddedResource.RestoreFromEmbeddedResource(m_SettingsFileName)
        End If
    End Sub

    Private Sub numericTextbox_Validating(sender As Object, e As CancelEventArgs)

        Dim thisControl = DirectCast(sender, Control)
        Try
            Dim chk As String = thisControl.Text
            Dim t = DirectCast(sender, NumericTextBox)
            Dim tmpNewMass As Double
            Dim forceNewValue As Boolean = t.ForceNewValue

            Dim blnIsNumber As Boolean

            blnIsNumber = Double.TryParse(chk, tmpNewMass)
            If Not blnIsNumber Then
                e.Cancel = True
                StatModErrorProvider.SetError(thisControl, "Not a valid number")
            Else
                If Math.Abs(tmpNewMass) < Single.Epsilon Then
                    thisControl.BackColor = SystemColors.Window
                    StatModErrorProvider.SetError(thisControl, "")
                    t.Tag = 0
                Else
                    StatModErrorProvider.SetError(thisControl, "")

                    If m_UseAutoTweak Then
                        Dim tmpModType As IMassTweaker.ModTypes = GetModTypeFromControlName(sender)
                        Dim tmpAtom As String
                        Dim tmpAA = ""
                        'Dim tmpSymbol As String = ""
                        'Dim tmpDesc As String = ""
                        'Dim tmpGMID As Integer = 0
                        Dim dr As DialogResult

                        If tmpModType = IMassTweaker.ModTypes.IsotopicMod Then
                            tmpAtom = GetAffectedIsoAtomFromControlName(sender)
                        Else
                            tmpAtom = "-"
                        End If

                        tmpNewMass = m_clsMassTweaker.GetTweakedMass(tmpNewMass, tmpAtom)

                        If Math.Abs(tmpNewMass) < Single.Epsilon Or forceNewValue = True Then
                            t.ForceNewValue = False
                            thisControl.BackColor = SystemColors.Window
                            Dim frmNewMass As New frmGlobalModNamer

                            With frmNewMass

                                .MassCorrectionsTable = m_clsMassTweaker.MassCorrectionsTable
                                .NewModMass = CDbl(chk)
                                .ModType = tmpModType
                                .AffectedResidues = tmpAA
                                .LoadGlobalMods(CDbl(chk), tmpAtom)

                                dr = frmNewMass.ShowDialog
                                Dim tmpSymbol = frmNewMass.NewSymbol
                                Dim tmpNewModMass = CDbl(.NewModMass)
                                Dim tmpDesc = .NewDescription

                                If dr = DialogResult.OK Then

                                    m_clsMassTweaker.AddMassCorrection(tmpSymbol, tmpDesc, tmpNewModMass)
                                    m_clsMassTweaker.RefreshGlobalModsTableCache(mySettings.DMS_ConnectionString)
                                    numericTextbox_Validating(sender, e)
                                ElseIf dr = DialogResult.Yes Then   'Use existing
                                    thisControl.Text = tmpNewModMass.ToString()
                                    numericTextbox_Validating(sender, e)
                                    Exit Sub
                                ElseIf dr = DialogResult.Cancel Then
                                    StatModErrorProvider.SetError(thisControl, "You must either choose an existing global mod, or define a new one!")
                                    e.Cancel = True
                                End If
                            End With
                        Else
                            Dim tmpSymbol = m_clsMassTweaker.TweakedSymbol
                            Dim tmpDesc = m_clsMassTweaker.TweakedDescription
                            Dim tmpGMID = m_clsMassTweaker.TweakedModID
                            tooltipProvider.SetToolTip(thisControl, tmpSymbol & ": " & tmpDesc)
                            thisControl.Tag = tmpGMID
                            thisControl.BackColor = Color.LightGoldenrodYellow
                            StatModErrorProvider.SetError(thisControl, "")

                        End If
                    End If
                End If
            End If
            thisControl.Text = PRISM.StringUtilities.DblToString(tmpNewMass, 5)

        Catch ex As Exception
            MessageBox.Show("Exception in numericTextbox_Validating: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try

    End Sub

    Private Sub AATextbox_Validating(sender As Object, e As CancelEventArgs)
        Dim thisControl = DirectCast(sender, Control)
        Dim chk As String = thisControl.Text
        If chk.Length > 0 AndAlso Not IsValidAAString(chk) Then
            e.Cancel = True
            StatModErrorProvider.SetError(thisControl, "Not a valid Amino Acid")
        Else
            StatModErrorProvider.SetError(thisControl, "")
        End If
    End Sub

    Private Sub numericTextBox_KeyDown(sender As Object, e As KeyEventArgs)
        Try
            Dim t = DirectCast(sender, NumericTextBox)
            If e.KeyCode = Keys.Escape Then
                t.Text = "0.0"
                numericTextbox_Validating(sender, Nothing)
            ElseIf e.KeyCode = Keys.Return OrElse e.KeyCode = Keys.Enter Then
                t.ForceNewValue = True
                numericTextbox_Validating(t, Nothing)
            End If
        Catch ex As Exception
            ' Exception: Perhaps the caller was not a numeric text box
            MessageBox.Show("Exception in numericTextBox_KeyDown: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Function IsValidAAString(AAString As String) As Boolean
        Dim counter As Integer
        Dim valid As Boolean

        If AAString Is Nothing OrElse AAString.Length = 0 Then
            valid = False
        Else
            For counter = 0 To AAString.Length - 1
                If IsAminoAcid(AAString.Chars(counter)) Then
                    valid = True
                Else
                    valid = False
                    Return valid
                End If
            Next
        End If

        Return valid
    End Function

    Private Function IsAminoAcid(AA As String) As Boolean
        'If InStr("<>[]", AA.ToUpper) Then
        '  Return True
        'Else
        Dim badChars As Char() = {"J"c, "O"c, "U"c}

        If Convert.ToInt32(AA.ToUpper.Chars(0)) < Convert.ToInt32("A"c) Or Convert.ToInt32(AA.ToUpper.Chars(0)) > Convert.ToInt32("Z"c) Then
            Return False
        ElseIf AA.ToUpper.IndexOfAny(badChars) >= 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    '' Function not used
    ''Private Function GetAffectedResidueFromControlName(sender As System.Object) As String
    ''    Dim tmpTB As System.Windows.Forms.TextBox = DirectCast(sender, System.Windows.Forms.TextBox)
    ''    Dim TBName As String = tmpTB.Name

    ''    Select Case TBName
    ''        Case "txtCTPep", "txtCTProt", "txtNTPep", "txtNTProt"
    ''            Return "-"
    ''        Case "txtAla"
    ''            Return "A"
    ''        Case "txtGly"
    ''            Return "G"
    ''        Case "txtSer"
    ''            Return "S"
    ''        Case "txtCys"
    ''            Return "C"
    ''        Case "txtPro"
    ''            Return "P"
    ''        Case "txtLorI"
    ''            Return "X"
    ''        Case "txtThr"
    ''            Return "T"
    ''        Case "txtIle"
    ''            Return "I"
    ''        Case "txtVal"
    ''            Return "V"
    ''        Case "txtLeu"
    ''            Return "L"
    ''        Case "txtNandD"
    ''            Return "B"
    ''        Case "txtQandE"
    ''            Return "Z"
    ''        Case "txtAsn"
    ''            Return "N"
    ''        Case "txtLys"
    ''            Return "K"
    ''        Case "txtOrn"
    ''            Return "O"
    ''        Case "txtGln"
    ''            Return "Q"
    ''        Case "txtAsp"
    ''            Return "D"
    ''        Case "txtArg"
    ''            Return "R"
    ''        Case "txtTrp"
    ''            Return "W"
    ''        Case "txtGlu"
    ''            Return "E"
    ''        Case "txtHis"
    ''            Return "H"
    ''        Case "txtPhe"
    ''            Return "F"
    ''        Case "txtTyr"
    ''            Return "Y"
    ''        Case "txtMet"
    ''            Return "M"
    ''        Case "txtDynMod1MassDiff"
    ''            Return txtDynMod1List.Text
    ''        Case "txtDynMod2MassDiff"
    ''            Return txtDynMod2List.Text
    ''        Case "txtDynMod3MassDiff"
    ''            Return txtDynMod3List.Text
    ''        Case Else
    ''            Return ""
    ''    End Select
    ''End Function

    Private Function GetAffectedIsoAtomFromControlName(sender As Object) As String
        Dim tmpTB = DirectCast(sender, TextBox)
        Dim TBName As String = tmpTB.Name

        Select Case TBName
            Case "txtIsoC"
                Return "C"
            Case "txtIsoH"
                Return "H"
            Case "txtIsoO"
                Return "O"
            Case "txtIsoN"
                Return "N"
            Case "txtIsoS"
                Return "S"
            Case Else
                Return ""
        End Select
    End Function

    Private Function GetModTypeFromControlName(sender As Object) As IMassTweaker.ModTypes
        Dim tmpTB = DirectCast(sender, TextBox)
        Dim TBName As String = tmpTB.Parent.Name.ToString

        Select Case TBName
            Case "gbxDynMods"
                Return IMassTweaker.ModTypes.DynamicMod
            Case "gbxIsoMods"
                Return IMassTweaker.ModTypes.IsotopicMod
            Case "gbxStaticMods"
                Return IMassTweaker.ModTypes.StaticMod
            Case Else
                MessageBox.Show("GetModTypeFromControlName encountered an unknown TextBox name: " & TBName)
                Return IMassTweaker.ModTypes.DynamicMod
        End Select
    End Function

    Private Sub SaveSequestFile(strTitle As String, eFileType As clsParams.ParamFileTypes)

        Try
            Dim FileOutput As New clsWriteOutput

            Dim SaveDialog As New SaveFileDialog

            With SaveDialog
                .Title = strTitle
                .FileName = newParams.FileName
            End With

            If SaveDialog.ShowDialog = DialogResult.OK Then
                Dim newFilePath = SaveDialog.FileName
                If newFilePath.Length > 0 Then
                    Dim iFileType = CType(eFileType, MakeParams.IGenerateFile.ParamFileType)
                    Call FileOutput.WriteOutputFile(newParams, newFilePath, iFileType)
                    MessageBox.Show("Param File: " & newFilePath & " written successfully", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error in SaveSequestFile: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try

    End Sub

    Private Sub cmdReTweak_Click(sender As Object, e As EventArgs) Handles cmdReTweak.Click
        If m_UseAutoTweak Then RetweakMasses()
    End Sub

    Private Sub RetweakMasses()
        Try
            Dim g_d As Control
            Dim t As Control

            For Each g_d In tabBasic.Controls
                If g_d.GetType.ToString = "System.Windows.Forms.GroupBox" Then
                    For Each t In g_d.Controls
                        If IsNumeric(t.Text) And t.GetType.ToString = "System.Windows.Forms.TextBox" Then
                            If Math.Abs(CSng(t.Text)) > Single.Epsilon Then
                                numericTextbox_Validating(t, Nothing)
                            Else
                                t.BackColor = SystemColors.Window
                            End If
                        End If
                    Next
                End If
            Next
        Catch ex As Exception
            MessageBox.Show("Error in RetweakMasses: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try


    End Sub

    Private Sub UpdateDescription()
        Try
            txtDescription.Text = m_DMSUpload.GetDiffsFromTemplate(newParams)
        Catch ex As Exception
            MessageBox.Show("Exception in UpdateDescription: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub mnuDebugSyncAll_Click(sender As Object, e As EventArgs) Handles mnuDebugSyncAll.Click
        Try
            Dim sync As New clsTransferParamEntriesToMassModList(mySettings)

            sync.SyncAll()

        Catch ex As Exception
            MessageBox.Show("Error in mnuDebugSyncAll_Click: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try

    End Sub

    Private Sub mnuDebugSyncSingle_Click(sender As Object, e As EventArgs) Handles mnuDebugSyncSingle.Click
        Dim sync As New clsTransferParamEntriesToMassModList(mySettings)
        Dim paramFileID = CInt(InputBox("Enter an Parameter File ID to be Sync'ed", "Param File to Sync"))
        sync.SyncOneJob(paramFileID)
    End Sub

    Private Sub mnuDebugSyncDesc_Click(sender As Object, e As EventArgs) Handles mnuDebugSyncDesc.Click
        Dim sync As New clsTransferParamEntriesToMassModList(mySettings)
        sync.SyncDescriptions(clsMainProcess.BaseLineParamSet)

    End Sub

    Private Sub mnuFileExit_Click(sender As Object, e As EventArgs) Handles mnuFileExit.Click
        Close()
    End Sub

    ''' <summary>
    ''' Parses the .StackTrace text of the given expression to return a compact description of the current stack
    ''' </summary>
    ''' <param name="objException"></param>
    ''' <returns>String similar to "Stack trace: clsCodeTest.Test-:-clsCodeTest.TestException-:-clsCodeTest.InnerTestException in clsCodeTest.vb:line 86"</returns>
    ''' <remarks></remarks>
    Public Shared Function GetExceptionStackTrace(objException As Exception, blnWriteStackTraceToFile As Boolean) As String
        Const REGEX_FUNCTION_NAME = "at ([^(]+)\("
        Const REGEX_FILE_NAME = "in .+\\(.+)"

        Dim trTextReader As StringReader
        Dim intIndex As Integer

        Dim strFunctions() As String

        Dim strCurrentFunction As String
        Dim strFinalFile As String = String.Empty

        Dim reFunctionName As New Regex(REGEX_FUNCTION_NAME, RegexOptions.Compiled Or RegexOptions.IgnoreCase)
        Dim reFileName As New Regex(REGEX_FILE_NAME, RegexOptions.Compiled Or RegexOptions.IgnoreCase)
        Dim objMatch As Match

        ' Process each line in objException.StackTrace
        ' Populate strFunctions() with the function name of each line
        trTextReader = New StringReader(objException.StackTrace)

        Dim intFunctionCount = 0
        ReDim strFunctions(9)

        Do While trTextReader.Peek >= 0
            Dim strLine = trTextReader.ReadLine

            If Not strLine Is Nothing AndAlso strLine.Length > 0 Then
                strCurrentFunction = String.Empty

                objMatch = reFunctionName.Match(strLine)
                If objMatch.Success AndAlso objMatch.Groups.Count > 1 Then
                    strCurrentFunction = objMatch.Groups(1).Value
                Else
                    ' Look for the word " in "
                    intIndex = strLine.ToLower.IndexOf(" in ", StringComparison.Ordinal)
                    If intIndex = 0 Then
                        ' " in" not found; look for the first space after startIndex 4
                        intIndex = strLine.IndexOf(" ", 4, StringComparison.Ordinal)
                    End If
                    If intIndex = 0 Then
                        ' Space not found; use the entire string
                        intIndex = strLine.Length - 1
                    End If

                    If intIndex > 0 Then
                        strCurrentFunction = strLine.Substring(0, intIndex)
                    End If

                End If

                If Not strCurrentFunction Is Nothing AndAlso strCurrentFunction.Length > 0 Then
                    If intFunctionCount >= strFunctions.Length Then
                        ' Reserve more space in strFunctions()
                        ReDim Preserve strFunctions(strFunctions.Length * 2 - 1)
                    End If

                    strFunctions(intFunctionCount) = strCurrentFunction
                    intFunctionCount += 1
                End If

                If strFinalFile.Length = 0 Then
                    ' Also extract the file name where the Exception occurred
                    objMatch = reFileName.Match(strLine)
                    If objMatch.Success AndAlso objMatch.Groups.Count > 1 Then
                        strFinalFile = objMatch.Groups(1).Value
                    End If
                End If

            End If
        Loop

        Dim strStackTrace = String.Empty
        For intIndex = intFunctionCount - 1 To 0 Step -1
            If Not strFunctions(intIndex) Is Nothing Then
                If strStackTrace.Length = 0 Then
                    strStackTrace = "Stack trace: " & strFunctions(intIndex)
                Else
                    strStackTrace &= "-:-" & strFunctions(intIndex)
                End If
            End If
        Next intIndex

        If Not strStackTrace Is Nothing AndAlso strFinalFile.Length > 0 Then
            strStackTrace &= " in " & strFinalFile
        End If

        If blnWriteStackTraceToFile Then
            Try
                Dim srOutFile As New StreamWriter(New FileStream("ExceptionStackTraceFile.txt", FileMode.Append, FileAccess.Write, FileShare.Read))
                srOutFile.WriteLine()
                srOutFile.WriteLine("Exception occurred at " & DateTime.Now.ToString)
                srOutFile.WriteLine(" message: " & objException.Message)
                srOutFile.WriteLine(" trace: " & strStackTrace)
                srOutFile.Close()

            Catch ex As Exception
                ' Ignore errors here
            End Try
        End If

        Return strStackTrace

    End Function

End Class

Public Class NumericTextBox
    Inherits TextBox

    Public Property ForceNewValue As Boolean
End Class

Class ComboBoxContents
    Sub New(strName As String, strValue As String)
        Me.Value = strValue
        DisplayName = strName
    End Sub

    Public ReadOnly Property DisplayName As String

    Public ReadOnly Property Value As String
End Class
