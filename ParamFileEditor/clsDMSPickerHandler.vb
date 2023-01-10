Imports ParamFileGenerator.DownloadParams
Imports ParamFileEditor.ProgramSettings
Imports PRISMDatabaseUtils

Public Class clsDMSPickerHandler
    Private m_tmpIDTable As DataTable
    Private m_forceReload As Boolean = False

    Private Sub SetupPickerListView(dbTools As IDBTools, lvw As ListView, Optional filterCriteria As String = "")
        'Load up available Param sets from DMS
        Dim c As New ParamsFromDMS(dbTools)
        'Dim availableParams As DataTable = c.RetrieveAvailableParams
        Dim paramRow As DataRow
        Dim paramRows() As DataRow
        Dim filterString As String

        If m_tmpIDTable Is Nothing Or m_forceReload Then
            m_tmpIDTable = Nothing
            m_tmpIDTable = c.RetrieveAvailableParams
        End If

        filterString = "Type_ID = 1000"

        filterCriteria = Trim(filterCriteria)
        If Len(filterCriteria) <> 0 Then
            filterString &= " AND ([Filename] LIKE '%" & filterCriteria & "%' OR [Diffs] LIKE '%" & filterCriteria & "%')"
        End If

        paramRows = m_tmpIDTable.Select(filterString)

        'Dim index As Integer
        'Dim maxIndex As Integer = availableParams.Rows.Count - 1

        'Fill ListView
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
            item.Text = CStr(paramRow.Item("ID"))
            item.SubItems.Add(CStr(paramRow.Item("Filename")))
            item.SubItems.Add(CStr(paramRow.Item("Diffs")))
            lvw.Items.Add(item)
        Next

        lvw.EndUpdate()
    End Sub

#Disable Warning BC40028 ' Type of parameter is not CLS-compliant
    Public Sub FillListView(dbTools As IDBTools, ListViewToFill As ListView)
        ListViewToFill.Items.Clear()
        SetupPickerListView(dbTools, ListViewToFill)
    End Sub

    Public Sub FillFilteredListView(dbTools As IDBTools, ListViewToFill As ListView, FilterString As String)
        ListViewToFill.Items.Clear()
        SetupPickerListView(dbTools, ListViewToFill, FilterString)
    End Sub
#Enable Warning BC40028 ' Type of parameter is not CLS-compliant

    Public Property ProgramSettings As clsSettings

    Public WriteOnly Property ForceIDTableReload As Boolean
        Set
            m_forceReload = Value
        End Set
    End Property
End Class
