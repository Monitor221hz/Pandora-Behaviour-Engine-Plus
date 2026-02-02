// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using ReactiveUI;
using System;

namespace Pandora.Views.Pages.Factories;

public sealed class PageFactory(Func<string, IRoutableViewModel> factory)
{
	public IRoutableViewModel GetPage(string route) => factory.Invoke(route);
}
