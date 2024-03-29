using HKX2;
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
	public class DebugPackFileExporter : Exporter<PackFile>
	{
		public DirectoryInfo ExportDirectory { get; set; }
        public DebugPackFileExporter()
        {
			ExportDirectory = new DirectoryInfo(Path.Join(Directory.GetCurrentDirectory()));
        }
        public bool Export(PackFile packFile)
		{
			var outputHandle = new FileInfo(Path.Join(ExportDirectory.FullName, Path.GetRelativePath(Directory.GetCurrentDirectory(), packFile.InputHandle.FullName.Replace("Pandora_Engine\\Skyrim\\Template", "meshes", StringComparison.OrdinalIgnoreCase))));

			if (outputHandle.Directory == null) return false;
			if (!outputHandle.Directory.Exists) { outputHandle.Directory.Create(); }
			if (outputHandle.Exists) { outputHandle.Delete(); }
			HKXHeader header = HKXHeader.SkyrimSE();
			IHavokObject rootObject;
			var debugOuputHandle = new FileInfo(outputHandle.DirectoryName + "\\pre_" + outputHandle.Name + ".xml");

			using (var writeStream = debugOuputHandle.Create())
			{
				packFile.Map.Save(writeStream);
			}
			using (var memoryStream = new MemoryStream())
			{
				packFile.Map.Save(memoryStream);
				memoryStream.Position = 0;
				var deserializer = new XmlDeserializer();
				rootObject = deserializer.Deserialize(memoryStream, header, false);
			}
			using (var writeStream = outputHandle.Create())
			{
				var binaryWriter = new BinaryWriterEx(writeStream);
				var serializer = new PackFileSerializer();
				serializer.Serialize(rootObject, binaryWriter, header);
			}
			debugOuputHandle = new FileInfo(outputHandle.FullName + ".xml");

			using (var writeStream = debugOuputHandle.Create())
			{

				var xmlSerializer = new HKX2.XmlSerializer();
				xmlSerializer.Serialize(rootObject, header, writeStream);
			}

			return true;
		}

		public PackFile Import(FileInfo file)
		{
			throw new NotImplementedException();
		}
	}
}
