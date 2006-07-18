Imports ParamFileGenerator
Namespace MakeParams

    Public Interface IGenerateFile
        Enum ParamFileType
            BioWorks_20  'Normal BioWorks 2.0 Sequest
            BioWorks_30  'BioWorks 3.0+ TurboSequest
            BioWorks_31  'BioWorks 3.1 ClusterQuest
        End Enum
        Function MakeFile(ByVal ParamFileName As String, _
             ByVal ParamFileType As ParamFileType, _
             ByVal FASTAFilePath As String, _
             ByVal OutputFilePath As String, _
             ByVal DMSConnectionString As String) As Boolean
        Function GetAvailableParamSetNames(ByVal DMSConnectionString As String) As System.Collections.Specialized.StringCollection
        ReadOnly Property LastError() As String


    End Interface


    public Class clsMakeParameterFile
        Implements IGenerateFile

        Private m_LastError As String

        Public Function MakeFile( _
            ByVal ParamFileName As String, _
            ByVal ParamFileType As IGenerateFile.ParamFileType, _
            ByVal FASTAFilePath As String, _
            ByVal OutputFilePath As String, _
            ByVal DMSConnectionString As String) As Boolean Implements IGenerateFile.MakeFile

            Try

                Dim l_LoadedParams As clsParams
                Dim l_DMS As New DownloadParams.clsParamsFromDMS(DMSConnectionString)
                Dim success As Boolean = False

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

                l_LoadedParams.DefaultFASTAPath = FASTAFilePath

                Dim l_Writer As New clsWriteOutput
                success = l_Writer.WriteOutputFile(l_LoadedParams, System.IO.Path.Combine(OutputFilePath, ParamFileName), ParamFileType)

                Return success
            Catch ex As Exception
                m_LastError = "Some other crazy thing happened... Hell if I know."
                Return False
            End Try

        End Function

        Public Function GetAvailableParamSetNames(ByVal DMSConnectionString As String) _
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

        Public ReadOnly Property LastError() As String Implements IGenerateFile.LastError
            Get
                Return m_LastError
            End Get
        End Property
    End Class
End Namespace
