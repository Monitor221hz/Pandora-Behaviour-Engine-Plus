// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

﻿// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Microsoft.Extensions.DependencyInjection;

namespace Pandora.ViewModels;

public static class Services
{
	extension(IServiceCollection serviceCollection)
	{
		public IServiceCollection AddViewModels()
		{
			return serviceCollection
				.AddSingleton<IEngineSharedState, EngineSharedState>()
				.AddTransient<EngineMenuViewModel>()
				.AddTransient<PatchBoxViewModel>()
				.AddTransient<LogBoxViewModel>()
				.AddTransient<LaunchElementViewModel>()
				.AddTransient<SettingsViewModel>()
				.AddTransient<DataGridOptionsViewModel>()
				.AddTransient<AboutDialogViewModel>()
				.AddTransient<EngineViewModel>()
				.AddTransient<MainWindowViewModel>();
		}
	}
}