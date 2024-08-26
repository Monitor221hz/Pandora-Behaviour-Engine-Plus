using System;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpCollisionFilter Signatire: 0x60960336 size: 72 flags: FLAGS_NONE

    // prepad class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 2 offset: 48 flags: FLAGS_NONE enum: 
    // type class:  Type.TYPE_ENUM Type.TYPE_UINT32 arrSize: 0 offset: 56 flags: FLAGS_NONE enum: hkpFilterType
    // postpad class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 3 offset: 60 flags: FLAGS_NONE enum: 
    public partial class hkpCollisionFilter : hkReferencedObject, IEquatable<hkpCollisionFilter?>
    {
        public uint[] prepad = new uint[2];
        public uint type { set; get; }
        public uint[] postpad = new uint[3];

        public override uint Signature { set; get; } = 0x60960336;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 32;
            prepad = des.ReadUInt32CStyleArray(br, 2);
            type = br.ReadUInt32();
            postpad = des.ReadUInt32CStyleArray(br, 3);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 32;
            s.WriteUInt32CStyleArray(bw, prepad);
            bw.WriteUInt32(type);
            s.WriteUInt32CStyleArray(bw, postpad);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            prepad = xd.ReadUInt32CStyleArray(xe, nameof(prepad), 2);
            type = xd.ReadFlag<hkpFilterType, uint>(xe, nameof(type));
            postpad = xd.ReadUInt32CStyleArray(xe, nameof(postpad), 3);
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumberArray(xe, nameof(prepad), prepad);
            xs.WriteEnum<hkpFilterType, uint>(xe, nameof(type), type);
            xs.WriteNumberArray(xe, nameof(postpad), postpad);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpCollisionFilter);
        }

        public bool Equals(hkpCollisionFilter? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   prepad.SequenceEqual(other.prepad) &&
                   type.Equals(other.type) &&
                   postpad.SequenceEqual(other.postpad) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(prepad.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(type);
            hashcode.Add(postpad.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

