Option Strict Off

Imports System.Collections.Generic
Imports System.IO
Imports System.Xml
Imports System.Text

Enum IniItemTypeEnum
    GetKeys = 0
    GetValues = 1
    GetKeysAndValues = 2
End Enum

Public Class IniFileReaderNotInitializedException
    Inherits System.ApplicationException
    Public Overrides ReadOnly Property Message As String
        Get
            Return "The IniFileReader instance has not been properly initialized."

        End Get
    End Property
End Class

Public Class IniFileReader
    Private m_IniFilename As String
    Private m_XmlDoc As XmlDocument
    Private sections As List(Of String) = New List(Of String)
    Private m_CaseSensitive As Boolean = False
    Private m_SaveFilename As String
    Private m_initialized As Boolean = False

    Public Sub New(settingsFileName As String)
        InitIniFileReader(settingsFileName, False)
    End Sub

    Public Sub New(settingsFileName As String, isCaseSensitive As Boolean)
        InitIniFileReader(settingsFileName, isCaseSensitive)
    End Sub

    Private Sub InitIniFileReader(settingsFileName As String, isCaseSensitive As Boolean)
        Dim fi As FileInfo
        Dim s As String
        Dim tr As TextReader = Nothing
        m_CaseSensitive = isCaseSensitive
        m_XmlDoc = New XmlDocument

        If ((settingsFileName Is Nothing) OrElse (settingsFileName.Trim() = "")) Then
            Return
        End If
        ' try to load the file as an XML file
        Try
            m_XmlDoc.Load(settingsFileName)
            UpdateSections()
            m_IniFilename = settingsFileName
            m_initialized = True

        Catch
            ' load the default XML
            m_XmlDoc.LoadXml("<?xml version=""1.0"" encoding=""UTF-8""?><sections></sections>")
            Try
                fi = New FileInfo(settingsFileName)
                If fi.Exists Then
                    tr = fi.OpenText
                    s = tr.ReadLine()
                    Do While Not s Is Nothing
                        If InStr(s, ";") Then
                            s = Left(s, InStr(s, ";") - 1).Trim
                        End If
                        ParseLineXml(s, m_XmlDoc)
                        s = tr.ReadLine()
                    Loop
                    m_IniFilename = settingsFileName
                    m_initialized = True
                Else
                    m_XmlDoc.Save(settingsFileName)
                    m_IniFilename = settingsFileName
                    m_initialized = True
                End If
            Catch e As Exception
                'MessageBox.Show(e.Message)
            Finally
                If (Not tr Is Nothing) Then
                    tr.Close()
                End If
            End Try
        End Try
    End Sub

    Public ReadOnly Property IniFilename As String
        Get
            If Not Initialized Then Throw New IniFileReaderNotInitializedException
            Return (m_IniFilename)
        End Get
    End Property

    Public ReadOnly Property Initialized As Boolean
        Get
            Return m_initialized
        End Get
    End Property

    Public ReadOnly Property CaseSensitive As Boolean
        Get
            Return m_CaseSensitive
        End Get
    End Property

    Private Function SetNameCase(aName As String) As String
        If (CaseSensitive) Then
            Return aName
        Else
            Return aName.ToLower()
        End If
    End Function

    Private Function GetRoot() As XmlElement
        Return m_XmlDoc.DocumentElement
    End Function

    Private Function GetLastSection() As XmlElement
        If sections.Count = 0 Then
            Return GetRoot()
        Else
            Return GetSection(sections(sections.Count - 1))
        End If
    End Function

    Private Function GetSection(sectionName As String) As XmlElement
        If (Not (sectionName = Nothing)) AndAlso (sectionName <> "") Then
            sectionName = SetNameCase(sectionName)
            Return CType(m_XmlDoc.SelectSingleNode("//section[@name='" & sectionName & "']"), XmlElement)
        End If
        Return Nothing
    End Function

    Private Function GetItem(sectionName As String, keyName As String) As XmlElement
        Dim section As XmlElement
        If (Not keyName Is Nothing) AndAlso (keyName <> "") Then
            keyName = SetNameCase(keyName)
            section = GetSection(sectionName)
            If (Not section Is Nothing) Then
                Return CType(section.SelectSingleNode("item[@key='" + keyName + "']"), XmlElement)
            End If
        End If
        Return Nothing
    End Function

    Public Function SetIniSection(oldSection As String, newSection As String) As Boolean
        Dim section As XmlElement
        If Not Initialized Then
            Throw New IniFileReaderNotInitializedException
        End If
        If (Not newSection Is Nothing) AndAlso (newSection <> "") Then
            section = GetSection(oldSection)
            If (Not (section Is Nothing)) Then
                section.SetAttribute("name", SetNameCase(newSection))
                UpdateSections()
                Return True
            End If
        End If
        Return False
    End Function

    Public Function SetIniValue(sectionName As String, keyName As String, newValue As String) As Boolean
        Dim item As XmlElement
        Dim section As XmlElement
        If Not Initialized Then Throw New IniFileReaderNotInitializedException
        section = GetSection(sectionName)
        If section Is Nothing Then
            If CreateSection(sectionName) Then
                section = GetSection(sectionName)
                ' exit if keyName is Nothing or blank
                If (keyName Is Nothing) OrElse (keyName = "") Then
                    Return True
                End If
            Else
                ' can't create section
                Return False
            End If
        End If
        If keyName Is Nothing Then
            ' delete the section
            Return DeleteSection(sectionName)
        End If

        item = GetItem(sectionName, keyName)
        If Not item Is Nothing Then
            If newValue Is Nothing Then
                ' delete this item
                Return DeleteItem(sectionName, keyName)
            Else
                ' add or update the value attribute
                item.SetAttribute("value", newValue)
                Return True
            End If
        Else
            ' try to create the item
            If (keyName <> "") AndAlso (Not newValue Is Nothing) Then
                ' construct a new item (blank values are OK)
                item = m_XmlDoc.CreateElement("item")
                item.SetAttribute("key", SetNameCase(keyName))
                item.SetAttribute("value", newValue)
                section.AppendChild(item)
                Return True
            End If
        End If
        Return False
    End Function

    Private Function DeleteSection(sectionName As String) As Boolean
        Dim section As XmlElement = GetSection(sectionName)
        If Not section Is Nothing Then
            section.ParentNode.RemoveChild(section)
            UpdateSections()
            Return True
        End If
        Return False
    End Function

    Private Function DeleteItem(sectionName As String, keyName As String) As Boolean
        Dim item As XmlElement = GetItem(sectionName, keyName)
        If Not item Is Nothing Then
            item.ParentNode.RemoveChild(item)
            Return True
        End If
        Return False
    End Function

    Public Function SetIniKey(sectionName As String, keyName As String, newValue As String) As Boolean
        If Not Initialized Then Throw New IniFileReaderNotInitializedException
        Dim item As XmlElement = GetItem(sectionName, keyName)
        If Not item Is Nothing Then
            item.SetAttribute("key", SetNameCase(newValue))
            Return True
        End If
        Return False
    End Function

    Public Function GetIniValue(sectionName As String, keyName As String) As String
        If Not Initialized Then Throw New IniFileReaderNotInitializedException
        Dim N As XmlNode = GetItem(sectionName, keyName)
        If Not N Is Nothing Then
            Return (N.Attributes.GetNamedItem("value").Value)
        End If
        Return Nothing
    End Function

    Public Function GetIniComments(sectionName As String) As StringCollection
        If Not Initialized Then Throw New IniFileReaderNotInitializedException
        Dim sc As StringCollection = New StringCollection
        Dim target As XmlNode
        Dim nodes As XmlNodeList
        Dim N As XmlNode
        If sectionName Is Nothing Then
            target = m_XmlDoc.DocumentElement
        Else
            target = GetSection(sectionName)
        End If
        If Not target Is Nothing Then
            nodes = target.SelectNodes("comment")
            If nodes.Count > 0 Then
                For Each N In nodes
                    sc.Add(N.InnerText)
                Next
            End If
        End If
        Return sc
    End Function

    Public Function SetIniComments(sectionName As String, comments As StringCollection) As Boolean
        If Not Initialized Then Throw New IniFileReaderNotInitializedException
        Dim target As XmlNode
        Dim nodes As XmlNodeList
        Dim N As XmlNode
        Dim s As String
        Dim NLastComment As XmlElement
        If sectionName Is Nothing Then
            target = m_XmlDoc.DocumentElement
        Else
            target = GetSection(sectionName)
        End If
        If Not target Is Nothing Then
            nodes = target.SelectNodes("comment")
            For Each N In nodes
                target.RemoveChild(N)
            Next
            For Each s In comments
                N = m_XmlDoc.CreateElement("comment")
                N.InnerText = s
                NLastComment = CType(target.SelectSingleNode("comment[last()]"), XmlElement)
                If NLastComment Is Nothing Then
                    target.PrependChild(N)
                Else
                    target.InsertAfter(N, NLastComment)
                End If
            Next
            Return True
        End If
        Return False
    Private Sub UpdateSections()
        sections = New List(Of String)

        For Each item As XmlElement In m_XmlDoc.SelectNodes("sections/section")
            sections.Add(item.GetAttribute("name"))
        Next
    End Sub

    Public ReadOnly Property AllSections As List(Of String)
        Get
            If Not Initialized Then
                Throw New IniFileReaderNotInitializedException
            End If
            Return sections
        End Get
    End Property

    Private Function GetItemsInSection(sectionName As String, itemType As IniItemTypeEnum) As List(Of String)
        Dim nodes As XmlNodeList
        Dim items = New List(Of String)
        Dim section As XmlNode = GetSection(sectionName)

        If section Is Nothing Then
            Return Nothing
        End If

        nodes = section.SelectNodes("item")
        If nodes.Count > 0 Then
            For Each currentNode As XmlNode In nodes
                Select Case itemType
                    Case IniItemTypeEnum.GetKeys
                        items.Add(currentNode.Attributes.GetNamedItem("key").Value)
                    Case IniItemTypeEnum.GetValues
                        items.Add(currentNode.Attributes.GetNamedItem("value").Value)
                    Case IniItemTypeEnum.GetKeysAndValues
                        items.Add(currentNode.Attributes.GetNamedItem("key").Value & "=" &
                                  currentNode.Attributes.GetNamedItem("value").Value)
                End Select
            Next
        End If
        Return items

    End Function

    Public Function AllKeysInSection(sectionName As String) As List(Of String)
        If Not Initialized Then Throw New IniFileReaderNotInitializedException
        Return GetItemsInSection(sectionName, IniItemTypeEnum.GetKeys)
    End Function

    Public Function AllValuesInSection(sectionName As String) As List(Of String)
        If Not Initialized Then Throw New IniFileReaderNotInitializedException
        Return GetItemsInSection(sectionName, IniItemTypeEnum.GetValues)
    End Function

    Public Function AllItemsInSection(sectionName As String) As List(Of String)
        If Not Initialized Then Throw New IniFileReaderNotInitializedException
        Return (GetItemsInSection(sectionName, IniItemTypeEnum.GetKeysAndValues))
    End Function

    Public Function GetCustomIniAttribute(sectionName As String, keyName As String, attributeName As String) As String
        Dim N As XmlElement
        If Not Initialized Then Throw New IniFileReaderNotInitializedException
        If (Not attributeName Is Nothing) AndAlso (attributeName <> "") Then
            N = GetItem(sectionName, keyName)
            If Not N Is Nothing Then
                attributeName = SetNameCase(attributeName)
                Return N.GetAttribute(attributeName)
            End If
        End If
        Return Nothing
    End Function

    Public Function SetCustomIniAttribute(sectionName As String, keyName As String, attributeName As String, attributeValue As String) As Boolean
        If Not Initialized Then Throw New IniFileReaderNotInitializedException
        If attributeName = "" Then
            Return False
        End If


        Dim item = GetItem(sectionName, keyName)
        If Not item Is Nothing Then
            Try
                If attributeValue Is Nothing Then
                    ' delete the attribute
                    item.RemoveAttribute(attributeName)
                    Return True
                Else
                    attributeName = SetNameCase(attributeName)
                    item.SetAttribute(attributeName, attributeValue)
                    Return True
                End If

            Catch e As Exception
                'MessageBox.Show(e.Message)
            End Try
        End If

        Return False

    End Function

    Private Function CreateSection(sectionName As String) As Boolean
        Dim item As XmlElement
        Dim itemAttribute As XmlAttribute
        If (Not sectionName Is Nothing) AndAlso (sectionName <> "") Then
            sectionName = SetNameCase(sectionName)
            Try
                item = m_XmlDoc.CreateElement("section")
                itemAttribute = m_XmlDoc.CreateAttribute("name")
                itemAttribute.Value = SetNameCase(sectionName)
                item.Attributes.SetNamedItem(itemAttribute)
                m_XmlDoc.DocumentElement.AppendChild(item)
                sections.Add(itemAttribute.Value)
                Return True
            Catch e As Exception
                'MessageBox.Show(e.Message)
                Return False
            End Try
        End If
        Return False
    End Function

    Private Function CreateItem(sectionName As String, keyName As String, newValue As String) As Boolean
        Dim item As XmlElement
        Dim section As XmlElement
        Try
            section = GetSection(sectionName)
            If Not section Is Nothing Then
                item = m_XmlDoc.CreateElement("item")
                item.SetAttribute("key", keyName)
                item.SetAttribute("newValue", newValue)
                section.AppendChild(item)
                Return True
            End If
            Return False
        Catch e As Exception
            'MessageBox.Show(e.Message)
            Return False
        End Try
    End Function

    Private Sub ParseLineXml(dataLine As String, doc As XmlDocument)

        dataLine = dataLine.TrimStart()

        If String.IsNullOrWhiteSpace(dataLine) Then
            Return
        End If

        Select Case (dataLine.Substring(0, 1))
            Case "["
                ' this is a section
                ' trim the first and last characters
                dataLine = dataLine.TrimStart("[")
                dataLine = dataLine.TrimEnd("]")
                ' create a new section element
                CreateSection(dataLine)
            Case ";"
                ' new comment
                Dim newComment = doc.CreateElement("comment")
                newComment.InnerText = dataLine.Substring(1)
                GetLastSection().AppendChild(newComment)
            Case Else
                ' split the string on the "=" sign, if present
                Dim key As String
                Dim value As String

                If (dataLine.IndexOf("=", StringComparison.Ordinal) > 0) Then
                    Dim parts = dataLine.Split("=")
                    key = parts(0).Trim()
                    value = parts(1).Trim()
                Else
                    key = dataLine
                    value = ""
                End If

                Dim newItem = doc.CreateElement("item")
                Dim keyAttribute = doc.CreateAttribute("key")
                keyAttribute.Value = SetNameCase(key)
                newItem.Attributes.SetNamedItem(keyAttribute)

                Dim valueAttribute = doc.CreateAttribute("value")
                valueAttribute.Value = value
                newItem.Attributes.SetNamedItem(valueAttribute)

                GetLastSection().AppendChild(newItem)
        End Select

    End Sub

    Public Property OutputFilename As String
        Get
            If Not Initialized Then Throw New IniFileReaderNotInitializedException
            Return m_SaveFilename
        End Get
        Set
            Dim fi As FileInfo
            If Not Initialized Then Throw New IniFileReaderNotInitializedException
            fi = New FileInfo(Value)
            If Not fi.Directory.Exists Then
                'MessageBox.Show("Invalid path.")
            Else
                m_SaveFilename = Value
            End If
        End Set
    End Property

    Public Sub Save()
        If Not Initialized Then Throw New IniFileReaderNotInitializedException
        If Not OutputFilename Is Nothing AndAlso Not m_XmlDoc Is Nothing Then
            Dim fi = New FileInfo(OutputFilename)
            If Not fi.Directory.Exists Then
                'MessageBox.Show("Invalid path.")
                Return
            End If
            If fi.Exists Then
                fi.Delete()
                m_XmlDoc.Save(OutputFilename)
            Else
                m_XmlDoc.Save(OutputFilename)
            End If
        End If
    End Sub

    'Public Function AsIniFile() As String
    '    If Not Initialized Then Throw New IniFileReaderNotInitializedException()
    '    Try
    '        Dim xsl As XslTransform = New XslTransform()
    '        xsl.Load("c:\\XMLToIni.xslt")
    '        Dim sb As StringBuilder = New StringBuilder()
    '        Dim sw As StringWriter = New StringWriter(sb)
    '        xsl.Transform(m_XmlDoc, Nothing, sw, Nothing)
    '        sw.Close()
    '        Return sb.ToString
    '    Catch e As Exception
    '        MessageBox.Show(e.Message)
    '        Return Nothing
    '    End Try
    'End Function

    Public ReadOnly Property XmlDoc As XmlDocument
        Get
            If Not Initialized Then Throw New IniFileReaderNotInitializedException
            Return m_XmlDoc
        End Get
    End Property

    Public ReadOnly Property XML As String
        Get
            If Not Initialized Then Throw New IniFileReaderNotInitializedException
            Dim sb = New StringBuilder
            Dim sw = New StringWriter(sb)
            Dim xw = New XmlTextWriter(sw) With {
                .Indentation = 3,
                .Formatting = Formatting.Indented
            }
            m_XmlDoc.WriteContentTo(xw)
            xw.Close()
            sw.Close()
            Return sb.ToString()
        End Get
    End Property
End Class