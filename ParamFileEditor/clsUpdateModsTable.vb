'Imports ParamFileEditor.SequestParams
Imports ParamFileEditor.ProgramSettings
Imports ParamFileGenerator
Imports System.Data.SqlClient

Public Class clsUpdateModsTable
    Inherits clsDBTask

#Region " Member Properties "
    Private m_ParamFileList As DataTable
    Private m_GlobalModList As DataTable
    Private m_Settings As IProgramSettings
    Private m_GetGlobalMods_DA As SqlClient.SqlDataAdapter
    Private m_GetGlobalMods_CB As SqlClient.SqlCommandBuilder
#End Region

#Region " Public Properties "
    Public Property ModParamFileList() As DataTable
        Get
            Return Me.m_ParamFileList
        End Get
        Set(ByVal Value As DataTable)
            Me.m_ParamFileList = Value
        End Set
    End Property
    Public Property ModGlobaList() As DataTable
        Get
            Return Me.m_GlobalModList
        End Get
        Set(ByVal Value As DataTable)
            Me.m_GlobalModList = Value
        End Set
    End Property
#End Region

#Region " Member Procedures "
    'Private Sub LoadParamFileListTable()
    '    Dim SQL As String = "SELECT * FROM " & m_Settings.MT_ModParamFileTable
    '    Me.m_ParamFileList = Me.GetTable(SQL)
    '    Me.SetPrimaryKey(3, Me.m_ParamFileList)

    'End Sub
    'Private Sub LoadGlobalModListTable()
    '    Dim SQL As String = "SELECT * FROM " & m_Settings.MT_GlobalModListTable
    '    Me.m_GlobalModList = Me.GetTable(SQL, Me.m_GetGlobalMods_DA, Me.m_GetGlobalMods_CB)
    '    Me.SetPrimaryKey(0, Me.m_GlobalModList)
    'End Sub
    'Private Sub CommitChanges()
    '    Me.UpdateSQLServerDB(Me.m_ParamFileList)
    '    Me.UpdateSQLServerDB(Me.m_GlobalModList)
    'End Sub
    Private Function GetModsCollInParamFile(ByVal paramFileName As String) As System.Collections.Specialized.StringCollection
        Dim sc As New System.Collections.Specialized.StringCollection
        Dim SelectedRows As DataRow()
        Dim currentRow As DataRow
        Dim tmpMod_ID As Integer
        SelectedRows = Me.m_ParamFileList.Select(Me.m_ParamFileList.Columns(0).ColumnName & " = '" & paramFileName & "'")

        For Each currentRow In SelectedRows
            sc.Add(currentRow.Item(2).ToString)
        Next
        Return sc

    End Function


    Private Function DoesDynModExistInDMS(ByVal dynMod As clsDynamicMods.clsModEntry) As Boolean
        Dim tmpAA As String = dynMod.ReturnAllAffectedResiduesString
        Dim tmpMass As Single = dynMod.MassDifference
        Dim foundRow As DataRow
        Dim foundRows As DataRow() = Me.m_GlobalModList.Select("Affected_Residues = '" & tmpAA & "'")
        If foundRows.Length > 0 Then
            For Each foundRow In foundRows
                If foundRow.Item("Mass_Correction_Factor").Equals(dynMod.MassDifference) Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Private Function DoesStaticModExistInDMS(ByVal statMod As clsStaticMods.clsModEntry)


    End Function




#End Region

#Region " Public Procedures "

    Public Sub New(ByVal mgrParams As ParamFileEditor.ProgramSettings.IProgramSettings)
        MyBase.New(mgrParams.MT_ConnectionString)
        'm_Settings = mgrParams
        'Me.LoadParamFileListTable()
        'Me.LoadGlobalModListTable()
    End Sub
    'Public Sub CommitChangesToDMS()
    '    Me.CommitChanges()
    'End Sub


#End Region

End Class
