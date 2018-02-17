Imports System.Collections.Specialized
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

    <Obsolete("Unused")>
    Public ReadOnly Property ParamSetsAddedList As StringCollection

    <Obsolete("Unused")>
    Public ReadOnly Property ParamSetsChangedList As StringCollection

    <Obsolete("Unused")>
    Public ReadOnly Property ParamSetsSkippedList As StringCollection

#End Region

#Region " Member Variables "
    Private m_added As Integer
    Private m_changed As Integer
    Private m_skipped As Integer

#End Region

#Region " Public Functions "

    Public Function UploadParamSetsToDMS(FilePathList As StringCollection) As Boolean
        Return BatchUploadParamSetsToDMS(FilePathList)
    End Function

#End Region

#Region " Member Functions "

    Private Function BatchUploadParamSetsToDMS(ParamFilePathList As StringCollection) As Boolean
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
                    Dim results = MessageBox.Show("A parameter set with the ID " & ParamSetID & " and the name '" & c.FileName &
                            "' already exists with the following differences '" & ParamSetDiffs & " Do you want to replace it?", "Parameter Set Exists!",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
                    If results = DialogResult.Yes Then
                        changed = changed + 1
                        Me.WriteParamsToDMS(c, True)
                    ElseIf results = DialogResult.No Then

                    End If
                End If
            Else
                'ParamSetID = Me.GetNextParamSetID
                'Me.WriteParamsToLocalStructure(c, ParamSetID)
                added = added + 1
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
