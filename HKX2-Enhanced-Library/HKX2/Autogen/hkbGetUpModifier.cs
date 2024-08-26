using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbGetUpModifier Signatire: 0x61cb7ac0 size: 128 flags: FLAGS_NONE

    // groundNormal class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // duration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // alignWithGroundDuration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 100 flags: FLAGS_NONE enum: 
    // rootBoneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 104 flags: FLAGS_NONE enum: 
    // otherBoneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 106 flags: FLAGS_NONE enum: 
    // anotherBoneIndex class:  Type.TYPE_INT16 Type.TYPE_VOID arrSize: 0 offset: 108 flags: FLAGS_NONE enum: 
    // timeSinceBegin class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 112 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // timeStep class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 116 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // initNextModify class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 120 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbGetUpModifier : hkbModifier, IEquatable<hkbGetUpModifier?>
    {
        public Vector4 groundNormal { set; get; }
        public float duration { set; get; }
        public float alignWithGroundDuration { set; get; }
        public short rootBoneIndex { set; get; }
        public short otherBoneIndex { set; get; }
        public short anotherBoneIndex { set; get; }
        private float timeSinceBegin { set; get; }
        private float timeStep { set; get; }
        private bool initNextModify { set; get; }

        public override uint Signature { set; get; } = 0x61cb7ac0;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            groundNormal = br.ReadVector4();
            duration = br.ReadSingle();
            alignWithGroundDuration = br.ReadSingle();
            rootBoneIndex = br.ReadInt16();
            otherBoneIndex = br.ReadInt16();
            anotherBoneIndex = br.ReadInt16();
            br.Position += 2;
            timeSinceBegin = br.ReadSingle();
            timeStep = br.ReadSingle();
            initNextModify = br.ReadBoolean();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(groundNormal);
            bw.WriteSingle(duration);
            bw.WriteSingle(alignWithGroundDuration);
            bw.WriteInt16(rootBoneIndex);
            bw.WriteInt16(otherBoneIndex);
            bw.WriteInt16(anotherBoneIndex);
            bw.Position += 2;
            bw.WriteSingle(timeSinceBegin);
            bw.WriteSingle(timeStep);
            bw.WriteBoolean(initNextModify);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            groundNormal = xd.ReadVector4(xe, nameof(groundNormal));
            duration = xd.ReadSingle(xe, nameof(duration));
            alignWithGroundDuration = xd.ReadSingle(xe, nameof(alignWithGroundDuration));
            rootBoneIndex = xd.ReadInt16(xe, nameof(rootBoneIndex));
            otherBoneIndex = xd.ReadInt16(xe, nameof(otherBoneIndex));
            anotherBoneIndex = xd.ReadInt16(xe, nameof(anotherBoneIndex));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(groundNormal), groundNormal);
            xs.WriteFloat(xe, nameof(duration), duration);
            xs.WriteFloat(xe, nameof(alignWithGroundDuration), alignWithGroundDuration);
            xs.WriteNumber(xe, nameof(rootBoneIndex), rootBoneIndex);
            xs.WriteNumber(xe, nameof(otherBoneIndex), otherBoneIndex);
            xs.WriteNumber(xe, nameof(anotherBoneIndex), anotherBoneIndex);
            xs.WriteSerializeIgnored(xe, nameof(timeSinceBegin));
            xs.WriteSerializeIgnored(xe, nameof(timeStep));
            xs.WriteSerializeIgnored(xe, nameof(initNextModify));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbGetUpModifier);
        }

        public bool Equals(hkbGetUpModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   groundNormal.Equals(other.groundNormal) &&
                   duration.Equals(other.duration) &&
                   alignWithGroundDuration.Equals(other.alignWithGroundDuration) &&
                   rootBoneIndex.Equals(other.rootBoneIndex) &&
                   otherBoneIndex.Equals(other.otherBoneIndex) &&
                   anotherBoneIndex.Equals(other.anotherBoneIndex) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(groundNormal);
            hashcode.Add(duration);
            hashcode.Add(alignWithGroundDuration);
            hashcode.Add(rootBoneIndex);
            hashcode.Add(otherBoneIndex);
            hashcode.Add(anotherBoneIndex);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

