// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using NSubstitute;
using Pandora.API.Patch;
using Pandora.API.Patch.Skyrim64;
using Pandora.API.Patch.Skyrim64.AnimSetData;
using Pandora.Core.Paths.Abstractions;
using Pandora.Models.Patch.Skyrim64.Format.Nemesis;
using Pandora.Skyrim.AnimData;
using Pandora.Skyrim.AnimSetData;

namespace PandoraTests.Unit;

public class NemesisCachePatchTests : IDisposable
{
	private readonly DirectoryInfo _output;

	public NemesisCachePatchTests()
	{
		_output = new DirectoryInfo(
			Path.Combine(Path.GetTempPath(), $"PandoraNemesis_{Guid.NewGuid():N}")
		);
		_output.Create();
	}

	public void Dispose()
	{
		if (_output.Exists)
		{
			_output.Delete(true);
		}
	}

	[Fact]
	public void LaterModCacheIndexesStartAfterEarlierAnimations()
	{
		string template = Template("animationdatasinglefile.txt");
		var anim = LoadAnimData(template);
		var projects = Substitute.For<IProjectManager>();
		projects.TryGetProject(Arg.Any<string>(), out Arg.Any<IProject?>()).Returns(false);
		var sets = new AnimSetDataManager(Paths(Path.GetDirectoryName(template)!));
		sets.SplitAnimSetDataSingleFile(projects);

		string first = ModFolder("aaaaaa", "DefaultMale", ["AlphaClip"], ["1", "0", "0", "0"]);
		string second = ModFolder("bbbbbb", "DefaultMale", ["BetaClip"], ["1", "0", "0", "0"]);

		var patcher = new NemesisCachePatcher();
		patcher.Apply(Mod("aaaaaa", 0, first), anim, sets, projects);
		patcher.Apply(Mod("bbbbbb", 1, second), anim, sets, projects);

		ProjectAnimData male = anim.AnimDataAt(anim.IndexOfProject("DefaultMale"))!;
		Assert.Contains(male.GetClipIDs(), id => id == "32767");
		Assert.Contains(male.GetClipIDs(), id => id == "32766");
		Assert.Equal(
			"32767",
			male.BoundMotionDataProject!.Blocks[^2].ClipID
		);
		Assert.Equal("32766", male.BoundMotionDataProject.Blocks[^1].ClipID);
	}

	[Fact]
	public void AnimationNamesWithoutCacheDataDoNotShiftLaterTokens()
	{
		string template = Template("animationdatasinglefile.txt");
		var anim = LoadAnimData(template);
		var projects = Substitute.For<IProjectManager>();
		var sets = new AnimSetDataManager(Paths(Path.GetDirectoryName(template)!));
		sets.SplitAnimSetDataSingleFile(projects);
		projects.TryGetProject(Arg.Any<string>(), out Arg.Any<IProject?>()).Returns(false);
		projects.ActivePackFiles.Returns([]);

		string first = ModFolder("aaaaaa", "DefaultMale", ["AlphaClip"], ["1", "0", "0", "0"]);
		Directory.CreateDirectory(Path.Combine(first, "defaultmale"));
		File.WriteAllLines(
			Path.Combine(first, "defaultmale", "#0029.txt"),
			[
				"<hkparam name=\"animationNames\" numelements=\"1656\">",
				"<!-- MOD_CODE ~aaaaaa~ OPEN -->",
				"<hkcstring>Animations\\Alpha.hkx</hkcstring>",
				"<hkcstring>Animations\\NoCache.hkx</hkcstring>",
				"<!-- CLOSE -->",
				"</hkparam>",
			]
		);
		string second = ModFolder("bbbbbb", "DefaultMale", ["BetaClip"], ["1", "0", "0", "0"]);

		var patcher = new NemesisCachePatcher();
		patcher.Apply(Mod("aaaaaa", 0, first), anim, sets, projects);
		patcher.Apply(Mod("bbbbbb", 1, second), anim, sets, projects);

		ProjectAnimData male = anim.AnimDataAt(anim.IndexOfProject("DefaultMale"))!;
		Assert.Contains(male.GetClipIDs(), id => id == "32767");
		Assert.Contains(male.GetClipIDs(), id => id == "32766");
	}

	[Fact]
	public void CompanionSetFileThatNamesAnotherSetDoesNotAbort()
	{
		string template = Template("animationsetdatasinglefile.txt");
		var anim = LoadAnimData(Template("animationdatasinglefile.txt"));
		var projects = Substitute.For<IProjectManager>();
		var sets = new AnimSetDataManager(Paths(Path.GetDirectoryName(template)!));
		Assert.True(sets.SplitAnimSetDataSingleFile(projects));
		projects.TryGetProject(Arg.Any<string>(), out Arg.Any<IProject?>()).Returns(false);
		projects.ActivePackFiles.Returns([]);

		IProjectAnimSetData horse = sets.AnimSetDataMap["HorseProject"];
		int before = horse.AnimSetsByName["FullCharacter.txt"].AnimInfos.Count;
		string root = Path.Combine(_output.FullName, "hpmhr");
		string folder = Path.Combine(
			root,
			"animationsetdatasinglefile",
			"HorseProjectData~HorseProject"
		);
		Directory.CreateDirectory(folder);
		File.WriteAllLines(
			Path.Combine(folder, "horseproject.txt"),
			[
				"FullCharacter.txt",
				"<!-- MOD_CODE ~hpmhr~ OPEN -->",
				"2682081315",
				"3020992181",
				"7891816",
				"<!-- CLOSE -->",
			]
		);
		var setLines = new List<string> { "V3", "0", "0", "0", before.ToString() };
		for (int i = 0; i < before; i++)
		{
			setLines.Add("1");
			setLines.Add("2");
			setLines.Add("7891816");
		}
		setLines.Add("<!-- MOD_CODE ~hpmhr~ OPEN -->");
		setLines.Add("2682081315");
		setLines.Add("3020992181");
		setLines.Add("7891816");
		setLines.Add("<!-- CLOSE -->");
		File.WriteAllLines(Path.Combine(folder, "fullcharacter.txt"), setLines);

		new NemesisCachePatcher().Apply(Mod("hpmhr", 0, root), anim, sets, projects);

		Assert.False(horse.AnimSetsByName.ContainsKey("horseproject.txt"));
		Assert.Equal(before + 1, horse.AnimSetsByName["FullCharacter.txt"].AnimInfos.Count);
	}

	[Fact]
	public void DummyCachePatchesAppearInMergedFiles()
	{
		string templateDir = Resources.TemplateDirectory.FullName;
		IEnginePathsFacade paths = Paths(templateDir, _output.FullName);
		var anim = new AnimDataManager(paths);
		var unloaded = Substitute.For<IProjectManager>();
		unloaded.ProjectLoaded(Arg.Any<string>()).Returns(false);
		anim.SplitAnimDataSingleFile(unloaded);
		var sets = new AnimSetDataManager(paths);
		Assert.True(sets.SplitAnimSetDataSingleFile(unloaded));
		var projects = Substitute.For<IProjectManager>();
		projects.TryGetProject(Arg.Any<string>(), out Arg.Any<IProject?>()).Returns(false);
		projects.ActivePackFiles.Returns([]);

		string root = ModFolder("dummyy", "DefaultMale", ["DummyClip"], ["1", "0", "0", "0"]);
		IProjectAnimSetData horse = sets.AnimSetDataMap["HorseProject"];
		int before = horse.AnimSetsByName["FullCharacter.txt"].AnimInfos.Count;
		string folder = Path.Combine(
			root,
			"animationsetdatasinglefile",
			"HorseProjectData~HorseProject"
		);
		Directory.CreateDirectory(folder);
		var setLines = new List<string> { "V3", "0", "0", "0", before.ToString() };
		for (int i = 0; i < before; i++)
		{
			setLines.Add("1");
			setLines.Add("2");
			setLines.Add("7891816");
		}
		setLines.Add("<!-- MOD_CODE ~dummyy~ OPEN -->");
		setLines.Add("111000111");
		setLines.Add("222000222");
		setLines.Add("7891816");
		setLines.Add("<!-- CLOSE -->");
		File.WriteAllLines(Path.Combine(folder, "fullcharacter.txt"), setLines);

		new NemesisCachePatcher().Apply(Mod("dummyy", 0, root), anim, sets, projects);
		anim.MergeAnimDataSingleFile();
		sets.MergeAnimSetDataSingleFile();

		string animText = File.ReadAllText(
			Path.Combine(_output.FullName, "animationdatasinglefile.txt")
		);
		string setText = File.ReadAllText(
			Path.Combine(_output.FullName, "animationsetdatasinglefile.txt")
		);
		Assert.Contains("DummyClip", animText);
		Assert.Contains("111000111", setText);
		Assert.Contains("222000222", setText);
		Assert.Equal(before + 1, horse.AnimSetsByName["FullCharacter.txt"].AnimInfos.Count);
	}

	private AnimDataManager LoadAnimData(string templatePath, string? outputFolder = null)
	{
		var manager = new AnimDataManager(
			Paths(Path.GetDirectoryName(templatePath)!, outputFolder)
		);
		var projects = Substitute.For<IProjectManager>();
		projects.ProjectLoaded(Arg.Any<string>()).Returns(false);
		manager.SplitAnimDataSingleFile(projects);
		return manager;
	}

	private IEnginePathsFacade Paths(string templateFolder, string? outputFolder = null)
	{
		var paths = Substitute.For<IEnginePathsFacade>();
		paths.TemplateFolder.Returns(new DirectoryInfo(templateFolder));
		paths.OutputMeshesFolder.Returns(new DirectoryInfo(outputFolder ?? _output.FullName));
		return paths;
	}

	private string ModFolder(
		string modcode,
		string project,
		string[] clipNames,
		string[] fixedFields
	)
	{
		string root = Path.Combine(_output.FullName, modcode);
		string folder = Path.Combine(root, "animationdatasinglefile", project);
		Directory.CreateDirectory(folder);
		for (int i = 0; i < clipNames.Length; i++)
		{
			string code = $"{modcode}${i}";
			File.WriteAllLines(
				Path.Combine(folder, $"{clipNames[i]}~{code}.txt"),
				[clipNames[i], code, fixedFields[0], fixedFields[1], fixedFields[2], "0", ""]
			);
			File.WriteAllLines(
				Path.Combine(folder, $"{code}.txt"),
				[code, "1", "1", "1 0 0 0", "1", "1 0 0 0 1", ""]
			);
		}
		return root;
	}

	private static IModInfo Mod(string code, uint priority, string folder)
	{
		var mod = Substitute.For<IModInfo>();
		mod.Code.Returns(code);
		mod.Priority.Returns(priority);
		mod.Format.Returns(IModInfo.ModFormat.Nemesis);
		mod.Folder.Returns(new DirectoryInfo(folder));
		return mod;
	}

	private static string Template(string fileName) =>
		Path.Combine(Resources.TemplateDirectory.FullName, fileName);
}
