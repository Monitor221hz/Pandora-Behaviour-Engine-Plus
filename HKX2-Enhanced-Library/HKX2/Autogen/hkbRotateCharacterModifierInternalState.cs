using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbRotateCharacterModifierInternalState Signatire: 0xdc40bf4a size: 24 flags: FLAGS_NONE

    // angle class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkbRotateCharacterModifierInternalState : hkReferencedObject, IEquatable<hkbRotateCharacterModifierInternalState?>
    {
        public float angle { set; get; }

        public override uint Signature { set; get; } = 0xdc40bf4a;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            angle = br.ReadSingle();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(angle);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            angle = xd.ReadSingle(xe, nameof(angle));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(angle), angle);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbRotateCharacterModifierInternalState);
        }

        public bool Equals(hkbRotateCharacterModifierInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   angle.Equals(other.angle) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(angle);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

