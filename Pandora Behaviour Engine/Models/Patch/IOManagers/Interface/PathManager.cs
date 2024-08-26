using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Core.IOManagers
{
	public interface PathManager
	{
		public bool Export(FileInfo inFile);

		public DirectoryInfo Import(FileInfo inFile);


	}
}
