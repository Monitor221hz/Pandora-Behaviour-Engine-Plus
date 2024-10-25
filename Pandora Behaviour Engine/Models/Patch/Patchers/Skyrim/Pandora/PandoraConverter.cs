using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.AnimData;
using Pandora.Patch.Patchers.Skyrim.AnimSetData;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.Pandora
{
	public class PandoraConverter
	{

		public PandoraFragmentAssembler Assembler { get; private set; }

		public PandoraConverter(ProjectManager projManager, AnimSetDataManager animSDManager, AnimDataManager animDManager) => Assembler = new PandoraFragmentAssembler(projManager, animSDManager, animDManager);

		public void TryGraphInjection(DirectoryInfo folder, PackFile packFile, PackFileChangeSet changeSet)
		{
			DirectoryInfo injectFolder = new DirectoryInfo($"{folder.FullName}\\inject");
			if (!injectFolder.Exists) { return; }


			//Assembler.AssembleGraphInjection(injectFolder, packFile, changeSet);
		}

		
		public void TryGenerateAnimDataPatchFile(DirectoryInfo folder)
		{
			var parentFolder = folder.Parent;
			if (parentFolder == null) return;
			

			FileInfo patchFile = new FileInfo($"{parentFolder.FullName}\\{folder.Name.Split('~')[0]}.txt");
			if (patchFile.Exists) return;

			using (var writeStream = patchFile.Create())
			{
				using (var writer =  new StreamWriter(writeStream))
				{
					var files = folder.GetFiles("*~*.txt");
					foreach (var file in files)
					{
						var clipName = file.Name.Split('~')[0];

						if (clipName.Contains('$')) continue;

						writer.WriteLine(clipName);

					}
				}
			}

		}
	}
}
