// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using HKX2E;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using System.IO;

namespace Pandora.Models.Patch.IO.Skyrim64;

public class DebugPackFileExporter : BasePackFileExporter
{
	public override bool Export(PackFile packFile)
	{
		var outputHandle = packFile.RebaseOutput(ExportDirectory);

		if (outputHandle.Directory == null) 
			return false;

		if (!outputHandle.Directory.Exists) 
			outputHandle.Directory.Create();

		if (outputHandle.Exists) 
			outputHandle.Delete();

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
}
