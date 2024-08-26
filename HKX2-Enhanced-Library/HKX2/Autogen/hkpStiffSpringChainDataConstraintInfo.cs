using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpStiffSpringChainDataConstraintInfo Signatire: 0xc624a180 size: 48 flags: FLAGS_NONE

    // pivotInA class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // pivotInB class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // springLength class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkpStiffSpringChainDataConstraintInfo : IHavokObject, IEquatable<hkpStiffSpringChainDataConstraintInfo?>
    {
        public Vector4 pivotInA { set; get; }
        public Vector4 pivotInB { set; get; }
        public float springLength { set; get; }

        public virtual uint Signature { set; get; } = 0xc624a180;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            pivotInA = br.ReadVector4();
            pivotInB = br.ReadVector4();
            springLength = br.ReadSingle();
            br.Position += 12;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteVector4(pivotInA);
            bw.WriteVector4(pivotInB);
            bw.WriteSingle(springLength);
            bw.Position += 12;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            pivotInA = xd.ReadVector4(xe, nameof(pivotInA));
            pivotInB = xd.ReadVector4(xe, nameof(pivotInB));
            springLength = xd.ReadSingle(xe, nameof(springLength));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteVector4(xe, nameof(pivotInA), pivotInA);
            xs.WriteVector4(xe, nameof(pivotInB), pivotInB);
            xs.WriteFloat(xe, nameof(springLength), springLength);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpStiffSpringChainDataConstraintInfo);
        }

        public bool Equals(hkpStiffSpringChainDataConstraintInfo? other)
        {
            return other is not null &&
                   pivotInA.Equals(other.pivotInA) &&
                   pivotInB.Equals(other.pivotInB) &&
                   springLength.Equals(other.springLength) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(pivotInA);
            hashcode.Add(pivotInB);
            hashcode.Add(springLength);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

