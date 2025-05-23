using HKX2E;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Pandora.Models.Patch.Skyrim64.Format.FNIS;

public partial class BasicAnimation : IFNISAnimation
{
	protected static readonly hkbBlendingTransitionEffect defaultTransition;

	private static readonly Dictionary<string, FNISAnimFlags> animFlagValues = new()
	{
		{ "a", FNISAnimFlags.Acyclic },
		{ "o", FNISAnimFlags.AnimObjects },
		{ "ac", FNISAnimFlags.AnimatedCamera },
		{ "ac1", FNISAnimFlags.AnimatedCameraSet },
		{ "ac0", FNISAnimFlags.AnimatedCameraReset },
		{ "bsa", FNISAnimFlags.BSA },
		{ "h", FNISAnimFlags.Headtracking },
		{ "k", FNISAnimFlags.Known },
		{ "md", FNISAnimFlags.MotionDriven },
		{ "st", FNISAnimFlags.Sticky },
		{ "Tn", FNISAnimFlags.TransitionNext },

	};
	protected static int GetPositiveHash(string name) => name.GetHashCode() & 0xfffffff;
	public FNISAnimType TemplateType { get; private set; } = FNISAnimType.Basic;
	public FNISAnimFlags Flags { get; set; } = FNISAnimFlags.None;

	public bool HasModifier => Flags.HasFlag(FNISAnimFlags.Headtracking) || Flags.HasFlag(FNISAnimFlags.MotionDriven);

	public int Hash { get; private set; }
	public string GraphEvent { get; private set; }
	public string AnimationFilePath { get; private set; }

	private List<string> animObjectNames = [];

	public IFNISAnimation? NextAnimation { get; set; } //unused

	public override string ToString() => $"PN_{TemplateType}_{GraphEvent}";
	static BasicAnimation()
	{
		defaultTransition = hkbBlendingTransitionEffect.GetDefault();
		defaultTransition.name = "DefaultTransition_0.6";
		defaultTransition.duration = 0.6f;
		defaultTransition.eventMode = (sbyte)EventMode.EVENT_MODE_PROCESS_ALL;
	}
	/// <summary>
	/// Assumes that match has the groups specified in the animLine regex.
	/// </summary>
	/// <param name="match"></param>
	public BasicAnimation(FNISAnimType animType, Match match)
	{
		TemplateType = animType;

		if (TemplateType != FNISAnimType.Basic && match.Groups[2].Success)
		{
			var optionValues = match.Groups[2].Value.Split(',');
			foreach (var optionValue in optionValues)
			{
				if (animFlagValues.TryGetValue(optionValue, out FNISAnimFlags animFlags))
				{
					Flags |= animFlags;
				}
			}
		}
		GraphEvent = match.Groups[3].Value;
		AnimationFilePath = match.Groups[4].Value;
		if (TemplateType != FNISAnimType.Basic && Flags.HasFlag(FNISAnimFlags.AnimObjects) && match.Groups[5].Success)
		{
			foreach (Capture capture in match.Groups[5].Captures)
			{
				animObjectNames.Add(capture.Value);
			}
		}
		Hash = GetPositiveHash(GraphEvent);
	}
	public BasicAnimation(Match match) : this(FNISAnimType.Basic, match)
	{

	}

	public BasicAnimation(FNISAnimType templateType, FNISAnimFlags flags, string graphEvent, string animationFilePath, BasicAnimation? nextAnimation, List<string> animationObjectNames)
	{
		TemplateType = templateType;
		Flags = flags;
		GraphEvent = graphEvent;
		AnimationFilePath = animationFilePath;
		NextAnimation = nextAnimation;
		animObjectNames = animationObjectNames;
		Hash = GetPositiveHash(GraphEvent);
	}
	public BasicAnimation(FNISAnimType templateType, FNISAnimFlags flags, string graphEvent, string animationFilePath, List<string> animationObjectNames) : this(templateType, flags, graphEvent, animationFilePath, null, animationObjectNames)
	{
	}
	/// <summary>
	/// Must always reassign the trigger array to the parent clip generator.
	/// </summary>
	/// <param name="clip"></param>
	/// <returns></returns>
	protected static hkbClipTriggerArray GetOrCreateTriggerArray(hkbClipGenerator clip)
	{
		return clip.triggers ?? new hkbClipTriggerArray() { triggers = [] };
	}
	protected static hkbStateMachineEventPropertyArray CreateEventArrayIfNull(hkbStateMachineEventPropertyArray? property)
	{
		return property ?? new hkbStateMachineEventPropertyArray() { events = [] };
	}
	public virtual void BuildFlags(FNISAnimationListBuildContext buildContext, PackFileGraph graph, hkbStateMachineStateInfo stateInfo, hkbClipGenerator clip)
	{
		if (HasModifier)
		{
			var modifierList = new hkbModifierList() { enable = true };
			var modifierGenerator = new hkbModifierGenerator() { modifier = modifierList, generator = clip };
			if (Flags.HasFlag(FNISAnimFlags.MotionDriven))
			{
				//buildContext.TargetProject?.AnimData?.AddDummyClipData(clip.name);
			}
		}
		clip.mode = (sbyte)(Flags.HasFlag(FNISAnimFlags.Acyclic) ? PlaybackMode.MODE_SINGLE_PLAY : PlaybackMode.MODE_LOOPING);
		if (Flags.HasFlag(FNISAnimFlags.AnimObjects))
		{
			bool enterEventsExist = stateInfo.enterNotifyEvents != null;
			bool exitEventsExist = stateInfo.exitNotifyEvents != null;
			var enterEventList = enterEventsExist ? stateInfo.enterNotifyEvents!.events : [];
			var exitEventList = exitEventsExist ? stateInfo.exitNotifyEvents!.events : [];
			hkbStringEventPayload payload;
			foreach (var animObjectName in animObjectNames)
			{
				payload = new hkbStringEventPayload() { data = animObjectName };
				lock (enterEventList)
				{
					enterEventList.Add(new hkbEventProperty() { id = 393, payload = payload });
					enterEventList.Add(new hkbEventProperty() { id = 394, payload = payload });
				}
				if (Flags.HasFlag(FNISAnimFlags.Sticky)) { continue; }
				lock (exitEventList)
				{
					exitEventList.Add(new hkbEventProperty() { id = 165, payload = null });
				}
			}
			if (!enterEventsExist) stateInfo.enterNotifyEvents = new hkbStateMachineEventPropertyArray() { events = enterEventList };
			if (!exitEventsExist) stateInfo.exitNotifyEvents = new hkbStateMachineEventPropertyArray() { events = exitEventList };
		}
		if (Flags.HasFlag(FNISAnimFlags.Acyclic))
		{
			clip.mode = (sbyte)PlaybackMode.MODE_SINGLE_PLAY;
		}
		if (Flags.HasFlag(FNISAnimFlags.SequenceStart) && NextAnimation != null)
		{
			var triggerObject = GetOrCreateTriggerArray(clip);
			triggerObject.triggers.Add
				(
					new hkbClipTrigger()
					{
						localTime = -0.3f,
						relativeToEndOfClip = true,
						@event = new hkbEventProperty()
						{
							id = graph.AddDefaultEvent(NextAnimation.GraphEvent)
						}
					}
				);
			clip.triggers = triggerObject;
		}
		if (Flags.HasFlag(FNISAnimFlags.SequenceFinish))
		{
			var triggerObject = GetOrCreateTriggerArray(clip);
			triggerObject.triggers.Add
			(
				new hkbClipTrigger()
				{
					localTime = -0.2f,
					relativeToEndOfClip = true,
					@event = new hkbEventProperty()
					{
						id = graph.AddDefaultEvent(string.Concat(GraphEvent, "_DONE"))
					}
				}
			);
			triggerObject.triggers.Add
			(
				new hkbClipTrigger()
				{
					localTime = -0.05f,
					relativeToEndOfClip = true,
					@event = new hkbEventProperty()
					{
						id = graph.FindEvent("IdleForceDefaultState")
					}
				}
			);
			clip.triggers = triggerObject;
		}

	}
	public virtual void BuildAnimation(Project project, ProjectManager projectManager)
	{
		if (project.Sibling != null) { projectManager.TryActivatePackFile(project.Sibling.CharacterPackFile); }
		projectManager.TryActivatePackFile(project.CharacterPackFile);
		project.CharacterPackFile.AddUniqueAnimation(AnimationFilePath);
	}
	public virtual bool BuildBehavior(FNISAnimationListBuildContext buildContext)
	{
		//var project = buildContext.TargetProject;
		//var projectManager = buildContext.ProjectManager;
		//if (project.Sibling != null) { projectManager.TryActivatePackFile(project.Sibling.CharacterPackFile); }
		//projectManager.TryActivatePackFile(project.CharacterPackFile);
		//project.CharacterPackFile.AddUniqueAnimation(AnimationFilePath);
		//return (project.BehaviorFile.AddEventBuffer(GraphEvent));
		return true;
	}
}