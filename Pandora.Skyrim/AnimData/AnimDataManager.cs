// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Pandora.API.Patch.Skyrim64;
using Pandora.API.Patch.Skyrim64.AnimData;
using Pandora.Core.Paths.Abstractions;

namespace Pandora.Skyrim.AnimData;

public class AnimDataManager : IAnimDataManager
{
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

	private const string ANIMDATA_FILENAME = "animationdatasinglefile.txt";

	private readonly HashSet<int> _usedClipIDs = [];
	public int NumClipIDs { get; private set; } = 0;

	private readonly List<string> _projectNames = [];
	private Dictionary<string, Dictionary<int, int>> MotionBlockIndexes { get; set; } = [];
	internal List<ProjectAnimData> AnimDataList { get; set; } = [];
	private List<IMotionData> MotionDataList { get; set; } = [];

	private readonly IEnginePathsFacade _pathContext;

	public FileInfo OutputAnimDataSingleFile => throw new NotImplementedException();
	public FileInfo TemplateAnimDataSingleFile { get; }

	private int LastID { get; set; } = 32767;

	public AnimDataManager(IEnginePathsFacade pathContext)
	{
		_pathContext = pathContext;
		TemplateAnimDataSingleFile = new(
			Path.Join(_pathContext.TemplateFolder.FullName, ANIMDATA_FILENAME)
		);
	}

	private void MapProjectAnimData(ProjectAnimData animData)
	{
		foreach (string clipId in animData.GetClipIDs())
		{
			_usedClipIDs.Add(int.Parse(clipId));
		}
	}

	private void MapAnimData()
	{
		foreach (ProjectAnimData animData in AnimDataList)
		{
			MapProjectAnimData(animData);
		}
	}

	public int GetNextValidID()
	{
		while (_usedClipIDs.Contains(LastID))
		{
			LastID--;
		}
		_usedClipIDs.Add(LastID);
		return LastID;
	}

	public void SplitAnimDataSingleFile(IProjectManager projectManager)
	{
		LastID = 32767;

		int numProjects;

		try
		{
			using (var readStream = TemplateAnimDataSingleFile.OpenRead())
			{
				using (StreamReader reader = new(readStream))
				{
					string? expectedLine;
					int projectIndex = 0;
					int sectionIndex = 0;
					numProjects = int.Parse(reader.ReadLine()!);
					Logger.Info(
						$"Reading TemplateAnimData file {TemplateAnimDataSingleFile.Name}, found {numProjects} projects."
					);
					IProject? activeProject = null;
					ProjectAnimData? animData = null;
					IMotionData? motionData;
					while ((expectedLine = reader.ReadLine()) != null)
					{
						if (expectedLine.Contains(".txt"))
						{
							_projectNames.Add(Path.GetFileNameWithoutExtension(expectedLine));
						}
						else if (int.TryParse(expectedLine, out int numLines))
						{
							string projectName = _projectNames[projectIndex].ToLower();

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
								AnimDataList.Add(animData);
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

								MotionDataList.Add(motionData);
								projectIndex++;
							}
						}
					}
				}
			}
			MapAnimData();
			Logger.Info("Successfully split AnimData into projects and mapped.");
		}
		catch (FormatException ex)
		{
			Logger.Error(
				ex,
				$"Invalid format while reading TemplateAnimData file {TemplateAnimDataSingleFile.Name}"
			);
		}
		catch (IOException ex)
		{
			Logger.Error(
				ex,
				$"I/O error while processing TemplateAnimData file {TemplateAnimDataSingleFile.Name}"
			);
		}
		catch (Exception ex)
		{
			Logger.Fatal(
				ex,
				$"Unexpected error while splitting TemplateAnimData from {TemplateAnimDataSingleFile.Name}"
			);
			throw;
		}
	}

	internal int IndexOfProject(string projectName)
	{
		for (int i = 0; i < _projectNames.Count; i++)
		{
			if (string.Equals(_projectNames[i], projectName, StringComparison.OrdinalIgnoreCase))
			{
				return i;
			}
		}
		return -1;
	}

	internal ProjectAnimData? AnimDataAt(int index) =>
		index >= 0 && index < AnimDataList.Count ? AnimDataList[index] : null;

	internal void AddProject(string projectName, ProjectAnimData animData)
	{
		_projectNames.Add(projectName);
		AnimDataList.Add(animData);
		if (animData.BoundMotionDataProject != null)
		{
			MotionDataList.Add(animData.BoundMotionDataProject);
		}
	}

	internal void ReorderProjects(IReadOnlyList<string> projectFileNames)
	{
		var pending = new Dictionary<string, Queue<ProjectAnimData>>(
			StringComparer.OrdinalIgnoreCase
		);
		for (int i = 0; i < _projectNames.Count; i++)
		{
			if (!pending.TryGetValue(_projectNames[i], out var queue))
			{
				queue = new Queue<ProjectAnimData>();
				pending[_projectNames[i]] = queue;
			}
			queue.Enqueue(AnimDataList[i]);
		}

		var names = new List<string>(projectFileNames.Count);
		var projects = new List<ProjectAnimData>(projectFileNames.Count);
		foreach (string fileName in projectFileNames)
		{
			string stem = Path.GetFileNameWithoutExtension(fileName);
			if (!pending.TryGetValue(stem, out var queue) || queue.Count == 0)
			{
				throw new InvalidDataException(
					$"Animation data project list names {stem}, which is not loaded."
				);
			}
			ProjectAnimData project = queue.Dequeue();
			string casing = stem;
			foreach (string existing in _projectNames)
			{
				if (string.Equals(existing, stem, StringComparison.OrdinalIgnoreCase))
				{
					casing = existing;
					break;
				}
			}
			names.Add(casing);
			projects.Add(project);
		}

		_projectNames.Clear();
		_projectNames.AddRange(names);
		AnimDataList = projects;
	}

	public void MergeAnimDataSingleFile()
	{
		try
		{
			var outputAnimDataSingleFile = new FileInfo(
				Path.Join(_pathContext.OutputMeshesFolder.FullName, ANIMDATA_FILENAME)
			);
			if (
				outputAnimDataSingleFile.Directory != null
				&& !outputAnimDataSingleFile.Directory.Exists
			)
			{
				outputAnimDataSingleFile.Directory.Create();
				Logger.Debug($"Created directory for OutputAnimData output");
			}

			string[] sections = new string[_projectNames.Count];
			Parallel.For(
				0,
				sections.Length,
				i =>
				{
					ProjectAnimData animData = AnimDataList[i];
					var motionData = animData.BoundMotionDataProject;
					var section = new StringBuilder();
					section.AppendLine(animData.GetLineCount().ToString());
					section.AppendLine(animData.ToString());
					if (motionData != null)
					{
						section.AppendLine(motionData.GetLineCount().ToString());
						section.AppendLine(motionData.ToString());
					}
					sections[i] = section.ToString();
				}
			);

			using (var writeStream = outputAnimDataSingleFile.Create())
			using (var writer = new StreamWriter(writeStream))
			{
				writer.WriteLine(_projectNames.Count);
				Logger.Info($"Merging {_projectNames.Count} projects into OutputAnimData file");

				foreach (var projectName in _projectNames)
				{
					writer.WriteLine($"{projectName}.txt");
				}

				foreach (string section in sections)
				{
					writer.Write(section);
				}
			}

			Logger.Info($"Successfully merged OutputAnimData file");
		}
		catch (IOException ex)
		{
			Logger.Error(ex, $"I/O error while writing OutputAnimData file");
		}
		catch (Exception ex)
		{
			Logger.Fatal(ex, $"Unexpected error while merging OutputAnimData file");
			throw;
		}
	}
}
