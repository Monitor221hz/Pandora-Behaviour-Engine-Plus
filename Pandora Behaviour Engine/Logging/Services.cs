using Microsoft.Extensions.DependencyInjection;
using Pandora.Logging.Diagnostics;
using Pandora.Logging.NLogger;
using Pandora.Logging.NLogger.Abstractions;
using Pandora.Logging.NLogger.Environment;
using Pandora.Logging.NLogger.UI;

namespace Pandora.Logging;

public static class Services
{
	extension(IServiceCollection serviceCollection)
	{
		public IServiceCollection AddLoggingServices()
		{
			return serviceCollection
				.AddSingleton<ILogEventStream, LogEventStream>()
				.AddSingleton<ObservableNLogTarget>()
				.AddSingleton<ILogPathProvider, UserLogPathProvider>()
				.AddSingleton<INLogTargetsFactory, NLogTargetsFactory>()
				.AddSingleton<INLogConfigurator, NLogConfigurator>()
				.AddSingleton<LogFilePathUpdater>()
				.AddSingleton<LoggingBootstrapper>()
				.AddSingleton<AppExceptionHandler>()
				.AddSingleton<CrashReporter>()
				.AddSingleton<CrashLogBuilder>()
				.AddSingleton<CrashLogWriter>();
		}
	}
}
