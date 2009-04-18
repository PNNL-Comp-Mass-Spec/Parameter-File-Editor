Imports ParamFileGenerator
Namespace MakeParams

    Public Interface IGenerateFile
        Enum ParamFileType
            BioWorks_20  'Normal BioWorks 2.0 Sequest
            BioWorks_30  'BioWorks 3.0+ TurboSequest
            BioWorks_31  'BioWorks 3.1 ClusterQuest
            BioWorks_32  'Bioworks 3.2 ClusterF***
            BioWorks_Current
            X_Tandem     'X!Tandem XML file
            Inspect      'InSpect
            Invalid      'Other stuff not currently handled
        End Enum
        Function MakeFile(ByVal ParamFileName As String, _
             ByVal ParamFileType As ParamFileType, _
             ByVal FASTAFilePath As String, _
             ByVal OutputFilePath As String, _
             ByVal DMSConnectionString As String) As Boolean

        Function MakeFile(ByVal ParamFileName As String, _
             ByVal ParamFileType As ParamFileType, _
             ByVal FASTAFilePath As String, _
             ByVal OutputFilePath As String, _
             ByVal DMSConnectionString As String, _
             ByVal DatasetID As Integer) As Boolean

        Function MakeFile(ByVal ParamFileName As String, _
             ByVal ParamFileType As ParamFileType, _
             ByVal FASTAFilePath As String, _
             ByVal OutputFilePath As String, _
             ByVal DMSConnectionString As String, _
             ByVal DatasetName As String) As Boolean

        Function GetAvailableParamSetNames(ByVal DMSConnectionString As String) As System.Collections.Specialized.StringCollection
        Function GetAvailableParamSetTable(ByVal DMSConnectionString As String) As DataTable

        Function GetAvailableParamFileTypes(ByVal DMSConnectionString As String) As DataTable

        ReadOnly Property LastError() As String

        Property TemplateFilePath() As String



    End Interface


    public Class clsMakeParameterFile
        Implements IGenerateFile

        Private m_LastError As String
        Private m_TemplateFilePathString As String

        Private m_TableGetter As IGetSQLData
        Private m_FileWriter As ParamFileGenerator.clsWriteOutput

        Public Property TemplateFilePath() As String Implements IGenerateFile.TemplateFilePath
            Get
                Return Me.m_TemplateFilePathString
            End Get
            Set(ByVal Value As String)
                Me.m_TemplateFilePathString = Value
            End Set
        End Property

        Protected ReadOnly Property LastErrorMsg() As String
            Get
                Return Me.m_LastError
            End Get
        End Property

        Protected Function MakeFile( _
            ByVal ParamFileName As String, _
            ByVal ParamFileType As IGenerateFile.ParamFileType, _
            ByVal FASTAFilePath As String, _
            ByVal OutputFilePath As String, _
            ByVal DMSConnectionString As String) As Boolean Implements IGenerateFile.MakeFile

            Return Me.MakeFile(ParamFileName, ParamFileType, FASTAFilePath, OutputFilePath, DMSConnectionString, False)

        End Function

        Protected Function MakeFile( _
            ByVal ParamFileName As String, _
            ByVal ParamFileType As IGenerateFile.ParamFileType, _
            ByVal FASTAFilePath As String, _
            ByVal OutputFilePath As String, _
            ByVal DMSConnectionString As String, _
            ByVal DatasetName As String) As Boolean Implements IGenerateFile.MakeFile

            Dim ForceMonoStatus As Boolean = Me.GetMonoMassStatus(DatasetName, DMSConnectionString)

            Return Me.MakeFile(ParamFileName, ParamFileType, FASTAFilePath, OutputFilePath, DMSConnectionString, ForceMonoStatus)

        End Function

        Protected Function MakeFile( _
            ByVal ParamFileName As String, _
            ByVal ParamFileType As IGenerateFile.ParamFileType, _
            ByVal FASTAFilePath As String, _
            ByVal OutputFilePath As String, _
            ByVal DMSConnectionString As String, _
            ByVal DatasetID As Integer) As Boolean Implements IGenerateFile.MakeFile

            Dim ForceMonoStatus As Boolean = Me.GetMonoMassStatus(DatasetID, DMSConnectionString)

            Return Me.MakeFile(ParamFileName, ParamFileType, FASTAFilePath, OutputFilePath, DMSConnectionString, ForceMonoStatus)

        End Function
        Protected Function MakeFile( _
            ByVal ParamFileName As String, _
            ByVal ParamFileType As IGenerateFile.ParamFileType, _
            ByVal FASTAFilePath As String, _
            ByVal OutputFilePath As String, _
            ByVal DMSConnectionString As String, _
            ByVal ForceMonoParentMass As Boolean) As Boolean



            Try
                Select Case ParamFileType
                    Case IGenerateFile.ParamFileType.X_Tandem
                        Return Me.MakeFileXT( _
                            ParamFileName, _
                            OutputFilePath, _
                            DMSConnectionString)
                    Case IGenerateFile.ParamFileType.Inspect
                        Return Me.MakeFileInspect( _
                            ParamFileName, _
                            OutputFilePath, _
                            DMSConnectionString)
                    Case IGenerateFile.ParamFileType.Invalid
                        Exit Function
                    Case Else
                        ParamFileType = IGenerateFile.ParamFileType.BioWorks_32
                        Return Me.MakeFileSQ( _
                            ParamFileName, _
                            ParamFileType, _
                            FASTAFilePath, _
                            OutputFilePath, _
                            DMSConnectionString, _
                            ForceMonoParentMass)
                End Select
                Return True

            Catch ex As Exception
                Me.m_LastError = ex.Message
                Return False

            End Try


        End Function

        Protected Function GetMonoMassStatus(ByVal DatasetID As Integer, ByVal DMSConnectionString As String) As Boolean
            Dim TypeCheckSQL As String = "SELECT TOP 1 Use_Mono_Parent FROM V_Analysis_Job_Use_MonoMass WHERE Dataset_ID = " + DatasetID.ToString
            Return Me.GetMonoParentStatusWorker(TypeCheckSQL, DMSConnectionString)
        End Function

        Protected Function GetMonoMassStatus(ByVal DatasetName As String, ByVal DMSConnectionString As String) As Boolean
            Dim TypeCheckSQL As String = "SELECT TOP 1 Use_Mono_Parent FROM V_Analysis_Job_Use_MonoMass WHERE Dataset_Name = '" + DatasetName + "'"
            Return Me.GetMonoParentStatusWorker(TypeCheckSQL, DMSConnectionString)
        End Function

        Private Function GetMonoParentStatusWorker(ByVal LookupSQL As String, ByVal DMSConnectionString As String) As Boolean
            Dim TypeCheckTable As DataTable

            Dim UseMonoMass As Boolean
            Dim UseMonoMassInt As Integer

            If Me.m_TableGetter Is Nothing Then
                Me.m_TableGetter = New clsDBTask(DMSConnectionString)
            End If

            TypeCheckTable = Me.m_TableGetter.GetTable(LookupSQL)

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

        Protected Function MakeFileSQ( _
            ByVal ParamFileName As String, _
            ByVal ParamFileType As IGenerateFile.ParamFileType, _
            ByVal FASTAFilePath As String, _
            ByVal OutputFilePath As String, _
            ByVal DMSConnectionString As String, _
            ByVal forceMonoParentMass As Boolean) As Boolean

            If Me.m_TableGetter Is Nothing Then
                Me.m_TableGetter = New clsDBTask(DMSConnectionString)
            End If

            Const DEF_TEMPLATE_FILEPATH As String = "\\Gigasax\dms_parameter_files\Sequest\sequest_N14_NE_Template.params"

            If Me.m_TemplateFilePathString = "" Then
                Me.m_TemplateFilePathString = DEF_TEMPLATE_FILEPATH
            End If

            Dim success As Boolean = False
            Dim successExtra As Boolean = False

            'Try
            Dim fi As New System.IO.FileInfo(m_TemplateFilePathString)
            If Not fi.Exists Then
                success = False
                Me.m_LastError = "Default template file '" & m_TemplateFilePathString & "' does not exist"
                Return success
            End If

            Dim l_MainCode As New clsMainProcess(m_TemplateFilePathString)

            Dim l_LoadedParams As clsParams
            Dim l_DMS As New DownloadParams.clsParamsFromDMS(DMSConnectionString)
            Dim l_ReconIsoMods As IReconstituteIsoMods
            l_ReconIsoMods = New clsReconstitueIsoMods(DMSConnectionString)

            If l_DMS.ParamFileTable Is Nothing Then
                success = False
                m_LastError = "Could Not Establish Database Connection"
                Return success
            End If

            If Not l_DMS.ParamSetNameExists(ParamFileName) Then
                success = False
                m_LastError = "Named Parameter File does not exist in the database"
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

            If Me.m_FileWriter Is Nothing Then
                Me.m_FileWriter = New clsWriteOutput
            End If

            success = Me.m_FileWriter.WriteOutputFile(l_LoadedParams, System.IO.Path.Combine(OutputFilePath, ParamFileName), ParamFileType)

            successExtra = Me.MakeSeqInfoRelatedFiles(ParamFileName, OutputFilePath, DMSConnectionString)

            Return success

        End Function


        Protected Function MakeFileInspect( _
            ByVal ParamFileName As String, _
            ByVal OutputFilePath As String, _
            ByVal DMSConnectionString As String) As Boolean

            Dim paramFilePathSQL As String

            Dim paramFilePathTable As DataTable
            Dim paramFilePath As String

            Dim fullOutputFilePath As String


            If Me.m_TableGetter Is Nothing Then
                Me.m_TableGetter = New clsDBTask(DMSConnectionString)
            End If

            paramFilePathSQL = _
                "SELECT TOP 1 AJT_parmFileStoragePath " & _
                "FROM T_Analysis_Tool " & _
                "WHERE AJT_Toolname = 'Inspect'"

            paramFilePathTable = Me.m_TableGetter.GetTable(paramFilePathSQL)

            paramFilePath = System.IO.Path.Combine(paramFilePathTable.Rows(0).Item(0).ToString, ParamFileName)
            fullOutputFilePath = System.IO.Path.Combine(OutputFilePath, ParamFileName)

            'Make sure that the paramfile doesn't already exist
            If System.IO.File.Exists(fullOutputFilePath) Then
                System.IO.File.Delete(fullOutputFilePath)
            End If

            'Copy the param file from gigasax to the working directory
            System.IO.File.Copy(paramFilePath, System.IO.Path.Combine(OutputFilePath, ParamFileName))

            Me.MakeSeqInfoRelatedFiles(ParamFileName, OutputFilePath, DMSConnectionString)

            Return True

        End Function

        Protected Function MakeFileXT( _
            ByVal ParamFileName As String, _
            ByVal OutputFilePath As String, _
            ByVal DMSConnectionString As String) As Boolean

            Dim paramFilePathSQL As String

            Dim paramFilePathTable As DataTable
            Dim paramFilePath As String

            Dim fullOutputFilePath As String


            If Me.m_TableGetter Is Nothing Then
                Me.m_TableGetter = New clsDBTask(DMSConnectionString)
            End If

            paramFilePathSQL = _
                "SELECT TOP 1 AJT_parmFileStoragePath " & _
                "FROM T_Analysis_Tool " & _
                "WHERE AJT_Toolname = 'XTandem'"

            paramFilePathTable = Me.m_TableGetter.GetTable(paramFilePathSQL)

            paramFilePath = System.IO.Path.Combine(paramFilePathTable.Rows(0).Item(0).ToString, ParamFileName)
            fullOutputFilePath = System.IO.Path.Combine(OutputFilePath, ParamFileName)

            'Make sure that the paramfile doesn't already exist
            If System.IO.File.Exists(fullOutputFilePath) Then
                System.IO.File.Delete(fullOutputFilePath)
            End If

            'Copy the param file from gigasax to the working directory
            System.IO.File.Copy(paramFilePath, System.IO.Path.Combine(OutputFilePath, ParamFileName))

            Me.MakeSeqInfoRelatedFiles(ParamFileName, OutputFilePath, DMSConnectionString)

            Return True

        End Function

        Private Function MakeSeqInfoRelatedFiles( _
            ByVal ParamFileName As String, _
            ByVal OutputFilePath As String, _
            ByVal DMSConnectionString As String) As Boolean

            Dim mctSQL As String
            Dim mdSQL As String

            Dim mctTable As DataTable
            Dim mdTable As DataTable

            If Me.m_TableGetter Is Nothing Then
                Me.m_TableGetter = New ParamFileGenerator.clsDBTask(DMSConnectionString)
            End If

            Dim baseParamfileName As String
            baseParamfileName = System.IO.Path.GetFileNameWithoutExtension(ParamFileName)

            If Me.m_FileWriter Is Nothing Then
                Me.m_FileWriter = New clsWriteOutput
            End If

            mctSQL = _
                "SELECT Mass_Correction_Tag, Monoisotopic_Mass_Correction, Affected_Atom " & _
                "FROM T_Mass_Correction_Factors " & _
                "ORDER BY Mass_Correction_Tag"

            mdSQL = _
                "SELECT Local_Symbol, Monoisotopic_Mass_Correction, Residue_Symbol, Mod_Type_Symbol, Mass_Correction_Tag " & _
                "FROM V_Param_File_Mass_Mod_Info " & _
                "WHERE Param_File_Name = '" & ParamFileName & "'"

            mctTable = Me.m_TableGetter.GetTable(mctSQL)

            mdTable = Me.m_TableGetter.GetTable(mdSQL)

            'Dump the Mass_Correction_Tags file to the working directory
            Me.m_FileWriter.WriteDatatableToOutputFile(mctTable, System.IO.Path.Combine(OutputFilePath, "Mass_Correction_Tags.txt"))

            'Dump the param file specific Mod Defs file to the working directory
            Me.m_FileWriter.WriteDatatableToOutputFile(mdTable, System.IO.Path.Combine(OutputFilePath, baseParamfileName & "_ModDefs.txt"))


        End Function



        Protected Function GetAvailableParamSetNames(ByVal DMSConnectionString As String) _
            As System.Collections.Specialized.StringCollection Implements IGenerateFile.GetAvailableParamSetNames

            Dim l_ParamSetsAvailable As New System.Collections.Specialized.StringCollection
            Dim l_DMS As New DownloadParams.clsParamsFromDMS(DMSConnectionString)
            Dim d_ParamSetsAvailable As DataTable = l_DMS.RetrieveAvailableParams()
            Dim dr As DataRow
            For Each dr In d_ParamSetsAvailable.Rows
                l_ParamSetsAvailable.Add(dr.Item("FileName").ToString)
            Next
            Return l_ParamSetsAvailable
        End Function

        Protected Function GetAvailableParamSetTable(ByVal DMSConnectionString As String) As DataTable Implements IGenerateFile.GetAvailableParamSetTable
            Dim l_DMS As New DownloadParams.clsParamsFromDMS(DMSConnectionString)
            Dim paramSetsAvailable As DataTable = l_DMS.RetrieveAvailableParams

            l_DMS = Nothing

            Return paramSetsAvailable

        End Function

        Protected Function GetAvailableParamSetTypes(ByVal DMSConnectionString As String) As DataTable Implements IGenerateFile.GetAvailableParamFileTypes
            Dim l_DMS As New DownloadParams.clsParamsFromDMS(DMSConnectionString)
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
