using System;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpAgent1nSector Signatire: 0x626e55a size: 512 flags: FLAGS_NONE

    // bytesAllocated class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // pad0 class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // pad1 class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // pad2 class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    // data class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 496 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkpAgent1nSector : IHavokObject, IEquatable<hkpAgent1nSector?>
    {
        public uint bytesAllocated { set; get; }
        public uint pad0 { set; get; }
        public uint pad1 { set; get; }
        public uint pad2 { set; get; }
        public byte[] data = new byte[496];

        public virtual uint Signature { set; get; } = 0x626e55a;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            bytesAllocated = br.ReadUInt32();
            pad0 = br.ReadUInt32();
            pad1 = br.ReadUInt32();
            pad2 = br.ReadUInt32();
            data = des.ReadByteCStyleArray(br, 496);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteUInt32(bytesAllocated);
            bw.WriteUInt32(pad0);
            bw.WriteUInt32(pad1);
            bw.WriteUInt32(pad2);
            s.WriteByteCStyleArray(bw, data);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            bytesAllocated = xd.ReadUInt32(xe, nameof(bytesAllocated));
            pad0 = xd.ReadUInt32(xe, nameof(pad0));
            pad1 = xd.ReadUInt32(xe, nameof(pad1));
            pad2 = xd.ReadUInt32(xe, nameof(pad2));
            data = xd.ReadByteCStyleArray(xe, nameof(data), 496);
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(bytesAllocated), bytesAllocated);
            xs.WriteNumber(xe, nameof(pad0), pad0);
            xs.WriteNumber(xe, nameof(pad1), pad1);
            xs.WriteNumber(xe, nameof(pad2), pad2);
            xs.WriteNumberArray(xe, nameof(data), data);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpAgent1nSector);
        }

        public bool Equals(hkpAgent1nSector? other)
        {
            return other is not null &&
                   bytesAllocated.Equals(other.bytesAllocated) &&
                   pad0.Equals(other.pad0) &&
                   pad1.Equals(other.pad1) &&
                   pad2.Equals(other.pad2) &&
                   data.SequenceEqual(other.data) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(bytesAllocated);
            hashcode.Add(pad0);
            hashcode.Add(pad1);
            hashcode.Add(pad2);
            hashcode.Add(data.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

