Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Text.RegularExpressions

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

Public Class ModEntry

    Enum ModificationTypes
        Dynamic
        [Static]
        Isotopic
        TermProt
        TermPep
    End Enum

    Public ReadOnly Property TotalNumResiduesAffected As Integer
        Get
            Return ResidueCollection.Count
        End Get
    End Property

    Public ReadOnly Property ReturnResidueAffected(residueSCIndex As Integer) As String
        Get
            Return ResidueCollection(residueSCIndex)
        End Get
    End Property

    Public ReadOnly Property ReturnAllAffectedResidues As List(Of String)
        Get
            Return ResidueCollection
        End Get
    End Property

    Public Property ResidueCollection As List(Of String)

    Public ReadOnly Property ReturnAllAffectedResiduesString As String
        Get
            Return ConvertListToAAString(ResidueCollection)
        End Get
    End Property

    Public Property MassDifference As Double

    Public Property GlobalModID As Integer

    Public ReadOnly Property ModificationTypeString As String
        Get
            Return GetModTypeSymbol()
        End Get
    End Property

    Public ReadOnly Property ModificationType As ModificationTypes

    Private Sub AddResidue(newResidue As String)
        ResidueCollection.Add(newResidue)
    End Sub

    Private Sub RemoveResidue(badResidue As String)
        ResidueCollection.Remove(badResidue)
    End Sub

    Private Function ConvertListToAAString(resCollection As IEnumerable(Of String)) As String
        Dim s As String
        Dim returnString = ""
        For Each s In resCollection
            s = Left(s, 1)
            returnString &= s
        Next
        Return returnString
    End Function

    Private Function GetModTypeSymbol() As String
        Select Case ModificationType
            Case ModificationTypes.Dynamic
                Return "D"
            Case ModificationTypes.Static
                Return "S"
            Case ModificationTypes.Isotopic
                Return "I"
            Case ModificationTypes.TermPep
                Return "T"
            Case ModificationTypes.TermProt
                Return "P"
            Case Else
                Return Nothing
        End Select
    End Function

    Public Sub New(
        affectedResidueList As List(Of String),
        massDiff As Double,
        modType As ModificationTypes,
        Optional modID As Integer = 0)

        ModificationType = modType
        ResidueCollection = affectedResidueList
        MassDifference = massDiff
        GlobalModID = modID
    End Sub

    Public Sub AddAffectedResidue(residueToAdd As String)
        AddResidue(residueToAdd)
    End Sub

    Public Sub RemoveAffectedResidue(ResidueToRemove As String)
        RemoveResidue(ResidueToRemove)
    End Sub

End Class

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



Public Class DynamicMods
    Inherits Mods
    '    Public Property Dyn_Mod_1_MassDiff As Double
    '        Get
    '            Return Dyn_Mod_n_MassDiff(1)
    '        End Get
    '        Set
    '            Dyn_Mod_n_MassDiff(1) = Value
    '        End Set
    '    End Property
    '    Public Property Dyn_Mod_2_MassDiff As Double
    '        Get
    '            Return Dyn_Mod_n_MassDiff(2)
    '        End Get
    '        Set
    '            Dyn_Mod_n_MassDiff(2) = Value
    '        End Set
    '    End Property
    '    Public Property Dyn_Mod_3_MassDiff As Double
    '        Get
    '            Return Dyn_Mod_n_MassDiff(3)
    '        End Get
    '        Set
    '            Dyn_Mod_n_MassDiff(3) = Value
    '        End Set
    '    End Property

    '    Public Property Dyn_Mod_1_AAList As String
    '        Get
    '            Return Dyn_Mod_n_AAList(1)
    '        End Get
    '        Set(Value As String)
    '            Dyn_Mod_n_AAList(1) = Value
    '        End Set
    '    End Property
    '    Public Property Dyn_Mod_2_AAList As String
    '        Get
    '            Return Dyn_Mod_n_AAList(2)
    '        End Get
    '        Set(Value As String)
    '            Dyn_Mod_n_AAList(2) = Value
    '        End Set
    '    End Property
    '    Public Property Dyn_Mod_3_AAList As String
    '        Get
    '            Return Dyn_Mod_n_AAList(3)
    '        End Get
    '        Set(Value As String)
    '            Dyn_Mod_n_AAList(3) = Value
    '        End Set
    '    End Property

    Public Sub New(DynamicModString As String)
        MyBase.New()
        m_OrigDynModString = DynamicModString
        ParseDynModString(m_OrigDynModString)
    End Sub

    Public Sub New()
        MyBase.New()
        m_OrigDynModString = Nothing
    End Sub

    Public Function ReturnDynModString(maxDynMods As Integer) As String
        Dim s As String
        'If Initialized Then
        s = AssembleModString(maxDynMods)
        Return s
        'Else
        '    Return ""
        'End If
    End Function

    ' 'TODO replace with real function for term dyn mods
    ' 'Just a placeholder for now
    ''Public Function ReturnDynTermModString As String
    ''    Return "0.0000 0.0000"
    ''End Function

    Public Overloads Sub Add(
        AffectedResidueString As String,
        MassDifference As Double)

        Dim residueList = ConvertAffectedResStringToList(AffectedResidueString)
        Dim newDynMod As New ModEntry(residueList, MassDifference, ModEntry.ModificationTypes.Dynamic)
        List.Add(newDynMod)
    End Sub
    Public Overloads Sub Add(ModToAdd As ModEntry)
        List.Add(ModToAdd)
    End Sub

    Public Property Dyn_Mod_n_MassDiff(DynModNumber As Integer) As Double
        Get
            Dim dm As ModEntry
            Dim index As Integer = DynModNumber - 1
            Try
                dm = DirectCast(List.Item(index), ModEntry)
            Catch ex As Exception
                dm = New ModEntry(ConvertAffectedResStringToList("C"), 0.0, ModEntry.ModificationTypes.Dynamic)
            End Try
            Return dm.MassDifference
        End Get
        Set
            Dim index As Integer = DynModNumber - 1
            Dim dm As ModEntry
            If index <= List.Count - 1 Then
                dm = DirectCast(List.Item(index), ModEntry)
                dm.MassDifference = Value
                Replace(index, dm)
            Else
                Add("C", Value)
            End If
        End Set
    End Property

    Public Property Dyn_Mod_n_AAList(DynModNumber As Integer) As String
        Get
            Dim dm As ModEntry
            Dim index As Integer = DynModNumber - 1
            Try
                dm = DirectCast(List.Item(index), ModEntry)
            Catch ex As Exception
                dm = New ModEntry(ConvertAffectedResStringToList("C"), 0.0, ModEntry.ModificationTypes.Dynamic)
            End Try
            Return dm.ReturnAllAffectedResiduesString
        End Get
        Set
            Dim index As Integer = DynModNumber - 1
            Dim dm As ModEntry
            If index <= List.Count - 1 Then
                dm = DirectCast(List.Item(index), ModEntry)
                dm.ResidueCollection = ConvertAffectedResStringToList(Value)
                Replace(index, dm)
            Else
                Add(Value, CDbl(0.0))
            End If
        End Set
    End Property

    Public Property Dyn_Mod_n_Global_ModID(DynModNumber As Integer) As Integer
        Get
            Dim dm As ModEntry
            Dim index As Integer = DynModNumber - 1
            Try
                dm = DirectCast(List.Item(index), ModEntry)
            Catch ex As Exception
                dm = New ModEntry(ConvertAffectedResStringToList("C"), 0.0, ModEntry.ModificationTypes.Dynamic)
            End Try
            Return dm.GlobalModID
        End Get
        Set
            Dim index As Integer = DynModNumber - 1
            Dim dm As ModEntry
            If index <= List.Count - 1 Then
                dm = DirectCast(List.Item(index), ModEntry)
                dm.GlobalModID = Value
                Replace(index, dm)
            Else
                Add("C", 0.0)
            End If
        End Set
    End Property

    Protected Overridable Function AssembleModString(counter As Integer) As String
        Dim s = ""
        Dim tmpModString As String
        Dim tmpModMass As Double
        Dim dynMod As ModEntry
        Dim padCount As Integer

        For Each dynMod In List
            tmpModMass = dynMod.MassDifference
            tmpModString = dynMod.ReturnAllAffectedResiduesString
            s = s & Format(tmpModMass, "0.0000") & " " & tmpModString & " "
            counter -= 1
        Next

        For padCount = 0 To counter - 1
            If padCount <= 2 Then
                s &= "0.0000 C "
            Else
                s &= "0.0000 X "
            End If
        Next

        Return s.Trim()
    End Function

    Protected Overridable Sub ParseDynModString(DMString As String)

        Dim splitRE = New Regex("(?<modmass>\d+\.\d+)\s+(?<residues>[A-Za-z]+)")
        Dim matches = splitRE.Matches(DMString)

        For Each m As Match In matches
            Dim tmpMass = CDbl(m.Groups("modmass").Value)
            Dim tmpResString = m.Groups("residues").ToString()

            If Math.Abs(tmpMass) > Single.Epsilon Then
                Dim residueList = New List(Of String)
                For resCounter = 1 To Len(tmpResString)
                    Dim tmpRes = Mid(tmpResString, resCounter, 1)
                    residueList.Add(tmpRes)
                Next
                Dim modEntry As New ModEntry(residueList, tmpMass, ModEntry.ModificationTypes.Dynamic)
                Add(modEntry)
            End If
        Next

    End Sub

    Protected m_OrigDynModString As String
    'private m_EmptyMod as New ModEntry(

End Class


Public Class StaticMods
    Inherits Mods

    Public Property CtermPeptide As Double
        Get
            Return FindAAMod(ResidueCode.C_Term_Peptide).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.C_Term_Peptide, Value)
        End Set
    End Property

    Public Property CtermProtein As Double
        Get
            Return FindAAMod(ResidueCode.C_Term_Protein).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.C_Term_Protein, Value)
        End Set
    End Property
    Public Property NtermPeptide As Double
        Get
            Return FindAAMod(ResidueCode.N_Term_Peptide).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.N_Term_Peptide, Value)
        End Set
    End Property

    Public Property NtermProtein As Double
        Get
            Return FindAAMod(ResidueCode.N_Term_Protein).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.N_Term_Protein, Value)
        End Set
    End Property

    Public Property G_Glycine As Double
        Get
            Return FindAAMod(ResidueCode.G_Glycine).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.G_Glycine, Value)
        End Set
    End Property

    Public Property A_Alanine As Double
        Get
            Return FindAAMod(ResidueCode.A_Alanine).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.A_Alanine, Value)
        End Set
    End Property

    Public Property S_Serine As Double
        Get
            Return FindAAMod(ResidueCode.S_Serine).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.S_Serine, Value)
        End Set
    End Property

    Public Property P_Proline As Double
        Get
            Return FindAAMod(ResidueCode.P_Proline).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.P_Proline, Value)
        End Set
    End Property

    Public Property V_Valine As Double
        Get
            Return FindAAMod(ResidueCode.V_Valine).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.V_Valine, Value)
        End Set
    End Property

    Public Property T_Threonine As Double
        Get
            Return FindAAMod(ResidueCode.T_Threonine).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.T_Threonine, Value)
        End Set
    End Property
    Public Property C_Cysteine As Double
        Get
            Return FindAAMod(ResidueCode.C_Cysteine).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.C_Cysteine, Value)
        End Set
    End Property

    Public Property L_Leucine As Double
        Get
            Return FindAAMod(ResidueCode.L_Leucine).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.L_Leucine, Value)
        End Set
    End Property

    Public Property I_Isoleucine As Double
        Get
            Return FindAAMod(ResidueCode.I_Isoleucine).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.I_Isoleucine, Value)
        End Set
    End Property

    Public Property X_LorI As Double
        Get
            Return FindAAMod(ResidueCode.X_LorI).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.X_LorI, Value)
        End Set
    End Property

    Public Property N_Asparagine As Double
        Get
            Return FindAAMod(ResidueCode.N_Asparagine).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.N_Asparagine, Value)
        End Set
    End Property

    Public Property O_Ornithine As Double
        Get
            Return FindAAMod(ResidueCode.O_Ornithine).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.O_Ornithine, Value)
        End Set
    End Property

    Public Property B_avg_NandD As Double
        Get
            Return FindAAMod(ResidueCode.B_avg_NandD).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.B_avg_NandD, Value)
        End Set
    End Property

    Public Property D_Aspartic_Acid As Double
        Get
            Return FindAAMod(ResidueCode.D_Aspartic_Acid).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.D_Aspartic_Acid, Value)
        End Set
    End Property

    Public Property Q_Glutamine As Double
        Get
            Return FindAAMod(ResidueCode.Q_Glutamine).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.Q_Glutamine, Value)
        End Set
    End Property

    Public Property K_Lysine As Double
        Get
            Return FindAAMod(ResidueCode.K_Lysine).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.K_Lysine, Value)
        End Set
    End Property

    Public Property Z_avg_QandE As Double
        Get
            Return FindAAMod(ResidueCode.Z_avg_QandE).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.Z_avg_QandE, Value)
        End Set
    End Property

    Public Property E_Glutamic_Acid As Double
        Get
            Return FindAAMod(ResidueCode.E_Glutamic_Acid).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.E_Glutamic_Acid, Value)
        End Set
    End Property

    Public Property M_Methionine As Double
        Get
            Return FindAAMod(ResidueCode.M_Methionine).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.M_Methionine, Value)
        End Set
    End Property

    Public Property H_Histidine As Double
        Get
            Return FindAAMod(ResidueCode.H_Histidine).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.H_Histidine, Value)
        End Set
    End Property

    Public Property F_Phenylalanine As Double
        Get
            Return FindAAMod(ResidueCode.F_Phenylalanine).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.F_Phenylalanine, Value)
        End Set
    End Property

    Public Property R_Arginine As Double
        Get
            Return FindAAMod(ResidueCode.R_Arginine).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.R_Arginine, Value)
        End Set
    End Property

    Public Property Y_Tyrosine As Double
        Get
            Return FindAAMod(ResidueCode.Y_Tyrosine).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.Y_Tyrosine, Value)
        End Set
    End Property

    Public Property W_Tryptophan As Double
        Get
            Return FindAAMod(ResidueCode.W_Tryptophan).MassDifference
        End Get
        Set
            ChangeAAMod(ResidueCode.W_Tryptophan, Value)
        End Set
    End Property

    Public Sub New()
        MyBase.New()
    End Sub

    Public Function GetResidue(index As Integer) As String
        Dim m = DirectCast(List.Item(index), ModEntry)
        Return m.ReturnResidueAffected(0)
    End Function
    Public Sub ChangeAAModification(ModifiedAA As ResidueCode, MassDifference As Double, Optional Additive As Boolean = False)
        ChangeAAMod(ModifiedAA, MassDifference, Additive)
    End Sub
    Public Sub EradicateEmptyMods()
        KillBlankMods()
    End Sub

    Private Function FindAAMod(ModifiedAA As ResidueCode) As ModEntry
        Return m_FindMod(ConvertResidueCodeToSLC(ModifiedAA))
    End Function

    Private Sub ChangeAAMod(ModifiedAA As ResidueCode, MassDifference As Double, Optional Additive As Boolean = False)
        Dim foundMod As ModEntry = FindAAMod(ModifiedAA)
        Dim ModAAString As String = ConvertResidueCodeToSLC(ModifiedAA)
        m_ChangeMod(foundMod, ModAAString, MassDifference, Additive)

    End Sub

    Private Sub KillBlankMods()
        Dim AA As String
        Dim AASLC As String
        Dim AAEnums = [Enum].GetNames(GetType(ResidueCode)).ToList()
        Dim currIndex As Integer
        Dim modEntry As ModEntry

        For Each AA In AAEnums
            If InStr(AA, "Term") = 0 Then
                AASLC = Left(AA, 1)
                currIndex = m_FindModIndex(AASLC)
                If currIndex <> -1 Then
                    modEntry = GetModEntry(currIndex)
                    If Math.Abs(modEntry.MassDifference) < Single.Epsilon Then
                        List.Remove(modEntry)
                    End If
                End If
            End If
        Next
    End Sub

End Class

Public Class IsoMods
    Inherits Mods

    Public Overloads Sub Add(AffectedAtom As IsotopeList, MassDifference As Double, Optional GlobalModID As Integer = 0)
        m_Add(AffectedAtom.ToString, MassDifference, ModEntry.ModificationTypes.Isotopic, GlobalModID)
    End Sub

    Public Function GetAtom(index As Integer) As String
        Dim m = DirectCast(List.Item(index), ModEntry)
        Return m.ReturnResidueAffected(0)
    End Function

    Public Property Iso_C As Double
        Get
            Return FindIsoMod(IsotopeList.C).MassDifference
        End Get
        Set
            ChangeIsoMod(IsotopeList.C, Value)
        End Set
    End Property

    Public Property Iso_H As Double
        Get
            Return FindIsoMod(IsotopeList.H).MassDifference
        End Get
        Set
            ChangeIsoMod(IsotopeList.H, Value)
        End Set
    End Property

    Public Property Iso_O As Double
        Get
            Return FindIsoMod(IsotopeList.O).MassDifference
        End Get
        Set
            ChangeIsoMod(IsotopeList.O, Value)
        End Set
    End Property

    Public Property Iso_N As Double
        Get
            Return FindIsoMod(IsotopeList.N).MassDifference
        End Get
        Set
            ChangeIsoMod(IsotopeList.N, Value)
        End Set
    End Property

    Public Property Iso_S As Double
        Get
            Return FindIsoMod(IsotopeList.S).MassDifference
        End Get
        Set
            ChangeIsoMod(IsotopeList.S, Value)
        End Set
    End Property

    Private Sub ChangeIsoMod(AffectedAtom As IsotopeList, MassDifference As Double)
        Dim foundMod As ModEntry = FindIsoMod(AffectedAtom)
        Dim ModAAString As String = AffectedAtom.ToString
        m_ChangeMod(foundMod, ModAAString, MassDifference)
    End Sub

    Private Function FindIsoMod(AffectedAtom As IsotopeList) As ModEntry
        Return m_FindMod(AffectedAtom.ToString)
    End Function

End Class