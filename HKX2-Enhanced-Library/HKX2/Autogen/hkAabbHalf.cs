using System;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkAabbHalf Signatire: 0x1d716a17 size: 16 flags: FLAGS_NONE

    // data class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 6 offset: 0 flags: FLAGS_NONE enum: 
    // extras class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 2 offset: 12 flags: FLAGS_NONE enum: 
    public partial class hkAabbHalf : IHavokObject, IEquatable<hkAabbHalf?>
    {
        public ushort[] data = new ushort[6];
        public ushort[] extras = new ushort[2];

        public virtual uint Signature { set; get; } = 0x1d716a17;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            data = des.ReadUInt16CStyleArray(br, 6);
            extras = des.ReadUInt16CStyleArray(br, 2);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteUInt16CStyleArray(bw, data);
            s.WriteUInt16CStyleArray(bw, extras);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            data = xd.ReadUInt16CStyleArray(xe, nameof(data), 6);
            extras = xd.ReadUInt16CStyleArray(xe, nameof(extras), 2);
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumberArray(xe, nameof(data), data);
            xs.WriteNumberArray(xe, nameof(extras), extras);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkAabbHalf);
        }

        public bool Equals(hkAabbHalf? other)
        {
            return other is not null &&
                   data.SequenceEqual(other.data) &&
                   extras.SequenceEqual(other.extras) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(data.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(extras.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

