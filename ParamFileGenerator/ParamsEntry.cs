namespace ParamFileGenerator;

public class ParamsEntry
{
    protected string m_Spec;
    protected string m_Value;
    protected DMSParamStorage.ParamTypes m_Type;

    public ParamsEntry(string ParamSpecifier, string ParamValue, DMSParamStorage.ParamTypes ParamType)
    {
        m_Spec = ParamSpecifier;
        m_Value = ParamValue;
        m_Type = ParamType;
    }

    public string Specifier => m_Spec;

    public string Value => m_Value;

    public DMSParamStorage.ParamTypes Type => m_Type;
}
