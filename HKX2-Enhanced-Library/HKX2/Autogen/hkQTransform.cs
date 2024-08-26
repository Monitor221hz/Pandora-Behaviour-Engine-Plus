using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkQTransform Signatire: 0x471a21ee size: 32 flags: FLAGS_NONE

    // rotation class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // translation class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkQTransform : IHavokObject, IEquatable<hkQTransform?>
    {
        public Quaternion rotation { set; get; }
        public Vector4 translation { set; get; }

        public virtual uint Signature { set; get; } = 0x471a21ee;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            rotation = des.ReadQuaternion(br);
            translation = br.ReadVector4();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteQuaternion(bw, rotation);
            bw.WriteVector4(translation);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            rotation = xd.ReadQuaternion(xe, nameof(rotation));
            translation = xd.ReadVector4(xe, nameof(translation));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteQuaternion(xe, nameof(rotation), rotation);
            xs.WriteVector4(xe, nameof(translation), translation);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkQTransform);
        }

        public bool Equals(hkQTransform? other)
        {
            return other is not null &&
                   rotation.Equals(other.rotation) &&
                   translation.Equals(other.translation) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(rotation);
            hashcode.Add(translation);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

