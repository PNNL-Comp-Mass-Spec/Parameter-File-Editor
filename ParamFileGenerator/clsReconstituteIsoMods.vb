Imports System.Collections.Generic
Imports PRISMDatabaseUtils

Public Interface IReconstituteIsoMods

    Function ReconstituteIsoMods(ParamsClass As clsParams) As clsParams

End Interface

Public Class clsReconstituteIsoMods
    Implements IReconstituteIsoMods
    Public Enum AvailableAtoms
        N
        C
        H
        O
        S
    End Enum

    ''' <summary>
    ''' Dictionary where keys are amino acid residue (one letter abbreviation)
    ''' and values are a dictionary with atom counts (number of C, H, N, O, and S atoms)
    ''' </summary>
    Private ReadOnly m_ResidueAtomCounts As Dictionary(Of Char, Dictionary(Of Char, Integer))

#Disable Warning BC40028 ' Type of parameter is not CLS-compliant
    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="dbTools"></param>
    Public Sub New(dbTools As IDBTools)
        Dim getResTable As New clsGetResiduesList(dbTools)
        m_ResidueAtomCounts = getResTable.ResidueAtomCounts
    End Sub
#Enable Warning BC40028 ' Type of parameter is not CLS-compliant

    Friend Function ReconIsoMods(ParamsClass As clsParams) As clsParams Implements IReconstituteIsoMods.ReconstituteIsoMods
        Return StreamlineIsoModsToStatics(ParamsClass, ParamsClass.IsotopicMods)
    End Function

    Protected Function GetMultiplier(AA As Char, Atom As AvailableAtoms) As Integer

        Dim atomCounts As Dictionary(Of Char, Integer) = Nothing
        If m_ResidueAtomCounts.TryGetValue(AA, atomCounts) Then
            Dim atomSymbol = Atom.ToString().Chars(0)

            Dim atomCount As Integer
            If atomCounts.TryGetValue(atomSymbol, atomCount) Then
                Return atomCount
            End If
        End If

        Return 0

    End Function

    Protected Function StreamlineIsoModsToStatics(
        ParamsClass As clsParams,
        IsoMods As clsIsoMods) As clsParams

        Dim im As clsModEntry

        Dim tmpAtom As String
        Dim tmpIsoMass As Double

        Dim AAEnums() As String = [Enum].GetNames(GetType(clsMods.ResidueCode))
        Dim tmpAA As String


        For Each im In IsoMods
            tmpAtom = im.ReturnResidueAffected(0)
            tmpIsoMass = im.MassDifference

            For Each tmpAA In AAEnums
                If InStr(tmpAA, "Term") = 0 Then
                    Dim tmpAASLC = Left(tmpAA, 1).Chars(0)
                    Dim tmpAtomCount = GetMultiplier(tmpAASLC, DirectCast([Enum].Parse(GetType(AvailableAtoms), tmpAtom), AvailableAtoms))
                    ParamsClass.StaticModificationsList.ChangeAAModification(
                        DirectCast([Enum].Parse(GetType(clsMods.ResidueCode), tmpAA), clsMods.ResidueCode),
                        tmpIsoMass * tmpAtomCount, True)
                End If
            Next

        Next

        Return ParamsClass

    End Function
End Class
