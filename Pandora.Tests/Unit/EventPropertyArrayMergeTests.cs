// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Linq;
using System.Xml.Linq;
using HKX2E;
using NSubstitute;
using Pandora.API.Patch.Skyrim64;
using Pandora.Skyrim.Hkx.Changes;
using Pandora.Skyrim.Hkx.Packfile;
using XmlCake.Linq;

namespace PandoraTests.Unit;

public class EventPropertyArrayMergeTests
{
	[Fact]
	public void AddedNotifyEvents_WhenMergeIsOff_LastModReplacesTheArray()
	{
		(XMapElement state, HavokXmlDeserializer deserializer) = StateWithNullEnter();
		var dispatcher = new PackFileDispatcher();
		IPackFile packFile = PackFile(state, deserializer, dispatcher);

		Assert.True(Replace("#modA$1").Apply(packFile));
		Assert.True(Replace("#modB$1").Apply(packFile));

		Assert.Equal("#modB$1", EnterValue(state));
		Assert.Equal(["10"], EventIds(deserializer, "#modA$1"));
	}

	[Fact]
	public void AddedNotifyEvents_WhenMergeIsOn_EventsFromEachModAreKept()
	{
		(XMapElement state, HavokXmlDeserializer deserializer) = StateWithNullEnter();
		var dispatcher = new PackFileDispatcher { MergeAddedEventPropertyArrays = true };
		IPackFile packFile = PackFile(state, deserializer, dispatcher);

		Assert.True(Replace("#modA$1").Apply(packFile));
		Assert.True(Replace("#modB$1").Apply(packFile));

		Assert.Equal("#modA$1", EnterValue(state));
		Assert.Equal(["10", "20"], EventIds(deserializer, "#modA$1"));
	}

	[Fact]
	public void ExistingNotifyEvents_WhenMergeIsOn_LaterModStillReplacesThePointer()
	{
		(XMapElement state, HavokXmlDeserializer deserializer) = StateWithNullEnter();
		state
			.Elements()
			.First(element => element.Attribute("name")?.Value == "enterNotifyEvents")
			.Value = "#vanilla$1";
		deserializer.Collect("#vanilla$1", EventArray("#vanilla$1", "1"));
		var dispatcher = new PackFileDispatcher { MergeAddedEventPropertyArrays = true };
		IPackFile packFile = PackFile(state, deserializer, dispatcher);

		Assert.True(Replace("#modA$1").Apply(packFile));

		Assert.Equal("#modA$1", EnterValue(state));
		Assert.Equal(["1"], EventIds(deserializer, "#vanilla$1"));
	}

	private static (XMapElement State, HavokXmlDeserializer Deserializer) StateWithNullEnter()
	{
		var state = new XMapElement(
			new XElement(
				"hkobject",
				new XAttribute("name", "#1821"),
				new XAttribute("class", "hkbStateMachineStateInfo"),
				new XElement("hkparam", new XAttribute("name", "enterNotifyEvents"), "null")
			)
		);
		state.MapAll();
		var deserializer = new HavokXmlDeserializer();
		deserializer.Collect("#modA$1", EventArray("#modA$1", "10"));
		deserializer.Collect("#modB$1", EventArray("#modB$1", "20"));
		return (state, deserializer);
	}

	private static IPackFile PackFile(
		XMapElement state,
		HavokXmlDeserializer deserializer,
		PackFileDispatcher dispatcher
	)
	{
		var packFile = Substitute.For<IPackFile>();
		packFile.XmlDeserializer.Returns(deserializer);
		packFile.Dispatcher.Returns(dispatcher);
		packFile
			.TryGetXMap(Arg.Any<string>(), out Arg.Any<XMapElement?>())
			.Returns(call =>
			{
				call[1] = state;
				return true;
			});
		return packFile;
	}

	private static ReplaceElementChange Replace(string reference)
	{
		return new ReplaceElementChange(
			"#1821",
			"enterNotifyEvents",
			new XElement("hkparam", new XAttribute("name", "enterNotifyEvents"), reference)
		);
	}

	private static XElement EventArray(string name, string eventId)
	{
		return new XElement(
			"hkobject",
			new XAttribute("name", name),
			new XAttribute("class", "hkbStateMachineEventPropertyArray"),
			new XElement(
				"hkparam",
				new XAttribute("name", "events"),
				new XAttribute("numelements", "1"),
				new XElement(
					"hkobject",
					new XElement("hkparam", new XAttribute("name", "id"), eventId),
					new XElement("hkparam", new XAttribute("name", "payload"), "null")
				)
			)
		);
	}

	private static string EnterValue(XMapElement state)
	{
		return state
			.Elements()
			.First(element => element.Attribute("name")?.Value == "enterNotifyEvents")
			.Value.Trim();
	}

	private static string[] EventIds(HavokXmlDeserializer deserializer, string name)
	{
		Assert.True(deserializer.TryGetCollectedElement(name, out XElement? array));
		return array!
			.Elements()
			.First(element => element.Attribute("name")?.Value == "events")
			.Elements()
			.Select(element =>
				element
					.Elements()
					.First(child => child.Attribute("name")?.Value == "id")
					.Value.Trim()
			)
			.ToArray();
	}
}
