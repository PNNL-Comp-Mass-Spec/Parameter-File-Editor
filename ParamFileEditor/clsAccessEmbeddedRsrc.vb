Imports System.IO

Friend Class clsAccessEmbeddedRsrc
    Private ReadOnly m_EmbeddedRsrcList As Array
    Private ReadOnly executingAssembly As System.Reflection.Assembly
    Private ReadOnly myNameSpace As String
    Private ReadOnly myExecutingDirectory As String


    Friend Sub New()
        Me.m_EmbeddedRsrcList = Me.GetListOfEmbeddedResources
        Me.executingAssembly = Reflection.Assembly.GetExecutingAssembly
        Me.myNameSpace = Me.executingAssembly.GetName().Name.ToString
        Me.myExecutingDirectory = System.IO.Path.GetDirectoryName(Me.executingAssembly.Location)
    End Sub

    Private Function GetListOfEmbeddedResources() As Array
        Return System.Reflection.Assembly.GetExecutingAssembly.GetManifestResourceNames

    End Function

    Private Function GetEmbeddedTextStream(streamName As String) As System.IO.Stream
        Dim executingAssembly As System.Reflection.Assembly = Reflection.Assembly.GetEntryAssembly()
        Dim myNameSpace As String = executingAssembly.GetName().Name.ToString
        'Me.printDebugList()
        Dim streamLocation As String = Me.myNameSpace + "." + streamName
        Dim ts As System.IO.Stream = _
             Me.executingAssembly.GetManifestResourceStream(myNameSpace + "." + streamName)
        Return ts
    End Function

    Private Function GetEmbeddedBitmap(pictureName As String) As Bitmap
        Dim bm As Bitmap
        Dim p As Stream
        p = GetEmbeddedTextStream(pictureName)

        bm = New Bitmap(p)
        p.Close()
        Return bm
    End Function

    Private Sub WriteEmbeddedTextStreamToFile(ResourceName As String, OutputFilePath As String)
        'Grab embedded stream
        Dim s As String

        Dim resourceStream As Stream = GetEmbeddedTextStream(ResourceName)
        Dim rsReader As New StreamReader(resourceStream)

        Dim outputStream As New IO.FileStream(OutputFilePath, FileMode.Create, FileAccess.Write)
        Dim sw As New StreamWriter(outputStream)
        'sw.BaseStream.Seek(0, SeekOrigin.End)

        While rsReader.Peek <> -1
            s = rsReader.ReadLine
            sw.WriteLine(s)
        End While
        sw.Flush()

        rsReader.Close()
        resourceStream.Close()
        outputStream.Close()

    End Sub
    Friend Sub RestoreFromEmbeddedResource(ResourceName As String)
        Me.WriteEmbeddedTextStreamToFile(ResourceName, Path.Combine(Me.myExecutingDirectory, ResourceName))
    End Sub
    Friend Sub RestoreFromEmbeddedResource(ResourceName As String, OutputFilePath As String)
        Me.WriteEmbeddedTextStreamToFile(ResourceName, OutputFilePath)
    End Sub

    Friend Function GetBitmapResource(pictureName As String) As Bitmap
        Return GetEmbeddedBitmap(pictureName)
    End Function

    Friend Function ResourceExists(resourceName As String) As Boolean
        Dim fi As New FileInfo(Path.Combine(Me.myExecutingDirectory, resourceName))
        If fi.Exists Then
            Return True
        Else
            Return False
        End If
    End Function

    Friend ReadOnly Property AvailableEmbeddedResources As Array
        Get
            Return Me.m_EmbeddedRsrcList
        End Get
    End Property

    Friend ReadOnly Property ExecutingDirectory As String
        Get
            Return Me.myExecutingDirectory
        End Get
    End Property

End Class
