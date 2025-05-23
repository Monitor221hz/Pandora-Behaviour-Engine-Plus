using HKX2E;
using Pandora.API.Patch.Engine.Skyrim64;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

public class PackFileSkeleton : PackFile, IPackFileSkeleton
{
	public PackFileSkeleton(FileInfo file, Project? project) : base(file, project) { Load(); }
	public PackFileSkeleton(FileInfo file) : this(file, null) { }
	public hkaAnimationContainer? MergedAnimationContainer { get; private set; }

	[MemberNotNullWhen(true, nameof(MergedAnimationContainer))]
	public bool HasAnimationContainer => MergedAnimationContainer != null;
	public hkMemoryResourceContainer? ResourceContainer { get; private set; }

	[MemberNotNullWhen(true, nameof(ResourceContainer))]
	public bool HasResourceContainer => ResourceContainer != null;
	public hkpPhysicsData? PhysicsData { get; private set; }
	[MemberNotNullWhen(true, nameof(PhysicsData))]
	public bool HasPhysicsData => PhysicsData != null;
	public hkaRagdollInstance? RagdollInstance { get; private set; }
	[MemberNotNullWhen(true, nameof(RagdollInstance))]
	public bool HasRagdollInstance => RagdollInstance != null;
	public List<hkaSkeletonMapper> SkeletonMappers { get; private set; }
	public bool HasSkeletonMappers => SkeletonMappers.Count > 0;

	[MemberNotNull(nameof(MergedAnimationContainer), nameof(PhysicsData), nameof(RagdollInstance), nameof(SkeletonMappers), nameof(ResourceContainer))]
	public override void Load()
	{
		base.Load();
		SkeletonMappers = [];
		foreach (var namedVariant in Container.namedVariants)
		{
			if (namedVariant == null || namedVariant.variant == null) { continue; }
			switch (namedVariant.variant)
			{
				case hkaAnimationContainer animContainer:
					MergedAnimationContainer = animContainer;
					break;
				case hkMemoryResourceContainer resourceContainer:
					ResourceContainer = resourceContainer;
					break;
				case hkpPhysicsData physicsData:
					PhysicsData = physicsData;
					break;
				case hkaRagdollInstance ragdollInstance:
					RagdollInstance = ragdollInstance;
					break;
				case hkaSkeletonMapper skeletonMapper:
					SkeletonMappers.Add(skeletonMapper);
					break;
			}
		}
	}
	public override void PopPriorityXmlAsObjects()
	{
		if (HasAnimationContainer) PopObjectAsXml(MergedAnimationContainer);
		if (HasResourceContainer) PopObjectAsXml(ResourceContainer);
		if (HasPhysicsData) PopObjectAsXml(PhysicsData);
		if (HasRagdollInstance) PopObjectAsXml(RagdollInstance);
		foreach (hkaSkeletonMapper skeletonMapper in SkeletonMappers)
		{
			PopObjectAsXml(skeletonMapper);
		}
	}
	public override void PushPriorityObjects()
	{
		base.PushPriorityObjects();
		if (HasAnimationContainer) PushXmlAsObject(MergedAnimationContainer);
		if (HasResourceContainer) PushXmlAsObject(ResourceContainer);
		if (HasPhysicsData) PushXmlAsObject(PhysicsData);
		if (HasRagdollInstance) PushXmlAsObject(RagdollInstance);
		foreach (hkaSkeletonMapper skeletonMapper in SkeletonMappers)
		{
			PushXmlAsObject(skeletonMapper);
		}
	}
}