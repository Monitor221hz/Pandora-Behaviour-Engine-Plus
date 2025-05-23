﻿using Pandora.Models.Extensions;
using System;
using System.Collections.Generic;
using System.IO;

namespace Pandora.Models.Patch.Skyrim64.AnimSetData;

public class AnimSetDataManager
{
	private static readonly string ANIMSETDATA_FILENAME = "animationsetdatasinglefile.txt";

	private DirectoryInfo templateFolder { get; set; }
	private DirectoryInfo outputMeshDirectory { get; set; }
	private FileInfo templateAnimSetDataSingleFile { get; set; }
	private FileInfo outputAnimSetDataSingleFile { get; set; }

	private FileInfo vanillaHkxFiles { get; set; }

	private HashSet<string> vanillaAnimationPaths { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

	private List<string> projectPaths { get; set; } = [];

	private List<ProjectAnimSetData> animSetDataList { get; set; } = [];

	public Dictionary<string, ProjectAnimSetData> AnimSetDataMap { get; private set; } = [];

	public AnimSetDataManager(DirectoryInfo templateFolder, DirectoryInfo meshDirectory)
	{
		this.templateFolder = templateFolder;
		templateAnimSetDataSingleFile = new FileInfo($"{templateFolder.FullName}\\{ANIMSETDATA_FILENAME}");
		vanillaHkxFiles = new FileInfo($"{templateFolder.FullName}\\vanilla_hkxpaths.txt");

		outputMeshDirectory = meshDirectory;
		outputAnimSetDataSingleFile = new FileInfo($"{meshDirectory.FullName}\\{ANIMSETDATA_FILENAME}");
	}

	public void SetOutputPath(DirectoryInfo meshDirectory)
	{
		outputMeshDirectory = meshDirectory;
		outputAnimSetDataSingleFile = new FileInfo($"{outputMeshDirectory.FullName}\\{ANIMSETDATA_FILENAME}");
	}

	public void SplitAnimSetDataSingleFile()
	{
		using (var readStream = templateAnimSetDataSingleFile.OpenRead())
		{
			using (var reader = new StreamReader(readStream))
			{
				int NumProjects = int.Parse(reader.ReadLine()!);
				for (int i = 0; i < NumProjects; i++)
				{

					int NumProjects = int.Parse(reader.ReadLine()!);
					for (int i = 0; i < NumProjects; i++)
					{
						projectPaths.Add(reader.ReadLineOrEmpty());
					}

					for (int i = 0; i < NumProjects; i++)
					{
						var animSetData = ProjectAnimSetData.Read(reader);
						animSetDataList.Add(animSetData);
						AnimSetDataMap.Add(Path.GetFileNameWithoutExtension(projectPaths[i]), animSetData);

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
		}
	}

	public void MergeAnimSetDataSingleFile()
	{
		if (outputAnimSetDataSingleFile.Directory != null && !outputAnimSetDataSingleFile.Directory.Exists) { outputAnimSetDataSingleFile.Directory.Create(); }

		using (var writeStream = outputAnimSetDataSingleFile.Create())
		{
			using (var writer = new StreamWriter(writeStream))
			{
				writer.WriteLine(projectPaths.Count);
				foreach (var projectPath in projectPaths) { writer.WriteLine(projectPath); }
				foreach (ProjectAnimSetData animSetData in animSetDataList)
				{
					writer.Write(animSetData);
				}
			}
		}
	}
}