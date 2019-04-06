Public Interface IMassTweaker
    Enum ModTypes
        StaticMod
        DynamicMod
        IsotopicMod
    End Enum

    Property MaxTweakDifference As Double
    ReadOnly Property TweakedSymbol As String
    ReadOnly Property TweakedDescription As String
    ReadOnly Property TweakedModID As Integer
    ReadOnly Property MassCorrectionsTable As DataTable

    Function GetTweakedMass(ModMass As Double, Optional AffectedAtom As String = "-") As Double

    Sub AddMassCorrection(modName As String,
        modDescription As String,
        modMassChange As Double,
        Optional modAffectedAtom As String = "-")
    Sub RefreshGlobalModsTableCache(connectionString As String)
End Interface

Public Class clsMassTweaker
    Inherits ParamFileGenerator.clsDBTask
    Implements IMassTweaker

#Region " Constants "
    Protected Const Mass_Corrections_Table_Name As String = "T_Mass_Correction_Factors"
    Protected Const Max_Tweak_Difference As Double = 0.005
#End Region

    Friend Property MassCorrectionsTable As DataTable Implements IMassTweaker.MassCorrectionsTable

    Friend ReadOnly Property Initialized As Boolean

    Friend Property MaxTweakDifference As Double Implements IMassTweaker.MaxTweakDifference

    Friend Property TweakedSymbol As String Implements IMassTweaker.TweakedSymbol

    Friend Property TweakedDescription As String Implements IMassTweaker.TweakedDescription

    Friend Property TweakedModID As Integer Implements IMassTweaker.TweakedModID


    Public Sub New(mgrParams As ProgramSettings.IProgramSettings)
        MyBase.New(mgrParams.DMS_ConnectionString)

        'Load T_Global_Mods Table from DMS
        If Not Initialized Then
            MassCorrectionsTable = GetMassCorrectionsTable()
            Initialized = True
        End If
        If Math.Abs(MaxTweakDifference) < Single.Epsilon Then
            MaxTweakDifference = Max_Tweak_Difference
        End If
    End Sub

    Public Sub New(connectionString As String)

        MyBase.New(connectionString)

        'Load T_Global_Mods Table from DMS
        If Not Initialized Then
            MassCorrectionsTable = GetMassCorrectionsTable()
            Initialized = True
        End If
        If Math.Abs(MaxTweakDifference) < Single.Epsilon Then
            MaxTweakDifference = Max_Tweak_Difference
        End If

    End Sub

    Private Function GetMassCorrectionsTable() As DataTable
        Dim m_GetGlobalMods_DA As SqlClient.SqlDataAdapter = Nothing
        Dim m_GetGlobalMods_CB As SqlClient.SqlCommandBuilder = Nothing

        Dim sql As String = "SELECT * FROM " & Mass_Corrections_Table_Name

        Dim tmpTable As DataTable = GetTable(sql, m_GetGlobalMods_DA, m_GetGlobalMods_CB)
        tmpTable.TableName = Mass_Corrections_Table_Name
        SetPrimaryKey(0, tmpTable)

        Return tmpTable
    End Function

    Private Sub RefreshGlobalModsTableCache(connString As String) Implements IMassTweaker.RefreshGlobalModsTableCache
        MassCorrectionsTable = GetMassCorrectionsTable()
    End Sub

    Private Function GetTweakedMass(ModMass As Double, Optional AffectedAtom As String = "-") As Double Implements IMassTweaker.GetTweakedMass

        Dim row As DataRow
        Dim rows() As DataRow

        Dim smallestDiffID As Integer
        Dim diff As Double
        Dim smallestDiff As Double = MaxTweakDifference
        Dim newMass As Double

        rows = MassCorrectionsTable.Select("[Monoisotopic_Mass] > " & ModMass - smallestDiff &
                " AND [Monoisotopic_Mass] < " & ModMass + smallestDiff & " AND [Affected_Atom] = '" & AffectedAtom & "'")

        For Each row In rows
            diff = Math.Abs(CDbl(row.Item("Monoisotopic_Mass")) - ModMass)
            If diff < MaxTweakDifference Then
                If diff < smallestDiff Then
                    smallestDiff = diff
                    smallestDiffID = DirectCast(row.Item("Mass_Correction_ID"), Int32)
                    newMass = CDbl(row.Item("Monoisotopic_Mass"))
                    TweakedDescription = CStr(row.Item("Description"))
                    TweakedSymbol = CStr(row.Item("Mass_Correction_Tag"))
                End If
            End If
        Next

        TweakedModID = smallestDiffID

        Return newMass

    End Function

    Private Sub RunSP_AddMassCorrectionEntry(
    modName As String,
    modDescription As String,
    modMassChange As Double,
    Optional modAffectedAtom As String = "-") Implements IMassTweaker.AddMassCorrection


        OpenConnection()
        Dim sp_Save = New SqlClient.SqlCommand("AddMassCorrectionEntry", m_DBCn)

        sp_Save.CommandType = CommandType.StoredProcedure

        ' Define the stored procedure return value
        sp_Save.Parameters.Add("@Return", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

        ' Define parameters for the stored procedure arguments
        sp_Save.Parameters.Add("@modName", SqlDbType.VarChar, 8).Value = modName

        sp_Save.Parameters.Add("@modDescription", SqlDbType.VarChar, 64).Value = modDescription

        sp_Save.Parameters.Add("@modMassChange", SqlDbType.Float, 8).Value = modMassChange

        sp_Save.Parameters.Add("@modAffectedAtom", SqlDbType.Char, 1).Value = modAffectedAtom

        sp_Save.Parameters.Add("@message", SqlDbType.VarChar, 512).Direction = ParameterDirection.Output

        'Execute the stored procedure
        sp_Save.ExecuteNonQuery()

        'Get return value
        Dim ret = CInt(sp_Save.Parameters("@Return").Value)

        If ret <> 0 Then
            ' Stored procedure error
            Console.WriteLine(CStr(sp_Save.Parameters("@message").Value))
        End If

        CloseConnection()

    End Sub


    'Private Sub RunSP_AddGlobalModEntry( _
    'modSymbol As String, _
    'modDescription As String, _
    'modType As IMassTweaker.ModTypes, _
    'modMassChange As Double, _
    'modResidues As String) Implements IMassTweaker.AddGlobalMod

    '    Dim sp_Save As SqlClient.SqlCommand

    '    Dim modChar As String

    '    If modType = IMassTweaker.ModTypes.DynamicMod Then
    '        modChar = "D"
    '    Else
    '        modChar = "S"
    '    End If

    '    OpenConnection()
    '    sp_Save = New SqlClient.SqlCommand("AddGlobalModEntry", m_DBCn)

    '    sp_Save.CommandType = CommandType.StoredProcedure

    '    ' Define the stored procedure return value
    '    myParam = sp_Save.Parameters.Add("@Return", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

    '    ' Define parameters for the stored procedure arguments
    '    sp_Save.Parameters.Add("@modSymbol", SqlDbType.VarChar, 8).Value = modSymbol

    '    sp_Save.Parameters.Add("@modDescription", SqlDbType.VarChar, 64).Value = modDescription

    '    sp_Save.Parameters.Add("@modType", SqlDbType.Char, 1).Value = CChar(modChar)

    '    sp_Save.Parameters.Add("@modMassChange", SqlDbType.Float, 8).Value = modMassChange

    '    sp_Save.Parameters.Add("@modResidues", SqlDbType.VarChar, 50).Value = modResidues

    '    sp_Save.Parameters.Add("@message", SqlDbType.VarChar, 512).Direction = ParameterDirection.Output

    '    ' Execute the stored procedure
    '    sp_Save.ExecuteNonQuery()

    '    'Get return value
    '    Dim ret As Integer = CInt(sp_Save.Parameters("@Return").Value)

    '    If ret <> 0 Then
    '        m_SPError = CStr(sp_Save.Parameters("@message").Value)
    '    End If

    '    CloseConnection()

    'End Sub

End Class