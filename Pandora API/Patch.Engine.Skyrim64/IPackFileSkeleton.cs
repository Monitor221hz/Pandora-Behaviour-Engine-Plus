using HKX2E;
using System.Collections.Generic;

namespace Pandora.API.Patch.Engine.Skyrim64;
public interface IPackFileSkeleton : IPackFile
{
	hkaAnimationContainer MergedAnimationContainer { get; }
	hkpPhysicsData PhysicsData { get; }
	hkaRagdollInstance RagdollInstance { get; }
	hkMemoryResourceContainer ResourceData { get; }
	List<hkaSkeletonMapper> SkeletonMappers { get; }
}