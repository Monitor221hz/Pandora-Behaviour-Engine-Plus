using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace HKX2E
{
    // hkxVertexDescription Signatire: 0x2df6313d size: 16 flags: FLAGS_NONE

    // decls class: hkxVertexDescriptionElementDecl Type.TYPE_ARRAY Type.TYPE_STRUCT arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    public partial class hkxVertexDescription : IHavokObject, IEquatable<hkxVertexDescription?>
    {
        public IList<hkxVertexDescriptionElementDecl> decls { set; get; } = Array.Empty<hkxVertexDescriptionElementDecl>();

        public virtual uint Signature { set; get; } = 0x2df6313d;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            decls = des.ReadClassArray<hkxVertexDescriptionElementDecl>(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteClassArray(bw, decls);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            decls = xd.ReadClassArray<hkxVertexDescriptionElementDecl>(xe, nameof(decls));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteClassArray(xe, nameof(decls), decls);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkxVertexDescription);
        }

        public bool Equals(hkxVertexDescription? other)
        {
            return other is not null &&
                   decls.SequenceEqual(other.decls) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(decls.Aggregate(0, (x, y) => x ^ y?.GetHashCode() ?? 0));
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

