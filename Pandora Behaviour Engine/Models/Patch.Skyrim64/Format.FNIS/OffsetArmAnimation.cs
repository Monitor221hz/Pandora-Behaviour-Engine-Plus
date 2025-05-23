using HKX2E;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Pandora.Models.Patch.Skyrim64.Format.FNIS;

public class OffsetArmAnimation : BasicAnimation
{
	public OffsetArmAnimation(Match match) : base(FNISAnimType.OffsetArm, match)
	{

	}
	public OffsetArmAnimation(FNISAnimType templateType, FNISAnimFlags flags, string graphEvent, string animationFilePath, List<string> animationObjectNames) : base(templateType, flags, graphEvent, animationFilePath, animationObjectNames)
	{

	}
	public override bool BuildBehavior(FNISAnimationListBuildContext buildContext)
	{
		var project = buildContext.TargetProject;
		var projectManager = buildContext.ProjectManager;
		var modInfo = buildContext.ModInfo;

		if (!base.BuildBehavior(buildContext) || !project.TryLookupPackFile("mt_behavior", out var targetPackFile) || targetPackFile is not PackFileGraph graph) //only supports humanoids as FNIS does
		{
			return false;
		}
		projectManager.TryActivatePackFile(targetPackFile);
		hkbClipGenerator clipGenerator = new()
		{
			name = $"{modInfo.Code}_{GraphEvent}_Clip",
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
			userData = 0,
			mode = (sbyte)PlaybackMode.MODE_LOOPING,
		};
		hkbStateMachineStateInfo stateInfo = new()
		{
			name = $"{GraphEvent}_StateInfo",
			probability = 1.0f,
			generator = clipGenerator,
			stateId = Hash,
			enable = true,
			transitions = graph.GetPushedObjectAs<hkbStateMachineTransitionInfoArray>("#5111")
		};
		hkbStateMachine rightArmState = graph.GetPushedObjectAs<hkbStateMachine>("#5138"); // possible crash here.
		hkbStateMachine leftArmState = graph.GetPushedObjectAs<hkbStateMachine>("#5183");
		lock (rightArmState) { rightArmState.states.Add(stateInfo); }
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
		BuildFlags(buildContext, graph, stateInfo, clipGenerator);
		return true;
	}
}