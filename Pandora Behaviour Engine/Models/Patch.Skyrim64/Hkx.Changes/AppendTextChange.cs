// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Xml;
using Pandora.API.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Changes;

public class AppendTextChange : IPackFileChange
{
	public IPackFileChange.ChangeType Type { get; } = IPackFileChange.ChangeType.Insert;
	public XmlNodeType AssociatedType { get; } = XmlNodeType.Text;
	public string Target { get; private set; }
	public string Path { get; private set; }
	private string value { get; set; }

	public AppendTextChange(string target, string path, string value)
	{
		Target = target;
		Path = path;
		this.value = value;
	}

	public bool Apply(IPackFile packFile)
	{
		if (!packFile.TryGetXMap(Target, out var xmap))
		{
			return false;
		}
		return PackFileEditor.AppendText(xmap!, Path, value);
	}
}
