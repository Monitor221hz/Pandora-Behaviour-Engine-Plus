using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpMultiRayShape Signatire: 0xea2e7ec9 size: 56 flags: FLAGS_NONE

    // rays class: hkpMultiRayShapeRay Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // rayPenetrationDistance class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkpMultiRayShape : hkpShape, IEquatable<hkpMultiRayShape?>
    {
        public IList<hkpMultiRayShapeRay> rays { set; get; } = Array.Empty<hkpMultiRayShapeRay>();
        public float rayPenetrationDistance { set; get; }

        public override uint Signature { set; get; } = 0xea2e7ec9;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            rays = des.ReadClassArray<hkpMultiRayShapeRay>(br);
            rayPenetrationDistance = br.ReadSingle();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, rays);
            bw.WriteSingle(rayPenetrationDistance);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            rays = xd.ReadClassArray<hkpMultiRayShapeRay>(xe, nameof(rays));
            rayPenetrationDistance = xd.ReadSingle(xe, nameof(rayPenetrationDistance));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(rays), rays);
            xs.WriteFloat(xe, nameof(rayPenetrationDistance), rayPenetrationDistance);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpMultiRayShape);
        }

        public bool Equals(hkpMultiRayShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   rays.SequenceEqual(other.rays) &&
                   rayPenetrationDistance.Equals(other.rayPenetrationDistance) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(rays.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(rayPenetrationDistance);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

