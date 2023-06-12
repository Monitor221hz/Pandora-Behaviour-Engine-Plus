using Pandora.Xml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Linq;
namespace Pandora.Patch
{
    public class PackFile
    {

        public XMap? Map { get; set; } 

        public string inputPath { get;}
        public string outputPath { get;}
        public List<Patch> Patches { get; set; } = new List<Patch>();
        public List<XParticle> Edits { get; set; } = new List<XParticle>();

        public string Name { get;}  
        public string FullName { get; }   

        Func<PackFile, bool> Validation { get; set; }
        public PackFile(string inPath, string outPath, string projectName, Func<PackFile, bool> validation)
        {
            
            inputPath = inPath;
            outputPath= outPath;
            Name = Path.GetFileNameWithoutExtension(inPath);
            FullName = $"{projectName}~{Name}";
            Validation= validation;
        }
        public int GetEditCounts()
        {
            return Patches.Select(p => Edits.Count).Sum();
        }
        

        public async Task BuildPatches()
        {
            Map = XMap.Load(inputPath);
            Map.Map(2);                                             
            List<Task> tasks = new List<Task>();
            foreach (Patch patch in Patches)
            {
                tasks.Add(patch.BuildPatch());
            }
            await Task.WhenAll(tasks);
            foreach (Patch patch in Patches)
            {
                Edits.AddRange(patch.edits);
            }
        }

        public async Task ApplyPatches()
        {
            List<Task> tasks = new List<Task>();
            
            ILookup<int, XParticle> EditsByDepth = Edits.OrderBy(x => x.StringData.Count(x => x == '/')).ToLookup(x => x.StringData.Count(x => x == '/'));
            StringBuilder depthBuilder = new StringBuilder();
            foreach (IGrouping<int, XParticle> depthGroup in EditsByDepth)
            {
                depthBuilder.Append(depthGroup.Key);
                foreach (XParticle particle in depthGroup)
                {
                    tasks.Add(particle.ExecuteAsync((XObject)Map!));
                }
                await Task.WhenAll(tasks);
                tasks.Clear();
            }
#if DEBUG
            Debug.WriteLine(depthBuilder.ToString());
#endif
        }

        public async Task<bool> Validate() => await Task.Run(() => Validation.Invoke(this));

        public async Task Save()
        {
            await Task.Run(() => {
                if (File.Exists(outputPath)) File.Delete(outputPath);
                if (!Directory.Exists(outputPath)) Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
                Map!.Save(outputPath); 
            });
        }



    }
}