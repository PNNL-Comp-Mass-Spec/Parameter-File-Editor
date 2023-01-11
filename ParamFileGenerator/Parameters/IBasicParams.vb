Public Interface IBasicParams
    Enum MassTypeList As Integer
        Average = 0
        Monoisotopic = 1
    End Enum

    ReadOnly Property FileType As Params.ParamFileTypes
    Property DMS_ID As Integer
    Property FileName As String
    Property Description As String
    Property SelectedEnzymeDetails As EnzymeDetails
    Property SelectedEnzymeIndex As Integer
    Property SelectedEnzymeCleavagePosition As Integer
    Property MaximumNumberMissedCleavages As Integer
    Property ParentMassType As MassTypeList
    Property FragmentMassType As MassTypeList
    Property DynamicMods As DynamicMods
    Property TermDynamicMods As TermDynamicMods
    Property StaticModificationsList As StaticMods
    Property IsotopicModificationsList As IsoMods
    Property PartialSequenceToMatch As String
    Property EnzymeList As EnzymeCollection
    Function RetrieveEnzymeDetails(index As Integer) As EnzymeDetails
End Interface
