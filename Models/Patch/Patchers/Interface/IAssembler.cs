using Pandora.API.Patch;
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

		public void GetPostMessages(StringBuilder builder);

		public virtual async Task AssemblePatchAsync(IModInfo mod)
		{
			await Task.Run(() => AssemblePatch(mod));	
		}
		public bool ApplyPatches();

		public Task<bool> ApplyPatchesAsync();

		public List<(FileInfo inFile, FileInfo outFile)> GetExportFiles(); 

		public void SetOutputPath(DirectoryInfo outputPath);

	}
}
