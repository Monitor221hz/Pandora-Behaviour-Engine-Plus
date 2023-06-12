using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace Pandora.Patch
{
    [XmlRoot("ProjectDefinition")]
    public class ProjectDefinition
    {
        [XmlAttribute("Name")]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute("Root")]
        public string Root { get; set; } = "meshes\\actors";

        [XmlAttribute("Template")]
        public string Template { get; set; } = "Pandora_Engine\\Templates";


        [XmlElement]
        public string ProjectFile { get; set; } = "Default.hkx";


        [XmlElement("BehaviorFolder")]
        public string BehaviorFolder { get; set; }= string.Empty;

        [XmlElement("CharacterFolder")]
        public string CharacterFolder { get; set; }= string.Empty;

        [XmlElement("SkeletonFolder")]
        public string SkeletonFolder { get; set; }= string.Empty;



        [XmlIgnore]
        public HashSet<PackFile> PackFiles { get; set; } = new HashSet<PackFile> { };




        public void LoadPackFiles(string parentPath, Func<PackFile, bool> validateFunc)
        {

            Root = Path.Combine(parentPath, Root);
            Template = Path.Combine(parentPath,Template);
            BehaviorFolder= Path.Combine(Template,BehaviorFolder);
            CharacterFolder = Path.Combine(Template, CharacterFolder);
            SkeletonFolder= Path.Combine(Template,SkeletonFolder);
            
            PackFiles =
                Directory.GetFiles(BehaviorFolder)
                .Concat(Directory.GetFiles(CharacterFolder))
                .Concat(Directory.GetFiles(SkeletonFolder))
                .Select(x =>  new PackFile(x, x.Replace(Template, Root), Name,validateFunc)).ToHashSet();
            
        }


        //[XmlElement("KeyDefinitions")]
        //public ProjectKeyFileSet KeySet { get; set; } = new ProjectKeyFileSet();
    }
}
