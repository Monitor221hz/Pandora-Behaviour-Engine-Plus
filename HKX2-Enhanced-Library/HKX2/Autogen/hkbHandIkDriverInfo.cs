using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbHandIkDriverInfo Signatire: 0xc299090a size: 40 flags: FLAGS_NONE

    // hands class: hkbHandIkDriverInfoHand Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // fadeInOutCurve class:  Type.TYPE_ENUM Type.TYPE_INT8 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: BlendCurve
    public partial class hkbHandIkDriverInfo : hkReferencedObject, IEquatable<hkbHandIkDriverInfo?>
    {
        public IList<hkbHandIkDriverInfoHand> hands { set; get; } = Array.Empty<hkbHandIkDriverInfoHand>();
        public sbyte fadeInOutCurve { set; get; }

        public override uint Signature { set; get; } = 0xc299090a;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            hands = des.ReadClassArray<hkbHandIkDriverInfoHand>(br);
            fadeInOutCurve = br.ReadSByte();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, hands);
            bw.WriteSByte(fadeInOutCurve);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            hands = xd.ReadClassArray<hkbHandIkDriverInfoHand>(xe, nameof(hands));
            fadeInOutCurve = xd.ReadFlag<BlendCurve, sbyte>(xe, nameof(fadeInOutCurve));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(hands), hands);
            xs.WriteEnum<BlendCurve, sbyte>(xe, nameof(fadeInOutCurve), fadeInOutCurve);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbHandIkDriverInfo);
        }

        public bool Equals(hkbHandIkDriverInfo? other)
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

