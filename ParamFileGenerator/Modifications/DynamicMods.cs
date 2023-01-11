Imports System.Collections.Generic
Imports System.Text.RegularExpressions

Public Class DynamicMods
    Inherits Mods
    '    Public Property Dyn_Mod_1_MassDiff As Double
    '        Get
    '            Return Dyn_Mod_n_MassDiff(1)
    '        End Get
    '        Set
    '            Dyn_Mod_n_MassDiff(1) = Value
    '        End Set
    '    End Property
    '    Public Property Dyn_Mod_2_MassDiff As Double
    '        Get
    '            Return Dyn_Mod_n_MassDiff(2)
    '        End Get
    '        Set
    '            Dyn_Mod_n_MassDiff(2) = Value
    '        End Set
    '    End Property
    '    Public Property Dyn_Mod_3_MassDiff As Double
    '        Get
    '            Return Dyn_Mod_n_MassDiff(3)
    '        End Get
    '        Set
    '            Dyn_Mod_n_MassDiff(3) = Value
    '        End Set
    '    End Property

    '    Public Property Dyn_Mod_1_AAList As String
    '        Get
    '            Return Dyn_Mod_n_AAList(1)
    '        End Get
    '        Set(Value As String)
    '            Dyn_Mod_n_AAList(1) = Value
    '        End Set
    '    End Property
    '    Public Property Dyn_Mod_2_AAList As String
    '        Get
    '            Return Dyn_Mod_n_AAList(2)
    '        End Get
    '        Set(Value As String)
    '            Dyn_Mod_n_AAList(2) = Value
    '        End Set
    '    End Property
    '    Public Property Dyn_Mod_3_AAList As String
    '        Get
    '            Return Dyn_Mod_n_AAList(3)
    '        End Get
    '        Set(Value As String)
    '            Dyn_Mod_n_AAList(3) = Value
    '        End Set
    '    End Property

    Public Sub New(DynamicModString As String)
        MyBase.New()
        m_OrigDynModString = DynamicModString
        ParseDynModString(m_OrigDynModString)
    End Sub

    Public Sub New()
        MyBase.New()
        m_OrigDynModString = Nothing
    End Sub

    Public Function ReturnDynModString(maxDynMods As Integer) As String
        Dim s As String
        'If Initialized Then
        s = AssembleModString(maxDynMods)
        Return s
        'Else
        '    Return ""
        'End If
    End Function

    ' 'TODO replace with real function for term dyn mods
    ' 'Just a placeholder for now
    ''Public Function ReturnDynTermModString As String
    ''    Return "0.0000 0.0000"
    ''End Function

    Public Overloads Sub Add(
        AffectedResidueString As String,
        MassDifference As Double)

        Dim residueList = ConvertAffectedResStringToList(AffectedResidueString)
        Dim newDynMod As New ModEntry(residueList, MassDifference, ModEntry.ModificationTypes.Dynamic)
        List.Add(newDynMod)
    End Sub
    Public Overloads Sub Add(ModToAdd As ModEntry)
        List.Add(ModToAdd)
    End Sub

    Public Property Dyn_Mod_n_MassDiff(DynModNumber As Integer) As Double
        Get
            Dim dm As ModEntry
            Dim index As Integer = DynModNumber - 1
            Try
                dm = DirectCast(List.Item(index), ModEntry)
            Catch ex As Exception
                dm = New ModEntry(ConvertAffectedResStringToList("C"), 0.0, ModEntry.ModificationTypes.Dynamic)
            End Try
            Return dm.MassDifference
        End Get
        Set
            Dim index As Integer = DynModNumber - 1
            Dim dm As ModEntry
            If index <= List.Count - 1 Then
                dm = DirectCast(List.Item(index), ModEntry)
                dm.MassDifference = Value
                Replace(index, dm)
            Else
                Add("C", Value)
            End If
        End Set
    End Property

    Public Property Dyn_Mod_n_AAList(DynModNumber As Integer) As String
        Get
            Dim dm As ModEntry
            Dim index As Integer = DynModNumber - 1
            Try
                dm = DirectCast(List.Item(index), ModEntry)
            Catch ex As Exception
                dm = New ModEntry(ConvertAffectedResStringToList("C"), 0.0, ModEntry.ModificationTypes.Dynamic)
            End Try
            Return dm.ReturnAllAffectedResiduesString
        End Get
        Set
            Dim index As Integer = DynModNumber - 1
            Dim dm As ModEntry
            If index <= List.Count - 1 Then
                dm = DirectCast(List.Item(index), ModEntry)
                dm.ResidueCollection = ConvertAffectedResStringToList(Value)
                Replace(index, dm)
            Else
                Add(Value, CDbl(0.0))
            End If
        End Set
    End Property

    Public Property Dyn_Mod_n_Global_ModID(DynModNumber As Integer) As Integer
        Get
            Dim dm As ModEntry
            Dim index As Integer = DynModNumber - 1
            Try
                dm = DirectCast(List.Item(index), ModEntry)
            Catch ex As Exception
                dm = New ModEntry(ConvertAffectedResStringToList("C"), 0.0, ModEntry.ModificationTypes.Dynamic)
            End Try
            Return dm.GlobalModID
        End Get
        Set
            Dim index As Integer = DynModNumber - 1
            Dim dm As ModEntry
            If index <= List.Count - 1 Then
                dm = DirectCast(List.Item(index), ModEntry)
                dm.GlobalModID = Value
                Replace(index, dm)
            Else
                Add("C", 0.0)
            End If
        End Set
    End Property

    Protected Overridable Function AssembleModString(counter As Integer) As String
        Dim s = ""
        Dim tmpModString As String
        Dim tmpModMass As Double
        Dim dynMod As ModEntry
        Dim padCount As Integer

        For Each dynMod In List
            tmpModMass = dynMod.MassDifference
            tmpModString = dynMod.ReturnAllAffectedResiduesString
            s = s & Format(tmpModMass, "0.0000") & " " & tmpModString & " "
            counter -= 1
        Next

        For padCount = 0 To counter - 1
            If padCount <= 2 Then
                s &= "0.0000 C "
            Else
                s &= "0.0000 X "
            End If
        Next

        Return s.Trim()
    End Function

    Protected Overridable Sub ParseDynModString(DMString As String)

        Dim splitRE = New Regex("(?<modmass>\d+\.\d+)\s+(?<residues>[A-Za-z]+)")
        Dim matches = splitRE.Matches(DMString)

        For Each m As Match In matches
            Dim tmpMass = CDbl(m.Groups("modmass").Value)
            Dim tmpResString = m.Groups("residues").ToString()

            If Math.Abs(tmpMass) > Single.Epsilon Then
                Dim residueList = New List(Of String)
                For resCounter = 1 To Len(tmpResString)
                    Dim tmpRes = Mid(tmpResString, resCounter, 1)
                    residueList.Add(tmpRes)
                Next
                Dim modEntry As New ModEntry(residueList, tmpMass, ModEntry.ModificationTypes.Dynamic)
                Add(modEntry)
            End If
        Next

    End Sub

    Protected m_OrigDynModString As String
    'private m_EmptyMod as New ModEntry(

End Class
