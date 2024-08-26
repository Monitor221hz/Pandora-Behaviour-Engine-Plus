using HKX2E;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Pandora.Patch.Patchers.Skyrim.FNIS;
public class OffsetArmAnimation : FNISAnimation
{
    public OffsetArmAnimation(Match match) : base(AnimType.OffsetArm, match)
    {
        
    }
	public OffsetArmAnimation(AnimType templateType, AnimFlags flags, string graphEvent, string animationFilePath, List<string> animationObjectNames) : base(templateType, flags, graphEvent, animationFilePath, animationObjectNames)
	{

	}
	public override bool BuildPatch(FNISAnimationListBuildContext buildContext)
	{
		var project = buildContext.TargetProject;
		var projectManager = buildContext.ProjectManager;
		base.BuildPatch(buildContext);

		if (!project.TryLookupPackFile("mt_behavior", out var targetPackFile) || targetPackFile is not PackFileGraph graph) //only supports humanoids as FNIS does
		{
			return false;
		}
		projectManager.TryActivatePackFile(targetPackFile);
		hkbClipGenerator clipGenerator = new()
		{
			name = GraphEvent,
			animationName = AnimationFilePath,
			triggers = null,
			cropStartAmountLocalTime = 0.0f,
			cropEndAmountLocalTime = 0.0f,
			startTime = 0.0f,
			playbackSpeed = 1.0f,
			enforcedDuration = 0.0f,
			userControlledTimeFraction = 0.0f,
			animationBindingIndex = 0,
			flags = 0,
			userData = 0
		};
		hkbStateMachineStateInfo stateInfo = new hkbStateMachineStateInfo() 
		{ 
			name = GraphEvent, 
			probability = 1.0f, 
			generator = clipGenerator, 
			stateId = (clipGenerator.name.GetHashCode() & 0xfffffff), 
			enable = true, 
			transitions = graph.GetPushedObjectAs<hkbStateMachineTransitionInfoArray>("#5111")
		};
		hkbStateMachine rightArmState = graph.GetPushedObjectAs<hkbStateMachine>("#5138");
		hkbStateMachine leftArmState = graph.GetPushedObjectAs<hkbStateMachine>("#5183");
		lock (rightArmState){ rightArmState.states.Add(stateInfo); }
		lock (leftArmState) { leftArmState.states.Add(stateInfo); }
		hkbStateMachineTransitionInfo transitionInfo = new()
		{
			flags = (short)(TransitionFlags.FLAG_IS_LOCAL_WILDCARD | TransitionFlags.FLAG_IS_GLOBAL_WILDCARD | TransitionFlags.FLAG_DISABLE_CONDITION),
			transition = graph.GetPushedObjectAs<hkbBlendingTransitionEffect>("#0093"),
			condition = null,
			eventId = graph.AddDefaultEvent(GraphEvent),
			toStateId = stateInfo.stateId,
			fromNestedStateId = 0,
			toNestedStateId = 0,
			priority = 0,
		};
		if (rightArmState.wildcardTransitions != null)
		{
			lock (rightArmState.wildcardTransitions)
			{
				rightArmState.wildcardTransitions.transitions.Add(transitionInfo);
			}
		}
		if (leftArmState.wildcardTransitions != null)
		{
			lock (leftArmState.wildcardTransitions)
			{
				leftArmState.wildcardTransitions.transitions.Add(transitionInfo); 
			}
		}
		BuildFlags(stateInfo, clipGenerator);
		return true; 
	}
}
