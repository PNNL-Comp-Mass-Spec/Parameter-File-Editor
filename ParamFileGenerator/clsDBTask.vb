Imports System.Collections.Specialized
Imports System.Data.SqlClient

Public Interface IGetSQLData
    Function GetTable(SelectSQL As String) As DataTable
    Function GetTable(
        SelectSQL As String,
        ByRef SQLDataAdapter As SqlClient.SqlDataAdapter,
        ByRef SQLCommandBuilder As SqlClient.SqlCommandBuilder) As DataTable

    Sub OpenConnection()
    Sub OpenConnection(dbConnectionString As String)
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
    Public Sub New(dbConnectionString As String, Optional PersistConnection As Boolean = False)
        Me.m_connection_str = dbConnectionString
        Me.SetupNew(PersistConnection)

    End Sub

    Public Sub New(Optional PersistConnection As Boolean = False)
        Me.SetupNew(PersistConnection)
    End Sub

    Private Sub SetupNew(PersistConnection As Boolean)
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

    Protected Sub OpenConnection(dbConnectionString As String) Implements IGetSQLData.OpenConnection
        Dim retryCount = 3
        If m_DBCn Is Nothing Then
            m_DBCn = New SqlConnection(dbConnectionString & "Connect Timeout=30")
        End If
        If m_DBCn.State <> ConnectionState.Open Then
            While retryCount > 0
                Try
                    m_DBCn.Open()
                    retryCount = 0
                Catch e As SqlException
                    retryCount -= 1
                    If retryCount = 0 Then
                        Throw New Exception("could not open database connection after three tries")
                    End If
                    System.Threading.Thread.Sleep(3000)
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
        Set(Value As String)
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

    Protected Function GetTable(
        SelectSQL As String,
        ByRef SQLDataAdapter As SqlClient.SqlDataAdapter,
        ByRef SQLCommandBuilder As SqlClient.SqlCommandBuilder) As DataTable Implements IGetSQLData.GetTable

        Dim tmpIDTable As New DataTable
        Dim GetID_CMD = New SqlClient.SqlCommand(SelectSQL)

        Dim numTries As Integer = 3
        'Dim tryCount As Integer
        'Try
        If Not Me.m_PersistConnection Then Me.OpenConnection()

        GetID_CMD.CommandTimeout = 120
        GetID_CMD.Connection = Me.m_DBCn

        If Me.Connected = True Then

            SQLDataAdapter = New SqlClient.SqlDataAdapter
            SQLCommandBuilder = New SqlClient.SqlCommandBuilder(SQLDataAdapter)
            SQLDataAdapter.SelectCommand = GetID_CMD

            While numTries > 0
                Try
                    SQLDataAdapter.Fill(tmpIDTable)
                    Exit While
                Catch ex As Exception
                    numTries -= 1
                    If numTries = 0 Then
                        Throw New Exception("could not get records after three tries")
                    End If
                    System.Threading.Thread.Sleep(10000)
                End Try

            End While
            If Not Me.m_PersistConnection Then Me.CloseConnection()
        Else
            tmpIDTable = Nothing
        End If

        Return tmpIDTable

    End Function

    Protected Function GetTable(SelectSQL As String) As DataTable Implements IGetSQLData.GetTable
        Dim tmpDA As SqlClient.SqlDataAdapter = Nothing
        Dim tmpCB As SqlClient.SqlCommandBuilder = Nothing

        Return GetTable(SelectSQL, tmpDA, tmpCB)

    End Function

    Protected Sub CreateRelationship(
        ds As DataSet,
        dt1 As DataTable,
        dt1_keyFieldName As String,
        dt2 As DataTable,
        dt2_keyFieldName As String)

        Dim dc_dt1_keyField As DataColumn = dt1.Columns(dt1_keyFieldName)
        Dim dc_dt2_keyField As DataColumn = dt2.Columns(dt2_keyFieldName)
        ds.Relations.Add(dc_dt1_keyField, dc_dt2_keyField)

    End Sub

    Protected Sub SetPrimaryKey(
        keyColumnIndex As Integer,
        dt As DataTable)

        Dim pKey(0) As DataColumn
        pKey(0) = dt.Columns(keyColumnIndex)
        dt.PrimaryKey = pKey

    End Sub

End Class
