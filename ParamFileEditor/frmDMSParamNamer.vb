Imports ParamFileGenerator

Public Class frmDMSParamNamer
    Inherits System.Windows.Forms.Form

    Private m_MainForm As frmMainGUI
    Private m_Params As clsParams
    Private m_SaveName As String
    Private m_clsDMSParams As clsDMSParamUpload
    'Private m_clsDMSUpload As clsDMSParamUpload


#Region " Windows Form Designer generated code "

    Public Sub New(ByVal CallingFrm As frmMainGUI, ByVal ParamSetToSave As clsParams)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        m_MainForm = CallingFrm
        m_Params = ParamSetToSave
        m_clsDMSParams = New clsDMSParamUpload(CallingFrm.mySettings)
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
    Friend WithEvents txtSaveFileName As System.Windows.Forms.TextBox
    Friend WithEvents lblSaveFileName As System.Windows.Forms.Label
    Friend WithEvents lblDiffs As System.Windows.Forms.Label
    Friend WithEvents txtDiffs As System.Windows.Forms.TextBox
    Friend WithEvents cmdUpload As System.Windows.Forms.Button
    Friend WithEvents NamingErrorProvider As System.Windows.Forms.ErrorProvider
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.txtSaveFileName = New System.Windows.Forms.TextBox
        Me.lblSaveFileName = New System.Windows.Forms.Label
        Me.lblDiffs = New System.Windows.Forms.Label
        Me.txtDiffs = New System.Windows.Forms.TextBox
        Me.cmdUpload = New System.Windows.Forms.Button
        Me.NamingErrorProvider = New System.Windows.Forms.ErrorProvider
        Me.SuspendLayout()
        '
        'txtSaveFileName
        '
        Me.txtSaveFileName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSaveFileName.Location = New System.Drawing.Point(12, 24)
        Me.txtSaveFileName.MaxLength = 64
        Me.txtSaveFileName.Name = "txtSaveFileName"
        Me.txtSaveFileName.Size = New System.Drawing.Size(376, 21)
        Me.txtSaveFileName.TabIndex = 0
        Me.txtSaveFileName.Text = ""
        '
        'lblSaveFileName
        '
        Me.lblSaveFileName.Location = New System.Drawing.Point(12, 8)
        Me.lblSaveFileName.Name = "lblSaveFileName"
        Me.lblSaveFileName.Size = New System.Drawing.Size(204, 16)
        Me.lblSaveFileName.TabIndex = 1
        Me.lblSaveFileName.Text = "Descriptive Filename for DMS Save"
        '
        'lblDiffs
        '
        Me.lblDiffs.Location = New System.Drawing.Point(12, 56)
        Me.lblDiffs.Name = "lblDiffs"
        Me.lblDiffs.Size = New System.Drawing.Size(208, 16)
        Me.lblDiffs.TabIndex = 2
        Me.lblDiffs.Text = "Differences from Standard Template"
        '
        'txtDiffs
        '
        Me.txtDiffs.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiffs.Location = New System.Drawing.Point(12, 72)
        Me.txtDiffs.Multiline = True
        Me.txtDiffs.Name = "txtDiffs"
        Me.txtDiffs.Size = New System.Drawing.Size(376, 132)
        Me.txtDiffs.TabIndex = 3
        Me.txtDiffs.Text = ""
        '
        'cmdUpload
        '
        Me.cmdUpload.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdUpload.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cmdUpload.Location = New System.Drawing.Point(280, 212)
        Me.cmdUpload.Name = "cmdUpload"
        Me.cmdUpload.Size = New System.Drawing.Size(108, 23)
        Me.cmdUpload.TabIndex = 4
        Me.cmdUpload.Text = "Upload to DMS..."
        '
        'NamingErrorProvider
        '
        Me.NamingErrorProvider.ContainerControl = Me
        '
        'frmDMSParamNamer
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(400, 242)
        Me.Controls.Add(Me.cmdUpload)
        Me.Controls.Add(Me.txtDiffs)
        Me.Controls.Add(Me.txtSaveFileName)
        Me.Controls.Add(Me.lblDiffs)
        Me.Controls.Add(Me.lblSaveFileName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmDMSParamNamer"
        Me.Text = "Save Param Set to T_Sequest_Params"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub frmDMSParamNamer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.txtSaveFileName.Text = m_Params.FileName
        m_clsDMSParams = New clsDMSParamUpload(Me.m_MainForm.mySettings)
        LoadParamDiffs(m_Params)

    End Sub

    Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
        'Dim dms As New clsParamsFromDMS(m_MainForm.MainCode.mySettings)
        Dim dms As clsDMSParamUpload = m_clsDMSParams
        Dim nameExists As Boolean = dms.ParamSetNameExists(m_SaveName)
        Dim ParamSetID As Integer = m_Params.DMS_ID
        Dim IDExists As Boolean = dms.ParamSetIDExists(ParamSetID)
        'Dim replace As Boolean = False

        Dim results As New DialogResult
        m_Params.FileName = m_SaveName

        If IDExists And nameExists Then
            results = MessageBox.Show("This Parameter Set already exists. Would you like to replace it with the current Parameter set?", _
                "Parameter set exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
            If results = DialogResult.No Then
                Exit Sub
            Else

            End If
        ElseIf nameExists And Not IDExists Then
            results = MessageBox.Show("A Parameter Set with this name already exists. Would you like to replace it with the current Parameter set?", _
                "Parameter set exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
            If results = DialogResult.No Then
                Exit Sub
            Else
            End If

        Else
            results = MessageBox.Show("Are you sure you want to add a new Parameter set?", _
                "Make New Parameter Set", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
            If results = DialogResult.No Then
                Exit Sub
            Else

            End If
        End If

        dms.WriteParamsToDMS(m_Params)

        Me.Close()

    End Sub


    Private Sub LoadParamDiffs(ByVal ParamSet As clsParams)
        Dim dms As clsDMSParamUpload = m_clsDMSParams
        Me.txtDiffs.Text = dms.GetDiffsFromTemplate(ParamSet)
    End Sub

    Private Sub txtSaveFileName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSaveFileName.TextChanged
        If Me.txtSaveFileName.Text.Length < 64 Then
            Me.m_SaveName = Me.txtSaveFileName.Text
            Me.NamingErrorProvider.SetError(Me.lblSaveFileName, "")
        Else
            Me.NamingErrorProvider.SetError(Me.lblSaveFileName, "Parameter File names must be < 64 characters in length")
        End If
    End Sub

    Private Sub txtSaveFileName_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtSaveFileName.Validating
        Dim tb As TextBox = DirectCast(sender, TextBox)
        If tb.Text.Length >= 64 Then
            Me.NamingErrorProvider.SetError(Me.lblSaveFileName, "Parameter File names must be < 64 characters in length")
            e.Cancel = True
        Else
            Me.NamingErrorProvider.SetError(Me.lblSaveFileName, "")
        End If

    End Sub
End Class
