Imports ParamFileGenerator
Imports ParamFileEditor.ProgramSettings

Public Class frmDMSPicker
    Inherits System.Windows.Forms.Form

    Private m_MySettings As clsSettings
    Private m_SelectedIndex As Integer
    Private m_frmMainGUI As frmMainGUI
    Private m_SortOrderAsc As Boolean = True
    Private m_SelectedCol As Integer = 0
    Private m_SearchActive As Boolean = False
    Private m_Loader As ParamFileEditor.clsDMSPickerHandler
    Friend WithEvents SearchTimer As New System.Timers.Timer(2000)

#Region " Windows Form Designer generated code "

    Public Sub New(ByRef Callingfrm As frmMainGUI)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        m_frmMainGUI = Callingfrm
        SearchTimer.AutoReset = False
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
    Friend WithEvents colFileName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDiffs As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwDMSPicklist As System.Windows.Forms.ListView
    Friend WithEvents colParamID As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmdLoadParam As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents txtLiveSearch As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmDMSPicker))
        Me.lvwDMSPicklist = New System.Windows.Forms.ListView
        Me.colParamID = New System.Windows.Forms.ColumnHeader
        Me.colFileName = New System.Windows.Forms.ColumnHeader
        Me.colDiffs = New System.Windows.Forms.ColumnHeader
        Me.cmdLoadParam = New System.Windows.Forms.Button
        Me.txtLiveSearch = New System.Windows.Forms.TextBox
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.SuspendLayout()
        '
        'lvwDMSPicklist
        '
        Me.lvwDMSPicklist.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwDMSPicklist.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colParamID, Me.colFileName, Me.colDiffs})
        Me.lvwDMSPicklist.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwDMSPicklist.FullRowSelect = True
        Me.lvwDMSPicklist.GridLines = True
        Me.lvwDMSPicklist.Location = New System.Drawing.Point(16, 16)
        Me.lvwDMSPicklist.MultiSelect = False
        Me.lvwDMSPicklist.Name = "lvwDMSPicklist"
        Me.lvwDMSPicklist.Size = New System.Drawing.Size(660, 412)
        Me.lvwDMSPicklist.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwDMSPicklist.TabIndex = 0
        Me.lvwDMSPicklist.View = System.Windows.Forms.View.Details
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
        Me.cmdLoadParam.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdLoadParam.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cmdLoadParam.Location = New System.Drawing.Point(528, 444)
        Me.cmdLoadParam.Name = "cmdLoadParam"
        Me.cmdLoadParam.Size = New System.Drawing.Size(148, 23)
        Me.cmdLoadParam.TabIndex = 1
        Me.cmdLoadParam.Text = "Load Selected Param Set"
        '
        'txtLiveSearch
        '
        Me.txtLiveSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtLiveSearch.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtLiveSearch.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLiveSearch.ForeColor = System.Drawing.SystemColors.InactiveCaption
        Me.txtLiveSearch.Location = New System.Drawing.Point(40, 450)
        Me.txtLiveSearch.Name = "txtLiveSearch"
        Me.txtLiveSearch.Size = New System.Drawing.Size(160, 14)
        Me.txtLiveSearch.TabIndex = 2
        Me.txtLiveSearch.Text = "Search"
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(16, 444)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(200, 24)
        Me.PictureBox1.TabIndex = 3
        Me.PictureBox1.TabStop = False
        '
        'frmDMSPicker
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(692, 478)
        Me.Controls.Add(Me.txtLiveSearch)
        Me.Controls.Add(Me.cmdLoadParam)
        Me.Controls.Add(Me.lvwDMSPicklist)
        Me.Controls.Add(Me.PictureBox1)
        Me.MinimumSize = New System.Drawing.Size(700, 512)
        Me.Name = "frmDMSPicker"
        Me.Text = "DMS Parameter Set Picker"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub frmDMSPicker_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_Loader = New ParamFileEditor.clsDMSPickerHandler
        m_Loader.ProgramSettings = Me.MySettings
        m_Loader.FillListView(Me.lvwDMSPicklist)
    End Sub

    Private Sub lvwDMSPicklist_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwDMSPicklist.ColumnClick

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


    Public Property MySettings() As clsSettings
        Get
            Return m_MySettings
        End Get
        Set(ByVal Value As clsSettings)
            m_MySettings = Value
        End Set
    End Property

    Private Sub cmdLoadParam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLoadParam.Click
        m_SelectedIndex = CInt(Me.lvwDMSPicklist.SelectedIndices(0))
        Dim tmpParamID As Integer = CInt(Me.lvwDMSPicklist.Items(m_SelectedIndex).Text)
        m_frmMainGUI.LoadDMSParamsFromID(tmpParamID)
        Me.Close()

    End Sub

    Private Sub txtLiveSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtLiveSearch.TextChanged
        If m_SearchActive Then
            SearchTimer.Start()
        End If
    End Sub

    Private Sub txtLiveSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLiveSearch.Click
        If m_SearchActive Then
        Else
            txtLiveSearch.Text = Nothing
            txtLiveSearch.ForeColor = System.Drawing.SystemColors.ControlText
            m_SearchActive = True
        End If
    End Sub

    Private Sub txtLiveSearch_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLiveSearch.Leave
        If txtLiveSearch.Text.Length = 0 Then
            txtLiveSearch.ForeColor = System.Drawing.SystemColors.InactiveCaption
            txtLiveSearch.Text = "Search"
            Me.m_SearchActive = False
            SearchTimer.Stop()
            Me.m_Loader.FillListView(Me.lvwDMSPicklist)
        End If
    End Sub

    Friend Sub TimerHandler(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles SearchTimer.Elapsed
        Me.m_Loader.FillFilteredListView(Me.lvwDMSPicklist, Me.txtLiveSearch.Text)
    End Sub

    Private Sub lvwDMSPicklist_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwDMSPicklist.DoubleClick
        Me.cmdLoadParam_Click(Me.cmdLoadParam, Nothing)
    End Sub
End Class
