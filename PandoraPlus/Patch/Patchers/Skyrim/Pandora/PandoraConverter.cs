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
	public class PandoraConverter
	{

		public PandoraAssembler Assembler { get; private set; }

		public PandoraConverter(ProjectManager projManager, AnimSetDataManager animSDManager, AnimDataManager animDManager) => Assembler = new PandoraAssembler(projManager, animSDManager, animDManager);	
		public void TryGenerateAnimDataPatchFile(DirectoryInfo folder)
		{
			var parentFolder = folder.Parent;
			if (parentFolder == null) return; 

			FileInfo patchFile = new FileInfo($"{parentFolder.FullName}\\{folder.Name.Split('~')[0]}.txt");
			if (patchFile.Exists) return;

			using (var writeStream = patchFile.OpenWrite())
			{
				using (var writer =  new StreamWriter(writeStream))
				{
					var files = folder.GetFiles();
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
