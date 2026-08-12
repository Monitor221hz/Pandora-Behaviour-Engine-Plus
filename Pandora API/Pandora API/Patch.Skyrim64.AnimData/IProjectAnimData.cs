using System.Diagnostics.CodeAnalysis;

namespace Pandora.API.Patch.Skyrim64.AnimData;

public interface IProjectAnimData
{
    IMotionData? BoundMotionDataProject { get; set; }
    IProjectAnimDataHeader Header { get; }
    void AddClipData(IClipDataBlock dataBlock);
    void AddClipData(IClipDataBlock dataBlock, IClipMotionDataBlock motionDataBlock);
    void AddDummyClipData(string clipName);
    int GetLineCount();
    string ToString();
}
