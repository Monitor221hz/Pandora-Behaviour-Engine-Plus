using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpWheelConstraintDataAtoms Signatire: 0x1188cbe1 size: 304 flags: FLAGS_NONE

    // suspensionBase class: hkpSetLocalTransformsConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // lin0Limit class: hkpLinLimitConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // lin0Soft class: hkpLinSoftConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 156 flags: FLAGS_NONE enum: 
    // lin1 class: hkpLinConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 168 flags: FLAGS_NONE enum: 
    // lin2 class: hkpLinConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 172 flags: FLAGS_NONE enum: 
    // steeringBase class: hkpSetLocalRotationsConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 176 flags: FLAGS_NONE enum: 
    // _2dAng class: hkp_2dAngConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 288 flags: FLAGS_NONE enum: 
    public partial class hkpWheelConstraintDataAtoms : IHavokObject, IEquatable<hkpWheelConstraintDataAtoms?>
    {
        public hkpSetLocalTransformsConstraintAtom suspensionBase { set; get; } = new();
        public hkpLinLimitConstraintAtom lin0Limit { set; get; } = new();
        public hkpLinSoftConstraintAtom lin0Soft { set; get; } = new();
        public hkpLinConstraintAtom lin1 { set; get; } = new();
        public hkpLinConstraintAtom lin2 { set; get; } = new();
        public hkpSetLocalRotationsConstraintAtom steeringBase { set; get; } = new();
        public hkp_2dAngConstraintAtom _2dAng { set; get; } = new();

        public virtual uint Signature { set; get; } = 0x1188cbe1;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            suspensionBase.Read(des, br);
            lin0Limit.Read(des, br);
            lin0Soft.Read(des, br);
            lin1.Read(des, br);
            lin2.Read(des, br);
            steeringBase.Read(des, br);
            _2dAng.Read(des, br);
            br.Position += 12;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            suspensionBase.Write(s, bw);
            lin0Limit.Write(s, bw);
            lin0Soft.Write(s, bw);
            lin1.Write(s, bw);
            lin2.Write(s, bw);
            steeringBase.Write(s, bw);
            _2dAng.Write(s, bw);
            bw.Position += 12;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            suspensionBase = xd.ReadClass<hkpSetLocalTransformsConstraintAtom>(xe, nameof(suspensionBase));
            lin0Limit = xd.ReadClass<hkpLinLimitConstraintAtom>(xe, nameof(lin0Limit));
            lin0Soft = xd.ReadClass<hkpLinSoftConstraintAtom>(xe, nameof(lin0Soft));
            lin1 = xd.ReadClass<hkpLinConstraintAtom>(xe, nameof(lin1));
            lin2 = xd.ReadClass<hkpLinConstraintAtom>(xe, nameof(lin2));
            steeringBase = xd.ReadClass<hkpSetLocalRotationsConstraintAtom>(xe, nameof(steeringBase));
            _2dAng = xd.ReadClass<hkp_2dAngConstraintAtom>(xe, nameof(_2dAng));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkpSetLocalTransformsConstraintAtom>(xe, nameof(suspensionBase), suspensionBase);
            xs.WriteClass<hkpLinLimitConstraintAtom>(xe, nameof(lin0Limit), lin0Limit);
            xs.WriteClass<hkpLinSoftConstraintAtom>(xe, nameof(lin0Soft), lin0Soft);
            xs.WriteClass<hkpLinConstraintAtom>(xe, nameof(lin1), lin1);
            xs.WriteClass<hkpLinConstraintAtom>(xe, nameof(lin2), lin2);
            xs.WriteClass<hkpSetLocalRotationsConstraintAtom>(xe, nameof(steeringBase), steeringBase);
            xs.WriteClass<hkp_2dAngConstraintAtom>(xe, nameof(_2dAng), _2dAng);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpWheelConstraintDataAtoms);
        }

        public bool Equals(hkpWheelConstraintDataAtoms? other)
        {
            return other is not null &&
                   ((suspensionBase is null && other.suspensionBase is null) || (suspensionBase is not null && other.suspensionBase is not null && suspensionBase.Equals((IHavokObject)other.suspensionBase))) &&
                   ((lin0Limit is null && other.lin0Limit is null) || (lin0Limit is not null && other.lin0Limit is not null && lin0Limit.Equals((IHavokObject)other.lin0Limit))) &&
                   ((lin0Soft is null && other.lin0Soft is null) || (lin0Soft is not null && other.lin0Soft is not null && lin0Soft.Equals((IHavokObject)other.lin0Soft))) &&
                   ((lin1 is null && other.lin1 is null) || (lin1 is not null && other.lin1 is not null && lin1.Equals((IHavokObject)other.lin1))) &&
                   ((lin2 is null && other.lin2 is null) || (lin2 is not null && other.lin2 is not null && lin2.Equals((IHavokObject)other.lin2))) &&
                   ((steeringBase is null && other.steeringBase is null) || (steeringBase is not null && other.steeringBase is not null && steeringBase.Equals((IHavokObject)other.steeringBase))) &&
                   ((_2dAng is null && other._2dAng is null) || (_2dAng is not null && other._2dAng is not null && _2dAng.Equals((IHavokObject)other._2dAng))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(suspensionBase);
            hashcode.Add(lin0Limit);
            hashcode.Add(lin0Soft);
            hashcode.Add(lin1);
            hashcode.Add(lin2);
            hashcode.Add(steeringBase);
            hashcode.Add(_2dAng);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

