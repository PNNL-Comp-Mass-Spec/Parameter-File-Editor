namespace ParamFileGenerator
{
    public class IonSeries
    {
        public IonSeries(string ionSeriesString)
        {
            ParseISS(ionSeriesString);
        }

        public IonSeries()
        {
            Initialized = true;
        }

        private void ParseISS(string ionString)
        {
            var tmpSplit = ionString.Split(' ');
            Use_a_Ions = Params.SafeCastInt(tmpSplit[0]) != 0;
            Use_b_Ions = Params.SafeCastInt(tmpSplit[1]) != 0;
            Use_y_Ions = Params.SafeCastInt(tmpSplit[2]) != 0;
            a_Ion_Weighting = float.Parse(tmpSplit[3]);
            b_Ion_Weighting = float.Parse(tmpSplit[4]);
            c_Ion_Weighting = float.Parse(tmpSplit[5]);
            d_Ion_Weighting = float.Parse(tmpSplit[6]);
            v_Ion_Weighting = float.Parse(tmpSplit[7]);
            w_Ion_Weighting = float.Parse(tmpSplit[8]);
            x_Ion_Weighting = float.Parse(tmpSplit[9]);
            y_Ion_Weighting = float.Parse(tmpSplit[10]);
            z_Ion_Weighting = float.Parse(tmpSplit[11]);
        }

        public bool Initialized { get; set; }

        public bool Use_a_Ions { get; set; }

        public bool Use_b_Ions { get; set; }

        public bool Use_y_Ions { get; set; }

        public float a_Ion_Weighting { get; set; }

        public float b_Ion_Weighting { get; set; }

        public float c_Ion_Weighting { get; set; }

        public float d_Ion_Weighting { get; set; }

        public float v_Ion_Weighting { get; set; }

        public float w_Ion_Weighting { get; set; }

        public float x_Ion_Weighting { get; set; }

        public float y_Ion_Weighting { get; set; }

        public float z_Ion_Weighting { get; set; }

        public string ReturnIonString()
        {
            var s = AssembleIonString();
            return s;
        }

        private string AssembleIonString()
        {
            // bool.GetHashCode() returns '0' for false, '1' for true
            return $"{Use_a_Ions.GetHashCode()} {Use_b_Ions.GetHashCode()} {Use_y_Ions.GetHashCode()} " +
                   $"{a_Ion_Weighting:0.0} {b_Ion_Weighting:0.0} {c_Ion_Weighting:0.0} " +
                   $"{d_Ion_Weighting:0.0} {v_Ion_Weighting:0.0} {w_Ion_Weighting:0.0} " +
                   $"{x_Ion_Weighting:0.0} {y_Ion_Weighting:0.0} {z_Ion_Weighting:0.0} ";
        }
    }
}
