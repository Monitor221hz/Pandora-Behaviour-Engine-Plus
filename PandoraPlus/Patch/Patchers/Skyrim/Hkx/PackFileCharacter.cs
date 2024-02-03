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
public class PackFileCharacter : PackFile
{
	public PackFileCharacter(FileInfo file) : base(file) { LoadAnimationNames(); }

	public PackFileCharacter(FileInfo file, Project project) : base(file, project) { LoadAnimationNames(); }

	private XElement? animationNamesContainer;

	public uint InitialAnimationCount { get; private set; } = 0;

	public string AnimationNamesPath { get; private set; }

	public string RigNamePath {  get; private set; }

	public string BehaviorFilenamePath { get; private set; }

	[MemberNotNull(nameof(AnimationNamesPath), nameof(RigNamePath), nameof(BehaviorFilenamePath))]
	private void LoadAnimationNames()
	{
		TryBuildClassLookup();

		XElement stringDataContainer = classLookup["hkbCharacterStringData"].First();

		string characterStringDataPath = Map.GenerateKey(stringDataContainer);
		Activate();
		MapNode(characterStringDataPath);

		AnimationNamesPath = $"{characterStringDataPath}/animationNames";
		RigNamePath = $"{characterStringDataPath}/rigName";
		BehaviorFilenamePath = $"{characterStringDataPath}/behaviorFilename";

		animationNamesContainer = Map.Lookup(AnimationNamesPath);

		var animationCountAttribute = animationNamesContainer.Attribute("numelements");
		InitialAnimationCount = (animationCountAttribute != null) ? uint.Parse(animationCountAttribute.Value) : NewAnimationCount;
	}


	public List<XElement> AnimationNames => animationNamesContainer!.Elements().ToList();

	public uint NewAnimationCount => ((uint)AnimationNames.Count - InitialAnimationCount);


}
