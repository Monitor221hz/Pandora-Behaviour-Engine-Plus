// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.API.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using System.Xml;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Changes;

public class ReplaceTextChange : IPackFileChange
{
	public IPackFileChange.ChangeType Type { get; } = IPackFileChange.ChangeType.Replace;

	public XmlNodeType AssociatedType { get; } = XmlNodeType.Text;

	public string Target { get; }
	public string Path { get; private set; }

	private readonly string _oldValue;

	private readonly string _newValue;

	private readonly string _preValue;

	public ReplaceTextChange(
		string target,
		string path,
		string preValue,
		string oldValue,
		string newValue
	)
	{
		Target = target;
		Path = path;
		_oldValue = oldValue;
		_newValue = newValue;
		_preValue = preValue;
	}

	public bool Apply(IPackFile packFile)
	{
		if (!packFile.TryGetXMap(Target, out var xmap))
		{
			return false;
		}
		return PackFileEditor.ReplaceText(xmap!, Path, _preValue, _oldValue, _newValue);
	}

	public bool Revert(PackFile packFile)
	{
		//PackFileEditor.ReplaceText(packFile, Path, newValue, oldValue);
		return true;
	}
}