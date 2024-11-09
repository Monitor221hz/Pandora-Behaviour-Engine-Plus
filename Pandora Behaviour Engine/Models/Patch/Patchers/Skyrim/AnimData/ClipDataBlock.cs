using Pandora.API.Patch.Engine.Skyrim64.AnimData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.AnimData
{


	public class ClipDataBlock : IClipDataBlock
	{
		public string Name { get; private set; } = string.Empty;

		public string ClipID { get; private set; } = string.Empty;

		public float PlaybackSpeed { get; private set; } = 1.0f;
		public float CropStartLocalTime { get; private set; } = 0.0f;
		public float CropEndLocalTime { get; private set; } = 0.0f;

		public int NumClipTriggers { get; private set; } = 0;

		public List<string> TriggerNames { get; private set; } = new List<string>();

		public ClipDataBlock()
		{

		}

		public ClipDataBlock(string name, string id)
		{
			Name = name;
			ClipID = id;
		}
		private static string ReadLineSafe(StreamReader reader)
		{
			string? expectedLine = reader.ReadLine();
			return expectedLine == null ? string.Empty : expectedLine;
		}
		public static ClipDataBlock ReadBlock(StreamReader reader)
		{
			ClipDataBlock block = new ClipDataBlock();
			try
			{
				block.Name = ReadLineSafe(reader);

				block.ClipID = ReadLineSafe(reader);
				block.PlaybackSpeed = float.Parse(ReadLineSafe(reader));
				block.CropStartLocalTime = float.Parse(ReadLineSafe(reader));
				block.CropEndLocalTime = float.Parse(ReadLineSafe(reader));

				block.NumClipTriggers = int.Parse(ReadLineSafe(reader));

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

		public static ClipDataBlock LoadBlock(string filePath)
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
