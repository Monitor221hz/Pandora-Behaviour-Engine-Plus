using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpBallSocketChainDataConstraintInfo Signatire: 0xc9cbedf2 size: 32 flags: FLAGS_NONE

    // pivotInA class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // pivotInB class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkpBallSocketChainDataConstraintInfo : IHavokObject, IEquatable<hkpBallSocketChainDataConstraintInfo?>
    {
        public Vector4 pivotInA { set; get; }
        public Vector4 pivotInB { set; get; }

        public virtual uint Signature { set; get; } = 0xc9cbedf2;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            pivotInA = br.ReadVector4();
            pivotInB = br.ReadVector4();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteVector4(pivotInA);
            bw.WriteVector4(pivotInB);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            pivotInA = xd.ReadVector4(xe, nameof(pivotInA));
            pivotInB = xd.ReadVector4(xe, nameof(pivotInB));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteVector4(xe, nameof(pivotInA), pivotInA);
            xs.WriteVector4(xe, nameof(pivotInB), pivotInB);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpBallSocketChainDataConstraintInfo);
        }

        public bool Equals(hkpBallSocketChainDataConstraintInfo? other)
        {
            return other is not null &&
                   pivotInA.Equals(other.pivotInA) &&
                   pivotInB.Equals(other.pivotInB) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(pivotInA);
            hashcode.Add(pivotInB);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

