namespace Pandora.Services.Interfaces;

public interface ILoggingConfigurationService
{
    void Configure();

    void UpdateLogPath(string newDirectory);
}