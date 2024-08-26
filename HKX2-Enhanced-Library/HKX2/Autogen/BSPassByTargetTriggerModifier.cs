using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // BSPassByTargetTriggerModifier Signatire: 0x703d7b66 size: 160 flags: FLAGS_NONE

    // targetPosition class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // radius class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // movementDirection class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // triggerEvent class: hkbEventProperty Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // targetPassed class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 144 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class BSPassByTargetTriggerModifier : hkbModifier, IEquatable<BSPassByTargetTriggerModifier?>
    {
        public Vector4 targetPosition { set; get; }
        public float radius { set; get; }
        public Vector4 movementDirection { set; get; }
        public hkbEventProperty triggerEvent { set; get; } = new();
        private bool targetPassed { set; get; }

        public override uint Signature { set; get; } = 0x703d7b66;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            targetPosition = br.ReadVector4();
            radius = br.ReadSingle();
            br.Position += 12;
            movementDirection = br.ReadVector4();
            triggerEvent.Read(des, br);
            targetPassed = br.ReadBoolean();
            br.Position += 15;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(targetPosition);
            bw.WriteSingle(radius);
            bw.Position += 12;
            bw.WriteVector4(movementDirection);
            triggerEvent.Write(s, bw);
            bw.WriteBoolean(targetPassed);
            bw.Position += 15;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            targetPosition = xd.ReadVector4(xe, nameof(targetPosition));
            radius = xd.ReadSingle(xe, nameof(radius));
            movementDirection = xd.ReadVector4(xe, nameof(movementDirection));
            triggerEvent = xd.ReadClass<hkbEventProperty>(xe, nameof(triggerEvent));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(targetPosition), targetPosition);
            xs.WriteFloat(xe, nameof(radius), radius);
            xs.WriteVector4(xe, nameof(movementDirection), movementDirection);
            xs.WriteClass<hkbEventProperty>(xe, nameof(triggerEvent), triggerEvent);
            xs.WriteSerializeIgnored(xe, nameof(targetPassed));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BSPassByTargetTriggerModifier);
        }

        public bool Equals(BSPassByTargetTriggerModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   targetPosition.Equals(other.targetPosition) &&
                   radius.Equals(other.radius) &&
                   movementDirection.Equals(other.movementDirection) &&
                   ((triggerEvent is null && other.triggerEvent is null) || (triggerEvent is not null && other.triggerEvent is not null && triggerEvent.Equals((IHavokObject)other.triggerEvent))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(targetPosition);
            hashcode.Add(radius);
            hashcode.Add(movementDirection);
            hashcode.Add(triggerEvent);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

