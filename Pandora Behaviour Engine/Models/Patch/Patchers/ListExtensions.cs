using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.Hkx;
public static class ListExtensions
{
	public static IEnumerable<T> FastReverse<T>(this IList<T> items)
	{
		for (int i = items.Count - 1; i >= 0; i--)
		{
			yield return items[i];
		}
	}

}
