using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbPoseMatchingGenerator Signatire: 0x29e271b4 size: 240 flags: FLAGS_NONE

    // worldFromModelRotation class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // blendSpeed class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 176 flags: FLAGS_NONE enum: 
    // minSpeedToSwitch class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 180 flags: FLAGS_NONE enum: 
    // minSwitchTimeNoError class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 184 flags: FLAGS_NONE enum: 
    // minSwitchTimeFullError class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 188 flags: FLAGS_NONE enum: 
    // startPlayingEventId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 192 flags: FLAGS_NONE enum: 
    // startMatchingEventId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 196 flags: FLAGS_NONE enum: 
    // rootBoneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 200 flags: FLAGS_NONE enum: 
    // otherBoneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 202 flags: FLAGS_NONE enum: 
    // anotherBoneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 204 flags: FLAGS_NONE enum: 
    // pelvisIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 206 flags: FLAGS_NONE enum: 
    // mode class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 208 flags: FLAGS_NONE enum: Mode
    // currentMatch class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 212 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // bestMatch class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 216 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // timeSinceBetterMatch class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 220 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // error class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 224 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // resetCurrentMatchLocalTime class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 228 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // poseMatchingUtility class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 232 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbPoseMatchingGenerator : hkbBlenderGenerator, IEquatable<hkbPoseMatchingGenerator?>
    {
        public Quaternion worldFromModelRotation { set; get; }
        public float blendSpeed { set; get; }
        public float minSpeedToSwitch { set; get; }
        public float minSwitchTimeNoError { set; get; }
        public float minSwitchTimeFullError { set; get; }
        public int startPlayingEventId { set; get; }
        public int startMatchingEventId { set; get; }
        public short rootBoneIndex { set; get; }
        public short otherBoneIndex { set; get; }
        public short anotherBoneIndex { set; get; }
        public short pelvisIndex { set; get; }
        public sbyte mode { set; get; }
        private int currentMatch { set; get; }
        private int bestMatch { set; get; }
        private float timeSinceBetterMatch { set; get; }
        private float error { set; get; }
        private bool resetCurrentMatchLocalTime { set; get; }
        private object? poseMatchingUtility { set; get; }

        public override uint Signature { set; get; } = 0x29e271b4;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            worldFromModelRotation = des.ReadQuaternion(br);
            blendSpeed = br.ReadSingle();
            minSpeedToSwitch = br.ReadSingle();
            minSwitchTimeNoError = br.ReadSingle();
            minSwitchTimeFullError = br.ReadSingle();
            startPlayingEventId = br.ReadInt32();
            startMatchingEventId = br.ReadInt32();
            rootBoneIndex = br.ReadInt16();
            otherBoneIndex = br.ReadInt16();
            anotherBoneIndex = br.ReadInt16();
            pelvisIndex = br.ReadInt16();
            mode = br.ReadSByte();
            br.Position += 3;
            currentMatch = br.ReadInt32();
            bestMatch = br.ReadInt32();
            timeSinceBetterMatch = br.ReadSingle();
            error = br.ReadSingle();
            resetCurrentMatchLocalTime = br.ReadBoolean();
            br.Position += 3;
            des.ReadEmptyPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteQuaternion(bw, worldFromModelRotation);
            bw.WriteSingle(blendSpeed);
            bw.WriteSingle(minSpeedToSwitch);
            bw.WriteSingle(minSwitchTimeNoError);
            bw.WriteSingle(minSwitchTimeFullError);
            bw.WriteInt32(startPlayingEventId);
            bw.WriteInt32(startMatchingEventId);
            bw.WriteInt16(rootBoneIndex);
            bw.WriteInt16(otherBoneIndex);
            bw.WriteInt16(anotherBoneIndex);
            bw.WriteInt16(pelvisIndex);
            bw.WriteSByte(mode);
            bw.Position += 3;
            bw.WriteInt32(currentMatch);
            bw.WriteInt32(bestMatch);
            bw.WriteSingle(timeSinceBetterMatch);
            bw.WriteSingle(error);
            bw.WriteBoolean(resetCurrentMatchLocalTime);
            bw.Position += 3;
            s.WriteVoidPointer(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            worldFromModelRotation = xd.ReadQuaternion(xe, nameof(worldFromModelRotation));
            blendSpeed = xd.ReadSingle(xe, nameof(blendSpeed));
            minSpeedToSwitch = xd.ReadSingle(xe, nameof(minSpeedToSwitch));
            minSwitchTimeNoError = xd.ReadSingle(xe, nameof(minSwitchTimeNoError));
            minSwitchTimeFullError = xd.ReadSingle(xe, nameof(minSwitchTimeFullError));
            startPlayingEventId = xd.ReadInt32(xe, nameof(startPlayingEventId));
            startMatchingEventId = xd.ReadInt32(xe, nameof(startMatchingEventId));
            rootBoneIndex = xd.ReadInt16(xe, nameof(rootBoneIndex));
            otherBoneIndex = xd.ReadInt16(xe, nameof(otherBoneIndex));
            anotherBoneIndex = xd.ReadInt16(xe, nameof(anotherBoneIndex));
            pelvisIndex = xd.ReadInt16(xe, nameof(pelvisIndex));
            mode = xd.ReadFlag<Mode, sbyte>(xe, nameof(mode));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteQuaternion(xe, nameof(worldFromModelRotation), worldFromModelRotation);
            xs.WriteFloat(xe, nameof(blendSpeed), blendSpeed);
            xs.WriteFloat(xe, nameof(minSpeedToSwitch), minSpeedToSwitch);
            xs.WriteFloat(xe, nameof(minSwitchTimeNoError), minSwitchTimeNoError);
            xs.WriteFloat(xe, nameof(minSwitchTimeFullError), minSwitchTimeFullError);
            xs.WriteNumber(xe, nameof(startPlayingEventId), startPlayingEventId);
            xs.WriteNumber(xe, nameof(startMatchingEventId), startMatchingEventId);
            xs.WriteNumber(xe, nameof(rootBoneIndex), rootBoneIndex);
            xs.WriteNumber(xe, nameof(otherBoneIndex), otherBoneIndex);
            xs.WriteNumber(xe, nameof(anotherBoneIndex), anotherBoneIndex);
            xs.WriteNumber(xe, nameof(pelvisIndex), pelvisIndex);
            xs.WriteEnum<Mode, sbyte>(xe, nameof(mode), mode);
            xs.WriteSerializeIgnored(xe, nameof(currentMatch));
            xs.WriteSerializeIgnored(xe, nameof(bestMatch));
            xs.WriteSerializeIgnored(xe, nameof(timeSinceBetterMatch));
            xs.WriteSerializeIgnored(xe, nameof(error));
            xs.WriteSerializeIgnored(xe, nameof(resetCurrentMatchLocalTime));
            xs.WriteSerializeIgnored(xe, nameof(poseMatchingUtility));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbPoseMatchingGenerator);
        }

        public bool Equals(hkbPoseMatchingGenerator? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   worldFromModelRotation.Equals(other.worldFromModelRotation) &&
                   blendSpeed.Equals(other.blendSpeed) &&
                   minSpeedToSwitch.Equals(other.minSpeedToSwitch) &&
                   minSwitchTimeNoError.Equals(other.minSwitchTimeNoError) &&
                   minSwitchTimeFullError.Equals(other.minSwitchTimeFullError) &&
                   startPlayingEventId.Equals(other.startPlayingEventId) &&
                   startMatchingEventId.Equals(other.startMatchingEventId) &&
                   rootBoneIndex.Equals(other.rootBoneIndex) &&
                   otherBoneIndex.Equals(other.otherBoneIndex) &&
                   anotherBoneIndex.Equals(other.anotherBoneIndex) &&
                   pelvisIndex.Equals(other.pelvisIndex) &&
                   mode.Equals(other.mode) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(worldFromModelRotation);
            hashcode.Add(blendSpeed);
            hashcode.Add(minSpeedToSwitch);
            hashcode.Add(minSwitchTimeNoError);
            hashcode.Add(minSwitchTimeFullError);
            hashcode.Add(startPlayingEventId);
            hashcode.Add(startMatchingEventId);
            hashcode.Add(rootBoneIndex);
            hashcode.Add(otherBoneIndex);
            hashcode.Add(anotherBoneIndex);
            hashcode.Add(pelvisIndex);
            hashcode.Add(mode);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

