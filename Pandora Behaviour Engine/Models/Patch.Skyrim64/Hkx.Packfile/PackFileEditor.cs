// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Pandora.API.Patch.Skyrim64;
using Pandora.Models.Extensions;
using XmlCake.Linq;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

public partial class PackFileEditor
{
	private static string NormalizeStringValue(string value)
	{
		int start = 0;
		int end = value.Length - 1;

		while (start <= end && char.IsWhiteSpace(value[start]))
			start++;

		while (end >= start && char.IsWhiteSpace(value[end]))
			end--;

		if (start > end)
			return string.Empty;
		StringBuilder stringBuilder = new StringBuilder();
		bool isInWhitespaceRun = false;

		for (int i = start; i <= end; i++)
		{
			if (char.IsWhiteSpace(value[i]))
			{
				if (!isInWhitespaceRun)
				{
					stringBuilder.Append(' ');
					isInWhitespaceRun = true;
				}
			}
			else
			{
				stringBuilder.Append(value[i]);
				isInWhitespaceRun = false;
			}
		}
		return stringBuilder.ToString();
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

		var source = element.Value;
		if (string.IsNullOrWhiteSpace(source) || skipChars >= source.Length)
		{
			return false;
		}
		var tail = source.AsSpan(skipChars);
		var head = source.AsSpan(0, skipChars);

		oldValue = NormalizeStringValue(oldValue);
		newValue = NormalizeStringValue(newValue);

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
		string source = element.Value;

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
		newValue = NormalizeStringValue(newValue);
		source = String.Concat(head, newValue, " ".AsSpan(), tail);
		element.SetValue(source);
		return true;
	}

	public static bool AppendText(IXMap xmap, string path, string newValue)
	{
		if (!xmap.PathExists(path))
		{
			return false;
		}
		XElement element = xmap.NavigateTo(path);
		string source = element.Value;
		if (string.IsNullOrEmpty(newValue))
		{
			return false;
		}

		newValue = NormalizeStringValue(newValue);
		source = string.Concat(source, " ", newValue, " ");
		element.SetValue(source);

		return true;
	}

	public static bool RemoveText(IXMap xmap, string path, string value, int findFrom)
	{
		XElement element = xmap.NavigateTo(path);
		if (string.IsNullOrWhiteSpace(value))
			return false;
		string source = element.Value;
		if (string.IsNullOrWhiteSpace(source) || findFrom >= source.Length)
		{
			return false;
		}
		var head = source.AsSpan(0, findFrom);
		var tail = source.AsSpan(findFrom);
		value = NormalizeStringValue(value);
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
