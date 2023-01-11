using System;

namespace ParamFileGenerator
{
    public class IonSeries
    {
        public string m_origIonSeriesString;

        private int m_use_a_Ions;
        private int m_use_b_Ions;
        private int m_use_y_Ions;
        private float m_aWeight;
        private float m_bWeight;
        private float m_cWeight;
        private float m_dWeight;
        private float m_vWeight;
        private float m_wWeight;
        private float m_xWeight;
        private float m_yWeight;
        private float m_zWeight;

        public IonSeries(string IonSeriesString)
        {
            m_origIonSeriesString = IonSeriesString;
            ParseISS(m_origIonSeriesString);
        }

        public IonSeries()
        {
            m_origIonSeriesString = null;
            Initialized = true;
        }

        public void RevertToOriginalIonString()
        {
            if (!string.IsNullOrEmpty(m_origIonSeriesString))
            {
                ParseISS(m_origIonSeriesString);
            }
        }

        private void ParseISS(string ionString)
        {
            string[] tmpSplit;
            tmpSplit = ionString.Split(' ');
            m_use_a_Ions = Params.SafeCastInt(tmpSplit[0]);
            m_use_b_Ions = Params.SafeCastInt(tmpSplit[1]);
            m_use_y_Ions = Params.SafeCastInt(tmpSplit[2]);
            m_aWeight = float.Parse(tmpSplit[3]);
            m_bWeight = float.Parse(tmpSplit[4]);
            m_cWeight = float.Parse(tmpSplit[5]);
            m_dWeight = float.Parse(tmpSplit[6]);
            m_vWeight = float.Parse(tmpSplit[7]);
            m_wWeight = float.Parse(tmpSplit[8]);
            m_xWeight = float.Parse(tmpSplit[9]);
            m_yWeight = float.Parse(tmpSplit[10]);
            m_zWeight = float.Parse(tmpSplit[11]);
        }

        public bool Initialized { get; set; }

        public int Use_a_Ions
        {
            get => Math.Abs(m_use_a_Ions);
            set => m_use_a_Ions = Math.Abs(value);
        }

        public int Use_b_Ions
        {
            get => Math.Abs(m_use_b_Ions);
            set => m_use_b_Ions = Math.Abs(value);
        }

        public int Use_y_Ions
        {
            get => Math.Abs(m_use_y_Ions);
            set => m_use_y_Ions = Math.Abs(value);
        }

        public float a_Ion_Weighting
        {
            get => m_aWeight;
            set => m_aWeight = value;
        }

        public float b_Ion_Weighting
        {
            get => m_bWeight;
            set => m_bWeight = value;
        }

        public float c_Ion_Weighting
        {
            get => m_cWeight;
            set => m_cWeight = value;
        }

        public float d_Ion_Weighting
        {
            get => m_dWeight;
            set => m_dWeight = value;
        }

        public float v_Ion_Weighting
        {
            get => m_vWeight;
            set => m_vWeight = value;
        }

        public float w_Ion_Weighting
        {
            get => m_wWeight;
            set => m_wWeight = value;
        }

        public float x_Ion_Weighting
        {
            get => m_xWeight;
            set => m_xWeight = value;
        }

        public float y_Ion_Weighting
        {
            get => m_yWeight;
            set => m_yWeight = value;
        }

        public float z_Ion_Weighting
        {
            get => m_zWeight;
            set => m_zWeight = value;
        }

        public string ReturnIonString()
        {
            string s = AssembleIonString();
            return s;
        }

        private string AssembleIonString()
        {
            var s = Use_a_Ions.ToString() + " " + Use_b_Ions.ToString() + " " + Use_y_Ions.ToString() + " " +
                    a_Ion_Weighting.ToString("0.0") + " " + b_Ion_Weighting.ToString("0.0") + " " +
                    c_Ion_Weighting.ToString("0.0") + " " + d_Ion_Weighting.ToString("0.0") + " " +
                    v_Ion_Weighting.ToString("0.0") + " " + w_Ion_Weighting.ToString("0.0") + " " +
                    x_Ion_Weighting.ToString("0.0") + " " + y_Ion_Weighting.ToString("0.0") + " " +
                    z_Ion_Weighting.ToString("0.0") + " ";

            return s;
        }
    }
}
