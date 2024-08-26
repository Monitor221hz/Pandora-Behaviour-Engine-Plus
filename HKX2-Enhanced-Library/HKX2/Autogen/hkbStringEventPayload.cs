using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbStringEventPayload Signatire: 0xed04256a size: 24 flags: FLAGS_NONE

    // data class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkbStringEventPayload : hkbEventPayload, IEquatable<hkbStringEventPayload?>
    {
        public string data { set; get; } = "";

        public override uint Signature { set; get; } = 0xed04256a;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            data = des.ReadStringPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointer(bw, data);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            data = xd.ReadString(xe, nameof(data));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteString(xe, nameof(data), data);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbStringEventPayload);
        }

        public bool Equals(hkbStringEventPayload? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   (data is null && other.data is null || data == other.data || data is null && other.data == "" || data == "" && other.data is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(data);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

