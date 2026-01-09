// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Microsoft.Extensions.DependencyInjection;
using Pandora.API.Services;
using Pandora.Logging;
using Pandora.Models.Patch.Plugins;
using Pandora.Services;
using Pandora.ViewModels;
using Pandora.Views;
using System;
using System.Globalization;
using System.IO;
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
			serviceCollection.AddViewModels();
			
			Services = serviceCollection.BuildServiceProvider();

			var exceptionHandler = Services.GetRequiredService<IAppExceptionHandler>();
			exceptionHandler.Initialize();

			SetupNLogConfig();

			if (PluginManager.EngineConfigurations.Count > 0)
			{
				EngineLoggerAdapter.AppendLine("Plugins loaded.");
			}

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

		try
		{
			var loadModsTask = modService.RefreshModsAsync();
			var initEngineTask = engine.InitializeAsync();

			await Task.WhenAll(loadModsTask, initEngineTask);
		}
		catch (Exception ex)
		{
			logger.Fatal(ex, "Startup failed");
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

	private void SetupNLogConfig()
	{
		var config = new NLog.Config.LoggingConfiguration();

		var fileTarget = new NLog.Targets.FileTarget("Engine Log")
		{
			FileName = Path.Combine(
				Services.GetRequiredService<IPathResolver>().GetOutputFolder().FullName,
				"Engine.log"
			),
			DeleteOldFileOnStartup = true,
			Layout = "${level:uppercase=true} : ${message}",
		};

		config.AddTarget(fileTarget);
		config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, fileTarget);

		NLog.LogManager.Configuration = config;
	}

	public static new App? Current => Application.Current as App;

	/// <summary>
	/// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
	/// </summary>
	public IServiceProvider Services { get; private set; }

	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
}
