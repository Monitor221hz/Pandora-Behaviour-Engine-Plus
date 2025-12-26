// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Xml.Linq;
using NSubstitute;
using Pandora.API.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Hkx.Changes;
using XmlCake.Linq;

namespace PandoraTests.Unit;

public class RemoveChangeTests
{
    [Fact]
    public void ApplyRemoveChange_Element_Succeeds()
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
        var change = new RemoveElementChange("#0069", "children/Element2");
        var elements = mapElement.Descendants();
        Assert.True(change.Apply(packFile));
        Assert.DoesNotContain<XElement>(elements, e => e.Value == "2");
    }

    [Fact]
    public void ApplyRemoveChange_ElementPathMissing_FailsUnchanged()
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
        var change = new RemoveElementChange("#0069", "nonexistant");
        var elements = mapElement.Descendants();
        Assert.False(change.Apply(packFile));
        Assert.Contains<XElement>(elements, e => e.Value == "2");
    }

    [Fact]
    public void ApplyRemoveChange_Text_Succeeds()
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
        var change = new RemoveTextChange("#0069", "generators", "#0005", 35);
        Assert.True(change.Apply(packFile));
        var elements = mapElement.Elements();
        var generatorsValue = elements.First(e => e.Attribute("name")?.Value == "generators").Value;
        Assert.DoesNotContain("#0005 #0005 #0005", generatorsValue);
        Assert.Contains("#0005 #0005", generatorsValue);
    }

    [Fact]
    public void ApplyRemoveChange_TextPathMissing_FailsUnchanged()
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
        var change = new RemoveTextChange("#0069", "something", "#0005", 35);
        Assert.False(change.Apply(packFile));
        var elements = mapElement.Elements();
        var generatorsValue = elements.First(e => e.Attribute("name")?.Value == "generators").Value;
        Assert.Contains("#0005\n#0005\n#0005", generatorsValue);
    }

    [Fact]
    public void ApplyRemoveChange_TextValueMissing_FailsUnchanged()
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
        var change = new RemoveTextChange("#0069", "generators", "#6969", 35);
        Assert.False(change.Apply(packFile));
        var elements = mapElement.Elements();
        var generatorsValue = elements.First(e => e.Attribute("name")?.Value == "generators").Value;
        Assert.Contains("#0005\n#0005\n#0005", generatorsValue);
    }

    [Fact]
    public void ApplyRemoveChange_TextIndexOutOfRange_FailsUnchanged()
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
        var change = new RemoveTextChange("#0069", "generators", "#0005", 999);
        Assert.False(change.Apply(packFile));
        var elements = mapElement.Elements();
        var generatorsValue = elements.First(e => e.Attribute("name")?.Value == "generators").Value;
        Assert.Contains("#0005\n#0005\n#0005", generatorsValue);
    }
}
