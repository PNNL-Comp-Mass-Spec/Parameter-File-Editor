Imports ParamFileGenerator.Modifications
Imports ParamFileGenerator.Parameters
Imports PRISMDatabaseUtils

Public Interface IDeconvolveIsoMods

    Function DeriveIsoMods(ParamsClass As Params) As Params

End Interface

Public Class clsDeconvolveIsoMods
    Inherits ParamFileGenerator.ReconstituteIsoMods
    Implements IDeconvolveIsoMods

    Private Const allowedDifference As Single = 0.05

    Private ReadOnly m_DBTools As IDBTools

#Disable Warning BC40028 ' Type of parameter is not CLS-compliant
    Public Sub New(dbTools As IDBTools)
#Enable Warning BC40028 ' Type of parameter is not CLS-compliant

        MyBase.New(dbTools)
        m_DBTools = dbTools

    End Sub

    Friend Function DeriveIsoMods(ParamsClass As Params) As Params Implements IDeconvolveIsoMods.DeriveIsoMods
        'Get all members of the static mods list
        If ParamsClass.StaticModificationsList.Count = 0 Then Return ParamsClass

        Dim modEntry As ModEntry
        Dim sm As StaticMods = ParamsClass.StaticModificationsList

        Dim atomEnums() As String = System.Enum.GetNames(GetType(AvailableAtoms))
        Dim atom As String

        Dim imCollection As New Hashtable
        Dim tmpAA As String
        Dim tmpIsoMod As Single
        Dim matchCount As Integer
        Dim maxMatchCount As Integer
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
                        tmpAA.Chars(0),
                        eAtom,
                        CSng(modEntry.MassDifference))
                    imCollection.Add(tmpAA, tmpIsoMod)
                End If
            Next modEntry
            matchCount = CorrelateIsoMods(imCollection, usedIsoMod)
            If matchCount > maxMatchCount Then
                maxMatchCount = matchCount
                maxIsoAtom = eAtom
                maxIsoMod = CSng(Math.Round(usedIsoMod, 4))
            End If

            imCollection.Clear()
        Next atom

        If maxMatchCount >= 5 Then
            'Rip out the existing version...
            Me.StripStaticIsoMod(ParamsClass, maxIsoMod, maxIsoAtom)
            '...and replace it with the new isoMod entity
            Dim at As IMassTweaker
            at = New clsMassTweaker(m_DBTools)
            maxIsoMod = CSng(at.GetTweakedMass(maxIsoMod, maxIsoAtom.ToString()))
            maxMassCorrectionID = at.TweakedModID

            Dim eIsotope As IsoMods.IsotopeList
            [Enum].TryParse(maxIsoAtom.ToString(), eIsotope)

            ParamsClass.IsotopicModificationsList.Add(eIsotope, maxIsoMod, maxMassCorrectionID)
        End If

        Return ParamsClass

    End Function

    Private Function getIsoModValue(AA As Char, Atom As AvailableAtoms, StaticModMass As Single) As Single
        Dim m_PossibleIsoMod As Single

        m_PossibleIsoMod = CSng(StaticModMass / CDbl(GetMultiplier(AA, Atom)))

        Return m_PossibleIsoMod
    End Function

    Private Function CorrelateIsoMods(IsoModCollection As IDictionary, ByRef commonMass As Single) As Integer
        Dim stepper As IDictionaryEnumerator = IsoModCollection.GetEnumerator
        Dim matchCount As Integer
        Dim currentIsoMod As Single
        Dim lastIsoMod As Single = 0


        If IsoModCollection.Count = 0 Then Return 0

        Do While stepper.MoveNext = True
            currentIsoMod = CSng(stepper.Value)
            If Math.Abs(currentIsoMod - lastIsoMod) <= allowedDifference Then
                matchCount += 1
                commonMass += currentIsoMod
                lastIsoMod = currentIsoMod
            Else
            End If
            If Math.Abs(lastIsoMod) < Single.Epsilon Then
                lastIsoMod = currentIsoMod
            End If
            'stepper.MoveNext()
        Loop

        commonMass = commonMass / matchCount
        Return matchCount

    End Function

    Private Sub StripStaticIsoMod(
        ByRef paramsClass As Params,
        modMassToRemove As Single,
        atom As AvailableAtoms)

        Dim entry As ModEntry

        For Each entry In paramsClass.StaticModificationsList
            Dim AA = entry.ReturnResidueAffected(0).Chars(0)
            Dim NumAtoms = GetMultiplier(AA, atom)
            If entry.MassDifference > 0.0 Then
                entry.MassDifference = CSng(Math.Round(entry.MassDifference - (modMassToRemove * NumAtoms), 4))
            End If
            If (Math.Abs(entry.MassDifference) < allowedDifference) Or (entry.MassDifference < 0) Then
                entry.MassDifference = 0
            End If
        Next
        paramsClass.StaticModificationsList.EradicateEmptyMods()

    End Sub

End Class
