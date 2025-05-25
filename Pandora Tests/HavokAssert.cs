using FastMember;
using HKX2E;
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
			case hkbBlenderGenerator blenderGenerator:
				Assert.NotEmpty(blenderGenerator.children);
				break;
			case hkbManualSelectorGenerator manualSelectorGenerator:
				Assert.NotEmpty(manualSelectorGenerator.generators);
				break;
			case hkbStateMachine stateMachine:
				Assert.NotEmpty(stateMachine.states);
				break;
			case hkbStateMachineStateInfo stateMachineStateInfo:
				break;
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
		foreach (var propertyInfo in havokObject.GetType().GetProperties())
		{
			var propertyType = propertyInfo.PropertyType;

			if (propertyType == typeof(string))
			{

#if DEBUG

				if (isDebuggerAttached)
				{
					Debug.WriteLine((string)accessor[havokObject, propertyInfo.Name]);

				}

#endif
				continue;
			}

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
