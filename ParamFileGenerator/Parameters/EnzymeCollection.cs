Public Class EnzymeCollection
    Inherits CollectionBase

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub add(Enzyme As EnzymeDetails)
        List.Add(Enzyme)
    End Sub

    Default Public Property Item(index As Integer) As EnzymeDetails
        Get
            Return DirectCast(List(index), EnzymeDetails)
        End Get
        Set
            List(index) = Value
        End Set
    End Property
End Class
