using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbMirroredSkeletonInfo Signatire: 0xc6c2da4f size: 48 flags: FLAGS_NONE

    // mirrorAxis class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // bonePairMap class:  Type.TYPE_ARRAY Type.TYPE_INT16 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkbMirroredSkeletonInfo : hkReferencedObject, IEquatable<hkbMirroredSkeletonInfo?>
    {
        public Vector4 mirrorAxis { set; get; }
        public IList<short> bonePairMap { set; get; } = Array.Empty<short>();

        public override uint Signature { set; get; } = 0xc6c2da4f;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            mirrorAxis = br.ReadVector4();
            bonePairMap = des.ReadInt16Array(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(mirrorAxis);
            s.WriteInt16Array(bw, bonePairMap);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            mirrorAxis = xd.ReadVector4(xe, nameof(mirrorAxis));
            bonePairMap = xd.ReadInt16Array(xe, nameof(bonePairMap));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(mirrorAxis), mirrorAxis);
            xs.WriteNumberArray(xe, nameof(bonePairMap), bonePairMap);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbMirroredSkeletonInfo);
        }

        public bool Equals(hkbMirroredSkeletonInfo? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   mirrorAxis.Equals(other.mirrorAxis) &&
                   bonePairMap.SequenceEqual(other.bonePairMap) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(mirrorAxis);
            hashcode.Add(bonePairMap.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

