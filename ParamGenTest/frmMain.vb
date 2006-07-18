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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtOutputPath As System.Windows.Forms.TextBox
    Friend WithEvents txtDMSConnectionString As System.Windows.Forms.TextBox
    Friend WithEvents cboFileTypes As System.Windows.Forms.ComboBox
    Friend WithEvents cboAvailableParams As System.Windows.Forms.ComboBox
    Friend WithEvents txtResults As System.Windows.Forms.TextBox
    Friend WithEvents cmdDoIt As System.Windows.Forms.Button
    Friend WithEvents txtFASTAPath As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.txtOutputPath = New System.Windows.Forms.TextBox
        Me.txtDMSConnectionString = New System.Windows.Forms.TextBox
        Me.cboFileTypes = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.cboAvailableParams = New System.Windows.Forms.ComboBox
        Me.txtResults = New System.Windows.Forms.TextBox
        Me.cmdDoIt = New System.Windows.Forms.Button
        Me.txtFASTAPath = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'txtOutputPath
        '
        Me.txtOutputPath.Location = New System.Drawing.Point(156, 20)
        Me.txtOutputPath.Name = "txtOutputPath"
        Me.txtOutputPath.Size = New System.Drawing.Size(400, 20)
        Me.txtOutputPath.TabIndex = 0
        Me.txtOutputPath.Text = "D:\Sequest\"
        '
        'txtDMSConnectionString
        '
        Me.txtDMSConnectionString.Location = New System.Drawing.Point(156, 100)
        Me.txtDMSConnectionString.Name = "txtDMSConnectionString"
        Me.txtDMSConnectionString.Size = New System.Drawing.Size(400, 20)
        Me.txtDMSConnectionString.TabIndex = 2
        Me.txtDMSConnectionString.Text = "Data Source=gigasax;Initial Catalog=DMS5;Integrated Security=SSPI;"
        '
        'cboFileTypes
        '
        Me.cboFileTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFileTypes.Location = New System.Drawing.Point(156, 140)
        Me.cboFileTypes.Name = "cboFileTypes"
        Me.cboFileTypes.Size = New System.Drawing.Size(400, 21)
        Me.cboFileTypes.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(16, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 23)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Output Path"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(16, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 23)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Param File Pick List"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(16, 100)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(120, 23)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Connect String"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(16, 144)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(120, 23)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Param File Type"
        '
        'cboAvailableParams
        '
        Me.cboAvailableParams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAvailableParams.Location = New System.Drawing.Point(156, 60)
        Me.cboAvailableParams.Name = "cboAvailableParams"
        Me.cboAvailableParams.Size = New System.Drawing.Size(400, 21)
        Me.cboAvailableParams.TabIndex = 8
        '
        'txtResults
        '
        Me.txtResults.Location = New System.Drawing.Point(24, 228)
        Me.txtResults.Multiline = True
        Me.txtResults.Name = "txtResults"
        Me.txtResults.Size = New System.Drawing.Size(384, 84)
        Me.txtResults.TabIndex = 9
        Me.txtResults.Text = ""
        '
        'cmdDoIt
        '
        Me.cmdDoIt.Location = New System.Drawing.Point(436, 280)
        Me.cmdDoIt.Name = "cmdDoIt"
        Me.cmdDoIt.Size = New System.Drawing.Size(124, 32)
        Me.cmdDoIt.TabIndex = 10
        Me.cmdDoIt.Text = "Go"
        '
        'txtFASTAPath
        '
        Me.txtFASTAPath.Location = New System.Drawing.Point(160, 184)
        Me.txtFASTAPath.Name = "txtFASTAPath"
        Me.txtFASTAPath.Size = New System.Drawing.Size(396, 20)
        Me.txtFASTAPath.TabIndex = 11
        Me.txtFASTAPath.Text = "D:\Org_DB\Bovine\FASTA\bsa.fasta"
        '
        'frmMain
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(576, 346)
        Me.Controls.Add(Me.txtFASTAPath)
        Me.Controls.Add(Me.cmdDoIt)
        Me.Controls.Add(Me.txtResults)
        Me.Controls.Add(Me.cboAvailableParams)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cboFileTypes)
        Me.Controls.Add(Me.txtDMSConnectionString)
        Me.Controls.Add(Me.txtOutputPath)
        Me.Name = "frmMain"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Dim m_OutputPath As String
    Dim m_DMSConnectString As String
    Dim m_ParamFileType As ParamFileGenerator.MakeParams.IGenerateFile.ParamFileType
    Dim m_ParamTypeID As Integer
    Dim m_ParamFileName As String
    Dim m_FASTAPath As String
    Dim m_AvailableParamFiles As DataTable

    'Dim m_DMS As New ParamFileGenerator.MakeParams.clsMakeParameterFile
    Dim m_DMS As ParamFileGenerator.MakeParams.IGenerateFile

    Private Sub cmdDoIt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDoIt.Click
        'If m_DMS Is Nothing Then
        '    Me.m_DMS = New ParamFileGenerator.MakeParams.clsMakeParameterFile
        'End If
        Dim success As Boolean = Me.m_DMS.MakeFile(m_ParamFileName, m_ParamFileType, m_FASTAPath, m_OutputPath, m_DMSConnectString)
        If success = True Then
            Me.txtResults.Text = "File successfully written to: " & m_OutputPath & m_ParamFileName
        Else
            Me.txtResults.Text = "Error!"
        End If
    End Sub

    Private Sub txtOutputPath_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtOutputPath.TextChanged
        Me.m_OutputPath = Me.txtOutputPath.Text
    End Sub

    Private Sub cboAvailableParams_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAvailableParams.SelectedIndexChanged
        Dim entry As ParamFileEntry
        entry = DirectCast(Me.cboAvailableParams.SelectedItem, ParamFileEntry)

        Me.m_ParamFileName = entry.Description
    End Sub

    Private Sub txtDMSConnectionString_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDMSConnectionString.TextChanged
        Me.m_DMSConnectString = Me.txtDMSConnectionString.Text
    End Sub

    Private Sub cboFileTypes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFileTypes.SelectedIndexChanged
        Me.m_ParamTypeID = CInt(Me.cboFileTypes.SelectedValue)

        Select Case Me.m_ParamTypeID
            Case 1000
                Me.m_ParamFileType = ParamFileGenerator.MakeParams.IGenerateFile.ParamFileType.BioWorks_31
            Case 1008
                Me.m_ParamFileType = ParamFileGenerator.MakeParams.IGenerateFile.ParamFileType.X_Tandem
            Case Else

        End Select
        'Me.m_ParamFileType = _
        '    CType([Enum].Parse(GetType(ParamFileGenerator.MakeParams.IGenerateFile.ParamFileType), _
        '    Me.cboFileTypes.Text), _
        '    ParamFileGenerator.MakeParams.IGenerateFile.ParamFileType)
        Me.LoadParamNames(Me.m_ParamTypeID)

    End Sub

    Private Sub LoadParamNames(Optional ByVal TypeID As Integer = 0)
        'Dim l_ParamColl As System.Collections.Specialized.StringCollection = m_DMS.GetAvailableParamSetNames(m_DMSConnectString)
        'Dim l_ParamTable As DataTable = Me.m_DMS.GetAvailableParamSetTable(Me.m_DMSConnectString)
        If Me.m_AvailableParamFiles Is Nothing Then
            Me.m_AvailableParamFiles = Me.m_DMS.GetAvailableParamSetTable(Me.m_DMSConnectString)
        End If
        Dim Name As String

        Dim dr As DataRow
        Dim foundrows() As DataRow

        If TypeID > 1 Then
            foundrows = Me.m_AvailableParamFiles.Select("Type_ID = " & TypeID.ToString)
        Else
            foundrows = Me.m_AvailableParamFiles.Select()
        End If

        Me.cboAvailableParams.BeginUpdate()
        Me.cboAvailableParams.Items.Clear()

        For Each dr In foundrows
            Me.cboAvailableParams.Items.Add(New ParamFileEntry( _
                CInt(dr.Item("ID")), dr.Item("Filename").ToString))
        Next
        Me.cboAvailableParams.DisplayMember = "Description"
        Me.cboAvailableParams.ValueMember = "Value"
        Me.cboAvailableParams.SelectedIndex = 0

        Me.cboAvailableParams.EndUpdate()

        'With Me.cboAvailableParams
        '    .DisplayMember = l_ParamTable.Columns(1).ColumnName
        '    .ValueMember = l_ParamTable.Columns(0).ColumnName
        '    .DataSource = l_ParamTable.Select(filterstring)
        'End With

        'For Each Name In l_ParamColl
        '    Me.cboAvailableParams.Items.Add(Name)
        'Next

        'Me.cboAvailableParams.SelectedIndex = 0
    End Sub

    Private Sub LoadParamFileTypes()

        Dim paramFileTypes As DataTable
        paramFileTypes = Me.m_DMS.GetAvailableParamFileTypes(Me.m_DMSConnectString)

        With Me.cboFileTypes
            .DisplayMember = "Type"
            .ValueMember = "ID"
            .DataSource = paramFileTypes
        End With

        'Me.cboFileTypes.DataSource = System.Enum.GetValues(GetType(ParamFileGenerator.MakeParams.IGenerateFile.ParamFileType))

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
