using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbHandIkModifier Signatire: 0xef8bc2f7 size: 120 flags: FLAGS_NONE

    // hands class: hkbHandIkModifierHand Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // fadeInOutCurve class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 96 flags: FLAGS_NONE enum: BlendCurve
    // internalHandData class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 104 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbHandIkModifier : hkbModifier, IEquatable<hkbHandIkModifier?>
    {
        public IList<hkbHandIkModifierHand> hands { set; get; } = Array.Empty<hkbHandIkModifierHand>();
        public sbyte fadeInOutCurve { set; get; }
        public IList<object> internalHandData { set; get; } = Array.Empty<object>();

        public override uint Signature { set; get; } = 0xef8bc2f7;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            hands = des.ReadClassArray<hkbHandIkModifierHand>(br);
            fadeInOutCurve = br.ReadSByte();
            br.Position += 7;
            des.ReadEmptyArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, hands);
            bw.WriteSByte(fadeInOutCurve);
            bw.Position += 7;
            s.WriteVoidArray(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            hands = xd.ReadClassArray<hkbHandIkModifierHand>(xe, nameof(hands));
            fadeInOutCurve = xd.ReadFlag<BlendCurve, sbyte>(xe, nameof(fadeInOutCurve));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(hands), hands);
            xs.WriteEnum<BlendCurve, sbyte>(xe, nameof(fadeInOutCurve), fadeInOutCurve);
            xs.WriteSerializeIgnored(xe, nameof(internalHandData));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbHandIkModifier);
        }

        public bool Equals(hkbHandIkModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   hands.SequenceEqual(other.hands) &&
                   fadeInOutCurve.Equals(other.fadeInOutCurve) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(hands.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(fadeInOutCurve);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

