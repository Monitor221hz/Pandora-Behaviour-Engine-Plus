using Pandora.API.Patch.Engine.Skyrim64.AnimData;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace Pandora.Models.Patch.Skyrim64.AnimData;

public class MotionData : IMotionData
{
	public MotionData(IList<ClipMotionDataBlock> blocks, Dictionary<int, ClipMotionDataBlock> blocksByID)
	{
		Blocks = blocks;
		BlocksByID = blocksByID;
	}

	public IList<ClipMotionDataBlock> Blocks { get; private set; } = [];
	public Dictionary<int, ClipMotionDataBlock> BlocksByID { get; private set; } = [];

	public bool TryGetBlock(int id, [NotNullWhen(true)] out IClipMotionDataBlock? block)
	{
		block = BlocksByID.TryGetValue(id, out var exBlock) ? exBlock as IClipMotionDataBlock : null;
		return block != null;
	}
	public IList<IClipMotionDataBlock> GetBlocks() => Blocks.Cast<IClipMotionDataBlock>().ToList();
	public void AddClipMotionData(ClipMotionDataBlock block)
	{
		lock (Blocks) { Blocks.Add(block); }
	}
	public void AddDummyClipMotionData(string id)
	{
		lock (Blocks) { Blocks.Add(new ClipMotionDataBlock(id)); }
	}
	public static bool TryReadProject(StreamReader reader, int lineLimit, [NotNullWhen(true)] out MotionData? motionData)
	{
		motionData = null;
		lineLimit -= 1;
		string? whitespace = string.Empty;
		List<ClipMotionDataBlock> blocks = [];
		Dictionary<int, ClipMotionDataBlock> blocksById = [];
		while (whitespace != null && lineLimit > 0)
		{
			if (!ClipMotionDataBlock.TryReadBlock(reader, out var block) || !int.TryParse(block.ClipID, out var id))
			{
				return false;
			}
			blocks.Add(block);
			blocksById.Add(id, block);
			lineLimit -= block.GetLineCount();

			whitespace = reader.ReadLine();
			lineLimit--;
		}
		motionData = new(blocks, blocksById);
		return true;
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