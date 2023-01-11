Imports System.Collections.Generic

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
