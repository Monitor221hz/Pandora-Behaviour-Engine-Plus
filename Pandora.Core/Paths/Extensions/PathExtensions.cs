// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.IO;
using System.Linq;

namespace Pandora.Core.Paths.Extensions;

public static class PathExtensions
{
	extension(string)
	{
		public static string operator /(string left, string right) => Path.Join(left, right);
	}

	/// <summary>
	/// Resolves a Windows-style relative path (backslash separators, case-insensitive), as found in
	/// Havok files and the template path lists, against <paramref name="baseDirectory"/>.
	/// On case-sensitive file systems, each segment that does not exist exactly is matched
	/// ignoring case. If nothing matches, the unmatched path is returned as-is.
	/// </summary>
	public static FileInfo ResolveFile(this DirectoryInfo baseDirectory, string relativePath)
	{
		var segments = relativePath.Split(['\\', '/'], StringSplitOptions.RemoveEmptyEntries);
		var exact = new FileInfo(Path.Join([baseDirectory.FullName, .. segments]));
		if (exact.Exists)
			return exact;

		string current = baseDirectory.FullName;
		for (int i = 0; i < segments.Length; i++)
		{
			string candidate = Path.Join(current, segments[i]);
			bool isLast = i == segments.Length - 1;
			if (!(isLast ? File.Exists(candidate) : Directory.Exists(candidate)))
			{
				var match = Directory.Exists(current)
					? (
						isLast
							? Directory.EnumerateFiles(current)
							: Directory.EnumerateDirectories(current)
					).FirstOrDefault(entry =>
						string.Equals(
							Path.GetFileName(entry),
							segments[i],
							StringComparison.OrdinalIgnoreCase
						)
					)
					: null;
				if (match is null)
					return exact;
				candidate = match;
			}
			current = candidate;
		}
		return new FileInfo(current);
	}
}
