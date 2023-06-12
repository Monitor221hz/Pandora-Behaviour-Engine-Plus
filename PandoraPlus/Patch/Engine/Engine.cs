using Pandora.MVVM.Model;
using Pandora.Patch;
using Pandora.Xml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Pandora.Patch
{
    public class PatchEngine
    {

        string ProjectDefinitionPath = "\\Pandora_Engine\\ProjectDefinitions";
        Dictionary<string, string> HkxPaths { get; set; } = new Dictionary<string, string>();
        Dictionary<string, PackFile> PackFileKeys { get; set; } = new Dictionary<string, PackFile>();

        HashSet<PackFile> ActivePackFiles { get; set; } = new HashSet<PackFile>();
        Dictionary<string, ProjectDefinition> ProjectDefs { get; set; } = new Dictionary<string, ProjectDefinition>();


        IPackFilePatcher Patcher = new SkyrimPatcher();

        private List<string> ModFolderPaths { get; set; } = new List<string>();

        public PatchEngine()
        {

        }

        
        private void LoadProjectDefinitions()
        {
            
            string CurrentDirectory = Directory.GetCurrentDirectory();
            string[] defPaths = Directory.GetFiles(CurrentDirectory + ProjectDefinitionPath, "*.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(ProjectDefinition));

            foreach (string defPath in defPaths)
            {
                //#if DEBUG
                //#endif
                FileStream stream = new FileInfo(defPath).OpenRead();
                ProjectDefinition projectDef = (ProjectDefinition)(serializer.Deserialize(stream)!);
                if (!ProjectDefs.ContainsKey(projectDef.Name)) ProjectDefs.Add(projectDef.Name, projectDef);
#if DEBUG
#endif
                projectDef.LoadPackFiles(CurrentDirectory,Patcher.ValidateFile);
                HashSet<PackFile> paths = projectDef.PackFiles;
                foreach (PackFile packFile in paths)
                {

                    if (!PackFileKeys.ContainsKey(packFile.Name))
                    {
                        PackFileKeys.Add(packFile.Name, packFile);
                    }
                    if (!PackFileKeys.ContainsKey(packFile.FullName))
                    {
                        
                        PackFileKeys.Add(packFile.FullName, packFile);
                    }
                }
                stream.Close();
            }
        }
        private async Task LoadModFolders()
        {
            List<Task> BuildTasks = new List<Task>();

            foreach (string folder in ModFolderPaths)
            {
                
                string[] subFolders = Directory.GetDirectories(folder);
                string modPrefix = folder.Split(Path.DirectorySeparatorChar).Last();
                HashSet<string> captureComments = new HashSet<string>() { $" MOD_CODE ~{modPrefix}~ OPEN " };
                foreach (string subFolder in subFolders)
                {
                    
                    string folderName = subFolder.Split(Path.DirectorySeparatorChar).Last();
                    PackFile pf;
                    //Debug.WriteLine($"mod: {modPrefix} key: {folderName}");
                    if (PackFileKeys.TryGetValue(folderName, out pf!))
                    {
                        
                        ActivePackFiles.Add(pf);
                        pf.Patches.Add(new Patch(subFolder, Patcher.ActionKeys, captureComments));
                    }
                }

            }
            foreach (PackFile pf in ActivePackFiles)
            {
                BuildTasks.Add(pf.BuildPatches());
            }
            await Task.WhenAll(BuildTasks);
        }

        public async Task<string> GetUpdateLog()
        {
            StringBuilder sb = new StringBuilder("--Active Files--");

            foreach (PackFile pf in ActivePackFiles)
            {
                sb.Append('\n');
                sb.Append(pf.FullName);
                sb.Append(' ');
                sb.Append(" edits: ");
                sb.Append(pf.GetEditCounts().ToString());
            }
            return sb.ToString();
        }

        public async Task Update(List<string> modFolders)
        {
            ModFolderPaths = modFolders;
            LoadProjectDefinitions();
            await LoadModFolders();
        }

        public async Task Launch()
        {
            List<Task> tasks = new List<Task>();
            foreach (PackFile pf in ActivePackFiles)
            {
                tasks.Add(pf.ApplyPatches());
            }
            await Task.WhenAll(tasks);
        }

        public async Task Export()
        {
            List<Task>  tasks = new List<Task>();
            foreach (PackFile pf in ActivePackFiles)
            {
                tasks.Add(pf.Validate());
            }
            await Task.WhenAll(tasks);
            tasks.Clear();
            foreach (PackFile pf in ActivePackFiles)
            {
                tasks.Add(pf.Save());
            }
            await Task.WhenAll(tasks);
        }
    }
}
