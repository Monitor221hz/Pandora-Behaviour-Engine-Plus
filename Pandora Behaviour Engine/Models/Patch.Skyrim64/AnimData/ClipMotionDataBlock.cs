using Pandora.API.Patch.Engine.Skyrim64.AnimData;
using Pandora.Models.Extensions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Pandora.Models.Patch.Skyrim64.AnimData;

public class ClipMotionDataBlock : IClipMotionDataBlock
{
	public string ClipID { get; private set; } = string.Empty;
	public float Duration { get; private set; } = 1.33f;
	public int NumTranslations { get; private set; } = 1;
	public IList<string> Translations { get; private set; } = ["1.33 0 0 0"];
	public int NumRotations { get; private set; } = 1;
	public IList<string> Rotations { get; private set; } = ["1 0 0 0 1"];

		public ClipMotionDataBlock()
		{

		}
		public ClipMotionDataBlock(string id)
		{
			ClipID = id;
		}
		public static bool TryReadBlock(StreamReader reader, [NotNullWhen(true)] out ClipMotionDataBlock? block)
		{
			block = null;
			if (!reader.TryReadLine(out var clipId)) { return false; }

			if (!float.TryParse(reader.ReadLine(), out var duration)) { return false; }

			if (!int.TryParse(reader.ReadLine(), out var numTranslations)) { return false; }

			var translations = new string[numTranslations];
			for (int i = 0; i < numTranslations; i++)
			{
				if (!reader.TryReadLine(out var value)) {  return false; }
				translations[i] = value;
			}

			if (!int.TryParse(reader.ReadLine(), out var numRotations)) { return false; }

			var rotations = new string[numRotations];
			for (int i = 0; i < numRotations; i++)
			{
				if (!reader.TryReadLine(out var value)) { return false; }
				rotations[i] = value;
			}
			block = new()
			{
				ClipID = clipId,
				Duration = duration,
				NumTranslations = numTranslations,
				NumRotations = numRotations,
				Translations = translations,
				Rotations = rotations
			};
			return true; 
		}
    
		public static bool TryReadBlock(FileInfo fileInfo, [NotNullWhen(true)] out ClipMotionDataBlock? block)
		{
			using (var fileStream = fileInfo.OpenRead())
			{
				using (var reader = new StreamReader(fileStream))
				{
					return TryReadBlock(reader, out block);
				}
			}
		}
    
		public static bool TryLoadBlock(FileInfo file,[NotNullWhen(true)] out ClipMotionDataBlock? motionDataBlock)
		{
			using (var fileStream = file.OpenRead())
			{
				using (StreamReader reader = new StreamReader(fileStream))
				{
					return TryReadBlock(reader, out motionDataBlock);
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
	public static ClipMotionDataBlock LoadBlock(string filePath)
	{
		using (StreamReader reader = new(filePath))
		{
			return ReadBlock(reader);
		}
	}

	public override string ToString()
	{
		StringBuilder sb = new();
		sb.AppendLine(ClipID).AppendLine(Duration.ToString()).AppendLine(Translations.Count.ToString()).AppendLine(string.Join("\r\n", Translations)).AppendLine(Rotations.Count.ToString()).AppendLine(string.Join("\r\n", Rotations));
		return sb.ToString();
	}
	public int GetLineCount()
	{
		return 4 + Translations.Count + Rotations.Count;
	}
}