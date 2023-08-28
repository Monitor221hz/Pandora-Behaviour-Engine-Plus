using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XmlCake.Linq;
using XmlCake.Linq.Expressions;
using XmlCake.String;

namespace Pandora.Patch.Patchers.Skyrim.Hkx
{
	public class PackFile
	{
		public XMap Map { get; private set; }

		public FileInfo Handle { get; private set;  }

		public PackFileEditor Editor {  get; private set; } = new PackFileEditor();

		public void ApplyChanges() => Editor.ApplyEdits(this); 

		private HashSet<string> mappedNodeNames = new HashSet<string>();

		public PackFile(FileInfo file)
		{
			Handle = file;
			Map = XMap.Load(file.FullName); 
		}

		public PackFile(string filePath) : this(new FileInfo(filePath)) { }

		public XElement GetNodeByClass(string className) => Map.NavigateTo(className, Map.NavigateTo("__data__"), (x) => XMap.TryGetAttributeName("class", x));

		public void MapNode(string nodeName)
		{
			if (mappedNodeNames.Contains(nodeName)) return; 
			mappedNodeNames.Add(nodeName);
			Map.MapSlice(nodeName);
		}













	}





}
