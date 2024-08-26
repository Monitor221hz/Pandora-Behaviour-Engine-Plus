using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbNamedEventPayload Signatire: 0x65bdd3a0 size: 24 flags: FLAGS_NONE

    // name class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkbNamedEventPayload : hkbEventPayload, IEquatable<hkbNamedEventPayload?>
    {
        public string name { set; get; } = "";

        public override uint Signature { set; get; } = 0x65bdd3a0;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            name = des.ReadStringPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointer(bw, name);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            name = xd.ReadString(xe, nameof(name));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteString(xe, nameof(name), name);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbNamedEventPayload);
        }

        public bool Equals(hkbNamedEventPayload? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   (name is null && other.name is null || name == other.name || name is null && other.name == "" || name == "" && other.name is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(name);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

