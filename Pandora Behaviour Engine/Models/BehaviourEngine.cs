using Microsoft.Win32;
using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;
using Pandora.Models.Patch.Configs;
using Pandora.Models.Patch.Engine.Plugins;
using Pandora.Models.Patch.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
namespace Pandora.Models
{
	public class BehaviourEngine
	{
		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		private static readonly PluginLoader pluginLoader = new PluginLoader();

		public static readonly DirectoryInfo AssemblyDirectory = new FileInfo(Assembly.GetEntryAssembly()!.Location).Directory!;

		//public static readonly DirectoryInfo AssemblyDirectory = new(AppContext.BaseDirectory);

		public static readonly DirectoryInfo CurrentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()!);

		public static readonly List<IEngineConfigurationPlugin> EngineConfigurations = new List<IEngineConfigurationPlugin>();

		public readonly static DirectoryInfo? SkyrimGameDirectory;

		private static void AddConfigurations(Assembly assembly)
		{
			foreach (Type type in assembly.GetTypes())
			{
				if (typeof(IEngineConfigurationPlugin).IsAssignableFrom(type))
				{
					IEngineConfigurationPlugin? result = Activator.CreateInstance(type) as IEngineConfigurationPlugin;
					if (result != null)
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
					string? defaultPath = key?.GetValue("Installed Path") as string;
					if (defaultPath != null)
					{
						SkyrimGameDirectory = new DirectoryInfo(Path.Join(defaultPath, "Data"));
					}
				}
			}
			var args = Environment.GetCommandLineArgs();
			var inputArg = args.Where(s => s.StartsWith("-tesv:", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

			if (inputArg != null)
			{
				var argArr = inputArg.AsSpan();
				var pathArr = argArr.Slice(6);
				var path = pathArr.Trim().ToString();

				SkyrimGameDirectory = new DirectoryInfo(Path.Join(path, "Data"));
			}
		}

		public IEngineConfiguration Configuration { get; private set; } = new SkyrimConfiguration();
		public bool IsExternalOutput = false;
		public DirectoryInfo OutputPath { get; private set; } = new DirectoryInfo(Directory.GetCurrentDirectory());
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

}
