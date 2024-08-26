using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkSphere Signatire: 0x143dff99 size: 16 flags: FLAGS_NONE

    // pos class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    public partial class hkSphere : IHavokObject, IEquatable<hkSphere?>
    {
        public Vector4 pos { set; get; }

        public virtual uint Signature { set; get; } = 0x143dff99;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            pos = br.ReadVector4();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteVector4(pos);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            pos = xd.ReadVector4(xe, nameof(pos));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteVector4(xe, nameof(pos), pos);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkSphere);
        }

        public bool Equals(hkSphere? other)
        {
            return other is not null &&
                   pos.Equals(other.pos) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(pos);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

