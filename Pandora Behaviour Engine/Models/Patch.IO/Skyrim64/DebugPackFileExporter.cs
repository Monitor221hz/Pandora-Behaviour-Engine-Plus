using HKX2E;
using Pandora.API.Patch.IOManagers;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using System;
using System.Collections.Generic;
using System.IO;

namespace Pandora.Models.Patch.IO.Skyrim64;

public class DebugPackFileExporter : IMetaDataExporter<PackFile>
{
	private static readonly FileInfo PreviousOutputFile = new(Path.Combine(BehaviourEngine.AssemblyDirectory.FullName, "Pandora_Engine\\PreviousOutput.txt"));
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
			FileInfo debugFile = new(Path.Join(outputHandle.DirectoryName!, $"{packFile.Name}~{element.Attribute("name")!.Value}.xml"));
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
			using (StreamReader reader = new(readStream))
			{
				string? expectedLine;
				while ((expectedLine = reader.ReadLine()) != null)
				{
					FileInfo file = new(expectedLine);
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
			using (StreamWriter writer = new(readStream))
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
