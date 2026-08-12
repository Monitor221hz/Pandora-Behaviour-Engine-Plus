namespace Pandora.API.Patch.Skyrim64;

public class AlternateAnimation
{
    public string Prefix { get; }

    /// <summary>
    /// e.g., `animations/XPMSE`
    /// </summary>
    public string AnimRoot { get; }

    public List<AASet> Sets { get; } = [];

    public AlternateAnimation(string prefix, string animRoot)
    {
        Prefix = prefix;
        AnimRoot = animRoot;
    }
}

public class AASet
{
    /// <summary>
    /// e.g. `_mt`
    /// </summary>
    public string Group { get; }
    /// <summary>
    /// e.g., `9`
    /// </summary>
    public int Slots { get; }
    public AASet(string group, int slots) { Group = group; Slots = slots; }
}