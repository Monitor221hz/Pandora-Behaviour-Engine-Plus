// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using Pandora.Models.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.AnimData;
using Pandora.Models.Patch.Skyrim64.AnimSetData;

namespace PandoraTests;
public class UnitTests
{
	[Fact]
	public void ProjectLoadTest()
	{
		ProjectManager manager = new ProjectManager(Resources.TemplateDirectory, Resources.CurrentDirectory);
		manager.LoadTrackedProjects();
	}

	[Fact]
	public void AnimationDataSingleFileTest()
	{
		const string animDataFileName = "animationdatasinglefile.txt";
		FileInfo inputFile = new(Path.Combine(Resources.TemplateDirectory.FullName, animDataFileName));
		Assert.True(inputFile.Exists);

		ProjectManager projectManager = new ProjectManager(Resources.TemplateDirectory, Resources.CurrentDirectory);
		projectManager.LoadTrackedProjects();
		AnimDataManager manager = new(Resources.TemplateDirectory, Resources.OutputDirectory);
		manager.SplitAnimDataSingleFile(projectManager);
		manager.MergeAnimDataSingleFile();

		FileInfo outputFile = new(Path.Combine(Resources.OutputDirectory.FullName, animDataFileName));
		Assert.True(outputFile.Exists);
		using Stream outStream = outputFile.OpenRead();
		using Stream stream = inputFile.OpenRead();
		using StreamReader outReader = new(outStream);
		using StreamReader inReader = new(stream);
		var inLine = inReader.ReadLine();
		var outLine = outReader.ReadLine();
		while (inLine != null && outLine != null)
		{
			Assert.Equal(inLine, outLine, StringComparer.Ordinal);
			inLine = inReader.ReadLine();
			outLine = outReader.ReadLine();
		}
	}

	[Fact]
	public void AnimationSetDataSingleFileTest()
	{
		const string animDataFileName = "animationsetdatasinglefile.txt";
		FileInfo inputFile = new(Path.Combine(Resources.TemplateDirectory.FullName, animDataFileName));
		Assert.True(inputFile.Exists);
		AnimSetDataManager manager = new(Resources.TemplateDirectory, Resources.OutputDirectory);
		manager.SplitAnimSetDataSingleFile();
		manager.MergeAnimSetDataSingleFile();

		FileInfo outputFile = new(Path.Combine(Resources.OutputDirectory.FullName, animDataFileName));
		Assert.True(outputFile.Exists);

		using Stream outStream = outputFile.OpenRead();
		using Stream stream = inputFile.OpenRead();
		using StreamReader outReader = new(outStream);
		using StreamReader inReader = new(stream);

		var inLine = inReader.ReadLine();
		var outLine = outReader.ReadLine();
		while (inLine != null && outLine != null)
		{
			Assert.Equal(inLine, outLine, StringComparer.Ordinal);
			inLine = inReader.ReadLine();
			outLine = outReader.ReadLine();
		}
	}

}
