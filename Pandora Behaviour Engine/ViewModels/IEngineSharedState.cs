// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Models.Engine;

namespace Pandora.ViewModels;

public interface IEngineSharedState
{
	bool IsEngineRunning { get; set; }
	bool IsPreloading { get; set; }
	string SearchTerm { get; set; }
	string OutputFolderUri { get; }
	string OutputDirectoryMessage { get; }
	bool IsOutputFolderCustomSet { get; }

	EngineState EngineState { get; set; }
}
