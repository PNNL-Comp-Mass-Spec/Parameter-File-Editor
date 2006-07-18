Public Class clsSimpleComparer
    Implements IComparer

        Private _propertyToSort As String
        Private _sortOrder As SortOrder

        Public Sub New(ByVal propertyToSort As String)
            Me.new(propertyToSort, System.Windows.Forms.SortOrder.Ascending)
        End Sub

        Public Sub New(ByVal propertyToSort As String, ByVal sortOrder As SortOrder)
            MyBase.new()
            _propertyToSort = propertyToSort
            _sortOrder = sortOrder
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
            Implements System.Collections.IComparer.Compare
            Dim prop As Reflection.PropertyInfo = x.GetType.GetProperty(Me.PropertyToSort)

            If Me.SortOrder = SortOrder.None OrElse prop.GetValue(x, Nothing) = _
                prop.GetValue(y, Nothing) Then
                Return 0
            Else
                If prop.GetValue(x, Nothing) > prop.GetValue(y, Nothing) Then
                    If Me.SortOrder = System.Windows.Forms.SortOrder.Ascending Then
                        Return 1
                    Else
                        Return -1
                    End If
                Else
                    If Me.SortOrder = System.Windows.Forms.SortOrder.Ascending Then
                        Return -1
                    Else
                        Return 1
                    End If
                End If
            End If
        End Function

        Public Property SortOrder() As SortOrder
            Get
                Return _sortOrder
            End Get
            Set(ByVal Value As SortOrder)
                _sortOrder = Value
            End Set
        End Property

        Public Property PropertyToSort() As String
            Get
                Return _propertyToSort
            End Get
            Set(ByVal Value As String)
                _propertyToSort = Value
            End Set
        End Property
    End Class
