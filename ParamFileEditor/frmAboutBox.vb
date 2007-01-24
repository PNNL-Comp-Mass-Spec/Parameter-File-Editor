Imports System.Reflection

Public Class frmAboutBox
    Inherits System.Windows.Forms.Form

    Private m_ConnectionString As String

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
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel2 As System.Windows.Forms.LinkLabel
    Friend WithEvents lblVersionString As System.Windows.Forms.Label
    Friend WithEvents ttProviderAboutBox As System.Windows.Forms.ToolTip
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmAboutBox))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel
        Me.lblVersionString = New System.Windows.Forms.Label
        Me.ttProviderAboutBox = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(12, 8)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(128, 128)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Verdana", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(158, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(240, 48)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Sequest Parameter File Editor"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(191, 92)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(174, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Copyright 2004, Ken Auberry"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'LinkLabel1
        '
        Me.LinkLabel1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.LinkLabel1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.LinkLabel1.LinkColor = System.Drawing.Color.Black
        Me.LinkLabel1.Location = New System.Drawing.Point(156, 111)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(244, 23)
        Me.LinkLabel1.TabIndex = 4
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Instrumentation Development Laboratory"
        Me.LinkLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'LinkLabel2
        '
        Me.LinkLabel2.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.LinkLabel2.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.LinkLabel2.LinkColor = System.Drawing.Color.Black
        Me.LinkLabel2.Location = New System.Drawing.Point(168, 124)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(220, 16)
        Me.LinkLabel2.TabIndex = 5
        Me.LinkLabel2.TabStop = True
        Me.LinkLabel2.Text = "Pacific Northwest National Laboratory"
        '
        'lblVersionString
        '
        Me.lblVersionString.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblVersionString.Location = New System.Drawing.Point(144, 60)
        Me.lblVersionString.Name = "lblVersionString"
        Me.lblVersionString.Size = New System.Drawing.Size(268, 28)
        Me.lblVersionString.TabIndex = 6
        Me.lblVersionString.Text = "Version xxx, 15 Sept 2004"
        Me.lblVersionString.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmAboutBox
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(436, 150)
        Me.Controls.Add(Me.lblVersionString)
        Me.Controls.Add(Me.LinkLabel2)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmAboutBox"
        Me.Text = "About Sequest Parameter File Editor"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub frmAboutBox_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Left = Screen.PrimaryScreen.WorkingArea.Width / 2 - Me.Width
        Me.Top = Screen.PrimaryScreen.WorkingArea.Height / 2 - Me.Height
        Dim compileVersion As String = Application.ProductVersion.ToString
        Dim fi As New System.IO.FileInfo(Application.ExecutablePath)
        Dim compileDate As String = Format(fi.LastWriteTime, "Medium Date")
        Dim compileTime As String = Format(fi.LastWriteTime, "Medium Time")

        Me.lblVersionString.Text = "Version " & compileVersion & vbCrLf & compileDate & ", " & compileTime
        Me.ttProviderAboutBox.SetToolTip(Me.Label1, Me.m_ConnectionString)
    End Sub

    Property ConnectionStringInUse() As String
        Get
            Return Me.m_ConnectionString
        End Get
        Set(ByVal Value As String)
            Me.m_ConnectionString = Value
        End Set
    End Property

End Class
