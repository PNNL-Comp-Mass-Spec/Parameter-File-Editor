Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Data.SqlClient
Imports System.Reflection

Namespace DownloadParams

    Public Class clsParamsFromDMS
        Inherits clsDBTask
#Region " Constants "
        Protected Const Param_File_Table As String = "T_Param_Files"
        Protected Const Param_Entry_Table As String = "T_Param_Entries"
        Protected Const Param_FileTypes_Table As String = "T_Param_File_Types"
        Protected Const Param_Class_Table As String = "T_Param_Entry_Types"
        Protected Const Param_MassType_Table As String = "T_Sequest_Params_MassType_Name"
        Protected Const Param_Mass_Mods_Table As String = "T_Param_File_Mass_Mods"
        Protected Const Mass_Corr_Factors As String = "T_Mass_Correction_Factors"
        Protected Const Residues_Table As String = "T_Residues"
#End Region

#Region " Enums "
        Public Enum AcceptableParams
            SelectedEnzymeIndex
            SelectedEnzymeCleavagePosition
            MaximumNumberMissedCleavages
            ParentMassType
            FragmentMassType
            DynamicMods
            TermDynamicMods
            StaticModificationsList
            IsotopicMods
            PartialSequenceToMatch
            CreateOutputFiles
            NumberOfResultsToProcess
            MaximumNumAAPerDynMod
            MaximumNumDifferentialPerPeptide
            PeptideMassTolerance
            FragmentIonTolerance
            NumberOfOutputLines
            NumberOfDescriptionLines
            ShowFragmentIons
            PrintDuplicateReferences
            SelectedNucReadingFrameIndex
            RemovePrecursorPeak
            IonSeries
            IonCutoffPercentage
            MinimumProteinMassToSearch
            MaximumProteinMassToSearch
            NumberOfDetectedPeaksToMatch
            NumberOfAllowedDetectedPeakErrors
            MatchedPeakMassTolerance
            AminoAcidsAllUpperCase
            SequenceHeaderInfoToFilter
            PeptideMassUnits
            FragmentMassUnits
        End Enum
        Public Enum BasicParams
            SelectedEnzymeIndex
            SelectedEnzymeCleavagePosition
            MaximumNumberMissedCleavages
            ParentMassType
            FragmentMassType
            PartialSequenceToMatch
        End Enum
        Public Enum AdvancedParams
            CreateOutputFiles
            NumberOfResultsToProcess
            MaximumNumAAPerDynMod
            MaximumNumDifferentialPerPeptide
            PeptideMassTolerance
            FragmentIonTolerance
            NumberOfOutputLines
            NumberOfDescriptionLines
            ShowFragmentIons
            PrintDuplicateReferences
            SelectedNucReadingFrameIndex
            RemovePrecursorPeak
            IonSeries
            IonCutoffPercentage
            MinimumProteinMassToSearch
            MaximumProteinMassToSearch
            NumberOfDetectedPeaksToMatch
            NumberOfAllowedDetectedPeakErrors
            MatchedPeakMassTolerance
            AminoAcidsAllUpperCase
            SequenceHeaderInfoToFilter
            PeptideMassUnits
            FragmentMassUnits
        End Enum

        Public Enum IonSeriesParams
            Use_a_Ions
            Use_b_Ions
            Use_y_Ions
            a_Ion_Weighting
            b_Ion_Weighting
            c_Ion_Weighting
            d_Ion_Weighting
            v_Ion_Weighting
            w_Ion_Weighting
            x_Ion_Weighting
            y_Ion_Weighting
            z_Ion_Weighting
        End Enum

        Public Enum eParamFileTypeConstants
            Unknown = 0
            None = 1
            Sequest = 1000
            QTOFPek = 1001
            DeNovoPeak = 1002
            icr2ls = 1003
            MLynxPek = 1004
            AgilentTOFPek = 1005
            LTQ_FTPek = 1006
            MASIC = 1007
            XTandem = 1008
            Decon2LS = 1010
            TIC_D2L = 1011
            Inspect = 1012
            MSXML_Gen = 1013
            DTA_Gen = 1014
            MSClusterDAT_Gen = 1015
            OMSSA = 1016
            MultiAlign = 1017
            MSGFDB = 1018
            MSAlign = 1019
            SMAQC = 1020
            LipidMapSearch = 1021
            MSAlign_Histone = 1022
            MODa = 1023
            GlyQIQ = 1024
            MSPathFinder = 1025
            MODPlus = 1028
        End Enum

#End Region

#Region " Member Properties "
        Protected m_ID As Integer
        Protected m_Name As String
        Protected m_ParamFileType As eParamFileTypeConstants
        Protected m_Params As clsParams
        Protected m_ParamsSet As DataSet
        Protected m_ParamSetCount As Integer
        Protected m_BaseLineParamSet As clsParams
        Protected m_AcceptableParams As StringCollection
        Protected m_BasicParams As StringCollection
        Protected m_AdvancedParams As StringCollection
        Protected m_IonSeriesParams As StringCollection
        Protected m_GetID_DA As SqlDataAdapter
        Protected m_GetID_DB As SqlCommandBuilder
        Protected m_GetEntries_DA As SqlDataAdapter
        Protected m_GetEntries_CB As SqlCommandBuilder
        Protected m_MassMods As DataTable

#End Region

#Region " public Properties "
        Public ReadOnly Property ParamFileTable() As DataTable
            Get
                Return m_ParamsSet.Tables(Param_File_Table)
            End Get
        End Property
        Public ReadOnly Property ParamEntryTable() As DataTable
            Get
                Return m_ParamsSet.Tables(Param_Entry_Table)
            End Get
        End Property

        Public ReadOnly Property ParamSetCount() As Integer
            Get
                Return m_ParamSetCount
            End Get
        End Property

        Public ReadOnly Property ParamFileType() As eParamFileTypeConstants
            Get
                Return m_ParamFileType
            End Get
        End Property
#End Region


#Region " public Functions "
        Public Sub New(ConnectionString As String)
            MyBase.New(ConnectionString, True)
            m_AcceptableParams = LoadAcceptableParamList()
            m_BasicParams = LoadBasicParams()
            m_AdvancedParams = LoadAdvancedParams()
            m_IonSeriesParams = LoadIonSeriesParams()
            m_BaseLineParamSet = clsMainProcess.BaseLineParamSet
            m_ParamsSet = GetParamsFromDMS()
            If m_ParamsSet Is Nothing Then
                Exit Sub
            End If
        End Sub

        Public Sub RefreshParamsFromDMS()
            m_ParamsSet = GetParamsFromDMS()
        End Sub

        Public Function ReadParamsFromDMS(ParamSetName As String) As clsParams
            'Retrieve ID number first, then use the same procedure as below
            m_Name = ParamSetName

            m_ParamFileType = GetTypeWithName(ParamSetName)

            If m_ParamFileType = eParamFileTypeConstants.Unknown Then
                Throw New Exception("Parameter file " & ParamSetName & " was not found in table " & Param_File_Table)
            End If

            If m_ParamFileType <> eParamFileTypeConstants.Sequest Then
                ' This param file type is not supported for export from DMS
                Dim paramFileTypeName As String = [Enum].GetName(GetType(eParamFileTypeConstants), m_ParamFileType)
                If String.IsNullOrEmpty(paramFileTypeName) Then
                    paramFileTypeName = "Unknown"
                End If

                Throw New NotSupportedException("Parameter file " & ParamSetName & " is of type " & paramFileTypeName & ", which isn't support for export from DMS")
            End If

            m_ID = GetIDWithName(m_Name, m_ParamFileType)

            m_Params = RetrieveParams(m_ID, m_ParamFileType)
            Return m_Params

        End Function

        Public Function ReadParamsFromDMS(ParamSetID As Integer) As clsParams
            m_ID = ParamSetID
            m_ParamFileType = GetTypeWithID(m_ID)

            If m_ParamFileType = eParamFileTypeConstants.Unknown Then
                Throw New Exception("Parameter file ID " & ParamSetID & " was not found in table " & Param_File_Table)
            End If

            If m_ParamFileType <> eParamFileTypeConstants.Sequest Then
                ' This param file type is not supported for export from DMS
                Throw New NotSupportedException("Parameter file ID " & ParamSetID & " is of type " & [Enum].GetName(GetType(eParamFileTypeConstants), m_ParamFileType) & ", which isn't support for export from DMS")
            End If

            m_Params = RetrieveParams(m_ID, m_ParamFileType)
            Return m_Params
        End Function

        Public Function RetrieveAvailableParams() As DataTable
            Return GetAvailableParamSets()
        End Function

        Public Function RetrieveParamFileTypes() As DataTable
            Return GetParamFileTypes()
        End Function

        Public Function ParamSetNameExists(ParamSetName As String) As Boolean
            Return DoesParamSetNameExist(ParamSetName)
        End Function

        Public Function ParamSetIDExists(ParamSetID As Integer) As Boolean
            Return DoesParamSetIDExist(ParamSetID)
        End Function

        Public Function GetParamSetIDFromName(Name As String) As Integer
            Dim eParamFileType As eParamFileTypeConstants
            eParamFileType = GetTypeWithName(Name)

            If eParamFileType = eParamFileTypeConstants.Unknown Then
                Console.WriteLine("Parameter file " & Name & "  was not found in table " & Param_File_Table)
                Return -1
            End If

            Return GetIDWithName(Name, eParamFileType)
        End Function

#End Region

#Region " Member Functions "
        Protected Function LoadAcceptableParamList() As StringCollection
            Dim ParamEnum() As String = [Enum].GetNames(GetType(AcceptableParams))
            Dim Param As String
            Dim sc As New StringCollection
            For Each Param In ParamEnum
                sc.Add(Param)
            Next
            Return sc
        End Function
        Protected Function LoadBasicParams() As StringCollection
            Dim ParamEnum() As String = [Enum].GetNames(GetType(BasicParams))
            Dim Param As String
            Dim sc As New StringCollection
            For Each Param In ParamEnum
                sc.Add(Param)
            Next
            Return sc
        End Function
        Protected Function LoadAdvancedParams() As StringCollection
            Dim ParamEnum() As String = [Enum].GetNames(GetType(AdvancedParams))
            Dim Param As String
            Dim sc As New StringCollection
            For Each Param In ParamEnum
                sc.Add(Param)
            Next
            Return sc
        End Function

        Protected Function LoadIonSeriesParams() As StringCollection
            Dim paramEnum() As String = [Enum].GetNames(GetType(IonSeriesParams))
            Dim param As String
            Dim sc As New StringCollection
            For Each param In paramEnum
                sc.Add(param)
            Next
            Return sc
        End Function

        Protected Function GetParamsFromDMS() As DataSet      'Common
            Dim SQL As String
            Dim tmpFileTable As DataTable
            Dim tmpEntryTable As DataTable
            Dim tmpSet As New DataSet

            'SQL to grab param file table
            SQL = "SELECT * FROM " & Param_File_Table ' & " WHERE [Param_File_Type_ID] = 1000"

            tmpFileTable = GetTable(SQL, m_GetID_DA, m_GetID_DB)
            tmpFileTable.TableName = Param_File_Table
            SetPrimaryKey(0, tmpFileTable)

            tmpSet.Tables.Add(tmpFileTable)

            'SQL to grab param entry table
            SQL = "SELECT * FROM " & Param_Entry_Table & " WHERE [Entry_Type] not like '%Modification'"

            tmpEntryTable = GetTable(SQL, m_GetEntries_DA, m_GetEntries_CB)
            tmpEntryTable.TableName = Param_Entry_Table

            tmpSet.Tables.Add(tmpEntryTable)

            Return tmpSet

        End Function

        Protected Function GetParamSetTableCount(psTable As DataTable) As Integer             'Common
            Dim count As Integer = psTable.Rows.Count
            Return count
        End Function

        Protected Function RetrieveParams(ParamSetID As Integer, eParamFileType As eParamFileTypeConstants) As clsParams                  'Download
            Dim p As clsParams = GetParamSetWithID(ParamSetID, eParamFileType)
            Return p
        End Function

        'TODO Fix this function for new mod handling
        Protected Function GetParamSetWithID(ParamSetID As Integer, eParamFileType As eParamFileTypeConstants, Optional DisableMassLookup As Boolean = False) As clsParams   'Download

            Dim dr As DataRow = GetFileRowWithID(ParamSetID)
            If dr Is Nothing Then
                ' Match not found
                Return New clsParams()
            End If

            Dim foundRows As DataRow() = m_ParamsSet.Tables(Param_Entry_Table).Select("[Param_File_ID] = " & ParamSetID, "[Entry_Sequence_Order]")

            Dim storageSet As clsDMSParamStorage = MakeStorageClassFromTableRowSet(foundRows)

            If Not DisableMassLookup Then
                storageSet = GetMassModsFromDMS(ParamSetID, eParamFileType, storageSet)
            End If

            Dim p As clsParams = UpdateParamSetFromDataCollection(storageSet)
            p.FileName = DirectCast(dr.Item("Param_File_Name"), String)
            p.Description = SummarizeDiffColl(storageSet)

            For Each paramRow As DataRow In foundRows
                p.AddLoadedParamName(paramRow.Item("Entry_Specifier").ToString, paramRow.Item("Entry_Value").ToString)
            Next

            Return p
        End Function

        Protected Function MakeStorageClassFromTableRowSet(foundRows As DataRow()) As clsDMSParamStorage
            Dim foundRow As DataRow
            Dim storageClass As New clsDMSParamStorage
            Dim tmpSpec As String
            Dim tmpValue As String
            'Dim tmpTypeString As String
            Dim tmpType As clsDMSParamStorage.ParamTypes

            For Each foundRow In foundRows
                tmpSpec = DirectCast(foundRow.Item("Entry_Specifier"), String)
                tmpValue = DirectCast(foundRow.Item("Entry_Value"), String)
                tmpType = DirectCast([Enum].Parse(GetType(clsDMSParamStorage.ParamTypes), foundRow.Item("Entry_Type").ToString), clsDMSParamStorage.ParamTypes)

                storageClass.Add(tmpSpec, tmpValue, tmpType)
            Next


            Return storageClass

        End Function
        'Todo Adding mass mod grabber
        Protected Function GetMassModsFromDMS(ParamSetID As Integer, eParamFileType As eParamFileTypeConstants, ByRef sc As clsDMSParamStorage) As clsDMSParamStorage
            Const MaxDynMods = 15

            Dim foundRow As DataRow
            Dim foundRows() As DataRow
            Dim tmpSpec As String
            Dim tmpValue As String
            'Dim tmpTypeString As String
            Dim tmpType As clsDMSParamStorage.ParamTypes

            'If m_MassMods Is Nothing Or m_MassMods.Rows.Count = 0 Then
            Dim SQL As String

            SQL = "SELECT mm.Mod_Type_Symbol as Mod_Type_Symbol, r.Residue_Symbol as Residue_Symbol, " &
              "mc.Monoisotopic_Mass_Correction as Monoisotopic_Mass_Correction, " &
              "mm.Local_Symbol_ID as Local_Symbol_ID, mc.Affected_Atom as Affected_Atom " &
              "FROM " & Param_Mass_Mods_Table & " mm INNER JOIN " &
              Mass_Corr_Factors & " mc ON mm.Mass_Correction_ID = mc.Mass_Correction_ID INNER JOIN " &
              Residues_Table & " r ON mm.Residue_ID = r.Residue_ID " &
              "WHERE mm.Param_File_ID = " & ParamSetID

            m_MassMods = GetTable(SQL)
            'End If
            'Look for Dynamic mods

            'Dim dt As DataTable = GetTable(SQL)

            Dim lstLocalSymbolIDs = New List(Of Integer)

            Select Case eParamFileType
                Case eParamFileTypeConstants.Sequest
                    lstLocalSymbolIDs.Add(1)    ' *
                    lstLocalSymbolIDs.Add(2)    ' #
                    lstLocalSymbolIDs.Add(3)    ' @
                    lstLocalSymbolIDs.Add(10)   ' ^
                    lstLocalSymbolIDs.Add(11)   ' ~
                    lstLocalSymbolIDs.Add(4)    ' $
                Case eParamFileTypeConstants.XTandem
                    lstLocalSymbolIDs.Add(1)    ' *
                    lstLocalSymbolIDs.Add(2)    ' #
                    lstLocalSymbolIDs.Add(3)    ' @
                    lstLocalSymbolIDs.Add(4)    ' $
                    lstLocalSymbolIDs.Add(5)    ' &
                    lstLocalSymbolIDs.Add(6)    ' !
                    lstLocalSymbolIDs.Add(7)    ' %
            End Select

            For intSymbolID = 1 To MaxDynMods
                If Not lstLocalSymbolIDs.Contains(intSymbolID) Then
                    lstLocalSymbolIDs.Add(intSymbolID)
                End If
            Next

            For Each intSymbolID As Integer In lstLocalSymbolIDs
                foundRows = m_MassMods.Select("[Mod_Type_Symbol] = 'D' AND [Local_Symbol_ID] = " & intSymbolID & " AND [Residue_Symbol] <> '<' AND [Residue_Symbol] <> '>'", "[Local_Symbol_ID]")
                If foundRows.Length > 0 Then
                    tmpSpec = GetDynModSpecifier(foundRows)
                    tmpValue = foundRows(0).Item("Monoisotopic_Mass_Correction").ToString
                    tmpType = clsDMSParamStorage.ParamTypes.DynamicModification
                    sc.Add(tmpSpec, tmpValue, tmpType)
                End If

            Next

            'Find N-Term Dyn Mods
            foundRows = m_MassMods.Select("[Mod_Type_Symbol] = 'D' AND [Residue_Symbol] = '<'")
            If foundRows.Length > 0 Then
                tmpSpec = GetDynModSpecifier(foundRows)
                tmpValue = foundRows(0).Item("Monoisotopic_Mass_Correction").ToString
                tmpType = clsDMSParamStorage.ParamTypes.TermDynamicModification
                sc.Add(tmpSpec, tmpValue, tmpType)
            End If

            'Find C-Term Dyn Mods
            foundRows = m_MassMods.Select("[Mod_Type_Symbol] = 'D' AND [Residue_Symbol] = '>'")
            If foundRows.Length > 0 Then
                tmpSpec = GetDynModSpecifier(foundRows)
                tmpValue = foundRows(0).Item("Monoisotopic_Mass_Correction").ToString
                tmpType = clsDMSParamStorage.ParamTypes.TermDynamicModification
                sc.Add(tmpSpec, tmpValue, tmpType)
            End If

            'Look for Static and terminal mods

            foundRows = m_MassMods.Select("[Mod_Type_Symbol] = 'S' OR [Mod_Type_Symbol] = 'P' or [Mod_Type_Symbol] = 'T'")

            For Each foundRow In foundRows
                tmpSpec = foundRow.Item("Residue_Symbol").ToString
                Select Case tmpSpec
                    Case "<"
                        tmpSpec = "N_Term_Peptide"
                    Case ">"
                        tmpSpec = "C_Term_Peptide"
                    Case "["
                        tmpSpec = "N_Term_Protein"
                    Case "]"
                        tmpSpec = "C_Term_Protein"
                End Select
                tmpValue = foundRow.Item("Monoisotopic_Mass_Correction").ToString
                tmpType = clsDMSParamStorage.ParamTypes.StaticModification
                sc.Add(tmpSpec, tmpValue, tmpType)
            Next

            'TODO Still need code to handle import/export of isotopic mods

            foundRows = m_MassMods.Select("[Mod_Type_Symbol] = 'I'")

            For Each foundRow In foundRows
                tmpSpec = foundRow.Item("Affected_Atom").ToString
                tmpValue = foundRow.Item("Monoisotopic_Mass_Correction").ToString
                tmpType = clsDMSParamStorage.ParamTypes.IsotopicModification
                sc.Add(tmpSpec, tmpValue, tmpType)
            Next


            Return sc

        End Function

        Protected Function GetDynModSpecifier(rowSet As DataRow()) As String

            Dim tmpSpec = ""

            If rowSet.Length > 0 Then               'We have dynamic mods
                For Each foundRow As DataRow In rowSet
                    tmpSpec = tmpSpec & foundRow.Item("Residue_Symbol").ToString
                Next
                Return tmpSpec
            Else
                Return Nothing
            End If
        End Function

        Protected Function GetIDWithName(Name As String, eParamFileType As eParamFileTypeConstants) As Integer            'Common
            Me.OpenConnection()
            Dim foundRows As DataRow() = Me.ParamFileTable.Select("[Param_File_Name] = '" & Name & "' AND [Param_File_Type_ID] = " & eParamFileType)
            Dim foundRow As DataRow
            Dim tmpID As Integer
            If foundRows.Length <> 0 Then
                foundRow = foundRows(0)
                tmpID = CInt(foundRow.Item("Param_File_ID"))
            Else
                tmpID = -1
            End If
            Return tmpID
        End Function


        'Protected Function GetIDWithName(Name As String) As Integer             'Common
        '    Dim foundRows As DataRow() = Me.ParamFileTable.Select("[Param_File_Name] = '" & Name & "' AND [Param_File_Type_ID] = 1000")
        '    Dim foundRow As DataRow
        '    Dim tmpID As Integer
        '    If foundRows.Length <> 0 Then
        '        foundRow = foundRows(0)
        '        tmpID = CInt(foundRow.Item(0))
        '    Else
        '        tmpID = -1
        '    End If
        '    Return tmpID
        'End Function

        Protected Function GetNameWithID(ID As Integer) As String
            Dim foundRows As DataRow() = Me.ParamFileTable.Select("[Param_File_ID] = " & ID)
            If foundRows.Length <> 0 Then
                Dim foundRow = foundRows(0)
                Return CStr(foundrow.Item("Param_File_Name"))
            Else
                Return Nothing
            End If

        End Function

        Protected Function GetTypeWithID(ID As Integer) As eParamFileTypeConstants
            Me.OpenConnection()
            Dim foundRows As DataRow() = Me.ParamFileTable.Select("[Param_File_ID] = " & ID.ToString())
            Dim foundRow As DataRow
            Dim tmpID As eParamFileTypeConstants
            If foundRows.Length <> 0 Then
                foundRow = foundRows(0)
                tmpID = CType(foundRow.Item("Param_File_Type_ID"), eParamFileTypeConstants)
            Else
                tmpID = eParamFileTypeConstants.None
            End If
            Return tmpID
        End Function

        Protected Function GetTypeWithName(ParamFileName As String) As eParamFileTypeConstants
            Me.OpenConnection()
            Dim foundRows As DataRow() = Me.ParamFileTable.Select("[Param_File_Name] = '" & ParamFileName & "'")
            Dim foundRow As DataRow
            Dim tmpID As eParamFileTypeConstants
            If foundRows.Length <> 0 Then
                foundRow = foundRows(0)
                tmpID = CType(foundRow.Item("Param_File_Type_ID"), eParamFileTypeConstants)
            Else
                tmpID = eParamFileTypeConstants.None
            End If
            Return tmpID
        End Function

        Protected Function GetDescriptionWithID(ID As Integer) As String
            Dim foundRows As DataRow() = Me.ParamFileTable.Select("[Param_File_ID] = " & ID)
            Dim tmpString As String
            If foundrows.Length <> 0 Then
                Dim foundRow = foundrows(0)
                tmpString = CStr(foundrow.Item("Param_File_Description"))
                If tmpString = "" Then
                    Return ""
                Else
                    Return tmpString
                End If
            Else
                Return Nothing
            End If
        End Function

        Protected Function GetFileRowWithID(ID As Integer) As DataRow             'Download
            Dim foundRow As DataRow = ParamFileTable.Rows.Find(ID)
            Return foundRow
        End Function

        ''' <summary>
        ''' Finds parameter file info for Sequest, X!Tandem, MSGF+, MSPathFinder, or MODPlus
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function GetAvailableParamSets() As DataTable
            Dim paramTableSQL As String =
              "SELECT " &
              "Param_File_ID as ID, " &
              "Param_File_Name AS Filename, " &
              "Param_File_Description as Diffs, " &
              "Param_File_Type_ID as Type_ID " &
              "FROM T_Param_Files " &
              "WHERE Param_File_Type_ID = " & eParamFileTypeConstants.Sequest &
              " or Param_File_Type_ID = " & eParamFileTypeConstants.XTandem &
              " or Param_File_Type_ID = " & eParamFileTypeConstants.MSGFDB &
              " or Param_File_Type_ID = " & eParamFileTypeConstants.MSPathFinder &
              " or Param_File_Type_ID = " & eParamFileTypeConstants.MODPlus

            Dim tmpIDTable = Me.GetTable(paramTableSQL)

            ''Load tmpIDTable
            Dim tmpID As Integer
            Dim tmpDiffs As String
            Dim tmpType As Integer

            Dim dr As DataRow

            For Each dr In tmpIDTable.Rows

                tmpType = DirectCast(dr.Item("Type_ID"), Integer)
                If tmpType = eParamFileTypeConstants.Sequest Then

                    tmpID = DirectCast(dr.Item("ID"), Integer)
                    tmpDiffs = DirectCast(dr.Item("Diffs"), String)
                    If tmpDiffs Is Nothing Then
                        Dim eParamFileTypeID As eParamFileTypeConstants
                        eParamFileTypeID = CType(tmpType, eParamFileTypeConstants)

                        tmpDiffs = DistillFeaturesFromParamSet(tmpID, eParamFileTypeID)

                        dr.Item("Diffs") = tmpDiffs
                        dr.AcceptChanges()
                    End If
                End If
            Next

            Return tmpIDTable

            'Need filtering code for tmpIDTable here...

        End Function

        Protected Function GetParamFileTypes() As DataTable
            Dim tmpTypeTable As DataTable
            Dim tableTypesSQL As String
            tableTypesSQL =
                "SELECT Param_File_Type_ID as ID, Param_File_Type AS Type " &
                "FROM T_Param_File_Types"

            tmpTypeTable = Me.GetTable(tableTypesSQL)

            Return tmpTypeTable

        End Function

        Protected Function DistillFeaturesFromParamSet(ParamSet As clsParams) As String       'Neither
            Dim templateColl As clsDMSParamStorage = WriteDataCollectionFromParamSet(Me.m_BaseLineParamSet)
            Dim checkColl As clsDMSParamStorage = WriteDataCollectionFromParamSet(ParamSet)


            Dim diffColl As clsDMSParamStorage = CompareDataCollections(templateColl, checkColl)
            Return Me.SummarizeDiffColl(diffColl)

        End Function

        Protected Function DistillFeaturesFromParamSet(ParamSetID As Integer, eParamFileTypeID As eParamFileTypeConstants) As String          'Common
            Dim p As clsParams = Me.GetParamSetWithID(ParamSetID, eParamFileTypeID)

            Return p.Description

        End Function

        Protected Function WriteDataCollectionFromParamSet(ParamSet As clsParams) As clsDMSParamStorage      'Upload
            Dim c = New clsDMSParamStorage()

            Dim pType As Type = ParamSet.GetType
            Dim tmpType As Type
            Dim pProps As PropertyInfo() = pType.GetProperties((BindingFlags.Public Or BindingFlags.Instance))
            Dim pProp As PropertyInfo

            Dim tmpName As String
            Dim tmpValue As String

            For Each pProp In pProps
                tmpName = pProp.Name
                tmpType = pProp.PropertyType
                If Me.m_AcceptableParams.Contains(tmpName) Then
                    If (tmpType.Name = "clsIonSeries") Then
                        c = ExpandIonSeries(ParamSet.IonSeries, c)

                    ElseIf (tmpType.Name = "clsIsoMods") Then
                        c = ExpandIsoTopicMods(ParamSet.IsotopicMods, c)

                    ElseIf tmpType.Name = "clsDynamicMods" Then
                        c = ExpandDynamicMods(ParamSet.DynamicMods, c, clsDMSParamStorage.ParamTypes.DynamicModification)

                    ElseIf tmpType.Name = "clsTermDynamicMods" Then
                        c = ExpandDynamicMods(ParamSet.TermDynamicMods, c, clsDMSParamStorage.ParamTypes.TermDynamicModification)

                    ElseIf (tmpType.Name = "clsStaticMods") Then
                        c = ExpandStaticMods(ParamSet.StaticModificationsList, c)

                    ElseIf Me.m_BasicParams.Contains(tmpName) Then
                        tmpValue = (pProp.GetValue(ParamSet, Nothing)).ToString
                        c.Add(tmpName, tmpValue.ToString, clsDMSParamStorage.ParamTypes.BasicParam)
                    ElseIf Me.m_AdvancedParams.Contains(tmpName) Then
                        tmpValue = (pProp.GetValue(ParamSet, Nothing)).ToString
                        c.Add(tmpName, tmpValue.ToString, clsDMSParamStorage.ParamTypes.AdvancedParam)
                    End If
                End If

            Next

            Return c

        End Function

        Protected Function UpdateParamSetFromDataCollection(dc As clsDMSParamStorage) As clsParams
            Dim p As New clsParams
            Dim tmpSpec As String
            Dim tmpValue As Object
            Dim tmpType As clsDMSParamStorage.ParamTypes
            Dim tmpTypeName As String

            'p = clsMainProcess.BaseLineParamSet
            p.LoadTemplate(clsMainProcess.TemplateFileName)
            Dim pType As Type = GetType(clsParams)
            Dim pFields As PropertyInfo() = pType.GetProperties(BindingFlags.Instance Or BindingFlags.Public)
            Dim pField As PropertyInfo = Nothing

            Dim ionType As Type = GetType(clsIonSeries)
            Dim ionFields As PropertyInfo() = ionType.GetProperties(BindingFlags.Instance Or BindingFlags.Public)
            Dim ionField As PropertyInfo

            Dim paramEntry As clsDMSParamStorage.ParamsEntry

            For Each paramEntry In dc
                tmpSpec = paramEntry.Specifier
                tmpValue = paramEntry.Value
                tmpType = paramEntry.Type

                If tmpType = clsDMSParamStorage.ParamTypes.BasicParam And
                    Me.m_BasicParams.Contains(tmpSpec) Then

                    For Each pField In pFields
                        If pField.Name = tmpSpec Then
                            tmpTypeName = pField.PropertyType.Name
                            If tmpTypeName = "Int32" Then
                                pField.SetValue(p, CType(tmpValue, Int32), Nothing)
                                Exit For
                            ElseIf tmpTypeName = "Single" Then
                                pField.SetValue(p, CType(tmpValue, Single), Nothing)
                                Exit For
                            ElseIf tmpTypeName = "String" Then
                                pField.SetValue(p, CType(tmpValue, String), Nothing)
                                Exit For
                            ElseIf tmpTypeName = "Boolean" Then
                                pField.SetValue(p, CType(tmpValue, Boolean), Nothing)
                                Exit For
                            ElseIf tmpTypeName = "MassTypeList" Then
                                pField.SetValue(p, CType([Enum].Parse(GetType(IBasicParams.MassTypeList), CStr(tmpValue), True), IBasicParams.MassTypeList), Nothing)
                                Exit For
                            ElseIf tmpTypeName = "MassUnitList" Then
                                pField.SetValue(p, CType([Enum].Parse(GetType(IAdvancedParams.MassUnitList), CStr(tmpValue), True), IAdvancedParams.MassUnitList), Nothing)
                            Else
                                Console.WriteLine(pField.PropertyType.Name.ToString)
                            End If

                            Exit For
                        End If
                    Next

                ElseIf tmpType = clsDMSParamStorage.ParamTypes.AdvancedParam And
                    Me.m_AdvancedParams.Contains(tmpSpec) Then

                    For Each pField In pFields
                        If pField.Name = tmpSpec Then
                            tmpTypeName = pField.PropertyType.Name
                            If tmpTypeName = "Int32" Then
                                pField.SetValue(p, CType(tmpValue, Int32), Nothing)
                                Exit For
                            ElseIf tmpTypeName = "Single" Then
                                pField.SetValue(p, CType(tmpValue, Single), Nothing)
                                Exit For
                            ElseIf tmpTypeName = "String" Then
                                pField.SetValue(p, CType(tmpValue, String), Nothing)
                                Exit For
                            ElseIf tmpTypeName = "Boolean" Then
                                pField.SetValue(p, CType(tmpValue, Boolean), Nothing)
                                Exit For
                            Else
                                Console.WriteLine(pField.PropertyType.Name.ToString)
                            End If

                            Exit For
                        End If
                    Next

                ElseIf tmpType = clsDMSParamStorage.ParamTypes.AdvancedParam And
                    Me.m_IonSeriesParams.Contains(tmpSpec) Then

                    For Each ionField In ionFields
                        If ionField.Name = tmpSpec Then
                            tmpTypeName = ionField.PropertyType.Name
                            If tmpTypeName = "Int32" Then
                                ionField.SetValue(p.IonSeries, CType(tmpValue, Int32), Nothing)
                                Exit For
                            ElseIf tmpTypeName = "Single" Then
                                ionField.SetValue(p.IonSeries, CType(tmpValue, Single), Nothing)
                                Exit For
                            ElseIf tmpTypeName = "String" Then
                                ionField.SetValue(p.IonSeries, CType(tmpValue, String), Nothing)
                                Exit For
                            ElseIf tmpTypeName = "Boolean" Then
                                ionField.SetValue(p.IonSeries, CType(tmpValue, Boolean), Nothing)
                                Exit For
                            Else
                                Console.WriteLine(pField.PropertyType.Name.ToString)
                            End If
                        End If
                    Next

                ElseIf tmpType = clsDMSParamStorage.ParamTypes.DynamicModification Then

                    p.DynamicMods.Add(tmpSpec.ToString, CDbl(Val(tmpValue.ToString)))
                ElseIf tmpType = clsDMSParamStorage.ParamTypes.StaticModification Then
                    p.StaticModificationsList.Add(tmpSpec.ToString, CDbl(Val(tmpValue.ToString)))
                ElseIf tmpType = clsDMSParamStorage.ParamTypes.IsotopicModification Then
                    p.IsotopicMods.Add(tmpSpec.ToString, CDbl(Val(tmpValue)))
                ElseIf tmpType = clsDMSParamStorage.ParamTypes.TermDynamicModification Then
                    p.TermDynamicMods.Add(tmpSpec.ToString, CDbl(Val(tmpValue)))
                End If
            Next

            Return p

        End Function

        Protected Function UpdateParamSetMember(
            ByRef ParamSet As clsParams,
            Specifier As String,
            Value As String) As Integer



        End Function

        Private Function ExpandDynamicMods(DynModSet As clsDynamicMods, ByRef ParamCollection As clsDMSParamStorage, eDynModType As clsDMSParamStorage.ParamTypes) As clsDMSParamStorage
            Dim maxCount As Integer = DynModSet.Count
            Dim counter As Integer
            Dim tmpName As String
            Dim tmpValue As String

            If maxCount = 0 Then
                Return ParamCollection
            End If

            If eDynModType <> clsDMSParamStorage.ParamTypes.DynamicModification And
               eDynModType <> clsDMSParamStorage.ParamTypes.TermDynamicModification Then
                ' This is unexpected; force eDynModType to be .DynamicModification
                eDynModType = clsDMSParamStorage.ParamTypes.DynamicModification
            End If

            For counter = 1 To maxCount
                tmpName = DynModSet.Dyn_Mod_n_AAList(counter)
                tmpValue = Format(DynModSet.Dyn_Mod_n_MassDiff(counter), "0.00000")
                ParamCollection.Add(tmpName, tmpValue, eDynModType)
            Next

            Return ParamCollection

        End Function

        Private Function ExpandStaticMods(StatModSet As clsStaticMods, ByRef ParamCollection As clsDMSParamStorage) As clsDMSParamStorage
            Dim maxCount As Integer = StatModSet.Count
            Dim counter As Integer
            Dim tmpName As String
            Dim tmpValue As String

            If maxCount = 0 Then
                Return ParamCollection
            End If

            For counter = 0 To maxCount - 1
                tmpName = StatModSet.GetResidue(counter)
                tmpValue = StatModSet.GetMassDiff(counter)
                ParamCollection.Add(tmpName, tmpValue, clsDMSParamStorage.ParamTypes.StaticModification)
            Next

            Return ParamCollection

        End Function

        Private Function ExpandIsoTopicMods(IsoModSet As clsIsoMods, ByRef ParamCollection As clsDMSParamStorage) As clsDMSParamStorage
            Dim maxCount As Integer = IsoModSet.Count
            Dim counter As Integer
            Dim tmpName As String
            Dim tmpValue As String

            If maxCount = 0 Then
                Return ParamCollection
            End If

            For counter = 0 To maxCount - 1
                tmpName = IsoModSet.GetAtom(counter)
                tmpValue = IsoModSet.GetMassDiff(counter)
                ParamCollection.Add(tmpName, tmpValue, clsDMSParamStorage.ParamTypes.IsotopicModification)
            Next

            Return ParamCollection
        End Function

        Private Function ExpandIonSeries(IonSeriesSet As clsIonSeries, ByRef ParamCollection As clsDMSParamStorage) As clsDMSParamStorage
            Dim pType As Type = GetType(clsIonSeries)
            Dim pFields As PropertyInfo() = pType.GetProperties(BindingFlags.Instance Or BindingFlags.Public)
            Dim pField As PropertyInfo

            For Each pField In pFields
                Dim tmpName As String = pField.Name
                Dim tmpValue As String = pField.GetValue(IonSeriesSet, Nothing).ToString
                ParamCollection.Add(tmpName, tmpValue, clsDMSParamStorage.ParamTypes.AdvancedParam)
            Next

            Return ParamCollection
        End Function

        Protected Function DoesParamSetNameExist(paramSetName As String) As Boolean       'Common

            Dim eParamFileType As eParamFileTypeConstants
            eParamFileType = GetTypeWithName(paramSetName)

            If eParamFileType = eParamFileTypeConstants.Unknown Then
                Console.WriteLine("Parameter file " & paramSetName & "  was not found in table " & Param_File_Table)
                Return False
            End If

            If eParamFileType <> eParamFileTypeConstants.Sequest Then
                ' This param file type is not supported for export from DMS
                Dim paramFileTypeName As String = [Enum].GetName(GetType(eParamFileTypeConstants), eParamFileType)
                If String.IsNullOrEmpty(paramFileTypeName) Then
                    paramFileTypeName = "Unknown"
                End If

                Console.WriteLine("Parameter file " & paramSetName & " is of type " & paramFileTypeName & ", which isn't support for export from DMS")
                Return False
            End If

            Dim tmpID As Integer = GetIDWithName(paramSetName, eParamFileType)
            If tmpID < 0 Then
                Console.WriteLine("Parameter file " & paramSetName & " with type ID " & eParamFileType.ToString() & " was not found in table " & Param_File_Table)
                Return False
            End If

            Return True

        End Function

        Protected Function DoesParamSetIDExist(paramSetID As Integer) As Boolean                  'Common

            Dim IDExists As Boolean = Me.ParamFileTable.Rows.Contains(paramSetID)
            Return IDExists
        End Function

        Protected Function CompareParamSets(ByRef templateSet As clsParams, ByRef checkSet As clsParams) As String         'Neither
            Dim diffCollection As clsDMSParamStorage = GetDiffColl(templateSet, checkSet)
            Return SummarizeDiffColl(diffCollection)
        End Function

        Protected Function GetDiffColl(ByRef templateSet As clsParams, ByRef checkSet As clsParams) As clsDMSParamStorage
            Dim templateColl As clsDMSParamStorage = Me.WriteDataCollectionFromParamSet(templateSet)
            Dim checkColl As clsDMSParamStorage = Me.WriteDataCollectionFromParamSet(checkSet)

            Dim diffCollection As clsDMSParamStorage = CompareDataCollections(templateColl, checkColl)
            Return diffCollection
        End Function

        Protected Function SummarizeDiffColl(ByRef diffColl As clsDMSParamStorage) As String

            Dim maxIndex As Integer = diffColl.Count - 1
            Dim index As Integer
            Dim tmpString As String

            Dim tmpIsoMods = ""
            Dim tmpOtherParams = ""

            Dim tmpDynModsList As Queue = Nothing
            Dim tmpTermDynModsList As Queue = Nothing
            Dim tmpStatModsList As Queue = Nothing
            Dim tmpIsoModsList As Queue = Nothing
            Dim tmpOtherParamsList As Queue = Nothing

            Dim intDynModCount = 0
            Dim intTermDynModCount = 0

            For index = 0 To maxIndex
                Dim tmpType = diffColl.Item(index).Type
                Dim tmpSpec = diffColl.Item(index).Specifier
                Dim tmpValue = diffColl.Item(index).Value

                Dim dblValue As Double
                Dim tmpValueFormatted As String
                Dim tmpSign As String

                If Double.TryParse(tmpValue, dblValue) Then
                    tmpValueFormatted = dblValue.ToString("0.0000")
                    If dblValue > 0 Then
                        tmpSign = "+"
                    Else
                        tmpSign = ""
                    End If
                Else
                    tmpValueFormatted = String.Copy(tmpValue)
                    tmpSign = ""
                End If

                If tmpType = clsDMSParamStorage.ParamTypes.StaticModification Then

                    If tmpStatModsList Is Nothing Then
                        tmpStatModsList = New Queue()
                        tmpStatModsList.Enqueue("Static Mods: ")
                    End If
                    tmpStatModsList.Enqueue(tmpSpec + " (" + tmpSign + tmpValueFormatted + ")")


                ElseIf tmpType = clsDMSParamStorage.ParamTypes.DynamicModification Then

                    If tmpDynModsList Is Nothing Then
                        tmpDynModsList = New Queue()
                        tmpDynModsList.Enqueue("Dynamic Mods: ")
                    End If
                    tmpDynModsList.Enqueue(tmpSpec + " (" + tmpSign + tmpValueFormatted + ")")

                    intDynModCount += 1


                ElseIf tmpType = clsDMSParamStorage.ParamTypes.TermDynamicModification Then

                    If tmpSpec = "<" Then
                        tmpSpec = "N-Term Peptide"
                    ElseIf tmpSpec = ">" Then
                        tmpSpec = "C-Term Peptide"
                    End If

                    If tmpTermDynModsList Is Nothing Then
                        tmpTermDynModsList = New Queue()
                        tmpTermDynModsList.Enqueue("PepTerm Dynamic Mods: ")
                    End If
                    tmpTermDynModsList.Enqueue(tmpSpec + " (" + tmpSign + tmpValueFormatted + ")")

                    intTermDynModCount += 1


                ElseIf tmpType = clsDMSParamStorage.ParamTypes.IsotopicModification Then

                    If tmpIsoMods = "" Then
                        tmpIsoMods = "Isotopic Mods: "
                    End If

                    If tmpIsoModsList Is Nothing Then
                        tmpIsoModsList = New Queue()
                        tmpIsoModsList.Enqueue(tmpIsoMods)
                    End If
                    tmpIsoModsList.Enqueue(tmpSpec + " (" + tmpSign + tmpValueFormatted + ")")


                Else
                    If tmpOtherParams = "" Then
                        tmpOtherParams = "Other Parameters: "
                    End If

                    If tmpOtherParamsList Is Nothing Then
                        tmpOtherParamsList = New Queue()
                        tmpOtherParamsList.Enqueue(tmpOtherParams)
                    End If
                    tmpOtherParamsList.Enqueue(tmpSpec & " = " & tmpValue)

                End If
            Next

            ' Build the string describing the mods
            tmpString = ""

            tmpString = MakeListOfMods(tmpString, tmpDynModsList, True)

            tmpString = MakeListOfMods(tmpString, tmpTermDynModsList, False)
            If intDynModCount = 0 And intTermDynModCount > 0 Then
                tmpString = "Dynamic Mods: " & tmpString
            End If

            tmpString = MakeListOfMods(tmpString, tmpStatModsList, True)
            tmpString = MakeListOfMods(tmpString, tmpIsoModsList, True)
            tmpString = MakeListOfMods(tmpString, tmpOtherParamsList, True)


            If tmpString = Nothing OrElse tmpString.Length = 0 Then
                tmpString = " --No Change-- "
            End If

            Return tmpString


        End Function

        Private Function MakeListOfMods(strModDescriptionPrevious As String,
                                        objModList As Queue,
                                        blnAddTitlePrefix As Boolean) As String

            If strModDescriptionPrevious Is Nothing Then strModDescriptionPrevious = ""
            If objModList Is Nothing Then
                Return strModDescriptionPrevious
            End If

            If objModList.Count > 0 Then
                If strModDescriptionPrevious.Length > 0 Then strModDescriptionPrevious += ", "

                Dim tmpElement = ""
                Dim elementTitle = objModList.Dequeue.ToString
                While objModList.Count > 0
                    Dim subItem = objModList.Dequeue().ToString
                    If tmpElement.Length > 0 Then tmpElement += ", "
                    tmpElement += subItem
                End While

                If blnAddTitlePrefix Then
                    strModDescriptionPrevious += elementTitle + tmpElement
                Else
                    strModDescriptionPrevious += tmpElement
                End If

            End If

            Return strModDescriptionPrevious

        End Function

        Protected Function CompareDataCollections(templateColl As clsDMSParamStorage, checkColl As clsDMSParamStorage) As clsDMSParamStorage        'Neither
            Dim diffColl As New clsDMSParamStorage()
            Dim maxIndex As Integer = checkColl.Count - 1

            For checkIndex = 0 To maxIndex
                Dim tmpCType = checkColl.Item(checkIndex).Type
                Dim tmpCSpec = checkColl.Item(checkIndex).Specifier
                Dim tmpCVal = checkColl.Item(checkIndex).Value


                Dim templateIndex = templateColl.IndexOf(tmpCSpec, tmpCType)

                If templateIndex >= 0 Then
                    Dim tmpTType = templateColl.Item(templateIndex).Type
                    Dim tmpTSpec = templateColl.Item(templateIndex).Specifier
                    Dim tmpTVal = templateColl.Item(templateIndex).Value
                    ' Dim tmpTemp = tmpTType.ToString & " - " & tmpTSpec & " = " & tmpTVal
                    ' Dim tmpCheck = tmpCType.ToString & " - " & tmpCSpec & " = " & tmpCVal

                    If tmpTType.ToString & tmpTSpec = tmpCType.ToString & tmpCSpec Then
                        If tmpTVal.Equals(tmpCVal) Then

                        Else
                            diffColl.Add(tmpCSpec, tmpCVal, tmpTType)
                        End If
                    End If
                Else
                    diffColl.Add(tmpCSpec, tmpCVal, tmpCType)
                End If


            Next

            Return diffColl

        End Function

#End Region

    End Class

End Namespace