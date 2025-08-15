using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;
using Pandora.Models.Patch.Configs;
using Pandora.Models.Patch.Plugins;
using Pandora.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Pandora.Models;

public class BehaviourEngine
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private static readonly PluginLoader pluginLoader = new();

	/// <summary>
	/// Gets the real file system path to the folder containing the running executable.
	/// 
	/// Unlike typical assembly path methods, this returns the actual disk path even when 
	/// running inside virtualized environments like Mod Organizer 2 (MO2), bypassing the VFS.
	/// </summary>
	public static readonly DirectoryInfo AssemblyDirectory = new(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName)!);

	public static readonly DirectoryInfo CurrentDirectory = new(Environment.CurrentDirectory!);
	public static readonly DirectoryInfo? SkyrimGameDirectory;
	public static readonly List<IEngineConfigurationPlugin> EngineConfigurations = [];

	public bool IsExternalOutput = false;

	public IEngineConfiguration Configuration { get; private set; } = new SkyrimConfiguration();
	public static DirectoryInfo OutputPath { get; private set; } = new(Environment.CurrentDirectory);

	static BehaviourEngine()
	{
		PluginManager.LoadAllPlugins(AssemblyDirectory);
		SkyrimGameDirectory = SkyrimPathResolver.Resolve();
	}

	public BehaviourEngine()
	{

	}
	public BehaviourEngine SetOutputPath(DirectoryInfo outputPath)
	{
		OutputPath = outputPath!;
		IsExternalOutput = CurrentDirectory != OutputPath;
		Configuration.Patcher.SetOutputPath(outputPath);
		return this;
	}

	public BehaviourEngine SetConfiguration(IEngineConfiguration? configuration)
	{
		Configuration = configuration ?? new SkyrimConfiguration();
		return this;
	}
	public void Launch(List<IModInfo> mods)
	{
		logger.Info($"Launching with configuration {Configuration.Name}");
		logger.Info($"Launching with patcher {Configuration.Patcher.GetVersionString()}");
		Configuration.Patcher.SetTarget(mods);
		Configuration.Patcher.Update();
		Configuration.Patcher.Run();
	}

	public async Task<bool> LaunchAsync(List<IModInfo> mods)
	{
		logger.Info($"Launching with configuration {Configuration.Name}");
		logger.Info($"Launching with patcher version {Configuration.Patcher.GetVersionString()}");
		Configuration.Patcher.SetTarget(mods);

		if (!OutputPath.Exists) OutputPath.Create();


		if (!await Configuration.Patcher.UpdateAsync()) { return false; }

		return await Configuration.Patcher.RunAsync();
	}


	public async Task PreloadAsync() => 
		await Configuration.Patcher.PreloadAsync();

	public string GetMessages(bool success) => 
		success ? Configuration.Patcher.GetPostRunMessages() : Configuration.Patcher.GetFailureMessages();
}
