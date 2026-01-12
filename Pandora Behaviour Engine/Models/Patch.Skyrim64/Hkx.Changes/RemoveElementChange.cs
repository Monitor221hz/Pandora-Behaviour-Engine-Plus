// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Xml;
using System.Xml.Linq;
using Pandora.API.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Changes;

public class RemoveElementChange : IPackFileChange
{
	public IPackFileChange.ChangeType Type { get; } = IPackFileChange.ChangeType.Remove;

	public XmlNodeType AssociatedType { get; } = XmlNodeType.Element;

	public string Target { get; }
	public string Path { get; private set; }
	private XElement? element { get; set; }

	public RemoveElementChange(string target, string path)
	{
		Target = target;
		Path = path;
	}

	public bool Apply(IPackFile packFile)
	{
		if (!packFile.TryGetXMap(Target, out var xmap))
		{
			return false;
		}
		element = PackFileEditor.RemoveElement(xmap!, Path);
		return !xmap!.PathExists(Path);
	}
}
