using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.API.Patch.Engine.Plugins;
public class PluginInfo : IPluginInfo
{
	public string Name { get; set; }
	public string Author { get; set; }
	public string Path { get; set; } 
}
