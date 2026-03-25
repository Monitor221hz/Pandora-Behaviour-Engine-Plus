// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using ReactiveUI;
using System;

namespace Pandora.Views.Pages;

public class DIViewLocator(IServiceProvider provider) : IViewLocator
{
	public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
	{
		if (viewModel == null) return null;

		var viewType = typeof(IViewFor<>).MakeGenericType(viewModel.GetType());
		return provider.GetService(viewType) as IViewFor;
	}
}
