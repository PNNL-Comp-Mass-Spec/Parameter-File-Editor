using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace ParamFileGenerator
{
    enum IniItemTypeEnum
    {
        GetKeys = 0,
        GetValues = 1,
        GetKeysAndValues = 2
    }

#pragma warning disable RCS1194
    public class IniFileReaderNotInitializedException : ApplicationException
#pragma warning restore RCS1194
    {
        public override string Message => "The IniFileReader instance has not been properly initialized.";
    }

    public class IniFileReader
    {
        private string mIniFilename;
        private XmlDocument mXmlDoc;
        private List<string> sections = new List<string>();
        private bool mCaseSensitive = false;
        private string mSaveFilename;
        private bool mInitialized = false;

        public IniFileReader(string settingsFileName)
        {
            InitIniFileReader(settingsFileName, false);
        }

        public IniFileReader(string settingsFileName, bool isCaseSensitive)
        {
            InitIniFileReader(settingsFileName, isCaseSensitive);
        }

        private void InitIniFileReader(string settingsFileName, bool isCaseSensitive)
        {
            TextReader tr = null;
            mCaseSensitive = isCaseSensitive;
            mXmlDoc = new XmlDocument();

            if (string.IsNullOrWhiteSpace(settingsFileName))
            {
                return;
            }

            // try to load the file as an XML file
            try
            {
                mXmlDoc.Load(settingsFileName);
                UpdateSections();
                mIniFilename = settingsFileName;
                mInitialized = true;
            }
            catch
            {
                // load the default XML
                mXmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?><sections></sections>");
                try
                {
                    var fi = new FileInfo(settingsFileName);
                    if (fi.Exists)
                    {
                        tr = fi.OpenText();
                        var s = tr.ReadLine();
                        while (s is not null)
                        {
                            if (s.Contains(";"))
                            {
                                s = s.Substring(0, Math.Max(s.IndexOf(";") - 1, 0)).Trim();
                            }
                            ParseLineXml(s, mXmlDoc);
                            s = tr.ReadLine();
                        }
                        mIniFilename = settingsFileName;
                        mInitialized = true;
                    }
                    else
                    {
                        mXmlDoc.Save(settingsFileName);
                        mIniFilename = settingsFileName;
                        mInitialized = true;
                    }
                }
                catch //(Exception e)
                {
                    // MessageBox.Show(e.Message);
                }
                finally
                {
                    tr?.Close();
                }
            }
        }

        public string IniFilename
        {
            get
            {
                if (!Initialized)
                    throw new IniFileReaderNotInitializedException();
                return mIniFilename;
            }
        }

        public bool Initialized => mInitialized;

        public bool CaseSensitive => mCaseSensitive;

        private string SetNameCase(string aName)
        {
            if (CaseSensitive)
            {
                return aName;
            }
            else
            {
                return aName.ToLower();
            }
        }

        private XmlElement GetRoot()
        {
            return mXmlDoc.DocumentElement;
        }

        private XmlElement GetLastSection()
        {
            if (sections.Count == 0)
            {
                return GetRoot();
            }
            else
            {
                return GetSection(sections[sections.Count - 1]);
            }
        }

        private XmlElement GetSection(string sectionName)
        {
            if (sectionName != default && !string.IsNullOrEmpty(sectionName))
            {
                sectionName = SetNameCase(sectionName);
                return (XmlElement)mXmlDoc.SelectSingleNode("//section[@name='" + sectionName + "']");
            }
            return null;
        }

        private XmlElement GetItem(string sectionName, string keyName)
        {
            if (keyName is not null && !string.IsNullOrEmpty(keyName))
            {
                keyName = SetNameCase(keyName);
                var section = GetSection(sectionName);
                if (section is not null)
                {
                    return (XmlElement)section.SelectSingleNode("item[@key='" + keyName + "']");
                }
            }
            return null;
        }

        public bool SetIniSection(string oldSection, string newSection)
        {
            if (!Initialized)
            {
                throw new IniFileReaderNotInitializedException();
            }
            if (newSection is not null && !string.IsNullOrEmpty(newSection))
            {
                var section = GetSection(oldSection);
                if (section is not null)
                {
                    section.SetAttribute("name", SetNameCase(newSection));
                    UpdateSections();
                    return true;
                }
            }
            return false;
        }

        public bool SetIniValue(string sectionName, string keyName, string newValue)
        {
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            var section = GetSection(sectionName);
            if (section is null)
            {
                if (CreateSection(sectionName))
                {
                    section = GetSection(sectionName);
                    // exit if keyName is Nothing or blank
                    if (keyName is null || string.IsNullOrEmpty(keyName))
                    {
                        return true;
                    }
                }
                else
                {
                    // can't create section
                    return false;
                }
            }
            if (keyName is null)
            {
                // delete the section
                return DeleteSection(sectionName);
            }

            var item = GetItem(sectionName, keyName);
            if (item is not null)
            {
                if (newValue is null)
                {
                    // delete this item
                    return DeleteItem(sectionName, keyName);
                }
                else
                {
                    // add or update the value attribute
                    item.SetAttribute("value", newValue);
                    return true;
                }
            }
            // try to create the item
            else if (!string.IsNullOrEmpty(keyName) && newValue is not null)
            {
                // construct a new item (blank values are OK)
                item = mXmlDoc.CreateElement("item");
                item.SetAttribute("key", SetNameCase(keyName));
                item.SetAttribute("value", newValue);
                section.AppendChild(item);
                return true;
            }
            return false;
        }

        private bool DeleteSection(string sectionName)
        {
            var section = GetSection(sectionName);
            if (section is not null)
            {
                section.ParentNode.RemoveChild(section);
                UpdateSections();
                return true;
            }
            return false;
        }

        private bool DeleteItem(string sectionName, string keyName)
        {
            var item = GetItem(sectionName, keyName);
            if (item is not null)
            {
                item.ParentNode.RemoveChild(item);
                return true;
            }
            return false;
        }

        public bool SetIniKey(string sectionName, string keyName, string newValue)
        {
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            var item = GetItem(sectionName, keyName);
            if (item is not null)
            {
                item.SetAttribute("key", SetNameCase(newValue));
                return true;
            }
            return false;
        }

        public string GetIniValue(string sectionName, string keyName)
        {
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            XmlNode N = GetItem(sectionName, keyName);
            if (N is not null)
            {
                return N.Attributes.GetNamedItem("value").Value;
            }
            return null;
        }

        private void UpdateSections()
        {
            sections = new List<string>();

            foreach (XmlElement item in mXmlDoc.SelectNodes("sections/section"))
                sections.Add(item.GetAttribute("name"));
        }

        public List<string> AllSections
        {
            get
            {
                if (!Initialized)
                {
                    throw new IniFileReaderNotInitializedException();
                }
                return sections;
            }
        }

        private List<string> GetItemsInSection(string sectionName, IniItemTypeEnum itemType)
        {
            var items = new List<string>();
            XmlNode section = GetSection(sectionName);

            if (section is null)
            {
                return null;
            }

            var nodes = section.SelectNodes("item");
            if (nodes.Count > 0)
            {
                foreach (XmlNode currentNode in nodes)
                {
                    switch (itemType)
                    {
                        case IniItemTypeEnum.GetKeys:
                            items.Add(currentNode.Attributes.GetNamedItem("key").Value);
                            break;
                        case IniItemTypeEnum.GetValues:
                            items.Add(currentNode.Attributes.GetNamedItem("value").Value);
                            break;
                        case IniItemTypeEnum.GetKeysAndValues:
                            items.Add(currentNode.Attributes.GetNamedItem("key").Value + "=" +
                                      currentNode.Attributes.GetNamedItem("value").Value);
                            break;
                    }
                }
            }
            return items;
        }

        public List<string> AllKeysInSection(string sectionName)
        {
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            return GetItemsInSection(sectionName, IniItemTypeEnum.GetKeys);
        }

        public List<string> AllValuesInSection(string sectionName)
        {
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            return GetItemsInSection(sectionName, IniItemTypeEnum.GetValues);
        }

        public List<string> AllItemsInSection(string sectionName)
        {
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            return GetItemsInSection(sectionName, IniItemTypeEnum.GetKeysAndValues);
        }

        public string GetCustomIniAttribute(string sectionName, string keyName, string attributeName)
        {
            XmlElement N;
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            if (attributeName is not null && !string.IsNullOrEmpty(attributeName))
            {
                N = GetItem(sectionName, keyName);
                if (N is not null)
                {
                    attributeName = SetNameCase(attributeName);
                    return N.GetAttribute(attributeName);
                }
            }
            return null;
        }

        public bool SetCustomIniAttribute(string sectionName, string keyName, string attributeName, string attributeValue)
        {
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            if (string.IsNullOrEmpty(attributeName))
            {
                return false;
            }

            var item = GetItem(sectionName, keyName);
            if (item is not null)
            {
                try
                {
                    if (attributeValue is null)
                    {
                        // delete the attribute
                        item.RemoveAttribute(attributeName);
                        return true;
                    }
                    else
                    {
                        attributeName = SetNameCase(attributeName);
                        item.SetAttribute(attributeName, attributeValue);
                        return true;
                    }
                }

                catch //(Exception e)
                {
                    // MessageBox.Show(e.Message)
                }
            }

            return false;
        }

        private bool CreateSection(string sectionName)
        {
            if (sectionName is not null && !string.IsNullOrEmpty(sectionName))
            {
                sectionName = SetNameCase(sectionName);
                try
                {
                    var item = mXmlDoc.CreateElement("section");
                    var itemAttribute = mXmlDoc.CreateAttribute("name");
                    itemAttribute.Value = SetNameCase(sectionName);
                    item.Attributes.SetNamedItem(itemAttribute);
                    mXmlDoc.DocumentElement.AppendChild(item);
                    sections.Add(itemAttribute.Value);
                    return true;
                }
                catch //(Exception e)
                {
                    // MessageBox.Show(e.Message)
                    return false;
                }
            }
            return false;
        }

        private bool CreateItem(string sectionName, string keyName, string newValue)
        {
            try
            {
                var section = GetSection(sectionName);
                if (section is not null)
                {
                    var item = mXmlDoc.CreateElement("item");
                    item.SetAttribute("key", keyName);
                    item.SetAttribute("newValue", newValue);
                    section.AppendChild(item);
                    return true;
                }
                return false;
            }
            catch //(Exception e)
            {
                // MessageBox.Show(e.Message)
                return false;
            }
        }

        private void ParseLineXml(string dataLine, XmlDocument doc)
        {
            dataLine = dataLine.TrimStart();

            if (string.IsNullOrWhiteSpace(dataLine))
            {
                return;
            }

            switch (dataLine.Substring(0, 1))
            {
                case "[":
                    // this is a section
                    // trim the first and last characters
                    dataLine = dataLine.TrimStart('[');
                    dataLine = dataLine.TrimEnd(']');
                    // create a new section element
                    CreateSection(dataLine);
                    break;

                case ";":
                    // new comment
                    var newComment = doc.CreateElement("comment");
                    newComment.InnerText = dataLine.Substring(1);
                    GetLastSection().AppendChild(newComment);
                    break;

                default:
                    // split the string on the "=" sign, if present
                    string key;
                    string value;

                    if (dataLine.IndexOf("=", StringComparison.Ordinal) > 0)
                    {
                        var parts = dataLine.Split('=');
                        key = parts[0].Trim();
                        value = parts[1].Trim();
                    }
                    else
                    {
                        key = dataLine;
                        value = "";
                    }

                    var newItem = doc.CreateElement("item");
                    var keyAttribute = doc.CreateAttribute("key");
                    keyAttribute.Value = SetNameCase(key);
                    newItem.Attributes.SetNamedItem(keyAttribute);

                    var valueAttribute = doc.CreateAttribute("value");
                    valueAttribute.Value = value;
                    newItem.Attributes.SetNamedItem(valueAttribute);

                    GetLastSection().AppendChild(newItem);
                    break;
            }
        }

        public string OutputFilename
        {
            get
            {
                if (!Initialized)
                    throw new IniFileReaderNotInitializedException();
                return mSaveFilename;
            }
            set
            {
                if (!Initialized)
                    throw new IniFileReaderNotInitializedException();
                var fi = new FileInfo(value);
                if (!fi.Directory.Exists)
                {
                }
                // MessageBox.Show("Invalid path.")
                else
                {
                    mSaveFilename = value;
                }
            }
        }

        public void Save()
        {
            if (!Initialized)
                throw new IniFileReaderNotInitializedException();
            if (OutputFilename is not null && mXmlDoc is not null)
            {
                var fi = new FileInfo(OutputFilename);
                if (!fi.Directory.Exists)
                {
                    // MessageBox.Show("Invalid path.")
                    return;
                }
                if (fi.Exists)
                {
                    fi.Delete();
                    mXmlDoc.Save(OutputFilename);
                }
                else
                {
                    mXmlDoc.Save(OutputFilename);
                }
            }
        }

        //public string AsIniFile()
        //{
        //    if (!Initialized)
        //        throw new IniFileReaderNotInitializedException();
        //    try
        //    {
        //        var xsl = new XslTransform();
        //        xsl.Load(@"c:\\XMLToIni.xslt");
        //        var sb = new StringBuilder();
        //        var sw = new StringWriter(sb);
        //        xsl.Transform(mXmlDoc, default, sw, default);
        //        sw.Close();
        //        return sb.ToString();
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message);
        //        return null;
        //    }
        //}

        public XmlDocument XmlDoc
        {
            get
            {
                if (!Initialized)
                    throw new IniFileReaderNotInitializedException();
                return mXmlDoc;
            }
        }

        public string XML
        {
            get
            {
                if (!Initialized)
                    throw new IniFileReaderNotInitializedException();
                var sb = new StringBuilder();
                var sw = new StringWriter(sb);
                var xw = new XmlTextWriter(sw)
                {
                    Indentation = 3,
                    Formatting = Formatting.Indented
                };
                mXmlDoc.WriteContentTo(xw);
                xw.Close();
                sw.Close();
                return sb.ToString();
            }
        }
    }
}
