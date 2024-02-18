using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers
{
	public interface IPatchFile
	{
		public FileInfo InputHandle { get;  }

		public FileInfo OutputHandle { get; }

		public bool Export();
	}
}
