Imports ParamFileGenerator

Public Class frmDMSParamNamer
    Inherits System.Windows.Forms.Form

    Private m_MainForm As frmMainGUI
    Private m_Params As clsParams
    Private m_SaveName As String
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Private m_clsDMSParams As clsDMSParamUpload
    'Private m_clsDMSUpload As clsDMSParamUpload


#Region "Windows Form Designer generated code"

    Public Sub New(CallingFrm As frmMainGUI, ParamSetToSave As clsParams)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        m_MainForm = CallingFrm
        m_Params = ParamSetToSave
        m_clsDMSParams = New clsDMSParamUpload(frmMainGUI.mySettings)
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
    Friend WithEvents txtSaveFileName As System.Windows.Forms.TextBox
    Friend WithEvents lblSaveFileName As System.Windows.Forms.Label
    Friend WithEvents lblDiffs As System.Windows.Forms.Label
    Friend WithEvents txtDiffs As System.Windows.Forms.TextBox
    Friend WithEvents cmdUpload As System.Windows.Forms.Button
    Friend WithEvents NamingErrorProvider As System.Windows.Forms.ErrorProvider
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.txtSaveFileName = New System.Windows.Forms.TextBox()
        Me.lblSaveFileName = New System.Windows.Forms.Label()
        Me.lblDiffs = New System.Windows.Forms.Label()
        Me.txtDiffs = New System.Windows.Forms.TextBox()
        Me.cmdUpload = New System.Windows.Forms.Button()
        Me.NamingErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button()
        CType(Me.NamingErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtSaveFileName
        '
        Me.txtSaveFileName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSaveFileName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSaveFileName.Location = New System.Drawing.Point(14, 28)
        Me.txtSaveFileName.MaxLength = 255
        Me.txtSaveFileName.Name = "txtSaveFileName"
        Me.txtSaveFileName.Size = New System.Drawing.Size(640, 24)
        Me.txtSaveFileName.TabIndex = 0
        '
        'lblSaveFileName
        '
        Me.lblSaveFileName.Location = New System.Drawing.Point(14, 9)
        Me.lblSaveFileName.Name = "lblSaveFileName"
        Me.lblSaveFileName.Size = New System.Drawing.Size(245, 19)
        Me.lblSaveFileName.TabIndex = 1
        Me.lblSaveFileName.Text = "Descriptive Filename for DMS Save"
        '
        'lblDiffs
        '
        Me.lblDiffs.Location = New System.Drawing.Point(14, 65)
        Me.lblDiffs.Name = "lblDiffs"
        Me.lblDiffs.Size = New System.Drawing.Size(250, 18)
        Me.lblDiffs.TabIndex = 2
        Me.lblDiffs.Text = "Differences from Standard Template"
        '
        'txtDiffs
        '
        Me.txtDiffs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDiffs.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDiffs.Location = New System.Drawing.Point(14, 83)
        Me.txtDiffs.Multiline = True
        Me.txtDiffs.Name = "txtDiffs"
        Me.txtDiffs.Size = New System.Drawing.Size(640, 152)
        Me.txtDiffs.TabIndex = 3
        '
        'cmdUpload
        '
        Me.cmdUpload.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.cmdUpload.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cmdUpload.Location = New System.Drawing.Point(17, 241)
        Me.cmdUpload.Name = "cmdUpload"
        Me.cmdUpload.Size = New System.Drawing.Size(130, 26)
        Me.cmdUpload.TabIndex = 4
        Me.cmdUpload.Text = "Upload to DMS..."
        '
        'NamingErrorProvider
        '
        Me.NamingErrorProvider.ContainerControl = Me
        '
        'cmdCancel
        '
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cmdCancel.Location = New System.Drawing.Point(181, 241)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(130, 26)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "Cancel"
        '
        'frmDMSParamNamer
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(666, 289)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdUpload)
        Me.Controls.Add(Me.txtDiffs)
        Me.Controls.Add(Me.txtSaveFileName)
        Me.Controls.Add(Me.lblDiffs)
        Me.Controls.Add(Me.lblSaveFileName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmDMSParamNamer"
        Me.Text = "Save Param Set to T_Sequest_Params"
        CType(Me.NamingErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub frmDMSParamNamer_Load(sender As Object, e As System.EventArgs) Handles MyBase.Load
        Me.txtSaveFileName.Text = m_Params.FileName
        m_clsDMSParams = New clsDMSParamUpload(frmMainGUI.mySettings)
        LoadParamDiffs(m_Params)

    End Sub

    Private Sub cmdUpload_Click(sender As System.Object, e As System.EventArgs) Handles cmdUpload.Click

        Dim nameExists As Boolean
        Dim ParamSetID As Integer
        Dim IDExists As Boolean
        Dim eDialogResult As New DialogResult
        Dim blnSuccess As Boolean

        Try

            nameExists = m_clsDMSParams.ParamSetNameExists(m_SaveName)
            ParamSetID = m_Params.DMS_ID
            IDExists = m_clsDMSParams.ParamSetIDExists(ParamSetID)

            m_Params.FileName = m_SaveName
            m_Params.Description = txtDiffs.Text

            If IDExists And nameExists Then
                eDialogResult = MessageBox.Show("This Parameter Set already exists. Would you like to replace it with the current Parameter set?",
                    "Parameter set exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                If eDialogResult = DialogResult.No Then
                    Exit Sub
                End If
            ElseIf nameExists And Not IDExists Then
                eDialogResult = MessageBox.Show("A Parameter Set with this name already exists. Would you like to replace it with the current Parameter set?",
                    "Parameter set exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                If eDialogResult = DialogResult.No Then
                    Exit Sub
                End If

            Else
                eDialogResult = MessageBox.Show("Are you sure you want to add a new Parameter set?",
                    "Make New Parameter Set", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
                If eDialogResult = DialogResult.No Then
                    Exit Sub
                End If
            End If

            blnSuccess = m_clsDMSParams.WriteParamsToDMS(m_Params, False)

            If blnSuccess Then
                Windows.Forms.MessageBox.Show("File successfully uploaded: " & m_SaveName, "Succcess", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Windows.Forms.MessageBox.Show("A problem occurred while uploading the file " & m_SaveName & ": " & m_clsDMSParams.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

        Catch ex As Exception
            Windows.Forms.MessageBox.Show("Error in cmdUpload_Click: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try


        Me.Close()

    End Sub


    Private Sub LoadParamDiffs(ParamSet As clsParams)
        Dim dms As clsDMSParamUpload = m_clsDMSParams
        Me.txtDiffs.Text = dms.GetDiffsFromTemplate(ParamSet)
    End Sub

    Private Sub txtSaveFileName_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtSaveFileName.TextChanged
        If Me.txtSaveFileName.Text.Length < 255 Then
            Me.m_SaveName = Me.txtSaveFileName.Text
            Me.NamingErrorProvider.SetError(Me.lblSaveFileName, "")
        Else
            Me.NamingErrorProvider.SetError(Me.lblSaveFileName, "Parameter File names must be < 255 characters in length")
        End If
    End Sub

    Private Sub txtSaveFileName_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtSaveFileName.Validating
        Dim tb As TextBox = DirectCast(sender, TextBox)
        If tb.Text.Length >= 255 Then
            Me.NamingErrorProvider.SetError(Me.lblSaveFileName, "Parameter File names must be < 255 characters in length")
            e.Cancel = True
        Else
            Me.NamingErrorProvider.SetError(Me.lblSaveFileName, "")
        End If

    End Sub

    Private Sub cmdCancel_Click(sender As System.Object, e As System.EventArgs) Handles cmdCancel.Click

        Me.Close()

    End Sub
End Class
