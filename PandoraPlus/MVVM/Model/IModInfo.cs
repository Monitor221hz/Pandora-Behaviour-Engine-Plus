using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Core
{
    
    public interface IModInfo
    {
        public enum ModFormat
        {
            FNIS, 
            Nemesis, 
            Pandora
        }

        public string Name { get; }

        public string Author { get; }

        public string URL { get; }

        public string Code { get; }

        public Version Version { get; }

        public DirectoryInfo Folder { get; }

        public ModFormat Format { get; }

		public bool Active { get; set;  }

        public uint Priority { get; set;  }
	}
}
