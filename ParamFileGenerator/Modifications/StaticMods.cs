Imports System.Linq

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
