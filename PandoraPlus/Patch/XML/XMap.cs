using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Pandora.Xml
{
    /// <summary>
    /// Represents an XML document, mapped for element access with XPath notation.
    /// </summary>
    public class XMap : XDocument
    {
        public enum KeyTypes
        {
            XAttribute,
            XElementName,
            XText
        }

        public KeyTypes KeyType { get; set; } = KeyTypes.XAttribute;
        public string File { get; set; } = string.Empty;

        public XMap() { }
        public XMap(XDocument doc) : base(doc) { }
        public XMap(XElement element) : base(element) { }
        public XMap(XName name) : base(name) { }
        public XMap(XName name, object? content) :base(name, content) { }
        public XMap(XName name, object[]? content) : base(name, content!) { }
        public XMap(XStreamingElement other) : base(other) { }

        public static new XMap Load(Stream stream) => new XMap(XDocument.Load(stream));
        public static new XMap Load(TextReader textReader) => new XMap(XDocument.Load(textReader));
        public static new XMap Load(string uri) => new XMap(XDocument.Load(uri));
        public static new XMap Load(XmlReader xmlReader) => new XMap(XDocument.Load(xmlReader));

        

        public XElement this[string s]
        {
            get => Dict[s]; 
            set => Dict[s] = value;
        }




        public Action<string> Log { get; set; } = (message) => Console.WriteLine(message);

        private string keyName = "name";
        private string defaultValue = "object";

        public Dictionary<string, XElement> Dict { get; set; } = new Dictionary<string, XElement>();
        
        
        private string MapChild(string path, XElement element, int elementindex)
        {
            //Console.WriteLine($"XPath {path} keyName {keyName}");
            //Console.WriteLine(element);
            StringBuilder defaultPathBuilder =  new StringBuilder(path).Append(defaultValue).Append(elementindex);
            StringBuilder pathBuilder = new StringBuilder(path);
            if (path != string.Empty && path[path.Length-1] != '/')
            {
                pathBuilder.Append("/");
                defaultPathBuilder = new StringBuilder(path).Append('/').Append(defaultValue).Append(elementindex);
            }
            switch (KeyType)
            {
                case KeyTypes.XAttribute:
                    pathBuilder.Append((element.Attribute(keyName) ?? new XAttribute("name", $"{defaultValue}{elementindex}")).Value);
                    break;
                case KeyTypes.XElementName:
                    pathBuilder.Append(element.Name ?? defaultValue);
                    break;  
                case KeyTypes.XText:
                    pathBuilder.Append(element.Value ?? defaultValue);
                    break;
            }
            string elementPath = pathBuilder.ToString();
            string defaultPath = defaultPathBuilder.ToString();

            if (elementPath != defaultPath) Dict.Add(elementPath, element);
            Dict.Add(defaultPathBuilder.ToString(), element);
            //Console.WriteLine($"Path: {elementPath} Default Path: {defaultPath}");
            return elementPath;
            
        }
        
        public bool Map(int skipdepth)
        {

            try
            {
                if (Root!.HasElements)
                {
                    IEnumerable<XElement> ChildElements = new XElement[] { Root };
                    for (int d = 0 ; d < skipdepth; d++)
                    {
                        ChildElements = ChildElements.First().Elements();
                    }
                    MapSubTree("", ChildElements);
                }
                return true;
            }
            catch (XmlException e)
            {
                throw new XmlException($"{e.Message} in file {Root!.Name}");
            }
            
            
        }


        public string GetKeyPath(XElement element, int skipdepth)
        {
            List<string> path = new List<string>();
            string key;
            XElement currentElement = element;
            while (currentElement != null)
            {
                key = currentElement?.Attribute(keyName)?.Value ?? defaultValue + currentElement?.ElementsBeforeSelf().Count().ToString() ?? throw new ArgumentNullException("Invalid XElement");
                path.Insert(0, key);
                currentElement = currentElement!.Parent!;
            }
            path.RemoveRange(0, skipdepth);
            return String.Join("/", path);
        }

        public string GetElementPathWithAttribute(int skipdepth, string attributeName, string value)
        {
            if (Root!.HasElements)
            {
                IEnumerable<XElement> ChildElements = new XElement[] { Root };
                for (int i = 0; i < skipdepth; i++)
                {
                    ChildElements = ChildElements.First().Elements();
                }
                foreach (XElement child in ChildElements)
                {
                    if (child.HasAttributes)
                    {
                        if (child.Attribute(attributeName)!.Value == value)
                        {
                            return GetKeyPath(child, 2);
                        }
                    }
                }
            }
            return "null";
        }
        private void MapSubTree(string path, IEnumerable<XElement> ChildElements)
        {
            string elementName;
            int defaultCount = 0;
            
            foreach (XElement childElement in ChildElements)
            {
                elementName = MapChild(path, childElement, defaultCount);
                if (childElement.HasElements)
                {
                    MapSubTree(elementName, childElement.Elements());
                }
                defaultCount++;
            }
        }
    }
}
