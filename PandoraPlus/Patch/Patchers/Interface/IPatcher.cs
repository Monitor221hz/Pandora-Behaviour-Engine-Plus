
using Pandora.Patch.Patchers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Core.Patchers
{
	public interface IPatcher
	{
		public void SetTarget(List<IModInfo> mods);

		public void Update();

		public string GetPostUpdateMessages() => string.Empty; 

		public void Run();

		public string GetPostRunMessages() => string.Empty;	

		public async Task UpdateAsync()
		{
			await Task.Run(() => { Update(); }); 
		}

		public  Task RunAsync();
	}
}
