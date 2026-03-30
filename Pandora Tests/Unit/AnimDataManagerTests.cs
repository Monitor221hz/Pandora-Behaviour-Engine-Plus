// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using NSubstitute;
using Pandora.API.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.AnimData;
using Pandora.Paths.Abstractions;

namespace PandoraTests.Unit;

public class AnimDataManagerTests : IDisposable
{
	private readonly DirectoryInfo _templateDir;
	private readonly DirectoryInfo _outputMeshesDir;
	private readonly IEnginePathsFacade _pathContext;
	private readonly IProjectManager _projectManager;

	private readonly string _vanillaFilePath;

	public AnimDataManagerTests()
	{
		var root = new DirectoryInfo(Environment.CurrentDirectory);
		_templateDir = new DirectoryInfo(
			Path.Combine(root.FullName, "Pandora_Engine", "Skyrim", "Template")
		);
		_vanillaFilePath = Path.Combine(_templateDir.FullName, "animationdatasinglefile.txt");

		_outputMeshesDir = new DirectoryInfo(
			Path.Combine(Path.GetTempPath(), $"PandoraAnimDataTest_{Guid.NewGuid():N}")
		);
		_outputMeshesDir.Create();

		_pathContext = Substitute.For<IEnginePathsFacade>();
		_pathContext.TemplateFolder.Returns(_templateDir);
		_pathContext.OutputMeshesFolder.Returns(_outputMeshesDir);

		_projectManager = Substitute.For<IProjectManager>();
		_projectManager.ProjectLoaded(Arg.Any<string>()).Returns(false);
	}

	public void Dispose()
	{
		if (_outputMeshesDir.Exists)
		{
			_outputMeshesDir.Delete(true);
		}
	}

	private AnimDataManager CreateManager() => new(_pathContext);

	private string GetOutputFilePath() =>
		Path.Combine(_outputMeshesDir.FullName, "animationdatasinglefile.txt");

	[Fact]
	public void Split_VanillaFile_DoesNotThrow()
	{
		var manager = CreateManager();

		var exception = Record.Exception(() => manager.SplitAnimDataSingleFile(_projectManager));

		Assert.Null(exception);
	}

	[Fact]
	public void Split_VanillaFile_PopulatesAnimDataList()
	{
		var manager = CreateManager();

		manager.SplitAnimDataSingleFile(_projectManager);

		Assert.NotEmpty(manager.AnimDataList);
	}

	[Fact]
	public void Split_VanillaFile_AnimDataCountMatchesProjectCount()
	{
		var manager = CreateManager();

		manager.SplitAnimDataSingleFile(_projectManager);

		// The first line of the vanilla file is the project count.
		using var reader = new StreamReader(_vanillaFilePath);
		var expectedCount = int.Parse(reader.ReadLine()!);

		Assert.Equal(expectedCount, manager.AnimDataList.Count);
	}

	[Fact]
	public void Split_VanillaFile_EachProjectHasAHeader()
	{
		var manager = CreateManager();
		manager.SplitAnimDataSingleFile(_projectManager);

		foreach (var projectData in manager.AnimDataList)
		{
			Assert.NotNull(projectData.Header);
		}
	}

	[Fact]
	public void SplitThenMerge_NoChanges_OutputFileIsCreated()
	{
		var manager = CreateManager();
		manager.SplitAnimDataSingleFile(_projectManager);

		manager.MergeAnimDataSingleFile();

		Assert.True(File.Exists(GetOutputFilePath()), "Merged output file should exist on disk.");
	}

	[Fact]
	public void SplitThenMerge_NoChanges_OutputBytesMatchVanillaFile()
	{
		var manager = CreateManager();
		manager.SplitAnimDataSingleFile(_projectManager);
		manager.MergeAnimDataSingleFile();

		var expectedBytes = File.ReadAllBytes(_vanillaFilePath);
		var actualBytes = File.ReadAllBytes(GetOutputFilePath());

		Assert.Equal(expectedBytes.Length, actualBytes.Length);
		Assert.True(
			expectedBytes.AsSpan().SequenceEqual(actualBytes),
			"Merged file bytes should be identical to the vanilla template."
		);
	}

	[Fact]
	public void SplitThenMerge_NoChanges_OutputTextMatchesVanillaFile()
	{
		var manager = CreateManager();
		manager.SplitAnimDataSingleFile(_projectManager);
		manager.MergeAnimDataSingleFile();

		var expected = File.ReadAllText(_vanillaFilePath);
		var actual = File.ReadAllText(GetOutputFilePath());

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void SplitThenMerge_WithDummyClip_OutputDiffersFromVanilla()
	{
		var manager = CreateManager();
		manager.SplitAnimDataSingleFile(_projectManager);

		// Find a project that has motion data so the full path is exercised.
		var targetProject = manager.AnimDataList.First(p => p.BoundMotionDataProject != null);

		targetProject.AddDummyClipData("TestDummyClip");

		manager.MergeAnimDataSingleFile();

		var vanilla = File.ReadAllText(_vanillaFilePath);
		var output = File.ReadAllText(GetOutputFilePath());

		Assert.NotEqual(vanilla, output);
	}

	[Fact]
	public void SplitThenMerge_WithDummyClip_OutputContainsDummyClipName()
	{
		var manager = CreateManager();
		manager.SplitAnimDataSingleFile(_projectManager);

		var targetProject = manager.AnimDataList.First(p => p.BoundMotionDataProject != null);

		targetProject.AddDummyClipData("MyUniqueTestClip_42");

		manager.MergeAnimDataSingleFile();

		var output = File.ReadAllText(GetOutputFilePath());

		Assert.Contains("MyUniqueTestClip_42", output);
	}

	[Fact]
	public void SplitThenMerge_WithDummyClip_CanBeReSplitSuccessfully()
	{
		var manager = CreateManager();
		manager.SplitAnimDataSingleFile(_projectManager);

		var originalProjectCount = manager.AnimDataList.Count;

		var targetProject = manager.AnimDataList.First(p => p.BoundMotionDataProject != null);
		var originalLineCount = targetProject.GetLineCount();

		targetProject.AddDummyClipData("ReSplitTestClip");

		manager.MergeAnimDataSingleFile();

		var reSplitPathContext = Substitute.For<IEnginePathsFacade>();
		reSplitPathContext.TemplateFolder.Returns(_outputMeshesDir);
		reSplitPathContext.OutputMeshesFolder.Returns(_outputMeshesDir);

		var reSplitManager = new AnimDataManager(reSplitPathContext);
		var exception = Record.Exception(() =>
			reSplitManager.SplitAnimDataSingleFile(_projectManager)
		);

		Assert.Null(exception);
		Assert.Equal(originalProjectCount, reSplitManager.AnimDataList.Count);
	}

	[Fact]
	public void SplitThenMerge_WithMultipleDummyClips_AllClipsPreservedInRoundTrip()
	{
		var manager = CreateManager();
		manager.SplitAnimDataSingleFile(_projectManager);

		var targetProject = manager.AnimDataList.First(p => p.BoundMotionDataProject != null);

		var clipNames = new[] { "DummyClipA", "DummyClipB", "DummyClipC" };
		foreach (var clipName in clipNames)
		{
			targetProject.AddDummyClipData(clipName);
		}

		manager.MergeAnimDataSingleFile();

		var output = File.ReadAllText(GetOutputFilePath());

		foreach (var clipName in clipNames)
		{
			Assert.Contains(clipName, output);
		}
	}

	[Fact]
	public void SplitThenMerge_WithDummyClipOnMotionProject_MotionDataGrows()
	{
		var manager = CreateManager();
		manager.SplitAnimDataSingleFile(_projectManager);

		var targetProject = manager.AnimDataList.First(p => p.BoundMotionDataProject != null);

		var motionData = targetProject.BoundMotionDataProject!;
		var originalMotionBlockCount = motionData.Blocks.Count;

		targetProject.AddDummyClipData("MotionGrowthTestClip");

		Assert.Equal(originalMotionBlockCount + 1, motionData.Blocks.Count);
	}

	[Fact]
	public void SplitThenMerge_WithAdditions_ProjectCountRemainsUnchanged()
	{
		var manager = CreateManager();
		manager.SplitAnimDataSingleFile(_projectManager);

		var originalProjectCount = manager.AnimDataList.Count;

		var targetProject = manager.AnimDataList.First(p => p.BoundMotionDataProject != null);
		targetProject.AddDummyClipData("ProjectCountTestClip");

		manager.MergeAnimDataSingleFile();

		// Re-split and verify project count is stable.
		var reSplitPathContext = Substitute.For<IEnginePathsFacade>();
		reSplitPathContext.TemplateFolder.Returns(_outputMeshesDir);
		reSplitPathContext.OutputMeshesFolder.Returns(_outputMeshesDir);

		var reSplitManager = new AnimDataManager(reSplitPathContext);
		reSplitManager.SplitAnimDataSingleFile(_projectManager);

		Assert.Equal(originalProjectCount, reSplitManager.AnimDataList.Count);
	}

	[Fact]
	public void SplitThenMerge_WithAdditions_UnmodifiedProjectsAreIdentical()
	{
		var manager = CreateManager();
		manager.SplitAnimDataSingleFile(_projectManager);

		var projectsWithMotion = manager
			.AnimDataList.Where(p => p.BoundMotionDataProject != null)
			.Take(2)
			.ToList();

		Assert.True(
			projectsWithMotion.Count >= 2,
			"Need at least 2 projects with motion data for this test."
		);

		var modifiedProject = projectsWithMotion[0];
		var unmodifiedProject = projectsWithMotion[1];

		var unmodifiedBefore = unmodifiedProject.ToString();
		var unmodifiedMotionBefore = unmodifiedProject.BoundMotionDataProject!.ToString();

		modifiedProject.AddDummyClipData("OnlyModifyFirstProject");

		manager.MergeAnimDataSingleFile();

		var reSplitPathContext = Substitute.For<IEnginePathsFacade>();
		reSplitPathContext.TemplateFolder.Returns(_outputMeshesDir);
		reSplitPathContext.OutputMeshesFolder.Returns(_outputMeshesDir);

		var reSplitManager = new AnimDataManager(reSplitPathContext);
		reSplitManager.SplitAnimDataSingleFile(_projectManager);

		var unmodifiedIndex = manager.AnimDataList.IndexOf(unmodifiedProject);
		var reSplitUnmodified = reSplitManager.AnimDataList[unmodifiedIndex];

		Assert.Equal(unmodifiedBefore, reSplitUnmodified.ToString());
		Assert.Equal(unmodifiedMotionBefore, reSplitUnmodified.BoundMotionDataProject!.ToString());
	}

	[Fact]
	public void SplitThenMerge_WithDummyClip_ClipGetsValidID()
	{
		var manager = CreateManager();
		manager.SplitAnimDataSingleFile(_projectManager);

		var targetProject = manager.AnimDataList.First(p => p.BoundMotionDataProject != null);

		targetProject.AddDummyClipData("ValidIDTestClip");

		manager.MergeAnimDataSingleFile();

		var output = File.ReadAllText(GetOutputFilePath());
		Assert.Contains("ValidIDTestClip", output);

		var reSplitPathContext = Substitute.For<IEnginePathsFacade>();
		reSplitPathContext.TemplateFolder.Returns(_outputMeshesDir);
		reSplitPathContext.OutputMeshesFolder.Returns(_outputMeshesDir);

		var reSplitManager = new AnimDataManager(reSplitPathContext);
		var exception = Record.Exception(() =>
			reSplitManager.SplitAnimDataSingleFile(_projectManager)
		);
		Assert.Null(exception);
	}

	[Fact]
	public void SplitThenMerge_WithDummyClipsAcrossMultipleProjects_AllPreserved()
	{
		var manager = CreateManager();
		manager.SplitAnimDataSingleFile(_projectManager);

		var projectsWithMotion = manager
			.AnimDataList.Where(p => p.BoundMotionDataProject != null)
			.Take(3)
			.ToList();

		Assert.True(
			projectsWithMotion.Count >= 3,
			"Need at least 3 projects with motion data for this test."
		);

		var clipNames = new[] { "CrossProjectClipA", "CrossProjectClipB", "CrossProjectClipC" };

		for (int i = 0; i < projectsWithMotion.Count; i++)
		{
			projectsWithMotion[i].AddDummyClipData(clipNames[i]);
		}

		manager.MergeAnimDataSingleFile();

		var output = File.ReadAllText(GetOutputFilePath());

		foreach (var clipName in clipNames)
		{
			Assert.Contains(clipName, output);
		}

		var reSplitPathContext = Substitute.For<IEnginePathsFacade>();
		reSplitPathContext.TemplateFolder.Returns(_outputMeshesDir);
		reSplitPathContext.OutputMeshesFolder.Returns(_outputMeshesDir);

		var reSplitManager = new AnimDataManager(reSplitPathContext);
		reSplitManager.SplitAnimDataSingleFile(_projectManager);

		Assert.Equal(manager.AnimDataList.Count, reSplitManager.AnimDataList.Count);
	}

	[Fact]
	public void SplitThenMerge_DuplicateDummyClipName_OnlyAddedOnce()
	{
		var manager = CreateManager();
		manager.SplitAnimDataSingleFile(_projectManager);

		var targetProject = manager.AnimDataList.First(p => p.BoundMotionDataProject != null);

		var motionData = targetProject.BoundMotionDataProject!;
		var originalMotionBlockCount = motionData.Blocks.Count;

		targetProject.AddDummyClipData("DuplicateClipTest");
		targetProject.AddDummyClipData("DuplicateClipTest");

		Assert.Equal(originalMotionBlockCount + 1, motionData.Blocks.Count);
	}
}
