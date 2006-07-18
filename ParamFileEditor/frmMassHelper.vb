Imports ParamFileEditor.ProgramSettings
Imports ParamFileGenerator

Public Class frmMassHelper
    Inherits System.Windows.Forms.Form

    'Private mwt As clsMolecularWeightCalc
    Private m_FormulaToAdd As String
    Private m_FormulaToSubtract As String
    Private m_UseHyd As Boolean
    Private m_mySettings As clsSettings

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
    Friend WithEvents lblCommonMods As System.Windows.Forms.Label
    Friend WithEvents lblFormulaEntry As System.Windows.Forms.Label
    Friend WithEvents txtFormulaEntry As System.Windows.Forms.TextBox
    Friend WithEvents txtMonoMass As System.Windows.Forms.TextBox
    Friend WithEvents lblMonoMass As System.Windows.Forms.Label
    Friend WithEvents txtAvgMass As System.Windows.Forms.TextBox
    Friend WithEvents lblAvgMass As System.Windows.Forms.Label
    Friend WithEvents cmdCalculateValues As System.Windows.Forms.Button
    Friend WithEvents cmdAddToList As System.Windows.Forms.Button
    Friend WithEvents cboCommonMods As System.Windows.Forms.ComboBox
    Friend WithEvents chkAttachAtHydrogen As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.lblCommonMods = New System.Windows.Forms.Label
        Me.lblFormulaEntry = New System.Windows.Forms.Label
        Me.cboCommonMods = New System.Windows.Forms.ComboBox
        Me.txtFormulaEntry = New System.Windows.Forms.TextBox
        Me.txtMonoMass = New System.Windows.Forms.TextBox
        Me.lblMonoMass = New System.Windows.Forms.Label
        Me.txtAvgMass = New System.Windows.Forms.TextBox
        Me.lblAvgMass = New System.Windows.Forms.Label
        Me.cmdCalculateValues = New System.Windows.Forms.Button
        Me.cmdAddToList = New System.Windows.Forms.Button
        Me.chkAttachAtHydrogen = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'lblCommonMods
        '
        Me.lblCommonMods.Location = New System.Drawing.Point(12, 8)
        Me.lblCommonMods.Name = "lblCommonMods"
        Me.lblCommonMods.Size = New System.Drawing.Size(212, 16)
        Me.lblCommonMods.TabIndex = 7
        Me.lblCommonMods.Text = "Common Modifications"
        '
        'lblFormulaEntry
        '
        Me.lblFormulaEntry.Location = New System.Drawing.Point(12, 52)
        Me.lblFormulaEntry.Name = "lblFormulaEntry"
        Me.lblFormulaEntry.Size = New System.Drawing.Size(160, 12)
        Me.lblFormulaEntry.TabIndex = 6
        Me.lblFormulaEntry.Text = "Enter Molecular Formula Here"
        '
        'cboCommonMods
        '
        Me.cboCommonMods.Location = New System.Drawing.Point(12, 24)
        Me.cboCommonMods.Name = "cboCommonMods"
        Me.cboCommonMods.Size = New System.Drawing.Size(244, 21)
        Me.cboCommonMods.TabIndex = 5
        '
        'txtFormulaEntry
        '
        Me.txtFormulaEntry.Location = New System.Drawing.Point(12, 68)
        Me.txtFormulaEntry.Name = "txtFormulaEntry"
        Me.txtFormulaEntry.Size = New System.Drawing.Size(244, 20)
        Me.txtFormulaEntry.TabIndex = 4
        Me.txtFormulaEntry.Text = ""
        '
        'txtMonoMass
        '
        Me.txtMonoMass.Location = New System.Drawing.Point(12, 116)
        Me.txtMonoMass.Name = "txtMonoMass"
        Me.txtMonoMass.Size = New System.Drawing.Size(112, 20)
        Me.txtMonoMass.TabIndex = 10
        Me.txtMonoMass.Text = ""
        '
        'lblMonoMass
        '
        Me.lblMonoMass.Location = New System.Drawing.Point(12, 100)
        Me.lblMonoMass.Name = "lblMonoMass"
        Me.lblMonoMass.Size = New System.Drawing.Size(104, 12)
        Me.lblMonoMass.TabIndex = 11
        Me.lblMonoMass.Text = "Monoisotopic Mass"
        '
        'txtAvgMass
        '
        Me.txtAvgMass.Location = New System.Drawing.Point(12, 160)
        Me.txtAvgMass.Name = "txtAvgMass"
        Me.txtAvgMass.Size = New System.Drawing.Size(112, 20)
        Me.txtAvgMass.TabIndex = 12
        Me.txtAvgMass.Text = ""
        '
        'lblAvgMass
        '
        Me.lblAvgMass.Location = New System.Drawing.Point(12, 144)
        Me.lblAvgMass.Name = "lblAvgMass"
        Me.lblAvgMass.Size = New System.Drawing.Size(100, 16)
        Me.lblAvgMass.TabIndex = 13
        Me.lblAvgMass.Text = "Average Mass"
        '
        'cmdCalculateValues
        '
        Me.cmdCalculateValues.Location = New System.Drawing.Point(144, 192)
        Me.cmdCalculateValues.Name = "cmdCalculateValues"
        Me.cmdCalculateValues.Size = New System.Drawing.Size(112, 28)
        Me.cmdCalculateValues.TabIndex = 14
        Me.cmdCalculateValues.Text = "Calculate Values"
        '
        'cmdAddToList
        '
        Me.cmdAddToList.Location = New System.Drawing.Point(12, 192)
        Me.cmdAddToList.Name = "cmdAddToList"
        Me.cmdAddToList.Size = New System.Drawing.Size(112, 28)
        Me.cmdAddToList.TabIndex = 15
        Me.cmdAddToList.Text = "Add To Drop-List..."
        '
        'chkAttachAtHydrogen
        '
        Me.chkAttachAtHydrogen.Location = New System.Drawing.Point(136, 116)
        Me.chkAttachAtHydrogen.Name = "chkAttachAtHydrogen"
        Me.chkAttachAtHydrogen.Size = New System.Drawing.Size(128, 24)
        Me.chkAttachAtHydrogen.TabIndex = 16
        Me.chkAttachAtHydrogen.Text = "Attach at Hydrogen?"
        '
        'frmMassHelper
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(268, 230)
        Me.Controls.Add(Me.chkAttachAtHydrogen)
        Me.Controls.Add(Me.cmdAddToList)
        Me.Controls.Add(Me.cmdCalculateValues)
        Me.Controls.Add(Me.lblAvgMass)
        Me.Controls.Add(Me.txtAvgMass)
        Me.Controls.Add(Me.lblMonoMass)
        Me.Controls.Add(Me.txtMonoMass)
        Me.Controls.Add(Me.lblCommonMods)
        Me.Controls.Add(Me.lblFormulaEntry)
        Me.Controls.Add(Me.cboCommonMods)
        Me.Controls.Add(Me.txtFormulaEntry)
        Me.Name = "frmMassHelper"
        Me.Text = "Mass Calculation Helper"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Property mySettings() As clsSettings
        Get
            Return m_mySettings
        End Get
        Set(ByVal Value As clsSettings)
            m_mySettings = Value
        End Set
    End Property

    Private Sub frmMassHelper_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Load Combobox from settings file
        LoadCommonModsComboBox(Me.cboCommonMods, mySettings.CommonModsCollection)

        AddHandler cboCommonMods.SelectedIndexChanged, AddressOf cboCommonMods_SelectedIndexChanged
        Me.cboCommonMods.SelectedIndex = 0
    End Sub


    Private Sub cmdCalculateValues_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCalculateValues.Click
        CalculateValues()
    End Sub


    Private Sub CalculateValues()

        Dim mwtToAdd As New clsMolecularWeightCalc
        Dim mwtToSubtract As New clsMolecularWeightCalc

        Dim m_monoMassToAdd As Single
        Dim m_monoMassToSubtract As Single
        Dim m_avgMassToAdd As Single
        Dim m_avgMassToSubtract As Single

        m_FormulaToAdd = Me.txtFormulaEntry.Text

        With mwtToAdd
            .CalculateMasses(m_FormulaToAdd)
            m_FormulaToAdd = .EmpiricalFormula
            m_monoMassToAdd = .MonoMass
            m_avgMassToAdd = .AverageMass
        End With

        If Me.chkAttachAtHydrogen.Checked Then
            With mwtToSubtract
                .CalculateMasses("H")
                m_FormulaToSubtract = .EmpiricalFormula
                m_monoMassToSubtract = .MonoMass
                m_avgMassToSubtract = .AverageMass
            End With
        Else
            m_FormulaToSubtract = ""
            m_monoMassToSubtract = 0.0
            m_avgMassToSubtract = 0.0
        End If

        Dim m_monoMass As Single = m_monoMassToAdd - m_monoMassToSubtract
        Dim m_avgMass As Single = m_avgMassToAdd - m_avgMassToSubtract

        With Me
            .txtFormulaEntry.Text = mwtToAdd.EmpiricalFormula
            .txtMonoMass.Text = Format(m_monoMass, "0.0000").ToString
            .txtAvgMass.Text = Format(m_avgMass, "0.0000").ToString
        End With


    End Sub

    Private Sub cmdAddToList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddToList.Click

    End Sub

    Private Sub cboCommonMods_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim m_ID As Integer
        m_ID = Me.cboCommonMods.SelectedIndex
        m_FormulaToAdd = mySettings.CommonModsCollection(m_ID).Formula
        Me.txtFormulaEntry.Text = m_FormulaToAdd
        CalculateValues()
    End Sub

    Private Sub chkAttachAtHydrogen_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAttachAtHydrogen.CheckedChanged
        If Me.chkAttachAtHydrogen.Checked = True Then
            m_UseHyd = True
        Else
            m_UseHyd = False
        End If
        CalculateValues()
    End Sub

    Private Sub LoadCommonModsComboBox(ByVal cbo As ComboBox, ByVal ModificationsCollection As clsCommonModsCollection)
        Dim modification As New clsCommonModDetails
        Dim currentIndex As Integer

        ModificationsCollection.sort()

        For Each modification In ModificationsCollection
            cbo.Items.Add(modification.Description)
        Next

    End Sub

End Class
