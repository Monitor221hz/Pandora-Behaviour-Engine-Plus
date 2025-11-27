// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Pandora.ViewModels;
using Pandora.Views;

namespace Pandora.Services;

public class PandoraServiceContext
{
	Window MainWindow { get; init; }
}

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddPandoraServices(this IServiceCollection serviceCollection)
	{
		return serviceCollection;
	}
}
