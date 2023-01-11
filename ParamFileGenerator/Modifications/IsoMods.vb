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
