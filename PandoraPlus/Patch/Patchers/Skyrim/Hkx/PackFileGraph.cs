using Pandora.Core.Patchers.Skyrim;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pandora.Patch.Patchers.Skyrim.Hkx;
public class PackFileGraph : PackFile
{

	private XElement? eventNameContainer;

	private XElement? eventFlagContainer;

	private XElement? variableNameContainer; 

	private XElement? variableValueContainer;

	private XElement? variableTypeContainer;

	public uint InitialEventCount { get; private set; } = 0;
	public uint InitialVariableCount {  get; private set; } = 0;


	public string EventNamesPath { get; private set; }

	public string EventFlagsPath {  get; private set; }

	public string VariableNamesPath { get; private set; }

	public string VariableValuesPath {  get; private set; }

	public string VariableTypesPath {  get; private set; }

	[MemberNotNull(nameof(EventNamesPath), nameof(EventFlagsPath), nameof(VariableNamesPath), nameof(VariableValuesPath), nameof(VariableTypesPath))]
	private void LoadEventsAndVariables()
	{
		TryBuildClassLookup();
		XElement stringDataContainer = classLookup["hkbBehaviorGraphStringData"].First();

		XElement variableValueSetContainer = classLookup["hkbVariableValueSet"].First();

		XElement graphDataContainer = classLookup["hkbBehaviorGraphData"].First();

		

		string stringDataPath = Map.GenerateKey(stringDataContainer);

		string variableValueSetPath = Map.GenerateKey(variableValueSetContainer);

		string graphDataPath = Map.GenerateKey(graphDataContainer);

		MapNode(stringDataPath);
		MapNode(variableValueSetPath);
		MapNode(graphDataPath);

		EventNamesPath = $"{stringDataPath}/eventNames";
		EventFlagsPath = $"{graphDataPath}/eventInfos";

		VariableNamesPath = $"{stringDataPath}/variableNames";
		VariableValuesPath = $"{variableValueSetPath}/wordVariableValues";
		VariableTypesPath = $"{graphDataPath}/variableInfos";

		eventNameContainer = Map.Lookup(EventNamesPath);

		eventFlagContainer = Map.Lookup(EventFlagsPath);

		variableNameContainer = Map.Lookup(VariableNamesPath);

		variableValueContainer = Map.Lookup(VariableValuesPath);

		variableTypeContainer = Map.Lookup(VariableTypesPath);

		InitialEventCount = (uint)eventNameContainer.Elements().Count();
		InitialVariableCount = (uint)variableNameContainer.Elements().Count();	
	}



	public List<string> GetAnimationFilePaths()
	{
		TryBuildClassLookup();

		const string clipGeneratorName = "hkbClipGenerator";
		if (classLookup == null || !classLookup.Contains(clipGeneratorName)) { return new List<string>();  }

		List<string> animationFilePaths = new();

		var clipGenerators = classLookup[clipGeneratorName]; 

		foreach(var clipGenerator in clipGenerators)
		{
			var animationParam = clipGenerator.Elements().Where(e => e.Attribute("name")?.Value == "animationName").FirstOrDefault();
			if (animationParam == null || animationParam.Value.Length == 0) continue;
			
			animationFilePaths.Add(animationParam.Value);
		}
		return animationFilePaths;	
	}

	public PackFileGraph(FileInfo file, Project project) : base(file, project) 
    {

	}

	public PackFileGraph(FileInfo file) : base(file)
	{

	}

	public override void Activate()
	{
		if (!CanActivate()) return;
		base.Activate();
		LoadEventsAndVariables();
	}
	public List<XElement> EventNames => eventNameContainer!.Elements().ToList();

	public List<XElement> EventFlags => eventFlagContainer!.Elements().ToList();

	public List<XElement> VariableNames => variableNameContainer!.Elements().ToList();	

	public List<XElement> VariableValues => variableValueContainer!.Elements().ToList();

	public List<XElement> VariableTypes => variableTypeContainer!.Elements().ToList();
}
