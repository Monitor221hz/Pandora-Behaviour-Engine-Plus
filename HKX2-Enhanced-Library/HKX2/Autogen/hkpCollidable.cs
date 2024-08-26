using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpCollidable Signatire: 0x9a0e42a5 size: 112 flags: FLAGS_NONE

    // ownerOffset class:  Type.TYPE_INT8 Type.TYPE_VOID arrSize: 0 offset: 32 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // forceCollideOntoPpu class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 33 flags: FLAGS_NONE enum: 
    // shapeSizeOnSpu class:  Type.TYPE_UINT16 Type.TYPE_VOID arrSize: 0 offset: 34 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // broadPhaseHandle class: hkpTypedBroadPhaseHandle Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 36 flags: FLAGS_NONE enum: 
    // boundingVolumeData class: hkpCollidableBoundingVolumeData Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 48 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // allowedPenetrationDepth class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    public partial class hkpCollidable : hkpCdBody, IEquatable<hkpCollidable?>
    {
        private sbyte ownerOffset { set; get; }
        public byte forceCollideOntoPpu { set; get; }
        private ushort shapeSizeOnSpu { set; get; }
        public hkpTypedBroadPhaseHandle broadPhaseHandle { set; get; } = new();
        public hkpCollidableBoundingVolumeData boundingVolumeData { set; get; } = new();
        public float allowedPenetrationDepth { set; get; }

        public override uint Signature { set; get; } = 0x9a0e42a5;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            ownerOffset = br.ReadSByte();
            forceCollideOntoPpu = br.ReadByte();
            shapeSizeOnSpu = br.ReadUInt16();
            broadPhaseHandle.Read(des, br);
            boundingVolumeData.Read(des, br);
            allowedPenetrationDepth = br.ReadSingle();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSByte(ownerOffset);
            bw.WriteByte(forceCollideOntoPpu);
            bw.WriteUInt16(shapeSizeOnSpu);
            broadPhaseHandle.Write(s, bw);
            boundingVolumeData.Write(s, bw);
            bw.WriteSingle(allowedPenetrationDepth);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            forceCollideOntoPpu = xd.ReadByte(xe, nameof(forceCollideOntoPpu));
            broadPhaseHandle = xd.ReadClass<hkpTypedBroadPhaseHandle>(xe, nameof(broadPhaseHandle));
            allowedPenetrationDepth = xd.ReadSingle(xe, nameof(allowedPenetrationDepth));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteSerializeIgnored(xe, nameof(ownerOffset));
            xs.WriteNumber(xe, nameof(forceCollideOntoPpu), forceCollideOntoPpu);
            xs.WriteSerializeIgnored(xe, nameof(shapeSizeOnSpu));
            xs.WriteClass<hkpTypedBroadPhaseHandle>(xe, nameof(broadPhaseHandle), broadPhaseHandle);
            xs.WriteSerializeIgnored(xe, nameof(boundingVolumeData));
            xs.WriteFloat(xe, nameof(allowedPenetrationDepth), allowedPenetrationDepth);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpCollidable);
        }

        public bool Equals(hkpCollidable? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   forceCollideOntoPpu.Equals(other.forceCollideOntoPpu) &&
                   ((broadPhaseHandle is null && other.broadPhaseHandle is null) || (broadPhaseHandle is not null && other.broadPhaseHandle is not null && broadPhaseHandle.Equals((IHavokObject)other.broadPhaseHandle))) &&
                   allowedPenetrationDepth.Equals(other.allowedPenetrationDepth) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(forceCollideOntoPpu);
            hashcode.Add(broadPhaseHandle);
            hashcode.Add(allowedPenetrationDepth);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

