using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkDescriptionAttribute Signatire: 0xe9f9578a size: 8 flags: FLAGS_NONE

    // @string class:  Type.TYPE_CSTRING Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    public partial class hkDescriptionAttribute : IHavokObject, IEquatable<hkDescriptionAttribute?>
    {
        public string @string { set; get; } = "";

        public virtual uint Signature { set; get; } = 0xe9f9578a;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            @string = des.ReadCString(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteCString(bw, @string);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            @string = xd.ReadString(xe, nameof(@string));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteString(xe, nameof(@string), @string);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkDescriptionAttribute);
        }

        public bool Equals(hkDescriptionAttribute? other)
        {
            return other is not null &&
                   (@string is null && other.@string is null || @string == other.@string || @string is null && other.@string == "" || @string == "" && other.@string is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(@string);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

