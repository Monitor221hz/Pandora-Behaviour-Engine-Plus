using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pandora.Patch.Patchers.Skyrim.AnimSetData
{
    public class ProjectAnimSetData
	{
		public int NumSets { get; private set; } = 1;

		public List<string> AnimSetFileNames { get; private set; } = new List<string>();

		public List<AnimSet> AnimSets { get; private set; } = new List<AnimSet>();

		public Dictionary<string, AnimSet> AnimSetsByName { get; private set; } = new Dictionary<string, AnimSet>();

		public static ProjectAnimSetData Read(StreamReader reader)
		{
			var setData = new ProjectAnimSetData();

			int numSets;

			if (!int.TryParse(reader.ReadLine(), out numSets)) { return setData; }
			setData.NumSets = numSets;

			for (int i = 0; i < numSets; i++) 
			{ 
				var fileName =  reader.ReadLineSafe();
				setData.AnimSetFileNames.Add(fileName);
			}

			for (int i = 0; i < numSets; i++)
			{
				var animSet = AnimSet.Read(reader);
				setData.AnimSets.Add(animSet);
				setData.AnimSetsByName.Add(setData.AnimSetFileNames[i], animSet);
			}

			return setData;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine(NumSets.ToString());
			if (NumSets > 0) 
			{ 
				sb.AppendJoin("\r\n", AnimSetFileNames).AppendLine();
				sb.AppendJoin("", AnimSets);
			}

			return sb.ToString();

		}
	}
}
