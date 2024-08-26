using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpSetLocalTransformsConstraintAtom Signatire: 0x6e2a5198 size: 144 flags: FLAGS_NONE

    // transformA class:  Type.TYPE_TRANSFORM Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // transformB class:  Type.TYPE_TRANSFORM Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    public partial class hkpSetLocalTransformsConstraintAtom : hkpConstraintAtom, IEquatable<hkpSetLocalTransformsConstraintAtom?>
    {
        public Matrix4x4 transformA { set; get; }
        public Matrix4x4 transformB { set; get; }

        public override uint Signature { set; get; } = 0x6e2a5198;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 14;
            transformA = des.ReadTransform(br);
            transformB = des.ReadTransform(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 14;
            s.WriteTransform(bw, transformA);
            s.WriteTransform(bw, transformB);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            transformA = xd.ReadTransform(xe, nameof(transformA));
            transformB = xd.ReadTransform(xe, nameof(transformB));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteTransform(xe, nameof(transformA), transformA);
            xs.WriteTransform(xe, nameof(transformB), transformB);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpSetLocalTransformsConstraintAtom);
        }

        public bool Equals(hkpSetLocalTransformsConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   transformA.Equals(other.transformA) &&
                   transformB.Equals(other.transformB) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(transformA);
            hashcode.Add(transformB);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

