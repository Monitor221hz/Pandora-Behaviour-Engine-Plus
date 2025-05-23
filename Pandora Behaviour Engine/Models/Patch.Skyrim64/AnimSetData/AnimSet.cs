using Pandora.Models.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pandora.Models.Patch.Skyrim64.AnimSetData;

public class AnimSet
{
	public string VersionName { get; private set; } = "V3";
	public int NumTriggers { get; private set; } = 0;

	public int NumConditions { get; private set; } = 0;

	public int NumAttackEntries { get; private set; } = 0;

	public int NumAnimationInfos { get; private set; } = 0;

	public List<string> Triggers { get; private set; } = [];

	public List<SetCondition> Conditions { get; private set; } = [];

	public List<SetAttackEntry> AttackEntries { get; private set; } = [];

	public List<SetCachedAnimInfo> AnimInfos { get; private set; } = [];

	public void AddAnimInfo(SetCachedAnimInfo animInfo) => AnimInfos.Add(animInfo);

	public static AnimSet Read(StreamReader reader)
	{
		var animSet = new AnimSet
		{
			VersionName = reader.ReadLineSafe()
		};


		if (!int.TryParse(reader.ReadLineSafe(), out int numTriggers)) return animSet;
		for (int i = 0; i < numTriggers; i++) { animSet.Triggers.Add(reader.ReadLineSafe()); }

		if (!int.TryParse(reader.ReadLineSafe(), out int numConditions)) return animSet;
		for (int i = 0; i < numConditions; i++) { animSet.Conditions.Add(SetCondition.ReadCondition(reader)); }

		if (!int.TryParse(reader.ReadLineSafe(), out int numAttacks)) return animSet;
		for (int i = 0; i < numAttacks; i++) { animSet.AttackEntries.Add(SetAttackEntry.ReadEntry(reader)); }

		if (!int.TryParse(reader.ReadLineSafe(), out int numAnimationInfos)) return animSet;
		for (int i = 0; i < numAnimationInfos; i++) { animSet.AnimInfos.Add(SetCachedAnimInfo.Read(reader)); }

		animSet.SyncCounts();

		return animSet;
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