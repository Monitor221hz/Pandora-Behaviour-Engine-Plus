using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpCapsuleShape Signatire: 0xdd0b1fd3 size: 80 flags: FLAGS_NONE

    // vertexA class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // vertexB class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    public partial class hkpCapsuleShape : hkpConvexShape, IEquatable<hkpCapsuleShape?>
    {
        public Vector4 vertexA { set; get; }
        public Vector4 vertexB { set; get; }

        public override uint Signature { set; get; } = 0xdd0b1fd3;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 8;
            vertexA = br.ReadVector4();
            vertexB = br.ReadVector4();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 8;
            bw.WriteVector4(vertexA);
            bw.WriteVector4(vertexB);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            vertexA = xd.ReadVector4(xe, nameof(vertexA));
            vertexB = xd.ReadVector4(xe, nameof(vertexB));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(vertexA), vertexA);
            xs.WriteVector4(xe, nameof(vertexB), vertexB);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpCapsuleShape);
        }

        public bool Equals(hkpCapsuleShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   vertexA.Equals(other.vertexA) &&
                   vertexB.Equals(other.vertexB) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(vertexA);
            hashcode.Add(vertexB);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

