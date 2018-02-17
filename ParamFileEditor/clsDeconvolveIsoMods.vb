Public Interface IDeconvolveIsoMods

    Function DeriveIsoMods(ParamsClass As ParamFileGenerator.clsParams) As ParamFileGenerator.clsParams


End Interface

Public Class clsDeconvolveIsoMods
    Inherits ParamFileGenerator.clsReconstitueIsoMods
    Implements IDeconvolveIsoMods

    Private Const allowedDifference As Single = 0.05

    Private ReadOnly m_ConnectionString As String

    Public Sub New(connectionString As String)
        MyBase.New(connectionString)
        Me.m_ConnectionString = connectionString

    End Sub

    Friend Function DeriveIsoMods(ParamsClass As ParamFileGenerator.clsParams) As ParamFileGenerator.clsParams Implements IDeconvolveIsoMods.DeriveIsoMods
        'Get all members of the static mods list
        If ParamsClass.StaticModificationsList.Count = 0 Then Return ParamsClass

        Dim modEntry As ParamFileGenerator.clsModEntry
        Dim sm As ParamFileGenerator.clsStaticMods = ParamsClass.StaticModificationsList

        Dim atomEnums() As String = System.Enum.GetNames(GetType(AvailableAtoms))
        Dim atom As String

        Dim imCollection As New Hashtable
        Dim tmpAA As String
        Dim tmpIsoMod As Single
        Dim matchCount As Integer
        Dim maxMatchcount As Integer
        'Dim isoMatch As Boolean
        Dim maxIsoAtom As AvailableAtoms
        Dim usedIsoMod As Single
        Dim maxIsoMod As Single
        Dim maxMassCorrectionID As Integer

        For Each atom In atomEnums

            Dim eAtom = CType(System.Enum.Parse(GetType(AvailableAtoms), atom), AvailableAtoms)

            For Each modEntry In sm

                tmpAA = modEntry.ReturnResidueAffected(0)

                If tmpAA.Length = 1 Then
                    tmpIsoMod = getIsoModValue(
                        tmpAA,
                        eAtom,
                        CSng(modEntry.MassDifference))
                    imCollection.Add(tmpAA, tmpIsoMod)
                End If
            Next modEntry
            matchCount = CorrelateIsoMods(imCollection, usedIsoMod)
            If matchCount > maxMatchcount Then
                maxMatchcount = matchCount
                maxIsoAtom = eAtom
                maxIsoMod = CSng(Math.Round(usedIsoMod, 4))
            End If

            imCollection.Clear()
        Next atom

        If maxMatchcount >= 5 Then
            'Rip out the existing version...
            Me.StripStaticIsoMod(ParamsClass, maxIsoMod, maxIsoAtom)
            '...and replace it with the new isomod entity
            Dim at As IMassTweaker
            at = New clsMassTweaker(Me.m_ConnectionString)
            maxIsoMod = CSng(at.GetTweakedMass(maxIsoMod, maxIsoAtom.ToString()))
            maxMassCorrectionID = at.TweakedModID

            Dim eIsotope As ParamFileGenerator.clsIsoMods.IsotopeList
            [Enum].TryParse(maxIsoAtom.ToString(), eIsotope)

            ParamsClass.IsotopicMods.Add(eIsotope, maxIsoMod, maxMassCorrectionID)
        End If

        Return ParamsClass

    End Function

    Private Function getIsoModValue(AA As String, Atom As AvailableAtoms, StaticModMass As Single) As Single
        Dim m_PossibleIsoMod As Single

        m_PossibleIsoMod = CSng(StaticModMass / CDbl(getMultiplier(AA, Atom)))

        Return m_PossibleIsoMod
    End Function

    Private Function CorrelateIsoMods(IsoModCollection As IDictionary, ByRef commonMass As Single) As Integer
        Dim stepper As IDictionaryEnumerator = IsoModCollection.GetEnumerator
        Dim matchCount As Integer
        Dim currIsoMod As Single
        Dim lastIsoMod As Single = 0


        If IsoModCollection.Count = 0 Then Return 0

        Do While stepper.MoveNext = True
            currIsoMod = CSng(stepper.Value)
            If Math.Abs(currIsoMod - lastIsoMod) <= allowedDifference Then
                matchCount += 1
                commonMass += currIsoMod
                lastIsoMod = currIsoMod
            Else
            End If
            If Math.Abs(lastIsoMod) < Single.Epsilon Then
                lastIsoMod = currIsoMod
            End If
            'stepper.MoveNext()
        Loop

        commonMass = commonMass / matchCount
        Return matchCount

    End Function

    Private Sub StripStaticIsoMod(
        ByRef Paramsclass As ParamFileGenerator.clsParams,
        ModMassToRemove As Single,
        Atom As AvailableAtoms)

        Dim entry As ParamFileGenerator.clsModEntry

        Dim AA As String
        Dim NumAtoms As Integer


        For Each entry In Paramsclass.StaticModificationsList
            AA = entry.ReturnResidueAffected(0)
            NumAtoms = CInt(getMultiplier(AA, Atom))
            If entry.MassDifference > 0.0 Then
                entry.MassDifference = CSng(Math.Round(entry.MassDifference - (ModMassToRemove * NumAtoms), 4))
            End If
            If (Math.Abs(entry.MassDifference) < allowedDifference) Or (entry.MassDifference < 0) Then
                entry.MassDifference = 0
            End If
        Next
        Paramsclass.StaticModificationsList.EradicateEmptyMods()

    End Sub

End Class
