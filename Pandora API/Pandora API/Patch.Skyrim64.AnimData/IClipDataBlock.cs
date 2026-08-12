namespace Pandora.API.Patch.Skyrim64.AnimData;

public interface IClipDataBlock
{
    string ClipID { get; set; }
    float CropEndLocalTime { get; }
    float CropStartLocalTime { get; }
    string Name { get; }
    int NumClipTriggers { get; }
    float PlaybackSpeed { get; }
    IList<string> TriggerNames { get; }

    int GetLineCount();
    string ToString();
}
