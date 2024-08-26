using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkReflectedFileAttribute Signatire: 0xedb6b8f7 size: 8 flags: FLAGS_NONE

    // value class:  Type.TYPE_CSTRING Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    public partial class hkReflectedFileAttribute : IHavokObject, IEquatable<hkReflectedFileAttribute?>
    {
        public string value { set; get; } = "";

        public virtual uint Signature { set; get; } = 0xedb6b8f7;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            value = des.ReadCString(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteCString(bw, value);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            value = xd.ReadString(xe, nameof(value));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteString(xe, nameof(value), value);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkReflectedFileAttribute);
        }

        public bool Equals(hkReflectedFileAttribute? other)
        {
            return other is not null &&
                   (value is null && other.value is null || value == other.value || value is null && other.value == "" || value == "" && other.value is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(value);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

