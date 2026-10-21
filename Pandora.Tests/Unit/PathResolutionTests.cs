// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Core.Paths.Extensions;

namespace PandoraTests.Unit;

public sealed class PathResolutionTests : IDisposable
{
	private readonly DirectoryInfo _root = Directory.CreateTempSubdirectory("pandora-paths-");

	public PathResolutionTests()
	{
		// Casing as a mod packed on Windows might ship it.
		var animations = _root.CreateSubdirectory(
			Path.Join("Meshes", "Actors", "Character", "Animations", "FNISZoo")
		);
		File.WriteAllText(Path.Join(animations.FullName, "FNIS_FNISZoo_List.TXT"), "");
	}

	public void Dispose() => _root.Delete(recursive: true);

	[Fact]
	public void ResolveFile_DifferentCaseAndBackslashes_FindsExistingFile()
	{
		var file = _root.ResolveFile(
			@"meshes\actors\character\animations\FNISZoo\FNIS_FNISZoo_List.txt"
		);

		Assert.True(file.Exists);
	}

	[Fact]
	public void ResolveDirectory_DifferentCase_FindsExistingDirectory()
	{
		var directory = _root.ResolveDirectory(
			Path.Join("meshes", "actors", "character", "animations")
		);

		Assert.True(directory.Exists);
		Assert.Equal("Animations", directory.Name);
	}

	[Fact]
	public void ResolveFile_MissingFile_KeepsMatchedPrefixAndRemainingSegments()
	{
		var file = _root.ResolveFile(Path.Join("meshes", "patches", "new.xml"));

		Assert.False(file.Exists);
		Assert.Equal(Path.Join(_root.FullName, "Meshes", "patches", "new.xml"), file.FullName);
	}
}
