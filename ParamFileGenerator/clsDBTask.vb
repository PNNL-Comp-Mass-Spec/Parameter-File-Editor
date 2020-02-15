Imports System.Data.SqlClient
Imports PRISMDatabaseUtils

'Public Interface IGetSQLData
'    Function GetTable(selectSQL As String) As DataTable
'    Function GetTable(
'                      selectSQL As String,
'        ByRef sqlDataAdapter As SqlClient.SqlDataAdapter,
'        ByRef sqlCommandBuilder As SqlClient.SqlCommandBuilder) As DataTable

'    Sub OpenConnection()
'    Sub OpenConnection(dbConnectionString As String)
'    Sub CloseConnection()

'    Property ConnectionString As String
'    ReadOnly Property Connected As Boolean
'    ReadOnly Property Connection As SqlClient.SqlConnection

'End Interface

Public Class clsDBTask

#Region "Member Variables"

    ' DB access

#Disable Warning BC40025 ' Type of member is not CLS-compliant
    Protected ReadOnly mDBTools As IDBTools
#Enable Warning BC40025 ' Type of member is not CLS-compliant

#End Region

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="connectionString"></param>
    Public Sub New(connectionString As String)
        mDBTools = DbToolsFactory.GetDBTools(connectionString)
    End Sub

#Disable Warning BC40028 ' Type of parameter is not CLS-compliant
    Public Sub New(existingDbTools As IDBTools)
        mDBTools = existingDbTools
    End Sub
#Enable Warning BC40028 ' Type of parameter is not CLS-compliant

    <Obsolete("Use the constructor that only has a connection string")>
    Public Sub New(connectionString As String, persistConnection As Boolean)
        mDBTools = DbToolsFactory.GetDBTools(connectionString)
    End Sub

    <Obsolete("Use the constructor that accepts a connection string", True)>
    Public Sub New(Optional persistConnection As Boolean = False)
        Throw New NotImplementedException()
    End Sub

    ''------[for DB access]-----------------------------------------------------------
    'Protected Sub OpenConnection() Implements IGetSQLData.OpenConnection
    '    If Me.m_connection_str = "" Then
    '        Exit Sub
    '    End If
    '    OpenConnection(Me.m_connection_str)
    'End Sub

    'Protected Sub OpenConnection(dbConnectionString As String) Implements IGetSQLData.OpenConnection
    '    Dim retryCount = 3
    '    If m_DBCn Is Nothing Then
    '        m_DBCn = New SqlConnection(dbConnectionString & "Connect Timeout=30")
    '    End If
    '    If m_DBCn.State <> ConnectionState.Open Then
    '        While retryCount > 0
    '            Try
    '                m_DBCn.Open()
    '                retryCount = 0
    '            Catch e As SqlException
    '                retryCount -= 1
    '                If retryCount = 0 Then
    '                    Throw New Exception("could not open database connection after three tries")
    '                End If
    '                System.Threading.Thread.Sleep(3000)
    '                m_DBCn.Close()
    '            End Try
    '        End While
    '    End If
    'End Sub

    'Protected Sub CloseConnection() Implements IGetSQLData.CloseConnection
    '    If Not m_DBCn Is Nothing Then
    '        m_DBCn.Close()
    '        m_DBCn = Nothing
    '    End If
    'End Sub

    'Protected ReadOnly Property Connected As Boolean Implements IGetSQLData.Connected
    '    Get
    '        If m_DBCn Is Nothing Then
    '            Return False
    '        Else
    '            If Me.m_DBCn.State = ConnectionState.Open Then
    '                Return True
    '            Else
    '                Return False
    '            End If
    '        End If
    '    End Get
    'End Property

    'Protected Property ConnectionString As String Implements IGetSQLData.ConnectionString
    '    Get
    '        Return Me.m_connection_str
    '    End Get
    '    Set
    '        Me.m_connection_str = Value
    '    End Set
    'End Property

    'Protected ReadOnly Property Connection As SqlClient.SqlConnection Implements IGetSQLData.Connection
    '    Get
    '        If Me.Connected Then
    '            Return Me.m_DBCn
    '        Else
    '            Me.OpenConnection()
    '            Return Me.m_DBCn
    '        End If
    '    End Get
    'End Property

    Protected Function GetTable(selectSQL As String) As DataTable

        Dim retryCount = 3
        Dim retryDelaySeconds = 5
        Dim timeoutSeconds = 120

        Dim queryResults As DataTable = Nothing
        Dim success = mDBTools.GetQueryResultsDataTable(selectSQL, queryResults, retryCount, retryDelaySeconds, timeoutSeconds)

        If Not success Then
            Throw New Exception("Could not get records after three tries; query: " & selectSQL)
        End If

        Return queryResults

    End Function

    'Protected Function GetTable(SelectSQL As String) As DataTable Implements IGetSQLData.GetTable
    '    Dim tmpDA As SqlClient.SqlDataAdapter = Nothing
    '    Dim tmpCB As SqlClient.SqlCommandBuilder = Nothing

    '    Return GetTable(SelectSQL, tmpDA, tmpCB)

    'End Function

    'Protected Sub CreateRelationship(
    '    ds As DataSet,
    '    dt1 As DataTable,
    '    dt1_keyFieldName As String,
    '    dt2 As DataTable,
    '    dt2_keyFieldName As String)

    '    Dim dc_dt1_keyField As DataColumn = dt1.Columns(dt1_keyFieldName)
    '    Dim dc_dt2_keyField As DataColumn = dt2.Columns(dt2_keyFieldName)
    '    ds.Relations.Add(dc_dt1_keyField, dc_dt2_keyField)

    'End Sub

    'Protected Sub SetPrimaryKey(
    '    keyColumnIndex As Integer,
    '    dt As DataTable)

    '    Dim pKey(0) As DataColumn
    '    pKey(0) = dt.Columns(keyColumnIndex)
    '    dt.PrimaryKey = pKey

    'End Sub

End Class
