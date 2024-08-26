using System;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpGroupFilter Signatire: 0x65ee88e4 size: 272 flags: FLAGS_NONE

    // nextFreeSystemGroup class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // collisionLookupTable class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 32 offset: 76 flags: FLAGS_NONE enum: 
    // pad256 class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 4 offset: 208 flags: FLAGS_NONE enum: 
    public partial class hkpGroupFilter : hkpCollisionFilter, IEquatable<hkpGroupFilter?>
    {
        public int nextFreeSystemGroup { set; get; }
        public uint[] collisionLookupTable = new uint[32];
        public Vector4[] pad256 = new Vector4[4];

        public override uint Signature { set; get; } = 0x65ee88e4;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            nextFreeSystemGroup = br.ReadInt32();
            collisionLookupTable = des.ReadUInt32CStyleArray(br, 32);
            br.Position += 4;
            pad256 = des.ReadVector4CStyleArray(br, 4);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteInt32(nextFreeSystemGroup);
            s.WriteUInt32CStyleArray(bw, collisionLookupTable);
            bw.Position += 4;
            s.WriteVector4CStyleArray(bw, pad256);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            nextFreeSystemGroup = xd.ReadInt32(xe, nameof(nextFreeSystemGroup));
            collisionLookupTable = xd.ReadUInt32CStyleArray(xe, nameof(collisionLookupTable), 32);
            pad256 = xd.ReadVector4CStyleArray(xe, nameof(pad256), 4);
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(nextFreeSystemGroup), nextFreeSystemGroup);
            xs.WriteNumberArray(xe, nameof(collisionLookupTable), collisionLookupTable);
            xs.WriteVector4Array(xe, nameof(pad256), pad256);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpGroupFilter);
        }

        public bool Equals(hkpGroupFilter? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   nextFreeSystemGroup.Equals(other.nextFreeSystemGroup) &&
                   collisionLookupTable.SequenceEqual(other.collisionLookupTable) &&
                   pad256.SequenceEqual(other.pad256) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(nextFreeSystemGroup);
            hashcode.Add(collisionLookupTable.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(pad256.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

