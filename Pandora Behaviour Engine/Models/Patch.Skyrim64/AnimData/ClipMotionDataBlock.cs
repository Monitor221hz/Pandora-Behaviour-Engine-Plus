using Pandora.API.Patch.Engine.Skyrim64.AnimData;
using Pandora.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Pandora.Models.Patch.Skyrim64.AnimData
{


	public class ClipMotionDataBlock : IClipMotionDataBlock
	{
		public string ClipID { get; set; } = string.Empty;
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
			if (!reader.TryReadLine(out var clipId) ||
				!float.TryParse(reader.ReadLine(), out var duration) ||
				!int.TryParse(reader.ReadLine(), out var numTranslations))
			{
				return false;
			}

			var translations = new string[Math.Max(numTranslations, 1)];
			for (int i = 0; i < translations.Length; i++)
			{
				if (!reader.TryReadNotEmptyLine(out var value))
				{
					value = "0.0 0 0 0";
				}
				translations[i] = value;
			}

			if (!int.TryParse(reader.ReadLine(), out var numRotations)) { return false; }
			var rotations = new string[Math.Max(numRotations, 1)];
			for (int i = 0; i < rotations.Length; i++)
			{
				if (!reader.TryReadNotEmptyLine(out var value))
				{
					value = "0.0 0 0 0 1"; // default rotation
				}
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
		public static bool TryLoadBlock(FileInfo file, [NotNullWhen(true)] out ClipMotionDataBlock? motionDataBlock)
		{
			using (var fileStream = file.OpenRead())
			{
				using (StreamReader reader = new StreamReader(fileStream))
				{
					return TryReadBlock(reader, out motionDataBlock);
				}
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(ClipID).AppendLine(Duration.ToString()).AppendLine(Translations.Count.ToString()).AppendLine(string.Join("\r\n", Translations)).AppendLine(Rotations.Count.ToString()).AppendLine(string.Join("\r\n", Rotations));
			return sb.ToString();
		}
		public int GetLineCount()
		{
			return 4 + Translations.Count + Rotations.Count;
		}

	}
}
