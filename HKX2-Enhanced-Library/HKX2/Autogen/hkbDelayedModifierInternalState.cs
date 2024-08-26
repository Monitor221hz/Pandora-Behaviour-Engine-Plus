using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbDelayedModifierInternalState Signatire: 0x85fb0b80 size: 24 flags: FLAGS_NONE

    // secondsElapsed class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // isActive class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 20 flags: FLAGS_NONE enum: 
    public partial class hkbDelayedModifierInternalState : hkReferencedObject, IEquatable<hkbDelayedModifierInternalState?>
    {
        public float secondsElapsed { set; get; }
        public bool isActive { set; get; }

        public override uint Signature { set; get; } = 0x85fb0b80;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            secondsElapsed = br.ReadSingle();
            isActive = br.ReadBoolean();
            br.Position += 3;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(secondsElapsed);
            bw.WriteBoolean(isActive);
            bw.Position += 3;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            secondsElapsed = xd.ReadSingle(xe, nameof(secondsElapsed));
            isActive = xd.ReadBoolean(xe, nameof(isActive));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(secondsElapsed), secondsElapsed);
            xs.WriteBoolean(xe, nameof(isActive), isActive);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbDelayedModifierInternalState);
        }

        public bool Equals(hkbDelayedModifierInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   secondsElapsed.Equals(other.secondsElapsed) &&
                   isActive.Equals(other.isActive) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(secondsElapsed);
            hashcode.Add(isActive);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

