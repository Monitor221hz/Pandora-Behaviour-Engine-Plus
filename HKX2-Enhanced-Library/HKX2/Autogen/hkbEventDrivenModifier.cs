using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbEventDrivenModifier Signatire: 0x7ed3f44e size: 104 flags: FLAGS_NONE

    // activateEventId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // deactivateEventId class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 92 flags: FLAGS_NONE enum: 
    // activeByDefault class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // isActive class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 97 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbEventDrivenModifier : hkbModifierWrapper, IEquatable<hkbEventDrivenModifier?>
    {
        public int activateEventId { set; get; }
        public int deactivateEventId { set; get; }
        public bool activeByDefault { set; get; }
        private bool isActive { set; get; }

        public override uint Signature { set; get; } = 0x7ed3f44e;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            activateEventId = br.ReadInt32();
            deactivateEventId = br.ReadInt32();
            activeByDefault = br.ReadBoolean();
            isActive = br.ReadBoolean();
            br.Position += 6;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteInt32(activateEventId);
            bw.WriteInt32(deactivateEventId);
            bw.WriteBoolean(activeByDefault);
            bw.WriteBoolean(isActive);
            bw.Position += 6;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            activateEventId = xd.ReadInt32(xe, nameof(activateEventId));
            deactivateEventId = xd.ReadInt32(xe, nameof(deactivateEventId));
            activeByDefault = xd.ReadBoolean(xe, nameof(activeByDefault));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(activateEventId), activateEventId);
            xs.WriteNumber(xe, nameof(deactivateEventId), deactivateEventId);
            xs.WriteBoolean(xe, nameof(activeByDefault), activeByDefault);
            xs.WriteSerializeIgnored(xe, nameof(isActive));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbEventDrivenModifier);
        }

        public bool Equals(hkbEventDrivenModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   activateEventId.Equals(other.activateEventId) &&
                   deactivateEventId.Equals(other.deactivateEventId) &&
                   activeByDefault.Equals(other.activeByDefault) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(activateEventId);
            hashcode.Add(deactivateEventId);
            hashcode.Add(activeByDefault);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

