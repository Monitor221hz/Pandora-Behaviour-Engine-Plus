// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Xml.Linq;
using NSubstitute;
using Pandora.API.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Hkx.Changes;
using XmlCake.Linq;

namespace PandoraTests.Unit;

public class AppendChangeTests
{
    [Fact]
    public void ApplyAppendChange_Element_Succeeds()
    {
        XElement originalElement = Resources.TestElement;
        XMapElement mapElement = new(originalElement);
        mapElement.MapAll();
        var packFile = Substitute.For<IPackFile>();
        packFile
            .TryGetXMap(Arg.Any<string>(), out Arg.Any<XMapElement?>())
            .Returns(s =>
            {
                s[1] = mapElement;
                return true;
            });

        var newElement = new XElement(
            "hkobject",
            new XElement("hkparam", new XAttribute("name", "value"), new XText("3"))
        );
        var change = new AppendElementChange("#0069", "children", newElement);
        var elements = mapElement.Descendants();
        Assert.True(change.Apply(packFile));
        Assert.Contains<XElement>(newElement, elements);
    }

    [Fact]
    public void ApplyAppendChange_ElementPathMissing_FailsUnchanged()
    {
        XElement originalElement = Resources.TestElement;
        XMapElement mapElement = new(originalElement);
        mapElement.MapAll();
        var packFile = Substitute.For<IPackFile>();
        packFile
            .TryGetXMap(Arg.Any<string>(), out Arg.Any<XMapElement?>())
            .Returns(s =>
            {
                s[1] = mapElement;
                return true;
            });
        var newElement = new XElement(
            "hkobject",
            new XElement("hkparam", new XAttribute("name", "value"), new XText("3"))
        );
        var change = new AppendElementChange("#0069", "nonexistant", newElement);
        var elements = mapElement.Descendants();
        Assert.False(change.Apply(packFile));
        Assert.DoesNotContain<XElement>(newElement, elements);
    }

    [Fact]
    public void ApplyAppendChange_Text_Succeeds()
    {
        XMapElement mapElement = new(Resources.TestElement);
        mapElement.MapAll();
        var packFile = Substitute.For<IPackFile>();
        packFile
            .TryGetXMap(Arg.Any<string>(), out Arg.Any<XMapElement?>())
            .Returns(s =>
            {
                s[1] = mapElement;
                return true;
            });
        var change = new AppendTextChange("#0069", "generators", "#5000");
        Assert.True(change.Apply(packFile));
        var elements = mapElement.Elements();
        var generatorsValue = elements.First(e => e.Attribute("name")?.Value == "generators").Value;
        Assert.Contains("#5000", generatorsValue);
        Assert.Contains("#0005 #0005 #0005", generatorsValue);
    }

    [Fact]
    public void ApplyAppendChange_TextPathMissing_FailsUnchanged()
    {
        XMapElement mapElement = new(Resources.TestElement);
        mapElement.MapAll();
        var packFile = Substitute.For<IPackFile>();
        packFile
            .TryGetXMap(Arg.Any<string>(), out Arg.Any<XMapElement?>())
            .Returns(s =>
            {
                s[1] = mapElement;
                return true;
            });
        var change = new AppendTextChange("#0069", "something", "#5000");
        Assert.False(change.Apply(packFile));
        var elements = mapElement.Elements();
        var generatorsValue = elements.First(e => e.Attribute("name")?.Value == "generators").Value;
        Assert.DoesNotContain("#5000", generatorsValue);
        Assert.Contains("#0005 #0005 #0005", generatorsValue);
    }

    [Fact]
    public void ApplyAppendChange_TextValueMissing_Fails()
    {
        XMapElement mapElement = new(Resources.TestElement);
        mapElement.MapAll();
        var packFile = Substitute.For<IPackFile>();
        packFile
            .TryGetXMap(Arg.Any<string>(), out Arg.Any<XMapElement?>())
            .Returns(s =>
            {
                s[1] = mapElement;
                return true;
            });
        var change = new AppendTextChange("#0069", "generators", string.Empty);
        Assert.False(change.Apply(packFile));
    }
}
