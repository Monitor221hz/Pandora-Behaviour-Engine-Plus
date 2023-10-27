using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.AnimData
{
	public class AnimDataBlock
	{
		public string Name { get; set; }

		public string ClipID { get; set; }
		public float PlaybackSpeed { get; set; }
		public float CropStartLocalTime { get; set; }
		public float CropEndLocalTime { get; set; }

		public int NumClipTriggers { get; set; }

		public List<string> TriggerNames { get; set; }

		public AnimDataBlock()
		{

		}

		private static string ReadLineSafe(StreamReader reader)
		{
			string? expectedLine = reader.ReadLine(); 
			return expectedLine == null ? string.Empty : expectedLine;
		}
		public static AnimDataBlock ReadBlock(StreamReader reader)
		{
			AnimDataBlock block = new AnimDataBlock();
			try
			{
				block.Name = ReadLineSafe(reader);

				block.ClipID = ReadLineSafe(reader);
				block.PlaybackSpeed = float.Parse(ReadLineSafe(reader));
				block.CropStartLocalTime = float.Parse(ReadLineSafe(reader));
				block.CropEndLocalTime = float.Parse(ReadLineSafe(reader));

				block.NumClipTriggers = int.Parse(ReadLineSafe(reader));
				block.TriggerNames = new List<string>();
				for (int i = 0; i < block.NumClipTriggers; i++)
				{
					block.TriggerNames.Add(ReadLineSafe(reader));
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message + " in ", ex);
			}
			return block;

		}

		public static AnimDataBlock LoadBlock(string filePath)
		{
			using (StreamReader reader = new StreamReader(filePath))
			{
				return ReadBlock(reader);
			}
		}

		public override string ToString()
		{

			StringBuilder sb = new StringBuilder();

			sb.AppendLine(Name).AppendLine(ClipID).AppendLine(PlaybackSpeed.ToString()).AppendLine(CropStartLocalTime.ToString()).AppendLine(CropEndLocalTime.ToString()).AppendLine(NumClipTriggers.ToString());

			if (TriggerNames.Count > 0)
			{



				sb.AppendJoin("\r\n", TriggerNames).AppendLine();

			}
			return sb.ToString();
		}

		public int GetLineCount()
		{
			return 6 + TriggerNames.Count;
		}
	}
}
