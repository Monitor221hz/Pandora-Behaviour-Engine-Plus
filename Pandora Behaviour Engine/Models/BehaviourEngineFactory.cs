using Pandora.API;

namespace Pandora.Models;

public class BehaviourEngineFactory()
{
	public static IBehaviourEngine Create()
	{
		return new BehaviourEngine();
	}
}
