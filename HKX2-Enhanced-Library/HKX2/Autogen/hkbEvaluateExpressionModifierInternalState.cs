using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbEvaluateExpressionModifierInternalState Signatire: 0xb414d58e size: 32 flags: FLAGS_NONE

    // internalExpressionsData class: hkbEvaluateExpressionModifierInternalExpressionData Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkbEvaluateExpressionModifierInternalState : hkReferencedObject, IEquatable<hkbEvaluateExpressionModifierInternalState?>
    {
        public IList<hkbEvaluateExpressionModifierInternalExpressionData> internalExpressionsData { set; get; } = Array.Empty<hkbEvaluateExpressionModifierInternalExpressionData>();

        public override uint Signature { set; get; } = 0xb414d58e;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            internalExpressionsData = des.ReadClassArray<hkbEvaluateExpressionModifierInternalExpressionData>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, internalExpressionsData);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            internalExpressionsData = xd.ReadClassArray<hkbEvaluateExpressionModifierInternalExpressionData>(xe, nameof(internalExpressionsData));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(internalExpressionsData), internalExpressionsData);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbEvaluateExpressionModifierInternalState);
        }

        public bool Equals(hkbEvaluateExpressionModifierInternalState? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   internalExpressionsData.SequenceEqual(other.internalExpressionsData) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(internalExpressionsData.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

