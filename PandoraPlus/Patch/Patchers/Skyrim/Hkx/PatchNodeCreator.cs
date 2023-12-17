using HKX2;
using Pandora.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pandora.Patch.Patchers.Skyrim.Hkx;
public class PatchNodeCreator
{
	private XmlSerializer serializer = new XmlSerializer();

	private readonly string newNodePrefix;
	private uint nodeCount = 0;

    public PatchNodeCreator(string newPrefix)
    {
        newNodePrefix = newPrefix;
    }
	public string GenerateNodeName(string uniqueName)
	{
		var name = $"#{uniqueName}${nodeCount.ToString()}i";
		nodeCount++;
		return name;
	}
	public XElement TranslateToLinq<T>(T hkobject, string nodeName) where T : IHavokObject
	{
		var element = serializer.WriteDetachedNode<T>(hkobject, nodeName); 
		hkobject.WriteXml(serializer, element);
		return element;
	}

	public hkbBehaviorReferenceGenerator CreateBehaviorReferenceGenerator(string behaviorName, out string nodeName)
	{
		nodeName = GenerateNodeName(behaviorName);
		var behaviorRefNode = new hkbBehaviorReferenceGenerator() { m_name = $"{behaviorName}ReferenceGenerator", m_behaviorName = behaviorName, m_variableBindingSet = null, m_userData = 0 };

		return behaviorRefNode;
	}

	public hkbStateMachineStateInfo CreateSimpleStateInfo(hkbGenerator generator)
	{
		var nodeName = GenerateNodeName(generator.m_name);
		var simpleStateInfo = new hkbStateMachineStateInfo() { m_name = "PN_SimpleStateInfo", m_probability = 1.0f, m_generator = generator, m_stateId = nodeName.GetHashCode() , m_enable = true};

		return simpleStateInfo;
	}

	public hkbStateMachineStateInfo CreateSimpleStateInfo(hkbGenerator generator, out string nodeName)
	{
		nodeName = GenerateNodeName(generator.m_name);
		var simpleStateInfo = new hkbStateMachineStateInfo() { m_name = "PN_SimpleStateInfo", m_probability = 1.0f, m_generator = generator, m_stateId =nodeName.GetHashCode(), m_enable = true };//not working for some reason; out of range? state id collision?

		return simpleStateInfo;
	}
}
