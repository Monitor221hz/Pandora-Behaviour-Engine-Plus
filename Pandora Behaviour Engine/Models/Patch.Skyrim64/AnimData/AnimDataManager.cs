using System.Collections.Generic;
using System.IO;

namespace Pandora.Models.Patch.Skyrim64.AnimData;

public class AnimDataManager
{
	private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	private static readonly string ANIMDATA_FILENAME = "animationdatasinglefile.txt";

	private HashSet<int> usedClipIDs = [];
	public int numClipIDs { get; private set; } = 0;

	private List<string> projectNames = [];
	private Dictionary<string, Dictionary<int, int>> MotionBlockIndexes { get; set; } = [];
	private List<ProjectAnimData> animDataList { get; set; } = [];
	private List<MotionData> motionDataList { get; set; } = [];

	private DirectoryInfo templateFolder;
	private DirectoryInfo outputMeshDirectory;

	private FileInfo templateAnimDataSingleFile { get; set; }

	private FileInfo outputAnimDataSingleFile { get; set; }


	private int LastID { get; set; } = 32767;

	public AnimDataManager(DirectoryInfo templateFolder, DirectoryInfo meshDirectory)
	{
		this.templateFolder = templateFolder;
		outputMeshDirectory = meshDirectory;
		templateAnimDataSingleFile = new FileInfo($"{templateFolder.FullName}\\{ANIMDATA_FILENAME}");
		outputAnimDataSingleFile = new FileInfo($"{meshDirectory.FullName}\\{ANIMDATA_FILENAME}");
	}

	public void SetOutputPath(DirectoryInfo meshDirectory)
	{
		outputMeshDirectory = meshDirectory;
		outputAnimDataSingleFile = new FileInfo($"{outputMeshDirectory.FullName}\\{ANIMDATA_FILENAME}");
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

	public void SplitAnimationDataSingleFile(ProjectManager projectManager)
	{
		LastID = 32767;

		int NumProjects;


		using (var readStream = templateAnimDataSingleFile.OpenRead())
		{
			using (StreamReader reader = new(readStream))
			{
				string? expectedLine;
				int projectIndex = 0;
				int sectionIndex = 0;
				NumProjects = int.Parse(reader.ReadLine()!);
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
						if (projectManager.ProjectLoaded(projectName)) activeProject = projectManager.LookupProject(projectName);
						sectionIndex++;


						if (sectionIndex % 2 != 0)
						{
							if (!ProjectAnimData.TryReadProject(reader, this, numLines, out animData))
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
							if (animData != null) animData.BoundMotionDataProject = motionData;


							motionDataList.Add(motionData);
							projectIndex++;
						}
					}
				}
			}
		}
		MapAnimData();
	}

	public void MergeAnimDataSingleFile()
	{
		outputAnimDataSingleFile.Directory?.Create();
		using (var writeStream = outputAnimDataSingleFile.Create())
		{
			using (var streamWriter = new StreamWriter(writeStream))
			{
				streamWriter.WriteLine(projectNames.Count);
				foreach (var projectName in projectNames) { streamWriter.WriteLine($"{projectName}.txt"); }

				for (int i = 0; i < projectNames.Count; i++)
				{
					var animData = animDataList[i];
					var motionData = animData.BoundMotionDataProject;

					streamWriter.WriteLine(animData.GetLineCount());
					streamWriter.WriteLine(animData.ToString());

					if (motionData == null) continue;

					streamWriter.WriteLine(motionData.GetLineCount());
					streamWriter.WriteLine(motionData.ToString());
				}
			}
		}

	}
}
