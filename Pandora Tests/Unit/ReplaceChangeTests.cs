// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using System.Xml.Linq;
using NSubstitute;
using Pandora.API.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Hkx.Changes;
using XmlCake.Linq;

namespace PandoraTests.Unit;

public class ReplaceChangeTests
{
    [Fact]
    public void ApplyReplaceChange_Element_Succeeds()
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

        var newElement = new XElement("replaced");
        var change = new ReplaceElementChange("#0069", "generators", newElement);
        var elements = mapElement.Elements();
        Assert.True(change.Apply(packFile));
        Assert.Contains<XElement>(newElement, elements);
    }

    [Fact]
    public void ApplyReplaceChange_ElementPathMissing_FailsUnchanged()
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

        var newElement = new XElement("replaced");
        var change = new ReplaceElementChange("#0069", "nonexistant", newElement);
        var elements = mapElement.Elements();
        Assert.False(change.Apply(packFile));
        Assert.DoesNotContain<XElement>(newElement, elements);
    }

    [Fact]
    public void ApplyReplaceChange_ElementTargetMissing_FailsUnchanged()
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

        var newElement = new XElement("replaced");
        var change = new ReplaceElementChange("#9999", "generators", newElement);
        var elements = mapElement.Elements();
        Assert.False(change.Apply(packFile));
        Assert.DoesNotContain<XElement>(newElement, elements);
    }

    //[Fact]
    //public void ApplyReplaceChange_Text_Succeeds()
    //{
    //    XMapElement mapElement = new(Resources.TestElement);
    //    mapElement.MapAll();
    //    var packFile = Substitute.For<IPackFile>();
    //    packFile
    //        .TryGetXMap(Arg.Any<string>(), out Arg.Any<XMapElement?>())
    //        .Returns(s =>
    //        {
    //            s[1] = mapElement;
    //            return true;
    //        });

    //    var newElement = new XElement("replaced");
    //    var change = new ReplaceElementChange("#0069", "generators", newElement);
    //    var elements = mapElement.Elements();
    //    Assert.True(change.Apply(packFile));
    //    Assert.DoesNotContain<XElement>(newElement, elements);
    //}
}
