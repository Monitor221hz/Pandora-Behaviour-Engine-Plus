// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿using Microsoft.Extensions.DependencyInjection;
using Pandora.API.Patch.IOManagers;
using Pandora.API.Patch.Skyrim64;
using Pandora.API.Patch.Skyrim64.AnimData;
using Pandora.API.Patch.Skyrim64.AnimSetData;
using Pandora.Models.Engine;
using Pandora.Models.Patch.IO.Skyrim64;
using Pandora.Models.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.AnimData;
using Pandora.Models.Patch.Skyrim64.AnimSetData;
using Pandora.Models.Patch.Skyrim64.Format.FNIS;
using Pandora.Models.Patch.Skyrim64.Format.Nemesis;
using Pandora.Models.Patch.Skyrim64.Format.Pandora;

namespace Pandora.Models;

public static class Services
{
	extension(IServiceCollection serviceCollection)
	{
		public IServiceCollection AddBehaviourEngine()
		{
			return serviceCollection
				.AddSingleton<IBehaviourEngine, BehaviourEngine>()
				.AddSingleton<IEngineStateMachine, EngineStateMachine>()
				.AddSingleton<IEngineRunner, EngineRunner>()
				.AddSingleton<EngineOrchestrator>();
		}

		public IServiceCollection AddPatchServices()
		{
			return serviceCollection
				.AddSingleton<IPatcherFactory, PatcherFactory>()
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
