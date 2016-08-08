using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BuildXDoc
{
    //TODO: Document each function.
    //TODO: Document how the element and attribute KeyValuePairs are KeyValuePair<Name,Value>.
    //TODO: Look at swapping amount and toWhere in all the functions
    //TODO: Look at adding a function (or function set) to select a specific node.
    public class Builder
    {
        public XDocument XmlData { get; private set; }
        public string RootName { get; private set; }
        public XElement SelectedElement { get; private set; }


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
            SelectedElement = XmlData.Elements().First(e => e.Name.LocalName == RootName);
        }

        #region Add
        //TODO:If more than one node in list, make selectable (i.e pass through a number to select.)
        public void Add(string elementName, int amount = 1)
        {
            for (var i = 0; i < amount; i++)
            {
                SelectedElement.Add(new XElement(elementName));
            }
        }

        //TODO:If more than one node in list, make selectable (i.e pass through a number to select.)
        public void Add(string elementName, string value, int amount = 1)
        {
            for (var i = 0; i < amount; i++)
            {
                SelectedElement.Add(new XElement(elementName, value));
            }
        }

        //TODO:If more than one node in list, make selectable (i.e pass through a number to select.)
        public void Add(string elementName, List<KeyValuePair<string, string>> attributes, int amount = 1)
        {
            var attrs = new List<XAttribute>();

            foreach (var attr in attributes)
            {
                attrs.Add(new XAttribute(attr.Key,attr.Value));
            }

            var attrsArray = attrs.ToArray();

            for (var i = 0; i < amount; i++)
            {
                
                SelectedElement.Add(new XElement(elementName,attrsArray));
            }
        }

        //TODO:If more than one node in list, make selectable (i.e pass through a number to select.)
        public void Add(string elementName, string value, List<KeyValuePair<string, string>> attributes, int amount = 1)
        {
            var attrs = new List<XAttribute>();

            foreach (var attr in attributes)
            {
                attrs.Add(new XAttribute(attr.Key,attr.Value));
            }

            var attrsArray = attrs.ToArray();

            for (var i = 0; i < amount; i++)
            {
                SelectedElement.Add(new XElement(elementName, value,attrsArray));
            }
        }
        #endregion

        #region AddRange

        public void AddRange(string[] elements, int amount = 1)
        {
            var eles = new List<XElement>();

            foreach (var element in elements)
            {
                eles.Add(new XElement(element));
            }

            var elesArray = eles.ToArray();

            for (var i = 0; i < amount; i++)
            {
                SelectedElement.Add(elesArray);
            }
        }

        public void AddRange(KeyValuePair<string, string>[] elesAndVals, int amount = 1)
        {
            var eles = new List<XElement>();

            foreach (var pair in elesAndVals)
            {
                eles.Add(new XElement(pair.Key,pair.Value));
            }

            var elesArray = eles.ToArray();

            for (var i = 0; i < amount; i++)
            {
                SelectedElement.Add(elesArray);
            }
        }
        //The attributes are cloned to each element.
        public void AddRange(KeyValuePair<string, string>[] elesAndValues, KeyValuePair<string, string>[] attributes,
            int amount = 1)
        {

            var attrs = new List<XAttribute>();

            
            foreach (var pair in attributes)
            {
                attrs.Add(new XAttribute(pair.Key,pair.Value));
            }

            var attrsArray = attrs.ToArray();

            for (var i = 0; i < amount; i++)
            {
                foreach (var pair in elesAndValues)
                {
                    SelectedElement.Add(new XElement(pair.Key, pair.Value, attrsArray));
                }
            }
        }
        #endregion

        #region Add Into element

        public void AddIn(string element, string elementToPlaceUnder, int index = 0, int amount = 1)
        {
            if(string.IsNullOrEmpty(element))
                throw new FormatException("The element cannot be null, empty or whitespace.");

            if (string.IsNullOrEmpty(elementToPlaceUnder))
                throw new FormatException(
                    "The element in which you're placing under cannot be null, empty or whitespace.");

            var eles = SelectedElement.Elements().Where(e => e.Name.LocalName == elementToPlaceUnder).ToList();

            if(eles.Count < index)
                throw new IndexOutOfRangeException("Error placing ");

            for (var i = 0; i < amount; i++)
            {
                eles[index].Add(new XElement(element));
            }
        }

        public void AddIn(KeyValuePair<string, string> eleAndValue, string elementToPlaceUnder, int index = 0,
            int amount = 1)
        {
            if(string.IsNullOrEmpty(eleAndValue.Key))
                throw new FormatException("The element cannot be null, empty or whitespace.");

            if(string.IsNullOrEmpty(elementToPlaceUnder))
                throw new FormatException("The element in which you're placing under cannot be null, empty or whitespace.");

            var eles = SelectedElement.Elements().Where(e => e.Name.LocalName == elementToPlaceUnder).ToList();

            for (var i = 0; i < amount; i++)
            {
                eles[index].Add(new XElement(eleAndValue.Key,eleAndValue.Value));
            }
        }

        public void AddIn(KeyValuePair<string, string>[] elesAndValues, string elementToPlaceUnder, int index = 0,
            int amount = 1)
        {
            foreach (var pair in elesAndValues)
            {
                if(string.IsNullOrEmpty(pair.Key))
                    throw new FormatException("The element cannot be null, empty or whitespace");
            }

            if (string.IsNullOrEmpty(elementToPlaceUnder))
                throw new FormatException("The element in which you're placing under cannot be null, empty or whitespace.");

            var eles = SelectedElement.Elements().Where(e => e.Name.LocalName == elementToPlaceUnder).ToList();

            for (var i = 0; i < amount; i++)
            {
                foreach (var pair in elesAndValues)
                {
                    eles[index].Add(new XElement(pair.Key,pair.Value));
                }
            }
        }

        #endregion
    }
}
