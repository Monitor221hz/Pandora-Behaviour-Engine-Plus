using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpLimitedHingeConstraintDataAtoms Signatire: 0x54c7715b size: 240 flags: FLAGS_NONE

    // transforms class: hkpSetLocalTransformsConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // setupStabilization class: hkpSetupStabilizationAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // angMotor class: hkpAngMotorConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // angFriction class: hkpAngFrictionConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 184 flags: FLAGS_NONE enum: 
    // angLimit class: hkpAngLimitConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 196 flags: FLAGS_NONE enum: 
    // _2dAng class: hkp_2dAngConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 212 flags: FLAGS_NONE enum: 
    // ballSocket class: hkpBallSocketConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 216 flags: FLAGS_NONE enum: 
    public partial class hkpLimitedHingeConstraintDataAtoms : IHavokObject, IEquatable<hkpLimitedHingeConstraintDataAtoms?>
    {
        public hkpSetLocalTransformsConstraintAtom transforms { set; get; } = new();
        public hkpSetupStabilizationAtom setupStabilization { set; get; } = new();
        public hkpAngMotorConstraintAtom angMotor { set; get; } = new();
        public hkpAngFrictionConstraintAtom angFriction { set; get; } = new();
        public hkpAngLimitConstraintAtom angLimit { set; get; } = new();
        public hkp_2dAngConstraintAtom _2dAng { set; get; } = new();
        public hkpBallSocketConstraintAtom ballSocket { set; get; } = new();

        public virtual uint Signature { set; get; } = 0x54c7715b;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            transforms.Read(des, br);
            setupStabilization.Read(des, br);
            angMotor.Read(des, br);
            angFriction.Read(des, br);
            angLimit.Read(des, br);
            _2dAng.Read(des, br);
            ballSocket.Read(des, br);
            br.Position += 8;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            transforms.Write(s, bw);
            setupStabilization.Write(s, bw);
            angMotor.Write(s, bw);
            angFriction.Write(s, bw);
            angLimit.Write(s, bw);
            _2dAng.Write(s, bw);
            ballSocket.Write(s, bw);
            bw.Position += 8;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            transforms = xd.ReadClass<hkpSetLocalTransformsConstraintAtom>(xe, nameof(transforms));
            setupStabilization = xd.ReadClass<hkpSetupStabilizationAtom>(xe, nameof(setupStabilization));
            angMotor = xd.ReadClass<hkpAngMotorConstraintAtom>(xe, nameof(angMotor));
            angFriction = xd.ReadClass<hkpAngFrictionConstraintAtom>(xe, nameof(angFriction));
            angLimit = xd.ReadClass<hkpAngLimitConstraintAtom>(xe, nameof(angLimit));
            _2dAng = xd.ReadClass<hkp_2dAngConstraintAtom>(xe, nameof(_2dAng));
            ballSocket = xd.ReadClass<hkpBallSocketConstraintAtom>(xe, nameof(ballSocket));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkpSetLocalTransformsConstraintAtom>(xe, nameof(transforms), transforms);
            xs.WriteClass<hkpSetupStabilizationAtom>(xe, nameof(setupStabilization), setupStabilization);
            xs.WriteClass<hkpAngMotorConstraintAtom>(xe, nameof(angMotor), angMotor);
            xs.WriteClass<hkpAngFrictionConstraintAtom>(xe, nameof(angFriction), angFriction);
            xs.WriteClass<hkpAngLimitConstraintAtom>(xe, nameof(angLimit), angLimit);
            xs.WriteClass<hkp_2dAngConstraintAtom>(xe, nameof(_2dAng), _2dAng);
            xs.WriteClass<hkpBallSocketConstraintAtom>(xe, nameof(ballSocket), ballSocket);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpLimitedHingeConstraintDataAtoms);
        }

        public bool Equals(hkpLimitedHingeConstraintDataAtoms? other)
        {
            return other is not null &&
                   ((transforms is null && other.transforms is null) || (transforms is not null && other.transforms is not null && transforms.Equals((IHavokObject)other.transforms))) &&
                   ((setupStabilization is null && other.setupStabilization is null) || (setupStabilization is not null && other.setupStabilization is not null && setupStabilization.Equals((IHavokObject)other.setupStabilization))) &&
                   ((angMotor is null && other.angMotor is null) || (angMotor is not null && other.angMotor is not null && angMotor.Equals((IHavokObject)other.angMotor))) &&
                   ((angFriction is null && other.angFriction is null) || (angFriction is not null && other.angFriction is not null && angFriction.Equals((IHavokObject)other.angFriction))) &&
                   ((angLimit is null && other.angLimit is null) || (angLimit is not null && other.angLimit is not null && angLimit.Equals((IHavokObject)other.angLimit))) &&
                   ((_2dAng is null && other._2dAng is null) || (_2dAng is not null && other._2dAng is not null && _2dAng.Equals((IHavokObject)other._2dAng))) &&
                   ((ballSocket is null && other.ballSocket is null) || (ballSocket is not null && other.ballSocket is not null && ballSocket.Equals((IHavokObject)other.ballSocket))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(transforms);
            hashcode.Add(setupStabilization);
            hashcode.Add(angMotor);
            hashcode.Add(angFriction);
            hashcode.Add(angLimit);
            hashcode.Add(_2dAng);
            hashcode.Add(ballSocket);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

