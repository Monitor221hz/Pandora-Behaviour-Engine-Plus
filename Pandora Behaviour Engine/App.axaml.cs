// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Filters;
using NLog.Targets.Wrappers;
using Pandora.API.Utils;
using Pandora.Logging;
using Pandora.Services;
using Pandora.Utils;
using Pandora.ViewModels;
using Pandora.Views;
using System;
using System.Globalization;

namespace Pandora;

public partial class App : Application
{
	private AppExceptionHandler? _appExceptionHandler;

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
			LaunchOptions.Parse(desktop.Args, caseInsensitive: true);

			// Line below is needed to remove Avalonia data validation.
			// Without this line you will get duplicate validations from both Avalonia and CT
			BindingPlugins.DataValidators.RemoveAt(0);

			var serviceCollection = new ServiceCollection();
			var mainWindow = new MainWindow();

			serviceCollection.AddPandoraServices(new() { MainWindow = mainWindow });
			serviceCollection.AddViewModels();
			Services = serviceCollection.BuildServiceProvider();
			SetupNLogConfig();
			mainWindow.DataContext = Services.GetRequiredService<MainWindowViewModel>();
			desktop.MainWindow = mainWindow;
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

	private void SetupExceptionHandler()
	{
		_appExceptionHandler = new AppExceptionHandler(
			Services.GetRequiredService<IPathResolver>()
		);
	}

	private void SetupNLogConfig()
	{
		var initialPath = Services.GetRequiredService<IPathResolver>().GetOutputFolder().FullName;

		var fileTarget = new NLog.Targets.FileTarget("EngineLog")
		{
			FileName = "${var:LogDir}/Engine.log",
			DeleteOldFileOnStartup = true,
			Layout = "${level:uppercase=true} : ${message}"
		};

		var uiTarget = new ObservableNLogTarget
		{
			Name = "ui",
			Layout = "${message}"
		};

		var asyncUiTarget = new AsyncTargetWrapper(uiTarget)
		{
			Name = "uiAsync",
			QueueLimit = 5000,
			OverflowAction = AsyncTargetWrapperOverflowAction.Discard
		};

		LogManager.Setup()
			.SetupInternalLogger(builder => builder
				.LogToConsole(true)
				.SetMinimumLogLevel(LogLevel.Trace))
			.LoadConfiguration(builder =>
			{
				// File Logger
				builder.ForLogger()
					.FilterDynamic(new ConditionBasedFilter
					{
						Condition = "equals('${event-properties:ui}', true)",
						Action = FilterResult.Ignore
					}, filterDefaultAction: FilterResult.Log)
					.WriteTo(fileTarget);
				// UI Logger
				builder.ForLogger()
					.FilterDynamic(new ConditionBasedFilter
					{
						Condition = "equals('${event-properties:ui}', true)",
						Action = FilterResult.Log
					})
					.WriteTo(asyncUiTarget);
			});

		LogManager.Configuration.Variables["LogDir"] = initialPath;
		LogManager.ReconfigExistingLoggers();
	}

	public static new App? Current => Application.Current as App;

	/// <summary>
	/// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
	/// </summary>
	public IServiceProvider Services { get; private set; }
}
