using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.API.Patch.Engine.Skyrim64;
public interface ISkyrim64Patch
{
	public void Run(IProjectManager projectManager);
}
