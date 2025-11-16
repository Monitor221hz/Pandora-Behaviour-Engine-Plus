// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.IO;

namespace Pandora.Models.Patch.Skyrim64.AnimData;

public class AnimDataManager
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private const string ANIMDATA_FILENAME = "animationdatasinglefile.txt";

	private HashSet<int> usedClipIDs = [];
	public int numClipIDs { get; private set; } = 0;

	private List<string> projectNames = [];
	private Dictionary<string, Dictionary<int, int>> MotionBlockIndexes { get; set; } = [];
	private List<ProjectAnimData> animDataList { get; set; } = [];
	private List<MotionData> motionDataList { get; set; } = [];

	private DirectoryInfo templateFolder;
	private DirectoryInfo outputMeshFolder;

	public FileInfo OutputAnimDataSingleFile =>
		new(Path.Join(outputMeshFolder.FullName, ANIMDATA_FILENAME));
	public FileInfo TemplateAnimDataSingleFile =>
		new(Path.Join(templateFolder.FullName, ANIMDATA_FILENAME));

	private int LastID { get; set; } = 32767;

	public AnimDataManager(DirectoryInfo templateFolder, DirectoryInfo outputMeshFolder)
	{
		this.templateFolder = templateFolder;
		this.outputMeshFolder = outputMeshFolder;
	}

	public void SetOutputPath(DirectoryInfo outputMeshFolder)
	{
		this.outputMeshFolder = outputMeshFolder;
	}

	private void MapProjectAnimData(ProjectAnimData animData)
	{
		foreach (ClipDataBlock block in animData.Blocks)
		{
			usedClipIDs.Add(int.Parse(block.ClipID));
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

	public void SplitAnimDataSingleFile(ProjectManager projectManager)
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
					Project? activeProject = null;
					ProjectAnimData? animData = null;
					MotionData? motionData;
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
			if (
				OutputAnimDataSingleFile.Directory != null
				&& !OutputAnimDataSingleFile.Directory.Exists
			)
			{
				OutputAnimDataSingleFile.Directory.Create();
				logger.Debug($"Created directory for OutputAnimData output");
			}

			using (var writeStream = OutputAnimDataSingleFile.Create())
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
