// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.IO;
using Pandora.API.Patch;
using Pandora.API.Patch.IOManagers;
using Pandora.API.Patch.Skyrim64;
using Pandora.API.Patch.Skyrim64.AnimData;
using Pandora.API.Patch.Skyrim64.AnimSetData;
using Pandora.API.Services;
using Pandora.Models.Patch.Skyrim64.AnimData;
using Pandora.Models.Patch.Skyrim64.AnimSetData;
using Pandora.Models.Patch.Skyrim64.Hkx.Changes;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

namespace Pandora.Models.Patch.Skyrim64.Format.Pandora;

public class PandoraBridgedAssembler
{
	private readonly PandoraAssembler _assembler;

	public PandoraBridgedAssembler(PandoraAssembler assembler)
	{
		_assembler = assembler;
	}

	public void TryGraphInjection(
		DirectoryInfo folder,
		PackFile packFile,
		PackFileChangeSet changeSet
	)
	{
		DirectoryInfo injectFolder = new($"{folder.FullName}\\inject");
		if (!injectFolder.Exists)
		{
			return;
		}

		//Assembler.AssembleGraphInjection(injectFolder, packFile, changeSet);
	}

	public void AssembleAnimDataPatch(DirectoryInfo folder) =>
		_assembler.AssembleAnimDataPatch(folder);

	public void AssembleAnimSetDataPatch(DirectoryInfo directoryInfo) =>
		_assembler.AssembleAnimSetDataPatch(directoryInfo);

	public void QueueNativePatches() => _assembler.QueueNativePatches();

	public void ApplyNativePatches(RuntimeMode mode, RunOrder order) =>
		_assembler.ApplyNativePatches(mode, order);

	public void TryGenerateAnimDataPatchFile(DirectoryInfo folder)
	{
		var parentFolder = folder.Parent;
		if (parentFolder == null)
			return;

		FileInfo patchFile = new($"{parentFolder.FullName}\\{folder.Name.Split('~')[0]}.txt");
		if (patchFile.Exists)
			return;

		using (var writeStream = patchFile.Create())
		{
			using (var writer = new StreamWriter(writeStream))
			{
				var files = folder.GetFiles("*~*.txt");
				foreach (var file in files)
				{
					var clipName = file.Name.Split('~')[0];

					if (clipName.Contains('$'))
						continue;

					writer.WriteLine(clipName);
				}
			}
		}
	}
}
