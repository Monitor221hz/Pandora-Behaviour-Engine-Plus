using Pandora.Xml;
using Pandora.Patch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Pandora.Patch
{
    public class SkyrimPatcher : IPackFilePatcher
    {

        public Dictionary<string, Func<XObject, XParticle, bool>> ActionKeys { get; set; } = new Dictionary<string, Func<XObject, XParticle, bool>>();
        public string KeyAttributeName { get; set; } = "name";
        public string CountAttributeName { get; set; } = "numelements";


        const string graphStringData = "hkbBehaviorGraphStringData";
        const string graphData = "hkbBehaviorGraphData";
        const string varValueSet = "hkbVariableValueSet";

        public string DefaultPathValue { get; set; } = "object";
        public SkyrimPatcher()
        {
            ActionKeys.Add("CLOSE", Append);
            ActionKeys.Add("ORIGINAL", Replace);
            ActionKeys.Add("DEFAULT", Insert);
        }

        public bool Insert(XObject obj, XParticle particle)
        {
            XMap map = (XMap)obj;
            XObject content = particle.Content;
            XElement immediateParent = map.Root!.Elements().First();
#if DEBUG
                    Debug.WriteLine($"XElement insert : {content}");
#endif 
            lock (immediateParent)
            {
                immediateParent.Add(content);
            }
            return true;
        }

        public bool Remove(XObject xObj, XParticle particle)
        {
            string key = particle.StringData;
            string parentPath = key[..key.LastIndexOf('/')];
            XMap map = (XMap)xObj;
            XElement element;
            XObject content = particle.Content;
            switch (content)
            {
                case XElement e:
#if DEBUG
                    Debug.WriteLine($"XElement remove : {key}");
#endif 
                    if (map.Dict.TryGetValue(key, out element!))
                    {
                        lock (element.Parent!)
                            lock (element)
                            {
                                element.Remove();
                                map.Dict.Remove(key);
                            }
                    }
                    else { return false; }

                    break;
                case XText t:
#if DEBUG
                    Debug.WriteLine($"XText remove : {key} value: {particle.Content.ToString()}");
#endif 
                    if (map.Dict.TryGetValue(parentPath, out element!))
                    {
                        lock (element)
                        {
                            int replaceIndex = Int32.Parse(key[(key.LastIndexOf('/') + 1)..]);
                            List<string> items = element.Value.Split().Where(x => !String.IsNullOrWhiteSpace(x)).ToList();
                            List<string> newItems = (t.Value.Split().Where(x => !String.IsNullOrWhiteSpace(x))).ToList();
                            foreach (string str in newItems)
                            {

                                if (replaceIndex < items.Count)
                                {
                                    items.RemoveAt(replaceIndex);
                                }
                                replaceIndex++;
                            }
                            StringBuilder sb = new StringBuilder("\r\n");
                            sb.Append(String.Join(' ', items));
                            sb.Append('\r');
                            sb.Append('\n');
                            element.SetValue(String.Join(' ', items));
                            XAttribute countAttribute = element.Attribute(CountAttributeName)!;
                            if (countAttribute != null)
                            {
                                lock (countAttribute)
                                {
                                    countAttribute.SetValue(items.Count);
                                }
                            }

                        }

                    }
                    else { return false; }
                    break;
            }


            return true;
        }
        public bool Replace(XObject xObj, XParticle particle)
        {
            string key = particle.StringData;
            string parentPath = key[..key.LastIndexOf('/')];
            XMap map = (XMap)xObj;
            XElement element;
            XObject content = particle.Content;
            switch (content)
            {
                case XElement e:
#if DEBUG
                    Debug.WriteLine($"XElement replace : {key}");
#endif 
                    if (map.Dict.TryGetValue(key, out element!)) 
                    { 
                        lock(element.Parent!)
                        lock(element) 
                        {
                            XElement newElement = (XElement)particle.Content;
                            element.ReplaceWith(newElement);
                            map.Dict[key] = newElement;
                        }
                    }
                    else { return false; }

                    break;
                case XText t:
#if DEBUG
                    Debug.WriteLine($"XText replace : {key} value: {particle.Content.ToString()}");
#endif 
                    if (map.Dict.TryGetValue(parentPath, out element!))
                    {
                        lock (element)
                        {
                            int replaceIndex = Int32.Parse(key[(key.LastIndexOf('/') + 1)..]);
                            List<string> items = element.Value.Split().Where(x => !String.IsNullOrWhiteSpace(x)).ToList();
                            List<string> newItems = (t.Value.Split().Where(x => !String.IsNullOrWhiteSpace(x))).ToList();
                            foreach (string str in newItems)
                            {
                                if (replaceIndex > items.Count - 1)
                                {
                                    items.Add(str);
                                }
                                else
                                {
                                    items[replaceIndex] = str;
                                }
                                replaceIndex++;
                            }
                            StringBuilder sb = new StringBuilder("\r\n");
                            sb.Append(String.Join(' ', items));
                            sb.Append('\r');
                            sb.Append('\n');
                            element.SetValue(String.Join(' ', items));
                            XAttribute countAttribute = element.Attribute(CountAttributeName)!;
                            if (countAttribute!= null)
                            {
                                lock (countAttribute)
                                {
                                    countAttribute.SetValue(items.Count);
                                }
                            }
                            
                        }
                        
                    }
                    else { return false; }
                    break;
            }

            
            return true;
        }

        public bool Append(XObject xObj, XParticle particle)
        {
            string key = particle.StringData;
            XObject content = particle.Content;
            string parentPath = key.Substring(0, key.LastIndexOf('/'));
            XMap map = (XMap)xObj;


            XElement parent;
            if (map.Dict.TryGetValue(parentPath, out parent!))
            {
                switch (content)
                {
                    case XElement e:
#if DEBUG
                        Debug.WriteLine($"XElement append: {parentPath}");
#endif
                        parent.Add(particle.Content);
                        StringBuilder newPathBuilder = new StringBuilder(parentPath);
                        newPathBuilder.Append('/');
                        XAttribute countAttribute = parent.Attribute(CountAttributeName)!;  
                        lock (countAttribute)
                        {
                            newPathBuilder.Append(DefaultPathValue);
                            newPathBuilder.Append(countAttribute.Value);
                            countAttribute.SetValue(Int32.Parse(countAttribute.Value) + 1);
                        }
                        map.Dict.Add(newPathBuilder.ToString(), e);

                        break;
                    case XText t:
#if DEBUG
                        Debug.WriteLine($"XText append: {parentPath}");
#endif
                        StringBuilder sb = new StringBuilder(parent.Value.TrimEnd(' ', '\n', '\r', '\t'));
                        sb.Append(' ');
                        sb.Append(t.ToString());
                        sb.Append('\r');
                        sb.Append('\n');
                        string newValue = sb.ToString();

                        parent.SetValue(newValue);
                        parent.Attribute(CountAttributeName)!.SetValue(newValue.Split().Count(x => !String.IsNullOrWhiteSpace(x)));
                        break;
                }

                
            }
            return true;
        }

        public void PreExportFile(PackFile packFile)
        {
            MemoryStream stream = new MemoryStream();
            packFile.Map!.Save(stream);
            stream.Position = 0;
            XmlWriterSettings writerSettings = new XmlWriterSettings() { IndentChars = "\t" };
            using(FileStream outStream = File.Create(packFile.outputPath))
            {
                using (XmlWriter writer = XmlWriter.Create(outStream, writerSettings))
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    while (reader.Read())
                    {
                        
                        writer.WriteNode(reader, true);
                    }
                }
            }
        }

        public bool ValidateFile(PackFile packFile)
        {
            ILookup<string, XElement> classLookup = packFile.Map!.Root!.Elements().First().Elements().ToLookup(x => x.Attribute("class")!.Value);
            RemoveDuplicateEvents(packFile.Map!, classLookup);
            RemoveDuplicateVars(packFile.Map!, classLookup);
            ResolveReferences(packFile.Map!, classLookup);
            return true;
        }

        public void ResolveReferences(XMap map, ILookup<string,XElement> ClassLookup)
        {

            if (ClassLookup.Contains(graphStringData))
            {
                Regex EventFormat = new Regex(@"[$]{1}eventID{1}[\[]{1}(.+)[\]]{1}[$]{1}");
                Regex VarFormat = new Regex(@"[$]{1}variableID{1}[\[]{1}(.+)[\]]{1}[$]{1}");

                Dictionary<string, int> eventDict= map[$"{ClassLookup[graphStringData].First().Attribute(KeyAttributeName)!.Value}/eventNames"].Elements().Select((s, index) => new { str = s.Value, index })
    .ToDictionary(x => x.str, x => x.index);
                Dictionary<string, int> varDict = map[$"{ClassLookup[graphStringData].First().Attribute(KeyAttributeName)!.Value}/variableNames"].Elements().Select((s, index) => new { str = s.Value, index })
    .ToDictionary(x => x.str, x => x.index);

                IEnumerable<XText> References = map.Root!.DescendantNodes().Where(x => x is XText).Cast<XText>().Where(x => x.Value.Contains('$'));
                Match EventMatch;
                Match VarMatch;
                foreach (XText reference in References)
                {
                    if ((EventMatch = EventFormat.Match(reference.Value)).Success)
                    {
                        reference.Value = eventDict[EventMatch.Groups[1].Value].ToString();
                    }
                    else if ((VarMatch = VarFormat.Match(reference.Value)).Success)
                    {
                        reference.Value = varDict[VarMatch.Groups[1].Value].ToString();
                    }
                }
            }
        }

        public void RemoveDuplicateEvents(XMap map, ILookup<string, XElement> ClassLookup)
        {
            XElement EventNamesNode;
            XElement EventInfosNode;

            
            if (ClassLookup.Contains(graphStringData) && ClassLookup.Contains(graphData))
            {
                EventNamesNode = map[$"{ClassLookup[graphStringData].First().Attribute(KeyAttributeName)!.Value}/eventNames"];
                EventInfosNode = map[$"{ClassLookup[graphData].First().Attribute(KeyAttributeName)!.Value}/eventInfos"];

                List<XElement> EventNames = EventNamesNode.Elements().ToList();
                List<XElement> EventInfos = EventInfosNode.Elements().ToList();
                int EventCount = EventNames.Count;

                var duplicates = EventNames
                .Select((t, i) => new { Index = i, Text = t })
                .GroupBy(g => g.Text.Value)
                .Where(g => g.Count() > 1);

                HashSet<XElement> removeTargets = new HashSet<XElement>();
                foreach (var g in duplicates)
                {

                    var copies = g.Skip(1);
                    foreach (var e in copies)
                    {

                        EventNames[e.Index].ReplaceWith(new XComment($"DUPLICATE EVENT {EventNames[e.Index].Value} REMOVED"));
                        //EventNames[e.Index].Remove();
                        EventInfos[e.Index].Remove();

                        EventCount--;

                    }
                }
                EventNamesNode.Attribute(CountAttributeName)!.SetValue(EventCount.ToString());
                EventInfosNode.Attribute(CountAttributeName)!.SetValue(EventCount.ToString());
            }
            
        }

        public void RemoveDuplicateVars(XMap map, ILookup<string, XElement> ClassLookup)
        {
            XElement VarNamesNode;
            XElement VarInfosNode;
            XElement VarValuesNode;


            if (ClassLookup.Contains(graphStringData) && ClassLookup.Contains(graphData) && ClassLookup.Contains(varValueSet)) 
            {
                VarNamesNode = map[$"{ClassLookup[graphStringData].First().Attribute(KeyAttributeName)!.Value}/variableNames"];
                VarInfosNode = map[$"{ClassLookup[graphData].First().Attribute(KeyAttributeName)!.Value}/variableInfos"];
                VarValuesNode = map[$"{ClassLookup[varValueSet].First().Attribute(KeyAttributeName)!.Value}/wordVariableValues"];

                List<XElement> VarNames = VarNamesNode.Elements().ToList();
                List<XElement> VarInfos = VarInfosNode.Elements().ToList();
                List<XElement> VarValues = VarValuesNode.Elements().ToList();


                int VarCount = VarNames.Count;
                var duplicates = VarNames
                .Select((t, i) => new { Index = i, Text = t })
                .GroupBy(g => g.Text.Value)
                .Where(g => g.Count() > 1);
                foreach (var g in duplicates)
                {

                    var copies = g.Skip(1);
                    foreach (var e in copies)
                    {
                        VarNames[e.Index].ReplaceWith(new XComment($"DUPLICATE VARIABLE {VarNames[e.Index].Value} REMOVED"));
                        //VarNames[e.Index].Remove();
                        VarInfos[e.Index].Remove();
                        VarValues[e.Index].Remove();
                        VarCount--;
                    }
                }
                VarNamesNode.Attribute(CountAttributeName)!.SetValue(VarCount.ToString());
                VarInfosNode.Attribute(CountAttributeName)!.SetValue(VarCount.ToString());
                VarValuesNode.Attribute(CountAttributeName)!.SetValue(VarCount.ToString());
            }            
        }

        public XMap LoadMap(string file)
        {
            XmlReaderSettings settings = new XmlReaderSettings() { IgnoreComments = true, IgnoreWhitespace = true };

            using (XmlReader reader = XmlReader.Create(file, settings))
            {
                return XMap.Load(reader);
            }
        }

        public void Save()
        {

        }
    }
}
