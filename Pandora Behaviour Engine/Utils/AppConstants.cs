using System;
using System.IO;

namespace Pandora.Utils;

public static class AppConstants
{
	public static readonly string ActiveModsPath = Path.Join("Pandora_Engine", "ActiveMods.json");
	public static readonly TimeSpan SearchThrottle = TimeSpan.FromMilliseconds(200);
}