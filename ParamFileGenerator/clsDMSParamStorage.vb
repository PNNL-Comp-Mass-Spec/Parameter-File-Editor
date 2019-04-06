Public Class clsDMSParamStorage
    Inherits CollectionBase

    Public Enum ParamTypes
        BasicParam
        AdvancedParam
        TermDynamicModification
        DynamicModification
        StaticModification
        IsotopicModification
    End Enum

    Public Sub Add(
        ParamSpecifier As String,
        ParamValue As String,
        ParamType As ParamTypes)

        Dim e As New ParamsEntry(ParamSpecifier, ParamValue, ParamType)
        List.Add(e)
    End Sub

    Public Sub Remove(index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(index As Integer) As ParamsEntry
        Get
            Return DirectCast(List(index), ParamsEntry)
        End Get
        Set
            List(index) = Value
        End Set
    End Property

    Default Public Property Item(ParamName As String, ParamType As ParamTypes) As ParamsEntry
        Get
            Dim index As Integer = IndexOf(ParamName, ParamType)
            If index >= 0 Then
                Return Item(IndexOf(ParamName, ParamType))
            Else
                Return Nothing
            End If
        End Get
        Set

        End Set
    End Property


    Public Function IndexOf(paramName As String, paramType As ParamTypes) As Integer
        Dim e As ParamsEntry
        Dim counter As Integer
        For Each e In List
            If e.Type = paramType And e.Specifier = paramName Then
                Return counter
            Else
                counter += 1
            End If
        Next
        Return -1
    End Function


    Public Class ParamsEntry
        Protected m_Spec As String
        Protected m_Value As String
        Protected m_Type As ParamTypes

        Public Sub New(ParamSpecifier As String, ParamValue As String, ParamType As ParamTypes)
            m_Spec = ParamSpecifier
            m_Value = ParamValue
            m_Type = ParamType
        End Sub

        Public ReadOnly Property Specifier As String
            Get
                Return m_Spec
            End Get
        End Property
        Public ReadOnly Property Value As String
            Get
                Return m_Value
            End Get
        End Property
        Public ReadOnly Property Type As ParamTypes
            Get
                Return m_Type
            End Get
        End Property

    End Class
End Class
