using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbTransformVectorModifierInternalState Signatire: 0x5ca91c99 size: 32 flags: FLAGS_NONE

    // vectorOut class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkbTransformVectorModifierInternalState : hkReferencedObject, IEquatable<hkbTransformVectorModifierInternalState?>
    {
        public Vector4 vectorOut { set; get; }

        public override uint Signature { set; get; } = 0x5ca91c99;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            vectorOut = br.ReadVector4();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(vectorOut);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            vectorOut = xd.ReadVector4(xe, nameof(vectorOut));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(vectorOut), vectorOut);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbTransformVectorModifierInternalState);
        }

        public bool Equals(hkbTransformVectorModifierInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   vectorOut.Equals(other.vectorOut) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(vectorOut);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

