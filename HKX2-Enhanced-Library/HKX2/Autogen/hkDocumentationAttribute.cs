using System;
using System.Xml.Linq;

namespace HKX2E
{
    // hkDocumentationAttribute Signatire: 0x630edd9e size: 8 flags: FLAGS_NONE

    // docsSectionTag class:  Type.TYPE_CSTRING Type.TYPE_VOID arrSize: 0 offset: 0 flags: FLAGS_NONE enum: 
    public partial class hkDocumentationAttribute : IHavokObject, IEquatable<hkDocumentationAttribute?>
    {
        public string docsSectionTag { set; get; } = "";

        public virtual uint Signature { set; get; } = 0x630edd9e;

        public virtual void Read(PackFileDeserializer des, BinaryReaderEx br)
        {
            docsSectionTag = des.ReadCString(br);
        }

        public virtual void Write(PackFileSerializer s, BinaryWriterEx bw)
        {
            s.WriteCString(bw, docsSectionTag);
        }

        public virtual void ReadXml(IHavokXmlReader xd, XElement xe)
        {
            docsSectionTag = xd.ReadString(xe, nameof(docsSectionTag));
        }

        public virtual void WriteXml(IHavokXmlWriter xs, XElement xe)
        {
            xs.WriteString(xe, nameof(docsSectionTag), docsSectionTag);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as hkDocumentationAttribute);
        }

        public bool Equals(hkDocumentationAttribute? other)
        {
            return other is not null &&
                   (docsSectionTag is null && other.docsSectionTag is null || docsSectionTag == other.docsSectionTag || docsSectionTag is null && other.docsSectionTag == "" || docsSectionTag == "" && other.docsSectionTag is null) &&
                   Signature == other.Signature; ;
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(docsSectionTag);
            hashcode.Add(Signature);
            return hashcode.ToHashCode();
        }
    }
}

