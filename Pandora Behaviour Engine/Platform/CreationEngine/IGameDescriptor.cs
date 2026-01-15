namespace Pandora.Platform.CreationEngine;

public interface IGameDescriptor
{
	string Id { get; }
	string Name { get; }
	uint[] SteamAppIds { get; }
	long? GogAppId { get; }
	string SubKey { get; }
	string[] ExecutableNames { get; }
}
