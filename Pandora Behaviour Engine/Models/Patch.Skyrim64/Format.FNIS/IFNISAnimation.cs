using HKX2E;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

namespace Pandora.Models.Patch.Skyrim64.Format.FNIS;
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