using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.API.Patch.Engine.Config;
public interface IEngineConfigurationPlugin
{
	public enum OptionFlags
	{
		None = 0, 
		HidePatches = 1,
	}
	public string MenuPath { get; }
	public IEngineConfigurationFactory Factory { get; }
}
