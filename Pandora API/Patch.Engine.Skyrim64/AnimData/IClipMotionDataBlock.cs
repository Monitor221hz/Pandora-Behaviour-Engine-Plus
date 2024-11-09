namespace Pandora.API.Patch.Engine.Skyrim64.AnimData;

public interface IClipMotionDataBlock
{
	string ClipID { get; }
	float Duration { get; }
	int NumRotations { get; }
	int NumTranslations { get; }
	List<string> Rotations { get; }
	List<string> Translations { get; }

	int GetLineCount();
	string ToString();
}
