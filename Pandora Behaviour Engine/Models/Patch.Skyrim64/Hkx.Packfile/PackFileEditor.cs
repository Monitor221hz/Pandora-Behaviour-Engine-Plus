// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Pandora.API.Patch.Skyrim64;
using Pandora.Models.Extensions;
using XmlCake.Linq;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

public partial class PackFileEditor
{
	private static readonly char[] trimChars = ['\t', '\r', '\n', ')', '('];

	[GeneratedRegex(@"(?:\s|\(|\))+", RegexOptions.Compiled)]
	private static partial Regex WhiteSpaceRegex { get; }

	[GeneratedRegex(@"(?:\*|\+|\?|\||\^|\.|\#)", RegexOptions.Compiled)]
	private static partial Regex EscapeRegex { get; }

	private static string NormalizeElementValue(XElement element)
	{
		var value = WhiteSpaceRegex.Replace(element.Value.Trim(trimChars), " ");
		return value;
	}

	private static string NormalizeStringValue(string value)
	{
		return WhiteSpaceRegex.Replace(value.Trim(trimChars), " ");
	}

	public static XElement ReplaceElement(IXMap xmap, string path, XElement element) =>
		xmap.ReplaceElement(path, element);

	public static string InsertElement(IXMap xmap, string path, XElement element)
	{
		return xmap.InsertElement(path, element, true);
	}

	public static string AppendElement(IXMap xmap, string path, XElement element)
	{
		return xmap.AppendElement(path, element);
	}

	public static string PushElement(IXMap xmap, string path, XElement element)
	{
		return xmap.PushElement(path, element);
	}

	public static XElement RemoveElement(IXMap xmap, string path) => xmap.RemoveElement(path);

	public static bool ReplaceText(
		IXMap xmap,
		string path,
		int skipChars,
		string oldValue,
		string newValue
	)
	{
		XElement element = xmap.NavigateTo(path);
		if (string.IsNullOrWhiteSpace(oldValue))
			return false;

		var source = NormalizeElementValue(element);
		if (string.IsNullOrWhiteSpace(source) || skipChars >= source.Length)
		{
			return false;
		}
		var tail = source.AsSpan(skipChars);
		var head = source.AsSpan(0, skipChars);

		var replaceIndex = tail.IndexOf(oldValue, StringComparison.Ordinal);
		if (replaceIndex == -1)
		{
			return false;
		}
		// A B C D E F G H I
		// 0 1 2 3 4 5 6 7 8

		// replace EF with Z
		// skipChars = 4
		// head = ABCD
		// tail = EFGHI

		// replaceIndex = 0
		source = String.Concat(
			head,
			(replaceIndex > 0 ? tail.Slice(0, replaceIndex) : string.Empty.AsSpan()),
			newValue,
			tail.Slice(replaceIndex + oldValue.Length)
		);
		element.SetValue(source);

		return true;
	}

	public static bool SetText(IXMap xmap, string path, string newValue)
	{
		XElement element = xmap.NavigateTo(path);
		element.SetValue(newValue);
		return true;
	}

	public static bool InsertText(IXMap xmap, string path, int index, string newValue)
	{
		XElement element = xmap.NavigateTo(path);
		string source = NormalizeElementValue(element);

		if (string.IsNullOrWhiteSpace(source) || index >= source.Length)
		{
			return false;
		}
		var tail = source.AsSpan(index);
		var head = source.AsSpan(0, index);
		// A B C D E F G H I
		// 0 1 2 3 4 5 6 7 8

		// insert Z at index 7 (H)
		// index = 8
		// head = ABCDEFGH
		// tail = I

		// replaceIndex = 0
		source = String.Concat(head, newValue, " ".AsSpan(), tail);
		element.SetValue(source);
		return true;
	}

	public static void AppendText(IXMap xmap, string path, string newValue)
	{
		XElement element = xmap.NavigateTo(path);
		string source = NormalizeElementValue(element);

		newValue = NormalizeStringValue(newValue);
		source = NormalizeStringValue(string.Concat(source, " ", newValue, " "));
		element.SetValue(source);
	}

	public static bool RemoveText(IXMap xmap, string path, string value, int findFrom)
	{
		XElement element = xmap.NavigateTo(path);
		if (string.IsNullOrWhiteSpace(value))
			return false;
		string source = NormalizeElementValue(element);
		if (string.IsNullOrWhiteSpace(source) || findFrom >= source.Length)
		{
			return false;
		}
		var head = source.AsSpan(0, findFrom);
		var tail = source.AsSpan(findFrom);
		var removeIndex = tail.IndexOf(value, StringComparison.Ordinal);
		if (removeIndex < 0)
		{
			return false;
		}
		source = String.Concat(
			head,
			(removeIndex > 0 ? tail.Slice(0, removeIndex) : string.Empty.AsSpan()),
			tail.Slice(removeIndex + value.Length)
		);
		element.SetValue(source);

		return true;
	}
}
