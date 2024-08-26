using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbBoneIndexArray Signatire: 0xaa8619 size: 64 flags: FLAGS_NONE

    // boneIndices class:  Type.TYPE_ARRAY Type.TYPE_INT16 arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkbBoneIndexArray : hkbBindable, IEquatable<hkbBoneIndexArray?>
    {
        public IList<short> boneIndices { set; get; } = Array.Empty<short>();

        public override uint Signature { set; get; } = 0xaa8619;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            boneIndices = des.ReadInt16Array(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteInt16Array(bw, boneIndices);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            boneIndices = xd.ReadInt16Array(xe, nameof(boneIndices));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumberArray(xe, nameof(boneIndices), boneIndices);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbBoneIndexArray);
        }

        public bool Equals(hkbBoneIndexArray? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   boneIndices.SequenceEqual(other.boneIndices) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(boneIndices.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

