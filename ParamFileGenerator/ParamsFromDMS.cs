Imports System.Collections.Generic
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports PRISMDatabaseUtils

Namespace DownloadParams

    Public Class ParamsFromDMS
        Inherits DBTask

        ' Ignore Spelling: diffs, mc

        Private Const Param_File_Table As String = "T_Param_Files"
        Private Const Param_Entry_Table As String = "T_Param_Entries"
        Private Const Param_Mass_Mods_Table As String = "T_Param_File_Mass_Mods"
        Private Const Mass_Corr_Factors As String = "T_Mass_Correction_Factors"
        Private Const Residues_Table As String = "T_Residues"

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
            TopFD = 1031
            TopPIC = 1032
            MSFragger = 1033
            MaxQuant = 1034
        End Enum

        Private m_ID As Integer
        Private m_Name As String
        Private m_ParamFileType As eParamFileTypeConstants
        Private m_Params As Params

        ''' <summary>
        ''' Parameter file table
        ''' </summary>
        Private m_ParamFileTable As DataTable
        Private m_ParamEntryTable As DataTable

        Private ReadOnly m_BaseLineParamSet As Params
        Private ReadOnly m_AcceptableParams As List(Of String)
        Private ReadOnly m_BasicParams As List(Of String)
        Private ReadOnly m_AdvancedParams As List(Of String)
        Private ReadOnly m_IonSeriesParams As List(Of String)
        Private m_MassMods As DataTable

        Public ReadOnly Property ParamFileTableLoaded As Boolean
            Get
                Return m_ParamFileTable IsNot Nothing AndAlso m_ParamFileTable.Rows.Count > 0
            End Get
        End Property

#Disable Warning BC40028 ' Type of parameter is not CLS-compliant
        Public Sub New(dbTools As IDBTools)
#Enable Warning BC40028 ' Type of parameter is not CLS-compliant
            MyBase.New(dbTools)

            m_AcceptableParams = LoadAcceptableParamList()
            m_BasicParams = LoadBasicParams()
            m_AdvancedParams = LoadAdvancedParams()
            m_IonSeriesParams = LoadIonSeriesParams()
            m_BaseLineParamSet = MainProcess.BaseLineParamSet

            Dim success = GetParamsFromDMS()
            If Not success Then
                Throw New Exception("Unable to obtain data from " & Param_File_Table & " and/or " & Param_Entry_Table)
            End If
        End Sub

        Public Sub RefreshParamsFromDMS()
            GetParamsFromDMS()
        End Sub

        Public Function ReadParamsFromDMS(paramSetName As String) As Params
            'Retrieve ID number first, then use the same procedure as below
            m_Name = paramSetName

            m_ParamFileType = GetTypeWithName(paramSetName)

            If m_ParamFileType = eParamFileTypeConstants.Unknown Then
                Throw New Exception("Parameter file " & paramSetName & " was not found in table " & Param_File_Table)
            End If

            If m_ParamFileType <> eParamFileTypeConstants.Sequest Then
                ' This param file type is not supported for export from DMS
                Dim paramFileTypeName As String = [Enum].GetName(GetType(eParamFileTypeConstants), m_ParamFileType)
                If String.IsNullOrEmpty(paramFileTypeName) Then
                    paramFileTypeName = "Unknown"
                End If

                Throw New NotSupportedException("Parameter file " & paramSetName & " is of type " & paramFileTypeName & ", which isn't support for export from DMS")
            End If

            m_ID = GetIDWithName(m_Name, m_ParamFileType)

            m_Params = RetrieveParams(m_ID, m_ParamFileType)
            Return m_Params

        End Function

        Public Function ReadParamsFromDMS(paramSetID As Integer) As Params
            m_ID = paramSetID
            m_ParamFileType = GetTypeWithID(m_ID)

            If m_ParamFileType = eParamFileTypeConstants.Unknown Then
                Throw New Exception("Parameter file ID " & paramSetID & " was not found in table " & Param_File_Table)
            End If

            If m_ParamFileType <> eParamFileTypeConstants.Sequest Then
                ' This param file type is not supported for export from DMS
                Throw New NotSupportedException("Parameter file ID " & paramSetID & " is of type " & [Enum].GetName(GetType(eParamFileTypeConstants), m_ParamFileType) & ", which isn't support for export from DMS")
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

        Public Function ParamSetNameExists(paramSetName As String) As Boolean
            Return DoesParamSetNameExist(paramSetName)
        End Function

        Public Function ParamSetIDExists(paramSetID As Integer) As Boolean
            Return DoesParamSetIDExist(paramSetID)
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

        Private Function LoadAcceptableParamList() As List(Of String)
            Dim paramEnum() As String = [Enum].GetNames(GetType(AcceptableParams))
            Dim paramList As New List(Of String)
            For Each param In paramEnum
                paramList.Add(param)
            Next
            Return paramList
        End Function

        Private Function LoadBasicParams() As List(Of String)
            Dim paramEnum() As String = [Enum].GetNames(GetType(BasicParams))
            Dim paramList As New List(Of String)
            For Each param In paramEnum
                paramList.Add(param)
            Next
            Return paramList
        End Function

        Private Function LoadAdvancedParams() As List(Of String)
            Dim paramEnum() As String = [Enum].GetNames(GetType(AdvancedParams))
            Dim paramList As New List(Of String)
            For Each param In paramEnum
                paramList.Add(param)
            Next
            Return paramList
        End Function

        Private Function LoadIonSeriesParams() As List(Of String)
            Dim paramEnum() As String = [Enum].GetNames(GetType(IonSeriesParams))
            Dim paramList As New List(Of String)
            For Each param In paramEnum
                paramList.Add(param)
            Next
            Return paramList
        End Function

        Private Function GetParamsFromDMS() As Boolean

            ' SQL to grab param file table
            ' The ID column is named Param_File_ID
            Dim query1 = "SELECT Param_File_ID, Param_File_Name, Param_File_Description, Param_File_Type_ID " &
                         "FROM " & Param_File_Table
            ' Optional: " WHERE [Param_File_Type_ID] = 1000"

            m_ParamFileTable = GetTable(query1)

            ' SQL to grab param entry table
            ' The ID column is named Param_Entry_ID
            Dim query2 = "SELECT Param_Entry_ID, Entry_Sequence_Order, Entry_Type, Entry_Specifier, Entry_Value, Param_File_ID " &
                         "FROM " & Param_Entry_Table & " " &
                         "WHERE [Entry_Type] not like '%Modification'"

            m_ParamEntryTable = GetTable(query2)

            Return True

        End Function

        Private Function RetrieveParams(paramSetID As Integer, eParamFileType As eParamFileTypeConstants) As Params
            Dim p As Params = GetParamSetWithID(paramSetID, eParamFileType)
            Return p
        End Function

        'TODO Fix this function for new mod handling
        Protected Function GetParamSetWithID(paramSetID As Integer, eParamFileType As eParamFileTypeConstants, Optional DisableMassLookup As Boolean = False) As Params
            Dim matchingRow As DataRow = Nothing

            If Not GetParamFileRowByID(paramSetID, matchingRow) Then
                ' Match not found
                Return New Params()
            End If

            Dim foundRows As DataRow() = m_ParamEntryTable.Select("[Param_File_ID] = " & paramSetID, "[Entry_Sequence_Order]")

            Dim storageSet As DMSParamStorage = MakeStorageClassFromTableRowSet(foundRows)

            If Not DisableMassLookup Then
                storageSet = GetMassModsFromDMS(paramSetID, eParamFileType, storageSet)
            End If

            Dim p As Params = UpdateParamSetFromDataCollection(storageSet)
            p.FileName = DirectCast(matchingRow.Item("Param_File_Name"), String)
            p.Description = SummarizeDiffColl(storageSet)

            For Each paramRow As DataRow In foundRows
                p.AddLoadedParamName(paramRow.Item("Entry_Specifier").ToString, paramRow.Item("Entry_Value").ToString)
            Next

            Return p
        End Function

        Private Function MakeStorageClassFromTableRowSet(foundRows As IEnumerable(Of DataRow)) As DMSParamStorage
            Dim foundRow As DataRow
            Dim storageClass As New DMSParamStorage
            Dim tmpSpec As String
            Dim tmpValue As String

            Dim tmpType As DMSParamStorage.ParamTypes

            For Each foundRow In foundRows
                tmpSpec = DirectCast(foundRow.Item("Entry_Specifier"), String)
                tmpValue = DirectCast(foundRow.Item("Entry_Value"), String)
                tmpType = DirectCast([Enum].Parse(GetType(DMSParamStorage.ParamTypes), foundRow.Item("Entry_Type").ToString), DMSParamStorage.ParamTypes)

                storageClass.Add(tmpSpec, tmpValue, tmpType)
            Next


            Return storageClass

        End Function

        Private Function GetMassModsFromDMS(paramSetID As Integer, eParamFileType As eParamFileTypeConstants, ByRef params As DMSParamStorage) As DMSParamStorage
            Const MaxDynMods = 15

            Dim foundRow As DataRow
            Dim foundRows() As DataRow
            Dim tmpSpec As String
            Dim tmpValue As String

            Dim tmpType As DMSParamStorage.ParamTypes

            'If m_MassMods Is Nothing Or m_MassMods.Rows.Count = 0 Then
            Dim SQL As String

            SQL = "SELECT mm.Mod_Type_Symbol as Mod_Type_Symbol, r.Residue_Symbol as Residue_Symbol, " &
              "mc.Monoisotopic_Mass as Monoisotopic_Mass, " &
              "mm.Local_Symbol_ID as Local_Symbol_ID, mc.Affected_Atom as Affected_Atom " &
              "FROM " & Param_Mass_Mods_Table & " mm INNER JOIN " &
              Mass_Corr_Factors & " mc ON mm.Mass_Correction_ID = mc.Mass_Correction_ID INNER JOIN " &
              Residues_Table & " r ON mm.Residue_ID = r.Residue_ID " &
              "WHERE mm.Param_File_ID = " & paramSetID

            m_MassMods = GetTable(SQL)

            'Look for Dynamic mods

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
                    tmpValue = foundRows(0).Item("Monoisotopic_Mass").ToString
                    tmpType = DMSParamStorage.ParamTypes.DynamicModification
                    params.Add(tmpSpec, tmpValue, tmpType)
                End If

            Next

            'Find N-Term Dynamic Mods
            foundRows = m_MassMods.Select("[Mod_Type_Symbol] = 'D' AND [Residue_Symbol] = '<'")
            If foundRows.Length > 0 Then
                tmpSpec = GetDynModSpecifier(foundRows)
                tmpValue = foundRows(0).Item("Monoisotopic_Mass").ToString
                tmpType = DMSParamStorage.ParamTypes.TermDynamicModification
                params.Add(tmpSpec, tmpValue, tmpType)
            End If

            'Find C-Term Dynamic Mods
            foundRows = m_MassMods.Select("[Mod_Type_Symbol] = 'D' AND [Residue_Symbol] = '>'")
            If foundRows.Length > 0 Then
                tmpSpec = GetDynModSpecifier(foundRows)
                tmpValue = foundRows(0).Item("Monoisotopic_Mass").ToString
                tmpType = DMSParamStorage.ParamTypes.TermDynamicModification
                params.Add(tmpSpec, tmpValue, tmpType)
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
                tmpValue = foundRow.Item("Monoisotopic_Mass").ToString
                tmpType = DMSParamStorage.ParamTypes.StaticModification
                params.Add(tmpSpec, tmpValue, tmpType)
            Next

            'TODO Still need code to handle import/export of isotopic mods

            foundRows = m_MassMods.Select("[Mod_Type_Symbol] = 'I'")

            For Each foundRow In foundRows
                tmpSpec = foundRow.Item("Affected_Atom").ToString
                tmpValue = foundRow.Item("Monoisotopic_Mass").ToString
                tmpType = DMSParamStorage.ParamTypes.IsotopicModification
                params.Add(tmpSpec, tmpValue, tmpType)
            Next


            Return params

        End Function

        Private Function GetDynModSpecifier(rowSet As DataRow()) As String

            Dim tmpSpec = ""

            If rowSet.Length > 0 Then               'We have dynamic mods
                For Each foundRow As DataRow In rowSet
                    tmpSpec &= foundRow.Item("Residue_Symbol").ToString
                Next
                Return tmpSpec
            Else
                Return Nothing
            End If
        End Function

        Private Function GetIDWithName(Name As String, eParamFileType As eParamFileTypeConstants) As Integer

            Dim foundRows As DataRow() = m_ParamFileTable.Select("[Param_File_Name] = '" & Name & "' AND [Param_File_Type_ID] = " & eParamFileType)
            Dim foundRow As DataRow
            Dim tmpID As Integer
            If foundRows.Length > 0 Then
                foundRow = foundRows(0)
                tmpID = CInt(foundRow.Item("Param_File_ID"))
            Else
                tmpID = -1
            End If
            Return tmpID
        End Function

        Private Function GetTypeWithID(paramFileID As Integer) As eParamFileTypeConstants
            Dim foundRows As DataRow() = m_ParamFileTable.Select("[Param_File_ID] = " & paramFileID.ToString())
            Dim foundRow As DataRow
            Dim tmpID As eParamFileTypeConstants
            If foundRows.Length > 0 Then
                foundRow = foundRows(0)
                tmpID = CType(foundRow.Item("Param_File_Type_ID"), eParamFileTypeConstants)
            Else
                tmpID = eParamFileTypeConstants.None
            End If
            Return tmpID
        End Function

        Private Function GetTypeWithName(paramFileName As String) As eParamFileTypeConstants

            Dim foundRows As DataRow() = m_ParamFileTable.Select("[Param_File_Name] = '" & paramFileName & "'")
            Dim foundRow As DataRow
            Dim tmpID As eParamFileTypeConstants
            If foundRows.Length > 0 Then
                foundRow = foundRows(0)
                tmpID = CType(foundRow.Item("Param_File_Type_ID"), eParamFileTypeConstants)
            Else
                tmpID = eParamFileTypeConstants.None
            End If
            Return tmpID
        End Function

        Private Function GetDescriptionWithID(paramFileID As Integer) As String
            Dim matchingRow As DataRow = Nothing

            If GetParamFileRowByID(paramFileID, matchingRow) Then
                Dim tmpString = CStr(matchingRow.Item("Param_File_Description"))
                If String.IsNullOrWhiteSpace(tmpString) Then
                    Return String.Empty
                Else
                    Return tmpString
                End If
            Else
                Return String.Empty
            End If
        End Function

        ''' <summary>
        ''' Finds the row in m_ParamFileTable with the given parameter file ID
        ''' </summary>
        ''' <param name="paramFileID"></param>
        ''' <param name="matchingRow">The row if found, otherwise null</param>
        ''' <returns>True if the parameter file was found, otherwise false</returns>
        Private Function GetParamFileRowByID(paramFileID As Integer, <Out> ByRef matchingRow As DataRow) As Boolean

            Dim foundRows As DataRow() = m_ParamFileTable.Select("[Param_File_ID] = " & paramFileID)

            If foundRows.Length > 0 Then
                matchingRow = foundRows(0)
                Return True
            End If

            matchingRow = Nothing
            Return False
        End Function

        ''' <summary>
        ''' Finds parameter file info for SEQUEST, X!Tandem, MSGF+, MSPathFinder, MODPlus, TopPIC, MSFragger, or MaxQuant
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetAvailableParamSets() As DataTable
            Dim paramTableSQL As String =
              "SELECT " &
              "  Param_File_ID as ID, " &
              "  Param_File_Name AS Filename, " &
              "  Param_File_Description as Diffs, " &
              "  Param_File_Type_ID as Type_ID " &
              "FROM T_Param_Files " &
              "WHERE Param_File_Type_ID = " & eParamFileTypeConstants.Sequest &
              "   or Param_File_Type_ID = " & eParamFileTypeConstants.XTandem &
              "   or Param_File_Type_ID = " & eParamFileTypeConstants.MSGFDB &
              "   or Param_File_Type_ID = " & eParamFileTypeConstants.MSPathFinder &
              "   or Param_File_Type_ID = " & eParamFileTypeConstants.MODPlus &
              "   or Param_File_Type_ID = " & eParamFileTypeConstants.TopPIC &
              "   or Param_File_Type_ID = " & eParamFileTypeConstants.MSFragger &
              "   or Param_File_Type_ID = " & eParamFileTypeConstants.MaxQuant

            Dim tmpIDTable = GetTable(paramTableSQL)

            ''Load tmpIDTable
            Dim tmpID As Integer
            Dim tmpDiffs As String
            Dim tmpType As Integer

            For Each dr As DataRow In tmpIDTable.Rows

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

        Private Function GetParamFileTypes() As DataTable
            Dim tmpTypeTable As DataTable
            Dim tableTypesSQL As String
            tableTypesSQL =
                "SELECT Param_File_Type_ID as ID, Param_File_Type AS Type " &
                "FROM T_Param_File_Types"

            tmpTypeTable = GetTable(tableTypesSQL)

            Return tmpTypeTable

        End Function

        Protected Function DistillFeaturesFromParamSet(paramSet As Params) As String
            Dim templateColl As DMSParamStorage = WriteDataCollectionFromParamSet(m_BaseLineParamSet)
            Dim checkColl As DMSParamStorage = WriteDataCollectionFromParamSet(paramSet)


            Dim diffColl As DMSParamStorage = CompareDataCollections(templateColl, checkColl)
            Return SummarizeDiffColl(diffColl)

        End Function

        Protected Function DistillFeaturesFromParamSet(paramSetID As Integer, eParamFileTypeID As eParamFileTypeConstants) As String
            Dim p As Params = GetParamSetWithID(paramSetID, eParamFileTypeID)

            Return p.Description

        End Function

        Private Function WriteDataCollectionFromParamSet(paramSet As Params) As DMSParamStorage
            Dim c = New DMSParamStorage()

            Dim pType As Type = paramSet.GetType
            Dim tmpType As Type
            Dim pProps As PropertyInfo() = pType.GetProperties((BindingFlags.Public Or BindingFlags.Instance))
            Dim pProp As PropertyInfo

            Dim tmpName As String
            Dim tmpValue As String

            For Each pProp In pProps
                tmpName = pProp.Name
                tmpType = pProp.PropertyType
                If m_AcceptableParams.Contains(tmpName) Then
                    If (tmpType.Name = "IonSeries") Then
                        c = ExpandIonSeries(paramSet.IonSeries, c)

                    ElseIf (tmpType.Name = "IsoMods") Then
                        c = ExpandIsoTopicMods(paramSet.IsotopicMods, c)

                    ElseIf tmpType.Name = "DynamicMods" Then
                        c = ExpandDynamicMods(paramSet.DynamicMods, c, DMSParamStorage.ParamTypes.DynamicModification)

                    ElseIf tmpType.Name = "TermDynamicMods" Then
                        c = ExpandDynamicMods(paramSet.TermDynamicMods, c, DMSParamStorage.ParamTypes.TermDynamicModification)

                    ElseIf (tmpType.Name = "StaticMods") Then
                        c = ExpandStaticMods(paramSet.StaticModificationsList, c)

                    ElseIf m_BasicParams.Contains(tmpName) Then
                        tmpValue = (pProp.GetValue(paramSet, Nothing)).ToString
                        c.Add(tmpName, tmpValue.ToString, DMSParamStorage.ParamTypes.BasicParam)
                    ElseIf m_AdvancedParams.Contains(tmpName) Then
                        tmpValue = (pProp.GetValue(paramSet, Nothing)).ToString
                        c.Add(tmpName, tmpValue.ToString, DMSParamStorage.ParamTypes.AdvancedParam)
                    End If
                End If

            Next

            Return c

        End Function

        Private Function UpdateParamSetFromDataCollection(dc As DMSParamStorage) As Params
            Dim p As New Params
            Dim tmpSpec As String
            Dim tmpValue As Object
            Dim tmpType As DMSParamStorage.ParamTypes
            Dim tmpTypeName As String

            'p = MainProcess.BaseLineParamSet
            p.LoadTemplate(MainProcess.TemplateFileName)
            Dim pType As Type = GetType(Params)
            Dim pFields As PropertyInfo() = pType.GetProperties(BindingFlags.Instance Or BindingFlags.Public)
            Dim pField As PropertyInfo = Nothing

            Dim ionType As Type = GetType(IonSeries)
            Dim ionFields As PropertyInfo() = ionType.GetProperties(BindingFlags.Instance Or BindingFlags.Public)
            Dim ionField As PropertyInfo

            Dim paramEntry As DMSParamStorage.ParamsEntry

            For Each paramEntry In dc
                tmpSpec = paramEntry.Specifier
                tmpValue = paramEntry.Value
                tmpType = paramEntry.Type

                If tmpType = DMSParamStorage.ParamTypes.BasicParam And
                    m_BasicParams.Contains(tmpSpec) Then

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

                ElseIf tmpType = DMSParamStorage.ParamTypes.AdvancedParam And
                    m_AdvancedParams.Contains(tmpSpec) Then

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

                ElseIf tmpType = DMSParamStorage.ParamTypes.AdvancedParam And
                    m_IonSeriesParams.Contains(tmpSpec) Then

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

                ElseIf tmpType = DMSParamStorage.ParamTypes.DynamicModification Then

                    p.DynamicMods.Add(tmpSpec.ToString, CDbl(Val(tmpValue.ToString)))
                ElseIf tmpType = DMSParamStorage.ParamTypes.StaticModification Then
                    p.StaticModificationsList.Add(tmpSpec.ToString, CDbl(Val(tmpValue.ToString)))
                ElseIf tmpType = DMSParamStorage.ParamTypes.IsotopicModification Then
                    p.IsotopicMods.Add(tmpSpec.ToString, CDbl(Val(tmpValue)))
                ElseIf tmpType = DMSParamStorage.ParamTypes.TermDynamicModification Then
                    p.TermDynamicMods.Add(tmpSpec.ToString, CDbl(Val(tmpValue)))
                End If
            Next

            Return p

        End Function

        Private Function ExpandDynamicMods(DynModSet As DynamicMods, ByRef ParamCollection As DMSParamStorage, eDynModType As DMSParamStorage.ParamTypes) As DMSParamStorage
            Dim maxCount As Integer = DynModSet.Count
            Dim counter As Integer
            Dim tmpName As String
            Dim tmpValue As String

            If maxCount = 0 Then
                Return ParamCollection
            End If

            If eDynModType <> DMSParamStorage.ParamTypes.DynamicModification And
               eDynModType <> DMSParamStorage.ParamTypes.TermDynamicModification Then
                ' This is unexpected; force eDynModType to be .DynamicModification
                eDynModType = DMSParamStorage.ParamTypes.DynamicModification
            End If

            For counter = 1 To maxCount
                tmpName = DynModSet.Dyn_Mod_n_AAList(counter)
                tmpValue = Format(DynModSet.Dyn_Mod_n_MassDiff(counter), "0.00000")
                ParamCollection.Add(tmpName, tmpValue, eDynModType)
            Next

            Return ParamCollection

        End Function

        Private Function ExpandStaticMods(StatModSet As StaticMods, ByRef ParamCollection As DMSParamStorage) As DMSParamStorage
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
                ParamCollection.Add(tmpName, tmpValue, DMSParamStorage.ParamTypes.StaticModification)
            Next

            Return ParamCollection

        End Function

        Private Function ExpandIsoTopicMods(IsoModSet As IsoMods, ByRef ParamCollection As DMSParamStorage) As DMSParamStorage
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
                ParamCollection.Add(tmpName, tmpValue, DMSParamStorage.ParamTypes.IsotopicModification)
            Next

            Return ParamCollection
        End Function

        Private Function ExpandIonSeries(IonSeriesSet As IonSeries, ByRef ParamCollection As DMSParamStorage) As DMSParamStorage
            Dim pType As Type = GetType(IonSeries)
            Dim pFields As PropertyInfo() = pType.GetProperties(BindingFlags.Instance Or BindingFlags.Public)
            Dim pField As PropertyInfo

            For Each pField In pFields
                Dim tmpName As String = pField.Name
                Dim tmpValue As String = pField.GetValue(IonSeriesSet, Nothing).ToString
                ParamCollection.Add(tmpName, tmpValue, DMSParamStorage.ParamTypes.AdvancedParam)
            Next

            Return ParamCollection
        End Function

        Private Function DoesParamSetNameExist(paramSetName As String) As Boolean

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

                Console.WriteLine("Parameter file " & paramSetName & " is of type " & paramFileTypeName & ", which isn't supported for export from DMS")
                Return False
            End If

            Dim tmpID As Integer = GetIDWithName(paramSetName, eParamFileType)
            If tmpID < 0 Then
                Console.WriteLine("Parameter file " & paramSetName & " with type ID " & eParamFileType.ToString() & " was not found in table " & Param_File_Table)
                Return False
            End If

            Return True

        End Function

        Private Function DoesParamSetIDExist(paramSetID As Integer) As Boolean
            Dim matchingRow As DataRow = Nothing
            Return GetParamFileRowByID(paramSetID, matchingRow)
        End Function

        Protected Function CompareParamSets(ByRef templateSet As Params, ByRef checkSet As Params) As String
            Dim diffCollection As DMSParamStorage = GetDiffColl(templateSet, checkSet)
            Return SummarizeDiffColl(diffCollection)
        End Function

        Protected Function GetDiffColl(ByRef templateSet As Params, ByRef checkSet As Params) As DMSParamStorage
            Dim templateColl As DMSParamStorage = WriteDataCollectionFromParamSet(templateSet)
            Dim checkColl As DMSParamStorage = WriteDataCollectionFromParamSet(checkSet)

            Dim diffCollection As DMSParamStorage = CompareDataCollections(templateColl, checkColl)
            Return diffCollection
        End Function

        Private Function SummarizeDiffColl(ByRef diffColl As DMSParamStorage) As String

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

                If tmpType = DMSParamStorage.ParamTypes.StaticModification Then

                    If tmpStatModsList Is Nothing Then
                        tmpStatModsList = New Queue()
                        tmpStatModsList.Enqueue("Static Mods: ")
                    End If
                    tmpStatModsList.Enqueue(tmpSpec + " (" + tmpSign + tmpValueFormatted + ")")


                ElseIf tmpType = DMSParamStorage.ParamTypes.DynamicModification Then

                    If tmpDynModsList Is Nothing Then
                        tmpDynModsList = New Queue()
                        tmpDynModsList.Enqueue("Dynamic Mods: ")
                    End If
                    tmpDynModsList.Enqueue(tmpSpec + " (" + tmpSign + tmpValueFormatted + ")")

                    intDynModCount += 1


                ElseIf tmpType = DMSParamStorage.ParamTypes.TermDynamicModification Then

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


                ElseIf tmpType = DMSParamStorage.ParamTypes.IsotopicModification Then

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

        Private Function CompareDataCollections(templateColl As DMSParamStorage, checkColl As DMSParamStorage) As DMSParamStorage
            Dim diffColl As New DMSParamStorage()
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

    End Class

End Namespace