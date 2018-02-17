Imports System.Collections.Specialized
Imports System.IO
Imports ParamFileGenerator.DownloadParams

Namespace MakeParams

    Public Interface IGenerateFile
        Enum ParamFileType
            Invalid = -1     'Other stuff not currently handled
            BioWorks_20 = 0  'Normal BioWorks 2.0 Sequest
            BioWorks_30 = 1  'BioWorks 3.0+ TurboSequest
            BioWorks_31 = 2  'BioWorks 3.1 ClusterQuest
            BioWorks_32 = 3  'Bioworks 3.2 Cluster
            BioWorks_Current = 4
            X_Tandem = 5     'X!Tandem XML file
            Inspect = 6      'InSpect
            MSGFPlus = 7     'MSGF-DB or MSGF+
            MSAlign = 8      'MSAlign
            MSAlignHistone = 9      'MSAlign_Histone (which is MS-Align+)
            MODa = 10
            MSPathFinder = 11
            MODPlus = 12
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

        Function GetAvailableParamSetNames(DMSConnectionString As String) As StringCollection
        Function GetAvailableParamSetTable(DMSConnectionString As String) As DataTable

        Function GetAvailableParamFileTypes(DMSConnectionString As String) As DataTable

        ReadOnly Property LastError() As String

        Property TemplateFilePath() As String



    End Interface


    Public Class clsMakeParameterFile
        Implements IGenerateFile

        Private m_LastError As String
        Private m_TemplateFilePathString As String

        Private m_TableGetter As IGetSQLData
        Private m_FileWriter As clsWriteOutput

        Public Property TemplateFilePath() As String Implements IGenerateFile.TemplateFilePath
            Get
                Return m_TemplateFilePathString
            End Get
            Set(Value As String)
                m_TemplateFilePathString = Value
            End Set
        End Property

        Protected ReadOnly Property LastErrorMsg() As String
            Get
                Return m_LastError
            End Get
        End Property

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

        Protected Function MakeFile(
          ParamFileName As String,
          ParamFileType As IGenerateFile.ParamFileType,
          FASTAFilePath As String,
          OutputFilePath As String,
          DMSConnectionString As String,
          ForceMonoParentMass As Boolean) As Boolean

            Try
                Select Case ParamFileType
                    Case IGenerateFile.ParamFileType.X_Tandem
                        Return MakeFileXT(
                         ParamFileName,
                         OutputFilePath,
                         DMSConnectionString)

                    Case IGenerateFile.ParamFileType.Inspect
                        Return MakeFileInspect(
                         ParamFileName,
                         OutputFilePath,
                         DMSConnectionString)

                    Case IGenerateFile.ParamFileType.MODa
                        Return MakeFileMODa(
                          ParamFileName,
                          OutputFilePath,
                          DMSConnectionString)

                    Case IGenerateFile.ParamFileType.MSGFPlus
                        Return MakeFileMSGFPlus(
                           ParamFileName,
                           OutputFilePath,
                           DMSConnectionString)

                    Case IGenerateFile.ParamFileType.MSAlign
                        Return MakeFileMSAlign(
                          ParamFileName,
                          OutputFilePath,
                          DMSConnectionString)

                    Case IGenerateFile.ParamFileType.MSAlignHistone
                        Return MakeFileMSAlignHistone(
                          ParamFileName,
                          OutputFilePath,
                          DMSConnectionString)

                    Case IGenerateFile.ParamFileType.MSPathFinder
                        Return MakeFileMSPathFinder(
                          ParamFileName,
                          OutputFilePath,
                          DMSConnectionString)

                    Case IGenerateFile.ParamFileType.MODPlus
                        Return MakeFileMODPlus(
                          ParamFileName,
                          OutputFilePath,
                          DMSConnectionString)

                    Case IGenerateFile.ParamFileType.Invalid
                        Exit Function

                    Case Else
                        ParamFileType = IGenerateFile.ParamFileType.BioWorks_32
                        Return MakeFileSQ(
                         ParamFileName,
                         ParamFileType,
                         FASTAFilePath,
                         OutputFilePath,
                         DMSConnectionString,
                         ForceMonoParentMass)
                End Select

            Catch ex As Exception
                m_LastError = ex.Message
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
            Dim TypeCheckTable As DataTable

            Dim UseMonoMass As Boolean
            Dim UseMonoMassInt As Integer

            If m_TableGetter Is Nothing Then
                m_TableGetter = New clsDBTask(DMSConnectionString)
            End If

            TypeCheckTable = m_TableGetter.GetTable(LookupSQL)

            If TypeCheckTable.Rows.Count > 0 Then
                UseMonoMassInt = CInt(TypeCheckTable.Rows(0).Item(0))
                If UseMonoMassInt > 0 Then
                    UseMonoMass = True
                Else
                    UseMonoMass = False
                End If
            End If
            Return UseMonoMass
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

            If m_TableGetter Is Nothing Then
                m_TableGetter = New clsDBTask(DMSConnectionString)
            End If

            Const DEF_TEMPLATE_FILEPATH = "\\Gigasax\dms_parameter_files\Sequest\sequest_N14_NE_Template.params"

            If m_TemplateFilePathString = "" Then
                m_TemplateFilePathString = DEF_TEMPLATE_FILEPATH
            End If

            Dim fi As New FileInfo(m_TemplateFilePathString)
            If Not fi.Exists Then
                Dim success = False
                m_LastError = "Default template file '" & m_TemplateFilePathString & "' does not exist"
                Return success
            End If

            ' Instantiate clsMainProcess so we can access its properties later

            ' ReSharper disable once UnusedVariable.Compiler
            Dim l_MainCode As New clsMainProcess(m_TemplateFilePathString)

            Dim l_LoadedParams As clsParams
            Dim l_DMS As New clsParamsFromDMS(DMSConnectionString)
            Dim l_ReconIsoMods As IReconstituteIsoMods
            l_ReconIsoMods = New clsReconstitueIsoMods(DMSConnectionString)

            If l_DMS.ParamFileTable Is Nothing Then
                Dim success = False
                m_LastError = "Could Not Establish Database Connection"
                Return success
            End If

            If Not l_DMS.ParamSetNameExists(ParamFileName) Then
                Dim success = False
                m_LastError = "Parameter File '" & ParamFileName & "' does not exist in the database"
                Return success
            End If

            l_LoadedParams = l_DMS.ReadParamsFromDMS(ParamFileName)
            l_LoadedParams = l_ReconIsoMods.ReconstitueIsoMods(l_LoadedParams)

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

        Protected Function MakeFileInspect(ParamFileName As String, OutputFilePath As String, DMSConnectionString As String) As Boolean

            Return RetrieveStaticPSMParameterFile("Inspect", ParamFileName, OutputFilePath, DMSConnectionString)

        End Function

        Protected Function MakeFileMODa(ParamFileName As String, OutputFilePath As String, DMSConnectionString As String) As Boolean

            Return RetrieveStaticPSMParameterFile("MODa", ParamFileName, OutputFilePath, DMSConnectionString)

        End Function

        Protected Function MakeFileMODPlus(ParamFileName As String, OutputFilePath As String, DMSConnectionString As String) As Boolean

            Return RetrieveStaticPSMParameterFile("MODPlus", ParamFileName, OutputFilePath, DMSConnectionString)

        End Function

        Protected Function MakeFileMSAlign(ParamFileName As String, OutputFilePath As String, DMSConnectionString As String) As Boolean

            Return RetrieveStaticPSMParameterFile("MSAlign", ParamFileName, OutputFilePath, DMSConnectionString)

        End Function

        Protected Function MakeFileMSAlignHistone(ParamFileName As String, OutputFilePath As String, DMSConnectionString As String) As Boolean

            Return RetrieveStaticPSMParameterFile("MSAlign_Histone", ParamFileName, OutputFilePath, DMSConnectionString)

        End Function

        Protected Function MakeFileMSGFPlus(ParamFileName As String, OutputFilePath As String, DMSConnectionString As String) As Boolean

            Return RetrieveStaticPSMParameterFile("MSGFPlus", ParamFileName, OutputFilePath, DMSConnectionString)

        End Function

        Protected Function MakeFileMSPathFinder(ParamFileName As String, OutputFilePath As String, DMSConnectionString As String) As Boolean

            Return RetrieveStaticPSMParameterFile("MSPathFinder", ParamFileName, OutputFilePath, DMSConnectionString)

        End Function

        Protected Function MakeFileXT(ParamFileName As String, OutputFilePath As String, DMSConnectionString As String) As Boolean

            Return RetrieveStaticPSMParameterFile("XTandem", ParamFileName, OutputFilePath, DMSConnectionString)

        End Function

        Private Function MakeSeqInfoRelatedFiles(
            paramFileName As String,
            targetDirectory As String,
            connectionStringDMS As String) As Boolean

            Dim mctSQL As String
            Dim mdSQL As String

            Dim mctTable As DataTable
            Dim mdTable As DataTable

            If m_TableGetter Is Nothing Then
                m_TableGetter = New clsDBTask(connectionStringDMS)
            End If

            Dim baseParamfileName As String
            baseParamfileName = Path.GetFileNameWithoutExtension(paramFileName)

            If m_FileWriter Is Nothing Then
                m_FileWriter = New clsWriteOutput
            End If

            mctSQL =
                "SELECT Mass_Correction_Tag, Monoisotopic_Mass_Correction, Affected_Atom " &
                "FROM T_Mass_Correction_Factors " &
                "ORDER BY Mass_Correction_Tag"

            mdSQL =
                "SELECT Local_Symbol, Monoisotopic_Mass_Correction, Residue_Symbol, Mod_Type_Symbol, Mass_Correction_Tag " &
                "FROM V_Param_File_Mass_Mod_Info " &
                "WHERE Param_File_Name = '" & paramFileName & "'"

            mctTable = m_TableGetter.GetTable(mctSQL)

            mdTable = m_TableGetter.GetTable(mdSQL)

            'Dump the Mass_Correction_Tags file to the working directory
            m_FileWriter.WriteDatatableToOutputFile(mctTable, Path.Combine(targetDirectory, "Mass_Correction_Tags.txt"))

            'Dump the param file specific Mod Defs file to the working directory
            m_FileWriter.WriteDatatableToOutputFile(mdTable, Path.Combine(targetDirectory, baseParamfileName & "_ModDefs.txt"))

        End Function

        Protected Function RetrieveStaticPSMParameterFile(
           analysisToolName As String,
           paramFileName As String,
           targetDirectory As String,
           connectionStringDMS As String) As Boolean

            Dim paramFilePathSQL As String

            Dim paramFilePathTable As DataTable
            Dim paramFilePath As String

            If m_TableGetter Is Nothing Then
                m_TableGetter = New clsDBTask(connectionStringDMS)
            End If

            paramFilePathSQL =
             "SELECT TOP 1 AJT_parmFileStoragePath " &
             "FROM T_Analysis_Tool " &
             "WHERE AJT_Toolname = '" & analysisToolName & "'"

            paramFilePathTable = m_TableGetter.GetTable(paramFilePathSQL)

            If paramFilePathTable.Rows.Count = 0 Then
                m_LastError = "Tool not found in T_Analysis_Tool: " & analysisToolName
                Return False
            End If

            paramFilePath = Path.Combine(paramFilePathTable.Rows(0).Item(0).ToString(), paramFileName)

            Dim fiSource = New FileInfo(paramFilePath)
            If Not fiSource.Exists Then
                m_LastError = "Parameter file not found: " & fiSource.FullName
                Return False
            End If

            'Copy the param file from gigasax to the working directory
            fiSource.CopyTo(Path.Combine(targetDirectory, paramFileName), True)

            MakeSeqInfoRelatedFiles(paramFileName, targetDirectory, connectionStringDMS)

            Return True

        End Function

        Protected Function GetAvailableParamSetNames(DMSConnectionString As String) As StringCollection Implements IGenerateFile.GetAvailableParamSetNames

            Dim l_ParamSetsAvailable As New StringCollection
            Dim l_DMS As New clsParamsFromDMS(DMSConnectionString)
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
                Return m_LastError
            End Get
        End Property
    End Class
End Namespace
