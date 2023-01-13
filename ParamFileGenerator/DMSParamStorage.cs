using System.Collections.Generic;

namespace ParamFileGenerator
{
    public class DMSParamStorage : List<ParamsEntry>
    {
        public enum ParamTypes
        {
            BasicParam,
            AdvancedParam,
            TermDynamicModification,
            DynamicModification,
            StaticModification,
            IsotopicModification
        }

        public void Add(
            string ParamSpecifier,
            string ParamValue,
            ParamTypes ParamType)
        {
            var e = new ParamsEntry(ParamSpecifier, ParamValue, ParamType);
            Add(e);
        }

        public ParamsEntry this[string ParamName, ParamTypes ParamType]
        {
            get
            {
                int index = IndexOf(ParamName, ParamType);
                if (index >= 0)
                {
                    return this[IndexOf(ParamName, ParamType)];
                }
                else
                {
                    return null;
                }
            }
            set
            {
            }
        }

        public int IndexOf(string paramName, ParamTypes paramType)
        {
            var counter = default(int);
            foreach (ParamsEntry e in this)
            {
                if (e.Type == paramType && (e.Specifier ?? "") == (paramName ?? ""))
                {
                    return counter;
                }
                else
                {
                    counter += 1;
                }
            }
            return -1;
        }
    }
}
