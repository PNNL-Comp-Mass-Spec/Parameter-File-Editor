namespace ParamFileGenerator.Parameters
{
    public class IonSeries
    {
        public IonSeries(string ionSeriesString)
        {
            ParseIonSeries(ionSeriesString);
        }

        public IonSeries()
        {
            Initialized = true;
        }

        private void ParseIonSeries(string ionString)
        {
            var ionSeries = ionString.Split(' ');

            Use_a_Ions = Params.SafeCastInt(ionSeries[0]) != 0;
            Use_b_Ions = Params.SafeCastInt(ionSeries[1]) != 0;
            Use_y_Ions = Params.SafeCastInt(ionSeries[2]) != 0;
            a_Ion_Weighting = float.Parse(ionSeries[3]);
            b_Ion_Weighting = float.Parse(ionSeries[4]);
            c_Ion_Weighting = float.Parse(ionSeries[5]);
            d_Ion_Weighting = float.Parse(ionSeries[6]);
            v_Ion_Weighting = float.Parse(ionSeries[7]);
            w_Ion_Weighting = float.Parse(ionSeries[8]);
            x_Ion_Weighting = float.Parse(ionSeries[9]);
            y_Ion_Weighting = float.Parse(ionSeries[10]);
            z_Ion_Weighting = float.Parse(ionSeries[11]);
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
