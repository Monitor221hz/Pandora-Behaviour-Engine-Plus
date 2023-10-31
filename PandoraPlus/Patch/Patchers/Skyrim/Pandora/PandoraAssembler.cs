using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.AnimData;
using Pandora.Patch.Patchers.Skyrim.AnimSetData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.Pandora
{
	public class PandoraAssembler
	{
		private ProjectManager projectManager { get; set; }	
		private AnimDataManager animDataManager { get; set; }	
		private AnimSetDataManager animSetDataManager { get; set; }

		private DirectoryInfo currentFolder = new DirectoryInfo(Directory.GetCurrentDirectory());

		private DirectoryInfo templateFolder = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Pandora_Engine\\Skyrim\\Template");

		private DirectoryInfo outputFolder = new DirectoryInfo($"{Directory.GetCurrentDirectory()}\\meshes");


		public PandoraAssembler()
		{
			projectManager = new ProjectManager(templateFolder, outputFolder);
			animSetDataManager = new AnimSetDataManager(templateFolder, outputFolder);
			animDataManager = new AnimDataManager(templateFolder, outputFolder);
		}

		public PandoraAssembler(ProjectManager projManager, AnimSetDataManager animSDManager, AnimDataManager animDManager)
		{
			this.projectManager = projManager;
			this.animSetDataManager = animSDManager;
			this.animDataManager = animDManager;
		}


		public void AssembleAnimDataPatch(DirectoryInfo folder)
		{
			var files = folder.GetFiles();
			foreach (var file in files)
			{
				Project? targetProject;
				if (!file.Exists || !projectManager.TryGetProject(Path.GetFileNameWithoutExtension(file.Name.ToLower()), out targetProject)) continue;

				using (var readStream = file.OpenRead())
				{
					using (var reader = new StreamReader(readStream))
					{
						string? expectedLine;
						while ((expectedLine = reader.ReadLine()) != null)
						{
							if (String.IsNullOrWhiteSpace(expectedLine)) continue;
							targetProject!.AnimData?.AddDummyClipData(expectedLine);
						}
					}
				}
			}
		}
	}
}
