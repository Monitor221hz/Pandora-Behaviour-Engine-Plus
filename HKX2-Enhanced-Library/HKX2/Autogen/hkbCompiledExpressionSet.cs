using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbCompiledExpressionSet Signatire: 0x3a7d76cc size: 56 flags: FLAGS_NONE

    // rpn class: hkbCompiledExpressionSetToken Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // expressionToRpnIndex class:  Type.TYPE_ARRAY Type.TYPE_INT32 arrSize: 0 offset: 32 flags: FLAGS_NONE enum: 
    // numExpressions class:  Type.TYPE_INT8 Type.TYPE_VOID arrSize: 0 offset: 48 flags: FLAGS_NONE enum: 
    public partial class hkbCompiledExpressionSet : hkReferencedObject, IEquatable<hkbCompiledExpressionSet?>
    {
        public IList<hkbCompiledExpressionSetToken> rpn { set; get; } = Array.Empty<hkbCompiledExpressionSetToken>();
        public IList<int> expressionToRpnIndex { set; get; } = Array.Empty<int>();
        public sbyte numExpressions { set; get; }

        public override uint Signature { set; get; } = 0x3a7d76cc;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            rpn = des.ReadClassArray<hkbCompiledExpressionSetToken>(br);
            expressionToRpnIndex = des.ReadInt32Array(br);
            numExpressions = br.ReadSByte();
            br.Position += 7;
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, rpn);
            s.WriteInt32Array(bw, expressionToRpnIndex);
            bw.WriteSByte(numExpressions);
            bw.Position += 7;
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            rpn = xd.ReadClassArray<hkbCompiledExpressionSetToken>(xe, nameof(rpn));
            expressionToRpnIndex = xd.ReadInt32Array(xe, nameof(expressionToRpnIndex));
            numExpressions = xd.ReadSByte(xe, nameof(numExpressions));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(rpn), rpn);
            xs.WriteNumberArray(xe, nameof(expressionToRpnIndex), expressionToRpnIndex);
            xs.WriteNumber(xe, nameof(numExpressions), numExpressions);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbCompiledExpressionSet);
        }

        public bool Equals(hkbCompiledExpressionSet? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   rpn.SequenceEqual(other.rpn) &&
                   expressionToRpnIndex.SequenceEqual(other.expressionToRpnIndex) &&
                   numExpressions.Equals(other.numExpressions) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(rpn.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(expressionToRpnIndex.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(numExpressions);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

