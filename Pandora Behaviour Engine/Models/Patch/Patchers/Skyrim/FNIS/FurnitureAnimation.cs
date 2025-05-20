using HKX2E;
using Pandora.Patch.Patchers.Skyrim.FNIS;
using Pandora.Patch.Patchers.Skyrim.Hkx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.FNIS;
public class FurnitureAnimation : BasicAnimation
{
	public FurnitureAnimation(Match match) : base(FNISAnimType.OffsetArm, match)
	{

	}
	public FurnitureAnimation(FNISAnimType templateType, FNISAnimFlags flags, string graphEvent, string animationFilePath, List<string> animationObjectNames) : base(templateType, flags, graphEvent, animationFilePath, animationObjectNames)
	{

	}

	public override void BuildFlags(PackFileGraph graph, hkbStateMachineStateInfo stateInfo, hkbClipGenerator clip)
	{
		base.BuildFlags(graph, stateInfo, clip);
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
			var triggerObject = GetOrCreateTriggerArray(clip);

			triggerObject.triggers.Add
			(
				new hkbClipTrigger()
				{
					localTime = -0.2f,
					relativeToEndOfClip = true,
					@event = new hkbEventProperty()
					{
						id = graph.FindEvent("IdleFurnitureExit")
					}
				}
			);
			triggerObject.triggers.Add
			(
				new hkbClipTrigger()
				{
					localTime = -0.3f,
					relativeToEndOfClip = true,
					@event = new hkbEventProperty()
					{
						id = graph.AddDefaultEvent(GraphEvent)
					}
				}
			);
			clip.triggers = triggerObject;
		}
	}
	public override bool BuildPatch(FNISAnimationListBuildContext buildContext)
	{
		var project = buildContext.TargetProject;
		var projectManager = buildContext.ProjectManager;
		var modInfo = buildContext.ModInfo;

		if (!base.BuildPatch(buildContext) || !project.TryLookupPackFile("mt_behavior", out var targetPackFile) || targetPackFile is not PackFileGraph graph) //only supports humanoids as FNIS does
		{
			return false;
		}

		hkbStateMachine furnitureBehaviorState = new()
		{
			name = $"{GraphEvent}_Behavior",
			wildcardTransitions = new()
			{
				transitions = new List<hkbStateMachineTransitionInfo>()
			}
		};
		furnitureBehaviorState.SetDefault();

		//variable binding set for animation driven
		hkbVariableBindingSet bindingSet = new()
		{
			bindings = new List<hkbVariableBindingSetBinding>()
			{
				new()
				{
					bindingType = 0, 
					bitIndex = -1, 
					variableIndex = 1, //bAnimationDriven, 
					memberPath = "isActive",
				}
			}
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
				events = new List<hkbEventProperty>()
				{
					new hkbEventProperty()
					{
						id = 20 // HeadTrackingOff
					}
				}
			},
			exitNotifyEvents = new()
			{
				events = new List<hkbEventProperty>()
				{
					new hkbEventProperty()
					{
						id = 14 // IdleChairSitting
					}
				}
			},
			generator = enterClip,
			transitions = exitTransitionInfoArray
			
		};
		enterStateInfo.SetDefault();

		BuildFlags(graph, enterStateInfo, enterClip);
		

		furnitureBehaviorState.states.Add(enterStateInfo);

		// reusable transition effect
		hkbBlendingTransitionEffect longTransition6 = hkbBlendingTransitionEffect.GetDefault();
		longTransition6.name = "LongBlendTransition_0.6";
		longTransition6.duration = 0.6f;
		longTransition6.eventMode = (sbyte)EventMode.EVENT_MODE_PROCESS_ALL;

		int enterEventIndex = graph.AddDefaultEvent(GraphEvent);

		// add root enter transition 
		hkbStateMachineTransitionInfo rootEnterTransition = hkbStateMachineTransitionInfo.GetDefault();
		rootEnterTransition.transition = longTransition6;
		rootEnterTransition.eventId = enterEventIndex;
		rootEnterTransition.toStateId = 4;
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
		transition.transition = longTransition6;
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
			//furnitureBehaviorState.wildcardTransitions.transitions.Add(exitTransition);
			furnitureGroupStateInfo.transitions = new hkbStateMachineTransitionInfoArray() { transitions = new List<hkbStateMachineTransitionInfo>() };
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
