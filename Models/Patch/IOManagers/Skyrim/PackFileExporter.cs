using HKX2E;
using NLog;
using Pandora.API.Patch.IOManagers;
using Pandora.Core;
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
public class PackFileExporter : IMetaDataExporter<PackFile>
{
	private static readonly FileInfo PreviousOutputFile = new FileInfo(Path.Combine(BehaviourEngine.AssemblyDirectory.FullName, "Pandora_Engine\\PreviousOutput.txt"));
	public DirectoryInfo ExportDirectory { get; set; }


	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

	public PackFileExporter()
	{
		ExportDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
	}

	public bool Export(PackFile packFile)
	{
		var launchDirectory = BehaviourEngine.AssemblyDirectory.FullName;

		var outputHandle = packFile.RebaseOutput(ExportDirectory);
		if (outputHandle.Directory == null) return false;
		if (!outputHandle.Directory.Exists) { outputHandle.Directory.Create(); }
		if (outputHandle.Exists) { outputHandle.Delete(); }
		HKXHeader header = HKXHeader.SkyrimSE();
		IHavokObject rootObject;

		try
		{
			using (var writeStream = outputHandle.Create())
			{
				var binaryWriter = new BinaryWriterEx(writeStream);
				var serializer = new PackFileSerializer();
				serializer.Serialize(packFile.Container, binaryWriter, header);
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
