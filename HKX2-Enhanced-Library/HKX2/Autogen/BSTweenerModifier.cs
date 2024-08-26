using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // BSTweenerModifier Signatire: 0xd2d9a04 size: 208 flags: FLAGS_NONE

    // tweenPosition class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // tweenRotation class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 81 flags: FLAGS_NONE enum: 
    // useTweenDuration class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 82 flags: FLAGS_NONE enum: 
    // tweenDuration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // targetPosition class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // targetRotation class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // duration class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 128 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // startTransform class:  Type.TYPE_QSTRANSFORM Type.TYPE_VOID arrSize: 0 offset: 144 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // time class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 192 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class BSTweenerModifier : hkbModifier, IEquatable<BSTweenerModifier?>
    {
        public bool tweenPosition { set; get; }
        public bool tweenRotation { set; get; }
        public bool useTweenDuration { set; get; }
        public float tweenDuration { set; get; }
        public Vector4 targetPosition { set; get; }
        public Quaternion targetRotation { set; get; }
        private float duration { set; get; }
        private Matrix4x4 startTransform { set; get; }
        private float time { set; get; }

        public override uint Signature { set; get; } = 0xd2d9a04;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            tweenPosition = br.ReadBoolean();
            tweenRotation = br.ReadBoolean();
            useTweenDuration = br.ReadBoolean();
            br.Position += 1;
            tweenDuration = br.ReadSingle();
            br.Position += 8;
            targetPosition = br.ReadVector4();
            targetRotation = des.ReadQuaternion(br);
            duration = br.ReadSingle();
            br.Position += 12;
            startTransform = des.ReadQSTransform(br);
            time = br.ReadSingle();
            br.Position += 12;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteBoolean(tweenPosition);
            bw.WriteBoolean(tweenRotation);
            bw.WriteBoolean(useTweenDuration);
            bw.Position += 1;
            bw.WriteSingle(tweenDuration);
            bw.Position += 8;
            bw.WriteVector4(targetPosition);
            s.WriteQuaternion(bw, targetRotation);
            bw.WriteSingle(duration);
            bw.Position += 12;
            s.WriteQSTransform(bw, startTransform);
            bw.WriteSingle(time);
            bw.Position += 12;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            tweenPosition = xd.ReadBoolean(xe, nameof(tweenPosition));
            tweenRotation = xd.ReadBoolean(xe, nameof(tweenRotation));
            useTweenDuration = xd.ReadBoolean(xe, nameof(useTweenDuration));
            tweenDuration = xd.ReadSingle(xe, nameof(tweenDuration));
            targetPosition = xd.ReadVector4(xe, nameof(targetPosition));
            targetRotation = xd.ReadQuaternion(xe, nameof(targetRotation));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteBoolean(xe, nameof(tweenPosition), tweenPosition);
            xs.WriteBoolean(xe, nameof(tweenRotation), tweenRotation);
            xs.WriteBoolean(xe, nameof(useTweenDuration), useTweenDuration);
            xs.WriteFloat(xe, nameof(tweenDuration), tweenDuration);
            xs.WriteVector4(xe, nameof(targetPosition), targetPosition);
            xs.WriteQuaternion(xe, nameof(targetRotation), targetRotation);
            xs.WriteSerializeIgnored(xe, nameof(duration));
            xs.WriteSerializeIgnored(xe, nameof(startTransform));
            xs.WriteSerializeIgnored(xe, nameof(time));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSTweenerModifier);
        }

        public bool Equals(BSTweenerModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   tweenPosition.Equals(other.tweenPosition) &&
                   tweenRotation.Equals(other.tweenRotation) &&
                   useTweenDuration.Equals(other.useTweenDuration) &&
                   tweenDuration.Equals(other.tweenDuration) &&
                   targetPosition.Equals(other.targetPosition) &&
                   targetRotation.Equals(other.targetRotation) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(tweenPosition);
            hashcode.Add(tweenRotation);
            hashcode.Add(useTweenDuration);
            hashcode.Add(tweenDuration);
            hashcode.Add(targetPosition);
            hashcode.Add(targetRotation);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

