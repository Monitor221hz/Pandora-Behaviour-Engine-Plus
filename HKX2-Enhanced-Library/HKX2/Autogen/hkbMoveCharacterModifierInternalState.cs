using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbMoveCharacterModifierInternalState Signatire: 0x28f67ba0 size: 24 flags: FLAGS_NONE

    // timeSinceLastModify class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkbMoveCharacterModifierInternalState : hkReferencedObject, IEquatable<hkbMoveCharacterModifierInternalState?>
    {
        public float timeSinceLastModify { set; get; }

        public override uint Signature { set; get; } = 0x28f67ba0;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            timeSinceLastModify = br.ReadSingle();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(timeSinceLastModify);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            timeSinceLastModify = xd.ReadSingle(xe, nameof(timeSinceLastModify));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(timeSinceLastModify), timeSinceLastModify);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbMoveCharacterModifierInternalState);
        }

        public bool Equals(hkbMoveCharacterModifierInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   timeSinceLastModify.Equals(other.timeSinceLastModify) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(timeSinceLastModify);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

