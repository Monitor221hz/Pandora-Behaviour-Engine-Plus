using Microsoft.Extensions.DependencyInjection;
using Pandora.Paths.Abstractions;
using Pandora.Paths.Validation;
using Pandora.Settings;

namespace Pandora.Paths;

public static class Services
{
	extension(IServiceCollection serviceCollection)
	{
		public IServiceCollection AddPathServices()
		{
			return serviceCollection
				.AddSingleton<IApplicationPaths, ApplicationPaths>()
				.AddSingleton<IOutputPaths, OutputPaths>()
				.AddSingleton<IUserPaths, UserPaths>()
				.AddSingleton<ISettingsRepository, SettingsRepository>()
				.AddSingleton<IEnginePathsFacade, EnginePathsFacade>()
				.AddSingleton<IGameDataValidator, GameDataValidator>();
		}
	}
}
