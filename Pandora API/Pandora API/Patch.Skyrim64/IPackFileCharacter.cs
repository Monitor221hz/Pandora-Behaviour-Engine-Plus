using HKX2E;

namespace Pandora.API.Patch.Skyrim64;

public interface IPackFileCharacter : IPackFile, IEquatable<IPackFileCharacter>
{
    IList<string> AnimationNames { get; }
    string BehaviorFileName { get; }
    hkbCharacterData Data { get; set; }
    uint InitialAnimationCount { get; }
    uint NewAnimationCount { get; }
    string SkeletonFileName { get; }
    hkbCharacterStringData StringData { get; set; }
    bool AddUniqueAnimation(string name);
    object GetUniqueAnimationLock();
    object AttachUniqueAnimationLock(IPackFileCharacter character);
}
