using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpConvexShape Signatire: 0xf8f74f85 size: 40 flags: FLAGS_NONE

    // radius class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkpConvexShape : hkpSphereRepShape, IEquatable<hkpConvexShape?>
    {
        public float radius { set; get; }

        public override uint Signature { set; get; } = 0xf8f74f85;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            radius = br.ReadSingle();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(radius);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            radius = xd.ReadSingle(xe, nameof(radius));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(radius), radius);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpConvexShape);
        }

        public bool Equals(hkpConvexShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   radius.Equals(other.radius) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(radius);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

