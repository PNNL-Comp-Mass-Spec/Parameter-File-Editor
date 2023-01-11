using System.IO;
using System.Reflection;

namespace ParamFileGenerator
{

    public class MainProcess
    {

        // Private basicTemplate As IBasicParams
        // Private advTemplate As IAdvancedParams

        // Private m_SettingsFileName As String = "ParamFileEditorSettings.xml"
        private static string m_TemplateFileName;
        private static string m_TemplateFilePath;
        // Private m_modsUpdate As UpdateModsTable
        // Private m_mainProcess As MainProcess
        private static Params m_BaseLineParams;
        // Const DEF_TEMPLATE_LABEL_TEXT As String = "Currently Loaded Template: "
        private const string DEF_TEMPLATE_FILENAME = "sequest_N14_NE.params";
        private static readonly string DEF_TEMPLATE_FILEPATH = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        // Public Shared mySettings As Settings

        public static Params BaseLineParamSet
        {
            get
            {
                return m_BaseLineParams;
            }
        }

        public static string TemplateFileName
        {
            get
            {
                return Path.Combine(m_TemplateFilePath, Path.GetFileName(m_TemplateFileName));
            }
        }

        public MainProcess()
        {

            m_TemplateFileName = Path.Combine(DEF_TEMPLATE_FILEPATH, DEF_TEMPLATE_FILENAME);
            m_BaseLineParams = new Params();
            m_TemplateFilePath = DEF_TEMPLATE_FILEPATH;


            {
                ref var withBlock = ref m_BaseLineParams;
                withBlock.FileName = DEF_TEMPLATE_FILENAME;
                withBlock.LoadTemplate(m_TemplateFileName);
            }

        }

        public MainProcess(string templateFilePath)
        {
            m_TemplateFileName = templateFilePath;
            m_BaseLineParams = new Params();
            m_TemplateFilePath = Path.GetDirectoryName(templateFilePath);

            {
                ref var withBlock = ref m_BaseLineParams;
                withBlock.FileName = Path.GetFileName(templateFilePath);
                withBlock.LoadTemplate(templateFilePath);

            }
        }

    }
}
