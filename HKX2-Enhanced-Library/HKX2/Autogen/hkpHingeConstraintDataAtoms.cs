using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpHingeConstraintDataAtoms Signatire: 0x6958371c size: 192 flags: FLAGS_NONE

    // transforms class: hkpSetLocalTransformsConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // setupStabilization class: hkpSetupStabilizationAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // _2dAng class: hkp_2dAngConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // ballSocket class: hkpBallSocketConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 164 flags: FLAGS_NONE enum: 
    public partial class hkpHingeConstraintDataAtoms : IHavokObject, IEquatable<hkpHingeConstraintDataAtoms?>
    {
        public hkpSetLocalTransformsConstraintAtom transforms { set; get; } = new();
        public hkpSetupStabilizationAtom setupStabilization { set; get; } = new();
        public hkp_2dAngConstraintAtom _2dAng { set; get; } = new();
        public hkpBallSocketConstraintAtom ballSocket { set; get; } = new();

        public virtual uint Signature { set; get; } = 0x6958371c;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            transforms.Read(des, br);
            setupStabilization.Read(des, br);
            _2dAng.Read(des, br);
            ballSocket.Read(des, br);
            br.Position += 12;
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            transforms.Write(s, bw);
            setupStabilization.Write(s, bw);
            _2dAng.Write(s, bw);
            ballSocket.Write(s, bw);
            bw.Position += 12;
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            transforms = xd.ReadClass<hkpSetLocalTransformsConstraintAtom>(xe, nameof(transforms));
            setupStabilization = xd.ReadClass<hkpSetupStabilizationAtom>(xe, nameof(setupStabilization));
            _2dAng = xd.ReadClass<hkp_2dAngConstraintAtom>(xe, nameof(_2dAng));
            ballSocket = xd.ReadClass<hkpBallSocketConstraintAtom>(xe, nameof(ballSocket));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkpSetLocalTransformsConstraintAtom>(xe, nameof(transforms), transforms);
            xs.WriteClass<hkpSetupStabilizationAtom>(xe, nameof(setupStabilization), setupStabilization);
            xs.WriteClass<hkp_2dAngConstraintAtom>(xe, nameof(_2dAng), _2dAng);
            xs.WriteClass<hkpBallSocketConstraintAtom>(xe, nameof(ballSocket), ballSocket);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpHingeConstraintDataAtoms);
        }

        public bool Equals(hkpHingeConstraintDataAtoms? other)
        {
            return other is not null &&
                   ((transforms is null && other.transforms is null) || (transforms is not null && other.transforms is not null && transforms.Equals((IHavokObject)other.transforms))) &&
                   ((setupStabilization is null && other.setupStabilization is null) || (setupStabilization is not null && other.setupStabilization is not null && setupStabilization.Equals((IHavokObject)other.setupStabilization))) &&
                   ((_2dAng is null && other._2dAng is null) || (_2dAng is not null && other._2dAng is not null && _2dAng.Equals((IHavokObject)other._2dAng))) &&
                   ((ballSocket is null && other.ballSocket is null) || (ballSocket is not null && other.ballSocket is not null && ballSocket.Equals((IHavokObject)other.ballSocket))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(transforms);
            hashcode.Add(setupStabilization);
            hashcode.Add(_2dAng);
            hashcode.Add(ballSocket);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

