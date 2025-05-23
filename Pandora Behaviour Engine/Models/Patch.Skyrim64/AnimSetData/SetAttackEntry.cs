using Pandora.Models.Extensions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Pandora.Models.Patch.Skyrim64.AnimSetData;

public class SetAttackEntry
{
	public SetAttackEntry(string attackTrigger, int unk, int numClips, IList<string> clipNames)
	{
		AttackTrigger = attackTrigger;
		Unk = unk;
		NumClips = numClips;
		ClipNames = clipNames;
	}

	public string AttackTrigger { get; private set; } = "attackStart";

	public int Unk { get; private set; } = 0;

	public int NumClips { get; private set; } = 1;

	public IList<string> ClipNames { get; private set; } = ["attackClip"];

	public static bool TryRead(StreamReader reader, [NotNullWhen(true)] out SetAttackEntry? entry)
	{
		entry = null;
		if (!reader.TryReadLine(out var attackTrigger) ||
			!int.TryParse(reader.ReadLineOrEmpty(), out int unk) ||
			!int.TryParse(reader.ReadLineOrEmpty(), out int numClips))
		{
			return false;
		}
		string[] clips = new string[numClips];
		for (int i = 0; i < numClips; i++)
		{
			if (!reader.TryReadLine(out var value)) { return false; }
			clips[i] = value;
		}

		entry = new(attackTrigger, unk, numClips, clips);
		return true;
	}
	//public static SetAttackEntry ReadEntry(StreamReader reader)
	//{
	//	SetAttackEntry entry = new()
	//	{
	//		AttackTrigger = reader.ReadLineOrEmpty()
	//	};

	//	if (!int.TryParse(reader.ReadLineOrEmpty(), out int unk) || !int.TryParse(reader.ReadLineOrEmpty(), out int numClips)) return entry;
	//	entry.NumClips = numClips;
	//	entry.Unk = unk;

	//	if (numClips > 0) { entry.ClipNames = []; }
	//	for (int i = 0; i < numClips; i++)
	//	{
	//		entry.ClipNames.Add(reader.ReadLineOrEmpty());
	//	}


	//	return entry;
	//}

	public override string ToString()
	{
		StringBuilder sb = new();

		sb.AppendLine(AttackTrigger);

		sb.AppendLine(Unk.ToString());

		if (NumClips > 0)
		{
			sb.AppendLine(NumClips.ToString());
			sb.AppendJoin("\r\n", ClipNames);
		}
		else
		{
			sb.Append(NumClips.ToString());
		}

		return sb.ToString();
	}
}