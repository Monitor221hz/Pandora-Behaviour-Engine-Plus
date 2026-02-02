// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Avalonia.Media;

namespace Pandora.Platform.Windows;

public interface IWindowStateService
{
	void Initialize();

    void SetVisualState(WindowVisualState state);

    void FlashWindow();

    void Shutdown();
}