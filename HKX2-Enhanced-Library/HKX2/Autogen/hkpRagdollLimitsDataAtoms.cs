using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpRagdollLimitsDataAtoms Signatire: 0x82b894c3 size: 176 flags: FLAGS_NONE

    // rotations class: hkpSetLocalRotationsConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // twistLimit class: hkpTwistLimitConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // coneLimit class: hkpConeLimitConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 132 flags: FLAGS_NONE enum: 
    // planesLimit class: hkpConeLimitConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 152 flags: FLAGS_NONE enum: 
    public partial class hkpRagdollLimitsDataAtoms : IHavokObject, IEquatable<hkpRagdollLimitsDataAtoms?>
    {
        public hkpSetLocalRotationsConstraintAtom rotations { set; get; } = new();
        public hkpTwistLimitConstraintAtom twistLimit { set; get; } = new();
        public hkpConeLimitConstraintAtom coneLimit { set; get; } = new();
        public hkpConeLimitConstraintAtom planesLimit { set; get; } = new();

        public virtual uint Signature { set; get; } = 0x82b894c3;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            rotations.Read(des, br);
            twistLimit.Read(des, br);
            coneLimit.Read(des, br);
            planesLimit.Read(des, br);
            br.Position += 4;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            rotations.Write(s, bw);
            twistLimit.Write(s, bw);
            coneLimit.Write(s, bw);
            planesLimit.Write(s, bw);
            bw.Position += 4;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            rotations = xd.ReadClass<hkpSetLocalRotationsConstraintAtom>(xe, nameof(rotations));
            twistLimit = xd.ReadClass<hkpTwistLimitConstraintAtom>(xe, nameof(twistLimit));
            coneLimit = xd.ReadClass<hkpConeLimitConstraintAtom>(xe, nameof(coneLimit));
            planesLimit = xd.ReadClass<hkpConeLimitConstraintAtom>(xe, nameof(planesLimit));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkpSetLocalRotationsConstraintAtom>(xe, nameof(rotations), rotations);
            xs.WriteClass<hkpTwistLimitConstraintAtom>(xe, nameof(twistLimit), twistLimit);
            xs.WriteClass<hkpConeLimitConstraintAtom>(xe, nameof(coneLimit), coneLimit);
            xs.WriteClass<hkpConeLimitConstraintAtom>(xe, nameof(planesLimit), planesLimit);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpRagdollLimitsDataAtoms);
        }

        public bool Equals(hkpRagdollLimitsDataAtoms? other)
        {
            return other is not null &&
                   ((rotations is null && other.rotations is null) || (rotations is not null && other.rotations is not null && rotations.Equals((IHavokObject)other.rotations))) &&
                   ((twistLimit is null && other.twistLimit is null) || (twistLimit is not null && other.twistLimit is not null && twistLimit.Equals((IHavokObject)other.twistLimit))) &&
                   ((coneLimit is null && other.coneLimit is null) || (coneLimit is not null && other.coneLimit is not null && coneLimit.Equals((IHavokObject)other.coneLimit))) &&
                   ((planesLimit is null && other.planesLimit is null) || (planesLimit is not null && other.planesLimit is not null && planesLimit.Equals((IHavokObject)other.planesLimit))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(rotations);
            hashcode.Add(twistLimit);
            hashcode.Add(coneLimit);
            hashcode.Add(planesLimit);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

