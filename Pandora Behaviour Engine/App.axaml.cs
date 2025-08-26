// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Pandora.Logging;
using Pandora.Utils;
using Pandora.ViewModels;
using Pandora.Views;
using System.Globalization;
using System.IO;

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
			desktop.MainWindow = new MainWindow
			{
				DataContext = new MainWindowViewModel(),
			};
		}

		base.OnFrameworkInitializationCompleted();
	}
	private static void SetupCultureInfo()
	{
		CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

		CultureInfo.DefaultThreadCurrentCulture = culture;
		CultureInfo.DefaultThreadCurrentUICulture = culture;
		CultureInfo.CurrentCulture = culture;
	}
	private static void SetupAppTheme()
	{
		var theme = Properties.GUISettings.Default.AppTheme;

		Application.Current!.RequestedThemeVariant = theme switch
		{
			0 => ThemeVariant.Light,
			1 => ThemeVariant.Dark,
			_ => ThemeVariant.Default
		};
	}
	private static void SetupNLogConfig()
	{
		var config = new NLog.Config.LoggingConfiguration();

		var fileTarget = new NLog.Targets.FileTarget("Engine Log")
		{
			FileName = Path.Combine(PandoraPaths.OutputPath.FullName, "Engine.log"),
			DeleteOldFileOnStartup = true,
			Layout = "${level:uppercase=true} : ${message}"
		};

		config.AddTarget(fileTarget);
		config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, fileTarget);

		NLog.LogManager.Configuration = config;
	}

}
