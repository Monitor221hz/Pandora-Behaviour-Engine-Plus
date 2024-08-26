using System;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpMultiSphereShape Signatire: 0x61a590fc size: 176 flags: FLAGS_NONE

    // numSpheres class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // spheres class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 8 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkpMultiSphereShape : hkpSphereRepShape, IEquatable<hkpMultiSphereShape?>
    {
        public int numSpheres { set; get; }
        public Vector4[] spheres = new Vector4[8];

        public override uint Signature { set; get; } = 0x61a590fc;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            numSpheres = br.ReadInt32();
            br.Position += 12;
            spheres = des.ReadVector4CStyleArray(br, 8);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteInt32(numSpheres);
            bw.Position += 12;
            s.WriteVector4CStyleArray(bw, spheres);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            numSpheres = xd.ReadInt32(xe, nameof(numSpheres));
            spheres = xd.ReadVector4CStyleArray(xe, nameof(spheres), 8);
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteNumber(xe, nameof(numSpheres), numSpheres);
            xs.WriteVector4Array(xe, nameof(spheres), spheres);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpMultiSphereShape);
        }

        public bool Equals(hkpMultiSphereShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   numSpheres.Equals(other.numSpheres) &&
                   spheres.SequenceEqual(other.spheres) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(numSpheres);
            hashcode.Add(spheres.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

