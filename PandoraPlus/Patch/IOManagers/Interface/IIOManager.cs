using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Core.IOManagers
{
	public interface IIOManager
	{
		public bool Export(FileInfo inFile, FileInfo outFile);

		public bool Import(FileInfo inFile);

		public bool Export(string inPath, string outPath) => Export(new FileInfo(inPath), new FileInfo(outPath));
	}
}
