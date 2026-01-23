// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using GameFinder.RegistryUtils;
using Microsoft.Extensions.DependencyInjection;
using NexusMods.Paths;
using Pandora.CLI;
using Pandora.Configuration;
using Pandora.Logging;
using Pandora.Models;
using Pandora.Mods;
using Pandora.Paths;
using Pandora.Platform;
using Pandora.Settings;
using Pandora.ViewModels;
using Pandora.Views;
using System;

namespace Pandora;

public static class Services
{
	extension (IServiceCollection serviceCollection)
	{
		public IServiceCollection AddPandoraServices()
		{
			return serviceCollection
				.AddAppBootstrapper()
				.AddCoreServices()
				.AddSettings()
				.AddLoggingServices()
				.AddCLIServices()
				.AddPathServices()
				.AddBehaviourEngine()
				.AddPatchServices()
				.AddModServices()
				.AddConfigurationServices()
				.AddPlatformServices()
				.AddViewModels();
		}

		private IServiceCollection AddAppBootstrapper()
		{
			return serviceCollection.AddSingleton<AppBootstrapper>();
		}

		private IServiceCollection AddCoreServices()
		{
			if (OperatingSystem.IsWindows())
				serviceCollection.AddSingleton<IRegistry>(WindowsRegistry.Shared);

			return serviceCollection
				.AddSingleton<IFileSystem>(FileSystem.Shared)
				.AddSingleton<MainWindow>();
		}
	}
}