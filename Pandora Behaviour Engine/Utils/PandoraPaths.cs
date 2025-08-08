using Pandora.Logging;
using System;
using System.IO;

namespace Pandora.Utils;

public static class PandoraPaths
{
	public static DirectoryInfo Root => _root.Value;
	private static readonly Lazy<DirectoryInfo> _root = new(() =>
	{
		var outputDir = LaunchOptions.Current.OutputDirectory?.FullName ?? Environment.CurrentDirectory;
		var dir = new DirectoryInfo(Path.Combine(outputDir, "Pandora_Engine"));
		dir.Create();
		return dir;
	});

	public static FileInfo ActiveModsFile => new(Path.Combine(Root.FullName, "ActiveMods.json"));
}