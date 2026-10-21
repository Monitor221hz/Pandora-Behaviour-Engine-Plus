// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Text;

namespace Pandora.Core.Extensions;

public static class StringBuilderExtensions
{
	public const string Crlf = "\r\n";

	/// <summary>
	/// Like <see cref="StringBuilder.AppendLine(string)"/>, but always terminates with CRLF instead of
	/// <see cref="System.Environment.NewLine"/>, as Skyrim's anim data files require.
	/// </summary>
	public static StringBuilder AppendCrlfLine(this StringBuilder sb, string? value = null) =>
		sb.Append(value).Append(Crlf);
}
