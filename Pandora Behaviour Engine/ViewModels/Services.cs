// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Microsoft.Extensions.DependencyInjection;

namespace Pandora.ViewModels;

public static class Services
{
	extension(IServiceCollection serviceCollection)
	{
		public IServiceCollection AddUtilViewModels()
		{
			return serviceCollection
				.AddSingleton<IEngineSharedState, EngineSharedState>()
				.AddTransient<DataGridOptionsViewModel>();
		}
	}
}