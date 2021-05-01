Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports ParamFileGenerator.DownloadParams
Imports PRISMDatabaseUtils

' ReSharper disable once CheckNamespace
Namespace MakeParams

    Public Interface IGenerateFile

        ' Ignore Spelling: Sequest

        Enum ParamFileType
            Invalid = -1            ' Other stuff not currently handled
            BioWorks_20 = 0         ' Normal BioWorks 2.0 SEQUEST
            BioWorks_30 = 1         ' BioWorks 3.0+ TurboSequest
            BioWorks_31 = 2         ' BioWorks 3.1 ClusterQuest
            BioWorks_32 = 3         ' BioWorks 3.2 Cluster
            BioWorks_Current = 4
            X_Tandem = 5            ' X!Tandem XML file
            Inspect = 6             ' Inspect
            MSGFPlus = 7            ' MSGF-DB or MSGF+
            MSAlign = 8             ' MSAlign
            MSAlignHistone = 9      ' MSAlign_Histone (which is MS-Align+)
            MODa = 10
            MSPathFinder = 11
            MODPlus = 12
            TopPIC = 13
            MSFragger = 14
            MaxQuant = 15
        End Enum

        Function MakeFile(
             paramFileName As String,
             paramFileType As ParamFileType,
             fastaFilePath As String,
             outputFilePath As String,
             dmsConnectionString As String) As Boolean

        Function MakeFile(
             paramFileName As String,
             paramFileType As ParamFileType,
             fastaFilePath As String,
             outputFilePath As String,
             dmsConnectionString As String,
             DatasetID As Integer) As Boolean

        Function MakeFile(
             paramFileName As String,
             paramFileType As ParamFileType,
             fastaFilePath As String,
             outputFilePath As String,
             dmsConnectionString As String,
             datasetName As String) As Boolean

#Disable Warning BC40028 ' Type of parameter is not CLS-compliant

        Function GetAvailableParamSetNames(dbTools As IDBTools) As List(Of String)
        Function GetAvailableParamSetTable(dbTools As IDBTools) As DataTable
        Function GetAvailableParamFileTypes(dbTools As IDBTools) As DataTable

#Enable Warning BC40028 ' Type of parameter is not CLS-compliant

        ReadOnly Property LastError As String
    End Interface


    Public Class clsMakeParameterFile
        Inherits PRISM.EventNotifier
        Implements IGenerateFile

        Private m_DbTools As PRISMDatabaseUtils.IDBTools
        Private m_FileWriter As clsWriteOutput

        Public Property TemplateFilePath As String

        Private Property LastErrorMsg As String = String.Empty

        Private Function MakeFile(
            paramFileName As String,
            paramFileType As IGenerateFile.ParamFileType,
            fastaFilePath As String,
            outputFilePath As String,
            dmsConnectionString As String) As Boolean Implements IGenerateFile.MakeFile

            Return MakeFile(paramFileName, paramFileType, fastaFilePath, outputFilePath, dmsConnectionString, False)

        End Function

        Private Function MakeFile(
            paramFileName As String,
            paramFileType As IGenerateFile.ParamFileType,
            fastaFilePath As String,
            outputFilePath As String,
            dmsConnectionString As String,
            datasetName As String) As Boolean Implements IGenerateFile.MakeFile

            Dim forceMonoStatus As Boolean = GetMonoMassStatus(datasetName, dmsConnectionString)

            Return MakeFile(paramFileName, paramFileType, fastaFilePath, outputFilePath, dmsConnectionString, forceMonoStatus)

        End Function

        Private Function MakeFile(
            paramFileName As String,
            paramFileType As IGenerateFile.ParamFileType,
            fastaFilePath As String,
            outputFilePath As String,
            dmsConnectionString As String,
            DatasetID As Integer) As Boolean Implements IGenerateFile.MakeFile

            Dim forceMonoStatus As Boolean = GetMonoMassStatus(DatasetID, dmsConnectionString)

            Return MakeFile(paramFileName, paramFileType, fastaFilePath, outputFilePath, dmsConnectionString, forceMonoStatus)

        End Function

        Private Function MakeFile(
          paramFileName As String,
          paramFileType As IGenerateFile.ParamFileType,
          fastaFilePath As String,
          outputFilePath As String,
          dmsConnectionString As String,
          forceMonoParentMass As Boolean) As Boolean

            LastErrorMsg = String.Empty

            Try
                Select Case paramFileType
                    Case IGenerateFile.ParamFileType.X_Tandem
                        Return RetrieveStaticPSMParameterFile("XTandem", paramFileName, outputFilePath, dmsConnectionString)

                    Case IGenerateFile.ParamFileType.Inspect
                        Return RetrieveStaticPSMParameterFile("Inspect", paramFileName, outputFilePath, dmsConnectionString)

                    Case IGenerateFile.ParamFileType.MODa
                        Return RetrieveStaticPSMParameterFile("MODa", paramFileName, outputFilePath, dmsConnectionString)

                    Case IGenerateFile.ParamFileType.MSGFPlus
                        Return RetrieveStaticPSMParameterFile("MSGFPlus", paramFileName, outputFilePath, dmsConnectionString)

                    Case IGenerateFile.ParamFileType.MSAlign
                        Return RetrieveStaticPSMParameterFile("MSAlign", paramFileName, outputFilePath, dmsConnectionString)

                    Case IGenerateFile.ParamFileType.MSAlignHistone
                        Return RetrieveStaticPSMParameterFile("MSAlign_Histone", paramFileName, outputFilePath, dmsConnectionString)

                    Case IGenerateFile.ParamFileType.MSPathFinder
                        Return RetrieveStaticPSMParameterFile("MSPathFinder", paramFileName, outputFilePath, dmsConnectionString)

                    Case IGenerateFile.ParamFileType.MODPlus
                        Return RetrieveStaticPSMParameterFile("MODPlus", paramFileName, outputFilePath, dmsConnectionString)

                    Case IGenerateFile.ParamFileType.TopPIC
                        Return RetrieveStaticPSMParameterFile("TopPIC", paramFileName, outputFilePath, dmsConnectionString)

                    Case IGenerateFile.ParamFileType.MSFragger
                        Return RetrieveStaticPSMParameterFile("MSFragger", paramFileName, outputFilePath, dmsConnectionString)

                    Case IGenerateFile.ParamFileType.MaxQuant
                        Return RetrieveStaticPSMParameterFile("MaxQuant", paramFileName, outputFilePath, dmsConnectionString)

                    Case IGenerateFile.ParamFileType.Invalid
                        Exit Function

                    Case Else
                        paramFileType = IGenerateFile.ParamFileType.BioWorks_32
                        Return MakeFileSQ(
                         paramFileName,
                         paramFileType,
                         fastaFilePath,
                         outputFilePath,
                         dmsConnectionString,
                         forceMonoParentMass)
                End Select

            Catch ex As Exception
                ReportError("Error in MakeFile: " + ex.Message, ex)
                Return False

            End Try


        End Function

        Private Function GetMonoMassStatus(DatasetID As Integer, dmsConnectionString As String) As Boolean
            Dim TypeCheckSQL As String = "SELECT TOP 1 Use_Mono_Parent FROM V_Analysis_Job_Use_MonoMass WHERE Dataset_ID = " + DatasetID.ToString
            Return GetMonoParentStatusWorker(TypeCheckSQL, dmsConnectionString)
        End Function

        Private Function GetMonoMassStatus(datasetName As String, dmsConnectionString As String) As Boolean
            Dim TypeCheckSQL As String = "SELECT TOP 1 Use_Mono_Parent FROM V_Analysis_Job_Use_MonoMass WHERE Dataset_Name = '" + datasetName + "'"
            Return GetMonoParentStatusWorker(TypeCheckSQL, dmsConnectionString)
        End Function

        Private Function GetMonoParentStatusWorker(sqlQuery As String, dmsConnectionString As String) As Boolean

            If m_DbTools Is Nothing Then
                m_DbTools = DbToolsFactory.GetDBTools(dmsConnectionString)
            End If

            Dim typeCheckTable As List(Of List(Of String)) = Nothing
            m_DbTools.GetQueryResults(sqlQuery, typeCheckTable)

            If typeCheckTable.Count > 0 Then

                Dim useMonoMassInt As Integer

                If Integer.TryParse(typeCheckTable.Item(0).First(), useMonoMassInt) Then
                    If useMonoMassInt > 0 Then
                        Return True
                    Else
                        Return False
                    End If
                End If
            End If

            Return False

        End Function

        ''' <summary>
        ''' Create SEQUEST parameter file
        ''' </summary>
        ''' <param name="paramFileName"></param>
        ''' <param name="paramFileType"></param>
        ''' <param name="fastaFilePath"></param>
        ''' <param name="outputFilePath"></param>
        ''' <param name="dmsConnectionString"></param>
        ''' <param name="forceMonoParentMass"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function MakeFileSQ(
            paramFileName As String,
            paramFileType As IGenerateFile.ParamFileType,
            fastaFilePath As String,
            outputFilePath As String,
            dmsConnectionString As String,
            forceMonoParentMass As Boolean) As Boolean

            If m_DbTools Is Nothing Then
                m_DbTools = DbToolsFactory.GetDBTools(dmsConnectionString)
            End If

            Const DEF_TEMPLATE_FILEPATH = "\\Gigasax\DMS_Parameter_Files\Sequest\sequest_N14_NE_Template.params"

            If String.IsNullOrWhiteSpace(TemplateFilePath) Then
                TemplateFilePath = DEF_TEMPLATE_FILEPATH
            End If

            Dim fi As New FileInfo(TemplateFilePath)
            If Not fi.Exists Then
                ReportError("Default template file '" & TemplateFilePath & "' does not exist")
                Return False
            End If

            ' Instantiate clsMainProcess so we can access its properties later

            ' ReSharper disable once UnusedVariable.Compiler
            Dim processor As New clsMainProcess(TemplateFilePath)

            Dim loadedParams As clsParams
            Dim dmsParams As New clsParamsFromDMS(m_DbTools)
            Dim modProcessor As IReconstituteIsoMods
            modProcessor = New clsReconstituteIsoMods(m_DbTools)

            If dmsParams.ParamFileTable Is Nothing Then
                ReportError("Could Not Establish Database Connection")
                Return False
            End If

            If Not dmsParams.ParamSetNameExists(paramFileName) Then
                ReportError("Parameter File '" & paramFileName & "' does not exist in the database")
                Return False
            End If

            loadedParams = dmsParams.ReadParamsFromDMS(paramFileName)
            loadedParams = modProcessor.ReconstituteIsoMods(loadedParams)

            If forceMonoParentMass And Not loadedParams.LoadedParamNames.ContainsKey("ParentMassType") Then
                loadedParams.ParentMassType = IBasicParams.MassTypeList.Monoisotopic
            End If

            If Not loadedParams.LoadedParamNames.ContainsKey("PeptideMassUnits") Then
                loadedParams.PeptideMassUnits = IAdvancedParams.MassUnitList.amu
            End If

            If Not loadedParams.LoadedParamNames.ContainsKey("FragmentMassUnits") Then
                loadedParams.FragmentMassUnits = IAdvancedParams.MassUnitList.amu
            End If

            loadedParams.DefaultFASTAPath = fastaFilePath

            If m_FileWriter Is Nothing Then
                m_FileWriter = New clsWriteOutput
            End If

            Dim writeSuccess = m_FileWriter.WriteOutputFile(loadedParams, Path.Combine(outputFilePath, paramFileName), paramFileType)

            Dim successExtra = MakeSeqInfoRelatedFiles(paramFileName, outputFilePath, dmsConnectionString)

            Return writeSuccess

        End Function

        Private Function MakeSeqInfoRelatedFiles(
            paramFileName As String,
            targetDirectory As String,
            dmsConnectionString As String) As Boolean

            Dim mctSQL As String
            Dim mdSQL As String

            If m_DbTools Is Nothing Then
                m_DbTools = DbToolsFactory.GetDBTools(dmsConnectionString)
            End If

            Dim baseParamFileName As String = Path.GetFileNameWithoutExtension(paramFileName)

            If m_FileWriter Is Nothing Then
                m_FileWriter = New clsWriteOutput
            End If

            mctSQL =
                "SELECT Mass_Correction_Tag, Monoisotopic_Mass, Affected_Atom " &
                "FROM T_Mass_Correction_Factors " &
                "ORDER BY Mass_Correction_Tag"

            mdSQL =
                "SELECT Local_Symbol, Monoisotopic_Mass, Residue_Symbol, Mod_Type_Symbol, Mass_Correction_Tag " &
                "FROM V_Param_File_Mass_Mod_Info " &
                "WHERE Param_File_Name = '" & paramFileName & "'"

            Dim mctTable As List(Of List(Of String)) = Nothing
            m_DbTools.GetQueryResults(mctSQL, mctTable)


            Dim mdTable As List(Of List(Of String)) = Nothing
            m_DbTools.GetQueryResults(mdSQL, mdTable)

            'Dump the Mass_Correction_Tags file to the working directory
            m_FileWriter.WriteDataTableToOutputFile(mctTable, Path.Combine(targetDirectory, "Mass_Correction_Tags.txt"))

            'Dump the param file specific modification definitions file to the working directory
            m_FileWriter.WriteDataTableToOutputFile(mdTable, Path.Combine(targetDirectory, baseParamFileName & "_ModDefs.txt"))

        End Function

        Private Function RetrieveStaticPSMParameterFile(
           analysisToolName As String,
           paramFileName As String,
           targetDirectory As String,
           dmsConnectionString As String) As Boolean

            Dim paramFilePath As String

            If m_DbTools Is Nothing Then
                m_DbTools = DbToolsFactory.GetDBTools(dmsConnectionString)
            End If

            ' ReSharper disable once StringLiteralTypo
            Dim paramFilePathSQL =
                "SELECT TOP 1 AJT_parmFileStoragePath " &
                "FROM T_Analysis_Tool " &
                "WHERE AJT_ToolName = '" & analysisToolName & "'"

            Dim paramFilePathTable As List(Of List(Of String)) = Nothing
            m_DbTools.GetQueryResults(paramFilePathSQL, paramFilePathTable)

            If paramFilePathTable.Count = 0 Then
                ReportError("Tool not found in T_Analysis_Tool: " & analysisToolName)
                Return False
            End If

            Dim paramFileDirectory = paramFilePathTable.Item(0).First()
            paramFilePath = Path.Combine(paramFileDirectory, paramFileName)

            If Not Directory.Exists(paramFileDirectory) Then
                ReportError(String.Format("Directory defined in T_Analysis_Tool for {0} was not found: {1}",
                                            analysisToolName, paramFileDirectory))

                Return False
            End If

            Dim fiSource = New FileInfo(paramFilePath)
            If Not fiSource.Exists Then
                ReportError("Parameter file not found: " & fiSource.FullName)
                Return False
            End If

            ' Copy the param file from Gigasax to the working directory
            fiSource.CopyTo(Path.Combine(targetDirectory, paramFileName), True)

            MakeSeqInfoRelatedFiles(paramFileName, targetDirectory, dmsConnectionString)

            Return True

        End Function

        Private Function GetAvailableParamSetNames(dbTools As IDBTools) As List(Of String) Implements IGenerateFile.GetAvailableParamSetNames

            Dim availableParamSets As New List(Of String)
            Dim dmsParams As New clsParamsFromDMS(dbTools)

            Dim retrievedParamSets As DataTable = dmsParams.RetrieveAvailableParams()
            Dim dr As DataRow
            For Each dr In retrievedParamSets.Rows
                availableParamSets.Add(dr.Item("FileName").ToString)
            Next
            Return availableParamSets
        End Function

        Private Function GetAvailableParamSetTable(dbTools As IDBTools) As DataTable Implements IGenerateFile.GetAvailableParamSetTable
            Dim paramGenerator As New clsParamsFromDMS(dbTools)
            Dim paramSetsAvailable As DataTable = paramGenerator.RetrieveAvailableParams

            Return paramSetsAvailable

        End Function

        Private Function GetAvailableParamSetTypes(dbTools As IDBTools) As DataTable Implements IGenerateFile.GetAvailableParamFileTypes
            Dim paramGenerator As New clsParamsFromDMS(dbTools)
            Dim paramTypesAvailable As DataTable = paramGenerator.RetrieveParamFileTypes
            Return paramTypesAvailable
        End Function

        Public ReadOnly Property LastError As String Implements IGenerateFile.LastError
            Get
                Return LastErrorMsg
            End Get
        End Property

        Private Sub ReportError(errorMessage As String, Optional ex As Exception = Nothing)
            OnErrorEvent(errorMessage, ex)
            LastErrorMsg = errorMessage
        End Sub

    End Class
End Namespace
