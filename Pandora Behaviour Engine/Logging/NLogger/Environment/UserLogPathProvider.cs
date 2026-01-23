// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using Pandora.Paths.Abstractions;
using System;
using System.IO;

namespace Pandora.Logging.NLogger.Environment;

public sealed class UserLogPathProvider(IUserPaths userPaths) : ILogPathProvider
{
	public DirectoryInfo Current => userPaths.Output;

	public IObservable<DirectoryInfo> Changed => userPaths.OutputChanged;
}
