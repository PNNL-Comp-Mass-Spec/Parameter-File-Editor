Imports System.Collections.Generic

Public Class clsGetResiduesList

    ''' <summary>
    ''' Dictionary where keys are amino acid residue (one letter abbreviation)
    ''' and values are a dictionary with atom counts (number of C, H, N, O, and S atoms)
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property ResidueAtomCounts As Dictionary(Of Char, Dictionary(Of Char, Integer))

    Public Sub New(connectionString As String)

        ResidueAtomCounts = New Dictionary(Of Char, Dictionary(Of Char, Integer))

        Dim SQL = "SELECT * FROM T_Residues WHERE [Num_C] > 0"

        Dim dbTools = New PRISM.DBTools(connectionString)

        Dim residuesTable As List(Of List(Of String)) = Nothing
        dbTools.GetQueryResults(SQL, residuesTable, "clsGetResiduesList")

        Dim columnNames = New List(Of String) From {
            "Residue_ID",
            "Residue_Symbol",
            "Description",
            "Average_Mass",
            "Monoisotopic_Mass",
            "Num_C",
            "Num_H",
            "Num_N",
            "Num_O",
            "Num_S"
        }

        '' This maps column name to column index
        Dim columnMap = dbTools.GetColumnMapping(columnNames)

        Dim elementSymbols = New List(Of Char) From {
            "C"c,
            "H"c,
            "N"c,
            "O"c,
            "S"c
        }

        For Each resultRow In residuesTable

            Dim residueSymbol = dbTools.GetColumnValue(resultRow, columnMap, "Residue_Symbol")

            Dim atomCountsForResidue = New Dictionary(Of Char, Integer)

            ' Get the atom counts
            ' This for loop access columns Num_C, Num_H, Num_N, etc.
            For Each elementSymbol In elementSymbols
                Dim columnName = "Num_" & elementSymbol
                Dim elementCount = dbTools.GetColumnValue(resultRow, columnMap, columnName)

                Dim elementCountVal As Integer
                If (Integer.TryParse(elementCount, elementCountVal)) Then
                    atomCountsForResidue.Add(elementSymbol, elementCountVal)
                Else
                    atomCountsForResidue.Add(elementSymbol, 0)
                End If

            Next

            ResidueAtomCounts.Add(residueSymbol.Chars(0), atomCountsForResidue)
        Next

        ' Me.m_ResiduesTable = Me.GetTable(SQL)

    End Sub
End Class
