using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpRagdollConstraintDataAtoms Signatire: 0xeed76b00 size: 352 flags: FLAGS_NONE

    // transforms class: hkpSetLocalTransformsConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // setupStabilization class: hkpSetupStabilizationAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // ragdollMotors class: hkpRagdollMotorConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // angFriction class: hkpAngFrictionConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 256 flags: FLAGS_NONE enum: 
    // twistLimit class: hkpTwistLimitConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 268 flags: FLAGS_NONE enum: 
    // coneLimit class: hkpConeLimitConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 288 flags: FLAGS_NONE enum: 
    // planesLimit class: hkpConeLimitConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 308 flags: FLAGS_NONE enum: 
    // ballSocket class: hkpBallSocketConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 328 flags: FLAGS_NONE enum: 
    public partial class hkpRagdollConstraintDataAtoms : IHavokObject, IEquatable<hkpRagdollConstraintDataAtoms?>
    {
        public hkpSetLocalTransformsConstraintAtom transforms { set; get; } = new();
        public hkpSetupStabilizationAtom setupStabilization { set; get; } = new();
        public hkpRagdollMotorConstraintAtom ragdollMotors { set; get; } = new();
        public hkpAngFrictionConstraintAtom angFriction { set; get; } = new();
        public hkpTwistLimitConstraintAtom twistLimit { set; get; } = new();
        public hkpConeLimitConstraintAtom coneLimit { set; get; } = new();
        public hkpConeLimitConstraintAtom planesLimit { set; get; } = new();
        public hkpBallSocketConstraintAtom ballSocket { set; get; } = new();

        public virtual uint Signature { set; get; } = 0xeed76b00;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            transforms.Read(des, br);
            setupStabilization.Read(des, br);
            ragdollMotors.Read(des, br);
            angFriction.Read(des, br);
            twistLimit.Read(des, br);
            coneLimit.Read(des, br);
            planesLimit.Read(des, br);
            ballSocket.Read(des, br);
            br.Position += 8;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            transforms.Write(s, bw);
            setupStabilization.Write(s, bw);
            ragdollMotors.Write(s, bw);
            angFriction.Write(s, bw);
            twistLimit.Write(s, bw);
            coneLimit.Write(s, bw);
            planesLimit.Write(s, bw);
            ballSocket.Write(s, bw);
            bw.Position += 8;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            transforms = xd.ReadClass<hkpSetLocalTransformsConstraintAtom>(xe, nameof(transforms));
            setupStabilization = xd.ReadClass<hkpSetupStabilizationAtom>(xe, nameof(setupStabilization));
            ragdollMotors = xd.ReadClass<hkpRagdollMotorConstraintAtom>(xe, nameof(ragdollMotors));
            angFriction = xd.ReadClass<hkpAngFrictionConstraintAtom>(xe, nameof(angFriction));
            twistLimit = xd.ReadClass<hkpTwistLimitConstraintAtom>(xe, nameof(twistLimit));
            coneLimit = xd.ReadClass<hkpConeLimitConstraintAtom>(xe, nameof(coneLimit));
            planesLimit = xd.ReadClass<hkpConeLimitConstraintAtom>(xe, nameof(planesLimit));
            ballSocket = xd.ReadClass<hkpBallSocketConstraintAtom>(xe, nameof(ballSocket));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkpSetLocalTransformsConstraintAtom>(xe, nameof(transforms), transforms);
            xs.WriteClass<hkpSetupStabilizationAtom>(xe, nameof(setupStabilization), setupStabilization);
            xs.WriteClass<hkpRagdollMotorConstraintAtom>(xe, nameof(ragdollMotors), ragdollMotors);
            xs.WriteClass<hkpAngFrictionConstraintAtom>(xe, nameof(angFriction), angFriction);
            xs.WriteClass<hkpTwistLimitConstraintAtom>(xe, nameof(twistLimit), twistLimit);
            xs.WriteClass<hkpConeLimitConstraintAtom>(xe, nameof(coneLimit), coneLimit);
            xs.WriteClass<hkpConeLimitConstraintAtom>(xe, nameof(planesLimit), planesLimit);
            xs.WriteClass<hkpBallSocketConstraintAtom>(xe, nameof(ballSocket), ballSocket);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpRagdollConstraintDataAtoms);
        }

        public bool Equals(hkpRagdollConstraintDataAtoms? other)
        {
            return other is not null &&
                   ((transforms is null && other.transforms is null) || (transforms is not null && other.transforms is not null && transforms.Equals((IHavokObject)other.transforms))) &&
                   ((setupStabilization is null && other.setupStabilization is null) || (setupStabilization is not null && other.setupStabilization is not null && setupStabilization.Equals((IHavokObject)other.setupStabilization))) &&
                   ((ragdollMotors is null && other.ragdollMotors is null) || (ragdollMotors is not null && other.ragdollMotors is not null && ragdollMotors.Equals((IHavokObject)other.ragdollMotors))) &&
                   ((angFriction is null && other.angFriction is null) || (angFriction is not null && other.angFriction is not null && angFriction.Equals((IHavokObject)other.angFriction))) &&
                   ((twistLimit is null && other.twistLimit is null) || (twistLimit is not null && other.twistLimit is not null && twistLimit.Equals((IHavokObject)other.twistLimit))) &&
                   ((coneLimit is null && other.coneLimit is null) || (coneLimit is not null && other.coneLimit is not null && coneLimit.Equals((IHavokObject)other.coneLimit))) &&
                   ((planesLimit is null && other.planesLimit is null) || (planesLimit is not null && other.planesLimit is not null && planesLimit.Equals((IHavokObject)other.planesLimit))) &&
                   ((ballSocket is null && other.ballSocket is null) || (ballSocket is not null && other.ballSocket is not null && ballSocket.Equals((IHavokObject)other.ballSocket))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(transforms);
            hashcode.Add(setupStabilization);
            hashcode.Add(ragdollMotors);
            hashcode.Add(angFriction);
            hashcode.Add(twistLimit);
            hashcode.Add(coneLimit);
            hashcode.Add(planesLimit);
            hashcode.Add(ballSocket);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

