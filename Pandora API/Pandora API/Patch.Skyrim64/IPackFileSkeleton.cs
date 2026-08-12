// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2023-2025 Pandora Behaviour Engine Contributors

using HKX2E;

namespace Pandora.API.Patch.Skyrim64;

public interface IPackFileSkeleton : IPackFile, IEquatable<IPackFileSkeleton>
{
    bool HasAnimationContainer { get; }
    bool HasPhysicsData { get; }
    bool HasRagdollInstance { get; }
    bool HasResourceContainer { get; }
    bool HasSkeletonMappers { get; }
    hkaAnimationContainer? MergedAnimationContainer { get; }
    hkpPhysicsData? PhysicsData { get; }
    hkaRagdollInstance? RagdollInstance { get; }
    hkMemoryResourceContainer? ResourceContainer { get; }
    List<hkaSkeletonMapper> SkeletonMappers { get; }
}
