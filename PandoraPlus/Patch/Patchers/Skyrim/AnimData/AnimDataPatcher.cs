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
	public class AnimDataPatcher
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		private HashSet<int> UsedClipIDs = new HashSet<int>();
		public int numClipIDs { get; private set; } = 0;

		private List<string> ProjectFileNames = new List<string>();
		private Dictionary<string, Dictionary<int, int>> MotionBlockIndexes { get; set; } = new Dictionary<string, Dictionary<int, int>>();
		private List<ProjectAnimData> AnimDataProjects { get; set; } = new List<ProjectAnimData>();
		private List<MotionData> MotionDataProjects { get; set; } = new List<MotionData>();

		DirectoryInfo TemplateFolder = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Pandora_Engine\\Skyrim\\Template");

		private int LastID { get; set; } = 32767;
		public void SplitAnimationDataSingleFile(ProjectManager projectManager)
		{
			LastID = 32767;

			int NumProjects;

			FileInfo SingleFile = new FileInfo(TemplateFolder.FullName + "\\animationdatasinglefile.txt");
			DirectoryInfo AnimDataProjectOutputFolder = TemplateFolder.CreateSubdirectory("AnimationData");
			DirectoryInfo MotionDataProjectOutputFolder = AnimDataProjectOutputFolder.CreateSubdirectory("boundanims");

			using (var readStream = SingleFile.OpenRead())
			{
				using (StreamReader reader = new StreamReader(readStream))
				{
					string? expectedLine;
					int numLines; 
					int projectIndex = 0;
					int sectionIndex = 0;
					NumProjects = Int32.Parse(reader.ReadLine()!);
					Project? activeProject = null; 
					ProjectAnimData animData = new ProjectAnimData();
					MotionData motionData;
					while ((expectedLine = reader.ReadLine()) != null)
					{
						
						if (expectedLine.Contains(".txt"))
						{
							ProjectFileNames.Add(expectedLine);

						}
						else if (Int32.TryParse(expectedLine, out numLines))
						{
							string projectName = Path.GetFileNameWithoutExtension(ProjectFileNames[projectIndex]);
							if (projectManager.ProjectLoaded(projectName)) activeProject = projectManager.LookupProject(projectName);
							sectionIndex++;


							if (sectionIndex % 2 != 0)
							{

								//using (StreamWriter writer = new StreamWriter(OutputFolder + "\\" + ProjectOrder[i]))
								//{

								animData = ProjectAnimData.ReadProject(reader, numLines);
#if DEBUG
								var outputFile = new FileInfo(AnimDataProjectOutputFolder.FullName + $"\\{ProjectFileNames[projectIndex]}");
								if (outputFile.Exists) outputFile.Delete();
								using (var outputWriteStream = outputFile.OpenWrite())
								{
									using (var writer = new StreamWriter(outputWriteStream))
									{
										writer.Write(animData.ToString());
									}
								}
#endif
								if (animData.Header.HasMotionData == 0)
								{
									projectIndex++;
									sectionIndex++;

								}
								AnimDataProjects.Add(animData);
								if (activeProject != null) activeProject.AnimData = animData;

								//writer.Write(project.ToString());
								//}
							}
							else
							{

								motionData = MotionData.ReadProject(reader, numLines);
								animData.BoundMotionDataProject = motionData;


								MotionDataProjects.Add(motionData);
#if DEBUG
								var outputFile = new FileInfo(MotionDataProjectOutputFolder.FullName + $"\\{ProjectFileNames[projectIndex]}");
								if (outputFile.Exists) outputFile.Delete();
								using (var outputWriteStream = outputFile.OpenWrite())
								{
									using (var writer = new StreamWriter(outputWriteStream))
									{
										writer.Write(motionData.ToString());
									}
								}
#endif
								projectIndex++;
							}
						}
					}
				}
			}
			
		}
	}
}
