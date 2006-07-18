Imports ParamFileEditor.ProgramSettings
Imports ParamFileEditor.DownloadParams
Imports System.IO

Public Class frmMainGUI
    Inherits System.Windows.Forms.Form

    Private frmMH As frmMassHelper
    Private m_clsParamsFromDMS As clsParamsFromDMS
#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

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
    Friend WithEvents txtDynMod1MassDiff As System.Windows.Forms.TextBox
    Friend WithEvents txtDynMod2List As System.Windows.Forms.TextBox
    Friend WithEvents txtDynMod2MassDiff As System.Windows.Forms.TextBox
    Friend WithEvents lblDynMod1List As System.Windows.Forms.Label
    Friend WithEvents lblDynMod2List As System.Windows.Forms.Label
    Friend WithEvents lblDynMod1MassDiff As System.Windows.Forms.Label
    Friend WithEvents lblDynMod2MassDiff As System.Windows.Forms.Label
    Friend WithEvents lblDynMod3MassDiff As System.Windows.Forms.Label
    Friend WithEvents gbxStaticMods As System.Windows.Forms.GroupBox
    Friend WithEvents txtCTPep As System.Windows.Forms.TextBox
    Friend WithEvents txtAla As System.Windows.Forms.TextBox
    Friend WithEvents txtCTProt As System.Windows.Forms.TextBox
    Friend WithEvents txtNTPep As System.Windows.Forms.TextBox
    Friend WithEvents txtNTProt As System.Windows.Forms.TextBox
    Friend WithEvents txtGly As System.Windows.Forms.TextBox
    Friend WithEvents txtSer As System.Windows.Forms.TextBox
    Friend WithEvents txtCys As System.Windows.Forms.TextBox
    Friend WithEvents txtPro As System.Windows.Forms.TextBox
    Friend WithEvents TxtLorI As System.Windows.Forms.TextBox
    Friend WithEvents txtThr As System.Windows.Forms.TextBox
    Friend WithEvents txtIle As System.Windows.Forms.TextBox
    Friend WithEvents txtVal As System.Windows.Forms.TextBox
    Friend WithEvents txtLeu As System.Windows.Forms.TextBox
    Friend WithEvents txtNandD As System.Windows.Forms.TextBox
    Friend WithEvents txtQandE As System.Windows.Forms.TextBox
    Friend WithEvents txtAsn As System.Windows.Forms.TextBox
    Friend WithEvents txtLys As System.Windows.Forms.TextBox
    Friend WithEvents txtOrn As System.Windows.Forms.TextBox
    Friend WithEvents txtGln As System.Windows.Forms.TextBox
    Friend WithEvents txtAsp As System.Windows.Forms.TextBox
    Friend WithEvents txtArg As System.Windows.Forms.TextBox
    Friend WithEvents txtTrp As System.Windows.Forms.TextBox
    Friend WithEvents txtGlu As System.Windows.Forms.TextBox
    Friend WithEvents txtHis As System.Windows.Forms.TextBox
    Friend WithEvents txtPhe As System.Windows.Forms.TextBox
    Friend WithEvents txtTyr As System.Windows.Forms.TextBox
    Friend WithEvents txtMet As System.Windows.Forms.TextBox
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
    Friend WithEvents txtDynMod3MassDiff As System.Windows.Forms.TextBox
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
    Friend WithEvents txtSeqHdrFilter As System.Windows.Forms.TextBox
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
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
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
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSaveBW2 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileSaveBW3 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFileUploadDMS As System.Windows.Forms.MenuItem
    Friend WithEvents mnuToolsMassHelper As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
    Friend WithEvents mnuBatchUploadDMS As System.Windows.Forms.MenuItem
    Friend WithEvents mnuTools As System.Windows.Forms.MenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmMainGUI))
        Me.tcMain = New System.Windows.Forms.TabControl
        Me.tabBasic = New System.Windows.Forms.TabPage
        Me.gbxStaticMods = New System.Windows.Forms.GroupBox
        Me.lblCTPep = New System.Windows.Forms.Label
        Me.txtCTPep = New System.Windows.Forms.TextBox
        Me.txtAla = New System.Windows.Forms.TextBox
        Me.txtCTProt = New System.Windows.Forms.TextBox
        Me.txtNTPep = New System.Windows.Forms.TextBox
        Me.txtNTProt = New System.Windows.Forms.TextBox
        Me.txtGly = New System.Windows.Forms.TextBox
        Me.txtSer = New System.Windows.Forms.TextBox
        Me.txtCys = New System.Windows.Forms.TextBox
        Me.txtPro = New System.Windows.Forms.TextBox
        Me.TxtLorI = New System.Windows.Forms.TextBox
        Me.txtThr = New System.Windows.Forms.TextBox
        Me.txtIle = New System.Windows.Forms.TextBox
        Me.txtVal = New System.Windows.Forms.TextBox
        Me.txtLeu = New System.Windows.Forms.TextBox
        Me.txtNandD = New System.Windows.Forms.TextBox
        Me.txtQandE = New System.Windows.Forms.TextBox
        Me.txtAsn = New System.Windows.Forms.TextBox
        Me.txtLys = New System.Windows.Forms.TextBox
        Me.txtOrn = New System.Windows.Forms.TextBox
        Me.txtGln = New System.Windows.Forms.TextBox
        Me.txtAsp = New System.Windows.Forms.TextBox
        Me.txtArg = New System.Windows.Forms.TextBox
        Me.txtTrp = New System.Windows.Forms.TextBox
        Me.txtGlu = New System.Windows.Forms.TextBox
        Me.txtHis = New System.Windows.Forms.TextBox
        Me.txtPhe = New System.Windows.Forms.TextBox
        Me.txtTyr = New System.Windows.Forms.TextBox
        Me.txtMet = New System.Windows.Forms.TextBox
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
        Me.lblDescription = New System.Windows.Forms.Label
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.gbxSearch = New System.Windows.Forms.GroupBox
        Me.lblPartialSeq = New System.Windows.Forms.Label
        Me.txtPartialSeq = New System.Windows.Forms.TextBox
        Me.cboFragmentMassType = New System.Windows.Forms.ComboBox
        Me.cboMissedCleavages = New System.Windows.Forms.ComboBox
        Me.cboParentMassType = New System.Windows.Forms.ComboBox
        Me.lblParentMassType = New System.Windows.Forms.Label
        Me.cboEnzymeSelect = New System.Windows.Forms.ComboBox
        Me.lblEnzymeSelect = New System.Windows.Forms.Label
        Me.lblMissedCleavages = New System.Windows.Forms.Label
        Me.lblFragmentMassType = New System.Windows.Forms.Label
        Me.gbxDynMods = New System.Windows.Forms.GroupBox
        Me.txtDynMod1List = New System.Windows.Forms.TextBox
        Me.txtDynMod1MassDiff = New System.Windows.Forms.TextBox
        Me.txtDynMod2List = New System.Windows.Forms.TextBox
        Me.txtDynMod2MassDiff = New System.Windows.Forms.TextBox
        Me.txtDynMod3List = New System.Windows.Forms.TextBox
        Me.txtDynMod3MassDiff = New System.Windows.Forms.TextBox
        Me.lblDynMod1List = New System.Windows.Forms.Label
        Me.lblDynMod2List = New System.Windows.Forms.Label
        Me.lblDynMod3List = New System.Windows.Forms.Label
        Me.lblDynMod1MassDiff = New System.Windows.Forms.Label
        Me.lblDynMod3MassDiff = New System.Windows.Forms.Label
        Me.lblDynMod2MassDiff = New System.Windows.Forms.Label
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
        Me.lblSeqHdrFilter = New System.Windows.Forms.Label
        Me.txtSeqHdrFilter = New System.Windows.Forms.TextBox
        Me.lblMaxAAPerDynMod = New System.Windows.Forms.Label
        Me.txtMaxAAPerDynMod = New System.Windows.Forms.TextBox
        Me.lblNucReadingFrame = New System.Windows.Forms.Label
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
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.mnuFile = New System.Windows.Forms.MenuItem
        Me.mnuFileNewFileFromTemp = New System.Windows.Forms.MenuItem
        Me.mnuLoadFromFile = New System.Windows.Forms.MenuItem
        Me.mnuFileLoadFromDMS = New System.Windows.Forms.MenuItem
        Me.mnuFileSaveToFile = New System.Windows.Forms.MenuItem
        Me.mnuFileSaveBW2 = New System.Windows.Forms.MenuItem
        Me.mnuFileSaveBW3 = New System.Windows.Forms.MenuItem
        Me.mnuFileUploadDMS = New System.Windows.Forms.MenuItem
        Me.mnuBatchUploadDMS = New System.Windows.Forms.MenuItem
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.mnuFileExit = New System.Windows.Forms.MenuItem
        Me.mnuTools = New System.Windows.Forms.MenuItem
        Me.mnuToolsMassHelper = New System.Windows.Forms.MenuItem
        Me.mnuHelp = New System.Windows.Forms.MenuItem
        Me.mnuHelpAbout = New System.Windows.Forms.MenuItem
        Me.lblParamFileInfo = New System.Windows.Forms.Label
        Me.tcMain.SuspendLayout()
        Me.tabBasic.SuspendLayout()
        Me.gbxStaticMods.SuspendLayout()
        Me.gbxDesc.SuspendLayout()
        Me.gbxSearch.SuspendLayout()
        Me.gbxDynMods.SuspendLayout()
        Me.tabAdvanced.SuspendLayout()
        Me.gbxIonWeighting.SuspendLayout()
        Me.gbxMiscParams.SuspendLayout()
        Me.gbxToleranceValues.SuspendLayout()
        Me.gbxSwitches.SuspendLayout()
        Me.SuspendLayout()
        '
        'tcMain
        '
        Me.tcMain.Controls.Add(Me.tabBasic)
        Me.tcMain.Controls.Add(Me.tabAdvanced)
        Me.tcMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tcMain.ItemSize = New System.Drawing.Size(100, 20)
        Me.tcMain.Location = New System.Drawing.Point(0, 4)
        Me.tcMain.Name = "tcMain"
        Me.tcMain.SelectedIndex = 0
        Me.tcMain.Size = New System.Drawing.Size(476, 640)
        Me.tcMain.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tcMain.TabIndex = 0
        '
        'tabBasic
        '
        Me.tabBasic.Controls.Add(Me.gbxStaticMods)
        Me.tabBasic.Controls.Add(Me.gbxDesc)
        Me.tabBasic.Controls.Add(Me.gbxSearch)
        Me.tabBasic.Controls.Add(Me.gbxDynMods)
        Me.tabBasic.Location = New System.Drawing.Point(4, 24)
        Me.tabBasic.Name = "tabBasic"
        Me.tabBasic.Size = New System.Drawing.Size(468, 612)
        Me.tabBasic.TabIndex = 3
        Me.tabBasic.Text = "Basic Parameters"
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
        Me.gbxStaticMods.Location = New System.Drawing.Point(8, 428)
        Me.gbxStaticMods.Name = "gbxStaticMods"
        Me.gbxStaticMods.Size = New System.Drawing.Size(452, 176)
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
        'txtCTPep
        '
        Me.txtCTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCTPep.Location = New System.Drawing.Point(12, 32)
        Me.txtCTPep.Name = "txtCTPep"
        Me.txtCTPep.Size = New System.Drawing.Size(55, 20)
        Me.txtCTPep.TabIndex = 12
        Me.txtCTPep.Text = ""
        '
        'txtAla
        '
        Me.txtAla.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAla.Location = New System.Drawing.Point(322, 32)
        Me.txtAla.Name = "txtAla"
        Me.txtAla.Size = New System.Drawing.Size(55, 20)
        Me.txtAla.TabIndex = 17
        Me.txtAla.Text = ""
        '
        'txtCTProt
        '
        Me.txtCTProt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCTProt.Location = New System.Drawing.Point(74, 32)
        Me.txtCTProt.Name = "txtCTProt"
        Me.txtCTProt.Size = New System.Drawing.Size(55, 20)
        Me.txtCTProt.TabIndex = 13
        Me.txtCTProt.Text = ""
        '
        'txtNTPep
        '
        Me.txtNTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNTPep.Location = New System.Drawing.Point(136, 32)
        Me.txtNTPep.Name = "txtNTPep"
        Me.txtNTPep.Size = New System.Drawing.Size(55, 20)
        Me.txtNTPep.TabIndex = 14
        Me.txtNTPep.Text = ""
        '
        'txtNTProt
        '
        Me.txtNTProt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNTProt.Location = New System.Drawing.Point(198, 32)
        Me.txtNTProt.Name = "txtNTProt"
        Me.txtNTProt.Size = New System.Drawing.Size(55, 20)
        Me.txtNTProt.TabIndex = 15
        Me.txtNTProt.Text = ""
        '
        'txtGly
        '
        Me.txtGly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGly.Location = New System.Drawing.Point(260, 32)
        Me.txtGly.Name = "txtGly"
        Me.txtGly.Size = New System.Drawing.Size(55, 20)
        Me.txtGly.TabIndex = 16
        Me.txtGly.Text = ""
        '
        'txtSer
        '
        Me.txtSer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSer.Location = New System.Drawing.Point(384, 32)
        Me.txtSer.Name = "txtSer"
        Me.txtSer.Size = New System.Drawing.Size(55, 20)
        Me.txtSer.TabIndex = 18
        Me.txtSer.Text = ""
        '
        'txtCys
        '
        Me.txtCys.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCys.Location = New System.Drawing.Point(198, 68)
        Me.txtCys.Name = "txtCys"
        Me.txtCys.Size = New System.Drawing.Size(55, 20)
        Me.txtCys.TabIndex = 22
        Me.txtCys.Text = ""
        '
        'txtPro
        '
        Me.txtPro.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPro.Location = New System.Drawing.Point(12, 68)
        Me.txtPro.Name = "txtPro"
        Me.txtPro.Size = New System.Drawing.Size(55, 20)
        Me.txtPro.TabIndex = 19
        Me.txtPro.Text = ""
        '
        'TxtLorI
        '
        Me.TxtLorI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtLorI.Location = New System.Drawing.Point(384, 68)
        Me.TxtLorI.Name = "TxtLorI"
        Me.TxtLorI.Size = New System.Drawing.Size(55, 20)
        Me.TxtLorI.TabIndex = 25
        Me.TxtLorI.Text = ""
        '
        'txtThr
        '
        Me.txtThr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThr.Location = New System.Drawing.Point(136, 68)
        Me.txtThr.Name = "txtThr"
        Me.txtThr.Size = New System.Drawing.Size(55, 20)
        Me.txtThr.TabIndex = 21
        Me.txtThr.Text = ""
        '
        'txtIle
        '
        Me.txtIle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIle.Location = New System.Drawing.Point(322, 68)
        Me.txtIle.Name = "txtIle"
        Me.txtIle.Size = New System.Drawing.Size(55, 20)
        Me.txtIle.TabIndex = 24
        Me.txtIle.Text = ""
        '
        'txtVal
        '
        Me.txtVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVal.Location = New System.Drawing.Point(74, 68)
        Me.txtVal.Name = "txtVal"
        Me.txtVal.Size = New System.Drawing.Size(55, 20)
        Me.txtVal.TabIndex = 20
        Me.txtVal.Text = ""
        '
        'txtLeu
        '
        Me.txtLeu.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLeu.Location = New System.Drawing.Point(260, 68)
        Me.txtLeu.Name = "txtLeu"
        Me.txtLeu.Size = New System.Drawing.Size(55, 20)
        Me.txtLeu.TabIndex = 23
        Me.txtLeu.Text = ""
        '
        'txtNandD
        '
        Me.txtNandD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNandD.Location = New System.Drawing.Point(136, 104)
        Me.txtNandD.Name = "txtNandD"
        Me.txtNandD.Size = New System.Drawing.Size(55, 20)
        Me.txtNandD.TabIndex = 28
        Me.txtNandD.Text = ""
        '
        'txtQandE
        '
        Me.txtQandE.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtQandE.Location = New System.Drawing.Point(384, 104)
        Me.txtQandE.Name = "txtQandE"
        Me.txtQandE.Size = New System.Drawing.Size(55, 20)
        Me.txtQandE.TabIndex = 32
        Me.txtQandE.Text = ""
        '
        'txtAsn
        '
        Me.txtAsn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAsn.Location = New System.Drawing.Point(12, 104)
        Me.txtAsn.Name = "txtAsn"
        Me.txtAsn.Size = New System.Drawing.Size(55, 20)
        Me.txtAsn.TabIndex = 26
        Me.txtAsn.Text = ""
        '
        'txtLys
        '
        Me.txtLys.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLys.Location = New System.Drawing.Point(322, 104)
        Me.txtLys.Name = "txtLys"
        Me.txtLys.Size = New System.Drawing.Size(55, 20)
        Me.txtLys.TabIndex = 31
        Me.txtLys.Text = ""
        '
        'txtOrn
        '
        Me.txtOrn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOrn.Location = New System.Drawing.Point(74, 104)
        Me.txtOrn.Name = "txtOrn"
        Me.txtOrn.Size = New System.Drawing.Size(55, 20)
        Me.txtOrn.TabIndex = 27
        Me.txtOrn.Text = ""
        '
        'txtGln
        '
        Me.txtGln.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGln.Location = New System.Drawing.Point(260, 104)
        Me.txtGln.Name = "txtGln"
        Me.txtGln.Size = New System.Drawing.Size(55, 20)
        Me.txtGln.TabIndex = 30
        Me.txtGln.Text = ""
        '
        'txtAsp
        '
        Me.txtAsp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAsp.Location = New System.Drawing.Point(198, 104)
        Me.txtAsp.Name = "txtAsp"
        Me.txtAsp.Size = New System.Drawing.Size(55, 20)
        Me.txtAsp.TabIndex = 29
        Me.txtAsp.Text = ""
        '
        'txtArg
        '
        Me.txtArg.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtArg.Location = New System.Drawing.Point(260, 140)
        Me.txtArg.Name = "txtArg"
        Me.txtArg.Size = New System.Drawing.Size(55, 20)
        Me.txtArg.TabIndex = 37
        Me.txtArg.Text = ""
        '
        'txtTrp
        '
        Me.txtTrp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTrp.Location = New System.Drawing.Point(384, 140)
        Me.txtTrp.Name = "txtTrp"
        Me.txtTrp.Size = New System.Drawing.Size(55, 20)
        Me.txtTrp.TabIndex = 39
        Me.txtTrp.Text = ""
        '
        'txtGlu
        '
        Me.txtGlu.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGlu.Location = New System.Drawing.Point(12, 140)
        Me.txtGlu.Name = "txtGlu"
        Me.txtGlu.Size = New System.Drawing.Size(55, 20)
        Me.txtGlu.TabIndex = 33
        Me.txtGlu.Text = ""
        '
        'txtHis
        '
        Me.txtHis.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHis.Location = New System.Drawing.Point(136, 140)
        Me.txtHis.Name = "txtHis"
        Me.txtHis.Size = New System.Drawing.Size(55, 20)
        Me.txtHis.TabIndex = 35
        Me.txtHis.Text = ""
        '
        'txtPhe
        '
        Me.txtPhe.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPhe.Location = New System.Drawing.Point(198, 140)
        Me.txtPhe.Name = "txtPhe"
        Me.txtPhe.Size = New System.Drawing.Size(55, 20)
        Me.txtPhe.TabIndex = 36
        Me.txtPhe.Text = ""
        '
        'txtTyr
        '
        Me.txtTyr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTyr.Location = New System.Drawing.Point(322, 140)
        Me.txtTyr.Name = "txtTyr"
        Me.txtTyr.Size = New System.Drawing.Size(55, 20)
        Me.txtTyr.TabIndex = 38
        Me.txtTyr.Text = ""
        '
        'txtMet
        '
        Me.txtMet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMet.Location = New System.Drawing.Point(74, 140)
        Me.txtMet.Name = "txtMet"
        Me.txtMet.Size = New System.Drawing.Size(55, 20)
        Me.txtMet.TabIndex = 34
        Me.txtMet.Text = ""
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
        Me.lblCys.Location = New System.Drawing.Point(196, 56)
        Me.lblCys.Name = "lblCys"
        Me.lblCys.Size = New System.Drawing.Size(56, 12)
        Me.lblCys.TabIndex = 1
        Me.lblCys.Text = "Cys (C)"
        Me.lblCys.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblLorI
        '
        Me.lblLorI.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLorI.Location = New System.Drawing.Point(384, 56)
        Me.lblLorI.Name = "lblLorI"
        Me.lblLorI.Size = New System.Drawing.Size(56, 12)
        Me.lblLorI.TabIndex = 1
        Me.lblLorI.Text = "L or I (X)"
        Me.lblLorI.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblThr
        '
        Me.lblThr.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblThr.Location = New System.Drawing.Point(136, 56)
        Me.lblThr.Name = "lblThr"
        Me.lblThr.Size = New System.Drawing.Size(56, 12)
        Me.lblThr.TabIndex = 1
        Me.lblThr.Text = "Thr (T)"
        Me.lblThr.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblVal
        '
        Me.lblVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVal.Location = New System.Drawing.Point(72, 56)
        Me.lblVal.Name = "lblVal"
        Me.lblVal.Size = New System.Drawing.Size(56, 12)
        Me.lblVal.TabIndex = 1
        Me.lblVal.Text = "Val (V)"
        Me.lblVal.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblLeu
        '
        Me.lblLeu.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLeu.Location = New System.Drawing.Point(260, 56)
        Me.lblLeu.Name = "lblLeu"
        Me.lblLeu.Size = New System.Drawing.Size(56, 12)
        Me.lblLeu.TabIndex = 1
        Me.lblLeu.Text = "Leu (L)"
        Me.lblLeu.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblIle
        '
        Me.lblIle.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIle.Location = New System.Drawing.Point(320, 56)
        Me.lblIle.Name = "lblIle"
        Me.lblIle.Size = New System.Drawing.Size(56, 12)
        Me.lblIle.TabIndex = 1
        Me.lblIle.Text = "Ile (I)"
        Me.lblIle.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPro
        '
        Me.lblPro.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPro.Location = New System.Drawing.Point(12, 56)
        Me.lblPro.Name = "lblPro"
        Me.lblPro.Size = New System.Drawing.Size(56, 12)
        Me.lblPro.TabIndex = 1
        Me.lblPro.Text = "Pro (P)"
        Me.lblPro.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblAsn
        '
        Me.lblAsn.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAsn.Location = New System.Drawing.Point(12, 92)
        Me.lblAsn.Name = "lblAsn"
        Me.lblAsn.Size = New System.Drawing.Size(56, 12)
        Me.lblAsn.TabIndex = 1
        Me.lblAsn.Text = "Asn (N)"
        Me.lblAsn.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblGln
        '
        Me.lblGln.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGln.Location = New System.Drawing.Point(260, 92)
        Me.lblGln.Name = "lblGln"
        Me.lblGln.Size = New System.Drawing.Size(56, 12)
        Me.lblGln.TabIndex = 1
        Me.lblGln.Text = "Gln (Q)"
        Me.lblGln.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblQandE
        '
        Me.lblQandE.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblQandE.Location = New System.Drawing.Point(382, 92)
        Me.lblQandE.Name = "lblQandE"
        Me.lblQandE.Size = New System.Drawing.Size(61, 12)
        Me.lblQandE.TabIndex = 1
        Me.lblQandE.Text = "Avg Q && E (Z)"
        Me.lblQandE.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblNandD
        '
        Me.lblNandD.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNandD.Location = New System.Drawing.Point(134, 92)
        Me.lblNandD.Name = "lblNandD"
        Me.lblNandD.Size = New System.Drawing.Size(64, 12)
        Me.lblNandD.TabIndex = 1
        Me.lblNandD.Text = "Avg N && D (B)"
        Me.lblNandD.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblOrn
        '
        Me.lblOrn.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrn.Location = New System.Drawing.Point(72, 92)
        Me.lblOrn.Name = "lblOrn"
        Me.lblOrn.Size = New System.Drawing.Size(56, 12)
        Me.lblOrn.TabIndex = 1
        Me.lblOrn.Text = "Orn (O)"
        Me.lblOrn.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblAsp
        '
        Me.lblAsp.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAsp.Location = New System.Drawing.Point(196, 92)
        Me.lblAsp.Name = "lblAsp"
        Me.lblAsp.Size = New System.Drawing.Size(56, 12)
        Me.lblAsp.TabIndex = 1
        Me.lblAsp.Text = "Asp (D)"
        Me.lblAsp.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblLys
        '
        Me.lblLys.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLys.Location = New System.Drawing.Point(320, 92)
        Me.lblLys.Name = "lblLys"
        Me.lblLys.Size = New System.Drawing.Size(56, 12)
        Me.lblLys.TabIndex = 1
        Me.lblLys.Text = "Lys (K)"
        Me.lblLys.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblArg
        '
        Me.lblArg.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArg.Location = New System.Drawing.Point(260, 128)
        Me.lblArg.Name = "lblArg"
        Me.lblArg.Size = New System.Drawing.Size(56, 12)
        Me.lblArg.TabIndex = 1
        Me.lblArg.Text = "Arg (R)"
        Me.lblArg.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblTrp
        '
        Me.lblTrp.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTrp.Location = New System.Drawing.Point(384, 128)
        Me.lblTrp.Name = "lblTrp"
        Me.lblTrp.Size = New System.Drawing.Size(56, 12)
        Me.lblTrp.TabIndex = 1
        Me.lblTrp.Text = "Trp (W)"
        Me.lblTrp.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblHis
        '
        Me.lblHis.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHis.Location = New System.Drawing.Point(136, 128)
        Me.lblHis.Name = "lblHis"
        Me.lblHis.Size = New System.Drawing.Size(56, 12)
        Me.lblHis.TabIndex = 1
        Me.lblHis.Text = "His (H)"
        Me.lblHis.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblMet
        '
        Me.lblMet.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMet.Location = New System.Drawing.Point(72, 128)
        Me.lblMet.Name = "lblMet"
        Me.lblMet.Size = New System.Drawing.Size(56, 12)
        Me.lblMet.TabIndex = 1
        Me.lblMet.Text = "Met (M)"
        Me.lblMet.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPhe
        '
        Me.lblPhe.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPhe.Location = New System.Drawing.Point(196, 128)
        Me.lblPhe.Name = "lblPhe"
        Me.lblPhe.Size = New System.Drawing.Size(56, 12)
        Me.lblPhe.TabIndex = 1
        Me.lblPhe.Text = "Phe (F)"
        Me.lblPhe.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblTyr
        '
        Me.lblTyr.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTyr.Location = New System.Drawing.Point(320, 128)
        Me.lblTyr.Name = "lblTyr"
        Me.lblTyr.Size = New System.Drawing.Size(56, 12)
        Me.lblTyr.TabIndex = 1
        Me.lblTyr.Text = "Tyr (Y)"
        Me.lblTyr.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblGlu
        '
        Me.lblGlu.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGlu.Location = New System.Drawing.Point(12, 128)
        Me.lblGlu.Name = "lblGlu"
        Me.lblGlu.Size = New System.Drawing.Size(56, 12)
        Me.lblGlu.TabIndex = 1
        Me.lblGlu.Text = "Glu (E)"
        Me.lblGlu.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'gbxDesc
        '
        Me.gbxDesc.Controls.Add(Me.lblDescription)
        Me.gbxDesc.Controls.Add(Me.txtDescription)
        Me.gbxDesc.Location = New System.Drawing.Point(8, 8)
        Me.gbxDesc.Name = "gbxDesc"
        Me.gbxDesc.Size = New System.Drawing.Size(452, 128)
        Me.gbxDesc.TabIndex = 0
        Me.gbxDesc.TabStop = False
        Me.gbxDesc.Text = "Name and Description Information"
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
        'txtDescription
        '
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.Location = New System.Drawing.Point(12, 36)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(428, 80)
        Me.txtDescription.TabIndex = 0
        Me.txtDescription.Text = ""
        '
        'gbxSearch
        '
        Me.gbxSearch.Controls.Add(Me.lblPartialSeq)
        Me.gbxSearch.Controls.Add(Me.txtPartialSeq)
        Me.gbxSearch.Controls.Add(Me.cboFragmentMassType)
        Me.gbxSearch.Controls.Add(Me.cboMissedCleavages)
        Me.gbxSearch.Controls.Add(Me.cboParentMassType)
        Me.gbxSearch.Controls.Add(Me.lblParentMassType)
        Me.gbxSearch.Controls.Add(Me.cboEnzymeSelect)
        Me.gbxSearch.Controls.Add(Me.lblEnzymeSelect)
        Me.gbxSearch.Controls.Add(Me.lblMissedCleavages)
        Me.gbxSearch.Controls.Add(Me.lblFragmentMassType)
        Me.gbxSearch.Location = New System.Drawing.Point(8, 144)
        Me.gbxSearch.Name = "gbxSearch"
        Me.gbxSearch.Size = New System.Drawing.Size(452, 156)
        Me.gbxSearch.TabIndex = 0
        Me.gbxSearch.TabStop = False
        Me.gbxSearch.Text = "Search Settings"
        '
        'lblPartialSeq
        '
        Me.lblPartialSeq.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartialSeq.Location = New System.Drawing.Point(12, 108)
        Me.lblPartialSeq.Name = "lblPartialSeq"
        Me.lblPartialSeq.Size = New System.Drawing.Size(160, 16)
        Me.lblPartialSeq.TabIndex = 11
        Me.lblPartialSeq.Text = "Partial Sequence To Match"
        '
        'txtPartialSeq
        '
        Me.txtPartialSeq.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartialSeq.Location = New System.Drawing.Point(12, 124)
        Me.txtPartialSeq.Name = "txtPartialSeq"
        Me.txtPartialSeq.Size = New System.Drawing.Size(428, 20)
        Me.txtPartialSeq.TabIndex = 5
        Me.txtPartialSeq.Text = ""
        '
        'cboFragmentMassType
        '
        Me.cboFragmentMassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFragmentMassType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboFragmentMassType.Location = New System.Drawing.Point(244, 36)
        Me.cboFragmentMassType.Name = "cboFragmentMassType"
        Me.cboFragmentMassType.Size = New System.Drawing.Size(200, 21)
        Me.cboFragmentMassType.TabIndex = 2
        '
        'cboMissedCleavages
        '
        Me.cboMissedCleavages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMissedCleavages.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMissedCleavages.Location = New System.Drawing.Point(244, 80)
        Me.cboMissedCleavages.Name = "cboMissedCleavages"
        Me.cboMissedCleavages.Size = New System.Drawing.Size(200, 21)
        Me.cboMissedCleavages.TabIndex = 4
        '
        'cboParentMassType
        '
        Me.cboParentMassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboParentMassType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboParentMassType.Location = New System.Drawing.Point(12, 36)
        Me.cboParentMassType.Name = "cboParentMassType"
        Me.cboParentMassType.Size = New System.Drawing.Size(200, 21)
        Me.cboParentMassType.TabIndex = 1
        '
        'lblParentMassType
        '
        Me.lblParentMassType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblParentMassType.Location = New System.Drawing.Point(12, 20)
        Me.lblParentMassType.Name = "lblParentMassType"
        Me.lblParentMassType.Size = New System.Drawing.Size(196, 16)
        Me.lblParentMassType.TabIndex = 2
        Me.lblParentMassType.Text = "Parent Ion Mass Type"
        '
        'cboEnzymeSelect
        '
        Me.cboEnzymeSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboEnzymeSelect.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboEnzymeSelect.Location = New System.Drawing.Point(12, 80)
        Me.cboEnzymeSelect.Name = "cboEnzymeSelect"
        Me.cboEnzymeSelect.Size = New System.Drawing.Size(200, 21)
        Me.cboEnzymeSelect.TabIndex = 3
        '
        'lblEnzymeSelect
        '
        Me.lblEnzymeSelect.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEnzymeSelect.Location = New System.Drawing.Point(12, 64)
        Me.lblEnzymeSelect.Name = "lblEnzymeSelect"
        Me.lblEnzymeSelect.Size = New System.Drawing.Size(132, 16)
        Me.lblEnzymeSelect.TabIndex = 0
        Me.lblEnzymeSelect.Text = "Enzyme Cleavage Rule"
        '
        'lblMissedCleavages
        '
        Me.lblMissedCleavages.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMissedCleavages.Location = New System.Drawing.Point(244, 64)
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
        Me.lblFragmentMassType.Size = New System.Drawing.Size(168, 20)
        Me.lblFragmentMassType.TabIndex = 7
        Me.lblFragmentMassType.Text = "Fragment Ion Mass Type"
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
        Me.gbxDynMods.Location = New System.Drawing.Point(8, 308)
        Me.gbxDynMods.Name = "gbxDynMods"
        Me.gbxDynMods.Size = New System.Drawing.Size(452, 112)
        Me.gbxDynMods.TabIndex = 0
        Me.gbxDynMods.TabStop = False
        Me.gbxDynMods.Text = "Dynamic Modifications to Apply"
        '
        'txtDynMod1List
        '
        Me.txtDynMod1List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDynMod1List.Location = New System.Drawing.Point(12, 36)
        Me.txtDynMod1List.Name = "txtDynMod1List"
        Me.txtDynMod1List.Size = New System.Drawing.Size(132, 20)
        Me.txtDynMod1List.TabIndex = 6
        Me.txtDynMod1List.Text = ""
        '
        'txtDynMod1MassDiff
        '
        Me.txtDynMod1MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDynMod1MassDiff.Location = New System.Drawing.Point(12, 80)
        Me.txtDynMod1MassDiff.Name = "txtDynMod1MassDiff"
        Me.txtDynMod1MassDiff.Size = New System.Drawing.Size(132, 20)
        Me.txtDynMod1MassDiff.TabIndex = 7
        Me.txtDynMod1MassDiff.Text = ""
        '
        'txtDynMod2List
        '
        Me.txtDynMod2List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDynMod2List.Location = New System.Drawing.Point(160, 36)
        Me.txtDynMod2List.Name = "txtDynMod2List"
        Me.txtDynMod2List.Size = New System.Drawing.Size(132, 20)
        Me.txtDynMod2List.TabIndex = 8
        Me.txtDynMod2List.Text = ""
        '
        'txtDynMod2MassDiff
        '
        Me.txtDynMod2MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDynMod2MassDiff.Location = New System.Drawing.Point(160, 80)
        Me.txtDynMod2MassDiff.Name = "txtDynMod2MassDiff"
        Me.txtDynMod2MassDiff.Size = New System.Drawing.Size(132, 20)
        Me.txtDynMod2MassDiff.TabIndex = 9
        Me.txtDynMod2MassDiff.Text = ""
        '
        'txtDynMod3List
        '
        Me.txtDynMod3List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDynMod3List.Location = New System.Drawing.Point(308, 36)
        Me.txtDynMod3List.Name = "txtDynMod3List"
        Me.txtDynMod3List.Size = New System.Drawing.Size(132, 20)
        Me.txtDynMod3List.TabIndex = 10
        Me.txtDynMod3List.Text = ""
        '
        'txtDynMod3MassDiff
        '
        Me.txtDynMod3MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDynMod3MassDiff.Location = New System.Drawing.Point(308, 80)
        Me.txtDynMod3MassDiff.Name = "txtDynMod3MassDiff"
        Me.txtDynMod3MassDiff.Size = New System.Drawing.Size(132, 20)
        Me.txtDynMod3MassDiff.TabIndex = 11
        Me.txtDynMod3MassDiff.Text = ""
        '
        'lblDynMod1List
        '
        Me.lblDynMod1List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDynMod1List.Location = New System.Drawing.Point(12, 20)
        Me.lblDynMod1List.Name = "lblDynMod1List"
        Me.lblDynMod1List.Size = New System.Drawing.Size(132, 16)
        Me.lblDynMod1List.TabIndex = 1
        Me.lblDynMod1List.Text = "Mod 1 AA List"
        '
        'lblDynMod2List
        '
        Me.lblDynMod2List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDynMod2List.Location = New System.Drawing.Point(160, 20)
        Me.lblDynMod2List.Name = "lblDynMod2List"
        Me.lblDynMod2List.Size = New System.Drawing.Size(128, 16)
        Me.lblDynMod2List.TabIndex = 2
        Me.lblDynMod2List.Text = "Mod 2 AA List"
        '
        'lblDynMod3List
        '
        Me.lblDynMod3List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDynMod3List.Location = New System.Drawing.Point(308, 20)
        Me.lblDynMod3List.Name = "lblDynMod3List"
        Me.lblDynMod3List.Size = New System.Drawing.Size(128, 16)
        Me.lblDynMod3List.TabIndex = 3
        Me.lblDynMod3List.Text = "Mod 3 AA List"
        '
        'lblDynMod1MassDiff
        '
        Me.lblDynMod1MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDynMod1MassDiff.Location = New System.Drawing.Point(12, 64)
        Me.lblDynMod1MassDiff.Name = "lblDynMod1MassDiff"
        Me.lblDynMod1MassDiff.Size = New System.Drawing.Size(120, 16)
        Me.lblDynMod1MassDiff.TabIndex = 4
        Me.lblDynMod1MassDiff.Text = "Mod 1 Mass Change"
        '
        'lblDynMod3MassDiff
        '
        Me.lblDynMod3MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDynMod3MassDiff.Location = New System.Drawing.Point(308, 64)
        Me.lblDynMod3MassDiff.Name = "lblDynMod3MassDiff"
        Me.lblDynMod3MassDiff.Size = New System.Drawing.Size(124, 16)
        Me.lblDynMod3MassDiff.TabIndex = 6
        Me.lblDynMod3MassDiff.Text = "Mod 3 Mass Change"
        '
        'lblDynMod2MassDiff
        '
        Me.lblDynMod2MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDynMod2MassDiff.Location = New System.Drawing.Point(160, 64)
        Me.lblDynMod2MassDiff.Name = "lblDynMod2MassDiff"
        Me.lblDynMod2MassDiff.Size = New System.Drawing.Size(116, 16)
        Me.lblDynMod2MassDiff.TabIndex = 5
        Me.lblDynMod2MassDiff.Text = "Mod 2 Mass Change"
        '
        'tabAdvanced
        '
        Me.tabAdvanced.Controls.Add(Me.gbxIonWeighting)
        Me.tabAdvanced.Controls.Add(Me.gbxMiscParams)
        Me.tabAdvanced.Controls.Add(Me.gbxToleranceValues)
        Me.tabAdvanced.Controls.Add(Me.gbxSwitches)
        Me.tabAdvanced.Location = New System.Drawing.Point(4, 24)
        Me.tabAdvanced.Name = "tabAdvanced"
        Me.tabAdvanced.Size = New System.Drawing.Size(468, 612)
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
        Me.gbxIonWeighting.Location = New System.Drawing.Point(8, 388)
        Me.gbxIonWeighting.Name = "gbxIonWeighting"
        Me.gbxIonWeighting.Size = New System.Drawing.Size(452, 84)
        Me.gbxIonWeighting.TabIndex = 3
        Me.gbxIonWeighting.TabStop = False
        Me.gbxIonWeighting.Text = "Ion Weighting Parameters"
        '
        'txtWWeight
        '
        Me.txtWWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWWeight.Location = New System.Drawing.Point(192, 56)
        Me.txtWWeight.Name = "txtWWeight"
        Me.txtWWeight.Size = New System.Drawing.Size(55, 20)
        Me.txtWWeight.TabIndex = 19
        Me.txtWWeight.Text = ""
        '
        'lblWWeight
        '
        Me.lblWWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWWeight.Location = New System.Drawing.Point(192, 44)
        Me.lblWWeight.Name = "lblWWeight"
        Me.lblWWeight.Size = New System.Drawing.Size(60, 12)
        Me.lblWWeight.TabIndex = 14
        Me.lblWWeight.Text = "w Ion Weight"
        Me.lblWWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtXWeight
        '
        Me.txtXWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtXWeight.Location = New System.Drawing.Point(256, 56)
        Me.txtXWeight.Name = "txtXWeight"
        Me.txtXWeight.Size = New System.Drawing.Size(55, 20)
        Me.txtXWeight.TabIndex = 20
        Me.txtXWeight.Text = ""
        '
        'lblXWeight
        '
        Me.lblXWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblXWeight.Location = New System.Drawing.Point(256, 44)
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
        Me.txtDWeight.Text = ""
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
        Me.txtCWeight.Text = ""
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
        Me.txtBWeight.Text = ""
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
        Me.lblVWeight.Location = New System.Drawing.Point(128, 44)
        Me.lblVWeight.Name = "lblVWeight"
        Me.lblVWeight.Size = New System.Drawing.Size(56, 12)
        Me.lblVWeight.TabIndex = 3
        Me.lblVWeight.Text = "v Ion Weight"
        Me.lblVWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtVWeight
        '
        Me.txtVWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVWeight.Location = New System.Drawing.Point(128, 56)
        Me.txtVWeight.Name = "txtVWeight"
        Me.txtVWeight.Size = New System.Drawing.Size(55, 20)
        Me.txtVWeight.TabIndex = 18
        Me.txtVWeight.Text = ""
        '
        'txtYWeight
        '
        Me.txtYWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtYWeight.Location = New System.Drawing.Point(320, 56)
        Me.txtYWeight.Name = "txtYWeight"
        Me.txtYWeight.Size = New System.Drawing.Size(55, 20)
        Me.txtYWeight.TabIndex = 21
        Me.txtYWeight.Text = ""
        '
        'lblYWeight
        '
        Me.lblYWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblYWeight.Location = New System.Drawing.Point(320, 44)
        Me.lblYWeight.Name = "lblYWeight"
        Me.lblYWeight.Size = New System.Drawing.Size(56, 12)
        Me.lblYWeight.TabIndex = 3
        Me.lblYWeight.Text = "y Ion Weight"
        Me.lblYWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtZWeight
        '
        Me.txtZWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtZWeight.Location = New System.Drawing.Point(384, 56)
        Me.txtZWeight.Name = "txtZWeight"
        Me.txtZWeight.Size = New System.Drawing.Size(55, 20)
        Me.txtZWeight.TabIndex = 22
        Me.txtZWeight.Text = ""
        '
        'lblZWeight
        '
        Me.lblZWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblZWeight.Location = New System.Drawing.Point(384, 44)
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
        Me.txtAWeight.Text = ""
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
        Me.chkUseAIons.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUseAIons.Location = New System.Drawing.Point(12, 20)
        Me.chkUseAIons.Name = "chkUseAIons"
        Me.chkUseAIons.Size = New System.Drawing.Size(104, 16)
        Me.chkUseAIons.TabIndex = 23
        Me.chkUseAIons.Text = "Use A Ions?"
        '
        'chkUseBIons
        '
        Me.chkUseBIons.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUseBIons.Location = New System.Drawing.Point(12, 40)
        Me.chkUseBIons.Name = "chkUseBIons"
        Me.chkUseBIons.Size = New System.Drawing.Size(104, 16)
        Me.chkUseBIons.TabIndex = 24
        Me.chkUseBIons.Text = "Use B Ions?"
        '
        'chkUseYIons
        '
        Me.chkUseYIons.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUseYIons.Location = New System.Drawing.Point(12, 60)
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
        Me.gbxMiscParams.Controls.Add(Me.lblSeqHdrFilter)
        Me.gbxMiscParams.Controls.Add(Me.txtSeqHdrFilter)
        Me.gbxMiscParams.Controls.Add(Me.lblMaxAAPerDynMod)
        Me.gbxMiscParams.Controls.Add(Me.txtMaxAAPerDynMod)
        Me.gbxMiscParams.Controls.Add(Me.lblNucReadingFrame)
        Me.gbxMiscParams.Location = New System.Drawing.Point(8, 176)
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
        Me.txtNumResults.Enabled = False
        Me.txtNumResults.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumResults.Location = New System.Drawing.Point(244, 168)
        Me.txtNumResults.Name = "txtNumResults"
        Me.txtNumResults.Size = New System.Drawing.Size(196, 20)
        Me.txtNumResults.TabIndex = 13
        Me.txtNumResults.Text = ""
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
        Me.txtNumDescLines.Text = ""
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
        Me.txtNumOutputLines.Text = ""
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
        Me.txtMatchPeakCountErrors.Text = ""
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
        Me.txtMatchPeakCount.Text = ""
        '
        'lblSeqHdrFilter
        '
        Me.lblSeqHdrFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSeqHdrFilter.Location = New System.Drawing.Point(244, 108)
        Me.lblSeqHdrFilter.Name = "lblSeqHdrFilter"
        Me.lblSeqHdrFilter.Size = New System.Drawing.Size(188, 16)
        Me.lblSeqHdrFilter.TabIndex = 15
        Me.lblSeqHdrFilter.Text = "Sequence Header Filter String"
        '
        'txtSeqHdrFilter
        '
        Me.txtSeqHdrFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSeqHdrFilter.Location = New System.Drawing.Point(244, 124)
        Me.txtSeqHdrFilter.Name = "txtSeqHdrFilter"
        Me.txtSeqHdrFilter.Size = New System.Drawing.Size(196, 20)
        Me.txtSeqHdrFilter.TabIndex = 11
        Me.txtSeqHdrFilter.Text = ""
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
        Me.txtMaxAAPerDynMod.Text = ""
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
        Me.gbxToleranceValues.Location = New System.Drawing.Point(8, 8)
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
        Me.txtFragMassTol.Text = ""
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
        Me.txtPepMassTol.Text = ""
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
        Me.txtIonCutoff.Text = ""
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
        Me.txtPeakMatchingTol.Text = ""
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
        Me.txtMaxProtMass.Text = ""
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
        Me.txtMinProtMass.Text = ""
        '
        'gbxSwitches
        '
        Me.gbxSwitches.Controls.Add(Me.chkResiduesInUpperCase)
        Me.gbxSwitches.Controls.Add(Me.chkPrintDupRefs)
        Me.gbxSwitches.Controls.Add(Me.chkRemovePrecursorPeaks)
        Me.gbxSwitches.Controls.Add(Me.chkShowFragmentIons)
        Me.gbxSwitches.Controls.Add(Me.chkCreateOutputFiles)
        Me.gbxSwitches.Location = New System.Drawing.Point(8, 480)
        Me.gbxSwitches.Name = "gbxSwitches"
        Me.gbxSwitches.Size = New System.Drawing.Size(452, 124)
        Me.gbxSwitches.TabIndex = 0
        Me.gbxSwitches.TabStop = False
        Me.gbxSwitches.Text = "Search Options"
        '
        'chkResiduesInUpperCase
        '
        Me.chkResiduesInUpperCase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkResiduesInUpperCase.Location = New System.Drawing.Point(12, 96)
        Me.chkResiduesInUpperCase.Name = "chkResiduesInUpperCase"
        Me.chkResiduesInUpperCase.Size = New System.Drawing.Size(248, 24)
        Me.chkResiduesInUpperCase.TabIndex = 30
        Me.chkResiduesInUpperCase.Text = "FASTA File has Residues in Upper Case?"
        '
        'chkPrintDupRefs
        '
        Me.chkPrintDupRefs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintDupRefs.Location = New System.Drawing.Point(12, 76)
        Me.chkPrintDupRefs.Name = "chkPrintDupRefs"
        Me.chkPrintDupRefs.Size = New System.Drawing.Size(248, 24)
        Me.chkPrintDupRefs.TabIndex = 29
        Me.chkPrintDupRefs.Text = "Print Duplicate References (ORFs)?"
        '
        'chkRemovePrecursorPeaks
        '
        Me.chkRemovePrecursorPeaks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRemovePrecursorPeaks.Location = New System.Drawing.Point(12, 56)
        Me.chkRemovePrecursorPeaks.Name = "chkRemovePrecursorPeaks"
        Me.chkRemovePrecursorPeaks.Size = New System.Drawing.Size(248, 24)
        Me.chkRemovePrecursorPeaks.TabIndex = 28
        Me.chkRemovePrecursorPeaks.Text = "Remove Precursor Ion Peaks?"
        '
        'chkShowFragmentIons
        '
        Me.chkShowFragmentIons.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowFragmentIons.Location = New System.Drawing.Point(12, 36)
        Me.chkShowFragmentIons.Name = "chkShowFragmentIons"
        Me.chkShowFragmentIons.Size = New System.Drawing.Size(248, 24)
        Me.chkShowFragmentIons.TabIndex = 27
        Me.chkShowFragmentIons.Text = "Show Fragment Ions?"
        '
        'chkCreateOutputFiles
        '
        Me.chkCreateOutputFiles.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCreateOutputFiles.Location = New System.Drawing.Point(12, 16)
        Me.chkCreateOutputFiles.Name = "chkCreateOutputFiles"
        Me.chkCreateOutputFiles.Size = New System.Drawing.Size(248, 24)
        Me.chkCreateOutputFiles.TabIndex = 26
        Me.chkCreateOutputFiles.Text = "Create Output Files?"
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuTools, Me.mnuHelp})
        '
        'mnuFile
        '
        Me.mnuFile.Index = 0
        Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileNewFileFromTemp, Me.mnuFileSaveToFile, Me.mnuFileUploadDMS, Me.mnuBatchUploadDMS, Me.MenuItem5, Me.mnuFileExit})
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
        Me.mnuFileSaveToFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileSaveBW2, Me.mnuFileSaveBW3})
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
        'MenuItem5
        '
        Me.MenuItem5.Index = 4
        Me.MenuItem5.Text = "-"
        '
        'mnuFileExit
        '
        Me.mnuFileExit.Index = 5
        Me.mnuFileExit.Shortcut = System.Windows.Forms.Shortcut.CtrlX
        Me.mnuFileExit.Text = "E&xit"
        '
        'mnuTools
        '
        Me.mnuTools.Index = 1
        Me.mnuTools.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuToolsMassHelper})
        Me.mnuTools.Text = "Tools"
        '
        'mnuToolsMassHelper
        '
        Me.mnuToolsMassHelper.Index = 0
        Me.mnuToolsMassHelper.Text = "View Mass Calculation Helper"
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
        'lblParamFileInfo
        '
        Me.lblParamFileInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblParamFileInfo.Location = New System.Drawing.Point(8, 652)
        Me.lblParamFileInfo.Name = "lblParamFileInfo"
        Me.lblParamFileInfo.Size = New System.Drawing.Size(460, 48)
        Me.lblParamFileInfo.TabIndex = 7
        Me.lblParamFileInfo.Text = "Currently Loaded Template: "
        '
        'frmMainGUI
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(476, 709)
        Me.Controls.Add(Me.lblParamFileInfo)
        Me.Controls.Add(Me.tcMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.MainMenu1
        Me.Name = "frmMainGUI"
        Me.Text = "Sequest Parameter File Editor"
        Me.tcMain.ResumeLayout(False)
        Me.tabBasic.ResumeLayout(False)
        Me.gbxStaticMods.ResumeLayout(False)
        Me.gbxDesc.ResumeLayout(False)
        Me.gbxSearch.ResumeLayout(False)
        Me.gbxDynMods.ResumeLayout(False)
        Me.tabAdvanced.ResumeLayout(False)
        Me.gbxIonWeighting.ResumeLayout(False)
        Me.gbxMiscParams.ResumeLayout(False)
        Me.gbxToleranceValues.ResumeLayout(False)
        Me.gbxSwitches.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Shared MainCode As clsMainProcess

    Private basicTemplate As IBasicParams
    Private advTemplate As IAdvancedParams
    Public newParams As clsParams
    Private newBasic As IBasicParams
    Private newAdv As IAdvancedParams

    Private Sub frmMainGUI_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MainCode = New clsMainProcess(Me)

        Call AddBasicTabHandlers()
        Call AddAdvTabHandlers()
    End Sub

    Private Sub AddBasicTabHandlers()
        AddHandler txtDescription.TextChanged, AddressOf txtDescription_TextChanged

        AddHandler cboParentMassType.SelectedIndexChanged, AddressOf cboParentMassType_SelectedIndexChanged
        AddHandler cboFragmentMassType.SelectedIndexChanged, AddressOf cboFragmentMassType_SelectedIndexChanged
        AddHandler cboEnzymeSelect.SelectedIndexChanged, AddressOf cboEnzymeSelect_SelectedIndexChanged
        AddHandler cboMissedCleavages.SelectedIndexChanged, AddressOf cboMissedCleavages_SelectedIndexChanged
        AddHandler txtPartialSeq.TextChanged, AddressOf txtPartialSeq_TextChanged

        AddHandler txtDynMod1List.TextChanged, AddressOf txtDynMod1List_TextChanged
        AddHandler txtDynMod2List.TextChanged, AddressOf txtDynMod2List_TextChanged
        AddHandler txtDynMod3List.TextChanged, AddressOf txtDynMod3List_TextChanged
        AddHandler txtDynMod1MassDiff.TextChanged, AddressOf txtDynMod1MassDiff_TextChanged
        AddHandler txtDynMod2MassDiff.TextChanged, AddressOf txtDynMod2MassDiff_TextChanged
        AddHandler txtDynMod3MassDiff.TextChanged, AddressOf txtDynMod3MassDiff_TextChanged

        AddHandler txtCTPep.TextChanged, AddressOf txtCTPep_TextChanged
        AddHandler txtCTProt.TextChanged, AddressOf txtCTProt_TextChanged
        AddHandler txtNTPep.TextChanged, AddressOf txtNTPep_TextChanged
        AddHandler txtNTProt.TextChanged, AddressOf txtNTProt_TextChanged
        AddHandler txtGly.TextChanged, AddressOf txtGly_TextChanged
        AddHandler txtAla.TextChanged, AddressOf txtAla_TextChanged
        AddHandler txtSer.TextChanged, AddressOf txtSer_TextChanged
        AddHandler txtPro.TextChanged, AddressOf txtPro_TextChanged
        AddHandler txtVal.TextChanged, AddressOf txtVal_TextChanged
        AddHandler txtThr.TextChanged, AddressOf txtThr_TextChanged
        AddHandler txtCys.TextChanged, AddressOf txtCys_TextChanged
        AddHandler txtLeu.TextChanged, AddressOf txtLeu_TextChanged
        AddHandler txtIle.TextChanged, AddressOf txtIle_TextChanged
        AddHandler TxtLorI.TextChanged, AddressOf TxtLorI_TextChanged
        AddHandler txtAsn.TextChanged, AddressOf txtAsn_TextChanged
        AddHandler txtOrn.TextChanged, AddressOf txtOrn_TextChanged
        AddHandler txtNandD.TextChanged, AddressOf txtNandD_TextChanged
        AddHandler txtAsp.TextChanged, AddressOf txtAsp_TextChanged
        AddHandler txtGln.TextChanged, AddressOf txtGln_TextChanged
        AddHandler txtLys.TextChanged, AddressOf txtLys_TextChanged
        AddHandler txtQandE.TextChanged, AddressOf txtQandE_TextChanged
        AddHandler txtGlu.TextChanged, AddressOf txtGlu_TextChanged
        AddHandler txtMet.TextChanged, AddressOf txtMet_TextChanged
        AddHandler txtHis.TextChanged, AddressOf txtHis_TextChanged
        AddHandler txtPhe.TextChanged, AddressOf txtPhe_TextChanged
        AddHandler txtArg.TextChanged, AddressOf txtArg_TextChanged
        AddHandler txtTyr.TextChanged, AddressOf txtTyr_TextChanged
        AddHandler txtTrp.TextChanged, AddressOf txtTrp_TextChanged

    End Sub
    Private Sub AddAdvTabHandlers()
        AddHandler txtPepMassTol.TextChanged, AddressOf txtPepMassTol_TextChanged
        AddHandler txtFragMassTol.TextChanged, AddressOf txtFragMassTol_TextChanged
        AddHandler txtPeakMatchingTol.TextChanged, AddressOf txtPeakMatchingTol_TextChanged
        AddHandler txtIonCutoff.TextChanged, AddressOf txtIonCutoff_TextChanged
        AddHandler txtMinProtMass.TextChanged, AddressOf txtMinProtMass_TextChanged
        AddHandler txtMaxProtMass.TextChanged, AddressOf txtMaxProtMass_TextChanged

        AddHandler txtNumOutputLines.TextChanged, AddressOf txtNumOutputLines_TextChanged
        AddHandler txtNumDescLines.TextChanged, AddressOf txtNumDescLines_TextChanged
        AddHandler txtMatchPeakCount.TextChanged, AddressOf txtMatchPeakCount_TextChanged
        AddHandler txtMatchPeakCountErrors.TextChanged, AddressOf txtMatchPeakCountErrors_TextChanged
        AddHandler txtMaxAAPerDynMod.TextChanged, AddressOf txtMaxAAPerDynMod_TextChanged
        AddHandler txtSeqHdrFilter.TextChanged, AddressOf txtSeqHdrFilter_TextChanged
        AddHandler cboNucReadingFrame.SelectedIndexChanged, AddressOf cboNucReadingFrame_SelectedIndexChanged

        AddHandler chkUseAIons.CheckedChanged, AddressOf chkUseAIons_CheckedChanged
        AddHandler chkUseBIons.CheckedChanged, AddressOf chkUseBIons_CheckedChanged
        AddHandler chkUseYIons.CheckedChanged, AddressOf chkUseYIons_CheckedChanged
        AddHandler chkCreateOutputFiles.CheckedChanged, AddressOf chkCreateOutputFiles_CheckedChanged
        AddHandler chkShowFragmentIons.CheckedChanged, AddressOf chkShowFragmentIons_CheckedChanged
        AddHandler chkRemovePrecursorPeaks.CheckedChanged, AddressOf chkRemovePrecursorPeaks_CheckedChanged
        AddHandler chkPrintDupRefs.CheckedChanged, AddressOf chkPrintDupRefs_CheckedChanged
        AddHandler chkResiduesInUpperCase.CheckedChanged, AddressOf chkResiduesInUpperCase_CheckedChanged

        AddHandler txtAWeight.TextChanged, AddressOf txtAWeight_TextChanged
        AddHandler txtBWeight.TextChanged, AddressOf txtBWeight_TextChanged
        AddHandler txtCWeight.TextChanged, AddressOf txtCWeight_TextChanged
        AddHandler txtDWeight.TextChanged, AddressOf txtDWeight_TextChanged
        AddHandler txtVWeight.TextChanged, AddressOf txtVWeight_TextChanged
        AddHandler txtWWeight.TextChanged, AddressOf txtWWeight_TextChanged
        AddHandler txtXWeight.TextChanged, AddressOf txtXWeight_TextChanged
        AddHandler txtYWeight.TextChanged, AddressOf txtYWeight_TextChanged
        AddHandler txtZWeight.TextChanged, AddressOf txtZWeight_TextChanged

    End Sub
    Private Sub LoadParamsetFromDMS(ByVal ParamSetID As Integer)
        If m_clsParamsFromDMS Is Nothing Then
            m_clsParamsFromDMS = LoadDMSParamsClass(Me.MainCode.mySettings)
        End If
        'Dim cls As New clsParamsFromDMS(Me.MainCode.mySettings)
        'Dim DMSParams As clsParams = m_clsParamsFromDMS.ReadParamsFromDMS(ParamSetID)
        newParams = m_clsParamsFromDMS.ReadParamsFromDMS(ParamSetID)

        Me.MainCode.RefreshTabs(Me, newParams)
    End Sub

    Private Sub LoadParamsetFromFile(ByVal FilePath As String)
        newParams.LoadTemplate(FilePath)
        Me.MainCode.RefreshTabs(Me, newParams)
    End Sub

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

#Region " [Basic] Name and Description Handlers "

    Private Sub txtDescription_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.Description = Me.txtDescription.Text
    End Sub

#End Region

#Region " [Basic] Search Settings Handlers "

    Private Sub cboParentMassType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.ParentMassType = CType(Me.cboParentMassType.SelectedIndex, IBasicParams.MassTypeList)
    End Sub
    Private Sub cboFragmentMassType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.FragmentMassType = CType(Me.cboFragmentMassType.SelectedIndex, IBasicParams.MassTypeList)
    End Sub
    Private Sub cboEnzymeSelect_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim tmpIndex As Integer
        tmpIndex = Me.cboEnzymeSelect.SelectedIndex
        newParams.SelectedEnzymeIndex = tmpIndex
        newParams.SelectedEnzymeDetails = newParams.RetrieveEnzymeDetails(tmpIndex)
    End Sub
    Private Sub cboMissedCleavages_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.MaximumNumberMissedCleavages = Me.cboMissedCleavages.SelectedIndex
    End Sub
    Private Sub txtPartialSeq_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.PartialSequenceToMatch = Me.txtPartialSeq.Text
    End Sub

#End Region

#Region " [Basic] Dynamic Modification Handlers "

    Private Sub txtDynMod1List_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.DynamicMods.Dyn_Mod_1_AAList = Me.txtDynMod1List.Text
    End Sub
    Private Sub txtDynMod2List_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.DynamicMods.Dyn_Mod_2_AAList = Me.txtDynMod2List.Text
    End Sub
    Private Sub txtDynMod3List_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.DynamicMods.Dyn_Mod_3_AAList = Me.txtDynMod3List.Text
    End Sub
    Private Sub txtDynMod1MassDiff_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.DynamicMods.Dyn_Mod_1_MassDiff = CSng(Me.txtDynMod1MassDiff.Text)
    End Sub
    Private Sub txtDynMod2MassDiff_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.DynamicMods.Dyn_Mod_2_MassDiff = CSng(Me.txtDynMod2MassDiff.Text)
    End Sub
    Private Sub txtDynMod3MassDiff_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.DynamicMods.Dyn_Mod_3_MassDiff = CSng(Me.txtDynMod3MassDiff.Text)
    End Sub

#End Region

#Region " [Basic] Static Modification Handlers "

    Private Sub txtCTPep_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.CtermPeptide = CSng(Me.txtCTPep.Text)
        Catch
            Me.txtCTPep.Text = "0.0"
        End Try
    End Sub
    Private Sub txtCTProt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.CtermProtein = CSng(Me.txtCTProt.Text)
        Catch
            Me.txtCTProt.Text = "0.0"
        End Try
    End Sub
    Private Sub txtNTPep_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.NtermPeptide = CSng(Me.txtNTPep.Text)
        Catch
            Me.txtNTPep.Text = "0.0"
        End Try
    End Sub
    Private Sub txtNTProt_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.NtermProtein = CSng(Me.txtNTProt.Text)
        Catch
            Me.txtNTProt.Text = "0.0"
        End Try
    End Sub
    Private Sub txtGly_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.G_Glycine = CSng(Me.txtGly.Text)
        Catch
            Me.txtGly.Text = "0.0"
        End Try
    End Sub
    Private Sub txtAla_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.A_Alanine = CSng(Me.txtAla.Text)
        Catch
            Me.txtAla.Text = "0.0"
        End Try
    End Sub
    Private Sub txtSer_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.S_Serine = CSng(Me.txtSer.Text)
        Catch
            Me.txtSer.Text = "0.0"
        End Try
    End Sub
    Private Sub txtPro_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.P_Proline = CSng(Me.txtPro.Text)
        Catch
            Me.txtPro.Text = "0.0"
        End Try
    End Sub
    Private Sub txtVal_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.V_Valine = CSng(Me.txtVal.Text)
        Catch
            Me.txtVal.Text = "0.0"
        End Try
    End Sub
    Private Sub txtThr_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.T_Threonine = CSng(Me.txtThr.Text)
        Catch
            Me.txtThr.Text = "0.0"
        End Try
    End Sub
    Private Sub txtCys_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.C_Cysteine = CSng(Me.txtCys.Text)
        Catch
            Me.txtCys.Text = "0.0"
        End Try
    End Sub
    Private Sub txtLeu_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.L_Leucine = CSng(Me.txtLeu.Text)
        Catch
            Me.txtLeu.Text = "0.0"
        End Try
    End Sub
    Private Sub txtIle_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.I_Isoleucine = CSng(Me.txtIle.Text)
        Catch
            Me.txtIle.Text = "0.0"
        End Try
    End Sub
    Private Sub TxtLorI_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.X_LorI = CSng(Me.TxtLorI.Text)
        Catch
            Me.TxtLorI.Text = "0.0"
        End Try
    End Sub
    Private Sub txtAsn_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.N_Asparagine = CSng(Me.txtAsn.Text)
        Catch
            Me.txtAsn.Text = "0.0"
        End Try
    End Sub
    Private Sub txtOrn_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.O_Ornithine = CSng(Me.txtOrn.Text)
        Catch
            Me.txtOrn.Text = "0.0"
        End Try
    End Sub
    Private Sub txtNandD_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.B_avg_NandD = CSng(Me.txtNandD.Text)
        Catch
            Me.txtNandD.Text = "0.0"
        End Try
    End Sub
    Private Sub txtAsp_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.D_Aspartic_Acid = CSng(Me.txtAsp.Text)
        Catch
            Me.txtAsp.Text = "0.0"
        End Try
    End Sub
    Private Sub txtGln_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.Q_Glutamine = CSng(Me.txtGln.Text)
        Catch
            Me.txtGln.Text = "0.0"
        End Try
    End Sub
    Private Sub txtLys_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.K_Lysine = CSng(Me.txtLys.Text)
        Catch
            Me.txtLys.Text = "0.0"
        End Try
    End Sub
    Private Sub txtQandE_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.Z_avg_QandE = CSng(Me.txtQandE.Text)
        Catch
            Me.txtQandE.Text = "0.0"
        End Try
    End Sub
    Private Sub txtGlu_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.E_Glutamic_Acid = CSng(Me.txtGlu.Text)
        Catch
            Me.txtGlu.Text = "0.0"
        End Try
    End Sub
    Private Sub txtMet_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.M_Methionine = CSng(Me.txtMet.Text)
        Catch
            Me.txtMet.Text = "0.0"
        End Try
    End Sub
    Private Sub txtHis_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.H_Histidine = CSng(Me.txtHis.Text)
        Catch
            Me.txtHis.Text = "0.0"
        End Try
    End Sub
    Private Sub txtPhe_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.F_Phenylalanine = CSng(Me.txtPhe.Text)
        Catch
            Me.txtPhe.Text = "0.0"
        End Try
    End Sub
    Private Sub txtArg_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.R_Arginine = CSng(Me.txtArg.Text)
        Catch
            Me.txtArg.Text = "0.0"
        End Try
    End Sub
    Private Sub txtTyr_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.Y_Tyrosine = CSng(Me.txtTyr.Text)
        Catch
            Me.txtTyr.Text = "0.0"
        End Try
    End Sub
    Private Sub txtTrp_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            newParams.StaticModificationsList.W_Tryptophan = CSng(Me.txtTrp.Text)
        Catch
            Me.txtTrp.Text = "0.0"
        End Try
    End Sub

#End Region

#Region " [Advanced] Searching Tolerances "

    Private Sub txtPepMassTol_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.PeptideMassTolerance = CSng(Me.txtPepMassTol.Text)
    End Sub
    Private Sub txtFragMassTol_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.FragmentIonTolerance = CSng(Me.txtFragMassTol.Text)
    End Sub
    Private Sub txtPeakMatchingTol_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.MatchedPeakMassTolerance = CSng(Me.txtPeakMatchingTol.Text)
    End Sub
    Private Sub txtIonCutoff_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.IonCutoffPercentage = CSng(Me.txtIonCutoff.Text)
    End Sub
    Private Sub txtMinProtMass_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.MinimumProteinMassToSearch = CInt(Me.txtMinProtMass.Text)
    End Sub
    Private Sub txtMaxProtMass_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.MaximumProteinMassToSearch = CInt(Me.txtMaxProtMass.Text)
    End Sub

#End Region

#Region " [Advanced] Miscellaneous Options "

    Private Sub txtNumOutputLines_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.NumberOfOutputLines = CInt(Me.txtNumOutputLines.Text)
    End Sub

    Private Sub txtNumDescLines_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.NumberOfDescriptionLines = CInt(Me.txtNumDescLines.Text)
    End Sub

    Private Sub txtMatchPeakCount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.NumberOfDetectedPeaksToMatch = CInt(Me.txtMatchPeakCountErrors.Text)
    End Sub

    Private Sub txtMatchPeakCountErrors_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.NumberOfAllowedDetectedPeakErrors = CInt(Me.txtMatchPeakCountErrors.Text)
    End Sub

    Private Sub txtMaxAAPerDynMod_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.MaximumNumAAPerDynMod = CInt(Me.txtMaxAAPerDynMod.Text)
    End Sub

    Private Sub txtSeqHdrFilter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.SequenceHeaderInfoToFilter = Me.txtSeqHdrFilter.Text
    End Sub

    Private Sub cboNucReadingFrame_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.SelectedNucReadingFrame = CType(Me.cboNucReadingFrame.SelectedIndex, Integer)
    End Sub

#End Region

#Region " [Advanced] Option Checkboxes "

    Private Sub chkUseAIons_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.IonSeries.Use_a_Ions = Me.chkUseAIons.Checked
    End Sub

    Private Sub chkUseBIons_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.IonSeries.Use_b_Ions = Me.chkUseBIons.Checked
    End Sub

    Private Sub chkUseYIons_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.IonSeries.Use_y_Ions = Me.chkUseYIons.Checked
    End Sub

    Private Sub chkCreateOutputFiles_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.CreateOutputFiles = Me.chkCreateOutputFiles.Checked
    End Sub

    Private Sub chkShowFragmentIons_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.ShowFragmentIons = Me.chkShowFragmentIons.Checked
    End Sub

    Private Sub chkRemovePrecursorPeaks_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.RemovePrecursorPeak = Me.chkRemovePrecursorPeaks.Checked
    End Sub

    Private Sub chkPrintDupRefs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.PrintDuplicateReferences = Me.chkPrintDupRefs.Checked
    End Sub

    Private Sub chkResiduesInUpperCase_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.AminoAcidsAllUpperCase = Me.chkResiduesInUpperCase.Checked
    End Sub

#End Region

#Region " [Advanced] Ion Weighting Constants"

    Private Sub txtAWeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.IonSeries.a_Ion_Weighting = CSng(Me.txtAWeight.Text)
    End Sub

    Private Sub txtBWeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.IonSeries.b_Ion_Weighting = CSng(Me.txtBWeight.Text)
    End Sub

    Private Sub txtCWeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.IonSeries.c_Ion_Weighting = CSng(Me.txtCWeight.Text)
    End Sub

    Private Sub txtDWeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.IonSeries.d_Ion_Weighting = CSng(Me.txtDWeight.Text)
    End Sub

    Private Sub txtVWeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.IonSeries.v_Ion_Weighting = CSng(Me.txtVWeight.Text)
    End Sub

    Private Sub txtWWeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.IonSeries.w_Ion_Weighting = CSng(Me.txtWWeight.Text)
    End Sub

    Private Sub txtXWeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.IonSeries.x_Ion_Weighting = CSng(Me.txtXWeight.Text)
    End Sub

    Private Sub txtYWeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.IonSeries.y_Ion_Weighting = CSng(Me.txtYWeight.Text)
    End Sub

    Private Sub txtZWeight_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        newParams.IonSeries.z_Ion_Weighting = CSng(Me.txtZWeight.Text)
    End Sub

#End Region


    Private Sub mnuFileSaveBW2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSaveBW2.Click
        Dim FileOutput As New clsWriteOutput
        Dim newFilePath As String

        Dim SaveDialog As New SaveFileDialog

        With SaveDialog
            .Title = "Save Sequest/BioWorks v2.0 Parameter File"
            .FileName = newParams.FileName
        End With

        If SaveDialog.ShowDialog = DialogResult.OK Then
            newFilePath = SaveDialog.FileName
        End If
        Call FileOutput.WriteOutputFile(newParams, newFilePath, clsParams.ParamFileTypes.BioWorks_20)
    End Sub
    Private Sub mnuFileSaveBW3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileSaveBW3.Click
        Dim FileOutput As New clsWriteOutput
        Dim newFilePath As String

        Dim SaveDialog As New SaveFileDialog

        With SaveDialog
            .Title = "Save Sequest/BioWorks v3.0 Parameter File"

        End With

        If SaveDialog.ShowDialog = DialogResult.OK Then
            newFilePath = SaveDialog.FileName
        End If
        Call FileOutput.WriteOutputFile(newParams, newFilePath, clsParams.ParamFileTypes.BioWorks_30)
    End Sub
    Private Sub mnuToolsMassHelper_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuToolsMassHelper.Click
        frmMH = New frmMassHelper
        If Me.mnuToolsMassHelper.Checked = False Then
            With frmMH
                .Show()
                .Top = Me.Top
                .Left = Me.Left + Me.Width + 10
            End With
        ElseIf Me.mnuToolsMassHelper.Checked = True Then
            frmMH.Hide()
        End If
    End Sub
    Private Sub mnuHelpAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelpAbout.Click
        Dim AboutBox As New frmAboutBox
        AboutBox.Show()
    End Sub

    Private Sub mnuFileLoadFromDMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFileLoadFromDMS.Click
        'Dim DMSParams As New ParamFileEditor.DownloadParams.clsParamsFromDMS(Me.MainCode.mySettings)
        Dim frmPicker As New frmDMSPicker(Me)
        frmPicker.MySettings = Me.MainCode.mySettings
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
            Call LoadParamsFromFile(newFilePath)
        End If
    End Sub

    Private Sub mnuBatchUploadDMS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuBatchUploadDMS.Click
        Dim batch As clsBatchLoadTemplates = New clsBatchLoadTemplates(Me.MainCode)
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

    Private Function ConvertStringArrayToSC(ByVal stringArray As String()) As System.collections.specialized.StringCollection
        Dim maxCount As Integer = UBound(stringArray)
        Dim sc As New System.Collections.Specialized.StringCollection
        Dim count As Integer

        For count = 0 To maxCount
            sc.Add(stringArray(count))
        Next

        Return sc
    End Function

End Class