// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using System;
using System.IO;

namespace Pandora.Paths.Abstractions;

public interface IUserPaths
{
	DirectoryInfo GameData { get; }
	DirectoryInfo Output { get; }

	IObservable<DirectoryInfo> GameDataChanged { get; }
	IObservable<DirectoryInfo> OutputChanged { get; }

	void SetGameData(DirectoryInfo dir);
	void SetOutput(DirectoryInfo dir);
}
