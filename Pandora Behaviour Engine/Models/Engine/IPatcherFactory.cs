using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;

namespace Pandora.Models.Engine;

public interface IPatcherFactory
{
	IPatcher Create();
	void SetConfiguration(IEngineConfiguration config);
}