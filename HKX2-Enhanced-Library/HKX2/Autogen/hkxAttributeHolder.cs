using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxAttributeHolder Signatire: 0x7468cc44 size: 32 flags: FLAGS_NONE

    // attributeGroups class: hkxAttributeGroup Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkxAttributeHolder : hkReferencedObject, IEquatable<hkxAttributeHolder?>
    {
        public IList<hkxAttributeGroup> attributeGroups { set; get; } = Array.Empty<hkxAttributeGroup>();

        public override uint Signature { set; get; } = 0x7468cc44;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            attributeGroups = des.ReadClassArray<hkxAttributeGroup>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, attributeGroups);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            attributeGroups = xd.ReadClassArray<hkxAttributeGroup>(xe, nameof(attributeGroups));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(attributeGroups), attributeGroups);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxAttributeHolder);
        }

        public bool Equals(hkxAttributeHolder? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   attributeGroups.SequenceEqual(other.attributeGroups) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(attributeGroups.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

