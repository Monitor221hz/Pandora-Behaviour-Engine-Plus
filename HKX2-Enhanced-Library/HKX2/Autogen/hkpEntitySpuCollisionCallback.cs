using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpEntitySpuCollisionCallback Signatire: 0x81147f05 size: 16 flags: FLAGS_NONE

    // util class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 0 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // capacity class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 8 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // eventFilter class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 10 flags: FLAGS_NONE enum: 
    // userFilter class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 11 flags: FLAGS_NONE enum: 
    public partial class hkpEntitySpuCollisionCallback : IHavokObject, IEquatable<hkpEntitySpuCollisionCallback?>
    {
        private object? util { set; get; }
        private ushort capacity { set; get; }
        public byte eventFilter { set; get; }
        public byte userFilter { set; get; }

        public virtual uint Signature { set; get; } = 0x81147f05;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            des.ReadEmptyPointer(br);
            capacity = br.ReadUInt16();
            eventFilter = br.ReadByte();
            userFilter = br.ReadByte();
            br.Position += 4;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteVoidPointer(bw);
            bw.WriteUInt16(capacity);
            bw.WriteByte(eventFilter);
            bw.WriteByte(userFilter);
            bw.Position += 4;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            eventFilter = xd.ReadByte(xe, nameof(eventFilter));
            userFilter = xd.ReadByte(xe, nameof(userFilter));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteSerializeIgnored(xe, nameof(util));
            xs.WriteSerializeIgnored(xe, nameof(capacity));
            xs.WriteNumber(xe, nameof(eventFilter), eventFilter);
            xs.WriteNumber(xe, nameof(userFilter), userFilter);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpEntitySpuCollisionCallback);
        }

        public bool Equals(hkpEntitySpuCollisionCallback? other)
        {
            return other is not null &&
                   eventFilter.Equals(other.eventFilter) &&
                   userFilter.Equals(other.userFilter) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(eventFilter);
            hashcode.Add(userFilter);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

