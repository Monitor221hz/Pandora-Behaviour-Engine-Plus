using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpConstraintInstanceSmallArraySerializeOverrideType Signatire: 0xee3c2aec size: 16 flags: FLAGS_NONE

    // data class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 0 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // size class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // capacityAndFlags class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 10 flags: FLAGS_NONE enum: 
    public partial class hkpConstraintInstanceSmallArraySerializeOverrideType : IHavokObject, IEquatable<hkpConstraintInstanceSmallArraySerializeOverrideType?>
    {
        private object? data { set; get; }
        public ushort size { set; get; }
        public ushort capacityAndFlags { set; get; }

        public virtual uint Signature { set; get; } = 0xee3c2aec;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            des.ReadEmptyPointer(br);
            size = br.ReadUInt16();
            capacityAndFlags = br.ReadUInt16();
            br.Position += 4;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteVoidPointer(bw);
            bw.WriteUInt16(size);
            bw.WriteUInt16(capacityAndFlags);
            bw.Position += 4;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            size = xd.ReadUInt16(xe, nameof(size));
            capacityAndFlags = xd.ReadUInt16(xe, nameof(capacityAndFlags));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteSerializeIgnored(xe, nameof(data));
            xs.WriteNumber(xe, nameof(size), size);
            xs.WriteNumber(xe, nameof(capacityAndFlags), capacityAndFlags);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpConstraintInstanceSmallArraySerializeOverrideType);
        }

        public bool Equals(hkpConstraintInstanceSmallArraySerializeOverrideType? other)
        {
            return other is not null &&
                   size.Equals(other.size) &&
                   capacityAndFlags.Equals(other.capacityAndFlags) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(size);
            hashcode.Add(capacityAndFlags);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

