Imports System.IO
Imports ParamFileGenerator.MakeParams
Imports PRISMDatabaseUtils

Public Class frmMain
    Inherits Form

#Region "Windows Form Designer generated code"

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

        mDMSConnectString = Me.txtDMSConnectionString.Text
        mOutputPath = Me.txtOutputPath.Text
        mFASTAPath = Me.txtFASTAPath.Text
        If mDMS Is Nothing Then
            mDMS = New clsMakeParameterFile()
        End If

        'Me.LoadParamNames()
        Me.LoadParamFileTypes()

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
    Friend WithEvents txtOutputPath As System.Windows.Forms.TextBox
    Friend WithEvents txtDMSConnectionString As System.Windows.Forms.TextBox
    Friend WithEvents cboFileTypes As System.Windows.Forms.ComboBox
    Friend WithEvents cboAvailableParams As System.Windows.Forms.ComboBox
    Friend WithEvents txtResults As System.Windows.Forms.TextBox
    Friend WithEvents cmdDoIt As System.Windows.Forms.Button
    Friend WithEvents txtFASTAPath As System.Windows.Forms.TextBox
    Friend WithEvents lblOutputPath As System.Windows.Forms.Label
    Friend WithEvents lblPickList As System.Windows.Forms.Label
    Friend WithEvents lblConnectionString As System.Windows.Forms.Label
    Friend WithEvents txtDatasetID As System.Windows.Forms.TextBox
    Friend WithEvents lblDatasetID As System.Windows.Forms.Label
    Friend WithEvents txtParamFileName As System.Windows.Forms.TextBox
    Friend WithEvents lblParamFileName As System.Windows.Forms.Label
    Friend WithEvents lblParamFileType As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.txtOutputPath = New System.Windows.Forms.TextBox
        Me.txtDMSConnectionString = New System.Windows.Forms.TextBox
        Me.cboFileTypes = New System.Windows.Forms.ComboBox
        Me.lblOutputPath = New System.Windows.Forms.Label
        Me.lblPickList = New System.Windows.Forms.Label
        Me.lblConnectionString = New System.Windows.Forms.Label
        Me.lblParamFileType = New System.Windows.Forms.Label
        Me.cboAvailableParams = New System.Windows.Forms.ComboBox
        Me.txtResults = New System.Windows.Forms.TextBox
        Me.cmdDoIt = New System.Windows.Forms.Button
        Me.txtFASTAPath = New System.Windows.Forms.TextBox
        Me.txtDatasetID = New System.Windows.Forms.TextBox
        Me.lblDatasetID = New System.Windows.Forms.Label
        Me.txtParamFileName = New System.Windows.Forms.TextBox
        Me.lblParamFileName = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'txtOutputPath
        '
        Me.txtOutputPath.Location = New System.Drawing.Point(156, 20)
        Me.txtOutputPath.Name = "txtOutputPath"
        Me.txtOutputPath.Size = New System.Drawing.Size(400, 20)
        Me.txtOutputPath.TabIndex = 0
        Me.txtOutputPath.Text = "F:\Temp\"
        '
        'txtDMSConnectionString
        '
        Me.txtDMSConnectionString.Location = New System.Drawing.Point(156, 125)
        Me.txtDMSConnectionString.Name = "txtDMSConnectionString"
        Me.txtDMSConnectionString.Size = New System.Drawing.Size(400, 20)
        Me.txtDMSConnectionString.TabIndex = 2
        Me.txtDMSConnectionString.Text = "Data Source=gigasax;Initial Catalog=DMS5;Integrated Security=SSPI;"
        '
        'cboFileTypes
        '
        Me.cboFileTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFileTypes.Location = New System.Drawing.Point(156, 165)
        Me.cboFileTypes.Name = "cboFileTypes"
        Me.cboFileTypes.Size = New System.Drawing.Size(400, 21)
        Me.cboFileTypes.TabIndex = 3
        '
        'lblOutputPath
        '
        Me.lblOutputPath.Location = New System.Drawing.Point(16, 24)
        Me.lblOutputPath.Name = "lblOutputPath"
        Me.lblOutputPath.Size = New System.Drawing.Size(120, 23)
        Me.lblOutputPath.TabIndex = 4
        Me.lblOutputPath.Text = "Output Path"
        '
        'lblPickList
        '
        Me.lblPickList.Location = New System.Drawing.Point(16, 64)
        Me.lblPickList.Name = "lblPickList"
        Me.lblPickList.Size = New System.Drawing.Size(120, 17)
        Me.lblPickList.TabIndex = 5
        Me.lblPickList.Text = "Param File Pick List"
        '
        'lblConnectionString
        '
        Me.lblConnectionString.Location = New System.Drawing.Point(16, 125)
        Me.lblConnectionString.Name = "lblConnectionString"
        Me.lblConnectionString.Size = New System.Drawing.Size(120, 23)
        Me.lblConnectionString.TabIndex = 6
        Me.lblConnectionString.Text = "Connect String"
        '
        'lblParamFileType
        '
        Me.lblParamFileType.Location = New System.Drawing.Point(16, 169)
        Me.lblParamFileType.Name = "lblParamFileType"
        Me.lblParamFileType.Size = New System.Drawing.Size(120, 23)
        Me.lblParamFileType.TabIndex = 7
        Me.lblParamFileType.Text = "Param File Type"
        '
        'cboAvailableParams
        '
        Me.cboAvailableParams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAvailableParams.Location = New System.Drawing.Point(156, 60)
        Me.cboAvailableParams.Name = "cboAvailableParams"
        Me.cboAvailableParams.Size = New System.Drawing.Size(400, 21)
        Me.cboAvailableParams.Sorted = True
        Me.cboAvailableParams.TabIndex = 8
        '
        'txtResults
        '
        Me.txtResults.Location = New System.Drawing.Point(24, 253)
        Me.txtResults.Multiline = True
        Me.txtResults.Name = "txtResults"
        Me.txtResults.Size = New System.Drawing.Size(384, 84)
        Me.txtResults.TabIndex = 9
        '
        'cmdDoIt
        '
        Me.cmdDoIt.Location = New System.Drawing.Point(436, 305)
        Me.cmdDoIt.Name = "cmdDoIt"
        Me.cmdDoIt.Size = New System.Drawing.Size(124, 32)
        Me.cmdDoIt.TabIndex = 10
        Me.cmdDoIt.Text = "&Go"
        '
        'txtFASTAPath
        '
        Me.txtFASTAPath.Location = New System.Drawing.Point(160, 209)
        Me.txtFASTAPath.Name = "txtFASTAPath"
        Me.txtFASTAPath.Size = New System.Drawing.Size(396, 20)
        Me.txtFASTAPath.TabIndex = 11
        Me.txtFASTAPath.Text = "C:\DMS_Temp_Org\bsa.fasta"
        '
        'txtDatasetID
        '
        Me.txtDatasetID.Location = New System.Drawing.Point(436, 279)
        Me.txtDatasetID.Name = "txtDatasetID"
        Me.txtDatasetID.Size = New System.Drawing.Size(120, 20)
        Me.txtDatasetID.TabIndex = 12
        Me.txtDatasetID.Text = "101986"
        Me.txtDatasetID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblDatasetID
        '
        Me.lblDatasetID.AutoSize = True
        Me.lblDatasetID.Location = New System.Drawing.Point(436, 260)
        Me.lblDatasetID.Name = "lblDatasetID"
        Me.lblDatasetID.Size = New System.Drawing.Size(106, 13)
        Me.lblDatasetID.TabIndex = 13
        Me.lblDatasetID.Text = "(Optional) Dataset ID"
        '
        'txtParamFileName
        '
        Me.txtParamFileName.Location = New System.Drawing.Point(156, 87)
        Me.txtParamFileName.Name = "txtParamFileName"
        Me.txtParamFileName.Size = New System.Drawing.Size(400, 20)
        Me.txtParamFileName.TabIndex = 14
        '
        'lblParamFileName
        '
        Me.lblParamFileName.Location = New System.Drawing.Point(16, 87)
        Me.lblParamFileName.Name = "lblParamFileName"
        Me.lblParamFileName.Size = New System.Drawing.Size(120, 20)
        Me.lblParamFileName.TabIndex = 15
        Me.lblParamFileName.Text = "Param File Name"
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(576, 346)
        Me.Controls.Add(Me.lblParamFileName)
        Me.Controls.Add(Me.txtParamFileName)
        Me.Controls.Add(Me.lblDatasetID)
        Me.Controls.Add(Me.txtDatasetID)
        Me.Controls.Add(Me.txtFASTAPath)
        Me.Controls.Add(Me.txtResults)
        Me.Controls.Add(Me.txtDMSConnectionString)
        Me.Controls.Add(Me.txtOutputPath)
        Me.Controls.Add(Me.cmdDoIt)
        Me.Controls.Add(Me.cboAvailableParams)
        Me.Controls.Add(Me.lblParamFileType)
        Me.Controls.Add(Me.lblConnectionString)
        Me.Controls.Add(Me.lblPickList)
        Me.Controls.Add(Me.lblOutputPath)
        Me.Controls.Add(Me.cboFileTypes)
        Me.Name = "frmMain"
        Me.Text = "SEQUEST Param Generator"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Dim mOutputPath As String
    Dim mDMSConnectString As String
    Dim mParamFileType As IGenerateFile.ParamFileType
    Dim mParamTypeID As Integer
    Dim mParamFileName As String
    Dim mFASTAPath As String
    Dim mAvailableParamFiles As DataTable

    Dim mCurrentConnectionString As String
    Dim mCurrentDBTools As IDBTools


    ReadOnly mDMS As IGenerateFile

    Private Sub cmdDoIt_Click(sender As Object, e As EventArgs) Handles cmdDoIt.Click
        'If mDMS Is Nothing Then
        '    mDMS = New ParamFileGenerator.MakeParams.clsMakeParameterFile
        'End If

        If txtParamFileName.TextLength > 0 Then
            mParamFileName = txtParamFileName.Text
        Else
            PopulateParamFileNameTextbox()
        End If

        Dim datasetID As Integer
        If Not Me.txtDatasetID.Text = "" Then
            datasetID = CInt(Me.txtDatasetID.Text)
        Else
            datasetID = -1
        End If
        Dim success As Boolean = mDMS.MakeFile(mParamFileName, mParamFileType, mFASTAPath, mOutputPath, mDMSConnectString, datasetID)
        If success = True Then
            Me.txtResults.Text = "File successfully written to: " & Path.Combine(mOutputPath, mParamFileName)
        Else
            Me.txtResults.Text = "Error!"
            If mDMS.LastError IsNot Nothing AndAlso mDMS.LastError.Length > 0 Then
                Me.txtResults.Text &= " " & mDMS.LastError
            End If
        End If
    End Sub

    Private Sub txtOutputPath_TextChanged(sender As Object, e As EventArgs) Handles txtOutputPath.TextChanged
        mOutputPath = Me.txtOutputPath.Text
    End Sub

    Private Sub cboAvailableParams_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboAvailableParams.SelectedIndexChanged
        PopulateParamFileNameTextbox()
    End Sub

    Private Sub txtDMSConnectionString_TextChanged(sender As Object, e As EventArgs) Handles txtDMSConnectionString.TextChanged
        mDMSConnectString = Me.txtDMSConnectionString.Text
    End Sub

    Private Sub cboFileTypes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFileTypes.SelectedIndexChanged
        mParamTypeID = CInt(Me.cboFileTypes.SelectedValue)

        Select Case mParamTypeID
            Case 1000
                mParamFileType = IGenerateFile.ParamFileType.BioWorks_32
            Case 1008
                mParamFileType = IGenerateFile.ParamFileType.X_Tandem
            Case 1018
                mParamFileType = IGenerateFile.ParamFileType.MSGFPlus
            Case 1019
                mParamFileType = IGenerateFile.ParamFileType.MSAlign
            Case 1022
                mParamFileType = IGenerateFile.ParamFileType.MSAlignHistone
            Case 1025
                mParamFileType = IGenerateFile.ParamFileType.MSPathFinder
            Case 1032
                mParamFileType = IGenerateFile.ParamFileType.TopPIC
            Case 1033
                mParamFileType = IGenerateFile.ParamFileType.MSFragger
            Case 1034
                mParamFileType = IGenerateFile.ParamFileType.MaxQuant

        End Select

        Me.LoadParamNames(mParamTypeID)

    End Sub

    Private Sub ValidateDBTools()
        If mCurrentDBTools Is Nothing OrElse
           String.IsNullOrWhiteSpace(mCurrentConnectionString) OrElse
           Not mCurrentConnectionString.Equals(mDMSConnectString) Then

            mCurrentConnectionString = String.Copy(mDMSConnectString)

            Dim connectionStringToUse = DbToolsFactory.AddApplicationNameToConnectionString(mCurrentConnectionString, "ParameterFileEditor_ParamGenTest")

            mCurrentDBTools = DbToolsFactory.GetDBTools(connectionStringToUse)
        End If
    End Sub

    Private Sub LoadParamNames(Optional ByVal TypeID As Integer = 0)
    Private Sub LoadParamNames(Optional TypeID As Integer = 0)
        If mAvailableParamFiles Is Nothing Then
            ValidateDBTools()
            mAvailableParamFiles = mDMS.GetAvailableParamSetTable(mCurrentDBTools)
        End If

        Dim foundRows() As DataRow

        If TypeID > 1 Then
            foundRows = mAvailableParamFiles.Select("Type_ID = " & TypeID.ToString)
        Else
            foundRows = mAvailableParamFiles.Select()
        End If

        cboAvailableParams.BeginUpdate()
        cboAvailableParams.Items.Clear()
        txtParamFileName.Text = String.Empty

        For Each dr As DataRow In foundRows
            cboAvailableParams.Items.Add(New ParamFileEntry(
                CInt(dr.Item("ID")), dr.Item("Filename").ToString))
        Next

        cboAvailableParams.DisplayMember = "Description"
        cboAvailableParams.ValueMember = "Value"

        If cboAvailableParams.Items.Count > 0 Then
            cboAvailableParams.SelectedIndex = 0
        End If

        cboAvailableParams.EndUpdate()

    End Sub

    Private Sub LoadParamFileTypes()
        ValidateDBTools()

        Dim paramFileTypes As DataTable
        paramFileTypes = m_DMS.GetAvailableParamFileTypes(m_CurrentDBTools)

        With Me.cboFileTypes
            .DisplayMember = "Type"
            .ValueMember = "ID"
            .DataSource = paramFileTypes
            .Text = "SEQUEST"
        End With

        'Me.cboFileTypes.DataSource = System.Enum.GetValues(GetType(ParamFileGenerator.MakeParams.IGenerateFile.ParamFileType))

    End Sub

    Private Sub PopulateParamFileNameTextBox()
        Dim entry As ParamFileEntry
        entry = DirectCast(Me.cboAvailableParams.SelectedItem, ParamFileEntry)

        txtParamFileName.Text = entry.Description
        mParamFileName = txtParamFileName.Text
    End Sub

    Private Sub txtFASTAPath_TextChanged(sender As Object, e As EventArgs) Handles txtFASTAPath.TextChanged
        mFASTAPath = Me.txtFASTAPath.Text
    End Sub


    Class ParamFileEntry
        Sub New(Value As Integer, Description As String)
            Me.Value = Value
            Me.Description = Description
        End Sub

        Property Value As Integer

        Property Description As String

    End Class


End Class
