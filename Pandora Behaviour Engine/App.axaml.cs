using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Pandora.ViewModels;
using Pandora.Views;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Pandora;

public partial class App : Application
{
	public override void Initialize()
	{
		// Register the custom exception handlers to log unhandled exceptions.
		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		SetupCultureInfo();

		var themeName = Properties.GUISettings.Default.AppTheme;

		Application.Current!.RequestedThemeVariant = themeName switch
		{
			0 => ThemeVariant.Light,
			1 => ThemeVariant.Dark,
			_ => ThemeVariant.Default,
		};

		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
		{
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

	/// <summary>
	/// Write unspecified crashes to the log.
	///
	/// Without this, if, for example, a file inside `Pandora_Engine` is missing for some reason, it will crash without even writing to the log.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>

	private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		var ex = e.ExceptionObject as Exception;
		// NOTE:
		// When using /p:IncludeAllContentForSelfExtract=true -> EXE runs from temp dir
		// => Use `Environment.CurrentDirectory`:  Current exe dir
		// => Use `Directory.GetCurrentDirectory()`: Tmp dir! -> template read fails!
		var currentDir = Environment.CurrentDirectory;
		var exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName ?? "unknown";

		var log = new StringBuilder();
		log.AppendLine("[ Pandora Critical Crash Log ]");
		log.AppendLine("=======================================");
		log.AppendLine($"Environment.CurrentDirectory: {currentDir}");
		log.AppendLine($"Process Executable Path: {exePath}");
		log.AppendLine();
		log.AppendLine("UnhandledException:");
		log.AppendLine(ex?.ToString() ?? "ExceptionObject is null");
		log.AppendLine("=======================================");

		var fileName = "Pandora_CriticalCrash_UnhandledException.log";
		File.WriteAllText(fileName, log.ToString());
	}

	/// <summary>
	/// Catches exceptions when unhandled exceptions occur in async fn.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private static void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
	{
		var log = new StringBuilder();
		log.AppendLine("[ Pandora Critical Crash Log ]");
		log.AppendLine("=======================================");
		log.AppendLine("UnobservedTaskException:");
		log.AppendLine(e.Exception.ToString());
		log.AppendLine("=======================================");

		var fileName = "Pandora_Critical_Crash_UnobservedTaskException.log";
		File.WriteAllText(fileName, log.ToString());
		e.SetObserved();
	}
}
