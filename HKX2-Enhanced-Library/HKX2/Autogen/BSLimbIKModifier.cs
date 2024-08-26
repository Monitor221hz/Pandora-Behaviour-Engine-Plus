using System;
using System.Xml.Linq;

namespace HKX2E
{
    // BSLimbIKModifier Signatire: 0x8ea971e5 size: 120 flags: FLAGS_NONE

    // limitAngleDegrees class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // currentAngle class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 84 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // startBoneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // endBoneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 90 flags: FLAGS_NONE enum: 
    // gain class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 92 flags: FLAGS_NONE enum: 
    // boneRadius class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // castOffset class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 100 flags: FLAGS_NONE enum: 
    // timeStep class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 104 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // pSkeletonMemory class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 112 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class BSLimbIKModifier : hkbModifier, IEquatable<BSLimbIKModifier?>
    {
        public float limitAngleDegrees { set; get; }
        private float currentAngle { set; get; }
        public short startBoneIndex { set; get; }
        public short endBoneIndex { set; get; }
        public float gain { set; get; }
        public float boneRadius { set; get; }
        public float castOffset { set; get; }
        private float timeStep { set; get; }
        private object? pSkeletonMemory { set; get; }

        public override uint Signature { set; get; } = 0x8ea971e5;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            limitAngleDegrees = br.ReadSingle();
            currentAngle = br.ReadSingle();
            startBoneIndex = br.ReadInt16();
            endBoneIndex = br.ReadInt16();
            gain = br.ReadSingle();
            boneRadius = br.ReadSingle();
            castOffset = br.ReadSingle();
            timeStep = br.ReadSingle();
            br.Position += 4;
            des.ReadEmptyPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(limitAngleDegrees);
            bw.WriteSingle(currentAngle);
            bw.WriteInt16(startBoneIndex);
            bw.WriteInt16(endBoneIndex);
            bw.WriteSingle(gain);
            bw.WriteSingle(boneRadius);
            bw.WriteSingle(castOffset);
            bw.WriteSingle(timeStep);
            bw.Position += 4;
            s.WriteVoidPointer(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            limitAngleDegrees = xd.ReadSingle(xe, nameof(limitAngleDegrees));
            startBoneIndex = xd.ReadInt16(xe, nameof(startBoneIndex));
            endBoneIndex = xd.ReadInt16(xe, nameof(endBoneIndex));
            gain = xd.ReadSingle(xe, nameof(gain));
            boneRadius = xd.ReadSingle(xe, nameof(boneRadius));
            castOffset = xd.ReadSingle(xe, nameof(castOffset));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(limitAngleDegrees), limitAngleDegrees);
            xs.WriteSerializeIgnored(xe, nameof(currentAngle));
            xs.WriteNumber(xe, nameof(startBoneIndex), startBoneIndex);
            xs.WriteNumber(xe, nameof(endBoneIndex), endBoneIndex);
            xs.WriteFloat(xe, nameof(gain), gain);
            xs.WriteFloat(xe, nameof(boneRadius), boneRadius);
            xs.WriteFloat(xe, nameof(castOffset), castOffset);
            xs.WriteSerializeIgnored(xe, nameof(timeStep));
            xs.WriteSerializeIgnored(xe, nameof(pSkeletonMemory));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSLimbIKModifier);
        }

        public bool Equals(BSLimbIKModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   limitAngleDegrees.Equals(other.limitAngleDegrees) &&
                   startBoneIndex.Equals(other.startBoneIndex) &&
                   endBoneIndex.Equals(other.endBoneIndex) &&
                   gain.Equals(other.gain) &&
                   boneRadius.Equals(other.boneRadius) &&
                   castOffset.Equals(other.castOffset) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(limitAngleDegrees);
            hashcode.Add(startBoneIndex);
            hashcode.Add(endBoneIndex);
            hashcode.Add(gain);
            hashcode.Add(boneRadius);
            hashcode.Add(castOffset);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

