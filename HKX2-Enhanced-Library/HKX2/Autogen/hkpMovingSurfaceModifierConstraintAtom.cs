using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpMovingSurfaceModifierConstraintAtom Signatire: 0x79ab517d size: 64 flags: FLAGS_NONE

    // velocity class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkpMovingSurfaceModifierConstraintAtom : hkpModifierConstraintAtom, IEquatable<hkpMovingSurfaceModifierConstraintAtom?>
    {
        public Vector4 velocity { set; get; }

        public override uint Signature { set; get; } = 0x79ab517d;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            velocity = br.ReadVector4();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(velocity);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            velocity = xd.ReadVector4(xe, nameof(velocity));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(velocity), velocity);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpMovingSurfaceModifierConstraintAtom);
        }

        public bool Equals(hkpMovingSurfaceModifierConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   velocity.Equals(other.velocity) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(velocity);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

