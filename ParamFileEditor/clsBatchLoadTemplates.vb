Imports System.Collections.Generic
Imports ParamFileGenerator

'Framework to handle the batch upload and download of templates/parameters to and from DMS
'
'
Friend Class clsBatchLoadTemplates
    Inherits clsDMSParamUpload

    Public Sub New()
        MyBase.New(frmMainGUI.mySettings)
        'm_Main = MainCode
    End Sub

#Region " Public Properties "
    Public ReadOnly Property NumParamSetsAdded As Integer
        Get
            Return m_added
        End Get
    End Property

    Public ReadOnly Property NumParamSetsChanged As Integer
        Get
            Return m_changed
        End Get
    End Property

    Public ReadOnly Property NumParamSetsSkipped As Integer
        Get
            Return m_skipped
        End Get
    End Property

#End Region

#Region " Member Variables "
    Private m_added As Integer
    Private m_changed As Integer
    Private m_skipped As Integer

#End Region

#Region " Public Functions "

    Public Function UploadParamSetsToDMS(filePaths As List(Of String)) As Boolean
        Return BatchUploadParamSetsToDMS(filePaths)
    End Function

#End Region

#Region " Member Functions "

    ''' <summary>
    ''' Batch upload a list of parameter files
    ''' </summary>
    ''' <param name="paramFileList"></param>
    ''' <returns>Number of param sets uploaded</returns>
    Private Function BatchUploadParamSetsToDMS(paramFileList As List(Of String)) As Boolean
        Dim added = 0
        Dim skipped = 0
        Dim changed = 0

        For Each paramFilePath In paramFileList
            Dim params = New clsParams()
            'Try
            Dim paramFileName = Mid(paramFilePath, InStrRev(paramFilePath, "\") + 1).ToString
            Console.WriteLine("Working on: " & paramFileName)
            params.LoadTemplate(paramFilePath)
            params.Description = Me.GetDiffsBetweenSets(clsMainProcess.BaseLineParamSet, params)
            params.FileName = paramFileName
            If Me.ParamSetNameExists(params.FileName) Then
                Dim ParamSetID = Me.GetParamSetIDFromName(params.FileName)
                Dim checkSet = Me.ReadParamsFromDMS(ParamSetID)
                Dim ParamSetDiffs = Me.GetDiffsBetweenSets(params, checkSet)
                If ParamSetDiffs = " --No Change-- " Then
                    skipped += 1
                Else
                    Dim results = MessageBox.Show("A parameter set with the ID " & ParamSetID & " and the name '" & params.FileName &
                            "' already exists with the following differences '" & ParamSetDiffs & " Do you want to replace it?", "Parameter Set Exists!",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                    If results = DialogResult.Yes Then
                        changed += 1
                        Me.WriteParamsToDMS(params, True)
                    ElseIf results = DialogResult.No Then

                    End If
                End If
            Else
                'ParamSetID = Me.GetNextParamSetID
                'Me.WriteParamsToLocalStructure(c, ParamSetID)
                added += 1
            End If


            'Catch ex As Exception
            '    skipped = skipped + 1
            'End Try
            Console.WriteLine("Finished")
        Next

        'Me.WriteLocalStructureToDMS()

        m_added = added
        m_changed = changed
        m_skipped = skipped

        Return True
    End Function

#End Region


End Class
