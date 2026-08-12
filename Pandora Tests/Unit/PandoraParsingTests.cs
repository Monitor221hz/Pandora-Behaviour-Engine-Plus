// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using System.Xml.Linq;
using NSubstitute;
using Pandora.API.Patch;
using Pandora.API.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Format.Pandora;
using Pandora.Models.Patch.Skyrim64.Hkx.Changes;

using ChangeType = Pandora.API.Patch.Skyrim64.IPackFileChange.ChangeType;

namespace PandoraTests.Unit
{
	public class PandoraParsingTests
	{
		private const string NodeName = "#0069";
		private const string Path = "#0069/property";

		private static IPackFile NewPackFile() => Substitute.For<IPackFile>();

		private static IPackFileChangeOwner NewChangeSet() =>
			Substitute.For<IPackFileChangeOwner>();

		[Fact]
		public void ParseEdit_WhenPathAttributeMissing_NoChange()
		{
			var element = new XElement("Remove");
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Remove, element, packFile, changeSet);

			changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
		}

		[Fact]
		public void ParseEdit_WhenPathWithoutSlash_NoChange()
		{
			var element = new XElement("Remove", new XAttribute("path", "invalid"));
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Remove, element, packFile, changeSet);

			changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
		}

		[Fact]
		public void ParseEdit_WhenPathStartsWithSlash_NoChange()
		{
			var element = new XElement("Remove", new XAttribute("path", "/property"));
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Remove, element, packFile, changeSet);

			changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
		}

		[Fact]
		public void ParseEdit_WhenRemoveWithoutSkip_AddsRemoveElementChange()
		{
			var element = new XElement("Remove", new XAttribute("path", Path));
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Remove, element, packFile, changeSet);

			changeSet.Received().AddChange(Arg.Any<RemoveElementChange>());
		}

		[Fact]
		public void ParseEdit_WhenRemoveWithSkipAndText_AddsRemoveTextChange()
		{
			var element = new XElement(
				"Remove",
				new XAttribute("path", Path),
				new XAttribute("skip", "3"),
				new XText("target text")
			);
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Remove, element, packFile, changeSet);

			changeSet.Received().AddChange(Arg.Any<RemoveTextChange>());
		}

		[Fact]
		public void ParseEdit_WhenRemoveWithSkipButEmptyText_NoChange()
		{
			var element = new XElement(
				"Remove",
				new XAttribute("path", Path),
				new XAttribute("skip", "3")
			);
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Remove, element, packFile, changeSet);

			changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
		}

		[Fact]
		public void ParseEdit_WhenRemoveWithNonNumericSkip_AddsRemoveTextChangeWithDefaultZero()
		{
			var element = new XElement(
				"Remove",
				new XAttribute("path", Path),
				new XAttribute("skip", "abc"),
				new XText("target text")
			);
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Remove, element, packFile, changeSet);

			changeSet.Received().AddChange(Arg.Any<RemoveTextChange>());
		}

		[Fact]
		public void ParseEdit_WhenInsertEmpty_NoChange()
		{
			var element = new XElement("Insert", new XAttribute("path", Path));
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Insert, element, packFile, changeSet);

			changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
		}

		[Fact]
		public void ParseEdit_WhenInsertElements_AddsMultipleInsertElementChanges()
		{
			var element = new XElement(
				"Insert",
				new XAttribute("path", Path),
				new XElement("child1"),
				new XElement("child2"),
				new XElement("child3")
			);
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Insert, element, packFile, changeSet);

			changeSet.Received(3).AddChange(Arg.Any<InsertElementChange>());
		}

		[Fact]
		public void ParseEdit_WhenInsertText_AddsInsertTextChange()
		{
			var element = new XElement(
				"Insert",
				new XAttribute("path", Path),
				new XAttribute("index", "2"),
				new XText("new text")
			);
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Insert, element, packFile, changeSet);

			changeSet.Received().AddChange(Arg.Any<InsertTextChange>());
		}

		[Fact]
		public void ParseEdit_WhenInsertTextWithoutIndex_AddsInsertTextChangeWithDefaultZero()
		{
			var element = new XElement(
				"Insert",
				new XAttribute("path", Path),
				new XText("new text")
			);
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Insert, element, packFile, changeSet);

			changeSet.Received().AddChange(Arg.Any<InsertTextChange>());
		}

		[Fact]
		public void ParseEdit_WhenInsertWhitespaceText_NoChange()
		{
			var element = new XElement(
				"Insert",
				new XAttribute("path", Path),
				new XText("   ")
			);
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Insert, element, packFile, changeSet);

			changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
		}

		[Fact]
		public void ParseEdit_WhenAppendEmpty_NoChange()
		{
			var element = new XElement("Append", new XAttribute("path", Path));
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Append, element, packFile, changeSet);

			changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
		}

		[Fact]
		public void ParseEdit_WhenAppendElements_AddsMultipleAppendElementChanges()
		{
			var element = new XElement(
				"Append",
				new XAttribute("path", Path),
				new XElement("child1"),
				new XElement("child2")
			);
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Append, element, packFile, changeSet);

			changeSet.Received(2).AddChange(Arg.Any<AppendElementChange>());
		}

		[Fact]
		public void ParseEdit_WhenAppendText_AddsAppendTextChange()
		{
			var element = new XElement(
				"Append",
				new XAttribute("path", Path),
				new XText("appended text")
			);
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Append, element, packFile, changeSet);

			changeSet.Received().AddChange(Arg.Any<AppendTextChange>());
		}

		[Fact]
		public void ParseEdit_WhenAppendWhitespaceText_NoChange()
		{
			var element = new XElement(
				"Append",
				new XAttribute("path", Path),
				new XText("  ")
			);
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Append, element, packFile, changeSet);

			changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
		}

		[Fact]
		public void ParseEdit_WhenReplaceWithoutSkip_AddsReplaceElementChange()
		{
			var element = new XElement(
				"Replace",
				new XAttribute("path", Path),
				new XElement("NewElement")
			);
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Replace, element, packFile, changeSet);

			changeSet.Received().AddChange(Arg.Any<ReplaceElementChange>());
		}

		[Fact]
		public void ParseEdit_WhenReplaceMultipleElements_AddsMultipleReplaceChanges()
		{
			var element = new XElement(
				"Replace",
				new XAttribute("path", Path),
				new XElement("NewElement1"),
				new XElement("NewElement2")
			);
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Replace, element, packFile, changeSet);

			changeSet.Received(2).AddChange(Arg.Any<ReplaceElementChange>());
		}

		[Fact]
		public void ParseEdit_WhenReplaceWithSkipAndTwoTextChildren_AddsReplaceTextChange()
		{
			var element = new XElement(
				"Replace",
				new XAttribute("path", Path),
				new XAttribute("skip", "1"),
				new XElement("old", new XText("OldText")),
				new XElement("new", new XText("NewText"))
			);
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Replace, element, packFile, changeSet);

			changeSet.Received().AddChange(Arg.Any<ReplaceTextChange>());
		}

		[Fact]
		public void ParseEdit_WhenReplaceWithSkipButSingleChild_NoChange()
		{
			var element = new XElement(
				"Replace",
				new XAttribute("path", Path),
				new XAttribute("skip", "1"),
				new XElement("only", new XText("value"))
			);
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Replace, element, packFile, changeSet);

			changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
		}

		[Fact]
		public void ParseEdit_WhenReplaceWithSkipButWhitespaceChild_NoChange()
		{
			var element = new XElement(
				"Replace",
				new XAttribute("path", Path),
				new XAttribute("skip", "1"),
				new XElement("old", new XText("   ")),
				new XElement("new", new XText("value"))
			);
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Replace, element, packFile, changeSet);

			changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
		}

		[Fact]
		public void ParseEdit_WhenReplaceEmpty_NoChange()
		{
			var element = new XElement("Replace", new XAttribute("path", Path));
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Replace, element, packFile, changeSet);

			changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
		}

		[Fact]
		public void ParseEdit_WhenPathEmptyAndInsertWithElements_TracksNewNodes()
		{
			var element = new XElement(
				"Insert",
				new XAttribute("path", ""),
				new XElement("child", new XAttribute("name", "newnode1")),
				new XElement("child", new XAttribute("name", "newnode2"))
			);
			var packFile = NewPackFile();
			var dispatcher = Substitute.For<IPackFileDispatcher>();
			packFile.Dispatcher.Returns(dispatcher);
			packFile.PopObjectAsXml(Arg.Any<string>()).Returns(false);
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Insert, element, packFile, changeSet);

			dispatcher.Received(2)
				.TrackPotentialNode(
					Arg.Any<IPackFile>(),
					Arg.Any<string>(),
					Arg.Any<XElement>()
				);
			changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
		}

		[Fact]
		public void ParseEdit_WhenPathEmptyAndAppendWithElements_TracksNewNodes()
		{
			var element = new XElement(
				"Append",
				new XAttribute("path", ""),
				new XElement("child", new XAttribute("name", "newnode"))
			);
			var packFile = NewPackFile();
			var dispatcher = Substitute.For<IPackFileDispatcher>();
			packFile.Dispatcher.Returns(dispatcher);
			packFile.PopObjectAsXml(Arg.Any<string>()).Returns(false);
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Append, element, packFile, changeSet);

			dispatcher.Received(1)
				.TrackPotentialNode(
					Arg.Any<IPackFile>(),
					Arg.Any<string>(),
					Arg.Any<XElement>()
				);
		}

		[Fact]
		public void ParseEdit_WhenPathEmptyAndInsertWithChildMissingName_SkipsChild()
		{
			var element = new XElement(
				"Insert",
				new XAttribute("path", ""),
				new XElement("child", new XAttribute("notname", "value"))
			);
			var packFile = NewPackFile();
			var dispatcher = Substitute.For<IPackFileDispatcher>();
			packFile.Dispatcher.Returns(dispatcher);
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Insert, element, packFile, changeSet);

			dispatcher.DidNotReceive()
				.TrackPotentialNode(
					Arg.Any<IPackFile>(),
					Arg.Any<string>(),
					Arg.Any<XElement>()
				);
		}

		[Fact]
		public void ParseEdit_WhenPathEmptyAndInsertWithExistingNode_LogsWarning_NoTracking()
		{
			var element = new XElement(
				"Insert",
				new XAttribute("path", ""),
				new XElement("child", new XAttribute("name", "existing"))
			);
			var packFile = NewPackFile();
			var dispatcher = Substitute.For<IPackFileDispatcher>();
			packFile.Dispatcher.Returns(dispatcher);
			packFile.PopObjectAsXml(Arg.Any<string>()).Returns(true);
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Insert, element, packFile, changeSet);

			dispatcher.DidNotReceive()
				.TrackPotentialNode(
					Arg.Any<IPackFile>(),
					Arg.Any<string>(),
					Arg.Any<XElement>()
				);
			changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
		}

		[Fact]
		public void ParseEdit_WhenPathEmptyAndRemoveWithElements_NoTracking()
		{
			var element = new XElement(
				"Remove",
				new XAttribute("path", ""),
				new XElement("child", new XAttribute("name", "newnode"))
			);
			var packFile = NewPackFile();
			var dispatcher = Substitute.For<IPackFileDispatcher>();
			packFile.Dispatcher.Returns(dispatcher);
			packFile.PopObjectAsXml(Arg.Any<string>()).Returns(false);
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Remove, element, packFile, changeSet);

			dispatcher.DidNotReceive()
				.TrackPotentialNode(
					Arg.Any<IPackFile>(),
					Arg.Any<string>(),
					Arg.Any<XElement>()
				);
			changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
		}

		[Fact]
		public void ParseEdit_WhenPathEmptyAndReplaceWithElements_NoTracking()
		{
			var element = new XElement(
				"Replace",
				new XAttribute("path", ""),
				new XElement("child", new XAttribute("name", "newnode"))
			);
			var packFile = NewPackFile();
			var dispatcher = Substitute.For<IPackFileDispatcher>();
			packFile.Dispatcher.Returns(dispatcher);
			packFile.PopObjectAsXml(Arg.Any<string>()).Returns(false);
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Replace, element, packFile, changeSet);

			dispatcher.DidNotReceive()
				.TrackPotentialNode(
					Arg.Any<IPackFile>(),
					Arg.Any<string>(),
					Arg.Any<XElement>()
				);
			changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
		}

		[Fact]
		public void ParseEdit_WhenPathEmptyAndInsertWithoutElements_NoChange()
		{
			var element = new XElement("Insert", new XAttribute("path", ""));
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdit(ChangeType.Insert, element, packFile, changeSet);

			changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
		}

		[Fact]
		public void ParseTypedEdits_WhenContainerHasMultipleChildren_PassesChangeTypeToEach()
		{
			var container = new XElement(
				"Insert",
				new XElement("child", new XAttribute("path", Path), new XText("text1")),
				new XElement("child", new XAttribute("path", Path), new XText("text2")),
				new XElement("child", new XAttribute("path", Path), new XText("text3"))
			);
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseTypedEdits(ChangeType.Insert, container, packFile, changeSet);

			changeSet.Received(3).AddChange(Arg.Any<InsertTextChange>());
		}

		[Fact]
		public void ParseEdits_WhenContainerEmpty_NoChange()
		{
			var container = new XElement("Container");
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdits(container, packFile, changeSet);

			changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
		}

		[Fact]
		public void ParseEdits_WhenElementIsChangeTypeWithAttributes_ParsesAsDirectEdit()
		{
			var container = new XElement(
				"root",
				new XElement("Insert", new XAttribute("path", Path), new XText("new text"))
			);
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdits(container, packFile, changeSet);

			changeSet.Received(1).AddChange(Arg.Any<InsertTextChange>());
		}

		[Fact]
		public void ParseEdits_WhenElementIsChangeTypeWithoutAttributes_ParsesAsTypedContainer()
		{
			var container = new XElement(
				"root",
				new XElement(
					"Insert",
					new XElement("child", new XAttribute("path", Path), new XText("text1")),
					new XElement("child", new XAttribute("path", Path), new XText("text2"))
				)
			);
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdits(container, packFile, changeSet);

			changeSet.Received(2).AddChange(Arg.Any<InsertTextChange>());
		}

		[Fact]
		public void ParseEdits_WhenElementIsNotChangeTypeNamed_RecursesIntoElement()
		{
			var container = new XElement(
				"root",
				new XElement(
					"wrapper",
					new XElement("Append", new XAttribute("path", Path), new XText("text"))
				)
			);
			var packFile = NewPackFile();
			var changeSet = NewChangeSet();

			PandoraParser.ParseEdits(container, packFile, changeSet);

			changeSet.Received(1).AddChange(Arg.Any<AppendTextChange>());
		}
	}
}
