using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkaSkeletonMapper Signatire: 0x12df42a5 size: 144 flags: FLAGS_NONE

    // mapping class: hkaSkeletonMapperData Type.TYPE_STRUCT Type.TYPE_VOID arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkaSkeletonMapper : hkReferencedObject, IEquatable<hkaSkeletonMapper?>
    {
        public hkaSkeletonMapperData mapping { set; get; } = new();

        public override uint Signature { set; get; } = 0x12df42a5;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            mapping.Read(des, br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            mapping.Write(s, bw);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            mapping = xd.ReadClass<hkaSkeletonMapperData>(xe, nameof(mapping));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClass<hkaSkeletonMapperData>(xe, nameof(mapping), mapping);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkaSkeletonMapper);
        }

        public bool Equals(hkaSkeletonMapper? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   ((mapping is null && other.mapping is null) || (mapping is not null && other.mapping is not null && mapping.Equals((IHavokObject)other.mapping))) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(mapping);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

