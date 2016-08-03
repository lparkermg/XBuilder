using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BuildXDoc
{
    public class Builder
    {
        public XDocument XmlData { get; private set; }
        public string RootName { get; private set; }

        public Builder(string key, List<KeyValuePair<string, string>> attributes = null)
        {
            List<object> attr = new List<object>();
            RootName = key;

            if (attributes != null)
            {
                foreach (var attribute in attributes)
                {
                    attr.Add(new XAttribute(attribute.Key,attribute.Value));
                }
            }
            
            XmlData = new XDocument(new XElement(key, attr.ToArray()));
        }
    }
}
