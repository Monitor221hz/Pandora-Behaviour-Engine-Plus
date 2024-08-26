using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbEventDrivenModifierInternalState Signatire: 0xd14bf000 size: 24 flags: FLAGS_NONE

    // isActive class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkbEventDrivenModifierInternalState : hkReferencedObject, IEquatable<hkbEventDrivenModifierInternalState?>
    {
        public bool isActive { set; get; }

        public override uint Signature { set; get; } = 0xd14bf000;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            isActive = br.ReadBoolean();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteBoolean(isActive);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            isActive = xd.ReadBoolean(xe, nameof(isActive));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteBoolean(xe, nameof(isActive), isActive);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbEventDrivenModifierInternalState);
        }

        public bool Equals(hkbEventDrivenModifierInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   isActive.Equals(other.isActive) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(isActive);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

