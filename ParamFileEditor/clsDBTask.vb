Imports System.Collections.Specialized
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports ParamFileEditor.ProgramSettings

'Imports GANETManager.Logging

Public MustInherit Class clsDBTask

#Region "Member Variables"

    ' access to mgr parameters
    Protected m_ProgramSettings As IProgramSettings

    ' DB access
    Protected m_connection_str As String
    Protected m_DBCn As SqlConnection
    Protected m_error_list As New StringCollection

    'Protected m_GetEntries_DA As SqlClient.SqlDataAdapter
    'Protected m_GetEntries_DB As SqlClient.SqlCommandBuilder


#End Region

    ' constructor
    Public Sub New()
        'm_ProgramSettings = mgrParams
        'm_connection_str = m_ProgramSettings.DMS_ConnectionString
    End Sub

    Public Sub New(ByVal ConnectionString As String)
        Me.m_connection_str = ConnectionString

    End Sub

    '------[for DB access]-----------------------------------------------------------
    Protected Sub OpenConnection()
        If Me.m_connection_str = "" Then
            Exit Sub
        End If
        OpenConnection(Me.m_connection_str)
    End Sub


    Protected Sub OpenConnection(ByVal ConnectionString As String)
        Dim retryCount As Integer = 3
        While retryCount > 0
            Try
                m_DBCn = New SqlConnection(ConnectionString)
                m_DBCn.Open()
                retryCount = 0
            Catch e As SqlException
                retryCount -= 1
                m_DBCn.Close()
            End Try
        End While
    End Sub

    Protected Sub CloseConnection()
        If Not m_DBCn Is Nothing Then
            m_DBCn.Close()
        End If
    End Sub

    Protected ReadOnly Property Connected() As Boolean
        Get
            If m_DBCn Is Nothing Then
                Return False
            Else
                Return (m_DBCn.State = ConnectionState.Open)
            End If
        End Get
    End Property

    Protected Function GetTable( _
        ByVal SelectSQL As String, _
        ByRef SQLDataAdapter As SqlClient.SqlDataAdapter, _
        ByRef SQLCommandBuilder As SqlClient.SqlCommandBuilder) As DataTable         'Common

        Me.OpenConnection()

        Dim GetID_CMD As SqlClient.SqlCommand = New SqlClient.SqlCommand(SelectSQL)
        GetID_CMD.CommandTimeout = 30
        GetID_CMD.Connection = Me.m_DBCn
        Dim tmpIDTable As New DataTable

        If Me.Connected = True Then

            SQLDataAdapter = New SqlClient.SqlDataAdapter
            SQLCommandBuilder = New SqlClient.SqlCommandBuilder(SQLDataAdapter)
            SQLDataAdapter.SelectCommand = GetID_CMD


            SQLDataAdapter.Fill(tmpIDTable)

            Me.CloseConnection()
        Else
            tmpIDTable = Nothing
        End If

        Return tmpIDTable

    End Function

    Protected Sub CreateRelationship( _
        ByVal ds As DataSet, _
        ByVal dt1 As DataTable, _
        ByVal dt1_keyFieldName As String, _
        ByVal dt2 As DataTable, _
        ByVal dt2_keyFieldName As String)

        Dim dc_dt1_keyField As DataColumn = dt1.Columns(dt1_keyFieldName)
        Dim dc_dt2_keyField As DataColumn = dt2.Columns(dt2_keyFieldName)
        ds.Relations.Add(dc_dt1_keyField, dc_dt2_keyField)

    End Sub

    Protected Sub SetPrimaryKey( _
        ByVal keyColumnIndex As Integer, _
        ByVal dt As DataTable)

        Dim pKey(0) As DataColumn
        pKey(0) = dt.Columns(keyColumnIndex)
        dt.PrimaryKey = pKey

    End Sub

    'Protected Sub UpdateSQLServerDB(ByVal dt As DataTable)
    '    Me.OpenConnection()
    '    Me.m_GetID_DA.Update(dt)
    '    Me.CloseConnection()

    'End Sub

End Class
