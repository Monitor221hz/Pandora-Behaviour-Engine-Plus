using Pandora.API.Patch.Engine.Skyrim64.AnimData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.AnimData
{


	public class ProjectAnimData : IProjectAnimData
	{
		public ProjectAnimDataHeader Header { get; private set; } = new ProjectAnimDataHeader();
		public IProjectAnimDataHeader GetHeader() => Header;
		public List<ClipDataBlock> Blocks { get; set; } = new List<ClipDataBlock>();
		public List<IClipDataBlock> GetBlocks() => Blocks.Cast<IClipDataBlock>().ToList();
		public MotionData? BoundMotionDataProject { get; set; }
		public IMotionData? GetBoundMotionData() => BoundMotionDataProject; 
		private AnimDataManager manager { get; set; }

		private HashSet<string> dummyClipNames { get; set; } = new HashSet<string>();
		public ProjectAnimData(AnimDataManager manager)
		{
			this.manager = manager;
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

		public static ProjectAnimData ReadProject(StreamReader reader, int lineLimit, AnimDataManager manager)
		{
			ProjectAnimData project = new ProjectAnimData(manager);

			project.Header = ProjectAnimDataHeader.ReadBlock(reader);

			int i = project.Header.GetLineCount() + 1; //+1 to account for 1 empty line 
			string? whiteSpace = "";

			while (whiteSpace != null && i < lineLimit)
			{
				ClipDataBlock block = ClipDataBlock.ReadBlock(reader);
				project.Blocks.Add(block);
				i += block.GetLineCount();

				whiteSpace = reader.ReadLine();
				i++;
			}
			return project;
		}
		public static ProjectAnimData ReadProject(StreamReader reader, AnimDataManager manager)
		{
			ProjectAnimData project = new ProjectAnimData(manager);
			project.Header = ProjectAnimDataHeader.ReadBlock(reader);

			string? whiteSpace = "";
			while (whiteSpace != null)
			{
				ClipDataBlock block = ClipDataBlock.ReadBlock(reader);
				project.Blocks.Add(block);

				whiteSpace = reader.ReadLine();
			}
			return project;
		}
		public static ProjectAnimData ExtractProject(StreamReader reader, string openString, string closeString, AnimDataManager manager)
		{
			while (reader.ReadLine()?.Contains(openString) == false) ;

			ProjectAnimData project = new ProjectAnimData(manager);
			project.Header = ProjectAnimDataHeader.ReadBlock(reader);

			string? whiteSpace = "";
			while (whiteSpace != null && !whiteSpace.Contains(closeString))
			{
				ClipDataBlock block = ClipDataBlock.ReadBlock(reader);
				project.Blocks.Add(block);

				whiteSpace = reader.ReadLineSafe();
			}
			return project;
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
