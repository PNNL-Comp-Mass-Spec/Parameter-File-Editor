Public Class frmMain
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Me.m_DMSConnectString = Me.txtDMSConnectionString.Text
        Me.m_OutputPath = Me.txtOutputPath.Text
        Me.m_FASTAPath = Me.txtFASTAPath.Text
        If m_DMS Is Nothing Then
            Me.m_DMS = New ParamFileGenerator.MakeParams.clsMakeParameterFile
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
        Me.Text = "Sequest Param Generator"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Dim m_OutputPath As String
    Dim m_DMSConnectString As String
    Dim m_ParamFileType As ParamFileGenerator.MakeParams.IGenerateFile.ParamFileType
    Dim m_ParamTypeID As Integer
    Dim m_ParamFileName As String
    Dim m_FASTAPath As String
    Dim m_AvailableParamFiles As DataTable

    Dim m_DMS As ParamFileGenerator.MakeParams.IGenerateFile

    Private Sub cmdDoIt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDoIt.Click
        'If m_DMS Is Nothing Then
        '    Me.m_DMS = New ParamFileGenerator.MakeParams.clsMakeParameterFile
        'End If

        If txtParamFileName.TextLength > 0 Then
            Me.m_ParamFileName = txtParamFileName.Text
        Else
            PopulateParamFileNameTextbox()
        End If

        Dim datasetID As Integer = 0
        If Not Me.txtDatasetID.Text = "" Then
            datasetID = CInt(Me.txtDatasetID.Text)
        Else
            datasetID = -1
        End If
        Dim success As Boolean = Me.m_DMS.MakeFile(m_ParamFileName, m_ParamFileType, m_FASTAPath, m_OutputPath, m_DMSConnectString, datasetID)
        If success = True Then
            Me.txtResults.Text = "File successfully written to: " & System.IO.Path.Combine(m_OutputPath, m_ParamFileName)
        Else
            Me.txtResults.Text = "Error!"
            If Not Me.m_DMS.LastError Is Nothing AndAlso Me.m_DMS.LastError.Length > 0 Then
                Me.txtResults.Text &= " " & Me.m_DMS.LastError
            End If
        End If
    End Sub

    Private Sub txtOutputPath_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOutputPath.TextChanged
        Me.m_OutputPath = Me.txtOutputPath.Text
    End Sub

    Private Sub cboAvailableParams_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAvailableParams.SelectedIndexChanged
        PopulateParamFileNameTextbox()
    End Sub

    Private Sub txtDMSConnectionString_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDMSConnectionString.TextChanged
        Me.m_DMSConnectString = Me.txtDMSConnectionString.Text
    End Sub

    Private Sub cboFileTypes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFileTypes.SelectedIndexChanged
        Me.m_ParamTypeID = CInt(Me.cboFileTypes.SelectedValue)

        Select Case Me.m_ParamTypeID
            Case 1000
                Me.m_ParamFileType = ParamFileGenerator.MakeParams.IGenerateFile.ParamFileType.BioWorks_32
            Case 1008
                Me.m_ParamFileType = ParamFileGenerator.MakeParams.IGenerateFile.ParamFileType.X_Tandem
            Case 1018
                Me.m_ParamFileType = ParamFileGenerator.MakeParams.IGenerateFile.ParamFileType.MSGFPlus
            Case 1019
                Me.m_ParamFileType = ParamFileGenerator.MakeParams.IGenerateFile.ParamFileType.MSAlign
            Case 1022
                Me.m_ParamFileType = ParamFileGenerator.MakeParams.IGenerateFile.paramFileType.MSAlignHistone
            Case 1025
                Me.m_ParamFileType = ParamFileGenerator.MakeParams.IGenerateFile.paramFileType.MSPathFinder
            Case 1032
                Me.m_ParamFileType = ParamFileGenerator.MakeParams.IGenerateFile.paramFileType.TopPIC
            Case Else

        End Select
        'Me.m_ParamFileType = _
        '    CType([Enum].Parse(GetType(ParamFileGenerator.MakeParams.IGenerateFile.ParamFileType), _
        '    Me.cboFileTypes.Text), _
        '    ParamFileGenerator.MakeParams.IGenerateFile.ParamFileType)
        Me.LoadParamNames(Me.m_ParamTypeID)

    End Sub

    Private Sub LoadParamNames(Optional ByVal TypeID As Integer = 0)
        If Me.m_AvailableParamFiles Is Nothing Then
            Me.m_AvailableParamFiles = Me.m_DMS.GetAvailableParamSetTable(Me.m_DMSConnectString)
        End If

        Dim dr As DataRow
        Dim foundrows() As DataRow

        If TypeID > 1 Then
            foundrows = Me.m_AvailableParamFiles.Select("Type_ID = " & TypeID.ToString)
        Else
            foundrows = Me.m_AvailableParamFiles.Select()
        End If

        Me.cboAvailableParams.BeginUpdate()
        Me.cboAvailableParams.Items.Clear()
        txtParamFileName.Text = String.Empty

        For Each dr In foundrows
            Me.cboAvailableParams.Items.Add(New ParamFileEntry(
                CInt(dr.Item("ID")), dr.Item("Filename").ToString))
        Next

        Me.cboAvailableParams.DisplayMember = "Description"
        Me.cboAvailableParams.ValueMember = "Value"

        If Me.cboAvailableParams.Items.Count > 0 Then
            Me.cboAvailableParams.SelectedIndex = 0
        End If

        Me.cboAvailableParams.EndUpdate()

    End Sub

    Private Sub LoadParamFileTypes()

        Dim paramFileTypes As DataTable
        paramFileTypes = Me.m_DMS.GetAvailableParamFileTypes(Me.m_DMSConnectString)

        With Me.cboFileTypes
            .DisplayMember = "Type"
            .ValueMember = "ID"
            .DataSource = paramFileTypes
            .Text = "Sequest"
        End With



        'Me.cboFileTypes.DataSource = System.Enum.GetValues(GetType(ParamFileGenerator.MakeParams.IGenerateFile.ParamFileType))

    End Sub

    Private Sub PopulateParamFileNameTextbox()
        Dim entry As ParamFileEntry
        entry = DirectCast(Me.cboAvailableParams.SelectedItem, ParamFileEntry)

        txtParamFileName.Text = entry.Description
        Me.m_ParamFileName = txtParamFileName.Text
    End Sub

    Private Sub txtFASTAPath_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFASTAPath.TextChanged
        Me.m_FASTAPath = Me.txtFASTAPath.Text
    End Sub


    Class ParamFileEntry
        Private m_Value As Integer
        Private m_Description As String

        Sub New(ByVal Value As Integer, ByVal Description As String)
            Me.m_Value = Value
            Me.m_Description = Description
        End Sub

        Property Value() As Integer
            Get
                Return Me.m_Value
            End Get
            Set(ByVal Value As Integer)
                Me.m_Value = Value
            End Set
        End Property

        Property Description() As String
            Get
                Return Me.m_Description
            End Get
            Set(ByVal Value As String)
                me.m_Description = value    
            End Set
        End Property

    End Class


End Class
