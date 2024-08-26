using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbTimerModifierInternalState Signatire: 0x83ec2d42 size: 24 flags: FLAGS_NONE

    // secondsElapsed class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkbTimerModifierInternalState : hkReferencedObject, IEquatable<hkbTimerModifierInternalState?>
    {
        public float secondsElapsed { set; get; }

        public override uint Signature { set; get; } = 0x83ec2d42;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            secondsElapsed = br.ReadSingle();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(secondsElapsed);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            secondsElapsed = xd.ReadSingle(xe, nameof(secondsElapsed));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(secondsElapsed), secondsElapsed);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbTimerModifierInternalState);
        }

        public bool Equals(hkbTimerModifierInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   secondsElapsed.Equals(other.secondsElapsed) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(secondsElapsed);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

