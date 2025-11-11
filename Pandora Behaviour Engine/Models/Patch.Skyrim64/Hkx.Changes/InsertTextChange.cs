// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Xml;
using Pandora.Models.Patch.Skyrim64.Hkx.Changes;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Changes;

public class InsertTextChange : IPackFileChange
{
	public IPackFileChange.ChangeType Type { get; } = IPackFileChange.ChangeType.Insert;

	public XmlNodeType AssociatedType { get; } = XmlNodeType.Text;
	public string Target { get; }
	public string Path { get; private set; }
	private string markerValue;
	private string value;

	public InsertTextChange(string target, string path, string markerValue, string value)
	{
		Target = target;
		Path = path;
		this.markerValue = markerValue;
		this.value = value;
	}

	public bool Apply(PackFile packFile)
	{
		if (!packFile.TryGetXMap(Target, out var xmap))
		{
			return false;
		}
		return PackFileEditor.InsertText(xmap!, Path, markerValue, value);
	}
}
