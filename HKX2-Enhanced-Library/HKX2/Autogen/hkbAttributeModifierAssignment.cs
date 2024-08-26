using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkbAttributeModifierAssignment Signatire: 0x48b8ad52 size: 8 flags: FLAGS_NONE

    // attributeIndex class:  Type.TYPE_INT32 Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    // attributeValue class:  Type.TYPE_REAL Type.TYPE_VOID arrSize: 0 offset: 4 flags: FLAGS_NONE enum: 
    public partial class hkbAttributeModifierAssignment : IHavokObject, IEquatable<hkbAttributeModifierAssignment?>
    {
        public int attributeIndex { set; get; }
        public float attributeValue { set; get; }

        public virtual uint Signature { set; get; } = 0x48b8ad52;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            attributeIndex = br.ReadInt32();
            attributeValue = br.ReadSingle();
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            bw.WriteInt32(attributeIndex);
            bw.WriteSingle(attributeValue);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            attributeIndex = xd.ReadInt32(xe, nameof(attributeIndex));
            attributeValue = xd.ReadSingle(xe, nameof(attributeValue));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumber(xe, nameof(attributeIndex), attributeIndex);
            xs.WriteFloat(xe, nameof(attributeValue), attributeValue);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkbAttributeModifierAssignment);
        }

        public bool Equals(hkbAttributeModifierAssignment? other)
        {
            return other is not null &&
                   attributeIndex.Equals(other.attributeIndex) &&
                   attributeValue.Equals(other.attributeValue) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(attributeIndex);
            hashcode.Add(attributeValue);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

