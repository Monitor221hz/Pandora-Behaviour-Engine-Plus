// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Pandora.API.Patch;
using Pandora.Models.Patch.Mod;

namespace Pandora.Data;

public class PandoraModInfoProvider : FileBasedModInfoProvider
{
	protected override string InfoFileName => "info.xml";

	public override string SingleRelativePath => Path.Join("Pandora_Engine", "mod");

	protected override async Task<IModInfo?> TryParseAsync(FileInfo infoFile)
	{
		try
		{
			await using var stream = infoFile.OpenRead();
			using var reader = XmlReader.Create(stream);
			if (serializer.Deserialize(reader) is PandoraModInfo modInfo)
			{
				modInfo.FillData(infoFile.Directory!);
				return modInfo;
			}
		}
		catch (Exception ex)
		{
			Debug.Write($"Error: {ex}");
		}
		return null;
	}

	private static readonly XmlSerializer serializer = new(typeof(PandoraModInfo));
}
