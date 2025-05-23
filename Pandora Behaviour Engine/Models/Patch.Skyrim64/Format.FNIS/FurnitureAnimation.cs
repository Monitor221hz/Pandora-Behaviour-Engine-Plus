using HKX2E;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Pandora.Models.Patch.Skyrim64.Format.FNIS;

public class FurnitureAnimation : BasicAnimation
{
	public FurnitureAnimation(Match match) : base(FNISAnimType.OffsetArm, match)
	{

	}
	public FurnitureAnimation(FNISAnimType templateType, FNISAnimFlags flags, string graphEvent, string animationFilePath, List<string> animationObjectNames) : base(templateType, flags, graphEvent, animationFilePath, animationObjectNames)
	{

	}
	private static readonly hkbEventProperty headTrackingOffEventProperty = new() { id = 20 };
	private static readonly hkbEventProperty idleChairSittingProperty = new() { id = 14 };
	private static readonly hkbEventProperty idleFurnitureExitProperty = new() { id = 5 };
	private static readonly hkbEventProperty headTrackingOnEventProperty = new() { id = 18 };
	private static readonly hkbVariableBindingSetBinding animationDrivenBinding = new()
	{
		bindingType = 0,
		bitIndex = -1,
		variableIndex = 1, //bAnimationDriven, 
		memberPath = "isActive",
	};
	private static readonly hkbClipTrigger idleFurnitureExitTrigger = new()
	{
		localTime = -0.2f,
		relativeToEndOfClip = true,
		@event = idleFurnitureExitProperty,
	};



	public override void BuildFlags(FNISAnimationListBuildContext buildContext, PackFileGraph graph, hkbStateMachineStateInfo stateInfo, hkbClipGenerator clip)
	{
		base.BuildFlags(buildContext, graph, stateInfo, clip);
		if (Flags.HasFlag(FNISAnimFlags.SequenceStart) && NextAnimation != null)
		{
			//var lastAnimation = NextAnimation;
			//while (lastAnimation.NextAnimation != null)
			//{
			//	lastAnimation = lastAnimation.NextAnimation;
			//}
			//hkbStateMachineTransitionInfoArray exitTransitionInfoArray = hkbStateMachineTransitionInfoArray.GetDefault();
			//hkbStateMachineTransitionInfo exitTransition = hkbStateMachineTransitionInfo.GetDefault();
			//int exitEventIndex = 152;  // IdleChairExitStart
			//exitTransition.eventId = exitEventIndex;
			//exitTransition.transition = defaultTransition;
			//exitTransition.toStateId = lastAnimation.Hash; 
			//exitTransition.toNestedStateId = lastAnimation.Hash;
			//exitTransitionInfoArray.transitions.Add(exitTransition);
			//stateInfo.transitions = exitTransitionInfoArray;
			//stateInfo.enterNotifyEvents = new()
			//{
			//	events = new List<hkbEventProperty>()
			//	{
			//		new hkbEventProperty()
			//		{
			//			id = 20 // HeadTrackingOff
			//		}
			//	}
			//};
			//stateInfo.exitNotifyEvents = new()
			//{
			//	events = new List<hkbEventProperty>()
			//	{
			//		new hkbEventProperty()
			//		{
			//			id = 14 // IdleChairSitting
			//		}
			//	}
			//};
		}
		if (Flags.HasFlag(FNISAnimFlags.SequenceFinish))
		{
			var arrayObject = CreateEventArrayIfNull(stateInfo.exitNotifyEvents);
			arrayObject.events.Add(headTrackingOnEventProperty);
			stateInfo.exitNotifyEvents = arrayObject;

			var triggerObject = GetOrCreateTriggerArray(clip);

			triggerObject.triggers.Add(idleFurnitureExitTrigger);
			//triggerObject.triggers.Add
			//(
			//	new hkbClipTrigger()
			//	{
			//		localTime = -0.3f,
			//		relativeToEndOfClip = true,
			//		@event = new hkbEventProperty()
			//		{
			//			id = graph.AddDefaultEvent(GraphEvent)
			//		}
			//	}
			//);
			clip.triggers = triggerObject;
		}
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

		hkbStateMachine furnitureBehaviorState = new()
		{
			name = $"{GraphEvent}_Behavior",
			wildcardTransitions = new()
			{
				transitions = []
			}
		};
		furnitureBehaviorState.SetDefault();

		//variable binding set for animation driven
		hkbVariableBindingSet bindingSet = new()
		{
			bindings =
			[
				animationDrivenBinding
			]
		};
		string uniqueStateInfoName = $"{modInfo.Code}_{GraphEvent}_State";
		int stateId = Hash;

		hkbStateMachineStateInfo furnitureGroupStateInfo = new()
		{
			name = uniqueStateInfoName,
			generator = furnitureBehaviorState,
			stateId = stateId,
			variableBindingSet = bindingSet,
			//transitions = graph.GetPushedObjectAs<hkbStateMachineTransitionInfoArray>("#4005")
		};
		furnitureGroupStateInfo.SetDefault();

		//#5195




		furnitureBehaviorState.name = $"Pandora_Furniture_Behavior";

		// declare exit transition array reference in advance
		hkbStateMachineTransitionInfoArray exitTransitionInfoArray = hkbStateMachineTransitionInfoArray.GetDefault();

		//enter furniture state and clip
		int eventIndex = graph.AddDefaultEvent(GraphEvent);
		hkbClipGenerator enterClip = new()
		{
			name = "IdleBlessingKneelEnter",
			//triggers = new()
			//{
			//	triggers = new List<hkbClipTrigger>()
			//	{
			//		new()
			//		{
			//			@event =  new()
			//			{
			//				id = eventIndex
			//			},
			//			localTime = -0.3f,
			//			relativeToEndOfClip = true,
			//			isAnnotation = false,
			//			acyclic = false,
			//		}
			//	}
			//},
			animationName = AnimationFilePath
		};
		enterClip.SetDefault();

		hkbStateMachineStateInfo enterStateInfo = new()
		{
			name = GraphEvent,
			stateId = stateId,
			enterNotifyEvents = new()
			{
				events =
				[
					headTrackingOffEventProperty,
				]
			},
			exitNotifyEvents = new()
			{
				events =
				[
					idleChairSittingProperty,
				]
			},
			generator = enterClip,
			transitions = exitTransitionInfoArray

		};
		enterStateInfo.SetDefault();

		BuildFlags(buildContext, graph, enterStateInfo, enterClip);


		furnitureBehaviorState.states.Add(enterStateInfo);

		int enterEventIndex = graph.AddDefaultEvent(GraphEvent);

		// add root enter transition 
		hkbStateMachineTransitionInfo rootEnterTransition = hkbStateMachineTransitionInfo.GetDefault();
		rootEnterTransition.transition = defaultTransition;
		rootEnterTransition.eventId = enterEventIndex;
		rootEnterTransition.toStateId = 4; // furniture state
		rootEnterTransition.toNestedStateId = stateId;
		rootEnterTransition.flags |= (short)TransitionFlags.FLAG_TO_NESTED_STATE_ID_IS_VALID | (short)TransitionFlags.FLAG_IS_LOCAL_WILDCARD;

		//hkbStateMachineTransitionInfo rootExitTransition = hkbStateMachineTransitionInfo.GetDefault();
		//rootExitTransition.transition = longTransition6;
		//rootExitTransition.eventId = enterEventIndex;
		//rootExitTransition.toStateId = 4;
		//rootExitTransition.toNestedStateId = stateId;
		//rootExitTransition.flags |= (short)TransitionFlags.FLAG_TO_NESTED_STATE_ID_IS_VALID | (short)TransitionFlags.FLAG_IS_LOCAL_WILDCARD;

		hkbStateMachineTransitionInfoArray rootTransitionArray = graph.GetPushedObjectAs<hkbStateMachineTransitionInfoArray>("#0089");
		lock (rootTransitionArray.transitions)
		{
			rootTransitionArray.transitions.Add(rootEnterTransition);
		}

		// add enter transition manually
		hkbStateMachineTransitionInfo transition = hkbStateMachineTransitionInfo.GetDefault();
		transition.transition = defaultTransition;
		transition.eventId = enterEventIndex;
		transition.toStateId = stateId;
		transition.flags |= (short)TransitionFlags.FLAG_IS_LOCAL_WILDCARD | (short)TransitionFlags.FLAG_IS_GLOBAL_WILDCARD;
		if (Flags.HasFlag(FNISAnimFlags.SequenceStart) && NextAnimation != null)
		{
			var lastAnimation = NextAnimation;
			while (lastAnimation.NextAnimation != null)
			{
				lastAnimation = lastAnimation.NextAnimation;
			}
			hkbStateMachineTransitionInfo exitTransition = hkbStateMachineTransitionInfo.GetDefault();
			exitTransition.transition = defaultTransition;
			exitTransition.eventId = 152;
			exitTransition.toStateId = lastAnimation.Hash;
			exitTransition.toNestedStateId = lastAnimation.Hash;
			exitTransition.flags |= (short)TransitionFlags.FLAG_TO_NESTED_STATE_ID_IS_VALID | (short)TransitionFlags.FLAG_IS_LOCAL_WILDCARD;
			furnitureGroupStateInfo.transitions = new hkbStateMachineTransitionInfoArray() { transitions = [] };
			furnitureGroupStateInfo.transitions.transitions.Add(exitTransition);
		}
		furnitureBehaviorState.wildcardTransitions.transitions.Add(transition);

		//IFNISAnimation? animation = this; 
		//IFNISAnimation lastAnimation = NextAnimation;
		//hkbStateMachineStateInfo stateInfo;
		//hkbClipGenerator? clipGenerator = null;
		//bool first = true;

		//while ((animation = animation.NextAnimation) != null)
		//{
		//	lastAnimation = animation;
		//	clipGenerator = new()
		//	{
		//		name = animation.GraphEvent,
		//		animationName = animation.AnimationFilePath
		//	};
		//	clipGenerator.SetDefault();
		//	stateInfo = new()
		//	{
		//		name = $"{animation.GraphEvent}_State",
		//		generator = clipGenerator,
		//		transitions = exitTransitionInfoArray,
		//	};
		//	stateInfo.SetDefault();
		//	animation.BuildFlags(graph, stateInfo, clipGenerator);
		//	stateId = furnitureBehaviorState.AddDefaultStateInfo(stateInfo);
		//	stateInfo.stateId = stateId;

		//	if (first)
		//	{
		//		first = false;
		//	}
		//	else
		//	{
		//		eventIndex = graph.AddDefaultEvent(animation.GraphEvent);
		//	}

		//	hkbStateMachineTransitionInfo enterTransition = hkbStateMachineTransitionInfo.GetDefault();
		//	enterTransition.transition = longTransition6;
		//	enterTransition.eventId = eventIndex;
		//	enterTransition.toStateId = stateId;
		//	enterTransition.flags |= (short)TransitionFlags.FLAG_IS_LOCAL_WILDCARD | (short)TransitionFlags.FLAG_IS_GLOBAL_WILDCARD;

		//	furnitureBehaviorState.wildcardTransitions.transitions.Add(enterTransition);

		//	//stateCount++;
		//}
		//hkbStateMachineTransitionInfo exitTransition = hkbStateMachineTransitionInfo.GetDefault();
		//int exitEventIndex = 152;  // IdleChairExitStart
		//exitTransition.eventId = exitEventIndex;
		//exitTransition.transition = longTransition6;
		//exitTransition.toStateId = furnitureBehaviorState.states.Count - 1; 

		//exitTransitionInfoArray.transitions.Add(exitTransition);
		//var exitState = furnitureBehaviorState.states.Last();
		//exitState.transitions = null;
		//exitState.exitNotifyEvents = new()
		//{
		//	events = new List<hkbEventProperty>()
		//	{
		//		new hkbEventProperty()
		//		{
		//			id = 18 //HeadTrackingOn
		//		}
		//	}
		//};


		//#4002
		hkbStateMachine furnitureGraph = graph.GetPushedObjectAs<hkbStateMachine>("#4002");
		lock (furnitureGraph.states)
		{
			furnitureGraph.states.Add(furnitureGroupStateInfo);
		}

		return true;
	}
}