using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpConvexVerticesShapeFourVectors Signatire: 0x3d80c5bf size: 48 flags: FLAGS_NONE

    // x class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // y class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // z class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkpConvexVerticesShapeFourVectors : IHavokObject, IEquatable<hkpConvexVerticesShapeFourVectors?>
    {
        public Vector4 x { set; get; }
        public Vector4 y { set; get; }
        public Vector4 z { set; get; }

        public virtual uint Signature { set; get; } = 0x3d80c5bf;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            x = br.ReadVector4();
            y = br.ReadVector4();
            z = br.ReadVector4();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteVector4(x);
            bw.WriteVector4(y);
            bw.WriteVector4(z);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            x = xd.ReadVector4(xe, nameof(x));
            y = xd.ReadVector4(xe, nameof(y));
            z = xd.ReadVector4(xe, nameof(z));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteVector4(xe, nameof(x), x);
            xs.WriteVector4(xe, nameof(y), y);
            xs.WriteVector4(xe, nameof(z), z);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpConvexVerticesShapeFourVectors);
        }

        public bool Equals(hkpConvexVerticesShapeFourVectors? other)
        {
            return other is not null &&
                   x.Equals(other.x) &&
                   y.Equals(other.y) &&
                   z.Equals(other.z) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(x);
            hashcode.Add(y);
            hashcode.Add(z);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

