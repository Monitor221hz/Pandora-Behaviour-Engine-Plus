using Pandora.Models.Patch.Skyrim64.Hkx.Changes;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Pandora.Models.Patch.Skyrim64.Format.Pandora;
using ChangeType = IPackFileChange.ChangeType;
public class PandoraParser
{
	private static readonly Dictionary<string, ChangeType> changeTypeNameMap = Enum.GetValues(typeof(ChangeType)).Cast<ChangeType>().ToDictionary(c => c.ToString(), v => v, StringComparer.OrdinalIgnoreCase);

	public static void ParseEdit(ChangeType changeType, XElement element, PackFile packFile, PackFileChangeSet changeSet)
	{
		XAttribute? pathAttribute = element.Attribute("path");
		if (pathAttribute == null) { return; }

		bool isPathEmpty = string.IsNullOrWhiteSpace(pathAttribute.Value);

		XAttribute? textAttribute = element.Attribute("text");
		XAttribute? preTextAttribute = element.Attribute("preText");
		string nodeName = pathAttribute.Value.Substring(0, pathAttribute.Value.IndexOf('/'));
		switch (changeType)
		{
			case ChangeType.Remove:
				if (textAttribute == null)
				{
					changeSet.AddChange(new RemoveElementChange(nodeName, pathAttribute.Value));
					break;
				}
				// assume text
				if (string.IsNullOrWhiteSpace(element.Value) || string.IsNullOrWhiteSpace(textAttribute.Value)) { break; }

				if (preTextAttribute == null)
				{
					changeSet.AddChange(new RemoveTextChange(nodeName, pathAttribute.Value, textAttribute.Value));
					break;
				}
				changeSet.AddChange(new ReplaceTextChange(nodeName, pathAttribute.Value, preTextAttribute.Value, textAttribute.Value, string.Empty));

				break;

			case ChangeType.Insert:
				if (element.IsEmpty) { break; }
				if (element.HasElements)
				{
					if (!isPathEmpty)
					{
						foreach (var childElement in element.Elements()) { changeSet.AddChange(new InsertElementChange(nodeName, pathAttribute.Value, childElement)); }
						break;
					}

					foreach (var childElement in element.Elements())
					{
						var nameAttribute = childElement.Attribute("name");
						if (nameAttribute == null) { continue; }
						string childNodeName = nameAttribute.Value;
						if (!packFile.PopObjectAsXml(childNodeName))
						{
							packFile.PartialDeserializer.DeserializeRuntimeObject(element);
						}
					}
					break;
				}
				if (textAttribute == null || isPathEmpty) { break; }

				changeSet.AddChange(new InsertTextChange(nodeName, pathAttribute.Value, textAttribute.Value, element.Value));

				break;
			case ChangeType.Append:
				if (element.IsEmpty) { break; }
				if (element.HasElements)
				{
					if (!isPathEmpty)
					{
						foreach (var childElement in element.Elements()) { changeSet.AddChange(new AppendElementChange(nodeName, pathAttribute.Value, childElement)); }
						break;
					}

					foreach (var childElement in element.Elements())
					{
						var nameAttribute = childElement.Attribute("name");
						if (nameAttribute == null) { continue; }
						string childNodeName = nameAttribute.Value;
						if (!packFile.PopObjectAsXml(childNodeName))
						{
							packFile.PartialDeserializer.DeserializeRuntimeObject(element);
						}
					}
					break;
				}

				if (isPathEmpty) { break; }
				changeSet.AddChange(new AppendTextChange(nodeName, pathAttribute.Value, element.Value));

				break;

			case ChangeType.Replace:
				if (element.IsEmpty || isPathEmpty) { break; }
				if (textAttribute == null && element.HasElements)
				{
					foreach (var childElement in element.Elements()) { changeSet.AddChange(new ReplaceElementChange(nodeName, pathAttribute.Value, new XElement(childElement))); }
					break;
				}
				if (textAttribute == null) { break; }
				if (preTextAttribute == null)
				{
					changeSet.AddChange(new ReplaceTextChange(nodeName, pathAttribute.Value, string.Empty, textAttribute.Value, element.Value));
					break;
				}
				changeSet.AddChange(new ReplaceTextChange(nodeName, pathAttribute.Value, preTextAttribute.Value, textAttribute.Value, element.Value));
				break;

			default:
				break;


		}

	}
	public static void ParseTypedEdits(ChangeType changeType, XElement container, PackFile packFile, PackFileChangeSet changeSet)
	{
		foreach (var element in container.Elements())
		{
			ParseEdit(changeType, element, packFile, changeSet);
		}
	}

	public static void ParseEdits(XElement container, PackFile packFile, PackFileChangeSet changeSet)
	{
		if (!container.HasElements) { return; }
		foreach (var element in container.Elements())
		{

			if (changeTypeNameMap.TryGetValue(element.Name.ToString(), out ChangeType changeType))
			{
				if (element.HasAttributes)
				{
					ParseEdit(changeType, element, packFile, changeSet);
					continue;
				}
				ParseTypedEdits(changeType, element, packFile, changeSet);
				continue;
			}
			ParseEdits(element, packFile, changeSet);

		}
	}
}
