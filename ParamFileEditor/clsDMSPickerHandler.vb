Imports ParamFileGenerator
Imports ParamFileGenerator.DownloadParams
Imports ParamFileEditor.ProgramSettings

Public Class clsDMSPickerHandler

    Private m_MySettings As clsSettings
    Private m_tmpIDTable As DataTable
    Private m_forceReload As Boolean = False

    Private Sub SetupPickerListView(ByVal lvw As ListView, Optional ByVal filterCriteria As String = "")
        'Load up available Paramsets from DMS
        Dim c As New clsParamsFromDMS(Me.ProgramSettings.DMS_ConnectionString)
        'Dim availableParams As DataTable = c.RetrieveAvailableParams
        Dim paramRow As DataRow
        Dim paramRows() As DataRow
        Dim filterString As String

        If m_tmpIDTable Is Nothing Or m_forceReload Then
            Me.m_tmpIDTable = Nothing
            Me.m_tmpIDTable = c.RetrieveAvailableParams
        End If

        filterString = "Type_ID = 1000"

        If Len(filterCriteria) <> 0 Then
            filterString &= " AND ([Filename] LIKE '%" & filterCriteria & "%' OR [Diffs] LIKE '%" & filterCriteria & "%')"
        End If

        paramRows = Me.m_tmpIDTable.Select(filterString)

        'Dim index As Integer
        'Dim maxIndex As Integer = availableParams.Rows.Count - 1

        'Fill listview
        'For index = 0 To maxIndex
        '    Dim item As New ListViewItem
        '    item.Text = availableParams.Rows(index).Item(0)
        '    item.SubItems.Add(availableParams.Rows(index).Item(1))
        '    item.SubItems.Add(availableParams.Rows(index).Item(2))
        '    lvw.Items.Add(item)
        'Next

        lvw.BeginUpdate()

        For Each paramRow In paramRows
            Dim item As New ListViewItem
            item.Text = paramRow.Item("ID")
            item.SubItems.Add(paramRow.Item("Filename"))
            item.SubItems.Add(paramRow.Item("Diffs"))
            lvw.Items.Add(item)
        Next

        lvw.EndUpdate()
    End Sub

    Public Sub FillListView(ByVal ListViewToFill As ListView)
        ListViewToFill.Items.Clear()
        SetupPickerListView(ListViewToFill)
    End Sub

    Public Sub FillFilteredListView(ByVal ListViewToFill As ListView, ByVal FilterString As String)
        ListViewToFill.Items.Clear()
        SetupPickerListView(ListViewToFill, FilterString)
    End Sub

    Public Property ProgramSettings() As clsSettings
        Get
            Return m_MySettings
        End Get
        Set(ByVal Value As clsSettings)
            m_MySettings = Value
        End Set
    End Property

    Public WriteOnly Property ForceIDTableReload() As Boolean
        Set(ByVal Value As Boolean)
            Me.m_forceReload = Value
        End Set
    End Property
End Class
