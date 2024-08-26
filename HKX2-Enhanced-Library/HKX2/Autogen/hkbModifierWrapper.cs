using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbModifierWrapper Signatire: 0x3697e044 size: 88 flags: FLAGS_NONE

    // modifier class: hkbModifier Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    public partial class hkbModifierWrapper : hkbModifier, IEquatable<hkbModifierWrapper?>
    {
        public hkbModifier? modifier { set; get; }

        public override uint Signature { set; get; } = 0x3697e044;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            modifier = des.ReadClassPointer<hkbModifier>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, modifier);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            modifier = xd.ReadClassPointer<hkbModifier>(this, xe, nameof(modifier));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(modifier), modifier);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbModifierWrapper);
        }

        public bool Equals(hkbModifierWrapper? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((modifier is null && other.modifier is null) || (modifier is not null && other.modifier is not null && modifier.Equals((IHavokObject)other.modifier))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(modifier);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

