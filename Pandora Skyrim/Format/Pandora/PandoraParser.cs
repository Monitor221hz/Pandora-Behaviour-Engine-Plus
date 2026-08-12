// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Pandora.API.Patch.Skyrim64;
using Pandora.Skyrim.Hkx.Changes;

namespace Pandora.Skyrim.Format.Pandora;

using ChangeType = IPackFileChange.ChangeType;

public class PandoraParser
{
	private const string PATH = "path";
	private const string NAME = "name";
	private const string SKIP = "skip";
	private const string INDEX = "index";
	private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger(); //to do: move logger into inheritable base class
	private static readonly Dictionary<string, ChangeType> ChangeTypeNameMap = Enum.GetValues(
			typeof(ChangeType)
		)
		.Cast<ChangeType>()
		.ToDictionary(c => c.ToString(), v => v, StringComparer.OrdinalIgnoreCase);

	public static void ParseEdit(
		ChangeType changeType,
		XElement element,
		IPackFile packFile,
		IPackFileChangeOwner changeSet
	)
	{
		XAttribute? pathAttribute = element.Attribute(PATH);
		if (pathAttribute == null)
		{
			return;
		}
		if (string.IsNullOrWhiteSpace(pathAttribute.Value))
		{
			if (
				(changeType == ChangeType.Insert || changeType == ChangeType.Append)
				&& element.HasElements
			)
			{
				foreach (var childElement in element.Elements())
				{
					var nameAttribute = childElement.Attribute(NAME);
					if (nameAttribute == null)
					{
						continue;
					}
					string childNodeName = nameAttribute.Value;
					lock (packFile)
					{
						if (!packFile.PopObjectAsXml(childNodeName))
						{
							packFile.Dispatcher.TrackPotentialNode(
								packFile,
								childNodeName,
								childElement
							);
						}
						else
						{
						Logger.Warn(
							$"Pandora Parser > Mod \"{changeSet.Origin.Name}\" > Node \"{childNodeName}\" > Track > FAILED > Already exists in pack file"
						);
						}
					}
				}
				return;
			}
			return;
		}
		int slashIndex = pathAttribute.Value.IndexOf('/');
		if (slashIndex < 1)
		{
		Logger.Warn(
			$"Pandora Parser > Mod \"{changeSet.Origin.Name}\" > Path \"{pathAttribute.Value}\" > Parse > FAILED > Invalid format (missing '/')"
		);
			return;
		}
		string nodeName = pathAttribute.Value.Substring(0, slashIndex);
		switch (changeType)
		{
			case ChangeType.Remove:
			{
				XAttribute? skipAttribute = element.Attribute(SKIP);
				if (skipAttribute == null)
				{
					changeSet.AddChange(new RemoveElementChange(nodeName, pathAttribute.Value));
					break;
				}

				if (string.IsNullOrWhiteSpace(element.Value))
				{
					break;
				}

				changeSet.AddChange(
					new RemoveTextChange(
						nodeName,
						pathAttribute.Value,
						element.Value,
						int.TryParse(skipAttribute.Value, out var findFrom) ? findFrom : 0
					)
				);
				break;
			}

			case ChangeType.Insert:
				if (element.IsEmpty)
				{
					break;
				}
				if (element.HasElements)
				{
					foreach (var childElement in element.Elements())
					{
						changeSet.AddChange(
							new InsertElementChange(nodeName, pathAttribute.Value, childElement)
						);
					}
					break;
				}
				if (string.IsNullOrWhiteSpace(element.Value))
				{
					break;
				}
				XAttribute? indexAttribute = element.Attribute(INDEX);
				changeSet.AddChange(
					new InsertTextChange(
						nodeName,
						pathAttribute.Value,
						int.TryParse(indexAttribute?.Value, out var index) ? index : 0,
						element.Value
					)
				);

				break;
			case ChangeType.Append:
				if (element.IsEmpty)
				{
					break;
				}
				if (element.HasElements)
				{
					foreach (var childElement in element.Elements())
					{
						changeSet.AddChange(
							new AppendElementChange(nodeName, pathAttribute.Value, childElement)
						);
					}
					break;
				}
				if (string.IsNullOrWhiteSpace(element.Value))
				{
					break;
				}
				changeSet.AddChange(
					new AppendTextChange(nodeName, pathAttribute.Value, element.Value)
				);
				break;

			case ChangeType.Replace:
			{
				if (element.IsEmpty || !element.HasElements)
				{
					break;
				}
				var skipAttribute = element.Attribute(SKIP);
				if (skipAttribute == null)
				{
					foreach (var childElement in element.Elements())
					{
						changeSet.AddChange(
							new ReplaceElementChange(nodeName, pathAttribute.Value, childElement) // don't think need clone here, tests needed
						);
					}
					break;
				}
				var textElements = element.Elements().ToList();
				if (
					textElements.Count != 2
					|| string.IsNullOrWhiteSpace(textElements[0].Value)
					|| string.IsNullOrWhiteSpace(textElements[1].Value)
				)
				{
					break;
				}
				changeSet.AddChange(
					new ReplaceTextChange(
						nodeName,
						pathAttribute.Value,
						int.TryParse(skipAttribute.Value, out var skip) ? skip : 0,
						textElements[0].Value,
						textElements[1].Value
					)
				);
				break;
			}
			default:
				break;
		}
	}

	public static void ParseTypedEdits(
		ChangeType changeType,
		XElement container,
		IPackFile packFile,
		IPackFileChangeOwner changeSet
	)
	{
		foreach (var element in container.Elements())
		{
			ParseEdit(changeType, element, packFile, changeSet);
		}
	}

	public static void ParseEdits(
		XElement container,
		IPackFile packFile,
		IPackFileChangeOwner changeSet
	)
	{
		if (!container.HasElements)
		{
			return;
		}
		foreach (var element in container.Elements())
		{
			if (ChangeTypeNameMap.TryGetValue(element.Name.ToString(), out ChangeType changeType))
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
