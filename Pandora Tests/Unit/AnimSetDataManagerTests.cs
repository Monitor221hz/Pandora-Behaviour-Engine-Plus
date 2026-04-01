// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using NSubstitute;
using Pandora.Models.Patch.Skyrim64.AnimSetData;
using Pandora.Paths.Abstractions;

namespace PandoraTests.Unit;

public class AnimSetDataManagerTests : IDisposable
{
	private readonly DirectoryInfo _templateDir;
	private readonly DirectoryInfo _outputMeshesDir;
	private readonly IEnginePathsFacade _pathContext;

	private readonly string _vanillaFilePath;

	public AnimSetDataManagerTests()
	{
		var root = new DirectoryInfo(Environment.CurrentDirectory);
		_templateDir = new DirectoryInfo(
			Path.Combine(root.FullName, "Pandora_Engine", "Skyrim", "Template")
		);
		_vanillaFilePath = Path.Combine(_templateDir.FullName, "animationsetdatasinglefile.txt");

		_outputMeshesDir = new DirectoryInfo(
			Path.Combine(Path.GetTempPath(), $"PandoraTest_{Guid.NewGuid():N}")
		);
		_outputMeshesDir.Create();

		_pathContext = Substitute.For<IEnginePathsFacade>();
		_pathContext.TemplateFolder.Returns(_templateDir);
		_pathContext.OutputMeshesFolder.Returns(_outputMeshesDir);
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
	public void Split_VanillaFile_ReturnsTrue()
	{
		var manager = CreateManager();

		var result = manager.SplitAnimSetDataSingleFile();

		Assert.True(result, "Splitting the vanilla animationsetdatasinglefile.txt should succeed.");
	}

	[Fact]
	public void Split_VanillaFile_PopulatesAnimSetDataMap()
	{
		var manager = CreateManager();

		manager.SplitAnimSetDataSingleFile();

		Assert.NotEmpty(manager.AnimSetDataMap);
	}

	[Fact]
	public void Split_VanillaFile_AnimSetDataMapCountMatchesProjectCount()
	{
		var manager = CreateManager();

		manager.SplitAnimSetDataSingleFile();

		using var reader = new StreamReader(_vanillaFilePath);
		var expectedCount = int.Parse(reader.ReadLine()!);

		Assert.Equal(expectedCount, manager.AnimSetDataMap.Count);
	}

	[Fact]
	public void Split_VanillaFile_EachProjectHasAtLeastOneAnimSet()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile();

		foreach (var (projectName, projectData) in manager.AnimSetDataMap)
		{
			Assert.True(
				projectData.NumSets >= 1,
				$"Project '{projectName}' should have at least 1 anim set but had {projectData.NumSets}."
			);
		}
	}

	[Fact]
	public void SplitThenMerge_NoChanges_OutputFileIsCreated()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile();

		manager.MergeAnimSetDataSingleFile();

		Assert.True(File.Exists(GetOutputFilePath()), "Merged output file should exist on disk.");
	}

	[Fact]
	public void SplitThenMerge_NoChanges_OutputMatchesVanillaFile()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile();
		manager.MergeAnimSetDataSingleFile();

		var expected = File.ReadAllText(_vanillaFilePath);
		var actual = File.ReadAllText(GetOutputFilePath());

		Assert.Equal(expected, actual);
	}

	[Fact]
	public void SplitThenMerge_NoChanges_OutputBytesMatchVanillaFile()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile();
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
	public void SplitThenMerge_WithAddedAnimInfo_OutputDiffersFromVanilla()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile();

		var firstProject = manager.AnimSetDataMap.Values.First();
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
		manager.SplitAnimSetDataSingleFile();

		var firstProject = manager.AnimSetDataMap.Values.First();
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
	public void SplitThenMerge_WithAddedAnimInfo_CanBeReSplitSuccessfully()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile();

		var targetProjectName = manager.AnimSetDataMap.Keys.First();
		var targetProject = manager.AnimSetDataMap[targetProjectName];
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
		var reSplitResult = reSplitManager.SplitAnimSetDataSingleFile();

		Assert.True(reSplitResult, "Re-splitting the modified merged file should succeed.");
		Assert.Equal(manager.AnimSetDataMap.Count, reSplitManager.AnimSetDataMap.Count);

		var reSplitProject = reSplitManager.AnimSetDataMap[targetProjectName];
		var reSplitAnimSet = reSplitProject.AnimSets[0];

		Assert.Equal(originalAnimInfoCount + 1, reSplitAnimSet.AnimInfos.Count);
	}

	[Fact]
	public void SplitThenMerge_WithMultipleAdditions_AllAdditionsPreservedInRoundTrip()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile();

		var targetProjectName = manager.AnimSetDataMap.Keys.First();
		var targetProject = manager.AnimSetDataMap[targetProjectName];
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
		Assert.True(reSplitManager.SplitAnimSetDataSingleFile());

		var reSplitAnimSet = reSplitManager.AnimSetDataMap[targetProjectName].AnimSets[0];
		Assert.Equal(originalAnimInfoCount + additionsCount, reSplitAnimSet.AnimInfos.Count);
	}

	[Fact]
	public void SplitThenMerge_WithAdditionsToMultipleProjects_AllPreservedInRoundTrip()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile();

		var projectNames = manager.AnimSetDataMap.Keys.Take(3).ToList();
		var originalCounts = new Dictionary<string, int>();

		foreach (var name in projectNames)
		{
			var animSet = manager.AnimSetDataMap[name].AnimSets[0];
			originalCounts[name] = animSet.AnimInfos.Count;

			animSet.AddAnimInfo(
				new SetCachedAnimInfo(
					encodedPath: 400000u,
					encodedFileName: 500000u,
					encodedExtension: SetCachedAnimInfo.ENCODED_EXTENSION_DEFAULT
				)
			);
		}

		manager.MergeAnimSetDataSingleFile();

		var reSplitPathContext = Substitute.For<IEnginePathsFacade>();
		reSplitPathContext.TemplateFolder.Returns(_outputMeshesDir);
		reSplitPathContext.OutputMeshesFolder.Returns(_outputMeshesDir);

		var reSplitManager = new AnimSetDataManager(reSplitPathContext);
		Assert.True(reSplitManager.SplitAnimSetDataSingleFile());

		foreach (var name in projectNames)
		{
			var reSplitAnimSet = reSplitManager.AnimSetDataMap[name].AnimSets[0];
			Assert.Equal(originalCounts[name] + 1, reSplitAnimSet.AnimInfos.Count);
		}
	}

	[Fact]
	public void SplitThenMerge_WithAdditions_ProjectCountRemainsUnchanged()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile();

		var originalProjectCount = manager.AnimSetDataMap.Count;

		var firstAnimSet = manager.AnimSetDataMap.Values.First().AnimSets[0];
		firstAnimSet.AddAnimInfo(
			new SetCachedAnimInfo(
				encodedPath: 700000u,
				encodedFileName: 800000u,
				encodedExtension: 7891816u
			)
		);

		manager.MergeAnimSetDataSingleFile();

		var reSplitPathContext = Substitute.For<IEnginePathsFacade>();
		reSplitPathContext.TemplateFolder.Returns(_outputMeshesDir);
		reSplitPathContext.OutputMeshesFolder.Returns(_outputMeshesDir);

		var reSplitManager = new AnimSetDataManager(reSplitPathContext);
		Assert.True(reSplitManager.SplitAnimSetDataSingleFile());
		Assert.Equal(originalProjectCount, reSplitManager.AnimSetDataMap.Count);
	}

	[Fact]
	public void SplitThenMerge_WithAdditions_UnmodifiedProjectsAreIdentical()
	{
		var manager = CreateManager();
		manager.SplitAnimSetDataSingleFile();

		var projectNames = manager.AnimSetDataMap.Keys.ToList();
		var modifiedProjectName = projectNames[0];

		var unmodifiedProjectName = projectNames[1];
		var unmodifiedProjectBefore = manager.AnimSetDataMap[unmodifiedProjectName].ToString();

		manager
			.AnimSetDataMap[modifiedProjectName]
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
		Assert.True(reSplitManager.SplitAnimSetDataSingleFile());

		var unmodifiedProjectAfter = reSplitManager
			.AnimSetDataMap[unmodifiedProjectName]
			.ToString();
		Assert.Equal(unmodifiedProjectBefore, unmodifiedProjectAfter);
	}
}
