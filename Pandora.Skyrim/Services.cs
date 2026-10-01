// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

// SPDX-License-Identifier-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using Microsoft.Extensions.DependencyInjection;
using Pandora.API.Patch.IOManagers;
using Pandora.API.Patch.Skyrim64;
using Pandora.API.Patch.Skyrim64.AnimData;
using Pandora.API.Patch.Skyrim64.AnimSetData;
using Pandora.Skyrim.AnimData;
using Pandora.Skyrim.AnimSetData;
using Pandora.Skyrim.CLI;
using Pandora.Skyrim.Format.FNIS;
using Pandora.Skyrim.Format.Nemesis;
using Pandora.Skyrim.Format.Pandora;
using Pandora.Skyrim.Patch.IO;

namespace Pandora.Skyrim;

public static class Services
{
	public static IServiceCollection AddSkyrimPatchServices(
		this IServiceCollection serviceCollection
	)
	{
		{
			return serviceCollection
				.AddSingleton<DebugPackFileExporter>()
				.AddSingleton<PackFileExporter>()
				.AddSingleton<IMetaDataExporter<IPackFile>>(sp =>
				{
					var options = sp.GetService<LaunchOptions>();
#if DEBUG
					return sp.GetRequiredService<DebugPackFileExporter>();
#else
					if (options is { UseSkyrimDebug64: true })
					{
						return sp.GetRequiredService<DebugPackFileExporter>();
					}
					return sp.GetRequiredService<PackFileExporter>();
#endif
				})
				.AddScoped<IFNISParser, FNISParser>()
				.AddScoped<IProjectManager, ProjectManager>()
				.AddScoped<IAnimDataManager, AnimDataManager>()
				.AddScoped<IAnimSetDataManager, AnimSetDataManager>()
				.AddScoped<NemesisAssembler>()
				.AddScoped<PandoraAssembler>()
				.AddScoped<PandoraBridgedAssembler>()
				.AddScoped<IPatchAssembler>(sp => sp.GetRequiredService<NemesisAssembler>())
				.AddScoped<SkyrimPatcher>()
				.AddScoped<NemesisPandoraConverter>()
				.AddScoped<PandoraTemplatePacker>();
		}
	}
}
