using Pandora.API.Patch.Engine.Skyrim64.AnimData;
using Pandora.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Pandora.Models.Patch.Skyrim64.AnimData
{


	public class ClipDataBlock : IClipDataBlock
	{
		public string Name { get; private set; } = string.Empty;

		public string ClipID { get; set; } = string.Empty;

		public float PlaybackSpeed { get; private set; } = 1.0f;
		public float CropStartLocalTime { get; private set; } = 0.0f;
		public float CropEndLocalTime { get; private set; } = 0.0f;

		public int NumClipTriggers { get; private set; } = 0;

		public IList<string> TriggerNames { get; private set; } = [];

		public ClipDataBlock()
		{

		}

		public ClipDataBlock(string name, string id)
		{
			Name = name;
			ClipID = id;
		}

		public ClipDataBlock(string name, string clipID, float playbackSpeed, float cropStartLocalTime, float cropEndLocalTime, int numClipTriggers, IList<string> triggerNames) : this(name, clipID)
		{
			PlaybackSpeed = playbackSpeed;
			CropStartLocalTime = cropStartLocalTime;
			CropEndLocalTime = cropEndLocalTime;
			NumClipTriggers = numClipTriggers;
			TriggerNames = triggerNames;
		}

		private static string ReadLineSafe(StreamReader reader)
		{
			string? expectedLine = reader.ReadLine();
			return expectedLine == null ? string.Empty : expectedLine;
		}

		public static bool TryReadBlock(StreamReader reader, [NotNullWhen(true)] out ClipDataBlock? block)
		{
			block = null;
			if (!reader.TryReadLine(out var clipName) ||
				!reader.TryReadLine(out var clipId) ||
				!float.TryParse(reader.ReadLine(), out float playBackSpeed) ||
				!float.TryParse(reader.ReadLine(), out float cropStartLocalTime) ||
				!float.TryParse(reader.ReadLine(), out float cropEndLocalTime) ||
				!int.TryParse(reader.ReadLine(), out int numClipTriggers))
			{
				return false;
			}
			string[] triggerNames = new string[numClipTriggers];
			for (int i = 0; i < numClipTriggers; i++)
			{
				if (!reader.TryReadLine(out var triggerName)) { return false; }
				triggerNames[i] = triggerName;
			}
			block = new ClipDataBlock(clipName, clipId, playBackSpeed, cropStartLocalTime, cropEndLocalTime, numClipTriggers, triggerNames);
			return true;
		}

		public static bool TryReadBlock(FileInfo fileInfo, [NotNullWhen(true)] out ClipDataBlock? block)
		{
			using (var fileStream = fileInfo.OpenRead())
			{
				using (var reader = new StreamReader(fileStream))
				{
					return TryReadBlock(reader, out block);
				}
			}
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
