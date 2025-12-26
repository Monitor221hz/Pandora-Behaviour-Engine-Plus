// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Xml;
using Pandora.API.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Changes;

public class ReplaceTextChange : IPackFileChange
{
	public IPackFileChange.ChangeType Type { get; } = IPackFileChange.ChangeType.Replace;

	public XmlNodeType AssociatedType { get; } = XmlNodeType.Text;

	public string Target { get; }
	public string Path { get; private set; }

	private string oldValue;

	private string newValue;

	private int _skipChars;

	public ReplaceTextChange(
		string target,
		string path,
		int skipChars,
		string oldvalue,
		string newvalue
	)
	{
		Target = target;
		Path = path;
		oldValue = oldvalue;
		newValue = newvalue;
		_skipChars = skipChars;
	}

	public bool Apply(IPackFile packFile)
	{
		if (!packFile.TryGetXMap(Target, out var xmap))
		{
			return false;
		}
		return PackFileEditor.ReplaceText(xmap!, Path, _skipChars, oldValue, newValue);
	}

	public bool Revert(PackFile packFile)
	{
		//PackFileEditor.ReplaceText(packFile, Path, newValue, oldValue);
		return true;
	}
}
