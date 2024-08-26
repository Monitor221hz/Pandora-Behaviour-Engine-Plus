using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbVariableValue Signatire: 0xb99bd6a size: 4 flags: FLAGS_NONE

    // value class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    public partial class hkbVariableValue : IHavokObject, IEquatable<hkbVariableValue?>
    {
        public int value { set; get; }

        public virtual uint Signature { set; get; } = 0xb99bd6a;
        public hkbVariableValue()
        {
            
        }
        public hkbVariableValue(hkbVariableValue other)
        {
            value = other.value;
        }
        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            value = br.ReadInt32();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteInt32(value);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            value = xd.ReadInt32(xe, nameof(value));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(value), value);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbVariableValue);
        }

        public bool Equals(hkbVariableValue? other)
        {
            return other is not null &&
                   value.Equals(other.value) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(value);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

