Imports ParamFileGenerator
Imports ParamFileEditor.ProgramSettings

Public Class frmDMSPicker
    Inherits System.Windows.Forms.Form

    Private m_MySettings As clsSettings
    Private m_SelectedIndex As Integer
    Private m_frmMainGUI As frmMainGUI

#Region " Windows Form Designer generated code "

    Public Sub New(ByRef Callingfrm As frmMainGUI)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        m_frmMainGUI = Callingfrm
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
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.lvwDMSPicklist = New System.Windows.Forms.ListView
        Me.colParamID = New System.Windows.Forms.ColumnHeader
        Me.colFileName = New System.Windows.Forms.ColumnHeader
        Me.colDiffs = New System.Windows.Forms.ColumnHeader
        Me.cmdLoadParam = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lvwDMSPicklist
        '
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
        Me.cmdLoadParam.Location = New System.Drawing.Point(528, 440)
        Me.cmdLoadParam.Name = "cmdLoadParam"
        Me.cmdLoadParam.Size = New System.Drawing.Size(148, 23)
        Me.cmdLoadParam.TabIndex = 1
        Me.cmdLoadParam.Text = "Load Selected Param Set"
        '
        'frmDMSPicker
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(692, 478)
        Me.Controls.Add(Me.cmdLoadParam)
        Me.Controls.Add(Me.lvwDMSPicklist)
        Me.Name = "frmDMSPicker"
        Me.Text = "DMS Parameter Set Picker"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub frmDMSPicker_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Loader As New ParamFileEditor.clsDMSPickerHandler
        Loader.ProgramSettings = Me.MySettings
        Loader.FillListView(Me.lvwDMSPicklist)
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

End Class
