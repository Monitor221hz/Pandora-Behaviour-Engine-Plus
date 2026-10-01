// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

// SPDX-License-Identifier-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using GameFinder.RegistryUtils;
using Microsoft.Extensions.DependencyInjection;
using NexusMods.Paths;
using Pandora.Skyrim.CLI;
using Pandora.Core.Configuration;
using Pandora.Core.Logging;
using Pandora.Core.Engine;
using Pandora.Core.Mods;
using Pandora.Core.Paths;
using Pandora.Core.Patch;
using Pandora.Core.Settings;
using Pandora.Logging.Diagnostics;
using Pandora.Mods;
using Pandora.Models;
using Pandora.Platform;
using Pandora.Skyrim;
using Pandora.Skyrim.Configuration;
using Pandora.Themes;
using Pandora.ViewModels;
using Pandora.Views;
using System;

namespace Pandora;

public static class Services
{
	extension(IServiceCollection serviceCollection)
	{
		public IServiceCollection AddPandoraServices()
		{
			return serviceCollection
				.AddAppBootstrapper()
				.AddSettings()
				.AddLoggingServices()
				.AddAppExceptionHandler()
				.AddCLIServices()
				.AddPathServices()
				.AddBehaviourEngine()
				.AddPatchServices()
				.AddSkyrimPatchServices()
				.AddModServices()
				.AddConfigurationServices()
				.AddPlatformServices()
				.AddPages()
				.AddViewModels()
				.AddUtilViewModels()
				.AddCoreServices()
				.AddTheme()
				.AddEngineOrchestrator();
		}

		private IServiceCollection AddAppBootstrapper()
		{
			return serviceCollection.AddSingleton<AppBootstrapper>();
		}

		private IServiceCollection AddAppExceptionHandler()
		{
			return serviceCollection.AddSingleton<AppExceptionHandler>();
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
