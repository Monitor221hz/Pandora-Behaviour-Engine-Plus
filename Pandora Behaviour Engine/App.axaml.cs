// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Microsoft.Extensions.DependencyInjection;
using Pandora.Services;
using Pandora.Services.Interfaces;
using Pandora.ViewModels;
using Pandora.Views;
using System;
using System.Globalization;

namespace Pandora;

public partial class App : Application
{
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		SetupCultureInfo();
		SetupAppTheme();
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			// Line below is needed to remove Avalonia data validation.
			// Without this line you will get duplicate validations from both Avalonia and CT
			BindingPlugins.DataValidators.RemoveAt(0);

			var serviceCollection = new ServiceCollection();

			serviceCollection.AddPandoraServices();
			
			Services = serviceCollection.BuildServiceProvider();

			var bootstrapper = Services.GetRequiredService<IAppBootstrapper>();

			bootstrapper.InitializeSync();

			var mainWindow = Services.GetRequiredService<MainWindow>();
			mainWindow.DataContext = Services.GetRequiredService<MainWindowViewModel>();

			desktop.MainWindow = mainWindow;

			_ = bootstrapper.InitializeAsync();
		}

		base.OnFrameworkInitializationCompleted();
	}

	private static void SetupCultureInfo()
	{
		CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

		CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");

		CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
	}

	private static void SetupAppTheme()
	{
		var theme = Properties.GUISettings.Default.AppTheme;

		Application.Current!.RequestedThemeVariant = theme switch
		{
			0 => ThemeVariant.Light,
			1 => ThemeVariant.Dark,
			_ => ThemeVariant.Default,
		};
	}

	/// <summary>
	/// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
	/// </summary>
	public IServiceProvider Services { get; private set; }
}
