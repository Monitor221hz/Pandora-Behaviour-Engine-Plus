// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Microsoft.Extensions.DependencyInjection;
using Pandora.Core.Paths.Abstractions;
using Pandora.Core.Paths.Validation;

namespace Pandora.Core.Paths;

public static class Services
{
	extension(IServiceCollection serviceCollection)
	{
		public IServiceCollection AddPathServices()
		{
			return serviceCollection
				.AddSingleton<IApplicationPaths, ApplicationPaths>()
				.AddSingleton<IOutputPaths, OutputPaths>()
				.AddSingleton<IUserPaths, UserPaths>()
				.AddSingleton<IEnginePathsFacade, EnginePathsFacade>()
				.AddSingleton<IGameDataValidator, GameDataValidator>();
		}
	}
}