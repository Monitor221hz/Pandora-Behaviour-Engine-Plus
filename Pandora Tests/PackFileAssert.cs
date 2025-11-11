// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using HKX2E;
using Pandora.Models.Patch.Skyrim64.Hkx.Packfile;

namespace PandoraTests;

public class PackFileAssert
{
    /// <summary>
    /// Animations between siblings MUST be IN ORDER and equal.
    /// </summary>
    /// <param name="character"></param>
    /// <param name="sibling"></param>
    /// <returns></returns>
    public static void ValidSiblingAnimations(
        PackFileCharacter character,
        PackFileCharacter sibling
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

    public static void ValidPackFile(PackFileGraph graph)
    {
        Assert.NotEmpty(graph.Container.namedVariants);

        var variant = graph.Container.namedVariants.First();
        Assert.NotNull(variant);

        var root = variant.variant as hkbBehaviorGraph;
        HavokAssert.NotNullValid(root);
    }

    public static void ValidPackFile(PackFileCharacter character)
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

    public static void ValidPackFile(PackFileSkeleton skeleton) { }

    public static void DowncastValidPackFile(PackFile packFile)
    {
        switch (packFile)
        {
            case PackFileSkeleton skeleton:
                ValidPackFile(skeleton);
                break;
            case PackFileGraph graph:
                ValidPackFile(graph);
                break;
            case PackFileCharacter character:
                ValidPackFile(character);
                break;
            default:
                Assert.Fail($"Could not downcast packfile {packFile.Name} to a type");
                break;
        }
    }
}
