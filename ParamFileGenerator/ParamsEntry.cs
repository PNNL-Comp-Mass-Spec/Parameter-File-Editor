namespace ParamFileGenerator
{
    public class ParamsEntry
    {
        public ParamsEntry(string paramSpecifier, string paramValue, ParamTypes paramType)
        {
            Specifier = paramSpecifier ?? string.Empty;
            Value = paramValue ?? string.Empty;
            Type = paramType;
        }

        public string Specifier { get; }

        public string Value { get; }

        public ParamTypes Type { get; }

        public bool TypeSpecifierEquals(ParamsEntry other)
        {
            return TypeSpecifierEquals(other.Type, other.Specifier);
        }

        public bool TypeSpecifierEquals(ParamTypes type, string specifier)
        {
            return Type == type && Specifier == specifier;
        }
    }
}
