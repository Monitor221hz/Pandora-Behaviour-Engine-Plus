using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.Hkx;
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
