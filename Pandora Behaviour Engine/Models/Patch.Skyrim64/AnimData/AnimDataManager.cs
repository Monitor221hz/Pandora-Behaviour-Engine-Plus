// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.IO;
using Pandora.API.Patch.Skyrim64;
using Pandora.API.Patch.Skyrim64.AnimData;
using Pandora.Services.Interfaces;

namespace Pandora.Models.Patch.Skyrim64.AnimData;

public class AnimDataManager : IAnimDataManager
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private const string ANIMDATA_FILENAME = "animationdatasinglefile.txt";

	private HashSet<int> usedClipIDs = [];
	public int NumClipIDs { get; private set; } = 0;

	private List<string> projectNames = [];
	private Dictionary<string, Dictionary<int, int>> MotionBlockIndexes { get; set; } = [];
	private List<IProjectAnimData> animDataList { get; set; } = [];
	private List<IMotionData> motionDataList { get; set; } = [];

	private readonly IPathResolver _pathResolver;

	public FileInfo OutputAnimDataSingleFile { get; }
	public FileInfo TemplateAnimDataSingleFile { get; }

	private int LastID { get; set; } = 32767;

	public AnimDataManager(IPathResolver pathResolver)
	{
		_pathResolver = pathResolver;
		TemplateAnimDataSingleFile = new(
			Path.Join(_pathResolver.GetTemplateFolder().FullName, ANIMDATA_FILENAME)
		);
	}

	private void MapProjectAnimData(ProjectAnimData animData)
	{
		foreach (string clipId in animData.GetClipIDs())
		{
			usedClipIDs.Add(int.Parse(clipId));
		}
	}

	private void MapAnimData()
	{
		foreach (ProjectAnimData animData in animDataList)
		{
			MapProjectAnimData(animData);
		}
	}

	public int GetNextValidID()
	{
		while (usedClipIDs.Contains(LastID))
		{
			LastID--;
		}
		usedClipIDs.Add(LastID);
		return LastID;
	}

	public void SplitAnimDataSingleFile(IProjectManager projectManager)
	{
		LastID = 32767;

		int NumProjects;

		try
		{
			using (var readStream = TemplateAnimDataSingleFile.OpenRead())
			{
				using (StreamReader reader = new(readStream))
				{
					string? expectedLine;
					int projectIndex = 0;
					int sectionIndex = 0;
					NumProjects = int.Parse(reader.ReadLine()!);
					logger.Info(
						$"Reading TemplateAnimData file {TemplateAnimDataSingleFile.Name}, found {NumProjects} projects."
					);
					IProject? activeProject = null;
					IProjectAnimData? animData = null;
					IMotionData? motionData;
					while ((expectedLine = reader.ReadLine()) != null)
					{
						if (expectedLine.Contains(".txt"))
						{
							projectNames.Add(Path.GetFileNameWithoutExtension(expectedLine));
						}
						else if (int.TryParse(expectedLine, out int numLines))
						{
							string projectName = projectNames[projectIndex].ToLower();

							if (projectManager.ProjectLoaded(projectName))
								activeProject = projectManager.LookupProject(projectName);

							sectionIndex++;

							if (sectionIndex % 2 != 0)
							{
								if (
									!ProjectAnimData.TryReadProject(
										reader,
										this,
										numLines,
										out animData
									)
								)
								{
									for (int i = 0; i < numLines; i++)
									{
										reader.ReadLine();
									}
									projectIndex++;
									sectionIndex++;
									continue;
								}

								if (animData.Header.HasMotionData == 0)
								{
									projectIndex++;
									sectionIndex++;
								}
								animDataList.Add(animData);
								if (activeProject != null)
								{
									activeProject.AnimData = animData;
								}
							}
							else
							{
								if (!MotionData.TryReadProject(reader, numLines, out motionData))
								{
									projectIndex++;
									continue;
								}
								if (animData != null)
									animData.BoundMotionDataProject = motionData;

								motionDataList.Add(motionData);
								projectIndex++;
							}
						}
					}
				}
			}
			MapAnimData();
			logger.Info("Successfully split AnimData into projects and mapped.");
		}
		catch (FormatException ex)
		{
			logger.Error(
				ex,
				$"Invalid format while reading TemplateAnimData file {TemplateAnimDataSingleFile.Name}"
			);
		}
		catch (IOException ex)
		{
			logger.Error(
				ex,
				$"I/O error while processing TemplateAnimData file {TemplateAnimDataSingleFile.Name}"
			);
		}
		catch (Exception ex)
		{
			logger.Fatal(
				ex,
				$"Unexpected error while splitting TemplateAnimData from {TemplateAnimDataSingleFile.Name}"
			);
			throw;
		}
	}

	public void MergeAnimDataSingleFile()
	{
		try
		{
			var outputAnimDataSingleFile = new FileInfo(
				Path.Join(_pathResolver.GetOutputMeshFolder().FullName, ANIMDATA_FILENAME)
			);
			if (
				outputAnimDataSingleFile.Directory != null
				&& !outputAnimDataSingleFile.Directory.Exists
			)
			{
				outputAnimDataSingleFile.Directory.Create();
				logger.Debug($"Created directory for OutputAnimData output");
			}

			using (var writeStream = outputAnimDataSingleFile.Create())
			using (var writer = new StreamWriter(writeStream))
			{
				writer.WriteLine(projectNames.Count);
				logger.Info($"Merging {projectNames.Count} projects into OutputAnimData file");

				foreach (var projectName in projectNames)
				{
					writer.WriteLine($"{projectName}.txt");
				}

				for (int i = 0; i < projectNames.Count; i++)
				{
					var animData = animDataList[i];
					var motionData = animData.BoundMotionDataProject;

					writer.WriteLine(animData.GetLineCount());
					writer.WriteLine(animData.ToString());

					if (motionData == null)
						continue;

					writer.WriteLine(motionData.GetLineCount());
					writer.WriteLine(motionData.ToString());
				}
			}

			logger.Info($"Successfully merged OutputAnimData file");
		}
		catch (IOException ex)
		{
			logger.Error(ex, $"I/O error while writing OutputAnimData file");
		}
		catch (Exception ex)
		{
			logger.Fatal(ex, $"Unexpected error while merging OutputAnimData file");
			throw;
		}
	}
}
