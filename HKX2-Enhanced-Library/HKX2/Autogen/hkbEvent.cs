using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbEvent Signatire: 0x3e0fd810 size: 24 flags: FLAGS_NONE

    // sender class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 16 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbEvent : hkbEventBase, IEquatable<hkbEvent?>
    {
        public static hkbEvent GetDefault() => new()
        {
            id = -1, 
            payload = null,
        };
        private object? sender { set; get; }

        public override uint Signature { set; get; } = 0x3e0fd810;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            des.ReadEmptyPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteVoidPointer(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteSerializeIgnored(xe, nameof(sender));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbEvent);
        }

        public bool Equals(hkbEvent? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

