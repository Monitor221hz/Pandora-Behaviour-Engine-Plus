// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Microsoft.Extensions.DependencyInjection;
using NexusMods.Paths;
using NSubstitute;
using Pandora.Core.Platform.CreationEngine;
using Pandora.Core.Platform.CreationEngine.Locators;

namespace PandoraTests.Unit;

public class GameLocatorTests
{
	[Fact]
	public void Locators_WithoutRegistry_CanBeResolved()
	{
		// IRegistry is only registered on Windows.
		using var provider = new ServiceCollection()
			.AddSingleton(Substitute.For<IGameDescriptor>())
			.AddSingleton<IFileSystem>(FileSystem.Shared)
			.AddTransient<SteamGameLocator>()
			.AddTransient<GogGameLocator>()
			.AddTransient<RegistryGameLocator>()
			.BuildServiceProvider();

		Assert.NotNull(provider.GetRequiredService<SteamGameLocator>());
		Assert.NotNull(provider.GetRequiredService<GogGameLocator>());
		Assert.NotNull(provider.GetRequiredService<RegistryGameLocator>());
	}
}
