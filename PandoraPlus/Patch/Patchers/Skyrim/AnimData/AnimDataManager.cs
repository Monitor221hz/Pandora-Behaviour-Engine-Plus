using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.AnimData
{
	public class AnimDataManager
	{

		private static readonly string ANIMDATA_FILENAME = "animationdatasinglefile.txt";

		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		private HashSet<int> usedClipIDs = new HashSet<int>();
		public int numClipIDs { get; private set; } = 0;

		private List<string> projectNames = new List<string>();
		private Dictionary<string, Dictionary<int, int>> MotionBlockIndexes { get; set; } = new Dictionary<string, Dictionary<int, int>>();
		private List<ProjectAnimData> animDataList { get; set; } = new List<ProjectAnimData>();
		private List<MotionData> motionDataList { get; set; } = new List<MotionData>();

		private DirectoryInfo templateFolder;
		private DirectoryInfo outputFolder; 

		private FileInfo templateAnimDataSingleFile { get; set; }

		private FileInfo outputAnimDataSingleFile { get; set; }


		private int LastID { get; set; } = 32767;

        public AnimDataManager(DirectoryInfo templateFolder, DirectoryInfo outputFolder)
        {
			this.templateFolder = templateFolder;
			this.outputFolder = outputFolder;
            templateAnimDataSingleFile = new FileInfo($"{templateFolder.FullName}\\{ANIMDATA_FILENAME}");
			outputAnimDataSingleFile = new FileInfo($"{outputFolder.FullName}\\{ANIMDATA_FILENAME}");
		}

		private void MapProjectAnimData(ProjectAnimData animData)
		{
			foreach (ClipDataBlock block in animData.Blocks)
			{
				usedClipIDs.Add(Int32.Parse(block.ClipID));
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

			DirectoryInfo AnimDataProjectOutputFolder = templateFolder.CreateSubdirectory("AnimationData");
			DirectoryInfo MotionDataProjectOutputFolder = AnimDataProjectOutputFolder.CreateSubdirectory("boundanims");

			using (var readStream = templateAnimDataSingleFile.OpenRead())
			{
				using (StreamReader reader = new StreamReader(readStream))
				{
					string? expectedLine;
					int numLines; 
					int projectIndex = 0;
					int sectionIndex = 0;
					NumProjects = Int32.Parse(reader.ReadLine()!);
					Project? activeProject = null;
					ProjectAnimData? animData = null;
					MotionData motionData;
					while ((expectedLine = reader.ReadLine()) != null)
					{
						
						if (expectedLine.Contains(".txt"))
						{
							projectNames.Add(Path.GetFileNameWithoutExtension(expectedLine));

						}
						else if (Int32.TryParse(expectedLine, out numLines))
						{
							string projectName = projectNames[projectIndex].ToLower();
							if (projectManager.ProjectLoaded(projectName)) activeProject = projectManager.LookupProject(projectName);
							sectionIndex++;


							if (sectionIndex % 2 != 0)
							{

								//using (StreamWriter writer = new StreamWriter(OutputFolder + "\\" + ProjectOrder[i]))
								//{

								animData = ProjectAnimData.ReadProject(reader, numLines, this);
#if DEBUG
								//var outputFile = new FileInfo(AnimDataProjectOutputFolder.FullName + $"\\{ProjectNames[projectIndex]}");
								//if (outputFile.Exists) outputFile.Delete();
								//using (var outputWriteStream = outputFile.OpenWrite())
								//{
								//	using (var writer = new StreamWriter(outputWriteStream))
								//	{
								//		writer.Write(animData.ToString());
								//	}
								//}
#endif
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

								//writer.Write(project.ToString());
								//}
							}
							else
							{

								motionData = MotionData.ReadProject(reader, numLines);
								if (animData != null) animData.BoundMotionDataProject = motionData;


								motionDataList.Add(motionData);
#if DEBUG
								//var outputFile = new FileInfo(MotionDataProjectOutputFolder.FullName + $"\\{ProjectNames[projectIndex]}");
								//if (outputFile.Exists) outputFile.Delete();
								//using (var outputWriteStream = outputFile.OpenWrite())
								//{
								//	using (var writer = new StreamWriter(outputWriteStream))
								//	{
								//		writer.Write(motionData.ToString());
								//	}
								//}
#endif
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
			if (outputAnimDataSingleFile.Exists) { outputAnimDataSingleFile.Delete(); }


			using (var writeStream = outputAnimDataSingleFile.OpenWrite())
			{
				using (var streamWriter =  new StreamWriter(writeStream))
				{
					streamWriter.WriteLine(projectNames.Count);
					foreach(var projectName in projectNames) { streamWriter.WriteLine($"{projectName}.txt"); }

					for(int i = 0;  i < projectNames.Count; i++)
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
}
