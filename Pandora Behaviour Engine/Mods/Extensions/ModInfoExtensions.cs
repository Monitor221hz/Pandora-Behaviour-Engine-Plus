// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.API.Patch;
using System;

namespace Pandora.Mods.Extensions;

public static class ModInfoExtensions
{
	private const string PandoraCode = "pandora";

	public static bool IsPandora(this IModInfo mod) => string.Equals(mod.Code, PandoraCode, StringComparison.OrdinalIgnoreCase);
	public static bool IsPandora(this string? code) => string.Equals(code, PandoraCode, StringComparison.OrdinalIgnoreCase);

}
