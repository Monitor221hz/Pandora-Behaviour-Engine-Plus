using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxEnvironment Signatire: 0x41e1aa5 size: 32 flags: FLAGS_NONE

    // variables class: hkxEnvironmentVariable Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 16 flags: FLAGS_NONE enum: 
    public partial class hkxEnvironment : hkReferencedObject, IEquatable<hkxEnvironment?>
    {
        public IList<hkxEnvironmentVariable> variables { set; get; } = Array.Empty<hkxEnvironmentVariable>();

        public override uint Signature { set; get; } = 0x41e1aa5;

        public override void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            base.Read(des, br);
            variables = des.ReadClassArray<hkxEnvironmentVariable>(br);
        }

        public override void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            base.Write(s, bw);
            s.WriteClassArray(bw, variables);
        }

        public override void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            base.ReadXml(xd, xe);
            variables = xd.ReadClassArray<hkxEnvironmentVariable>(xe, nameof(variables));
        }

        public override void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            base.WriteXml(xs, xe);
            xs.WriteClassArray(xe, nameof(variables), variables);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxEnvironment);
        }

        public bool Equals(hkxEnvironment? other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   variables.SequenceEqual(other.variables) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(base.GetHashCode());
            hashcode.Add(variables.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

