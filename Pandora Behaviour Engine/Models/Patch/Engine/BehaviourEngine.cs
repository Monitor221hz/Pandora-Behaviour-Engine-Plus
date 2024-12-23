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
using System.Diagnostics;
namespace Pandora.Core
{
	public class BehaviourEngine
	{
		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
		private static readonly PluginLoader pluginLoader = new PluginLoader();

		public static readonly DirectoryInfo AssemblyDirectory = new FileInfo(System.Reflection.Assembly.GetEntryAssembly()!.Location).Directory!;

		public static readonly List<IEngineConfigurationPlugin> EngineConfigurations = new List<IEngineConfigurationPlugin>();

		public readonly static DirectoryInfo? SkyrimGameDirectory; 

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
						var dataPathBethesda = Path.Join(defaultPathBethesda, "Data");
                    				SkyrimGameDirectory = ResolveSymbolicLink(new DirectoryInfo(dataPathBethesda));
						return;
					}
				}
						
				var subKey = "SOFTWARE\\WOW6432Node\\GOG.com\\Games\\1711230643";
				using (var key = Registry.LocalMachine.OpenSubKey(subKey, false))
				{
					 string? defaultPath = key?.GetValue("Installed Path") as string;
					if (defaultPath != null)
					{
						var dataPathGOG = Path.Join(defaultPath, "Data");
				                SkyrimGameDirectory = ResolveSymbolicLink(new DirectoryInfo(dataPath));
						return;
					}
				}
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        		{
				var userHomePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            			var possibleLinuxPaths = new[]
            			{
                			Path.Combine(userHomePath, "/home/{0}/.local/share/Skyrim Special Edition/Data",
                			"/usr/local/share/skyrimse/Data",
                			"/opt/SkyrimSpecialEdition/Data"
            			};

            			foreach (var pathTemplate in possibleLinuxPaths)
            			{
                			var path = string.Format(pathTemplate, Environment.UserName);
                			var dataDir = new DirectoryInfo(path);
                			if (dataDir.Exists)
                			{
                    				SkyrimGameDirectory = ResolveSymbolicLink(dataDir);
                    				return;
                			}
            			}
			}
	
			var args = Environment.GetCommandLineArgs(); 
			var inputArg = args.Where(s => s.StartsWith("-tesv:", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

			if (!string.IsNullOrEmpty(inputArg))
        		{
            			var path = inputArg.Substring(6).Trim();
            			if (!string.IsNullOrEmpty(path))
            			{
                			var dataPathCmd = Path.Join(path, "Data");
                			SkyrimGameDirectory = ResolveSymbolicLink(new DirectoryInfo(dataPathCmd));
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

        		SkyrimGameDirectory = ResolveSymbolicLink(new DirectoryInfo(Path.Join(path, "Data")));
    		}
	}

	private static DirectoryInfo ResolveSymbolicLink(DirectoryInfo directory)
	{
    		if (directory.Exists)
    		{
        		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        		{
            			var attributes = directory.Attributes;
            			if (attributes.HasFlag(FileAttributes.ReparsePoint))
            			{
                			string resolvedPath = GetSymbolicLinkTargetWindows(directory.FullName);
                			if (!string.IsNullOrEmpty(resolvedPath))
                			{
                    				return new DirectoryInfo(resolvedPath);
                			}
            			}
        		}
        		else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        		{
            			var linkTarget = directory.ResolveLinkTarget(true);
            			if (linkTarget != null && Directory.Exists(linkTarget.FullName))
            		{
                		return new DirectoryInfo(linkTarget.FullName);
            		}
        	}
    	}
    	return directory;
}

[DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
private static extern uint GetFinalPathNameByHandle(IntPtr hFile, [Out] StringBuilder lpszFilePath, uint cchFilePath, uint dwFlags);

private static string GetSymbolicLinkTargetWindows(string linkPath)
{
    using (var fileHandle = File.Open(linkPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
    {
        var handle = fileHandle.SafeFileHandle;
        var builder = new StringBuilder(1024);
        uint result = GetFinalPathNameByHandle(handle.DangerousGetHandle(), builder, (uint)builder.Capacity, 0);
        return result > 0 ? builder.ToString() : linkPath;
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
