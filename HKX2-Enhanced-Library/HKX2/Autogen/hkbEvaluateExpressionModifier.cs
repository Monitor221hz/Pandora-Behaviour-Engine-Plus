using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbEvaluateExpressionModifier Signatire: 0xf900f6be size: 112 flags: FLAGS_NONE

    // expressions class: hkbExpressionDataArray Type.TYPE_POINTER Type.TYPE_STRUCT arrSize: 0 offset: 80 flags: FLAGS_NONE enum: 
    // compiledExpressionSet class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 88 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // internalExpressionsData class:  Type.TYPE_ARRAY Type.TYPE_VOID arrSize: 0 offset: 96 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbEvaluateExpressionModifier : hkbModifier, IEquatable<hkbEvaluateExpressionModifier?>
    {
        public hkbExpressionDataArray? expressions { set; get; }
        private object? compiledExpressionSet { set; get; }
        public IList<object> internalExpressionsData { set; get; } = Array.Empty<object>();

        public override uint Signature { set; get; } = 0xf900f6be;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            expressions = des.ReadClassPointer<hkbExpressionDataArray>(br);
            des.ReadEmptyPointer(br);
            des.ReadEmptyArray(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassPointer(bw, expressions);
            s.WriteVoidPointer(bw);
            s.WriteVoidArray(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            expressions = xd.ReadClassPointer<hkbExpressionDataArray>(this, xe, nameof(expressions));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassPointer(xe, nameof(expressions), expressions);
            xs.WriteSerializeIgnored(xe, nameof(compiledExpressionSet));
            xs.WriteSerializeIgnored(xe, nameof(internalExpressionsData));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbEvaluateExpressionModifier);
        }

        public bool Equals(hkbEvaluateExpressionModifier? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((expressions is null && other.expressions is null) || (expressions is not null && other.expressions is not null && expressions.Equals((IHavokObject)other.expressions))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(expressions);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

