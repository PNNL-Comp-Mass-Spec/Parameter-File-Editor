Imports PRISMDatabaseUtils

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
        Dim connectionStringToUse = DbToolsFactory.AddApplicationNameToConnectionString(connectionString, "ParamFileGenerator")
        mDBTools = DbToolsFactory.GetDBTools(connectionStringToUse)
    End Sub

#Disable Warning BC40028 ' Type of parameter is not CLS-compliant
    Public Sub New(existingDbTools As IDBTools)
        mDBTools = existingDbTools
    End Sub
#Enable Warning BC40028 ' Type of parameter is not CLS-compliant

    <Obsolete("Use the constructor that only has a connection string")>
    Public Sub New(connectionString As String, persistConnection As Boolean)
        Dim connectionStringToUse = DbToolsFactory.AddApplicationNameToConnectionString(connectionString, "ParamFileGenerator")
        mDBTools = DbToolsFactory.GetDBTools(connectionStringToUse)
    End Sub

    <Obsolete("Use the constructor that accepts a connection string", True)>
    Public Sub New(Optional persistConnection As Boolean = False)
        Throw New NotImplementedException()
    End Sub

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

End Class
