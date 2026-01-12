// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.IO;
using System.Text.RegularExpressions;
using Pandora.API.Patch;

namespace Pandora.Models.Patch.Mod;

public partial class FNISModInfo : IModInfo
{
	private static readonly Regex whiteSpaceRegex = WhiteSpaceRegex();
	public string Name { get; set; }

	public override int GetHashCode()
	{
		return Code.GetHashCode();
	}

	public bool Equals(IModInfo? other)
	{
		return other != null && Code == other.Code && Version == other.Version;
	}

	public string Description { get; private set; } = string.Empty;

	public string Author { get; private set; } = "unknown";

	public Version Version { get; private set; } = new Version();

	public IModInfo.ModFormat Format { get; } = IModInfo.ModFormat.FNIS;

	public string URL { get; private set; } = "null";

	public string Code { get; private set; } = "n/a";

	public DirectoryInfo Folder { get; private set; }

	public bool Active { get; set; } = true;
	public uint Priority { get; set; } = 0;

	public FNISModInfo(FileInfo file)
	{
		Name = Path.GetFileNameWithoutExtension(file.Name);
		Folder = file.Directory!;
		Code = whiteSpaceRegex.Replace(Name, string.Empty);
	}

	[GeneratedRegex(@"\s+", RegexOptions.Compiled)]
	private static partial Regex WhiteSpaceRegex();
}
