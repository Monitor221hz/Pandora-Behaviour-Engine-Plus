using HKX2E;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

namespace Pandora.Models.Patch.Skyrim64.Format.FNIS;

public interface IFNISAnimation
{
	public string AnimationFilePath { get; }
	public FNISAnimFlags Flags { get; set; }
	public string GraphEvent { get; }
	public bool HasModifier { get; }
	public int Hash { get; }
	public IFNISAnimation? NextAnimation { get; set; }
	public FNISAnimType TemplateType { get; }
	public void BuildAnimation(Project project, ProjectManager projectManager);
	public bool BuildBehavior(FNISAnimationListBuildContext buildContext);
	public void BuildFlags(FNISAnimationListBuildContext buildContext, PackFileGraph graph, hkbStateMachineStateInfo stateInfo, hkbClipGenerator clip);
	public string ToString();
}