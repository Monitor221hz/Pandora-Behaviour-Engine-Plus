using HKX2E;
using Pandora.Core;
using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml.Linq;

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
		var deserializer = graph.Deserializer;
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
			transitions = deserializer.GetObjectAs<hkbStateMachineTransitionInfoArray>("#5111")
		};
		hkbStateMachine rightArmState = deserializer.GetObjectAs<hkbStateMachine>("#5138");
		hkbStateMachine leftArmState = deserializer.GetObjectAs<hkbStateMachine>("#5183");
		lock (rightArmState){ rightArmState.states.Add(stateInfo); }
		lock (leftArmState) { leftArmState.states.Add(stateInfo); }
		hkbStateMachineTransitionInfo transitionInfo = new()
		{
			flags = (short)(TransitionFlags.FLAG_IS_LOCAL_WILDCARD | TransitionFlags.FLAG_IS_GLOBAL_WILDCARD | TransitionFlags.FLAG_DISABLE_CONDITION),
			transition = deserializer.GetObjectAs<hkbBlendingTransitionEffect>("#0093"),
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
		BuildFlags(stateInfo, clipGenerator,graph);
		//XElement stateInfoElement = serializer.WriteRegisteredNamedObject<hkbStateMachineStateInfo>(stateInfo, stateInfo.m_name);
		//stateInfoElement.Elements().ElementAt(4).Value = "#5111";
		//targetCache.AddChange(graph, new AppendTextChange("#5138/states", stateInfo.m_name));
		//targetCache.AddChange(graph, new AppendTextChange("#5183/states", stateInfo.m_name));

		//XElement transitionInfoElement = serializer.WriteDetachedObject(transitionInfo);
		//var transitionInfoElements = transitionInfoElement.Elements().ToList();
		//transitionInfoElements[4].Value = PatchNodeCreator.AsEventFormat(GraphEvent);
		//transitionInfoElements[2].Value = "#0093";
		//targetCache.AddChange(graph, new AppendElementChange("#4038/transitions", transitionInfoElement));
		//targetCache.AddChange(graph, new AppendElementChange("#5141/transitions", new XElement(transitionInfoElement)));

		//var clipGeneratorElement = serializer.WriteRegisteredNode<hkbClipGenerator>(clipGenerator);

		////targetCache.AddElementAsChange(graph, clipGeneratorElement);
		////targetCache.AddElementAsChange(graph, stateInfoElement);

		//patchNodeCreator.AddDefaultEvent(graph, targetCache, GraphEvent);
		//graph.PopNodes("#5138", "#5141", "#4038");
		return true; 
	}
}
