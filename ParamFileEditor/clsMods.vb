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

#End Region

#Region " Public Properties "
    Public ReadOnly Property ModCount() As Integer
        Get
            Return Me.List.Count
        End Get
    End Property
    Friend ReadOnly Property Initialized() As Boolean
        Get
            If Me.List.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
#End Region

#Region " Friend Procedures "
    Friend Sub New()
        MyBase.New()
        Me.LoadAAMappingColl()
    End Sub

    Friend Sub Add(ByVal AffectedResidue As ResidueCode, ByVal MassDifference As Single)
        Dim tmpAAString As String = Me.ConvertResidueCodeToSLC(AffectedResidue)
        Dim sc As StringCollection = Me.ConvertAffectedResStringToSC(tmpAAString)
        Dim newMod As New clsModEntry(sc, MassDifference)
        Me.List.Add(newMod)
    End Sub
    Friend Sub Add(ByVal AffectedResidueString As String, ByVal MassDifference As Single)
        Dim sc As StringCollection = Me.ConvertAffectedResStringToSC(AffectedResidueString)
        Dim newMod As New clsModEntry(sc, MassDifference)
        Me.List.Add(newMod)
    End Sub

    Friend Sub Insert(ByVal index As Integer, ByVal newMod As clsModEntry)
        Me.List.Insert(index, newMod)
    End Sub
    Friend Sub Remove(ByVal index As Integer)
        Me.List.RemoveAt(index)
    End Sub
    Friend Sub Replace(ByVal index As Integer, ByVal newMod As clsModEntry)
        Me.List.RemoveAt(index)
        Me.List.Insert(index, newMod)
    End Sub

#End Region

#Region " Member Properties "
    Protected m_AAMappingTable As NameValueCollection

#End Region

#Region " Member Procedures "
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

#End Region






    Class clsModEntry

#Region " Member Properties "
        Private m_ResiduesAffected As StringCollection
        Private m_MassDiff As Single
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
            Dim returnString As String
            For Each s In resCollection
                s = Left(s, 1)
                returnString = returnString & s
            Next
            Return returnString
        End Function
#End Region

#Region " Public Procedures "
        Public Sub New(ByVal AffectedResidueList As StringCollection, ByVal MassDifference As Single)
            Me.m_ResiduesAffected = AffectedResidueList
            Me.m_MassDiff = MassDifference
        End Sub
        Public Sub AddAffectedResidue(ByVal ResidueToAdd As String)
            Me.AddResidue(ResidueToAdd)
        End Sub
        Public Sub RemoveAffectedResidue(ByVal ResidueToRemove As String)
            Me.RemoveResidue(ResidueToRemove)
        End Sub
#End Region

    End Class

End Class






Public Class clsDynamicMods
    Inherits clsMods
#Region " Public Legacy Functions and Properties "
    Public Property Dyn_Mod_1_MassDiff() As Single
        Get
            Return Me.Dyn_Mod_n_MassDiff(1)
        End Get
        Set(ByVal Value As Single)
            Dyn_Mod_n_MassDiff(1) = Value
        End Set
    End Property
    Public Property Dyn_Mod_2_MassDiff() As Single
        Get
            Return Me.Dyn_Mod_n_MassDiff(2)
        End Get
        Set(ByVal Value As Single)
            Dyn_Mod_n_MassDiff(2) = Value
        End Set
    End Property
    Public Property Dyn_Mod_3_MassDiff() As Single
        Get
            Return Me.Dyn_Mod_n_MassDiff(3)
        End Get
        Set(ByVal Value As Single)
            Dyn_Mod_n_MassDiff(3) = Value
        End Set
    End Property

    Public Property Dyn_Mod_1_AAList() As String
        Get
            Return Me.Dyn_Mod_n_AAList(1)
        End Get
        Set(ByVal Value As String)
            Dyn_Mod_n_AAList(1) = Value
        End Set
    End Property
    Public Property Dyn_Mod_2_AAList() As String
        Get
            Return Me.Dyn_Mod_n_AAList(2)
        End Get
        Set(ByVal Value As String)
            Dyn_Mod_n_AAList(2) = Value
        End Set
    End Property
    Public Property Dyn_Mod_3_AAList() As String
        Get
            Return Me.Dyn_Mod_n_AAList(3)
        End Get
        Set(ByVal Value As String)
            Dyn_Mod_n_AAList(3) = Value
        End Set
    End Property
#End Region

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

    Public Function ReturnDynModString() As String
        Dim s As String
        'If Initialized Then
        s = AssembleModString()
        Return s
        'Else
        '    Return ""
        'End If
    End Function
    Public Overloads Sub Add(ByVal AffectedResidueString As String, ByVal MassDifference As Single)
        Dim sc As StringCollection = Me.ConvertAffectedResStringToSC(AffectedResidueString)
        Dim newDynMod As New clsModEntry(sc, MassDifference)
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
                dm = Me.List.Item(index)
            Catch ex As Exception
                dm = New clsModEntry(Me.ConvertAffectedResStringToSC("C"), 0.0)
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
                dm = Me.List.Item(index)
            Catch ex As Exception
                dm = New clsModEntry(ConvertAffectedResStringToSC("C"), 0.0)
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
#End Region

#Region " Member Procedures "
    Private Function AssembleModString() As String
        Dim s As String
        Dim tmpModString As String
        Dim tmpModRes As String
        Dim tmpModMass As Single
        Dim dynMod As clsModEntry
        Dim counter As Integer = 2
        Dim padCount As Integer

        For Each dynMod In Me.List
            tmpModMass = dynMod.MassDifference
            tmpModString = dynMod.ReturnAllAffectedResiduesString
            s = s & Format(tmpModMass, "0.0000") & " " & tmpModString & " "
            counter -= 1
        Next

        For padCount = 0 To counter
            s = s & "0.0000 C "
        Next

        Return s.Trim()
    End Function
    Private Sub ParseDynModString(ByVal DMString As String)
        Dim tmpsplit() As String = DMString.Split(" ")
        Dim counter As Integer
        Dim maxCount As Integer = UBound(tmpsplit)
        Dim sc As StringCollection
        Dim tmpResString As String
        Dim tmpRes As String
        Dim resCounter As Integer
        Dim tmpMass As Single

        For counter = 0 To maxCount Step 2
            tmpMass = CSng(tmpsplit(counter))
            tmpResString = tmpsplit(counter + 1)
            If tmpMass > 0 Then
                sc = New StringCollection
                For resCounter = 1 To Len(tmpResString)
                    tmpRes = Mid(tmpResString, resCounter, 1)
                    sc.Add(tmpRes)
                Next
                Dim modEntry As New clsModEntry(sc, tmpMass)
                Me.Add(modEntry)
            End If
        Next

        'Console.WriteLine(AssembleModString())
    End Sub
#End Region

#Region " Public Properties "


#End Region

#Region " Member Properties "
    Private m_OrigDynModString As String
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
            ChangeAAMod(Me.ResidueCode.C_Term_Peptide, Value)
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
    Public Function GetMassDiff(ByVal index As Integer) As String
        Dim m As clsModEntry = DirectCast(Me.List.Item(index), clsModEntry)
        Return Format(m.MassDifference, "0.0000")
    End Function
#End Region

#Region " Member Procedures "
    Private Function FindAAMod(ByVal ModifiedAA As ResidueCode) As clsModEntry
        Dim tmpAA As String = Me.ConvertResidueCodeToSLC(ModifiedAA)
        Dim statMod As clsModEntry
        Dim affRes As StringCollection
        Dim testAA As String

        For Each statMod In Me.List
            affRes = statMod.ReturnAllAffectedResidues
            For Each testAA In affRes
                If testAA = tmpAA Then
                    Return statMod
                End If
            Next
        Next
        Dim sc As StringCollection = Me.ConvertAffectedResStringToSC(tmpAA)
        Dim emptyMod As New clsModEntry(sc, 0.0)
        Return emptyMod
    End Function
    Private Function FindAAModIndex(ByVal ModifiedAA As ResidueCode) As Integer
        Dim statMod As clsModEntry = FindAAMod(ModifiedAA)

    End Function
    Private Sub ChangeAAMod(ByVal ModifiedAA As ResidueCode, ByVal MassDifference As Single)
        Dim foundMod As clsModEntry = FindAAMod(ModifiedAA)
        If foundMod.MassDifference = 0.0 And MassDifference <> 0.0 Then
            Me.Add(ModifiedAA, MassDifference)
            Exit Sub
        ElseIf foundMod.MassDifference = 0.0 And MassDifference = 0.0 Then
            Exit Sub
        ElseIf foundMod.MassDifference <> 0.0 Then          'Not an emptyMod
            Dim counter As Integer
            Dim tempMod As clsModEntry
            Dim modAAString As String = Me.ConvertResidueCodeToSLC(ModifiedAA)
            Dim sc As StringCollection = Me.ConvertAffectedResStringToSC(modAAString)
            Dim changeMod As New clsModEntry(sc, MassDifference)

            For Each tempMod In Me.List
                If foundMod.Equals(tempMod) And MassDifference <> 0.0 Then
                    Me.Replace(counter, changeMod)
                    Exit Sub
                ElseIf foundMod.Equals(tempMod) And MassDifference = 0.0 Then
                    Me.RemoveAt(counter)
                End If
                counter += 1
            Next

        End If



    End Sub
#End Region

End Class