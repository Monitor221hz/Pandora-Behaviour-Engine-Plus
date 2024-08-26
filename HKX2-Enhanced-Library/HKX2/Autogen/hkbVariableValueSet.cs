using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbVariableValueSet Signatire: 0x27812d8d size: 64 flags: FLAGS_NONE

    // wordVariableValues class: hkbVariableValue Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // quadVariableValues class:  Type.TYPE_ARRAY Type.TYPE_VECTOR4 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // variantVariableValues class: hkReferencedObject Type.TYPE_ARRAY Type.TYPE_POINTER arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkbVariableValueSet : hkReferencedObject, IEquatable<hkbVariableValueSet?>
    {
        public IList<hkbVariableValue> wordVariableValues { set; get; } = Array.Empty<hkbVariableValue>();
        public IList<Vector4> quadVariableValues { set; get; } = Array.Empty<Vector4>();
        public IList<hkReferencedObject> variantVariableValues { set; get; } = Array.Empty<hkReferencedObject>();

        public override uint Signature { set; get; } = 0x27812d8d;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            wordVariableValues = des.ReadClassArray<hkbVariableValue>(br);
            quadVariableValues = des.ReadVector4Array(br);
            variantVariableValues = des.ReadClassPointerArray<hkReferencedObject>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, wordVariableValues);
            s.WriteVector4Array(bw, quadVariableValues);
            s.WriteClassPointerArray(bw, variantVariableValues);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            wordVariableValues = xd.ReadClassArray<hkbVariableValue>(xe, nameof(wordVariableValues));
            quadVariableValues = xd.ReadVector4Array(xe, nameof(quadVariableValues));
            variantVariableValues = xd.ReadClassPointerArray<hkReferencedObject>(this, xe, nameof(variantVariableValues));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(wordVariableValues), wordVariableValues);
            xs.WriteVector4Array(xe, nameof(quadVariableValues), quadVariableValues);
            xs.WriteClassPointerArray(xe, nameof(variantVariableValues), variantVariableValues!);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbVariableValueSet);
        }

        public bool Equals(hkbVariableValueSet? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   wordVariableValues.SequenceEqual(other.wordVariableValues) &&
                   quadVariableValues.SequenceEqual(other.quadVariableValues) &&
                   variantVariableValues.SequenceEqual(other.variantVariableValues) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(wordVariableValues.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(quadVariableValues.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(variantVariableValues.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

