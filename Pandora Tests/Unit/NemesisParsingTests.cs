using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using NSubstitute;
using Pandora.API.Patch;
using Pandora.Models.Patch.Mod;
using Pandora.Models.Patch.Skyrim64.Format.Nemesis;
using Pandora.Models.Patch.Skyrim64.Hkx.Changes;
using XmlCake.Linq;
using XmlCake.Linq.Expressions;

namespace PandoraTests.Unit
{
    public class NemesisParsingTests
    {
        [Fact]
        public void ParseReplaceEdit_WhenEditEmpty_OnlyThrowsException()
        {
            string nodeName = "#0069";
            var match = new XMatch();
            IPackFileChangeOwner changeSet = Substitute.For<IPackFileChangeOwner>();
            var lookup = Substitute.For<IXPathLookup>();

            Assert.ThrowsAny<Exception>(() =>
            {
                NemesisParser.ParseReplaceEdit(nodeName, match, changeSet, lookup);
            });
            changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
        }

        [Fact]
        public void ParseReplaceEdit_WhenEditElements_AddsReplaceChange()
        {
            string nodeName = "#0069";
            var match = new XMatch([
                new XComment("OPEN"),
                new XElement("NewElement"),
                new XComment("ORIGINAL"),
                new XElement("OldElement"),
                new XComment("CLOSE"),
            ]);
            IPackFileChangeOwner changeSet = Substitute.For<IPackFileChangeOwner>();
            var lookup = Substitute.For<IXPathLookup>();
            NemesisParser.ParseReplaceEdit(nodeName, match, changeSet, lookup);
            changeSet.Received().AddChange(Arg.Any<ReplaceElementChange>());
        }

        [Fact]
        public void ParseReplaceEdit_WhenEditNoNewElements_AddsRemoveChange()
        {
            string nodeName = "#0069";
            var match = new XMatch([
                new XComment("OPEN"),
                new XComment("ORIGINAL"),
                new XElement("OldElement"),
                new XComment("CLOSE"),
            ]);
            IPackFileChangeOwner changeSet = Substitute.For<IPackFileChangeOwner>();
            var lookup = Substitute.For<IXPathLookup>();
            NemesisParser.ParseReplaceEdit(nodeName, match, changeSet, lookup);
            changeSet.Received().AddChange(Arg.Any<RemoveElementChange>());
        }

        [Fact]
        public void ParseReplaceEdit_WhenEditElementsMissingComments_NoChange()
        {
            string nodeName = "#0069";
            var match = new XMatch([
                new XComment("OPEN"),
                new XElement("NewElement"),
                new XElement("OldElement"),
                new XComment("CLOSE"),
            ]);
            var match2 = new XMatch([
                new XElement("NewElement"),
                new XComment("ORIGINAL"),
                new XElement("OldElement"),
                new XComment("CLOSE"),
            ]);
            var match3 = new XMatch([
                new XComment("OPEN"),
                new XElement("NewElement"),
                new XComment("ORIGINAL"),
                new XElement("OldElement"),
            ]);
            IPackFileChangeOwner changeSet = Substitute.For<IPackFileChangeOwner>();
            var lookup = Substitute.For<IXPathLookup>();
            NemesisParser.ParseReplaceEdit(nodeName, match, changeSet, lookup);
            changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
            NemesisParser.ParseReplaceEdit(nodeName, match2, changeSet, lookup);
            changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
            NemesisParser.ParseReplaceEdit(nodeName, match3, changeSet, lookup);
            changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
        }

        [Fact]
        public void ParseReplaceEdit_WhenEditText_AddsReplaceChange()
        {
            string nodeName = "#0069";
            var match = new XMatch([
                new XComment("OPEN"),
                new XText("NewText"),
                new XComment("ORIGINAL"),
                new XText("OldText"),
                new XComment("CLOSE"),
            ]);
            IPackFileChangeOwner changeSet = Substitute.For<IPackFileChangeOwner>();
            var lookup = Substitute.For<IXPathLookup>();
            NemesisParser.ParseReplaceEdit(nodeName, match, changeSet, lookup);
            changeSet.Received().AddChange(Arg.Any<ReplaceTextChange>());
        }

        [Fact]
        public void ParseReplaceEdit_WhenEditNoNewText_AddsRemoveChange()
        {
            string nodeName = "#0069";
            var match = new XMatch([
                new XComment("OPEN"),
                new XComment("ORIGINAL"),
                new XText("OldText"),
                new XComment("CLOSE"),
            ]);
            IPackFileChangeOwner changeSet = Substitute.For<IPackFileChangeOwner>();
            var lookup = Substitute.For<IXPathLookup>();
            NemesisParser.ParseReplaceEdit(nodeName, match, changeSet, lookup);
            changeSet.Received().AddChange(Arg.Any<RemoveTextChange>());
        }

        [Fact]
        public void ParseReplaceEdit_WhenEditTextMissingComments_NoChange()
        {
            string nodeName = "#0069";
            var match = new XMatch([
                new XComment("OPEN"),
                new XText("NewText"),
                new XText("OldText"),
                new XComment("CLOSE"),
            ]);
            var match2 = new XMatch([
                new XText("NewText"),
                new XComment("ORIGINAL"),
                new XText("OldText"),
                new XComment("CLOSE"),
            ]);
            var match3 = new XMatch([
                new XComment("OPEN"),
                new XText("NewText"),
                new XComment("ORIGINAL"),
                new XText("OldText"),
            ]);
            IPackFileChangeOwner changeSet = Substitute.For<IPackFileChangeOwner>();
            var lookup = Substitute.For<IXPathLookup>();
            NemesisParser.ParseReplaceEdit(nodeName, match, changeSet, lookup);
            changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
            NemesisParser.ParseReplaceEdit(nodeName, match2, changeSet, lookup);
            changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
            NemesisParser.ParseReplaceEdit(nodeName, match3, changeSet, lookup);
            changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
        }

        [Fact]
        public void ParseInsertEdit_WhenEditEmpty_OnlyThrowsException()
        {
            string nodeName = "#0069";
            var match = new XMatch();
            IPackFileChangeOwner changeSet = Substitute.For<IPackFileChangeOwner>();
            var lookup = Substitute.For<IXPathLookup>();

            Assert.ThrowsAny<Exception>(() =>
            {
                NemesisParser.ParseInsertEdit(nodeName, match, changeSet, lookup);
            });
            changeSet.DidNotReceive().AddChange(Arg.Any<IPackFileChange>());
        }

        [Fact]
        public void ParseInsertEdit_WhenEditElements_AddsA
    }
}
