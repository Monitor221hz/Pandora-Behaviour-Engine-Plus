// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Xml;
using System.Xml.Linq;
using Pandora.API.Patch.Skyrim64;
using Pandora.Skyrim.Format.Pandora;
using Pandora.Skyrim.Hkx.Packfile;

namespace Pandora.Skyrim.Hkx.Changes;

public class AppendTextChange : IPackFileChange
{
	public IPackFileChange.ChangeType Type { get; } = IPackFileChange.ChangeType.Append;
	public XmlNodeType AssociatedType { get; } = XmlNodeType.Text;
	public string Target { get; private set; }
	public string Path { get; private set; }
	private readonly string _value;

	public AppendTextChange(string target, string path, string value)
	{
		Target = target;
		Path = path;
		_value = value;
	}

	public bool Apply(IPackFile packFile)
	{
		if (!packFile.TryGetXMap(Target, out var xmap))
		{
			return false;
		}
		return PackFileEditor.AppendText(xmap!, Path, _value);
	}

	public XElement AsPandoraEdit()
	{
		return new XElement(Type.ToString(), _value, new XAttribute(PandoraParser.PATH, Path));
	}
}
