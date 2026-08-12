// SPDX-License-Identifier-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using Microsoft.Extensions.DependencyInjection;

namespace Pandora.Core.Engine;

public static class Services
{
	extension(IServiceCollection serviceCollection)
	{
		public IServiceCollection AddBehaviourEngine()
		{
			return serviceCollection
				.AddSingleton<IBehaviourEngine, BehaviourEngine>()
				.AddSingleton<IEngineStateMachine, EngineStateMachine>()
				.AddSingleton<IEngineRunner, EngineRunner>();
		}
	}
}
