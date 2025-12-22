// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using Pandora.API.Patch.Skyrim64.AnimSetData;
using Pandora.API.Utils;
using Pandora.Models.Extensions;

namespace Pandora.Models.Patch.Skyrim64.AnimSetData;

public class AnimSetDataManager : IAnimSetDataManager
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	private const string ANIMSETDATA_FILENAME = "animationsetdatasinglefile.txt";
	private const string VANILLA_HKXPATHS_FILENAME = "vanilla_hkxpaths.txt";

	private readonly IPathResolver _pathResolver;

	public FileInfo TemplateAnimSetDataSingleFile { get; }
	public FileInfo OutputAnimSetDataSingleFile { get; }

	private readonly FileInfo _vanillaHkxFiles;

	private HashSet<string> vanillaAnimationPaths = new HashSet<string>(
		StringComparer.OrdinalIgnoreCase
	);

	private IList<string> projectPaths = [];
	private readonly IList<IProjectAnimSetData> animSetDataList = [];

	public Dictionary<string, IProjectAnimSetData> AnimSetDataMap { get; private set; } = [];

	public AnimSetDataManager(IPathResolver pathResolver)
	{
		_pathResolver = pathResolver;

		_vanillaHkxFiles = new FileInfo(
			Path.Join(pathResolver.GetTemplateFolder().FullName, VANILLA_HKXPATHS_FILENAME)
		);
		TemplateAnimSetDataSingleFile = new(
			Path.Join(_pathResolver.GetTemplateFolder().FullName, ANIMSETDATA_FILENAME)
		);
		OutputAnimSetDataSingleFile = new(
			Path.Join(_pathResolver.GetTemplateFolder().FullName, ANIMSETDATA_FILENAME)
		);
	}

	public bool SplitAnimSetDataSingleFile()
	{
		try
		{
			using (var readStream = TemplateAnimSetDataSingleFile.OpenRead())
			{
				using (var reader = new StreamReader(readStream))
				{
					int NumProjects = int.Parse(reader.ReadLine()!);
					for (int i = 0; i < NumProjects; i++)
					{
						projectPaths.Add(reader.ReadLineOrEmpty());
					}

					for (int i = 0; i < NumProjects; i++)
					{
						if (!ProjectAnimSetData.TryRead(reader, out var animSetData))
						{
							return false;
						}
						animSetDataList.Add(animSetData);
						AnimSetDataMap.Add(
							Path.GetFileNameWithoutExtension(projectPaths[i]),
							animSetData
						);

						//#if DEBUG
						//						FileInfo animDataFile = new FileInfo($"{outputFolder.FullName}\\animsetdata\\{(Path.GetFileName(projectPaths[i]))}");
						//						if (animDataFile.Exists) { animDataFile.Delete(); }
						//						if (!(animDataFile.Directory!.Exists)) { animDataFile.Directory.Create(); }
						//						using (var stream = animDataFile.OpenWrite())
						//						{
						//							using (var writer = new StreamWriter(stream))
						//							{
						//								writer.Write(animSetDataList[i]);
						//							}
						//						}

						//#endif
					}
				}
			}
			Logger.Info("Successfully split TemplateAnimSetData into individual entries.");
			return true;
		}
		catch (FormatException ex)
		{
			Logger.Error(ex, $"Invalid format while reading {TemplateAnimSetDataSingleFile.Name}");
			return false;
		}
		catch (IOException ex)
		{
			Logger.Error(ex, $"I/O error while processing {TemplateAnimSetDataSingleFile.Name}");
			return false;
		}
		catch (Exception ex)
		{
			Logger.Fatal(
				ex,
				$"Unexpected error while splitting TemplateAnimSetData from {TemplateAnimSetDataSingleFile.Name}"
			);
			throw;
		}
	}

	public void MergeAnimSetDataSingleFile()
	{
		try
		{
			if (
				OutputAnimSetDataSingleFile.Directory != null
				&& !OutputAnimSetDataSingleFile.Directory.Exists
			)
			{
				OutputAnimSetDataSingleFile.Directory.Create();
				Logger.Debug($"Created directory for OutputAnimSetData output");
			}

			using (var writeStream = OutputAnimSetDataSingleFile.Create())
			using (var writer = new StreamWriter(writeStream))
			{
				writer.WriteLine(projectPaths.Count);
				Logger.Info($"Merging {projectPaths.Count} projects into OutputAnimSetData file");

				foreach (var projectPath in projectPaths)
				{
					writer.WriteLine(projectPath);
				}

				foreach (ProjectAnimSetData animSetData in animSetDataList)
				{
					writer.Write(animSetData);
				}
			}

			Logger.Info($"Successfully merged OutputAnimSetData file");
		}
		catch (IOException ex)
		{
			Logger.Error(ex, $"I/O error while writing OutputAnimSetData file");
		}
		catch (Exception ex)
		{
			Logger.Fatal(ex, $"Unexpected error while merging AnimSetData file");
			throw;
		}
	}
}
