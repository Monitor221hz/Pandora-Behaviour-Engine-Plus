using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpStiffSpringConstraintDataAtoms Signatire: 0x207eb376 size: 64 flags: FLAGS_NONE

    // pivots class: hkpSetLocalTranslationsConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // spring class: hkpStiffSpringConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkpStiffSpringConstraintDataAtoms : IHavokObject, IEquatable<hkpStiffSpringConstraintDataAtoms?>
    {
        public hkpSetLocalTranslationsConstraintAtom pivots { set; get; } = new();
        public hkpStiffSpringConstraintAtom spring { set; get; } = new();

        public virtual uint Signature { set; get; } = 0x207eb376;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            pivots.Read(des, br);
            spring.Read(des, br);
            br.Position += 8;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            pivots.Write(s, bw);
            spring.Write(s, bw);
            bw.Position += 8;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            pivots = xd.ReadClass<hkpSetLocalTranslationsConstraintAtom>(xe, nameof(pivots));
            spring = xd.ReadClass<hkpStiffSpringConstraintAtom>(xe, nameof(spring));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkpSetLocalTranslationsConstraintAtom>(xe, nameof(pivots), pivots);
            xs.WriteClass<hkpStiffSpringConstraintAtom>(xe, nameof(spring), spring);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpStiffSpringConstraintDataAtoms);
        }

        public bool Equals(hkpStiffSpringConstraintDataAtoms? other)
        {
            return other is not null &&
                   ((pivots is null && other.pivots is null) || (pivots is not null && other.pivots is not null && pivots.Equals((IHavokObject)other.pivots))) &&
                   ((spring is null && other.spring is null) || (spring is not null && other.spring is not null && spring.Equals((IHavokObject)other.spring))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(pivots);
            hashcode.Add(spring);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

