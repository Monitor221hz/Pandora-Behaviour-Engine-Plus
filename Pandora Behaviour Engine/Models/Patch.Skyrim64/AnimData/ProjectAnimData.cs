using Pandora.API.Patch.Engine.Skyrim64.AnimData;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace Pandora.Models.Patch.Skyrim64.AnimData
{


	public class ProjectAnimData : IProjectAnimData
	{
		public ProjectAnimDataHeader Header { get; private set; }
		public IProjectAnimDataHeader GetHeader() => Header;
		public IList<ClipDataBlock> Blocks { get; set; } = new List<ClipDataBlock>();
		public IList<IClipDataBlock> GetBlocks() => Blocks.Cast<IClipDataBlock>().ToList();
		public MotionData? BoundMotionDataProject { get; set; }
		public IMotionData? GetBoundMotionData() => BoundMotionDataProject;
		private AnimDataManager manager { get; set; }

		private HashSet<string> dummyClipNames = new HashSet<string>();
		public ProjectAnimData(AnimDataManager manager)
		{
			this.manager = manager;
		}

		public ProjectAnimData(ProjectAnimDataHeader header, IList<ClipDataBlock> blocks, AnimDataManager manager)
		{
			Header = header;
			Blocks = blocks;
			this.manager = manager;
		}

		public void AddClipData(ClipDataBlock dataBlock, ClipMotionDataBlock motionDataBlock)
		{
			var id = manager.GetNextValidID().ToString();
			dataBlock.ClipID = id;
			motionDataBlock.ClipID = id;

			AddClipData(dataBlock);

			BoundMotionDataProject?.AddClipMotionData(motionDataBlock);
		}

		public void AddClipData(ClipDataBlock dataBlock)
		{
			lock (Blocks) { Blocks.Add(dataBlock); }
		}
		public void AddDummyClipData(string clipName)
		{
			lock (dummyClipNames)
			{
				if (dummyClipNames.Contains(clipName)) return;
			}



			var id = manager.GetNextValidID().ToString();
			Blocks.Add(new ClipDataBlock(clipName, id));

			BoundMotionDataProject?.AddDummyClipMotionData(id);
			lock (dummyClipNames)
			{
				dummyClipNames.Add(clipName);
			}

		}
		public static bool TryReadProject(StreamReader reader, AnimDataManager manager, int lineLimit, [NotNullWhen(true)] out ProjectAnimData? projectAnimData)
		{
			projectAnimData = null;
			if (!ProjectAnimDataHeader.TryReadBlock(reader, out var header)) { return false; }

			lineLimit -= header.GetLineCount() + 1;
			string? whiteSpace = "";
			List<ClipDataBlock> blocks = [];
			while (whiteSpace != null && lineLimit > 0)
			{
				if (!ClipDataBlock.TryReadBlock(reader, out var block)) { break; }
				blocks.Add(block);
				lineLimit -= block.GetLineCount();

				whiteSpace = reader.ReadLine();
				lineLimit--;
			}
			projectAnimData = new(header, blocks, manager);

			return true;
		}
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(Header.ToString());
			if (Blocks.Count > 0)
			{
				sb.AppendJoin("\r\n", Blocks);
			}
			return sb.ToString();
			//byte[] bytes = Encoding.Default.GetBytes(sb.ToString());
			//return Encoding.UTF8.GetString(bytes);
		}
		public int GetLineCount()
		{
			int i = Header.GetLineCount() + 1;
			foreach (ClipDataBlock block in Blocks)
			{
				i += block.GetLineCount();
				i++;
			}
			return i;
		}
	}
}
