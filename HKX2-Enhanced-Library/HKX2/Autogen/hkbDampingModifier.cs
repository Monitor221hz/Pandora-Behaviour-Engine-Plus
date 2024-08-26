using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbDampingModifier Signatire: 0x9a040f03 size: 192 flags: FLAGS_NONE

    // kP class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // kI class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 84 flags: FLAGS_NONE enum: 
    // kD class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 88 flags: FLAGS_NONE enum: 
    // enableScalarDamping class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 92 flags: FLAGS_NONE enum: 
    // enableVectorDamping class:  Type.TYPE_BOOL Type.TYPE_VOID arrSize: 0 offset: 93 flags: FLAGS_NONE enum: 
    // rawValue class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 96 flags: FLAGS_NONE enum: 
    // dampedValue class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 100 flags: FLAGS_NONE enum: 
    // rawVector class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 112 flags: FLAGS_NONE enum: 
    // dampedVector class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 128 flags: FLAGS_NONE enum: 
    // vecErrorSum class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 144 flags: FLAGS_NONE enum: 
    // vecPreviousError class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 160 flags: FLAGS_NONE enum: 
    // errorSum class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 176 flags: FLAGS_NONE enum: 
    // previousError class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 180 flags: FLAGS_NONE enum: 
    public partial class hkbDampingModifier : hkbModifier, IEquatable<hkbDampingModifier?>
    {
        public float kP { set; get; }
        public float kI { set; get; }
        public float kD { set; get; }
        public bool enableScalarDamping { set; get; }
        public bool enableVectorDamping { set; get; }
        public float rawValue { set; get; }
        public float dampedValue { set; get; }
        public Vector4 rawVector { set; get; }
        public Vector4 dampedVector { set; get; }
        public Vector4 vecErrorSum { set; get; }
        public Vector4 vecPreviousError { set; get; }
        public float errorSum { set; get; }
        public float previousError { set; get; }

        public override uint Signature { set; get; } = 0x9a040f03;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            kP = br.ReadSingle();
            kI = br.ReadSingle();
            kD = br.ReadSingle();
            enableScalarDamping = br.ReadBoolean();
            enableVectorDamping = br.ReadBoolean();
            br.Position += 2;
            rawValue = br.ReadSingle();
            dampedValue = br.ReadSingle();
            br.Position += 8;
            rawVector = br.ReadVector4();
            dampedVector = br.ReadVector4();
            vecErrorSum = br.ReadVector4();
            vecPreviousError = br.ReadVector4();
            errorSum = br.ReadSingle();
            previousError = br.ReadSingle();
            br.Position += 8;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteSingle(kP);
            bw.WriteSingle(kI);
            bw.WriteSingle(kD);
            bw.WriteBoolean(enableScalarDamping);
            bw.WriteBoolean(enableVectorDamping);
            bw.Position += 2;
            bw.WriteSingle(rawValue);
            bw.WriteSingle(dampedValue);
            bw.Position += 8;
            bw.WriteVector4(rawVector);
            bw.WriteVector4(dampedVector);
            bw.WriteVector4(vecErrorSum);
            bw.WriteVector4(vecPreviousError);
            bw.WriteSingle(errorSum);
            bw.WriteSingle(previousError);
            bw.Position += 8;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            kP = xd.ReadSingle(xe, nameof(kP));
            kI = xd.ReadSingle(xe, nameof(kI));
            kD = xd.ReadSingle(xe, nameof(kD));
            enableScalarDamping = xd.ReadBoolean(xe, nameof(enableScalarDamping));
            enableVectorDamping = xd.ReadBoolean(xe, nameof(enableVectorDamping));
            rawValue = xd.ReadSingle(xe, nameof(rawValue));
            dampedValue = xd.ReadSingle(xe, nameof(dampedValue));
            rawVector = xd.ReadVector4(xe, nameof(rawVector));
            dampedVector = xd.ReadVector4(xe, nameof(dampedVector));
            vecErrorSum = xd.ReadVector4(xe, nameof(vecErrorSum));
            vecPreviousError = xd.ReadVector4(xe, nameof(vecPreviousError));
            errorSum = xd.ReadSingle(xe, nameof(errorSum));
            previousError = xd.ReadSingle(xe, nameof(previousError));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteFloat(xe, nameof(kP), kP);
            xs.WriteFloat(xe, nameof(kI), kI);
            xs.WriteFloat(xe, nameof(kD), kD);
            xs.WriteBoolean(xe, nameof(enableScalarDamping), enableScalarDamping);
            xs.WriteBoolean(xe, nameof(enableVectorDamping), enableVectorDamping);
            xs.WriteFloat(xe, nameof(rawValue), rawValue);
            xs.WriteFloat(xe, nameof(dampedValue), dampedValue);
            xs.WriteVector4(xe, nameof(rawVector), rawVector);
            xs.WriteVector4(xe, nameof(dampedVector), dampedVector);
            xs.WriteVector4(xe, nameof(vecErrorSum), vecErrorSum);
            xs.WriteVector4(xe, nameof(vecPreviousError), vecPreviousError);
            xs.WriteFloat(xe, nameof(errorSum), errorSum);
            xs.WriteFloat(xe, nameof(previousError), previousError);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbDampingModifier);
        }

        public bool Equals(hkbDampingModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   kP.Equals(other.kP) &&
                   kI.Equals(other.kI) &&
                   kD.Equals(other.kD) &&
                   enableScalarDamping.Equals(other.enableScalarDamping) &&
                   enableVectorDamping.Equals(other.enableVectorDamping) &&
                   rawValue.Equals(other.rawValue) &&
                   dampedValue.Equals(other.dampedValue) &&
                   rawVector.Equals(other.rawVector) &&
                   dampedVector.Equals(other.dampedVector) &&
                   vecErrorSum.Equals(other.vecErrorSum) &&
                   vecPreviousError.Equals(other.vecPreviousError) &&
                   errorSum.Equals(other.errorSum) &&
                   previousError.Equals(other.previousError) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(kP);
            hashcode.Add(kI);
            hashcode.Add(kD);
            hashcode.Add(enableScalarDamping);
            hashcode.Add(enableVectorDamping);
            hashcode.Add(rawValue);
            hashcode.Add(dampedValue);
            hashcode.Add(rawVector);
            hashcode.Add(dampedVector);
            hashcode.Add(vecErrorSum);
            hashcode.Add(vecPreviousError);
            hashcode.Add(errorSum);
            hashcode.Add(previousError);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

