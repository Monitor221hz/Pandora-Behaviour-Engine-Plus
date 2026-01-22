using NLog.Targets;

namespace Pandora.Logging.NLogger.Abstractions;

public interface INLogConfigurator
{
	void Configure(Target fileTarget, Target uiTarget);
}
