Imports System.Collections.Specialized
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb

Public Interface IGetSQLData
    Function GetTable(ByVal SelectSQL As String) As DataTable
    Function GetTable( _
        ByVal SelectSQL As String, _
        ByRef SQLDataAdapter As SqlClient.SqlDataAdapter, _
        ByRef SQLCommandBuilder As SqlClient.SqlCommandBuilder) As DataTable

    Sub OpenConnection()
    Sub OpenConnection(ByVal ConnectionString As String)
    Sub CloseConnection()

    Property ConnectionString() As String
    ReadOnly Property Connected() As Boolean
    ReadOnly Property Connection() As SqlClient.SqlConnection

End Interface

Public Class clsDBTask
    Implements IGetSQLData

#Region "Member Variables"

    ' DB access
    Protected m_connection_str As String
    Protected m_DBCn As SqlConnection
    Protected m_error_list As New StringCollection
    Protected m_PersistConnection As Boolean

#End Region

    ' constructor
    Public Sub New(ByVal ConnectionString As String, Optional ByVal PersistConnection As Boolean = False)
        Me.m_connection_str = ConnectionString
        Me.SetupNew(PersistConnection)

    End Sub

    Public Sub New(Optional ByVal PersistConnection As Boolean = False)
        Me.SetupNew(PersistConnection)
    End Sub

    Private Sub SetupNew(ByVal PersistConnection As Boolean)
        Me.m_PersistConnection = PersistConnection
        If Me.m_PersistConnection Then
            Me.OpenConnection(Me.m_connection_str)
        Else

            'if 

        End If
    End Sub


    '------[for DB access]-----------------------------------------------------------
    Protected Sub OpenConnection() Implements IGetSQLData.OpenConnection
        If Me.m_connection_str = "" Then
            Exit Sub
        End If
        OpenConnection(Me.m_connection_str)
    End Sub

    Protected Sub OpenConnection(ByVal ConnectionString As String) Implements IGetSQLData.OpenConnection
        Dim retryCount As Integer = 3
        If m_DBCn Is Nothing Then
            m_DBCn = New SqlConnection(ConnectionString & "Connect Timeout=30")
        End If
        If m_DBCn.State <> ConnectionState.Open Then
            While retryCount > 0
                Try
                    m_DBCn.Open()
                    retryCount = 0
                Catch e As SqlException
                    retryCount -= 1
                    m_DBCn.Close()
                End Try
            End While
        End If
    End Sub

    Protected Sub CloseConnection() Implements IGetSQLData.CloseConnection
        If Not m_DBCn Is Nothing Then
            m_DBCn.Close()
            m_DBCn = Nothing
        End If
    End Sub

    Protected ReadOnly Property Connected() As Boolean Implements IGetSQLData.Connected
        Get
            If m_DBCn Is Nothing Then
                Return False
            Else
                If Me.m_DBCn.State = ConnectionState.Open Then
                    Return True
                Else
                    Return False
                End If
            End If
        End Get
    End Property

    Protected Property ConnectionString() As String Implements IGetSQLData.ConnectionString
        Get
            Return Me.m_connection_str
        End Get
        Set(ByVal Value As String)
            Me.m_connection_str = Value
        End Set
    End Property

    Protected ReadOnly Property Connection() As SqlClient.SqlConnection Implements IGetSQLData.Connection
        Get
            If Me.Connected Then
                Return Me.m_DBCn
            Else
                Me.OpenConnection()
                Return Me.m_DBCn
            End If
        End Get
    End Property

    Protected Function GetTable( _
        ByVal SelectSQL As String, _
        ByRef SQLDataAdapter As SqlClient.SqlDataAdapter, _
        ByRef SQLCommandBuilder As SqlClient.SqlCommandBuilder) As DataTable Implements IGetSQLData.GetTable

        If Not Me.Connected Then Me.OpenConnection()

        Dim GetID_CMD As SqlClient.SqlCommand = New SqlClient.SqlCommand(SelectSQL)
        GetID_CMD.CommandTimeout = 30
        GetID_CMD.Connection = Me.m_DBCn
        Dim tmpIDTable As New DataTable

        If Me.Connected = True Then

            SQLDataAdapter = New SqlClient.SqlDataAdapter
            SQLCommandBuilder = New SqlClient.SqlCommandBuilder(SQLDataAdapter)
            SQLDataAdapter.SelectCommand = GetID_CMD


            SQLDataAdapter.Fill(tmpIDTable)

            If Not Me.m_PersistConnection Then Me.CloseConnection()
        Else
            tmpIDTable = Nothing
        End If

        Return tmpIDTable

    End Function

    Protected Function GetTable(ByVal SelectSQL As String) As DataTable Implements IGetSQLData.GetTable
        Dim tmpDA As SqlClient.SqlDataAdapter
        Dim tmpCB As SqlClient.SqlCommandBuilder

        Return GetTable(SelectSQL, tmpDA, tmpCB)

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

End Class
