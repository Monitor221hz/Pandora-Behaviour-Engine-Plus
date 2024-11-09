namespace Pandora.API.Patch.Engine.Skyrim64.AnimData;

public interface IProjectAnimDataHeader
	{
		int AssetCount { get; set; }
		int HasMotionData { get; set; }
		int LeadInt { get; set; }
		List<string> ProjectAssets { get; set; }

		int GetLineCount();
		string ToString();
	}
