using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbSenseHandleModifier Signatire: 0x2a064d99 size: 224 flags: FLAGS_NONE

    // handle class: hkbHandle Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 80 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // sensorLocalOffset class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // ranges class: hkbSenseHandleModifierRange Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // handleOut class: hkbHandle Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // handleIn class: hkbHandle Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 168 flags: FLAGS_NONE enum: 
    // localFrameName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 176 flags: FLAGS_NONE enum: 
    // sensorLocalFrameName class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 184 flags: FLAGS_NONE enum: 
    // minDistance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 192 flags: FLAGS_NONE enum: 
    // maxDistance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 196 flags: FLAGS_NONE enum: 
    // distanceOut class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 200 flags: FLAGS_NONE enum: 
    // collisionFilterInfo class:  Type.TYPE_UINT32 Type.TYPE_VOID arrSize: 0 offset: 204 flags: FLAGS_NONE enum: 
    // sensorRagdollBoneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 208 flags: FLAGS_NONE enum: 
    // sensorAnimationBoneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 210 flags: FLAGS_NONE enum: 
    // sensingMode class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 212 flags: FLAGS_NONE enum: SensingMode
    // extrapolateSensorPosition class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 213 flags: FLAGS_NONE enum: 
    // keepFirstSensedHandle class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 214 flags: FLAGS_NONE enum: 
    // foundHandleOut class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 215 flags: FLAGS_NONE enum: 
    // timeSinceLastModify class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 216 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // rangeIndexForEventToSendNextUpdate class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 220 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbSenseHandleModifier : hkbModifier, IEquatable<hkbSenseHandleModifier?>
    {
        public hkbHandle handle { set; get; } = new();
        public Vector4 sensorLocalOffset { set; get; }
        public IList<hkbSenseHandleModifierRange> ranges { set; get; } = Array.Empty<hkbSenseHandleModifierRange>();
        public hkbHandle? handleOut { set; get; }
        public hkbHandle? handleIn { set; get; }
        public string localFrameName { set; get; } = "";
        public string sensorLocalFrameName { set; get; } = "";
        public float minDistance { set; get; }
        public float maxDistance { set; get; }
        public float distanceOut { set; get; }
        public uint collisionFilterInfo { set; get; }
        public short sensorRagdollBoneIndex { set; get; }
        public short sensorAnimationBoneIndex { set; get; }
        public sbyte sensingMode { set; get; }
        public bool extrapolateSensorPosition { set; get; }
        public bool keepFirstSensedHandle { set; get; }
        public bool foundHandleOut { set; get; }
        private float timeSinceLastModify { set; get; }
        private int rangeIndexForEventToSendNextUpdate { set; get; }

        public override uint Signature { set; get; } = 0x2a064d99;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            handle.Read(des, br);
            sensorLocalOffset = br.ReadVector4();
            ranges = des.ReadClassArray<hkbSenseHandleModifierRange>(br);
            handleOut = des.ReadClassPointer<hkbHandle>(br);
            handleIn = des.ReadClassPointer<hkbHandle>(br);
            localFrameName = des.ReadStringPointer(br);
            sensorLocalFrameName = des.ReadStringPointer(br);
            minDistance = br.ReadSingle();
            maxDistance = br.ReadSingle();
            distanceOut = br.ReadSingle();
            collisionFilterInfo = br.ReadUInt32();
            sensorRagdollBoneIndex = br.ReadInt16();
            sensorAnimationBoneIndex = br.ReadInt16();
            sensingMode = br.ReadSByte();
            extrapolateSensorPosition = br.ReadBoolean();
            keepFirstSensedHandle = br.ReadBoolean();
            foundHandleOut = br.ReadBoolean();
            timeSinceLastModify = br.ReadSingle();
            rangeIndexForEventToSendNextUpdate = br.ReadInt32();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            handle.Write(s, bw);
            bw.WriteVector4(sensorLocalOffset);
            s.WriteClassArray(bw, ranges);
            s.WriteClassPointer(bw, handleOut);
            s.WriteClassPointer(bw, handleIn);
            s.WriteStringPointer(bw, localFrameName);
            s.WriteStringPointer(bw, sensorLocalFrameName);
            bw.WriteSingle(minDistance);
            bw.WriteSingle(maxDistance);
            bw.WriteSingle(distanceOut);
            bw.WriteUInt32(collisionFilterInfo);
            bw.WriteInt16(sensorRagdollBoneIndex);
            bw.WriteInt16(sensorAnimationBoneIndex);
            bw.WriteSByte(sensingMode);
            bw.WriteBoolean(extrapolateSensorPosition);
            bw.WriteBoolean(keepFirstSensedHandle);
            bw.WriteBoolean(foundHandleOut);
            bw.WriteSingle(timeSinceLastModify);
            bw.WriteInt32(rangeIndexForEventToSendNextUpdate);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            sensorLocalOffset = xd.ReadVector4(xe, nameof(sensorLocalOffset));
            ranges = xd.ReadClassArray<hkbSenseHandleModifierRange>(xe, nameof(ranges));
            handleOut = xd.ReadClassPointer<hkbHandle>(this, xe, nameof(handleOut));
            handleIn = xd.ReadClassPointer<hkbHandle>(this, xe, nameof(handleIn));
            localFrameName = xd.ReadString(xe, nameof(localFrameName));
            sensorLocalFrameName = xd.ReadString(xe, nameof(sensorLocalFrameName));
            minDistance = xd.ReadSingle(xe, nameof(minDistance));
            maxDistance = xd.ReadSingle(xe, nameof(maxDistance));
            distanceOut = xd.ReadSingle(xe, nameof(distanceOut));
            collisionFilterInfo = xd.ReadUInt32(xe, nameof(collisionFilterInfo));
            sensorRagdollBoneIndex = xd.ReadInt16(xe, nameof(sensorRagdollBoneIndex));
            sensorAnimationBoneIndex = xd.ReadInt16(xe, nameof(sensorAnimationBoneIndex));
            sensingMode = xd.ReadFlag<SensingMode, sbyte>(xe, nameof(sensingMode));
            extrapolateSensorPosition = xd.ReadBoolean(xe, nameof(extrapolateSensorPosition));
            keepFirstSensedHandle = xd.ReadBoolean(xe, nameof(keepFirstSensedHandle));
            foundHandleOut = xd.ReadBoolean(xe, nameof(foundHandleOut));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteSerializeIgnored(xe, nameof(handle));
            xs.WriteVector4(xe, nameof(sensorLocalOffset), sensorLocalOffset);
            xs.WriteClassArray(xe, nameof(ranges), ranges);
            xs.WriteClassPointer(xe, nameof(handleOut), handleOut);
            xs.WriteClassPointer(xe, nameof(handleIn), handleIn);
            xs.WriteString(xe, nameof(localFrameName), localFrameName);
            xs.WriteString(xe, nameof(sensorLocalFrameName), sensorLocalFrameName);
            xs.WriteFloat(xe, nameof(minDistance), minDistance);
            xs.WriteFloat(xe, nameof(maxDistance), maxDistance);
            xs.WriteFloat(xe, nameof(distanceOut), distanceOut);
            xs.WriteNumber(xe, nameof(collisionFilterInfo), collisionFilterInfo);
            xs.WriteNumber(xe, nameof(sensorRagdollBoneIndex), sensorRagdollBoneIndex);
            xs.WriteNumber(xe, nameof(sensorAnimationBoneIndex), sensorAnimationBoneIndex);
            xs.WriteEnum<SensingMode, sbyte>(xe, nameof(sensingMode), sensingMode);
            xs.WriteBoolean(xe, nameof(extrapolateSensorPosition), extrapolateSensorPosition);
            xs.WriteBoolean(xe, nameof(keepFirstSensedHandle), keepFirstSensedHandle);
            xs.WriteBoolean(xe, nameof(foundHandleOut), foundHandleOut);
            xs.WriteSerializeIgnored(xe, nameof(timeSinceLastModify));
            xs.WriteSerializeIgnored(xe, nameof(rangeIndexForEventToSendNextUpdate));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbSenseHandleModifier);
        }

        public bool Equals(hkbSenseHandleModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   sensorLocalOffset.Equals(other.sensorLocalOffset) &&
                   ranges.SequenceEqual(other.ranges) &&
                   ((handleOut is null && other.handleOut is null) || (handleOut is not null && other.handleOut is not null && handleOut.Equals((IHavokObject)other.handleOut))) &&
                   ((handleIn is null && other.handleIn is null) || (handleIn is not null && other.handleIn is not null && handleIn.Equals((IHavokObject)other.handleIn))) &&
                   (localFrameName is null && other.localFrameName is null || localFrameName == other.localFrameName || localFrameName is null && other.localFrameName == "" || localFrameName == "" && other.localFrameName is null) &&
                   (sensorLocalFrameName is null && other.sensorLocalFrameName is null || sensorLocalFrameName == other.sensorLocalFrameName || sensorLocalFrameName is null && other.sensorLocalFrameName == "" || sensorLocalFrameName == "" && other.sensorLocalFrameName is null) &&
                   minDistance.Equals(other.minDistance) &&
                   maxDistance.Equals(other.maxDistance) &&
                   distanceOut.Equals(other.distanceOut) &&
                   collisionFilterInfo.Equals(other.collisionFilterInfo) &&
                   sensorRagdollBoneIndex.Equals(other.sensorRagdollBoneIndex) &&
                   sensorAnimationBoneIndex.Equals(other.sensorAnimationBoneIndex) &&
                   sensingMode.Equals(other.sensingMode) &&
                   extrapolateSensorPosition.Equals(other.extrapolateSensorPosition) &&
                   keepFirstSensedHandle.Equals(other.keepFirstSensedHandle) &&
                   foundHandleOut.Equals(other.foundHandleOut) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(sensorLocalOffset);
            hashcode.Add(ranges.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(handleOut);
            hashcode.Add(handleIn);
            hashcode.Add(localFrameName);
            hashcode.Add(sensorLocalFrameName);
            hashcode.Add(minDistance);
            hashcode.Add(maxDistance);
            hashcode.Add(distanceOut);
            hashcode.Add(collisionFilterInfo);
            hashcode.Add(sensorRagdollBoneIndex);
            hashcode.Add(sensorAnimationBoneIndex);
            hashcode.Add(sensingMode);
            hashcode.Add(extrapolateSensorPosition);
            hashcode.Add(keepFirstSensedHandle);
            hashcode.Add(foundHandleOut);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

