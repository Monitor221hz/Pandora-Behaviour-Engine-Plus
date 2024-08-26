using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxVertexDescriptionElementDecl Signatire: 0x483a429b size: 16 flags: FLAGS_NONE

    // byteOffset class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // type class:  Type.TYPE_ENUM Type.TYPE_UINT16 arrSize: 0 offset: 4 flags: FLAGS_NONE enum: DataType
    // usage class:  Type.TYPE_ENUM Type.TYPE_UINT16 arrSize: 0 offset: 6 flags: FLAGS_NONE enum: DataUsage
    // byteStride class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // numElements class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    public partial class hkxVertexDescriptionElementDecl : IHavokObject, IEquatable<hkxVertexDescriptionElementDecl?>
    {
        public uint byteOffset { set; get; }
        public ushort type { set; get; }
        public ushort usage { set; get; }
        public uint byteStride { set; get; }
        public byte numElements { set; get; }

        public virtual uint Signature { set; get; } = 0x483a429b;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            byteOffset = br.ReadUInt32();
            type = br.ReadUInt16();
            usage = br.ReadUInt16();
            byteStride = br.ReadUInt32();
            numElements = br.ReadByte();
            br.Position += 3;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteUInt32(byteOffset);
            bw.WriteUInt16(type);
            bw.WriteUInt16(usage);
            bw.WriteUInt32(byteStride);
            bw.WriteByte(numElements);
            bw.Position += 3;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            byteOffset = xd.ReadUInt32(xe, nameof(byteOffset));
            type = xd.ReadFlag<DataType, ushort>(xe, nameof(type));
            usage = xd.ReadFlag<DataUsage, ushort>(xe, nameof(usage));
            byteStride = xd.ReadUInt32(xe, nameof(byteStride));
            numElements = xd.ReadByte(xe, nameof(numElements));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(byteOffset), byteOffset);
            xs.WriteEnum<DataType, ushort>(xe, nameof(type), type);
            xs.WriteEnum<DataUsage, ushort>(xe, nameof(usage), usage);
            xs.WriteNumber(xe, nameof(byteStride), byteStride);
            xs.WriteNumber(xe, nameof(numElements), numElements);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxVertexDescriptionElementDecl);
        }

        public bool Equals(hkxVertexDescriptionElementDecl? other)
        {
            return other is not null &&
                   byteOffset.Equals(other.byteOffset) &&
                   type.Equals(other.type) &&
                   usage.Equals(other.usage) &&
                   byteStride.Equals(other.byteStride) &&
                   numElements.Equals(other.numElements) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(byteOffset);
            hashcode.Add(type);
            hashcode.Add(usage);
            hashcode.Add(byteStride);
            hashcode.Add(numElements);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

