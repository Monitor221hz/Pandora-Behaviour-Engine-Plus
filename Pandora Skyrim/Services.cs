// SPDX-License-Identifier-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Microsoft.Extensions.DependencyInjection;
using Pandora.API.Patch.IOManagers;
using Pandora.API.Patch.Skyrim64;
using Pandora.API.Patch.Skyrim64.AnimData;
using Pandora.API.Patch.Skyrim64.AnimSetData;
using Pandora.Skyrim.AnimData;
using Pandora.Skyrim.AnimSetData;
using Pandora.Skyrim.Format.FNIS;
using Pandora.Skyrim.Format.Nemesis;
using Pandora.Skyrim.Format.Pandora;
using Pandora.Skyrim.Patch.IO;
using System;

namespace Pandora.Skyrim;

public static class Services
{
	extension(IServiceCollection serviceCollection)
	{
		public IServiceCollection AddSkyrimPatchServices()
		{
			return serviceCollection
#if DEBUG
				.AddSingleton<IMetaDataExporter<IPackFile>, DebugPackFileExporter>()
#else
				.AddSingleton<IMetaDataExporter<IPackFile>, PackFileExporter>()
#endif
				.AddScoped<IFNISParser, FNISParser>()
				.AddScoped<IProjectManager, ProjectManager>()
				.AddScoped<IAnimDataManager, AnimDataManager>()
				.AddScoped<IAnimSetDataManager, AnimSetDataManager>()
				.AddScoped<NemesisAssembler>()
				.AddScoped<PandoraAssembler>()
				.AddScoped<PandoraBridgedAssembler>()
				.AddScoped<IPatchAssembler>(sp => sp.GetRequiredService<NemesisAssembler>())
				.AddScoped<SkyrimPatcher>();
		}
	}
}
