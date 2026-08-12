namespace Pandora.API.Patch.Skyrim64.AnimData;

public interface IClipMotionDataBlock
{
    string ClipID { get; set; }
    float Duration { get; }
    int NumRotations { get; }
    int NumTranslations { get; }
    IList<string> Rotations { get; }
    IList<string> Translations { get; }

    int GetLineCount();
    string ToString();
}
