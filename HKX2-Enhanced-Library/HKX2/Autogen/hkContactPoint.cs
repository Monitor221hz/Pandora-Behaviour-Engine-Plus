using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkContactPoint Signatire: 0x91d7dd8e size: 32 flags: FLAGS_NONE

    // position class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // separatingNormal class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkContactPoint : IHavokObject, IEquatable<hkContactPoint?>
    {
        public Vector4 position { set; get; }
        public Vector4 separatingNormal { set; get; }

        public virtual uint Signature { set; get; } = 0x91d7dd8e;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            position = br.ReadVector4();
            separatingNormal = br.ReadVector4();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteVector4(position);
            bw.WriteVector4(separatingNormal);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            position = xd.ReadVector4(xe, nameof(position));
            separatingNormal = xd.ReadVector4(xe, nameof(separatingNormal));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteVector4(xe, nameof(position), position);
            xs.WriteVector4(xe, nameof(separatingNormal), separatingNormal);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkContactPoint);
        }

        public bool Equals(hkContactPoint? other)
        {
            return other is not null &&
                   position.Equals(other.position) &&
                   separatingNormal.Equals(other.separatingNormal) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(position);
            hashcode.Add(separatingNormal);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

