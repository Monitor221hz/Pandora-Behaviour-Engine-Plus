# Pandora Behaviour Engine+ — Linux ARM64 fixes

Native Linux ARM64 fixes tested on Ayn Odin 3 running Armada / Fedora 44.

## Fixed issues

Pandora's Skyrim templates contain Windows-style paths and casing assumptions
which fail on Linux's case-sensitive filesystem.

The Linux fixes include:

- normalize Windows `\` path separators to native Linux `/`
- normalize project paths loaded from `vanilla_projectpaths.txt`
- normalize HKX-relative paths before filesystem access
- resolve character HKX paths case-insensitively
- resolve skeleton HKX paths case-insensitively
- resolve behavior HKX paths case-insensitively
- Linux-specific application/service fixes required for native execution

Examples fixed:

    actors\atronachflame\atronachflame.hkx
    Characters\AtronachFlame.hkx
    Character Assets\skeleton.hkx

on Linux resolve to the actual files such as:

    actors/atronachflame/atronachflame.hkx
    characters/atronachflame.hkx
    character assets/skeleton.hkx

## Build

Tested with .NET 10 on ARM64:

    dotnet publish \
      "Pandora Behaviour Engine/Pandora Behaviour Engine.csproj" \
      -c Release \
      -r linux-arm64 \
      --self-contained true \
      -p:PublishSingleFile=false \
      -p:DebugType=none \
      -p:DebugSymbols=false \
      -o ./publish-linux-arm64

Multi-file publishing is intentional so the managed DLL can be updated and
verified independently.

## Verified result

Pandora successfully:

- loads all 49 tracked vanilla projects
- reads and maps 429 animation-data projects
- generates merged animation data
- processes FNISAA variables
- generates Skyrim behavior HKX output without path-related exceptions

Observed generated behavior files include:

    meshes/actors/character/behaviors/0_master.hkx
    meshes/actors/character/behaviors/magicbehavior.hkx
    meshes/actors/character/behaviors/magicmountedbehavior.hkx

