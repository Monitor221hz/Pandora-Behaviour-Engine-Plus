using Microsoft.Extensions.DependencyInjection;
using Pandora.API.Patch;
using Pandora.API.Patch.Engine.Config;

namespace Pandora.Models.Engine;

public sealed class PatcherFactory : IPatcherFactory
{
	private readonly IServiceScopeFactory _scopeFactory;
	private IServiceScope? _scope;
	private IEngineConfiguration _config;

	public PatcherFactory(
		IServiceScopeFactory scopeFactory,
		IEngineConfiguration config)
	{
		_scopeFactory = scopeFactory;
		_config = config;
	}

	public IPatcher Create()
	{
		_scope?.Dispose();
		_scope = _scopeFactory.CreateScope();

		return (IPatcher)_scope.ServiceProvider.GetRequiredService(_config.PatcherType);
	}

	public void SetConfiguration(IEngineConfiguration config)
	{
		_config = config;
		_scope?.Dispose();
		_scope = null;
	}
}
