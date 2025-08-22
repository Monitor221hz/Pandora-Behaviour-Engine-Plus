// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.Models;

namespace PandoraTests;

public static class Resources
{
	public static readonly DirectoryInfo TemplateDirectory = new(Path.Combine(Environment.CurrentDirectory, "Pandora_Engine", "Skyrim", "Template"));
	public static readonly DirectoryInfo OutputDirectory = new(Path.Combine(Environment.CurrentDirectory, "Output"));
	public static readonly DirectoryInfo CurrentDirectory = new(Environment.CurrentDirectory);

	static Resources()
	{
		OutputDirectory.Create();
		TemplateDirectory.Create();
	}
}
