using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbFootIkControlData Signatire: 0xa111b704 size: 48 flags: FLAGS_NONE

    // gains class: hkbFootIkGains Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: ALIGN_16|FLAGS_NONE enum: 
    public partial class hkbFootIkControlData : IHavokObject, IEquatable<hkbFootIkControlData?>
    {
        public hkbFootIkGains gains { set; get; } = new();

        public virtual uint Signature { set; get; } = 0xa111b704;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            gains.Read(des, br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            gains.Write(s, bw);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            gains = xd.ReadClass<hkbFootIkGains>(xe, nameof(gains));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClass<hkbFootIkGains>(xe, nameof(gains), gains);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbFootIkControlData);
        }

        public bool Equals(hkbFootIkControlData? other)
        {
            return other is not null &&
                   ((gains is null && other.gains is null) || (gains is not null && other.gains is not null && gains.Equals((IHavokObject)other.gains))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(gains);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

