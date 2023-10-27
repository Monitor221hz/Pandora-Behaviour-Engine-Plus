using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.AnimData;

public class MotionData
{
	public List<MotionDataBlock> Blocks { get; set; } = new List<MotionDataBlock>();
	public Dictionary<int, MotionDataBlock> BlocksByID { get; set; } = new Dictionary<int, MotionDataBlock>();
	public static MotionData ReadProject(StreamReader reader, int lineLimit)
	{
		MotionData project = new MotionData();
		int i = 1; //+1 to account for 1 empty line 
		string whiteSpace = "";

		while (whiteSpace != null && i < lineLimit)
		{
			MotionDataBlock block = MotionDataBlock.ReadBlock(reader);
			project.Blocks.Add(block);
			project.BlocksByID.Add(Int32.Parse(block.ClipID), block);
			i += block.GetLineCount();

			whiteSpace = reader.ReadLine();
			i++;

		}
		return project;
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendJoin("\r\n", Blocks);
		return sb.ToString();
		//byte[] bytes = Encoding.Default.GetBytes(sb.ToString());
		//return Encoding.UTF8.GetString(bytes);
	}
	public int GetLineCount()
	{
		int i = 0;
		foreach (MotionDataBlock block in Blocks)
		{
			i += block.GetLineCount();
			i++;
		}
		return i;
	}

}
