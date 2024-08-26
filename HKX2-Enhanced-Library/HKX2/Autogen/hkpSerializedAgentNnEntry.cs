using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpSerializedAgentNnEntry Signatire: 0x49ec7de3 size: 368 flags: FLAGS_NONE

    // bodyA class: hkpEntity Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // bodyB class: hkpEntity Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // bodyAId class:  Type.TYPE_ULONG Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // bodyBId class:  Type.TYPE_ULONG Type.TYPE_VOID arrSize: 0 offset: 40 flags: FLAGS_NONE enum: 
    // useEntityIds class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // agentType class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 49 flags: FLAGS_NONE enum: SerializedAgentType
    // atom class: hkpSimpleContactConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // propertiesStream class:  Type.TYPE_ARRAY Type.TYPE_UINT8 arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // contactPoints class: hkContactPoint Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // cpIdMgr class:  Type.TYPE_ARRAY Type.TYPE_UINT8 arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // nnEntryData class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 160 offset: 160 flags: FLAGS_NONE enum: 
    // trackInfo class: hkpSerializedTrack1nInfo Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 320 flags: FLAGS_NONE enum: 
    // endianCheckBuffer class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 4 offset: 352 flags: FLAGS_NONE enum: 
    // version class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 356 flags: FLAGS_NONE enum: 
    public partial class hkpSerializedAgentNnEntry : hkReferencedObject, IEquatable<hkpSerializedAgentNnEntry?>
    {
        public hkpEntity? bodyA { set; get; }
        public hkpEntity? bodyB { set; get; }
        public ulong bodyAId { set; get; }
        public ulong bodyBId { set; get; }
        public bool useEntityIds { set; get; }
        public sbyte agentType { set; get; }
        public hkpSimpleContactConstraintAtom atom { set; get; } = new();
        public IList<byte> propertiesStream { set; get; } = Array.Empty<byte>();
        public IList<hkContactPoint> contactPoints { set; get; } = Array.Empty<hkContactPoint>();
        public IList<byte> cpIdMgr { set; get; } = Array.Empty<byte>();
        public byte[] nnEntryData = new byte[160];
        public hkpSerializedTrack1nInfo trackInfo { set; get; } = new();
        public byte[] endianCheckBuffer = new byte[4];
        public uint version { set; get; }

        public override uint Signature { set; get; } = 0x49ec7de3;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            bodyA = des.ReadClassPointer<hkpEntity>(br);
            bodyB = des.ReadClassPointer<hkpEntity>(br);
            bodyAId = br.ReadUInt64();
            bodyBId = br.ReadUInt64();
            useEntityIds = br.ReadBoolean();
            agentType = br.ReadSByte();
            br.Position += 14;
            atom.Read(des, br);
            propertiesStream = des.ReadByteArray(br);
            contactPoints = des.ReadClassArray<hkContactPoint>(br);
            cpIdMgr = des.ReadByteArray(br);
            nnEntryData = des.ReadByteCStyleArray(br, 160);
            trackInfo.Read(des, br);
            endianCheckBuffer = des.ReadByteCStyleArray(br, 4);
            version = br.ReadUInt32();
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, bodyA);
            s.WriteClassPointer(bw, bodyB);
            bw.WriteUInt64(bodyAId);
            bw.WriteUInt64(bodyBId);
            bw.WriteBoolean(useEntityIds);
            bw.WriteSByte(agentType);
            bw.Position += 14;
            atom.Write(s, bw);
            s.WriteByteArray(bw, propertiesStream);
            s.WriteClassArray(bw, contactPoints);
            s.WriteByteArray(bw, cpIdMgr);
            s.WriteByteCStyleArray(bw, nnEntryData);
            trackInfo.Write(s, bw);
            s.WriteByteCStyleArray(bw, endianCheckBuffer);
            bw.WriteUInt32(version);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            bodyA = xd.ReadClassPointer<hkpEntity>(this, xe, nameof(bodyA));
            bodyB = xd.ReadClassPointer<hkpEntity>(this, xe, nameof(bodyB));
            bodyAId = xd.ReadUInt64(xe, nameof(bodyAId));
            bodyBId = xd.ReadUInt64(xe, nameof(bodyBId));
            useEntityIds = xd.ReadBoolean(xe, nameof(useEntityIds));
            agentType = xd.ReadFlag<SerializedAgentType, sbyte>(xe, nameof(agentType));
            atom = xd.ReadClass<hkpSimpleContactConstraintAtom>(xe, nameof(atom));
            propertiesStream = xd.ReadByteArray(xe, nameof(propertiesStream));
            contactPoints = xd.ReadClassArray<hkContactPoint>(xe, nameof(contactPoints));
            cpIdMgr = xd.ReadByteArray(xe, nameof(cpIdMgr));
            nnEntryData = xd.ReadByteCStyleArray(xe, nameof(nnEntryData), 160);
            trackInfo = xd.ReadClass<hkpSerializedTrack1nInfo>(xe, nameof(trackInfo));
            endianCheckBuffer = xd.ReadByteCStyleArray(xe, nameof(endianCheckBuffer), 4);
            version = xd.ReadUInt32(xe, nameof(version));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(bodyA), bodyA);
            xs.WriteClassPointer(xe, nameof(bodyB), bodyB);
            xs.WriteNumber(xe, nameof(bodyAId), bodyAId);
            xs.WriteNumber(xe, nameof(bodyBId), bodyBId);
            xs.WriteBoolean(xe, nameof(useEntityIds), useEntityIds);
            xs.WriteEnum<SerializedAgentType, sbyte>(xe, nameof(agentType), agentType);
            xs.WriteClass<hkpSimpleContactConstraintAtom>(xe, nameof(atom), atom);
            xs.WriteNumberArray(xe, nameof(propertiesStream), propertiesStream);
            xs.WriteClassArray(xe, nameof(contactPoints), contactPoints);
            xs.WriteNumberArray(xe, nameof(cpIdMgr), cpIdMgr);
            xs.WriteNumberArray(xe, nameof(nnEntryData), nnEntryData);
            xs.WriteClass<hkpSerializedTrack1nInfo>(xe, nameof(trackInfo), trackInfo);
            xs.WriteNumberArray(xe, nameof(endianCheckBuffer), endianCheckBuffer);
            xs.WriteNumber(xe, nameof(version), version);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpSerializedAgentNnEntry);
        }

        public bool Equals(hkpSerializedAgentNnEntry? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((bodyA is null && other.bodyA is null) || (bodyA is not null && other.bodyA is not null && bodyA.Equals((IHavokObject)other.bodyA))) &&
                   ((bodyB is null && other.bodyB is null) || (bodyB is not null && other.bodyB is not null && bodyB.Equals((IHavokObject)other.bodyB))) &&
                   bodyAId.Equals(other.bodyAId) &&
                   bodyBId.Equals(other.bodyBId) &&
                   useEntityIds.Equals(other.useEntityIds) &&
                   agentType.Equals(other.agentType) &&
                   ((atom is null && other.atom is null) || (atom is not null && other.atom is not null && atom.Equals((IHavokObject)other.atom))) &&
                   propertiesStream.SequenceEqual(other.propertiesStream) &&
                   contactPoints.SequenceEqual(other.contactPoints) &&
                   cpIdMgr.SequenceEqual(other.cpIdMgr) &&
                   nnEntryData.SequenceEqual(other.nnEntryData) &&
                   ((trackInfo is null && other.trackInfo is null) || (trackInfo is not null && other.trackInfo is not null && trackInfo.Equals((IHavokObject)other.trackInfo))) &&
                   endianCheckBuffer.SequenceEqual(other.endianCheckBuffer) &&
                   version.Equals(other.version) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(bodyA);
            hashcode.Add(bodyB);
            hashcode.Add(bodyAId);
            hashcode.Add(bodyBId);
            hashcode.Add(useEntityIds);
            hashcode.Add(agentType);
            hashcode.Add(atom);
            hashcode.Add(propertiesStream.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(contactPoints.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(cpIdMgr.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(nnEntryData.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(trackInfo);
            hashcode.Add(endianCheckBuffer.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(version);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

