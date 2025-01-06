using HKX2E;
using Pandora.API.Patch;
using Pandora.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pandora.Patch.Patchers.Skyrim.Hkx;
public class PatchNodeCreator
{
	public HavokXmlSerializer Serializer { get; } = new HavokXmlSerializer();

	private readonly string newNodePrefix;

	private const string havokStringName = "hkcstring";
	private const string havokParamName = "hkparam";
	private HashSet<string> uniqueNames = new(StringComparer.OrdinalIgnoreCase);
	private uint nodeCount = 0;

    public PatchNodeCreator(string newPrefix)
    {
        newNodePrefix = newPrefix;
    }
	public static XElement TranslateToXml(string str)
	{
		return new XElement(havokStringName, str);
	}
	public static XElement TranslateToXml(string name, string value)
	{
		return new XElement(havokParamName, new XAttribute("name", name), value);
	}
	public static XElement TranslateToXml(XElement element)
	{
		var hkObject = new XElement("hkobject"); 
		hkObject.Add(element);
		return hkObject;
	}
	public static string AsEventFormat(string name)
	{
		return $"$eventID[{name}]$";
	}
	public bool IsUnique(string uniqueName) => uniqueNames.Contains(uniqueName);
	public string GenerateNodeName(IModInfo modInfo, string uniqueName)
	{
		var name = $"#{modInfo.Code}_{uniqueName}${nodeCount.ToString()}";
		nodeCount = nodeCount == uint.MaxValue ? 0 : nodeCount + 1;
		return name; 
	}
	/// <summary>
	/// There is a need to generate intentionally collidable node names to resolve inter-mod duplications of common objects like string event payloads during xml deserialization.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="modInfo"></param>
	/// <param name="uniqueName"></param>
	/// <returns></returns>
	public string GenerateCollidableNodeName(IModInfo modInfo, string uniqueName)
	{
		return $"#{uniqueName}";
	}
	public string GenerateNodeName(string uniqueName)
	{
		var name = $"#{uniqueName}${nodeCount.ToString()}";
		nodeCount = nodeCount == uint.MaxValue ? 0 : nodeCount + 1; 
		return name;
	}
	public string GenerateNumericNodeName<T>(IModInfo modInfo, string uniqueName)
	{
		var hash = new HashCode();
		hash.Add(typeof(T).Name);
		hash.Add(modInfo.Name);
		hash.Add(uniqueName);
		return hash.ToHashCode().ToString(); 
	}
	public hkbBehaviorReferenceGenerator CreateBehaviorReferenceGenerator(string behaviorName, out string nodeName)
	{
		nodeName = GenerateNodeName(behaviorName);
		var behaviorRefNode = new hkbBehaviorReferenceGenerator() { name = $"{behaviorName}ReferenceGenerator", behaviorName = behaviorName, variableBindingSet = null, userData = 0 };

		return behaviorRefNode;
	}
	public hkbBehaviorReferenceGenerator CreateBehaviorReferenceGenerator(string generatorName, string behaviorName, out string nodeName)
	{
		nodeName = GenerateNodeName(generatorName);
		var behaviorRefNode = new hkbBehaviorReferenceGenerator() { name = $"{generatorName}_RG", behaviorName = behaviorName, variableBindingSet = null, userData = 0 };

		return behaviorRefNode;
	}

	public hkbStateMachineStateInfo CreateSimpleStateInfo(hkbGenerator generator)
	{
		var nodeName = GenerateNodeName(generator.name);
		var simpleStateInfo = new hkbStateMachineStateInfo() { name = "PN_SimpleStateInfo", probability = 1.0f, generator = generator, stateId = (nodeName.GetHashCode() & 0xfffffff), enable = true};

		return simpleStateInfo;
	}

	public hkbStateMachineStateInfo CreateSimpleStateInfo(hkbGenerator generator, out string nodeName)
	{
		nodeName = GenerateNodeName(generator.name);
		var simpleStateInfo = new hkbStateMachineStateInfo() { name = "PN_SimpleStateInfo", probability = 1.0f, generator = generator, stateId = (nodeName.GetHashCode() & 0xfffffff), enable = true };//not working for some reason; out of range? state id collision?

		return simpleStateInfo;
	}
}
