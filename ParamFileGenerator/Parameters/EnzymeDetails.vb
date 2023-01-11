Imports System.Text

Public Class EnzymeDetails

    Private ReadOnly m_EnzymeString As String

    Private m_Number As Integer         'Enzyme ID Number
    Private m_Name As String            'Descriptive Name
    Private m_Offset As Integer         'Cut position --> 0 = N-terminal, 1 = C-Terminal
    Private m_CleavePoints As String    'Amino Acids at which to cleave
    Private m_NoCleavePoints As String  'Amino Acids to skip cleavage

    Public Sub New(EnzymeString As String)
        m_EnzymeString = EnzymeString
        Call ParseEnzymeString(m_EnzymeString)
    End Sub

    Public Sub New()

    End Sub

    Private Sub ParseEnzymeString(enzStr As String)
        Dim s() As String = Nothing
        Dim placeCounter = 0
        Dim prevChar = ""
        Dim tmpString = ""

        For counter = 1 To Len(enzStr) + 1
            Dim currChar = Mid(enzStr, counter, 1)
            If (currChar = " " And prevChar <> " ") Or (counter = Len(enzStr) + 1) Then
                ReDim Preserve s(placeCounter)
                s(placeCounter) = tmpString
                placeCounter += 1
                tmpString = ""
            ElseIf currChar <> " " Then
                tmpString += currChar
            End If

            prevChar = currChar
        Next

        m_Number = CInt(s(0))
        m_Name = s(1)
        m_Offset = CInt(s(2))
        m_CleavePoints = s(3)
        m_NoCleavePoints = s(4)

    End Sub

    Public Property EnzymeID As Integer
        Get
            Return m_Number
        End Get
        Set
            m_Number = Value
        End Set
    End Property
    Public Property EnzymeName As String
        Get
            Return m_Name
        End Get
        Set
            m_Name = Value
        End Set
    End Property
    Public Property EnzymeCleaveOffset As Integer
        Get
            Return m_Offset
        End Get
        Set
            m_Offset = Value
        End Set
    End Property
    Public Property EnzymeCleavePoints As String
        Get
            Return m_CleavePoints
        End Get
        Set
            m_CleavePoints = Value
        End Set
    End Property
    Public Property EnzymeNoCleavePoints As String
        Get
            Return m_NoCleavePoints
        End Get
        Set
            m_NoCleavePoints = Value
        End Set
    End Property

    Public Function ReturnEnzymeString() As String
        Dim s As String

        s = EnzymeID.ToString & "."
        s = s.PadRight(4, Convert.ToChar(" ")) & EnzymeName
        s = s.PadRight(30, Convert.ToChar(" ")) & EnzymeCleaveOffset.ToString
        s = s.PadRight(35, Convert.ToChar(" ")) & EnzymeCleavePoints
        s = s.PadRight(48, Convert.ToChar(" ")) & EnzymeNoCleavePoints

        Return s

    End Function

    Public Function ReturnBW32EnzymeInfoString(cleavagePosition As Integer) As String
        Dim sb = New StringBuilder

        sb.Append(EnzymeName)
        sb.Append("(")
        sb.Append(EnzymeCleavePoints)
        If EnzymeNoCleavePoints.Length > 0 And EnzymeNoCleavePoints <> "-" Then
            sb.Append("/")
            sb.Append(EnzymeNoCleavePoints)
        End If
        sb.Append(")")
        sb.Append(" ")
        sb.Append(cleavagePosition.ToString)
        sb.Append(" ")
        sb.Append(EnzymeCleaveOffset.ToString)
        sb.Append(" ")
        sb.Append(EnzymeCleavePoints)
        sb.Append(" ")
        sb.Append(EnzymeNoCleavePoints)

        Return sb.ToString
    End Function

End Class
