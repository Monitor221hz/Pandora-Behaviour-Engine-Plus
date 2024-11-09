using HKX2E;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Pandora.API.Patch.Engine.Skyrim64;
public interface IPackFileSkeleton : IPackFile
{
	hkaAnimationContainer? MergedAnimationContainer { get; }
	[MemberNotNullWhen(true, nameof(MergedAnimationContainer))]
	public bool HasAnimationContainer => MergedAnimationContainer != null;
	hkpPhysicsData? PhysicsData { get; }
	[MemberNotNullWhen(true, nameof(PhysicsData))]
	public bool HasPhysicsData => PhysicsData != null;
	hkaRagdollInstance? RagdollInstance { get; }
	[MemberNotNullWhen(true, nameof(RagdollInstance))]
	public bool HasRagdollInstance => RagdollInstance != null;
	hkMemoryResourceContainer? ResourceContainer { get; }
	[MemberNotNullWhen(true, nameof(ResourceContainer))]
	public bool HasResourceContainer => ResourceContainer != null;
	List<hkaSkeletonMapper> SkeletonMappers { get; }
	public bool HasSkeletonMappers => SkeletonMappers.Count > 0;
}