// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using System.IO;

namespace Pandora.Paths.Extensions;

public static class PathExtensions
{
	extension(string)
	{
		public static string operator /(string left, string right)
			=> Path.Combine(left, right);
	}
}
