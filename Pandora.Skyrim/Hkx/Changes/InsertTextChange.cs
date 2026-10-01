// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Xml;
using System.Xml.Linq;
using Pandora.API.Patch.Skyrim64;
using Pandora.Skyrim.Format.Pandora;
using Pandora.Skyrim.Hkx.Packfile;

namespace Pandora.Skyrim.Hkx.Changes;

public class InsertTextChange : IPackFileChange
{
	public IPackFileChange.ChangeType Type { get; } = IPackFileChange.ChangeType.Insert;

	public XmlNodeType AssociatedType { get; } = XmlNodeType.Text;
	public string Target { get; }
	public string Path { get; private set; }
	private readonly int _index;
	private readonly string _value;

	public InsertTextChange(string target, string path, int index, string value)
	{
		Target = target;
		Path = path;
		_index = index;
		_value = value;
	}

	public bool Apply(IPackFile packFile)
	{
		if (!packFile.TryGetXMap(Target, out var xmap))
		{
			return false;
		}
		return PackFileEditor.InsertText(xmap!, Path, _index, _value);
	}

	public XElement AsPandoraEdit()
	{
		return new XElement(
			Type.ToString(),
			_value,
			new XAttribute(PandoraParser.PATH, Path),
			new XAttribute(PandoraParser.INDEX, _index)
		);
	}
}
