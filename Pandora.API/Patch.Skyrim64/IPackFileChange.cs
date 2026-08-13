// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Xml;
using System.Xml.Linq;

namespace Pandora.API.Patch.Skyrim64;

public interface IPackFileChange
{
	public enum ChangeType
	{
		Remove,
		Insert,
		Replace,
		Append,
	}

	public bool Apply(IPackFile packFile);
	public ChangeType Type { get; }
	public XmlNodeType AssociatedType { get; }
	public string Path { get; }
	public string Target { get; }

	public XElement AsPandoraEdit();
}
