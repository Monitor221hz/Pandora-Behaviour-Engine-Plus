using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpWheelConstraintData Signatire: 0xb4c46671 size: 368 flags: FLAGS_NONE

    // atoms class: hkpWheelConstraintDataAtoms Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 32 flags: ALIGN_16|FLAGS_NONE enum: 
    // initialAxleInB class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 336 flags: FLAGS_NONE enum: 
    // initialSteeringAxisInB class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 352 flags: FLAGS_NONE enum: 
    public partial class hkpWheelConstraintData : hkpConstraintData, IEquatable<hkpWheelConstraintData?>
    {
        public hkpWheelConstraintDataAtoms atoms { set; get; } = new();
        public Vector4 initialAxleInB { set; get; }
        public Vector4 initialSteeringAxisInB { set; get; }

        public override uint Signature { set; get; } = 0xb4c46671;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            br.Position += 8;
            atoms.Read(des, br);
            initialAxleInB = br.ReadVector4();
            initialSteeringAxisInB = br.ReadVector4();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.Position += 8;
            atoms.Write(s, bw);
            bw.WriteVector4(initialAxleInB);
            bw.WriteVector4(initialSteeringAxisInB);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            atoms = xd.ReadClass<hkpWheelConstraintDataAtoms>(xe, nameof(atoms));
            initialAxleInB = xd.ReadVector4(xe, nameof(initialAxleInB));
            initialSteeringAxisInB = xd.ReadVector4(xe, nameof(initialSteeringAxisInB));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkpWheelConstraintDataAtoms>(xe, nameof(atoms), atoms);
            xs.WriteVector4(xe, nameof(initialAxleInB), initialAxleInB);
            xs.WriteVector4(xe, nameof(initialSteeringAxisInB), initialSteeringAxisInB);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpWheelConstraintData);
        }

        public bool Equals(hkpWheelConstraintData? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((atoms is null && other.atoms is null) || (atoms is not null && other.atoms is not null && atoms.Equals((IHavokObject)other.atoms))) &&
                   initialAxleInB.Equals(other.initialAxleInB) &&
                   initialSteeringAxisInB.Equals(other.initialSteeringAxisInB) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(atoms);
            hashcode.Add(initialAxleInB);
            hashcode.Add(initialSteeringAxisInB);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

