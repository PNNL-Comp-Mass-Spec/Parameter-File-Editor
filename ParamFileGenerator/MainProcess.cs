using System.IO;
using System.Reflection;

namespace ParamFileGenerator
{
    public class MainProcess
    {
        //private IBasicParams basicTemplate;
        //private IAdvancedParams advTemplate;

        //private string m_SettingsFileName = "ParamFileEditorSettings.xml";
        private static string m_TemplateFileName;
        private static string m_TemplateFilePath;
        //private UpdateModsTable m_modsUpdate;
        //private MainProcess m_mainProcess;
        private static Params m_BaseLineParams;
        //private const string DEF_TEMPLATE_LABEL_TEXT = "Currently Loaded Template: ";
        private const string DEF_TEMPLATE_FILENAME = "sequest_N14_NE.params";
        private static readonly string DEF_TEMPLATE_FILEPATH = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        //public static Settings mySettings;

        public static Params BaseLineParamSet => m_BaseLineParams;

        public static string TemplateFileName => Path.Combine(m_TemplateFilePath, Path.GetFileName(m_TemplateFileName));

        public MainProcess()
        {
            m_TemplateFileName = Path.Combine(DEF_TEMPLATE_FILEPATH, DEF_TEMPLATE_FILENAME);
            m_BaseLineParams = new Params();
            m_TemplateFilePath = DEF_TEMPLATE_FILEPATH;

            m_BaseLineParams.FileName = DEF_TEMPLATE_FILENAME;
            m_BaseLineParams.LoadTemplate(m_TemplateFileName);
        }

        public MainProcess(string templateFilePath)
        {
            m_TemplateFileName = templateFilePath;
            m_BaseLineParams = new Params();
            m_TemplateFilePath = Path.GetDirectoryName(templateFilePath);

            m_BaseLineParams.FileName = Path.GetFileName(templateFilePath);
            m_BaseLineParams.LoadTemplate(templateFilePath);
        }
    }
}
