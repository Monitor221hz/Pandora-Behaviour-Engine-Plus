namespace Pandora.API.Patch.Engine.Skyrim64.AnimData;

public interface IProjectAnimData
{
	public IProjectAnimDataHeader GetHeader();

	public IMotionData? GetBoundMotionData();
	public List<IClipDataBlock> GetBlocks();
	void AddDummyClipData(string clipName);
	int GetLineCount();
	string ToString();
}
