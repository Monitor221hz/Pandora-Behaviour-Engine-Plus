// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia.Controls;

namespace Pandora.Services;

public sealed class PandoraServiceContext
{
	public Window MainWindow { get; }

	public PandoraServiceContext(
		Window mainWindow
	)
	{
		MainWindow = mainWindow;
	}
}