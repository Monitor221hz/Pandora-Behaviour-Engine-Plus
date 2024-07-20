using HKX2;
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
	public XmlSerializer Serializer { get; } = new XmlSerializer();

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
	public bool AddAnimationPath(PackFileCharacter characterPackFile, PackFileChangeSet changeSet, string animationPath)
	{
		changeSet.AddChange(new AppendElementChange(characterPackFile.AnimationNamesPath, TranslateToXml(animationPath)));
		return true; 
	}
	public bool AddDefaultEvent(PackFileGraph graphPackFile, PackFileChangeSet changeSet, string eventName)
	{
		changeSet.AddChange(new AppendElementChange(graphPackFile.EventNamesPath, TranslateToXml(eventName)));
		changeSet.AddChange(new AppendElementChange(graphPackFile.EventFlagsPath, TranslateToXml(TranslateToXml("flags", "0"))));
		return true; 
	}
	public bool AddDefaultEvent(PackFileGraph graphPackFile, PackFileTargetCache targetCache, string eventName)
	{
		targetCache.AddChange(graphPackFile, new AppendElementChange(graphPackFile.EventNamesPath, TranslateToXml(eventName)));
		targetCache.AddChange(graphPackFile, new AppendElementChange(graphPackFile.EventFlagsPath, TranslateToXml(TranslateToXml("flags", "0"))));
		return true; 
	}
	public hkbBehaviorReferenceGenerator CreateBehaviorReferenceGenerator(string behaviorName, out string nodeName)
	{
		nodeName = GenerateNodeName(behaviorName);
		var behaviorRefNode = new hkbBehaviorReferenceGenerator() { m_name = $"{behaviorName}ReferenceGenerator", m_behaviorName = behaviorName, m_variableBindingSet = null, m_userData = 0 };

		return behaviorRefNode;
	}
	public hkbBehaviorReferenceGenerator CreateBehaviorReferenceGenerator(string generatorName, string behaviorName, out string nodeName)
	{
		nodeName = GenerateNodeName(generatorName);
		var behaviorRefNode = new hkbBehaviorReferenceGenerator() { m_name = $"{generatorName}_RG", m_behaviorName = behaviorName, m_variableBindingSet = null, m_userData = 0 };

		return behaviorRefNode;
	}

	public hkbStateMachineStateInfo CreateSimpleStateInfo(hkbGenerator generator)
	{
		var nodeName = GenerateNodeName(generator.m_name);
		var simpleStateInfo = new hkbStateMachineStateInfo() { m_name = "PN_SimpleStateInfo", m_probability = 1.0f, m_generator = generator, m_stateId = (nodeName.GetHashCode() & 0xfffffff), m_enable = true};

		return simpleStateInfo;
	}

	public hkbStateMachineStateInfo CreateSimpleStateInfo(hkbGenerator generator, out string nodeName)
	{
		nodeName = GenerateNodeName(generator.m_name);
		var simpleStateInfo = new hkbStateMachineStateInfo() { m_name = "PN_SimpleStateInfo", m_probability = 1.0f, m_generator = generator, m_stateId = (nodeName.GetHashCode() & 0xfffffff), m_enable = true };//not working for some reason; out of range? state id collision?

		return simpleStateInfo;
	}
}
