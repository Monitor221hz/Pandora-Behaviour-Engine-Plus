using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Skyrim64;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamplePlugin;
public class ExampleSkyrim64Patch : ISkyrim64Patch
{
	private IModInfo? origin;

	public void SetOrigin(IModInfo origin) => this.origin = origin;

	public void Run(IProjectManager projectManager)
	{
		if (projectManager.TryGetProjectEx("defaultmale", out  var ex))
		{
			var characterPackFile = ex!.GetCharacterPackFile()!;
			characterPackFile.AddUniqueAnimation("exampleanimation");
		}
	}
}
