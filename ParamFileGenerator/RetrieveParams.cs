﻿using System;
using System.Collections.Generic;

namespace ParamFileGenerator
{
    public class RetrieveParams
    {
        // Ignore Spelling: ini

        private IniFileReader mIniFileReader;

        /// <summary>
        /// Default section name
        /// </summary>
        private string mDefaultSection = "";

        public RetrieveParams(string iniFilePath = "", bool isCaseSensitive = false)
        {
            if (!string.IsNullOrEmpty(iniFilePath))
            {
                IniFilePath = iniFilePath;
                LoadSettings(isCaseSensitive);
            }
        }

        public string IniFilePath { get; set; } = "";

        public bool LoadSettings(bool isCaseSensitive = false)
        {
            mIniFileReader = new IniFileReader(IniFilePath, isCaseSensitive);
            return mIniFileReader is not null;
        }

        public void SaveSettings()
        {
            mIniFileReader.OutputFilename = IniFilePath;
            mIniFileReader.Save();
        }

        public bool LoadSettings(string settingsFilePath)
        {
            IniFilePath = settingsFilePath;
            return LoadSettings();
        }

        public string GetParam(string item)
        {
            return mIniFileReader.GetIniValue(mDefaultSection, item);
        }

        public string GetParam(string section, string item)
        {
            var s = mIniFileReader.GetIniValue(section, item);

            return s ?? throw new Exception("No ini value for parameter '" + item + "'");
        }

        public string GetParam(string section, string item, string attributeName)
        {
            var s = mIniFileReader.GetCustomIniAttribute(section, item, attributeName);

            return s ?? throw new Exception("No custom ini value for parameter '" + item + "'");
        }

        public void SetParam(string name, string value)
        {
            mIniFileReader.SetIniValue(mDefaultSection, name, value);
        }

        public void SetParam(string section, string name, string value)
        {
            mIniFileReader.SetIniValue(section, name, value);
        }

        public void SetSection(string name)
        {
            mDefaultSection = name;
        }

        public List<string> GetAllKeysInSection(string section)
        {
            var keyNames = mIniFileReader.AllKeysInSection(section);

            return keyNames ?? throw new Exception("No Keys in section '" + section + "'");
        }
    }
}
