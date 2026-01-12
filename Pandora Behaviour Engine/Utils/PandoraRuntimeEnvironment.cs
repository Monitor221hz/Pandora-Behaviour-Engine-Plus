// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;

namespace Pandora.Utils;

public sealed class PandoraRuntimeEnvironment : IRuntimeEnvironment
{
	public string CurrentDirectory => Environment.CurrentDirectory;
}
