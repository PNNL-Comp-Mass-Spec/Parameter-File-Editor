Public Interface IMassTweaker
    Enum ModTypes
        StaticMod
        DynamicMod
        IsotopicMod
    End Enum

    Property MaxTweakDifference() As Single
    ReadOnly Property TweakedSymbol() As String
    ReadOnly Property TweakedDescription() As String
    ReadOnly Property TweakedModID() As Integer
    ReadOnly Property MassCorrectionsTable() As DataTable

    Function GetTweakedMass(ByVal ModMass As Single, Optional ByVal AffectedAtom As String = "-") As Single

    Sub AddMassCorrection(ByVal modName As String, _
        ByVal modDescription As String, _
        ByVal modMassChange As Single, _
        Optional ByVal modAffectedAtom As String = "-")
    Sub RefreshGlobalModsTableCache(ByVal connectionString As String)
End Interface





Public Class clsMassTweaker
    Inherits ParamFileGenerator.clsDBTask
    Implements IMassTweaker

#Region " Constants "
    Protected Const Mass_Corrections_Table_Name As String = "T_Mass_Correction_Factors"
    Protected Const Max_Tweak_Difference As Single = 0.15
#End Region

    Private m_SPError As String
    Private m_UsedSymbol As String
    Private m_UsedDescription As String
    Private m_UsedID As Integer

    Private m_MassCorrectionsTable As DataTable
    Private m_Initialized As Boolean
    Private m_MaxTweakDiff As Single

    Friend ReadOnly Property MassCorrectionsTable() As DataTable Implements IMassTweaker.MassCorrectionsTable
        Get
            Return Me.m_MassCorrectionsTable
        End Get
    End Property

    Friend ReadOnly Property Initialized() As Boolean
        Get
            Return Me.m_Initialized
        End Get
    End Property

    Friend Property MaxTweakDifference() As Single Implements IMassTweaker.MaxTweakDifference
        Get
            Return Me.m_MaxTweakDiff
        End Get
        Set(ByVal Value As Single)
            Me.m_MaxTweakDiff = Value
        End Set
    End Property


    Friend ReadOnly Property TweakedSymbol() As String Implements IMassTweaker.TweakedSymbol
        Get
            Return Me.m_UsedSymbol
        End Get
    End Property
    Friend ReadOnly Property TweakedDescription() As String Implements IMassTweaker.TweakedDescription
        Get
            Return Me.m_UsedDescription
        End Get
    End Property

    Friend ReadOnly Property TweakedModID() As Integer Implements IMassTweaker.TweakedModID
        Get
            Return Me.m_UsedID
        End Get
    End Property

    Public Sub New(ByVal mgrParams As ParamFileEditor.ProgramSettings.IProgramSettings)
        MyBase.New(mgrParams.DMS_ConnectionString)
        'Load T_Global_Mods Table from DMS
        If Not Initialized Then
            Me.m_MassCorrectionsTable = Me.GetMassCorrectionsTable(mgrParams.DMS_ConnectionString)
            Me.m_Initialized = True
        End If
        If MaxTweakDifference = 0 Then
            MaxTweakDifference = Max_Tweak_Difference
        End If
    End Sub

    Public Sub New(ByVal connectionString As String)

        MyBase.New(connectionString)
        'Load T_Global_Mods Table from DMS
        If Not Initialized Then
            Me.m_MassCorrectionsTable = Me.GetMassCorrectionsTable(connectionString)
            Me.m_Initialized = True
        End If
        If MaxTweakDifference = 0 Then
            MaxTweakDifference = Max_Tweak_Difference
        End If

    End Sub

    Private Function GetMassCorrectionsTable(ByVal connectionString As String) As DataTable
        Dim m_GetGlobalMods_DA As SqlClient.SqlDataAdapter = Nothing
        Dim m_GetGlobalMods_CB As SqlClient.SqlCommandBuilder = Nothing

        Dim sql As String = "SELECT * FROM " & clsMassTweaker.Mass_Corrections_Table_Name

        Dim tmpTable As DataTable = GetTable(sql, m_GetGlobalMods_DA, m_GetGlobalMods_CB)
        tmpTable.TableName = clsMassTweaker.Mass_Corrections_Table_Name
        setprimarykey(0, tmpTable)

        Return tmpTable
    End Function

    Private Sub RefreshGlobalModsTableCache(ByVal connectionString As String) Implements IMassTweaker.RefreshGlobalModsTableCache
        Me.m_MassCorrectionsTable = Me.GetMassCorrectionsTable(connectionString)
    End Sub

    Private Function GetTweakedMass(ByVal ModMass As Single, Optional ByVal AffectedAtom As String = "-") As Single Implements IMassTweaker.GetTweakedMass

        Dim row As DataRow
        Dim rows() As DataRow

        Dim smallestDiffID As Integer
        Dim diff As Single
        Dim smallestDiff As Single = Me.m_MaxTweakDiff
        Dim newMass As Single

        rows = Me.MassCorrectionsTable.Select("[Monoisotopic_Mass_Correction] > " & ModMass - smallestDiff & _
                " AND [Monoisotopic_Mass_Correction] < " & ModMass + smallestDiff & " AND [Affected_Atom] = '" & AffectedAtom & "'")

        For Each row In rows
            diff = Math.Abs(CSng(row.Item("Monoisotopic_Mass_Correction")) - ModMass)
            If diff < Me.m_MaxTweakDiff Then
                If diff < smallestDiff Then
                    smallestDiff = diff
                    smallestDiffID = DirectCast(row.Item("Mass_Correction_ID"), Int32)
                    newMass = CSng(row.Item("Monoisotopic_Mass_Correction"))
                    Me.m_UsedDescription = row.Item("Description")
                    Me.m_UsedSymbol = row.Item("Mass_Correction_Tag")
                End If
            End If
        Next

        Me.m_UsedID = smallestDiffID

        Return newMass

    End Function

    Private Sub RunSP_AddMassCorrectionEntry( _
    ByVal modName As String, _
    ByVal modDescription As String, _
    ByVal modMassChange As Single, _
    Optional ByVal modAffectedAtom As String = "-") Implements IMassTweaker.AddMassCorrection


        Dim sp_Save As SqlClient.SqlCommand

        Me.OpenConnection()
        sp_Save = New SqlClient.SqlCommand("AddMassCorrectionEntry", Me.m_DBCn)

        sp_Save.CommandType = CommandType.StoredProcedure

        'Define parameters
        Dim myParam As SqlClient.SqlParameter

        'Define parameter for sp's return value
        myParam = sp_Save.Parameters.Add("@Return", SqlDbType.Int)
        myParam.Direction = ParameterDirection.ReturnValue

        'Define parameters for the sp's arguments
        myParam = sp_Save.Parameters.Add("@modName", SqlDbType.VarChar, 8)
        myParam.Direction = ParameterDirection.Input
        myParam.Value = modName

        myParam = sp_Save.Parameters.Add("@modDescription", SqlDbType.VarChar, 64)
        myParam.Direction = ParameterDirection.Input
        myParam.Value = modDescription

        myParam = sp_Save.Parameters.Add("@modMassChange", SqlDbType.Float, 8)
        myParam.Direction = ParameterDirection.Input
        myParam.Value = modMassChange

        myParam = sp_Save.Parameters.Add("@modAffectedAtom", SqlDbType.Char, 1)
        myParam.Direction = ParameterDirection.Input
        myParam.Value = modAffectedAtom


        myParam = sp_Save.Parameters.Add("@message", SqlDbType.VarChar, 512)
        myParam.Direction = ParameterDirection.Output


        'Execute the sp
        sp_Save.ExecuteNonQuery()

        'Get return value
        Dim ret As Integer = CInt(sp_Save.Parameters("@Return").Value)

        If ret <> 0 Then
            Me.m_SPError = CStr(sp_Save.Parameters("@message").Value)
        End If

        Me.CloseConnection()

    End Sub


    'Private Sub RunSP_AddGlobalModEntry( _
    'ByVal modSymbol As String, _
    'ByVal modDescription As String, _
    'ByVal modType As IMassTweaker.ModTypes, _
    'ByVal modMassChange As Single, _
    'ByVal modResidues As String) Implements IMassTweaker.AddGlobalMod

    '    Dim sp_Save As SqlClient.SqlCommand

    '    Dim modChar As String

    '    If modType = IMassTweaker.ModTypes.DynamicMod Then
    '        modChar = "D"
    '    Else
    '        modChar = "S"
    '    End If

    '    Me.OpenConnection()
    '    sp_Save = New SqlClient.SqlCommand("AddGlobalModEntry", Me.m_DBCn)

    '    sp_Save.CommandType = CommandType.StoredProcedure

    '    'Define parameters
    '    Dim myParam As SqlClient.SqlParameter

    '    'Define parameter for sp's return value
    '    myParam = sp_Save.Parameters.Add("@Return", SqlDbType.Int)
    '    myParam.Direction = ParameterDirection.ReturnValue

    '    'Define parameters for the sp's arguments
    '    myParam = sp_Save.Parameters.Add("@modSymbol", SqlDbType.VarChar, 8)
    '    myParam.Direction = ParameterDirection.Input
    '    myParam.Value = modSymbol

    '    myParam = sp_Save.Parameters.Add("@modDescription", SqlDbType.VarChar, 64)
    '    myParam.Direction = ParameterDirection.Input
    '    myParam.Value = modDescription

    '    myParam = sp_Save.Parameters.Add("@modType", SqlDbType.Char, 1)
    '    myParam.Direction = ParameterDirection.Input
    '    myParam.Value = CChar(modChar)

    '    myParam = sp_Save.Parameters.Add("@modMassChange", SqlDbType.Float, 8)
    '    myParam.Direction = ParameterDirection.Input
    '    myParam.Value = modMassChange

    '    myParam = sp_Save.Parameters.Add("@modResidues", SqlDbType.VarChar, 50)
    '    myParam.Direction = ParameterDirection.Input
    '    myParam.Value = modResidues


    '    myParam = sp_Save.Parameters.Add("@message", SqlDbType.VarChar, 512)
    '    myParam.Direction = ParameterDirection.Output


    '    'Execute the sp
    '    sp_Save.ExecuteNonQuery()

    '    'Get return value
    '    Dim ret As Integer = CInt(sp_Save.Parameters("@Return").Value)

    '    If ret <> 0 Then
    '        Me.m_SPError = CStr(sp_Save.Parameters("@message").Value)
    '    End If

    '    Me.CloseConnection()

    'End Sub

End Class