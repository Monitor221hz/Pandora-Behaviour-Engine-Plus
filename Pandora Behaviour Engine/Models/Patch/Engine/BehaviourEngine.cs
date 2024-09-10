using Pandora.Core.Engine.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Core
{
	public class BehaviourEngine
	{
		public static readonly DirectoryInfo AssemblyDirectory = new FileInfo(System.Reflection.Assembly.GetEntryAssembly()!.Location).Directory!;
		public IEngineConfiguration Configuration { get; private set; } = new SkyrimConfiguration();

		private bool ClearOutputPath = false;
        private DirectoryInfo CurrentPath { get; } = new DirectoryInfo(Directory.GetCurrentDirectory());
        public DirectoryInfo OutputPath { get; private set; } = new DirectoryInfo(Directory.GetCurrentDirectory());
        public void SetOutputPath(DirectoryInfo outputPath)
		{
			ClearOutputPath = (outputPath != CurrentPath);
			OutputPath = outputPath!;
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

			if (ClearOutputPath)
            {
                ClearFolder(OutputPath);
            }

			var fnisESP = new FileInfo(Path.Combine(AssemblyDirectory.FullName, "FNIS.esp"));
			if (fnisESP.Exists)
				fnisESP.CopyTo(Path.Combine(OutputPath.FullName, "FNIS.esp"), true);

            if (!await Configuration.Patcher.UpdateAsync()) { return false; }

			return await Configuration.Patcher.RunAsync();
		}

        private void ClearFolder(DirectoryInfo dir)
        {
            foreach (var item in dir.GetFiles())
            {
                if (item.Name.Equals("ActiveMods.txt", StringComparison.InvariantCultureIgnoreCase))
					continue;
				else
                    item.Delete();
            }
            foreach (var item in dir.GetDirectories())
            {
				if (item.Name.Equals("Pandora_Engine", StringComparison.InvariantCultureIgnoreCase))
					ClearFolder(item);
				else
					item.Delete(true);
            }
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
