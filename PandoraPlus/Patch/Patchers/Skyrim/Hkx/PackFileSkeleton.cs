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
	public PackFileSkeleton(FileInfo file) : base(file) { LoadSkeletonData(); }

	public PackFileSkeleton(FileInfo file, Project project) : base(file, project) { LoadSkeletonData(); }

	public string SkeletonPath { get; private set; }
	public string BonesPath { get; private set; }

	[MemberNotNull("BonesPath", "SkeletonPath")]
	private void LoadSkeletonData()
	{
		TryBuildClassLookup();

		XElement skeletonContainer = classLookup["hkaSkeleton"].First();
		SkeletonPath = Map.GenerateKey(skeletonContainer);
		Activate();
		MapNode(SkeletonPath);
		
		BonesPath = $"{SkeletonPath}/bones";

		InitialBoneCount = BoneCount;
	}

	public uint InitialBoneCount { get; private set; } = 0;
	public uint BoneCount => (uint)Map.Lookup(BonesPath).Elements().Count();
}
