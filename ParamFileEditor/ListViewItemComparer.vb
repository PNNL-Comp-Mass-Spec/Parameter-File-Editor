Class ListViewItemComparer
	Implements IComparer

	' Implements the manual sorting of items by columns.
	Dim m_SortOrderAsc As Boolean = True

	Private col As Integer

	Public Sub New()
		col = 0
	End Sub

	Public Sub New(ByVal column As Integer)
		col = column
	End Sub

	Public Sub New(ByVal column As Integer, ByVal SortOrderAsc As Boolean)
		col = column
		m_SortOrderAsc = SortOrderAsc
	End Sub

	Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
	 Implements IComparer.Compare

		Dim TempResult As Integer

		TempResult = [String].Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text)
		If m_SortOrderAsc Then
			Return TempResult
		Else
			Return -TempResult
		End If

	End Function

	Public Property SortOrder() As Boolean
		Get
			Return m_SortOrderAsc
		End Get
		Set(ByVal Value As Boolean)
			m_SortOrderAsc = False
		End Set
	End Property

End Class

