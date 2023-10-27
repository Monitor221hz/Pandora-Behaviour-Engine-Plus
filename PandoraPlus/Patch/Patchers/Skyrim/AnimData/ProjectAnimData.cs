using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.AnimData
{
	public class ProjectAnimData
	{
		public AnimDataProjectHeader Header { get; private set; }
		public List<AnimDataBlock> Blocks { get; set; } = new List<AnimDataBlock>();

		public MotionData BoundMotionDataProject { get; set; }
		public static ProjectAnimData ReadProject(StreamReader reader, int lineLimit)
		{
			ProjectAnimData project = new ProjectAnimData();

			project.Header = AnimDataProjectHeader.ReadBlock(reader);

			int i = project.Header.GetLineCount() + 1; //+1 to account for 1 empty line 
			string whiteSpace = "";

			while (whiteSpace != null && i < lineLimit)
			{
				AnimDataBlock block = AnimDataBlock.ReadBlock(reader);
				project.Blocks.Add(block);
				i += block.GetLineCount();

				whiteSpace = reader.ReadLine();
				i++;

			}
			return project;
		}
		public static ProjectAnimData ReadProject(StreamReader reader)
		{
			ProjectAnimData project = new ProjectAnimData();
			project.Header = AnimDataProjectHeader.ReadBlock(reader);


			string whiteSpace = "";
			while (whiteSpace != null)
			{
				AnimDataBlock block = AnimDataBlock.ReadBlock(reader);
				project.Blocks.Add(block);

				whiteSpace = reader.ReadLine();

			}
			return project;
		}
		public static ProjectAnimData ExtractProject(StreamReader reader, string openString, string closeString)
		{
			string s;
			while (!(s = reader.ReadLine()).Contains(openString))
			{

			}
			ProjectAnimData project = new ProjectAnimData();
			project.Header = AnimDataProjectHeader.ReadBlock(reader);


			string whiteSpace = "";
			while (whiteSpace != null && !whiteSpace.Contains(closeString))
			{
				AnimDataBlock block = AnimDataBlock.ReadBlock(reader);
				project.Blocks.Add(block);

				whiteSpace = reader.ReadLine();

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
			foreach (AnimDataBlock block in Blocks)
			{
				i += block.GetLineCount();
				i++;
			}
			return i;
		}
	}
}
