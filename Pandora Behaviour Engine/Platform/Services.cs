using Microsoft.Extensions.DependencyInjection;
using Pandora.Platform.Avalonia;
using Pandora.Platform.CreationEngine;
using Pandora.Platform.CreationEngine.Game;
using Pandora.Platform.CreationEngine.Locators;
using Pandora.Platform.Windows;
using Pandora.Views;

namespace Pandora.Platform;

public static class Services
{
	extension(IServiceCollection serviceCollection)
	{
		public IServiceCollection AddPlatformServices()
		{
			return serviceCollection
				.AddSingleton<IDiskDialogService>(sp => new DiskDialogService(sp.GetRequiredService<MainWindow>()))
				.AddSingleton<IWindowStateService, WindowStateService>()
				.AddSingleton<IGameDescriptor, SkyrimDescriptor>()
				.AddTransient<CommandLineGameLocator>()
				.AddTransient<SteamGameLocator>()
				.AddTransient<GogGameLocator>()
				.AddTransient<RegistryGameLocator>()
				.AddSingleton<IGameLocator>(sp =>
					new CompositeGameLocator(
						[
							sp.GetRequiredService<CommandLineGameLocator>(),
							sp.GetRequiredService<SteamGameLocator>(),
							sp.GetRequiredService<GogGameLocator>(),
							sp.GetRequiredService<RegistryGameLocator>(),
						]
					)
				);
		}
	}
}
