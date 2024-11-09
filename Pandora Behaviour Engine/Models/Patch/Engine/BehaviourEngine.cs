﻿using Microsoft.Win32;
using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;
using Pandora.Core.Engine.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Pandora.Models.Patch.Engine;
using Pandora.Models.Patch.Engine.Plugins;
using System.Diagnostics;
using System.Runtime.Versioning;
namespace Pandora.Core
{
	public class BehaviourEngine
	{
		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		private static readonly PluginLoader pluginLoader = new PluginLoader();

		public static readonly DirectoryInfo AssemblyDirectory = new FileInfo(System.Reflection.Assembly.GetEntryAssembly()!.Location).Directory!;

		public static readonly List<IEngineConfigurationPlugin> EngineConfigurations = new List<IEngineConfigurationPlugin>();

		public readonly static DirectoryInfo? SkyrimGameDataDirectory; 

		private static void AddConfigurations(Assembly assembly)
		{
			foreach(Type type in assembly.GetTypes())
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
			
			var pluginsDirectory = AssemblyDirectory.CreateSubdirectory("Plugins");
			Assembly assembly;
			foreach (DirectoryInfo pluginDirectory in pluginsDirectory.EnumerateDirectories())
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
				assembly = pluginLoader.LoadPlugin(pluginDirectory);
#endif
				AddConfigurations(assembly);
			}
		}
		private void ReadSkyrimPath()
		{

		}
		[SupportedOSPlatform("windows")]
		private static string? GetInstalledPath(string keyPath)
		{
			using (var key = Registry.LocalMachine.OpenSubKey(keyPath, false))
			{
				if (key == null) { return null; }
				return key.GetValue("Installed Path") as string;
			}

		}
		static BehaviourEngine()
		{
			LoadPlugins(); 
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				var subKey = "SOFTWARE\\Wow6432Node\\Bethesda Softworks\\Skyrim Special Edition";
				string? defaultPath = GetInstalledPath(subKey);
				defaultPath ??= GetInstalledPath("SOFTWARE\\Wow6432Node\\Bethesda Softworks\\Skyrim VR");

				if (defaultPath != null)
				{
					SkyrimGameDataDirectory = new DirectoryInfo(Path.Join(defaultPath, "Data")); 
				}

			}
			var args = Environment.GetCommandLineArgs(); 
			var inputArg = args.Where(s => s.StartsWith("-tesv:", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

			if (inputArg != null)
			{
				var argArr = inputArg.AsSpan();
				var pathArr = argArr.Slice(6);
				var path = pathArr.Trim().ToString();

				SkyrimGameDataDirectory = new DirectoryInfo(Path.Join(path, "Data")); 
			}
		}

		public IEngineConfiguration Configuration { get; private set; } = new SkyrimConfiguration();
		public bool IsExternalOutput = false; 
        private DirectoryInfo CurrentDirectory { get; } = new DirectoryInfo(Directory.GetCurrentDirectory());
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
