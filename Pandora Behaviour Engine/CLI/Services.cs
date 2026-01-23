using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

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
