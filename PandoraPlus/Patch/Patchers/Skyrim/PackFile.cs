using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlCake.Linq;
using XmlCake.Linq.Expressions;

namespace Pandora.Patch.Patchers.Skyrim
{
	public class PackFile
	{
		
		public PackFile(FileInfo file)
		{

		}
		public XPathLookup XPathLookup { get; set; } = new XPathLookup();

		public IAssembler Assembler { get; private set; }


	}
}
