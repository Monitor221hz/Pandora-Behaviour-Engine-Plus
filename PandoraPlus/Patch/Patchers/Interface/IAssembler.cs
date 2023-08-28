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

		public async Task LoadResourcesAsync()
		{
			await Task.Run(() => LoadResources());
		}

		public void AssemblePatch(DirectoryInfo folder);

		public async Task AssemblePatchAsync(DirectoryInfo folder)
		{
			await Task.Run(() => AssemblePatch(folder));	
		}
		public void ApplyPatches();

		public async Task ApplyPatchesAsync()
		{
			await Task.Run(() => ApplyPatches());
		}

		public List<(FileInfo inFile, FileInfo outFile)> GetExportFiles(); 

	}
}
