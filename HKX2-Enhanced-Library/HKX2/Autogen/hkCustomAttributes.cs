using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkCustomAttributes Signatire: 0xbff19005 size: 16 flags: FLAGS_NONE

    // attributes class: hkCustomAttributesAttribute Type.TYPE_SIMPLEARRAY Type.TYPE_STRUCT arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    public partial class hkCustomAttributes : IHavokObject, IEquatable<hkCustomAttributes?>
    {
        public object? attributes { set; get; }

        public virtual uint Signature { set; get; } = 0xbff19005;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            throw new NotImplementedException("TPYE_SIMPLEARRAY");
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            throw new NotImplementedException("TPYE_SIMPLEARRAY");
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            throw new NotImplementedException("TPYE_SIMPLEARRAY");
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            throw new NotImplementedException("TPYE_SIMPLEARRAY");
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkCustomAttributes);
        }

        public bool Equals(hkCustomAttributes? other)
        {
            return other is not null &&
                   attributes!.Equals(other.attributes) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(attributes);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

