Public Class frmGlobalModNamer
    Inherits Form

#Region "Windows Form Designer generated code"

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
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
    Friend WithEvents lblSymbolName As System.Windows.Forms.Label
    Friend WithEvents txtSymbolName As System.Windows.Forms.TextBox
    Friend WithEvents lblModMass As System.Windows.Forms.Label
    Friend WithEvents txtModMass As System.Windows.Forms.TextBox
    Friend WithEvents gbxExistingMods As System.Windows.Forms.GroupBox
    Friend WithEvents colSymbol As System.Windows.Forms.ColumnHeader
    Friend WithEvents colModMass As System.Windows.Forms.ColumnHeader
    Friend WithEvents gbxNewMod As System.Windows.Forms.GroupBox
    Friend WithEvents lvwExistingMods As System.Windows.Forms.ListView
    Friend WithEvents colDescription As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmdAddNew As System.Windows.Forms.Button
    Friend WithEvents errorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents toolTipProvider As System.Windows.Forms.ToolTip
    Friend WithEvents cmdUseSelectedMod As System.Windows.Forms.Button
    Friend WithEvents lblMessages As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
        gbxNewMod = New System.Windows.Forms.GroupBox()
        lblDescription = New System.Windows.Forms.Label()
        txtDescription = New System.Windows.Forms.TextBox()
        cmdAddNew = New System.Windows.Forms.Button()
        txtModMass = New System.Windows.Forms.TextBox()
        lblModMass = New System.Windows.Forms.Label()
        txtSymbolName = New System.Windows.Forms.TextBox()
        lblSymbolName = New System.Windows.Forms.Label()
        gbxExistingMods = New System.Windows.Forms.GroupBox()
        lvwExistingMods = New System.Windows.Forms.ListView()
        colSymbol = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        colDescription = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        colModMass = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        errorProvider = New System.Windows.Forms.ErrorProvider(components)
        toolTipProvider = New System.Windows.Forms.ToolTip(components)
        lblMessages = New System.Windows.Forms.Label()
        cmdUseSelectedMod = New System.Windows.Forms.Button()
        gbxNewMod.SuspendLayout()
        gbxExistingMods.SuspendLayout()
        CType(errorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        '
        'gbxNewMod
        '
        gbxNewMod.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        gbxNewMod.Controls.Add(lblDescription)
        gbxNewMod.Controls.Add(txtDescription)
        gbxNewMod.Controls.Add(cmdAddNew)
        gbxNewMod.Controls.Add(txtModMass)
        gbxNewMod.Controls.Add(lblModMass)
        gbxNewMod.Controls.Add(txtSymbolName)
        gbxNewMod.Controls.Add(lblSymbolName)
        gbxNewMod.Location = New System.Drawing.Point(17, 10)
        gbxNewMod.Name = "gbxNewMod"
        gbxNewMod.Size = New System.Drawing.Size(569, 261)
        gbxNewMod.TabIndex = 0
        gbxNewMod.TabStop = False
        gbxNewMod.Text = "New Mod Info"
        '
        'lblDescription
        '
        lblDescription.Location = New System.Drawing.Point(39, 89)
        lblDescription.Name = "lblDescription"
        lblDescription.Size = New System.Drawing.Size(179, 19)
        lblDescription.TabIndex = 6
        lblDescription.Text = "Description"
        '
        'txtDescription
        '
        txtDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        txtDescription.Location = New System.Drawing.Point(39, 108)
        txtDescription.Multiline = True
        txtDescription.Name = "txtDescription"
        txtDescription.Size = New System.Drawing.Size(513, 123)
        txtDescription.TabIndex = 5
        '
        'cmdAddNew
        '
        cmdAddNew.DialogResult = System.Windows.Forms.DialogResult.OK
        cmdAddNew.FlatStyle = System.Windows.Forms.FlatStyle.System
        cmdAddNew.Location = New System.Drawing.Point(409, 43)
        cmdAddNew.Name = "cmdAddNew"
        cmdAddNew.Size = New System.Drawing.Size(140, 28)
        cmdAddNew.TabIndex = 4
        cmdAddNew.Text = "Add New Mod"
        '
        'txtModMass
        '
        txtModMass.Location = New System.Drawing.Point(224, 44)
        txtModMass.MaxLength = 8
        txtModMass.Name = "txtModMass"
        txtModMass.Size = New System.Drawing.Size(146, 23)
        txtModMass.TabIndex = 3
        '
        'lblModMass
        '
        lblModMass.Location = New System.Drawing.Point(224, 25)
        lblModMass.Name = "lblModMass"
        lblModMass.Size = New System.Drawing.Size(78, 19)
        lblModMass.TabIndex = 2
        lblModMass.Text = "Mod Mass"
        '
        'txtSymbolName
        '
        txtSymbolName.Location = New System.Drawing.Point(39, 44)
        txtSymbolName.MaxLength = 8
        txtSymbolName.Name = "txtSymbolName"
        txtSymbolName.Size = New System.Drawing.Size(146, 23)
        txtSymbolName.TabIndex = 1
        '
        'lblSymbolName
        '
        lblSymbolName.Location = New System.Drawing.Point(39, 25)
        lblSymbolName.Name = "lblSymbolName"
        lblSymbolName.Size = New System.Drawing.Size(118, 19)
        lblSymbolName.TabIndex = 0
        lblSymbolName.Text = "Mod Name"
        '
        'gbxExistingMods
        '
        gbxExistingMods.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        gbxExistingMods.Controls.Add(lvwExistingMods)
        gbxExistingMods.Location = New System.Drawing.Point(17, 281)
        gbxExistingMods.Name = "gbxExistingMods"
        gbxExistingMods.Size = New System.Drawing.Size(569, 379)
        gbxExistingMods.TabIndex = 1
        gbxExistingMods.TabStop = False
        gbxExistingMods.Text = "Closely-Matching Existing Entries"
        '
        'lvwExistingMods
        '
        lvwExistingMods.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        lvwExistingMods.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {colSymbol, colDescription, colModMass})
        lvwExistingMods.FullRowSelect = True
        lvwExistingMods.GridLines = True
        lvwExistingMods.HideSelection = False
        lvwExistingMods.Location = New System.Drawing.Point(17, 25)
        lvwExistingMods.MultiSelect = False
        lvwExistingMods.Name = "lvwExistingMods"
        lvwExistingMods.Size = New System.Drawing.Size(535, 334)
        lvwExistingMods.TabIndex = 0
        lvwExistingMods.UseCompatibleStateImageBehavior = False
        lvwExistingMods.View = System.Windows.Forms.View.Details
        '
        'colSymbol
        '
        colSymbol.Text = "Mod Name"
        colSymbol.Width = 64
        '
        'colDescription
        '
        colDescription.Text = "Description"
        colDescription.Width = 240
        '
        'colModMass
        '
        colModMass.Text = "Mod Mass"
        colModMass.Width = 85
        '
        'errorProvider
        '
        errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink
        errorProvider.ContainerControl = Me
        '
        'lblMessages
        '
        lblMessages.Location = New System.Drawing.Point(28, 665)
        lblMessages.Name = "lblMessages"
        lblMessages.Size = New System.Drawing.Size(566, 44)
        lblMessages.TabIndex = 3
        lblMessages.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmdUseSelectedMod
        '
        cmdUseSelectedMod.Location = New System.Drawing.Point(381, 681)
        cmdUseSelectedMod.Name = "cmdUseSelectedMod"
        cmdUseSelectedMod.Size = New System.Drawing.Size(224, 28)
        cmdUseSelectedMod.TabIndex = 4
        cmdUseSelectedMod.Text = "Use Selected Mod Mass"
        cmdUseSelectedMod.UseVisualStyleBackColor = True
        '
        'frmGlobalModNamer
        '
        AcceptButton = cmdAddNew
        AutoScaleBaseSize = New System.Drawing.Size(7, 16)
        ClientSize = New System.Drawing.Size(603, 586)
        Controls.Add(cmdUseSelectedMod)
        Controls.Add(gbxExistingMods)
        Controls.Add(gbxNewMod)
        Controls.Add(lblMessages)
        Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Name = "frmGlobalModNamer"
        Text = "Add New Global Modification"
        gbxNewMod.ResumeLayout(False)
        gbxNewMod.PerformLayout()
        gbxExistingMods.ResumeLayout(False)
        CType(errorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)

    End Sub

#End Region

    Private m_SymbolValid As Boolean = False
    Private m_MassValid As Boolean = True
    Private m_DescValid As Boolean = False

    Friend Property MassCorrectionsTable As DataTable

    Friend Property NewSymbol As String
        Get
            Return txtSymbolName.Text
        End Get
        Set
            txtSymbolName.Text = Value
        End Set
    End Property

    Friend Property NewModMass As Double
        Get
            Return CDbl(txtModMass.Text)
        End Get
        Set
            txtModMass.Text = Format(CDbl(Value), "0.00000")
            ' m_origMass = Value
        End Set
    End Property

    Friend Property NewDescription As String
        Get
            Return txtDescription.Text
        End Get
        Set
            txtDescription.Text = Value
        End Set
    End Property

    Friend Property ModType As IMassTweaker.ModTypes

    Friend Property AffectedResidues As String

    Friend Sub LoadGlobalMods(ModMass As Double, AffectedAtom As String)
        LoadExistingModsListView(lvwExistingMods, ModMass, AffectedAtom)
    End Sub

    Private Sub LoadExistingModsListView(
        lvw As ListView,
        ModMass As Double,
        AffectedAtom As String)

        Dim modRow As DataRow
        Dim modRows() As DataRow

        Dim filterString As String
        Const MassVariance = 3.0
        Dim counter = 1

        Do

            filterString = "([Monoisotopic_Mass] >= " & ModMass - MassVariance * counter & " AND " &
                            "[Monoisotopic_Mass] <= " & ModMass + MassVariance * counter & ") AND " &
                            "([Affected_Atom] = '" & AffectedAtom & "')"

            modRows = MassCorrectionsTable.Select(filterString, "[Monoisotopic_Mass]")

            counter += 1

        Loop Until modRows.Length >= 10 Or counter = 10

        lvw.BeginUpdate()

        For Each modRow In modRows
            Dim item As New ListViewItem
            item.Text = CStr(modRow.Item(1))
            item.SubItems.Add(CStr(modRow.Item(2)))
            item.SubItems.Add(CStr(modRow.Item(3)))
            lvw.Items.Add(item)
        Next
        lvw.EndUpdate()

    End Sub

#Region "Validation Stuff"

    Private Sub txtModMass_Validating(sender As Object, e As ComponentModel.CancelEventArgs)
        Dim thisControl = DirectCast(sender, TextBox)
        Dim chk As String = thisControl.Text
        Dim tmpNewMass As Double

        If Not Double.TryParse(chk, 0) Then
            e.Cancel = True
            errorProvider.SetError(thisControl, "Not a valid number")
            m_MassValid = False
        ElseIf IsNumeric(chk) = True And Math.Abs(CSng(chk)) > Single.Epsilon Then
            errorProvider.SetError(thisControl, "")
            tmpNewMass = CDbl(chk)

        End If
        thisControl.Text = Format(CDbl(tmpNewMass), "0.00000")

    End Sub

    Private Sub txtModMass_Validated(sender As Object, e As EventArgs) Handles txtModMass.Validated
        m_MassValid = True
    End Sub

    Private Sub txtSymbolName_Validating(sender As Object, e As ComponentModel.CancelEventArgs) Handles txtSymbolName.Validating
        Dim thisControl = DirectCast(sender, TextBox)
        Dim chk As String = thisControl.Text

        If Len(chk) > 8 Then
            e.Cancel = True
            m_SymbolValid = False
            errorProvider.SetError(thisControl, "Modification Name must be < 8 characters")
        ElseIf CheckSymbolNameExists(chk) Then
            m_SymbolValid = False
            errorProvider.SetError(thisControl, "Modification Name already exists in database")
        Else
            errorProvider.SetError(thisControl, "")
        End If

    End Sub

    Private Sub txtSymbolName_Validated(sender As Object, e As EventArgs) Handles txtSymbolName.Validated
        'NewSymbol = txtSymbolName.Text
        m_SymbolValid = True
    End Sub

#End Region
    Private Function Validate_cmdAddNew() As Boolean
        If Not m_MassValid Or Not m_SymbolValid Or Not m_DescValid Then
            If Not m_MassValid Then
                'e.Cancel = True
                errorProvider.SetError(txtModMass, "Need a Valid Mass Value")
                txtModMass.Focus()
                Return False
            ElseIf Not m_SymbolValid Then
                'e.Cancel = True
                errorProvider.SetError(txtSymbolName, "Need a Valid Mod Name")
                txtSymbolName.Focus()
                Return False
            ElseIf Not m_DescValid Then
                'e.Cancel = True
                errorProvider.SetError(txtDescription, "Need a Valid Description")
                txtDescription.Focus()
                Return False
            End If
        End If

        If CheckSymbolNameExists(txtSymbolName.Text) Then
            m_SymbolValid = False
            Dim msgText = "Modification Name already exists in database"
            errorProvider.SetError(txtSymbolName, msgText)
            lblMessages.Text = msgText
            Return False
        End If

        Dim msg As String = "Are you sure you want to add the following global mod entry?" & vbCrLf &
            "Name: " & NewSymbol &
            ", Mass Diff: " & NewModMass.ToString

        Dim dr As MsgBoxResult = MsgBox(msg, MsgBoxStyle.YesNo, "Confirm Global Mod Addition")
        If dr = MsgBoxResult.Yes Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Sub cmdAddNew_Click(sender As System.Object, e As EventArgs) Handles cmdAddNew.Click
        If Validate_cmdAddNew() Then
            If SelectedModExists(CDbl(txtModMass.Text)) Then
                DialogResult = DialogResult.Yes
            Else
                DialogResult = DialogResult.OK
            End If
        Else
            DialogResult = DialogResult.None
        End If
    End Sub

    Private Sub txtDescription_Validating(sender As Object, e As ComponentModel.CancelEventArgs) Handles txtDescription.Validating
        Dim thisControl = DirectCast(sender, TextBox)
        If Len(thisControl.Text) <> 0 Then
            m_DescValid = True
        Else
            m_DescValid = False
        End If
    End Sub

    Private Sub lvwExistingMods_SelectedIndexChanged(sender As System.Object, e As EventArgs) Handles lvwExistingMods.SelectedIndexChanged

        Dim tmpSymbol As String

        Dim tmpDesc As String

        Dim tmpIndex As Integer
        If lvwExistingMods.SelectedIndices.Count = 0 Then
            Exit Sub
        Else
            tmpIndex = CInt(lvwExistingMods.SelectedIndices(0))

            tmpSymbol = lvwExistingMods.Items(tmpIndex).Text
            tmpDesc = lvwExistingMods.Items(tmpIndex).SubItems(1).Text

            txtSymbolName.Text = tmpSymbol
            txtDescription.Text = tmpDesc
        End If

    End Sub

    Private Function CheckSymbolNameExists(symbolName As String) As Boolean
        Dim rows() As DataRow = MassCorrectionsTable.Select("[Mass_Correction_Tag] = '" & symbolName & "'")

        If rows.Length > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Function SelectedModExists(modMass As Double) As Boolean

        Dim rows() As DataRow = MassCorrectionsTable.Select("[Monoisotopic_Mass] = " & modMass)

        If rows.Length > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function ConvertModTypeToString(eModType As IMassTweaker.ModTypes) As String
        If eModType = IMassTweaker.ModTypes.DynamicMod Then
            Return "D"
        Else
            Return "S"
        End If
    End Function

    Private Sub frmGlobalModNamer_Closing(sender As Object, e As ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If DialogResult = DialogResult.Abort Then
            e.Cancel = True
        End If
    End Sub

    Private Sub lvwExistingMods_DoubleClick(sender As Object, e As EventArgs) Handles lvwExistingMods.DoubleClick

        Dim modExists As Boolean = UseSelectedMod()

        If modExists Then
            DialogResult = DialogResult.Yes
            errorProvider.SetError(lblMessages, "")
            lblMessages.Text = ""
        Else

        End If
    End Sub

    Private Sub cmdUseSelectedMod_Click(sender As System.Object, e As EventArgs) Handles cmdUseSelectedMod.Click
        Dim modExists As Boolean = UseSelectedMod()
        If modExists Then
            DialogResult = DialogResult.Yes
            errorProvider.SetError(lblMessages, "")
            lblMessages.Text = ""
        End If
    End Sub

    Private Function UseSelectedMod() As Boolean
        Dim modExists = False

        Try
            Dim lvw As ListView = lvwExistingMods
            If lvw.SelectedItems.Count = 0 Then Exit Try

            Dim tmpNewSymbol As String = lvw.SelectedItems(0).Text
            Dim tmpNewDesc As String = lvw.SelectedItems(0).SubItems(1).Text
            Dim tmpNewMass = CDbl(lvw.SelectedItems(0).SubItems(2).Text)

            NewSymbol = tmpNewSymbol
            NewDescription = tmpNewDesc
            NewModMass = tmpNewMass

            modExists = SelectedModExists(tmpNewMass)

            If Not modExists Then
                MessageBox.Show("Could not find mass " & tmpNewMass.ToString & " in the mod list (MassCorrectionsTable).", "Not found", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show("Exception looking for specified mod: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try

        Return modExists

    End Function
End Class
