using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpMountedBallGun Signatire: 0x6791ffce size: 128 flags: FLAGS_NONE

    // position class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    public partial class hkpMountedBallGun : hkpBallGun, IEquatable<hkpMountedBallGun?>
    {
        public Vector4 position { set; get; }

        public override uint Signature { set; get; } = 0x6791ffce;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            position = br.ReadVector4();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(position);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            position = xd.ReadVector4(xe, nameof(position));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(position), position);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpMountedBallGun);
        }

        public bool Equals(hkpMountedBallGun? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   position.Equals(other.position) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(position);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

