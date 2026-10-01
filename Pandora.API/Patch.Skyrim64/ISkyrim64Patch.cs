// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

namespace Pandora.API.Patch.Skyrim64;

public interface ISkyrim64Patch
{
	public RuntimeMode Mode { get; }
	public RunOrder Order { get; }
	public void Run(IProjectManager projectManager);
}
