using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpRotationalConstraintData Signatire: 0x74867d9e size: 160 flags: FLAGS_NONE

    // atoms class: hkpRotationalConstraintDataAtoms Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 32 flags: ALIGN_16|FLAGS_NONE enum: 
    public partial class hkpRotationalConstraintData : hkpConstraintData, IEquatable<hkpRotationalConstraintData?>
    {
        public hkpRotationalConstraintDataAtoms atoms { set; get; } = new();

        public override uint Signature { set; get; } = 0x74867d9e;

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
            atoms = xd.ReadClass<hkpRotationalConstraintDataAtoms>(xe, nameof(atoms));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkpRotationalConstraintDataAtoms>(xe, nameof(atoms), atoms);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpRotationalConstraintData);
        }

        public bool Equals(hkpRotationalConstraintData? other)
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

