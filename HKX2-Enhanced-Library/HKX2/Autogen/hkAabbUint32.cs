using System;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkAabbUint32 Signatire: 0x11e7c11 size: 32 flags: FLAGS_NONE

    // min class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 3 offset: 0 flags: ALIGN_16|FLAGS_NONE enum: 
    // expansionMin class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 3 offset: 12 flags: FLAGS_NONE enum: 
    // expansionShift class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 15 flags: FLAGS_NONE enum: 
    // max class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 3 offset: 16 flags: FLAGS_NONE enum: 
    // expansionMax class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 3 offset: 28 flags: FLAGS_NONE enum: 
    // shapeKeyByte class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 31 flags: FLAGS_NONE enum: 
    public partial class hkAabbUint32 : IHavokObject, IEquatable<hkAabbUint32?>
    {
        public uint[] min = new uint[3];
        public byte[] expansionMin = new byte[3];
        public byte expansionShift { set; get; }
        public uint[] max = new uint[3];
        public byte[] expansionMax = new byte[3];
        public byte shapeKeyByte { set; get; }

        public virtual uint Signature { set; get; } = 0x11e7c11;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            min = des.ReadUInt32CStyleArray(br, 3);
            expansionMin = des.ReadByteCStyleArray(br, 3);
            expansionShift = br.ReadByte();
            max = des.ReadUInt32CStyleArray(br, 3);
            expansionMax = des.ReadByteCStyleArray(br, 3);
            shapeKeyByte = br.ReadByte();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteUInt32CStyleArray(bw, min);
            s.WriteByteCStyleArray(bw, expansionMin);
            bw.WriteByte(expansionShift);
            s.WriteUInt32CStyleArray(bw, max);
            s.WriteByteCStyleArray(bw, expansionMax);
            bw.WriteByte(shapeKeyByte);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            min = xd.ReadUInt32CStyleArray(xe, nameof(min), 3);
            expansionMin = xd.ReadByteCStyleArray(xe, nameof(expansionMin), 3);
            expansionShift = xd.ReadByte(xe, nameof(expansionShift));
            max = xd.ReadUInt32CStyleArray(xe, nameof(max), 3);
            expansionMax = xd.ReadByteCStyleArray(xe, nameof(expansionMax), 3);
            shapeKeyByte = xd.ReadByte(xe, nameof(shapeKeyByte));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumberArray(xe, nameof(min), min);
            xs.WriteNumberArray(xe, nameof(expansionMin), expansionMin);
            xs.WriteNumber(xe, nameof(expansionShift), expansionShift);
            xs.WriteNumberArray(xe, nameof(max), max);
            xs.WriteNumberArray(xe, nameof(expansionMax), expansionMax);
            xs.WriteNumber(xe, nameof(shapeKeyByte), shapeKeyByte);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkAabbUint32);
        }

        public bool Equals(hkAabbUint32? other)
        {
            return other is not null &&
                   min.SequenceEqual(other.min) &&
                   expansionMin.SequenceEqual(other.expansionMin) &&
                   expansionShift.Equals(other.expansionShift) &&
                   max.SequenceEqual(other.max) &&
                   expansionMax.SequenceEqual(other.expansionMax) &&
                   shapeKeyByte.Equals(other.shapeKeyByte) &&
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
            hashcode.Add(shapeKeyByte);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

