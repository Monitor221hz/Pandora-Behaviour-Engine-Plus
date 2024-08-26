using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpCenterOfMassChangerModifierConstraintAtom Signatire: 0x1d7dbdd2 size: 80 flags: FLAGS_NONE

    // displacementA class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // displacementB class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    public partial class hkpCenterOfMassChangerModifierConstraintAtom : hkpModifierConstraintAtom, IEquatable<hkpCenterOfMassChangerModifierConstraintAtom?>
    {
        public Vector4 displacementA { set; get; }
        public Vector4 displacementB { set; get; }

        public override uint Signature { set; get; } = 0x1d7dbdd2;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            displacementA = br.ReadVector4();
            displacementB = br.ReadVector4();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(displacementA);
            bw.WriteVector4(displacementB);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            displacementA = xd.ReadVector4(xe, nameof(displacementA));
            displacementB = xd.ReadVector4(xe, nameof(displacementB));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(displacementA), displacementA);
            xs.WriteVector4(xe, nameof(displacementB), displacementB);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpCenterOfMassChangerModifierConstraintAtom);
        }

        public bool Equals(hkpCenterOfMassChangerModifierConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   displacementA.Equals(other.displacementA) &&
                   displacementB.Equals(other.displacementB) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(displacementA);
            hashcode.Add(displacementB);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

