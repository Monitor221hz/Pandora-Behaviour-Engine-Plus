using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpPrismaticConstraintData Signatire: 0x3996c387 size: 240 flags: FLAGS_NONE

    // atoms class: hkpPrismaticConstraintDataAtoms Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 32 flags: ALIGN_16|FLAGS_NONE enum: 
    public partial class hkpPrismaticConstraintData : hkpConstraintData, IEquatable<hkpPrismaticConstraintData?>
    {
        public hkpPrismaticConstraintDataAtoms atoms { set; get; } = new();

        public override uint Signature { set; get; } = 0x3996c387;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 8;
            atoms.Read(des, br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 8;
            atoms.Write(s, bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            atoms = xd.ReadClass<hkpPrismaticConstraintDataAtoms>(xe, nameof(atoms));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkpPrismaticConstraintDataAtoms>(xe, nameof(atoms), atoms);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpPrismaticConstraintData);
        }

        public bool Equals(hkpPrismaticConstraintData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((atoms is null && other.atoms is null) || (atoms is not null && other.atoms is not null && atoms.Equals((IHavokObject)other.atoms))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(atoms);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

