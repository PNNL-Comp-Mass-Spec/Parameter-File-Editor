Imports ParamFileEditor.DownloadParams
Imports ParamFileEditor.ProgramSettings

Public Class clsDMSPickerHandler

    Private m_MySettings As clsSettings

    Private Sub SetupPickerListView(ByVal lvw As ListView)
        'Load up available Paramsets from DMS
        Dim c As New clsParamsFromDMS(Me.ProgramSettings.DMS_ConnectionString)
        Dim availableParams As DataTable = c.RetrieveAvailableParams
        Dim index As Integer
        Dim maxIndex As Integer = availableParams.Rows.Count - 1

        'Fill listview
        For index = 0 To maxIndex
            Dim item As New ListViewItem
            item.Text = availableParams.Rows(index).Item(0)
            item.SubItems.Add(availableParams.Rows(index).Item(1))
            item.SubItems.Add(availableParams.Rows(index).Item(2))
            lvw.Items.Add(item)
        Next

    End Sub

    Public Sub FillListView(ByVal ListViewToFill As ListView)
        SetupPickerListView(ListViewToFill)
    End Sub

    Public Property ProgramSettings() As clsSettings
        Get
            Return m_MySettings
        End Get
        Set(ByVal Value As clsSettings)
            m_MySettings = Value
        End Set
    End Property
End Class
