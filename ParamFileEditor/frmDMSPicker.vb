Imports ParamFileEditor.ProgramSettings
Imports PRISMDatabaseUtils

Public Class frmDMSPicker
    Inherits Form

    Private m_SelectedIndex As Integer

    Private ReadOnly m_frmMainGUI As frmMainGUI
    Private ReadOnly m_DBTools As IDBTools

    Private m_SortOrderAsc As Boolean = True
    Private m_SelectedCol As Integer = 0
    'Private m_SearchActive As Boolean = False

    Private m_Loader As clsDMSPickerHandler
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents cmdSearch As Button
    'Friend WithEvents SearchTimer As New System.Timers.Timer(2000)

#Region "Windows Form Designer generated code"

#Disable Warning BC40028 ' Type of parameter is not CLS-compliant
    Public Sub New(ByRef Callingfrm As frmMainGUI, dbTools As IDBTools)
        MyBase.New()
#Enable Warning BC40028 ' Type of parameter is not CLS-compliant

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        m_frmMainGUI = Callingfrm
        m_DBTools = dbTools

        'SearchTimer.AutoReset = False
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
    Friend WithEvents colFileName As ColumnHeader
    Friend WithEvents colDiffs As ColumnHeader
    Friend WithEvents lvwDMSPicklist As ListView
    Friend WithEvents colParamID As ColumnHeader
    Friend WithEvents cmdLoadParam As Button
    Friend WithEvents txtLiveSearch As TextBox

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDMSPicker))
        Me.lvwDMSPicklist = New ListView()
        Me.colParamID = CType(New ColumnHeader(), ColumnHeader)
        Me.colFileName = CType(New ColumnHeader(), ColumnHeader)
        Me.colDiffs = CType(New ColumnHeader(), ColumnHeader)
        Me.cmdLoadParam = New Button()
        Me.txtLiveSearch = New TextBox()
        Me.cmdSearch = New Button()
        Me.PictureBox1 = New PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lvwDMSPicklist
        '
        Me.lvwDMSPicklist.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) Or AnchorStyles.Left) Or AnchorStyles.Right), AnchorStyles)
        Me.lvwDMSPicklist.Columns.AddRange(New ColumnHeader() {Me.colParamID, Me.colFileName, Me.colDiffs})
        Me.lvwDMSPicklist.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwDMSPicklist.FullRowSelect = True
        Me.lvwDMSPicklist.GridLines = True
        Me.lvwDMSPicklist.Location = New System.Drawing.Point(19, 18)
        Me.lvwDMSPicklist.MultiSelect = False
        Me.lvwDMSPicklist.Name = "lvwDMSPicklist"
        Me.lvwDMSPicklist.Size = New System.Drawing.Size(784, 470)
        Me.lvwDMSPicklist.Sorting = SortOrder.Ascending
        Me.lvwDMSPicklist.TabIndex = 0
        Me.lvwDMSPicklist.UseCompatibleStateImageBehavior = False
        Me.lvwDMSPicklist.View = View.Details
        '
        'colParamID
        '
        Me.colParamID.Text = "ID"
        Me.colParamID.Width = 42
        '
        'colFileName
        '
        Me.colFileName.Text = "Parameter File Name"
        Me.colFileName.Width = 154
        '
        'colDiffs
        '
        Me.colDiffs.Text = "Differences From Template File"
        Me.colDiffs.Width = 460
        '
        'cmdLoadParam
        '
        Me.cmdLoadParam.Anchor = CType((AnchorStyles.Bottom Or AnchorStyles.Right), AnchorStyles)
        Me.cmdLoadParam.FlatStyle = FlatStyle.System
        Me.cmdLoadParam.Location = New System.Drawing.Point(626, 506)
        Me.cmdLoadParam.Name = "cmdLoadParam"
        Me.cmdLoadParam.Size = New System.Drawing.Size(177, 27)
        Me.cmdLoadParam.TabIndex = 3
        Me.cmdLoadParam.Text = "&Load Selected Param Set"
        '
        'txtLiveSearch
        '
        Me.txtLiveSearch.Anchor = CType((AnchorStyles.Bottom Or AnchorStyles.Left), AnchorStyles)
        Me.txtLiveSearch.BorderStyle = BorderStyle.None
        Me.txtLiveSearch.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLiveSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.txtLiveSearch.Location = New System.Drawing.Point(48, 510)
        Me.txtLiveSearch.Name = "txtLiveSearch"
        Me.txtLiveSearch.Size = New System.Drawing.Size(425, 17)
        Me.txtLiveSearch.TabIndex = 1
        '
        'cmdSearch
        '
        Me.cmdSearch.Anchor = CType((AnchorStyles.Bottom Or AnchorStyles.Right), AnchorStyles)
        Me.cmdSearch.FlatStyle = FlatStyle.System
        Me.cmdSearch.Location = New System.Drawing.Point(509, 506)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(95, 27)
        Me.cmdSearch.TabIndex = 2
        Me.cmdSearch.Text = "&Search"
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((AnchorStyles.Bottom Or AnchorStyles.Left), AnchorStyles)
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(19, 506)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(34, 28)
        Me.PictureBox1.TabIndex = 3
        Me.PictureBox1.TabStop = False
        '
        'frmDMSPicker
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(822, 546)
        Me.Controls.Add(Me.cmdSearch)
        Me.Controls.Add(Me.txtLiveSearch)
        Me.Controls.Add(Me.cmdLoadParam)
        Me.Controls.Add(Me.lvwDMSPicklist)
        Me.Controls.Add(Me.PictureBox1)
        Me.MinimumSize = New System.Drawing.Size(840, 591)
        Me.Name = "frmDMSPicker"
        Me.Text = "DMS Parameter Set Picker"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

#End Region

    Private Sub SearchNow()
        Me.m_Loader.FillFilteredListView(m_DBTools, Me.lvwDMSPicklist, txtLiveSearch.Text)
    End Sub

    Private Sub frmDMSPicker_Load(sender As System.Object, e As EventArgs) Handles MyBase.Load
        m_Loader = New clsDMSPickerHandler With {
            .ProgramSettings = Me.MySettings
        }
        m_Loader.FillListView(m_DBTools, Me.lvwDMSPicklist)
    End Sub

    Private Sub lvwDMSPickList_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles lvwDMSPicklist.ColumnClick

        'If selected column is same as previously selected column, then reverse sort order. Otherwise,
        '	sort newly selected column in ascending order

        'Set up ascending/descending criteria
        If e.Column = m_SelectedCol Then
            m_SortOrderAsc = Not m_SortOrderAsc
        Else
            m_SortOrderAsc = True
            m_SelectedCol = e.Column
        End If

        'Perform sort
        Me.lvwDMSPicklist.ListViewItemSorter = New ListViewItemComparer(e.Column, m_SortOrderAsc)
    End Sub

    Public Property MySettings As clsSettings

    Private Sub cmdLoadParam_Click(sender As System.Object, e As EventArgs) Handles cmdLoadParam.Click
        If Me.lvwDMSPicklist.SelectedIndices.Count = 0 Then
            MessageBox.Show("Please select a parameter file to load", "Nothing selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            m_SelectedIndex = CInt(Me.lvwDMSPicklist.SelectedIndices(0))
            Dim tmpParamID = CInt(Me.lvwDMSPicklist.Items(m_SelectedIndex).Text)
            m_frmMainGUI.LoadDMSParamsFromID(tmpParamID)
            Me.Close()
        End If
    End Sub

    Private Sub txtLiveSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtLiveSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            SearchNow()
        End If
    End Sub

    ''Private Sub txtLiveSearch_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtLiveSearch.TextChanged
    ''    If txtLiveSearch.ForeColor = System.Drawing.SystemColors.InactiveCaption AndAlso _
    ''       txtLiveSearch.Text <> "Search" Then
    ''        ' This code is needed to handle the user right clicking and pasting text to search
    ''        txtLiveSearch.ForeColor = System.Drawing.SystemColors.ControlText
    ''        'm_SearchActive = True
    ''        'SearchTimer.Start()
    ''    End If

    ''    'If m_SearchActive Then
    ''    ' SearchTimer.Start()
    ''    'End If
    ''End Sub

    ''Private Sub txtLiveSearch_Click(sender As Object, e As System.EventArgs) Handles txtLiveSearch.Click
    ''    ' Note: This _Click event is fired on left click but not on right click
    ''    ''If m_SearchActive Then
    ''    ''    ' Do nothing
    ''    ''Else
    ''    ''    txtLiveSearch.Text = Nothing
    ''    ''    txtLiveSearch.ForeColor = System.Drawing.SystemColors.ControlText
    ''    ''    m_SearchActive = True
    ''    ''End If

    ''    If txtLiveSearch.ForeColor = System.Drawing.SystemColors.InactiveCaption AndAlso _
    ''       txtLiveSearch.Text = "Search" Then
    ''        txtLiveSearch.Text = ""
    ''        txtLiveSearch.ForeColor = System.Drawing.SystemColors.ControlText
    ''    End If
    ''End Sub

    Private Sub txtLiveSearch_Leave(sender As Object, e As EventArgs) Handles txtLiveSearch.Leave
        If txtLiveSearch.Text.Length = 0 Then
            'Me.m_SearchActive = False
            'SearchTimer.Stop()
            'txtLiveSearch.ForeColor = System.Drawing.SystemColors.InactiveCaption
            'txtLiveSearch.Text = "Search"
            Me.m_Loader.FillListView(m_DBTools, Me.lvwDMSPicklist)
        End If
    End Sub

    'Friend Sub TimerHandler(sender As Object, e As System.Timers.ElapsedEventArgs) Handles SearchTimer.Elapsed
    '    SearchNow
    'End Sub

    Private Sub lvwDMSPickList_DoubleClick(sender As Object, e As EventArgs) Handles lvwDMSPicklist.DoubleClick
        Me.cmdLoadParam_Click(Me.cmdLoadParam, Nothing)
    End Sub

    Private Sub cmdSearch_Click(sender As System.Object, e As EventArgs) Handles cmdSearch.Click
        SearchNow()
    End Sub
End Class
