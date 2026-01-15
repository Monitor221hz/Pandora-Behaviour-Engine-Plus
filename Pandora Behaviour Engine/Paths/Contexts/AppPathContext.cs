using Pandora.Paths.Extensions;
using System;
using System.Diagnostics;
using System.IO;

namespace Pandora.Paths.Contexts;

public sealed class AppPathContext : IAppPathContext
{
	private const string PANDORA_ENGINE_FOLDERNAME = "Pandora_Engine";
	private const string CONFIG_FILE = "Paths.json";

	public FileInfo PathConfig => _pathConfig.Value;
	private readonly Lazy<FileInfo> _pathConfig;

	public DirectoryInfo AssemblyDirectory => new(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule!.FileName!)!);
	public DirectoryInfo CurrentDirectory => new(Environment.CurrentDirectory);
	public DirectoryInfo EngineDirectory => new(AssemblyDirectory.FullName / PANDORA_ENGINE_FOLDERNAME);
	public DirectoryInfo TemplateDirectory => new(EngineDirectory.FullName / "Skyrim" / "Template");

	public AppPathContext()
	{
		_pathConfig = new Lazy<FileInfo>(() => new FileInfo(EngineDirectory.FullName / CONFIG_FILE));
	}
}
 