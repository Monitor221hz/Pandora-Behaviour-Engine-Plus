using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.AnimSetData
{
    public class AnimSet
	{
		public string VersionName { get; private set; } = "V3";
		public int NumTriggers { get; private set; } = 0;

		public int NumConditions { get; private set; } = 0;

		public int NumAttackEntries { get; private set; } = 0;

		public int NumAnimationInfos { get; private set; } = 0;

		public List<string> Triggers { get; private set; } = new List<string>();

		public List<SetCondition> Conditions { get; private set; } = new List<SetCondition>();

		public List<SetAttackEntry> AttackEntries { get; private set; } = new List<SetAttackEntry>();

		public List<SetCachedAnimInfo> AnimInfos { get; private set; } = new List<SetCachedAnimInfo>();

		public void AddAnimInfo(SetCachedAnimInfo animInfo) => AnimInfos.Add(animInfo);

		public static AnimSet Read(StreamReader reader)
		{
			var animSet = new AnimSet();

			animSet.VersionName = reader.ReadLineSafe();

			int numTriggers;
			int numConditions;
			int numAttacks;
			int numAnimationInfos;

			if (!int.TryParse(reader.ReadLineSafe(), out numTriggers)) return animSet;
			for (int i = 0; i < numTriggers; i++) { animSet.Triggers.Add(reader.ReadLineSafe()); }

			if (!int.TryParse(reader.ReadLineSafe(), out numConditions)) return animSet;
			for (int i = 0; i < numConditions; i++) { animSet.Conditions.Add(SetCondition.ReadCondition(reader)); }

			if (!int.TryParse(reader.ReadLineSafe(), out numAttacks)) return animSet;
			for (int i = 0; i < numAttacks; i++) { animSet.AttackEntries.Add(SetAttackEntry.ReadEntry(reader)); }

			if (!int.TryParse(reader.ReadLineSafe(), out numAnimationInfos)) return animSet;
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

			StringBuilder sb = new StringBuilder();

			sb.AppendLine(VersionName);

			sb.AppendLine(NumTriggers.ToString());
			if (NumTriggers > 0) { sb.AppendJoin("\r\n", Triggers).AppendLine(); }

			sb.AppendLine(NumConditions.ToString());
			if (NumConditions > 0) { sb.AppendJoin("", Conditions);  }

			sb.AppendLine(NumAttackEntries.ToString());
			if (NumAttackEntries > 0) { sb.AppendJoin("\r\n", AttackEntries).AppendLine(); }

			

			sb.AppendLine(NumAnimationInfos.ToString());
			if (NumAnimationInfos > 0) { sb.AppendJoin("", AnimInfos); }

			return sb.ToString();
		}

	}
}
