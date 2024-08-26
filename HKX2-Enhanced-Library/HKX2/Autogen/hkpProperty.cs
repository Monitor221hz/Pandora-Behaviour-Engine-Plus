using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpProperty Signatire: 0x9ce308e9 size: 16 flags: FLAGS_NONE

    // key class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // alignmentPadding class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // value class: hkpPropertyValue Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkpProperty : IHavokObject, IEquatable<hkpProperty?>
    {
        public uint key { set; get; }
        public uint alignmentPadding { set; get; }
        public hkpPropertyValue value { set; get; } = new();

        public virtual uint Signature { set; get; } = 0x9ce308e9;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            key = br.ReadUInt32();
            alignmentPadding = br.ReadUInt32();
            value.Read(des, br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteUInt32(key);
            bw.WriteUInt32(alignmentPadding);
            value.Write(s, bw);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            key = xd.ReadUInt32(xe, nameof(key));
            alignmentPadding = xd.ReadUInt32(xe, nameof(alignmentPadding));
            value = xd.ReadClass<hkpPropertyValue>(xe, nameof(value));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(key), key);
            xs.WriteNumber(xe, nameof(alignmentPadding), alignmentPadding);
            xs.WriteClass<hkpPropertyValue>(xe, nameof(value), value);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpProperty);
        }

        public bool Equals(hkpProperty? other)
        {
            return other is not null &&
                   key.Equals(other.key) &&
                   alignmentPadding.Equals(other.alignmentPadding) &&
                   ((value is null && other.value is null) || (value is not null && other.value is not null && value.Equals((IHavokObject)other.value))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(key);
            hashcode.Add(alignmentPadding);
            hashcode.Add(value);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

