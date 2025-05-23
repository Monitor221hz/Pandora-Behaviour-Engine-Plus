using Pandora.Models.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pandora.Models.Patch.Skyrim64.AnimSetData;

public class SetAttackEntry
{
	public string AttackTrigger { get; private set; } = "attackStart";

	public int Unk { get; private set; } = 0;

	public int NumClips { get; private set; } = 1;

	public List<string> ClipNames { get; private set; } = ["attackClip"];

	public static SetAttackEntry ReadEntry(StreamReader reader)
	{
		SetAttackEntry entry = new()
		{
			AttackTrigger = reader.ReadLineSafe()
		};

		if (!int.TryParse(reader.ReadLineSafe(), out int unk) || !int.TryParse(reader.ReadLineSafe(), out int numClips)) return entry;
		entry.NumClips = numClips;
		entry.Unk = unk;

		if (numClips > 0) { entry.ClipNames = []; }
		for (int i = 0; i < numClips; i++)
		{
			entry.ClipNames.Add(reader.ReadLineSafe());
		}


		return entry;
	}

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