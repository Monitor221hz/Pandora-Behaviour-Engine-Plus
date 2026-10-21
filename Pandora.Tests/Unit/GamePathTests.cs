// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Core.Paths;

namespace PandoraTests.Unit;

public class GamePathTests
{
	[Fact]
	public void Join_Segments_UsesBackslashOnEveryPlatform()
	{
		var result = GamePath.Join("meshes", @"actors\character", "animations", "FNISZoo");

		Assert.Equal(@"meshes\actors\character\animations\FNISZoo", result);
	}

	[Fact]
	public void Join_SegmentEndingWithSeparator_DoesNotDoubleSeparator()
	{
		Assert.Equal(@"animations\Mod\x.hkx", GamePath.Join(@"animations\Mod\", "x.hkx"));
	}

	[Fact]
	public void FromPlatform_PlatformBuiltPath_ConvertsToBackslashes()
	{
		var platformPath = Path.Join("actors", "character");

		Assert.Equal(@"actors\character", GamePath.FromPlatform(platformPath));
	}

	[Fact]
	public void GetFileNameWithoutExtension_BackslashPath_ReturnsFileName()
	{
		Assert.Equal(
			"ChickenProject",
			GamePath.GetFileNameWithoutExtension(@"ChickenProjectData\ChickenProject.txt")
		);
	}

	[Fact]
	public void GetDirectoryName_BackslashPath_ReturnsBackslashDirectory()
	{
		Assert.Equal(
			@"Animations\FNISZoo",
			GamePath.GetDirectoryName(@"Animations\FNISZoo\zoo_idle.hkx")
		);
	}
}
