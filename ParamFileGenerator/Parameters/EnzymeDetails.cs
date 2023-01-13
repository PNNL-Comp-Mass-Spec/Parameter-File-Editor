﻿using System;
using System.Text;

namespace ParamFileGenerator
{
    public class EnzymeDetails
    {
        private readonly string m_EnzymeString;

        private int m_Number;             // Enzyme ID Number
        private string m_Name;            // Descriptive Name
        private int m_Offset;             // Cut position --> 0 = N-terminal, 1 = C-Terminal
        private string m_CleavePoints;    // Amino Acids at which to cleave
        private string m_NoCleavePoints;  // Amino Acids to skip cleavage

        public EnzymeDetails(string EnzymeString)
        {
            m_EnzymeString = EnzymeString;
            ParseEnzymeString(m_EnzymeString);
        }

        public EnzymeDetails()
        {
        }

        private void ParseEnzymeString(string enzStr)
        {
            string[] s = null;
            int placeCounter = 0;
            string prevChar = "";
            string tmpString = "";

            for (int counter = 0; counter < enzStr.Length; counter++)
            {
                string currChar = enzStr.Substring(counter, 1);
                if (currChar == " " && prevChar != " " || counter == enzStr.Length + 1)
                {
                    Array.Resize(ref s, placeCounter + 1);
                    s[placeCounter] = tmpString;
                    placeCounter += 1;
                    tmpString = "";
                }
                else if (currChar != " ")
                {
                    tmpString += currChar;
                }

                prevChar = currChar;
            }

            m_Number = Params.SafeCastInt(s[0]);
            m_Name = s[1];
            m_Offset = Params.SafeCastInt(s[2]);
            m_CleavePoints = s[3];
            m_NoCleavePoints = s[4];
        }

        public int EnzymeID
        {
            get => m_Number;
            set => m_Number = value;
        }

        public string EnzymeName
        {
            get => m_Name;
            set => m_Name = value;
        }

        public int EnzymeCleaveOffset
        {
            get => m_Offset;
            set => m_Offset = value;
        }

        public string EnzymeCleavePoints
        {
            get => m_CleavePoints;
            set => m_CleavePoints = value;
        }

        public string EnzymeNoCleavePoints
        {
            get => m_NoCleavePoints;
            set => m_NoCleavePoints = value;
        }

        public string ReturnEnzymeString()
        {
            string s;

            s = EnzymeID.ToString() + ".";
            s = s.PadRight(4, Convert.ToChar(" ")) + EnzymeName;
            s = s.PadRight(30, Convert.ToChar(" ")) + EnzymeCleaveOffset.ToString();
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