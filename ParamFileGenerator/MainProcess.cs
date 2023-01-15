using System;
using System.IO;
using System.Reflection;
using ParamFileGenerator.Parameters;

namespace ParamFileGenerator
{
    public class MainProcess
    {
        //private IBasicParams basicTemplate;
        //private IAdvancedParams advTemplate;

        //private string mSettingsFileName = "ParamFileEditorSettings.xml";
        private static string mTemplateFileName;
        private static string mTemplateFilePath;
        //private UpdateModsTable mModsUpdate;
        //private MainProcess mMainProcess;
        private static Params mBaseLineParams;
        //private const string DEF_TEMPLATE_LABEL_TEXT = "Currently Loaded Template: ";
        private const string DEF_TEMPLATE_FILENAME = "sequest_N14_NE.params";
        private static readonly string DEF_TEMPLATE_FILEPATH = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        //public static Settings mySettings;

        public static Params BaseLineParamSet => mBaseLineParams;

        public static string TemplateFileName => Path.Combine(mTemplateFilePath, Path.GetFileName(mTemplateFileName));

        public MainProcess()
        {
            mTemplateFileName = Path.Combine(DEF_TEMPLATE_FILEPATH, DEF_TEMPLATE_FILENAME);
            mBaseLineParams = new Params();
            mTemplateFilePath = DEF_TEMPLATE_FILEPATH;

            mBaseLineParams.FileName = DEF_TEMPLATE_FILENAME;
            mBaseLineParams.LoadTemplate(mTemplateFileName);
        }

        public MainProcess(string templateFilePath)
        {
            mTemplateFileName = templateFilePath;
            mBaseLineParams = new Params();
            mTemplateFilePath = Path.GetDirectoryName(templateFilePath);

            mBaseLineParams.FileName = Path.GetFileName(templateFilePath);
            mBaseLineParams.LoadTemplate(templateFilePath);
        }
    }
}
