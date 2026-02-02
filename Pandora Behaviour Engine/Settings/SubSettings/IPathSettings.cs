// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Settings.DTOs;
using System;
using System.IO;

namespace Pandora.Settings.SubSettings;

public interface IPathSettings : INotifySettingsChanged
{
	DirectoryInfo GameData { get; }
	DirectoryInfo Output { get; }

	bool IsGameDataValid { get; }
	bool NeedsUserSelection { get; }

	IObservable<DirectoryInfo> GameDataChanged { get; }
	IObservable<DirectoryInfo> OutputChanged { get; }

	void SetGameDataFolder(DirectoryInfo directory);
	void SetOutputFolder(DirectoryInfo directory);

	void Initialize(GameSettings gameSettings);
}