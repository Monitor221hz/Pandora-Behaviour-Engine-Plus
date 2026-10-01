// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using NSubstitute;
using Pandora.API.Patch.Skyrim64;
using Pandora.Skyrim.AnimSetData;
using Pandora.Core.Paths.Abstractions;

namespace PandoraTests.Unit;

public class AnimSetDataManagerTests : IDisposable
{
	private readonly DirectoryInfo _templateDir;
	private readonly DirectoryInfo _outputMeshesDir;
	private readonly IEnginePathsFacade _pathContext;
	private readonly IProjectManager _projectManager;

	private readonly string _vanillaFilePath;

	public AnimSetDataManagerTests()
	{
		var root = new DirectoryInfo(Environment.CurrentDirectory);
		_templateDir = new DirectoryInfo(
			Path.Combine(root.FullName, "Pandora_Engine", "Skyrim", "Template")
		);
		_vanillaFilePath = Path.Combine(_templateDir.FullName, "animationsetdatasinglefile.txt");

		_outputMeshesDir = new DirectoryInfo(
			Path.Combine(Path.GetTempPath(), $"PandoraAnimSetDataTest_{Guid.NewGuid():N}")
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

	private AnimSetDataManager CreateManager() => new(_pathContext);

	private string GetOutputFilePath() =>
		Path.Combine(_outputMeshesDir.FullName, "animationsetdatasinglefile.txt");

	[Fact]
	public void Split_VanillaFile_DoesNotThrow()
	{
		var manager = CreateManager();

		var result = false;
		var exception = Record.Exception(() =>
			result = manager.SplitAnimSetDataSingleFile(_projectManager)
		);

		Assert.Null(exception);
		Assert.True(result, "Splitting the vanilla animationsetdatasinglefile.txt should succeed.");
	}

	[Fact]
	public void Split_VanillaFile_PopulatesAnimSetDataList()
	{
		var manager = CreateManager();

		manager.SplitAnimSetDataSingleFile(_projectManager);

		Assert.NotEmpty(manager.AnimSetDataList);
	}

	[Fact]
	public void Split_VanillaFile_AnimSetDataListCountMatchesProjectCount()
	{
		var manager = CreateManager();

		manager.SplitAnimSetDataSingleFile(_projectManager);

		// The first line of the vanilla file is the project count.
		using var reader = new StreamReader(_vanillaFilePath);
		var expectedCount = int.Parse(reader.ReadLine()!);

		Assert.Equal(expectedCount, manager.AnimSetDataList.Count);
	}

	[Fact]
	public void Split_VanillaFile_EachProjectHasAtLeastOneAnimSet()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile(_projectManager);

		foreach (var projectData in manager.AnimSetDataList)
		{
			Assert.True(
				projectData.NumSets >= 1,
				$"Project should have at least 1 anim set but had {projectData.NumSets}."
			);
		}
	}

	[Fact]
	public void SplitThenMerge_NoChanges_OutputFileIsCreated()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile(_projectManager);

		manager.MergeAnimSetDataSingleFile();

		Assert.True(File.Exists(GetOutputFilePath()), "Merged output file should exist on disk.");
	}

	[Fact]
	public void SplitThenMerge_NoChanges_OutputBytesMatchVanillaFile()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile(_projectManager);
		manager.MergeAnimSetDataSingleFile();

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
		manager.SplitAnimSetDataSingleFile(_projectManager);
		manager.MergeAnimSetDataSingleFile();

		var expected = File.ReadAllText(_vanillaFilePath);
		var actual = File.ReadAllText(GetOutputFilePath());

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void SplitThenMerge_WithAddedAnimInfo_OutputDiffersFromVanilla()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile(_projectManager);

		var firstProject = manager.AnimSetDataList[0];
		var firstAnimSet = firstProject.AnimSets[0];
		var dummyInfo = new SetCachedAnimInfo(
			encodedPath: 999u,
			encodedFileName: 888u,
			encodedExtension: 7891816u
		);
		firstAnimSet.AddAnimInfo(dummyInfo);

		manager.MergeAnimSetDataSingleFile();

		var vanilla = File.ReadAllText(_vanillaFilePath);
		var output = File.ReadAllText(GetOutputFilePath());

		Assert.NotEqual(vanilla, output);
	}

	[Fact]
	public void SplitThenMerge_WithAddedAnimInfo_OutputContainsNewEncodedValues()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile(_projectManager);

		var firstProject = manager.AnimSetDataList[0];
		var firstAnimSet = firstProject.AnimSets[0];

		var dummyInfo = new SetCachedAnimInfo(
			encodedPath: 11111u,
			encodedFileName: 22222u,
			encodedExtension: 7891816u
		);
		firstAnimSet.AddAnimInfo(dummyInfo);

		manager.MergeAnimSetDataSingleFile();

		var output = File.ReadAllText(GetOutputFilePath());

		Assert.Contains("11111", output);
		Assert.Contains("22222", output);
	}

	[Fact]
	public void SplitThenMerge_WithAddedAnimInfo_AnimInfosCountGrows()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile(_projectManager);

		var firstProject = manager.AnimSetDataList[0];
		var firstAnimSet = firstProject.AnimSets[0];

		var originalAnimInfoCount = firstAnimSet.AnimInfos.Count;

		firstAnimSet.AddAnimInfo(
			new SetCachedAnimInfo(
				encodedPath: 33333u,
				encodedFileName: 44444u,
				encodedExtension: SetCachedAnimInfo.ENCODED_EXTENSION_DEFAULT
			)
		);

		Assert.Equal(originalAnimInfoCount + 1, firstAnimSet.AnimInfos.Count);
	}

	[Fact]
	public void SplitThenMerge_WithAddedAnimInfo_CanBeReSplitSuccessfully()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile(_projectManager);

		const int targetIndex = 0;
		var targetProject = manager.AnimSetDataList[targetIndex];
		var targetAnimSet = targetProject.AnimSets[0];

		var originalAnimInfoCount = targetAnimSet.AnimInfos.Count;

		var dummyInfo = new SetCachedAnimInfo(
			encodedPath: 55555u,
			encodedFileName: 66666u,
			encodedExtension: 7891816u
		);
		targetAnimSet.AddAnimInfo(dummyInfo);

		manager.MergeAnimSetDataSingleFile();

		var reSplitPathContext = Substitute.For<IEnginePathsFacade>();
		reSplitPathContext.TemplateFolder.Returns(_outputMeshesDir);
		reSplitPathContext.OutputMeshesFolder.Returns(_outputMeshesDir);

		var reSplitManager = new AnimSetDataManager(reSplitPathContext);
		var exception = Record.Exception(() =>
			reSplitManager.SplitAnimSetDataSingleFile(_projectManager)
		);

		Assert.Null(exception);
		Assert.Equal(manager.AnimSetDataList.Count, reSplitManager.AnimSetDataList.Count);

		var reSplitAnimSet = reSplitManager.AnimSetDataList[targetIndex].AnimSets[0];

		Assert.Equal(originalAnimInfoCount + 1, reSplitAnimSet.AnimInfos.Count);
	}

	[Fact]
	public void SplitThenMerge_WithMultipleAdditions_AllAdditionsPreservedInRoundTrip()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile(_projectManager);

		const int targetIndex = 0;
		var targetProject = manager.AnimSetDataList[targetIndex];
		var targetAnimSet = targetProject.AnimSets[0];

		var originalAnimInfoCount = targetAnimSet.AnimInfos.Count;
		const int additionsCount = 3;

		for (uint i = 0; i < additionsCount; i++)
		{
			var info = new SetCachedAnimInfo(
				encodedPath: 100000u + i,
				encodedFileName: 200000u + i,
				encodedExtension: SetCachedAnimInfo.ENCODED_EXTENSION_DEFAULT
			);
			targetAnimSet.AddAnimInfo(info);
		}

		manager.MergeAnimSetDataSingleFile();

		// Re-split to verify.
		var reSplitPathContext = Substitute.For<IEnginePathsFacade>();
		reSplitPathContext.TemplateFolder.Returns(_outputMeshesDir);
		reSplitPathContext.OutputMeshesFolder.Returns(_outputMeshesDir);

		var reSplitManager = new AnimSetDataManager(reSplitPathContext);
		Assert.True(reSplitManager.SplitAnimSetDataSingleFile(_projectManager));

		var reSplitAnimSet = reSplitManager.AnimSetDataList[targetIndex].AnimSets[0];
		Assert.Equal(originalAnimInfoCount + additionsCount, reSplitAnimSet.AnimInfos.Count);
	}

	[Fact]
	public void SplitThenMerge_WithAdditionsToMultipleProjects_AllPreservedInRoundTrip()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile(_projectManager);

		Assert.True(
			manager.AnimSetDataList.Count >= 3,
			"Need at least 3 projects for this test."
		);

		int[] targetIndices = [0, 1, 2];
		var originalCounts = new Dictionary<int, int>();

		foreach (var i in targetIndices)
		{
			var animSet = manager.AnimSetDataList[i].AnimSets[0];
			originalCounts[i] = animSet.AnimInfos.Count;

			animSet.AddAnimInfo(
				new SetCachedAnimInfo(
					encodedPath: 400000u + (uint)i,
					encodedFileName: 500000u + (uint)i,
					encodedExtension: SetCachedAnimInfo.ENCODED_EXTENSION_DEFAULT
				)
			);
		}

		manager.MergeAnimSetDataSingleFile();

		var reSplitPathContext = Substitute.For<IEnginePathsFacade>();
		reSplitPathContext.TemplateFolder.Returns(_outputMeshesDir);
		reSplitPathContext.OutputMeshesFolder.Returns(_outputMeshesDir);

		var reSplitManager = new AnimSetDataManager(reSplitPathContext);
		Assert.True(reSplitManager.SplitAnimSetDataSingleFile(_projectManager));

		foreach (var i in targetIndices)
		{
			var reSplitAnimSet = reSplitManager.AnimSetDataList[i].AnimSets[0];
			Assert.Equal(originalCounts[i] + 1, reSplitAnimSet.AnimInfos.Count);
		}
	}

	[Fact]
	public void SplitThenMerge_WithAdditions_ProjectCountRemainsUnchanged()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile(_projectManager);

		var originalProjectCount = manager.AnimSetDataList.Count;

		var firstAnimSet = manager.AnimSetDataList[0].AnimSets[0];
		firstAnimSet.AddAnimInfo(
			new SetCachedAnimInfo(
				encodedPath: 700000u,
				encodedFileName: 800000u,
				encodedExtension: 7891816u
			)
		);

		manager.MergeAnimSetDataSingleFile();

		// Re-split and verify project count is stable.
		var reSplitPathContext = Substitute.For<IEnginePathsFacade>();
		reSplitPathContext.TemplateFolder.Returns(_outputMeshesDir);
		reSplitPathContext.OutputMeshesFolder.Returns(_outputMeshesDir);

		var reSplitManager = new AnimSetDataManager(reSplitPathContext);
		reSplitManager.SplitAnimSetDataSingleFile(_projectManager);

		Assert.Equal(originalProjectCount, reSplitManager.AnimSetDataList.Count);
	}

	[Fact]
	public void SplitThenMerge_WithAdditions_UnmodifiedProjectsAreIdentical()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile(_projectManager);

		Assert.True(
			manager.AnimSetDataList.Count >= 2,
			"Need at least 2 projects for this test."
		);

		var modifiedProject = manager.AnimSetDataList[0];
		var unmodifiedProject = manager.AnimSetDataList[1];

		var unmodifiedBefore = unmodifiedProject.ToString();

		modifiedProject
			.AnimSets[0]
			.AddAnimInfo(
				new SetCachedAnimInfo(
					encodedPath: 900000u,
					encodedFileName: 900001u,
					encodedExtension: 7891816u
				)
			);

		manager.MergeAnimSetDataSingleFile();

		var reSplitPathContext = Substitute.For<IEnginePathsFacade>();
		reSplitPathContext.TemplateFolder.Returns(_outputMeshesDir);
		reSplitPathContext.OutputMeshesFolder.Returns(_outputMeshesDir);

		var reSplitManager = new AnimSetDataManager(reSplitPathContext);
		reSplitManager.SplitAnimSetDataSingleFile(_projectManager);

		var unmodifiedIndex = manager.AnimSetDataList.IndexOf(unmodifiedProject);
		var reSplitUnmodified = reSplitManager.AnimSetDataList[unmodifiedIndex];

		Assert.Equal(unmodifiedBefore, reSplitUnmodified.ToString());
	}
}
