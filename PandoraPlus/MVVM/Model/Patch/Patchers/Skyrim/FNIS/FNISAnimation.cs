using HKX2;
using Pandora.Core;
using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pandora.Patch.Patchers.Skyrim.FNIS;
public partial class FNISAnimation : IFNISAnimation
{
	public enum AnimType
	{
		Basic,
		Sequenced,
		SequencedOptimized,
		Furniture,
		FurnitureOptimized,
		SequencedContinued,
		OffsetArm,
		Paired,
		Killmove,
		Alternate,
		Chair
	}
	[Flags]
	public enum AnimFlags
	{
		None,
		Acyclic,
		AnimObjects,
		AnimatedCamera,
		AnimatedCameraSet,
		AnimatedCameraReset,
		BSA,
		Headtracking,
		Known,
		MotionDriven,
		Sticky,
		TransitionNext
	}


	private static readonly Dictionary<string, AnimFlags> animFlagValues = new()
	{
		{ "a", AnimFlags.Acyclic },
		{ "o", AnimFlags.AnimObjects },
		{ "ac", AnimFlags.AnimatedCamera },
		{ "ac1", AnimFlags.AnimatedCameraSet },
		{ "ac0", AnimFlags.AnimatedCameraReset },
		{ "bsa", AnimFlags.BSA },
		{ "h", AnimFlags.Headtracking },
		{ "k", AnimFlags.Known },
		{ "md", AnimFlags.MotionDriven },
		{ "st", AnimFlags.Sticky },
		{ "Tn", AnimFlags.TransitionNext }

	};
	public AnimType TemplateType { get; private set; } = AnimType.Basic;
	public AnimFlags Flags { get; private set; } = AnimFlags.None;

	public bool HasModifier => Flags.HasFlag(AnimFlags.Headtracking) || Flags.HasFlag(AnimFlags.MotionDriven);
	public string GraphEvent { get; private set; }
	public string AnimationFilePath { get; private set; }

	private List<string> animObjectNames = new();
	public FNISAnimation? NextAnimation { get; private set; } //unused

	public override string ToString() => $"PN_{TemplateType}_{GraphEvent}";

	/// <summary>
	/// Assumes that match has the groups specified in the animLine regex.
	/// </summary>
	/// <param name="match"></param>
	public FNISAnimation(AnimType animType, Match match)
	{
		TemplateType = animType;

		if (TemplateType != AnimType.Basic && match.Groups[2].Success)
		{
			var optionValues = match.Groups[2].Value.Split(',');
			AnimFlags animFlags;
			foreach (var optionValue in optionValues)
			{
				if (animFlagValues.TryGetValue(optionValue, out animFlags))
				{
					Flags |= animFlags;
				}
			}
		}
		GraphEvent = match.Groups[3].Value;
		AnimationFilePath = match.Groups[4].Value;
		if (TemplateType != AnimType.Basic && Flags.HasFlag(AnimFlags.AnimObjects) && match.Groups[5].Success)
		{
			foreach (Capture capture in match.Groups[5].Captures)
			{
				animObjectNames.Add(capture.Value);
			}
		}
	}
	public FNISAnimation(Match match) : this(AnimType.Basic, match)
	{
		
	}

	public FNISAnimation(AnimType templateType, AnimFlags flags, string graphEvent, string animationFilePath, FNISAnimation? nextAnimation, List<string> animationObjectNames)
	{
		TemplateType = templateType;
		Flags = flags;
		GraphEvent = graphEvent;
		AnimationFilePath = animationFilePath;
		NextAnimation = nextAnimation;
		animObjectNames = animationObjectNames;
	}
	public FNISAnimation(AnimType templateType, AnimFlags flags, string graphEvent, string animationFilePath, List<string> animationObjectNames) : this(templateType, flags, graphEvent, animationFilePath, null, animationObjectNames)
	{
	}

	protected void BuildFlags(hkbStateMachineStateInfo stateInfo, hkbClipGenerator clip,PackFileGraph graph, PackFileTargetCache targetCache, PatchNodeCreator patchNodeCreator)
	{
		var serializer = targetCache.GetSerializer(graph);
		var modInfo = targetCache.Origin; 

		if (HasModifier)
		{
			var modifierList = new hkbModifierList() { m_enable = true };
			var modifierGenerator = new hkbModifierGenerator() { m_modifier = modifierList, m_generator = clip };
			if (Flags.HasFlag(AnimFlags.MotionDriven))
			{

			}
		}
		clip.m_mode = (sbyte)(Flags.HasFlag(AnimFlags.Acyclic) ? PlaybackMode.MODE_SINGLE_PLAY : PlaybackMode.MODE_LOOPING);
		if (Flags.HasFlag(AnimFlags.AnimObjects))
		{
			var enterEventList = new List<hkbEventProperty>();
			var exitEventList = new List<hkbEventProperty>();
			hkbStringEventPayload payload;
			foreach (var animObjectName in animObjectNames)
			{
				payload = new hkbStringEventPayload() { m_data = animObjectName };
				targetCache.AddNamedHkObjectAsChange(payload, graph, patchNodeCreator.GenerateCollidableNodeName(modInfo, animObjectName));
				enterEventList.Add(new hkbEventProperty() { m_id = 393, m_payload = payload });
				enterEventList.Add(new hkbEventProperty() { m_id = 394, m_payload = payload });
				exitEventList.Add(new hkbEventProperty() { m_id = 165, m_payload = null });
			}
			stateInfo.m_enterNotifyEvents = new hkbStateMachineEventPropertyArray() { m_events = enterEventList };
			targetCache.AddNamedHkObjectAsChange(stateInfo.m_enterNotifyEvents, graph, patchNodeCreator.GenerateNodeName(modInfo, nameof(stateInfo.m_enterNotifyEvents)));
			if (!Flags.HasFlag(AnimFlags.Sticky))
			{
				stateInfo.m_exitNotifyEvents = new hkbStateMachineEventPropertyArray() { m_events = exitEventList };
				targetCache.AddNamedHkObjectAsChange(stateInfo.m_exitNotifyEvents, graph, patchNodeCreator.GenerateNodeName(modInfo, nameof(stateInfo.m_exitNotifyEvents))); 
			}
		}
	}
	public virtual bool BuildPatch(FNISAnimationListBuildContext buildContext)
	{
		var targetCache = buildContext.TargetCache;
		var project = buildContext.TargetProject; 

		targetCache.AddChange(project.CharacterPackFile, new AppendElementChange(project.CharacterPackFile.AnimationNamesPath, new XElement("hkcstring", AnimationFilePath)));
		if (project.Sibling == null) { return true;  }
		targetCache.AddChange(project.Sibling.CharacterPackFile, new AppendElementChange(project.Sibling.CharacterPackFile.AnimationNamesPath, new XElement("hkcstring", AnimationFilePath)));
		return true; 
	}
}
