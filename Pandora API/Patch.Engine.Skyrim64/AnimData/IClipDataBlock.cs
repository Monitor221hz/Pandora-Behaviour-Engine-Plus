namespace Pandora.API.Patch.Engine.Skyrim64.AnimData;

public interface IClipDataBlock
{
	string ClipID { get; }
	float CropEndLocalTime { get; }
	float CropStartLocalTime { get; }
	string Name { get; }
	int NumClipTriggers { get; }
	float PlaybackSpeed { get; }
	List<string> TriggerNames { get; }

	int GetLineCount();
	string ToString();
}