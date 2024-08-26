using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpTypedBroadPhaseHandle Signatire: 0xf4b0f799 size: 12 flags: FLAGS_NONE

    // type class:  Type.TYPE_INT8 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // ownerOffset class:  Type.TYPE_INT8 Type.TYPE_VOID arrSize: 0 offset: 5 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // objectQualityType class:  Type.TYPE_INT8 Type.TYPE_VOID arrSize: 0 offset: 6 flags: FLAGS_NONE enum: 
    // collisionFilterInfo class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    public partial class hkpTypedBroadPhaseHandle : hkpBroadPhaseHandle, IEquatable<hkpTypedBroadPhaseHandle?>
    {
        public sbyte type { set; get; }
        private sbyte ownerOffset { set; get; }
        public sbyte objectQualityType { set; get; }
        public uint collisionFilterInfo { set; get; }

        public override uint Signature { set; get; } = 0xf4b0f799;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            type = br.ReadSByte();
            ownerOffset = br.ReadSByte();
            objectQualityType = br.ReadSByte();
            br.Position += 1;
            collisionFilterInfo = br.ReadUInt32();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSByte(type);
            bw.WriteSByte(ownerOffset);
            bw.WriteSByte(objectQualityType);
            bw.Position += 1;
            bw.WriteUInt32(collisionFilterInfo);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            type = xd.ReadSByte(xe, nameof(type));
            objectQualityType = xd.ReadSByte(xe, nameof(objectQualityType));
            collisionFilterInfo = xd.ReadUInt32(xe, nameof(collisionFilterInfo));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(type), type);
            xs.WriteSerializeIgnored(xe, nameof(ownerOffset));
            xs.WriteNumber(xe, nameof(objectQualityType), objectQualityType);
            xs.WriteNumber(xe, nameof(collisionFilterInfo), collisionFilterInfo);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpTypedBroadPhaseHandle);
        }

        public bool Equals(hkpTypedBroadPhaseHandle? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   type.Equals(other.type) &&
                   objectQualityType.Equals(other.objectQualityType) &&
                   collisionFilterInfo.Equals(other.collisionFilterInfo) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(type);
            hashcode.Add(objectQualityType);
            hashcode.Add(collisionFilterInfo);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

