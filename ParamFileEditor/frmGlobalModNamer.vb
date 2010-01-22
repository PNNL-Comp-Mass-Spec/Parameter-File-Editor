Public Class frmGlobalModNamer
    Inherits System.Windows.Forms.Form

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
        Me.components = New System.ComponentModel.Container
        Me.gbxNewMod = New System.Windows.Forms.GroupBox
        Me.lblDescription = New System.Windows.Forms.Label
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.cmdAddNew = New System.Windows.Forms.Button
        Me.txtModMass = New System.Windows.Forms.TextBox
        Me.lblModMass = New System.Windows.Forms.Label
        Me.txtSymbolName = New System.Windows.Forms.TextBox
        Me.lblSymbolName = New System.Windows.Forms.Label
        Me.gbxExistingMods = New System.Windows.Forms.GroupBox
        Me.lvwExistingMods = New System.Windows.Forms.ListView
        Me.colSymbol = New System.Windows.Forms.ColumnHeader
        Me.colDescription = New System.Windows.Forms.ColumnHeader
        Me.colModMass = New System.Windows.Forms.ColumnHeader
        Me.errorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.toolTipProvider = New System.Windows.Forms.ToolTip(Me.components)
        Me.lblMessages = New System.Windows.Forms.Label
        Me.cmdUseSelectedMod = New System.Windows.Forms.Button
        Me.gbxNewMod.SuspendLayout()
        Me.gbxExistingMods.SuspendLayout()
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gbxNewMod
        '
        Me.gbxNewMod.Controls.Add(Me.lblDescription)
        Me.gbxNewMod.Controls.Add(Me.txtDescription)
        Me.gbxNewMod.Controls.Add(Me.cmdAddNew)
        Me.gbxNewMod.Controls.Add(Me.txtModMass)
        Me.gbxNewMod.Controls.Add(Me.lblModMass)
        Me.gbxNewMod.Controls.Add(Me.txtSymbolName)
        Me.gbxNewMod.Controls.Add(Me.lblSymbolName)
        Me.gbxNewMod.Location = New System.Drawing.Point(12, 8)
        Me.gbxNewMod.Name = "gbxNewMod"
        Me.gbxNewMod.Size = New System.Drawing.Size(420, 212)
        Me.gbxNewMod.TabIndex = 0
        Me.gbxNewMod.TabStop = False
        Me.gbxNewMod.Text = "New Mod Info"
        '
        'lblDescription
        '
        Me.lblDescription.Location = New System.Drawing.Point(28, 72)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(128, 16)
        Me.lblDescription.TabIndex = 6
        Me.lblDescription.Text = "Description"
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(28, 88)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(364, 100)
        Me.txtDescription.TabIndex = 5
        '
        'cmdAddNew
        '
        Me.cmdAddNew.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdAddNew.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cmdAddNew.Location = New System.Drawing.Point(292, 35)
        Me.cmdAddNew.Name = "cmdAddNew"
        Me.cmdAddNew.Size = New System.Drawing.Size(100, 23)
        Me.cmdAddNew.TabIndex = 4
        Me.cmdAddNew.Text = "Add New Mod"
        '
        'txtModMass
        '
        Me.txtModMass.Location = New System.Drawing.Point(160, 36)
        Me.txtModMass.MaxLength = 8
        Me.txtModMass.Name = "txtModMass"
        Me.txtModMass.Size = New System.Drawing.Size(104, 20)
        Me.txtModMass.TabIndex = 3
        '
        'lblModMass
        '
        Me.lblModMass.Location = New System.Drawing.Point(160, 20)
        Me.lblModMass.Name = "lblModMass"
        Me.lblModMass.Size = New System.Drawing.Size(56, 16)
        Me.lblModMass.TabIndex = 2
        Me.lblModMass.Text = "Mod Mass"
        '
        'txtSymbolName
        '
        Me.txtSymbolName.Location = New System.Drawing.Point(28, 36)
        Me.txtSymbolName.MaxLength = 8
        Me.txtSymbolName.Name = "txtSymbolName"
        Me.txtSymbolName.Size = New System.Drawing.Size(104, 20)
        Me.txtSymbolName.TabIndex = 1
        '
        'lblSymbolName
        '
        Me.lblSymbolName.Location = New System.Drawing.Point(28, 20)
        Me.lblSymbolName.Name = "lblSymbolName"
        Me.lblSymbolName.Size = New System.Drawing.Size(84, 16)
        Me.lblSymbolName.TabIndex = 0
        Me.lblSymbolName.Text = "Mod Name"
        '
        'gbxExistingMods
        '
        Me.gbxExistingMods.Controls.Add(Me.lvwExistingMods)
        Me.gbxExistingMods.Location = New System.Drawing.Point(12, 228)
        Me.gbxExistingMods.Name = "gbxExistingMods"
        Me.gbxExistingMods.Size = New System.Drawing.Size(420, 308)
        Me.gbxExistingMods.TabIndex = 1
        Me.gbxExistingMods.TabStop = False
        Me.gbxExistingMods.Text = "Closely-Matching Existing Entries"
        '
        'lvwExistingMods
        '
        Me.lvwExistingMods.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colSymbol, Me.colDescription, Me.colModMass})
        Me.lvwExistingMods.FullRowSelect = True
        Me.lvwExistingMods.GridLines = True
        Me.lvwExistingMods.HideSelection = False
        Me.lvwExistingMods.Location = New System.Drawing.Point(12, 20)
        Me.lvwExistingMods.MultiSelect = False
        Me.lvwExistingMods.Name = "lvwExistingMods"
        Me.lvwExistingMods.Size = New System.Drawing.Size(396, 272)
        Me.lvwExistingMods.TabIndex = 0
        Me.lvwExistingMods.UseCompatibleStateImageBehavior = False
        Me.lvwExistingMods.View = System.Windows.Forms.View.Details
        '
        'colSymbol
        '
        Me.colSymbol.Text = "Mod Name"
        Me.colSymbol.Width = 64
        '
        'colDescription
        '
        Me.colDescription.Text = "Description"
        Me.colDescription.Width = 264
        '
        'colModMass
        '
        Me.colModMass.Text = "Mod Mass"
        Me.colModMass.Width = 64
        '
        'errorProvider
        '
        Me.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink
        Me.errorProvider.ContainerControl = Me
        '
        'lblMessages
        '
        Me.lblMessages.Location = New System.Drawing.Point(20, 540)
        Me.lblMessages.Name = "lblMessages"
        Me.lblMessages.Size = New System.Drawing.Size(404, 36)
        Me.lblMessages.TabIndex = 3
        Me.lblMessages.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmdUseSelectedMod
        '
        Me.cmdUseSelectedMod.Location = New System.Drawing.Point(272, 553)
        Me.cmdUseSelectedMod.Name = "cmdUseSelectedMod"
        Me.cmdUseSelectedMod.Size = New System.Drawing.Size(160, 23)
        Me.cmdUseSelectedMod.TabIndex = 4
        Me.cmdUseSelectedMod.Text = "Use Selected Mod Mass"
        Me.cmdUseSelectedMod.UseVisualStyleBackColor = True
        '
        'frmGlobalModNamer
        '
        Me.AcceptButton = Me.cmdAddNew
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(444, 586)
        Me.Controls.Add(Me.cmdUseSelectedMod)
        Me.Controls.Add(Me.gbxExistingMods)
        Me.Controls.Add(Me.gbxNewMod)
        Me.Controls.Add(Me.lblMessages)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmGlobalModNamer"
        Me.Text = "Add New Global Modification"
        Me.gbxNewMod.ResumeLayout(False)
        Me.gbxNewMod.PerformLayout()
        Me.gbxExistingMods.ResumeLayout(False)
        CType(Me.errorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private m_MassCorrectionsTable As DataTable

    Private m_SymbolValid As Boolean = False
    Private m_MassValid As Boolean = True
    Private m_DescValid As Boolean = False

    Private m_origMass As Single
    Private m_origType As IMassTweaker.ModTypes
    Private m_origResidues As String


    Friend Property MassCorrectionsTable() As DataTable
        Get
            Return Me.m_MassCorrectionsTable
        End Get
        Set(ByVal Value As DataTable)
            Me.m_MassCorrectionsTable = Value
        End Set
    End Property

    Friend Property NewSymbol() As String
        Get
            Return Me.txtSymbolName.Text
        End Get
        Set(ByVal Value As String)
            Me.txtSymbolName.Text = Value
        End Set
    End Property

    Friend Property NewModMass() As Single
        Get
            Return CSng(Me.txtModMass.Text)
        End Get
        Set(ByVal Value As Single)
            Me.txtModMass.Text = Format(CDbl(Value), "0.0000")
            Me.m_origMass = Value
        End Set
    End Property

    Friend Property NewDescription() As String
        Get
            Return Me.txtDescription.Text
        End Get
        Set(ByVal Value As String)
            Me.txtDescription.Text = Value
        End Set
    End Property

    Friend Property ModType() As IMassTweaker.ModTypes
        Get
            Return Me.m_origType
        End Get
        Set(ByVal Value As IMassTweaker.ModTypes)
            Me.m_origType = Value
        End Set
    End Property

    Friend Property AffectedResidues() As String
        Get
            Return Me.m_origResidues
        End Get
        Set(ByVal Value As String)
            Me.m_origResidues = Value
        End Set
    End Property

    Friend Sub LoadGlobalMods(ByVal ModMass As Single, ByVal AffectedAtom As String)
        Me.LoadExistingModsListview(Me.lvwExistingMods, ModMass, AffectedAtom)
    End Sub

    Private Sub LoadExistingModsListview( _
        ByVal lvw As ListView, _
        ByVal ModMass As Single, _
        ByVal AffectedAtom As String)

        Dim modRow As DataRow
        Dim modRows() As DataRow

        Dim filterString As String
        Const MassVariance As Single = 3.0
        Dim counter As Integer = 1

        Do

            filterString = "([Monoisotopic_Mass_Correction] >= " & ModMass - MassVariance * counter & " AND " & _
                            "[Monoisotopic_Mass_Correction] <= " & ModMass + MassVariance * counter & ") AND " & _
                            "([Affected_Atom] = '" & AffectedAtom & "')"

            modRows = Me.MassCorrectionsTable.Select(filterString, "[Monoisotopic_Mass_Correction]")

            counter += 1

        Loop Until modRows.Length >= 10 Or counter = 10

        lvw.BeginUpdate()

        For Each modRow In modRows
            Dim item As New ListViewItem
            item.Text = modRow.Item(1)
            item.SubItems.Add(modRow.Item(2))
            item.SubItems.Add(modRow.Item(3))
            lvw.Items.Add(item)
        Next
        lvw.EndUpdate()

    End Sub

#Region " Validation Stuff "

    Private Sub txtModMass_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        Dim chk As String = sender.Text
        Dim tmpNewMass As Single

        If IsNumeric(chk) = False Then
            e.Cancel = True
            Me.errorProvider.SetError(sender, "Not a valid number")
            Me.m_MassValid = False
        ElseIf IsNumeric(chk) = True And CSng(chk) <> 0.0 Then
            Me.errorProvider.SetError(sender, "")
            tmpNewMass = CSng(chk)

        End If
        sender.Text = Format(CDbl(tmpNewMass), "0.0000")

    End Sub

    Private Sub txtModMass_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtModMass.Validated
        Me.m_MassValid = True
    End Sub

    Private Sub txtSymbolName_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtSymbolName.Validating
        Dim chk As String = sender.Text

        If Len(chk) > 8 Then
            e.Cancel = True
            Me.m_SymbolValid = False
            Me.errorProvider.SetError(sender, "Modification Name must be < 8 characters")
        ElseIf CheckSymbolNameExists(chk) Then
            Me.m_SymbolValid = False
            Me.errorProvider.SetError(sender, "Modification Name already exists in database")
        Else
            Me.errorProvider.SetError(sender, "")
        End If

    End Sub

    Private Sub txtSymbolName_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSymbolName.Validated
        'Me.NewSymbol = Me.txtSymbolName.Text
        Me.m_SymbolValid = True
    End Sub

#End Region
    Private Function Validate_cmdAddNew() As Boolean
        If Not Me.m_MassValid Or Not Me.m_SymbolValid Or Not Me.m_DescValid Then
            If Not Me.m_MassValid Then
                'e.Cancel = True
                Me.errorProvider.SetError(Me.txtModMass, "Need a Valid Mass Value")
                Me.txtModMass.Focus()
                Return False
            ElseIf Not Me.m_SymbolValid Then
                'e.Cancel = True
                Me.errorProvider.SetError(Me.txtSymbolName, "Need a Valid Mod Name")
                Me.txtSymbolName.Focus()
                Return False
            ElseIf Not Me.m_DescValid Then
                'e.Cancel = True
                Me.errorProvider.SetError(Me.txtDescription, "Need a Valid Description")
                Me.txtDescription.Focus()
                Return False
            End If
        End If

        If CheckSymbolNameExists(Me.txtSymbolName.Text) Then
            Me.m_SymbolValid = False
            Dim msgText As String = "Modification Name already exists in database"
            Me.errorProvider.SetError(Me.txtSymbolName, msgText)
            Me.lblMessages.Text = msgText
            Return False
        End If

        Dim msg As String = "Are you sure you want to add the following global mod entry?" & vbCrLf & _
            "Name: " & Me.NewSymbol & _
            ", Mass Diff: " & Me.NewModMass.ToString

        Dim dr As MsgBoxResult = MsgBox(msg, MsgBoxStyle.YesNo, "Confirm Global Mod Addition")
        If dr = MsgBoxResult.Yes Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Sub cmdAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        If Validate_cmdAddNew() Then
            If Me.SelectedModExists(CSng(Me.txtModMass.Text)) Then
                DialogResult = DialogResult.Yes
            Else
                DialogResult = DialogResult.OK
            End If
        Else
            DialogResult = DialogResult.None
        End If
    End Sub

    Private Sub txtDescription_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtDescription.Validating
        If Len(sender.text) <> 0 Then
            Me.m_DescValid = True
        Else
            Me.m_DescValid = False
        End If
    End Sub

    Private Sub lvwExistingMods_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwExistingMods.SelectedIndexChanged

        Dim tmpSymbol As String

        Dim tmpDesc As String

        Dim tmpIndex As Integer
        If Me.lvwExistingMods.SelectedIndices.Count = 0 Then
            Exit Sub
        Else
            tmpIndex = CInt(Me.lvwExistingMods.SelectedIndices(0))

            tmpSymbol = Me.lvwExistingMods.Items(tmpIndex).Text
            tmpDesc = Me.lvwExistingMods.Items(tmpIndex).SubItems(1).Text

            Me.txtSymbolName.Text = tmpSymbol
            Me.txtDescription.Text = tmpDesc
        End If

    End Sub

    Private Function CheckSymbolNameExists(ByVal symbolName As String) As Boolean
        Dim rows() As DataRow = Me.MassCorrectionsTable.Select("[Mass_Correction_Tag] = '" & symbolName & "'")

        If rows.Length > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Function SelectedModExists(ByVal modMass As Double) As Boolean

        Dim rows() As DataRow = Me.MassCorrectionsTable.Select("[Monoisotopic_Mass_Correction] = " & modMass)

        If rows.Length > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function ConvertModTypeToString(ByVal modType As IMassTweaker.ModTypes) As String
        If modType = IMassTweaker.ModTypes.DynamicMod Then
            Return "D"
        Else
            Return "S"
        End If
    End Function

    Private Sub frmGlobalModNamer_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If DialogResult = DialogResult.Abort Then
            e.Cancel = True
        End If
    End Sub

    Private Sub lvwExistingMods_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwExistingMods.DoubleClick

        Dim modExists As Boolean = UseSelectedMod()

        If modExists Then
            DialogResult = DialogResult.Yes
            Me.errorProvider.SetError(Me.lblMessages, "")
            Me.lblMessages.Text = ""
        Else

        End If
    End Sub

    Private Sub cmdUseSelectedMod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUseSelectedMod.Click
        Dim modExists As Boolean = UseSelectedMod()
        If modExists Then
            DialogResult = DialogResult.Yes
            Me.errorProvider.SetError(Me.lblMessages, "")
            Me.lblMessages.Text = ""
        End If
    End Sub

    Private Function UseSelectedMod() As Boolean
        Dim modExists As Boolean = False

        Try
            Dim lvw As ListView = Me.lvwExistingMods
            If lvw.SelectedItems.Count = 0 Then Exit Try

            Dim tmpNewSymbol As String = lvw.SelectedItems(0).Text
            Dim tmpNewDesc As String = lvw.SelectedItems(0).SubItems(1).Text
            Dim tmpNewMass As Double = CDbl(lvw.SelectedItems(0).SubItems(2).Text)

            Me.NewSymbol = tmpNewSymbol
            Me.NewDescription = tmpNewDesc
            Me.NewModMass = tmpNewMass

            modExists = SelectedModExists(tmpNewMass)

            If Not modExists Then
                System.Windows.Forms.MessageBox.Show("Could not find mass " & tmpNewMass.ToString & " in the mod list (MassCorrectionsTable).", "Not found", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show("Exception looking for specified mod: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try

        Return modExists

    End Function
End Class
