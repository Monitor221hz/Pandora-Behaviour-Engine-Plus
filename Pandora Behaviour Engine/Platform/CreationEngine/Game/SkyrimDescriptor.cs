namespace Pandora.Platform.CreationEngine.Game;

public class SkyrimDescriptor : IGameDescriptor
{
	public string Id => "SkyrimSE";
	public string Name => "Skyrim Special Edition";
	public uint[] SteamAppIds => [489830, 611670];
	public long? GogAppId => 711230643;
	public string SubKey => @"SOFTWARE\Wow6432Node\Bethesda Softworks\Skyrim Special Edition";
	public string[] ExecutableNames => 
	[
		"SkyrimSE.exe", 
		"SkyrimSELauncher.exe", 
		"SkyrimVR.exe"
	];
}