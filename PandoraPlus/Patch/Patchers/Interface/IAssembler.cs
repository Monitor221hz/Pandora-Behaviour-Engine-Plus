using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlCake.Linq;
using XmlCake.Linq.Expressions;

namespace Pandora.Patch.Patchers
{
	public interface IAssembler
	{
		public void LoadResources(); 
		public void AssemblePatch(DirectoryInfo folder);

	}
}
