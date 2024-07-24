using HKX2;
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
		var targetCache = buildContext.TargetCache;
		var patchNodeCreator = buildContext.Helper; 
		base.BuildPatch(buildContext);
		
		if (!project.TryLookupPackFile("mt_behavior", out var targetPackFile) || targetPackFile is not PackFileGraph graph) //only supports humanoids as FNIS does
		{
			return false; 
		}
		var serializer = targetCache.GetSerializer(graph); 
		var clipGenerator = new hkbClipGenerator()
		{
			m_name = patchNodeCreator.GenerateNodeName(targetCache.Origin, GraphEvent),
			m_animationName = AnimationFilePath,
			m_triggers = null,
			m_cropStartAmountLocalTime = 0.0f,
			m_cropEndAmountLocalTime = 0.0f,
			m_startTime = 0.0f,
			m_playbackSpeed = 1.0f,
			m_enforcedDuration = 0.0f,
			m_userControlledTimeFraction = 0.0f,
			m_animationBindingIndex = 0,
			m_flags = 0,
			m_userData = 0
		};
		var stateInfo = new hkbStateMachineStateInfo() { m_name = patchNodeCreator.GenerateNodeName(targetCache.Origin, GraphEvent), m_probability = 1.0f, m_generator = clipGenerator, m_stateId = (clipGenerator.m_name.GetHashCode() & 0xfffffff), m_enable = true };
		BuildFlags(stateInfo, clipGenerator,graph, targetCache, patchNodeCreator);
		XElement stateInfoElement = serializer.WriteRegisteredNamedObject<hkbStateMachineStateInfo>(stateInfo, stateInfo.m_name);
		stateInfoElement.Elements().ElementAt(4).Value = "#5111";
		targetCache.AddChange(graph, new AppendTextChange("#5138/states", stateInfo.m_name));
		targetCache.AddChange(graph, new AppendTextChange("#5183/states", stateInfo.m_name));
		var transitionInfo = new hkbStateMachineTransitionInfo()
		{
			m_flags = (short)(TransitionFlags.FLAG_IS_LOCAL_WILDCARD | TransitionFlags.FLAG_IS_GLOBAL_WILDCARD | TransitionFlags.FLAG_DISABLE_CONDITION),
			m_transition = null,
			m_condition = null,
			m_eventId = -1,
			m_toStateId = stateInfo.m_stateId,
			m_fromNestedStateId = 0,
			m_toNestedStateId = 0,
			m_priority = 0,
		};
		XElement transitionInfoElement = serializer.WriteDetachedObject(transitionInfo);
		var transitionInfoElements = transitionInfoElement.Elements().ToList();
		transitionInfoElements[4].Value = PatchNodeCreator.AsEventFormat(GraphEvent);
		transitionInfoElements[2].Value = "#0093";
		targetCache.AddChange(graph, new AppendElementChange("#4038/transitions", transitionInfoElement));
		targetCache.AddChange(graph, new AppendElementChange("#5141/transitions", new XElement(transitionInfoElement)));

		var clipGeneratorElement = serializer.WriteRegisteredNode<hkbClipGenerator>(clipGenerator);

		targetCache.AddElementAsChange(graph, clipGeneratorElement);
		targetCache.AddElementAsChange(graph, stateInfoElement);

		patchNodeCreator.AddDefaultEvent(graph, targetCache, GraphEvent);
		graph.MapNodes("#5138", "#5141", "#4038");
		return true; 
	}
}
