using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbMoveCharacterModifier Signatire: 0x8f7492a0 size: 112 flags: FLAGS_NONE

    // offsetPerSecondMS class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // timeSinceLastModify class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 96 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbMoveCharacterModifier : hkbModifier, IEquatable<hkbMoveCharacterModifier?>
    {
        public Vector4 offsetPerSecondMS { set; get; }
        private float timeSinceLastModify { set; get; }

        public override uint Signature { set; get; } = 0x8f7492a0;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            offsetPerSecondMS = br.ReadVector4();
            timeSinceLastModify = br.ReadSingle();
            br.Position += 12;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(offsetPerSecondMS);
            bw.WriteSingle(timeSinceLastModify);
            bw.Position += 12;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            offsetPerSecondMS = xd.ReadVector4(xe, nameof(offsetPerSecondMS));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(offsetPerSecondMS), offsetPerSecondMS);
            xs.WriteSerializeIgnored(xe, nameof(timeSinceLastModify));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbMoveCharacterModifier);
        }

        public bool Equals(hkbMoveCharacterModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   offsetPerSecondMS.Equals(other.offsetPerSecondMS) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(offsetPerSecondMS);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

