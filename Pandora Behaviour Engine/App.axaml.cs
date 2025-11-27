// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.Globalization;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Microsoft.Extensions.DependencyInjection;
using Pandora.Logging;
using Pandora.Services;
using Pandora.Utils;
using Pandora.ViewModels;
using Pandora.Views;

namespace Pandora;

public partial class App : Application
{
	public override void Initialize()
	{
		AppExceptionHandler.Register();
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		SetupCultureInfo();
		SetupAppTheme();
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
			LaunchOptions.Parse(desktop.Args, caseInsensitive: true);
			SetupNLogConfig();
			// Line below is needed to remove Avalonia data validation.
			// Without this line you will get duplicate validations from both Avalonia and CT
			BindingPlugins.DataValidators.RemoveAt(0);
			desktop.MainWindow = new MainWindow { DataContext = new MainWindowViewModel() };
			var services = new ServiceCollection();
			services.AddPandoraServices(desktop)

			Services = services.BuildServiceProvider();
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

	private static void SetupNLogConfig()
	{
		var config = new NLog.Config.LoggingConfiguration();

		var fileTarget = new NLog.Targets.FileTarget("Engine Log")
		{
			FileName = Path.Combine(PandoraPaths.OutputPath.FullName, "Engine.log"),
			DeleteOldFileOnStartup = true,
			Layout = "${level:uppercase=true} : ${message}",
		};

		config.AddTarget(fileTarget);
		config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, fileTarget);

		NLog.LogManager.Configuration = config;
	}

	/// <summary>
	/// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
	/// </summary>
	public IServiceProvider? Services { get; private set; }
}
