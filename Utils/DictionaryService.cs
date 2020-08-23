using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Utils.Properties;

namespace Utils
{
    public class DictionaryService : IDictionaryService
    {
        public XmlDocument Dictionary { get; set; }
        public DictionaryService()
        {
            LoadDictionary();
        }

        private void LoadDictionary()
        {
            Dictionary = new XmlDocument();
            Dictionary.LoadXml(Resources.Dictionary);
        }

        public string GetBreadcrumbsTranslation(string item, string language = "pl")
        {
            var node = Dictionary.SelectSingleNode($"/dictionary/breadcrumbs/items[@lang='{language}']/item[@key='{item}']");
            return node.InnerText;
        }
    }
}
