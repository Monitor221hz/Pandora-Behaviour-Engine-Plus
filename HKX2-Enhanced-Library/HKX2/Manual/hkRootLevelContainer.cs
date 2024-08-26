using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkRootLevelContainer Signatire: 0x2772c11e size: 16 flags: FLAGS_NONE

    // namedVariants class: hkRootLevelContainerNamedVariant Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 

    public partial class hkRootLevelContainer : IHavokObject, IEquatable<hkRootLevelContainer?>
    {

        public IList<hkRootLevelContainerNamedVariant?> namedVariants { get; set; } = Array.Empty<hkRootLevelContainerNamedVariant?>();

        public uint Signature { set; get; } = 0x2772c11e;

        public void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            namedVariants = des.ReadClassArray<hkRootLevelContainerNamedVariant>(br)!;
        }

        public void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteClassArray(bw, namedVariants);
        }

        public void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            namedVariants = xd.ReadClassArray<hkRootLevelContainerNamedVariant>(xe, nameof(namedVariants));
        }

        public void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClassArray(xe, nameof(namedVariants), namedVariants);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkRootLevelContainer);
        }

        public bool Equals(hkRootLevelContainer? other)
        {
            return other is not null &&
                   namedVariants.SequenceEqual(other.namedVariants) &&
                   Signature == other.Signature;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(namedVariants.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

