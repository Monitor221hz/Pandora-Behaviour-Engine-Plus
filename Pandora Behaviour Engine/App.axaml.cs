using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Pandora.Logging;
using Pandora.Utils;
using Pandora.ViewModels;
using Pandora.Views;
using System.Diagnostics;
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
			LaunchOptions.Current = LaunchOptions.Parse(desktop.Args);
			SetupNLogConfigForSingleFilePublish();
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
	private static void SetupNLogConfigForSingleFilePublish()
	{
		var configPath = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), "NLog.config");
		var config = new NLog.Config.XmlLoggingConfiguration(configPath);

		var fileTarget = config.FindTargetByName<NLog.Targets.FileTarget>("Engine Log");
		if (fileTarget != null)
		{
			fileTarget.FileName = NLog.Layouts.Layout.FromString(Path.Combine(PandoraPaths.OutputPath.FullName, "Engine.log"));
		}

		NLog.LogManager.Configuration = config;

	}
}
