using HKX2;
using NLog;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XmlCake.Linq;

namespace Pandora.Patch.IOManagers.Skyrim;
public class PackFileExporter : Exporter<PackFile>
{
	public DirectoryInfo ExportDirectory { get; set; }


	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

	public PackFileExporter()
	{
		ExportDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
	}

	public bool Export(PackFile packFile)
	{
		var outputHandle = new FileInfo(Path.Join(ExportDirectory.FullName, Path.GetRelativePath(Directory.GetCurrentDirectory(), packFile.InputHandle.FullName.Replace("Pandora_Engine\\Skyrim\\Template", "meshes", StringComparison.OrdinalIgnoreCase))));

		if (outputHandle.Directory == null) return false;
		if (!outputHandle.Directory.Exists) { outputHandle.Directory.Create(); }
		if (outputHandle.Exists) { outputHandle.Delete(); }
		HKXHeader header = HKXHeader.SkyrimSE();
		IHavokObject rootObject;

		try
		{
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
		}
		catch (Exception ex)
		{
			Logger.Fatal($"Export > {packFile.ParentProject?.Identifier}~{packFile.Name} > FAILED > {ex.ToString()}");
			using (var writeStream = outputHandle.Create())
			{
				packFile.Map.Save(writeStream);
			}
			return false;
		}
		return true;
	}

	public PackFile Import(FileInfo file)
	{
		throw new NotImplementedException();
	}
}
