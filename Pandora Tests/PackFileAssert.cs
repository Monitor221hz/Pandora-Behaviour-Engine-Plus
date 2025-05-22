using FastMember;
using HKX2E;
using Pandora.Models.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace PandoraTests;

public static class HavokAssert
{
	private static readonly bool isDebuggerAttached = Debugger.IsAttached;
	private static readonly List<System.Type> types = new()
	{
		typeof(hkbStateMachineStateInfo),
	};
	public static bool IsAllowedNull(System.Type havokType)
	{
		foreach (var type in types)
		{
			if (type.IsAssignableFrom(havokType))
			{
				return true;
			}
		}
		return !havokType.IsAssignableFrom(typeof(hkbGenerator));
	}
	public static void FilterValid(IHavokObject referencedObject)
	{
		switch (referencedObject)
		{
			//case BGSGamebryoSequenceGenerator sequenceGenerator:
			//	break;
			//case BSBoneSwitchGenerator boneSwitchGenerator:
			//	break;
			//case BSBoneSwitchGeneratorBoneData boneSwitchGeneratorBoneData:
			//	break;
			//case BSCyclicBlendTransitionGenerator cyclicBlendTransitionGenerator:
			//	break;
			//case BSiStateTaggingGenerator stateTaggingGenerator:
			//	break;
			//case BSOffsetAnimationGenerator offsetAnimationGenerator:
			//	break;
			//case BSSynchronizedClipGenerator synchronizedClipGenerator:
			//	break;
			//case hkbBehaviorGraph behaviorGraph:
			//	break;
			//case hkbBehaviorReferenceGenerator behaviorReferenceGenerator:
			//	break;
			case hkbBlenderGenerator blenderGenerator:
				Assert.NotEmpty(blenderGenerator.children);
				break;
			//case hkbBlenderGeneratorChild blenderGeneratorChild:
			//	break;
			//case hkbClipGenerator clipGenerator:
			//	break;
			//case hkbGeneratorTransitionEffect generatorTransitionEffect:
			//	break;
			case hkbManualSelectorGenerator manualSelectorGenerator:
				Assert.NotEmpty(manualSelectorGenerator.generators);
				break;
			//case hkbModifierGenerator modifierGenerator:
			//	break;
			//case hkbReferencePoseGenerator referencePoseGenerator:
			//	break;
			//case hkbRegisteredGenerator registeredGenerator:
			//	break;
			//case hkbSetBehaviorCommand setBehaviorCommand:
			//	break;
			case hkbStateMachine stateMachine:
				Assert.NotEmpty(stateMachine.states);
				break;
			case hkbStateMachineStateInfo stateMachineStateInfo:
				break;
				//case hkbTransitionEffect transitionEffect:
				//	break;
				//case hkbGenerator generator:
				//	break;
		}
	}
	private static bool IsList(System.Type type)
	{
		return type.IsAssignableFrom(typeof(IList<>)) || type.IsAssignableFrom(typeof(IEnumerable<>)) || type.GetInterfaces().Any(i => IsList(i));
	}
	public static void Valid(IHavokObject havokObject, HashSet<IHavokObject> visited)
	{
		if (!visited.Add(havokObject))
		{
			return;
		}
		FilterValid(havokObject);
		var type = havokObject.GetType();
		TypeAccessor accessor = TypeAccessor.Create(type);
		var isList = new Predicate<System.Type>(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>));
		//var members = accessor.GetMembers();
		foreach (var propertyInfo in havokObject.GetType().GetProperties())
		{
			var propertyType = propertyInfo.PropertyType;
#if DEBUG
			if (propertyType == typeof(string))
			{

				if (isDebuggerAttached)
				{
					Debug.WriteLine((string)accessor[havokObject, propertyInfo.Name]);

				}
				continue;

			}
#endif
			if (propertyType == typeof(IHavokObject) || propertyType.GetInterfaces().Any(i => i.IsAssignableFrom(typeof(IHavokObject))))
			{
				IHavokObject? propertyValue = (IHavokObject)accessor[havokObject, propertyInfo.Name];
				var nullable = new NullabilityInfoContext().Create(propertyInfo).WriteState != NullabilityState.NotNull;
				if (nullable && IsAllowedNull(propertyType))
				{
					if (propertyValue == null) { continue; }
					Valid(propertyValue, visited);
				}
				else
				{
					NotNullValid(propertyValue, visited);
				}

			}

			if (IsList(propertyType))
			{
				var enumerable = (IEnumerable)accessor[havokObject, propertyInfo.Name];
				foreach (var item in enumerable)
				{
					if (item is not IHavokObject) { break; }
					NotNullValid((IHavokObject)item, visited);
				}
			}
		}
		//		foreach (var member in members)
		//		{
		//#if DEBUG
		//			if (member.Type == typeof(string))
		//			{

		//				if (isDebuggerAttached)
		//				{
		//					Debug.WriteLine((string)accessor[referencedObject, member.Name]);

		//				}
		//				continue;

		//			}
		//#endif
		//			if (member.Type == typeof(IHavokObject) || member.Type.IsSubclassOf(typeof(IHavokObject)))
		//			{
		//				IHavokObject other = (IHavokObject)accessor[referencedObject, member.Name];
		//				if (other != null && visited.Add(other))
		//				{
		//					if (!member.Type.IsValueType && Nullable.GetUnderlyingType(member.Type) != null) // if nullable
		//					{
		//						Valid(other, visited);
		//					}
		//					else
		//					{
		//						NotNullValid(other, visited);
		//					}
		//				}
		//				continue;
		//			}
		//			var obj = accessor[referencedObject, member.Name];
		//			if (obj is IEnumerable container)
		//			{
		//				foreach (var item in container)
		//				{
		//					if (item is IHavokObject refObject)
		//					{
		//						if (visited.Add(refObject))
		//						{
		//							NotNullValid(refObject, visited);
		//						}
		//					}
		//				}
		//				continue;
		//			}
		//		}


	}
	public static void NotNullValid(IHavokObject? referencedObject, HashSet<IHavokObject> visited)
	{
		Assert.NotNull(referencedObject);
		HavokAssert.Valid(referencedObject, visited);
	}

	public static void NotNullValid(IHavokObject? referencedObject)
	{
		NotNullValid(referencedObject, new HashSet<IHavokObject>(ReferenceEqualityComparer.Instance));
	}
}

public class PackFileAssert
{
	/// <summary>
	/// Animations between siblings MUST be IN ORDER and equal.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="sibling"></param>
	/// <returns></returns>
	public static bool ValidSiblingAnimations(PackFileCharacter character, PackFileCharacter sibling)
	{
		for (int i = 0; i < character.AnimationNames.Count; i++)
		{
			if (!character.AnimationNames[i].Equals(sibling.AnimationNames[i], System.StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
		}
		return true;
	}

	public static void ValidPackFile(PackFileGraph graph)
	{
		Assert.NotEmpty(graph.Container.namedVariants);

		var variant = graph.Container.namedVariants.First();
		Assert.NotNull(variant);

		var root = variant.variant as hkbBehaviorGraph;
		HavokAssert.NotNullValid(root);
	}
	public static void ValidPackFile(PackFileCharacter character)
	{
		Assert.NotNull(character.ParentProject);

		if (character.ParentProject.Sibling != null)
		{
			PackFileAssert.ValidSiblingAnimations(character.ParentProject.CharacterPackFile, character.ParentProject.Sibling.CharacterPackFile);
		}
	}

	public static void ValidPackFile(PackFileSkeleton skeleton)
	{

	}
	public static void DowncastValidPackFile(PackFile packFile)
	{
		switch (packFile)
		{
			case PackFileSkeleton skeleton:
				ValidPackFile(skeleton);
				break;
			case PackFileGraph graph:
				ValidPackFile(graph);
				break;
			case PackFileCharacter character:
				ValidPackFile(character);
				break;
			default:
				Assert.Fail($"Could not downcast packfile {packFile.Name} to a type");
				break;
		}

	}
	public static void ValidProject(Project project)
	{

	}
}
