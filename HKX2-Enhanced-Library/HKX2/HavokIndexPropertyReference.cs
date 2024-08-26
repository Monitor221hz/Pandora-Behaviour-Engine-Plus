using FastMember;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HKX2E
{
	struct HavokIndexPropertyReference : IHavokReference
	{
		public HavokIndexPropertyReference(HavokObjectReference owner, string propertyName, int index)
		{
			Owner = owner;
			PropertyName = propertyName;
			Index = index;
		}

		public HavokObjectReference Owner { get; }
		public string PropertyName { get; }
		public int Index { get; }
		public void Update<T>(T value) where T : IHavokObject
		{
			IHavokObject target = Owner.Object;
			TypeAccessor accessor = TypeAccessor.Create(target.GetType());
#if DEBUG
			Debug.WriteLine($"{target.GetType()} -> {PropertyName} : {accessor[target, PropertyName].GetType()}");
#endif
			var objects = (accessor[target, PropertyName] as IList<T>);
			if (objects == null)
			{
				dynamic dynamicList = accessor[target, PropertyName];
				if (dynamicList == null)
				{
					throw new InvalidCastException($"Could not cast {PropertyName} to IList<{typeof(T)} or to IList<dynamic>. Is property invalid?");
				}
				dynamicList[Index] = (dynamic)value;
				return;
			}
			objects[Index] = value;
		}
	}
}
