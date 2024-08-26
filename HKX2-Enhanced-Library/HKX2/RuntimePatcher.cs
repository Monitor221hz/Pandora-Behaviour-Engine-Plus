using FastMember;
using HKX2E;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKX2E;
public class RuntimePatcher
{
	private static object ResolvePath(object node, string path, out string lastName)
	{
		string[] names = path.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
		TypeAccessor accessor;
		int limit = names.Length - 1;
		for (int i = 0; i < limit; i++)
		{
			string name = names[i];
			accessor = TypeAccessor.Create(node.GetType());
			node = accessor[node, name];
		}
		lastName = names.Last(); 
		return node;
	}
	public static void SetProperty(object node, string path, dynamic value) 
	{
		node = ResolvePath(node, path, out string name);
		TypeAccessor accessor = TypeAccessor.Create(node.GetType());
		accessor[node, name] = value;
	}
	public static void AddPropertyList(object node, string path, dynamic value)
	{
		node = ResolvePath(node, path, out string name);
		TypeAccessor accessor = TypeAccessor.Create(node.GetType());
		dynamic list = accessor[node, name];
		list.Add(value); 
	}
}
