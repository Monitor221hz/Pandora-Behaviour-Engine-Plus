// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using GameFinder.RegistryUtils;
using Microsoft.Extensions.DependencyInjection;
using NexusMods.Paths;
using Pandora.API;
using Pandora.API.Patch.Skyrim64;
using Pandora.API.Service;
using Pandora.Models;
using Pandora.Models.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Format.Nemesis;
using Pandora.Utils.Skyrim;
using Pandora.ViewModels;

namespace Pandora.Services;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddPandoraServices(
		this IServiceCollection serviceCollection,
		PandoraServiceContext context
	)
	{
		serviceCollection.AddSingleton<IFileSystem>(FileSystem.Shared);

		if (OperatingSystem.IsWindows())
		{
			serviceCollection.AddSingleton<IRegistry>(WindowsRegistry.Shared);
		}
		serviceCollection.AddSingleton<IDiskDialogService>(
			new DiskDialogService(context.MainWindow)
		);

		serviceCollection.AddTransient<IBehaviourEngine, BehaviourEngine>();
		serviceCollection.AddTransient<IProjectManager, ProjectManager>();
		serviceCollection.AddTransient<IPatchAssembler, NemesisAssembler>();

		serviceCollection.AddSingleton<IPathResolver, SkyrimPathResolver>();
		return serviceCollection;
	}

	public static IServiceCollection AddViewModels(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddSingleton<EngineViewModel>();
		serviceCollection.AddSingleton<MainWindowViewModel>();
		return serviceCollection;
	}
}
