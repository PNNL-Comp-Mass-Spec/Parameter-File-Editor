using System;
using System.Collections.Generic;

namespace ParamFileGenerator
{
    public interface IRetrieveParams
    {
        string GetParam(string section, string item, string attributeName);
        string GetParam(string section, string item);
        string GetParam(string item);
        void SetParam(string section, string name, string Value);
        void SetParam(string name, string Value);
        void SetSection(string name);
    }

    public class RetrieveParams : IRetrieveParams
    {
        private IniFileReader m_iniFileReader;

        /// <summary>
        /// Default section name
        /// </summary>
        private string m_defaultSection = "";

        public RetrieveParams(string iniFilePath = "", bool IsCaseSensitive = false)
        {
            if (!string.IsNullOrEmpty(iniFilePath))
            {
                IniFilePath = iniFilePath;
                LoadSettings(IsCaseSensitive);
            }
        }

        public string IniFilePath { get; set; } = "";

        public bool LoadSettings(bool IsCaseSensitive = false)
        {
            m_iniFileReader = new IniFileReader(IniFilePath, IsCaseSensitive);
            return m_iniFileReader is not null;
        }

        public void SaveSettings()
        {
            m_iniFileReader.OutputFilename = IniFilePath;
            m_iniFileReader.Save();
        }

        public bool LoadSettings(string settingsFilePath)
        {
            IniFilePath = settingsFilePath;
            return LoadSettings();
        }

        public string GetParam(string item)
        {
            return m_iniFileReader.GetIniValue(m_defaultSection, item);
        }

        public string GetParam(string section, string item)
        {
            string s = m_iniFileReader.GetIniValue(section, item);
            if (s is null)
                throw new Exception("No ini value for parameter '" + item + "'");
            return s;
        }

        public string GetParam(string section, string item, string attributeName)
        {
            string s = m_iniFileReader.GetCustomIniAttribute(section, item, attributeName);
            if (s is null)
                throw new Exception("No custom ini value for parameter '" + item + "'");

            return s;
        }

        public void SetParam(string name, string Value)
        {
            m_iniFileReader.SetIniValue(m_defaultSection, name, Value);
        }

        public void SetParam(string section, string name, string Value)
        {
            m_iniFileReader.SetIniValue(section, name, Value);
        }

        public void SetSection(string name)
        {
            m_defaultSection = name;
        }

        public List<string> GetAllKeysInSection(string section)
        {
            List<string> keyNames;
            keyNames = m_iniFileReader.AllKeysInSection(section);
            if (keyNames is null)
                throw new Exception("No Keys in section '" + section + "'");

            return keyNames;
        }
    }
}
