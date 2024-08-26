using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpPropertyValue Signatire: 0xc75925aa size: 8 flags: FLAGS_NONE

    // data class:  Type.TYPE_UINT64 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    public partial class hkpPropertyValue : IHavokObject, IEquatable<hkpPropertyValue?>
    {
        public ulong data { set; get; }

        public virtual uint Signature { set; get; } = 0xc75925aa;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            data = br.ReadUInt64();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteUInt64(data);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            data = xd.ReadUInt64(xe, nameof(data));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(data), data);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpPropertyValue);
        }

        public bool Equals(hkpPropertyValue? other)
        {
            return other is not null &&
                   data.Equals(other.data) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(data);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

