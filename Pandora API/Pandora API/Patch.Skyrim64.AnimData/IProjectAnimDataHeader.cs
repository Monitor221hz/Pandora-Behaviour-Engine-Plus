namespace Pandora.API.Patch.Skyrim64.AnimData;

public interface IProjectAnimDataHeader
{
    int AssetCount { get; set; }
    int HasMotionData { get; set; }
    int LeadInt { get; set; }
    IList<string> ProjectAssets { get; set; }

    int GetLineCount();
    string ToString();
}
