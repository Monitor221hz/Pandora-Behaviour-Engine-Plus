﻿using Pandora.Models.Extensions;
using System.IO;
using System.Text;

namespace Pandora.Models.Patch.Skyrim64.AnimSetData;

public class SetCachedAnimInfo
{
	public uint encodedPath { get; private set; } = 3064642194; //vanilla actor animation folder path

	public uint encodedFileName { get; private set; } = 0; //animation name in lowercase

	public uint encodedExtension { get; private set; } = 7891816; //xkh



	public static SetCachedAnimInfo Read(StreamReader reader)
	{
		var animInfo = new SetCachedAnimInfo
		{
			encodedPath = uint.Parse(reader.ReadLineSafe()),
			encodedFileName = uint.Parse(reader.ReadLineSafe()),
			encodedExtension = uint.Parse(reader.ReadLineSafe())
		};

		return animInfo;
	}

	public static SetCachedAnimInfo Encode(string folderPath, string fileName) //filename without extension
	{
		var animInfo = new SetCachedAnimInfo
		{
			encodedPath = BSCRC32.GetValueUInt32(folderPath.ToLower()),
			encodedFileName = BSCRC32.GetValueUInt32(fileName.ToLower())
		};

		return animInfo;
	}

	public override string ToString()
	{
		StringBuilder sb = new();

		sb.AppendLine(encodedPath.ToString());
		sb.AppendLine(encodedFileName.ToString());
		sb.AppendLine(encodedExtension.ToString());

		return sb.ToString();
	}
}