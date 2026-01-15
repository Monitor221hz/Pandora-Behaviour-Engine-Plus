using System.Threading.Tasks;

namespace Pandora.Services.Interfaces;

public interface IAppBootstrapper
{
	void InitializeSync();
	Task InitializeAsync();
}