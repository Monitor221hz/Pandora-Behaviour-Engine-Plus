using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlCake.Linq;

namespace Pandora.Patch.Patchers.Skyrim
{
    public class PackFileValidator
    {
        private HashSet<string> pathHistory = new HashSet<string>();
        public void AddPathHistory(string path) => pathHistory.Add(path);   

        
        public void Validate(XMap map)
        {
            foreach(string path in pathHistory)
            {

            }
        }

        


    }
}
