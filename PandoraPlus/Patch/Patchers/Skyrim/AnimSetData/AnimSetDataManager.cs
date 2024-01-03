using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;


namespace Pandora.Patch.Patchers.Skyrim.AnimSetData
{
    public class AnimSetDataManager
	{
		private static readonly string ANIMSETDATA_FILENAME = "animationsetdatasinglefile.txt";

		private DirectoryInfo templateFolder { get; set; }
		private DirectoryInfo outputFolder { get; set; }
		private FileInfo templateAnimSetDataSingleFile { get; set; }
		private FileInfo outputAnimSetDataSingleFile { get; set; }

		private FileInfo vanillaHkxFiles { get; set; }

		private HashSet<string> vanillaAnimationPaths { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		private List<string> projectPaths { get; set; } = new List<string>();

		private List<ProjectAnimSetData> animSetDataList { get; set; } = new List<ProjectAnimSetData>();

		public Dictionary<string, ProjectAnimSetData> AnimSetDataMap { get; private set; } = new Dictionary<string, ProjectAnimSetData>();

		public AnimSetDataManager(DirectoryInfo templateFolder, DirectoryInfo outputFolder)
        {
			this.templateFolder = templateFolder;
			this.outputFolder = outputFolder;
			templateAnimSetDataSingleFile = new FileInfo($"{templateFolder.FullName}\\{ANIMSETDATA_FILENAME}");
			outputAnimSetDataSingleFile = new FileInfo($"{outputFolder.FullName}\\{ANIMSETDATA_FILENAME}");
			vanillaHkxFiles = new FileInfo($"{templateFolder.FullName}\\vanilla_hkxpaths.txt");
		}

        public void SplitAnimSetDataSingleFile()
		{
			using (var readStream = templateAnimSetDataSingleFile.OpenRead())
			{
				using (var reader =  new StreamReader(readStream))
				{
					int NumProjects = Int32.Parse(reader.ReadLine()!);
					for(int i = 0; i < NumProjects; i++)
					{
						projectPaths.Add(reader.ReadLineSafe());
					}

					for(int i = 0; i < NumProjects; i++)
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

		public void MergeAnimSetDataSingleFile()
		{
			if (outputAnimSetDataSingleFile.Exists) { outputAnimSetDataSingleFile.Delete(); }
			if (outputAnimSetDataSingleFile.Directory != null && !outputAnimSetDataSingleFile.Directory.Exists) { outputAnimSetDataSingleFile.Directory.Create();  }

			using (var writeStream = outputAnimSetDataSingleFile.OpenWrite())
			{
				using (var writer = new StreamWriter(writeStream))
				{
					writer.WriteLine(projectPaths.Count);
					foreach (var projectPath in projectPaths) { writer.WriteLine(projectPath); }
					foreach(ProjectAnimSetData animSetData in animSetDataList)
					{
						writer.Write(animSetData);
					}
				}
			}
		}
	}
}
