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

    Public Sub Add( _
        ByVal ParamSpecifier As String, _
        ByVal ParamValue As String, _
        ByVal ParamType As ParamTypes)

        Dim e As New ParamsEntry(ParamSpecifier, ParamValue, ParamType)
        Me.List.Add(DirectCast(e, ParamsEntry))
    End Sub

    Public Sub Remove(ByVal index As Integer)
        Me.List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal index As Integer) As ParamsEntry
        Get
            Return DirectCast(Me.List(index), ParamsEntry)
        End Get
        Set(ByVal Value As ParamsEntry)
            Me.List(index) = Value
        End Set
    End Property

    Default Public Property Item(ByVal ParamName As String, ByVal ParamType As ParamTypes) As ParamsEntry
        Get
            Dim index As Integer = Me.IndexOf(ParamName, ParamType)
            If index >= 0 Then
                Return Me.Item(Me.IndexOf(ParamName, ParamType))
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal Value As ParamsEntry)

        End Set
    End Property


    Public Function IndexOf(ByVal paramName As String, ByVal paramType As ParamTypes) As Integer
        Dim e As ParamsEntry
        Dim counter As Integer
        For Each e In Me.List
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

        Public Sub New(ByVal ParamSpecifier As String, ByVal ParamValue As String, ByVal ParamType As ParamTypes)
            Me.m_Spec = ParamSpecifier
            Me.m_Value = ParamValue
            Me.m_Type = ParamType
        End Sub

        Public ReadOnly Property Specifier() As String
            Get
                Return Me.m_Spec
            End Get
        End Property
        Public ReadOnly Property Value() As String
            Get
                Return Me.m_Value
            End Get
        End Property
        Public ReadOnly Property Type() As ParamTypes
            Get
                Return Me.m_Type
            End Get
        End Property

    End Class
End Class
