using System;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkTraceStreamTitle Signatire: 0x6a4ca82c size: 32 flags: FLAGS_NOT_SERIALIZABLE

    // value class:  Type.TYPE_CHAR Type.TYPE_VOID arrSize: 32 offset: 0 flags: FLAGS_NONE enum: 
    public partial class hkTraceStreamTitle : IHavokObject, IEquatable<hkTraceStreamTitle?>
    {
        public string value { set; get; } = "";

        public virtual uint Signature { set; get; } = 0x6a4ca82c;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            value = br.ReadASCII(32);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteASCII(value);
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
            return Equals(obj as hkTraceStreamTitle);
        }

        public bool Equals(hkTraceStreamTitle? other)
        {
            return other is not null &&
                   value.SequenceEqual(other.value) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(value.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

