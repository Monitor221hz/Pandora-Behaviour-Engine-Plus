using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxAnimatedQuaternion Signatire: 0xb4f01baa size: 32 flags: FLAGS_NONE

    // quaternions class:  Type.TYPE_ARRAY Type.TYPE_QUATERNION arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkxAnimatedQuaternion : hkReferencedObject, IEquatable<hkxAnimatedQuaternion?>
    {
        public IList<Quaternion> quaternions { set; get; } = Array.Empty<Quaternion>();

        public override uint Signature { set; get; } = 0xb4f01baa;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            quaternions = des.ReadQuaternionArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteQuaternionArray(bw, quaternions);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            quaternions = xd.ReadQuaternionArray(xe, nameof(quaternions));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteQuaternionArray(xe, nameof(quaternions), quaternions);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxAnimatedQuaternion);
        }

        public bool Equals(hkxAnimatedQuaternion? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   quaternions.SequenceEqual(other.quaternions) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(quaternions.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

