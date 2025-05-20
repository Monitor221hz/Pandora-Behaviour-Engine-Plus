using Pandora.API.Patch.Engine.Skyrim64.AnimData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Pandora.Models.Patch.Skyrim64.AnimData
{


	public class ClipMotionDataBlock : IClipMotionDataBlock
	{
		public string ClipID { get; private set; } = string.Empty;
		public float Duration { get; private set; } = 1.33f;
		public int NumTranslations { get; private set; } = 1;
		public List<string> Translations { get; private set; } = new List<string>() { "1.33 0 0 0" };

		public int NumRotations { get; private set; } = 1;
		public List<string> Rotations { get; private set; } = new List<string>() { "1 0 0 0 1" };


		public ClipMotionDataBlock(string id)
		{
			ClipID = id;
		}

		public static ClipMotionDataBlock ReadBlock(StreamReader reader)
		{

			ClipMotionDataBlock block = new ClipMotionDataBlock("");
			try
			{
				block.Rotations = new List<string>();
				block.Translations = new List<string>();

				block.ClipID = reader.ReadLine();

				block.Duration = float.Parse(reader.ReadLine());

				block.NumTranslations = int.Parse(reader.ReadLine());

				for (int i = 0; i < block.NumTranslations; i++)
				{
					block.Translations.Add(reader.ReadLine());

				}

				block.NumRotations = int.Parse(reader.ReadLine());

				for (int i = 0; i < block.NumRotations; i++)
				{
					block.Rotations.Add(reader.ReadLine());

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
			using (StreamReader reader = new StreamReader(filePath))
			{
				return ReadBlock(reader);
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
