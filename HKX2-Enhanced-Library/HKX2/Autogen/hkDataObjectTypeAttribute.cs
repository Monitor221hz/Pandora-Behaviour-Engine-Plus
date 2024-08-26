using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkDataObjectTypeAttribute Signatire: 0x1e3857bb size: 8 flags: FLAGS_NONE

    // typeName class:  Type.TYPE_CSTRING Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    public partial class hkDataObjectTypeAttribute : IHavokObject, IEquatable<hkDataObjectTypeAttribute?>
    {
        public string typeName { set; get; } = "";

        public virtual uint Signature { set; get; } = 0x1e3857bb;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            typeName = des.ReadCString(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteCString(bw, typeName);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            typeName = xd.ReadString(xe, nameof(typeName));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteString(xe, nameof(typeName), typeName);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkDataObjectTypeAttribute);
        }

        public bool Equals(hkDataObjectTypeAttribute? other)
        {
            return other is not null &&
                   (typeName is null && other.typeName is null || typeName == other.typeName || typeName is null && other.typeName == "" || typeName == "" && other.typeName is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(typeName);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

