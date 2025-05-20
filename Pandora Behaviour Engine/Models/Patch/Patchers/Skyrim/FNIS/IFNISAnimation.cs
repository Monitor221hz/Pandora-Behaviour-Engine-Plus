using HKX2E;
using NLog.Targets;
using Pandora.Core;
using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Pandora.Patch.Patchers.Skyrim.FNIS;
public interface IFNISAnimation
{
	string AnimationFilePath { get; }
	FNISAnimFlags Flags { get; set; }
	string GraphEvent { get; }
	bool HasModifier { get; }
	public int Hash { get; }
	IFNISAnimation? NextAnimation { get; set; }
	FNISAnimType TemplateType { get; }
	bool BuildPatch(FNISAnimationListBuildContext buildContext);
	void BuildFlags(FNISAnimationListBuildContext buildContext, PackFileGraph graph, hkbStateMachineStateInfo stateInfo, hkbClipGenerator clip);
	string ToString();
}