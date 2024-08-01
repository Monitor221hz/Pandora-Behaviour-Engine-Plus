using HKX2E;
using NLog;
using Pandora.Core.Patchers.Skyrim;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pandora.Patch.Patchers.Skyrim.Hkx;
public class PackFileSkeleton : PackFile
{
	public PackFileSkeleton(FileInfo file, Project? project) : base(file, project) { LoadSkeletonData(); }
	public PackFileSkeleton(FileInfo file) : this(file, null) { }
	public hkaAnimationContainer MergedAnimationContainer { get; private set; }
	public hkMemoryResourceContainer ResourceData { get; private set; }
	public hkpPhysicsData PhysicsData { get; private set; }
	public hkaRagdollInstance RagdollInstance { get; private set; }	
	public List<hkaSkeletonMapper> SkeletonMappers { get; private set; }

	[MemberNotNull(nameof(MergedAnimationContainer), nameof(PhysicsData), nameof(RagdollInstance), nameof(SkeletonMappers), nameof(ResourceData))]
	private void LoadSkeletonData()
	{
		if (Container.namedVariants.Count < 4) 
		{
			throw new InvalidDataException($"{nameof(PackFileSkeleton)} had too little named variants");
		}
		MergedAnimationContainer = (hkaAnimationContainer)Container.namedVariants[0]!.variant!;
		ResourceData = (hkMemoryResourceContainer)Container.namedVariants[1]!.variant!;
		PhysicsData = (hkpPhysicsData)Container.namedVariants[2]!.variant!;
		RagdollInstance = (hkaRagdollInstance)Container.namedVariants[3]!.variant!;
		SkeletonMappers = new(); 
		for (int i = 4; i < Container.namedVariants.Count; i++)
		{
			var namedVariant = Container.namedVariants[i];
			if (namedVariant == null || namedVariant.variant == null || namedVariant.variant is not hkaSkeletonMapper skeletonMapper) { continue; }
			SkeletonMappers.Add(skeletonMapper);
		}
	}
	public override void PopPriorityXmlAsObjects()
	{
		PopObjectAsXml(MergedAnimationContainer);
		PopObjectAsXml(ResourceData);
		PopObjectAsXml(PhysicsData);
		PopObjectAsXml(RagdollInstance);
		foreach(hkaSkeletonMapper skeletonMapper in SkeletonMappers)
		{
			PopObjectAsXml(skeletonMapper);
		}
	}
	public override void PushPriorityObjects()
	{
		base.PushPriorityObjects();
		PushXmlAsObject(MergedAnimationContainer);
		PushXmlAsObject(ResourceData);
		PushXmlAsObject(PhysicsData);
		PushXmlAsObject(RagdollInstance);
		foreach (hkaSkeletonMapper skeletonMapper in SkeletonMappers)
		{
			PushXmlAsObject(skeletonMapper);
		}
	}

}
