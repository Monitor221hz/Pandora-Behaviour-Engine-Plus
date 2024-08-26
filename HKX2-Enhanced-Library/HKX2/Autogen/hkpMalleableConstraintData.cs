using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpMalleableConstraintData Signatire: 0x6748b2cf size: 64 flags: FLAGS_NONE

    // constraintData class: hkpConstraintData Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 24 flags: FLAGS_NONE enum: 
    // atoms class: hkpBridgeAtoms Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // strength class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 56 flags: FLAGS_NONE enum: 
    public partial class hkpMalleableConstraintData : hkpConstraintData, IEquatable<hkpMalleableConstraintData?>
    {
        public hkpConstraintData? constraintData { set; get; }
        public hkpBridgeAtoms atoms { set; get; } = new();
        public float strength { set; get; }

        public override uint Signature { set; get; } = 0x6748b2cf;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            constraintData = des.ReadClassPointer<hkpConstraintData>(br);
            atoms.Read(des, br);
            strength = br.ReadSingle();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, constraintData);
            atoms.Write(s, bw);
            bw.WriteSingle(strength);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            constraintData = xd.ReadClassPointer<hkpConstraintData>(this, xe, nameof(constraintData));
            atoms = xd.ReadClass<hkpBridgeAtoms>(xe, nameof(atoms));
            strength = xd.ReadSingle(xe, nameof(strength));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(constraintData), constraintData);
            xs.WriteClass<hkpBridgeAtoms>(xe, nameof(atoms), atoms);
            xs.WriteFloat(xe, nameof(strength), strength);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpMalleableConstraintData);
        }

        public bool Equals(hkpMalleableConstraintData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((constraintData is null && other.constraintData is null) || (constraintData is not null && other.constraintData is not null && constraintData.Equals((IHavokObject)other.constraintData))) &&
                   ((atoms is null && other.atoms is null) || (atoms is not null && other.atoms is not null && atoms.Equals((IHavokObject)other.atoms))) &&
                   strength.Equals(other.strength) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(constraintData);
            hashcode.Add(atoms);
            hashcode.Add(strength);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

