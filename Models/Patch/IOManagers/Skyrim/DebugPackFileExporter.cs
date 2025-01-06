using HKX2E;
using Pandora.API.Patch.IOManagers;
using Pandora.Core;
using Pandora.Core.IOManagers;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlCake.Linq;

namespace Pandora.Patch.IOManagers.Skyrim
{
	public class DebugPackFileExporter : IMetaDataExporter<PackFile>
	{
		private static readonly FileInfo PreviousOutputFile = new FileInfo(Path.Combine(BehaviourEngine.AssemblyDirectory.FullName, "Pandora_Engine\\PreviousOutput.txt"));
		public DirectoryInfo ExportDirectory { get; set; }
		public DebugPackFileExporter()
		{
			ExportDirectory = new DirectoryInfo(Path.Join(Directory.GetCurrentDirectory()));
		}
		public bool Export(PackFile packFile)
		{
			var launchDirectory = BehaviourEngine.AssemblyDirectory.FullName; 

			var outputHandle = packFile.RebaseOutput(ExportDirectory);
			if (outputHandle.Directory == null) return false;
			if (!outputHandle.Directory.Exists) { outputHandle.Directory.Create(); }
			if (outputHandle.Exists) { outputHandle.Delete(); }
			HKXHeader header = HKXHeader.SkyrimSE();

			foreach (var element in packFile.IndexedElements)
			{
				FileInfo debugFile = new FileInfo(Path.Join(outputHandle.DirectoryName!, $"{packFile.Name}~{element.Attribute("name")!.Value}.xml"));
				using (var writeStream = debugFile.Create())
				{
					element.Save(writeStream);
				}
			}
			//var debugOuputHandle = new FileInfo(outputHandle.DirectoryName + "\\pre_" + outputHandle.Name + ".xml");
			//using (var writeStream = debugOuputHandle.Create())
			//{
			//	packFile.Map.Save(writeStream);
			//}
			using (var writeStream = outputHandle.Create())
			{
				var binaryWriter = new BinaryWriterEx(writeStream);
				var serializer = new PackFileSerializer();
				serializer.Serialize(packFile.Container, binaryWriter, header);
			}
			var debugOuputHandle = new FileInfo(outputHandle.FullName + ".xml");

			using (var writeStream = debugOuputHandle.Create())
			{
				packFile.Serializer.Serialize(packFile.Container, header, writeStream);
			}

			return true;
		}

		public PackFile Import(FileInfo file)
		{
			throw new NotImplementedException();
		}

		public void LoadMetaData()
		{
			if (!PreviousOutputFile.Exists) { return; }

			using (FileStream readStream = PreviousOutputFile.OpenRead())
			{
				using (StreamReader reader = new StreamReader(readStream))
				{
					string? expectedLine;
					while ((expectedLine = reader.ReadLine()) != null)
					{
						FileInfo file = new FileInfo(expectedLine);
						if (!file.Exists) { continue; }

						file.Delete();
					}
				}
			}
		}

		public void SaveMetaData(IEnumerable<PackFile> packFiles)
		{
			using (FileStream readStream = PreviousOutputFile.Create())
			{
				using (StreamWriter writer = new StreamWriter(readStream))
				{
					foreach (PackFile packFile in packFiles)
					{
						if (!packFile.ExportSuccess) { continue; }

						writer.WriteLine(packFile.OutputHandle.FullName);
					}
				}
			}
		}
	}
}
