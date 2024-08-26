using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbExpressionDataArray Signatire: 0x4b9ee1a2 size: 32 flags: FLAGS_NONE

    // expressionsData class: hkbExpressionData Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkbExpressionDataArray : hkReferencedObject, IEquatable<hkbExpressionDataArray?>
    {
        public IList<hkbExpressionData> expressionsData { set; get; } = Array.Empty<hkbExpressionData>();

        public override uint Signature { set; get; } = 0x4b9ee1a2;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            expressionsData = des.ReadClassArray<hkbExpressionData>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, expressionsData);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            expressionsData = xd.ReadClassArray<hkbExpressionData>(xe, nameof(expressionsData));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(expressionsData), expressionsData);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbExpressionDataArray);
        }

        public bool Equals(hkbExpressionDataArray? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   expressionsData.SequenceEqual(other.expressionsData) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(expressionsData.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

