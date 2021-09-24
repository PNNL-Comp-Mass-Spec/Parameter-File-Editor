Public Class clsSimpleComparer
    Implements IComparer

    Public Sub New(propertyToSort As String)
        Me.New(propertyToSort, System.Windows.Forms.SortOrder.Ascending)
    End Sub

    Public Sub New(propertyToSort As String, sortOrder As SortOrder)
        MyBase.New()
        Me.PropertyToSort = propertyToSort
        Me.SortOrder = sortOrder
    End Sub

    Public Function Compare(x As Object, y As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim prop As Reflection.PropertyInfo = x.GetType.GetProperty(Me.PropertyToSort)

        Dim xValue = prop.GetValue(x, Nothing)
        Dim yValue = prop.GetValue(y, Nothing)

        If SortOrder = SortOrder.None OrElse
           xValue Is yValue OrElse
           xValue Is Nothing AndAlso yValue Is Nothing Then
            Return 0
        Else
            Dim xGreater As Boolean
            If xValue IsNot Nothing AndAlso yValue Is Nothing Then
                xGreater = True
            ElseIf xValue Is Nothing AndAlso yValue IsNot Nothing Then
                xGreater = False
            Else
                xGreater = xValue.ToString() > yValue.ToString()
            End If

            If xGreater Then
                If Me.SortOrder = SortOrder.Ascending Then
                    Return 1
                Else
                    Return -1
                End If
            Else
                If Me.SortOrder = SortOrder.Ascending Then
                    Return -1
                Else
                    Return 1
                End If
            End If
        End If
    End Function

    Public Property SortOrder As SortOrder

    Public Property PropertyToSort As String
End Class
