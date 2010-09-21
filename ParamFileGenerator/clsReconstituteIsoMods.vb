Public Interface IReconstituteIsoMods

    Function ReconstitueIsoMods(ByVal ParamsClass As ParamFileGenerator.clsParams) As ParamFileGenerator.clsParams

End Interface

Public Class clsReconstitueIsoMods
    Implements IReconstituteIsoMods
    Public Enum AvailableAtoms
        N
        C
        H
        O
        S
    End Enum


    Private m_ResTable As DataTable
    Private m_ConnectionString As String

    Public Sub New(ByVal connectionString As String)
        Dim getResTable As New ParamFileGenerator.clsGetResiduesList(connectionString)
        Me.m_ResTable = getResTable.ResiduesTable
        Me.m_ConnectionString = connectionString

    End Sub

    Friend Function ReconIsoMods(ByVal ParamsClass As ParamFileGenerator.clsParams) As ParamFileGenerator.clsParams Implements IReconstituteIsoMods.ReconstitueIsoMods
        Return Me.StreamlineIsoModsToStatics(ParamsClass, ParamsClass.IsotopicMods)
    End Function
    Protected Function getMultiplier(ByVal AA As String, ByVal Atom As AvailableAtoms) As Integer

        Dim m_Atomrows() As DataRow = Me.m_ResTable.Select("[Residue_Symbol] = '" & AA & "'")
        Dim m_AtomRow As DataRow = m_Atomrows(0)
        Dim m_AtomCount As Integer = CInt(m_AtomRow.Item(getAtomCountColumn(Atom)))

        Return m_AtomCount

    End Function

    Protected Function getAtomCountColumn(ByVal Atom As AvailableAtoms) As String

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
    Protected Function StreamlineIsoModsToStatics( _
        ByVal ParamsClass As ParamFileGenerator.clsParams, _
        ByVal IsoMods As ParamFileGenerator.clsIsoMods) As ParamFileGenerator.clsParams

        Dim im As ParamFileGenerator.clsModEntry

        Dim tmpAtom As String
        Dim tmpIsoMass As Double
        Dim tmpAtomCount As Integer

        Dim AAEnums() As String = System.Enum.GetNames(GetType(ParamFileGenerator.clsStaticMods.ResidueCode))
        Dim tmpAA As String
        Dim tmpAASLC As String


        For Each im In IsoMods
            tmpAtom = im.ReturnResidueAffected(0)
            tmpIsoMass = im.MassDifference

            For Each tmpAA In AAEnums
                If InStr(tmpAA, "Term") = 0 Then
                    tmpAASLC = Left(tmpAA, 1)
                    tmpAtomCount = CInt(getMultiplier(tmpAASLC, DirectCast(System.Enum.Parse(GetType(AvailableAtoms), tmpAtom), AvailableAtoms)))
                    ParamsClass.StaticModificationsList.ChangeAAModification( _
                        DirectCast(System.Enum.Parse(GetType(ParamFileGenerator.clsStaticMods.ResidueCode), tmpAA), ParamFileGenerator.clsStaticMods.ResidueCode), _
                        tmpIsoMass * tmpAtomCount, True)
                End If
            Next

        Next

        Return ParamsClass

    End Function
End Class
