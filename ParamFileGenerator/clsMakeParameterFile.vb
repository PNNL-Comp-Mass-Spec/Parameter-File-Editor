Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports ParamFileGenerator.DownloadParams

' ReSharper disable once CheckNamespace
Namespace MakeParams

    Public Interface IGenerateFile

        Enum paramFileType
            Invalid = -1            ' Other stuff not currently handled
            BioWorks_20 = 0         ' Normal BioWorks 2.0 Sequest
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
        End Enum

        Function MakeFile(ParamFileName As String,
             ParamFileType As ParamFileType,
             FASTAFilePath As String,
             OutputFilePath As String,
             DMSConnectionString As String) As Boolean

        Function MakeFile(ParamFileName As String,
             ParamFileType As ParamFileType,
             FASTAFilePath As String,
             OutputFilePath As String,
             DMSConnectionString As String,
             DatasetID As Integer) As Boolean

        Function MakeFile(ParamFileName As String,
             ParamFileType As ParamFileType,
             FASTAFilePath As String,
             OutputFilePath As String,
             DMSConnectionString As String,
             DatasetName As String) As Boolean
             datasetName As String) As Boolean

        Function GetAvailableParamSetNames(dmsConnectionString As String) As List(Of String)
        Function GetAvailableParamSetTable(dmsConnectionString As String) As DataTable

        Function GetAvailableParamFileTypes(dmsConnectionString As String) As DataTable

        ReadOnly Property LastError() As String

        Property TemplateFilePath() As String

    End Interface


    Public Class clsMakeParameterFile
        Inherits PRISM.EventNotifier
        Implements IGenerateFile

        Private m_TemplateFilePathString As String

        Private m_DbTools As PRISM.DBTools
        Private m_FileWriter As clsWriteOutput

        Public Property TemplateFilePath() As String Implements IGenerateFile.TemplateFilePath
            Get
                Return m_TemplateFilePathString
            End Get
            Set(Value As String)
                m_TemplateFilePathString = Value
            End Set
        End Property

        Protected Property LastErrorMsg As String = String.Empty

        Protected Function MakeFile(
            ParamFileName As String,
            ParamFileType As IGenerateFile.ParamFileType,
            FASTAFilePath As String,
            OutputFilePath As String,
            DMSConnectionString As String) As Boolean Implements IGenerateFile.MakeFile

            Return MakeFile(ParamFileName, ParamFileType, FASTAFilePath, OutputFilePath, DMSConnectionString, False)

        End Function

        Protected Function MakeFile(
            ParamFileName As String,
            ParamFileType As IGenerateFile.ParamFileType,
            FASTAFilePath As String,
            OutputFilePath As String,
            DMSConnectionString As String,
            DatasetName As String) As Boolean Implements IGenerateFile.MakeFile

            Dim ForceMonoStatus As Boolean = GetMonoMassStatus(DatasetName, DMSConnectionString)

            Return MakeFile(ParamFileName, ParamFileType, FASTAFilePath, OutputFilePath, DMSConnectionString, ForceMonoStatus)

        End Function

        Protected Function MakeFile(
            ParamFileName As String,
            ParamFileType As IGenerateFile.ParamFileType,
            FASTAFilePath As String,
            OutputFilePath As String,
            DMSConnectionString As String,
            DatasetID As Integer) As Boolean Implements IGenerateFile.MakeFile

            Dim ForceMonoStatus As Boolean = GetMonoMassStatus(DatasetID, DMSConnectionString)

            Return MakeFile(ParamFileName, ParamFileType, FASTAFilePath, OutputFilePath, DMSConnectionString, ForceMonoStatus)

        End Function

        Private Function MakeFile(
          paramFileName As String,
          paramFileType As IGenerateFile.paramFileType,
          fastaFilePath As String,
          outputFilePath As String,
          dmsConnectionString As String,
          forceMonoParentMass As Boolean) As Boolean

            LastErrorMsg = String.Empty

            Try
                Select Case paramFileType
                    Case IGenerateFile.paramFileType.X_Tandem
                        Return RetrieveStaticPSMParameterFile("XTandem", paramFileName, outputFilePath, dmsConnectionString)

                    Case IGenerateFile.paramFileType.Inspect
                        Return RetrieveStaticPSMParameterFile("Inspect", paramFileName, outputFilePath, dmsConnectionString)

                    Case IGenerateFile.paramFileType.MODa
                        Return RetrieveStaticPSMParameterFile("MODa", paramFileName, outputFilePath, dmsConnectionString)

                    Case IGenerateFile.paramFileType.MSGFPlus
                        Return RetrieveStaticPSMParameterFile("MSGFPlus", paramFileName, outputFilePath, dmsConnectionString)

                    Case IGenerateFile.paramFileType.MSAlign
                        Return RetrieveStaticPSMParameterFile("MSAlign", paramFileName, outputFilePath, dmsConnectionString)

                    Case IGenerateFile.paramFileType.MSAlignHistone
                        Return RetrieveStaticPSMParameterFile("MSAlign_Histone", paramFileName, outputFilePath, dmsConnectionString)

                    Case IGenerateFile.paramFileType.MSPathFinder
                        Return RetrieveStaticPSMParameterFile("MSPathFinder", paramFileName, outputFilePath, dmsConnectionString)

                    Case IGenerateFile.paramFileType.MODPlus
                        Return RetrieveStaticPSMParameterFile("MODPlus", paramFileName, outputFilePath, dmsConnectionString)

                    Case IGenerateFile.paramFileType.TopPIC
                        Return RetrieveStaticPSMParameterFile("TopPIC", paramFileName, outputFilePath, dmsConnectionString)

                    Case IGenerateFile.paramFileType.Invalid
                        Exit Function

                    Case Else
                        paramFileType = IGenerateFile.paramFileType.BioWorks_32
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

        Protected Function GetMonoMassStatus(DatasetID As Integer, DMSConnectionString As String) As Boolean
            Dim TypeCheckSQL As String = "SELECT TOP 1 Use_Mono_Parent FROM V_Analysis_Job_Use_MonoMass WHERE Dataset_ID = " + DatasetID.ToString
            Return GetMonoParentStatusWorker(TypeCheckSQL, DMSConnectionString)
        End Function

        Protected Function GetMonoMassStatus(DatasetName As String, DMSConnectionString As String) As Boolean
            Dim TypeCheckSQL As String = "SELECT TOP 1 Use_Mono_Parent FROM V_Analysis_Job_Use_MonoMass WHERE Dataset_Name = '" + DatasetName + "'"
            Return GetMonoParentStatusWorker(TypeCheckSQL, DMSConnectionString)
        End Function

        Private Function GetMonoParentStatusWorker(LookupSQL As String, DMSConnectionString As String) As Boolean

            If m_DbTools Is Nothing Then
                m_DbTools = New PRISM.DBTools(DMSConnectionString)
            End If

            Dim typeCheckTable As List(Of List(Of String)) = Nothing
            m_DbTools.GetQueryResults(LookupSQL, typeCheckTable, "GetMonoParentStatusWorker")

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
        ''' Create Sequest parameter file
        ''' </summary>
        ''' <param name="ParamFileName"></param>
        ''' <param name="ParamFileType"></param>
        ''' <param name="FASTAFilePath"></param>
        ''' <param name="OutputFilePath"></param>
        ''' <param name="DMSConnectionString"></param>
        ''' <param name="forceMonoParentMass"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function MakeFileSQ(
            ParamFileName As String,
            ParamFileType As IGenerateFile.ParamFileType,
            FASTAFilePath As String,
            OutputFilePath As String,
            DMSConnectionString As String,
            forceMonoParentMass As Boolean) As Boolean

            If m_DbTools Is Nothing Then
                m_DbTools = New PRISM.DBTools(DMSConnectionString)
            End If

            Const DEF_TEMPLATE_FILEPATH = "\\Gigasax\dms_parameter_files\Sequest\sequest_N14_NE_Template.params"

            If m_TemplateFilePathString = "" Then
                m_TemplateFilePathString = DEF_TEMPLATE_FILEPATH
            End If

            Dim fi As New FileInfo(m_TemplateFilePathString)
            If Not fi.Exists Then
                ReportError("Default template file '" & m_TemplateFilePathString & "' does not exist")
                Return False
            End If

            ' Instantiate clsMainProcess so we can access its properties later

            ' ReSharper disable once UnusedVariable.Compiler
            Dim l_MainCode As New clsMainProcess(m_TemplateFilePathString)

            Dim l_LoadedParams As clsParams
            Dim l_DMS As New clsParamsFromDMS(DMSConnectionString)
            Dim l_ReconIsoMods As IReconstituteIsoMods
            l_ReconIsoMods = New clsReconstituteIsoMods(DMSConnectionString)

            If l_DMS.ParamFileTable Is Nothing Then
                ReportError("Could Not Establish Database Connection")
                Return False
            End If

            If Not l_DMS.ParamSetNameExists(ParamFileName) Then
                ReportError("Parameter File '" & ParamFileName & "' does not exist in the database")
                Return False
            End If

            l_LoadedParams = l_DMS.ReadParamsFromDMS(ParamFileName)
            l_LoadedParams = l_ReconIsoMods.ReconstituteIsoMods(l_LoadedParams)

            If forceMonoParentMass And Not l_LoadedParams.LoadedParamNames.ContainsKey("ParentMassType") Then
                l_LoadedParams.ParentMassType = IBasicParams.MassTypeList.Monoisotopic
            End If

            If Not l_LoadedParams.LoadedParamNames.ContainsKey("PeptideMassUnits") Then
                l_LoadedParams.PeptideMassUnits = IAdvancedParams.MassUnitList.amu
            End If

            If Not l_LoadedParams.LoadedParamNames.ContainsKey("FragmentMassUnits") Then
                l_LoadedParams.FragmentMassUnits = IAdvancedParams.MassUnitList.amu
            End If

            l_LoadedParams.DefaultFASTAPath = FASTAFilePath

            If m_FileWriter Is Nothing Then
                m_FileWriter = New clsWriteOutput
            End If

            Dim writeSuccess = m_FileWriter.WriteOutputFile(l_LoadedParams, Path.Combine(OutputFilePath, ParamFileName), ParamFileType)

            Dim successExtra = MakeSeqInfoRelatedFiles(ParamFileName, OutputFilePath, DMSConnectionString)

            Return writeSuccess

        End Function

        Private Function MakeSeqInfoRelatedFiles(
            paramFileName As String,
            targetDirectory As String,
            DMSConnectionString As String) As Boolean

            Dim mctSQL As String
            Dim mdSQL As String

            If m_DbTools Is Nothing Then
                m_DbTools = New PRISM.DBTools(DMSConnectionString)
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
            m_DbTools.GetQueryResults(mctSQL, mctTable, "MakeSeqInfoRelatedFiles_A")


            Dim mdTable As List(Of List(Of String)) = Nothing
            m_DbTools.GetQueryResults(mdSQL, mdTable, "MakeSeqInfoRelatedFiles_B")

            'Dump the Mass_Correction_Tags file to the working directory
            m_FileWriter.WriteDataTableToOutputFile(mctTable, Path.Combine(targetDirectory, "Mass_Correction_Tags.txt"))

            'Dump the param file specific Mod Defs file to the working directory
            m_FileWriter.WriteDataTableToOutputFile(mdTable, Path.Combine(targetDirectory, baseParamFileName & "_ModDefs.txt"))

        End Function

        Protected Function RetrieveStaticPSMParameterFile(
           analysisToolName As String,
           paramFileName As String,
           targetDirectory As String,
           DMSConnectionString As String) As Boolean

            Dim paramFilePath As String

            If m_DbTools Is Nothing Then
                m_DbTools = New PRISM.DBTools(DMSConnectionString)
            End If

            Dim paramFilePathSQL =
             "SELECT TOP 1 AJT_parmFileStoragePath " &
             "FROM T_Analysis_Tool " &
             "WHERE AJT_ToolName = '" & analysisToolName & "'"

            Dim paramFilePathTable As List(Of List(Of String)) = Nothing
            m_DbTools.GetQueryResults(paramFilePathSQL, paramFilePathTable, "RetrieveStaticPSMParameterFile")

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

            MakeSeqInfoRelatedFiles(paramFileName, targetDirectory, DMSConnectionString)

            Return True

        End Function

        Private Function GetAvailableParamSetNames(dmsConnectionString As String) As List(Of String) Implements IGenerateFile.GetAvailableParamSetNames

            Dim l_ParamSetsAvailable As New List(Of String)
            Dim l_DMS As New clsParamsFromDMS(dmsConnectionString)
            Dim d_ParamSetsAvailable As DataTable = l_DMS.RetrieveAvailableParams()
            Dim dr As DataRow
            For Each dr In d_ParamSetsAvailable.Rows
                l_ParamSetsAvailable.Add(dr.Item("FileName").ToString)
            Next
            Return l_ParamSetsAvailable
        End Function

        Protected Function GetAvailableParamSetTable(DMSConnectionString As String) As DataTable Implements IGenerateFile.GetAvailableParamSetTable
            Dim l_DMS As New clsParamsFromDMS(DMSConnectionString)
            Dim paramSetsAvailable As DataTable = l_DMS.RetrieveAvailableParams

            Return paramSetsAvailable

        End Function

        Protected Function GetAvailableParamSetTypes(DMSConnectionString As String) As DataTable Implements IGenerateFile.GetAvailableParamFileTypes
            Dim l_DMS As New clsParamsFromDMS(DMSConnectionString)
            Dim paramTypesAvailable As DataTable = l_DMS.RetrieveParamFileTypes
            Return paramTypesAvailable
        End Function

        Public ReadOnly Property LastError() As String Implements IGenerateFile.LastError
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
