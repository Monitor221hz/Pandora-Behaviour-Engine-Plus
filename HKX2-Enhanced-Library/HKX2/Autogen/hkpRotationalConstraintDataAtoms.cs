using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpRotationalConstraintDataAtoms Signatire: 0xa0c64586 size: 128 flags: FLAGS_NONE

    // rotations class: hkpSetLocalRotationsConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // ang class: hkpAngConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    public partial class hkpRotationalConstraintDataAtoms : IHavokObject, IEquatable<hkpRotationalConstraintDataAtoms?>
    {
        public hkpSetLocalRotationsConstraintAtom rotations { set; get; } = new();
        public hkpAngConstraintAtom ang { set; get; } = new();

        public virtual uint Signature { set; get; } = 0xa0c64586;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            rotations.Read(des, br);
            ang.Read(des, br);
            br.Position += 12;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            rotations.Write(s, bw);
            ang.Write(s, bw);
            bw.Position += 12;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            rotations = xd.ReadClass<hkpSetLocalRotationsConstraintAtom>(xe, nameof(rotations));
            ang = xd.ReadClass<hkpAngConstraintAtom>(xe, nameof(ang));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkpSetLocalRotationsConstraintAtom>(xe, nameof(rotations), rotations);
            xs.WriteClass<hkpAngConstraintAtom>(xe, nameof(ang), ang);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpRotationalConstraintDataAtoms);
        }

        public bool Equals(hkpRotationalConstraintDataAtoms? other)
        {
            return other is not null &&
                   ((rotations is null && other.rotations is null) || (rotations is not null && other.rotations is not null && rotations.Equals((IHavokObject)other.rotations))) &&
                   ((ang is null && other.ang is null) || (ang is not null && other.ang is not null && ang.Equals((IHavokObject)other.ang))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(rotations);
            hashcode.Add(ang);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

