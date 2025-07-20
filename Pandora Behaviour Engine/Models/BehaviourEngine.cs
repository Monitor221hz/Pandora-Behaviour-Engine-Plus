using Microsoft.Win32;
using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;
using Pandora.Models.Patch.Configs;
using Pandora.Models.Patch.Engine.Plugins;
using Pandora.Models.Patch.Plugins;
using Pandora.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Pandora.Models;

public class BehaviourEngine
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private static readonly PluginLoader pluginLoader = new();

	public static readonly DirectoryInfo AssemblyDirectory = new FileInfo(Assembly.GetEntryAssembly()!.Location).Directory!;

	//public static readonly DirectoryInfo AssemblyDirectory = new(AppContext.BaseDirectory);

	public static readonly DirectoryInfo CurrentDirectory = new(Environment.CurrentDirectory!);

	public static readonly List<IEngineConfigurationPlugin> EngineConfigurations = [];

	public readonly static DirectoryInfo? SkyrimGameDirectory;

	private static void AddConfigurations(Assembly assembly)
	{
		foreach (Type type in assembly.GetTypes())
		{
			if (typeof(IEngineConfigurationPlugin).IsAssignableFrom(type))
			{
				if (Activator.CreateInstance(type) is IEngineConfigurationPlugin result)
				{
					EngineConfigurations.Add(result);
				}
			}
		}
	}
	private static void LoadPlugins()
	{

		var pluginsDirectory = new DirectoryInfo(Path.Join(AssemblyDirectory.FullName, "Plugins"));
		if (!pluginsDirectory.Exists)
		{
			return;
		}
		Assembly? assembly;
		var subDirectories = pluginsDirectory.GetDirectories();
		foreach (DirectoryInfo pluginDirectory in subDirectories)
		{
#if DEBUG
			// only for debug. DO NOT introduce json field plugin loading to release builds
			IMetaPluginLoader metaPluginLoader = new JsonPluginLoader();

			if (!metaPluginLoader.TryLoadMetadata(pluginDirectory, out var pluginInfo))
			{
				continue;
			}
			assembly = metaPluginLoader.LoadPlugin(pluginDirectory, pluginInfo);
#else
			try
			{
				assembly = pluginLoader.LoadPlugin(pluginDirectory);
			}
			catch (Exception ex)
			{
				logger.Error($"Critical error loading plugins: {ex.ToString()}");
				return;
			}
#endif
			if (assembly != null) { AddConfigurations(assembly); }
		}
	}
	private void ReadSkyrimPath()
	{

	}
	static BehaviourEngine()
	{
		LoadPlugins();
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			var subKey = "SOFTWARE\\Wow6432Node\\Bethesda Softworks\\Skyrim Special Edition";
			using (var key = Registry.LocalMachine.OpenSubKey(subKey, false))
			{
				if (key?.GetValue("Installed Path") is string defaultPath)
				{
					SkyrimGameDirectory = new DirectoryInfo(Path.Join(defaultPath, "Data"));
				}
			}
		}
		var rawArgs = Environment.GetCommandLineArgs().Skip(1).ToArray();
		var options = LaunchOptions.Parse(rawArgs);

		if (options.SkyrimGameDirectory is not null)
		{
			SkyrimGameDirectory = new DirectoryInfo(Path.Join(options.SkyrimGameDirectory.FullName, "Data"));
		}
	}

	public IEngineConfiguration Configuration { get; private set; } = new SkyrimConfiguration();
	public bool IsExternalOutput = false;
	public DirectoryInfo OutputPath { get; private set; } = new DirectoryInfo(Environment.CurrentDirectory);
	public void SetOutputPath(DirectoryInfo outputPath)
	{
		OutputPath = outputPath!;
		IsExternalOutput = CurrentDirectory != OutputPath;
		Configuration.Patcher.SetOutputPath(outputPath);
	}

	public BehaviourEngine()
	{

	}
	public BehaviourEngine(IEngineConfiguration configuration)
	{
		Configuration = configuration;
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

	public async Task PreloadAsync()
	{
		await Configuration.Patcher.PreloadAsync();
	}

	public string GetMessages(bool success)
	{
		return success ? Configuration.Patcher.GetPostRunMessages() : Configuration.Patcher.GetFailureMessages();
	}
}
