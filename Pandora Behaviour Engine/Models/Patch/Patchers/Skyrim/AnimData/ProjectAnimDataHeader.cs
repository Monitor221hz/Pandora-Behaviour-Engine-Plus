using Pandora.API.Patch.Engine.Skyrim64.AnimData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.AnimData
{

	public class ProjectAnimDataHeader : IProjectAnimDataHeader
	{
		public int LeadInt { get; set; }

		public int AssetCount { get; set; }
		public List<string> ProjectAssets { get; set; } = new List<string>();
		public int HasMotionData { get; set; }

		public static ProjectAnimDataHeader ReadBlock(StreamReader reader)
		{
			ProjectAnimDataHeader header = new ProjectAnimDataHeader();
			try
			{
				int[] headerData = new int[0];

				header.LeadInt = Int32.Parse(reader.ReadLine());

				header.AssetCount = Int32.Parse(reader.ReadLine());


				for (int i = 0; i < header.AssetCount; i++)
				{
					header.ProjectAssets.Add(reader.ReadLine());

				}

				header.HasMotionData = Int32.Parse(reader.ReadLine());
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message, ex);
			}


			return header;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(LeadInt.ToString()).AppendLine(ProjectAssets.Count.ToString()).AppendLine(String.Join("\r\n", ProjectAssets));
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
