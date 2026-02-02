// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Microsoft.Extensions.DependencyInjection;
using Pandora.ViewModels;
using Pandora.Views.Pages;
using Pandora.Views.Pages.DTOs;
using Pandora.Views.Pages.Factories;
using ReactiveUI;
using System;

namespace Pandora.Views;

public static class Services
{
	extension(IServiceCollection serviceCollection)
	{
		public IServiceCollection AddViewModels()
		{
			return serviceCollection
				.AddSingleton<MainWindowViewModel>()
				.AddSingleton<IScreen>(x => x.GetRequiredService<MainWindowViewModel>())
				.AddSingleton<EngineMenuViewModel>()
				.AddSingleton<PatchBoxViewModel>()
				.AddSingleton<LogBoxViewModel>()
				.AddSingleton<LaunchElementViewModel>()
				.AddSingleton<EnginePageViewModel>()
				.AddSingleton<AboutPageViewModel>()
				.AddSingleton<SettingsPageViewModel>();
		}

		public IServiceCollection AddPages()
		{
			return serviceCollection
				.AddSingleton<PageFactory>()
				.AddSingleton<Func<string, IRoutableViewModel>>(sp => uri => uri switch
				{
					Routes.Engine => sp.GetRequiredService<EnginePageViewModel>(),
					Routes.Settings => sp.GetRequiredService<SettingsPageViewModel>(),
					Routes.About => sp.GetRequiredService<AboutPageViewModel>(),
					_ => throw new InvalidOperationException($"Page '{uri}' don't registry")
				})
				.AddTransient<IViewFor<EnginePageViewModel>, EnginePage>()
				.AddTransient<IViewFor<AboutPageViewModel>, AboutPage>()
				.AddTransient<IViewFor<SettingsPageViewModel>, SettingsPage>();
		}
	}
}