# HKX2Library

A standalone customized version of ret2end's [HKX2 library](https://github.com/ret2end/HKX2Library) for Skyrim SE Havok packfile deserialization and serialization.

This fork modified the library to serve the purposes of packfile editing in addition to the usual (de)serialization functions. It also refactors some of the code to be more error tolerant and optimized.

### Differences

- **Serialize** and **Deserialize** .hkx file. (include **behaviors**, **skeleton** and **aniamtions**)
- Export to **XML**.
- ~XML to HKX~ use [figment/hkxcmd](https://github.com/figment/hkxcmd), [nexus](https://www.nexusmods.com/skyrim/mods/1797)
- Partial serialization and deserialization for packfile/xml fragments.

### Known issues

- ~Ragdoll files (.hkrg) differ from vanilla files because of different fixup ordering. This issue shouldn't affect functionality.~
- can't deserialize some old FNIS generated hkx files due to malformed(?) `__classname__` or virtualFixup section or wrong assigned member (`hkbBlendingTransitionEffec` assign to `hkbStateMachineTransitionInfoArray`)

### Usage

`git submodule add https://github.com/Monitor221hz/HKX2-Enhanced-Library <your-repo-dir>/some/path/HKX2EnhancedLibrary`

Then reference it in your solution and project.

```C#
using HKX2E;

namespace PlatformConverter
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            string inFile = args[0];
            string outFile = args[1];
            string outPlatform = args[2];

            HKXHeader header = HKXHeader.SkyrimSE();

            using (FileStream rs = File.OpenRead(inFile))
            {
                var br = new BinaryReaderEx(rs);
                var des = new PackFileDeserializer();

                var root = (hkRootLevelContainer) des.Deserialize(br);

                using (FileStream ws = File.Create(outFile))
                {   
                    // to hkx
                    var bw = new BinaryWriterEx(ws);
                    var s = new PackFileSerializer();

                    s.Serialize(root, bw, header);
                    
                    // or to xml
                    var xs = new XmlSerializer();
                    xs.Serialize(root, header, ws);
                }
            }
        }
    }
}
```

### Technical details

- `./HKX2/Autogen/` contains Havok classes generated from dumped skyrim classes by SKSE plugin.
- `./HKX2/Manual/` also contains generated classes with small adjustment.

### Credits

- [katalash](https://github.com/katalash) - The original HKX2 library included in [DSMapStudio](https://github.com/katalash/DSMapStudio).
- [JKAnderson](https://github.com/JKAnderson) - BinaryReaderEx and BinaryWriterEx included in [SoulsFormats](https://github.com/JKAnderson/SoulsFormats).
- [krenyy](https://gitlab.com/HKX2/HKX2Library) - HKX2 library by krenyy.
- [Dexesttp](https://github.com/Dexesttp/hkxpack/tree/main/doc/hkx%20findings) with hkx research documentary.
- [ret2end](https://github.com/ret2end) for original HKX2 library supporting Skyrim SE.
- SkyrimSE RE and SkyrimGuild community with valuable skse plugin, hkx, animation, behavior information.
