using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpPulleyConstraintDataAtoms Signatire: 0xb149e5a size: 112 flags: FLAGS_NONE

    // translations class: hkpSetLocalTranslationsConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // pulley class: hkpPulleyConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkpPulleyConstraintDataAtoms : IHavokObject, IEquatable<hkpPulleyConstraintDataAtoms?>
    {
        public hkpSetLocalTranslationsConstraintAtom translations { set; get; } = new();
        public hkpPulleyConstraintAtom pulley { set; get; } = new();

        public virtual uint Signature { set; get; } = 0xb149e5a;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            translations.Read(des, br);
            pulley.Read(des, br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            translations.Write(s, bw);
            pulley.Write(s, bw);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            translations = xd.ReadClass<hkpSetLocalTranslationsConstraintAtom>(xe, nameof(translations));
            pulley = xd.ReadClass<hkpPulleyConstraintAtom>(xe, nameof(pulley));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkpSetLocalTranslationsConstraintAtom>(xe, nameof(translations), translations);
            xs.WriteClass<hkpPulleyConstraintAtom>(xe, nameof(pulley), pulley);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpPulleyConstraintDataAtoms);
        }

        public bool Equals(hkpPulleyConstraintDataAtoms? other)
        {
            return other is not null &&
                   ((translations is null && other.translations is null) || (translations is not null && other.translations is not null && translations.Equals((IHavokObject)other.translations))) &&
                   ((pulley is null && other.pulley is null) || (pulley is not null && other.pulley is not null && pulley.Equals((IHavokObject)other.pulley))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(translations);
            hashcode.Add(pulley);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

