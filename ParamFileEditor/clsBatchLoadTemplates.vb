Imports System.Collections.Specialized
Imports ParamFileEditor.DownloadParams

'Framework to handle the batch upload and download of templates/parameters to and from DMS
'
'   
Friend Class clsBatchLoadTemplates
    Inherits clsDMSParamUpload

    Public Sub New(ByVal MainCode As ParamFileEditor.clsMainProcess)
        MyBase.New(MainCode.mySettings)
        m_Main = MainCode
    End Sub

#Region " Public Properties "
    Public ReadOnly Property NumParamSetsAdded() As Integer
        Get
            Return m_added
        End Get
    End Property
    Public ReadOnly Property NumParamSetsChanged() As Integer
        Get
            Return m_changed
        End Get
    End Property
    Public ReadOnly Property NumParamSetsSkipped() As Integer
        Get
            Return m_skipped
        End Get
    End Property

    Public ReadOnly Property ParamSetsAddedList() As StringCollection
        Get
            Return m_addedList
        End Get
    End Property
    Public ReadOnly Property ParamSetsChangedList() As StringCollection
        Get
            Return m_changedList
        End Get
    End Property
    Public ReadOnly Property ParamSetsSkippedList() As StringCollection
        Get
            Return m_skippedList
        End Get
    End Property
#End Region

#Region " Member Variables "
    Private m_FilePathList As StringCollection
    Private m_Main As clsMainProcess
    Private m_added As Integer
    Private m_changed As Integer
    Private m_skipped As Integer
    Private m_addedList As StringCollection
    Private m_changedList As StringCollection
    Private m_skippedList As StringCollection

#End Region

#Region " Public Functions "

    Public Function UploadParamSetsToDMS(ByVal FilePathList As StringCollection) As Integer
        Return BatchUploadParamSetsToDMS(FilePathList)
    End Function

#End Region

#Region " Member Functions "

    Private Function BatchUploadParamSetsToDMS(ByVal ParamFilePathList As StringCollection) As Boolean
        'Returns number of param sets uploaded
        Dim ParamFilePath As String
        Dim ParamFileName As String
        Dim ParamSetID As Integer
        Dim ParamSetDiffs As String
        Dim c As clsParams
        Dim checkSet As clsParams
        Dim added As Integer
        Dim skipped As Integer
        Dim changed As Integer

        Dim results As New DialogResult

        For Each ParamFilePath In ParamFilePathList
            c = New clsParams
            'Try
            ParamFileName = Mid(ParamFilePath, InStrRev(ParamFilePath, "\") + 1).ToString
            Console.WriteLine("Working on: " & ParamFileName)
            c.LoadTemplate(ParamFilePath)
            c.Description = Me.GetDiffsBetweenSets(clsMainProcess.BaseLineParamSet, c)
            c.FileName = ParamFileName
            If Me.ParamSetNameExists(c.FileName) Then
                ParamSetID = Me.GetParamSetIDFromName(c.FileName)
                checkSet = Me.ReadParamsFromDMS(ParamSetID)
                ParamSetDiffs = Me.GetDiffsBetweenSets(c, checkSet)
                If ParamSetDiffs = " --No Change-- " Then
                    skipped = skipped + 1
                Else
                    results = MessageBox.Show("A parameter set with the ID " & ParamSetID & " and the name '" & c.FileName & _
                            "' already exists with the following differences '" & ParamSetDiffs & " Do you want to replace it?", "Parameter Set Exists!", _
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                    If results = DialogResult.Yes Then
                        changed = changed + 1
                        Me.WriteParamsToDMS(c, ParamSetID)
                    ElseIf results = DialogResult.No Then

                    End If
                End If
            Else
                ParamSetID = Me.GetNextParamSetID
                Me.WriteParamsToLocalStructure(c, ParamSetID)
                added = added + 1
            End If


            'Catch ex As Exception
            '    skipped = skipped + 1
            'End Try
            Console.WriteLine("Finished")
        Next

        Me.WriteLocalStructureToDMS()

        m_added = added
        m_changed = changed
        m_skipped = skipped

        Return True
    End Function

#End Region


End Class
