using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Pandora.Patch.Patchers.Skyrim.Hkx
{
	public class PackFileValidator
	{
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

		private static Regex EventFormat = new Regex(@"[$]{1}eventID{1}[\[]{1}(.+)[\]]{1}[$]{1}");
		private static Regex VarFormat = new Regex(@"[$]{1}variableID{1}[\[]{1}(.+)[\]]{1}[$]{1}");

		private Dictionary<string, int> eventIndices = new Dictionary<string, int>();
		private Dictionary<string, int> variableIndices = new Dictionary<string, int>();


		private int GetIndexFromMatch(Dictionary<string, int> map, Match match)
		{
			if (!match.Success) return -1;

			int index;
			if (!map.TryGetValue(match.Groups[1].Value, out index)) return -1;

			return index;
		}
		public bool ValidateEventsAndVariables(PackFileGraph graph)
		{
			var eventNameElements = graph.EventNames;
			var eventFlagElements = graph.EventFlags;

			var variableNameElements = graph.VariableNames;
			var variableValueElements = graph.VariableValues;
			var variableTypeElements = graph.VariableTypes;


			eventNameElements.Reverse();
			eventFlagElements.Reverse();

			variableNameElements.Reverse();
			variableValueElements.Reverse();
			variableTypeElements.Reverse();
			 //reverse is necessary so that validator doesn't remove the original element if a duplicate is found


			var uniqueEventNames = new HashSet<string>();
			var uniqueVariableNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

			eventIndices.Clear();
			variableIndices.Clear();

			int eventLowerBound = eventNameElements.Count - (int)(graph.InitialEventCount) - 1;
			int variableLowerBound = variableNameElements.Count - (int)(graph.InitialVariableCount) - 1;

			
			for (int i = eventLowerBound; i >= 0; i--)
			{
				var eventNameElement = eventNameElements[i];
				var eventName = eventNameElement.Value.Trim();
				if (!uniqueEventNames.Add(eventName))
				{
					eventNameElement.Remove();
					eventFlagElements[i].Remove();

					eventNameElements.RemoveAt(i);
					eventFlagElements.RemoveAt(i);
					Logger.Warn($"Validator > {graph.ParentProject?.Identifier}~{graph.Name} > Duplicate Event > {eventName} > Index > {i} > REMOVED");
					continue;
				}
//#if DEBUG
//				Logger.Debug($"Validator > {graph.ParentProject?.Identifier}~{graph.Name} > Mapped Event > {eventName} > Index {i}");
//#endif

			}

			for (int i = variableLowerBound; i >= 0; i--)
			{
				var variableNameElement = variableNameElements[i];	
				var variableName = variableNameElement.Value;
				if (!uniqueVariableNames.Add(variableName))
				{
					variableNameElement.Remove();
					variableTypeElements[i].Remove();
					variableValueElements[i].Remove();

					variableNameElements.RemoveAt(i);
					variableTypeElements.RemoveAt(i);
					variableValueElements.RemoveAt(i);
					Logger.Warn($"Validator > {graph.ParentProject?.Identifier}~{graph.Name} > Duplicate Variable > {variableName} > Index > {i} > REMOVED");
					continue; 
				}
//#if DEBUG
//				Logger.Debug($"Validator > {packFile.ParentProject?.Identifier}~{packFile.Name} > Mapped Variable > {variableName} > Index {i}");
//#endif
				
			}
			eventLowerBound -= 2;
			variableLowerBound -= 2;
			for (int i = 0; i < eventLowerBound; i++)
			{
				eventIndices.Add(eventNameElements[i].Value, eventNameElements.Count - 1 - i);
			}
			for (int i = 0; i < variableLowerBound; i++)
			{
				variableIndices.Add(variableNameElements[i].Value, variableNameElements.Count - 1 - i);
			}
			return true; 
		}
		private void ValidateElementText(XElement element, Dictionary<string, int> eventIndices, Dictionary<string, int> variableIndices)
		{
			string rawValue = element.Value;

			var eventMatch = EventFormat.Matches(rawValue);
			foreach (Match match in eventMatch)
			{
				var index = GetIndexFromMatch(eventIndices, match);
				
				rawValue = rawValue.Replace(match.Value, index.ToString());
			}

			var varMatch = VarFormat.Matches(element.Value);
			foreach (Match match in varMatch)
			{
				var index = GetIndexFromMatch(variableIndices, match);
				rawValue = rawValue.Replace(match.Value, index.ToString());
			}
			element.SetValue(rawValue);
		}

		private void ValidateElementContent(XElement element, Dictionary<string, int> eventIndices, Dictionary<string, int> variableIndices)
		{

			if (!element.HasElements)
			{
				ValidateElementText(element, eventIndices, variableIndices);
				return;
			}


			foreach (var xelement in element.Elements())
			{
				
				ValidateElementContent(xelement, eventIndices, variableIndices);
			}

		}

		public void TryValidateClipGenerator(string path, PackFile packFile)
		{
			XElement element;
			if (!packFile.Map.TryLookup($"{path}/animationName", out element)) return;
			string clipName = packFile.Map.Lookup($"{path}/name").Value!;
			packFile.ParentProject?.AnimData?.AddDummyClipData(clipName);
		}

		public void Validate(PackFile packFile, params List<IPackFileChange>[] changeLists)
		{
			//if (!ValidateEventsAndVariables(packFile)) return; 

			foreach(var changeList in changeLists)
			{
				foreach(IPackFileChange change in changeList)
				{
					XElement element;
					if (!packFile.Map.TryLookup(change.Path, out element))
					{
						continue;
					}
					//ValidateElementCount(element.Parent!); might not be needed with hkx2 library; testing needed.
					ValidateElementContent(element, eventIndices, variableIndices);

				}
			}
		}
	}
}
