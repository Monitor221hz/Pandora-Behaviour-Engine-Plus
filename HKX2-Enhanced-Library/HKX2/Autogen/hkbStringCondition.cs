using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbStringCondition Signatire: 0x5ab50487 size: 24 flags: FLAGS_NONE

    // conditionString class:  Type.TYPE_STRINGPTR Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkbStringCondition : hkbCondition, IEquatable<hkbStringCondition?>
    {
        public string conditionString { set; get; } = "";

        public override uint Signature { set; get; } = 0x5ab50487;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            conditionString = des.ReadStringPointer(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteStringPointer(bw, conditionString);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            conditionString = xd.ReadString(xe, nameof(conditionString));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteString(xe, nameof(conditionString), conditionString);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbStringCondition);
        }

        public bool Equals(hkbStringCondition? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   (conditionString is null && other.conditionString is null || conditionString == other.conditionString || conditionString is null && other.conditionString == "" || conditionString == "" && other.conditionString is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(conditionString);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

