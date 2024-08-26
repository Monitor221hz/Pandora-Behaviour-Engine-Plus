using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkGeometryTriangle Signatire: 0x9687513b size: 16 flags: FLAGS_NONE

    // a class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // b class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // c class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // material class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    public partial class hkGeometryTriangle : IHavokObject, IEquatable<hkGeometryTriangle?>
    {
        public int a { set; get; }
        public int b { set; get; }
        public int c { set; get; }
        public int material { set; get; }

        public virtual uint Signature { set; get; } = 0x9687513b;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            a = br.ReadInt32();
            b = br.ReadInt32();
            c = br.ReadInt32();
            material = br.ReadInt32();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteInt32(a);
            bw.WriteInt32(b);
            bw.WriteInt32(c);
            bw.WriteInt32(material);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            a = xd.ReadInt32(xe, nameof(a));
            b = xd.ReadInt32(xe, nameof(b));
            c = xd.ReadInt32(xe, nameof(c));
            material = xd.ReadInt32(xe, nameof(material));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(a), a);
            xs.WriteNumber(xe, nameof(b), b);
            xs.WriteNumber(xe, nameof(c), c);
            xs.WriteNumber(xe, nameof(material), material);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkGeometryTriangle);
        }

        public bool Equals(hkGeometryTriangle? other)
        {
            return other is not null &&
                   a.Equals(other.a) &&
                   b.Equals(other.b) &&
                   c.Equals(other.c) &&
                   material.Equals(other.material) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(a);
            hashcode.Add(b);
            hashcode.Add(c);
            hashcode.Add(material);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

