using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace Pandora.Xml
{
    public class XParticleCollector
    {
        public Action<string?>? WriteLog;


        public Dictionary<string, Func<XObject, XParticle, bool>> ActionDict { get; set; } = new Dictionary<string, Func<XObject, XParticle, bool>>();
        

        public XParticleCollector()
        {
        }
        public async Task<List<XParticle>> CollectReadFolder(string folder, HashSet<string> captureStartComments)
        {
            List<Task<List<XParticle>>> tasks = new List<Task<List<XParticle>>>();
            string[] files = Directory.GetFiles(folder, "*.txt");

            foreach(string file in files)
            {
                tasks.Add(Task.Run(() => CollectReader(file, captureStartComments)));
            }
            List<XParticle> particles = new List<XParticle>();
            await Task.WhenAll(tasks);

            foreach(var task in tasks)
            {
                particles = particles.Concat(task.Result).ToList();     
            }

            return particles;

        }
        public async Task<List<XParticle>> CollectReadFolder(string folder, HashSet<string> captureStartComments, string searchPattern)
        {
            List<Task<List<XParticle>>> tasks = new List<Task<List<XParticle>>>();
            string[] files = Directory.GetFiles(folder, searchPattern);

            foreach (string file in files)
            {
                tasks.Add(Task.Run(() => CollectReader(file, captureStartComments)));
            }
            List<XParticle> particles = new List<XParticle>();
            await Task.WhenAll(tasks);

            foreach (var task in tasks)
            {
                particles = particles.Concat(task.Result).ToList();
            }

            return particles;

        }

        public HashSet<XParticle> CollectDocument(string file, HashSet<string> captureStartComments)
        {
            bool captureAll = captureStartComments.Count == 0;
            XmlReaderSettings settings = new XmlReaderSettings() { IgnoreWhitespace = true, CheckCharacters = false, IgnoreProcessingInstructions = true };
            HashSet<XParticle> particles = new HashSet<XParticle>();
            HashSet<XParticle> ParticleSet = new HashSet<XParticle>();

            List<string> pathStack = new List<string>();
            List<int> genericCounts = new List<int>();

            HashSet<string> elementPaths = new HashSet<string>();
            HashSet<string> textPaths = new HashSet<string>();

            XParticle lastTextParticle = new XParticle(new XText(""), "");
            bool textOnly = false;

            XElement root = XElement.Load(file);


            IEnumerable<XNode> children = root.DescendantNodes();
            List<XComment> captureComments = children.Where(x => x is XComment && captureStartComments.Contains(((XComment)x).Value)).Select(x => (XComment)x).ToList();
            foreach (XComment captureComment in captureComments)
            {
                string s = GetXPath(captureComment);
                XNode? nextNode = captureComment.NextNode;
                bool ignore = false;
                bool onlyText = false;

                IEnumerable<XNode> nextNodes = captureComment.NodesAfterSelf();

                foreach (XNode node in nextNodes)
                {
                    Func<XObject, XParticle, bool> func;
                    switch (node)
                    {
                        
                        case XComment comment:
                            
                            if (ActionDict.TryGetValue(comment.Value, out func!))
                            {

                            }
                            break;
                        case XText text:
                            XElement parent = text.Parent!;
                            parent.Value = RemoveInvalidSnippets(parent.Value);
                            break;

                    }

                }
            }
            throw new NotImplementedException();
        }
        private string RemoveInvalidSnippets(string str)
        {
            return "";
        }
        public string GetXPath(XNode node, string keyName="name", string defaultValue="object")
        {
            List<string> path = new List<string>();
            string key;
            XNode nextNode = node.NextNode!;
            XElement currentElement;
            switch (nextNode)
            {
                case XElement e:
                    currentElement = e;
                    break;
                default:
                    currentElement = node.Parent!;
                    break;
                    
            }
            while (currentElement != null)
            {
                key = currentElement?.Attribute(keyName)?.Value ?? defaultValue + currentElement?.ElementsBeforeSelf().Count().ToString() ?? throw new ArgumentNullException("Invalid XElement");
                path.Insert(0, key);
                currentElement = currentElement!.Parent!;
            }
            return String.Join("/", path);
        }
        public List<XParticle> CollectReader(string file, HashSet<string> captureStartComments)
        {
            bool captureAll = captureStartComments.Count == 0;
            XmlReaderSettings settings = new XmlReaderSettings() { IgnoreWhitespace = false, CheckCharacters = false, IgnoreProcessingInstructions = true };
            List<XParticle> particles = new List<XParticle>();
            List<XParticle> ParticleSet = new List<XParticle>();

            List<string> pathStack = new List<string>();
            List<int> genericCounts = new List<int>();

            HashSet<string> elementPaths = new HashSet<string>();
            HashSet<string> textPaths = new HashSet<string>();

            XParticle lastTextParticle = new XParticle(new XText(""), "");
            List<string> whiteSpacePaths = new List<string>();
            bool textOnly = false;



            int maxDepth = -1;
            int lastDepth = 2147483647;
            int Depth;
            string readerValue;
            XmlNodeType NodeType;
            bool capture = false;
            int textIndex = -1;

            bool ignore = false;

            try
            {
                using (XmlReader reader = XmlReader.Create(file, settings))
                {


                    while (reader.Read())
                    {
                        Depth = reader.Depth;
                        readerValue = reader.Value;
                        NodeType = reader.NodeType;

                        if (Depth < lastDepth && Depth + 1 < pathStack.Count)
                        {
                            genericCounts[Depth + 1] = 0;
                            textOnly = false;
                            textIndex= -1;
                        }
                        lastDepth = Depth;
                        if (Depth > maxDepth)
                        {
                            maxDepth = Depth;

                            if (reader.HasAttributes)
                            {
                                pathStack.Add(reader.GetAttribute(0));
                                genericCounts.Add(0);
                            }
                            else
                            {
                                pathStack.Add("object" + "0");
                                genericCounts.Add(1);
                            }
                        }
                        else if (Depth < pathStack.Count)
                        {
                            if (reader.HasAttributes)
                            {
                                pathStack[Depth] = reader.GetAttribute(0);
                            }
                            else if (XmlNodeType.EndElement != NodeType && XmlNodeType.Comment != NodeType)
                            {
                                pathStack[Depth] = "object" + genericCounts[Depth];
                                genericCounts[Depth]++;

                            }
                        }
                        if (capture)
                        {
                            string xpath = String.Join("/", pathStack.SkipLast(maxDepth - Depth));

                            switch (NodeType)
                            {

                                case XmlNodeType.EndElement:
                                    
                                case XmlNodeType.Element:
                                    whiteSpacePaths.Clear();
                                    textIndex = 0;
                                    //WriteLog("Capture element found");
                                    using (XmlReader subreader = reader.ReadSubtree())
                                        {
                                            if (!elementPaths.Contains(xpath))
                                            {
                                                particles.Add(new XParticle(XElement.Load(subreader), xpath));
                                                elementPaths.Add(xpath);
                                            }
                                        }
                                    
                                    break;
                                case XmlNodeType.Text:
                                    if (!textPaths.Contains(xpath))
                                    {
                                        whiteSpacePaths.Clear();
                                        textIndex += 1;
                                        string[] parts = xpath.Split('/');
                                        parts[parts.Length-1] = textIndex.ToString();

                                        xpath = String.Join('/', parts);
                                        lastTextParticle = new XParticle(new XText(readerValue), xpath);
                                        particles.Add(lastTextParticle);
                                        textOnly = true;
                                        textIndex += readerValue.Split().Count(x => !String.IsNullOrWhiteSpace(x))-1;
#if DEBUG
                                        Debug.WriteLine($"readerValue {readerValue} textIndex: {textIndex} path: {xpath} inc: {readerValue.Split().Count(x => !String.IsNullOrWhiteSpace(x))-1}");
#endif
                                    }
                                    break;
                                case XmlNodeType.Whitespace:
                                    
                                    //lastTextParticle = new XParticle(new XText(string.Empty), xpath);
                                    if (particles.Count == 0 && textIndex > -1)
                                    {
                                        textIndex += 1;
                                        string[] parts = xpath.Split('/');
                                        parts[parts.Length - 1] = textIndex.ToString();
                                        xpath = String.Join('/', parts);
                                        whiteSpacePaths.Add(xpath);
                                        particles.Add(new XParticle(new XText(string.Empty), xpath));
#if DEBUG
                                        Debug.WriteLine($"Potential Whitespace: {xpath}");
#endif
                                    }

                                    break;
                                case XmlNodeType.Comment:
                                    WriteLog?.Invoke($"Reader value {readerValue.Split().Count(x => !String.IsNullOrWhiteSpace(x))}");


                                    Func<XObject, XParticle, bool> func;
                                    if (textOnly)
                                    {
                                        //particles.Add(lastTextParticle);
                                    }
                                    Func<XObject, XParticle, bool> emptyFunc;
                                    if (whiteSpacePaths.Count > 0 && ActionDict.TryGetValue("EMPTY", out emptyFunc!))
                                    {
                                        foreach(string path in whiteSpacePaths)
                                        {
                                            particles.Add(new XParticle(new XText(""), emptyFunc, path));
#if DEBUG
                                            Debug.WriteLine($"Whitespace: {path}");
#endif
                                        }
                                        
                                    }
                                    else if (ActionDict.TryGetValue(readerValue.Trim(), out func!))
                                    {
                                        WriteLog?.Invoke("Valid capture block. Values captured:");
                                        foreach (XParticle particle in particles)
                                        {
                                            WriteLog?.Invoke(particle.Content.ToString());
                                            particle.SetWork(func);
                                            ParticleSet.Add(particle);
                                        }
                                        particles.Clear();
                                        elementPaths.Clear();
                                        capture = false;
                                    }

                                    break;
                            }
                        }
                        else if (NodeType == XmlNodeType.Comment)
                        {
                            if (captureStartComments.Contains(readerValue))
                            {
                                WriteLog?.Invoke("Capture started.");
                                capture = true;
                            }
                            else
                            {
                                ignore = !ignore;
                                Debug.WriteLine($"Ignore at {readerValue}");
                            }
                            
                        }
                        else if (NodeType== XmlNodeType.Text) 
                        {


                            if (!ignore) textIndex += readerValue.Split().Count(x => !String.IsNullOrWhiteSpace(x));
                            if (ignore) Debug.WriteLine($"Ignoring: {readerValue}");
                            //textIndex += readerValue.Split().Count(x => !String.IsNullOrWhiteSpace(x));
                        }

                    }
                }
                Func<XObject, XParticle, bool> defaultFunc;
                if (ParticleSet.Count == 0 && ActionDict.TryGetValue("DEFAULT", out defaultFunc!)) 
                {
                    ParticleSet.Add(new XParticle(XElement.Load(file), defaultFunc));
                }
                return ParticleSet;
            }
            catch (XmlException e)
            {
                throw new XmlException($"Pandora: {e.Message} in file {file}");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
