// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Xml;
using Pandora.API.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Changes;

public class RemoveTextChange : IPackFileChange
{
	public IPackFileChange.ChangeType Type { get; } = IPackFileChange.ChangeType.Remove;

	public XmlNodeType AssociatedType { get; } = XmlNodeType.Text;
	public string Target { get; }

	private string _remove;
	public string Path { get; private set; }
	private int _findFrom;

	public RemoveTextChange(string target, string path, string remove, int findFrom)
	{
		Target = target;
		Path = path;
		_findFrom = findFrom;
		_remove = remove;
	}

	public bool Apply(IPackFile packFile)
	{
		if (!packFile.TryGetXMap(Target, out var xmap))
		{
			return false;
		}
		return PackFileEditor.RemoveText(xmap!, Path, _remove, _findFrom);
	}

	public bool Revert(PackFile packFile)
	{
		//PackFileEditor.InsertText(packFile, Path, value);
		return true;
	}
}
