// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Text;

namespace Pandora.Models.Patch.Skyrim64.Format.Nemesis;

/// <summary>
/// A condition is three lines: variable, minimum, maximum. Some Nemesis edits
/// start after the variable name, so the comment is read as one of the numbers.
/// This moves that edit onto the whole condition.
/// </summary>
public static class NemesisSetConditionFormatter
{
	private const int ConditionWidth = 3;

	public static int ReformatMod(DirectoryInfo folder)
	{
		string root = Path.Combine(folder.FullName, "animationsetdatasinglefile");
		if (!Directory.Exists(root))
		{
			return 0;
		}

		int changed = 0;
		foreach (
			string file in Directory.EnumerateFiles(root, "*.txt", SearchOption.AllDirectories)
		)
		{
			if (ReformatFile(file))
			{
				changed++;
			}
		}

		return changed;
	}

	public static bool ReformatFile(string path)
	{
		string text = File.ReadAllText(path);
		string newline = text.Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";
		bool hadTrailingNewline = text.EndsWith('\n');
		string normalized = text.Replace("\r\n", "\n", StringComparison.Ordinal);
		if (hadTrailingNewline && normalized.Length > 0)
		{
			normalized = normalized[..^1];
		}

		string[] lines = normalized.Length == 0 ? [] : normalized.Split('\n');
		if (!TryReformat(lines, out List<string>? reformatted))
		{
			return false;
		}

		string rebuilt = string.Join(newline, reformatted);
		if (hadTrailingNewline)
		{
			rebuilt += newline;
		}

		if (rebuilt == text)
		{
			return false;
		}

		File.WriteAllText(path, rebuilt, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
		return true;
	}

	public static bool TryReformat(IReadOnlyList<string> lines, out List<string> reformatted)
	{
		reformatted = [];
		if (!TryFindConditions(lines, out int count, out int bodyStart))
		{
			return false;
		}

		var rebuilt = new List<string>(lines.Count);
		for (int i = 0; i < bodyStart; i++)
		{
			rebuilt.Add(lines[i]);
		}

		int index = bodyStart;
		int consumed = 0;
		var record = new List<string>(ConditionWidth);
		bool changed = false;
		while (consumed < count && index < lines.Count)
		{
			if (IsOpen(lines[index]))
			{
				int blockStart = index;
				if (
					!TryReadBlock(
						lines,
						ref index,
						out string openLine,
						out List<string> edited,
						out List<string> original,
						out string closeLine
					)
				)
				{
					return false;
				}

				if (record.Count == 0)
				{
					AppendRange(rebuilt, lines, blockStart, index);
					int groups = original.Count > 0 ? original.Count : edited.Count;
					consumed += Math.Max(1, groups / ConditionWidth);
					continue;
				}

				if (
					!TryAlign(
						record,
						edited,
						original,
						lines,
						ref index,
						out List<string>? editedRecord,
						out List<string>? originalRecord
					)
				)
				{
					return false;
				}

				rebuilt.Add(openLine);
				rebuilt.AddRange(editedRecord);
				rebuilt.Add("<!-- ORIGINAL -->");
				rebuilt.AddRange(originalRecord);
				rebuilt.Add(closeLine);
				consumed += originalRecord.Count / ConditionWidth;
				record.Clear();
				changed = true;
				continue;
			}

			record.Add(lines[index]);
			index++;
			if (record.Count == ConditionWidth)
			{
				rebuilt.AddRange(record);
				record.Clear();
				consumed++;
			}
		}

		if (!changed)
		{
			return false;
		}

		rebuilt.AddRange(record);
		for (; index < lines.Count; index++)
		{
			rebuilt.Add(lines[index]);
		}

		reformatted = rebuilt;
		return true;
	}

	private static bool TryFindConditions(
		IReadOnlyList<string> lines,
		out int count,
		out int bodyStart
	)
	{
		count = 0;
		bodyStart = 0;
		int index = 0;
		while (index < lines.Count && (IsMarker(lines[index]) || lines[index].Trim().Length == 0))
		{
			index++;
		}

		if (index >= lines.Count || !IsVersion(lines[index]))
		{
			return false;
		}

		index++;
		if (!TrySkipCounted(lines, ref index))
		{
			return false;
		}

		if (index >= lines.Count || !int.TryParse(lines[index].Trim(), out count))
		{
			return false;
		}

		bodyStart = index + 1;
		return true;
	}

	private static bool TrySkipCounted(IReadOnlyList<string> lines, ref int index)
	{
		if (index >= lines.Count || !int.TryParse(lines[index].Trim(), out int count))
		{
			return false;
		}

		index++;
		int consumed = 0;
		while (consumed < count)
		{
			if (index >= lines.Count)
			{
				return false;
			}

			if (IsOpen(lines[index]))
			{
				if (
					!TryReadBlock(
						lines,
						ref index,
						out _,
						out List<string> edited,
						out List<string> original,
						out _
					)
				)
				{
					return false;
				}

				int items = original.Count > 0 ? original.Count : edited.Count;
				if (items == 0)
				{
					return false;
				}

				consumed += items;
				continue;
			}

			index++;
			consumed++;
		}

		return true;
	}

	private static bool TryAlign(
		List<string> prefix,
		List<string> edited,
		List<string> original,
		IReadOnlyList<string> lines,
		ref int index,
		out List<string> editedRecord,
		out List<string> originalRecord
	)
	{
		editedRecord = new List<string>(prefix);
		editedRecord.AddRange(edited);
		originalRecord = new List<string>(prefix);
		originalRecord.AddRange(original);
		if (editedRecord.Count > ConditionWidth || originalRecord.Count > ConditionWidth)
		{
			return false;
		}

		int missing = ConditionWidth - editedRecord.Count;
		if (missing != ConditionWidth - originalRecord.Count)
		{
			return false;
		}

		for (int i = 0; i < missing; i++)
		{
			if (index >= lines.Count || IsMarker(lines[index]))
			{
				return false;
			}

			string shared = lines[index];
			index++;
			editedRecord.Add(shared);
			originalRecord.Add(shared);
		}

		return editedRecord.Count == ConditionWidth && originalRecord.Count == ConditionWidth;
	}

	private static bool TryReadBlock(
		IReadOnlyList<string> lines,
		ref int index,
		out string openLine,
		out List<string> edited,
		out List<string> original,
		out string closeLine
	)
	{
		openLine = lines[index];
		index++;
		edited = [];
		original = [];
		closeLine = "<!-- CLOSE -->";
		bool inOriginal = false;
		while (index < lines.Count && !IsClose(lines[index]))
		{
			string line = lines[index];
			index++;
			if (IsOriginal(line))
			{
				inOriginal = true;
				continue;
			}

			if (inOriginal)
			{
				original.Add(line);
			}
			else
			{
				edited.Add(line);
			}
		}

		if (index >= lines.Count || !IsClose(lines[index]))
		{
			return false;
		}

		closeLine = lines[index];
		index++;
		return true;
	}

	private static void AppendRange(
		List<string> target,
		IReadOnlyList<string> lines,
		int start,
		int end
	)
	{
		for (int i = start; i < end; i++)
		{
			target.Add(lines[i]);
		}
	}

	private static bool IsVersion(string line)
	{
		string trimmed = line.Trim();
		return trimmed.Length > 1
			&& (trimmed[0] == 'V' || trimmed[0] == 'v')
			&& char.IsDigit(trimmed[1]);
	}

	private static bool IsOpen(string line)
	{
		string trimmed = line.Trim();
		return trimmed.StartsWith("<!--", StringComparison.Ordinal)
			&& trimmed.Contains("MOD_CODE", StringComparison.Ordinal)
			&& trimmed.Contains("OPEN", StringComparison.Ordinal);
	}

	private static bool IsClose(string line)
	{
		string trimmed = line.Trim();
		return trimmed.StartsWith("<!--", StringComparison.Ordinal)
			&& trimmed.Contains("CLOSE", StringComparison.Ordinal)
			&& !trimmed.Contains("MOD_CODE", StringComparison.Ordinal);
	}

	private static bool IsOriginal(string line)
	{
		string trimmed = line.Trim();
		return trimmed.StartsWith("<!--", StringComparison.Ordinal)
			&& trimmed.Contains("ORIGINAL", StringComparison.Ordinal);
	}

	private static bool IsMarker(string line) => IsOpen(line) || IsClose(line) || IsOriginal(line);
}
