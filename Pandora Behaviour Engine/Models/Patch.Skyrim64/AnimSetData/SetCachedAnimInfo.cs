using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Pandora.Models.Patch.Skyrim64.AnimSetData;

public class SetCachedAnimInfo
{
	public const uint ENCODED_EXTENSION_DEFAULT = 7891816;
	public const uint ENCODED_PATH_VANILLA = 3064642194;
	public uint EncodedPath { get; private set; } = ENCODED_PATH_VANILLA; //vanilla actor animation folder path
	public uint EncodedFileName { get; private set; } = 0; //animation name in lowercase
	public uint EncodedExtension { get; private set; } = ENCODED_EXTENSION_DEFAULT; //xkh
	public SetCachedAnimInfo(uint encodedPath, uint encodedFileName, uint encodedExtension)
	{
		EncodedPath = encodedPath;
		EncodedFileName = encodedFileName;
		EncodedExtension = encodedExtension;
	}
	public SetCachedAnimInfo(uint encodedPath, uint encodedFileName) : this(encodedPath, encodedFileName, ENCODED_EXTENSION_DEFAULT) { }
	public SetCachedAnimInfo(uint encodedFileName) : this(ENCODED_PATH_VANILLA, encodedFileName, ENCODED_EXTENSION_DEFAULT) { }
	public static bool TryRead(StreamReader reader, [NotNullWhen(true)] out SetCachedAnimInfo? info)
	{
		info = null;
		if (!uint.TryParse(reader.ReadLine(), out var encodedPath) ||
			!uint.TryParse(reader.ReadLine(), out var encodedFileName) ||
			!uint.TryParse(reader.ReadLine(), out var encodedExtension))
		{
			return false;
		}
		info = new(encodedPath, encodedFileName, encodedExtension);
		return true;
	}

	public static SetCachedAnimInfo Encode(string folderPath, string fileName) //filename without extension
	{
		var animInfo = new SetCachedAnimInfo(BSCRC32.GetValueUInt32(folderPath.ToLower()), BSCRC32.GetValueUInt32(fileName.ToLower()));
		return animInfo;
	}

	public override string ToString()
	{
		StringBuilder sb = new();

		sb.AppendLine(EncodedPath.ToString());
		sb.AppendLine(EncodedFileName.ToString());
		sb.AppendLine(EncodedExtension.ToString());

		return sb.ToString();
	}
}