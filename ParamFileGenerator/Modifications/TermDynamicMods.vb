Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions

Public Class TermDynamicMods
    Inherits DynamicMods

    Public Const NTERM_SYMBOL As String = "<"
    Public Const CTERM_SYMBOL As String = ">"

    Sub New(TermDynModString As String)
        MyBase.New()
        m_OrigDynModString = TermDynModString
        ParseDynModString(m_OrigDynModString)

    End Sub

    Public Property Dyn_Mod_NTerm As Double
        Get
            Return GetTermDynMod(NTERM_SYMBOL)
        End Get
        Set
            UpdateTermDynMod(NTERM_SYMBOL, Value)
        End Set
    End Property

    Public Property Dyn_Mod_CTerm As Double
        Get
            Return GetTermDynMod(CTERM_SYMBOL)
        End Get
        Set
            UpdateTermDynMod(CTERM_SYMBOL, Value)
        End Set
    End Property

    Protected Function GetTermDynMod(strSymbol As String) As Double
        Dim objModEntry As ModEntry
        objModEntry = m_FindMod(strSymbol)

        If objModEntry Is Nothing Then
            Return 0
        Else
            Return objModEntry.MassDifference
        End If
    End Function

    Protected Sub UpdateTermDynMod(strSymbol As String, sngMass As Double)

        Dim intIndex As Integer
        intIndex = m_FindModIndex(strSymbol)

        If intIndex < 0 Then
            ' Mod was not found
            ' Add it (assuming sngMass is non-zero)
            If Math.Abs(sngMass) > Single.Epsilon Then
                Dim resCollection = New List(Of String) From {
                    strSymbol
                }

                Add(New ModEntry(resCollection, sngMass, ModEntry.ModificationTypes.Dynamic))
            End If
        Else
            ' Mod was found
            ' Update the mass (or remove it if sngMass is zero)
            If Math.Abs(sngMass) < Single.Epsilon Then
                Remove(intIndex)
            Else
                Dim objModEntry As ModEntry
                objModEntry = GetModEntry(intIndex)
                objModEntry.MassDifference = sngMass
            End If
        End If
    End Sub

    Protected Overrides Sub ParseDynModString(DMString As String)
        Dim tmpCTMass As Double
        Dim tmpNTMass As Double

        Dim splitRE = New Regex("(?<ctmodmass>\d+\.*\d*)\s+(?<ntmodmass>\d+\.*\d*)")
        Dim m As Match

        If DMString Is Nothing Then
            Exit Sub
        End If


        If splitRE.IsMatch(DMString) Then
            m = splitRE.Match(DMString)

            tmpCTMass = CDbl(m.Groups("ctmodmass").Value)
            tmpNTMass = CDbl(m.Groups("ntmodmass").Value)

            Dyn_Mod_NTerm = tmpNTMass
            Dyn_Mod_CTerm = tmpCTMass

        End If

    End Sub

    Protected Overrides Function AssembleModString(counter As Integer) As String
        Dim sb = New StringBuilder()

        Dim tmpModString As String
        ' Dim ctRes As String = ">"
        ' Dim ntRes As String = "<"

        Dim ctModMass = 0.0
        Dim ntModMass = 0.0

        Dim tmpModMass As Double

        Dim dynMod As ModEntry

        For Each dynMod In List
            tmpModMass = dynMod.MassDifference
            tmpModString = dynMod.ReturnAllAffectedResiduesString
            If tmpModString = ">" Then
                ctModMass = tmpModMass
                ' ctRes = tmpModString
            ElseIf tmpModString = "<" Then
                ntModMass = tmpModMass
                ' ntRes = tmpModString
            End If
        Next

        sb.Append(Format(ctModMass, "0.000000"))
        sb.Append(" ")
        sb.Append(Format(ntModMass, "0.000000"))

        Return sb.ToString.Trim()
    End Function

End Class
