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

        public void Add(string paramSpecifier, string paramValue, ParamTypes paramType)
        {
            Add(new ParamsEntry(paramSpecifier, paramValue, paramType));
        }

        public ParamsEntry this[string paramName, ParamTypes paramType]
        {
            get
            {
                var index = IndexOf(paramName, paramType);
                if (index >= 0)
                {
                    return this[index];
                }

                return null;
            }
            set
            {
            }
        }

        public int IndexOf(string paramName, ParamTypes paramType)
        {
            if (string.IsNullOrWhiteSpace(paramName))
            {
                return -1;
            }

            for (var i = 0; i < Count; i++)
            {
                if (this[i].TypeSpecifierEquals(paramType, paramName))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
