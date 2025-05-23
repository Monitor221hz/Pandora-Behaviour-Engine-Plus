using Pandora.API.Patch.Engine.Skyrim64.AnimData;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace Pandora.Models.Patch.Skyrim64.AnimData;

public class MotionData : IMotionData
{
	public List<ClipMotionDataBlock> Blocks { get; private set; } = [];
	public Dictionary<int, ClipMotionDataBlock> BlocksByID { get; private set; } = [];

	public bool TryGetBlock(int id, [NotNullWhen(true)] out IClipMotionDataBlock? block)
	{
		block = BlocksByID.TryGetValue(id, out var exBlock) ? exBlock as IClipMotionDataBlock : null;
		return block != null;
	}
	public List<IClipMotionDataBlock> GetBlocks() => Blocks.Cast<IClipMotionDataBlock>().ToList();
	public void AddClipMotionData(ClipMotionDataBlock block)
	{
		lock (Blocks) {	Blocks.Add(block); }
	}
	public void AddDummyClipMotionData(string id)
	{
		lock (Blocks) { Blocks.Add(new ClipMotionDataBlock(id)); }
	}
	public static MotionData ReadProject(StreamReader reader, int lineLimit)
	{
		MotionData project = new();
		int i = 1; //+1 to account for 1 empty line 
		string? whiteSpace = "";

		while (whiteSpace != null && i < lineLimit)
		{
			if (!ClipMotionDataBlock.TryReadBlock(reader, out var block)) 
			{ 
				break; 
			}
			project.Blocks.Add(block);
			project.BlocksByID.Add(int.Parse(block.ClipID), block);
			i += block.GetLineCount();

			whiteSpace = reader.ReadLine();
			i++;
		}
		return project;
	}

	public override string ToString()
	{
		StringBuilder sb = new();
		sb.AppendJoin("\r\n", Blocks);
		return sb.ToString();
		//byte[] bytes = Encoding.Default.GetBytes(sb.ToString());
		//return Encoding.UTF8.GetString(bytes);
	}
	public int GetLineCount()
	{
		int i = 0;
		foreach (ClipMotionDataBlock block in Blocks)
		{
			i += block.GetLineCount();
			i++;
		}
		return i;
	}
}