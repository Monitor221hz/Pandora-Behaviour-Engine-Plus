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
using HKX2;
using NLog;
using NLog.LayoutRenderers.Wrappers;
using System.Xml.Serialization;
using System.Threading;
using Pandora.Core.Patchers.Skyrim;

namespace Pandora.Patch.Patchers.Skyrim.Hkx
{
	public class PackFile : IPatchFile
	{
		public XMap Map { get; private set; }

		public static readonly string ROOT_CONTAINER_NAME = "__data__";

		public string Name { get; private set; }
		public FileInfo InputHandle { get; private set;  }

		public FileInfo OutputHandle {  get; private set; }

		public static bool DebugFiles { get; set; } = false;

		public PackFileEditor Editor {  get; private set; } = new PackFileEditor();

		public XElement ContainerNode { get; private set; } 

		public PackFileDispatcher edits { get; private set; } = new PackFileDispatcher();

		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

		public void ApplyChanges() => edits.ApplyChanges(this);

		private HashSet<string> mappedNodeNames = new HashSet<string>();

		public Project? ParentProject { get; set; }


		
		public PackFile(FileInfo file)
		{
			InputHandle = file;
			OutputHandle = new FileInfo(file.FullName.Replace("Template", "meshes").Replace("\\Pandora_Engine\\Skyrim",""));
			using (var stream = file.OpenRead())
			{
				Map = XMap.Load(stream);
			}

			ContainerNode = Map.NavigateTo("__data__");
			Name = Path.GetFileNameWithoutExtension(InputHandle.Name).ToLower();
		}



		public PackFile(string filePath) : this(new FileInfo(filePath)) { }


		public XElement SafeNavigateTo(string path) => Map.NavigateTo(path, ContainerNode); 
		public XElement GetNodeByClass(string className) => Map.NavigateTo(className, ContainerNode, (x) => XMap.TryGetAttributeName("class", x));

		public void DeleteExistingOutput()
		{
			if (OutputHandle.Exists)
			{
				OutputHandle.Delete();	
			}
		}

		public void MapNode(string nodeName)
		{
			if (mappedNodeNames.Contains(nodeName)) return; 
			mappedNodeNames.Add(nodeName);
			Map.MapSlice(nodeName);
		}

		public void Export()
		{
			if (OutputHandle.Directory == null) return; 
			if (!OutputHandle.Directory.Exists) { OutputHandle.Directory.Create(); }
			if (OutputHandle.Exists) { OutputHandle.Delete();  }
			using (var writeStream = OutputHandle.OpenWrite())
			{
				Map.Save(writeStream);
			}
			HKXHeader header = HKXHeader.SkyrimSE();
			IHavokObject rootObject;
			try
			{
			using (var readStream = OutputHandle.OpenRead())
			{
				var deserializer = new XmlDeserializer();
				rootObject = deserializer.Deserialize(readStream, header, false);
			}
			using (var writeStream = OutputHandle.Create())
			{
				var binaryWriter = new BinaryWriterEx(writeStream);
				var serializer = new PackFileSerializer();
				serializer.Serialize(rootObject, binaryWriter, header);
			}
				if (DebugFiles)
				{
					var debugOuputHandle = new FileInfo(OutputHandle.FullName + ".xml");
					if (debugOuputHandle.Exists) { debugOuputHandle.Delete(); }
					using (var writeStream = debugOuputHandle.Create())
					{

						var xmlSerializer = new HKX2.XmlSerializer();
						xmlSerializer.Serialize(rootObject, header, writeStream);
					}

				}

			}
			catch(Exception ex) 
			{
				Logger.Fatal($"Export > {ParentProject?.Identifier}~{Name} > FAILED > {ex.Message}");
				using (var writeStream = OutputHandle.Create())
				{
					Map.Save(writeStream);
				}
			}

			
#if DEBUG
			//if (debugOuputHandle.Exists) { debugOuputHandle.Delete(); }
			//using (var readStream = OutputHandle.OpenRead())
			//{
			//	var binaryReader = new BinaryReaderEx(readStream);
			//	var deserializer = new PackFileDeserializer();
			//	rootObject = deserializer.Deserialize(binaryReader, false);
			//}
			//using (var writeStream = debugOuputHandle.Create())
			//{
			//	var xmlSerializer = new HKX2.XmlSerializer();
			//	xmlSerializer.Serialize(rootObject, header, writeStream);
			//}
#endif


		}











	}





}
