// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using ReactiveUI;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Pandora.Views.Pages;

public class DIViewLocator(IServiceProvider provider) : IViewLocator
{
    public IViewFor<TViewModel>? ResolveView<TViewModel>()
        where TViewModel : class
    {
        return ResolveView<TViewModel>(null);
    }

    public IViewFor<TViewModel>? ResolveView<TViewModel>(string? contract)
        where TViewModel : class
    {
        var viewType = typeof(IViewFor<TViewModel>);
        var service = provider.GetService(viewType);
        return service as IViewFor<TViewModel>;
    }

    [RequiresUnreferencedCode(
        "This method uses reflection to determine the view model type at runtime, which may be incompatible with trimming.")]
    [RequiresDynamicCode(
        "Trimming can't validate that the requirements of those annotations are met.")]
    public IViewFor? ResolveView(object? instance)
    {
        return ResolveView(instance, null);
    }

    [RequiresUnreferencedCode(
        "This method uses reflection to determine the view model type at runtime, which may be incompatible with trimming.")]
    [RequiresDynamicCode(
        "Trimming can't validate that the requirements of those annotations are met.")]
    public IViewFor? ResolveView(object? instance, string? contract)
    {
        if (instance == null)
            return null;

        var viewType = typeof(IViewFor<>).MakeGenericType(instance.GetType());
        return provider.GetService(viewType) as IViewFor;
    }
}