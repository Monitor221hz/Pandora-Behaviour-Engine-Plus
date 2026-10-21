// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.IO;
using System.Text;

namespace Pandora.Core.Paths;

/// <summary>
/// <see cref="Path"/> operations for in-game paths (animation names, anim set data folders), which use
/// backslash separators. Results are what <see cref="Path"/> produces on Windows, on every platform,
/// so the output (and any CRC computed from it) does not depend on where Pandora runs.
/// </summary>
public static class GamePath
{
	public const char Separator = '\\';

	/// <summary>
	/// Joins path segments with <see cref="Separator"/>, like <see cref="Path.Join(string[])"/> on Windows.
	/// </summary>
	public static string Join(params string[] paths)
	{
		var sb = new StringBuilder();
		foreach (var path in paths)
		{
			if (string.IsNullOrEmpty(path))
				continue;
			if (sb.Length > 0 && !IsSeparator(sb[^1]) && !IsSeparator(path[0]))
				sb.Append(Separator);
			sb.Append(path);
		}
		return sb.ToString();
	}

	/// <summary>
	/// Converts a path built with platform APIs (e.g. <see cref="Path.GetRelativePath"/>) to an in-game path.
	/// </summary>
	public static string FromPlatform(string path) =>
		path.Replace(Path.DirectorySeparatorChar, Separator);

	public static string? GetDirectoryName(string path) =>
		Path.GetDirectoryName(ToPlatform(path))?.Replace(Path.DirectorySeparatorChar, Separator);

	public static string GetFileNameWithoutExtension(string path) =>
		Path.GetFileNameWithoutExtension(ToPlatform(path));

	private static bool IsSeparator(char c) => c is Separator or '/';

	private static string ToPlatform(string path) =>
		path.Replace(Separator, Path.DirectorySeparatorChar);
}
