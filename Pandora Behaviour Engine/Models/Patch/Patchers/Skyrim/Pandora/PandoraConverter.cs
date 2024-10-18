using Pandora.Core;
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


		public void TryGenerateAnimDataPatchFile(IModInfo modInfo, DirectoryInfo folder)
		{
			var parentFolder = folder.Parent;
			if (parentFolder == null) return;


			FileInfo patchFile = new FileInfo($"{parentFolder.FullName}\\{folder.Name.Split('~')[0]}.txt");
			if (patchFile.Exists)
			{
				using (var readStream = patchFile.OpenRead())
				{
					using (var reader = new StreamReader(readStream))
					{
						if (Version.TryParse(reader.ReadLine(), out var version))
						{
							if (modInfo.Version == version)
							{
								return;
							}
						}
					}
				}


				using (var writeStream = patchFile.Create())
				{
					using (var writer = new StreamWriter(writeStream))
					{
						writer.WriteLine(modInfo.Version);
						var files = folder.GetFiles("*.txt");
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
}
