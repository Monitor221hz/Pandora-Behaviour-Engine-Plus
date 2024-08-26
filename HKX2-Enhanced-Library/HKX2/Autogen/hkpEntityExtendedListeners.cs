using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkpEntityExtendedListeners Signatire: 0xf557023c size: 32 flags: FLAGS_NONE

    // activationListeners class: hkpEntitySmallArraySerializeOverrideType Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 0 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    // entityListeners class: hkpEntitySmallArraySerializeOverrideType Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 16 flags: SERIALIZE_IGNORED|FLAGS_NONE enum: 
    public partial class hkpEntityExtendedListeners : IHavokObject, IEquatable<hkpEntityExtendedListeners?>
    {
        public hkpEntitySmallArraySerializeOverrideType activationListeners { set; get; } = new();
        public hkpEntitySmallArraySerializeOverrideType entityListeners { set; get; } = new();

        public virtual uint Signature { set; get; } = 0xf557023c;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            activationListeners.Read(des, br);
            entityListeners.Read(des, br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            activationListeners.Write(s, bw);
            entityListeners.Write(s, bw);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {

        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteSerializeIgnored(xe, nameof(activationListeners));
            xs.WriteSerializeIgnored(xe, nameof(entityListeners));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkpEntityExtendedListeners);
        }

        public bool Equals(hkpEntityExtendedListeners? other)
        {
            return other is not null &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();

            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

