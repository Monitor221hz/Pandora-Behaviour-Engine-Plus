using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbMessageLog Signatire: 0x26a196c5 size: 16 flags: FLAGS_NONE

    // messages class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 0 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // maxMessages class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 8 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbMessageLog : IHavokObject, IEquatable<hkbMessageLog?>
    {
        private object? messages { set; get; }
        private int maxMessages { set; get; }

        public virtual uint Signature { set; get; } = 0x26a196c5;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            des.ReadEmptyPointer(br);
            maxMessages = br.ReadInt32();
            br.Position += 4;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteVoidPointer(bw);
            bw.WriteInt32(maxMessages);
            bw.Position += 4;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {

        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteSerializeIgnored(xe, nameof(messages));
            xs.WriteSerializeIgnored(xe, nameof(maxMessages));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbMessageLog);
        }

        public bool Equals(hkbMessageLog? other)
        {
            return other is not null &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();

            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

