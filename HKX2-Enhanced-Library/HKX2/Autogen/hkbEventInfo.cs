using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbEventInfo Signatire: 0x5874eed4 size: 4 flags: FLAGS_NONE

    // flags class:  Type.TYPE_FLAGS Type.TYPE_UINT32 arrSize: 0 offset: 0 flags: FLAGS_NONE enum: Flags
    public partial class hkbEventInfo : IHavokObject, IEquatable<hkbEventInfo?>
    {
        public uint flags { set; get; }

        public virtual uint Signature { set; get; } = 0x5874eed4;
        public hkbEventInfo()
        {
            
        }
        public hkbEventInfo(hkbEventInfo other)
        {
            flags = other.flags;   
        }

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            flags = br.ReadUInt32();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteUInt32(flags);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            flags = xd.ReadFlag<Flags, uint>(xe, nameof(flags));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteFlag<Flags, uint>(xe, nameof(flags), flags);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbEventInfo);
        }

        public bool Equals(hkbEventInfo? other)
        {
            return other is not null &&
                   flags.Equals(other.flags) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(flags);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

