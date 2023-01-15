Option Strict On
Option Infer On

Imports System.Collections.Generic
Imports System.ComponentModel
Imports ParamFileEditor.ProgramSettings
Imports ParamFileGenerator
Imports System.IO
Imports System.Linq
Imports System.Text.RegularExpressions
Imports ParamFileGenerator.Parameters
Imports PRISMDatabaseUtils

Public Class frmMainGUI
    Inherits Form

    Private m_clsParamsFromDMS As ParamsFromDMS
    Private m_clsMassTweaker As IMassTweaker

    Private m_DMSUpload As clsDMSParamUpload
    Friend WithEvents MenuItem2 As MenuItem

    ' ReSharper disable once NotAccessedField.Local
    ''' <summary>
    ''' This class needs to be instantiated so that we can read properties BaseLineParamSet and templateFileName
    ''' </summary>
    Private ReadOnly m_sharedMain As MainProcess

    Private m_CurrentDBTools As IDBTools

    Private m_CurrentConnectionString As String = String.Empty

#Region "Windows Form Designer generated code"

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        CheckForSettingsFileExistence()
        CheckForParamFileExistence()
        m_sharedMain = New MainProcess()
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMainGUI))
        Me.tcMain = New System.Windows.Forms.TabControl()
        Me.tabBasic = New System.Windows.Forms.TabPage()
        Me.gbxIsoMods = New System.Windows.Forms.GroupBox()
        Me.lblIsoS = New System.Windows.Forms.Label()
        Me.lblIsoN = New System.Windows.Forms.Label()
        Me.lblIsoH = New System.Windows.Forms.Label()
        Me.lblIsoC = New System.Windows.Forms.Label()
        Me.lblIsoO = New System.Windows.Forms.Label()
        Me.cmdReTweak = New System.Windows.Forms.Button()
        Me.chkAutoTweak = New System.Windows.Forms.CheckBox()
        Me.gbxStaticMods = New System.Windows.Forms.GroupBox()
        Me.lblCTPep = New System.Windows.Forms.Label()
        Me.lblCTProt = New System.Windows.Forms.Label()
        Me.lblNTPep = New System.Windows.Forms.Label()
        Me.lblNTProt = New System.Windows.Forms.Label()
        Me.lblGly = New System.Windows.Forms.Label()
        Me.lblAla = New System.Windows.Forms.Label()
        Me.lblSer = New System.Windows.Forms.Label()
        Me.lblCys = New System.Windows.Forms.Label()
        Me.lblLorI = New System.Windows.Forms.Label()
        Me.lblThr = New System.Windows.Forms.Label()
        Me.lblVal = New System.Windows.Forms.Label()
        Me.lblLeu = New System.Windows.Forms.Label()
        Me.lblIle = New System.Windows.Forms.Label()
        Me.lblPro = New System.Windows.Forms.Label()
        Me.lblAsn = New System.Windows.Forms.Label()
        Me.lblGln = New System.Windows.Forms.Label()
        Me.lblQandE = New System.Windows.Forms.Label()
        Me.lblNandD = New System.Windows.Forms.Label()
        Me.lblOrn = New System.Windows.Forms.Label()
        Me.lblAsp = New System.Windows.Forms.Label()
        Me.lblLys = New System.Windows.Forms.Label()
        Me.lblArg = New System.Windows.Forms.Label()
        Me.lblTrp = New System.Windows.Forms.Label()
        Me.lblHis = New System.Windows.Forms.Label()
        Me.lblMet = New System.Windows.Forms.Label()
        Me.lblPhe = New System.Windows.Forms.Label()
        Me.lblTyr = New System.Windows.Forms.Label()
        Me.lblGlu = New System.Windows.Forms.Label()
        Me.gbxDesc = New System.Windows.Forms.GroupBox()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.gbxSearch = New System.Windows.Forms.GroupBox()
        Me.txtPartialSeq = New System.Windows.Forms.TextBox()
        Me.lblPartialSeq = New System.Windows.Forms.Label()
        Me.cboParentMassUnits = New System.Windows.Forms.ComboBox()
        Me.cboFragmentMassUnits = New System.Windows.Forms.ComboBox()
        Me.cboFragmentMassType = New System.Windows.Forms.ComboBox()
        Me.cboMissedCleavages = New System.Windows.Forms.ComboBox()
        Me.cboParentMassType = New System.Windows.Forms.ComboBox()
        Me.lblFragmentMassUnits = New System.Windows.Forms.Label()
        Me.lblParentMassUnits = New System.Windows.Forms.Label()
        Me.lblParentMassType = New System.Windows.Forms.Label()
        Me.cboEnzymeSelect = New System.Windows.Forms.ComboBox()
        Me.lblEnzymeSelect = New System.Windows.Forms.Label()
        Me.lblMissedCleavages = New System.Windows.Forms.Label()
        Me.lblFragmentMassType = New System.Windows.Forms.Label()
        Me.cboCleavagePosition = New System.Windows.Forms.ComboBox()
        Me.lblCleavagePosition = New System.Windows.Forms.Label()
        Me.gbxDynMods = New System.Windows.Forms.GroupBox()
        Me.lblDynCTPep = New System.Windows.Forms.Label()
        Me.lblDynNTPep = New System.Windows.Forms.Label()
        Me.txtDynMod1List = New System.Windows.Forms.TextBox()
        Me.txtDynMod2List = New System.Windows.Forms.TextBox()
        Me.txtDynMod3List = New System.Windows.Forms.TextBox()
        Me.lblDynMod1List = New System.Windows.Forms.Label()
        Me.lblDynMod2List = New System.Windows.Forms.Label()
        Me.lblDynMod3List = New System.Windows.Forms.Label()
        Me.lblDynMod1MassDiff = New System.Windows.Forms.Label()
        Me.lblDynMod3MassDiff = New System.Windows.Forms.Label()
        Me.lblDynMod2MassDiff = New System.Windows.Forms.Label()
        Me.txtDynMod4List = New System.Windows.Forms.TextBox()
        Me.lblDynMod4List = New System.Windows.Forms.Label()
        Me.lblDynMod4MassDiff = New System.Windows.Forms.Label()
        Me.lblDynMod5MassDiff = New System.Windows.Forms.Label()
        Me.txtDynMod5List = New System.Windows.Forms.TextBox()
        Me.lblDynMod5List = New System.Windows.Forms.Label()
        Me.tabAdvanced = New System.Windows.Forms.TabPage()
        Me.gbxIonWeighting = New System.Windows.Forms.GroupBox()
        Me.lblWWeight = New System.Windows.Forms.Label()
        Me.lblXWeight = New System.Windows.Forms.Label()
        Me.lblVWeight = New System.Windows.Forms.Label()
        Me.lblYWeight = New System.Windows.Forms.Label()
        Me.lblZWeight = New System.Windows.Forms.Label()
        Me.txtWWeight = New System.Windows.Forms.TextBox()
        Me.txtXWeight = New System.Windows.Forms.TextBox()
        Me.txtDWeight = New System.Windows.Forms.TextBox()
        Me.lblDWeight = New System.Windows.Forms.Label()
        Me.txtCWeight = New System.Windows.Forms.TextBox()
        Me.lblCWeight = New System.Windows.Forms.Label()
        Me.txtBWeight = New System.Windows.Forms.TextBox()
        Me.lblBWeight = New System.Windows.Forms.Label()
        Me.txtVWeight = New System.Windows.Forms.TextBox()
        Me.txtYWeight = New System.Windows.Forms.TextBox()
        Me.txtZWeight = New System.Windows.Forms.TextBox()
        Me.txtAWeight = New System.Windows.Forms.TextBox()
        Me.lblAWeight = New System.Windows.Forms.Label()
        Me.chkUseAIons = New System.Windows.Forms.CheckBox()
        Me.chkUseBIons = New System.Windows.Forms.CheckBox()
        Me.chkUseYIons = New System.Windows.Forms.CheckBox()
        Me.gbxMiscParams = New System.Windows.Forms.GroupBox()
        Me.lblNumResults = New System.Windows.Forms.Label()
        Me.txtNumResults = New System.Windows.Forms.TextBox()
        Me.cboNucReadingFrame = New System.Windows.Forms.ComboBox()
        Me.txtNumDescLines = New System.Windows.Forms.TextBox()
        Me.lblOutputLines = New System.Windows.Forms.Label()
        Me.txtNumOutputLines = New System.Windows.Forms.TextBox()
        Me.lblNumDescLines = New System.Windows.Forms.Label()
        Me.txtMatchPeakCountErrors = New System.Windows.Forms.TextBox()
        Me.lblMatchPeakCountErrors = New System.Windows.Forms.Label()
        Me.lblMatchPeakCount = New System.Windows.Forms.Label()
        Me.txtMatchPeakCount = New System.Windows.Forms.TextBox()
        Me.txtMaxDiffPerPeptide = New System.Windows.Forms.TextBox()
        Me.lblMaxAAPerDynMod = New System.Windows.Forms.Label()
        Me.txtMaxAAPerDynMod = New System.Windows.Forms.TextBox()
        Me.lblNucReadingFrame = New System.Windows.Forms.Label()
        Me.lblSeqHdrFilter = New System.Windows.Forms.Label()
        Me.gbxToleranceValues = New System.Windows.Forms.GroupBox()
        Me.txtFragMassTol = New System.Windows.Forms.TextBox()
        Me.lblPepMassTol = New System.Windows.Forms.Label()
        Me.txtPepMassTol = New System.Windows.Forms.TextBox()
        Me.lblFragMassTol = New System.Windows.Forms.Label()
        Me.txtIonCutoff = New System.Windows.Forms.TextBox()
        Me.lblIonCutoff = New System.Windows.Forms.Label()
        Me.lblPeakMatchingTol = New System.Windows.Forms.Label()
        Me.txtPeakMatchingTol = New System.Windows.Forms.TextBox()
        Me.lblMaxProtMass = New System.Windows.Forms.Label()
        Me.txtMaxProtMass = New System.Windows.Forms.TextBox()
        Me.lblMinProtMass = New System.Windows.Forms.Label()
        Me.txtMinProtMass = New System.Windows.Forms.TextBox()
        Me.gbxSwitches = New System.Windows.Forms.GroupBox()
        Me.chkResiduesInUpperCase = New System.Windows.Forms.CheckBox()
        Me.chkPrintDupRefs = New System.Windows.Forms.CheckBox()
        Me.chkRemovePrecursorPeaks = New System.Windows.Forms.CheckBox()
        Me.chkShowFragmentIons = New System.Windows.Forms.CheckBox()
        Me.chkCreateOutputFiles = New System.Windows.Forms.CheckBox()
        Me.mnuMain = New System.Windows.Forms.MainMenu(Me.components)
        Me.mnuFile = New System.Windows.Forms.MenuItem()
        Me.mnuFileLoadFromDMS = New System.Windows.Forms.MenuItem()
        Me.mnuLoadFromFile = New System.Windows.Forms.MenuItem()
        Me.MenuItem2 = New System.Windows.Forms.MenuItem()
        Me.mnuFileSaveToFile = New System.Windows.Forms.MenuItem()
        Me.mnuFileSaveBW2 = New System.Windows.Forms.MenuItem()
        Me.mnuFileSaveBW3 = New System.Windows.Forms.MenuItem()
        Me.mnuFileSaveBW32 = New System.Windows.Forms.MenuItem()
        Me.mnuFileUploadDMS = New System.Windows.Forms.MenuItem()
        Me.mnuBatchUploadDMS = New System.Windows.Forms.MenuItem()
        Me.mnuDiv1 = New System.Windows.Forms.MenuItem()
        Me.mnuFileExit = New System.Windows.Forms.MenuItem()
        Me.MenuItem1 = New System.Windows.Forms.MenuItem()
        Me.mnuOptionsAutoTweakParams = New System.Windows.Forms.MenuItem()
        Me.mnuHelp = New System.Windows.Forms.MenuItem()
        Me.mnuHelpAbout = New System.Windows.Forms.MenuItem()
        Me.mnuDebug = New System.Windows.Forms.MenuItem()
        Me.mnuDebugSyncAll = New System.Windows.Forms.MenuItem()
        Me.mnuDebugSyncSingle = New System.Windows.Forms.MenuItem()
        Me.mnuDebugSyncDesc = New System.Windows.Forms.MenuItem()
        Me.StatModErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.tooltipProvider = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtParamInfo = New ParamFileEditor.NumericTextBox()
        Me.txtIsoS = New ParamFileEditor.NumericTextBox()
        Me.txtIsoH = New ParamFileEditor.NumericTextBox()
        Me.txtIsoN = New ParamFileEditor.NumericTextBox()
        Me.txtIsoO = New ParamFileEditor.NumericTextBox()
        Me.txtIsoC = New ParamFileEditor.NumericTextBox()
        Me.txtCTPep = New ParamFileEditor.NumericTextBox()
        Me.txtAla = New ParamFileEditor.NumericTextBox()
        Me.txtCTProt = New ParamFileEditor.NumericTextBox()
        Me.txtNTPep = New ParamFileEditor.NumericTextBox()
        Me.txtNTProt = New ParamFileEditor.NumericTextBox()
        Me.txtGly = New ParamFileEditor.NumericTextBox()
        Me.txtSer = New ParamFileEditor.NumericTextBox()
        Me.txtCys = New ParamFileEditor.NumericTextBox()
        Me.txtPro = New ParamFileEditor.NumericTextBox()
        Me.TxtLorI = New ParamFileEditor.NumericTextBox()
        Me.txtThr = New ParamFileEditor.NumericTextBox()
        Me.txtIle = New ParamFileEditor.NumericTextBox()
        Me.txtVal = New ParamFileEditor.NumericTextBox()
        Me.txtLeu = New ParamFileEditor.NumericTextBox()
        Me.txtNandD = New ParamFileEditor.NumericTextBox()
        Me.txtQandE = New ParamFileEditor.NumericTextBox()
        Me.txtAsn = New ParamFileEditor.NumericTextBox()
        Me.txtLys = New ParamFileEditor.NumericTextBox()
        Me.txtOrn = New ParamFileEditor.NumericTextBox()
        Me.txtGln = New ParamFileEditor.NumericTextBox()
        Me.txtAsp = New ParamFileEditor.NumericTextBox()
        Me.txtArg = New ParamFileEditor.NumericTextBox()
        Me.txtTrp = New ParamFileEditor.NumericTextBox()
        Me.txtGlu = New ParamFileEditor.NumericTextBox()
        Me.txtHis = New ParamFileEditor.NumericTextBox()
        Me.txtPhe = New ParamFileEditor.NumericTextBox()
        Me.txtTyr = New ParamFileEditor.NumericTextBox()
        Me.txtMet = New ParamFileEditor.NumericTextBox()
        Me.txtDynNTPep = New ParamFileEditor.NumericTextBox()
        Me.txtDynCTPep = New ParamFileEditor.NumericTextBox()
        Me.txtDynMod1MassDiff = New ParamFileEditor.NumericTextBox()
        Me.txtDynMod2MassDiff = New ParamFileEditor.NumericTextBox()
        Me.txtDynMod3MassDiff = New ParamFileEditor.NumericTextBox()
        Me.txtDynMod4MassDiff = New ParamFileEditor.NumericTextBox()
        Me.txtDynMod5MassDiff = New ParamFileEditor.NumericTextBox()
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
        Me.tcMain.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tcMain.Controls.Add(Me.tabBasic)
        Me.tcMain.Controls.Add(Me.tabAdvanced)
        Me.tcMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tcMain.ItemSize = New System.Drawing.Size(100, 20)
        Me.tcMain.Location = New System.Drawing.Point(0, 0)
        Me.tcMain.Name = "tcMain"
        Me.tcMain.SelectedIndex = 0
        Me.tcMain.Size = New System.Drawing.Size(633, 743)
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
        Me.tabBasic.Size = New System.Drawing.Size(625, 715)
        Me.tabBasic.TabIndex = 3
        Me.tabBasic.Text = "Basic Parameters"
        '
        'gbxIsoMods
        '
        Me.gbxIsoMods.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
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
        Me.gbxIsoMods.Location = New System.Drawing.Point(10, 605)
        Me.gbxIsoMods.Name = "gbxIsoMods"
        Me.gbxIsoMods.Size = New System.Drawing.Size(606, 71)
        Me.gbxIsoMods.TabIndex = 4
        Me.gbxIsoMods.TabStop = False
        Me.gbxIsoMods.Text = "Isotopic Modifications to Apply"
        '
        'lblIsoS
        '
        Me.lblIsoS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIsoS.Location = New System.Drawing.Point(427, 21)
        Me.lblIsoS.Name = "lblIsoS"
        Me.lblIsoS.Size = New System.Drawing.Size(101, 18)
        Me.lblIsoS.TabIndex = 8
        Me.lblIsoS.Text = "S - Sulfur"
        Me.lblIsoS.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblIsoN
        '
        Me.lblIsoN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIsoN.Location = New System.Drawing.Point(326, 21)
        Me.lblIsoN.Name = "lblIsoN"
        Me.lblIsoN.Size = New System.Drawing.Size(82, 18)
        Me.lblIsoN.TabIndex = 6
        Me.lblIsoN.Text = "N - Nitrogen"
        Me.lblIsoN.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblIsoH
        '
        Me.lblIsoH.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIsoH.Location = New System.Drawing.Point(115, 21)
        Me.lblIsoH.Name = "lblIsoH"
        Me.lblIsoH.Size = New System.Drawing.Size(87, 18)
        Me.lblIsoH.TabIndex = 2
        Me.lblIsoH.Text = "H - Hydrogen"
        Me.lblIsoH.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblIsoC
        '
        Me.lblIsoC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIsoC.Location = New System.Drawing.Point(14, 21)
        Me.lblIsoC.Name = "lblIsoC"
        Me.lblIsoC.Size = New System.Drawing.Size(77, 18)
        Me.lblIsoC.TabIndex = 0
        Me.lblIsoC.Text = "C- Carbon"
        Me.lblIsoC.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblIsoO
        '
        Me.lblIsoO.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIsoO.Location = New System.Drawing.Point(226, 21)
        Me.lblIsoO.Name = "lblIsoO"
        Me.lblIsoO.Size = New System.Drawing.Size(76, 18)
        Me.lblIsoO.TabIndex = 4
        Me.lblIsoO.Text = "O - Oxygen"
        Me.lblIsoO.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'cmdReTweak
        '
        Me.cmdReTweak.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cmdReTweak.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReTweak.Location = New System.Drawing.Point(422, 683)
        Me.cmdReTweak.Name = "cmdReTweak"
        Me.cmdReTweak.Size = New System.Drawing.Size(120, 24)
        Me.cmdReTweak.TabIndex = 6
        Me.cmdReTweak.Text = "&Retweak Masses"
        '
        'chkAutoTweak
        '
        Me.chkAutoTweak.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkAutoTweak.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAutoTweak.Location = New System.Drawing.Point(19, 678)
        Me.chkAutoTweak.Name = "chkAutoTweak"
        Me.chkAutoTweak.Size = New System.Drawing.Size(173, 28)
        Me.chkAutoTweak.TabIndex = 5
        Me.chkAutoTweak.Text = "Auto Tweak Masses?"
        '
        'gbxStaticMods
        '
        Me.gbxStaticMods.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
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
        Me.gbxStaticMods.Location = New System.Drawing.Point(10, 411)
        Me.gbxStaticMods.Name = "gbxStaticMods"
        Me.gbxStaticMods.Size = New System.Drawing.Size(606, 189)
        Me.gbxStaticMods.TabIndex = 3
        Me.gbxStaticMods.TabStop = False
        Me.gbxStaticMods.Text = "Static Modifications to Apply"
        '
        'lblCTPep
        '
        Me.lblCTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCTPep.Location = New System.Drawing.Point(8, 18)
        Me.lblCTPep.Name = "lblCTPep"
        Me.lblCTPep.Size = New System.Drawing.Size(84, 18)
        Me.lblCTPep.TabIndex = 1
        Me.lblCTPep.Text = "C-Term Pep"
        Me.lblCTPep.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblCTProt
        '
        Me.lblCTProt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCTProt.Location = New System.Drawing.Point(94, 18)
        Me.lblCTProt.Name = "lblCTProt"
        Me.lblCTProt.Size = New System.Drawing.Size(84, 18)
        Me.lblCTProt.TabIndex = 1
        Me.lblCTProt.Text = "C-Term Prot"
        Me.lblCTProt.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblNTPep
        '
        Me.lblNTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNTPep.Location = New System.Drawing.Point(179, 18)
        Me.lblNTPep.Name = "lblNTPep"
        Me.lblNTPep.Size = New System.Drawing.Size(84, 18)
        Me.lblNTPep.TabIndex = 1
        Me.lblNTPep.Text = "N-Term Pep"
        Me.lblNTPep.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblNTProt
        '
        Me.lblNTProt.BackColor = System.Drawing.Color.Transparent
        Me.lblNTProt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNTProt.Location = New System.Drawing.Point(264, 18)
        Me.lblNTProt.Name = "lblNTProt"
        Me.lblNTProt.Size = New System.Drawing.Size(84, 18)
        Me.lblNTProt.TabIndex = 1
        Me.lblNTProt.Text = "N-Term Prot"
        Me.lblNTProt.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblGly
        '
        Me.lblGly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGly.Location = New System.Drawing.Point(349, 18)
        Me.lblGly.Name = "lblGly"
        Me.lblGly.Size = New System.Drawing.Size(84, 18)
        Me.lblGly.TabIndex = 1
        Me.lblGly.Text = "Gly (G)"
        Me.lblGly.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblAla
        '
        Me.lblAla.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAla.Location = New System.Drawing.Point(434, 18)
        Me.lblAla.Name = "lblAla"
        Me.lblAla.Size = New System.Drawing.Size(84, 18)
        Me.lblAla.TabIndex = 1
        Me.lblAla.Text = "Ala (A)"
        Me.lblAla.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblSer
        '
        Me.lblSer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSer.Location = New System.Drawing.Point(520, 18)
        Me.lblSer.Name = "lblSer"
        Me.lblSer.Size = New System.Drawing.Size(84, 18)
        Me.lblSer.TabIndex = 1
        Me.lblSer.Text = "Ser (S)"
        Me.lblSer.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblCys
        '
        Me.lblCys.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCys.Location = New System.Drawing.Point(264, 60)
        Me.lblCys.Name = "lblCys"
        Me.lblCys.Size = New System.Drawing.Size(84, 18)
        Me.lblCys.TabIndex = 1
        Me.lblCys.Text = "Cys (C)"
        Me.lblCys.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblLorI
        '
        Me.lblLorI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLorI.Location = New System.Drawing.Point(520, 60)
        Me.lblLorI.Name = "lblLorI"
        Me.lblLorI.Size = New System.Drawing.Size(84, 18)
        Me.lblLorI.TabIndex = 1
        Me.lblLorI.Text = "L or I (X)"
        Me.lblLorI.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblThr
        '
        Me.lblThr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblThr.Location = New System.Drawing.Point(179, 60)
        Me.lblThr.Name = "lblThr"
        Me.lblThr.Size = New System.Drawing.Size(84, 18)
        Me.lblThr.TabIndex = 1
        Me.lblThr.Text = "Thr (T)"
        Me.lblThr.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblVal
        '
        Me.lblVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVal.Location = New System.Drawing.Point(94, 60)
        Me.lblVal.Name = "lblVal"
        Me.lblVal.Size = New System.Drawing.Size(84, 18)
        Me.lblVal.TabIndex = 1
        Me.lblVal.Text = "Val (V)"
        Me.lblVal.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblLeu
        '
        Me.lblLeu.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLeu.Location = New System.Drawing.Point(349, 60)
        Me.lblLeu.Name = "lblLeu"
        Me.lblLeu.Size = New System.Drawing.Size(84, 18)
        Me.lblLeu.TabIndex = 1
        Me.lblLeu.Text = "Leu (L)"
        Me.lblLeu.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblIle
        '
        Me.lblIle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIle.Location = New System.Drawing.Point(434, 60)
        Me.lblIle.Name = "lblIle"
        Me.lblIle.Size = New System.Drawing.Size(84, 18)
        Me.lblIle.TabIndex = 1
        Me.lblIle.Text = "Ile (I)"
        Me.lblIle.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPro
        '
        Me.lblPro.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPro.Location = New System.Drawing.Point(8, 60)
        Me.lblPro.Name = "lblPro"
        Me.lblPro.Size = New System.Drawing.Size(84, 18)
        Me.lblPro.TabIndex = 1
        Me.lblPro.Text = "Pro (P)"
        Me.lblPro.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblAsn
        '
        Me.lblAsn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAsn.Location = New System.Drawing.Point(8, 97)
        Me.lblAsn.Name = "lblAsn"
        Me.lblAsn.Size = New System.Drawing.Size(84, 18)
        Me.lblAsn.TabIndex = 1
        Me.lblAsn.Text = "Asn (N)"
        Me.lblAsn.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblGln
        '
        Me.lblGln.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGln.Location = New System.Drawing.Point(349, 97)
        Me.lblGln.Name = "lblGln"
        Me.lblGln.Size = New System.Drawing.Size(84, 18)
        Me.lblGln.TabIndex = 1
        Me.lblGln.Text = "Gln (Q)"
        Me.lblGln.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblQandE
        '
        Me.lblQandE.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblQandE.Location = New System.Drawing.Point(520, 97)
        Me.lblQandE.Name = "lblQandE"
        Me.lblQandE.Size = New System.Drawing.Size(84, 18)
        Me.lblQandE.TabIndex = 1
        Me.lblQandE.Text = "Avg Q && E (Z)"
        Me.lblQandE.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblNandD
        '
        Me.lblNandD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNandD.Location = New System.Drawing.Point(179, 97)
        Me.lblNandD.Name = "lblNandD"
        Me.lblNandD.Size = New System.Drawing.Size(84, 18)
        Me.lblNandD.TabIndex = 1
        Me.lblNandD.Text = "Avg N && D (B)"
        Me.lblNandD.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblOrn
        '
        Me.lblOrn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrn.Location = New System.Drawing.Point(94, 97)
        Me.lblOrn.Name = "lblOrn"
        Me.lblOrn.Size = New System.Drawing.Size(84, 18)
        Me.lblOrn.TabIndex = 1
        Me.lblOrn.Text = "Orn (O)"
        Me.lblOrn.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblAsp
        '
        Me.lblAsp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAsp.Location = New System.Drawing.Point(264, 97)
        Me.lblAsp.Name = "lblAsp"
        Me.lblAsp.Size = New System.Drawing.Size(84, 18)
        Me.lblAsp.TabIndex = 1
        Me.lblAsp.Text = "Asp (D)"
        Me.lblAsp.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblLys
        '
        Me.lblLys.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLys.Location = New System.Drawing.Point(434, 97)
        Me.lblLys.Name = "lblLys"
        Me.lblLys.Size = New System.Drawing.Size(84, 18)
        Me.lblLys.TabIndex = 1
        Me.lblLys.Text = "Lys (K)"
        Me.lblLys.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblArg
        '
        Me.lblArg.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArg.Location = New System.Drawing.Point(349, 136)
        Me.lblArg.Name = "lblArg"
        Me.lblArg.Size = New System.Drawing.Size(84, 18)
        Me.lblArg.TabIndex = 1
        Me.lblArg.Text = "Arg (R)"
        Me.lblArg.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblTrp
        '
        Me.lblTrp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTrp.Location = New System.Drawing.Point(520, 136)
        Me.lblTrp.Name = "lblTrp"
        Me.lblTrp.Size = New System.Drawing.Size(84, 18)
        Me.lblTrp.TabIndex = 1
        Me.lblTrp.Text = "Trp (W)"
        Me.lblTrp.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblHis
        '
        Me.lblHis.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHis.Location = New System.Drawing.Point(179, 136)
        Me.lblHis.Name = "lblHis"
        Me.lblHis.Size = New System.Drawing.Size(84, 18)
        Me.lblHis.TabIndex = 1
        Me.lblHis.Text = "His (H)"
        Me.lblHis.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblMet
        '
        Me.lblMet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMet.Location = New System.Drawing.Point(94, 136)
        Me.lblMet.Name = "lblMet"
        Me.lblMet.Size = New System.Drawing.Size(84, 18)
        Me.lblMet.TabIndex = 1
        Me.lblMet.Text = "Met (M)"
        Me.lblMet.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblPhe
        '
        Me.lblPhe.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPhe.Location = New System.Drawing.Point(264, 136)
        Me.lblPhe.Name = "lblPhe"
        Me.lblPhe.Size = New System.Drawing.Size(84, 18)
        Me.lblPhe.TabIndex = 1
        Me.lblPhe.Text = "Phe (F)"
        Me.lblPhe.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblTyr
        '
        Me.lblTyr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTyr.Location = New System.Drawing.Point(434, 136)
        Me.lblTyr.Name = "lblTyr"
        Me.lblTyr.Size = New System.Drawing.Size(84, 18)
        Me.lblTyr.TabIndex = 1
        Me.lblTyr.Text = "Tyr (Y)"
        Me.lblTyr.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblGlu
        '
        Me.lblGlu.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGlu.Location = New System.Drawing.Point(8, 136)
        Me.lblGlu.Name = "lblGlu"
        Me.lblGlu.Size = New System.Drawing.Size(84, 18)
        Me.lblGlu.TabIndex = 1
        Me.lblGlu.Text = "Glu (E)"
        Me.lblGlu.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'gbxDesc
        '
        Me.gbxDesc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbxDesc.Controls.Add(Me.txtDescription)
        Me.gbxDesc.Controls.Add(Me.lblDescription)
        Me.gbxDesc.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.gbxDesc.Location = New System.Drawing.Point(10, 5)
        Me.gbxDesc.Name = "gbxDesc"
        Me.gbxDesc.Size = New System.Drawing.Size(606, 110)
        Me.gbxDesc.TabIndex = 0
        Me.gbxDesc.TabStop = False
        Me.gbxDesc.Text = "Name and Description Information"
        '
        'txtDescription
        '
        Me.txtDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.Location = New System.Drawing.Point(14, 39)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ReadOnly = True
        Me.txtDescription.Size = New System.Drawing.Size(578, 58)
        Me.txtDescription.TabIndex = 1
        '
        'lblDescription
        '
        Me.lblDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.Location = New System.Drawing.Point(14, 23)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(317, 19)
        Me.lblDescription.TabIndex = 0
        Me.lblDescription.Text = "Parameter File Descriptive Text"
        '
        'gbxSearch
        '
        Me.gbxSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
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
        Me.gbxSearch.Location = New System.Drawing.Point(10, 120)
        Me.gbxSearch.Name = "gbxSearch"
        Me.gbxSearch.Size = New System.Drawing.Size(606, 162)
        Me.gbxSearch.TabIndex = 1
        Me.gbxSearch.TabStop = False
        Me.gbxSearch.Text = "Search Settings"
        '
        'txtPartialSeq
        '
        Me.txtPartialSeq.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartialSeq.Location = New System.Drawing.Point(320, 127)
        Me.txtPartialSeq.Name = "txtPartialSeq"
        Me.txtPartialSeq.Size = New System.Drawing.Size(265, 23)
        Me.txtPartialSeq.TabIndex = 15
        '
        'lblPartialSeq
        '
        Me.lblPartialSeq.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartialSeq.Location = New System.Drawing.Point(320, 111)
        Me.lblPartialSeq.Name = "lblPartialSeq"
        Me.lblPartialSeq.Size = New System.Drawing.Size(192, 18)
        Me.lblPartialSeq.TabIndex = 14
        Me.lblPartialSeq.Text = "Partial Sequence To Match"
        '
        'cboParentMassUnits
        '
        Me.cboParentMassUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboParentMassUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboParentMassUnits.Location = New System.Drawing.Point(218, 39)
        Me.cboParentMassUnits.Name = "cboParentMassUnits"
        Me.cboParentMassUnits.Size = New System.Drawing.Size(72, 25)
        Me.cboParentMassUnits.TabIndex = 3
        '
        'cboFragmentMassUnits
        '
        Me.cboFragmentMassUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFragmentMassUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboFragmentMassUnits.Location = New System.Drawing.Point(513, 39)
        Me.cboFragmentMassUnits.Name = "cboFragmentMassUnits"
        Me.cboFragmentMassUnits.Size = New System.Drawing.Size(72, 25)
        Me.cboFragmentMassUnits.TabIndex = 7
        '
        'cboFragmentMassType
        '
        Me.cboFragmentMassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFragmentMassType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboFragmentMassType.Location = New System.Drawing.Point(320, 39)
        Me.cboFragmentMassType.Name = "cboFragmentMassType"
        Me.cboFragmentMassType.Size = New System.Drawing.Size(186, 25)
        Me.cboFragmentMassType.TabIndex = 5
        '
        'cboMissedCleavages
        '
        Me.cboMissedCleavages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMissedCleavages.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMissedCleavages.Location = New System.Drawing.Point(320, 83)
        Me.cboMissedCleavages.Name = "cboMissedCleavages"
        Me.cboMissedCleavages.Size = New System.Drawing.Size(265, 25)
        Me.cboMissedCleavages.TabIndex = 11
        '
        'cboParentMassType
        '
        Me.cboParentMassType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboParentMassType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboParentMassType.Location = New System.Drawing.Point(14, 39)
        Me.cboParentMassType.Name = "cboParentMassType"
        Me.cboParentMassType.Size = New System.Drawing.Size(188, 25)
        Me.cboParentMassType.TabIndex = 1
        '
        'lblFragmentMassUnits
        '
        Me.lblFragmentMassUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFragmentMassUnits.Location = New System.Drawing.Point(512, 23)
        Me.lblFragmentMassUnits.Name = "lblFragmentMassUnits"
        Me.lblFragmentMassUnits.Size = New System.Drawing.Size(57, 23)
        Me.lblFragmentMassUnits.TabIndex = 6
        Me.lblFragmentMassUnits.Text = "Units"
        '
        'lblParentMassUnits
        '
        Me.lblParentMassUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblParentMassUnits.Location = New System.Drawing.Point(217, 23)
        Me.lblParentMassUnits.Name = "lblParentMassUnits"
        Me.lblParentMassUnits.Size = New System.Drawing.Size(58, 23)
        Me.lblParentMassUnits.TabIndex = 2
        Me.lblParentMassUnits.Text = "Units"
        '
        'lblParentMassType
        '
        Me.lblParentMassType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblParentMassType.Location = New System.Drawing.Point(14, 23)
        Me.lblParentMassType.Name = "lblParentMassType"
        Me.lblParentMassType.Size = New System.Drawing.Size(140, 23)
        Me.lblParentMassType.TabIndex = 0
        Me.lblParentMassType.Text = "Parent Ion Mass Type"
        '
        'cboEnzymeSelect
        '
        Me.cboEnzymeSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboEnzymeSelect.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboEnzymeSelect.Location = New System.Drawing.Point(14, 83)
        Me.cboEnzymeSelect.Name = "cboEnzymeSelect"
        Me.cboEnzymeSelect.Size = New System.Drawing.Size(276, 25)
        Me.cboEnzymeSelect.TabIndex = 9
        '
        'lblEnzymeSelect
        '
        Me.lblEnzymeSelect.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEnzymeSelect.Location = New System.Drawing.Point(14, 67)
        Me.lblEnzymeSelect.Name = "lblEnzymeSelect"
        Me.lblEnzymeSelect.Size = New System.Drawing.Size(159, 18)
        Me.lblEnzymeSelect.TabIndex = 8
        Me.lblEnzymeSelect.Text = "Enzyme Cleavage Rule"
        '
        'lblMissedCleavages
        '
        Me.lblMissedCleavages.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMissedCleavages.Location = New System.Drawing.Point(320, 67)
        Me.lblMissedCleavages.Name = "lblMissedCleavages"
        Me.lblMissedCleavages.Size = New System.Drawing.Size(240, 18)
        Me.lblMissedCleavages.TabIndex = 10
        Me.lblMissedCleavages.Text = "Number of Allowed Missed Cleavages"
        '
        'lblFragmentMassType
        '
        Me.lblFragmentMassType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFragmentMassType.Location = New System.Drawing.Point(320, 23)
        Me.lblFragmentMassType.Name = "lblFragmentMassType"
        Me.lblFragmentMassType.Size = New System.Drawing.Size(158, 23)
        Me.lblFragmentMassType.TabIndex = 4
        Me.lblFragmentMassType.Text = "Fragment Ion Mass Type"
        '
        'cboCleavagePosition
        '
        Me.cboCleavagePosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCleavagePosition.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCleavagePosition.Location = New System.Drawing.Point(14, 127)
        Me.cboCleavagePosition.Name = "cboCleavagePosition"
        Me.cboCleavagePosition.Size = New System.Drawing.Size(276, 25)
        Me.cboCleavagePosition.TabIndex = 13
        '
        'lblCleavagePosition
        '
        Me.lblCleavagePosition.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCleavagePosition.Location = New System.Drawing.Point(14, 111)
        Me.lblCleavagePosition.Name = "lblCleavagePosition"
        Me.lblCleavagePosition.Size = New System.Drawing.Size(178, 18)
        Me.lblCleavagePosition.TabIndex = 12
        Me.lblCleavagePosition.Text = "Enzyme Cleavage Positions"
        '
        'gbxDynMods
        '
        Me.gbxDynMods.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbxDynMods.Controls.Add(Me.txtDynNTPep)
        Me.gbxDynMods.Controls.Add(Me.lblDynCTPep)
        Me.gbxDynMods.Controls.Add(Me.txtDynCTPep)
        Me.gbxDynMods.Controls.Add(Me.lblDynNTPep)
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
        Me.gbxDynMods.Location = New System.Drawing.Point(10, 286)
        Me.gbxDynMods.Name = "gbxDynMods"
        Me.gbxDynMods.Size = New System.Drawing.Size(606, 120)
        Me.gbxDynMods.TabIndex = 2
        Me.gbxDynMods.TabStop = False
        Me.gbxDynMods.Text = "Dynamic Modifications to Apply"
        '
        'lblDynCTPep
        '
        Me.lblDynCTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDynCTPep.Location = New System.Drawing.Point(524, 69)
        Me.lblDynCTPep.Name = "lblDynCTPep"
        Me.lblDynCTPep.Size = New System.Drawing.Size(96, 16)
        Me.lblDynCTPep.TabIndex = 22
        Me.lblDynCTPep.Text = "C-Term Pep"
        '
        'lblDynNTPep
        '
        Me.lblDynNTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDynNTPep.Location = New System.Drawing.Point(524, 23)
        Me.lblDynNTPep.Name = "lblDynNTPep"
        Me.lblDynNTPep.Size = New System.Drawing.Size(96, 16)
        Me.lblDynNTPep.TabIndex = 20
        Me.lblDynNTPep.Text = "N-Term Pep"
        '
        'txtDynMod1List
        '
        Me.txtDynMod1List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDynMod1List.Location = New System.Drawing.Point(14, 39)
        Me.txtDynMod1List.Name = "txtDynMod1List"
        Me.txtDynMod1List.Size = New System.Drawing.Size(72, 23)
        Me.txtDynMod1List.TabIndex = 1
        '
        'txtDynMod2List
        '
        Me.txtDynMod2List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDynMod2List.Location = New System.Drawing.Point(116, 39)
        Me.txtDynMod2List.Name = "txtDynMod2List"
        Me.txtDynMod2List.Size = New System.Drawing.Size(72, 23)
        Me.txtDynMod2List.TabIndex = 5
        '
        'txtDynMod3List
        '
        Me.txtDynMod3List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDynMod3List.Location = New System.Drawing.Point(218, 39)
        Me.txtDynMod3List.Name = "txtDynMod3List"
        Me.txtDynMod3List.Size = New System.Drawing.Size(72, 23)
        Me.txtDynMod3List.TabIndex = 9
        '
        'lblDynMod1List
        '
        Me.lblDynMod1List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDynMod1List.Location = New System.Drawing.Point(14, 23)
        Me.lblDynMod1List.Name = "lblDynMod1List"
        Me.lblDynMod1List.Size = New System.Drawing.Size(96, 16)
        Me.lblDynMod1List.TabIndex = 0
        Me.lblDynMod1List.Text = "AA List 1"
        '
        'lblDynMod2List
        '
        Me.lblDynMod2List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDynMod2List.Location = New System.Drawing.Point(116, 23)
        Me.lblDynMod2List.Name = "lblDynMod2List"
        Me.lblDynMod2List.Size = New System.Drawing.Size(96, 16)
        Me.lblDynMod2List.TabIndex = 4
        Me.lblDynMod2List.Text = "AA List 2"
        '
        'lblDynMod3List
        '
        Me.lblDynMod3List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDynMod3List.Location = New System.Drawing.Point(218, 23)
        Me.lblDynMod3List.Name = "lblDynMod3List"
        Me.lblDynMod3List.Size = New System.Drawing.Size(96, 16)
        Me.lblDynMod3List.TabIndex = 8
        Me.lblDynMod3List.Text = "AA List 3"
        '
        'lblDynMod1MassDiff
        '
        Me.lblDynMod1MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDynMod1MassDiff.Location = New System.Drawing.Point(14, 69)
        Me.lblDynMod1MassDiff.Name = "lblDynMod1MassDiff"
        Me.lblDynMod1MassDiff.Size = New System.Drawing.Size(96, 16)
        Me.lblDynMod1MassDiff.TabIndex = 2
        Me.lblDynMod1MassDiff.Text = "Mass Delta 1"
        '
        'lblDynMod3MassDiff
        '
        Me.lblDynMod3MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDynMod3MassDiff.Location = New System.Drawing.Point(218, 69)
        Me.lblDynMod3MassDiff.Name = "lblDynMod3MassDiff"
        Me.lblDynMod3MassDiff.Size = New System.Drawing.Size(96, 16)
        Me.lblDynMod3MassDiff.TabIndex = 10
        Me.lblDynMod3MassDiff.Text = "Mass Delta 3"
        '
        'lblDynMod2MassDiff
        '
        Me.lblDynMod2MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDynMod2MassDiff.Location = New System.Drawing.Point(116, 69)
        Me.lblDynMod2MassDiff.Name = "lblDynMod2MassDiff"
        Me.lblDynMod2MassDiff.Size = New System.Drawing.Size(96, 16)
        Me.lblDynMod2MassDiff.TabIndex = 6
        Me.lblDynMod2MassDiff.Text = "Mass Delta 2"
        '
        'txtDynMod4List
        '
        Me.txtDynMod4List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDynMod4List.Location = New System.Drawing.Point(320, 42)
        Me.txtDynMod4List.Name = "txtDynMod4List"
        Me.txtDynMod4List.Size = New System.Drawing.Size(72, 23)
        Me.txtDynMod4List.TabIndex = 13
        '
        'lblDynMod4List
        '
        Me.lblDynMod4List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDynMod4List.Location = New System.Drawing.Point(320, 23)
        Me.lblDynMod4List.Name = "lblDynMod4List"
        Me.lblDynMod4List.Size = New System.Drawing.Size(96, 16)
        Me.lblDynMod4List.TabIndex = 12
        Me.lblDynMod4List.Text = "AA List 4"
        '
        'lblDynMod4MassDiff
        '
        Me.lblDynMod4MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDynMod4MassDiff.Location = New System.Drawing.Point(320, 69)
        Me.lblDynMod4MassDiff.Name = "lblDynMod4MassDiff"
        Me.lblDynMod4MassDiff.Size = New System.Drawing.Size(96, 16)
        Me.lblDynMod4MassDiff.TabIndex = 14
        Me.lblDynMod4MassDiff.Text = "Mass Delta 4"
        '
        'lblDynMod5MassDiff
        '
        Me.lblDynMod5MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDynMod5MassDiff.Location = New System.Drawing.Point(422, 69)
        Me.lblDynMod5MassDiff.Name = "lblDynMod5MassDiff"
        Me.lblDynMod5MassDiff.Size = New System.Drawing.Size(96, 16)
        Me.lblDynMod5MassDiff.TabIndex = 18
        Me.lblDynMod5MassDiff.Text = "Mass Delta 5"
        '
        'txtDynMod5List
        '
        Me.txtDynMod5List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDynMod5List.Location = New System.Drawing.Point(422, 42)
        Me.txtDynMod5List.Name = "txtDynMod5List"
        Me.txtDynMod5List.Size = New System.Drawing.Size(72, 23)
        Me.txtDynMod5List.TabIndex = 17
        '
        'lblDynMod5List
        '
        Me.lblDynMod5List.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDynMod5List.Location = New System.Drawing.Point(422, 23)
        Me.lblDynMod5List.Name = "lblDynMod5List"
        Me.lblDynMod5List.Size = New System.Drawing.Size(96, 16)
        Me.lblDynMod5List.TabIndex = 16
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
        Me.tabAdvanced.Size = New System.Drawing.Size(625, 715)
        Me.tabAdvanced.TabIndex = 1
        Me.tabAdvanced.Text = "Advanced Parameters"
        '
        'gbxIonWeighting
        '
        Me.gbxIonWeighting.Controls.Add(Me.lblWWeight)
        Me.gbxIonWeighting.Controls.Add(Me.lblXWeight)
        Me.gbxIonWeighting.Controls.Add(Me.lblVWeight)
        Me.gbxIonWeighting.Controls.Add(Me.lblYWeight)
        Me.gbxIonWeighting.Controls.Add(Me.lblZWeight)
        Me.gbxIonWeighting.Controls.Add(Me.txtWWeight)
        Me.gbxIonWeighting.Controls.Add(Me.txtXWeight)
        Me.gbxIonWeighting.Controls.Add(Me.txtDWeight)
        Me.gbxIonWeighting.Controls.Add(Me.lblDWeight)
        Me.gbxIonWeighting.Controls.Add(Me.txtCWeight)
        Me.gbxIonWeighting.Controls.Add(Me.lblCWeight)
        Me.gbxIonWeighting.Controls.Add(Me.txtBWeight)
        Me.gbxIonWeighting.Controls.Add(Me.lblBWeight)
        Me.gbxIonWeighting.Controls.Add(Me.txtVWeight)
        Me.gbxIonWeighting.Controls.Add(Me.txtYWeight)
        Me.gbxIonWeighting.Controls.Add(Me.txtZWeight)
        Me.gbxIonWeighting.Controls.Add(Me.txtAWeight)
        Me.gbxIonWeighting.Controls.Add(Me.lblAWeight)
        Me.gbxIonWeighting.Controls.Add(Me.chkUseAIons)
        Me.gbxIonWeighting.Controls.Add(Me.chkUseBIons)
        Me.gbxIonWeighting.Controls.Add(Me.chkUseYIons)
        Me.gbxIonWeighting.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.gbxIonWeighting.Location = New System.Drawing.Point(10, 443)
        Me.gbxIonWeighting.Name = "gbxIonWeighting"
        Me.gbxIonWeighting.Size = New System.Drawing.Size(609, 106)
        Me.gbxIonWeighting.TabIndex = 3
        Me.gbxIonWeighting.TabStop = False
        Me.gbxIonWeighting.Text = "Ion Weighting Parameters"
        '
        'lblWWeight
        '
        Me.lblWWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWWeight.Location = New System.Drawing.Point(251, 51)
        Me.lblWWeight.Name = "lblWWeight"
        Me.lblWWeight.Size = New System.Drawing.Size(90, 18)
        Me.lblWWeight.TabIndex = 14
        Me.lblWWeight.Text = "w Ion Weight"
        Me.lblWWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblXWeight
        '
        Me.lblXWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblXWeight.Location = New System.Drawing.Point(342, 51)
        Me.lblXWeight.Name = "lblXWeight"
        Me.lblXWeight.Size = New System.Drawing.Size(90, 18)
        Me.lblXWeight.TabIndex = 12
        Me.lblXWeight.Text = "x Ion Weight"
        Me.lblXWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblVWeight
        '
        Me.lblVWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVWeight.Location = New System.Drawing.Point(162, 51)
        Me.lblVWeight.Name = "lblVWeight"
        Me.lblVWeight.Size = New System.Drawing.Size(90, 18)
        Me.lblVWeight.TabIndex = 3
        Me.lblVWeight.Text = "v Ion Weight"
        Me.lblVWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblYWeight
        '
        Me.lblYWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblYWeight.Location = New System.Drawing.Point(427, 51)
        Me.lblYWeight.Name = "lblYWeight"
        Me.lblYWeight.Size = New System.Drawing.Size(90, 18)
        Me.lblYWeight.TabIndex = 3
        Me.lblYWeight.Text = "y Ion Weight"
        Me.lblYWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblZWeight
        '
        Me.lblZWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblZWeight.Location = New System.Drawing.Point(518, 51)
        Me.lblZWeight.Name = "lblZWeight"
        Me.lblZWeight.Size = New System.Drawing.Size(90, 18)
        Me.lblZWeight.TabIndex = 3
        Me.lblZWeight.Text = "z Ion Weight"
        Me.lblZWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtWWeight
        '
        Me.txtWWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWWeight.Location = New System.Drawing.Point(261, 69)
        Me.txtWWeight.Name = "txtWWeight"
        Me.txtWWeight.Size = New System.Drawing.Size(66, 23)
        Me.txtWWeight.TabIndex = 19
        '
        'txtXWeight
        '
        Me.txtXWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtXWeight.Location = New System.Drawing.Point(349, 69)
        Me.txtXWeight.Name = "txtXWeight"
        Me.txtXWeight.Size = New System.Drawing.Size(66, 23)
        Me.txtXWeight.TabIndex = 20
        '
        'txtDWeight
        '
        Me.txtDWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDWeight.Location = New System.Drawing.Point(489, 28)
        Me.txtDWeight.Name = "txtDWeight"
        Me.txtDWeight.Size = New System.Drawing.Size(66, 23)
        Me.txtDWeight.TabIndex = 17
        '
        'lblDWeight
        '
        Me.lblDWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDWeight.Location = New System.Drawing.Point(478, 10)
        Me.lblDWeight.Name = "lblDWeight"
        Me.lblDWeight.Size = New System.Drawing.Size(90, 18)
        Me.lblDWeight.TabIndex = 10
        Me.lblDWeight.Text = "d Ion Weight"
        Me.lblDWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtCWeight
        '
        Me.txtCWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCWeight.Location = New System.Drawing.Point(398, 28)
        Me.txtCWeight.Name = "txtCWeight"
        Me.txtCWeight.Size = New System.Drawing.Size(66, 23)
        Me.txtCWeight.TabIndex = 16
        '
        'lblCWeight
        '
        Me.lblCWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCWeight.Location = New System.Drawing.Point(384, 10)
        Me.lblCWeight.Name = "lblCWeight"
        Me.lblCWeight.Size = New System.Drawing.Size(90, 18)
        Me.lblCWeight.TabIndex = 8
        Me.lblCWeight.Text = "c Ion Weight"
        Me.lblCWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtBWeight
        '
        Me.txtBWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBWeight.Location = New System.Drawing.Point(307, 28)
        Me.txtBWeight.Name = "txtBWeight"
        Me.txtBWeight.Size = New System.Drawing.Size(66, 23)
        Me.txtBWeight.TabIndex = 15
        '
        'lblBWeight
        '
        Me.lblBWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBWeight.Location = New System.Drawing.Point(294, 10)
        Me.lblBWeight.Name = "lblBWeight"
        Me.lblBWeight.Size = New System.Drawing.Size(90, 18)
        Me.lblBWeight.TabIndex = 6
        Me.lblBWeight.Text = "b Ion Weight"
        Me.lblBWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtVWeight
        '
        Me.txtVWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVWeight.Location = New System.Drawing.Point(173, 69)
        Me.txtVWeight.Name = "txtVWeight"
        Me.txtVWeight.Size = New System.Drawing.Size(66, 23)
        Me.txtVWeight.TabIndex = 18
        '
        'txtYWeight
        '
        Me.txtYWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtYWeight.Location = New System.Drawing.Point(437, 69)
        Me.txtYWeight.Name = "txtYWeight"
        Me.txtYWeight.Size = New System.Drawing.Size(66, 23)
        Me.txtYWeight.TabIndex = 21
        '
        'txtZWeight
        '
        Me.txtZWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtZWeight.Location = New System.Drawing.Point(525, 69)
        Me.txtZWeight.Name = "txtZWeight"
        Me.txtZWeight.Size = New System.Drawing.Size(66, 23)
        Me.txtZWeight.TabIndex = 22
        '
        'txtAWeight
        '
        Me.txtAWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAWeight.Location = New System.Drawing.Point(216, 28)
        Me.txtAWeight.Name = "txtAWeight"
        Me.txtAWeight.Size = New System.Drawing.Size(66, 23)
        Me.txtAWeight.TabIndex = 14
        '
        'lblAWeight
        '
        Me.lblAWeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAWeight.Location = New System.Drawing.Point(203, 10)
        Me.lblAWeight.Name = "lblAWeight"
        Me.lblAWeight.Size = New System.Drawing.Size(90, 18)
        Me.lblAWeight.TabIndex = 3
        Me.lblAWeight.Text = "a Ion Weight"
        Me.lblAWeight.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'chkUseAIons
        '
        Me.chkUseAIons.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkUseAIons.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUseAIons.Location = New System.Drawing.Point(14, 27)
        Me.chkUseAIons.Name = "chkUseAIons"
        Me.chkUseAIons.Size = New System.Drawing.Size(150, 18)
        Me.chkUseAIons.TabIndex = 23
        Me.chkUseAIons.Text = "A ion neutral loss?"
        '
        'chkUseBIons
        '
        Me.chkUseBIons.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkUseBIons.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUseBIons.Location = New System.Drawing.Point(14, 50)
        Me.chkUseBIons.Name = "chkUseBIons"
        Me.chkUseBIons.Size = New System.Drawing.Size(150, 18)
        Me.chkUseBIons.TabIndex = 24
        Me.chkUseBIons.Text = "B ion neutral loss?"
        '
        'chkUseYIons
        '
        Me.chkUseYIons.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkUseYIons.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUseYIons.Location = New System.Drawing.Point(14, 73)
        Me.chkUseYIons.Name = "chkUseYIons"
        Me.chkUseYIons.Size = New System.Drawing.Size(150, 18)
        Me.chkUseYIons.TabIndex = 25
        Me.chkUseYIons.Text = "Y ion neutral loss?"
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
        Me.gbxMiscParams.Location = New System.Drawing.Point(10, 198)
        Me.gbxMiscParams.Name = "gbxMiscParams"
        Me.gbxMiscParams.Size = New System.Drawing.Size(542, 236)
        Me.gbxMiscParams.TabIndex = 2
        Me.gbxMiscParams.TabStop = False
        Me.gbxMiscParams.Text = "Miscellaneous Options"
        '
        'lblNumResults
        '
        Me.lblNumResults.Enabled = False
        Me.lblNumResults.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumResults.Location = New System.Drawing.Point(293, 175)
        Me.lblNumResults.Name = "lblNumResults"
        Me.lblNumResults.Size = New System.Drawing.Size(192, 19)
        Me.lblNumResults.TabIndex = 18
        Me.lblNumResults.Text = "Number of Results To Process"
        '
        'txtNumResults
        '
        Me.txtNumResults.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumResults.Location = New System.Drawing.Point(293, 194)
        Me.txtNumResults.Name = "txtNumResults"
        Me.txtNumResults.Size = New System.Drawing.Size(235, 23)
        Me.txtNumResults.TabIndex = 13
        '
        'cboNucReadingFrame
        '
        Me.cboNucReadingFrame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboNucReadingFrame.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboNucReadingFrame.Location = New System.Drawing.Point(14, 194)
        Me.cboNucReadingFrame.Name = "cboNucReadingFrame"
        Me.cboNucReadingFrame.Size = New System.Drawing.Size(240, 25)
        Me.cboNucReadingFrame.TabIndex = 12
        '
        'txtNumDescLines
        '
        Me.txtNumDescLines.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumDescLines.Location = New System.Drawing.Point(293, 42)
        Me.txtNumDescLines.Name = "txtNumDescLines"
        Me.txtNumDescLines.Size = New System.Drawing.Size(235, 23)
        Me.txtNumDescLines.TabIndex = 7
        '
        'lblOutputLines
        '
        Me.lblOutputLines.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOutputLines.Location = New System.Drawing.Point(14, 23)
        Me.lblOutputLines.Name = "lblOutputLines"
        Me.lblOutputLines.Size = New System.Drawing.Size(226, 19)
        Me.lblOutputLines.TabIndex = 9
        Me.lblOutputLines.Text = "Number of Peptide Results to Show"
        '
        'txtNumOutputLines
        '
        Me.txtNumOutputLines.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumOutputLines.Location = New System.Drawing.Point(14, 42)
        Me.txtNumOutputLines.Name = "txtNumOutputLines"
        Me.txtNumOutputLines.Size = New System.Drawing.Size(236, 23)
        Me.txtNumOutputLines.TabIndex = 6
        '
        'lblNumDescLines
        '
        Me.lblNumDescLines.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumDescLines.Location = New System.Drawing.Point(293, 23)
        Me.lblNumDescLines.Name = "lblNumDescLines"
        Me.lblNumDescLines.Size = New System.Drawing.Size(225, 19)
        Me.lblNumDescLines.TabIndex = 13
        Me.lblNumDescLines.Text = "Number of Descriptions to Show"
        '
        'txtMatchPeakCountErrors
        '
        Me.txtMatchPeakCountErrors.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMatchPeakCountErrors.Location = New System.Drawing.Point(293, 92)
        Me.txtMatchPeakCountErrors.Name = "txtMatchPeakCountErrors"
        Me.txtMatchPeakCountErrors.Size = New System.Drawing.Size(235, 23)
        Me.txtMatchPeakCountErrors.TabIndex = 9
        '
        'lblMatchPeakCountErrors
        '
        Me.lblMatchPeakCountErrors.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMatchPeakCountErrors.Location = New System.Drawing.Point(293, 74)
        Me.lblMatchPeakCountErrors.Name = "lblMatchPeakCountErrors"
        Me.lblMatchPeakCountErrors.Size = New System.Drawing.Size(225, 18)
        Me.lblMatchPeakCountErrors.TabIndex = 14
        Me.lblMatchPeakCountErrors.Text = "Number of Peak Errors Allowed"
        '
        'lblMatchPeakCount
        '
        Me.lblMatchPeakCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMatchPeakCount.Location = New System.Drawing.Point(14, 74)
        Me.lblMatchPeakCount.Name = "lblMatchPeakCount"
        Me.lblMatchPeakCount.Size = New System.Drawing.Size(264, 18)
        Me.lblMatchPeakCount.TabIndex = 8
        Me.lblMatchPeakCount.Text = "Number of Peaks to Try to Match"
        '
        'txtMatchPeakCount
        '
        Me.txtMatchPeakCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMatchPeakCount.Location = New System.Drawing.Point(14, 92)
        Me.txtMatchPeakCount.Name = "txtMatchPeakCount"
        Me.txtMatchPeakCount.Size = New System.Drawing.Size(236, 23)
        Me.txtMatchPeakCount.TabIndex = 8
        '
        'txtMaxDiffPerPeptide
        '
        Me.txtMaxDiffPerPeptide.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaxDiffPerPeptide.Location = New System.Drawing.Point(293, 143)
        Me.txtMaxDiffPerPeptide.Name = "txtMaxDiffPerPeptide"
        Me.txtMaxDiffPerPeptide.Size = New System.Drawing.Size(235, 23)
        Me.txtMaxDiffPerPeptide.TabIndex = 11
        '
        'lblMaxAAPerDynMod
        '
        Me.lblMaxAAPerDynMod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaxAAPerDynMod.Location = New System.Drawing.Point(14, 125)
        Me.lblMaxAAPerDynMod.Name = "lblMaxAAPerDynMod"
        Me.lblMaxAAPerDynMod.Size = New System.Drawing.Size(226, 18)
        Me.lblMaxAAPerDynMod.TabIndex = 7
        Me.lblMaxAAPerDynMod.Text = "Maximum Dynamic Mods Per AA"
        '
        'txtMaxAAPerDynMod
        '
        Me.txtMaxAAPerDynMod.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaxAAPerDynMod.Location = New System.Drawing.Point(14, 143)
        Me.txtMaxAAPerDynMod.Name = "txtMaxAAPerDynMod"
        Me.txtMaxAAPerDynMod.Size = New System.Drawing.Size(236, 23)
        Me.txtMaxAAPerDynMod.TabIndex = 10
        '
        'lblNucReadingFrame
        '
        Me.lblNucReadingFrame.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNucReadingFrame.Location = New System.Drawing.Point(14, 175)
        Me.lblNucReadingFrame.Name = "lblNucReadingFrame"
        Me.lblNucReadingFrame.Size = New System.Drawing.Size(226, 19)
        Me.lblNucReadingFrame.TabIndex = 7
        Me.lblNucReadingFrame.Text = "Nucleotide Reading Frame"
        '
        'lblSeqHdrFilter
        '
        Me.lblSeqHdrFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSeqHdrFilter.Location = New System.Drawing.Point(293, 125)
        Me.lblSeqHdrFilter.Name = "lblSeqHdrFilter"
        Me.lblSeqHdrFilter.Size = New System.Drawing.Size(245, 18)
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
        Me.gbxToleranceValues.Location = New System.Drawing.Point(10, 5)
        Me.gbxToleranceValues.Name = "gbxToleranceValues"
        Me.gbxToleranceValues.Size = New System.Drawing.Size(542, 184)
        Me.gbxToleranceValues.TabIndex = 1
        Me.gbxToleranceValues.TabStop = False
        Me.gbxToleranceValues.Text = "Search Tolerance Values"
        '
        'txtFragMassTol
        '
        Me.txtFragMassTol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFragMassTol.Location = New System.Drawing.Point(293, 42)
        Me.txtFragMassTol.Name = "txtFragMassTol"
        Me.txtFragMassTol.Size = New System.Drawing.Size(235, 23)
        Me.txtFragMassTol.TabIndex = 1
        '
        'lblPepMassTol
        '
        Me.lblPepMassTol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPepMassTol.Location = New System.Drawing.Point(14, 23)
        Me.lblPepMassTol.Name = "lblPepMassTol"
        Me.lblPepMassTol.Size = New System.Drawing.Size(226, 19)
        Me.lblPepMassTol.TabIndex = 1
        Me.lblPepMassTol.Text = "Parent Mass Tolerance"
        '
        'txtPepMassTol
        '
        Me.txtPepMassTol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPepMassTol.Location = New System.Drawing.Point(14, 42)
        Me.txtPepMassTol.Name = "txtPepMassTol"
        Me.txtPepMassTol.Size = New System.Drawing.Size(236, 23)
        Me.txtPepMassTol.TabIndex = 0
        '
        'lblFragMassTol
        '
        Me.lblFragMassTol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFragMassTol.Location = New System.Drawing.Point(293, 23)
        Me.lblFragMassTol.Name = "lblFragMassTol"
        Me.lblFragMassTol.Size = New System.Drawing.Size(225, 19)
        Me.lblFragMassTol.TabIndex = 3
        Me.lblFragMassTol.Text = "Fragment Mass Tolerance"
        '
        'txtIonCutoff
        '
        Me.txtIonCutoff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIonCutoff.Location = New System.Drawing.Point(293, 92)
        Me.txtIonCutoff.Name = "txtIonCutoff"
        Me.txtIonCutoff.Size = New System.Drawing.Size(235, 23)
        Me.txtIonCutoff.TabIndex = 3
        '
        'lblIonCutoff
        '
        Me.lblIonCutoff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIonCutoff.Location = New System.Drawing.Point(293, 74)
        Me.lblIonCutoff.Name = "lblIonCutoff"
        Me.lblIonCutoff.Size = New System.Drawing.Size(225, 18)
        Me.lblIonCutoff.TabIndex = 3
        Me.lblIonCutoff.Text = "Preliminary Score Cutoff Percentage"
        '
        'lblPeakMatchingTol
        '
        Me.lblPeakMatchingTol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPeakMatchingTol.Location = New System.Drawing.Point(14, 74)
        Me.lblPeakMatchingTol.Name = "lblPeakMatchingTol"
        Me.lblPeakMatchingTol.Size = New System.Drawing.Size(250, 18)
        Me.lblPeakMatchingTol.TabIndex = 1
        Me.lblPeakMatchingTol.Text = "Detected Peak Matching Tolerance"
        '
        'txtPeakMatchingTol
        '
        Me.txtPeakMatchingTol.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPeakMatchingTol.Location = New System.Drawing.Point(14, 92)
        Me.txtPeakMatchingTol.Name = "txtPeakMatchingTol"
        Me.txtPeakMatchingTol.Size = New System.Drawing.Size(236, 23)
        Me.txtPeakMatchingTol.TabIndex = 2
        '
        'lblMaxProtMass
        '
        Me.lblMaxProtMass.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaxProtMass.Location = New System.Drawing.Point(293, 125)
        Me.lblMaxProtMass.Name = "lblMaxProtMass"
        Me.lblMaxProtMass.Size = New System.Drawing.Size(225, 18)
        Me.lblMaxProtMass.TabIndex = 3
        Me.lblMaxProtMass.Text = "Maximum Allowed Protein Mass"
        '
        'txtMaxProtMass
        '
        Me.txtMaxProtMass.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaxProtMass.Location = New System.Drawing.Point(293, 143)
        Me.txtMaxProtMass.Name = "txtMaxProtMass"
        Me.txtMaxProtMass.Size = New System.Drawing.Size(235, 23)
        Me.txtMaxProtMass.TabIndex = 5
        '
        'lblMinProtMass
        '
        Me.lblMinProtMass.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMinProtMass.Location = New System.Drawing.Point(14, 125)
        Me.lblMinProtMass.Name = "lblMinProtMass"
        Me.lblMinProtMass.Size = New System.Drawing.Size(226, 18)
        Me.lblMinProtMass.TabIndex = 1
        Me.lblMinProtMass.Text = "Minimum Allowed Protein Mass"
        '
        'txtMinProtMass
        '
        Me.txtMinProtMass.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMinProtMass.Location = New System.Drawing.Point(14, 143)
        Me.txtMinProtMass.Name = "txtMinProtMass"
        Me.txtMinProtMass.Size = New System.Drawing.Size(236, 23)
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
        Me.gbxSwitches.Location = New System.Drawing.Point(10, 554)
        Me.gbxSwitches.Name = "gbxSwitches"
        Me.gbxSwitches.Size = New System.Drawing.Size(542, 143)
        Me.gbxSwitches.TabIndex = 0
        Me.gbxSwitches.TabStop = False
        Me.gbxSwitches.Text = "Search Options"
        '
        'chkResiduesInUpperCase
        '
        Me.chkResiduesInUpperCase.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkResiduesInUpperCase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkResiduesInUpperCase.Location = New System.Drawing.Point(14, 111)
        Me.chkResiduesInUpperCase.Name = "chkResiduesInUpperCase"
        Me.chkResiduesInUpperCase.Size = New System.Drawing.Size(298, 27)
        Me.chkResiduesInUpperCase.TabIndex = 30
        Me.chkResiduesInUpperCase.Text = "FASTA File has Residues in Upper Case?"
        '
        'chkPrintDupRefs
        '
        Me.chkPrintDupRefs.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkPrintDupRefs.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintDupRefs.Location = New System.Drawing.Point(14, 88)
        Me.chkPrintDupRefs.Name = "chkPrintDupRefs"
        Me.chkPrintDupRefs.Size = New System.Drawing.Size(298, 27)
        Me.chkPrintDupRefs.TabIndex = 29
        Me.chkPrintDupRefs.Text = "Print Duplicate References (ORFs)?"
        '
        'chkRemovePrecursorPeaks
        '
        Me.chkRemovePrecursorPeaks.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkRemovePrecursorPeaks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkRemovePrecursorPeaks.Location = New System.Drawing.Point(14, 65)
        Me.chkRemovePrecursorPeaks.Name = "chkRemovePrecursorPeaks"
        Me.chkRemovePrecursorPeaks.Size = New System.Drawing.Size(298, 27)
        Me.chkRemovePrecursorPeaks.TabIndex = 28
        Me.chkRemovePrecursorPeaks.Text = "Remove Precursor Ion Peaks?"
        '
        'chkShowFragmentIons
        '
        Me.chkShowFragmentIons.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkShowFragmentIons.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowFragmentIons.Location = New System.Drawing.Point(14, 42)
        Me.chkShowFragmentIons.Name = "chkShowFragmentIons"
        Me.chkShowFragmentIons.Size = New System.Drawing.Size(298, 27)
        Me.chkShowFragmentIons.TabIndex = 27
        Me.chkShowFragmentIons.Text = "Show Fragment Ions?"
        '
        'chkCreateOutputFiles
        '
        Me.chkCreateOutputFiles.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.chkCreateOutputFiles.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCreateOutputFiles.Location = New System.Drawing.Point(14, 18)
        Me.chkCreateOutputFiles.Name = "chkCreateOutputFiles"
        Me.chkCreateOutputFiles.Size = New System.Drawing.Size(298, 28)
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
        Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFileLoadFromDMS, Me.mnuLoadFromFile, Me.MenuItem2, Me.mnuFileSaveToFile, Me.mnuFileUploadDMS, Me.mnuBatchUploadDMS, Me.mnuDiv1, Me.mnuFileExit})
        Me.mnuFile.Text = "&File"
        '
        'mnuFileLoadFromDMS
        '
        Me.mnuFileLoadFromDMS.Index = 0
        Me.mnuFileLoadFromDMS.Shortcut = System.Windows.Forms.Shortcut.CtrlL
        Me.mnuFileLoadFromDMS.Text = "Load Param File from &DMS..."
        '
        'mnuLoadFromFile
        '
        Me.mnuLoadFromFile.Index = 1
        Me.mnuLoadFromFile.Text = "Load Param File from &Local Template File..."
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 2
        Me.MenuItem2.Text = "-"
        '
        'mnuFileSaveToFile
        '
        Me.mnuFileSaveToFile.Index = 3
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
        Me.mnuFileSaveBW32.Shortcut = System.Windows.Forms.Shortcut.CtrlS
        Me.mnuFileSaveBW32.Text = "&BioWorks 3.2 Format..."
        '
        'mnuFileUploadDMS
        '
        Me.mnuFileUploadDMS.Index = 4
        Me.mnuFileUploadDMS.Shortcut = System.Windows.Forms.Shortcut.CtrlU
        Me.mnuFileUploadDMS.Text = "&Upload Current Settings to DMS (Restricted)..."
        '
        'mnuBatchUploadDMS
        '
        Me.mnuBatchUploadDMS.Index = 5
        Me.mnuBatchUploadDMS.Text = "&Batch Upload Param Files to DMS (Restricted)"
        '
        'mnuDiv1
        '
        Me.mnuDiv1.Index = 6
        Me.mnuDiv1.Text = "-"
        '
        'mnuFileExit
        '
        Me.mnuFileExit.Index = 7
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
        Me.mnuHelp.Text = "&Help"
        '
        'mnuHelpAbout
        '
        Me.mnuHelpAbout.Index = 0
        Me.mnuHelpAbout.Text = "&About Parameter File Editor..."
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
        'StatModErrorProvider
        '
        Me.StatModErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink
        Me.StatModErrorProvider.ContainerControl = Me
        '
        'txtParamInfo
        '
        Me.txtParamInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtParamInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtParamInfo.ForceNewValue = False
        Me.txtParamInfo.Location = New System.Drawing.Point(5, 750)
        Me.txtParamInfo.Multiline = True
        Me.txtParamInfo.Name = "txtParamInfo"
        Me.txtParamInfo.ReadOnly = True
        Me.txtParamInfo.Size = New System.Drawing.Size(625, 38)
        Me.txtParamInfo.TabIndex = 12
        Me.txtParamInfo.Tag = "0"
        Me.txtParamInfo.Text = "Currently Loaded Template: "
        '
        'txtIsoS
        '
        Me.txtIsoS.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIsoS.ForceNewValue = False
        Me.txtIsoS.Location = New System.Drawing.Point(437, 36)
        Me.txtIsoS.Name = "txtIsoS"
        Me.txtIsoS.Size = New System.Drawing.Size(77, 23)
        Me.txtIsoS.TabIndex = 9
        '
        'txtIsoH
        '
        Me.txtIsoH.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIsoH.ForceNewValue = False
        Me.txtIsoH.Location = New System.Drawing.Point(120, 36)
        Me.txtIsoH.Name = "txtIsoH"
        Me.txtIsoH.Size = New System.Drawing.Size(77, 23)
        Me.txtIsoH.TabIndex = 3
        '
        'txtIsoN
        '
        Me.txtIsoN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIsoN.ForceNewValue = False
        Me.txtIsoN.Location = New System.Drawing.Point(331, 36)
        Me.txtIsoN.Name = "txtIsoN"
        Me.txtIsoN.Size = New System.Drawing.Size(77, 23)
        Me.txtIsoN.TabIndex = 7
        '
        'txtIsoO
        '
        Me.txtIsoO.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIsoO.ForceNewValue = False
        Me.txtIsoO.Location = New System.Drawing.Point(226, 36)
        Me.txtIsoO.Name = "txtIsoO"
        Me.txtIsoO.Size = New System.Drawing.Size(76, 23)
        Me.txtIsoO.TabIndex = 5
        '
        'txtIsoC
        '
        Me.txtIsoC.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIsoC.ForceNewValue = False
        Me.txtIsoC.Location = New System.Drawing.Point(19, 36)
        Me.txtIsoC.Name = "txtIsoC"
        Me.txtIsoC.Size = New System.Drawing.Size(77, 23)
        Me.txtIsoC.TabIndex = 1
        '
        'txtCTPep
        '
        Me.txtCTPep.BackColor = System.Drawing.SystemColors.Window
        Me.txtCTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCTPep.ForceNewValue = False
        Me.txtCTPep.Location = New System.Drawing.Point(14, 37)
        Me.txtCTPep.Name = "txtCTPep"
        Me.txtCTPep.Size = New System.Drawing.Size(66, 23)
        Me.txtCTPep.TabIndex = 12
        '
        'txtAla
        '
        Me.txtAla.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAla.ForceNewValue = False
        Me.txtAla.Location = New System.Drawing.Point(440, 37)
        Me.txtAla.Name = "txtAla"
        Me.txtAla.Size = New System.Drawing.Size(66, 23)
        Me.txtAla.TabIndex = 17
        '
        'txtCTProt
        '
        Me.txtCTProt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCTProt.ForceNewValue = False
        Me.txtCTProt.Location = New System.Drawing.Point(100, 37)
        Me.txtCTProt.Name = "txtCTProt"
        Me.txtCTProt.Size = New System.Drawing.Size(66, 23)
        Me.txtCTProt.TabIndex = 13
        '
        'txtNTPep
        '
        Me.txtNTPep.BackColor = System.Drawing.SystemColors.Window
        Me.txtNTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNTPep.ForceNewValue = False
        Me.txtNTPep.Location = New System.Drawing.Point(185, 37)
        Me.txtNTPep.Name = "txtNTPep"
        Me.txtNTPep.Size = New System.Drawing.Size(66, 23)
        Me.txtNTPep.TabIndex = 14
        '
        'txtNTProt
        '
        Me.txtNTProt.BackColor = System.Drawing.SystemColors.Window
        Me.txtNTProt.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNTProt.ForceNewValue = False
        Me.txtNTProt.Location = New System.Drawing.Point(270, 37)
        Me.txtNTProt.Name = "txtNTProt"
        Me.txtNTProt.Size = New System.Drawing.Size(66, 23)
        Me.txtNTProt.TabIndex = 15
        '
        'txtGly
        '
        Me.txtGly.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGly.ForceNewValue = False
        Me.txtGly.Location = New System.Drawing.Point(355, 37)
        Me.txtGly.Name = "txtGly"
        Me.txtGly.Size = New System.Drawing.Size(66, 23)
        Me.txtGly.TabIndex = 16
        '
        'txtSer
        '
        Me.txtSer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSer.ForceNewValue = False
        Me.txtSer.Location = New System.Drawing.Point(526, 37)
        Me.txtSer.Name = "txtSer"
        Me.txtSer.Size = New System.Drawing.Size(66, 23)
        Me.txtSer.TabIndex = 18
        '
        'txtCys
        '
        Me.txtCys.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCys.ForceNewValue = False
        Me.txtCys.Location = New System.Drawing.Point(270, 76)
        Me.txtCys.Name = "txtCys"
        Me.txtCys.Size = New System.Drawing.Size(66, 23)
        Me.txtCys.TabIndex = 22
        '
        'txtPro
        '
        Me.txtPro.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPro.ForceNewValue = False
        Me.txtPro.Location = New System.Drawing.Point(14, 76)
        Me.txtPro.Name = "txtPro"
        Me.txtPro.Size = New System.Drawing.Size(66, 23)
        Me.txtPro.TabIndex = 19
        '
        'TxtLorI
        '
        Me.TxtLorI.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtLorI.ForceNewValue = False
        Me.TxtLorI.Location = New System.Drawing.Point(530, 76)
        Me.TxtLorI.Name = "TxtLorI"
        Me.TxtLorI.Size = New System.Drawing.Size(66, 23)
        Me.TxtLorI.TabIndex = 25
        '
        'txtThr
        '
        Me.txtThr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtThr.ForceNewValue = False
        Me.txtThr.Location = New System.Drawing.Point(185, 76)
        Me.txtThr.Name = "txtThr"
        Me.txtThr.Size = New System.Drawing.Size(66, 23)
        Me.txtThr.TabIndex = 21
        '
        'txtIle
        '
        Me.txtIle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIle.ForceNewValue = False
        Me.txtIle.Location = New System.Drawing.Point(440, 76)
        Me.txtIle.Name = "txtIle"
        Me.txtIle.Size = New System.Drawing.Size(66, 23)
        Me.txtIle.TabIndex = 24
        '
        'txtVal
        '
        Me.txtVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtVal.ForceNewValue = False
        Me.txtVal.Location = New System.Drawing.Point(100, 76)
        Me.txtVal.Name = "txtVal"
        Me.txtVal.Size = New System.Drawing.Size(66, 23)
        Me.txtVal.TabIndex = 20
        '
        'txtLeu
        '
        Me.txtLeu.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLeu.ForceNewValue = False
        Me.txtLeu.Location = New System.Drawing.Point(355, 76)
        Me.txtLeu.Name = "txtLeu"
        Me.txtLeu.Size = New System.Drawing.Size(66, 23)
        Me.txtLeu.TabIndex = 23
        '
        'txtNandD
        '
        Me.txtNandD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNandD.ForceNewValue = False
        Me.txtNandD.Location = New System.Drawing.Point(185, 115)
        Me.txtNandD.Name = "txtNandD"
        Me.txtNandD.Size = New System.Drawing.Size(66, 23)
        Me.txtNandD.TabIndex = 28
        '
        'txtQandE
        '
        Me.txtQandE.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtQandE.ForceNewValue = False
        Me.txtQandE.Location = New System.Drawing.Point(530, 115)
        Me.txtQandE.Name = "txtQandE"
        Me.txtQandE.Size = New System.Drawing.Size(66, 23)
        Me.txtQandE.TabIndex = 32
        '
        'txtAsn
        '
        Me.txtAsn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAsn.ForceNewValue = False
        Me.txtAsn.Location = New System.Drawing.Point(14, 115)
        Me.txtAsn.Name = "txtAsn"
        Me.txtAsn.Size = New System.Drawing.Size(66, 23)
        Me.txtAsn.TabIndex = 26
        '
        'txtLys
        '
        Me.txtLys.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLys.ForceNewValue = False
        Me.txtLys.Location = New System.Drawing.Point(440, 115)
        Me.txtLys.Name = "txtLys"
        Me.txtLys.Size = New System.Drawing.Size(66, 23)
        Me.txtLys.TabIndex = 31
        '
        'txtOrn
        '
        Me.txtOrn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOrn.ForceNewValue = False
        Me.txtOrn.Location = New System.Drawing.Point(100, 115)
        Me.txtOrn.Name = "txtOrn"
        Me.txtOrn.Size = New System.Drawing.Size(66, 23)
        Me.txtOrn.TabIndex = 27
        '
        'txtGln
        '
        Me.txtGln.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGln.ForceNewValue = False
        Me.txtGln.Location = New System.Drawing.Point(355, 115)
        Me.txtGln.Name = "txtGln"
        Me.txtGln.Size = New System.Drawing.Size(66, 23)
        Me.txtGln.TabIndex = 30
        '
        'txtAsp
        '
        Me.txtAsp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAsp.ForceNewValue = False
        Me.txtAsp.Location = New System.Drawing.Point(270, 115)
        Me.txtAsp.Name = "txtAsp"
        Me.txtAsp.Size = New System.Drawing.Size(66, 23)
        Me.txtAsp.TabIndex = 29
        '
        'txtArg
        '
        Me.txtArg.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtArg.ForceNewValue = False
        Me.txtArg.Location = New System.Drawing.Point(355, 155)
        Me.txtArg.Name = "txtArg"
        Me.txtArg.Size = New System.Drawing.Size(66, 23)
        Me.txtArg.TabIndex = 37
        '
        'txtTrp
        '
        Me.txtTrp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTrp.ForceNewValue = False
        Me.txtTrp.Location = New System.Drawing.Point(530, 155)
        Me.txtTrp.Name = "txtTrp"
        Me.txtTrp.Size = New System.Drawing.Size(66, 23)
        Me.txtTrp.TabIndex = 39
        '
        'txtGlu
        '
        Me.txtGlu.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGlu.ForceNewValue = False
        Me.txtGlu.Location = New System.Drawing.Point(14, 155)
        Me.txtGlu.Name = "txtGlu"
        Me.txtGlu.Size = New System.Drawing.Size(66, 23)
        Me.txtGlu.TabIndex = 33
        '
        'txtHis
        '
        Me.txtHis.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHis.ForceNewValue = False
        Me.txtHis.Location = New System.Drawing.Point(185, 155)
        Me.txtHis.Name = "txtHis"
        Me.txtHis.Size = New System.Drawing.Size(66, 23)
        Me.txtHis.TabIndex = 35
        '
        'txtPhe
        '
        Me.txtPhe.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPhe.ForceNewValue = False
        Me.txtPhe.Location = New System.Drawing.Point(270, 155)
        Me.txtPhe.Name = "txtPhe"
        Me.txtPhe.Size = New System.Drawing.Size(66, 23)
        Me.txtPhe.TabIndex = 36
        '
        'txtTyr
        '
        Me.txtTyr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTyr.ForceNewValue = False
        Me.txtTyr.Location = New System.Drawing.Point(440, 155)
        Me.txtTyr.Name = "txtTyr"
        Me.txtTyr.Size = New System.Drawing.Size(66, 23)
        Me.txtTyr.TabIndex = 38
        '
        'txtMet
        '
        Me.txtMet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMet.ForceNewValue = False
        Me.txtMet.Location = New System.Drawing.Point(100, 155)
        Me.txtMet.Name = "txtMet"
        Me.txtMet.Size = New System.Drawing.Size(66, 23)
        Me.txtMet.TabIndex = 34
        '
        'txtDynNTPep
        '
        Me.txtDynNTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDynNTPep.ForceNewValue = False
        Me.txtDynNTPep.Location = New System.Drawing.Point(524, 42)
        Me.txtDynNTPep.Name = "txtDynNTPep"
        Me.txtDynNTPep.Size = New System.Drawing.Size(72, 23)
        Me.txtDynNTPep.TabIndex = 24
        Me.txtDynNTPep.Tag = "0"
        '
        'txtDynCTPep
        '
        Me.txtDynCTPep.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDynCTPep.ForceNewValue = False
        Me.txtDynCTPep.Location = New System.Drawing.Point(524, 85)
        Me.txtDynCTPep.Name = "txtDynCTPep"
        Me.txtDynCTPep.Size = New System.Drawing.Size(72, 23)
        Me.txtDynCTPep.TabIndex = 23
        Me.txtDynCTPep.Tag = "0"
        '
        'txtDynMod1MassDiff
        '
        Me.txtDynMod1MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDynMod1MassDiff.ForceNewValue = False
        Me.txtDynMod1MassDiff.Location = New System.Drawing.Point(14, 85)
        Me.txtDynMod1MassDiff.Name = "txtDynMod1MassDiff"
        Me.txtDynMod1MassDiff.Size = New System.Drawing.Size(72, 23)
        Me.txtDynMod1MassDiff.TabIndex = 3
        Me.txtDynMod1MassDiff.Tag = "0"
        '
        'txtDynMod2MassDiff
        '
        Me.txtDynMod2MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDynMod2MassDiff.ForceNewValue = False
        Me.txtDynMod2MassDiff.Location = New System.Drawing.Point(116, 85)
        Me.txtDynMod2MassDiff.Name = "txtDynMod2MassDiff"
        Me.txtDynMod2MassDiff.Size = New System.Drawing.Size(72, 23)
        Me.txtDynMod2MassDiff.TabIndex = 7
        Me.txtDynMod2MassDiff.Tag = "0"
        '
        'txtDynMod3MassDiff
        '
        Me.txtDynMod3MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDynMod3MassDiff.ForceNewValue = False
        Me.txtDynMod3MassDiff.Location = New System.Drawing.Point(218, 85)
        Me.txtDynMod3MassDiff.Name = "txtDynMod3MassDiff"
        Me.txtDynMod3MassDiff.Size = New System.Drawing.Size(72, 23)
        Me.txtDynMod3MassDiff.TabIndex = 11
        Me.txtDynMod3MassDiff.Tag = "0"
        '
        'txtDynMod4MassDiff
        '
        Me.txtDynMod4MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDynMod4MassDiff.ForceNewValue = False
        Me.txtDynMod4MassDiff.Location = New System.Drawing.Point(320, 85)
        Me.txtDynMod4MassDiff.Name = "txtDynMod4MassDiff"
        Me.txtDynMod4MassDiff.Size = New System.Drawing.Size(72, 23)
        Me.txtDynMod4MassDiff.TabIndex = 15
        Me.txtDynMod4MassDiff.Tag = "0"
        '
        'txtDynMod5MassDiff
        '
        Me.txtDynMod5MassDiff.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDynMod5MassDiff.ForceNewValue = False
        Me.txtDynMod5MassDiff.Location = New System.Drawing.Point(422, 85)
        Me.txtDynMod5MassDiff.Name = "txtDynMod5MassDiff"
        Me.txtDynMod5MassDiff.Size = New System.Drawing.Size(72, 23)
        Me.txtDynMod5MassDiff.TabIndex = 19
        Me.txtDynMod5MassDiff.Tag = "0"
        '
        'frmMainGUI
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(635, 693)
        Me.Controls.Add(Me.txtParamInfo)
        Me.Controls.Add(Me.tcMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximumSize = New System.Drawing.Size(720, 858)
        Me.Menu = Me.mnuMain
        Me.MinimumSize = New System.Drawing.Size(240, 231)
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
        Me.PerformLayout()

    End Sub

    <System.STAThread()> Public Shared Sub Main()
        System.Windows.Forms.Application.EnableVisualStyles()
        System.Windows.Forms.Application.DoEvents()
        Try
            System.Windows.Forms.Application.Run(New frmMainGUI)
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
    Public newParams As Params
    ' Unused: Private newBasic As IBasicParams
    ' Unused: Private newAdv As IAdvancedParams
    Private ReadOnly m_SettingsFileName As String = "ParamFileEditorSettings.xml"
    Const DEF_TEMPLATE_LABEL_TEXT As String = "Currently Loaded Template: "
    Private ReadOnly embeddedResource As New clsAccessEmbeddedRsrc
    Private m_UseAutoTweak As Boolean

    Private Sub RefreshTabs(frm As frmMainGUI, ParamsClass As Params)
        SetupBasicTab(frm, ParamsClass)
        SetupAdvancedTab(frm, ParamsClass)
        RetweakMasses()
    End Sub

    Private Sub frmMainGUI_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        mySettings = New clsSettings
        mySettings.LoadSettings(m_SettingsFileName)

        ValidateDBTools(mySettings.DMS_ConnectionString)

        newParams = New Params()
        m_DMSUpload = New clsDMSParamUpload(m_CurrentDBTools)

        With newParams
            .FileName = Mid(MainProcess.TemplateFileName, InStrRev(MainProcess.TemplateFileName, "\") + 1)
            .LoadTemplate(mySettings.TemplateFileName)
        End With

        RefreshTabs(Me, newParams)

        'AddBasicTabHandlers()
        'AddAdvTabHandlers()

    End Sub

    Private Sub SetupBasicTab(frm As frmMainGUI, bt As IBasicParams)
        RemoveBasicTabHandlers()

        Try

            'Load combo boxes
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

                For Each enz As EnzymeDetails In frm.newParams.EnzymeList
                    .Items.Add(enz.EnzymeID & " - " & enz.EnzymeName & " [" & enz.EnzymeCleavePoints & "]")
                Next
                .EndUpdate()

                If .Items.Count = 0 Then
                    Try
                        MessageBox.Show("Current parameter file had an empty enzyme list; will load the defaults from " & Path.GetFileName(MainProcess.TemplateFileName), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        ' No enzymes were defined; update to show the defaults

                        Dim objParams As New Params

                        With objParams
                            .FileName = Path.GetFileName(MainProcess.TemplateFileName)
                            .LoadTemplate(mySettings.TemplateFileName)
                        End With

                        .BeginUpdate()
                        .Items.Clear()
                        For Each enz As EnzymeDetails In objParams.EnzymeList
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
                frm.chkUseAIons.Checked = .IonSeries.Use_a_Ions
                frm.chkUseBIons.Checked = .IonSeries.Use_b_Ions
                frm.chkUseYIons.Checked = .IonSeries.Use_y_Ions
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
                frm.txtMaxDiffPerPeptide.Text = .MaximumNumDifferentialPerPeptide.ToString()

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

    Private Sub LoadParamSetFromDMS(ParamSetID As Integer)
        Try
            If m_clsParamsFromDMS Is Nothing Then
                m_clsParamsFromDMS = LoadDMSParamsClass()
            Else
                ' Need to run a Refresh of m_clsParamsFromDMS so that it knows about any newly imported param files
                m_clsParamsFromDMS.RefreshParamsFromDMS()
            End If
            newParams = m_clsParamsFromDMS.ReadParamsFromDMS(ParamSetID)

            Dim iso As New clsDeconvolveIsoMods(m_CurrentDBTools)
            newParams = iso.DeriveIsoMods(newParams)

            RefreshTabs(Me, newParams)
        Catch ex As Exception
            MessageBox.Show("Error in LoadParamSetFromDMS: " & ex.Message)
        End Try

    End Sub

    Private Sub LoadParamSetFromFile(FilePath As String)
        newParams.LoadTemplate(FilePath)
        Dim iso As New clsDeconvolveIsoMods(m_CurrentDBTools)
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

    Private Function LoadDMSParamsClass() As ParamsFromDMS
        Dim dms As New ParamsFromDMS(m_CurrentDBTools)
        Return dms
    End Function

    ' Unused
    'Private Function LoadDMSParamUploadClass(Settings As clsSettings) As clsDMSParamUpload
    '    Dim dms As New clsDMSParamUpload(Settings)
    '    Return dms
    'End Function

    Public Sub LoadDMSParamsFromID(ParamSetID As Integer)
        LoadParamSetFromDMS(ParamSetID)
    End Sub

    Public Sub LoadParamsFromFile(FilePath As String)
        LoadParamSetFromFile(FilePath)
    End Sub

    Private Sub ValidateDBTools(newConnectionString As String)
        If m_CurrentDBTools Is Nothing OrElse
           String.IsNullOrWhiteSpace(m_CurrentConnectionString) OrElse
           Not m_CurrentConnectionString.Equals(newConnectionString) Then

            m_CurrentConnectionString = String.Copy(newConnectionString)

            Dim connectionStringToUse = DbToolsFactory.AddApplicationNameToConnectionString(m_CurrentConnectionString, "ParamFileGenerator")
            m_CurrentDBTools = DbToolsFactory.GetDBTools(connectionStringToUse)
        End If
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

#Region "[Basic] Name and Description Handlers"

    Private Sub txtDescription_Leave(sender As Object, e As EventArgs)
        newParams.Description = txtDescription.Text
    End Sub

#End Region

#Region "[Basic] Search Settings Handlers"

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
            If cbc IsNot Nothing Then
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

#Region "[Basic] Dynamic Modification Handlers"

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
                                   ByRef txtModAATextBox As TextBox,
                                   ByRef txtModMassTextBox As NumericTextBox,
                                   intModNumber As Integer)

        Try
            If txtModAATextBox.TextLength = 0 Then
                txtModAATextBox.Text = "C"
            End If

            StatModErrorProvider.SetError(DirectCast(sender, Control), "")
            txtModAATextBox.Text = txtModAATextBox.Text.ToUpper
            If Math.Abs(CSng(txtModMassTextBox.Text)) > Single.Epsilon Then
                ' Only update this mod if the mass is non-zero
                newParams.DynamicMods.Dyn_Mod_n_AAList(intModNumber, txtModAATextBox.Text)
            Else
                ' If this mod, and all other mods after this mod are 0, then remove this mod and all subsequent mods
                If newParams.DynamicMods.Count = intModNumber Then
                    newParams.DynamicMods.RemoveAt(intModNumber - 1)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Exception in UpdateDynamicModAA: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try

        UpdateDescription()

    End Sub

    Private Sub UpdateDynamicModMass(ByRef txtModMassTextBox As NumericTextBox,
                                     ByRef txtModAATextBox As TextBox,
                                     intModNumber As Integer)
        Dim dblModMass As Double

        Try
            dblModMass = CDbl(txtModMassTextBox.Text)
            If Math.Abs(dblModMass) > Single.Epsilon Then
                ' Make sure this mod is defined
                newParams.DynamicMods.Dyn_Mod_n_AAList(intModNumber, txtModAATextBox.Text)
            End If

            If newParams.DynamicMods.Count < intModNumber And Math.Abs(dblModMass) < Single.Epsilon Then
                ' Nothing to update
            Else
                If Math.Abs(dblModMass) > Single.Epsilon Then
                    newParams.DynamicMods.Dyn_Mod_n_MassDiff(intModNumber, dblModMass)
                Else
                    ' If this mod, and all other mods after this mod are 0, then remove this mod and all subsequent mods
                    If newParams.DynamicMods.Count = intModNumber Then
                        newParams.DynamicMods.RemoveAt(intModNumber - 1)
                    Else
                        newParams.DynamicMods.Dyn_Mod_n_MassDiff(intModNumber, dblModMass)
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

#Region "[Basic] Static Modification Handlers"

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

#Region "[Basic] Isotopic Modification Handlers"
    Private Sub txtIsoC_Validated(sender As Object, e As EventArgs)
        Dim thisControl = DirectCast(sender, Control)
        Try
            newParams.IsotopicModificationsList.Iso_C = CDbl(thisControl.Text)
        Catch
            thisControl.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtIsoH_Validated(sender As Object, e As EventArgs)
        Dim thisControl = DirectCast(sender, Control)
        Try
            newParams.IsotopicModificationsList.Iso_H = CDbl(thisControl.Text)
        Catch
            thisControl.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtIsoO_Validated(sender As Object, e As EventArgs)
        Dim thisControl = DirectCast(sender, Control)
        Try
            newParams.IsotopicModificationsList.Iso_O = CDbl(thisControl.Text)
        Catch
            thisControl.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtIsoN_Validated(sender As Object, e As EventArgs)
        Dim thisControl = DirectCast(sender, Control)
        Try
            newParams.IsotopicModificationsList.Iso_N = CDbl(thisControl.Text)
        Catch
            thisControl.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub

    Private Sub txtIsoS_Validated(sender As Object, e As EventArgs)
        Dim thisControl = DirectCast(sender, Control)
        Try
            newParams.IsotopicModificationsList.Iso_S = CDbl(thisControl.Text)
        Catch
            thisControl.Text = "0.0"
        End Try
        UpdateDescription()
    End Sub


#End Region

#Region "[Advanced] Searching Tolerances"

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

#Region "[Advanced] Miscellaneous Options"

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

#Region "[Advanced] Option Checkboxes"

    Private Sub chkUseAIons_CheckedChanged(sender As Object, e As EventArgs)
        newParams.IonSeries.Use_a_Ions = CBool(IIf(chkUseAIons.Checked, 1, 0))
        UpdateDescription()
    End Sub

    Private Sub chkUseBIons_CheckedChanged(sender As Object, e As EventArgs)
        newParams.IonSeries.Use_b_Ions = CBool(IIf(chkUseBIons.Checked, 1, 0))
        UpdateDescription()
    End Sub

    Private Sub chkUseYIons_CheckedChanged(sender As Object, e As EventArgs)
        newParams.IonSeries.Use_y_Ions = CBool(IIf(chkUseYIons.Checked, 1, 0))
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

#Region "[Advanced] Ion Weighting Constants"

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

#Region "Menu Handlers"

    Private Sub mnuFileSaveBW2_Click(sender As Object, e As EventArgs) Handles mnuFileSaveBW2.Click
        SaveSequestFile("Save Sequest/BioWorks v2.0 Parameter File", Params.ParamFileTypes.BioWorks_20)
    End Sub

    Private Sub mnuFileSaveBW3_Click(sender As Object, e As EventArgs) Handles mnuFileSaveBW3.Click
        SaveSequestFile("Save Sequest/BioWorks v3.0 Parameter File", Params.ParamFileTypes.BioWorks_30)
    End Sub

    Private Sub mnuFileSaveBW32_Click(sender As Object, e As EventArgs) Handles mnuFileSaveBW32.Click
        SaveSequestFile("Save Sequest/BioWorks v3.2 Parameter File", Params.ParamFileTypes.BioWorks_32)
    End Sub

    Private Sub mnuHelpAbout_Click(sender As Object, e As EventArgs) Handles mnuHelpAbout.Click
        Dim AboutBox As New frmAboutBox With {
            .ConnectionStringInUse = mySettings.DMS_ConnectionString
        }
        AboutBox.Show()
    End Sub

    Private Sub mnuFileLoadFromDMS_Click(sender As Object, e As EventArgs) Handles mnuFileLoadFromDMS.Click
        Try
            Dim frmPicker As New frmDMSPicker(Me, m_CurrentDBTools) With {
                .MySettings = mySettings
            }

            frmPicker.txtLiveSearch.Focus()
            frmPicker.Show()
        Catch ex As Exception
            MessageBox.Show("Error in mnuFileLoadFromDMS_Click: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try

    End Sub

    Private Sub mnuFileUploadDMS_Click(sender As Object, e As EventArgs) Handles mnuFileUploadDMS.Click
        Try
            Dim SaveFrm As New frmDMSParamNamer(m_CurrentDBTools, newParams)
            SaveFrm.Show()

        Catch ex As Exception
            MessageBox.Show("Error in mnuFileUploadDMS_Click: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try

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

            Dim openDialog = New OpenFileDialog With {
                .Multiselect = True,
                .InitialDirectory = "\\gigasax\DMS_Parameter_Files\Sequest\",
                .Filter = "Sequest Param files (*.params)|*.params|All files (*.*)|*.*",
                .FilterIndex = 1,
                .RestoreDirectory = True
            }


            If openDialog.ShowDialog = DialogResult.OK Then
                Dim fileNameList As List(Of String) = openDialog.FileNames.ToList()

                Dim batch = New clsBatchLoadTemplates(m_CurrentDBTools)
                batch.UploadParamSetsToDMS(fileNameList)
                Dim numAdded = batch.NumParamSetsAdded
                Dim numChanged = batch.NumParamSetsChanged
                Dim numSkipped = batch.NumParamSetsSkipped

                MessageBox.Show(numAdded & " new Parameter sets added; " & numChanged & " Parameter sets changed; " & numSkipped & " Parameter sets skipped",
                                "Operation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If


        Catch ex As Exception
            MessageBox.Show("Error in mnuBatchUploadDMS_Click: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try

    End Sub

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

    Private Sub NumericTextBox_Validating(sender As Object, e As CancelEventArgs)

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
                                    NumericTextBox_Validating(sender, e)
                                ElseIf dr = DialogResult.Yes Then   'Use existing
                                    thisControl.Text = tmpNewModMass.ToString()
                                    NumericTextBox_Validating(sender, e)
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
                NumericTextBox_Validating(sender, Nothing)
            ElseIf e.KeyCode = Keys.Return OrElse e.KeyCode = Keys.Enter Then
                t.ForceNewValue = True
                NumericTextBox_Validating(t, Nothing)
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

    Private Sub SaveSequestFile(strTitle As String, eFileType As Params.ParamFileTypes)

        Try
            Dim FileOutput As New WriteOutput

            Dim SaveDialog As New SaveFileDialog

            With SaveDialog
                .Title = strTitle
                .FileName = newParams.FileName
            End With

            If SaveDialog.ShowDialog = DialogResult.OK Then
                Dim newFilePath = SaveDialog.FileName
                If newFilePath.Length > 0 Then
                    Dim iFileType = CType(eFileType, IGenerateFile.ParamFileType)
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
                                NumericTextBox_Validating(t, Nothing)
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
            Dim sync As New clsTransferParamEntriesToMassModList(m_CurrentDBTools)

            sync.SyncAll()

        Catch ex As Exception
            MessageBox.Show("Error in mnuDebugSyncAll_Click: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try

    End Sub

    Private Sub mnuDebugSyncSingle_Click(sender As Object, e As EventArgs) Handles mnuDebugSyncSingle.Click
        Dim sync As New clsTransferParamEntriesToMassModList(m_CurrentDBTools)
        Dim paramFileID = CInt(InputBox("Enter a Parameter File ID to be Sync'd", "Param File to Sync"))
        sync.SyncOneJob(paramFileID)
    End Sub

    Private Sub mnuDebugSyncDesc_Click(sender As Object, e As EventArgs) Handles mnuDebugSyncDesc.Click
        Dim sync As New clsTransferParamEntriesToMassModList(m_CurrentDBTools)
        sync.SyncDescriptions()

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

            If strLine IsNot Nothing AndAlso strLine.Length > 0 Then
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

                If strCurrentFunction IsNot Nothing AndAlso strCurrentFunction.Length > 0 Then
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
            If strFunctions(intIndex) IsNot Nothing Then
                If strStackTrace.Length = 0 Then
                    strStackTrace = "Stack trace: " & strFunctions(intIndex)
                Else
                    strStackTrace &= "-:-" & strFunctions(intIndex)
                End If
            End If
        Next intIndex

        If strStackTrace IsNot Nothing AndAlso strFinalFile.Length > 0 Then
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
