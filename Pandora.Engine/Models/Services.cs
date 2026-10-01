// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

// SPDX-License-Identifier-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Microsoft.Extensions.DependencyInjection;
using Pandora.Core.Engine;
using Pandora.Models.Engine;

namespace Pandora.Models;

public static class Services
{
	extension(IServiceCollection serviceCollection)
	{
		public IServiceCollection AddEngineOrchestrator()
		{
			return serviceCollection
				.AddSingleton<EngineOrchestrator>();
		}
	}
}
