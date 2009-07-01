Imports ParamFileEditor.ProgramSettings
Imports ParamFileGenerator
Imports ParamFileGenerator.DownloadParams
Imports System.IO

Public Class frmMainGUI
  Inherits System.Windows.Forms.Form

  Private m_clsParamsFromDMS As clsParamsFromDMS
  Private m_clsMassTweaker As IMassTweaker

  Private m_DMSUpload As clsDMSParamUpload
  Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
  Friend WithEvents mnuOptionsAutoTweakParams As System.Windows.Forms.MenuItem
  Friend WithEvents cboParentMassUnits As System.Windows.Forms.ComboBox
  Friend WithEvents cboFragmentMassUnits As System.Windows.Forms.ComboBox
  Friend WithEvents lblFragmentMassUnits As System.Windows.Forms.Label
  Friend WithEvents lblParentMassUnits As System.Windows.Forms.Label
  Private m_sharedMain As clsMainProcess
#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    'This call is required by the Windows Form Designer.
    InitializeComponent()

    'Add any initialization after the InitializeComponent() call
    Me.CheckForSettingsFileExistence()
    Me.CheckForParamFileExistence()
    Me.m_sharedMain = New clsMainProcess

  End Sub

  'Form overrides dispose to clean up the component list.
  Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
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
  Friend WithEvents mnuFileNewFileFromTemp As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileSaveToFile As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileExit As System.Windows.Forms.MenuItem
  Friend WithEvents mnuHelpAbout As System.Windows.Forms.MenuItem
  Friend WithEvents mnuLoadFromFile As System.Windows.Forms.MenuItem
  Friend WithEvents mnuFileLoadFromDMS As System.Windows.Forms.MenuItem
  Friend WithEvents lblNucReadingFrame As System.Windows.Forms.Label
  Friend WithEvents lblParamFileInfo As System.Windows.Forms.Label
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

  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMainGUI))
    Me.tcMain = New System.Windows.Forms.TabControl
    Me.tabBasic = New System.Windows.Forms.TabPage
    Me.gbxIsoMods = New System.Windows.Forms.GroupBox
    Me.lblIsoS = New System.Windows.Forms.Label
    Me.lblIsoN = New System.Windows.Forms.Label
    Me.lblIsoH = New System.Windows.Forms.Label
    Me.lblIsoC = New System.Windows.Forms.Label
    Me.lblIsoO = New System.Windows.Forms.Label
    Me.cmdReTweak = New System.Windows.Forms.Button
    Me.chkAutoTweak = New System.Windows.Forms.CheckBox
    Me.gbxStaticMods = New System.Windows.Forms.GroupBox
    Me.lblCTPep = New System.Windows.Forms.Label
    Me.lblCTProt = New System.Windows.Forms.Label
    Me.lblNTPep = New System.Windows.Forms.Label
    Me.lblNTProt = New System.Windows.Forms.Label
    Me.lblGly = New System.Windows.Forms.Label
    Me.lblAla = New System.Windows.Forms.Label
    Me.lblSer = New System.Windows.Forms.Label
    Me.lblCys = New System.Windows.Forms.Label
    Me.lblLorI = New System.Windows.Forms.Label
    Me.lblThr = New System.Windows.Forms.Label
    Me.lblVal = New System.Windows.Forms.Label
    Me.lblLeu = New System.Windows.Forms.Label
    Me.lblIle = New System.Windows.Forms.Label
    Me.lblPro = New System.Windows.Forms.Label
    Me.lblAsn = New System.Windows.Forms.Label
    Me.lblGln = New System.Windows.Forms.Label
    Me.lblQandE = New System.Windows.Forms.Label
    Me.lblNandD = New System.Windows.Forms.Label
    Me.lblOrn = New System.Windows.Forms.Label
    Me.lblAsp = New System.Windows.Forms.Label
    Me.lblLys = New System.Windows.Forms.Label
    Me.lblArg = New System.Windows.Forms.Label
    Me.lblTrp = New System.Windows.Forms.Label
    Me.lblHis = New System.Windows.Forms.Label
    Me.lblMet = New System.Windows.Forms.Label
    Me.lblPhe = New System.Windows.Forms.Label
    Me.lblTyr = New System.Windows.Forms.Label
    Me.lblGlu = New System.Windows.Forms.Label
    Me.gbxDesc = New System.Windows.Forms.GroupBox
    Me.txtDescription = New System.Windows.Forms.TextBox
    Me.lblDescription = New System.Windows.Forms.Label
    Me.gbxSearch = New System.Windows.Forms.GroupBox
    Me.txtPartialSeq = New System.Windows.Forms.TextBox
    Me.lblPartialSeq = New System.Windows.Forms.Label
    Me.cboFragmentMassType = New System.Windows.Forms.ComboBox
    Me.cboMissedCleavages = New System.Windows.Forms.ComboBox
    Me.cboParentMassType = New System.Windows.Forms.ComboBox
    Me.lblParentMassType = New System.Windows.Forms.Label
    Me.cboEnzymeSelect = New System.Windows.Forms.ComboBox
    Me.lblEnzymeSelect = New System.Windows.Forms.Label
    Me.lblMissedCleavages = New System.Windows.Forms.Label
    Me.lblFragmentMassType = New System.Windows.Forms.Label
    Me.cboCleavagePosition = New System.Windows.Forms.ComboBox
    Me.lblCleavagePosition = New System.Windows.Forms.Label
    Me.gbxDynMods = New System.Windows.Forms.GroupBox
    Me.txtDynMod1List = New System.Windows.Forms.TextBox
    Me.txtDynMod2List = New System.Windows.Forms.TextBox
    Me.txtDynMod3List = New System.Windows.Forms.TextBox
    Me.lblDynMod1List = New System.Windows.Forms.Label
    Me.lblDynMod2List = New System.Windows.Forms.Label
    Me.lblDynMod3List = New System.Windows.Forms.Label
    Me.lblDynMod1MassDiff = New System.Windows.Forms.Label
    Me.lblDynMod3MassDiff = New System.Windows.Forms.Label
    Me.lblDynMod2MassDiff = New System.Windows.Forms.Label
    Me.txtDynMod4List = New System.Windows.Forms.TextBox
    Me.lblDynMod4List = New System.Windows.Forms.Label
    Me.lblDynMod4MassDiff = New System.Windows.Forms.Label
    Me.lblDynMod5MassDiff = New System.Windows.Forms.Label
    Me.txtDynMod5List = New System.Windows.Forms.TextBox
    Me.lblDynMod5List = New System.Windows.Forms.Label
    Me.tabAdvanced = New System.Windows.Forms.TabPage
    Me.gbxIonWeighting = New System.Windows.Forms.GroupBox
    Me.txtWWeight = New System.Windows.Forms.TextBox
    Me.lblWWeight = New System.Windows.Forms.Label
    Me.txtXWeight = New System.Windows.Forms.TextBox
    Me.lblXWeight = New System.Windows.Forms.Label
    Me.txtDWeight = New System.Windows.Forms.TextBox
    Me.lblDWeight = New System.Windows.Forms.Label
    Me.txtCWeight = New System.Windows.Forms.TextBox
    Me.lblCWeight = New System.Windows.Forms.Label
    Me.txtBWeight = New System.Windows.Forms.TextBox
    Me.lblBWeight = New System.Windows.Forms.Label
    Me.lblVWeight = New System.Windows.Forms.Label
    Me.txtVWeight = New System.Windows.Forms.TextBox
    Me.txtYWeight = New System.Windows.Forms.TextBox
    Me.lblYWeight = New System.Windows.Forms.Label
    Me.txtZWeight = New System.Windows.Forms.TextBox
    Me.lblZWeight = New System.Windows.Forms.Label
    Me.txtAWeight = New System.Windows.Forms.TextBox
    Me.lblAWeight = New System.Windows.Forms.Label
    Me.chkUseAIons = New System.Windows.Forms.CheckBox
    Me.chkUseBIons = New System.Windows.Forms.CheckBox
    Me.chkUseYIons = New System.Windows.Forms.CheckBox
    Me.gbxMiscParams = New System.Windows.Forms.GroupBox
    Me.lblNumResults = New System.Windows.Forms.Label
    Me.txtNumResults = New System.Windows.Forms.TextBox
    Me.cboNucReadingFrame = New System.Windows.Forms.ComboBox
    Me.txtNumDescLines = New System.Windows.Forms.TextBox
    Me.lblOutputLines = New System.Windows.Forms.Label
    Me.txtNumOutputLines = New System.Windows.Forms.TextBox
    Me.lblNumDescLines = New System.Windows.Forms.Label
    Me.txtMatchPeakCountErrors = New System.Windows.Forms.TextBox
    Me.lblMatchPeakCountErrors = New System.Windows.Forms.Label
    Me.lblMatchPeakCount = New System.Windows.Forms.Label
    Me.txtMatchPeakCount = New System.Windows.Forms.TextBox
    Me.txtMaxDiffPerPeptide = New System.Windows.Forms.TextBox
    Me.lblMaxAAPerDynMod = New System.Windows.Forms.Label
    Me.txtMaxAAPerDynMod = New System.Windows.Forms.TextBox
    Me.lblNucReadingFrame = New System.Windows.Forms.Label
    Me.lblSeqHdrFilter = New System.Windows.Forms.Label
    Me.gbxToleranceValues = New System.Windows.Forms.GroupBox
    Me.txtFragMassTol = New System.Windows.Forms.TextBox
    Me.lblPepMassTol = New System.Windows.Forms.Label
    Me.txtPepMassTol = New System.Windows.Forms.TextBox
    Me.lblFragMassTol = New System.Windows.Forms.Label
    Me.txtIonCutoff = New System.Windows.Forms.TextBox
    Me.lblIonCutoff = New System.Windows.Forms.Label
    Me.lblPeakMatchingTol = New System.Windows.Forms.Label
    Me.txtPeakMatchingTol = New System.Windows.Forms.TextBox
    Me.lblMaxProtMass = New System.Windows.Forms.Label
    Me.txtMaxProtMass = New System.Windows.Forms.TextBox
    Me.lblMinProtMass = New System.Windows.Forms.Label
    Me.txtMinProtMass = New System.Windows.Forms.TextBox
    Me.gbxSwitches = New System.Windows.Forms.GroupBox
    Me.chkResiduesInUpperCase = New System.Windows.Forms.CheckBox
    Me.chkPrintDupRefs = New System.Windows.Forms.CheckBox
    Me.chkRemovePrecursorPeaks = New System.Windows.Forms.CheckBox
    Me.chkShowFragmentIons = New System.Windows.Forms.CheckBox
    Me.chkCreateOutputFiles = New System.Windows.Forms.CheckBox
    Me.mnuMain = New System.Windows.Forms.MainMenu(Me.components)
    Me.mnuFile = New System.Windows.Forms.MenuItem
    Me.mnuFileNewFileFromTemp = New System.Windows.Forms.MenuItem
    Me.mnuLoadFromFile = New System.Windows.Forms.MenuItem
    Me.mnuFileLoadFromDMS = New System.Windows.Forms.MenuItem
    Me.mnuFileSaveToFile = New System.Windows.Forms.MenuItem
    Me.mnuFileSaveBW2 = New System.Windows.Forms.MenuItem
    Me.mnuFileSaveBW3 = New System.Windows.Forms.MenuItem
    Me.mnuFileSaveBW32 = New System.Windows.Forms.MenuItem
    Me.mnuFileUploadDMS = New System.Windows.Forms.MenuItem
    Me.mnuBatchUploadDMS = New System.Windows.Forms.MenuItem
    Me.mnuDiv1 = New System.Windows.Forms.MenuItem
    Me.mnuFileExit = New System.Windows.Forms.MenuItem
    Me.MenuItem1 = New System.Windows.Forms.MenuItem
    Me.mnuOptionsAutoTweakParams = New System.Windows.Forms.MenuItem
    Me.mnuHelp = New System.Windows.Forms.MenuItem
    Me.mnuHelpAbout = New System.Windows.Forms.MenuItem
    Me.mnuDebug = New System.Windows.Forms.MenuItem
    Me.mnuDebugSyncAll = New System.Windows.Forms.MenuItem
    Me.mnuDebugSyncSingle = New System.Windows.Forms.MenuItem
    Me.mnuDebugSyncDesc = New System.Windows.Forms.MenuItem
    Me.lblParamFileInfo = New System.Windows.Forms.Label
    Me.StatModErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
    Me.tooltipProvider = New System.Windows.Forms.ToolTip(Me.components)
    Me.txtIsoS = New ParamFileEditor.NumericTextBox
    Me.txtIsoH = New ParamFileEditor.NumericTextBox
    Me.txtIsoN = New ParamFileEditor.NumericTextBox
    Me.txtIsoO = New ParamFileEditor.NumericTextBox
    Me.txtIsoC = New ParamFileEditor.NumericTextBox
    Me.txtCTPep = New ParamFileEditor.NumericTextBox
    Me.txtAla = New ParamFileEditor.NumericTextBox
    Me.txtCTProt = New ParamFileEditor.NumericTextBox
    Me.txtNTPep = New ParamFileEditor.NumericTextBox
    Me.txtNTProt = New ParamFileEditor.NumericTextBox
    Me.txtGly = New ParamFileEditor.NumericTextBox
    Me.txtSer = New ParamFileEditor.NumericTextBox
    Me.txtCys = New ParamFileEditor.NumericTextBox
    Me.txtPro = New ParamFileEditor.NumericTextBox
    Me.TxtLorI = New ParamFileEditor.NumericTextBox
    Me.txtThr = New ParamFileEditor.NumericTextBox
    Me.txtIle = New ParamFileEditor.NumericTextBox
    Me.txtVal = New ParamFileEditor.NumericTextBox
    Me.txtLeu = New ParamFileEditor.NumericTextBox
    Me.txtNandD = New ParamFileEditor.NumericTextBox
    Me.txtQandE = New ParamFileEditor.NumericTextBox
    Me.txtAsn = New ParamFileEditor.NumericTextBox
    Me.txtLys = New ParamFileEditor.NumericTextBox
    Me.txtOrn = New ParamFileEditor.NumericTextBox
    Me.txtGln = New ParamFileEditor.NumericTextBox
    Me.txtAsp = New ParamFileEditor.NumericTextBox
    Me.txtArg = New ParamFileEditor.NumericTextBox
    Me.txtTrp = New ParamFileEditor.NumericTextBox
    Me.txtGlu = New ParamFileEditor.NumericTextBox
    Me.txtHis = New ParamFileEditor.NumericTextBox
    Me.txtPhe = New ParamFileEditor.NumericTextBox
    Me.txtTyr = New ParamFileEditor.NumericTextBox
    Me.txtMet = New ParamFileEditor.NumericTextBox
    Me.txtDynMod1MassDiff = New ParamFileEditor.NumericTextBox
    Me.txtDynMod2MassDiff = New ParamFileEditor.NumericTextBox
    Me.txtDynMod3MassDiff = New ParamFileEditor.NumericTextBox
    Me.txtDynMod4MassDiff = New ParamFileEditor.NumericTextBox
    Me.txtDynMod5MassDiff = New ParamFileEditor.NumericTextBox
    Me.cboFragmentMassUnits = New System.Windows.Forms.ComboBox
    Me.cboParentMassUnits = New System.Windows.Forms.ComboBox
    Me.lblParentMassUnits = New System.Windows.Forms.Label
    Me.lblFragmentMassUnits = New System.Windows.Forms.Label
    Me.tcMain.SuspendLayout()
    Me.tabBasic.SuspendLayout()
    Me.gbxIsoMods.SuspendLayout()
    Me.gbxStaticMods.SuspendLayout()
    Me.gbxDesc.SuspendLayout()
    Me.gbxSearch.SuspendLayout()
    Me.gbxDynMods.SuspendLayout()
    Me.tabAdvanced.SuspendLayout()
    Me.gbxIonWeighting.SuspendLayout()
    Me.gbxMiscParams.SuspendLayout()
    Me.gbxToleranceValues.SuspendLayout()
    Me.gbxSwitches.SuspendLayout()
    CType(Me.StatModErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'tcMain
    '
    Me.tcMain.Controls.Add(Me.tabBasic)
    Me.tcMain.Controls.Add(Me.tabAdvanced)
    Me.tcMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.tcMain.ItemSize = New System.Drawing.Size(100, 20)
    Me.tcMain.Location = New System.Drawing.Point(0, 0)
    Me.tcMain.Name = "tcMain"
    Me.tcMain.SelectedIndex = 0
    Me.tcMain.Size = New System.Drawing.Size(476, 644)
    Me.tcMain.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
    Me.tcMain.TabIndex = 0
    '
    'tabBasic
    '
    Me.tabBasic.Controls.Add(Me.gbxIsoMods)
    Me.tabBasic.Controls.Add(Me.cmdReTweak)
    Me.tabBasic.Controls.Add(Me.chkAutoTweak)
    Me.tabBasic.Controls.Add(Me.gbxStaticMods)
    Me.tabBasic.Controls.Add(Me.gbxDesc)
    Me.tabBasic.Controls.Add(Me.gbxSearch)
    Me.tabBasic.Controls.Add(Me.gbxDynMods)
    Me.tabBasic.Location = New System.Drawing.Point(4, 24)
    Me.tabBasic.Name = "tabBasic"
    Me.tabBasic.Size = New System.Drawing.Size(468, 616)
    Me.tabBasic.TabIndex = 3
    Me.tabBasic.Text = "Basic Parameters"
    '
    'gbxIsoMods
    '
    Me.gbxIsoMods.Controls.Add(Me.txtIsoS)
    Me.gbxIsoMods.Controls.Add(Me.txtIsoH)
    Me.gbxIsoMods.Controls.Add(Me.txtIsoN)
    Me.gbxIsoMods.Controls.Add(Me.txtIsoO)
    Me.gbxIsoMods.Controls.Add(Me.txtIsoC)
    Me.gbxIsoMods.Controls.Add(Me.lblIsoS)
    Me.gbxIsoMods.Controls.Add(Me.lblIsoN)
    Me.gbxIsoMods.Controls.Add(Me.lblIsoH)
    Me.gbxIsoMods.Controls.Add(Me.lblIsoC)
    Me.gbxIsoMods.Controls.Add(Me.lblIsoO)
    Me.gbxIsoMods.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.gbxIsoMods.Location = New System.Drawing.Point(8, 524)
    Me.gbxIsoMods.Name = "gbxIsoMods"
    Me.gbxIsoMods.Size = New System.Drawing.Size(452, 62)
    Me.gbxIsoMods.TabIndex = 11
    Me.gbxIsoMods.TabStop = False
    Me.gbxIsoMods.Text = "Isotopic Modifications to Apply"
    '
    'lblIsoS
    '
    Me.lblIsoS.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblIsoS.Location = New System.Drawing.Point(356, 18)
    Me.lblIsoS.Name = "lblIsoS"
    Me.lblIsoS.Size = New System.Drawing.Size(84, 16)
    Me.lblIsoS.TabIndex = 12
    Me.lblIsoS.Text = "S - Sulfur"
    Me.lblIsoS.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblIsoN
    '
    Me.lblIsoN.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblIsoN.Location = New System.Drawing.Point(272, 18)
    Me.lblIsoN.Name = "lblIsoN"
    Me.lblIsoN.Size = New System.Drawing.Size(68, 16)
    Me.lblIsoN.TabIndex = 11
    Me.lblIsoN.Text = "N - Nitrogen"
    Me.lblIsoN.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblIsoH
    '
    Me.lblIsoH.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblIsoH.Location = New System.Drawing.Point(96, 18)
    Me.lblIsoH.Name = "lblIsoH"
    Me.lblIsoH.Size = New System.Drawing.Size(72, 16)
    Me.lblIsoH.TabIndex = 10
    Me.lblIsoH.Text = "H - Hydrogen"
    Me.lblIsoH.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblIsoC
    '
    Me.lblIsoC.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblIsoC.Location = New System.Drawing.Point(16, 18)
    Me.lblIsoC.Name = "lblIsoC"
    Me.lblIsoC.Size = New System.Drawing.Size(64, 16)
    Me.lblIsoC.TabIndex = 3
    Me.lblIsoC.Text = "C- Carbon"
    Me.lblIsoC.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblIsoO
    '
    Me.lblIsoO.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblIsoO.Location = New System.Drawing.Point(188, 18)
    Me.lblIsoO.Name = "lblIsoO"
    Me.lblIsoO.Size = New System.Drawing.Size(64, 16)
    Me.lblIsoO.TabIndex = 10
    Me.lblIsoO.Text = "O - Oxygen"
    Me.lblIsoO.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'cmdReTweak
    '
    Me.cmdReTweak.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.cmdReTweak.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cmdReTweak.Location = New System.Drawing.Point(352, 592)
    Me.cmdReTweak.Name = "cmdReTweak"
    Me.cmdReTweak.Size = New System.Drawing.Size(100, 21)
    Me.cmdReTweak.TabIndex = 10
    Me.cmdReTweak.Text = "Retweak Masses"
    '
    'chkAutoTweak
    '
    Me.chkAutoTweak.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkAutoTweak.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkAutoTweak.Location = New System.Drawing.Point(16, 588)
    Me.chkAutoTweak.Name = "chkAutoTweak"
    Me.chkAutoTweak.Size = New System.Drawing.Size(144, 24)
    Me.chkAutoTweak.TabIndex = 9
    Me.chkAutoTweak.Text = "Auto Tweak Masses?"
    '
    'gbxStaticMods
    '
    Me.gbxStaticMods.Controls.Add(Me.lblCTPep)
    Me.gbxStaticMods.Controls.Add(Me.txtCTPep)
    Me.gbxStaticMods.Controls.Add(Me.txtAla)
    Me.gbxStaticMods.Controls.Add(Me.txtCTProt)
    Me.gbxStaticMods.Controls.Add(Me.txtNTPep)
    Me.gbxStaticMods.Controls.Add(Me.txtNTProt)
    Me.gbxStaticMods.Controls.Add(Me.txtGly)
    Me.gbxStaticMods.Controls.Add(Me.txtSer)
    Me.gbxStaticMods.Controls.Add(Me.txtCys)
    Me.gbxStaticMods.Controls.Add(Me.txtPro)
    Me.gbxStaticMods.Controls.Add(Me.TxtLorI)
    Me.gbxStaticMods.Controls.Add(Me.txtThr)
    Me.gbxStaticMods.Controls.Add(Me.txtIle)
    Me.gbxStaticMods.Controls.Add(Me.txtVal)
    Me.gbxStaticMods.Controls.Add(Me.txtLeu)
    Me.gbxStaticMods.Controls.Add(Me.txtNandD)
    Me.gbxStaticMods.Controls.Add(Me.txtQandE)
    Me.gbxStaticMods.Controls.Add(Me.txtAsn)
    Me.gbxStaticMods.Controls.Add(Me.txtLys)
    Me.gbxStaticMods.Controls.Add(Me.txtOrn)
    Me.gbxStaticMods.Controls.Add(Me.txtGln)
    Me.gbxStaticMods.Controls.Add(Me.txtAsp)
    Me.gbxStaticMods.Controls.Add(Me.txtArg)
    Me.gbxStaticMods.Controls.Add(Me.txtTrp)
    Me.gbxStaticMods.Controls.Add(Me.txtGlu)
    Me.gbxStaticMods.Controls.Add(Me.txtHis)
    Me.gbxStaticMods.Controls.Add(Me.txtPhe)
    Me.gbxStaticMods.Controls.Add(Me.txtTyr)
    Me.gbxStaticMods.Controls.Add(Me.txtMet)
    Me.gbxStaticMods.Controls.Add(Me.lblCTProt)
    Me.gbxStaticMods.Controls.Add(Me.lblNTPep)
    Me.gbxStaticMods.Controls.Add(Me.lblNTProt)
    Me.gbxStaticMods.Controls.Add(Me.lblGly)
    Me.gbxStaticMods.Controls.Add(Me.lblAla)
    Me.gbxStaticMods.Controls.Add(Me.lblSer)
    Me.gbxStaticMods.Controls.Add(Me.lblCys)
    Me.gbxStaticMods.Controls.Add(Me.lblLorI)
    Me.gbxStaticMods.Controls.Add(Me.lblThr)
    Me.gbxStaticMods.Controls.Add(Me.lblVal)
    Me.gbxStaticMods.Controls.Add(Me.lblLeu)
    Me.gbxStaticMods.Controls.Add(Me.lblIle)
    Me.gbxStaticMods.Controls.Add(Me.lblPro)
    Me.gbxStaticMods.Controls.Add(Me.lblAsn)
    Me.gbxStaticMods.Controls.Add(Me.lblGln)
    Me.gbxStaticMods.Controls.Add(Me.lblQandE)
    Me.gbxStaticMods.Controls.Add(Me.lblNandD)
    Me.gbxStaticMods.Controls.Add(Me.lblOrn)
    Me.gbxStaticMods.Controls.Add(Me.lblAsp)
    Me.gbxStaticMods.Controls.Add(Me.lblLys)
    Me.gbxStaticMods.Controls.Add(Me.lblArg)
    Me.gbxStaticMods.Controls.Add(Me.lblTrp)
    Me.gbxStaticMods.Controls.Add(Me.lblHis)
    Me.gbxStaticMods.Controls.Add(Me.lblMet)
    Me.gbxStaticMods.Controls.Add(Me.lblPhe)
    Me.gbxStaticMods.Controls.Add(Me.lblTyr)
    Me.gbxStaticMods.Controls.Add(Me.lblGlu)
    Me.gbxStaticMods.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.gbxStaticMods.Location = New System.Drawing.Point(8, 356)
    Me.gbxStaticMods.Name = "gbxStaticMods"
    Me.gbxStaticMods.Size = New System.Drawing.Size(452, 164)
    Me.gbxStaticMods.TabIndex = 1
    Me.gbxStaticMods.TabStop = False
    Me.gbxStaticMods.Text = "Static Modifications to Apply"
    '
    'lblCTPep
    '
    Me.lblCTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblCTPep.Location = New System.Drawing.Point(12, 20)
    Me.lblCTPep.Name = "lblCTPep"
    Me.lblCTPep.Size = New System.Drawing.Size(56, 12)
    Me.lblCTPep.TabIndex = 1
    Me.lblCTPep.Text = "C-Term Pep"
    Me.lblCTPep.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblCTProt
    '
    Me.lblCTProt.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblCTProt.Location = New System.Drawing.Point(74, 20)
    Me.lblCTProt.Name = "lblCTProt"
    Me.lblCTProt.Size = New System.Drawing.Size(56, 12)
    Me.lblCTProt.TabIndex = 1
    Me.lblCTProt.Text = "C-Term Prot"
    Me.lblCTProt.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblNTPep
    '
    Me.lblNTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblNTPep.Location = New System.Drawing.Point(136, 20)
    Me.lblNTPep.Name = "lblNTPep"
    Me.lblNTPep.Size = New System.Drawing.Size(56, 12)
    Me.lblNTPep.TabIndex = 1
    Me.lblNTPep.Text = "N-Term Pep"
    Me.lblNTPep.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblNTProt
    '
    Me.lblNTProt.BackColor = System.Drawing.Color.Transparent
    Me.lblNTProt.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblNTProt.Location = New System.Drawing.Point(198, 20)
    Me.lblNTProt.Name = "lblNTProt"
    Me.lblNTProt.Size = New System.Drawing.Size(56, 12)
    Me.lblNTProt.TabIndex = 1
    Me.lblNTProt.Text = "N-Term Prot"
    Me.lblNTProt.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblGly
    '
    Me.lblGly.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblGly.Location = New System.Drawing.Point(260, 20)
    Me.lblGly.Name = "lblGly"
    Me.lblGly.Size = New System.Drawing.Size(56, 12)
    Me.lblGly.TabIndex = 1
    Me.lblGly.Text = "Gly (G)"
    Me.lblGly.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblAla
    '
    Me.lblAla.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblAla.Location = New System.Drawing.Point(322, 20)
    Me.lblAla.Name = "lblAla"
    Me.lblAla.Size = New System.Drawing.Size(56, 12)
    Me.lblAla.TabIndex = 1
    Me.lblAla.Text = "Ala (A)"
    Me.lblAla.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblSer
    '
    Me.lblSer.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblSer.Location = New System.Drawing.Point(384, 20)
    Me.lblSer.Name = "lblSer"
    Me.lblSer.Size = New System.Drawing.Size(56, 12)
    Me.lblSer.TabIndex = 1
    Me.lblSer.Text = "Ser (S)"
    Me.lblSer.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblCys
    '
    Me.lblCys.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblCys.Location = New System.Drawing.Point(196, 54)
    Me.lblCys.Name = "lblCys"
    Me.lblCys.Size = New System.Drawing.Size(56, 12)
    Me.lblCys.TabIndex = 1
    Me.lblCys.Text = "Cys (C)"
    Me.lblCys.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblLorI
    '
    Me.lblLorI.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblLorI.Location = New System.Drawing.Point(384, 54)
    Me.lblLorI.Name = "lblLorI"
    Me.lblLorI.Size = New System.Drawing.Size(56, 12)
    Me.lblLorI.TabIndex = 1
    Me.lblLorI.Text = "L or I (X)"
    Me.lblLorI.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblThr
    '
    Me.lblThr.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblThr.Location = New System.Drawing.Point(136, 54)
    Me.lblThr.Name = "lblThr"
    Me.lblThr.Size = New System.Drawing.Size(56, 12)
    Me.lblThr.TabIndex = 1
    Me.lblThr.Text = "Thr (T)"
    Me.lblThr.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblVal
    '
    Me.lblVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblVal.Location = New System.Drawing.Point(72, 54)
    Me.lblVal.Name = "lblVal"
    Me.lblVal.Size = New System.Drawing.Size(56, 12)
    Me.lblVal.TabIndex = 1
    Me.lblVal.Text = "Val (V)"
    Me.lblVal.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblLeu
    '
    Me.lblLeu.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblLeu.Location = New System.Drawing.Point(260, 54)
    Me.lblLeu.Name = "lblLeu"
    Me.lblLeu.Size = New System.Drawing.Size(56, 12)
    Me.lblLeu.TabIndex = 1
    Me.lblLeu.Text = "Leu (L)"
    Me.lblLeu.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblIle
    '
    Me.lblIle.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblIle.Location = New System.Drawing.Point(320, 54)
    Me.lblIle.Name = "lblIle"
    Me.lblIle.Size = New System.Drawing.Size(56, 12)
    Me.lblIle.TabIndex = 1
    Me.lblIle.Text = "Ile (I)"
    Me.lblIle.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblPro
    '
    Me.lblPro.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblPro.Location = New System.Drawing.Point(12, 54)
    Me.lblPro.Name = "lblPro"
    Me.lblPro.Size = New System.Drawing.Size(56, 12)
    Me.lblPro.TabIndex = 1
    Me.lblPro.Text = "Pro (P)"
    Me.lblPro.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblAsn
    '
    Me.lblAsn.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblAsn.Location = New System.Drawing.Point(12, 88)
    Me.lblAsn.Name = "lblAsn"
    Me.lblAsn.Size = New System.Drawing.Size(56, 12)
    Me.lblAsn.TabIndex = 1
    Me.lblAsn.Text = "Asn (N)"
    Me.lblAsn.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblGln
    '
    Me.lblGln.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblGln.Location = New System.Drawing.Point(260, 88)
    Me.lblGln.Name = "lblGln"
    Me.lblGln.Size = New System.Drawing.Size(56, 12)
    Me.lblGln.TabIndex = 1
    Me.lblGln.Text = "Gln (Q)"
    Me.lblGln.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblQandE
    '
    Me.lblQandE.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblQandE.Location = New System.Drawing.Point(382, 88)
    Me.lblQandE.Name = "lblQandE"
    Me.lblQandE.Size = New System.Drawing.Size(61, 12)
    Me.lblQandE.TabIndex = 1
    Me.lblQandE.Text = "Avg Q && E (Z)"
    Me.lblQandE.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblNandD
    '
    Me.lblNandD.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblNandD.Location = New System.Drawing.Point(134, 88)
    Me.lblNandD.Name = "lblNandD"
    Me.lblNandD.Size = New System.Drawing.Size(64, 12)
    Me.lblNandD.TabIndex = 1
    Me.lblNandD.Text = "Avg N && D (B)"
    Me.lblNandD.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblOrn
    '
    Me.lblOrn.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblOrn.Location = New System.Drawing.Point(72, 88)
    Me.lblOrn.Name = "lblOrn"
    Me.lblOrn.Size = New System.Drawing.Size(56, 12)
    Me.lblOrn.TabIndex = 1
    Me.lblOrn.Text = "Orn (O)"
    Me.lblOrn.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblAsp
    '
    Me.lblAsp.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblAsp.Location = New System.Drawing.Point(196, 88)
    Me.lblAsp.Name = "lblAsp"
    Me.lblAsp.Size = New System.Drawing.Size(56, 12)
    Me.lblAsp.TabIndex = 1
    Me.lblAsp.Text = "Asp (D)"
    Me.lblAsp.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblLys
    '
    Me.lblLys.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblLys.Location = New System.Drawing.Point(320, 88)
    Me.lblLys.Name = "lblLys"
    Me.lblLys.Size = New System.Drawing.Size(56, 12)
    Me.lblLys.TabIndex = 1
    Me.lblLys.Text = "Lys (K)"
    Me.lblLys.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblArg
    '
    Me.lblArg.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblArg.Location = New System.Drawing.Point(260, 122)
    Me.lblArg.Name = "lblArg"
    Me.lblArg.Size = New System.Drawing.Size(56, 12)
    Me.lblArg.TabIndex = 1
    Me.lblArg.Text = "Arg (R)"
    Me.lblArg.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblTrp
    '
    Me.lblTrp.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblTrp.Location = New System.Drawing.Point(384, 122)
    Me.lblTrp.Name = "lblTrp"
    Me.lblTrp.Size = New System.Drawing.Size(56, 12)
    Me.lblTrp.TabIndex = 1
    Me.lblTrp.Text = "Trp (W)"
    Me.lblTrp.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblHis
    '
    Me.lblHis.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblHis.Location = New System.Drawing.Point(136, 122)
    Me.lblHis.Name = "lblHis"
    Me.lblHis.Size = New System.Drawing.Size(56, 12)
    Me.lblHis.TabIndex = 1
    Me.lblHis.Text = "His (H)"
    Me.lblHis.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblMet
    '
    Me.lblMet.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblMet.Location = New System.Drawing.Point(72, 122)
    Me.lblMet.Name = "lblMet"
    Me.lblMet.Size = New System.Drawing.Size(56, 12)
    Me.lblMet.TabIndex = 1
    Me.lblMet.Text = "Met (M)"
    Me.lblMet.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblPhe
    '
    Me.lblPhe.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblPhe.Location = New System.Drawing.Point(196, 122)
    Me.lblPhe.Name = "lblPhe"
    Me.lblPhe.Size = New System.Drawing.Size(56, 12)
    Me.lblPhe.TabIndex = 1
    Me.lblPhe.Text = "Phe (F)"
    Me.lblPhe.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblTyr
    '
    Me.lblTyr.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblTyr.Location = New System.Drawing.Point(320, 122)
    Me.lblTyr.Name = "lblTyr"
    Me.lblTyr.Size = New System.Drawing.Size(56, 12)
    Me.lblTyr.TabIndex = 1
    Me.lblTyr.Text = "Tyr (Y)"
    Me.lblTyr.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblGlu
    '
    Me.lblGlu.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblGlu.Location = New System.Drawing.Point(12, 122)
    Me.lblGlu.Name = "lblGlu"
    Me.lblGlu.Size = New System.Drawing.Size(56, 12)
    Me.lblGlu.TabIndex = 1
    Me.lblGlu.Text = "Glu (E)"
    Me.lblGlu.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'gbxDesc
    '
    Me.gbxDesc.Controls.Add(Me.txtDescription)
    Me.gbxDesc.Controls.Add(Me.lblDescription)
    Me.gbxDesc.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.gbxDesc.Location = New System.Drawing.Point(8, 4)
    Me.gbxDesc.Name = "gbxDesc"
    Me.gbxDesc.Size = New System.Drawing.Size(452, 96)
    Me.gbxDesc.TabIndex = 0
    Me.gbxDesc.TabStop = False
    Me.gbxDesc.Text = "Name and Description Information"
    '
    'txtDescription
    '
    Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
    Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtDescription.Location = New System.Drawing.Point(12, 34)
    Me.txtDescription.Multiline = True
    Me.txtDescription.Name = "txtDescription"
    Me.txtDescription.ReadOnly = True
    Me.txtDescription.Size = New System.Drawing.Size(428, 50)
    Me.txtDescription.TabIndex = 0
    '
    'lblDescription
    '
    Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblDescription.Location = New System.Drawing.Point(12, 20)
    Me.lblDescription.Name = "lblDescription"
    Me.lblDescription.Size = New System.Drawing.Size(264, 16)
    Me.lblDescription.TabIndex = 1
    Me.lblDescription.Text = "Parameter File Descriptive Text"
    '
    'gbxSearch
    '
    Me.gbxSearch.Controls.Add(Me.txtPartialSeq)
    Me.gbxSearch.Controls.Add(Me.lblPartialSeq)
    Me.gbxSearch.Controls.Add(Me.cboParentMassUnits)
    Me.gbxSearch.Controls.Add(Me.cboFragmentMassUnits)
    Me.gbxSearch.Controls.Add(Me.cboFragmentMassType)
    Me.gbxSearch.Controls.Add(Me.cboMissedCleavages)
    Me.gbxSearch.Controls.Add(Me.cboParentMassType)
    Me.gbxSearch.Controls.Add(Me.lblFragmentMassUnits)
    Me.gbxSearch.Controls.Add(Me.lblParentMassUnits)
    Me.gbxSearch.Controls.Add(Me.lblParentMassType)
    Me.gbxSearch.Controls.Add(Me.cboEnzymeSelect)
    Me.gbxSearch.Controls.Add(Me.lblEnzymeSelect)
    Me.gbxSearch.Controls.Add(Me.lblMissedCleavages)
    Me.gbxSearch.Controls.Add(Me.lblFragmentMassType)
    Me.gbxSearch.Controls.Add(Me.cboCleavagePosition)
    Me.gbxSearch.Controls.Add(Me.lblCleavagePosition)
    Me.gbxSearch.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.gbxSearch.Location = New System.Drawing.Point(8, 104)
    Me.gbxSearch.Name = "gbxSearch"
    Me.gbxSearch.Size = New System.Drawing.Size(452, 140)
    Me.gbxSearch.TabIndex = 0
    Me.gbxSearch.TabStop = False
    Me.gbxSearch.Text = "Search Settings"
    '
    'txtPartialSeq
    '
    Me.txtPartialSeq.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtPartialSeq.Location = New System.Drawing.Point(244, 110)
    Me.txtPartialSeq.Name = "txtPartialSeq"
    Me.txtPartialSeq.Size = New System.Drawing.Size(196, 20)
    Me.txtPartialSeq.TabIndex = 5
    '
    'lblPartialSeq
    '
    Me.lblPartialSeq.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblPartialSeq.Location = New System.Drawing.Point(244, 96)
    Me.lblPartialSeq.Name = "lblPartialSeq"
    Me.lblPartialSeq.Size = New System.Drawing.Size(160, 16)
    Me.lblPartialSeq.TabIndex = 11
    Me.lblPartialSeq.Text = "Partial Sequence To Match"
    '
    'cboFragmentMassType
    '
    Me.cboFragmentMassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cboFragmentMassType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cboFragmentMassType.Location = New System.Drawing.Point(244, 34)
    Me.cboFragmentMassType.Name = "cboFragmentMassType"
    Me.cboFragmentMassType.Size = New System.Drawing.Size(132, 21)
    Me.cboFragmentMassType.TabIndex = 2
    '
    'cboMissedCleavages
    '
    Me.cboMissedCleavages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cboMissedCleavages.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cboMissedCleavages.Location = New System.Drawing.Point(244, 72)
    Me.cboMissedCleavages.Name = "cboMissedCleavages"
    Me.cboMissedCleavages.Size = New System.Drawing.Size(200, 21)
    Me.cboMissedCleavages.TabIndex = 4
    '
    'cboParentMassType
    '
    Me.cboParentMassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cboParentMassType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cboParentMassType.Location = New System.Drawing.Point(12, 34)
    Me.cboParentMassType.Name = "cboParentMassType"
    Me.cboParentMassType.Size = New System.Drawing.Size(132, 21)
    Me.cboParentMassType.TabIndex = 1
    '
    'lblParentMassType
    '
    Me.lblParentMassType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblParentMassType.Location = New System.Drawing.Point(12, 20)
    Me.lblParentMassType.Name = "lblParentMassType"
    Me.lblParentMassType.Size = New System.Drawing.Size(116, 20)
    Me.lblParentMassType.TabIndex = 2
    Me.lblParentMassType.Text = "Parent Ion Mass Type"
    '
    'cboEnzymeSelect
    '
    Me.cboEnzymeSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cboEnzymeSelect.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cboEnzymeSelect.Location = New System.Drawing.Point(12, 72)
    Me.cboEnzymeSelect.Name = "cboEnzymeSelect"
    Me.cboEnzymeSelect.Size = New System.Drawing.Size(200, 21)
    Me.cboEnzymeSelect.TabIndex = 3
    '
    'lblEnzymeSelect
    '
    Me.lblEnzymeSelect.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblEnzymeSelect.Location = New System.Drawing.Point(12, 58)
    Me.lblEnzymeSelect.Name = "lblEnzymeSelect"
    Me.lblEnzymeSelect.Size = New System.Drawing.Size(132, 16)
    Me.lblEnzymeSelect.TabIndex = 0
    Me.lblEnzymeSelect.Text = "Enzyme Cleavage Rule"
    '
    'lblMissedCleavages
    '
    Me.lblMissedCleavages.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblMissedCleavages.Location = New System.Drawing.Point(244, 58)
    Me.lblMissedCleavages.Name = "lblMissedCleavages"
    Me.lblMissedCleavages.Size = New System.Drawing.Size(200, 16)
    Me.lblMissedCleavages.TabIndex = 6
    Me.lblMissedCleavages.Text = "Number of Allowed Missed Cleavages"
    '
    'lblFragmentMassType
    '
    Me.lblFragmentMassType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblFragmentMassType.Location = New System.Drawing.Point(244, 20)
    Me.lblFragmentMassType.Name = "lblFragmentMassType"
    Me.lblFragmentMassType.Size = New System.Drawing.Size(132, 20)
    Me.lblFragmentMassType.TabIndex = 7
    Me.lblFragmentMassType.Text = "Fragment Ion Mass Type"
    '
    'cboCleavagePosition
    '
    Me.cboCleavagePosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cboCleavagePosition.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cboCleavagePosition.Location = New System.Drawing.Point(12, 110)
    Me.cboCleavagePosition.Name = "cboCleavagePosition"
    Me.cboCleavagePosition.Size = New System.Drawing.Size(200, 21)
    Me.cboCleavagePosition.TabIndex = 3
    '
    'lblCleavagePosition
    '
    Me.lblCleavagePosition.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblCleavagePosition.Location = New System.Drawing.Point(12, 96)
    Me.lblCleavagePosition.Name = "lblCleavagePosition"
    Me.lblCleavagePosition.Size = New System.Drawing.Size(148, 16)
    Me.lblCleavagePosition.TabIndex = 0
    Me.lblCleavagePosition.Text = "Enzyme Cleavage Positions"
    '
    'gbxDynMods
    '
    Me.gbxDynMods.Controls.Add(Me.txtDynMod1List)
    Me.gbxDynMods.Controls.Add(Me.txtDynMod1MassDiff)
    Me.gbxDynMods.Controls.Add(Me.txtDynMod2List)
    Me.gbxDynMods.Controls.Add(Me.txtDynMod2MassDiff)
    Me.gbxDynMods.Controls.Add(Me.txtDynMod3List)
    Me.gbxDynMods.Controls.Add(Me.txtDynMod3MassDiff)
    Me.gbxDynMods.Controls.Add(Me.lblDynMod1List)
    Me.gbxDynMods.Controls.Add(Me.lblDynMod2List)
    Me.gbxDynMods.Controls.Add(Me.lblDynMod3List)
    Me.gbxDynMods.Controls.Add(Me.lblDynMod1MassDiff)
    Me.gbxDynMods.Controls.Add(Me.lblDynMod3MassDiff)
    Me.gbxDynMods.Controls.Add(Me.lblDynMod2MassDiff)
    Me.gbxDynMods.Controls.Add(Me.txtDynMod4List)
    Me.gbxDynMods.Controls.Add(Me.txtDynMod4MassDiff)
    Me.gbxDynMods.Controls.Add(Me.lblDynMod4List)
    Me.gbxDynMods.Controls.Add(Me.lblDynMod4MassDiff)
    Me.gbxDynMods.Controls.Add(Me.lblDynMod5MassDiff)
    Me.gbxDynMods.Controls.Add(Me.txtDynMod5List)
    Me.gbxDynMods.Controls.Add(Me.txtDynMod5MassDiff)
    Me.gbxDynMods.Controls.Add(Me.lblDynMod5List)
    Me.gbxDynMods.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.gbxDynMods.Location = New System.Drawing.Point(8, 248)
    Me.gbxDynMods.Name = "gbxDynMods"
    Me.gbxDynMods.Size = New System.Drawing.Size(452, 104)
    Me.gbxDynMods.TabIndex = 0
    Me.gbxDynMods.TabStop = False
    Me.gbxDynMods.Text = "Dynamic Modifications to Apply"
    '
    'txtDynMod1List
    '
    Me.txtDynMod1List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtDynMod1List.Location = New System.Drawing.Point(12, 34)
    Me.txtDynMod1List.Name = "txtDynMod1List"
    Me.txtDynMod1List.Size = New System.Drawing.Size(76, 20)
    Me.txtDynMod1List.TabIndex = 6
    '
    'txtDynMod2List
    '
    Me.txtDynMod2List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtDynMod2List.Location = New System.Drawing.Point(100, 34)
    Me.txtDynMod2List.Name = "txtDynMod2List"
    Me.txtDynMod2List.Size = New System.Drawing.Size(75, 20)
    Me.txtDynMod2List.TabIndex = 8
    '
    'txtDynMod3List
    '
    Me.txtDynMod3List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtDynMod3List.Location = New System.Drawing.Point(188, 34)
    Me.txtDynMod3List.Name = "txtDynMod3List"
    Me.txtDynMod3List.Size = New System.Drawing.Size(74, 20)
    Me.txtDynMod3List.TabIndex = 10
    '
    'lblDynMod1List
    '
    Me.lblDynMod1List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblDynMod1List.Location = New System.Drawing.Point(12, 20)
    Me.lblDynMod1List.Name = "lblDynMod1List"
    Me.lblDynMod1List.Size = New System.Drawing.Size(84, 14)
    Me.lblDynMod1List.TabIndex = 1
    Me.lblDynMod1List.Text = "AA List 1"
    '
    'lblDynMod2List
    '
    Me.lblDynMod2List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblDynMod2List.Location = New System.Drawing.Point(100, 20)
    Me.lblDynMod2List.Name = "lblDynMod2List"
    Me.lblDynMod2List.Size = New System.Drawing.Size(84, 14)
    Me.lblDynMod2List.TabIndex = 2
    Me.lblDynMod2List.Text = "AA List 2"
    '
    'lblDynMod3List
    '
    Me.lblDynMod3List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblDynMod3List.Location = New System.Drawing.Point(188, 20)
    Me.lblDynMod3List.Name = "lblDynMod3List"
    Me.lblDynMod3List.Size = New System.Drawing.Size(84, 14)
    Me.lblDynMod3List.TabIndex = 3
    Me.lblDynMod3List.Text = "AA List 3"
    '
    'lblDynMod1MassDiff
    '
    Me.lblDynMod1MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblDynMod1MassDiff.Location = New System.Drawing.Point(12, 60)
    Me.lblDynMod1MassDiff.Name = "lblDynMod1MassDiff"
    Me.lblDynMod1MassDiff.Size = New System.Drawing.Size(84, 14)
    Me.lblDynMod1MassDiff.TabIndex = 4
    Me.lblDynMod1MassDiff.Text = "Mass Change 1"
    '
    'lblDynMod3MassDiff
    '
    Me.lblDynMod3MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblDynMod3MassDiff.Location = New System.Drawing.Point(188, 60)
    Me.lblDynMod3MassDiff.Name = "lblDynMod3MassDiff"
    Me.lblDynMod3MassDiff.Size = New System.Drawing.Size(84, 14)
    Me.lblDynMod3MassDiff.TabIndex = 6
    Me.lblDynMod3MassDiff.Text = "Mass Change 3"
    '
    'lblDynMod2MassDiff
    '
    Me.lblDynMod2MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblDynMod2MassDiff.Location = New System.Drawing.Point(100, 60)
    Me.lblDynMod2MassDiff.Name = "lblDynMod2MassDiff"
    Me.lblDynMod2MassDiff.Size = New System.Drawing.Size(84, 14)
    Me.lblDynMod2MassDiff.TabIndex = 5
    Me.lblDynMod2MassDiff.Text = "Mass Change 2"
    '
    'txtDynMod4List
    '
    Me.txtDynMod4List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtDynMod4List.Location = New System.Drawing.Point(276, 36)
    Me.txtDynMod4List.Name = "txtDynMod4List"
    Me.txtDynMod4List.Size = New System.Drawing.Size(73, 20)
    Me.txtDynMod4List.TabIndex = 10
    '
    'lblDynMod4List
    '
    Me.lblDynMod4List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblDynMod4List.Location = New System.Drawing.Point(276, 20)
    Me.lblDynMod4List.Name = "lblDynMod4List"
    Me.lblDynMod4List.Size = New System.Drawing.Size(84, 14)
    Me.lblDynMod4List.TabIndex = 3
    Me.lblDynMod4List.Text = "AA List 4"
    '
    'lblDynMod4MassDiff
    '
    Me.lblDynMod4MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblDynMod4MassDiff.Location = New System.Drawing.Point(276, 60)
    Me.lblDynMod4MassDiff.Name = "lblDynMod4MassDiff"
    Me.lblDynMod4MassDiff.Size = New System.Drawing.Size(84, 14)
    Me.lblDynMod4MassDiff.TabIndex = 6
    Me.lblDynMod4MassDiff.Text = "Mass Change 4"
    '
    'lblDynMod5MassDiff
    '
    Me.lblDynMod5MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblDynMod5MassDiff.Location = New System.Drawing.Point(364, 60)
    Me.lblDynMod5MassDiff.Name = "lblDynMod5MassDiff"
    Me.lblDynMod5MassDiff.Size = New System.Drawing.Size(84, 14)
    Me.lblDynMod5MassDiff.TabIndex = 6
    Me.lblDynMod5MassDiff.Text = "Mass Change 5"
    '
    'txtDynMod5List
    '
    Me.txtDynMod5List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtDynMod5List.Location = New System.Drawing.Point(364, 36)
    Me.txtDynMod5List.Name = "txtDynMod5List"
    Me.txtDynMod5List.Size = New System.Drawing.Size(76, 20)
    Me.txtDynMod5List.TabIndex = 10
    '
    'lblDynMod5List
    '
    Me.lblDynMod5List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblDynMod5List.Location = New System.Drawing.Point(364, 20)
    Me.lblDynMod5List.Name = "lblDynMod5List"
    Me.lblDynMod5List.Size = New System.Drawing.Size(84, 14)
    Me.lblDynMod5List.TabIndex = 3
    Me.lblDynMod5List.Text = "AA List 5"
    '
    'tabAdvanced
    '
    Me.tabAdvanced.Controls.Add(Me.gbxIonWeighting)
    Me.tabAdvanced.Controls.Add(Me.gbxMiscParams)
    Me.tabAdvanced.Controls.Add(Me.gbxToleranceValues)
    Me.tabAdvanced.Controls.Add(Me.gbxSwitches)
    Me.tabAdvanced.Location = New System.Drawing.Point(4, 24)
    Me.tabAdvanced.Name = "tabAdvanced"
    Me.tabAdvanced.Size = New System.Drawing.Size(468, 616)
    Me.tabAdvanced.TabIndex = 1
    Me.tabAdvanced.Text = "Advanced Parameters"
    '
    'gbxIonWeighting
    '
    Me.gbxIonWeighting.Controls.Add(Me.txtWWeight)
    Me.gbxIonWeighting.Controls.Add(Me.lblWWeight)
    Me.gbxIonWeighting.Controls.Add(Me.txtXWeight)
    Me.gbxIonWeighting.Controls.Add(Me.lblXWeight)
    Me.gbxIonWeighting.Controls.Add(Me.txtDWeight)
    Me.gbxIonWeighting.Controls.Add(Me.lblDWeight)
    Me.gbxIonWeighting.Controls.Add(Me.txtCWeight)
    Me.gbxIonWeighting.Controls.Add(Me.lblCWeight)
    Me.gbxIonWeighting.Controls.Add(Me.txtBWeight)
    Me.gbxIonWeighting.Controls.Add(Me.lblBWeight)
    Me.gbxIonWeighting.Controls.Add(Me.lblVWeight)
    Me.gbxIonWeighting.Controls.Add(Me.txtVWeight)
    Me.gbxIonWeighting.Controls.Add(Me.txtYWeight)
    Me.gbxIonWeighting.Controls.Add(Me.lblYWeight)
    Me.gbxIonWeighting.Controls.Add(Me.txtZWeight)
    Me.gbxIonWeighting.Controls.Add(Me.lblZWeight)
    Me.gbxIonWeighting.Controls.Add(Me.txtAWeight)
    Me.gbxIonWeighting.Controls.Add(Me.lblAWeight)
    Me.gbxIonWeighting.Controls.Add(Me.chkUseAIons)
    Me.gbxIonWeighting.Controls.Add(Me.chkUseBIons)
    Me.gbxIonWeighting.Controls.Add(Me.chkUseYIons)
    Me.gbxIonWeighting.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.gbxIonWeighting.Location = New System.Drawing.Point(8, 384)
    Me.gbxIonWeighting.Name = "gbxIonWeighting"
    Me.gbxIonWeighting.Size = New System.Drawing.Size(452, 92)
    Me.gbxIonWeighting.TabIndex = 3
    Me.gbxIonWeighting.TabStop = False
    Me.gbxIonWeighting.Text = "Ion Weighting Parameters"
    '
    'txtWWeight
    '
    Me.txtWWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtWWeight.Location = New System.Drawing.Point(192, 60)
    Me.txtWWeight.Name = "txtWWeight"
    Me.txtWWeight.Size = New System.Drawing.Size(55, 20)
    Me.txtWWeight.TabIndex = 19
    '
    'lblWWeight
    '
    Me.lblWWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblWWeight.Location = New System.Drawing.Point(192, 48)
    Me.lblWWeight.Name = "lblWWeight"
    Me.lblWWeight.Size = New System.Drawing.Size(60, 12)
    Me.lblWWeight.TabIndex = 14
    Me.lblWWeight.Text = "w Ion Weight"
    Me.lblWWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'txtXWeight
    '
    Me.txtXWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtXWeight.Location = New System.Drawing.Point(256, 60)
    Me.txtXWeight.Name = "txtXWeight"
    Me.txtXWeight.Size = New System.Drawing.Size(55, 20)
    Me.txtXWeight.TabIndex = 20
    '
    'lblXWeight
    '
    Me.lblXWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblXWeight.Location = New System.Drawing.Point(256, 48)
    Me.lblXWeight.Name = "lblXWeight"
    Me.lblXWeight.Size = New System.Drawing.Size(56, 12)
    Me.lblXWeight.TabIndex = 12
    Me.lblXWeight.Text = "x Ion Weight"
    Me.lblXWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'txtDWeight
    '
    Me.txtDWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtDWeight.Location = New System.Drawing.Point(352, 24)
    Me.txtDWeight.Name = "txtDWeight"
    Me.txtDWeight.Size = New System.Drawing.Size(55, 20)
    Me.txtDWeight.TabIndex = 17
    '
    'lblDWeight
    '
    Me.lblDWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblDWeight.Location = New System.Drawing.Point(352, 12)
    Me.lblDWeight.Name = "lblDWeight"
    Me.lblDWeight.Size = New System.Drawing.Size(60, 12)
    Me.lblDWeight.TabIndex = 10
    Me.lblDWeight.Text = "d Ion Weight"
    Me.lblDWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'txtCWeight
    '
    Me.txtCWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtCWeight.Location = New System.Drawing.Point(288, 24)
    Me.txtCWeight.Name = "txtCWeight"
    Me.txtCWeight.Size = New System.Drawing.Size(55, 20)
    Me.txtCWeight.TabIndex = 16
    '
    'lblCWeight
    '
    Me.lblCWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblCWeight.Location = New System.Drawing.Point(288, 12)
    Me.lblCWeight.Name = "lblCWeight"
    Me.lblCWeight.Size = New System.Drawing.Size(56, 12)
    Me.lblCWeight.TabIndex = 8
    Me.lblCWeight.Text = "c Ion Weight"
    Me.lblCWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'txtBWeight
    '
    Me.txtBWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtBWeight.Location = New System.Drawing.Point(224, 24)
    Me.txtBWeight.Name = "txtBWeight"
    Me.txtBWeight.Size = New System.Drawing.Size(55, 20)
    Me.txtBWeight.TabIndex = 15
    '
    'lblBWeight
    '
    Me.lblBWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblBWeight.Location = New System.Drawing.Point(224, 12)
    Me.lblBWeight.Name = "lblBWeight"
    Me.lblBWeight.Size = New System.Drawing.Size(56, 12)
    Me.lblBWeight.TabIndex = 6
    Me.lblBWeight.Text = "b Ion Weight"
    Me.lblBWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'lblVWeight
    '
    Me.lblVWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblVWeight.Location = New System.Drawing.Point(128, 48)
    Me.lblVWeight.Name = "lblVWeight"
    Me.lblVWeight.Size = New System.Drawing.Size(56, 12)
    Me.lblVWeight.TabIndex = 3
    Me.lblVWeight.Text = "v Ion Weight"
    Me.lblVWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'txtVWeight
    '
    Me.txtVWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtVWeight.Location = New System.Drawing.Point(128, 60)
    Me.txtVWeight.Name = "txtVWeight"
    Me.txtVWeight.Size = New System.Drawing.Size(55, 20)
    Me.txtVWeight.TabIndex = 18
    '
    'txtYWeight
    '
    Me.txtYWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtYWeight.Location = New System.Drawing.Point(320, 60)
    Me.txtYWeight.Name = "txtYWeight"
    Me.txtYWeight.Size = New System.Drawing.Size(55, 20)
    Me.txtYWeight.TabIndex = 21
    '
    'lblYWeight
    '
    Me.lblYWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblYWeight.Location = New System.Drawing.Point(320, 48)
    Me.lblYWeight.Name = "lblYWeight"
    Me.lblYWeight.Size = New System.Drawing.Size(56, 12)
    Me.lblYWeight.TabIndex = 3
    Me.lblYWeight.Text = "y Ion Weight"
    Me.lblYWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'txtZWeight
    '
    Me.txtZWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtZWeight.Location = New System.Drawing.Point(384, 60)
    Me.txtZWeight.Name = "txtZWeight"
    Me.txtZWeight.Size = New System.Drawing.Size(55, 20)
    Me.txtZWeight.TabIndex = 22
    '
    'lblZWeight
    '
    Me.lblZWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblZWeight.Location = New System.Drawing.Point(384, 48)
    Me.lblZWeight.Name = "lblZWeight"
    Me.lblZWeight.Size = New System.Drawing.Size(56, 12)
    Me.lblZWeight.TabIndex = 3
    Me.lblZWeight.Text = "z Ion Weight"
    Me.lblZWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'txtAWeight
    '
    Me.txtAWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtAWeight.Location = New System.Drawing.Point(160, 24)
    Me.txtAWeight.Name = "txtAWeight"
    Me.txtAWeight.Size = New System.Drawing.Size(55, 20)
    Me.txtAWeight.TabIndex = 14
    '
    'lblAWeight
    '
    Me.lblAWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblAWeight.Location = New System.Drawing.Point(160, 12)
    Me.lblAWeight.Name = "lblAWeight"
    Me.lblAWeight.Size = New System.Drawing.Size(56, 12)
    Me.lblAWeight.TabIndex = 3
    Me.lblAWeight.Text = "a Ion Weight"
    Me.lblAWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
    '
    'chkUseAIons
    '
    Me.chkUseAIons.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkUseAIons.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkUseAIons.Location = New System.Drawing.Point(12, 23)
    Me.chkUseAIons.Name = "chkUseAIons"
    Me.chkUseAIons.Size = New System.Drawing.Size(104, 16)
    Me.chkUseAIons.TabIndex = 23
    Me.chkUseAIons.Text = "Use A Ions?"
    '
    'chkUseBIons
    '
    Me.chkUseBIons.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkUseBIons.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkUseBIons.Location = New System.Drawing.Point(12, 43)
    Me.chkUseBIons.Name = "chkUseBIons"
    Me.chkUseBIons.Size = New System.Drawing.Size(104, 16)
    Me.chkUseBIons.TabIndex = 24
    Me.chkUseBIons.Text = "Use B Ions?"
    '
    'chkUseYIons
    '
    Me.chkUseYIons.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkUseYIons.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkUseYIons.Location = New System.Drawing.Point(12, 63)
    Me.chkUseYIons.Name = "chkUseYIons"
    Me.chkUseYIons.Size = New System.Drawing.Size(104, 16)
    Me.chkUseYIons.TabIndex = 25
    Me.chkUseYIons.Text = "Use Y Ions?"
    '
    'gbxMiscParams
    '
    Me.gbxMiscParams.Controls.Add(Me.lblNumResults)
    Me.gbxMiscParams.Controls.Add(Me.txtNumResults)
    Me.gbxMiscParams.Controls.Add(Me.cboNucReadingFrame)
    Me.gbxMiscParams.Controls.Add(Me.txtNumDescLines)
    Me.gbxMiscParams.Controls.Add(Me.lblOutputLines)
    Me.gbxMiscParams.Controls.Add(Me.txtNumOutputLines)
    Me.gbxMiscParams.Controls.Add(Me.lblNumDescLines)
    Me.gbxMiscParams.Controls.Add(Me.txtMatchPeakCountErrors)
    Me.gbxMiscParams.Controls.Add(Me.lblMatchPeakCountErrors)
    Me.gbxMiscParams.Controls.Add(Me.lblMatchPeakCount)
    Me.gbxMiscParams.Controls.Add(Me.txtMatchPeakCount)
    Me.gbxMiscParams.Controls.Add(Me.txtMaxDiffPerPeptide)
    Me.gbxMiscParams.Controls.Add(Me.lblMaxAAPerDynMod)
    Me.gbxMiscParams.Controls.Add(Me.txtMaxAAPerDynMod)
    Me.gbxMiscParams.Controls.Add(Me.lblNucReadingFrame)
    Me.gbxMiscParams.Controls.Add(Me.lblSeqHdrFilter)
    Me.gbxMiscParams.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.gbxMiscParams.Location = New System.Drawing.Point(8, 172)
    Me.gbxMiscParams.Name = "gbxMiscParams"
    Me.gbxMiscParams.Size = New System.Drawing.Size(452, 204)
    Me.gbxMiscParams.TabIndex = 2
    Me.gbxMiscParams.TabStop = False
    Me.gbxMiscParams.Text = "Miscellaneous Options"
    '
    'lblNumResults
    '
    Me.lblNumResults.Enabled = False
    Me.lblNumResults.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblNumResults.Location = New System.Drawing.Point(244, 152)
    Me.lblNumResults.Name = "lblNumResults"
    Me.lblNumResults.Size = New System.Drawing.Size(160, 16)
    Me.lblNumResults.TabIndex = 18
    Me.lblNumResults.Text = "Number of Results To Process"
    '
    'txtNumResults
    '
    Me.txtNumResults.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtNumResults.Location = New System.Drawing.Point(244, 168)
    Me.txtNumResults.Name = "txtNumResults"
    Me.txtNumResults.Size = New System.Drawing.Size(196, 20)
    Me.txtNumResults.TabIndex = 13
    '
    'cboNucReadingFrame
    '
    Me.cboNucReadingFrame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cboNucReadingFrame.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cboNucReadingFrame.Location = New System.Drawing.Point(12, 168)
    Me.cboNucReadingFrame.Name = "cboNucReadingFrame"
    Me.cboNucReadingFrame.Size = New System.Drawing.Size(200, 21)
    Me.cboNucReadingFrame.TabIndex = 12
    '
    'txtNumDescLines
    '
    Me.txtNumDescLines.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtNumDescLines.Location = New System.Drawing.Point(244, 36)
    Me.txtNumDescLines.Name = "txtNumDescLines"
    Me.txtNumDescLines.Size = New System.Drawing.Size(196, 20)
    Me.txtNumDescLines.TabIndex = 7
    '
    'lblOutputLines
    '
    Me.lblOutputLines.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblOutputLines.Location = New System.Drawing.Point(12, 20)
    Me.lblOutputLines.Name = "lblOutputLines"
    Me.lblOutputLines.Size = New System.Drawing.Size(188, 16)
    Me.lblOutputLines.TabIndex = 9
    Me.lblOutputLines.Text = "Number of Peptide Results to Show"
    '
    'txtNumOutputLines
    '
    Me.txtNumOutputLines.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtNumOutputLines.Location = New System.Drawing.Point(12, 36)
    Me.txtNumOutputLines.Name = "txtNumOutputLines"
    Me.txtNumOutputLines.Size = New System.Drawing.Size(196, 20)
    Me.txtNumOutputLines.TabIndex = 6
    '
    'lblNumDescLines
    '
    Me.lblNumDescLines.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblNumDescLines.Location = New System.Drawing.Point(244, 20)
    Me.lblNumDescLines.Name = "lblNumDescLines"
    Me.lblNumDescLines.Size = New System.Drawing.Size(188, 16)
    Me.lblNumDescLines.TabIndex = 13
    Me.lblNumDescLines.Text = "Number of Descriptions to Show"
    '
    'txtMatchPeakCountErrors
    '
    Me.txtMatchPeakCountErrors.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtMatchPeakCountErrors.Location = New System.Drawing.Point(244, 80)
    Me.txtMatchPeakCountErrors.Name = "txtMatchPeakCountErrors"
    Me.txtMatchPeakCountErrors.Size = New System.Drawing.Size(196, 20)
    Me.txtMatchPeakCountErrors.TabIndex = 9
    '
    'lblMatchPeakCountErrors
    '
    Me.lblMatchPeakCountErrors.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblMatchPeakCountErrors.Location = New System.Drawing.Point(244, 64)
    Me.lblMatchPeakCountErrors.Name = "lblMatchPeakCountErrors"
    Me.lblMatchPeakCountErrors.Size = New System.Drawing.Size(188, 16)
    Me.lblMatchPeakCountErrors.TabIndex = 14
    Me.lblMatchPeakCountErrors.Text = "Number of Peak Errors Allowed"
    '
    'lblMatchPeakCount
    '
    Me.lblMatchPeakCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblMatchPeakCount.Location = New System.Drawing.Point(12, 64)
    Me.lblMatchPeakCount.Name = "lblMatchPeakCount"
    Me.lblMatchPeakCount.Size = New System.Drawing.Size(220, 16)
    Me.lblMatchPeakCount.TabIndex = 8
    Me.lblMatchPeakCount.Text = "Number of Peaks to Try to Match"
    '
    'txtMatchPeakCount
    '
    Me.txtMatchPeakCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtMatchPeakCount.Location = New System.Drawing.Point(12, 80)
    Me.txtMatchPeakCount.Name = "txtMatchPeakCount"
    Me.txtMatchPeakCount.Size = New System.Drawing.Size(196, 20)
    Me.txtMatchPeakCount.TabIndex = 8
    '
    'txtMaxDiffPerPeptide
    '
    Me.txtMaxDiffPerPeptide.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtMaxDiffPerPeptide.Location = New System.Drawing.Point(244, 124)
    Me.txtMaxDiffPerPeptide.Name = "txtMaxDiffPerPeptide"
    Me.txtMaxDiffPerPeptide.Size = New System.Drawing.Size(196, 20)
    Me.txtMaxDiffPerPeptide.TabIndex = 11
    '
    'lblMaxAAPerDynMod
    '
    Me.lblMaxAAPerDynMod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblMaxAAPerDynMod.Location = New System.Drawing.Point(12, 108)
    Me.lblMaxAAPerDynMod.Name = "lblMaxAAPerDynMod"
    Me.lblMaxAAPerDynMod.Size = New System.Drawing.Size(188, 16)
    Me.lblMaxAAPerDynMod.TabIndex = 7
    Me.lblMaxAAPerDynMod.Text = "Maximum Dynamic Mods Per AA"
    '
    'txtMaxAAPerDynMod
    '
    Me.txtMaxAAPerDynMod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtMaxAAPerDynMod.Location = New System.Drawing.Point(12, 124)
    Me.txtMaxAAPerDynMod.Name = "txtMaxAAPerDynMod"
    Me.txtMaxAAPerDynMod.Size = New System.Drawing.Size(196, 20)
    Me.txtMaxAAPerDynMod.TabIndex = 10
    '
    'lblNucReadingFrame
    '
    Me.lblNucReadingFrame.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblNucReadingFrame.Location = New System.Drawing.Point(12, 152)
    Me.lblNucReadingFrame.Name = "lblNucReadingFrame"
    Me.lblNucReadingFrame.Size = New System.Drawing.Size(188, 16)
    Me.lblNucReadingFrame.TabIndex = 7
    Me.lblNucReadingFrame.Text = "Nucleotide Reading Frame"
    '
    'lblSeqHdrFilter
    '
    Me.lblSeqHdrFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblSeqHdrFilter.Location = New System.Drawing.Point(244, 108)
    Me.lblSeqHdrFilter.Name = "lblSeqHdrFilter"
    Me.lblSeqHdrFilter.Size = New System.Drawing.Size(204, 16)
    Me.lblSeqHdrFilter.TabIndex = 15
    Me.lblSeqHdrFilter.Text = "Maximum Differential Mods Per Peptide"
    '
    'gbxToleranceValues
    '
    Me.gbxToleranceValues.Controls.Add(Me.txtFragMassTol)
    Me.gbxToleranceValues.Controls.Add(Me.lblPepMassTol)
    Me.gbxToleranceValues.Controls.Add(Me.txtPepMassTol)
    Me.gbxToleranceValues.Controls.Add(Me.lblFragMassTol)
    Me.gbxToleranceValues.Controls.Add(Me.txtIonCutoff)
    Me.gbxToleranceValues.Controls.Add(Me.lblIonCutoff)
    Me.gbxToleranceValues.Controls.Add(Me.lblPeakMatchingTol)
    Me.gbxToleranceValues.Controls.Add(Me.txtPeakMatchingTol)
    Me.gbxToleranceValues.Controls.Add(Me.lblMaxProtMass)
    Me.gbxToleranceValues.Controls.Add(Me.txtMaxProtMass)
    Me.gbxToleranceValues.Controls.Add(Me.lblMinProtMass)
    Me.gbxToleranceValues.Controls.Add(Me.txtMinProtMass)
    Me.gbxToleranceValues.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.gbxToleranceValues.Location = New System.Drawing.Point(8, 4)
    Me.gbxToleranceValues.Name = "gbxToleranceValues"
    Me.gbxToleranceValues.Size = New System.Drawing.Size(452, 160)
    Me.gbxToleranceValues.TabIndex = 1
    Me.gbxToleranceValues.TabStop = False
    Me.gbxToleranceValues.Text = "Search Tolerance Values"
    '
    'txtFragMassTol
    '
    Me.txtFragMassTol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtFragMassTol.Location = New System.Drawing.Point(244, 36)
    Me.txtFragMassTol.Name = "txtFragMassTol"
    Me.txtFragMassTol.Size = New System.Drawing.Size(196, 20)
    Me.txtFragMassTol.TabIndex = 1
    '
    'lblPepMassTol
    '
    Me.lblPepMassTol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblPepMassTol.Location = New System.Drawing.Point(12, 20)
    Me.lblPepMassTol.Name = "lblPepMassTol"
    Me.lblPepMassTol.Size = New System.Drawing.Size(188, 16)
    Me.lblPepMassTol.TabIndex = 1
    Me.lblPepMassTol.Text = "Parent Mass Tolerance"
    '
    'txtPepMassTol
    '
    Me.txtPepMassTol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtPepMassTol.Location = New System.Drawing.Point(12, 36)
    Me.txtPepMassTol.Name = "txtPepMassTol"
    Me.txtPepMassTol.Size = New System.Drawing.Size(196, 20)
    Me.txtPepMassTol.TabIndex = 0
    '
    'lblFragMassTol
    '
    Me.lblFragMassTol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblFragMassTol.Location = New System.Drawing.Point(244, 20)
    Me.lblFragMassTol.Name = "lblFragMassTol"
    Me.lblFragMassTol.Size = New System.Drawing.Size(188, 16)
    Me.lblFragMassTol.TabIndex = 3
    Me.lblFragMassTol.Text = "Fragment Mass Tolerance"
    '
    'txtIonCutoff
    '
    Me.txtIonCutoff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtIonCutoff.Location = New System.Drawing.Point(244, 80)
    Me.txtIonCutoff.Name = "txtIonCutoff"
    Me.txtIonCutoff.Size = New System.Drawing.Size(196, 20)
    Me.txtIonCutoff.TabIndex = 3
    '
    'lblIonCutoff
    '
    Me.lblIonCutoff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblIonCutoff.Location = New System.Drawing.Point(244, 64)
    Me.lblIonCutoff.Name = "lblIonCutoff"
    Me.lblIonCutoff.Size = New System.Drawing.Size(188, 16)
    Me.lblIonCutoff.TabIndex = 3
    Me.lblIonCutoff.Text = "Preliminary Score Cutoff Percentage"
    '
    'lblPeakMatchingTol
    '
    Me.lblPeakMatchingTol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblPeakMatchingTol.Location = New System.Drawing.Point(12, 64)
    Me.lblPeakMatchingTol.Name = "lblPeakMatchingTol"
    Me.lblPeakMatchingTol.Size = New System.Drawing.Size(208, 16)
    Me.lblPeakMatchingTol.TabIndex = 1
    Me.lblPeakMatchingTol.Text = "Detected Peak Matching Tolerance"
    '
    'txtPeakMatchingTol
    '
    Me.txtPeakMatchingTol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtPeakMatchingTol.Location = New System.Drawing.Point(12, 80)
    Me.txtPeakMatchingTol.Name = "txtPeakMatchingTol"
    Me.txtPeakMatchingTol.Size = New System.Drawing.Size(196, 20)
    Me.txtPeakMatchingTol.TabIndex = 2
    '
    'lblMaxProtMass
    '
    Me.lblMaxProtMass.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblMaxProtMass.Location = New System.Drawing.Point(244, 108)
    Me.lblMaxProtMass.Name = "lblMaxProtMass"
    Me.lblMaxProtMass.Size = New System.Drawing.Size(188, 16)
    Me.lblMaxProtMass.TabIndex = 3
    Me.lblMaxProtMass.Text = "Maximum Allowed Protein Mass"
    '
    'txtMaxProtMass
    '
    Me.txtMaxProtMass.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtMaxProtMass.Location = New System.Drawing.Point(244, 124)
    Me.txtMaxProtMass.Name = "txtMaxProtMass"
    Me.txtMaxProtMass.Size = New System.Drawing.Size(196, 20)
    Me.txtMaxProtMass.TabIndex = 5
    '
    'lblMinProtMass
    '
    Me.lblMinProtMass.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblMinProtMass.Location = New System.Drawing.Point(12, 108)
    Me.lblMinProtMass.Name = "lblMinProtMass"
    Me.lblMinProtMass.Size = New System.Drawing.Size(188, 16)
    Me.lblMinProtMass.TabIndex = 1
    Me.lblMinProtMass.Text = "Minimum Allowed Protein Mass"
    '
    'txtMinProtMass
    '
    Me.txtMinProtMass.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtMinProtMass.Location = New System.Drawing.Point(12, 124)
    Me.txtMinProtMass.Name = "txtMinProtMass"
    Me.txtMinProtMass.Size = New System.Drawing.Size(196, 20)
    Me.txtMinProtMass.TabIndex = 4
    '
    'gbxSwitches
    '
    Me.gbxSwitches.Controls.Add(Me.chkResiduesInUpperCase)
    Me.gbxSwitches.Controls.Add(Me.chkPrintDupRefs)
    Me.gbxSwitches.Controls.Add(Me.chkRemovePrecursorPeaks)
    Me.gbxSwitches.Controls.Add(Me.chkShowFragmentIons)
    Me.gbxSwitches.Controls.Add(Me.chkCreateOutputFiles)
    Me.gbxSwitches.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.gbxSwitches.Location = New System.Drawing.Point(8, 480)
    Me.gbxSwitches.Name = "gbxSwitches"
    Me.gbxSwitches.Size = New System.Drawing.Size(452, 124)
    Me.gbxSwitches.TabIndex = 0
    Me.gbxSwitches.TabStop = False
    Me.gbxSwitches.Text = "Search Options"
    '
    'chkResiduesInUpperCase
    '
    Me.chkResiduesInUpperCase.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkResiduesInUpperCase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkResiduesInUpperCase.Location = New System.Drawing.Point(12, 96)
    Me.chkResiduesInUpperCase.Name = "chkResiduesInUpperCase"
    Me.chkResiduesInUpperCase.Size = New System.Drawing.Size(248, 24)
    Me.chkResiduesInUpperCase.TabIndex = 30
    Me.chkResiduesInUpperCase.Text = "FASTA File has Residues in Upper Case?"
    '
    'chkPrintDupRefs
    '
    Me.chkPrintDupRefs.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkPrintDupRefs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkPrintDupRefs.Location = New System.Drawing.Point(12, 76)
    Me.chkPrintDupRefs.Name = "chkPrintDupRefs"
    Me.chkPrintDupRefs.Size = New System.Drawing.Size(248, 24)
    Me.chkPrintDupRefs.TabIndex = 29
    Me.chkPrintDupRefs.Text = "Print Duplicate References (ORFs)?"
    '
    'chkRemovePrecursorPeaks
    '
    Me.chkRemovePrecursorPeaks.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkRemovePrecursorPeaks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkRemovePrecursorPeaks.Location = New System.Drawing.Point(12, 56)
    Me.chkRemovePrecursorPeaks.Name = "chkRemovePrecursorPeaks"
    Me.chkRemovePrecursorPeaks.Size = New System.Drawing.Size(248, 24)
    Me.chkRemovePrecursorPeaks.TabIndex = 28
    Me.chkRemovePrecursorPeaks.Text = "Remove Precursor Ion Peaks?"
    '
    'chkShowFragmentIons
    '
    Me.chkShowFragmentIons.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkShowFragmentIons.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkShowFragmentIons.Location = New System.Drawing.Point(12, 36)
    Me.chkShowFragmentIons.Name = "chkShowFragmentIons"
    Me.chkShowFragmentIons.Size = New System.Drawing.Size(248, 24)
    Me.chkShowFragmentIons.TabIndex = 27
    Me.chkShowFragmentIons.Text = "Show Fragment Ions?"
    '
    'chkCreateOutputFiles
    '
    Me.chkCreateOutputFiles.FlatStyle = System.Windows.Forms.FlatStyle.System
    Me.chkCreateOutputFiles.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.chkCreateOutputFiles.Location = New System.Drawing.Point(12, 16)
    Me.chkCreateOutputFiles.Name = "chkCreateOutputFiles"
    Me.chkCreateOutputFiles.Size = New System.Drawing.Size(248, 24)
    Me.chkCreateOutputFiles.TabIndex = 26
    Me.chkCreateOutputFiles.Text = "Create Output Files?"
    '
    'mnuMain
    '
    Me.mnuMain.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.MenuItem1, Me.mnuHelp, Me.mnuDebug})
    '
    'mnuFile
    '
    Me.mnuFile.Index = 0
    Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileNewFileFromTemp, Me.mnuFileSaveToFile, Me.mnuFileUploadDMS, Me.mnuBatchUploadDMS, Me.mnuDiv1, Me.mnuFileExit})
    Me.mnuFile.Text = "File"
    '
    'mnuFileNewFileFromTemp
    '
    Me.mnuFileNewFileFromTemp.Index = 0
    Me.mnuFileNewFileFromTemp.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuLoadFromFile, Me.mnuFileLoadFromDMS})
    Me.mnuFileNewFileFromTemp.Shortcut = System.Windows.Forms.Shortcut.CtrlN
    Me.mnuFileNewFileFromTemp.Text = "Load Param File Settings"
    '
    'mnuLoadFromFile
    '
    Me.mnuLoadFromFile.Index = 0
    Me.mnuLoadFromFile.Text = "From Local Template File..."
    '
    'mnuFileLoadFromDMS
    '
    Me.mnuFileLoadFromDMS.Index = 1
    Me.mnuFileLoadFromDMS.Text = "From DMS Parameter Storage..."
    '
    'mnuFileSaveToFile
    '
    Me.mnuFileSaveToFile.Index = 1
    Me.mnuFileSaveToFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileSaveBW2, Me.mnuFileSaveBW3, Me.mnuFileSaveBW32})
    Me.mnuFileSaveToFile.Shortcut = System.Windows.Forms.Shortcut.CtrlS
    Me.mnuFileSaveToFile.Text = "&Save Current Settings as New Param File"
    '
    'mnuFileSaveBW2
    '
    Me.mnuFileSaveBW2.Index = 0
    Me.mnuFileSaveBW2.Text = "BioWorks 2.0 Format..."
    '
    'mnuFileSaveBW3
    '
    Me.mnuFileSaveBW3.Index = 1
    Me.mnuFileSaveBW3.Text = "BioWorks 3.0 Format..."
    '
    'mnuFileSaveBW32
    '
    Me.mnuFileSaveBW32.Index = 2
    Me.mnuFileSaveBW32.Text = "BioWorks 3.2 Format..."
    '
    'mnuFileUploadDMS
    '
    Me.mnuFileUploadDMS.Index = 2
    Me.mnuFileUploadDMS.Text = "Upload Current Settings to DMS (Restricted)..."
    '
    'mnuBatchUploadDMS
    '
    Me.mnuBatchUploadDMS.Index = 3
    Me.mnuBatchUploadDMS.Text = "Batch Upload Param Files to DMS (Restricted)"
    '
    'mnuDiv1
    '
    Me.mnuDiv1.Index = 4
    Me.mnuDiv1.Text = "-"
    '
    'mnuFileExit
    '
    Me.mnuFileExit.Index = 5
    Me.mnuFileExit.Shortcut = System.Windows.Forms.Shortcut.CtrlX
    Me.mnuFileExit.Text = "E&xit"
    '
    'MenuItem1
    '
    Me.MenuItem1.Enabled = False
    Me.MenuItem1.Index = 1
    Me.MenuItem1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuOptionsAutoTweakParams})
    Me.MenuItem1.Text = "Options"
    Me.MenuItem1.Visible = False
    '
    'mnuOptionsAutoTweakParams
    '
    Me.mnuOptionsAutoTweakParams.Index = 0
    Me.mnuOptionsAutoTweakParams.Text = "Change Auto Tweak Parameters..."
    '
    'mnuHelp
    '
    Me.mnuHelp.Index = 2
    Me.mnuHelp.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuHelpAbout})
    Me.mnuHelp.Text = "Help"
    '
    'mnuHelpAbout
    '
    Me.mnuHelpAbout.Index = 0
    Me.mnuHelpAbout.Text = "About Parameter File Editor..."
    '
    'mnuDebug
    '
    Me.mnuDebug.Index = 3
    Me.mnuDebug.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuDebugSyncAll, Me.mnuDebugSyncSingle, Me.mnuDebugSyncDesc})
    Me.mnuDebug.Text = "Debug"
    Me.mnuDebug.Visible = False
    '
    'mnuDebugSyncAll
    '
    Me.mnuDebugSyncAll.Index = 0
    Me.mnuDebugSyncAll.Text = "Sync Old Tables"
    '
    'mnuDebugSyncSingle
    '
    Me.mnuDebugSyncSingle.Index = 1
    Me.mnuDebugSyncSingle.Text = "Sync Single Job..."
    '
    'mnuDebugSyncDesc
    '
    Me.mnuDebugSyncDesc.Index = 2
    Me.mnuDebugSyncDesc.Text = "Sync Param File Descriptions"
    '
    'lblParamFileInfo
    '
    Me.lblParamFileInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblParamFileInfo.Location = New System.Drawing.Point(8, 648)
    Me.lblParamFileInfo.Name = "lblParamFileInfo"
    Me.lblParamFileInfo.Size = New System.Drawing.Size(460, 36)
    Me.lblParamFileInfo.TabIndex = 7
    Me.lblParamFileInfo.Text = "Currently Loaded Template: "
    '
    'StatModErrorProvider
    '
    Me.StatModErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink
    Me.StatModErrorProvider.ContainerControl = Me
    '
    'txtIsoS
    '
    Me.txtIsoS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtIsoS.ForceNewValue = False
    Me.txtIsoS.Location = New System.Drawing.Point(364, 31)
    Me.txtIsoS.Name = "txtIsoS"
    Me.txtIsoS.Size = New System.Drawing.Size(64, 20)
    Me.txtIsoS.TabIndex = 9
    '
    'txtIsoH
    '
    Me.txtIsoH.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtIsoH.ForceNewValue = False
    Me.txtIsoH.Location = New System.Drawing.Point(100, 31)
    Me.txtIsoH.Name = "txtIsoH"
    Me.txtIsoH.Size = New System.Drawing.Size(64, 20)
    Me.txtIsoH.TabIndex = 8
    '
    'txtIsoN
    '
    Me.txtIsoN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtIsoN.ForceNewValue = False
    Me.txtIsoN.Location = New System.Drawing.Point(276, 31)
    Me.txtIsoN.Name = "txtIsoN"
    Me.txtIsoN.Size = New System.Drawing.Size(64, 20)
    Me.txtIsoN.TabIndex = 6
    '
    'txtIsoO
    '
    Me.txtIsoO.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtIsoO.ForceNewValue = False
    Me.txtIsoO.Location = New System.Drawing.Point(188, 31)
    Me.txtIsoO.Name = "txtIsoO"
    Me.txtIsoO.Size = New System.Drawing.Size(64, 20)
    Me.txtIsoO.TabIndex = 5
    '
    'txtIsoC
    '
    Me.txtIsoC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtIsoC.ForceNewValue = False
    Me.txtIsoC.Location = New System.Drawing.Point(16, 31)
    Me.txtIsoC.Name = "txtIsoC"
    Me.txtIsoC.Size = New System.Drawing.Size(64, 20)
    Me.txtIsoC.TabIndex = 4
    '
    'txtCTPep
    '
    Me.txtCTPep.BackColor = System.Drawing.SystemColors.Window
    Me.txtCTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtCTPep.ForceNewValue = False
    Me.txtCTPep.Location = New System.Drawing.Point(12, 32)
    Me.txtCTPep.Name = "txtCTPep"
    Me.txtCTPep.Size = New System.Drawing.Size(55, 20)
    Me.txtCTPep.TabIndex = 12
    '
    'txtAla
    '
    Me.txtAla.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtAla.ForceNewValue = False
    Me.txtAla.Location = New System.Drawing.Point(322, 32)
    Me.txtAla.Name = "txtAla"
    Me.txtAla.Size = New System.Drawing.Size(55, 20)
    Me.txtAla.TabIndex = 17
    '
    'txtCTProt
    '
    Me.txtCTProt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtCTProt.ForceNewValue = False
    Me.txtCTProt.Location = New System.Drawing.Point(74, 32)
    Me.txtCTProt.Name = "txtCTProt"
    Me.txtCTProt.Size = New System.Drawing.Size(55, 20)
    Me.txtCTProt.TabIndex = 13
    '
    'txtNTPep
    '
    Me.txtNTPep.BackColor = System.Drawing.SystemColors.Window
    Me.txtNTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtNTPep.ForceNewValue = False
    Me.txtNTPep.Location = New System.Drawing.Point(136, 32)
    Me.txtNTPep.Name = "txtNTPep"
    Me.txtNTPep.Size = New System.Drawing.Size(55, 20)
    Me.txtNTPep.TabIndex = 14
    '
    'txtNTProt
    '
    Me.txtNTProt.BackColor = System.Drawing.SystemColors.Window
    Me.txtNTProt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtNTProt.ForceNewValue = False
    Me.txtNTProt.Location = New System.Drawing.Point(198, 32)
    Me.txtNTProt.Name = "txtNTProt"
    Me.txtNTProt.Size = New System.Drawing.Size(55, 20)
    Me.txtNTProt.TabIndex = 15
    '
    'txtGly
    '
    Me.txtGly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtGly.ForceNewValue = False
    Me.txtGly.Location = New System.Drawing.Point(260, 32)
    Me.txtGly.Name = "txtGly"
    Me.txtGly.Size = New System.Drawing.Size(55, 20)
    Me.txtGly.TabIndex = 16
    '
    'txtSer
    '
    Me.txtSer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtSer.ForceNewValue = False
    Me.txtSer.Location = New System.Drawing.Point(384, 32)
    Me.txtSer.Name = "txtSer"
    Me.txtSer.Size = New System.Drawing.Size(55, 20)
    Me.txtSer.TabIndex = 18
    '
    'txtCys
    '
    Me.txtCys.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtCys.ForceNewValue = False
    Me.txtCys.Location = New System.Drawing.Point(198, 66)
    Me.txtCys.Name = "txtCys"
    Me.txtCys.Size = New System.Drawing.Size(55, 20)
    Me.txtCys.TabIndex = 22
    '
    'txtPro
    '
    Me.txtPro.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtPro.ForceNewValue = False
    Me.txtPro.Location = New System.Drawing.Point(12, 66)
    Me.txtPro.Name = "txtPro"
    Me.txtPro.Size = New System.Drawing.Size(55, 20)
    Me.txtPro.TabIndex = 19
    '
    'TxtLorI
    '
    Me.TxtLorI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.TxtLorI.ForceNewValue = False
    Me.TxtLorI.Location = New System.Drawing.Point(384, 66)
    Me.TxtLorI.Name = "TxtLorI"
    Me.TxtLorI.Size = New System.Drawing.Size(55, 20)
    Me.TxtLorI.TabIndex = 25
    '
    'txtThr
    '
    Me.txtThr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtThr.ForceNewValue = False
    Me.txtThr.Location = New System.Drawing.Point(136, 66)
    Me.txtThr.Name = "txtThr"
    Me.txtThr.Size = New System.Drawing.Size(55, 20)
    Me.txtThr.TabIndex = 21
    '
    'txtIle
    '
    Me.txtIle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtIle.ForceNewValue = False
    Me.txtIle.Location = New System.Drawing.Point(322, 66)
    Me.txtIle.Name = "txtIle"
    Me.txtIle.Size = New System.Drawing.Size(55, 20)
    Me.txtIle.TabIndex = 24
    '
    'txtVal
    '
    Me.txtVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtVal.ForceNewValue = False
    Me.txtVal.Location = New System.Drawing.Point(74, 66)
    Me.txtVal.Name = "txtVal"
    Me.txtVal.Size = New System.Drawing.Size(55, 20)
    Me.txtVal.TabIndex = 20
    '
    'txtLeu
    '
    Me.txtLeu.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtLeu.ForceNewValue = False
    Me.txtLeu.Location = New System.Drawing.Point(260, 66)
    Me.txtLeu.Name = "txtLeu"
    Me.txtLeu.Size = New System.Drawing.Size(55, 20)
    Me.txtLeu.TabIndex = 23
    '
    'txtNandD
    '
    Me.txtNandD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtNandD.ForceNewValue = False
    Me.txtNandD.Location = New System.Drawing.Point(136, 100)
    Me.txtNandD.Name = "txtNandD"
    Me.txtNandD.Size = New System.Drawing.Size(55, 20)
    Me.txtNandD.TabIndex = 28
    '
    'txtQandE
    '
    Me.txtQandE.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtQandE.ForceNewValue = False
    Me.txtQandE.Location = New System.Drawing.Point(384, 100)
    Me.txtQandE.Name = "txtQandE"
    Me.txtQandE.Size = New System.Drawing.Size(55, 20)
    Me.txtQandE.TabIndex = 32
    '
    'txtAsn
    '
    Me.txtAsn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtAsn.ForceNewValue = False
    Me.txtAsn.Location = New System.Drawing.Point(12, 100)
    Me.txtAsn.Name = "txtAsn"
    Me.txtAsn.Size = New System.Drawing.Size(55, 20)
    Me.txtAsn.TabIndex = 26
    '
    'txtLys
    '
    Me.txtLys.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtLys.ForceNewValue = False
    Me.txtLys.Location = New System.Drawing.Point(322, 100)
    Me.txtLys.Name = "txtLys"
    Me.txtLys.Size = New System.Drawing.Size(55, 20)
    Me.txtLys.TabIndex = 31
    '
    'txtOrn
    '
    Me.txtOrn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtOrn.ForceNewValue = False
    Me.txtOrn.Location = New System.Drawing.Point(74, 100)
    Me.txtOrn.Name = "txtOrn"
    Me.txtOrn.Size = New System.Drawing.Size(55, 20)
    Me.txtOrn.TabIndex = 27
    '
    'txtGln
    '
    Me.txtGln.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtGln.ForceNewValue = False
    Me.txtGln.Location = New System.Drawing.Point(260, 100)
    Me.txtGln.Name = "txtGln"
    Me.txtGln.Size = New System.Drawing.Size(55, 20)
    Me.txtGln.TabIndex = 30
    '
    'txtAsp
    '
    Me.txtAsp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtAsp.ForceNewValue = False
    Me.txtAsp.Location = New System.Drawing.Point(198, 100)
    Me.txtAsp.Name = "txtAsp"
    Me.txtAsp.Size = New System.Drawing.Size(55, 20)
    Me.txtAsp.TabIndex = 29
    '
    'txtArg
    '
    Me.txtArg.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtArg.ForceNewValue = False
    Me.txtArg.Location = New System.Drawing.Point(260, 134)
    Me.txtArg.Name = "txtArg"
    Me.txtArg.Size = New System.Drawing.Size(55, 20)
    Me.txtArg.TabIndex = 37
    '
    'txtTrp
    '
    Me.txtTrp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtTrp.ForceNewValue = False
    Me.txtTrp.Location = New System.Drawing.Point(384, 134)
    Me.txtTrp.Name = "txtTrp"
    Me.txtTrp.Size = New System.Drawing.Size(55, 20)
    Me.txtTrp.TabIndex = 39
    '
    'txtGlu
    '
    Me.txtGlu.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtGlu.ForceNewValue = False
    Me.txtGlu.Location = New System.Drawing.Point(12, 134)
    Me.txtGlu.Name = "txtGlu"
    Me.txtGlu.Size = New System.Drawing.Size(55, 20)
    Me.txtGlu.TabIndex = 33
    '
    'txtHis
    '
    Me.txtHis.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtHis.ForceNewValue = False
    Me.txtHis.Location = New System.Drawing.Point(136, 134)
    Me.txtHis.Name = "txtHis"
    Me.txtHis.Size = New System.Drawing.Size(55, 20)
    Me.txtHis.TabIndex = 35
    '
    'txtPhe
    '
    Me.txtPhe.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtPhe.ForceNewValue = False
    Me.txtPhe.Location = New System.Drawing.Point(198, 134)
    Me.txtPhe.Name = "txtPhe"
    Me.txtPhe.Size = New System.Drawing.Size(55, 20)
    Me.txtPhe.TabIndex = 36
    '
    'txtTyr
    '
    Me.txtTyr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtTyr.ForceNewValue = False
    Me.txtTyr.Location = New System.Drawing.Point(322, 134)
    Me.txtTyr.Name = "txtTyr"
    Me.txtTyr.Size = New System.Drawing.Size(55, 20)
    Me.txtTyr.TabIndex = 38
    '
    'txtMet
    '
    Me.txtMet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtMet.ForceNewValue = False
    Me.txtMet.Location = New System.Drawing.Point(74, 134)
    Me.txtMet.Name = "txtMet"
    Me.txtMet.Size = New System.Drawing.Size(55, 20)
    Me.txtMet.TabIndex = 34
    '
    'txtDynMod1MassDiff
    '
    Me.txtDynMod1MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtDynMod1MassDiff.ForceNewValue = False
    Me.txtDynMod1MassDiff.Location = New System.Drawing.Point(12, 74)
    Me.txtDynMod1MassDiff.Name = "txtDynMod1MassDiff"
    Me.txtDynMod1MassDiff.Size = New System.Drawing.Size(76, 20)
    Me.txtDynMod1MassDiff.TabIndex = 7
    Me.txtDynMod1MassDiff.Tag = "0"
    '
    'txtDynMod2MassDiff
    '
    Me.txtDynMod2MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtDynMod2MassDiff.ForceNewValue = False
    Me.txtDynMod2MassDiff.Location = New System.Drawing.Point(100, 74)
    Me.txtDynMod2MassDiff.Name = "txtDynMod2MassDiff"
    Me.txtDynMod2MassDiff.Size = New System.Drawing.Size(76, 20)
    Me.txtDynMod2MassDiff.TabIndex = 9
    Me.txtDynMod2MassDiff.Tag = "0"
    '
    'txtDynMod3MassDiff
    '
    Me.txtDynMod3MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtDynMod3MassDiff.ForceNewValue = False
    Me.txtDynMod3MassDiff.Location = New System.Drawing.Point(188, 74)
    Me.txtDynMod3MassDiff.Name = "txtDynMod3MassDiff"
    Me.txtDynMod3MassDiff.Size = New System.Drawing.Size(76, 20)
    Me.txtDynMod3MassDiff.TabIndex = 11
    Me.txtDynMod3MassDiff.Tag = "0"
    '
    'txtDynMod4MassDiff
    '
    Me.txtDynMod4MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtDynMod4MassDiff.ForceNewValue = False
    Me.txtDynMod4MassDiff.Location = New System.Drawing.Point(276, 74)
    Me.txtDynMod4MassDiff.Name = "txtDynMod4MassDiff"
    Me.txtDynMod4MassDiff.Size = New System.Drawing.Size(76, 20)
    Me.txtDynMod4MassDiff.TabIndex = 11
    Me.txtDynMod4MassDiff.Tag = "0"
    '
    'txtDynMod5MassDiff
    '
    Me.txtDynMod5MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.txtDynMod5MassDiff.ForceNewValue = False
    Me.txtDynMod5MassDiff.Location = New System.Drawing.Point(364, 74)
    Me.txtDynMod5MassDiff.Name = "txtDynMod5MassDiff"
    Me.txtDynMod5MassDiff.Size = New System.Drawing.Size(76, 20)
    Me.txtDynMod5MassDiff.TabIndex = 11
    Me.txtDynMod5MassDiff.Tag = "0"
    '
    'cboFragmentMassUnits
    '
    Me.cboFragmentMassUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cboFragmentMassUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cboFragmentMassUnits.Location = New System.Drawing.Point(384, 34)
    Me.cboFragmentMassUnits.Name = "cboFragmentMassUnits"
    Me.cboFragmentMassUnits.Size = New System.Drawing.Size(60, 21)
    Me.cboFragmentMassUnits.TabIndex = 2
    '
    'cboParentMassUnits
    '
    Me.cboParentMassUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
    Me.cboParentMassUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.cboParentMassUnits.Location = New System.Drawing.Point(152, 34)
    Me.cboParentMassUnits.Name = "cboParentMassUnits"
    Me.cboParentMassUnits.Size = New System.Drawing.Size(60, 21)
    Me.cboParentMassUnits.TabIndex = 2
    '
    'lblParentMassUnits
    '
    Me.lblParentMassUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblParentMassUnits.Location = New System.Drawing.Point(151, 20)
    Me.lblParentMassUnits.Name = "lblParentMassUnits"
    Me.lblParentMassUnits.Size = New System.Drawing.Size(48, 20)
    Me.lblParentMassUnits.TabIndex = 2
    Me.lblParentMassUnits.Text = "Units"
    '
    'lblFragmentMassUnits
    '
    Me.lblFragmentMassUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lblFragmentMassUnits.Location = New System.Drawing.Point(383, 20)
    Me.lblFragmentMassUnits.Name = "lblFragmentMassUnits"
    Me.lblFragmentMassUnits.Size = New System.Drawing.Size(48, 20)
    Me.lblFragmentMassUnits.TabIndex = 2
    Me.lblFragmentMassUnits.Text = "Units"
    '
    'frmMainGUI
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
    Me.ClientSize = New System.Drawing.Size(474, 710)
    Me.Controls.Add(Me.lblParamFileInfo)
    Me.Controls.Add(Me.tcMain)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.MaximumSize = New System.Drawing.Size(482, 744)
    Me.Menu = Me.mnuMain
    Me.MinimumSize = New System.Drawing.Size(482, 744)
    Me.Name = "frmMainGUI"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "Sequest Parameter File Editor"
    Me.tcMain.ResumeLayout(False)
    Me.tabBasic.ResumeLayout(False)
    Me.gbxIsoMods.ResumeLayout(False)
    Me.gbxIsoMods.PerformLayout()
    Me.gbxStaticMods.ResumeLayout(False)
    Me.gbxStaticMods.PerformLayout()
    Me.gbxDesc.ResumeLayout(False)
    Me.gbxDesc.PerformLayout()
    Me.gbxSearch.ResumeLayout(False)
    Me.gbxSearch.PerformLayout()
    Me.gbxDynMods.ResumeLayout(False)
    Me.gbxDynMods.PerformLayout()
    Me.tabAdvanced.ResumeLayout(False)
    Me.gbxIonWeighting.ResumeLayout(False)
    Me.gbxIonWeighting.PerformLayout()
    Me.gbxMiscParams.ResumeLayout(False)
    Me.gbxMiscParams.PerformLayout()
    Me.gbxToleranceValues.ResumeLayout(False)
    Me.gbxToleranceValues.PerformLayout()
    Me.gbxSwitches.ResumeLayout(False)
    CType(Me.StatModErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub

  <System.STAThread()> Public Shared Sub Main()
    System.Windows.Forms.Application.EnableVisualStyles()
    System.Windows.Forms.Application.DoEvents()
    System.Windows.Forms.Application.Run(New frmMainGUI)  ' replace frmDecode by the name of your form!!!
  End Sub

#End Region

  Public Shared mySettings As clsSettings

  Private basicTemplate As IBasicParams
  Private advTemplate As IAdvancedParams
  Public newParams As clsParams
  Private newBasic As IBasicParams
  Private newAdv As IAdvancedParams
  Private m_SettingsFileName As String = "ParamFileEditorSettings.xml"
  Const DEF_TEMPLATE_LABEL_TEXT As String = "Currently Loaded Template: "
  Private embeddedResource As New clsAccessEmbeddedRsrc
  Private m_UseAutoTweak As Boolean

  Private Sub RefreshTabs(ByVal frm As frmMainGUI, ByVal ParamsClass As clsParams)
    SetupBasicTab(frm, ParamsClass)
    SetupAdvancedTab(frm, ParamsClass)
    Me.RetweakMasses()
  End Sub

  Private Sub frmMainGUI_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

    mySettings = New clsSettings
    mySettings.LoadSettings(m_SettingsFileName)
    Me.newParams = New clsParams
    Me.m_DMSUpload = New clsDMSParamUpload(mySettings)


    With Me.newParams
      .FileName = Mid(clsMainProcess.TemplateFileName, InStrRev(clsMainProcess.TemplateFileName, "\") + 1)
      .LoadTemplate(mySettings.TemplateFileName)
    End With

    RefreshTabs(Me, newParams)

    'AddBasicTabHandlers()
    'AddAdvTabHandlers()

  End Sub
  Private Sub SetupBasicTab(ByVal frm As frmMainGUI, ByVal bt As IBasicParams)
    Me.RemoveBasicTabHandlers()
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
      .Items.Add("mmu")
      .Items.Add("ppm")
      .EndUpdate()
    End With

    Dim enz As New clsEnzymeDetails

    With frm.cboEnzymeSelect
      .BeginUpdate()
      .Items.Clear()
      For Each enz In frm.newParams.EnzymeList
        .Items.Add(enz.EnzymeID & " - " & enz.EnzymeName & " [" & enz.EnzymeCleavePoints & "]")
      Next
      .EndUpdate()
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

    Dim counter As Integer = 0
    With frm.cboMissedCleavages
      .BeginUpdate()
      .Items.Clear()
      For counter = 0 To 5
        .Items.Add(counter.ToString)
      Next
      .EndUpdate()
    End With

    With bt
      'Name and description info
      frm.lblParamFileInfo.Text = DEF_TEMPLATE_LABEL_TEXT & .FileName
      frm.txtDescription.Text = .Description

      'Search settings
      frm.cboParentMassType.SelectedIndex = CType(.ParentMassType, Integer)
      frm.cboFragmentMassType.SelectedIndex = CType(.FragmentMassType, Integer)
      frm.cboEnzymeSelect.SelectedIndex = .SelectedEnzymeIndex()
      frm.cboCleavagePosition.SelectedValue = .SelectedEnzymeCleavagePosition
      frm.cboMissedCleavages.SelectedIndex = .MaximumNumberMissedCleavages
      frm.txtPartialSeq.Text = .PartialSequenceToMatch

      'Dynamic Mods
      frm.txtDynMod1List.Text = .DynamicMods.Dyn_Mod_n_AAList(1)
      frm.txtDynMod2List.Text = .DynamicMods.Dyn_Mod_n_AAList(2)
      frm.txtDynMod3List.Text = .DynamicMods.Dyn_Mod_n_AAList(3)
      frm.txtDynMod4List.Text = .DynamicMods.Dyn_Mod_n_AAList(4)
      frm.txtDynMod5List.Text = .DynamicMods.Dyn_Mod_n_AAList(5)
      frm.txtDynMod1MassDiff.Text = Format(.DynamicMods.Dyn_Mod_n_MassDiff(1), "0.0000").ToString
      frm.txtDynMod2MassDiff.Text = Format(.DynamicMods.Dyn_Mod_n_MassDiff(2), "0.0000").ToString
      frm.txtDynMod3MassDiff.Text = Format(.DynamicMods.Dyn_Mod_n_MassDiff(3), "0.0000").ToString
      frm.txtDynMod4MassDiff.Text = Format(.DynamicMods.Dyn_Mod_n_MassDiff(4), "0.0000").ToString
      frm.txtDynMod5MassDiff.Text = Format(.DynamicMods.Dyn_Mod_n_MassDiff(5), "0.0000").ToString

      'Static Mods
      frm.txtCTPep.Text = Format(.StaticModificationsList.CtermPeptide, "0.0000").ToString
      frm.txtCTProt.Text = Format(.StaticModificationsList.CtermProtein, "0.0000").ToString
      frm.txtNTPep.Text = Format(.StaticModificationsList.NtermPeptide, "0.0000").ToString
      frm.txtNTProt.Text = Format(.StaticModificationsList.NtermProtein, "0.0000").ToString
      frm.txtGly.Text = Format(.StaticModificationsList.G_Glycine, "0.0000").ToString
      frm.txtAla.Text = Format(.StaticModificationsList.A_Alanine, "0.0000").ToString
      frm.txtSer.Text = Format(.StaticModificationsList.S_Serine, "0.0000").ToString

      frm.txtPro.Text = Format(.StaticModificationsList.P_Proline, "0.0000").ToString
      frm.txtVal.Text = Format(.StaticModificationsList.V_Valine, "0.0000").ToString
      frm.txtThr.Text = Format(.StaticModificationsList.T_Threonine, "0.0000").ToString
      frm.txtCys.Text = Format(.StaticModificationsList.C_Cysteine, "0.0000").ToString
      frm.txtLeu.Text = Format(.StaticModificationsList.L_Leucine, "0.0000").ToString
      frm.txtIle.Text = Format(.StaticModificationsList.I_Isoleucine, "0.0000").ToString
      frm.TxtLorI.Text = Format(.StaticModificationsList.X_LorI, "0.0000").ToString

      frm.txtAsn.Text = Format(.StaticModificationsList.N_Asparagine, "0.0000").ToString
      frm.txtOrn.Text = Format(.StaticModificationsList.O_Ornithine, "0.0000").ToString
      frm.txtNandD.Text = Format(.StaticModificationsList.B_avg_NandD, "0.0000").ToString
      frm.txtAsp.Text = Format(.StaticModificationsList.D_Aspartic_Acid, "0.0000").ToString
      frm.txtGln.Text = Format(.StaticModificationsList.Q_Glutamine, "0.0000").ToString
      frm.txtLys.Text = Format(.StaticModificationsList.K_Lysine, "0.0000").ToString
      frm.txtQandE.Text = Format(.StaticModificationsList.Z_avg_QandE, "0.0000").ToString

      frm.txtGlu.Text = Format(.StaticModificationsList.E_Glutamic_Acid, "0.0000").ToString
      frm.txtMet.Text = Format(.StaticModificationsList.M_Methionine, "0.0000").ToString
      frm.txtHis.Text = Format(.StaticModificationsList.H_Histidine, "0.0000").ToString
      frm.txtPhe.Text = Format(.StaticModificationsList.F_Phenylalanine, "0.0000").ToString
      frm.txtArg.Text = Format(.StaticModificationsList.R_Arginine, "0.0000").ToString
      frm.txtTyr.Text = Format(.StaticModificationsList.Y_Tyrosine, "0.0000").ToString
      frm.txtTrp.Text = Format(.StaticModificationsList.W_Tryptophan, "0.0000").ToString

      frm.txtIsoC.Text = Format(.IsotopicModificationsList.Iso_C, "0.0000").ToString
      frm.txtIsoH.Text = Format(.IsotopicModificationsList.Iso_H, "0.0000").ToString
      frm.txtIsoO.Text = Format(.IsotopicModificationsList.Iso_O, "0.0000").ToString
      frm.txtIsoN.Text = Format(.IsotopicModificationsList.Iso_N, "0.0000").ToString
      frm.txtIsoS.Text = Format(.IsotopicModificationsList.Iso_S, "0.0000").ToString

    End With

    'TODO Change code to check connection status/availability and set accordingly
    frm.chkAutoTweak.Checked = True

    Me.AddBasicTabHandlers()

  End Sub
  Private Sub SetupAdvancedTab(ByVal frm As frmMainGUI, ByVal at As IAdvancedParams)
    Me.RemoveAdvTabHandlers()
    'Load Combo Box
    With frm.cboNucReadingFrame.Items
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
      frm.chkUseAIons.Checked = .IonSeries.Use_a_Ions
      frm.chkUseBIons.Checked = .IonSeries.Use_b_Ions
      frm.chkUseYIons.Checked = .IonSeries.Use_y_Ions
      frm.cboParentMassUnits.SelectedIndex = CType(.PeptideMassUnits, Integer)
      frm.cboFragmentMassUnits.SelectedIndex = CType(.FragmentMassUnits, Integer)

      frm.chkCreateOutputFiles.Checked = .CreateOutputFiles
      frm.chkShowFragmentIons.Checked = .ShowFragmentIons
      frm.chkRemovePrecursorPeaks.Checked = .RemovePrecursorPeak
      frm.chkPrintDupRefs.Checked = .PrintDuplicateReferences
      frm.chkResiduesInUpperCase.Checked = .AminoAcidsAllUpperCase

      'Setup Search Tolerances
      frm.txtPepMassTol.Text = Format(.PeptideMassTolerance, "0.0000").ToString
      frm.txtFragMassTol.Text = Format(.FragmentIonTolerance, "0.0000").ToString
      frm.txtPeakMatchingTol.Text = Format(.MatchedPeakMassTolerance, "0.0000").ToString
      frm.txtIonCutoff.Text = Format(.IonCutoffPercentage, "0.0000").ToString
      frm.txtMinProtMass.Text = Format(.MinimumProteinMassToSearch, "0.0000").ToString
      frm.txtMaxProtMass.Text = Format(.MaximumProteinMassToSearch, "0.0000").ToString
      frm.cboNucReadingFrame.SelectedIndex = .SelectedNucReadingFrame
      frm.txtNumResults.Text = CInt(.NumberOfResultsToProcess)


      'Setup Misc Options
      frm.txtNumOutputLines.Text = Format(.NumberOfOutputLines, "0").ToString
      frm.txtNumDescLines.Text = Format(.NumberOfDescriptionLines, "0").ToString
      frm.txtMatchPeakCount.Text = Format(.NumberOfDetectedPeaksToMatch, "0").ToString
      frm.txtMatchPeakCountErrors.Text = Format(.NumberOfAllowedDetectedPeakErrors, "0").ToString
      frm.txtMaxAAPerDynMod.Text = Format(.MaximumNumAAPerDynMod, "0").ToString
      frm.txtMaxDiffPerPeptide.Text = .MaximumDifferentialPerPeptide

      'Setup Ion Weighting
      frm.txtAWeight.Text = Format(.IonSeries.a_Ion_Weighting, "0.0").ToString
      frm.txtBWeight.Text = Format(.IonSeries.b_Ion_Weighting, "0.0").ToString
      frm.txtCWeight.Text = Format(.IonSeries.c_Ion_Weighting, "0.0").ToString
      frm.txtDWeight.Text = Format(.IonSeries.d_Ion_Weighting, "0.0").ToString
      frm.txtVWeight.Text = Format(.IonSeries.v_Ion_Weighting, "0.0").ToString
      frm.txtWWeight.Text = Format(.IonSeries.w_Ion_Weighting, "0.0").ToString
      frm.txtXWeight.Text = Format(.IonSeries.x_Ion_Weighting, "0.0").ToString
      frm.txtYWeight.Text = Format(.IonSeries.y_Ion_Weighting, "0.0").ToString
      frm.txtZWeight.Text = Format(.IonSeries.z_Ion_Weighting, "0.0").ToString
    End With
    Me.AddAdvTabHandlers()

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

    AddHandler txtDynMod1MassDiff.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtDynMod2MassDiff.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtDynMod3MassDiff.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtDynMod4MassDiff.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtDynMod5MassDiff.KeyDown, AddressOf numericTextBox_EscapeButton

    AddHandler txtDynMod1MassDiff.Validated, AddressOf txtDynMod1MassDiff_Validated
    AddHandler txtDynMod2MassDiff.Validated, AddressOf txtDynMod2MassDiff_Validated
    AddHandler txtDynMod3MassDiff.Validated, AddressOf txtDynMod3MassDiff_Validated
    AddHandler txtDynMod4MassDiff.Validated, AddressOf txtDynMod4MassDiff_Validated
    AddHandler txtDynMod5MassDiff.Validated, AddressOf txtDynMod5MassDiff_Validated

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


    AddHandler txtCTPep.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtCTProt.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtNTPep.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtNTProt.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtGly.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtAla.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtSer.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtPro.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtVal.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtThr.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtCys.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtLeu.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtIle.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler TxtLorI.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtAsn.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtOrn.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtNandD.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtAsp.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtGln.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtLys.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtQandE.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtGlu.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtMet.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtHis.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtPhe.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtArg.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtTyr.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtTrp.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtIsoC.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtIsoH.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtIsoO.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtIsoN.KeyDown, AddressOf numericTextBox_EscapeButton
    AddHandler txtIsoS.KeyDown, AddressOf numericTextBox_EscapeButton


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

    RemoveHandler txtDynMod1MassDiff.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtDynMod2MassDiff.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtDynMod3MassDiff.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtDynMod4MassDiff.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtDynMod5MassDiff.KeyDown, AddressOf numericTextBox_EscapeButton

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


    RemoveHandler txtCTPep.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtCTProt.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtNTPep.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtNTProt.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtGly.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtAla.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtSer.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtPro.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtVal.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtThr.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtCys.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtLeu.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtIle.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler TxtLorI.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtAsn.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtOrn.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtNandD.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtAsp.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtGln.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtLys.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtQandE.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtGlu.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtMet.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtHis.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtPhe.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtArg.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtTyr.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtTrp.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtIsoC.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtIsoH.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtIsoO.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtIsoN.KeyDown, AddressOf numericTextBox_EscapeButton
    RemoveHandler txtIsoS.KeyDown, AddressOf numericTextBox_EscapeButton


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

  Private Sub LoadParamsetFromDMS(ByVal ParamSetID As Integer)
    If m_clsParamsFromDMS Is Nothing Then
      m_clsParamsFromDMS = LoadDMSParamsClass(mySettings)
    End If
    newParams = m_clsParamsFromDMS.ReadParamsFromDMS(ParamSetID)

    Dim iso As New clsDeconvolveIsoMods(mySettings.DMS_ConnectionString)
    newParams = iso.DeriveIsoMods(newParams)

    RefreshTabs(Me, newParams)
  End Sub

  Private Sub LoadParamsetFromFile(ByVal FilePath As String)
    newParams.LoadTemplate(FilePath)
    Dim iso As New clsDeconvolveIsoMods(mySettings.DMS_ConnectionString)
    newParams = iso.DeriveIsoMods(newParams)
    RefreshTabs(Me, newParams)
  End Sub

  Private Function SetupAutoTweak(ByVal sender As System.Object) As Boolean
    Dim successCode As Boolean
    Try
      If Me.m_clsMassTweaker Is Nothing Then
        Me.m_clsMassTweaker = New clsMassTweaker(mySettings)
        successCode = True
        Me.StatModErrorProvider.SetError(sender, "")
      End If
    Catch ex As Exception
      Me.StatModErrorProvider.SetError(sender, "Could not connect to DMS to retrieve Global Mod Info")
      successCode = False
    End Try

    Return successCode

  End Function

  Private Function LoadDMSParamsClass(ByVal Settings As clsSettings) As clsParamsFromDMS
    Dim dms As New clsParamsFromDMS(Settings.DMS_ConnectionString)
    Return dms
  End Function
  Private Function LoadDMSParamUploadClass(ByVal Settings As clsSettings) As clsDMSParamUpload
    Dim dms As New clsDMSParamUpload(Settings)
    Return dms
  End Function

  Public Sub LoadDMSParamsFromID(ByVal ParamSetID As Integer)
    LoadParamsetFromDMS(ParamSetID)
  End Sub

  Public Sub LoadParamsFromFile(ByVal FilePath As String)
    LoadParamsetFromFile(FilePath)
  End Sub

  Private Sub chkAutoTweak_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoTweak.CheckedChanged
    Dim success As Boolean
    If Me.chkAutoTweak.CheckState = CheckState.Checked Then
      success = SetupAutoTweak(sender)
      Me.m_UseAutoTweak = True
      Me.cmdReTweak.Enabled = True
    Else
      Me.m_UseAutoTweak = False
      Me.cmdReTweak.Enabled = False
    End If

  End Sub

#Region " [Basic] Name and Description Handlers "

  Private Sub txtDescription_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.Description = Me.txtDescription.Text
  End Sub

#End Region

#Region " [Basic] Search Settings Handlers "

  Private Sub cboParentMassType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.ParentMassType = CType(Me.cboParentMassType.SelectedIndex, IBasicParams.MassTypeList)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub
  Private Sub cboFragmentMassType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.FragmentMassType = CType(Me.cboFragmentMassType.SelectedIndex, IBasicParams.MassTypeList)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub
  Private Sub cboFragmentMassUnits_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.FragmentMassUnits = CType(Me.cboFragmentMassUnits.SelectedIndex, IAdvancedParams.MassUnitList)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub
  Private Sub cboParentMassUnits_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.PeptideMassUnits = CType(Me.cboParentMassUnits.SelectedIndex, IAdvancedParams.MassUnitList)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub
  Private Sub cboEnzymeSelect_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Dim tmpIndex As Integer
    tmpIndex = Me.cboEnzymeSelect.SelectedIndex
    newParams.SelectedEnzymeIndex = tmpIndex
    newParams.SelectedEnzymeDetails = newParams.RetrieveEnzymeDetails(tmpIndex)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)

    If tmpIndex = 0 Then
      With Me.cboCleavagePosition
        .DisplayMember = "DisplayName"
        .ValueMember = "Value"
        .BeginUpdate()
        .Items.Clear()
        .Items.Add(New ComboBoxContents("No Enzyme", "0"))
        .EndUpdate()
      End With
      Me.cboCleavagePosition.Text = "No Enzyme"
      Me.cboCleavagePosition.Enabled = False
    Else
      With Me.cboCleavagePosition
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

      Dim cItem As ComboBoxContents
      For Each cItem In Me.cboCleavagePosition.Items
        If CInt(cItem.Value) = Me.newParams.SelectedEnzymeCleavagePosition Then
          Me.cboCleavagePosition.Text = cItem.DisplayName
        End If
      Next

      Me.cboCleavagePosition.Enabled = True
    End If

    Debug.WriteLine("")
  End Sub
  Private Sub cboMissedCleavages_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.MaximumNumberMissedCleavages = Me.cboMissedCleavages.SelectedIndex
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub
  Private Sub cboCleavagePosition_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCleavagePosition.SelectedIndexChanged
    Dim cbc As ComboBoxContents = DirectCast(Me.cboCleavagePosition.SelectedItem, ComboBoxContents)
    If Not cbc Is Nothing Then
      newParams.SelectedEnzymeCleavagePosition = CInt(cbc.Value)
    Else
      newParams.SelectedEnzymeCleavagePosition = 0
    End If
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub txtPartialSeq_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.PartialSequenceToMatch = Me.txtPartialSeq.Text
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

#End Region

#Region " [Basic] Dynamic Modification Handlers "

  Private Sub txtDynMod1List_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    If Me.txtDynMod1List.Text <> "C" Then
      Me.StatModErrorProvider.SetError(sender, "")
      Me.txtDynMod1List.Text = Me.txtDynMod1List.Text.ToUpper
      If CSng(Me.txtDynMod1MassDiff.Text) <> 0.0 Then
        newParams.DynamicMods.Dyn_Mod_n_AAList(1) = Me.txtDynMod1List.Text
      End If
    End If
  End Sub
  Private Sub txtDynMod2List_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    If Me.txtDynMod2List.Text <> "C" Then
      Me.StatModErrorProvider.SetError(sender, "")
      Me.txtDynMod2List.Text = Me.txtDynMod2List.Text.ToUpper
      If CSng(Me.txtDynMod2MassDiff.Text) <> 0.0 Then
        newParams.DynamicMods.Dyn_Mod_n_AAList(2) = Me.txtDynMod2List.Text
      End If
    End If
  End Sub
  Private Sub txtDynMod3List_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    If Me.txtDynMod3List.Text <> "C" Then
      Me.StatModErrorProvider.SetError(sender, "")
      Me.txtDynMod3List.Text = Me.txtDynMod3List.Text.ToUpper
      If CSng(Me.txtDynMod3MassDiff.Text) <> 0.0 Then
        newParams.DynamicMods.Dyn_Mod_n_AAList(3) = Me.txtDynMod3List.Text
      End If
    End If
  End Sub
  Private Sub txtDynMod4List_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    If Me.txtDynMod4List.Text <> "C" Then
      Me.StatModErrorProvider.SetError(sender, "")
      Me.txtDynMod3List.Text = Me.txtDynMod3List.Text.ToUpper
      If CSng(Me.txtDynMod4MassDiff.Text) <> 0.0 Then
        newParams.DynamicMods.Dyn_Mod_n_AAList(4) = Me.txtDynMod4List.Text
      End If
    End If
  End Sub
  Private Sub txtDynMod5List_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    If Me.txtDynMod5List.Text <> "C" Then
      Me.StatModErrorProvider.SetError(sender, "")
      Me.txtDynMod5List.Text = Me.txtDynMod3List.Text.ToUpper
      If CSng(Me.txtDynMod5MassDiff.Text) <> 0.0 Then
        newParams.DynamicMods.Dyn_Mod_n_AAList(5) = Me.txtDynMod5List.Text
      End If
    End If
  End Sub
  Private Sub txtDynMod1MassDiff_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    If CSng(Me.txtDynMod1MassDiff.Text) <> 0.0 Then
      newParams.DynamicMods.Dyn_Mod_n_AAList(1) = Me.txtDynMod1List.Text
    End If
    newParams.DynamicMods.Dyn_Mod_n_MassDiff(1) = CSng(Me.txtDynMod1MassDiff.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub
  Private Sub txtDynMod2MassDiff_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    If CSng(Me.txtDynMod2MassDiff.Text) <> 0.0 Then
      newParams.DynamicMods.Dyn_Mod_n_AAList(2) = Me.txtDynMod2List.Text
    End If
    newParams.DynamicMods.Dyn_Mod_n_MassDiff(2) = CSng(Me.txtDynMod2MassDiff.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub
  Private Sub txtDynMod3MassDiff_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    If CSng(Me.txtDynMod3MassDiff.Text) <> 0.0 Then
      newParams.DynamicMods.Dyn_Mod_n_AAList(3) = Me.txtDynMod3List.Text
    End If
    newParams.DynamicMods.Dyn_Mod_n_MassDiff(3) = CSng(Me.txtDynMod3MassDiff.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub
  Private Sub txtDynMod4MassDiff_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    If CSng(Me.txtDynMod4MassDiff.Text) <> 0.0 Then
      newParams.DynamicMods.Dyn_Mod_n_AAList(4) = Me.txtDynMod3List.Text
    End If
    newParams.DynamicMods.Dyn_Mod_n_MassDiff(4) = CSng(Me.txtDynMod4MassDiff.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub
  Private Sub txtDynMod5MassDiff_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    If CSng(Me.txtDynMod3MassDiff.Text) <> 0.0 Then
      newParams.DynamicMods.Dyn_Mod_n_AAList(5) = Me.txtDynMod3List.Text
    End If
    newParams.DynamicMods.Dyn_Mod_n_MassDiff(5) = CSng(Me.txtDynMod5MassDiff.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

#End Region

#Region " [Basic] Static Modification Handlers "

  Private Sub txtCTPep_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.CtermPeptide = CSng(Me.txtCTPep.Text)
    Catch
      Me.txtCTPep.Text = "0.0"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub


  Private Sub txtCTProt_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.CtermProtein = CSng(Me.txtCTProt.Text)
    Catch
      Me.txtCTProt.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtNTPep_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.NtermPeptide = CSng(Me.txtNTPep.Text)
    Catch
      Me.txtNTPep.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtNTProt_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.NtermProtein = CSng(Me.txtNTProt.Text)
    Catch
      Me.txtNTProt.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtGly_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.G_Glycine = CSng(Me.txtGly.Text)
    Catch
      Me.txtGly.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtAla_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.A_Alanine = CSng(Me.txtAla.Text)
    Catch
      Me.txtAla.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtSer_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.S_Serine = CSng(Me.txtSer.Text)
    Catch
      Me.txtSer.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtPro_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.P_Proline = CSng(Me.txtPro.Text)
    Catch
      Me.txtPro.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtVal_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.V_Valine = CSng(Me.txtVal.Text)
    Catch
      Me.txtVal.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtThr_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.T_Threonine = CSng(Me.txtThr.Text)
    Catch
      Me.txtThr.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtCys_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.C_Cysteine = CSng(Me.txtCys.Text)
    Catch
      Me.txtCys.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtLeu_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.L_Leucine = CSng(Me.txtLeu.Text)
    Catch
      Me.txtLeu.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtIle_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.I_Isoleucine = CSng(Me.txtIle.Text)
    Catch
      Me.txtIle.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub TxtLorI_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.X_LorI = CSng(Me.TxtLorI.Text)
    Catch
      Me.TxtLorI.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtAsn_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.N_Asparagine = CSng(Me.txtAsn.Text)
    Catch
      Me.txtAsn.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtOrn_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.O_Ornithine = CSng(Me.txtOrn.Text)
    Catch
      Me.txtOrn.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtNandD_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.B_avg_NandD = CSng(Me.txtNandD.Text)
    Catch
      Me.txtNandD.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtAsp_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.D_Aspartic_Acid = CSng(Me.txtAsp.Text)
    Catch
      Me.txtAsp.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtGln_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.Q_Glutamine = CSng(Me.txtGln.Text)
    Catch
      Me.txtGln.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtLys_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.K_Lysine = CSng(Me.txtLys.Text)
    Catch
      Me.txtLys.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtQandE_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.Z_avg_QandE = CSng(Me.txtQandE.Text)
    Catch
      Me.txtQandE.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtGlu_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.E_Glutamic_Acid = CSng(Me.txtGlu.Text)
    Catch
      Me.txtGlu.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtMet_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.M_Methionine = CSng(Me.txtMet.Text)
    Catch
      Me.txtMet.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtHis_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.H_Histidine = CSng(Me.txtHis.Text)
    Catch
      Me.txtHis.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtPhe_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.F_Phenylalanine = CSng(Me.txtPhe.Text)
    Catch
      Me.txtPhe.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtArg_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.R_Arginine = CSng(Me.txtArg.Text)
    Catch
      Me.txtArg.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtTyr_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.Y_Tyrosine = CSng(Me.txtTyr.Text)
    Catch
      Me.txtTyr.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub
  Private Sub txtTrp_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      newParams.StaticModificationsList.W_Tryptophan = CSng(Me.txtTrp.Text)
    Catch
      Me.txtTrp.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub

#End Region

#Region " [Basic] Isotopic Modification Handlers "
  Private Sub txtIsoC_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      'newParams.StaticModificationsList.W_Tryptophan = CSng(Me.txtTrp.Text)
      newParams.IsotopicMods.Iso_C = CSng(sender.text)
    Catch
      sender.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub

  Private Sub txtIsoH_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      'newParams.StaticModificationsList.W_Tryptophan = CSng(Me.txtTrp.Text)
      newParams.IsotopicMods.Iso_H = CSng(sender.text)
    Catch
      sender.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub

  Private Sub txtIsoO_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      'newParams.StaticModificationsList.W_Tryptophan = CSng(Me.txtTrp.Text)
      newParams.IsotopicMods.Iso_O = CSng(sender.text)
    Catch
      sender.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub

  Private Sub txtIsoN_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      'newParams.StaticModificationsList.W_Tryptophan = CSng(Me.txtTrp.Text)
      newParams.IsotopicMods.Iso_N = CSng(sender.text)
    Catch
      sender.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub

  Private Sub txtIsoS_Validated(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Try
      'newParams.StaticModificationsList.W_Tryptophan = CSng(Me.txtTrp.Text)
      newParams.IsotopicMods.Iso_S = CSng(sender.text)
    Catch
      sender.Text = "0.0000"
      Me.txtDescription.Text = Me.UpdateDescription(newParams)
    End Try
  End Sub


#End Region

#Region " [Advanced] Searching Tolerances "

  Private Sub txtPepMassTol_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.PeptideMassTolerance = CSng(Me.txtPepMassTol.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub
  Private Sub txtFragMassTol_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.FragmentIonTolerance = CSng(Me.txtFragMassTol.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub
  Private Sub txtPeakMatchingTol_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.MatchedPeakMassTolerance = CSng(Me.txtPeakMatchingTol.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub
  Private Sub txtIonCutoff_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.IonCutoffPercentage = CSng(Me.txtIonCutoff.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub
  Private Sub txtMinProtMass_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.MinimumProteinMassToSearch = CInt(Me.txtMinProtMass.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub
  Private Sub txtMaxProtMass_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.MaximumProteinMassToSearch = CInt(Me.txtMaxProtMass.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

#End Region

#Region " [Advanced] Miscellaneous Options "

  Private Sub txtNumOutputLines_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.NumberOfOutputLines = CInt(Me.txtNumOutputLines.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub txtNumDescLines_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.NumberOfDescriptionLines = CInt(Me.txtNumDescLines.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub
  Private Sub txtNumResults_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.NumberOfResultsToProcess = CInt(Me.txtNumResults.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub
  Private Sub txtMatchPeakCount_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.NumberOfDetectedPeaksToMatch = CInt(Me.txtMatchPeakCountErrors.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub txtMatchPeakCountErrors_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.NumberOfAllowedDetectedPeakErrors = CInt(Me.txtMatchPeakCountErrors.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub txtMaxAAPerDynMod_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.MaximumNumAAPerDynMod = CInt(Me.txtMaxAAPerDynMod.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub txtMaxDiffPerPeptide_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.MaximumNumDifferentialPerPeptide = Me.txtMaxDiffPerPeptide.Text
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub cboNucReadingFrame_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.SelectedNucReadingFrame = CType(Me.cboNucReadingFrame.SelectedIndex, Integer)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

#End Region

#Region " [Advanced] Option Checkboxes "

  Private Sub chkUseAIons_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.IonSeries.Use_a_Ions = Me.chkUseAIons.Checked
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub chkUseBIons_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.IonSeries.Use_b_Ions = Me.chkUseBIons.Checked
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub chkUseYIons_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.IonSeries.Use_y_Ions = Me.chkUseYIons.Checked
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub chkCreateOutputFiles_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.CreateOutputFiles = Me.chkCreateOutputFiles.Checked
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub chkShowFragmentIons_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.ShowFragmentIons = Me.chkShowFragmentIons.Checked
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub chkRemovePrecursorPeaks_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.RemovePrecursorPeak = Me.chkRemovePrecursorPeaks.Checked
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub chkPrintDupRefs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.PrintDuplicateReferences = Me.chkPrintDupRefs.Checked
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub chkResiduesInUpperCase_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.AminoAcidsAllUpperCase = Me.chkResiduesInUpperCase.Checked
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

#End Region

#Region " [Advanced] Ion Weighting Constants"

  Private Sub txtAWeight_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.IonSeries.a_Ion_Weighting = CSng(Me.txtAWeight.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub txtBWeight_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.IonSeries.b_Ion_Weighting = CSng(Me.txtBWeight.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub txtCWeight_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.IonSeries.c_Ion_Weighting = CSng(Me.txtCWeight.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub txtDWeight_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.IonSeries.d_Ion_Weighting = CSng(Me.txtDWeight.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub txtVWeight_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.IonSeries.v_Ion_Weighting = CSng(Me.txtVWeight.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub txtWWeight_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.IonSeries.w_Ion_Weighting = CSng(Me.txtWWeight.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub txtXWeight_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.IonSeries.x_Ion_Weighting = CSng(Me.txtXWeight.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub txtYWeight_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.IonSeries.y_Ion_Weighting = CSng(Me.txtYWeight.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

  Private Sub txtZWeight_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)
    newParams.IonSeries.z_Ion_Weighting = CSng(Me.txtZWeight.Text)
    Me.txtDescription.Text = Me.UpdateDescription(newParams)
  End Sub

#End Region

#Region " Menu Handlers "
  Private Sub mnuFileSaveBW2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSaveBW2.Click
    Dim FileOutput As New clsWriteOutput
    Dim newFilePath As String = ""

    Dim SaveDialog As New SaveFileDialog

    With SaveDialog
      .Title = "Save Sequest/BioWorks v2.0 Parameter File"
      .FileName = newParams.FileName
    End With

    If SaveDialog.ShowDialog = DialogResult.OK Then
      newFilePath = SaveDialog.FileName
    End If
    Call FileOutput.WriteOutputFile(newParams, newFilePath, clsParams.ParamFileTypes.BioWorks_20)

    MsgBox("Param File: " & SaveDialog.FileName & " written successfully")
  End Sub
  Private Sub mnuFileSaveBW3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSaveBW3.Click
    Dim FileOutput As New clsWriteOutput
    Dim newFilePath As String = ""

    Dim SaveDialog As New SaveFileDialog

    With SaveDialog
      .Title = "Save Sequest/BioWorks v3.0 Parameter File"

    End With

    If SaveDialog.ShowDialog = DialogResult.OK Then
      newFilePath = SaveDialog.FileName
    End If
    FileOutput.WriteOutputFile(newParams, newFilePath, clsParams.ParamFileTypes.BioWorks_30)
  End Sub


  Private Sub mnuFileSaveBW32_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSaveBW32.Click
    Dim FileOutput As New clsWriteOutput
    Dim newFilePath As String = ""

    Dim SaveDialog As New SaveFileDialog

    With SaveDialog
      .Title = "Save Sequest/BioWorks v3.2 Parameter File"

    End With

    If SaveDialog.ShowDialog = DialogResult.OK Then
      newFilePath = SaveDialog.FileName
    End If
    FileOutput.WriteOutputFile(newParams, newFilePath, clsParams.ParamFileTypes.BioWorks_32)

  End Sub

  Private Sub mnuHelpAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelpAbout.Click
    Dim AboutBox As New frmAboutBox
    AboutBox.ConnectionStringInUse = mySettings.DMS_ConnectionString
    AboutBox.Show()
  End Sub

  Private Sub mnuFileLoadFromDMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileLoadFromDMS.Click
    Dim frmPicker As New frmDMSPicker(Me)
    frmPicker.MySettings = mySettings
    frmPicker.Show()

  End Sub

  Private Sub mnuFileUploadDMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileUploadDMS.Click
    'Gonna need security stuff in here to keep morons outa DMS
    'Dim DMSParams As New clsParamsFromDMS(Me.MainCode.mySettings)

    'Dim ident As System.Security.Principal.WindowsIdentity

    'ident = ident.GetCurrent

    'If m_clsParamsFromDMS Is Nothing Then
    '    m_clsParamsFromDMS = Me.LoadDMSParamUploadClass(Me.MainCode.mySettings)
    'End If
    Dim SaveFrm As New frmDMSParamNamer(Me, Me.newParams)
    SaveFrm.Show()
    'Dim success As Boolean = dmsparams.ParamSetNameExists(testName)

    'Call m_clsParamsFromDMS.WriteParamsToDMS(newParams)

  End Sub

  Private Sub mnuLoadFromFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLoadFromFile.Click
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
  End Sub

  Private Sub mnuBatchUploadDMS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuBatchUploadDMS.Click
    Dim batch As clsBatchLoadTemplates = New clsBatchLoadTemplates(Me)
    Dim openDialog As New OpenFileDialog
    Dim FileNameArray As String()
    Dim fc As System.Collections.Specialized.StringCollection
    Dim numAdded As Integer
    Dim numChanged As Integer
    Dim numSkipped As Integer
    Dim success As Boolean

    With openDialog
      .Multiselect = True
      .InitialDirectory = "\\gigasax\DMS_Parameter_Files\Sequest\"
      .Filter = "Sequest Param files (*.params)|*.params|All files (*.*)|*.*"
      .FilterIndex = 1
      .RestoreDirectory = True
    End With

    If openDialog.ShowDialog = DialogResult.OK Then
      FileNameArray = openDialog.FileNames
      fc = ConvertStringArrayToSC(FileNameArray)
      success = batch.UploadParamSetsToDMS(fc)
      numAdded = batch.NumParamSetsAdded
      numChanged = batch.NumParamSetsChanged
      numSkipped = batch.NumParamSetsSkipped
    End If

    MessageBox.Show(numAdded & " new Parameter sets added; " & numChanged & " Parameter sets changed; " & numSkipped & " Parameter sets skipped", _
        "Operation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)


  End Sub

  Private Function ConvertStringArrayToSC(ByVal stringArray As String()) As System.Collections.Specialized.StringCollection
    Dim maxCount As Integer = UBound(stringArray)
    Dim sc As New System.Collections.Specialized.StringCollection
    Dim count As Integer

    For count = 0 To maxCount
      sc.Add(stringArray(count))
    Next

    Return sc
  End Function
#End Region

  Private Sub CheckForParamFileExistence()

    Dim TemplateName As String = "sequest_N14_NE.params"

    If Not Me.embeddedResource.ResourceExists(TemplateName) Then
      Me.embeddedResource.RestoreFromEmbeddedResource(TemplateName)
    End If

  End Sub

  Private Sub CheckForSettingsFileExistence()
    If Not Me.embeddedResource.ResourceExists(Me.m_SettingsFileName) Then
      Me.embeddedResource.RestoreFromEmbeddedResource(Me.m_SettingsFileName)
    End If
  End Sub

  Private Sub numericTextbox_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
    Dim chk As String = sender.Text
    Dim t As NumericTextBox = DirectCast(sender, NumericTextBox)
    Dim tmpNewMass As Single
    Dim forceNewValue As Boolean = CBool(t.ForceNewValue)

    If IsNumeric(chk) = False Then
      e.Cancel = True
      Me.StatModErrorProvider.SetError(sender, "Not a valid number")
    ElseIf IsNumeric(chk) = True And CSng(chk) = 0.0 Then
      sender.backcolor = System.Drawing.SystemColors.Window
      Me.StatModErrorProvider.SetError(sender, "")
      t.Tag = 0

    ElseIf IsNumeric(chk) = True And CSng(chk) <> 0.0 Then
      Me.StatModErrorProvider.SetError(sender, "")
      tmpNewMass = CSng(chk)
      't.Tag = newTagValue

      If Me.m_UseAutoTweak Then
        Dim tmpModType As IMassTweaker.ModTypes = Me.GetModTypeFromControlName(sender)
        Dim tmpAtom As String = ""
        Dim tmpAA As String = ""
        Dim tmpSymbol As String = ""
        Dim tmpDesc As String = ""
        Dim tmpGMID As Integer = 0
        Dim dr As DialogResult

        If tmpModType = IMassTweaker.ModTypes.IsotopicMod Then
          tmpAtom = Me.GetAffectedIsoAtomFromControlName(sender)
        Else
          tmpAtom = "-"
        End If

        tmpNewMass = Me.m_clsMassTweaker.GetTweakedMass(tmpNewMass, tmpAtom)

        If tmpNewMass = 0.0 Or forceNewValue = True Then
          t.ForceNewValue = False
          sender.backcolor = System.Drawing.SystemColors.Window
          Dim frmNewMass As New frmGlobalModNamer

          With frmNewMass

            .MassCorrectionsTable = Me.m_clsMassTweaker.MassCorrectionsTable
            .NewModMass = CSng(chk)
            .ModType = tmpModType
            .AffectedResidues = tmpAA
            .LoadGlobalMods(CSng(chk), tmpAtom)

            dr = frmNewMass.ShowDialog
            tmpSymbol = frmNewMass.NewSymbol
            tmpNewMass = CSng(.NewModMass)
            tmpDesc = .NewDescription

            If dr = DialogResult.OK Then

              Me.m_clsMassTweaker.AddMassCorrection(tmpSymbol, tmpDesc, _
                  tmpNewMass)
              Me.m_clsMassTweaker.RefreshGlobalModsTableCache(mySettings.DMS_ConnectionString)
              Me.numericTextbox_Validating(sender, e)
            ElseIf dr = DialogResult.Yes Then   'Use existing
              sender.text = tmpNewMass
              Me.numericTextbox_Validating(sender, e)
              Exit Sub
            ElseIf dr = DialogResult.Cancel Then
              Me.StatModErrorProvider.SetError(sender, "You must either choose an existing global mod, or define a new one!")
              e.Cancel = True
            End If
          End With
        Else
          tmpSymbol = Me.m_clsMassTweaker.TweakedSymbol
          tmpDesc = Me.m_clsMassTweaker.TweakedDescription
          tmpGMID = Me.m_clsMassTweaker.TweakedModID
          Me.tooltipProvider.SetToolTip(sender, tmpSymbol & ": " & tmpDesc)
          sender.tag = tmpGMID
          sender.backcolor = System.Drawing.SystemColors.InactiveCaptionText
          Me.StatModErrorProvider.SetError(sender, "")

        End If
      End If

    End If
    sender.Text = Format(CDbl(tmpNewMass), "0.0000")


  End Sub

  Private Sub AATextbox_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
    Dim chk As String = sender.Text
    If IsValidAAString(chk) = False Then
      e.Cancel = True
      Me.StatModErrorProvider.SetError(sender, "Not a valid Amino Acid")
    Else
      'Me.StatModErrorProvider.SetError(sender, "")
      'sender.text = chk.ToUpper
    End If
  End Sub

  Private Sub numericTextBox_EscapeButton(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
    Dim t As NumericTextBox = DirectCast(sender, NumericTextBox)
    If e.KeyCode = Keys.Escape Then
      t.Text = "0.0000"
      Me.numericTextbox_Validating(sender, Nothing)
    ElseIf e.KeyCode = Keys.Return Or e.KeyCode = Keys.Enter Then
      t.ForceNewValue = True
      Me.numericTextbox_Validating(t, Nothing)
    End If
  End Sub


  Private Function IsValidAAString(ByVal AAString As String) As Boolean
    Dim counter As Integer
    Dim chk As String
    Dim valid As Boolean

    For counter = 1 To Len(AAString)
      chk = Mid(AAString, counter, 1)
      If IsAminoAcid(chk) Then
        valid = True
      Else
        valid = False
        Return valid
      End If
    Next
    Return valid
  End Function

  Private Function IsAminoAcid(ByVal AA As String) As Boolean
    'If InStr("<>[]", AA.ToUpper) Then
    '  Return True
    'Else
    If Asc(AA.ToUpper) < Asc("A") Or Asc(AA.ToUpper) > Asc("Z") Then
      Return False
    ElseIf InStr("JOU", AA.ToUpper) Then
      Return False
    Else
      Return True
    End If
  End Function

  Private Function GetAffectedResidueFromControlName(ByVal sender As System.Object) As String
    Dim tmpTB As System.Windows.Forms.TextBox = DirectCast(sender, System.Windows.Forms.TextBox)
    Dim TBName As String = tmpTB.Name

    Select Case TBName
      Case "txtCTPep", "txtCTProt", "txtNTPep", "txtNTProt"
        Return "-"
      Case "txtAla"
        Return "A"
      Case "txtGly"
        Return "G"
      Case "txtSer"
        Return "S"
      Case "txtCys"
        Return "C"
      Case "txtPro"
        Return "P"
      Case "txtLorI"
        Return "X"
      Case "txtThr"
        Return "T"
      Case "txtIle"
        Return "I"
      Case "txtVal"
        Return "V"
      Case "txtLeu"
        Return "L"
      Case "txtNandD"
        Return "B"
      Case "txtQandE"
        Return "Z"
      Case "txtAsn"
        Return "N"
      Case "txtLys"
        Return "K"
      Case "txtOrn"
        Return "O"
      Case "txtGln"
        Return "Q"
      Case "txtAsp"
        Return "D"
      Case "txtArg"
        Return "R"
      Case "txtTrp"
        Return "W"
      Case "txtGlu"
        Return "E"
      Case "txtHis"
        Return "H"
      Case "txtPhe"
        Return "F"
      Case "txtTyr"
        Return "Y"
      Case "txtMet"
        Return "M"
      Case "txtDynMod1MassDiff"
        Return Me.txtDynMod1List.Text
      Case "txtDynMod2MassDiff"
        Return Me.txtDynMod2List.Text
      Case "txtDynMod3MassDiff"
        Return Me.txtDynMod3List.Text
      Case Else
        Return ""
    End Select
  End Function

  Private Function GetAffectedIsoAtomFromControlName(ByVal sender As System.Object) As String
    Dim tmpTB As System.Windows.Forms.TextBox = DirectCast(sender, System.Windows.Forms.TextBox)
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
  Private Function GetModTypeFromControlName(ByVal sender As System.Object) As IMassTweaker.ModTypes
    Dim tmpTB As System.Windows.Forms.TextBox = DirectCast(sender, System.Windows.Forms.TextBox)
    Dim TBName As String = tmpTB.Parent.Name.ToString

    Select Case TBName
      Case "gbxDynMods"
        Return IMassTweaker.ModTypes.DynamicMod
      Case "gbxIsoMods"
        Return IMassTweaker.ModTypes.IsotopicMod
      Case "gbxStaticMods"
        Return IMassTweaker.ModTypes.StaticMod
    End Select
  End Function

  Private Sub cmdReTweak_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReTweak.Click
    If Me.m_UseAutoTweak Then RetweakMasses()
  End Sub

  Private Sub RetweakMasses()
    Dim g_d As Control
    Dim t As Control


    For Each g_d In Me.tabBasic.Controls
      If g_d.GetType.ToString = "System.Windows.Forms.GroupBox" Then
        For Each t In g_d.Controls
          If IsNumeric(t.Text) And t.GetType.ToString = "System.Windows.Forms.TextBox" Then
            If CSng(t.Text) <> 0.0 Then
              numericTextbox_Validating(t, Nothing)
            Else
              t.BackColor = System.Drawing.SystemColors.Window
            End If
          End If
        Next
      End If
    Next

  End Sub

  Private Function UpdateDescription(ByVal paramSet As ParamFileGenerator.clsParams) As String
    Return Me.m_DMSUpload.GetDiffsFromTemplate(paramSet)
  End Function

  Private Sub mnuDebugSyncAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDebugSyncAll.Click
    Dim sync As New clsTransferParamEntriesToMassModList(mySettings)

    sync.SyncAll()

  End Sub

  Private Sub mnuDebugSyncSingle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDebugSyncSingle.Click
    Dim sync As New clsTransferParamEntriesToMassModList(mySettings)
    Dim paramFileID As Integer = CInt(InputBox("Enter an Parameter File ID to be Sync'ed", "Param File to Sync"))
    sync.SyncOneJob(paramFileID)
  End Sub

  Private Sub mnuDebugSyncDesc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDebugSyncDesc.Click
    Dim sync As New clsTransferParamEntriesToMassModList(mySettings)
    sync.SyncDescriptions(clsMainProcess.BaseLineParamSet)

  End Sub

End Class

Public Class NumericTextBox
  Inherits System.Windows.Forms.TextBox

  Private m_ForceChange As Boolean

  Public Property ForceNewValue() As Boolean
    Get
      Return Me.m_ForceChange
    End Get
    Set(ByVal Value As Boolean)
      Me.m_ForceChange = Value
    End Set
  End Property


End Class

Class ComboBoxContents
  Private m_Name As String
  Private m_Value As String

  Sub New(ByVal name As String, ByVal value As String)
    Me.m_Value = value
    Me.m_Name = name
  End Sub

  ReadOnly Property DisplayName() As String
    Get
      Return Me.m_Name
    End Get
  End Property
  ReadOnly Property Value() As String
    Get
      Return Me.m_Value
    End Get
  End Property
End Class