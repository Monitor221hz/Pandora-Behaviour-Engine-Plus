// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Pandora.CLI;

public static class Services
{
	extension(IServiceCollection serviceCollection)
	{
		public IServiceCollection AddCLIServices()
		{
			return serviceCollection
				.AddSingleton<CommandLineParser>()
				.AddSingleton<LaunchOptions>(sp =>
				{
					var parser = sp.GetRequiredService<CommandLineParser>();
					return parser.Parse([.. Environment.GetCommandLineArgs().Skip(1)]);
				});
		}
	}
}
