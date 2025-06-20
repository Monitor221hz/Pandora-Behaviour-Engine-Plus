using Pandora.Models.Extensions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Pandora.Models.Patch.Skyrim64.AnimSetData;

public class AnimSet
{
	public AnimSet(string versionName, int numTriggers, int numConditions, int numAttackEntries, int numAnimationInfos, IList<string> triggers, IList<SetCondition> conditions, IList<SetAttackEntry> attackEntries, IList<SetCachedAnimInfo> animInfos)
	{
		VersionName = versionName;
		NumTriggers = numTriggers;
		NumConditions = numConditions;
		NumAttackEntries = numAttackEntries;
		NumAnimationInfos = numAnimationInfos;
		Triggers = triggers;
		Conditions = conditions;
		AttackEntries = attackEntries;
		AnimInfos = animInfos;
	}

	public string VersionName { get; private set; } = "V3";
	public int NumTriggers { get; private set; } = 0;

	public int NumConditions { get; private set; } = 0;

	public int NumAttackEntries { get; private set; } = 0;

	public int NumAnimationInfos { get; private set; } = 0;

	public IList<string> Triggers { get; private set; } = [];

	public IList<SetCondition> Conditions { get; private set; } = [];

	public IList<SetAttackEntry> AttackEntries { get; private set; } = [];

	public IList<SetCachedAnimInfo> AnimInfos { get; private set; } = [];

	public void AddAnimInfo(SetCachedAnimInfo animInfo) => AnimInfos.Add(animInfo);

	public static bool TryRead(StreamReader reader, [NotNullWhen(true)] out AnimSet? animSet)
	{
		animSet = null;
		if (!reader.TryReadLine(out var versionName)) { return false; }

		if (!int.TryParse(reader.ReadLineOrEmpty(), out int numTriggers)) { return false; }
		string[] triggers = new string[numTriggers];
		for (int i = 0; i < numTriggers; i++)
		{
			if (!reader.TryReadLine(out var value)) { return false; }
			triggers[i] = value;
		}

		if (!int.TryParse(reader.ReadLineOrEmpty(), out int numConditions)) { return false; }
		List<SetCondition> conditions = new(numConditions);
		for (int i = 0; i < numConditions; i++)
		{
			if (!SetCondition.TryRead(reader, out var value)) { return false; }
			conditions.Add(value);
		}

		if (!int.TryParse(reader.ReadLineOrEmpty(), out int numAttacks)) { return false; }
		List<SetAttackEntry> attacks = new(numAttacks);
		for (int i = 0; i < numAttacks; i++)
		{
			if (!SetAttackEntry.TryRead(reader, out var value)) { return false; }
			attacks.Add(value);
		}

		if (!int.TryParse(reader.ReadLineOrEmpty(), out int numAnimationInfos)) { return false; }
		List<SetCachedAnimInfo> animationInfos = new(numAnimationInfos);
		for (int i = 0; i < numAnimationInfos; i++)
		{
			if (!SetCachedAnimInfo.TryRead(reader, out var value)) { return false; }
			animationInfos.Add(value);
		}

		animSet = new(versionName, numTriggers, numConditions, numAttacks, numAnimationInfos, triggers, conditions, attacks, animationInfos);
		return true;
	}
	private void SyncCounts()
	{
		NumTriggers = Triggers.Count;
		NumConditions = Conditions.Count;
		NumAttackEntries = AttackEntries.Count;
		NumAnimationInfos = AnimInfos.Count;
	}
	public override string ToString()
	{
		SyncCounts();

		StringBuilder sb = new();

		sb.AppendLine(VersionName);

		sb.AppendLine(NumTriggers.ToString());
		if (NumTriggers > 0) { sb.AppendJoin("\r\n", Triggers).AppendLine(); }

		sb.AppendLine(NumConditions.ToString());
		if (NumConditions > 0) { sb.AppendJoin("", Conditions); }

		sb.AppendLine(NumAttackEntries.ToString());
		if (NumAttackEntries > 0) { sb.AppendJoin("\r\n", AttackEntries).AppendLine(); }



		sb.AppendLine(NumAnimationInfos.ToString());
		if (NumAnimationInfos > 0) { sb.AppendJoin("", AnimInfos); }

		return sb.ToString();
	}
}