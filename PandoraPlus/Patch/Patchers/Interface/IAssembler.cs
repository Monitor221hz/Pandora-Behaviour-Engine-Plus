using Pandora.Core;
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

		public Task LoadResourcesAsync();

		public void AssemblePatch(IModInfo mod);

		public virtual async Task AssemblePatchAsync(IModInfo mod)
		{
			await Task.Run(() => AssemblePatch(mod));	
		}
		public void ApplyPatches();

		public Task ApplyPatchesAsync();

		public List<(FileInfo inFile, FileInfo outFile)> GetExportFiles(); 

	}
}
