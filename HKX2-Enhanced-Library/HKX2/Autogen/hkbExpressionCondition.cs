using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbExpressionCondition Signatire: 0x1c3c1045 size: 32 flags: FLAGS_NONE

    // expression class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    // compiledExpressionSet class:  Type.TYPE_POINTER Type.TYPE_VOID arrSize: 0 offset: 24 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkbExpressionCondition : hkbCondition, IEquatable<hkbExpressionCondition?>
    {
        public string expression { set; get; } = "";
        private object? compiledExpressionSet { set; get; }

        public override uint Signature { set; get; } = 0x1c3c1045;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            expression = des.ReadStringPointer(br);
            des.ReadEmptyPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointer(bw, expression);
            s.WriteVoidPointer(bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            expression = xd.ReadString(xe, nameof(expression));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteString(xe, nameof(expression), expression);
            xs.WriteSerializeIgnored(xe, nameof(compiledExpressionSet));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbExpressionCondition);
        }

        public bool Equals(hkbExpressionCondition? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   (expression is null && other.expression is null || expression == other.expression || expression is null && other.expression == "" || expression == "" && other.expression is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(expression);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

