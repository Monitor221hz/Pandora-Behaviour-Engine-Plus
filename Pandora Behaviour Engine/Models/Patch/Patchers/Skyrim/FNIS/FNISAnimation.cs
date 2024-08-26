using HKX2E;
using Pandora.Core;
using Pandora.Core.Patchers.Skyrim;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
		{ "Tn", AnimFlags.TransitionNext }, 

	};
	protected static int GetPositiveHash(string name) => name.GetHashCode() & 0xfffffff;
	public AnimType TemplateType { get; private set; } = AnimType.Basic;
	public AnimFlags Flags { get; private set; } = AnimFlags.None;

	public bool HasModifier => Flags.HasFlag(AnimFlags.Headtracking) || Flags.HasFlag(AnimFlags.MotionDriven);
	public string GraphEvent { get; private set; }
	public string AnimationFilePath { get; private set; }

	private List<string> animObjectNames = new();
	public FNISAnimation? NextAnimation { get; set; } //unused

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
	public void BuildFlags(hkbStateMachineStateInfo stateInfo, hkbClipGenerator clip)
	{

		if (HasModifier)
		{
			var modifierList = new hkbModifierList() { enable = true };
			var modifierGenerator = new hkbModifierGenerator() { modifier = modifierList, generator = clip };
			if (Flags.HasFlag(AnimFlags.MotionDriven))
			{

			}
		}
		clip.mode = (sbyte)(Flags.HasFlag(AnimFlags.Acyclic) ? PlaybackMode.MODE_SINGLE_PLAY : PlaybackMode.MODE_LOOPING);
		if (Flags.HasFlag(AnimFlags.AnimObjects))
		{
			bool enterEventsExist = stateInfo.enterNotifyEvents != null; 
			bool exitEventsExist = stateInfo.exitNotifyEvents != null;
			var enterEventList = enterEventsExist ? stateInfo.enterNotifyEvents!.events : new List<hkbEventProperty>();
			var exitEventList = exitEventsExist ? stateInfo.exitNotifyEvents!.events : new List<hkbEventProperty>();
			hkbStringEventPayload payload;
			foreach (var animObjectName in animObjectNames)
			{
				payload = new hkbStringEventPayload() { data = animObjectName };
				lock (enterEventList)
				{
					enterEventList.Add(new hkbEventProperty() { id = 393, payload = payload });
					enterEventList.Add(new hkbEventProperty() { id = 394, payload = payload });
				}
				if (Flags.HasFlag(AnimFlags.Sticky)) { continue; }
				lock (exitEventList)
				{
					exitEventList.Add(new hkbEventProperty() { id = 165, payload = null });
				}
			}
			if (!enterEventsExist) stateInfo.enterNotifyEvents = new hkbStateMachineEventPropertyArray() { events = enterEventList };
			if (!exitEventsExist) stateInfo.exitNotifyEvents = new hkbStateMachineEventPropertyArray() { events = exitEventList };
		}
	}
	public virtual bool BuildPatch(FNISAnimationListBuildContext buildContext)
	{
		var project = buildContext.TargetProject;
		var projectManager = buildContext.ProjectManager;
		if (project.Sibling != null) { projectManager.TryActivatePackFile(project.Sibling.CharacterPackFile); }
		projectManager.TryActivatePackFile(project.CharacterPackFile);
		project.CharacterPackFile.AddUniqueAnimation(AnimationFilePath);
		return true;
	}
}
