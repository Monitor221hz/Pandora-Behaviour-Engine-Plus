using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpGenericConstraintData Signatire: 0xfa824640 size: 128 flags: FLAGS_NONE

    // atoms class: hkpBridgeAtoms Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // scheme class: hkpGenericConstraintDataScheme Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkpGenericConstraintData : hkpConstraintData, IEquatable<hkpGenericConstraintData?>
    {
        public hkpBridgeAtoms atoms { set; get; } = new();
        public hkpGenericConstraintDataScheme scheme { set; get; } = new();

        public override uint Signature { set; get; } = 0xfa824640;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            atoms.Read(des, br);
            scheme.Read(des, br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            atoms.Write(s, bw);
            scheme.Write(s, bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            atoms = xd.ReadClass<hkpBridgeAtoms>(xe, nameof(atoms));
            scheme = xd.ReadClass<hkpGenericConstraintDataScheme>(xe, nameof(scheme));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkpBridgeAtoms>(xe, nameof(atoms), atoms);
            xs.WriteClass<hkpGenericConstraintDataScheme>(xe, nameof(scheme), scheme);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpGenericConstraintData);
        }

        public bool Equals(hkpGenericConstraintData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((atoms is null && other.atoms is null) || (atoms is not null && other.atoms is not null && atoms.Equals((IHavokObject)other.atoms))) &&
                   ((scheme is null && other.scheme is null) || (scheme is not null && other.scheme is not null && scheme.Equals((IHavokObject)other.scheme))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(atoms);
            hashcode.Add(scheme);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

