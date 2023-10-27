using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.AnimData
{
	public class MotionDataBlock
	{
		public string ClipID { get; set; }
		public float Duration { get; set; }
		public int NumTranslations { get; set; }
		public List<string> Translations { get; set; } = new List<string>();

		public int NumRotations { get; set; }

		public List<string> Rotations { get; set; } = new List<string>();


		public static MotionDataBlock ReadBlock(StreamReader reader)
		{

			MotionDataBlock block = new MotionDataBlock();
			try
			{

				block.ClipID = reader.ReadLine();

				block.Duration = float.Parse(reader.ReadLine());

				block.NumTranslations = Int32.Parse(reader.ReadLine());

				for (int i = 0; i < block.NumTranslations; i++)
				{
					block.Translations.Add(reader.ReadLine());

				}

				block.NumRotations = Int32.Parse(reader.ReadLine());

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

		public static MotionDataBlock LoadBlock(string filePath)
		{
			using (StreamReader reader = new StreamReader(filePath))
			{
				return ReadBlock(reader);
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(ClipID).AppendLine(Duration.ToString()).AppendLine(Translations.Count.ToString()).AppendLine(String.Join("\r\n", Translations)).AppendLine(Rotations.Count.ToString()).AppendLine(String.Join("\r\n", Rotations));
			return sb.ToString();
		}
		public int GetLineCount()
		{
			return 4 + Translations.Count + Rotations.Count;
		}

	}
}
