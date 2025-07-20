using HKX2E;
using NLog;
using Pandora.API.Patch.IOManagers;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using System;
using System.Collections.Generic;
using System.IO;

namespace Pandora.Models.Patch.IO.Skyrim64;

public class PackFileExporter : IMetaDataExporter<PackFile>
{
	private static readonly FileInfo PreviousOutputFile = new(Path.Combine(Environment.CurrentDirectory, "Pandora_Engine\\PreviousOutput.txt"));
	public DirectoryInfo ExportDirectory { get; set; }


	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public PackFileExporter()
	{
		ExportDirectory = new DirectoryInfo(Environment.CurrentDirectory);
	}

	public bool Export(PackFile packFile)
	{
		var launchDirectory = Environment.CurrentDirectory;

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
			Logger.Fatal($"Export > {packFile.ParentProject?.Identifier}~{packFile.Name} > FAILED > {ex}");
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
