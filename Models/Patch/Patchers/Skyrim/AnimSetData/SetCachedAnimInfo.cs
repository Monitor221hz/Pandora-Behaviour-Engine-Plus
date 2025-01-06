using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Pandora.Patch.Patchers.Skyrim.AnimSetData
{
	public class SetCachedAnimInfo
	{

		public UInt32 encodedPath { get; private set; } = 3064642194; //vanilla actor animation folder path

		public UInt32 encodedFileName { get; private set; } = 0; //animation name in lowercase

		public UInt32 encodedExtension { get; private set; } = 7891816; //xkh


		public static SetCachedAnimInfo Read(StreamReader reader)
		{
			var animInfo = new SetCachedAnimInfo();

			animInfo.encodedPath = UInt32.Parse(reader.ReadLineSafe());
			animInfo.encodedFileName = UInt32.Parse(reader.ReadLineSafe());
			animInfo.encodedExtension = UInt32.Parse(reader.ReadLineSafe());

			return animInfo;
		}

		public static SetCachedAnimInfo Encode(string folderPath, string fileName) //filename without extension
		{
			var animInfo = new SetCachedAnimInfo();

			animInfo.encodedPath = BSCRC32.GetValueUInt32(folderPath.ToLower());
			animInfo.encodedFileName = BSCRC32.GetValueUInt32(fileName.ToLower());

			return animInfo;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine(encodedPath.ToString());
			sb.AppendLine(encodedFileName.ToString());
			sb.AppendLine(encodedExtension.ToString());

			return sb.ToString();
		}


	}
}
