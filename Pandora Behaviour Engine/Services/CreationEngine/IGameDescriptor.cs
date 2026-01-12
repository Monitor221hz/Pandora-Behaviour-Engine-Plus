using System;
using System.Collections.Generic;
using System.Text;

namespace Pandora.Services.CreationEngine;

public interface IGameDescriptor
{
	string Id { get; }
	string Name { get; }
	uint[] SteamAppIds { get; }
	long? GogAppId { get; }
	string SubKey { get; }
	string[] ExecutableNames { get; }
}
