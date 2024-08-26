using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpShapeCollection Signatire: 0xe8c3991d size: 48 flags: FLAGS_NONE

    // disableWelding class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // collectionType class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 41 flags: FLAGS_NONE enum: CollectionType
    public partial class hkpShapeCollection : hkpShape, IEquatable<hkpShapeCollection?>
    {
        public bool disableWelding { set; get; }
        public byte collectionType { set; get; }

        public override uint Signature { set; get; } = 0xe8c3991d;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 8;
            disableWelding = br.ReadBoolean();
            collectionType = br.ReadByte();
            br.Position += 6;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 8;
            bw.WriteBoolean(disableWelding);
            bw.WriteByte(collectionType);
            bw.Position += 6;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            disableWelding = xd.ReadBoolean(xe, nameof(disableWelding));
            collectionType = xd.ReadFlag<CollectionType, byte>(xe, nameof(collectionType));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteBoolean(xe, nameof(disableWelding), disableWelding);
            xs.WriteEnum<CollectionType, byte>(xe, nameof(collectionType), collectionType);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpShapeCollection);
        }

        public bool Equals(hkpShapeCollection? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   disableWelding.Equals(other.disableWelding) &&
                   collectionType.Equals(other.collectionType) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(disableWelding);
            hashcode.Add(collectionType);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

