using System;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

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
            tmpSplit = ionString.Split(Convert.ToChar(" "));
            m_use_a_Ions = Conversions.ToInteger(tmpSplit[0]);
            m_use_b_Ions = Conversions.ToInteger(tmpSplit[1]);
            m_use_y_Ions = Conversions.ToInteger(tmpSplit[2]);
            m_aWeight = Conversions.ToSingle(tmpSplit[3]);
            m_bWeight = Conversions.ToSingle(tmpSplit[4]);
            m_cWeight = Conversions.ToSingle(tmpSplit[5]);
            m_dWeight = Conversions.ToSingle(tmpSplit[6]);
            m_vWeight = Conversions.ToSingle(tmpSplit[7]);
            m_wWeight = Conversions.ToSingle(tmpSplit[8]);
            m_xWeight = Conversions.ToSingle(tmpSplit[9]);
            m_yWeight = Conversions.ToSingle(tmpSplit[10]);
            m_zWeight = Conversions.ToSingle(tmpSplit[11]);
        }

        public bool Initialized { get; set; }

        public int Use_a_Ions
        {
            get
            {
                return Math.Abs(m_use_a_Ions);
            }
            set
            {
                m_use_a_Ions = Math.Abs(value);
            }
        }
        public int Use_b_Ions
        {
            get
            {
                return Math.Abs(m_use_b_Ions);
            }
            set
            {
                m_use_b_Ions = Math.Abs(value);
            }
        }
        public int Use_y_Ions
        {
            get
            {
                return Math.Abs(m_use_y_Ions);
            }
            set
            {
                m_use_y_Ions = Math.Abs(value);
            }
        }
        public float a_Ion_Weighting
        {
            get
            {
                return m_aWeight;
            }
            set
            {
                m_aWeight = value;
            }
        }
        public float b_Ion_Weighting
        {
            get
            {
                return m_bWeight;
            }
            set
            {
                m_bWeight = value;
            }
        }
        public float c_Ion_Weighting
        {
            get
            {
                return m_cWeight;
            }
            set
            {
                m_cWeight = value;
            }
        }
        public float d_Ion_Weighting
        {
            get
            {
                return m_dWeight;
            }
            set
            {
                m_dWeight = value;
            }
        }
        public float v_Ion_Weighting
        {
            get
            {
                return m_vWeight;
            }
            set
            {
                m_vWeight = value;
            }
        }
        public float w_Ion_Weighting
        {
            get
            {
                return m_wWeight;
            }
            set
            {
                m_wWeight = value;
            }
        }
        public float x_Ion_Weighting
        {
            get
            {
                return m_xWeight;
            }
            set
            {
                m_xWeight = value;
            }
        }
        public float y_Ion_Weighting
        {
            get
            {
                return m_yWeight;
            }
            set
            {
                m_yWeight = value;
            }
        }
        public float z_Ion_Weighting
        {
            get
            {
                return m_zWeight;
            }
            set
            {
                m_zWeight = value;
            }
        }

        public string ReturnIonString()
        {
            string s = AssembleIonString();
            return s;
        }

        private string AssembleIonString()
        {
            string s = Use_a_Ions.ToString() + " " + Use_b_Ions.ToString() + " " + Use_y_Ions.ToString() + " " + Strings.Format(a_Ion_Weighting, "0.0").ToString() + " " + Strings.Format(b_Ion_Weighting, "0.0").ToString() + " " + Strings.Format(c_Ion_Weighting, "0.0").ToString() + " " + Strings.Format(d_Ion_Weighting, "0.0").ToString() + " " + Strings.Format(v_Ion_Weighting, "0.0").ToString() + " " + Strings.Format(w_Ion_Weighting, "0.0").ToString() + " " + Strings.Format(x_Ion_Weighting, "0.0").ToString() + " " + Strings.Format(y_Ion_Weighting, "0.0").ToString() + " " + Strings.Format(z_Ion_Weighting, "0.0").ToString() + " ";

            return s;
        }

    }
}
