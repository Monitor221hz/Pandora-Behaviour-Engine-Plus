using NLog;
using NLog.Filters;
using NLog.Targets;
using NLog.Targets.Wrappers;
using Pandora.Paths.Abstractions;
using Pandora.Paths.Extensions;
using System;
using System.IO;
using System.Reactive.Linq;

namespace Pandora.Logging.Services;

public class NLogConfigurationService : ILoggingConfigurationService, IDisposable
{
	private readonly IUserPaths _userPaths;
	private readonly IDisposable? _subscription;

	public NLogConfigurationService(IUserPaths userPaths)
	{
		_userPaths = userPaths;

		_subscription = _userPaths.OutputChanged
			.Skip(1)
			.Subscribe(UpdateLogPath);
	}

	public void Initialize()
	{
		var logDir = _userPaths.Output.FullName;
		var logPath = logDir / "Engine.log";

		var fileTarget = new FileTarget("EngineLog")
		{
			FileName = logPath,
			DeleteOldFileOnStartup = true,
			Layout = "${level:uppercase=true} : ${message} ${exception:format=toString}"
		};

		var uiTarget = new ObservableNLogTarget
		{
			Name = "ui",
			Layout = "${message} ${exception:format=toString}"
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
	}

	private void UpdateLogPath(DirectoryInfo newFolder)
	{
		var config = LogManager.Configuration;
		var target = config?.FindTargetByName<FileTarget>("EngineLog");

		if (target is null)
			return;

		target.FileName = newFolder.FullName / "Engine.log";
		LogManager.ReconfigExistingLoggers();
	}

	public void Dispose()
	{
		_subscription?.Dispose();
	}
}