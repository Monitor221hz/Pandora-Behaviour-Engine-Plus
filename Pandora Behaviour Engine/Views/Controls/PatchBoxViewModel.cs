// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Avalonia.Controls;
using DynamicData;
using DynamicData.Binding;
using Pandora.Mods.Abstractions;
using Pandora.Mods.Extensions;
using Pandora.Utils;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

namespace Pandora.ViewModels;

public partial class PatchBoxViewModel : ViewModelBase, IActivatableViewModel
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private readonly IModService _modService;

	public IEngineSharedState State { get; }

	[Reactive]
	private bool _isSearchVisible;

	[BindableDerivedList]
	private readonly ReadOnlyObservableCollection<ModInfoViewModel> _modViewModels;

	[ObservableAsProperty(ReadOnly = false)]
	private bool? _allSelected;

	public DataGridOptionsViewModel DataGridOptions { get; }

	public ViewModelActivator Activator { get; } = new();

	public PatchBoxViewModel(
		IModService modService,
		IEngineSharedState state,
		DataGridOptionsViewModel dataGridOptions)
	{
		_modService = modService;

		State = state;
		DataGridOptions = dataGridOptions;

		_modService
		   .Connect()
		   .AutoRefresh(x => x.Priority)
		   .Filter(this.WhenAnyValue(x => x.State.SearchTerm).Select(ModViewModelExtensions.BuildNameFilter))
		   .Sort(SortExpressionComparer<ModInfoViewModel>.Ascending(x => x.Priority))
		   .Bind(out _modViewModels)
		   .Subscribe();

		this.WhenActivated(disposables =>
		{
			_allSelectedHelper = _modService
				.Connect()
				.AutoRefresh(x => x.Active)
				.ToCollection()
				.Select(items => ModViewModelExtensions.AreAllNonPandoraModsSelected(items))
				.DistinctUntilChanged()
				.ToProperty(this, x => x.AllSelected)
				.DisposeWith(disposables);

			this.WhenAnyValue(x => x.IsSearchVisible)
				.Where(isVisible => !isVisible)
				.Subscribe(_ =>
				{
					State.SearchTerm = string.Empty;
				})
				.DisposeWith(disposables);
		});
	}

	[ReactiveCommand]
	private void ToggleSelectAll(bool? isChecked)
	{
		if (isChecked is not bool check)
			return;

		_modViewModels.SetAllActive(check);
	}

	[ReactiveCommand]
	private void SortAscending(DataGridColumnHeader? header) =>
		DataGridUtils.ApplySort(header, c => c.Sort(ListSortDirection.Ascending));

	[ReactiveCommand]
	private void SortDescending(DataGridColumnHeader? header) =>
		DataGridUtils.ApplySort(header, c => c.Sort(ListSortDirection.Descending));

	[ReactiveCommand]
	private void ClearSort(DataGridColumnHeader? header) =>
		DataGridUtils.ApplySort(header, c => c.ClearSort());

	[ReactiveCommand]
	private void HideSearch() => IsSearchVisible = false;
}
