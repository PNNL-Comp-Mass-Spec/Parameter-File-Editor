Public Class IonSeries
    Public m_origIonSeriesString As String

    Private m_use_a_Ions As Integer
    Private m_use_b_Ions As Integer
    Private m_use_y_Ions As Integer
    Private m_aWeight As Single
    Private m_bWeight As Single
    Private m_cWeight As Single
    Private m_dWeight As Single
    Private m_vWeight As Single
    Private m_wWeight As Single
    Private m_xWeight As Single
    Private m_yWeight As Single
    Private m_zWeight As Single

    Public Sub New(IonSeriesString As String)
        m_origIonSeriesString = IonSeriesString
        Call ParseISS(m_origIonSeriesString)
    End Sub

    Public Sub New()
        m_origIonSeriesString = Nothing
        Initialized = True
    End Sub

    Public Sub RevertToOriginalIonString()
        If m_origIonSeriesString <> "" Then
            Call ParseISS(m_origIonSeriesString)
        End If
    End Sub

    Private Sub ParseISS(ionString As String)
        Dim tmpSplit() As String
        tmpSplit = ionString.Split(Convert.ToChar(" "))
        m_use_a_Ions = CInt(tmpSplit(0))
        m_use_b_Ions = CInt(tmpSplit(1))
        m_use_y_Ions = CInt(tmpSplit(2))
        m_aWeight = CSng(tmpSplit(3))
        m_bWeight = CSng(tmpSplit(4))
        m_cWeight = CSng(tmpSplit(5))
        m_dWeight = CSng(tmpSplit(6))
        m_vWeight = CSng(tmpSplit(7))
        m_wWeight = CSng(tmpSplit(8))
        m_xWeight = CSng(tmpSplit(9))
        m_yWeight = CSng(tmpSplit(10))
        m_zWeight = CSng(tmpSplit(11))
    End Sub

    Public Property Initialized As Boolean

    Public Property Use_a_Ions As Integer
        Get
            Return Math.Abs(m_use_a_Ions)
        End Get
        Set
            m_use_a_Ions = Math.Abs(Value)
        End Set
    End Property
    Public Property Use_b_Ions As Integer
        Get
            Return Math.Abs(m_use_b_Ions)
        End Get
        Set
            m_use_b_Ions = Math.Abs(Value)
        End Set
    End Property
    Public Property Use_y_Ions As Integer
        Get
            Return Math.Abs(m_use_y_Ions)
        End Get
        Set
            m_use_y_Ions = Math.Abs(Value)
        End Set
    End Property
    Public Property a_Ion_Weighting As Single
        Get
            Return m_aWeight
        End Get
        Set
            m_aWeight = Value
        End Set
    End Property
    Public Property b_Ion_Weighting As Single
        Get
            Return m_bWeight
        End Get
        Set
            m_bWeight = Value
        End Set
    End Property
    Public Property c_Ion_Weighting As Single
        Get
            Return m_cWeight
        End Get
        Set
            m_cWeight = Value
        End Set
    End Property
    Public Property d_Ion_Weighting As Single
        Get
            Return m_dWeight
        End Get
        Set
            m_dWeight = Value
        End Set
    End Property
    Public Property v_Ion_Weighting As Single
        Get
            Return m_vWeight
        End Get
        Set
            m_vWeight = Value
        End Set
    End Property
    Public Property w_Ion_Weighting As Single
        Get
            Return m_wWeight
        End Get
        Set
            m_wWeight = Value
        End Set
    End Property
    Public Property x_Ion_Weighting As Single
        Get
            Return m_xWeight
        End Get
        Set
            m_xWeight = Value
        End Set
    End Property
    Public Property y_Ion_Weighting As Single
        Get
            Return m_yWeight
        End Get
        Set
            m_yWeight = Value
        End Set
    End Property
    Public Property z_Ion_Weighting As Single
        Get
            Return m_zWeight
        End Get
        Set
            m_zWeight = Value
        End Set
    End Property

    Public Function ReturnIonString() As String
        Dim s As String = AssembleIonString()
        Return s
    End Function

    Private Function AssembleIonString() As String
        Dim s = Use_a_Ions.ToString & " " & Use_b_Ions.ToString & " " & Use_y_Ions.ToString & " " &
                Format(a_Ion_Weighting, "0.0").ToString & " " & Format(b_Ion_Weighting, "0.0").ToString & " " &
                Format(c_Ion_Weighting, "0.0").ToString & " " & Format(d_Ion_Weighting, "0.0").ToString & " " &
                Format(v_Ion_Weighting, "0.0").ToString & " " & Format(w_Ion_Weighting, "0.0").ToString & " " &
                Format(x_Ion_Weighting, "0.0").ToString & " " &
                Format(y_Ion_Weighting, "0.0").ToString & " " & Format(z_Ion_Weighting, "0.0").ToString & " "

        Return s
    End Function

End Class
