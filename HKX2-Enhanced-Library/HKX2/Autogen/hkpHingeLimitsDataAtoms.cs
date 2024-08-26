using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpHingeLimitsDataAtoms Signatire: 0x555876ff size: 144 flags: FLAGS_NONE

    // rotations class: hkpSetLocalRotationsConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // angLimit class: hkpAngLimitConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // _2dAng class: hkp_2dAngConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    public partial class hkpHingeLimitsDataAtoms : IHavokObject, IEquatable<hkpHingeLimitsDataAtoms?>
    {
        public hkpSetLocalRotationsConstraintAtom rotations { set; get; } = new();
        public hkpAngLimitConstraintAtom angLimit { set; get; } = new();
        public hkp_2dAngConstraintAtom _2dAng { set; get; } = new();

        public virtual uint Signature { set; get; } = 0x555876ff;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            rotations.Read(des, br);
            angLimit.Read(des, br);
            _2dAng.Read(des, br);
            br.Position += 12;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            rotations.Write(s, bw);
            angLimit.Write(s, bw);
            _2dAng.Write(s, bw);
            bw.Position += 12;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            rotations = xd.ReadClass<hkpSetLocalRotationsConstraintAtom>(xe, nameof(rotations));
            angLimit = xd.ReadClass<hkpAngLimitConstraintAtom>(xe, nameof(angLimit));
            _2dAng = xd.ReadClass<hkp_2dAngConstraintAtom>(xe, nameof(_2dAng));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkpSetLocalRotationsConstraintAtom>(xe, nameof(rotations), rotations);
            xs.WriteClass<hkpAngLimitConstraintAtom>(xe, nameof(angLimit), angLimit);
            xs.WriteClass<hkp_2dAngConstraintAtom>(xe, nameof(_2dAng), _2dAng);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpHingeLimitsDataAtoms);
        }

        public bool Equals(hkpHingeLimitsDataAtoms? other)
        {
            return other is not null &&
                   ((rotations is null && other.rotations is null) || (rotations is not null && other.rotations is not null && rotations.Equals((IHavokObject)other.rotations))) &&
                   ((angLimit is null && other.angLimit is null) || (angLimit is not null && other.angLimit is not null && angLimit.Equals((IHavokObject)other.angLimit))) &&
                   ((_2dAng is null && other._2dAng is null) || (_2dAng is not null && other._2dAng is not null && _2dAng.Equals((IHavokObject)other._2dAng))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(rotations);
            hashcode.Add(angLimit);
            hashcode.Add(_2dAng);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

