// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Microsoft.Extensions.DependencyInjection;
using Pandora.Core.Logging.Diagnostics;
using Pandora.Core.Logging.NLogger;
using Pandora.Core.Logging.NLogger.Abstractions;
using Pandora.Core.Logging.NLogger.Environment;
using Pandora.Core.Logging.NLogger.UI;

namespace Pandora.Core.Logging;

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
				.AddSingleton<CrashReporter>()
				.AddSingleton<CrashLogBuilder>()
				.AddSingleton<CrashLogWriter>();
		}
	}
}