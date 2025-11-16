// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Xml.Linq;
using Pandora.API.Patch;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Changes;

public interface IPackFileChangeOwner
{
	IModInfo Origin { get; set; }
	void AddChange(IPackFileChange change);
	void AddElementAsChange(XElement element);
}
