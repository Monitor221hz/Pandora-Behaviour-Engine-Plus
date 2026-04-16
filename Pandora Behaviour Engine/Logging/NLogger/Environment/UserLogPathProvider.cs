// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.IO;
using Pandora.Paths.Abstractions;

namespace Pandora.Logging.NLogger.Environment;

public sealed class UserLogPathProvider(IUserPaths userPaths) : ILogPathProvider
{
	public DirectoryInfo Current => userPaths.Output;

	public IObservable<DirectoryInfo> Changed => userPaths.OutputChanged;
}
