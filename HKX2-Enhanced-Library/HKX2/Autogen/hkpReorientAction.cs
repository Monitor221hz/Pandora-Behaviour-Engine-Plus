using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpReorientAction Signatire: 0x2dc0ec6a size: 112 flags: FLAGS_NONE

    // rotationAxis class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // upAxis class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // strength class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // damping class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 100 flags: FLAGS_NONE enum: 
    public partial class hkpReorientAction : hkpUnaryAction, IEquatable<hkpReorientAction?>
    {
        public Vector4 rotationAxis { set; get; }
        public Vector4 upAxis { set; get; }
        public float strength { set; get; }
        public float damping { set; get; }

        public override uint Signature { set; get; } = 0x2dc0ec6a;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 8;
            rotationAxis = br.ReadVector4();
            upAxis = br.ReadVector4();
            strength = br.ReadSingle();
            damping = br.ReadSingle();
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 8;
            bw.WriteVector4(rotationAxis);
            bw.WriteVector4(upAxis);
            bw.WriteSingle(strength);
            bw.WriteSingle(damping);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            rotationAxis = xd.ReadVector4(xe, nameof(rotationAxis));
            upAxis = xd.ReadVector4(xe, nameof(upAxis));
            strength = xd.ReadSingle(xe, nameof(strength));
            damping = xd.ReadSingle(xe, nameof(damping));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(rotationAxis), rotationAxis);
            xs.WriteVector4(xe, nameof(upAxis), upAxis);
            xs.WriteFloat(xe, nameof(strength), strength);
            xs.WriteFloat(xe, nameof(damping), damping);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpReorientAction);
        }

        public bool Equals(hkpReorientAction? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   rotationAxis.Equals(other.rotationAxis) &&
                   upAxis.Equals(other.upAxis) &&
                   strength.Equals(other.strength) &&
                   damping.Equals(other.damping) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(rotationAxis);
            hashcode.Add(upAxis);
            hashcode.Add(strength);
            hashcode.Add(damping);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

