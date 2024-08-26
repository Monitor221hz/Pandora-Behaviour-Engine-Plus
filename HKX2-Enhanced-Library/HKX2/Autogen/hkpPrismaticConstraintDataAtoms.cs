using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpPrismaticConstraintDataAtoms Signatire: 0x7f516137 size: 208 flags: FLAGS_NONE

    // transforms class: hkpSetLocalTransformsConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // motor class: hkpLinMotorConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // friction class: hkpLinFrictionConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 168 flags: FLAGS_NONE enum: 
    // ang class: hkpAngConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 176 flags: FLAGS_NONE enum: 
    // lin0 class: hkpLinConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 180 flags: FLAGS_NONE enum: 
    // lin1 class: hkpLinConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 184 flags: FLAGS_NONE enum: 
    // linLimit class: hkpLinLimitConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 188 flags: FLAGS_NONE enum: 
    public partial class hkpPrismaticConstraintDataAtoms : IHavokObject, IEquatable<hkpPrismaticConstraintDataAtoms?>
    {
        public hkpSetLocalTransformsConstraintAtom transforms { set; get; } = new();
        public hkpLinMotorConstraintAtom motor { set; get; } = new();
        public hkpLinFrictionConstraintAtom friction { set; get; } = new();
        public hkpAngConstraintAtom ang { set; get; } = new();
        public hkpLinConstraintAtom lin0 { set; get; } = new();
        public hkpLinConstraintAtom lin1 { set; get; } = new();
        public hkpLinLimitConstraintAtom linLimit { set; get; } = new();

        public virtual uint Signature { set; get; } = 0x7f516137;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            transforms.Read(des, br);
            motor.Read(des, br);
            friction.Read(des, br);
            ang.Read(des, br);
            lin0.Read(des, br);
            lin1.Read(des, br);
            linLimit.Read(des, br);
            br.Position += 8;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            transforms.Write(s, bw);
            motor.Write(s, bw);
            friction.Write(s, bw);
            ang.Write(s, bw);
            lin0.Write(s, bw);
            lin1.Write(s, bw);
            linLimit.Write(s, bw);
            bw.Position += 8;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            transforms = xd.ReadClass<hkpSetLocalTransformsConstraintAtom>(xe, nameof(transforms));
            motor = xd.ReadClass<hkpLinMotorConstraintAtom>(xe, nameof(motor));
            friction = xd.ReadClass<hkpLinFrictionConstraintAtom>(xe, nameof(friction));
            ang = xd.ReadClass<hkpAngConstraintAtom>(xe, nameof(ang));
            lin0 = xd.ReadClass<hkpLinConstraintAtom>(xe, nameof(lin0));
            lin1 = xd.ReadClass<hkpLinConstraintAtom>(xe, nameof(lin1));
            linLimit = xd.ReadClass<hkpLinLimitConstraintAtom>(xe, nameof(linLimit));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkpSetLocalTransformsConstraintAtom>(xe, nameof(transforms), transforms);
            xs.WriteClass<hkpLinMotorConstraintAtom>(xe, nameof(motor), motor);
            xs.WriteClass<hkpLinFrictionConstraintAtom>(xe, nameof(friction), friction);
            xs.WriteClass<hkpAngConstraintAtom>(xe, nameof(ang), ang);
            xs.WriteClass<hkpLinConstraintAtom>(xe, nameof(lin0), lin0);
            xs.WriteClass<hkpLinConstraintAtom>(xe, nameof(lin1), lin1);
            xs.WriteClass<hkpLinLimitConstraintAtom>(xe, nameof(linLimit), linLimit);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpPrismaticConstraintDataAtoms);
        }

        public bool Equals(hkpPrismaticConstraintDataAtoms? other)
        {
            return other is not null &&
                   ((transforms is null && other.transforms is null) || (transforms is not null && other.transforms is not null && transforms.Equals((IHavokObject)other.transforms))) &&
                   ((motor is null && other.motor is null) || (motor is not null && other.motor is not null && motor.Equals((IHavokObject)other.motor))) &&
                   ((friction is null && other.friction is null) || (friction is not null && other.friction is not null && friction.Equals((IHavokObject)other.friction))) &&
                   ((ang is null && other.ang is null) || (ang is not null && other.ang is not null && ang.Equals((IHavokObject)other.ang))) &&
                   ((lin0 is null && other.lin0 is null) || (lin0 is not null && other.lin0 is not null && lin0.Equals((IHavokObject)other.lin0))) &&
                   ((lin1 is null && other.lin1 is null) || (lin1 is not null && other.lin1 is not null && lin1.Equals((IHavokObject)other.lin1))) &&
                   ((linLimit is null && other.linLimit is null) || (linLimit is not null && other.linLimit is not null && linLimit.Equals((IHavokObject)other.linLimit))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(transforms);
            hashcode.Add(motor);
            hashcode.Add(friction);
            hashcode.Add(ang);
            hashcode.Add(lin0);
            hashcode.Add(lin1);
            hashcode.Add(linLimit);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

