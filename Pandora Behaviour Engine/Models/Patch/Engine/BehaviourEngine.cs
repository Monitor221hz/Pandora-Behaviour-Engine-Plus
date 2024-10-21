using Microsoft.Win32;
using Pandora.Core.Engine.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Core
{
	public class BehaviourEngine
	{
		public static readonly DirectoryInfo AssemblyDirectory = new FileInfo(System.Reflection.Assembly.GetEntryAssembly()!.Location).Directory!;

		public readonly static DirectoryInfo? GameDirectory; 

		static BehaviourEngine()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				var subKey = "SOFTWARE\\Wow6432Node\\Bethesda Softworks\\Skyrim Special Edition";
				using (var key = Registry.LocalMachine.OpenSubKey(subKey, false))
				{
					string? defaultPath = key?.GetValue("Installed Path") as string;
					if (defaultPath != null)
					{
						GameDirectory = new DirectoryInfo(Path.Join(defaultPath, "Data"));
					}
				}
				
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
