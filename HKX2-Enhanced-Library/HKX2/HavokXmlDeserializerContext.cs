using System.Collections.Generic;
using System.Xml.Linq;

namespace HKX2E
{
	public struct HavokXmlDeserializerContext
	{
		public HavokXmlDeserializerContext(Dictionary<string, IHavokObject> objectNameMap, Dictionary<string, XElement> elementNameMap, HavokXmlDeserializerOptions options)
		{
			ObjectNameMap = objectNameMap;
			ElementNameMap = elementNameMap;
			Options = options;
		}

		public Dictionary<string, IHavokObject> ObjectNameMap { get; set; } = new();
		public Dictionary<string, XElement> ElementNameMap { get; set; } = new(); 
		public HavokXmlDeserializerOptions Options { get; set; } = new();
	}
}
