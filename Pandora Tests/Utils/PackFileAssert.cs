// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2026 Pandora Behaviour Engine Contributors

using HKX2E;
using Pandora.API.Patch.Skyrim64;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

namespace PandoraTests.Utils;

public class PackFileAssert
{
    /// <summary>
    /// Animations between siblings MUST be IN ORDER and equal.
    /// </summary>
    /// <param name="character"></param>
    /// <param name="sibling"></param>
    /// <returns></returns>
    public static void ValidSiblingAnimations(
        IPackFileCharacter character,
        IPackFileCharacter sibling
    )
    {
        for (int i = 0; i < character.AnimationNames.Count; i++)
        {
            Assert.Equal(
                Path.GetFileName(character.AnimationNames[i]),
                Path.GetFileName(sibling.AnimationNames[i]),
                StringComparer.OrdinalIgnoreCase
            );
        }
    }

    public static void ValidPackFile(IPackFileGraph graph)
    {
        Assert.NotEmpty(graph.Container.namedVariants);

        var variant = graph.Container.namedVariants.First();
        Assert.NotNull(variant);

        var root = variant.variant as hkbBehaviorGraph;
        HavokAssert.NotNullValid(root);
    }

    public static void ValidPackFile(IPackFileCharacter character)
    {
        Assert.NotNull(character.ParentProject);

        if (character.ParentProject.Sibling != null)
        {
            PackFileAssert.ValidSiblingAnimations(
                character.ParentProject.CharacterPackFile,
                character.ParentProject.Sibling.CharacterPackFile
            );
        }
    }

    public static void ValidPackFile(IPackFileSkeleton skeleton) { }

    public static void DowncastValidPackFile(IPackFile packFile)
    {
        switch (packFile)
        {
            case IPackFileSkeleton skeleton:
                ValidPackFile(skeleton);
                break;
            case IPackFileGraph graph:
                ValidPackFile(graph);
                break;
            case IPackFileCharacter character:
                ValidPackFile(character);
                break;
            default:
                Assert.Fail($"Could not downcast packfile {packFile.Name} to a type");
                break;
        }
    }
}
