Imports PRISMDatabaseUtils

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
    Inherits ParamFileGenerator.DBTask
    Implements IMassTweaker

#Region "Constants"
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

#Disable Warning BC40028 ' Type of parameter is not CLS-compliant
    Public Sub New(dbTools As IDBTools)
#Enable Warning BC40028 ' Type of parameter is not CLS-compliant

        MyBase.New(dbTools)

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
        Dim sql As String = "SELECT * FROM " & Mass_Corrections_Table_Name

        Dim tmpTable As DataTable = GetTable(sql)
        tmpTable.TableName = Mass_Corrections_Table_Name

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

        Dim cmdSave = mDBTools.CreateCommand("add_mass_correction_entry", CommandType.StoredProcedure)

        ' Define the stored procedure return value
        mDBTools.AddParameter(cmdSave, "@Return", SqlType.BigInt, ParameterDirection.ReturnValue)

        ' Define parameters for the stored procedure arguments
        mDBTools.AddParameter(cmdSave, "@modName", SqlType.VarChar, 8).Value = modName

        mDBTools.AddParameter(cmdSave, "@modDescription", SqlType.VarChar, 64).Value = modDescription

        mDBTools.AddParameter(cmdSave, "@modMassChange", SqlType.Float).Value = modMassChange

        mDBTools.AddParameter(cmdSave, "@modAffectedAtom", SqlType.Char, 1).Value = modAffectedAtom

        Dim messageParam = mDBTools.AddParameter(cmdSave, "@message", SqlType.VarChar, 512, ParameterDirection.Output)

        Dim errorMessage As String = String.Empty

        'Execute the stored procedure
        Dim returnValue = mDBTools.ExecuteSP(cmdSave, errorMessage)

        If returnValue <> 0 Then
            ' Stored procedure error
            Console.WriteLine("Failed executing procedure " & cmdSave.CommandText)
            Console.WriteLine(messageParam.CastDBVal(String.Empty))
        End If


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
    '    myParam = mDBTools.AddParameter("@Return", SqlType.Int).Direction = ParameterDirection.ReturnValue

    '    ' Define parameters for the stored procedure arguments
    '    mDBTools.AddParameter("@modSymbol", SqlType.VarChar, 8).Value = modSymbol

    '    mDBTools.AddParameter("@modDescription", SqlType.VarChar, 64).Value = modDescription

    '    mDBTools.AddParameter("@modType", SqlType.Char, 1).Value = CChar(modChar)

    '    mDBTools.AddParameter("@modMassChange", SqlType.Float, 8).Value = modMassChange

    '    mDBTools.AddParameter("@modResidues", SqlType.VarChar, 50).Value = modResidues

    '    mDBTools.AddParameter("@message", SqlType.VarChar, 512).Direction = ParameterDirection.Output

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