Imports System.Collections.Specialized

Public Class clsMods
    Inherits CollectionBase

#Region " Enums "
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

#End Region

#Region " Public Properties "
    Public ReadOnly Property ModCount() As Integer
        Get
            Return Me.List.Count
        End Get
    End Property
    public ReadOnly Property Initialized() As Boolean
        Get
            If Me.List.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property GetModEntry(ByVal index As Integer) As clsModEntry
        Get
            Return CType(Me.List.Item(index), clsModEntry)
        End Get
    End Property

    Public ReadOnly Property NumMods() As Integer
        Get
            Return Me.List.Count()
        End Get
    End Property
#End Region

#Region " public Procedures "
    Public Sub New()
        MyBase.New()
        Me.LoadAAMappingColl()
    End Sub

    Public Overridable Sub Add( _
        ByVal AffectedResidue As ResidueCode, _
        ByVal MassDifference As Single, _
        Optional ByVal GlobalModID As Integer = 0)

        Me.m_Add(Me.ConvertResidueCodeToSLC(AffectedResidue), MassDifference, clsModEntry.ModificationTypes.Static, GlobalModID)
    End Sub

    Public Sub Add( _
        ByVal AffectedResidueString As String, _
        ByVal MassDifference As Single)

        Dim sc As StringCollection = Me.ConvertAffectedResStringToSC(AffectedResidueString)
        Dim newMod As New clsModEntry(sc, MassDifference, clsModEntry.ModificationTypes.Static)
        Me.List.Add(newMod)
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal newMod As clsModEntry)
        Me.List.Insert(index, newMod)
    End Sub
    Public Sub Remove(ByVal index As Integer)
        Me.List.RemoveAt(index)
    End Sub
    Public Sub Replace(ByVal index As Integer, ByVal newMod As clsModEntry)
        Me.List.RemoveAt(index)
        Me.List.Insert(index, newMod)
    End Sub
    Public Function GetMassDiff(ByVal index As Integer) As String
        Dim m As clsModEntry = DirectCast(Me.List.Item(index), clsModEntry)
        Return Format(m.MassDifference, "0.0000")
    End Function

#End Region

#Region " Member Properties "
    Protected m_AAMappingTable As NameValueCollection
#End Region

#Region " Member Procedures "
    Protected Sub m_Add( _
        ByVal AffectedEntity As String, _
        ByVal MassDifference As Single, _
        ByVal ModType As clsModEntry.ModificationTypes, _
        Optional ByVal GlobalModID As Integer = 0)

        Dim sc As StringCollection = Me.ConvertAffectedResStringToSC(AffectedEntity)
        Dim newMod As New clsModEntry(sc, MassDifference, ModType, GlobalModID)
        Me.List.Add(newMod)

    End Sub

    Protected Sub LoadAAMappingColl()
        Dim AAEnums() As String = System.Enum.GetNames(GetType(ResidueCode))
        Dim AA As String
        'C_Term_Protein()
        'C_Term_Peptide()
        'N_Term_Protein()
        'N_Term_Peptide()

        m_AAMappingTable = New NameValueCollection

        For Each AA In AAEnums
            If AA = "C_Term_Protein" Or AA = "C_Term_Peptide" Or AA = "N_Term_Protein" Or AA = "N_Term_Peptide" Then
                Me.m_AAMappingTable.Add(AA, AA)
            Else
                Me.m_AAMappingTable.Add(AA, Left(AA, 1))
            End If
        Next
    End Sub

    Protected Function ConvertAffectedResStringToSC(ByVal AffectedResidueString As String) As StringCollection
        Dim counter As Integer
        Dim AA As String = AffectedResidueString
        Dim tmpAA As String
        Dim sc As New StringCollection

        If AA = "C_Term_Protein" Or AA = "C_Term_Peptide" Or AA = "N_Term_Protein" Or AA = "N_Term_Peptide" Then
            sc.Add(AA)
        Else
            For counter = 1 To Len(AffectedResidueString)
                tmpAA = Mid(AffectedResidueString, counter, 1)
                sc.Add(tmpAA)
            Next
        End If
        Return sc
    End Function
    Protected Function ConvertResidueCodeToSLC(ByVal Residue As ResidueCode) As String
        Dim tmpRes As String = Residue.ToString
        Dim tmpSLC As String = Me.m_AAMappingTable.Get(tmpRes)
        Return tmpSLC
    End Function

    Protected Function ConvertSLCToResidueCode(ByVal SingleLetterAA As String) As ResidueCode
        Dim stepper As IEnumerator = Me.m_AAMappingTable.GetEnumerator
        Dim ResString As String
        Do While stepper.MoveNext = True
            ResString = CStr(stepper.Current)
            If SingleLetterAA = Left(ResString, 1) And InStr(ResString, "Term") = 0 Then
                Return DirectCast(System.Enum.Parse(GetType(ResidueCode), ResString), ResidueCode)
            End If
        Loop


    End Function
    Protected Function m_FindModIndex(ByVal modifiedEntity As String) As Integer
        Dim statMod As clsModEntry
        Dim testCase As String
        Dim tmpEntity As String = modifiedEntity

        For Each statMod In Me.List
            testCase = statMod.ReturnResidueAffected(0)
            If testCase = tmpEntity Then
                Return Me.List.IndexOf(statMod)
            End If
        Next

        Return -1
    End Function

    Protected Function m_FindMod(ByVal ModifiedEntity As String) As clsModEntry
        Dim ModEntry As clsModEntry
        Dim ModIndex As Integer = m_FindModIndex(ModifiedEntity)
        If ModIndex = -1 Then
            ModEntry = Nothing
        Else
            ModEntry = DirectCast(Me.List.Item(ModIndex), clsModEntry)
        End If


        If ModEntry Is Nothing Then
            Dim sc As New StringCollection
            sc.Add(ModifiedEntity)
            Dim emptyMod As New clsModEntry(sc, 0.0, clsModEntry.ModificationTypes.Dynamic)
            Return emptyMod
        Else
            Return ModEntry
        End If

    End Function


    Protected Sub m_ChangeMod( _
        ByVal foundMod As clsModEntry, _
        ByVal ModifiedEntity As String, _
        ByVal MassDifference As Single, _
        Optional ByVal Additive As Boolean = False)

        'Dim foundMod As clsModEntry = FindAAMod(ModifiedAA)
        If foundMod.MassDifference = 0.0 And MassDifference <> 0.0 Then
            Me.m_Add(ModifiedEntity, MassDifference, foundMod.ModificationType)
            Exit Sub
        ElseIf foundMod.MassDifference = 0.0 And MassDifference = 0.0 Then
            Exit Sub
        ElseIf foundMod.MassDifference <> 0.0 Then          'Not an emptyMod
            Dim counter As Integer
            Dim tempMod As clsModEntry
            'Dim modAAString As String = Me.ConvertResidueCodeToSLC(ModifiedAA)
            Dim sc As StringCollection = Me.ConvertAffectedResStringToSC(ModifiedEntity)
            Dim changeMod As clsModEntry

            If Additive Then
                changeMod = New clsModEntry(sc, MassDifference + foundMod.MassDifference, foundMod.ModificationType)
            Else
                changeMod = New clsModEntry(sc, MassDifference, foundMod.ModificationType)
            End If

            For Each tempMod In Me.List
                If foundMod.Equals(tempMod) And MassDifference <> 0.0 Then
                    Me.Replace(counter, changeMod)
                    Exit Sub
                ElseIf foundMod.Equals(tempMod) And MassDifference = 0.0 Then
                    Me.RemoveAt(counter)
                End If
                If Me.List.Count = 0 Then Exit For
                counter += 1
            Next

        End If
    End Sub

#End Region

End Class




Public Class clsModEntry

    Enum ModificationTypes
        Dynamic
        [Static]
        Isotopic
        TermProt
        TermPep
    End Enum

#Region " Member Properties "
    Private m_ResiduesAffected As StringCollection
    Private m_MassDiff As Single
    Private m_GlobalModID As Integer
    Private m_IsIsotopic As Boolean
    Private m_ModType As ModificationTypes

#End Region

#Region " Public Properties "

    Public ReadOnly Property TotalNumResiduesAffected() As Integer
        Get
            Return Me.m_ResiduesAffected.Count
        End Get
    End Property
    Public ReadOnly Property ReturnResidueAffected(ByVal ResidueSCIndex As Integer) As String
        Get
            Return Me.m_ResiduesAffected(ResidueSCIndex)
        End Get
    End Property
    Public ReadOnly Property ReturnAllAffectedResidues() As StringCollection
        Get
            Return Me.m_ResiduesAffected
        End Get
    End Property
    Public Property ResidueCollection() As StringCollection
        Get
            Return Me.m_ResiduesAffected
        End Get
        Set(ByVal Value As StringCollection)
            Me.m_ResiduesAffected = Value
        End Set
    End Property
    Public ReadOnly Property ReturnAllAffectedResiduesString() As String
        Get
            Return Me.ConvertSCToAAString(Me.m_ResiduesAffected)
        End Get
    End Property
    Public Property MassDifference() As Single
        Get
            Return Me.m_MassDiff
        End Get
        Set(ByVal Value As Single)
            Me.m_MassDiff = Value
        End Set
    End Property

    Public Property GlobalModID() As Integer
        Get
            Return Me.m_GlobalModID
        End Get
        Set(ByVal Value As Integer)
            Me.m_GlobalModID = Value
        End Set
    End Property

    Public ReadOnly Property ModificationTypeString() As String
        Get
            Return Me.GetModTypeSymbol
        End Get
    End Property

    Public ReadOnly Property ModificationType() As ModificationTypes
        Get
            Return Me.m_ModType
        End Get
    End Property

#End Region

#Region " Member Procedures "
    Private Sub AddResidue(ByVal newResidue As String)
        Me.m_ResiduesAffected.Add(newResidue)
    End Sub
    Private Sub RemoveResidue(ByVal badResidue As String)
        Me.m_ResiduesAffected.Remove(badResidue)
    End Sub
    Private Function ConvertSCToAAString(ByVal resCollection As StringCollection) As String
        Dim s As String
        Dim returnString As String = ""
        For Each s In resCollection
            s = Left(s, 1)
            returnString = returnString & s
        Next
        Return returnString
    End Function

    Private Function GetModTypeSymbol() As String
        Select Case Me.m_ModType
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


#End Region

#Region " Public Procedures "
    Public Sub New( _
        ByVal AffectedResidueList As StringCollection, _
        ByVal MassDifference As Single, _
        ByVal ModType As ModificationTypes, _
        Optional ByVal GlobalModID As Integer = 0)

        Me.m_ModType = ModType
        Me.m_ResiduesAffected = AffectedResidueList
        Me.m_MassDiff = MassDifference
        Me.m_GlobalModID = GlobalModID
    End Sub
    Public Sub AddAffectedResidue(ByVal ResidueToAdd As String)
        Me.AddResidue(ResidueToAdd)
    End Sub
    Public Sub RemoveAffectedResidue(ByVal ResidueToRemove As String)
        Me.RemoveResidue(ResidueToRemove)
    End Sub
#End Region

End Class


Public Class clsTermDynamicMods
    Inherits clsDynamicMods

#Region " Public Procedures "
    Sub New(ByVal TermDynModString As String)
        MyBase.New()
        Me.m_OrigDynModString = TermDynModString
        ParseDynModString(Me.m_OrigDynModString)

    End Sub

    Protected Overrides Sub ParseDynModString(ByVal DMString As String)
        Dim tmpCTMass As Single
        Dim tmpNTMass As Single
        'Dim tmpRes As String
        Dim resCollection As StringCollection

        Dim splitRE As System.Text.RegularExpressions.Regex = _
            New System.Text.RegularExpressions.Regex("(?<ctmodmass>\d+\.\d+)\s+(?<ntmodmass>\d+\.\d+)")
        Dim m As System.Text.RegularExpressions.Match

        If DMString Is Nothing Then
            Exit Sub
        End If


        If splitRE.IsMatch(DMString) Then
            m = splitRE.Match(DMString)

            tmpCTMass = CSng(m.Groups("ctmodmass").Value)
            tmpNTMass = CSng(m.Groups("ntmodmass").Value)

            resCollection = New StringCollection
            resCollection.Add("<")
            Me.Add(New clsModEntry(resCollection, tmpNTMass, clsModEntry.ModificationTypes.Dynamic))

            resCollection = New StringCollection
            resCollection.Add(">")
            Me.Add(New clsModEntry(resCollection, tmpCTMass, clsModEntry.ModificationTypes.Dynamic))
        End If

    End Sub

    Protected Overrides Function AssembleModString(ByVal counter As Integer) As String
        Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder

        Dim tmpModString As String
        Dim ctRes As String = ">"
        Dim ntRes As String = "<"

        Dim ctModMass As Single = 0.0
        Dim ntModMass As Single = 0.0

        Dim tmpModMass As Single

        Dim dynMod As clsModEntry

        For Each dynMod In Me.List
            tmpModMass = dynMod.MassDifference
            tmpModString = dynMod.ReturnAllAffectedResiduesString
            If tmpModString = ">" Then
                ctModMass = tmpModMass
                ctRes = tmpModString
            ElseIf tmpModString = "<" Then
                ntModMass = tmpModMass
                ntRes = tmpModString
            End If
        Next

        sb.Append(Format(ctModMass, "0.000000"))
        sb.Append(" ")
        sb.Append(Format(ntModMass, "0.000000"))

        Return sb.ToString.Trim()
    End Function


#End Region

End Class



Public Class clsDynamicMods
    Inherits clsMods
    '#Region " Public Legacy Functions and Properties "
    '    Public Property Dyn_Mod_1_MassDiff() As Single
    '        Get
    '            Return Me.Dyn_Mod_n_MassDiff(1)
    '        End Get
    '        Set(ByVal Value As Single)
    '            Dyn_Mod_n_MassDiff(1) = Value
    '        End Set
    '    End Property
    '    Public Property Dyn_Mod_2_MassDiff() As Single
    '        Get
    '            Return Me.Dyn_Mod_n_MassDiff(2)
    '        End Get
    '        Set(ByVal Value As Single)
    '            Dyn_Mod_n_MassDiff(2) = Value
    '        End Set
    '    End Property
    '    Public Property Dyn_Mod_3_MassDiff() As Single
    '        Get
    '            Return Me.Dyn_Mod_n_MassDiff(3)
    '        End Get
    '        Set(ByVal Value As Single)
    '            Dyn_Mod_n_MassDiff(3) = Value
    '        End Set
    '    End Property

    '    Public Property Dyn_Mod_1_AAList() As String
    '        Get
    '            Return Me.Dyn_Mod_n_AAList(1)
    '        End Get
    '        Set(ByVal Value As String)
    '            Dyn_Mod_n_AAList(1) = Value
    '        End Set
    '    End Property
    '    Public Property Dyn_Mod_2_AAList() As String
    '        Get
    '            Return Me.Dyn_Mod_n_AAList(2)
    '        End Get
    '        Set(ByVal Value As String)
    '            Dyn_Mod_n_AAList(2) = Value
    '        End Set
    '    End Property
    '    Public Property Dyn_Mod_3_AAList() As String
    '        Get
    '            Return Me.Dyn_Mod_n_AAList(3)
    '        End Get
    '        Set(ByVal Value As String)
    '            Dyn_Mod_n_AAList(3) = Value
    '        End Set
    '    End Property
    '#End Region

#Region " Public Procedures "

    Public Sub New(ByVal DynamicModString As String)
        MyBase.New()
        m_OrigDynModString = DynamicModString
        ParseDynModString(m_OrigDynModString)
    End Sub

    Public Sub New()
        MyBase.New()
        m_OrigDynModString = Nothing
    End Sub

    Public Function ReturnDynModString(ByVal maxDynMods As Integer) As String
        Dim s As String
        'If Initialized Then
        s = AssembleModString(maxDynMods)
        Return s
        'Else
        '    Return ""
        'End If
    End Function
    'TODO replace with real function for term dyn mods
    'Just a placeholder for now
    Public Function ReturnDynTermModString() As String
        Return "0.0000 0.0000"
    End Function
    Public Overloads Sub Add( _
        ByVal AffectedResidueString As String, _
        ByVal MassDifference As Single, _
        Optional ByVal GlobalModID As Integer = 0)

        Dim sc As StringCollection = Me.ConvertAffectedResStringToSC(AffectedResidueString)
        Dim newDynMod As New clsModEntry(sc, MassDifference, clsModEntry.ModificationTypes.Dynamic)
        Me.List.Add(newDynMod)
    End Sub
    Public Overloads Sub Add(ByVal ModToAdd As clsModEntry)
        Me.List.Add(ModToAdd)
    End Sub

    Public Property Dyn_Mod_n_MassDiff(ByVal DynModNumber As Integer) As Single
        Get
            Dim dm As clsModEntry
            Dim index As Integer = DynModNumber - 1
            Try
                dm = DirectCast(Me.List.Item(index), clsModEntry)
            Catch ex As Exception
                dm = New clsModEntry(Me.ConvertAffectedResStringToSC("C"), 0.0, clsModEntry.ModificationTypes.Dynamic)
            End Try
            Return dm.MassDifference
        End Get
        Set(ByVal Value As Single)
            Dim index As Integer = DynModNumber - 1
            Dim dm As clsModEntry
            If index <= Me.List.Count - 1 Then
                dm = DirectCast(Me.List.Item(index), clsModEntry)
                dm.MassDifference = Value
                Me.Replace(index, dm)
            Else
                Me.Add("C", Value)
            End If
        End Set
    End Property
    Public Property Dyn_Mod_n_AAList(ByVal DynModNumber As Integer) As String
        Get
            Dim dm As clsModEntry
            Dim index As Integer = DynModNumber - 1
            Try
                dm = DirectCast(Me.List.Item(index), clsModEntry)
            Catch ex As Exception
                dm = New clsModEntry(ConvertAffectedResStringToSC("C"), 0.0, clsModEntry.ModificationTypes.Dynamic)
            End Try
            Return dm.ReturnAllAffectedResiduesString
        End Get
        Set(ByVal Value As String)
            Dim index As Integer = DynModNumber - 1
            Dim dm As clsModEntry
            If index <= Me.List.Count - 1 Then
                dm = DirectCast(Me.List.Item(index), clsModEntry)
                dm.ResidueCollection = Me.ConvertAffectedResStringToSC(Value)
                Me.Replace(index, dm)
            Else
                Me.Add(Value, CSng(0.0))
            End If
        End Set
    End Property

    Public Property Dyn_Mod_n_Global_ModID(ByVal DynModNumber As Integer) As Integer
        Get
            Dim dm As clsModEntry
            Dim index As Integer = DynModNumber - 1
            Try
                dm = DirectCast(Me.List.Item(index), clsModEntry)
            Catch ex As Exception
                dm = New clsModEntry(Me.ConvertAffectedResStringToSC("C"), 0.0, clsModEntry.ModificationTypes.Dynamic)
            End Try
            Return dm.GlobalModID
        End Get
        Set(ByVal Value As Integer)
            Dim index As Integer = DynModNumber - 1
            Dim dm As clsModEntry
            If index <= Me.List.Count - 1 Then
                dm = DirectCast(Me.List.Item(index), clsModEntry)
                dm.GlobalModID = Value
                Me.Replace(index, dm)
            Else
                Me.Add("C", 0.0, Value)
            End If
        End Set
    End Property
#End Region

#Region " Member Procedures "
    Protected Overridable Function AssembleModString(ByVal counter As Integer) As String
        Dim s As String = ""
        Dim tmpModString As String
        Dim tmpModMass As Single
        Dim dynMod As clsModEntry
        Dim padCount As Integer

        For Each dynMod In Me.List
            tmpModMass = dynMod.MassDifference
            tmpModString = dynMod.ReturnAllAffectedResiduesString
            s = s & Format(tmpModMass, "0.0000") & " " & tmpModString & " "
            counter -= 1
        Next

        For padCount = 0 To counter - 1
            If padCount <= 2 Then
                s = s & "0.0000 C "
            Else
                s = s & "0.0000 X "
            End If
        Next

        Return s.Trim()
    End Function
    Protected Overridable Sub ParseDynModString(ByVal DMString As String)
        Dim sc As StringCollection
        Dim tmpResString As String
        Dim tmpRes As String
        Dim resCounter As Integer
        Dim tmpMass As Single

        Dim splitRE As System.Text.RegularExpressions.Regex = _
            New System.Text.RegularExpressions.Regex("(?<modmass>\d+\.\d+)\s+(?<residues>[A-Za-z]+)")
        Dim matches As System.Text.RegularExpressions.MatchCollection
        matches = splitRE.Matches(DMString)
        Dim m As System.Text.RegularExpressions.Match

        For Each m In matches
            tmpMass = CSng(m.Groups("modmass").Value)
            tmpResString = m.Groups("residues").ToString
            If tmpMass <> 0 Then
                sc = New StringCollection
                For resCounter = 1 To Len(tmpResString)
                    tmpRes = Mid(tmpResString, resCounter, 1)
                    sc.Add(tmpRes)
                Next
                Dim modEntry As New clsModEntry(sc, tmpMass, clsModEntry.ModificationTypes.Dynamic)
                Me.Add(modEntry)
            End If
        Next

    End Sub
#End Region

#Region " Member Properties "
    Protected m_OrigDynModString As String
    'private m_EmptyMod as New clsModEntry(
#End Region

End Class



Public Class clsStaticMods
    Inherits clsMods

#Region " Legacy Access "
    Public Property CtermPeptide() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.C_Term_Peptide).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsStaticMods.ResidueCode.C_Term_Peptide, Value)
        End Set
    End Property
    Public Property CtermProtein() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.C_Term_Protein).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.C_Term_Protein, Value)
        End Set
    End Property
    Public Property NtermPeptide() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.N_Term_Peptide).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.N_Term_Peptide, Value)
        End Set
    End Property
    Public Property NtermProtein() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.N_Term_Protein).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.N_Term_Protein, Value)
        End Set
    End Property

    Public Property G_Glycine() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.G_Glycine).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.G_Glycine, Value)
        End Set
    End Property
    Public Property A_Alanine() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.A_Alanine).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.A_Alanine, Value)
        End Set
    End Property
    Public Property S_Serine() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.S_Serine).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.S_Serine, Value)
        End Set
    End Property
    Public Property P_Proline() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.P_Proline).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.P_Proline, Value)
        End Set
    End Property
    Public Property V_Valine() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.V_Valine).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.V_Valine, Value)
        End Set
    End Property
    Public Property T_Threonine() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.T_Threonine).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.T_Threonine, Value)
        End Set
    End Property
    Public Property C_Cysteine() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.C_Cysteine).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.C_Cysteine, Value)
        End Set
    End Property
    Public Property L_Leucine() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.L_Leucine).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.L_Leucine, Value)
        End Set
    End Property
    Public Property I_Isoleucine() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.I_Isoleucine).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.I_Isoleucine, Value)
        End Set
    End Property
    Public Property X_LorI() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.X_LorI).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.X_LorI, Value)
        End Set
    End Property
    Public Property N_Asparagine() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.N_Asparagine).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.N_Asparagine, Value)
        End Set
    End Property
    Public Property O_Ornithine() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.O_Ornithine).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.O_Ornithine, Value)
        End Set
    End Property
    Public Property B_avg_NandD() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.B_avg_NandD).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.B_avg_NandD, Value)
        End Set
    End Property
    Public Property D_Aspartic_Acid() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.D_Aspartic_Acid).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.D_Aspartic_Acid, Value)
        End Set
    End Property
    Public Property Q_Glutamine() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.Q_Glutamine).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.Q_Glutamine, Value)
        End Set
    End Property
    Public Property K_Lysine() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.K_Lysine).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.K_Lysine, Value)
        End Set
    End Property
    Public Property Z_avg_QandE() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.Z_avg_QandE).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.Z_avg_QandE, Value)
        End Set
    End Property
    Public Property E_Glutamic_Acid() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.E_Glutamic_Acid).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.E_Glutamic_Acid, Value)
        End Set
    End Property
    Public Property M_Methionine() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.M_Methionine).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.M_Methionine, Value)
        End Set
    End Property
    Public Property H_Histidine() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.H_Histidine).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.H_Histidine, Value)
        End Set
    End Property
    Public Property F_Phenylalanine() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.F_Phenylalanine).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.F_Phenylalanine, Value)
        End Set
    End Property
    Public Property R_Arginine() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.R_Arginine).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.R_Arginine, Value)
        End Set
    End Property
    Public Property Y_Tyrosine() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.Y_Tyrosine).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.Y_Tyrosine, Value)
        End Set
    End Property
    Public Property W_Tryptophan() As Single
        Get
            Return Me.FindAAMod(clsMods.ResidueCode.W_Tryptophan).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeAAMod(clsMods.ResidueCode.W_Tryptophan, Value)
        End Set
    End Property

#End Region

#Region " Public Procedures "
    Public Sub New()
        MyBase.New()
    End Sub

    Public Function GetResidue(ByVal index As Integer) As String
        Dim m As clsModEntry = DirectCast(Me.List.Item(index), clsModEntry)
        Return m.ReturnResidueAffected(0)
    End Function
    Public Sub ChangeAAModification(ByVal ModifiedAA As ResidueCode, ByVal MassDifference As Single, Optional ByVal Additive As Boolean = False)
        Me.ChangeAAMod(ModifiedAA, MassDifference, Additive)
    End Sub
    Public Sub EradicateEmptyMods()
        Me.KillBlankMods()
    End Sub
#End Region

#Region " Member Procedures "
    Private Function FindAAMod(ByVal ModifiedAA As ResidueCode) As clsModEntry
        Return Me.m_FindMod(Me.ConvertResidueCodeToSLC(ModifiedAA))
    End Function

    Private Sub ChangeAAMod(ByVal ModifiedAA As ResidueCode, ByVal MassDifference As Single, Optional ByVal Additive As Boolean = False)
        Dim foundMod As clsModEntry = FindAAMod(ModifiedAA)
        Dim ModAAString As String = Me.ConvertResidueCodeToSLC(ModifiedAA)
        Me.m_ChangeMod(foundMod, ModAAString, MassDifference, Additive)

    End Sub

    Private Sub KillBlankMods()
        Dim AA As String
        Dim AASLC As String
        Dim AAEnums() As String
        AAEnums = System.Enum.GetNames(GetType(ResidueCode))
        Dim currIndex As Integer
        Dim modEntry As clsModEntry

        For Each AA In AAEnums
            If InStr(AA, "Term") = 0 Then
                AASLC = Left(AA, 1)
                currIndex = m_FindModIndex(AASLC)
                If currIndex <> -1 Then
                    modEntry = Me.GetModEntry(currIndex)
                    If modEntry.MassDifference = 0.0 Then
                        Me.List.Remove(modEntry)
                    End If
                End If
            End If
        Next
    End Sub

#End Region

End Class




Public Class clsIsoMods
    Inherits clsMods


    Public Overloads Sub Add(ByVal AffectedAtom As IsotopeList, ByVal MassDifference As Single, Optional ByVal GlobalModID As Integer = 0)
        Me.m_Add(AffectedAtom.ToString, MassDifference, clsModEntry.ModificationTypes.Isotopic, GlobalModID)
    End Sub

#Region " Public Procedures "
    Public Function GetAtom(ByVal index As Integer) As String
        Dim m As clsModEntry = DirectCast(Me.List.Item(index), clsModEntry)
        Return m.ReturnResidueAffected(0)
    End Function
#End Region

#Region " Legacy Access "
    Public Property Iso_C() As Single
        Get
            Return Me.FindIsoMod(IsotopeList.C).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeIsoMod(clsMods.IsotopeList.C, Value)
        End Set
    End Property

    Public Property Iso_H() As Single
        Get
            Return Me.FindIsoMod(IsotopeList.H).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeIsoMod(clsMods.IsotopeList.H, Value)
        End Set
    End Property

    Public Property Iso_O() As Single
        Get
            Return Me.FindIsoMod(IsotopeList.O).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeIsoMod(clsMods.IsotopeList.O, Value)
        End Set
    End Property

    Public Property Iso_N() As Single
        Get
            Return Me.FindIsoMod(IsotopeList.N).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeIsoMod(clsMods.IsotopeList.N, Value)
        End Set
    End Property

    Public Property Iso_S() As Single
        Get
            Return Me.FindIsoMod(IsotopeList.S).MassDifference
        End Get
        Set(ByVal Value As Single)
            ChangeIsoMod(clsMods.IsotopeList.S, Value)
        End Set
    End Property

#End Region

    Private Sub ChangeIsoMod(ByVal AffectedAtom As IsotopeList, ByVal MassDifference As Single, Optional ByVal Additive As Boolean = False)
        Dim foundMod As clsModEntry = FindIsoMod(AffectedAtom)
        Dim ModAAString As String = AffectedAtom.ToString
        Me.m_ChangeMod(foundMod, ModAAString, MassDifference)
    End Sub

    Private Function FindIsoMod(ByVal AffectedAtom As IsotopeList) As clsModEntry
        Return Me.m_FindMod(AffectedAtom.ToString)
    End Function

End Class