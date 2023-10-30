using Pandora.Core.Engine.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Core
{
	public class BehaviourEngine
	{
		public IEngineConfiguration Configuration { get; private set; } = new SkyrimConfiguration();
		

		public void Launch(List<IModInfo> mods)
		{

			Configuration.Patcher.SetTarget(mods); 
			Configuration.Patcher.Update(); 
			Configuration.Patcher.Run();
		}

		public async Task LaunchAsync(List<IModInfo> mods)
		{
			Configuration.Patcher.SetTarget(mods);
			await Configuration.Patcher.UpdateAsync();
			await Configuration.Patcher.RunAsync(); 
		}

		public string GetMessages()
		{
			return Configuration.Patcher.GetPostRunMessages();
		}
	}

}
