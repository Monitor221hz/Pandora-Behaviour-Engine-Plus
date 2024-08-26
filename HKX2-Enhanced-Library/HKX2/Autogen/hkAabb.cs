using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkAabb Signatire: 0x4a948b16 size: 32 flags: FLAGS_NONE

    // min class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // max class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkAabb : IHavokObject, IEquatable<hkAabb?>
    {
        public Vector4 min { set; get; }
        public Vector4 max { set; get; }

        public virtual uint Signature { set; get; } = 0x4a948b16;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            min = br.ReadVector4();
            max = br.ReadVector4();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteVector4(min);
            bw.WriteVector4(max);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            min = xd.ReadVector4(xe, nameof(min));
            max = xd.ReadVector4(xe, nameof(max));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteVector4(xe, nameof(min), min);
            xs.WriteVector4(xe, nameof(max), max);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkAabb);
        }

        public bool Equals(hkAabb? other)
        {
            return other is not null &&
                   min.Equals(other.min) &&
                   max.Equals(other.max) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(min);
            hashcode.Add(max);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

