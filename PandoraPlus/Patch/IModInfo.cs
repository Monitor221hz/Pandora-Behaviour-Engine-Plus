using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Core
{
    public interface IModInfo
    {
        public string Name { get; }

        public string Author { get; }

        public string URL { get; }

        public string Code { get; }

        public Version Version { get; }

        public DirectoryInfo Folder { get; }


		public bool Active { get; }
	}
}
