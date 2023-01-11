Imports System.Collections.Generic
Imports System.IO

Public Class GetEnzymeBlock

    Private ReadOnly m_templateFilePath As String
    Private ReadOnly m_sectionName As String
    Private ReadOnly m_EnzymeBlockCollection As List(Of String)

    Public Property EnzymeList As EnzymeCollection

    Public Sub New(
        TemplateFilePath As String,
        SectionName As String)

        m_templateFilePath = TemplateFilePath
        m_sectionName = SectionName

        m_EnzymeBlockCollection = GetEnzymeBlock()
        EnzymeList = InterpretEnzymeBlockCollection(m_EnzymeBlockCollection)

    End Sub

    Private Function GetEnzymeBlock() As List(Of String)

        Dim enzymesFromFile As New List(Of String)

        Dim fi = New FileInfo(m_templateFilePath)

        Using reader = New StreamReader(New FileStream(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            While Not reader.EndOfStream
                Dim dataLine = reader.ReadLine()
                If String.IsNullOrWhiteSpace(dataLine) Then
                    Continue While
                End If

                If dataLine = "[" & m_sectionName & "]" Then
                    While Not reader.EndOfStream
                        Dim enzymeLine = reader.ReadLine()
                        If String.IsNullOrWhiteSpace(enzymeLine) Then
                            Continue While
                        End If

                        enzymesFromFile.Add(enzymeLine)
                    End While
                    Exit While
                End If
            End While
        End Using

        If enzymesFromFile.Count = 0 Then
            enzymesFromFile = LoadDefaultEnzymes()
        End If

        Return enzymesFromFile

    End Function

    Private Function LoadDefaultEnzymes() As List(Of String)

        Dim defaultEnzymes As New List(Of String) From {
            "0.  No_Enzyme              0      -           -",
            "1.  Trypsin                1      KR          -",
            "2.  Trypsin_modified       1      KRLNH       -",
            "3.  Chymotrypsin           1      FWYL        -",
            "4.  Chymotrypsin__modified 1      FWY         -",
            "5.  Clostripain            1      R           -",
            "6.  Cyanogen_Bromide       1      M           -",
            "7.  IodosoBenzoate         1      W           -",
            "8.  Proline_Endopept       1      P           -",
            "9.  Staph_Protease         1      E           -",
            "10. Trypsin_K              1      K           P",
            "11. Trypsin_R              1      R           P",
            "12. GluC                   1      ED          -",
            "13. LysC                   1      K           -",
            "14. AspN                   0      D           -",
            "15. Elastase               1      ALIV        P",
            "16. Elastase/Tryp/Chymo    1      ALIVKRWFY   P",
            "17. ArgC                   1      R-          P",
            "18. Do_not_cleave          1      B           -",
            "19. LysN                   0      K           -"
        }

        Return defaultEnzymes

    End Function

    Private Function InterpretEnzymeBlockCollection(enzymeBlock As IEnumerable(Of String)) As EnzymeCollection

        Dim enzymeInfo = New EnzymeCollection()

        For Each s In enzymeBlock
            Dim sTmp = s.Substring(0, InStr(s, " "))
            If InStr(sTmp, ". ") > 0 Then
                Dim tempEnzyme = New EnzymeDetails(s)
                enzymeInfo.add(tempEnzyme)
            End If
        Next

        Return enzymeInfo

    End Function


End Class
