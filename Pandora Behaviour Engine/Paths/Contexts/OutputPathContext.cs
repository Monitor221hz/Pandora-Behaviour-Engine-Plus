using System.IO;
using Pandora.Paths.Extensions;

namespace Pandora.Paths.Contexts;

public sealed class OutputPathContext : IOutputPathContext
{
    private readonly IUserPathContext _userPaths;

    private const string ENGINE_FOLDER = "Pandora_Engine";
    private const string MESH_FOLDER = "meshes";
    private const string ACTIVE_MODS = "ActiveMods.json";
    private const string PREVIOUS_OUT = "PreviousOutput.txt";

    public OutputPathContext(IUserPathContext userPaths)
    {
        _userPaths = userPaths;
    }

    public DirectoryInfo PandoraEngineDirectory => new(_userPaths.Output.FullName / ENGINE_FOLDER);
    public DirectoryInfo MeshesDirectory => new(_userPaths.Output.FullName / MESH_FOLDER);

    public FileInfo ActiveModsFile => new(PandoraEngineDirectory.FullName / ACTIVE_MODS);
    public FileInfo PreviousOutputFile => new(PandoraEngineDirectory.FullName / PREVIOUS_OUT);
}

