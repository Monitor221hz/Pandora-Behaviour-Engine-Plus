using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbDetectCloseToGroundModifierInternalState Signatire: 0x7b32d942 size: 24 flags: FLAGS_NONE

    // isCloseToGround class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkbDetectCloseToGroundModifierInternalState : hkReferencedObject, IEquatable<hkbDetectCloseToGroundModifierInternalState?>
    {
        public bool isCloseToGround { set; get; }

        public override uint Signature { set; get; } = 0x7b32d942;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            isCloseToGround = br.ReadBoolean();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteBoolean(isCloseToGround);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            isCloseToGround = xd.ReadBoolean(xe, nameof(isCloseToGround));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteBoolean(xe, nameof(isCloseToGround), isCloseToGround);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbDetectCloseToGroundModifierInternalState);
        }

        public bool Equals(hkbDetectCloseToGroundModifierInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   isCloseToGround.Equals(other.isCloseToGround) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(isCloseToGround);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

