using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaMeshBindingMapping Signatire: 0x48aceb75 size: 16 flags: FLAGS_NONE

    // mapping class:  Type.TYPE_ARRAY Type.TYPE_INT16 arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    public partial class hkaMeshBindingMapping : IHavokObject, IEquatable<hkaMeshBindingMapping?>
    {
        public IList<short> mapping { set; get; } = Array.Empty<short>();

        public virtual uint Signature { set; get; } = 0x48aceb75;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            mapping = des.ReadInt16Array(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteInt16Array(bw, mapping);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            mapping = xd.ReadInt16Array(xe, nameof(mapping));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteNumberArray(xe, nameof(mapping), mapping);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaMeshBindingMapping);
        }

        public bool Equals(hkaMeshBindingMapping? other)
        {
            return other is not null &&
                   mapping.SequenceEqual(other.mapping) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(mapping.Aggregate(0, (x, y) => x ^ y.GetHashCode()));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

