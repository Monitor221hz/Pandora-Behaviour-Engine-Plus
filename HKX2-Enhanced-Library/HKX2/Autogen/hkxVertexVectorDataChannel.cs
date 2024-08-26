using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxVertexVectorDataChannel Signatire: 0x2ea63179 size: 32 flags: FLAGS_NONE

    // perVertexVectors class:  Type.TYPE_ARRAY Type.TYPE_VECTOR4 arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkxVertexVectorDataChannel : hkReferencedObject, IEquatable<hkxVertexVectorDataChannel?>
    {
        public IList<Vector4> perVertexVectors { set; get; } = Array.Empty<Vector4>();

        public override uint Signature { set; get; } = 0x2ea63179;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            perVertexVectors = des.ReadVector4Array(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteVector4Array(bw, perVertexVectors);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            perVertexVectors = xd.ReadVector4Array(xe, nameof(perVertexVectors));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4Array(xe, nameof(perVertexVectors), perVertexVectors);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxVertexVectorDataChannel);
        }

        public bool Equals(hkxVertexVectorDataChannel? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   perVertexVectors.SequenceEqual(other.perVertexVectors) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(perVertexVectors.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

