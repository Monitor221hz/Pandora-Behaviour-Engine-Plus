namespace Pandora.API.Patch;

/// <summary>
/// Strongly recommended to implement GetHashCode() in addition to IEquatable.
/// </summary>
public interface IModInfo : IEquatable<IModInfo>
{
    public enum ModFormat
    {
        FNIS,
        Nemesis,
        Pandora
    }

    public string Name { get; }

    public string Author { get; }

    public string URL { get; }

    public string Code { get; }

    public Version Version { get; }

    public DirectoryInfo Folder { get; }

    public ModFormat Format { get; }

    public bool Active { get; set; }

    public uint Priority { get; set; }
}
