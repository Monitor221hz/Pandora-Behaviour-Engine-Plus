using Pandora.DTOs;

namespace Pandora.Services.Interfaces;

public interface ILaunchOptionsParser
{
    LaunchOptions Parse(string[] args);
}
