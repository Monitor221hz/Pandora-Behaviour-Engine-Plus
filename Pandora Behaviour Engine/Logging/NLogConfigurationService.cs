using NLog;
using NLog.Filters;
using NLog.Targets;
using NLog.Targets.Wrappers;
using Pandora.Services.Interfaces;
using Pandora.Utils;

namespace Pandora.Logging;

public class NLogConfigurationService : ILoggingConfigurationService
{
	private readonly IPathResolver _pathResolver;

	public NLogConfigurationService(IPathResolver pathResolver)
	{
		_pathResolver = pathResolver;
	}

	public void Configure()
	{
		var logDir = _pathResolver.GetOutputFolder().FullName;
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

	public void UpdateLogPath(string newDirectory)
	{
		var config = LogManager.Configuration;
		var target = config?.FindTargetByName<FileTarget>("EngineLog");

		if (target is not null)
		{
			var newPath = newDirectory / "Engine.log";
			target.FileName = newPath;
			LogManager.ReconfigExistingLoggers();
		}
	}
}