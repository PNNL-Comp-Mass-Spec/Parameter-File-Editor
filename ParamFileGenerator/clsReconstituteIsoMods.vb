
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


    Private ReadOnly m_ResTable As DataTable

    Public Sub New(connectionString As String)
        Dim getResTable As New clsGetResiduesList(connectionString)
        m_ResTable = getResTable.ResiduesTable
        ' Unused: m_ConnectionString = connectionString

    End Sub

    Friend Function ReconIsoMods(ParamsClass As clsParams) As clsParams Implements IReconstituteIsoMods.ReconstitueIsoMods
        Return StreamlineIsoModsToStatics(ParamsClass, ParamsClass.IsotopicMods)
    End Function
    Protected Function getMultiplier(AA As String, Atom As AvailableAtoms) As Integer

        Dim m_Atomrows() As DataRow = m_ResTable.Select("[Residue_Symbol] = '" & AA & "'")
        Dim m_AtomRow As DataRow = m_Atomrows(0)
        Dim m_AtomCount = CInt(m_AtomRow.Item(getAtomCountColumn(Atom)))

        Return m_AtomCount

    End Function

    Protected Function getAtomCountColumn(Atom As AvailableAtoms) As String

        Select Case Atom
            Case AvailableAtoms.C
                Return "Num_C"
            Case AvailableAtoms.H
                Return "Num_H"
            Case AvailableAtoms.O
                Return "Num_O"
            Case AvailableAtoms.N
                Return "Num_N"
            Case AvailableAtoms.S
                Return "Num_S"
            Case Else
                Return Nothing
        End Select

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
