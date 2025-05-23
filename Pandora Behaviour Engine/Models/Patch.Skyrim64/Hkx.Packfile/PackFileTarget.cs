using Pandora.Models.Patch.Skyrim64.Hkx.Changes;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
/// <summary>
/// A loosely coupled collection of a pack file and a change set.
/// </summary>
public readonly struct PackFileTarget
{
	public PackFileTarget(PackFile packFile, PackFileChangeSet packFileChangeSet)
	{
		Target = packFile;
		ChangeSet = packFileChangeSet;
	}
	public void Build()
	{
		Target.Dispatcher.AddChangeSet(ChangeSet);
	}
	public PackFile Target { get; }
	public PackFileChangeSet ChangeSet { get; }

}