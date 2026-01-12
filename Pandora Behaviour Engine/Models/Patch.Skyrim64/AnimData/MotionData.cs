// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using Pandora.API.Patch.Skyrim64.AnimData;

namespace Pandora.Models.Patch.Skyrim64.AnimData;

public class MotionData : IMotionData
{
	public MotionData(
		IList<IClipMotionDataBlock> blocks,
		Dictionary<int, IClipMotionDataBlock> blocksByID
	)
	{
		Blocks = blocks;
		BlocksByID = blocksByID;
	}

	public IList<IClipMotionDataBlock> Blocks { get; private set; } = [];
	public Dictionary<int, IClipMotionDataBlock> BlocksByID { get; private set; } = [];

	public bool TryGetBlock(int id, [NotNullWhen(true)] out IClipMotionDataBlock? block)
	{
		block = BlocksByID.TryGetValue(id, out var exBlock)
			? exBlock as IClipMotionDataBlock
			: null;
		return block != null;
	}

	public IList<IClipMotionDataBlock> GetBlocks() => Blocks.Cast<IClipMotionDataBlock>().ToList();

	public void AddClipMotionData(IClipMotionDataBlock block)
	{
		lock (Blocks)
		{
			Blocks.Add(block);
		}
	}

	public void AddDummyClipMotionData(string id)
	{
		lock (Blocks)
		{
			Blocks.Add(new ClipMotionDataBlock(id));
		}
	}

	public static bool TryReadProject(
		StreamReader reader,
		int lineLimit,
		[NotNullWhen(true)] out IMotionData? motionData
	)
	{
		motionData = null;
		lineLimit -= 1;
		string? whitespace = string.Empty;
		List<IClipMotionDataBlock> blocks = [];
		Dictionary<int, IClipMotionDataBlock> blocksById = [];
		while (whitespace != null && lineLimit > 0)
		{
			if (
				!ClipMotionDataBlock.TryReadBlock(reader, out var block)
				|| !int.TryParse(block.ClipID, out var id)
			)
			{
				return false;
			}
			blocks.Add(block);
			blocksById.Add(id, block);
			lineLimit -= block.GetLineCount();

			whitespace = reader.ReadLine();
			lineLimit--;
		}
		motionData = new MotionData(blocks, blocksById);
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
