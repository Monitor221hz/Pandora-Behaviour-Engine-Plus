// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Text;
using Pandora.Paths.Abstractions;

namespace Pandora.Logging.Diagnostics;

public sealed class CrashLogBuilder(IApplicationPaths paths)
{
	public string Build(CrashType type, string content)
	{
		var sb = new StringBuilder();

		sb.AppendLine("[ Pandora Critical Crash Log ]");
		sb.AppendLine("=======================================");
		sb.AppendLine($"Type: {type}");
		sb.AppendLine($"CurrentDirectory: {Environment.CurrentDirectory}");
		sb.AppendLine($"ExecutablePath: {paths.AssemblyDirectory?.FullName ?? "unknown"}");
		sb.AppendLine();
		sb.AppendLine(content);
		sb.AppendLine("=======================================");

		return sb.ToString();
	}
}
