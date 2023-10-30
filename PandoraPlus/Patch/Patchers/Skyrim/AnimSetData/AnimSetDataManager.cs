using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		private List<string> projectPaths { get; set; } = new List<string>();

		private List<AnimSetData> animSetDataList { get; set; } = new List<AnimSetData>();
		public AnimSetDataManager(DirectoryInfo templateFolder, DirectoryInfo outputFolder)
        {
			this.templateFolder = templateFolder;
			this.outputFolder = outputFolder;
			templateAnimSetDataSingleFile = new FileInfo($"{templateFolder.FullName}\\{ANIMSETDATA_FILENAME}");
			outputAnimSetDataSingleFile = new FileInfo($"{outputFolder.FullName}\\{ANIMSETDATA_FILENAME}");
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
						animSetDataList.Add(AnimSetData.Read(reader));

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

			using (var writeStream = outputAnimSetDataSingleFile.OpenWrite())
			{
				using (var writer = new StreamWriter(writeStream))
				{
					writer.WriteLine(projectPaths.Count);
					foreach (var projectPath in projectPaths) { writer.WriteLine(projectPath); }
					foreach(AnimSetData animSetData in animSetDataList)
					{
						writer.Write(animSetData);
					}
				}
			}
		}
	}
}
