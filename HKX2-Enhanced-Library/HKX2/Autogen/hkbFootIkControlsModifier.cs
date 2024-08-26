using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbFootIkControlsModifier Signatire: 0xe5b6f544 size: 176 flags: FLAGS_NONE

    // controlData class: hkbFootIkControlData Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // legs class: hkbFootIkControlsModifierLeg Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // errorOutTranslation class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // alignWithGroundRotation class:  Type.TYPE_QUATERNION Type.TYPE_VOID arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    public partial class hkbFootIkControlsModifier : hkbModifier, IEquatable<hkbFootIkControlsModifier?>
    {
        public hkbFootIkControlData controlData { set; get; } = new();
        public IList<hkbFootIkControlsModifierLeg> legs { set; get; } = Array.Empty<hkbFootIkControlsModifierLeg>();
        public Vector4 errorOutTranslation { set; get; }
        public Quaternion alignWithGroundRotation { set; get; }

        public override uint Signature { set; get; } = 0xe5b6f544;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            controlData.Read(des, br);
            legs = des.ReadClassArray<hkbFootIkControlsModifierLeg>(br);
            errorOutTranslation = br.ReadVector4();
            alignWithGroundRotation = des.ReadQuaternion(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            controlData.Write(s, bw);
            s.WriteClassArray(bw, legs);
            bw.WriteVector4(errorOutTranslation);
            s.WriteQuaternion(bw, alignWithGroundRotation);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            controlData = xd.ReadClass<hkbFootIkControlData>(xe, nameof(controlData));
            legs = xd.ReadClassArray<hkbFootIkControlsModifierLeg>(xe, nameof(legs));
            errorOutTranslation = xd.ReadVector4(xe, nameof(errorOutTranslation));
            alignWithGroundRotation = xd.ReadQuaternion(xe, nameof(alignWithGroundRotation));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkbFootIkControlData>(xe, nameof(controlData), controlData);
            xs.WriteClassArray(xe, nameof(legs), legs);
            xs.WriteVector4(xe, nameof(errorOutTranslation), errorOutTranslation);
            xs.WriteQuaternion(xe, nameof(alignWithGroundRotation), alignWithGroundRotation);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbFootIkControlsModifier);
        }

        public bool Equals(hkbFootIkControlsModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((controlData is null && other.controlData is null) || (controlData is not null && other.controlData is not null && controlData.Equals((IHavokObject)other.controlData))) &&
                   legs.SequenceEqual(other.legs) &&
                   errorOutTranslation.Equals(other.errorOutTranslation) &&
                   alignWithGroundRotation.Equals(other.alignWithGroundRotation) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(controlData);
            hashcode.Add(legs.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(errorOutTranslation);
            hashcode.Add(alignWithGroundRotation);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

