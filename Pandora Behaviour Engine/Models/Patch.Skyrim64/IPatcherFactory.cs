using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;

namespace Pandora.Models.Patch.Skyrim64;

public interface IPatcherFactory
{
	IPatcher Create();
	void SetConfiguration(IEngineConfiguration config);
}