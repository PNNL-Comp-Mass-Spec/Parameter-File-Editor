Friend Class clsDMSParamStorage
    Inherits CollectionBase

    Friend Enum ParamTypes
        BasicParam
        AdvancedParam
        DynamicModification
        StaticModification
    End Enum

    Friend Sub Add( _
        ByVal ParamSpecifier As String, _
        ByVal ParamValue As String, _
        ByVal ParamType As ParamTypes)

        Dim e As New ParamsEntry(ParamSpecifier, ParamValue, ParamType)
        Me.List.Add(DirectCast(e, ParamsEntry))
    End Sub

    Friend Sub Remove(ByVal index As Integer)
        Me.List.RemoveAt(index)
    End Sub

    Default Friend Property Item(ByVal index As Integer) As ParamsEntry
        Get
            Return Me.List(index)
        End Get
        Set(ByVal Value As ParamsEntry)
            Me.List(index) = Value
        End Set
    End Property

    Default Friend Property Item(ByVal ParamName As String, ByVal ParamType As ParamTypes) As ParamsEntry
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


    Friend Function IndexOf(ByVal paramName As String, ByVal paramType As ParamTypes) As Integer
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


    Friend Class ParamsEntry
        Protected m_Spec As String
        Protected m_Value As String
        Protected m_Type As ParamTypes

        Friend Sub New(ByVal ParamSpecifier As String, ByVal ParamValue As String, ByVal ParamType As ParamTypes)
            Me.m_Spec = ParamSpecifier
            Me.m_Value = ParamValue
            Me.m_Type = ParamType
        End Sub

        Friend ReadOnly Property Specifier() As String
            Get
                Return Me.m_Spec
            End Get
        End Property
        Friend ReadOnly Property Value() As String
            Get
                Return Me.m_Value
            End Get
        End Property
        Friend ReadOnly Property Type() As ParamTypes
            Get
                Return Me.m_Type
            End Get
        End Property

    End Class
End Class
