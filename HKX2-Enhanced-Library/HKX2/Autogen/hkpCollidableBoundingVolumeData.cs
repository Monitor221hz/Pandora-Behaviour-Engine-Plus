using System;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpCollidableBoundingVolumeData Signatire: 0xb5f0e6b1 size: 56 flags: FLAGS_NONE

    // min class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 3 offset: 0 flags: FLAGS_NONE enum: 
    // expansionMin class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 3 offset: 12 flags: FLAGS_NONE enum: 
    // expansionShift class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 15 flags: FLAGS_NONE enum: 
    // max class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 3 offset: 16 flags: FLAGS_NONE enum: 
    // expansionMax class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 3 offset: 28 flags: FLAGS_NONE enum: 
    // padding class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 31 flags: FLAGS_NONE enum: 
    // numChildShapeAabbs class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 32 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // capacityChildShapeAabbs class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 34 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // childShapeAabbs class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 40 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // childShapeKeys class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 48 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkpCollidableBoundingVolumeData : IHavokObject, IEquatable<hkpCollidableBoundingVolumeData?>
    {
        public uint[] min = new uint[3];
        public byte[] expansionMin = new byte[3];
        public byte expansionShift { set; get; }
        public uint[] max = new uint[3];
        public byte[] expansionMax = new byte[3];
        public byte padding { set; get; }
        private ushort numChildShapeAabbs { set; get; }
        private ushort capacityChildShapeAabbs { set; get; }
        private object? childShapeAabbs { set; get; }
        private object? childShapeKeys { set; get; }

        public virtual uint Signature { set; get; } = 0xb5f0e6b1;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            min = des.ReadUInt32CStyleArray(br, 3);
            expansionMin = des.ReadByteCStyleArray(br, 3);
            expansionShift = br.ReadByte();
            max = des.ReadUInt32CStyleArray(br, 3);
            expansionMax = des.ReadByteCStyleArray(br, 3);
            padding = br.ReadByte();
            numChildShapeAabbs = br.ReadUInt16();
            capacityChildShapeAabbs = br.ReadUInt16();
            br.Position += 4;
            des.ReadEmptyPointer(br);
            des.ReadEmptyPointer(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteUInt32CStyleArray(bw, min);
            s.WriteByteCStyleArray(bw, expansionMin);
            bw.WriteByte(expansionShift);
            s.WriteUInt32CStyleArray(bw, max);
            s.WriteByteCStyleArray(bw, expansionMax);
            bw.WriteByte(padding);
            bw.WriteUInt16(numChildShapeAabbs);
            bw.WriteUInt16(capacityChildShapeAabbs);
            bw.Position += 4;
            s.WriteVoidPointer(bw);
            s.WriteVoidPointer(bw);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            min = xd.ReadUInt32CStyleArray(xe, nameof(min), 3);
            expansionMin = xd.ReadByteCStyleArray(xe, nameof(expansionMin), 3);
            expansionShift = xd.ReadByte(xe, nameof(expansionShift));
            max = xd.ReadUInt32CStyleArray(xe, nameof(max), 3);
            expansionMax = xd.ReadByteCStyleArray(xe, nameof(expansionMax), 3);
            padding = xd.ReadByte(xe, nameof(padding));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumberArray(xe, nameof(min), min);
            xs.WriteNumberArray(xe, nameof(expansionMin), expansionMin);
            xs.WriteNumber(xe, nameof(expansionShift), expansionShift);
            xs.WriteNumberArray(xe, nameof(max), max);
            xs.WriteNumberArray(xe, nameof(expansionMax), expansionMax);
            xs.WriteNumber(xe, nameof(padding), padding);
            xs.WriteSerializeIgnored(xe, nameof(numChildShapeAabbs));
            xs.WriteSerializeIgnored(xe, nameof(capacityChildShapeAabbs));
            xs.WriteSerializeIgnored(xe, nameof(childShapeAabbs));
            xs.WriteSerializeIgnored(xe, nameof(childShapeKeys));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpCollidableBoundingVolumeData);
        }

        public bool Equals(hkpCollidableBoundingVolumeData? other)
        {
            return other is not null &&
                   min.SequenceEqual(other.min) &&
                   expansionMin.SequenceEqual(other.expansionMin) &&
                   expansionShift.Equals(other.expansionShift) &&
                   max.SequenceEqual(other.max) &&
                   expansionMax.SequenceEqual(other.expansionMax) &&
                   padding.Equals(other.padding) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(min.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(expansionMin.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(expansionShift);
            hashcode.Add(max.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(expansionMax.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(padding);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

