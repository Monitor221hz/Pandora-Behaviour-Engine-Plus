// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Pandora.API.Patch.Skyrim64;
using Pandora.Skyrim.Format.Pandora;
using Pandora.Skyrim.Hkx.Packfile;
using XmlCake.Linq;

namespace Pandora.Skyrim.Hkx.Changes;

public class ReplaceElementChange : IPackFileChange
{
	public IPackFileChange.ChangeType Type { get; } = IPackFileChange.ChangeType.Replace;

	public XmlNodeType AssociatedType { get; } = XmlNodeType.Element;
	public string Target { get; private set; }
	public string Path { get; private set; }
	private XElement _element;

	public ReplaceElementChange(string target, string path, XElement element)
	{
		Target = target;
		Path = path;
		_element = element;
	}

	public bool Apply(IPackFile packFile)
	{
		if (!packFile.TryGetXMap(Target, out var xmap))
		{
			return false;
		}

		if (
			packFile.Dispatcher.MergeAddedEventPropertyArrays
			&& TryMergeAddedNotifyEvents(packFile, xmap!)
		)
		{
			return true;
		}

		_element = PackFileEditor.ReplaceElement(xmap!, Path, _element);
		return _element != null && _element.Parent != null;
	}

	private bool TryMergeAddedNotifyEvents(IPackFile packFile, IXMap xmap)
	{
		if (!IsAddedNotifyPointer(_element, out string parameterName, out string incomingRef))
		{
			return false;
		}

		if (!xmap.TryLookup(Path, out XElement current))
		{
			return false;
		}

		string currentRef = current.Value.Trim();
		if (IsNullPointer(currentRef))
		{
			_element = PackFileEditor.ReplaceElement(xmap, Path, _element);
			if (_element == null || _element.Parent == null)
			{
				return false;
			}

			packFile.Dispatcher.NoteNotifyEventsAddedFromNull(Target, parameterName);
			return true;
		}

		if (
			!packFile.Dispatcher.WereNotifyEventsAddedFromNull(Target, parameterName)
			|| !TryAppendEvents(packFile, currentRef, incomingRef)
		)
		{
			return false;
		}

		return true;
	}

	private static bool TryAppendEvents(IPackFile packFile, string currentRef, string incomingRef)
	{
		if (
			!packFile.XmlDeserializer.TryGetCollectedElement(currentRef, out XElement? existing)
			|| !packFile.XmlDeserializer.TryGetCollectedElement(incomingRef, out XElement? added)
			|| !IsEventPropertyArray(existing)
			|| !IsEventPropertyArray(added)
		)
		{
			return false;
		}

		XElement? existingEvents = FindEvents(existing);
		XElement? addedEvents = FindEvents(added);
		if (existingEvents == null || addedEvents == null)
		{
			return false;
		}

		foreach (XElement child in addedEvents.Elements().ToList())
		{
			existingEvents.Add(new XElement(child));
		}

		existingEvents.SetAttributeValue(
			"numelements",
			existingEvents.Elements().Count().ToString()
		);
		return true;
	}

	private static bool IsAddedNotifyPointer(
		XElement element,
		out string parameterName,
		out string reference
	)
	{
		parameterName = element.Attribute("name")?.Value ?? string.Empty;
		reference = element.Value.Trim();
		return (parameterName == "enterNotifyEvents" || parameterName == "exitNotifyEvents")
			&& reference.StartsWith('#');
	}

	private static bool IsNullPointer(string value)
	{
		return value.Length == 0 || value.Equals("null", StringComparison.OrdinalIgnoreCase);
	}

	private static bool IsEventPropertyArray(XElement element)
	{
		return element.Attribute("class")?.Value == "hkbStateMachineEventPropertyArray";
	}

	private static XElement? FindEvents(XElement array)
	{
		return array
			.Elements()
			.FirstOrDefault(element => element.Attribute("name")?.Value == "events");
	}

	public bool Revert(PackFile packFile)
	{
		return Apply(packFile);
	}

	public XElement AsPandoraEdit()
	{
		return new XElement(Type.ToString(), _element, new XAttribute(PandoraParser.PATH, Path));
	}
}
