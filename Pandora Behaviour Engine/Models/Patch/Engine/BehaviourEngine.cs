using Microsoft.Win32;
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
namespace Pandora.Core
{
	public class BehaviourEngine
	{
		public static readonly DirectoryInfo AssemblyDirectory = new FileInfo(System.Reflection.Assembly.GetEntryAssembly()!.Location).Directory!;

		public static readonly List<IEngineConfigurationInjection> EngineConfigurations = new List<IEngineConfigurationInjection>();

		public readonly static DirectoryInfo? SkyrimGameDirectory; 
		private static IEnumerable<IEngineConfigurationInjection> CreateConfigurations(Assembly assembly)
		{
			foreach(Type type in assembly.GetTypes())
			{
				if (typeof(IEngineConfigurationInjection).IsAssignableFrom(type))
				{
					IEngineConfigurationInjection? result = Activator.CreateInstance(type) as IEngineConfigurationInjection;
					if (result != null)
					{
						yield return result;
					}
				}
			}
			yield break;
		}
		private static void LoadPlugins()
		{
			var pluginLoader = new JsonPluginLoader(); 
			var pluginsDirectory = AssemblyDirectory.CreateSubdirectory("Plugins");
			Assembly assembly;
			foreach (DirectoryInfo pluginDirectory in pluginsDirectory.EnumerateDirectories())
			{
				if (!pluginLoader.TryLoadMetadata(pluginDirectory, out var pluginInfo))
				{
					continue; 
				}
				
				try
				{
					assembly = pluginLoader.LoadPlugin(pluginDirectory, pluginInfo);
					EngineConfigurations.AddRange(CreateConfigurations(assembly));
				}
				catch
				{

				}
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

			Configuration.Patcher.SetTarget(mods); 
			Configuration.Patcher.Update(); 
			Configuration.Patcher.Run();
		}

		public async Task<bool> LaunchAsync(List<IModInfo> mods)
		{
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
