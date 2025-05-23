using Pandora.API.Patch.Engine.Skyrim64.AnimData;
using Pandora.Models.Extensions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Pandora.Models.Patch.Skyrim64.AnimData
{

	public class ProjectAnimDataHeader : IProjectAnimDataHeader
	{
		public ProjectAnimDataHeader(int leadInt, int assetCount, IList<string> projectAssets, int hasMotionData)
		{
			LeadInt = leadInt;
			AssetCount = assetCount;
			ProjectAssets = projectAssets;
			HasMotionData = hasMotionData;
		}

		public int LeadInt { get; set; }

		public int AssetCount { get; set; }
		public IList<string> ProjectAssets { get; set; } = [];
		public int HasMotionData { get; set; }
		public static bool TryReadBlock(StreamReader reader, [NotNullWhen(true)] out ProjectAnimDataHeader? header)
		{
			header = null;
			if (!int.TryParse(reader.ReadLine(), out var leadInt) || !int.TryParse(reader.ReadLine(), out var assetCount)) { return false; }

			string[] projectAssets = new string[assetCount];
			for (int i = 0; i < assetCount; i++)
			{
				if (!reader.TryReadLine(out var value)) { continue; }
				projectAssets[i] = value;
			}

			if (!int.TryParse(reader.ReadLine(), out var hasMotionData)) { return false; }
			header = new(leadInt, assetCount, projectAssets, hasMotionData);
			return true;
		}
		public override string ToString()
		{
			StringBuilder sb = new();
			sb.AppendLine(LeadInt.ToString()).AppendLine(ProjectAssets.Count.ToString()).AppendLine(string.Join("\r\n", ProjectAssets));
			if (HasMotionData == 1)
			{
				sb.AppendLine(HasMotionData.ToString());
			}
			else
			{
				sb.Append(HasMotionData.ToString());
			}
			return sb.ToString();
		}
		public int GetLineCount()
		{
			return 2 + ProjectAssets.Count;
		}
	}
}
