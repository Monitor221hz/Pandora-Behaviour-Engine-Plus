using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpMassChangerModifierConstraintAtom Signatire: 0xb6b28240 size: 80 flags: FLAGS_NONE

    // factorA class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // factorB class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    public partial class hkpMassChangerModifierConstraintAtom : hkpModifierConstraintAtom, IEquatable<hkpMassChangerModifierConstraintAtom?>
    {
        public Vector4 factorA { set; get; }
        public Vector4 factorB { set; get; }

        public override uint Signature { set; get; } = 0xb6b28240;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            factorA = br.ReadVector4();
            factorB = br.ReadVector4();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(factorA);
            bw.WriteVector4(factorB);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            factorA = xd.ReadVector4(xe, nameof(factorA));
            factorB = xd.ReadVector4(xe, nameof(factorB));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(factorA), factorA);
            xs.WriteVector4(xe, nameof(factorB), factorB);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpMassChangerModifierConstraintAtom);
        }

        public bool Equals(hkpMassChangerModifierConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   factorA.Equals(other.factorA) &&
                   factorB.Equals(other.factorB) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(factorA);
            hashcode.Add(factorB);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

