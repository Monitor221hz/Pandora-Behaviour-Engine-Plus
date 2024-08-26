using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpConvexTranslateShape Signatire: 0x5ba0a5f7 size: 80 flags: FLAGS_NONE

    // translation class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    public partial class hkpConvexTranslateShape : hkpConvexTransformShapeBase, IEquatable<hkpConvexTranslateShape?>
    {
        public Vector4 translation { set; get; }

        public override uint Signature { set; get; } = 0x5ba0a5f7;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            translation = br.ReadVector4();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(translation);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            translation = xd.ReadVector4(xe, nameof(translation));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(translation), translation);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpConvexTranslateShape);
        }

        public bool Equals(hkpConvexTranslateShape? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   translation.Equals(other.translation) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(translation);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

