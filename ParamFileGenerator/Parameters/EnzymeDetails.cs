using System;
using System.Linq;
using System.Text;

namespace ParamFileGenerator.Parameters
{
    public class EnzymeDetails
    {
        public EnzymeDetails(string enzymeString)
        {
            ParseEnzymeString(enzymeString);
        }

        public EnzymeDetails()
        {
        }

        private void ParseEnzymeString(string enzStr)
        {
            var parts = enzStr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (parts.Count < 5)
            {
                parts.AddRange(Enumerable.Range(1, 5 - parts.Count).Select(_ => ""));
            }

            EnzymeID = Params.SafeCastInt(parts[0]);
            EnzymeName = parts[1];
            EnzymeCleaveOffset = Params.SafeCastInt(parts[2]);
            EnzymeCleavePoints = parts[3];
            EnzymeNoCleavePoints = parts[4];
        }

        public int EnzymeID { get; set; }

        public string EnzymeName { get; set; }

        public int EnzymeCleaveOffset { get; set; }

        public string EnzymeCleavePoints { get; set; }

        public string EnzymeNoCleavePoints { get; set; }

        public string ReturnEnzymeString()
        {
            var s = EnzymeID + ".";
            s = s.PadRight(4, Convert.ToChar(" ")) + EnzymeName;
            s = s.PadRight(30, Convert.ToChar(" ")) + EnzymeCleaveOffset;
            s = s.PadRight(35, Convert.ToChar(" ")) + EnzymeCleavePoints;
            s = s.PadRight(48, Convert.ToChar(" ")) + EnzymeNoCleavePoints;

            return s;
        }

        public string ReturnBW32EnzymeInfoString(int cleavagePosition)
        {
            var sb = new StringBuilder();

            sb.Append(EnzymeName);
            sb.Append("(");
            sb.Append(EnzymeCleavePoints);

            if (EnzymeNoCleavePoints.Length > 0 && EnzymeNoCleavePoints != "-")
            {
                sb.Append("/");
                sb.Append(EnzymeNoCleavePoints);
            }

            sb.Append(")");
            sb.Append(" ");
            sb.Append(cleavagePosition.ToString());
            sb.Append(" ");
            sb.Append(EnzymeCleaveOffset.ToString());
            sb.Append(" ");
            sb.Append(EnzymeCleavePoints);
            sb.Append(" ");
            sb.Append(EnzymeNoCleavePoints);

            return sb.ToString();
        }
    }
}
