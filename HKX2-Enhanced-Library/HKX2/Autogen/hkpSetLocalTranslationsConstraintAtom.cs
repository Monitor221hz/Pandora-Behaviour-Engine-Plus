using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpSetLocalTranslationsConstraintAtom Signatire: 0x5cbfcf4a size: 48 flags: FLAGS_NONE

    // translationA class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // translationB class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    public partial class hkpSetLocalTranslationsConstraintAtom : hkpConstraintAtom, IEquatable<hkpSetLocalTranslationsConstraintAtom?>
    {
        public Vector4 translationA { set; get; }
        public Vector4 translationB { set; get; }

        public override uint Signature { set; get; } = 0x5cbfcf4a;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 14;
            translationA = br.ReadVector4();
            translationB = br.ReadVector4();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 14;
            bw.WriteVector4(translationA);
            bw.WriteVector4(translationB);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            translationA = xd.ReadVector4(xe, nameof(translationA));
            translationB = xd.ReadVector4(xe, nameof(translationB));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(translationA), translationA);
            xs.WriteVector4(xe, nameof(translationB), translationB);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpSetLocalTranslationsConstraintAtom);
        }

        public bool Equals(hkpSetLocalTranslationsConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   translationA.Equals(other.translationA) &&
                   translationB.Equals(other.translationB) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(translationA);
            hashcode.Add(translationB);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

