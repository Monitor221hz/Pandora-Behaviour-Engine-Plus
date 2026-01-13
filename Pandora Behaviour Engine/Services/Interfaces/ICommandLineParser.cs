using Pandora.DTOs;

namespace Pandora.Services.Interfaces;

public interface ICommandLineParser
{
    LaunchOptions Parse(string[] args);
}
