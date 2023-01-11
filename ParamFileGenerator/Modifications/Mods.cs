Imports System.Collections.Generic
Imports System.Linq

Public Class Mods
    Inherits CollectionBase

    Public Enum ResidueCode
        C_Term_Protein
        C_Term_Peptide
        N_Term_Protein
        N_Term_Peptide
        G_Glycine
        A_Alanine
        S_Serine
        P_Proline
        V_Valine
        T_Threonine
        C_Cysteine
        L_Leucine
        I_Isoleucine
        X_LorI
        N_Asparagine
        O_Ornithine
        B_avg_NandD
        D_Aspartic_Acid
        Q_Glutamine
        K_Lysine
        Z_avg_QandE
        E_Glutamic_Acid
        M_Methionine
        H_Histidine
        F_Phenylalanine
        R_Arginine
        Y_Tyrosine
        W_Tryptophan
    End Enum

    Public Enum IsotopeList
        C
        H
        O
        N
        S
    End Enum

    Public ReadOnly Property ModCount As Integer
        Get
            Return List.Count
        End Get
    End Property
    Public ReadOnly Property Initialized As Boolean
        Get
            If List.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property GetModEntry(index As Integer) As ModEntry
        Get
            Return CType(List.Item(index), ModEntry)
        End Get
    End Property

    Public ReadOnly Property NumMods As Integer
        Get
            Return List.Count()
        End Get
    End Property

    Public Sub New()
        MyBase.New()
        LoadAAMappingColl()
    End Sub

    Public Overridable Sub Add(
        AffectedResidue As ResidueCode,
        MassDifference As Double,
        Optional GlobalModID As Integer = 0)

        m_Add(ConvertResidueCodeToSLC(AffectedResidue), MassDifference, ModEntry.ModificationTypes.Static, GlobalModID)
    End Sub

    Public Sub Add(
        AffectedResidueString As String,
        MassDifference As Double)

        Dim residueList = ConvertAffectedResStringToList(AffectedResidueString)
        Dim newMod As New ModEntry(residueList, MassDifference, ModEntry.ModificationTypes.Static)
        List.Add(newMod)
    End Sub

    Public Sub Insert(index As Integer, newMod As ModEntry)
        List.Insert(index, newMod)
    End Sub

    Public Sub Remove(index As Integer)
        List.RemoveAt(index)
    End Sub

    Public Sub Replace(index As Integer, newMod As ModEntry)
        List.RemoveAt(index)
        List.Insert(index, newMod)
    End Sub

    Public Function GetMassDiff(index As Integer) As String
        Dim m = DirectCast(List.Item(index), ModEntry)
        Return Format(m.MassDifference, "0.00000")
    End Function

    ''' <summary>
    ''' Keys are residue names from ResidueCode (e.g. P_Proline)
    ''' Values are the single letter abbreviation if an amino acid
    ''' Or, if not an amino acid, one of: C_Term_Protein, C_Term_Peptide, N_Term_Protein, or N_Term_Peptide
    ''' </summary>
    Protected m_AAMappingTable As Dictionary(Of String, String)
    Protected Sub m_Add(
        AffectedEntity As String,
        MassDifference As Double,
        ModType As ModEntry.ModificationTypes,
        Optional GlobalModID As Integer = 0)

        Dim residueList = ConvertAffectedResStringToList(AffectedEntity)
        Dim newMod As New ModEntry(residueList, MassDifference, ModType, GlobalModID)
        List.Add(newMod)

    End Sub

    Protected Sub LoadAAMappingColl()
        Dim AAEnums = [Enum].GetNames(GetType(ResidueCode)).ToList()

        m_AAMappingTable = New Dictionary(Of String, String)

        For Each AA In AAEnums
            If AA = "C_Term_Protein" Or AA = "C_Term_Peptide" Or AA = "N_Term_Protein" Or AA = "N_Term_Peptide" Then
                m_AAMappingTable.Add(AA, AA)
            Else
                m_AAMappingTable.Add(AA, AA.Substring(0, 1))
            End If
        Next
    End Sub

    Protected Function ConvertAffectedResStringToList(affectedResidueString As String) As List(Of String)
        Dim aaList As New List(Of String)

        If affectedResidueString = "C_Term_Protein" OrElse
           affectedResidueString = "C_Term_Peptide" OrElse
           affectedResidueString = "N_Term_Protein" OrElse
           affectedResidueString = "N_Term_Peptide" Then
            aaList.Add(affectedResidueString)
        Else
            For counter = 1 To Len(affectedResidueString)
                Dim tmpAA = Mid(affectedResidueString, counter, 1)
                'If InStr("><[]",tmpAA) = 0 Then
                aaList.Add(tmpAA)
                'End If

            Next
        End If

        Return aaList
    End Function

    Protected Function ConvertResidueCodeToSLC(Residue As ResidueCode) As String
        Dim tmpRes As String = Residue.ToString()

        Dim tmpSLC As String = Nothing
        If m_AAMappingTable.TryGetValue(tmpRes, tmpSLC) Then
            Return tmpSLC
        End If

        Return String.Empty
    End Function

    Protected Function ConvertSLCToResidueCode(SingleLetterAA As String) As ResidueCode

        For Each item In m_AAMappingTable
            Dim ResString = item.Key
            If SingleLetterAA = ResString.Substring(0, 1) And Not ResString.Contains("Term") Then
                Return DirectCast([Enum].Parse(GetType(ResidueCode), ResString), ResidueCode)
            End If
        Next

    End Function

    Protected Function m_FindModIndex(modifiedEntity As String) As Integer
        Dim statMod As ModEntry

        For Each statMod In List
            Dim testCase = statMod.ReturnResidueAffected(0)
            If testCase = modifiedEntity Then
                Return List.IndexOf(statMod)
            End If
        Next

        Return -1
    End Function

    Protected Function m_FindMod(ModifiedEntity As String) As ModEntry
        Dim ModEntry As ModEntry
        Dim ModIndex As Integer = m_FindModIndex(ModifiedEntity)
        If ModIndex = -1 Then
            ModEntry = Nothing
        Else
            ModEntry = DirectCast(List.Item(ModIndex), ModEntry)
        End If

        If ModEntry Is Nothing Then
            Dim sc As New List(Of String) From {
                ModifiedEntity
            }

            Dim emptyMod As New ModEntry(sc, 0.0, ModEntry.ModificationTypes.Dynamic)
            Return emptyMod
        Else
            Return ModEntry
        End If

    End Function

    Protected Sub m_ChangeMod(
        foundMod As ModEntry,
        ModifiedEntity As String,
        MassDifference As Double,
        Optional Additive As Boolean = False)

        If Math.Abs(foundMod.MassDifference) < Single.Epsilon And Math.Abs(MassDifference) > Single.Epsilon Then
            m_Add(ModifiedEntity, MassDifference, foundMod.ModificationType)
            Exit Sub
        ElseIf Math.Abs(foundMod.MassDifference) < Single.Epsilon And Math.Abs(MassDifference) < Single.Epsilon Then
            Exit Sub
        ElseIf Math.Abs(foundMod.MassDifference) > Single.Epsilon Then          'Not an emptyMod
            Dim counter As Integer
            Dim tempMod As ModEntry

            Dim residueList As List(Of String) = ConvertAffectedResStringToList(ModifiedEntity)
            Dim changeMod As ModEntry

            If Additive Then
                changeMod = New ModEntry(residueList, MassDifference + foundMod.MassDifference, foundMod.ModificationType)
            Else
                changeMod = New ModEntry(residueList, MassDifference, foundMod.ModificationType)
            End If

            For Each tempMod In List
                If foundMod.Equals(tempMod) And Math.Abs(MassDifference) > Single.Epsilon Then
                    Replace(counter, changeMod)
                    Exit Sub
                ElseIf foundMod.Equals(tempMod) And Math.Abs(MassDifference) < Single.Epsilon Then
                    RemoveAt(counter)
                End If
                If List.Count = 0 Then Exit For
                counter += 1
            Next

        End If
    End Sub

End Class
