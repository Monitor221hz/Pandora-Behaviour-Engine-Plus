using Pandora.API.Patch;
using Pandora.Models.Patch.Skyrim64.AnimData;
using Pandora.Models.Patch.Skyrim64.AnimSetData;
using Pandora.Models.Patch.Skyrim64.Hkx.Changes;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using System.IO;

namespace Pandora.Models.Patch.Skyrim64.Format.Pandora;

public class PandoraBridgedAssembler
{
	private PandoraAssembler assembler;

	public PandoraBridgedAssembler(ProjectManager projManager, AnimSetDataManager animSDManager, AnimDataManager animDManager) => assembler = new PandoraAssembler(projManager, animSDManager, animDManager);

	public void TryGraphInjection(DirectoryInfo folder, PackFile packFile, PackFileChangeSet changeSet)
	{
		DirectoryInfo injectFolder = new($"{folder.FullName}\\inject");
		if (!injectFolder.Exists) { return; }


		//Assembler.AssembleGraphInjection(injectFolder, packFile, changeSet);
	}
	public void AssembleAnimDataPatch(DirectoryInfo folder) => assembler.AssembleAnimDataPatch(folder);
	public void AssembleAnimSetDataPatch(DirectoryInfo directoryInfo) => assembler.AssembleAnimSetDataPatch(directoryInfo);
	public void QueueNativePatches() => assembler.QueueNativePatches();
	public void ApplyNativePatches(RuntimeMode mode, RunOrder order) => assembler.ApplyNativePatches(mode, order);

	public void TryGenerateAnimDataPatchFile(DirectoryInfo folder)
	{
		var parentFolder = folder.Parent;
		if (parentFolder == null) return;


		FileInfo patchFile = new($"{parentFolder.FullName}\\{folder.Name.Split('~')[0]}.txt");
		if (patchFile.Exists) return;

		using (var writeStream = patchFile.Create())
		{
			using (var writer = new StreamWriter(writeStream))
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