using System;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbDampingModifierInternalState Signatire: 0x508d3b36 size: 80 flags: FLAGS_NONE

    // dampedVector class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // vecErrorSum class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // vecPreviousError class:  Type.TYPE_VECTOR4 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    // dampedValue class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 64 flags: FLAGS_NONE enum: 
    // errorSum class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 68 flags: FLAGS_NONE enum: 
    // previousError class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 72 flags: FLAGS_NONE enum: 
    public partial class hkbDampingModifierInternalState : hkReferencedObject, IEquatable<hkbDampingModifierInternalState?>
    {
        public Vector4 dampedVector { set; get; }
        public Vector4 vecErrorSum { set; get; }
        public Vector4 vecPreviousError { set; get; }
        public float dampedValue { set; get; }
        public float errorSum { set; get; }
        public float previousError { set; get; }

        public override uint Signature { set; get; } = 0x508d3b36;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            dampedVector = br.ReadVector4();
            vecErrorSum = br.ReadVector4();
            vecPreviousError = br.ReadVector4();
            dampedValue = br.ReadSingle();
            errorSum = br.ReadSingle();
            previousError = br.ReadSingle();
            br.Position += 4;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            bw.WriteVector4(dampedVector);
            bw.WriteVector4(vecErrorSum);
            bw.WriteVector4(vecPreviousError);
            bw.WriteSingle(dampedValue);
            bw.WriteSingle(errorSum);
            bw.WriteSingle(previousError);
            bw.Position += 4;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            dampedVector = xd.ReadVector4(xe, nameof(dampedVector));
            vecErrorSum = xd.ReadVector4(xe, nameof(vecErrorSum));
            vecPreviousError = xd.ReadVector4(xe, nameof(vecPreviousError));
            dampedValue = xd.ReadSingle(xe, nameof(dampedValue));
            errorSum = xd.ReadSingle(xe, nameof(errorSum));
            previousError = xd.ReadSingle(xe, nameof(previousError));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteVector4(xe, nameof(dampedVector), dampedVector);
            xs.WriteVector4(xe, nameof(vecErrorSum), vecErrorSum);
            xs.WriteVector4(xe, nameof(vecPreviousError), vecPreviousError);
            xs.WriteFloat(xe, nameof(dampedValue), dampedValue);
            xs.WriteFloat(xe, nameof(errorSum), errorSum);
            xs.WriteFloat(xe, nameof(previousError), previousError);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbDampingModifierInternalState);
        }

        public bool Equals(hkbDampingModifierInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   dampedVector.Equals(other.dampedVector) &&
                   vecErrorSum.Equals(other.vecErrorSum) &&
                   vecPreviousError.Equals(other.vecPreviousError) &&
                   dampedValue.Equals(other.dampedValue) &&
                   errorSum.Equals(other.errorSum) &&
                   previousError.Equals(other.previousError) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(dampedVector);
            hashcode.Add(vecErrorSum);
            hashcode.Add(vecPreviousError);
            hashcode.Add(dampedValue);
            hashcode.Add(errorSum);
            hashcode.Add(previousError);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

