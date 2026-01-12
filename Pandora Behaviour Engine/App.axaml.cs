// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Microsoft.Extensions.DependencyInjection;
using Pandora.API.Services;
using Pandora.Models.Patch.Plugins;
using Pandora.Services;
using Pandora.Utils;
using Pandora.ViewModels;
using Pandora.Views;
using System;
using System.Globalization;
using System.Threading.Tasks;

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

			var mainWindow = Services.GetRequiredService<MainWindow>();
			mainWindow.DataContext = Services.GetRequiredService<MainWindowViewModel>();

			desktop.MainWindow = mainWindow;


			_ = InitializeApplicationAsync();
		}

		base.OnFrameworkInitializationCompleted();
	}

	private async Task InitializeApplicationAsync()
	{
		var modService = Services.GetRequiredService<IModService>();
		var engine = Services.GetRequiredService<IBehaviourEngine>();

		var logConfig = Services.GetRequiredService<ILoggingConfigurationService>();
		var exceptionHandler = Services.GetRequiredService<IAppExceptionHandler>();

		try
		{
			logConfig.Configure();
			exceptionHandler.Initialize();

			if (PluginManager.EngineConfigurations.Count > 0)
			{
				logger.UiInfo("Plugins loaded.");
			}

			var loadModsTask = modService.RefreshModsAsync();
			var initEngineTask = engine.InitializeAsync();

			await Task.WhenAll(loadModsTask, initEngineTask);
		}
		catch (Exception ex)
		{
			logger.Fatal(ex, $"Startup failed");
		}
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

	public static new App? Current => Application.Current as App;

	/// <summary>
	/// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
	/// </summary>
	public IServiceProvider Services { get; private set; }

	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
}
