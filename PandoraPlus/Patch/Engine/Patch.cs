using Pandora.Xml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace Pandora.Patch
{
    public class Patch
    {

        public List<XParticle> edits { get; set; } = new List<XParticle>();

#nullable disable
        public ILookup<int, XParticle> EditLookups { get; set; }
#nullable enable


        private string Folder { get; set; }
        private Dictionary<string, Func<XObject, XParticle, bool>> ActionDict { get; set; }

        HashSet<string> StartCaptureComments { get; set; }
        public Patch(string folder, Dictionary<string, Func<XObject, XParticle, bool>> actionDict, HashSet<string> startCaptureComments)
        {

            Folder = folder;
            ActionDict = actionDict;
            StartCaptureComments = startCaptureComments;
        }

        public async Task BuildPatch()
        {
            XParticleCollector collector = new XParticleCollector();
            collector.ActionDict = ActionDict;
            Task<List<XParticle>> collectTask = collector.CollectReadFolder(Folder, StartCaptureComments);
            string[] insertElements = Directory.GetFiles(Folder, "*$*.txt");
            
            edits = await collectTask;
#if DEBUG
            Debug.WriteLine(edits.Count);
#endif
        }

        public async Task ApplyPatch(PackFile packfile)
        {
            
            EditLookups = edits.ToLookup(x => x.StringData.Count(x => x=='/')                                                           );
            
            foreach (IEnumerable<XParticle> editGroup in EditLookups)
            {
                List<Task<bool>> tasks = new List<Task<bool>>();
                foreach (XParticle particle in editGroup)
                {
                    tasks.Add(particle.ExecuteAsync((XObject)packfile.Map!));
                }
                await Task.WhenAll(tasks);
            }
            
            
        }

    }
}
