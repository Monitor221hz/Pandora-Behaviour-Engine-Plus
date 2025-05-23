using Pandora.API.Patch.Engine.Skyrim64.AnimData;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pandora.Models.Patch.Skyrim64.AnimData;

public class ProjectAnimDataHeader : IProjectAnimDataHeader
{
	public int LeadInt { get; set; }

	public int AssetCount { get; set; }
	public List<string> ProjectAssets { get; set; } = [];
	public int HasMotionData { get; set; }

	public static ProjectAnimDataHeader ReadBlock(StreamReader reader)
	{
		ProjectAnimDataHeader header = new();
		try
		{

			int[] headerData = [];

			header.LeadInt = int.Parse(reader.ReadLine());

			header.AssetCount = int.Parse(reader.ReadLine());


			for (int i = 0; i < header.AssetCount; i++)
			{
				header.ProjectAssets.Add(reader.ReadLine());

			}
			header.HasMotionData = int.Parse(reader.ReadLine());
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message, ex);
		}


		return header;
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