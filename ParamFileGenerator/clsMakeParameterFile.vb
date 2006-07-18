Namespace MakeParams

    Public Interface IGenerateFile
        Enum SequestParamFileType
            BioWorks_20  'Normal BioWorks 2.0 Sequest
            BioWorks_30  'BioWorks 3.0+ TurboSequest
            BioWorks_31  'BioWorks 3.1 ClusterQuest
        End Enum
        Function MakeFile(ByVal ParamFileName As String, _
             ByVal ParamFileType As SequestParamFileType, _
             ByVal FASTAFilePath As String, _
             ByVal OutputFilePath As String, _
             ByVal DMSConnectionString As String) As Boolean
        Function GetAvailableParamSetNames(ByVal DMSConnectionString As String) As System.Collections.Specialized.StringCollection
        ReadOnly Property LastError() As String


    End Interface


    Public Class clsMakeParameterFile
        Implements IGenerateFile

        Private m_LastError As String

        Public Function MakeFile( _
            ByVal ParamFileName As String, _
            ByVal ParamFileType As IGenerateFile.SequestParamFileType, _
            ByVal FASTAFilePath As String, _
            ByVal OutputFilePath As String, _
            ByVal DMSConnectionString As String) As Boolean Implements IGenerateFile.MakeFile

            Try

                Dim l_LoadedParams As clsParams
                Dim l_DMS As New clsParamsFromDMS(DMSConnectionString)
                Dim success As Boolean = False

                If l_DMS.ParamSetTable Is Nothing Then
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

            Dim l_ParamSetsAvailable As System.Collections.Specialized.StringCollection
            Dim l_DMS As New clsParamsFromDMS(DMSConnectionString)
            l_ParamSetsAvailable = l_DMS.RetrieveAvailableParams()
            Return l_ParamSetsAvailable
        End Function

        Public ReadOnly Property LastError() As String Implements IGenerateFile.LastError
            Get
                Return m_LastError
            End Get
        End Property
    End Class
End Namespace
