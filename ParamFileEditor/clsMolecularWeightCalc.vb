Imports MwtWinDll.MolecularWeightCalculatorClass
Imports ParamFileEditor.ProgramSettings
Imports ParamFileGenerator.clsMainProcess

Public Class clsMolecularWeightCalc

    Private mwt As New MwtWinDll.MolecularWeightCalculatorClass
    Private m_Formula As String
    Private m_EmpiricalFormula As String
    Private m_MonoMass As Single
    Private m_AvgMass As Single

    Public Sub New()

    End Sub

    Public Sub CalculateMasses()
        Call CalcFormulaInfo(m_Formula)
    End Sub

    Public Sub CalculateMasses(ByVal Formula As String)
        Call CalcFormulaInfo(Formula)

    End Sub

    Public Property MonoMass() As Single
        Get
            Return m_MonoMass
        End Get
        Set(ByVal Value As Single)
            m_MonoMass = Value
        End Set
    End Property

    Public Property AverageMass() As Single
        Get
            Return m_AvgMass
        End Get
        Set(ByVal Value As Single)
            m_AvgMass = Value
        End Set
    End Property

    Public Property EmpiricalFormula() As String
        Get
            Return m_EmpiricalFormula
        End Get
        Set(ByVal Value As String)
            m_EmpiricalFormula = Value
        End Set
    End Property

    Private Sub CalcFormulaInfo(ByVal FormulaString As String)
        Dim tmpMass As Single
        With mwt
            .SetElementMode(MwtWinDll.emElementModeConstants.emIsotopicMass)
            .Compound.Formula = FormulaString
            m_MonoMass = .Compound.Mass
            .SetElementMode(MwtWinDll.emElementModeConstants.emAverageMass)
            .Compound.Formula = FormulaString
            m_AvgMass = .Compound.Mass
            m_EmpiricalFormula = .Compound.ConvertToEmpirical
        End With

    End Sub
End Class
