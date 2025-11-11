// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using HKX2E;
using NLog;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

namespace Pandora.Models.Patch.IO.Skyrim64;

public class PackFileExporter : BasePackFileExporter
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
			Logger.Fatal(
				$"Export > {packFile.ParentProject?.Identifier}~{packFile.Name} > FAILED > {ex}"
			);
			using (var writeStream = outputHandle.Create())
			{
				var serializer = new HavokXmlSerializer();
				serializer.Serialize(packFile.Container, header, writeStream);
			}
			return false;
		}
		return true;
	}
}
