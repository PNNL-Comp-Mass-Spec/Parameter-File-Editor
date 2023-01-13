using System.Collections;

namespace ParamFileGenerator
{
    public class DMSParamStorage : CollectionBase
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
            List.Add(e);
        }

        public void Remove(int index)
        {
            List.RemoveAt(index);
        }

        public ParamsEntry this[int index]
        {
            get => (ParamsEntry)List[index];
            set => List[index] = value;
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
            foreach (ParamsEntry e in List)
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
