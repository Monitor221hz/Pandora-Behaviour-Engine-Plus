using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbModifierList Signatire: 0xa4180ca1 size: 96 flags: FLAGS_NONE

    // modifiers class: hkbModifier Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    public partial class hkbModifierList : hkbModifier, IEquatable<hkbModifierList?>
    {
        public IList<hkbModifier> modifiers { set; get; } = Array.Empty<hkbModifier>();

        public override uint Signature { set; get; } = 0xa4180ca1;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            modifiers = des.ReadClassPointerArray<hkbModifier>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointerArray(bw, modifiers);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            modifiers = xd.ReadClassPointerArray<hkbModifier>(this, xe, nameof(modifiers));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointerArray(xe, nameof(modifiers), modifiers!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbModifierList);
        }

        public bool Equals(hkbModifierList? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   modifiers.SequenceEqual(other.modifiers) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(modifiers.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

