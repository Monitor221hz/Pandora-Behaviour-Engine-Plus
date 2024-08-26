using System;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpGroupCollisionFilter Signatire: 0x5cc01561 size: 208 flags: FLAGS_NONE

    // noGroupCollisionEnabled class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // collisionGroups class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 32 offset: 76 flags: FLAGS_NONE enum: 
    public partial class hkpGroupCollisionFilter : hkpCollisionFilter, IEquatable<hkpGroupCollisionFilter?>
    {
        public bool noGroupCollisionEnabled { set; get; }
        public uint[] collisionGroups = new uint[32];

        public override uint Signature { set; get; } = 0x5cc01561;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            noGroupCollisionEnabled = br.ReadBoolean();
            br.Position += 3;
            collisionGroups = des.ReadUInt32CStyleArray(br, 32);
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteBoolean(noGroupCollisionEnabled);
            bw.Position += 3;
            s.WriteUInt32CStyleArray(bw, collisionGroups);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            noGroupCollisionEnabled = xd.ReadBoolean(xe, nameof(noGroupCollisionEnabled));
            collisionGroups = xd.ReadUInt32CStyleArray(xe, nameof(collisionGroups), 32);
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteBoolean(xe, nameof(noGroupCollisionEnabled), noGroupCollisionEnabled);
            xs.WriteNumberArray(xe, nameof(collisionGroups), collisionGroups);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpGroupCollisionFilter);
        }

        public bool Equals(hkpGroupCollisionFilter? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   noGroupCollisionEnabled.Equals(other.noGroupCollisionEnabled) &&
                   collisionGroups.SequenceEqual(other.collisionGroups) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(noGroupCollisionEnabled);
            hashcode.Add(collisionGroups.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

