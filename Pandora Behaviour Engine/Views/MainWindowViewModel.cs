// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using FluentAvalonia.UI.Controls;
using Pandora.Settings;
using Pandora.Views.Pages.DTOs;
using Pandora.Views.Pages.Factories;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Pandora.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IScreen, IActivatableViewModel
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private readonly PageFactory _pageFactory;
	private readonly ISettingsService _settingsService;

	public RoutingState Router { get; } = new();
	public ViewModelActivator Activator { get; } = new();

	public IReadOnlyList<NavigationItem> NavigationItems { get; private set; }
	public IReadOnlyList<NavigationItem> FooterNavigationItems { get; private set; }

	[Reactive]
	private NavigationItem? _selectedMenuItem;

	public EngineMenuViewModel EngineMenuVM { get; }

	public MainWindowViewModel(
		PageFactory pageFactory,
		EngineMenuViewModel engineMenuVM,
		ISettingsService settingsService)
	{
		_pageFactory = pageFactory;
		EngineMenuVM = engineMenuVM;
		_settingsService = settingsService;

		var engineIcon = GetResource<StreamGeometry>("IconPandoraOutline");
		var settingsIcon = new SymbolIconSource { Symbol = Symbol.Setting };
		var infoIcon = GetResource<FontIconSource>("Info") ?? new FontIconSource { Glyph = "i" };

		NavigationItems =
		[
			new NavigationItem("Engine", new PathIconSource { Data = engineIcon }, Routes.Engine)
		];

		FooterNavigationItems =
		[
			new NavigationItem("About", infoIcon, Routes.About),
			new NavigationItem("Settings", settingsIcon, Routes.Settings),
		];

		SetInitialPage();

		this.WhenActivated(disposables =>
		{
			this.WhenAnyValue(x => x.SelectedMenuItem)
				.WhereNotNull()
				.Select(x => x.Route)
				.DistinctUntilChanged()
				.InvokeCommand(NavigateToUriCommand)
				.DisposeWith(disposables);

			Router.CurrentViewModel
				.WhereNotNull()
				.Select(vm => FindMenuItemByRoute(vm.UrlPathSegment))
				.WhereNotNull()
				.BindTo(this, x => x.SelectedMenuItem)
				.DisposeWith(disposables);
		});
	}

	private void SetInitialPage()
	{
		string targetRoute;

		if (_settingsService.Paths.NeedsUserSelection)
		{
			targetRoute = Routes.Settings;
		}
		else
		{
			targetRoute = Routes.Engine;
		}

		SelectedMenuItem = FindMenuItemByRoute(targetRoute) ?? NavigationItems.FirstOrDefault();
	}

	private static T? GetResource<T>(string key) where T : class
	{
		if (Application.Current?.TryFindResource(key, out var resource) == true && resource is T tResource)
		{
			return tResource;
		}
		return null;
	}

	private NavigationItem? FindMenuItemByRoute(string? route)
	{
		if (string.IsNullOrEmpty(route)) return null;
		return NavigationItems.Concat(FooterNavigationItems)
							  .FirstOrDefault(i => i.Route == route);
	}

	[ReactiveCommand]
	private async Task NavigateToUri(string uri)
	{
		try
		{
			var viewModel = _pageFactory.GetPage(uri);
			if (viewModel == null)
			{
				logger.Warn($"[Router] Page not found for URI: '{uri}'");
				return;
			}

			await Router.Navigate.Execute(viewModel);
		}
		catch (Exception ex)
		{
			logger.Error(ex, $"Error navigating to page '{uri}'");
		}
	}
}
