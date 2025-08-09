using Pandora.Models;
using System;
using System.IO;

namespace Pandora.Utils;

public static class PandoraPaths
{
	public static DirectoryInfo OutputPath => LaunchOptions.Current?.OutputDirectory ?? BehaviourEngine.SkyrimGameDirectory ?? BehaviourEngine.CurrentDirectory;

	public static DirectoryInfo PandoraEngine => _pandoraEngine.Value;
	private static readonly Lazy<DirectoryInfo> _pandoraEngine = new(() =>
	{
		var dir = new DirectoryInfo(Path.Combine(OutputPath.FullName, "Pandora_Engine"));
		dir.Create();
		return dir;
	});

	public static FileInfo ActiveModsFile => new(Path.Combine(PandoraEngine.FullName, "ActiveMods.json"));
	public static FileInfo PreviousOutputFile => new(Path.Combine(PandoraEngine.FullName, "PreviousOutput.txt"));
}