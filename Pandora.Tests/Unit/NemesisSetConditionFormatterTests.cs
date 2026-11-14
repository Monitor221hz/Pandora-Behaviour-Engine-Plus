// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Pandora.Models.Patch.Skyrim64.Format.Nemesis;

namespace PandoraTests.Unit;

public class NemesisSetConditionFormatterTests
{
	[Fact]
	public void EditInsideAConditionMovesOntoTheWholeCondition()
	{
		string[] lines =
		[
			"V3",
			"3",
			"ForceEquipNoWeapChange",
			"weapEquip",
			"weapForceEquip",
			"2",
			"iLeftHandType",
			"0",
			"0",
			"iRightHandType",
			"1",
			"<!-- MOD_CODE ~xfdda~ OPEN -->",
			"2",
			"<!-- ORIGINAL -->",
			"1",
			"<!-- CLOSE -->",
			"7",
			"attackStart1HMBackSlash",
		];

		Assert.True(NemesisSetConditionFormatter.TryReformat(lines, out List<string> reformatted));

		Assert.Equal(
			[
				"V3",
				"3",
				"ForceEquipNoWeapChange",
				"weapEquip",
				"weapForceEquip",
				"2",
				"iLeftHandType",
				"0",
				"0",
				"<!-- MOD_CODE ~xfdda~ OPEN -->",
				"iRightHandType",
				"1",
				"2",
				"<!-- ORIGINAL -->",
				"iRightHandType",
				"1",
				"1",
				"<!-- CLOSE -->",
				"7",
				"attackStart1HMBackSlash",
			],
			reformatted
		);
	}

	[Fact]
	public void SharedTailStaysOnBothSidesOfTheCondition()
	{
		string[] lines =
		[
			"V3",
			"1",
			"weapEquip",
			"1",
			"iLeftHandType",
			"<!-- MOD_CODE ~xfdda~ OPEN -->",
			"5",
			"<!-- ORIGINAL -->",
			"0",
			"<!-- CLOSE -->",
			"0",
			"3",
		];

		Assert.True(NemesisSetConditionFormatter.TryReformat(lines, out List<string> reformatted));
		Assert.Equal(
			[
				"V3",
				"1",
				"weapEquip",
				"1",
				"<!-- MOD_CODE ~xfdda~ OPEN -->",
				"iLeftHandType",
				"5",
				"0",
				"<!-- ORIGINAL -->",
				"iLeftHandType",
				"0",
				"0",
				"<!-- CLOSE -->",
				"3",
			],
			reformatted
		);
	}

	[Fact]
	public void AlignedConditionEditIsLeftAlone()
	{
		string[] lines =
		[
			"V3",
			"1",
			"weapEquip",
			"1",
			"<!-- MOD_CODE ~xfdda~ OPEN -->",
			"iRightHandType",
			"1",
			"2",
			"<!-- ORIGINAL -->",
			"iRightHandType",
			"1",
			"1",
			"<!-- CLOSE -->",
			"0",
		];

		Assert.False(NemesisSetConditionFormatter.TryReformat(lines, out _));
	}

	[Fact]
	public void ReformatModRewritesTheSetFile()
	{
		var root = new DirectoryInfo(
			Path.Combine(Path.GetTempPath(), $"PandoraReformat_{Guid.NewGuid():N}")
		);
		var set = new DirectoryInfo(
			Path.Combine(
				root.FullName,
				"animationsetdatasinglefile",
				"DraugrProjectData~DraugrProject"
			)
		);
		set.Create();
		string path = Path.Combine(set.FullName, "Draugr_1HMSword.txt");
		File.WriteAllText(
			path,
			"V3\n1\nweapEquip\n1\niRightHandType\n1\n<!-- MOD_CODE ~xfdda~ OPEN -->\n2\n<!-- ORIGINAL -->\n1\n<!-- CLOSE -->\n0\n"
		);

		try
		{
			Assert.Equal(1, NemesisSetConditionFormatter.ReformatMod(root));
			string text = File.ReadAllText(path);
			Assert.Contains("<!-- MOD_CODE ~xfdda~ OPEN -->\niRightHandType\n1\n2\n", text);
			Assert.Contains("<!-- ORIGINAL -->\niRightHandType\n1\n1\n", text);
			Assert.Equal(0, NemesisSetConditionFormatter.ReformatMod(root));
		}
		finally
		{
			root.Delete(recursive: true);
		}
	}
}
