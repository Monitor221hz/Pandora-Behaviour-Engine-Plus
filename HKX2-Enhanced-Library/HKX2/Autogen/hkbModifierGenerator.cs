using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbModifierGenerator Signatire: 0x1f81fae6 size: 88 flags: FLAGS_NONE

    // modifier class: hkbModifier Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    // generator class: hkbGenerator Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    public partial class hkbModifierGenerator : hkbGenerator, IEquatable<hkbModifierGenerator?>
    {
        public hkbModifier? modifier { set; get; }
        public hkbGenerator? generator { set; get; }

        public override uint Signature { set; get; } = 0x1f81fae6;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            modifier = des.ReadClassPointer<hkbModifier>(br);
            generator = des.ReadClassPointer<hkbGenerator>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, modifier);
            s.WriteClassPointer(bw, generator);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            modifier = xd.ReadClassPointer<hkbModifier>(this, xe, nameof(modifier));
            generator = xd.ReadClassPointer<hkbGenerator>(this, xe, nameof(generator));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(modifier), modifier);
            xs.WriteClassPointer(xe, nameof(generator), generator);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbModifierGenerator);
        }

        public bool Equals(hkbModifierGenerator? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((modifier is null && other.modifier is null) || (modifier is not null && other.modifier is not null && modifier.Equals((IHavokObject)other.modifier))) &&
                   ((generator is null && other.generator is null) || (generator is not null && other.generator is not null && generator.Equals((IHavokObject)other.generator))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(modifier);
            hashcode.Add(generator);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

