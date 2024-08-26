using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpBallSocketConstraintAtom Signatire: 0xe70e4dfa size: 16 flags: FLAGS_NONE

    // solvingMethod class:  Type.TYPE_ENUM Type.TYPE_UINT8 arrSize: 0 offset: 2 flags: FLAGS_NONE enum: SolvingMethod
    // bodiesToNotify class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 3 flags: FLAGS_NONE enum: 
    // velocityStabilizationFactor class:  Type.TYPE_UINT8 Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    // maxImpulse class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 8 flags: FLAGS_NONE enum: 
    // inertiaStabilizationFactor class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 12 flags: FLAGS_NONE enum: 
    public partial class hkpBallSocketConstraintAtom : hkpConstraintAtom, IEquatable<hkpBallSocketConstraintAtom?>
    {
        public byte solvingMethod { set; get; }
        public byte bodiesToNotify { set; get; }
        public byte velocityStabilizationFactor { set; get; }
        public float maxImpulse { set; get; }
        public float inertiaStabilizationFactor { set; get; }

        public override uint Signature { set; get; } = 0xe70e4dfa;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            solvingMethod = br.ReadByte();
            bodiesToNotify = br.ReadByte();
            velocityStabilizationFactor = br.ReadByte();
            br.Position += 3;
            maxImpulse = br.ReadSingle();
            inertiaStabilizationFactor = br.ReadSingle();
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteByte(solvingMethod);
            bw.WriteByte(bodiesToNotify);
            bw.WriteByte(velocityStabilizationFactor);
            bw.Position += 3;
            bw.WriteSingle(maxImpulse);
            bw.WriteSingle(inertiaStabilizationFactor);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            solvingMethod = xd.ReadFlag<SolvingMethod, byte>(xe, nameof(solvingMethod));
            bodiesToNotify = xd.ReadByte(xe, nameof(bodiesToNotify));
            velocityStabilizationFactor = xd.ReadByte(xe, nameof(velocityStabilizationFactor));
            maxImpulse = xd.ReadSingle(xe, nameof(maxImpulse));
            inertiaStabilizationFactor = xd.ReadSingle(xe, nameof(inertiaStabilizationFactor));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteEnum<SolvingMethod, byte>(xe, nameof(solvingMethod), solvingMethod);
            xs.WriteNumber(xe, nameof(bodiesToNotify), bodiesToNotify);
            xs.WriteNumber(xe, nameof(velocityStabilizationFactor), velocityStabilizationFactor);
            xs.WriteFloat(xe, nameof(maxImpulse), maxImpulse);
            xs.WriteFloat(xe, nameof(inertiaStabilizationFactor), inertiaStabilizationFactor);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpBallSocketConstraintAtom);
        }

        public bool Equals(hkpBallSocketConstraintAtom? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   solvingMethod.Equals(other.solvingMethod) &&
                   bodiesToNotify.Equals(other.bodiesToNotify) &&
                   velocityStabilizationFactor.Equals(other.velocityStabilizationFactor) &&
                   maxImpulse.Equals(other.maxImpulse) &&
                   inertiaStabilizationFactor.Equals(other.inertiaStabilizationFactor) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(solvingMethod);
            hashcode.Add(bodiesToNotify);
            hashcode.Add(velocityStabilizationFactor);
            hashcode.Add(maxImpulse);
            hashcode.Add(inertiaStabilizationFactor);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

