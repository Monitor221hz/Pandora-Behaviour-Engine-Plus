// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Xml;
using System.Xml.Linq;
using Pandora.API.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Changes;

public class InsertElementChange : IPackFileChange
{
	public IPackFileChange.ChangeType Type { get; } = IPackFileChange.ChangeType.Insert;

	public XmlNodeType AssociatedType { get; } = XmlNodeType.Element;

	public string Target { get; }
	public string Path { get; private set; }
	private XElement element { get; set; }

	public InsertElementChange(string target, string path, XElement element)
	{
		Target = target;
		Path = path;
		this.element = element;
	}

	public bool Apply(IPackFile packFile)
	{
		if (!packFile.TryGetXMap(Target, out var xmap))
		{
			return false;
		}
		string newPath = PackFileEditor.InsertElement(xmap!, Path, element);
		Path = string.IsNullOrEmpty(newPath) ? Path : newPath;
		return xmap!.PathExists(Path);
	}
}
