using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpBallAndSocketConstraintDataAtoms Signatire: 0xc73dcaf9 size: 80 flags: FLAGS_NONE

    // pivots class: hkpSetLocalTranslationsConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // setupStabilization class: hkpSetupStabilizationAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // ballSocket class: hkpBallSocketConstraintAtom Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    public partial class hkpBallAndSocketConstraintDataAtoms : IHavokObject, IEquatable<hkpBallAndSocketConstraintDataAtoms?>
    {
        public hkpSetLocalTranslationsConstraintAtom pivots { set; get; } = new();
        public hkpSetupStabilizationAtom setupStabilization { set; get; } = new();
        public hkpBallSocketConstraintAtom ballSocket { set; get; } = new();

        public virtual uint Signature { set; get; } = 0xc73dcaf9;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            pivots.Read(des, br);
            setupStabilization.Read(des, br);
            ballSocket.Read(des, br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            pivots.Write(s, bw);
            setupStabilization.Write(s, bw);
            ballSocket.Write(s, bw);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            pivots = xd.ReadClass<hkpSetLocalTranslationsConstraintAtom>(xe, nameof(pivots));
            setupStabilization = xd.ReadClass<hkpSetupStabilizationAtom>(xe, nameof(setupStabilization));
            ballSocket = xd.ReadClass<hkpBallSocketConstraintAtom>(xe, nameof(ballSocket));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkpSetLocalTranslationsConstraintAtom>(xe, nameof(pivots), pivots);
            xs.WriteClass<hkpSetupStabilizationAtom>(xe, nameof(setupStabilization), setupStabilization);
            xs.WriteClass<hkpBallSocketConstraintAtom>(xe, nameof(ballSocket), ballSocket);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpBallAndSocketConstraintDataAtoms);
        }

        public bool Equals(hkpBallAndSocketConstraintDataAtoms? other)
        {
            return other is not null &&
                   ((pivots is null && other.pivots is null) || (pivots is not null && other.pivots is not null && pivots.Equals((IHavokObject)other.pivots))) &&
                   ((setupStabilization is null && other.setupStabilization is null) || (setupStabilization is not null && other.setupStabilization is not null && setupStabilization.Equals((IHavokObject)other.setupStabilization))) &&
                   ((ballSocket is null && other.ballSocket is null) || (ballSocket is not null && other.ballSocket is not null && ballSocket.Equals((IHavokObject)other.ballSocket))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(pivots);
            hashcode.Add(setupStabilization);
            hashcode.Add(ballSocket);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

