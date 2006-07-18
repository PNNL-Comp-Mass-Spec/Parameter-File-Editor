Public Class clsGetResiduesList
    Inherits ParamFileGenerator.clsDBTask

    Private m_ResiduesTable As DataTable

    Public ReadOnly Property ResiduesTable() As DataTable
        Get
            Return Me.m_ResiduesTable
        End Get
    End Property

    Public Sub New(ByVal connectionString As String)
        MyBase.New(connectionString)
        Dim SQL As String
        Dim residuesTableName As String = "T_Residues"

        Me.OpenConnection()

        SQL = "SELECT * FROM " & residuesTableName & " WHERE [Num_C] > 0"

        Me.m_ResiduesTable = Me.GetTable(SQL)

    End Sub
End Class
