// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Pandora.API.Patch.Skyrim64.AnimData;

namespace Pandora.Models.Patch.Skyrim64.AnimData
{
	public class ProjectAnimData : IProjectAnimData
	{
		public IProjectAnimDataHeader Header { get; private set; }

		private readonly List<IClipDataBlock> _blocks = new List<IClipDataBlock>();

		public IMotionData? BoundMotionDataProject { get; set; }

		private IAnimDataManager _manager;

		private HashSet<string> dummyClipNames = new HashSet<string>();

		public ProjectAnimData(IAnimDataManager manager)
		{
			this._manager = manager;
		}

		public ProjectAnimData(
			ProjectAnimDataHeader header,
			List<IClipDataBlock> blocks,
			IAnimDataManager manager
		)
		{
			Header = header;
			_blocks = blocks;
			this._manager = manager;
		}

		public IEnumerable<string> GetClipIDs() => _blocks.ConvertAll(b => b.ClipID);

		public void AddClipData(IClipDataBlock dataBlock, IClipMotionDataBlock motionDataBlock)
		{
			var id = _manager.GetNextValidID().ToString();
			dataBlock.ClipID = id;
			motionDataBlock.ClipID = id;

			AddClipData(dataBlock);

			BoundMotionDataProject?.AddClipMotionData(motionDataBlock);
		}

		public void AddClipData(IClipDataBlock dataBlock)
		{
			lock (_blocks)
			{
				_blocks.Add(dataBlock);
			}
		}

		public void AddDummyClipData(string clipName)
		{
			lock (dummyClipNames)
			{
				if (dummyClipNames.Contains(clipName))
					return;
			}

			var id = _manager.GetNextValidID().ToString();
			_blocks.Add(new ClipDataBlock(clipName, id));

			BoundMotionDataProject?.AddDummyClipMotionData(id);
			lock (dummyClipNames)
			{
				dummyClipNames.Add(clipName);
			}
		}

		public static bool TryReadProject(
			StreamReader reader,
			IAnimDataManager manager,
			int lineLimit,
			[NotNullWhen(true)] out IProjectAnimData? projectAnimData
		)
		{
			projectAnimData = null;
			if (!ProjectAnimDataHeader.TryReadBlock(reader, out var header))
			{
				return false;
			}

			lineLimit -= header.GetLineCount() + 1;
			string? whiteSpace = "";
			List<IClipDataBlock> blocks = [];
			while (whiteSpace != null && lineLimit > 0)
			{
				if (!ClipDataBlock.TryReadBlock(reader, out var block))
				{
					break;
				}
				blocks.Add(block);
				lineLimit -= block.GetLineCount();

				whiteSpace = reader.ReadLine();
				lineLimit--;
			}
			projectAnimData = new ProjectAnimData(header, blocks, manager);

			return true;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(Header.ToString());
			if (_blocks.Count > 0)
			{
				sb.AppendJoin("\r\n", _blocks);
			}
			return sb.ToString();
			//byte[] bytes = Encoding.Default.GetBytes(sb.ToString());
			//return Encoding.UTF8.GetString(bytes);
		}

		public int GetLineCount()
		{
			int i = Header.GetLineCount() + 1;
			foreach (ClipDataBlock block in _blocks)
			{
				i += block.GetLineCount();
				i++;
			}
			return i;
		}
	}
}
