using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkMultiThreadCheck Signatire: 0x11e4408b size: 12 flags: FLAGS_NONE

    // threadId class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 0 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // stackTraceId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 4 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // markCount class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 8 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // markBitStack class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 10 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkMultiThreadCheck : IHavokObject, IEquatable<hkMultiThreadCheck?>
    {
        private uint threadId { set; get; }
        private int stackTraceId { set; get; }
        private ushort markCount { set; get; }
        private ushort markBitStack { set; get; }

        public virtual uint Signature { set; get; } = 0x11e4408b;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            threadId = br.ReadUInt32();
            stackTraceId = br.ReadInt32();
            markCount = br.ReadUInt16();
            markBitStack = br.ReadUInt16();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteUInt32(threadId);
            bw.WriteInt32(stackTraceId);
            bw.WriteUInt16(markCount);
            bw.WriteUInt16(markBitStack);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {

        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteSerializeIgnored(xe, nameof(threadId));
            xs.WriteSerializeIgnored(xe, nameof(stackTraceId));
            xs.WriteSerializeIgnored(xe, nameof(markCount));
            xs.WriteSerializeIgnored(xe, nameof(markBitStack));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkMultiThreadCheck);
        }

        public bool Equals(hkMultiThreadCheck? other)
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

