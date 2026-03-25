// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using System;
using System.IO;

namespace Pandora.Logging.NLogger.Environment;

public interface ILogPathProvider
{
	DirectoryInfo Current { get; }
	IObservable<DirectoryInfo> Changed { get; }
}
